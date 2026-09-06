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
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmProductionConsumption : Form
{
	private bool UpdateMode;

	private int RecId;

	private int RecIdConsumption;

	private int RecIdOutPut;

	private int VoucherHeadId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexInput = -1;

	private int updateDetailIndexConsumption = -1;

	private double AvgRate;

	private double AvgRateWithoutExp;

	private string InputDetailRowsRemoveIds = "";

	private string OutputDetailRowsRemoveIds = "";

	private int ScheduleId;

	public static bool ValuesShowRights;

	private DataTable dtBranch = new DataTable();

	private DataTable dtCropGlobal = new DataTable();

	private DataTable dtCropOutPut = new DataTable();

	private DataTable dtCropInPut = new DataTable();

	private DataTable dtJobGlobal = new DataTable();

	private DataTable dtJobOutPut = new DataTable();

	private DataTable dtJobInPut = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtuomlst = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private DataTable dtwarehouse = new DataTable();

	private DataTable dtEntryType = new DataTable();

	private DataTable dtItemOutPut = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable dtReturnToGodown = new DataTable();

	private DataTable tableConsumption = new DataTable();

	private DataTable dtItemOuPutGlobal = new DataTable();

	private DataTable dtitemConsumption = new DataTable();

	private DataTable OverHeadHistorydt = new DataTable();

	private DataTable dtItemOuPutReturnToGodownGlobal = new DataTable();

	private bool LoaderStatus;

	private bool WagesStatus;

	private bool FIFOCGSFlag;

	private bool IsManualEntryOnInputNotAllowed;

	private bool WagesActiveOrInActiveForInput;

	private bool WagesActiveOrInActiveForOutPut;

	private bool WagesActiveOrInActiveForConsumption;

	private bool ByProductRateEditAbleOrNot;

	private bool SaleCostingJobOrderWise;

	private bool OutputItemsByJobOrderRateSchedule;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadOutPutPendingforRates LoadPendingforRates;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private LoadConsumptionPendingforRates LoadConsumptionPendingforRate;

	private LoadavailableTransactionsForIssuanceOnConsumption LoadTransactionOnConsumption;

	private frmProductionPackingMaterial PackingMaterialForm;

	private frmProductionOverhead OverHeadForm;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel PanelOtherForms;

	public TabControl tabControl1;

	private TabPage InPut;

	public TabControl tabControl2;

	private TabPage tabPage1;

	private Panel panel28;

	private Panel panel5;

	private Panel panel14;

	internal GridEX grdStockConversionProductionDetail;

	private Panel panel12;

	private Label label2;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel PanelInPutManualEntry;

	private GroupBox groupBox2;

	internal UltraCombo cmbWareHouseInput;

	private Label label82;

	private Label label64;

	private TextBox txtQtyInput;

	private TextBox txtBalanceWeight;

	private TextBox txtNetWeightInput;

	internal UltraCombo cmbItemUomInput;

	private Label label81;

	internal UltraCombo cmbLotInput;

	private TextBox txtRemarksInput;

	internal UltraCombo cmbItemInput;

	private TextBox txtAmountInput;

	internal UltraCombo cmbCropYearInput;

	private Label lblAmountInput;

	private Label label61;

	internal UltraCombo cmbRateuomInput;

	private Label label78;

	private Label lblRateUom;

	private Label label65;

	private TextBox txtRateInput;

	private Label label74;

	private Label lblRateInput;

	internal UltraCombo cmbPackingTypeInput;

	private Button btnAddInput;

	private TextBox txtBalanceQty;

	private Button btnCancelInput;

	private Label label66;

	private Button btnUpdateDetailInput;

	private Label label72;

	private Label label80;

	private Label label67;

	private Panel panel72;

	private GroupBox groupBox1;

	private Label label1;

	internal UltraCombo CmbPlantFeeder;

	private TextBox txtdocnumber;

	private Label label83;

	private DateTimePicker txtDocdate;

	internal UltraCombo CmbWorkInProcess;

	private Label label27;

	internal UltraCombo CmbProductionDepartment;

	private Label label112;

	internal UltraCombo CmbWIPItem;

	private Label label77;

	private Label label23;

	internal UltraCombo CmbJobOrderNo;

	private Button btnLoadInvoices;

	private Label label6;

	private TextBox txtRemarks;

	private Panel panel63;

	private Label label30;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private ToolStripButton btnVoucherReport;

	private ToolStripButton btnJobOrderDefine;

	private ToolStripButton btnPendingForRateInput;

	private TabPage tabPage4;

	internal GridEX grdinputhistory;

	private Panel panel78;

	private Label label130;

	internal GridEX grdInputDetail;

	private Panel panel59;

	private Label label127;

	private CtrlGrdBar ctrlGrdBar10;

	private Panel panel58;

	private GroupBox groupBox11;

	private Button BtnShowInputHistory;

	private Label label145;

	internal UltraCombo CmbJobOrderInputHistory;

	private DateTimePicker txtToDateHistory;

	private Label label146;

	private Label label147;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label148;

	private Label label149;

	private TextBox txtFromNoHistory;

	private Panel panel57;

	private Label label124;

	private ToolStrip toolStrip7;

	private ToolStripButton toolStripButton4;

	private ToolStripButton BtnRefereshInputHistory;

	private TabPage OutPut;

	private Panel panel3;

	public TabControl tabControl3;

	private TabPage tabPage5;

	private Panel panel2;

	private Panel panel24;

	internal GridEX grdByProduct;

	private Panel panel22;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar7;

	private Panel panel30;

	private Panel panel64;

	private Label label32;

	private GroupBox groupBox4;

	private TextBox txtEbTotal;

	private Label label165;

	private TextBox txtEbUnit;

	private Label label166;

	private Label label167;

	private TextBox txtGrossWeight;

	internal UltraCombo CmbLotOutPut;

	private Label label13;

	private TextBox txtRemarksOutPut;

	internal UltraCombo CmbUomOutPut;

	private TextBox txtAmountOutPut;

	private Label label70;

	private Label lblAmountOUtPUt;

	internal UltraCombo CmbWarehouseOutput;

	internal UltraCombo CmbRateUomOutPut;

	internal UltraCombo CmbPackTypeOutPut;

	private Label label45;

	private Label lblRateUOMOUtPut;

	private Label label36;

	internal UltraCombo CmbItemOutput;

	private TextBox txtRateOutPut;

	private Label label69;

	private Label lblRateOutPUt;

	private TextBox txtQtyOutPut;

	private Button btnAddOutPut;

	internal UltraCombo CmbCropYearOutPut;

	private Label label20;

	private Button btnCancelOutPut;

	private Label label24;

	internal UltraCombo CmbEntryType;

	private Button btnUpdateDetailOutPut;

	private Label label43;

	private TextBox txtNetWeightOutPut;

	private Label label11;

	private Label label9;

	private GroupBox groupBox3;

	private TextBox txtDocNoOutPut;

	internal UltraCombo CmbExportContractSchedule;

	internal UltraCombo CmbPlantFeederOutput;

	private Label label73;

	private Label label170;

	private Label label16;

	private Label label86;

	private Label label63;

	private DateTimePicker DocDateOutPut;

	private Label label71;

	internal UltraCombo CmbJobOrderNoOutput;

	private Label label54;

	private Label label60;

	private TextBox txtBalWeightTotal;

	private TextBox txtBalQtyTotal;

	private Label label19;

	private Label label62;

	private TextBox txtRemarksHeaderOutPut;

	private TextBox txtInPutTotalQty;

	private TextBox txtInPutTotalWeight;

	private Label label53;

	private TextBox txtOutQtyTotal;

	private TextBox txtOutWeightTotal;

	private TextBox txtAvgFinishGoodRate;

	private Button btnApplyRate;

	private TextBox txtFGRate;

	private Label label76;

	private TextBox txtFGWeight;

	private Label label75;

	private Panel panel4;

	private ToolStrip toolStrip6;

	private ToolStripButton btnNewOutPut;

	private ToolStripButton btnRefreshOutPut;

	private ToolStripButton btnSaveOutPut;

	private ToolStripButton btnUpdateOutPut;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnPrintOutPut;

	private ToolStripButton btnVoucherOutPut;

	private ToolStripButton btnPenForRateEntries;

	private ToolStripButton btnDailyPlantConsumedHours;

	private CtrlGrdBar ctrlGrdBar4;

	private TabPage tabPage6;

	internal GridEX grdOutputHistory;

	private Panel panel85;

	private Label label156;

	private Panel panel84;

	private Label label155;

	private CtrlGrdBar ctrlGrdBar11;

	private Panel panel81;

	private GroupBox groupBox12;

	private Button BtnShowOutPuthistory;

	private Label label150;

	internal UltraCombo CmbJobOrderOutPuthistory;

	private DateTimePicker txttoDateOutPuthistory;

	private Label label151;

	private Label label152;

	private TextBox txtDocnotoOutPuthistory;

	private DateTimePicker txtFromDateOutPuthistory;

	private Label label153;

	private Label label154;

	private TextBox txtDocNoFromOutPuthistory;

	internal GridEX grdOutPutDetail;

	private Panel panel60;

	private ToolStrip toolStrip8;

	private ToolStripButton BtnNewOutPuthistory;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton BtnRefereshOutPutHistory;

	private TabPage Consumption;

	public TabControl tabControl6;

	private TabPage tabPage9;

	private Panel panel48;

	private Panel panel74;

	private Panel panel76;

	internal GridEX grdConsumption;

	private Panel panel77;

	private Label label120;

	private CtrlGrdBar ctrlGrdBar16;

	private Panel panelManualEntryConsumption;

	private GroupBox groupBox9;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	internal UltraCombo cmbWarehouseConsumption;

	private Label label121;

	private Label label122;

	private TextBox txtItemQtyConsumption;

	private TextBox txtBalanceWeightConsumption;

	private TextBox txtNetWeightConsumption;

	internal UltraCombo cmbPackUomConsumption;

	private Label label123;

	internal UltraCombo CmbJobLotConsumption;

	private TextBox txtRemarksDetailConsumption;

	internal UltraCombo CmbItemNameConsumption;

	private TextBox txtAmountConsumption;

	internal UltraCombo CmbCropYearConsumption;

	private Label lblAmountConsumption;

	private Label label125;

	internal UltraCombo CmbRateUomConsumption;

	private Label label126;

	private Label lblRateUomConsumption;

	private Label label128;

	private TextBox txtRateConsumption;

	private Label label129;

	private Label lblRateConsumption;

	internal UltraCombo CmbPackingTypeConsumption;

	private Button BtnAddConsumption;

	private TextBox txtBalanceQtyConsumption;

	private Button BtnCancelConsumption;

	private Label label131;

	private Button BtnUpdateDetailConsumption;

	private Label label132;

	private Label label133;

	private Label label134;

	private Panel panel79;

	private GroupBox groupBox10;

	private Label label135;

	internal UltraCombo CmbPlantConsumption;

	private TextBox txtDocNoConsumption;

	private Label label136;

	private DateTimePicker txtDocDateConsumption;

	internal UltraCombo cmbWIPAcConsumption;

	private Label label137;

	internal UltraCombo CmbDepartmentConsumption;

	private Label label138;

	internal UltraCombo cmbWIPItemConsumption;

	private Label label139;

	private Label label140;

	internal UltraCombo cmbJobOrderConsumption;

	private Label label141;

	private TextBox txtRemarksHeaderConsumption;

	private Panel panel80;

	private Label label142;

	private ToolStrip toolStripConsumption;

	private ToolStripButton BtnNewConsumption;

	private ToolStripButton BtnRefreshConsumption;

	private ToolStripButton btnSaveConsumption;

	private ToolStripButton BtnUpdateconsumption;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton BtnPrintSlipConsumption;

	private ToolStripButton btnPrintVoucherConsumption;

	private ToolStripButton BtnPendingForRatesFormConsumption;

	private ToolStripButton BtnLoadDataForConsumptionEntry;

	private TabPage tabPage10;

	internal GridEX grdHistoryConsumptionDetail;

	private Panel panel82;

	private Label label144;

	internal GridEX grdHistoryConsumptionMai;

	private Panel panel83;

	private CtrlGrdBar ctrlGrdBar17;

	private Label label157;

	private Panel panel62;

	private GroupBox groupBox13;

	private Button BtnShowConsupmtionHistory;

	private Label label158;

	internal UltraCombo CmbJobOrderNoConsupmtionHistory;

	private DateTimePicker txtToDateConsupmtionHistory;

	private Label label159;

	private Label label160;

	private TextBox txtDocNoToConsupmtionHistory;

	private DateTimePicker txtFromDateConsupmtionHistory;

	private Label label161;

	private Label label162;

	private TextBox txtDocNofromConsupmtionHistory;

	private Panel panel61;

	private Label label143;

	private ToolStrip toolStripConsumptionHistory;

	private ToolStripButton BtnNewHistoryConsumption;

	private ToolStripButton btnRefreshConsupmtionHistory;

	private ToolStrip miniToolStrip;

	public frmProductionConsumption(UserAccount user)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFoodProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1280, 820);
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 80);
			WagesActiveOrInActiveForInput = obj != null && obj.IsActive;
			WagesRefDocumentsStatus obj2 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 112);
			WagesActiveOrInActiveForOutPut = obj2 != null && obj2.IsActive;
			WagesRefDocumentsStatus obj3 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 181);
			WagesActiveOrInActiveForConsumption = obj3 != null && obj3.IsActive;
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			OutputItemsByJobOrderRateSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OutputItemsByJobOrderRateSchedule"));
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("WagesCompulsoryOnProduction,IssuanceByLoader,IsManualEntryOnInputNotAllowed,ByProductRateEditableIsAllow");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "WagesCompulsoryOnProduction")
					{
						WagesStatus = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "IssuanceByLoader")
					{
						LoaderStatus = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "IsManualEntryOnInputNotAllowed")
					{
						IsManualEntryOnInputNotAllowed = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "ByProductRateEditableIsAllow")
					{
						ByProductRateEditAbleOrNot = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (IsManualEntryOnInputNotAllowed)
			{
				PanelInPutManualEntry.Visible = false;
				panelManualEntryConsumption.Visible = false;
			}
			if (clsGlobalVariables.ScreenViewReights.Any((tblUserRights dd) => dd.ScreenName == "FoodProductionWithValues"))
			{
				formright = CommonServices.SetRightsValueInRightsObject("FoodProductionWithValues");
			}
			else
			{
				formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			}
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			Print.Enabled = formright.DoHavePrintRights;
			btnVoucherReport.Enabled = formright.DoHavePrintRights;
			btnSaveOutPut.Enabled = formright.DoHaveSaveRight;
			btnUpdateOutPut.Enabled = formright.DoHaveUpdateRights;
			btnVoucherOutPut.Enabled = formright.DoHavePrintRights;
			btnPrintOutPut.Enabled = formright.DoHavePrintRights;
			btnSaveConsumption.Enabled = formright.DoHaveSaveRight;
			BtnUpdateconsumption.Enabled = formright.DoHaveUpdateRights;
			btnPrintVoucherConsumption.Enabled = formright.DoHavePrintRights;
			BtnPrintSlipConsumption.Enabled = formright.DoHavePrintRights;
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
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar3.mGridChooseFielder.Enabled = false;
				ctrlGrdBar4.mGridChooseFielder.Enabled = false;
				ctrlGrdBar16.mGridChooseFielder.Enabled = false;
				ctrlGrdBar17.mGridChooseFielder.Enabled = false;
			}
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			if (FIFOCGSFlag)
			{
				label83.Visible = true;
				btnPendingForRateInput.Visible = false;
				BtnPendingForRatesFormConsumption.Visible = false;
			}
			else
			{
				label83.Visible = false;
			}
			dtitemConsumption.Columns.Add("Id", typeof(int));
			dtitemConsumption.Columns.Add("ItemName", typeof(string));
			dtitemConsumption.Columns.Add("ItemCode", typeof(string));
			dtItemOutPut.Columns.Add("Id", typeof(int));
			dtItemOutPut.Columns.Add("ItemName", typeof(string));
			dtCropInPut.Columns.Add("Id", typeof(int));
			dtCropInPut.Columns.Add("CropYear");
			dtCropOutPut.Columns.Add("Id", typeof(int));
			dtCropOutPut.Columns.Add("CropYear");
			dtJobInPut.Columns.Add("Id", typeof(int));
			dtJobInPut.Columns.Add("JobLotDescription");
			dtJobOutPut.Columns.Add("Id", typeof(int));
			dtJobOutPut.Columns.Add("JobLotDescription");
			dtwarehouse.Columns.Add("Id", typeof(int));
			dtwarehouse.Columns.Add("WareHouseName");
			dtpacktype.Columns.Add("Id", typeof(int));
			dtpacktype.Columns.Add("PackingType");
			table.Columns.Add("Id");
			table.Columns.Add("RefDocumentTypeId");
			table.Columns.Add("RefDocumentType");
			table.Columns.Add("RefDocIdNo");
			table.Columns.Add("RefDocSubIdNo");
			table.Columns.Add("SupplierCustomerId", typeof(int));
			table.Columns.Add("PartyName");
			table.Columns.Add("DocDate");
			table.Columns.Add("DocNo");
			table.Columns.Add("WareHouseId");
			table.Columns.Add("WareHouse");
			table.Columns.Add("ItemId");
			table.Columns.Add("Item");
			table.Columns.Add("ItemUOMId");
			table.Columns.Add("ItemUOM");
			table.Columns.Add("ItemEquivalent");
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("PackingTypeId");
			table.Columns.Add("PackingType");
			table.Columns.Add("BalanceQty", typeof(double));
			table.Columns.Add("BalanceWeight", typeof(double));
			table.Columns.Add("Quantity", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateForCheck", typeof(double));
			table.Columns.Add("RateUOMId");
			table.Columns.Add("RateUOM");
			table.Columns.Add("RateUOMForCheck");
			table.Columns.Add("ItemAmount", typeof(decimal));
			table.Columns.Add("Remarks");
			tableByProduct.Columns.Add("Id");
			tableByProduct.Columns.Add("EntryType");
			tableByProduct.Columns.Add("WareHouseId");
			tableByProduct.Columns.Add("WareHouse");
			tableByProduct.Columns.Add("ItemId");
			tableByProduct.Columns.Add("Item");
			tableByProduct.Columns.Add("ItemUOMId");
			tableByProduct.Columns.Add("ItemUOM");
			tableByProduct.Columns.Add("ItemEquivalent");
			tableByProduct.Columns.Add("CropYearId");
			tableByProduct.Columns.Add("CropYear");
			tableByProduct.Columns.Add("JobLotId");
			tableByProduct.Columns.Add("JobLot");
			tableByProduct.Columns.Add("PackingTypeId");
			tableByProduct.Columns.Add("PackingType");
			tableByProduct.Columns.Add("Quantity", typeof(double));
			tableByProduct.Columns.Add("GrossWeight", typeof(double));
			tableByProduct.Columns.Add("EbUnit", typeof(double));
			tableByProduct.Columns.Add("EbTotal", typeof(double));
			tableByProduct.Columns.Add("Weight", typeof(double));
			tableByProduct.Columns.Add("Rate", typeof(double));
			tableByProduct.Columns.Add("RateUOMId", typeof(int));
			tableByProduct.Columns.Add("RateUOM", typeof(string));
			tableByProduct.Columns.Add("RateEquivalent", typeof(double));
			tableByProduct.Columns.Add("ItemAmount", typeof(double));
			tableByProduct.Columns.Add("Remarks");
			tableByProduct.Columns.Add("StockAcId");
			tableByProduct.Columns.Add("StockAc");
			tableByProduct.Columns.Add("IssueWeight", typeof(double));
			tableByProduct.Columns.Add("ScheduleId", typeof(int));
			tableConsumption.Columns.Add("Id", typeof(int));
			tableConsumption.Columns.Add("RefDocumentTypeId", typeof(int));
			tableConsumption.Columns.Add("RefDocIdNo", typeof(int));
			tableConsumption.Columns.Add("RefDocSubIdNo", typeof(int));
			tableConsumption.Columns.Add("DocDate");
			tableConsumption.Columns.Add("DocNo", typeof(int));
			tableConsumption.Columns.Add("WareHouseId", typeof(int));
			tableConsumption.Columns.Add("WareHouse");
			tableConsumption.Columns.Add("ItemId", typeof(int));
			tableConsumption.Columns.Add("Item");
			tableConsumption.Columns.Add("ItemCode");
			tableConsumption.Columns.Add("ItemUOMId", typeof(int));
			tableConsumption.Columns.Add("ItemUOM");
			tableConsumption.Columns.Add("ItemEquivalent", typeof(double));
			tableConsumption.Columns.Add("CropYear");
			tableConsumption.Columns.Add("JobLotId", typeof(int));
			tableConsumption.Columns.Add("JobLot");
			tableConsumption.Columns.Add("PackingTypeId", typeof(int));
			tableConsumption.Columns.Add("PackingType");
			tableConsumption.Columns.Add("BalanceQty", typeof(double));
			tableConsumption.Columns.Add("BalanceWeight", typeof(double));
			tableConsumption.Columns.Add("Quantity", typeof(double));
			tableConsumption.Columns.Add("Weight", typeof(double));
			tableConsumption.Columns.Add("Rate", typeof(double));
			tableConsumption.Columns.Add("RateForCheck", typeof(double));
			tableConsumption.Columns.Add("RateUOMId", typeof(int));
			tableConsumption.Columns.Add("RateUOM");
			tableConsumption.Columns.Add("RateUOMForCheck");
			tableConsumption.Columns.Add("ItemAmount", typeof(decimal));
			tableConsumption.Columns.Add("Remarks");
			BranchesFill();
			UOMFill();
			CommondtForCombosFillInput();
			CommondtForCombosFillOutPut();
			JobLotInput();
			JobLotOutPut();
			CropYearInput();
			CropYearOutPut();
			bagType();
			JobOrderNoFill();
			WarehouseInputFill();
			GenerateDocNumberInput();
			BindProductionDepartment();
			if (!OutputItemsByJobOrderRateSchedule)
			{
				AllItemForOutPut();
			}
			btnUpdateDetailOutPut.Visible = false;
			btnUpdateDetailInput.Visible = false;
			btnCancelOutPut.Visible = false;
			btnCancelInput.Visible = false;
			BtnAddConsumption.Visible = true;
			BtnUpdateDetailConsumption.Visible = false;
			BtnCancelConsumption.Visible = false;
			if (!formright.DoHaveCanRateandAmount)
			{
				txtRemarksInput.Width = 640;
				txtRateInput.Visible = false;
				((Control)(object)cmbRateuomInput).Visible = false;
				txtAmountInput.Visible = false;
				lblRateInput.Visible = false;
				lblRateUom.Visible = false;
				lblAmountInput.Visible = false;
				btnPenForRateEntries.Visible = false;
				txtRemarksDetailConsumption.Width = 640;
				txtRateConsumption.Visible = false;
				((Control)(object)CmbRateUomConsumption).Visible = false;
				txtAmountConsumption.Visible = false;
				lblRateConsumption.Visible = false;
				lblRateUomConsumption.Visible = false;
				lblAmountConsumption.Visible = false;
				BtnPendingForRatesFormConsumption.Visible = false;
				txtRemarksOutPut.Width = 542;
				txtRateOutPut.Visible = false;
				((Control)(object)CmbRateUomOutPut).Visible = false;
				txtAmountOutPut.Visible = false;
				lblRateOutPUt.Visible = false;
				lblRateUOMOUtPut.Visible = false;
				lblAmountOUtPUt.Visible = false;
				btnVoucherReport.Visible = false;
				btnPrintVoucherConsumption.Visible = false;
				btnVoucherOutPut.Visible = false;
			}
			txtFromDateOutPuthistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderOutputHCombobind();
			txtFromdateHistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderInputHCombobind();
			txtFromDateConsupmtionHistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderConsumptionHCombobind();
			if (FIFOCGSFlag)
			{
				txtRateInput.Enabled = false;
				((Control)(object)cmbRateuomInput).Enabled = false;
				txtRateConsumption.Enabled = false;
				((Control)(object)CmbRateUomConsumption).Enabled = false;
			}
			else
			{
				txtRateInput.Enabled = true;
				txtRateConsumption.Enabled = true;
				((Control)(object)cmbRateuomInput).Enabled = false;
				((Control)(object)CmbRateUomConsumption).Enabled = false;
			}
			txtDocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
	}

	private void Warehouse()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtware = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtware.Rows.Count > 0)
			{
				DDL.BindDDL(dtware, CmbWarehouseOutput, "Id", "WareHouseName", "Issue From ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommondtForCombosFillOutPut()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtJobGlobal = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(obj);
			dtCropGlobal = CommonServices.CropYearGetAllService();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommondtForCombosFillInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtjo = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtjo.Rows.Count > 0)
			{
				dtJobInPut.Clear();
				foreach (DataRow r2 in dtjo.Rows)
				{
					dtJobInPut.Rows.Add(r2["Id"], r2["JobLotDescription"]);
				}
			}
			DataTable dtcr = CommonServices.CropYearGetAllService();
			if (dtcr.Rows.Count <= 0)
			{
				return;
			}
			dtCropInPut.Clear();
			foreach (DataRow r in dtcr.Rows)
			{
				dtCropInPut.Rows.Add(r["Id"], r["CropYear"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotInput()
	{
		try
		{
			int Id = Conversion.ToInt(cmbLotInput.Value);
			if (dtJobInPut.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobInPut, cmbLotInput, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtJobInPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbLotInput.Value = Id;
					}
					else
					{
						((Control)(object)cmbLotInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLotInput).Text = string.Empty;
				((Control)(object)cmbLotInput).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearInput()
	{
		try
		{
			int Id = Conversion.ToInt(cmbCropYearInput.Value);
			if (dtCropInPut.Rows.Count > 0)
			{
				DDL.BindDDL(dtCropInPut, cmbCropYearInput, "Id", "CropYear", "Crop Year", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtCropInPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbCropYearInput.Value = Id;
					}
					else
					{
						((Control)(object)cmbCropYearInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbCropYearInput).Text = string.Empty;
				((Control)(object)cmbCropYearInput).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bagType()
	{
		try
		{
			DataTable dtpackingtype = new DataTable();
			dtpackingtype = InvPackingType.Getall();
			dtpacktype.Clear();
			for (int i = 0; i < dtpackingtype.Rows.Count; i++)
			{
				dtpacktype.Rows.Add(dtpackingtype.Rows[i]["Id"], dtpackingtype.Rows[i]["PackTypeDesc"]);
			}
			if (dtpacktype.Rows.Count > 0)
			{
				DDL.BindDDL(dtpacktype, CmbPackTypeOutPut, "Id", "PackingType", "Packing Type", ZeroIndex: false);
				DDL.BindDDL(dtpacktype, cmbPackingTypeInput, "Id", "PackingType", "Packing Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindProductionDepartment()
	{
		try
		{
			DataTable dt = CommonServices.GetActiveWareHouseByWareHouseType(2);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbProductionDepartment, "Id", "WareHouseName", "Production Department", ZeroIndex: false);
				DDL.BindDDL(dt, CmbDepartmentConsumption, "Id", "WareHouseName", "Production Department", ZeroIndex: false);
				((UltraGridBase)CmbProductionDepartment).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbDepartmentConsumption).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			int JobInputId = Conversion.ToInt(CmbJobOrderNo.Value);
			int JobOutPutId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			int JobConsumId = Conversion.ToInt(cmbJobOrderConsumption.Value);
			DataTable dtPJO = InvProductionJobOrder.JobOrdersForProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PlanCode");
				dt.Columns.Add("DocumentTypeId", typeof(int));
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"], dtPJO.Rows[i]["DocumentTypeId"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbJobOrderNo, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					DDL.BindDDL(dt, cmbJobOrderConsumption, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)CmbJobOrderNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbJobOrderConsumption).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbJobOrderNoOutput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (JobInputId > 0)
				{
					CmbJobOrderNo.Value = JobInputId;
				}
				if (JobOutPutId > 0)
				{
					CmbJobOrderNoOutput.Value = JobOutPutId;
				}
				if (JobConsumId > 0)
				{
					cmbJobOrderConsumption.Value = JobConsumId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomlst = UOMSchedule.Getall(obj);
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
			dtEntryType.Rows.Clear();
			if (dtEntryType.Columns.Count == 0)
			{
				dtEntryType.Columns.Add("Id");
				dtEntryType.Columns.Add("Name");
			}
			dtEntryType.Rows.Add(1, "ByProduct");
			dtEntryType.Rows.Add(2, "FinishGoods");
			dtEntryType.Rows.Add(3, "ReturnToGodown");
			dtEntryType.Rows.Add(4, "Wastage");
			DDL.BindDDL(dtEntryType, CmbEntryType, "Id", "Name", "Entry Type", ZeroIndex: false);
			if (string.IsNullOrEmpty(((Control)(object)CmbEntryType).Text))
			{
				((UltraGridBase)CmbEntryType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseInputFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtware = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtware.Rows.Count > 0)
			{
				DDL.BindDDL(dtware, cmbWareHouseInput, "Id", "WareHouseName", "WareHouse", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemByWareHouseId(int WarehouseId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dtItemInput = Item.GetItemByWarehouseIdfromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId
			});
			if (dtItemInput.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemInput, cmbItemInput, "ItemId", "ItemName", "Item", false);
				return;
			}
			((Control)(object)cmbItemInput).Text = string.Empty;
			((UltraGridBase)cmbItemInput).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPlantFeeder(int JobOrderId)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			int JobInputId = Conversion.ToInt(CmbPlantFeeder.Value);
			int JobConsumId = Conversion.ToInt(CmbPlantConsumption.Value);
			DataTable dt = InvProductionJobOrder.PlantsForProductionByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = JobOrderId
			});
			if (dt.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbJobOrderNo).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeeder, "PlantId", "PlantName", "Plant Name", false);
					if (dt.Rows.Count == 1)
					{
						((UltraGridBase)CmbPlantFeeder).Rows[0].Activate();
					}
				}
				if (((UltraGridBase)cmbJobOrderConsumption).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantConsumption, "PlantId", "PlantName", "Plant Name", false);
				}
				if (JobInputId > 0)
				{
					CmbPlantFeeder.Value = JobInputId;
				}
				if (JobConsumId > 0)
				{
					CmbPlantConsumption.Value = JobConsumId;
				}
			}
			else
			{
				((Control)(object)CmbPlantFeeder).Text = string.Empty;
				((Control)(object)CmbPlantConsumption).Text = string.Empty;
				((UltraGridBase)CmbPlantFeeder).DataSource = null;
				((UltraGridBase)CmbPlantConsumption).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void OutPutPlantFillFromProductionInput(int Id, int BranchesId)
	{
		try
		{
			DataTable dt = InvProductionJobOrder.GetPlantByJobOrderIdFromProductionInput(Id, BranchesId);
			if (dt.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeederOutput, "PlantId", "PlantName", "Plant Name", false);
					if (dt.Rows.Count == 1)
					{
						((UltraGridBase)CmbPlantFeederOutput).Rows[0].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbPlantFeederOutput).Text = string.Empty;
				((UltraGridBase)CmbPlantFeederOutput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetVoucherHeadId(int Id, int DocumentTypeId)
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

	private void frmFoodProduction_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.NumPad1)
			{
				tabControl1.SelectedIndex = 0;
			}
			if (e.Control && e.KeyCode == Keys.NumPad2)
			{
				tabControl1.SelectedIndex = 1;
				DocDateOutPut.Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad3)
			{
				tabControl1.SelectedIndex = 2;
				txtDocDateConsumption.Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnsave_Click(null, null);
				}
				if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					btnLoadInvoices_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdStockConversionProductionDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnSaveOutPut_Click(null, null);
				}
				if (btnUpdateOutPut.Visible && btnUpdateOutPut.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdateOutPut_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDateOutPut.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdByProduct).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdByProduct_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewOutPut_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshOutPut_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 2)
			{
				if (btnSaveConsumption.Visible && btnSaveConsumption.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnSaveConsumption_Click(null, null);
				}
				if (BtnUpdateconsumption.Visible && BtnUpdateconsumption.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					BtnUpdateconsumption_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDateConsumption.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdByProduct).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdByProduct_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewConsumption_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshConsumption_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedTab.Name == "tabSummary")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				ProductionSummaryReport form = new ProductionSummaryReport(UserAccount);
				FormHelper.OpenFormInPanel("ProductionSummaryReport", form, PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabPackingMaterial")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				string screenName = "frmProductionPackingMaterial";
				PackingMaterialForm = FormHelper.OpenOrGetFormInPanel(screenName, () => new frmProductionPackingMaterial(UserAccount), PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabOverhead")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				string screenName2 = "frmProductionOverhead";
				OverHeadForm = FormHelper.OpenOrGetFormInPanel(screenName2, () => new frmProductionOverhead(UserAccount), PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabSettlement")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				frmProductionSettlement form2 = new frmProductionSettlement(UserAccount);
				form2.OpenInProductionTab = true;
				form2.OverHeadReadById += SettlementForm_OnOverHeadReadById;
				form2.PackingMaterialReadById += SettlementForm_OnPackingMaterialReadById;
				FormHelper.OpenFormInPanel("frmProductionSettlement", form2, PanelOtherForms);
			}
			else
			{
				PanelOtherForms.Visible = false;
				tabControl1.Dock = DockStyle.Fill;
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					GenerateDocNumberInput();
					txtDocdate.Focus();
				}
				WarehouseInputFill();
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
				{
					GenerateDocNumberOutPut();
				}
				EntryTypeFill();
				Warehouse();
				ItemFillOutPut();
				DocDateOutPut.Focus();
			}
			if (tabControl1.SelectedIndex == 2)
			{
				if (btnSaveConsumption.Visible && btnSaveConsumption.Enabled)
				{
					GenerateDocConsumption();
				}
				ConsumptionDetailComboBind();
				txtDocDateConsumption.Focus();
			}
			_ = tabControl1.SelectedIndex;
			_ = 3;
			_ = tabControl1.SelectedIndex;
			_ = 4;
			_ = tabControl1.SelectedIndex;
			_ = 5;
			_ = tabControl1.SelectedIndex;
			_ = 7;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvailableStockForInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			DataTable dt = CommonServices.GetWeightCurrStockByItem(new ReportsParameters
			{
				WarehouseId = Conversion.ToInt(cmbWareHouseInput.Value),
				ItemId = Conversion.ToInt(cmbItemInput.Value),
				JobLotId = Conversion.ToInt(cmbLotInput.Value),
				CropYear = Conversion.ToString((object)((Control)(object)cmbCropYearInput).Text.Trim()),
				ToDate = Conversion.ToDateTime((object)txtDocdate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtBalanceQty.Text = Conversion.ToDouble(dt.Rows[0]["AvailableQty"]).ToString("#,##0.###");
				txtBalanceWeight.Text = Conversion.ToDouble(dt.Rows[0]["AvailableStock"]).ToString("#,##0.####");
			}
			else
			{
				txtBalanceQty.Text = 0.ToString();
				txtBalanceWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountInputCalu()
	{
		try
		{
			if (Conversion.ToDouble((object)txtNetWeightInput.Text) > 0.0 && Conversion.ToInt(cmbRateuomInput.Value) > 0 && Conversion.ToDouble((object)txtRateInput.Text) > 0.0)
			{
				double num = Conversion.ToDouble((object)txtNetWeightInput.Text);
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)cmbRateuomInput).SelectedRow.Cells["Equivalent"].Value);
				double Rate = Conversion.ToDouble((object)txtRateInput.Text);
				double Amount = num / RateUom * Rate;
				if (Amount > 0.0)
				{
					txtAmountInput.Text = Amount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtAmountInput.Text = 0.ToString();
				}
			}
			else
			{
				txtAmountInput.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountInputCalu();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountInputCalu();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateuomInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountInputCalu();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemInput_TextChanged(object sender, EventArgs e)
	{
	}

	private void cmbWareHouseInput_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForInput();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCropYearInput_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForInput();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLotInput_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumberInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = InvFoodProduction.GetSerialNumber(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocumentTypeId = 80,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUomInput()
	{
		try
		{
			if (dtuomlst.Rows.Count <= 0)
			{
				return;
			}
			string PackUom = ((Control)(object)cmbItemUomInput).Text;
			((Control)(object)cmbItemUomInput).Text = string.Empty;
			((Control)(object)cmbRateuomInput).Text = string.Empty;
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("PackUom");
			dtUom.Columns.Add("Equivalent", typeof(double));
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtuomlst.Rows[i]["ItemId"]) == Convert.ToInt32(cmbItemInput.Value))
				{
					dtUom.Rows.Add(dtuomlst.Rows[i]["Id"], dtuomlst.Rows[i]["UOMCode"], dtuomlst.Rows[i]["Equivalent"]);
				}
			}
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbItemUomInput, "Id", "PackUom", "PackUom", ZeroIndex: false);
				DDL.BindDDL(dtUom, cmbRateuomInput, "Id", "PackUom", "RateUom", ZeroIndex: false);
				((UltraGridBase)cmbItemUomInput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbItemUomInput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbRateuomInput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbRateuomInput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("PackUom") == PackUom))
				{
					((Control)(object)cmbItemUomInput).Text = PackUom;
				}
				else
				{
					((Control)(object)cmbItemUomInput).Text = string.Empty;
				}
				DataRow[] dr = dtUom.Select("Equivalent='" + Conversion.ToDouble((object)40) + "'");
				if (dr.Length != 0)
				{
					cmbRateuomInput.Value = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				else
				{
					((Control)(object)cmbRateuomInput).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvProductionJobOrder Pro = new InvProductionJobOrder();
			Pro.OrganizationId = UserAccount.OrganizationId;
			Pro.CompanyId = UserAccount.CompanyId;
			Pro.Id = Conversion.ToInt(CmbJobOrderNo.Value);
			DataTable dt = InvProductionJobOrder.GetGlAccountsByJobOrderId(Pro);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dt, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)CmbWorkInProcess).DataSource = null;
				((UltraGridBase)CmbWIPItem).DataSource = null;
			}
			GetPlantFeeder(Pro.Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWareHouseInput_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbWareHouseInput).ActiveRow != null)
			{
				GetItemByWareHouseId(Conversion.ToInt(cmbWareHouseInput.Value));
			}
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
			GetAvailableStockForInput();
			bindUomInput();
			GetAvgRateByItemAndJoblot();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRateByItemAndJoblot()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		try
		{
			if (formright.DoHaveCanRateandAmount)
			{
				double rate = 0.0;
				rate = ((!FIFOCGSFlag) ? CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(cmbItemInput.Value), Conversion.ToDateTime((object)txtDocdate.Value), 80, RecId, Conversion.ToInt(cmbLotInput.Value), Conversion.ToInt(cmbCropYearInput.Value), null, Conversion.ToInt(cmbWareHouseInput.Value)) : CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
					WarehouseId = Conversion.ToInt(cmbWareHouseInput.Value),
					ItemId = Conversion.ToInt(cmbItemInput.Value),
					stockUOM = Conversion.ToInt(cmbItemUomInput.Value),
					JobLotId = Conversion.ToInt(cmbLotInput.Value),
					PackingTypeId = Conversion.ToInt(cmbPackingTypeInput.Value),
					CropYear = Conversion.ToString((object)((Control)(object)cmbCropYearInput).Text),
					ItemQty = Conversion.ToDouble((object)txtQtyInput.Text),
					NetWeight = Conversion.ToDouble((object)txtNetWeightInput.Text)
				}));
				if (rate > 0.0)
				{
					txtRateInput.Text = (rate * 40.0).ToString("#,##0.###");
				}
				else
				{
					txtRateInput.Text = "0";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text == "0")
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeeder).ActiveRow == null)
		{
			MessageBox.Show("Plant Name Field Required");
			CmbPlantFeeder.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWorkInProcess).ActiveRow == null)
		{
			MessageBox.Show("WorkInProcess Field Required");
			CmbWorkInProcess.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWIPItem).ActiveRow == null)
		{
			MessageBox.Show("WIP Item Field Required");
			CmbWIPItem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortionInPut()
	{
		if (((UltraGridBase)cmbWareHouseInput).ActiveRow == null)
		{
			MessageBox.Show("Ware house Field Required");
			cmbWareHouseInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemInput).ActiveRow == null)
		{
			MessageBox.Show("ItemName Field Required");
			cmbItemInput.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbItemUomInput.Value) <= 0)
		{
			MessageBox.Show("UOM Field Required");
			cmbItemUomInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYearInput).ActiveRow == null)
		{
			MessageBox.Show("CropYear Field Required");
			cmbCropYearInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLotInput).ActiveRow == null)
		{
			MessageBox.Show("JobLot Field Required");
			cmbLotInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingTypeInput).ActiveRow == null)
		{
			MessageBox.Show("Bag Type Field Required");
			cmbPackingTypeInput.Focus();
			return false;
		}
		if (txtQtyInput.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQtyInput.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQtyInput.Focus();
			return false;
		}
		if (formright.DoHaveCanRateandAmount)
		{
			if (txtRateInput.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRateInput.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Rate Field Required");
				txtRateInput.Focus();
				return false;
			}
			if (((UltraGridBase)cmbRateuomInput).ActiveRow == null || Conversion.ToDouble(cmbRateuomInput.Value) == 0.0)
			{
				MessageBox.Show("RateUom Field Required! " + ((Control)(object)cmbItemInput).Text + "40Kg Uom not Defined");
				cmbRateuomInput.Focus();
				return false;
			}
			if (txtAmountInput.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountInput.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtAmountInput.Focus();
				return false;
			}
		}
		if (txtNetWeightInput.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetWeightInput.Text.Trim()) == 0.0)
		{
			MessageBox.Show("NetWeight Field Required");
			txtNetWeightInput.Focus();
			return false;
		}
		return true;
	}

	private void InsertInput()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvFoodProduction invFoodProduction = new InvFoodProduction();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invFoodProduction.DocumentTypeId = 80;
			invFoodProduction.DocCode = Conversion.ToInt((object)txtdocnumber.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invFoodProduction.MainRemarks = txtRemarks.Text.ToString();
			invFoodProduction.EntryType = "Input";
			invFoodProduction.InvFoodProductionPlanId = 0;
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.EBDepartmentId = Conversion.ToInt(CmbProductionDepartment.Value);
			invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNo).Text.Trim());
			if (Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNo).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
			{
				invFoodProduction.BaseDocumentTypeId = 2;
			}
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantFeeder.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (InputDetailRowsRemoveIds != null && InputDetailRowsRemoveIds != string.Empty)
			{
				invFoodProduction.InputDetailRowsRemoveIds = InputDetailRowsRemoveIds;
			}
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			invFoodProduction.InvFoodProductionConsumptionDetailslist = new List<InvFoodProductionConsumptionDetail>();
			if (grdStockConversionProductionDetail.RowCount > 0)
			{
				double GrossWeight = 0.0;
				GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (!(Conversion.ToDouble((object)r.Cells["Weight"].Text) > 0.0))
					{
						continue;
					}
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					Conversiondetail.EntryType = Conversion.ToString((object)"Issue");
					Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					Conversiondetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					Conversiondetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					_ = LoaderStatus;
					if (r.Cells["WareHouseId"].Text == string.Empty || r.Cells["WareHouseId"].Text == "0")
					{
						MessageBox.Show("WareHouse Field Require in Input Detail");
						return;
					}
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || r.Cells["ItemId"].Text == "0")
					{
						MessageBox.Show("Item Field Require in Input Detail");
						return;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					if (r.Cells["CropYear"].Text == string.Empty || r.Cells["CropYear"].Text == "0")
					{
						MessageBox.Show("CropYear Field Require in Input Detail");
						return;
					}
					Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
					if (r.Cells["JobLotId"].Text == string.Empty)
					{
						MessageBox.Show("JobLot Field Require in Input Detail");
						return;
					}
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Conversiondetail.PackUnit = Conversion.ToInt(r.Cells["ItemUOM"].Value);
					Conversiondetail.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					GrossWeight += Conversiondetail.Weight;
					if (Conversiondetail.Weight == 0.0)
					{
						throw new Exception("ItemWeight Field Required");
					}
					Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					Conversiondetail.NetRate = Conversiondetail.Rate;
					Conversiondetail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
					if (!formright.DoHaveCanRateandAmount && Conversiondetail.RefDocumentTypeId == 0 && Conversiondetail.RefDocNoId == 0)
					{
						Conversiondetail.Rate = 0.0;
						Conversiondetail.NetRate = 0.0;
						Conversiondetail.Amount = 0.0;
						Conversiondetail.TotalAmount = 0.0;
					}
					else
					{
						Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						Conversiondetail.NetRate = Conversiondetail.Rate;
						Conversiondetail.Amount = Conversion.ToInt(r.Cells["ItemAmount"].Value);
						Conversiondetail.TotalAmount = Conversiondetail.Amount;
					}
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					if (Conversion.ToDouble(r.Cells["RateUOMForCheck"].Value) != 0.0)
					{
						DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
						if (dt.Rows.Count > 0)
						{
							DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r.Cells["RateUOMForCheck"].Value) + "'");
							if (dr.Length == 0)
							{
								MessageBox.Show("RateUOM On Row No " + (r.RowIndex + 1) + " is not define please check");
								return;
							}
							Conversiondetail.RateUOMId = Conversion.ToInt(dr[0].ItemArray[0]);
						}
					}
					Conversiondetail.VoucherHeadId = 0;
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}
				int success = InvFoodProduction.Save(invFoodProduction);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				if (WagesStatus && WagesActiveOrInActiveForInput)
				{
					CommonServices.WagesDeleteByReferenceIds(80, success);
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 80;
					frmwagesBillHeader.RefDocId = success;
					frmwagesBillHeader.GrossWeightTotal = GrossWeight;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				reset();
			}
			else
			{
				MessageBox.Show("Enter Detail First ...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdInput(int ID)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			RecId = ID;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			InputDetailRowsRemoveIds = "";
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC == null)
			{
				return;
			}
			if (SC.IsApproved)
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString((object)SC.PlanStatus) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString((object)SC.PlanStatus) + " JobPlanStatus");
			}
			txtdocnumber.Text = Conversion.ToString((object)SC.DocCode);
			txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
			txtRemarks.Text = Conversion.ToString((object)SC.MainRemarks);
			CmbProductionDepartment.Value = SC.EBDepartmentId;
			CmbJobOrderNo.Value = SC.InvJobOrderId;
			CmbJobOrderNo_Leave(null, null);
			CmbPlantFeeder.Value = SC.PlantId;
			table.Rows.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType == "Issue")
				{
					tabControl1.SelectedIndex = 0;
					tabControl2.SelectedIndex = 0;
					table.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].RefDocumentTypeId, SC.invFoodProductionDetails[i].RefDocumentType, SC.invFoodProductionDetails[i].RefDocNoId, SC.invFoodProductionDetails[i].RefDocSubIdNo, SC.invFoodProductionDetails[i].SupplierCustomerId, SC.invFoodProductionDetails[i].PartyName, Convert.ToDateTime(SC.invFoodProductionDetails[i].RefDocDate).ToString("dd-MMM-yy"), SC.invFoodProductionDetails[i].RefDocNo, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].ItemUomId, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].RateEquivalent, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
			}
			GetVoucherHeadId(RecId, 80);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			InsertInput();
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
			InsertInput();
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
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			InputDetailRowsRemoveIds = "";
			RecId = 0;
			VoucherHeadId = 0;
			txtRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbJobOrderNo).Enabled = true;
			table.Clear();
			grdStockConversionProductionDetail.ClearStructure();
			GenerateDocNumberInput();
			((UltraGridBase)CmbJobOrderNo).DataSource = null;
			((Control)(object)CmbJobOrderNo).Text = string.Empty;
			JobOrderNoFill();
			((Control)(object)CmbPlantFeeder).Text = string.Empty;
			ResetInputFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetInputFields()
	{
		try
		{
			((Control)(object)cmbWareHouseInput).Text = string.Empty;
			((Control)(object)cmbItemInput).Text = string.Empty;
			((UltraGridBase)cmbItemInput).DataSource = null;
			((Control)(object)cmbItemUomInput).Text = string.Empty;
			((UltraGridBase)cmbItemUomInput).DataSource = null;
			((Control)(object)cmbCropYearInput).Text = string.Empty;
			((Control)(object)cmbLotInput).Text = string.Empty;
			((Control)(object)cmbPackingTypeInput).Text = string.Empty;
			txtBalanceQty.Text = string.Empty;
			txtBalanceWeight.Text = string.Empty;
			txtQtyInput.Text = string.Empty;
			txtNetWeightInput.Text = string.Empty;
			txtRemarksInput.Text = string.Empty;
			txtRateInput.Text = string.Empty;
			((Control)(object)cmbRateuomInput).Text = string.Empty;
			((UltraGridBase)cmbRateuomInput).DataSource = null;
			txtAmountInput.Text = string.Empty;
			btnAddInput.Visible = true;
			btnUpdateDetailInput.Visible = false;
			btnCancelInput.Visible = false;
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
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 80);
			WagesActiveOrInActiveForInput = obj != null && obj.IsActive;
			JobOrderNoFill();
			CropYearInput();
			JobLotInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		try
		{
			grdStockConversionProductionDetail.UpdateData();
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemEquivalent"].Value.ToString());
				double Weight3 = 0.0;
				if (Conversion.ToDouble(item.Cells["BalanceQty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BalanceQty"].Value) < Qty && (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0))
				{
					item.Cells["Quantity"].Value = 0;
					item.Cells["Weight"].Value = 0;
					Qty = 0.0;
					MessageBox.Show("Qty cannot greater than BalanceQty Please check!");
				}
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					Weight3 = Conversion.ToDouble(item.Cells["BalanceWeight"].Value) / Conversion.ToDouble(item.Cells["BalanceQty"].Value) * Qty;
					if (Conversion.ToDouble(item.Cells["BalanceWeight"].Value) < Weight3)
					{
						Weight3 = 0.0;
						item.Cells["Weight"].Value = 0;
						MessageBox.Show("Weight cannot greater than Balance Weight Please check!");
					}
				}
				else
				{
					Weight3 = Qty * Uom;
				}
				item.Cells["Weight"].Value = Weight3;
				if (FIFOCGSFlag && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) <= 0)
				{
					double rate2 = CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
						WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value),
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						ItemQty = Conversion.ToDouble(item.Cells["Quantity"].Value),
						NetWeight = Conversion.ToDouble((object)item.Cells["Weight"].Text)
					});
					item.Cells["Rate"].Value = rate2 * 40.0;
				}
				double Rate3 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM3 = Conversion.ToDouble(item.Cells["RateUOMForCheck"].Value);
				double Amount3 = Weight3 / RateUOM3 * Rate3;
				item.Cells["ItemAmount"].Value = Amount3;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight2 = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				if (Conversion.ToDouble((object)item.Cells["BalanceWeight"].Value.ToString()) < Weight2 && (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0))
				{
					item.Cells["Weight"].Value = 0;
					Weight2 = 0.0;
					MessageBox.Show("Weight cannot greater than BalanceWeight Please check!");
				}
				if (FIFOCGSFlag && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) <= 0)
				{
					double rate = CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
						WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value),
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						ItemQty = Conversion.ToDouble(item.Cells["Quantity"].Value),
						NetWeight = Conversion.ToDouble((object)item.Cells["Weight"].Text)
					});
					item.Cells["Rate"].Value = rate * 40.0;
				}
				double Rate2 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUOM2 = Conversion.ToDouble(item.Cells["RateUOMForCheck"].Value);
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Math.Round(Amount2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "Rate" || e.Column.Key == "RateUOM")
			{
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0)
				{
					item.Cells["RateUOM"].Value = item.Cells["RateUOMForCheck"].Value;
					item.Cells["Rate"].Value = item.Cells["RateForCheck"].Value;
					throw new Exception("The item rate or rate unit of measure (RateUom) is not editable because this entry is loaded from the loader.");
				}
				double Amount = 0.0;
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				Amount = ((RateUOM != 0.0 && Rate != 0.0) ? (Weight / RateUOM * Rate) : 0.0);
				item.Cells["ItemAmount"].Value = Amount;
			}
			grdStockConversionProductionDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
					{
						item.Delete();
					}
					else
					{
						InputDetailRowsRemoveIds = InputDetailRowsRemoveIds + "," + Conversion.ToString(item.Cells["Id"].Value);
						item.Delete();
					}
				}
			}
			grdStockConversionProductionDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockConversionProductionDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (item != null)
			{
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					throw new Exception("The record was not updated as it was loaded directly from the Loader.");
				}
				updateDetailIndexInput = item.RowIndex;
				cmbWareHouseInput.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbItemInput.Value = item.Cells["ItemId"].Value.ToString();
				cmbItemInput_Leave(null, null);
				cmbItemUomInput.Value = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
				((Control)(object)cmbCropYearInput).Text = item.Cells["CropYear"].Text.ToString();
				cmbLotInput.Value = item.Cells["JobLotId"].Value.ToString();
				cmbPackingTypeInput.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtBalanceQty.Text = item.Cells["BalanceQty"].Text.ToString();
				txtBalanceWeight.Text = item.Cells["BalanceWeight"].Text.ToString();
				txtQtyInput.Text = item.Cells["Quantity"].Text.ToString();
				txtNetWeightInput.Text = item.Cells["Weight"].Text.ToString();
				txtQtyOutPut.Text = item.Cells["Quantity"].Text.ToString();
				txtNetWeightOutPut.Text = item.Cells["Weight"].Text.ToString();
				txtRateInput.Text = item.Cells["Rate"].Value.ToString();
				cmbRateuomInput.Value = item.Cells["RateUOMId"].Value.ToString();
				txtAmountInput.Text = item.Cells["ItemAmount"].Text.ToString();
				txtRemarksInput.Text = item.Cells["Remarks"].Text.ToString();
				btnAddInput.Visible = false;
				btnUpdateDetailInput.Visible = true;
				btnCancelInput.Visible = true;
				cmbWareHouseInput.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettings()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdStockConversionProductionDetail.RootTable.Columns["Rate"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdStockConversionProductionDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PartyName"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemEquivalent"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateForCheck"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMForCheck"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocIdNo"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubIdNo"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["PartyName"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdStockConversionProductionDetail.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdStockConversionProductionDetail.RootTable.Columns["Id"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateForCheck"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMForCheck"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemEquivalent"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].Width = 150;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].Width = 300;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdStockConversionProductionDetail);
			grdStockConversionProductionDetail.RootTable.Columns.Add("Delete");
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].Caption = "X";
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].Width = 35;
			grdStockConversionProductionDetail.RootTable.Columns["Delete"].Position = 0;
			grdStockConversionProductionDetail.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionInPut())
			{
				table.Rows.Add(0, 0, "", 0, 0, 0, "", 0, 0, cmbWareHouseInput.Value, ((Control)(object)cmbWareHouseInput).Text, cmbItemInput.Value, ((Control)(object)cmbItemInput).Text, cmbItemUomInput.Value, ((Control)(object)cmbItemUomInput).Text, Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["Equivalent"].Value), ((Control)(object)cmbCropYearInput).Text, cmbLotInput.Value, ((Control)(object)cmbLotInput).Text, cmbPackingTypeInput.Value, ((Control)(object)cmbPackingTypeInput).Text, Conversion.ToDouble((object)txtBalanceQty.Text), Conversion.ToDouble((object)txtBalanceWeight.Text), Conversion.ToDouble((object)txtQtyInput.Text), Conversion.ToDouble((object)txtNetWeightInput.Text), Conversion.ToDouble((object)txtRateInput.Text), Conversion.ToDouble((object)txtRateInput.Text), cmbRateuomInput.Value, ((Control)(object)cmbRateuomInput).Text, (Conversion.ToInt(cmbRateuomInput.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)cmbRateuomInput).SelectedRow.Cells["Equivalent"].Value) : 0, Conversion.ToDouble((object)txtAmountInput.Text), txtRemarksInput.Text);
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
				ResetInputFields();
				cmbWareHouseInput.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionInPut())
			{
				table.Rows[updateDetailIndexInput]["WareHouseId"] = cmbWareHouseInput.Value;
				table.Rows[updateDetailIndexInput]["WareHouse"] = ((Control)(object)cmbWareHouseInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["JobLotId"] = cmbLotInput.Value;
				table.Rows[updateDetailIndexInput]["JobLot"] = ((Control)(object)cmbLotInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["PackingTypeId"] = cmbPackingTypeInput.Value;
				table.Rows[updateDetailIndexInput]["PackingType"] = ((Control)(object)cmbPackingTypeInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["ItemId"] = cmbItemInput.Value;
				table.Rows[updateDetailIndexInput]["Item"] = ((Control)(object)cmbItemInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["ItemUOMId"] = cmbItemUomInput.Value;
				table.Rows[updateDetailIndexInput]["ItemUOM"] = ((Control)(object)cmbItemUomInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["ItemEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndexInput]["CropYear"] = ((Control)(object)cmbCropYearInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["Quantity"] = txtQtyInput.Text.Trim();
				table.Rows[updateDetailIndexInput]["BalanceQty"] = txtBalanceQty.Text.Trim();
				table.Rows[updateDetailIndexInput]["BalanceWeight"] = txtBalanceWeight.Text.Trim();
				table.Rows[updateDetailIndexInput]["Weight"] = txtNetWeightInput.Text.Trim();
				table.Rows[updateDetailIndexInput]["Rate"] = txtRateInput.Text.Trim();
				table.Rows[updateDetailIndexInput]["RateForCheck"] = txtRateInput.Text.Trim();
				table.Rows[updateDetailIndexInput]["RateUOM"] = ((Control)(object)cmbRateuomInput).Text.Trim();
				table.Rows[updateDetailIndexInput]["RateUOMForCheck"] = ((UltraDropDownBase)cmbRateuomInput).SelectedRow.Cells["Equivalent"].Value;
				table.Rows[updateDetailIndexInput]["RateUOMId"] = cmbRateuomInput.Value;
				table.Rows[updateDetailIndexInput]["ItemAmount"] = txtAmountInput.Text.Trim();
				table.Rows[updateDetailIndexInput]["Remarks"] = txtRemarksInput.Text.Trim();
				ResetInputFields();
				cmbWareHouseInput.Focus();
				CommonServices.GridAutoAdjustmentNew(grdStockConversionProductionDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelInput_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddInput.Visible = true;
			btnUpdateDetailInput.Visible = false;
			btnCancelInput.Visible = false;
			ResetInputFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddOutPut.Visible = true;
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
			resetDetailOutPut();
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
				GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
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
					table.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocumentType"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], dtpurchaseinvoice.Rows[i]["SupplierCustomerId"], dtpurchaseinvoice.Rows[i]["PartyName"], Conversion.ToDateTime(dtpurchaseinvoice.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpurchaseinvoice.Rows[i]["DocNo"], dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["PackUom"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["RateUOM"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"], "");
				}
			}
			grdStockConversionProductionDetail.DataSource = table;
			grdStockConversionProductionDetail.RetrieveStructure();
			gridsettings();
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
			CommonServices.ProductionInPutAndOutPut601(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void jobOrderInputHCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "80",
				Activity = "JobOrder"
			});
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("JobOrder");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, CmbJobOrderInputHistory, "Id", "JobOrder", "Job Order", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbJobOrderInputHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderInputHistory).DataSource = null;
			}
			else
			{
				((Control)(object)CmbJobOrderInputHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderInputHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefereshInputHistory_Click(object sender, EventArgs e)
	{
		try
		{
			jobOrderInputHCombobind();
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
			if (tabControl2.SelectedIndex == 1)
			{
				txtFromdateHistory.Focus();
			}
			else
			{
				txtDocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetInputhistory()
	{
		try
		{
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderInputHistory).Text = string.Empty;
			txtFromdateHistory.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtFromdateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewInputHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetInputhistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowInputHistory_Click(object sender, EventArgs e)
	{
		try
		{
			InputGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		try
		{
			DataTable dthis = InvFoodProduction.ProductionFormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value),
				ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value),
				FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text),
				ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text),
				JobOrderId = Conversion.ToInt(CmbJobOrderInputHistory.Value)
			});
			if (dthis.Rows.Count > 0)
			{
				DataTable dtgrdinput = new DataTable();
				dtgrdinput.Columns.Add("Id");
				dtgrdinput.Columns.Add("DocNo", typeof(int));
				dtgrdinput.Columns.Add("WagesNo", typeof(int));
				dtgrdinput.Columns.Add("DocDate", typeof(DateTime));
				dtgrdinput.Columns.Add("JobOrderNo");
				dtgrdinput.Columns.Add("PlantName");
				dtgrdinput.Columns.Add("EntryType");
				dtgrdinput.Columns.Add("Remarks");
				dtgrdinput.Columns.Add("IsApproved");
				dtgrdinput.Columns.Add("StartDate", typeof(DateTime));
				dtgrdinput.Columns.Add("EndDate", typeof(DateTime));
				dtgrdinput.Columns.Add("EntryDate", typeof(DateTime));
				dtgrdinput.Columns.Add("EntryUser", typeof(string));
				dtgrdinput.Columns.Add("ModifyDate", typeof(DateTime));
				dtgrdinput.Columns.Add("ModifyUser", typeof(string));
				dtgrdinput.Columns.Add("PlanStatus", typeof(string));
				dtgrdinput.Columns.Add("WagesId", typeof(int));
				dtgrdinput.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					if (Conversion.ToInt(dthis.Rows[i]["DocumentTypeId"]) == 80)
					{
						dtgrdinput.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], dthis.Rows[i]["WagesNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["PlantName"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
					}
				}
				grdinputhistory.DataSource = dtgrdinput;
				grdinputhistory.RetrieveStructure();
				GridInputSetting();
			}
			else
			{
				grdinputhistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridInputSetting()
	{
		try
		{
			grdinputhistory.RootTable.Columns["Id"].Visible = false;
			grdinputhistory.RootTable.Columns["WagesId"].Visible = false;
			grdinputhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdinputhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdinputhistory);
			grdinputhistory.RootTable.Columns.Add("Edit");
			grdinputhistory.RootTable.Columns["Edit"].Key = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdinputhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdinputhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].Width = 40;
			grdinputhistory.RootTable.Columns.Add("WagesPrint");
			grdinputhistory.RootTable.Columns["WagesPrint"].Key = "WagesPrint";
			grdinputhistory.RootTable.Columns["WagesPrint"].Caption = "WagesPrint";
			grdinputhistory.RootTable.Columns["WagesPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdinputhistory.RootTable.Columns["WagesPrint"].ButtonStyle = (ButtonStyle)5;
			grdinputhistory.RootTable.Columns["WagesPrint"].ButtonText = "WagesPrint";
			grdinputhistory.RootTable.Columns["WagesPrint"].Width = 60;
			grdinputhistory.RootTable.Columns["WagesPrint"].Position = 0;
			grdinputhistory.RootTable.Columns["Edit"].Position = 1;
			grdinputhistory.FrozenColumns = 2;
			grdinputhistory.RootTable.Columns["Remarks"].Width = 300;
			ctrlGrdBar10_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdinputhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && grdinputhistory.CurrentRow.Cells["EntryType"].Value.ToString() == "Input")
			{
				if (Conversion.ToString(grdinputhistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString(grdinputhistory.CurrentRow.Cells["PlanStatus"].Value) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString(grdinputhistory.CurrentRow.Cells["PlanStatus"].Value) + " JobPlanStatus");
				}
				RecId = Conversion.ToInt(grdinputhistory.CurrentRow.Cells["Id"].Value);
				ReadByIdInput(RecId);
			}
			if (e.Column.Key == "WagesPrint")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(grdinputhistory.CurrentRow.Cells["WagesId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAmountInput_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtBalanceQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBalanceWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyInput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetWeightInput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateInput_KeyPress_1(object sender, KeyPressEventArgs e)
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

	private void txtAmountInput_KeyPress_1(object sender, KeyPressEventArgs e)
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

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdStockConversionProductionDetail).Name);
			if (fs != null)
			{
				grdStockConversionProductionDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar10_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdinputhistory).Name);
			if (fs != null)
			{
				grdinputhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnJobOrderDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new frmProductionJobOrderMain(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void GetLastItemRateByJobOrder()
	{
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		try
		{
			if (((Control)(object)CmbEntryType).Text == "Wastage")
			{
				txtRateOutPut.Enabled = true;
				((Control)(object)CmbRateUomOutPut).Enabled = true;
			}
			else if (((Control)(object)CmbEntryType).Text == "ByProduct")
			{
				if (ByProductRateEditAbleOrNot)
				{
					txtRateOutPut.Enabled = true;
					((Control)(object)CmbRateUomOutPut).Enabled = true;
				}
				else
				{
					txtRateOutPut.Enabled = false;
					((Control)(object)CmbRateUomOutPut).Enabled = false;
				}
			}
			else
			{
				txtRateOutPut.Enabled = false;
				((Control)(object)CmbRateUomOutPut).Enabled = false;
			}
			if (Conversion.ToInt(CmbEntryType.Value) != 3)
			{
				if (Conversion.ToInt(CmbJobOrderNoOutput.Value) > 0 && Conversion.ToInt(CmbEntryType.Value) > 0 && Conversion.ToInt(CmbItemOutput.Value) > 0)
				{
					DataTable dt = InvProductionJobOrder.GetLastItemRateByJobOrder(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
						TypeId = Conversion.ToInt(CmbEntryType.Value),
						ItemId = Conversion.ToInt(CmbItemOutput.Value),
						EffectedDate = DocDateOutPut.Value
					});
					if (dt != null && dt.Rows.Count > 0)
					{
						ScheduleId = Conversion.ToInt(dt.Rows[0]["Id"]);
						txtRateOutPut.Text = Conversion.ToDouble(dt.Rows[0]["ItemRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
						return;
					}
					ScheduleId = 0;
					if (((Control)(object)CmbEntryType).Text == "FinishGoods")
					{
						txtRateOutPut.Text = txtAvgFinishGoodRate.Text;
					}
					else if (grdByProduct.CurrentRow != null && grdByProduct.CurrentRow.Cells.Count > 0 && updateDetailIndex > -1 && Conversion.ToDouble(grdByProduct.CurrentRow.Cells["Rate"].Value) > 0.0)
					{
						if (Conversion.ToInt(CmbItemOutput.Value) == Conversion.ToInt(grdByProduct.CurrentRow.Cells["ItemId"].Value))
						{
							txtRateOutPut.Text = Conversion.ToDouble(grdByProduct.CurrentRow.Cells["Rate"].Value).ToString("#,##0.###");
						}
						else
						{
							txtRateOutPut.Text = "0";
						}
					}
					else
					{
						txtRateOutPut.Text = "0";
					}
					return;
				}
				ScheduleId = 0;
				if (Conversion.ToInt(CmbJobOrderNoOutput.Value) <= 0)
				{
					return;
				}
				if (((Control)(object)CmbEntryType).Text == "FinishGoods")
				{
					txtRateOutPut.Text = txtAvgFinishGoodRate.Text;
				}
				else if (updateDetailIndex > -1 && Conversion.ToDouble(grdByProduct.CurrentRow.Cells["Rate"].Value) > 0.0)
				{
					if (Conversion.ToInt(CmbItemOutput.Value) == Conversion.ToInt(grdByProduct.CurrentRow.Cells["ItemId"].Value))
					{
						txtRateOutPut.Text = Conversion.ToDouble(grdByProduct.CurrentRow.Cells["Rate"].Value).ToString("#,##0.###");
					}
					else
					{
						txtRateOutPut.Text = "0";
					}
				}
				else
				{
					txtRateOutPut.Text = "0";
				}
			}
			else
			{
				DataTable dt2 = InvProductionJobOrder.AvgRateForReturnToGodown_Production(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 80,
					JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
					PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value),
					ItemId = Conversion.ToInt(CmbItemOutput.Value),
					CropYear = Conversion.ToString((object)((Control)(object)CmbCropYearOutPut).Text)
				});
				if (dt2 != null && dt2.Rows.Count > 0)
				{
					txtRateOutPut.Text = Conversion.ToDouble(dt2.Rows[0]["AvgRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
					CmbRateUomOutPut.Value = Conversion.ToInt(dt2.Rows[0]["RateUomId"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewOutPut_Click(object sender, EventArgs e)
	{
		resetOutPut();
		resetDetailOutPut();
	}

	private void btnRefreshOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 112);
			WagesActiveOrInActiveForOutPut = obj != null && obj.IsActive;
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			OutputItemsByJobOrderRateSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OutputItemsByJobOrderRateSchedule"));
			UOMFill();
			if (Conversion.ToInt(CmbEntryType.Value) == 3)
			{
				ItemBindForReturnToGodown();
				ItemFillOutPut();
			}
			else
			{
				if (OutputItemsByJobOrderRateSchedule)
				{
					AllItemForOutPutAgainstJobOrder();
				}
				else
				{
					AllItemForOutPut();
				}
				ItemFillOutPut();
				CropYearOutPut();
				JobLotOutPut();
			}
			Warehouse();
			bagType();
			JobOrderNoFill();
			getProductionInputForReturnToGodown();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOutPut()
	{
		if (txtDocNoOutPut.Text.Trim() == string.Empty || txtDocNoOutPut.Text == "0")
		{
			MessageBox.Show("DocNo Field Required");
			txtDocNoOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow == null)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNoOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeederOutput).ActiveRow == null)
		{
			MessageBox.Show("Plant Name Field Is Required");
			CmbPlantFeederOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWorkInProcess).ActiveRow == null)
		{
			MessageBox.Show("WorkInProcess Field Required");
			CmbWorkInProcess.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWIPItem).ActiveRow == null)
		{
			MessageBox.Show("WIP Item Field Required");
			CmbWIPItem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortionOutPut()
	{
		if (((UltraGridBase)CmbEntryType).ActiveRow == null)
		{
			MessageBox.Show("Entry Field Required");
			CmbEntryType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouseOutput).ActiveRow == null)
		{
			MessageBox.Show("Ware house Field Required");
			CmbWarehouseOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemOutput).ActiveRow == null)
		{
			MessageBox.Show("ItemName Field Required");
			CmbItemOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbUomOutPut).ActiveRow == null)
		{
			MessageBox.Show("UOM Field Required");
			CmbUomOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYearOutPut).ActiveRow == null)
		{
			MessageBox.Show("CropYear Field Required");
			CmbCropYearOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLotOutPut).ActiveRow == null)
		{
			MessageBox.Show("JobLot Field Required");
			CmbLotOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackTypeOutPut).ActiveRow == null)
		{
			MessageBox.Show("Bag Type Field Required");
			CmbPackTypeOutPut.Focus();
			return false;
		}
		if (txtQtyOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQtyOutPut.Focus();
			return false;
		}
		if (ScheduleId > 0)
		{
			if (txtRateOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRateOutPut.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Rate Field Required");
				txtRateOutPut.Focus();
				return false;
			}
			if (((UltraGridBase)CmbRateUomOutPut).ActiveRow == null || Conversion.ToDouble(CmbRateUomOutPut.Value) == 0.0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUomOutPut.Focus();
				return false;
			}
			if (txtAmountOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountOutPut.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtAmountOutPut.Focus();
				return false;
			}
		}
		else if (formright.DoHaveCanRateandAmount)
		{
			if (txtRateOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRateOutPut.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Rate Field Required");
				txtRateOutPut.Focus();
				return false;
			}
			if (((UltraGridBase)CmbRateUomOutPut).ActiveRow == null || Conversion.ToDouble(CmbRateUomOutPut.Value) == 0.0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUomOutPut.Focus();
				return false;
			}
			if (txtAmountOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountOutPut.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtAmountOutPut.Focus();
				return false;
			}
		}
		if (txtNetWeightOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Weight Field Required");
			txtNetWeightOutPut.Focus();
			return false;
		}
		return true;
	}

	private void ExportContractScheduleBind(int JobOrderId)
	{
		try
		{
			DataTable dt = InvProductionJobOrder.ExportContract_GetForProduction(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, JobOrderId, RecId);
			InfragisticsHelper.BindAndRetainSelection(CmbExportContractSchedule, dt, "ContractScheduleId", "ScheduleCode", "Contract Schedule", AllColumns: true, Conversion.ToInt(CmbExportContractSchedule.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbExportContractSchedule).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbExportContractSchedule).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotOutPut()
	{
		try
		{
			int Id = Conversion.ToInt(CmbLotOutPut.Value);
			if (Conversion.ToInt(CmbEntryType.Value) != 3 && dtJobGlobal.Rows.Count > 0)
			{
				dtJobOutPut.Clear();
				foreach (DataRow r in dtJobGlobal.Rows)
				{
					dtJobOutPut.Rows.Add(r["Id"], r["JobLotDescription"]);
				}
			}
			if (dtJobOutPut.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobOutPut, CmbLotOutPut, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtJobOutPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbLotOutPut.Value = Id;
					}
					else
					{
						((Control)(object)CmbLotOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbLotOutPut).Text = string.Empty;
				((Control)(object)CmbLotOutPut).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearOutPut()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCropYearOutPut.Value);
			if (Conversion.ToInt(CmbEntryType.Value) != 3 && dtCropGlobal.Rows.Count > 0)
			{
				dtCropOutPut.Clear();
				foreach (DataRow r in dtCropGlobal.Rows)
				{
					dtCropOutPut.Rows.Add(r["Id"], r["CropYear"]);
				}
			}
			if (dtCropOutPut.Rows.Count > 0)
			{
				DDL.BindDDL(dtCropOutPut, CmbCropYearOutPut, "Id", "CropYear", "Crop Year", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtCropOutPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCropYearOutPut.Value = Id;
					}
					else
					{
						((Control)(object)CmbCropYearOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropYearOutPut).Text = string.Empty;
				((Control)(object)CmbCropYearOutPut).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllItemForOutPut()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtItemOuPutGlobal.Rows.Clear();
			Item item = new Item();
			item.OrganizationId = UserAccount.OrganizationId;
			item.CompanyId = UserAccount.CompanyId;
			dtItemOuPutGlobal = Item.ReadAllItems(item);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllItemForOutPutAgainstJobOrder()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtItemOuPutGlobal.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			dtItemOuPutGlobal = InvProductionJobOrder.GetByProductAndFinishGoodItemsByJobOrderId(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFillOutPut()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemOutput.Value);
			if (Conversion.ToInt(CmbEntryType.Value) != 3 && dtItemOuPutGlobal.Rows.Count > 0)
			{
				dtItemOutPut.Clear();
				if (OutputItemsByJobOrderRateSchedule)
				{
					if (Conversion.ToInt(CmbJobOrderNoOutput.Value) == 0)
					{
						((Control)(object)CmbItemOutput).Text = string.Empty;
						((UltraGridBase)CmbItemOutput).DataSource = null;
						CmbJobOrderNoOutput.Focus();
						MessageBox.Show("Please Select JobOrder First!");
						return;
					}
					DataTable dt = new DataTable();
					if (Conversion.ToInt(CmbEntryType.Value) == 1 || Conversion.ToInt(CmbEntryType.Value) == 4)
					{
						EnumerableRowCollection<DataRow> rows = from r in dtItemOuPutGlobal.AsEnumerable()
							where r.Field<int>("TransTypeId") == 1
							select r;
						dt = (rows.Any() ? rows.CopyToDataTable() : dtItemOuPutGlobal.Clone());
					}
					else if (Conversion.ToInt(CmbEntryType.Value) == 2)
					{
						EnumerableRowCollection<DataRow> rows2 = from r in dtItemOuPutGlobal.AsEnumerable()
							where r.Field<int>("TransTypeId") == 2
							select r;
						dt = (rows2.Any() ? rows2.CopyToDataTable() : dtItemOuPutGlobal.Clone());
					}
					if (dt.Rows.Count > 0)
					{
						foreach (DataRow r2 in dt.Rows)
						{
							dtItemOutPut.Rows.Add(r2["Id"], r2["ItemName"]);
						}
					}
				}
				else
				{
					foreach (DataRow r3 in dtItemOuPutGlobal.Rows)
					{
						dtItemOutPut.Rows.Add(r3["Id"], r3["ItemName"]);
					}
				}
			}
			if (dtItemOutPut.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemOutPut, CmbItemOutput, "Id", "ItemName", "ItemName", ZeroIndex: false);
				((UltraGridBase)CmbItemOutput).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (Id > 0)
				{
					if (dtItemOutPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbItemOutput.Value = Id;
					}
					else
					{
						((Control)(object)CmbItemOutput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemOutput).Text = string.Empty;
				((UltraGridBase)CmbItemOutput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumberOutPut()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = InvFoodProduction.GetSerialNumber(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocumentTypeId = 112,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoOutPut.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUomOutPut()
	{
		try
		{
			if (dtuomlst.Rows.Count > 0)
			{
				string PackUom = ((Control)(object)CmbUomOutPut).Text;
				((Control)(object)CmbUomOutPut).Text = string.Empty;
				string RateUom = ((Control)(object)CmbRateUomOutPut).Text;
				((Control)(object)CmbRateUomOutPut).Text = string.Empty;
				int selectedItemId = Conversion.ToInt(CmbItemOutput.Value);
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("Id");
				dtUom.Columns.Add("UomCode");
				dtUom.Columns.Add("Equivalent");
				DataRow[] array = dtuomlst.Select($"ItemId = {selectedItemId}");
				foreach (DataRow row2 in array)
				{
					dtUom.Rows.Add(row2["Id"], row2["UOMCode"], row2["Equivalent"]);
				}
				if (dtUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtUom, CmbUomOutPut, "Id", "UomCode", "UOM", ZeroIndex: false);
					((UltraGridBase)CmbUomOutPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					DDL.BindDDL(dtUom, CmbRateUomOutPut, "Id", "UomCode", "UOM", ZeroIndex: false);
					((UltraGridBase)CmbRateUomOutPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
					{
						((Control)(object)CmbUomOutPut).Text = PackUom;
					}
					else
					{
						((Control)(object)CmbUomOutPut).Text = string.Empty;
					}
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
					{
						((Control)(object)CmbRateUomOutPut).Text = RateUom;
					}
					else
					{
						((Control)(object)CmbRateUomOutPut).Text = string.Empty;
					}
				}
				else
				{
					((Control)(object)CmbUomOutPut).Text = string.Empty;
					((Control)(object)CmbRateUomOutPut).Text = string.Empty;
					((UltraGridBase)CmbUomOutPut).DataSource = null;
					((UltraGridBase)CmbRateUomOutPut).DataSource = null;
				}
				DataRow[] dr = dtUom.Select("Equivalent='" + Conversion.ToInt((object)40) + "'");
				if (dr.Length != 0)
				{
					CmbRateUomOutPut.Value = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				else
				{
					((Control)(object)CmbRateUomOutPut).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbUomOutPut).Text = string.Empty;
				((Control)(object)CmbRateUomOutPut).Text = string.Empty;
				((UltraGridBase)CmbUomOutPut).DataSource = null;
				((UltraGridBase)CmbRateUomOutPut).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemOutput_Leave(object sender, EventArgs e)
	{
		try
		{
			OtherComboBindForReturnToGodownByItem();
			bindUomOutPut();
			GetLastItemRateByJobOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUomOutPut_Leave(object sender, EventArgs e)
	{
		CalculateGrossWeight();
	}

	private void CmbJobOrderNoOutput_Leave(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		try
		{
			OutPutPlantFillFromProductionInput(Conversion.ToInt(CmbJobOrderNoOutput.Value), UserAccount.BranchesId);
			DataTable dt = InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
				PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InQty = 0.0;
				double InWeight = 0.0;
				double ReturnQty = 0.0;
				double ReturnWeight = 0.0;
				double OutQty = 0.0;
				double OutWeight = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Input")
					{
						InQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						InWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "ReturnToGodown")
					{
						ReturnQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						ReturnWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "ByProduct" || Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "FinishGoods")
					{
						OutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						OutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
				}
				InQty -= ReturnQty;
				InWeight -= ReturnWeight;
				txtInPutTotalQty.Text = InQty.ToString("#,#");
				txtInPutTotalWeight.Text = InWeight.ToString("#,#");
				txtOutQtyTotal.Text = OutQty.ToString("#,#");
				txtOutWeightTotal.Text = OutWeight.ToString("#,#");
				double BalQty = InQty - OutQty;
				double BalWeight = InWeight - OutWeight;
				txtBalQtyTotal.Text = BalQty.ToString("#,#");
				txtBalWeightTotal.Text = BalWeight.ToString("#,#");
			}
			else
			{
				txtInPutTotalQty.Text = "0";
				txtInPutTotalWeight.Text = "0";
				txtOutQtyTotal.Text = "0";
				txtOutWeightTotal.Text = "0";
				txtBalQtyTotal.Text = "0";
				txtBalWeightTotal.Text = "0";
			}
			DataTable dtWIP = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dtWIP.Rows.Count > 0)
			{
				if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(dtWIP.Rows[0]["FinishGoodRate"]);
				}
				else if (Conversion.ToDouble((object)txtAvgFinishGoodRate.Text.Trim()) == 0.0)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(dtWIP.Rows[0]["FinishGoodRate"]);
				}
				DDL.BindDDLNew(dtWIP, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dtWIP, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
			}
			else
			{
				txtAvgFinishGoodRate.Text = "0";
			}
			if (OutputItemsByJobOrderRateSchedule)
			{
				AllItemForOutPutAgainstJobOrder();
			}
			GetLastItemRateByJobOrder();
			if (dtReturnToGodown == null || dtReturnToGodown.Rows.Count == 0)
			{
				getProductionInputForReturnToGodown();
			}
			CmbEntryType_Leave(null, null);
			ExportContractScheduleBind(Conversion.ToInt(CmbJobOrderNoOutput.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEntryType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbEntryType.Value) == 3)
			{
				ItemBindForReturnToGodown();
			}
			else
			{
				ItemFillOutPut();
			}
			GetLastItemRateByJobOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getProductionInputForReturnToGodown()
	{
		try
		{
			dtReturnToGodown = InvFoodProduction.GetProductionInPutDataByJobAndPlantId(UserAccount.OrganizationId, UserAccount.CompanyId, 80, Conversion.ToInt(CmbJobOrderNoOutput.Value), Conversion.ToInt(CmbPlantFeederOutput.Value), (string)null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBindForReturnToGodown()
	{
		try
		{
			if (dtReturnToGodown == null || dtReturnToGodown.Rows.Count == 0)
			{
				((Control)(object)CmbItemOutput).Text = string.Empty;
				((UltraGridBase)CmbItemOutput).DataSource = null;
				((Control)(object)CmbLotOutPut).Text = string.Empty;
				((UltraGridBase)CmbLotOutPut).DataSource = null;
				((Control)(object)CmbCropYearOutPut).Text = string.Empty;
				((UltraGridBase)CmbCropYearOutPut).DataSource = null;
				return;
			}
			dtItemOutPut.Clear();
			foreach (DataRow r in dtReturnToGodown.Rows)
			{
				if (!dtItemOuPutReturnToGodownGlobal.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r["ItemId"])))
				{
					dtItemOutPut.Rows.Add(r["ItemId"], r["ItemName"]);
				}
			}
			ItemFillOutPut();
			CmbItemOutput_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherComboBindForReturnToGodownByItem()
	{
		try
		{
			dtJobOutPut.Clear();
			dtCropOutPut.Clear();
			if (Conversion.ToInt(CmbEntryType.Value) == 3)
			{
				if (((UltraGridBase)CmbItemOutput).ActiveRow == null || Conversion.ToInt(CmbItemOutput.Value) == 0)
				{
					((Control)(object)CmbLotOutPut).Text = string.Empty;
					((UltraGridBase)CmbLotOutPut).DataSource = null;
					((Control)(object)CmbCropYearOutPut).Text = string.Empty;
					((UltraGridBase)CmbCropYearOutPut).DataSource = null;
					return;
				}
				foreach (DataRow r in dtReturnToGodown.Rows)
				{
					if (!dtJobOutPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r["JobLotId"])))
					{
						dtJobOutPut.Rows.Add(r["JobLotId"], r["JobLotDescription"]);
					}
					if (!dtCropOutPut.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r["CropYearId"])))
					{
						dtCropOutPut.Rows.Add(r["CropYearId"], r["CropBatch"]);
					}
				}
				JobLotOutPut();
				CropYearOutPut();
			}
			else
			{
				JobLotOutPut();
				CropYearOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				CmbEntryType_Leave(null, null);
				CmbWarehouseOutput.Value = item.Cells["WareHouseId"].Value.ToString();
				((Control)(object)CmbWarehouseOutput).Text = item.Cells["WareHouse"].Text.ToString();
				CmbLotOutPut.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)CmbLotOutPut).Text = item.Cells["JobLot"].Text.ToString();
				CmbPackTypeOutPut.Value = item.Cells["PackingTypeId"].Value.ToString();
				((Control)(object)CmbPackTypeOutPut).Text = item.Cells["PackingType"].Text.ToString();
				CmbItemOutput.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbItemOutput).Text = item.Cells["Item"].Text.ToString();
				CmbItemOutput_Leave(null, null);
				CmbUomOutPut.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbUomOutPut).Text = item.Cells["ItemUOM"].Text.ToString();
				((Control)(object)CmbCropYearOutPut).Text = item.Cells["CropYear"].Text.ToString();
				txtQtyOutPut.Text = Conversion.ToDouble(item.Cells["Quantity"].Value).ToString("#,##0.##");
				txtGrossWeight.Text = Conversion.ToDouble(item.Cells["GrossWeight"].Value).ToString("#,##0.###");
				txtEbUnit.Text = Conversion.ToDouble(item.Cells["EbUnit"].Value).ToString("#,##0.####");
				txtEbTotal.Text = Conversion.ToDouble(item.Cells["EbTotal"].Value).ToString("#,##0.###");
				txtNetWeightOutPut.Text = Conversion.ToDouble(item.Cells["Weight"].Value).ToString("#,##0.###");
				txtRateOutPut.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				CmbRateUomOutPut.Value = item.Cells["RateUOMId"].Value.ToString();
				((Control)(object)CmbRateUomOutPut).Text = item.Cells["RateUOM"].Text.ToString();
				txtAmountOutPut.Text = Conversion.ToDouble(item.Cells["ItemAmount"].Value).ToString("#,##0.###");
				txtRemarksOutPut.Text = item.Cells["Remarks"].Text.ToString();
				btnAddOutPut.Visible = false;
				btnUpdateDetailOutPut.Visible = true;
				btnCancelOutPut.Visible = true;
				CmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOutPut())
			{
				tableByProduct.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)CmbEntryType).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["WareHouseId"] = CmbWarehouseOutput.Value;
				tableByProduct.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)CmbWarehouseOutput).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["JobLotId"] = CmbLotOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbLotOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackTypeOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackTypeOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ItemId"] = CmbItemOutput.Value;
				tableByProduct.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemOutput).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ItemUOMId"] = CmbUomOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["ItemUOM"] = ((Control)(object)CmbUomOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ItemEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells[2].Value);
				tableByProduct.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYearOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["Quantity"] = Conversion.ToDouble((object)txtQtyOutPut.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtEbUnit.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEbTotal.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRateOutPut.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells[2].Value);
				tableByProduct.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUomOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUomOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmountOutPut.Text.Trim());
				tableByProduct.Rows[updateDetailIndex]["Remarks"] = txtRemarksOutPut.Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ScheduleId"] = ScheduleId;
				btnUpdateDetailOutPut.Visible = false;
				btnCancelOutPut.Visible = false;
				btnAddOutPut.Visible = true;
				resetDetailOutPut();
				CommonServices.GridAutoAdjustmentNew(grdByProduct);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOutPut())
			{
				double RateUom = 0.0;
				if (((UltraGridBase)CmbRateUomOutPut).ActiveRow != null)
				{
					RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells[2].Value);
				}
				tableByProduct.Rows.Add(0, ((Control)(object)CmbEntryType).Text, CmbWarehouseOutput.Value, ((Control)(object)CmbWarehouseOutput).Text, CmbItemOutput.Value, ((Control)(object)CmbItemOutput).Text, CmbUomOutPut.Value, ((Control)(object)CmbUomOutPut).Text, Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells[2].Value), CmbCropYearOutPut.Value, ((Control)(object)CmbCropYearOutPut).Text, CmbLotOutPut.Value, ((Control)(object)CmbLotOutPut).Text, CmbPackTypeOutPut.Value, ((Control)(object)CmbPackTypeOutPut).Text, txtQtyOutPut.Text, Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtEbUnit.Text), Conversion.ToDouble((object)txtEbTotal.Text), Conversion.ToDouble((object)txtNetWeightOutPut.Text), Conversion.ToDouble((object)txtRateOutPut.Text.Trim()), Conversion.ToInt(CmbRateUomOutPut.Value), Conversion.ToString((object)((Control)(object)CmbRateUomOutPut).Text), RateUom, Conversion.ToDouble((object)txtAmountOutPut.Text), txtRemarksOutPut.Text, 0, "", 0, ScheduleId);
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				if (ScheduleId > 0)
				{
					((Control)(object)CmbJobOrderNoOutput).Enabled = false;
				}
				resetDetailOutPut();
				gridsettingsOutPut();
				CmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettingsOutPut()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdByProduct.RootTable.Columns["Rate"].Visible = false;
				grdByProduct.RootTable.Columns["RateUOM"].Visible = false;
				grdByProduct.RootTable.Columns["ItemAmount"].Visible = false;
				grdByProduct.RootTable.Columns["StockAc"].Visible = false;
			}
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["StockAcId"].Visible = false;
			grdByProduct.RootTable.Columns["StockAc"].Visible = false;
			grdByProduct.RootTable.Columns["CropYearId"].Visible = false;
			grdByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["JobLotId"].Visible = false;
			grdByProduct.RootTable.Columns["PackingTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["RateUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["RateEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["ScheduleId"].Visible = false;
			grdByProduct.RootTable.Columns["Weight"].Caption = "Net Weight";
			grdByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["IssueWeight"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["StockAcId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EbUnit"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EbTotal"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["StockAc"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ScheduleId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["IssueWeight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemUOMId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["WareHouseId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["CropYearId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["StockAcId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EntryType"].FilterEditType = (FilterEditType)3;
			grdByProduct.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["GrossWeight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EbUnit"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EbTotal"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["StockAc"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["EbUnit"].FormatString = "#,##0.####";
			grdByProduct.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.####";
			grdByProduct.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["EbTotal"].FormatString = "#,##0.####";
			grdByProduct.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.####";
			grdByProduct.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["EntryType"].ColumnType = (ColumnType)0;
			grdByProduct.RootTable.Columns["EntryType"].HasValueList = true;
			grdByProduct.RootTable.Columns["EntryType"].LimitToList = true;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].Caption = "EntryType";
			grdByProduct.RootTable.Columns["EntryType"].ValueList.PopulateValueList((IEnumerable)dtEntryType.DefaultView, "Id", "Name");
			grdByProduct.RootTable.Columns["EntryType"].Width = 150;
			CommonServices.GridAutoAdjustmentNew(grdByProduct);
			grdByProduct.RootTable.Columns.Add("Delete");
			grdByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdByProduct.UpdateData();
			GridEXRow item = grdByProduct.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemEquivalent"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
				double Weight2 = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double Rate2 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Amount2;
			}
			if (e.Column.Key == "Weight")
			{
				Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				Conversion.ToDouble((object)item.Cells["ItemEquivalent"].Value.ToString());
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount = Weight / RateUOM * Rate;
				item.Cells["ItemAmount"].Value = Amount;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellEdited(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
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
				if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
				{
					item.Delete();
				}
				else
				{
					OutputDetailRowsRemoveIds = OutputDetailRowsRemoveIds + "," + Conversion.ToString(item.Cells["Id"].Value);
					item.Delete();
				}
			}
			grdByProduct.UpdateData();
			if (tableByProduct.Rows.Count == 0)
			{
				((Control)(object)CmbJobOrderNoOutput).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateGrossWeight()
	{
		try
		{
			double Qty = ((Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) > 0.0) ? Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) : 0.0);
			double UOM = 0.0;
			double GrossWeight = 0.0;
			if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
			{
				if (((UltraGridBase)CmbUomOutPut).ActiveRow != null && Qty > 0.0)
				{
					UOM = Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells[2].Value);
				}
				txtGrossWeight.Text = (Qty * UOM).ToString("#,##0.###");
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
			double Qty = ((Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) > 0.0) ? Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) : 0.0);
			double num = Conversion.ToDouble((object)txtGrossWeight.Text);
			double EbUnit = Conversion.ToDouble((object)txtEbUnit.Text);
			double EbTotal = Conversion.ToDouble((object)txtEbTotal.Text);
			double NetWeight = 0.0;
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbUnit")
			{
				EbTotal = EbUnit * Qty;
				txtEbTotal.Text = EbTotal.ToString("#,##0.###");
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbTotal")
			{
				EbUnit = EbTotal / Qty;
				txtEbUnit.Text = EbUnit.ToString("#,##0.####");
			}
			else
			{
				EbUnit = ((Qty > 0.0) ? (EbTotal / Qty) : 0.0);
				txtEbUnit.Text = EbUnit.ToString("#,##0.####");
			}
			NetWeight = num - EbTotal;
			txtNetWeightOutPut.Text = NetWeight.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			if (((Control)(object)CmbUomOutPut).Text != string.Empty && txtQtyOutPut.Text != string.Empty && txtNetWeightOutPut.Text != string.Empty && txtRateOutPut.Text != string.Empty && ((Control)(object)CmbRateUomOutPut).Text != string.Empty && ((UltraGridBase)CmbRateUomOutPut).ActiveRow != null)
			{
				double Rate = 0.0;
				double RateUOM = 0.0;
				double Amount = 0.0;
				Rate = Conversion.ToDouble((object)txtRateOutPut.Text.Trim());
				double num = Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim());
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells[2].Value);
				Amount = num / RateUOM * Rate;
				txtAmountOutPut.Text = Amount.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyOutPut_TextChanged(object sender, EventArgs e)
	{
		CalculateGrossWeight();
	}

	private void txtRateOutPut_TextChanged(object sender, EventArgs e)
	{
		TotalAmount();
	}

	private void CmbRateUomOutPut_Leave(object sender, EventArgs e)
	{
		TotalAmount();
	}

	private void txtQtyOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtNetWeightOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtRateOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	public void ReadByIdOutPut(int ID)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Expected O, but got Unknown
		try
		{
			if (ID <= 0)
			{
				return;
			}
			RecIdOutPut = ID;
			btnSaveOutPut.Visible = false;
			btnUpdateOutPut.Visible = true;
			OutputDetailRowsRemoveIds = "";
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC == null)
			{
				return;
			}
			if (SC.IsApproved)
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString((object)SC.PlanStatus) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString((object)SC.PlanStatus) + " JobPlanStatus");
			}
			txtDocNoOutPut.Text = Conversion.ToString((object)SC.DocCode);
			DocDateOutPut.Value = Conversion.ToDateTime((object)SC.DocDate);
			txtRemarksHeaderOutPut.Text = Conversion.ToString((object)SC.MainRemarks);
			CmbJobOrderNoOutput.Value = SC.InvJobOrderId;
			CmbJobOrderNoOutput_Leave(null, null);
			CmbPlantFeederOutput.Value = SC.PlantId;
			CmbExportContractSchedule.Value = SC.ContractScheduleId;
			tableByProduct.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType != "Issue")
				{
					tabControl1.SelectedIndex = 1;
					tabControl3.SelectedIndex = 0;
					tableByProduct.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].ItemUomId, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].GrossWeight, SC.invFoodProductionDetails[i].EbUnit, SC.invFoodProductionDetails[i].EbTotal, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].NetRate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].RateEquivalent, SC.invFoodProductionDetails[i].TotalAmount, SC.invFoodProductionDetails[i].Remarks, SC.invFoodProductionDetails[i].StockAcId, SC.invFoodProductionDetails[i].AccountTitle, SC.invFoodProductionDetails[i].IssueWeight, SC.invFoodProductionDetails[i].JobOrderScheduleId);
					if (SC.invFoodProductionDetails[i].EntryType == "FinishGoods" && SC.invFoodProductionDetails[i].NetRate > 0.0)
					{
						txtAvgFinishGoodRate.Text = Conversion.ToString((object)SC.invFoodProductionDetails[i].NetRate);
					}
				}
			}
			if (SC.invFoodProductionDetails.Any((InvFoodProductionDetail row) => row.JobOrderScheduleId > 0 && !ByProductRateEditAbleOrNot))
			{
				((Control)(object)CmbJobOrderNoOutput).Enabled = false;
			}
			DataTable dtWIP = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dtWIP.Rows.Count > 0)
			{
				if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(dtWIP.Rows[0]["FinishGoodRate"]);
				}
				else if (Conversion.ToDouble((object)txtAvgFinishGoodRate.Text.Trim()) == 0.0)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(dtWIP.Rows[0]["FinishGoodRate"]);
				}
				DDL.BindDDLNew(dtWIP, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dtWIP, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
			}
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsOutPut();
			GetVoucherHeadId(RecIdOutPut, 112);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdOutPut = 0;
			OutPutInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			OutPutInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void OutPutInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		try
		{
			if (!FormValidationOutPut())
			{
				return;
			}
			InvFoodProduction invFoodProduction = new InvFoodProduction();
			if (RecIdOutPut > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.Id = RecIdOutPut;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invFoodProduction.DocCode = Conversion.ToInt((object)txtDocNoOutPut.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)DocDateOutPut.Value);
			invFoodProduction.MainRemarks = txtRemarksHeaderOutPut.Text.ToString();
			invFoodProduction.InvFoodProductionPlanId = 0;
			invFoodProduction.DocumentTypeId = 112;
			invFoodProduction.EntryType = "Output";
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoOutput).Text.Trim());
			if (Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
			{
				invFoodProduction.BaseDocumentTypeId = 2;
			}
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value);
			invFoodProduction.ContractScheduleId = Conversion.ToInt(CmbExportContractSchedule.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			invFoodProduction.InvFoodProductionConsumptionDetailslist = new List<InvFoodProductionConsumptionDetail>();
			if (grdByProduct.RowCount > 0)
			{
				double GrossWeight = 0.0;
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					if (Conversion.ToInt(r.Cells["Quantity"].Value) > 0 && Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
					{
						if (Conversion.ToString((object)r.Cells["EntryType"].Text.Trim()) == string.Empty)
						{
							MessageBox.Show("EntryType Field Require in Output Detail");
							return;
						}
						Conversiondetail.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
						Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						if (r.Cells["WareHouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
						{
							MessageBox.Show("WareHouse Field Require in Output Detail");
							return;
						}
						Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
						{
							MessageBox.Show("Item Field Require in Output Detail");
							return;
						}
						Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
						if (r.Cells["JobLotId"].Text == string.Empty || r.Cells["JobLotId"].Text == "0")
						{
							MessageBox.Show("JobLot Field Require in Output Detail");
							return;
						}
						Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Conversiondetail.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
						Conversiondetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
						Conversiondetail.EbUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
						Conversiondetail.EbTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
						Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
						GrossWeight += Conversiondetail.Weight;
						Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						if (formright.DoHaveCanRateandAmount && FIFOCGSFlag && Conversiondetail.Rate == 0.0)
						{
							throw new Exception("Rate not found");
						}
						Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						Conversiondetail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
						double RateEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
						if (formright.DoHaveCanRateandAmount && RateEquivalent == 0.0)
						{
							throw new Exception("Rate Equivalent not found");
						}
						double AmountReCalculate = Conversiondetail.Weight / RateEquivalent * Conversiondetail.Rate;
						if (formright.DoHaveCanRateandAmount && FIFOCGSFlag && AmountReCalculate == 0.0)
						{
							throw new Exception("Amount not Found in detail Grid");
						}
						Conversiondetail.Amount = AmountReCalculate;
						Conversiondetail.NetRate = Conversiondetail.Rate;
						Conversiondetail.TotalAmount = Conversiondetail.Amount;
						Conversiondetail.VoucherHeadId = 0;
						Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						Conversiondetail.StockAcId = Conversion.ToInt(r.Cells["StockAcId"].Value);
						Conversiondetail.JobOrderScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
						invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
					}
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("grid Record not found please check!");
				}
				if (OutputDetailRowsRemoveIds != null && OutputDetailRowsRemoveIds != string.Empty)
				{
					invFoodProduction.OutputDetailRowsRemoveIds = OutputDetailRowsRemoveIds;
				}
				int success = InvFoodProduction.Save(invFoodProduction);
				if (RecIdOutPut > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				if (WagesStatus && WagesActiveOrInActiveForOutPut)
				{
					CommonServices.WagesDeleteByReferenceIds(112, success);
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 112;
					frmwagesBillHeader.RefDocId = success;
					frmwagesBillHeader.GrossWeightTotal = GrossWeight;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				resetOutPut();
				resetDetailOutPut();
			}
			else
			{
				MessageBox.Show("Enter Detail First ...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void resetOutPut()
	{
		try
		{
			RecIdOutPut = 0;
			VoucherHeadId = 0;
			OutputDetailRowsRemoveIds = "";
			txtInPutTotalQty.Text = "0";
			txtInPutTotalWeight.Text = "0";
			txtOutQtyTotal.Text = "0";
			txtOutWeightTotal.Text = "0";
			txtBalQtyTotal.Text = "0";
			txtBalWeightTotal.Text = "0";
			btnSaveOutPut.Visible = true;
			btnUpdateOutPut.Visible = false;
			((Control)(object)CmbJobOrderNoOutput).Enabled = true;
			tableByProduct.Clear();
			grdByProduct.ClearStructure();
			GenerateDocNumberOutPut();
			txtAvgFinishGoodRate.Text = string.Empty;
			JobOrderNoFill();
			CmbJobOrderNoOutput_Leave(null, null);
			btnAddOutPut.Visible = true;
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetailOutPut()
	{
		try
		{
			ScheduleId = 0;
			AvgRate = 0.0;
			AvgRateWithoutExp = 0.0;
			txtRateOutPut.Text = string.Empty;
			txtAmountOutPut.Text = string.Empty;
			((Control)(object)CmbRateUomOutPut).Text = string.Empty;
			CmbItemOutput.Value = "";
			CmbUomOutPut.Value = "";
			txtQtyOutPut.Text = "";
			txtGrossWeight.Text = string.Empty;
			txtEbUnit.Text = string.Empty;
			txtEbTotal.Text = string.Empty;
			txtNetWeightOutPut.Text = string.Empty;
			txtRemarksOutPut.Text = string.Empty;
			txtRateOutPut.Text = string.Empty;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void BtnRefereshOutPutHistory_Click(object sender, EventArgs e)
	{
		try
		{
			jobOrderOutputHCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void jobOrderOutputHCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "112",
				Activity = "JobOrder"
			});
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("JobOrder");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, CmbJobOrderOutPuthistory, "Id", "JobOrder", "Job Order", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbJobOrderOutPuthistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderOutPuthistory).DataSource = null;
			}
			else
			{
				((Control)(object)CmbJobOrderOutPuthistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderOutPuthistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 1)
			{
				txtFromDateOutPuthistory.Focus();
			}
			else
			{
				DocDateOutPut.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOutputhistory()
	{
		try
		{
			txtDocNoFromOutPuthistory.Text = string.Empty;
			txtDocnotoOutPuthistory.Text = string.Empty;
			((Control)(object)CmbJobOrderOutPuthistory).Text = string.Empty;
			txtFromDateOutPuthistory.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtFromDateOutPuthistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowOutPuthistory_Click(object sender, EventArgs e)
	{
		try
		{
			OutputGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewOutPuthistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetOutputhistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutputGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		try
		{
			DataTable dthis = InvFoodProduction.ProductionFormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDateOutPuthistory.Value),
				ToDate = Conversion.ToDateTime((object)txttoDateOutPuthistory.Value),
				FromDocNo = Conversion.ToInt((object)txtDocNoFromOutPuthistory.Text),
				ToDocNo = Conversion.ToInt((object)txtDocnotoOutPuthistory.Text),
				JobOrderId = Conversion.ToInt(CmbJobOrderOutPuthistory.Value)
			});
			if (dthis.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtgrdoutput = new DataTable();
			dtgrdoutput.Columns.Add("Id");
			dtgrdoutput.Columns.Add("DocNo", typeof(int));
			dtgrdoutput.Columns.Add("WagesNo", typeof(int));
			dtgrdoutput.Columns.Add("DocDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("JobOrderNo");
			dtgrdoutput.Columns.Add("PlantName");
			dtgrdoutput.Columns.Add("EntryType");
			dtgrdoutput.Columns.Add("ContractSchedule");
			dtgrdoutput.Columns.Add("Remarks");
			dtgrdoutput.Columns.Add("IsApproved");
			dtgrdoutput.Columns.Add("StartDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("EndDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("EntryDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("EntryUser", typeof(string));
			dtgrdoutput.Columns.Add("ModifyDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("ModifyUser", typeof(string));
			dtgrdoutput.Columns.Add("PlanStatus", typeof(string));
			dtgrdoutput.Columns.Add("WagesId", typeof(int));
			dtgrdoutput.Clear();
			for (int i = 0; i < dthis.Rows.Count; i++)
			{
				if (Conversion.ToInt(dthis.Rows[i]["DocumentTypeId"]) == 112)
				{
					dtgrdoutput.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], dthis.Rows[i]["WagesNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["PlantName"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["ScheduleCode"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
				}
			}
			grdOutputHistory.DataSource = dtgrdoutput;
			grdOutputHistory.RetrieveStructure();
			GridOutputSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridOutputSetting()
	{
		try
		{
			grdOutputHistory.RootTable.Columns["Id"].Visible = false;
			grdOutputHistory.RootTable.Columns["WagesId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdOutputHistory);
			grdOutputHistory.RootTable.Columns.Add("Edit");
			grdOutputHistory.RootTable.Columns["Edit"].Key = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].Width = 40;
			grdOutputHistory.RootTable.Columns.Add("WagesPrint");
			grdOutputHistory.RootTable.Columns["WagesPrint"].Key = "WagesPrint";
			grdOutputHistory.RootTable.Columns["WagesPrint"].Caption = "WagesPrint";
			grdOutputHistory.RootTable.Columns["WagesPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistory.RootTable.Columns["WagesPrint"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistory.RootTable.Columns["WagesPrint"].ButtonText = "WagesPrint";
			grdOutputHistory.RootTable.Columns["WagesPrint"].Width = 60;
			grdOutputHistory.RootTable.Columns["WagesPrint"].Position = 0;
			grdOutputHistory.RootTable.Columns["Edit"].Position = 1;
			grdOutputHistory.FrozenColumns = 2;
			grdOutputHistory.RootTable.Columns["Remarks"].Width = 60;
			ctrlGrdBar11_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (Conversion.ToString(grdOutputHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString(grdOutputHistory.CurrentRow.Cells["PlanStatus"].Value) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString(grdOutputHistory.CurrentRow.Cells["PlanStatus"].Value) + " JobPlanStatus");
				}
				RecIdOutPut = Conversion.ToInt((object)grdOutputHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadByIdOutPut(RecIdOutPut);
			}
			if (e.Column.Key == "WagesPrint")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(grdOutputHistory.CurrentRow.Cells["WagesId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdOutputHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(grdOutputHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdOutputHistory.CurrentRow.Cells["PlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdOutputHistory.CurrentRow.Cells["PlanStatus"].Value) + " JobPlanStatus");
			}
			RecIdOutPut = Conversion.ToInt((object)grdOutputHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadByIdOutPut(RecIdOutPut);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoFromOutPuthistory_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void txtDocnotoOutPuthistory_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdByProduct).Name);
			if (fs != null)
			{
				grdByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar11_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutputHistory).Name);
			if (fs != null)
			{
				grdOutputHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProductionInPutAndOutPut601(RecIdOutPut);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPenForRateEntries_Click(object sender, EventArgs e)
	{
		try
		{
			LoadPendingforRates = new LoadOutPutPendingforRates(UserAccount);
			LoadPendingforRates.EntryType = "ByProduct";
			LoadPendingforRates.ShowDialog();
			LoadPendingforRates.BringToFront();
			if (LoadPendingforRates.OutPutDocId != 0)
			{
				ReadByIdOutPut(LoadPendingforRates.OutPutDocId);
			}
			UpdateUomForLoadingPendingForRateEntriesOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateUomForLoadingPendingForRateEntriesOutPut()
	{
		try
		{
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["RateUom"].Value) == 0)
				{
					item.BeginEdit();
					item.Cells["RateUom"].Value = 40;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplyRate_Click(object sender, EventArgs e)
	{
		try
		{
			ProporationFinishGoods();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProporationFinishGoods()
	{
		try
		{
			if (txtFGWeight.Text == string.Empty || txtFGWeight.Text == "0")
			{
				MessageBox.Show("Please Enter Finish Goods Weight");
				return;
			}
			grdByProduct.UpdateData();
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (r.Cells["EntryType"].Value.ToString() == "FinishGoods")
				{
					double num = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					double num2 = Conversion.ToDouble((object)AvgRate);
					double RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					double FgRate = num2 * RateUOM;
					r.Cells["Rate"].Value = FgRate;
					double Amount = num / RateUOM * Conversion.ToDouble((object)r.Cells["Rate"].Value.ToString());
					r.Cells["ItemAmount"].Value = Math.Round(Amount, 2).ToString();
					grdByProduct.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherReport_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFGWeight_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
				NetWeight = Conversion.ToDouble((object)txtFGWeight.Text.Trim())
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["TranType"].ToString() == "Output Finish Goods")
					{
						AvgRate = Conversion.ToDouble(dt.Rows[i]["AvgRate"]);
						txtFGRate.Text = Math.Round(Conversion.ToDouble(dt.Rows[i]["AvgRate"]), 3).ToString();
					}
				}
			}
			else
			{
				txtFGRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdinputhistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			InputDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdinputhistory.CurrentRow == null || (int)grdinputhistory.CurrentRow.RowType != 0 || grdinputhistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(Conversion.ToInt(grdinputhistory.CurrentRow.Cells["Id"].Value));
			DataTable dt = new DataTable();
			dt.Columns.Add("RefDocumentType");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("PartyName");
			dt.Columns.Add("WareHouseName");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackUom");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("RateUom");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks");
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType == "Issue")
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].RefDocumentType, Convert.ToDateTime(SC.invFoodProductionDetails[i].RefDocDate).ToString("dd-MMM-yy"), SC.invFoodProductionDetails[i].RefDocNo, SC.invFoodProductionDetails[i].PartyName, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdInputDetail.DataSource = dt;
				grdInputDetail.RetrieveStructure();
				if (!formright.DoHaveCanRateandAmount)
				{
					grdInputDetail.RootTable.Columns["Rate"].Visible = false;
					grdInputDetail.RootTable.Columns["RateUom"].Visible = false;
					grdInputDetail.RootTable.Columns["Amount"].Visible = false;
				}
				grdInputDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdInputDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdInputDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdInputDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Qty"].FormatString = "#,#";
				grdInputDetail.RootTable.Columns["Weight"].FormatString = "#,#";
				grdInputDetail.RootTable.Columns["Amount"].FormatString = "#,#";
				grdInputDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#";
				grdInputDetail.RootTable.Columns["Weight"].TotalFormatString = "#,#";
				grdInputDetail.RootTable.Columns["Amount"].TotalFormatString = "#,#";
				CommonServices.GridAutoAdjustmentNew(grdInputDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdOutputHistory.CurrentRow == null || (int)grdOutputHistory.CurrentRow.RowType != 0 || grdOutputHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(Conversion.ToInt(grdOutputHistory.CurrentRow.Cells["Id"].Value));
			DataTable dt = new DataTable();
			dt.Columns.Add("WareHouseName");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackUom");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("EbUnit", typeof(double));
			dt.Columns.Add("EbTotal", typeof(double));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("RateUom");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks");
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType != "Issue")
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].GrossWeight, SC.invFoodProductionDetails[i].EbUnit, SC.invFoodProductionDetails[i].EbTotal, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdOutPutDetail.DataSource = dt;
				grdOutPutDetail.RetrieveStructure();
				if (!formright.DoHaveCanRateandAmount)
				{
					grdOutPutDetail.RootTable.Columns["Rate"].Visible = false;
					grdOutPutDetail.RootTable.Columns["RateUom"].Visible = false;
					grdOutPutDetail.RootTable.Columns["Amount"].Visible = false;
				}
				grdOutPutDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["Qty"].FormatString = "#,##0.##";
				grdOutPutDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
				grdOutPutDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["EbUnit"].FormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["EbTotal"].FormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["Amount"].FormatString = "#,##0.###";
				grdOutPutDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
				CommonServices.GridAutoAdjustmentNew(grdOutPutDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			OutPutDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FoodProductionWithValues_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtQtyInput_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightInput_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateUomForLoadingPendingForRateEntriesInPut()
	{
		try
		{
			GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["RateUom"].Value) == 0)
				{
					item.BeginEdit();
					item.Cells["RateUom"].Value = 40;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForRateInput_Click(object sender, EventArgs e)
	{
		try
		{
			LoadPendingforRates = new LoadOutPutPendingforRates(UserAccount);
			LoadPendingforRates.EntryType = "Issue";
			LoadPendingforRates.ShowDialog();
			LoadPendingforRates.BringToFront();
			if (LoadPendingforRates.OutPutDocId > 0)
			{
				ReadByIdInput(LoadPendingforRates.OutPutDocId);
			}
			UpdateUomForLoadingPendingForRateEntriesInPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightOutPut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightOutPut_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyOutPut_Leave(object sender, EventArgs e)
	{
		try
		{
			Total();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemUomInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (Conversion.ToInt(cmbItemUomInput.Value) > 0)
			{
				double num = Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells[2].Value);
				double qty = Conversion.ToDouble((object)txtQtyInput.Text);
				txtNetWeightInput.Text = (num * qty).ToString("#,##0.###");
			}
			else
			{
				txtNetWeightInput.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPackingTypeInput_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblot();
			}
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

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
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

	private void txtEbUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
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

	private void txtEbTotal_TextChanged(object sender, EventArgs e)
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

	private void CmbPlantFeederOutput_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
				PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InQty = 0.0;
				double InWeight = 0.0;
				double ReturnQty = 0.0;
				double ReturnWeight = 0.0;
				double OutQty = 0.0;
				double OutWeight = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Input")
					{
						InQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						InWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "ReturnToGodown")
					{
						ReturnQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						ReturnWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "ByProduct" || Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "FinishGoods")
					{
						OutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						OutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
				}
				InQty -= ReturnQty;
				InWeight -= ReturnWeight;
				txtInPutTotalQty.Text = InQty.ToString("#,#");
				txtInPutTotalWeight.Text = InWeight.ToString("#,#");
				txtOutQtyTotal.Text = OutQty.ToString("#,#");
				txtOutWeightTotal.Text = OutWeight.ToString("#,#");
				double BalQty = InQty - OutQty;
				double BalWeight = InWeight - OutWeight;
				txtBalQtyTotal.Text = BalQty.ToString("#,#");
				txtBalWeightTotal.Text = BalWeight.ToString("#,#");
			}
			else
			{
				txtInPutTotalQty.Text = "0";
				txtInPutTotalWeight.Text = "0";
				txtOutQtyTotal.Text = "0";
				txtOutWeightTotal.Text = "0";
				txtBalQtyTotal.Text = "0";
				txtBalWeightTotal.Text = "0";
			}
			if (dtReturnToGodown == null || dtReturnToGodown.Rows.Count == 0)
			{
				getProductionInputForReturnToGodown();
			}
			CmbEntryType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocConsumption()
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
			int code = InvFoodProduction.GetSerialNumber(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocumentTypeId = 181,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoConsumption.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConsumptionDetailComboBind()
	{
		try
		{
			DataTable dtAll = InvFoodProduction.DropDownForConsumptionForm(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, UserAccount.BranchesId.ToString());
			if (dtAll.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtWare = new DataTable();
			dtWare.Columns.Add("Id", typeof(int));
			dtWare.Columns.Add("name");
			DataTable dtCrop = new DataTable();
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("name");
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id", typeof(int));
			dtJobLot.Columns.Add("name");
			DataTable dtPackType = new DataTable();
			dtPackType.Columns.Add("Id", typeof(int));
			dtPackType.Columns.Add("name");
			dtitemConsumption.Clear();
			for (int i = 0; i < dtAll.Rows.Count; i++)
			{
				if (Conversion.ToString(dtAll.Rows[i]["ActivityType"]) == "Warehouse")
				{
					dtWare.Rows.Add(dtAll.Rows[i]["Id"], dtAll.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtAll.Rows[i]["ActivityType"]) == "Items")
				{
					dtitemConsumption.Rows.Add(dtAll.Rows[i]["Id"], dtAll.Rows[i]["name"], dtAll.Rows[i]["Code"]);
				}
				if (Conversion.ToString(dtAll.Rows[i]["ActivityType"]) == "CropYear")
				{
					dtCrop.Rows.Add(dtAll.Rows[i]["Id"], dtAll.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtAll.Rows[i]["ActivityType"]) == "JobLot")
				{
					dtJobLot.Rows.Add(dtAll.Rows[i]["Id"], dtAll.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtAll.Rows[i]["ActivityType"]) == "PackingType")
				{
					dtPackType.Rows.Add(dtAll.Rows[i]["Id"], dtAll.Rows[i]["name"]);
				}
			}
			if (dtWare.Rows.Count > 0)
			{
				int Id5 = Conversion.ToInt(cmbWarehouseConsumption.Value);
				DDL.BindDDL(dtWare, cmbWarehouseConsumption, "Id", "name", "WareHouse", ZeroIndex: true);
				((UltraGridBase)cmbWarehouseConsumption).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id5 > 0)
				{
					cmbWarehouseConsumption.Value = Id5;
				}
			}
			if (dtitemConsumption.Rows.Count > 0)
			{
				int Id4 = Conversion.ToInt(CmbItemNameConsumption.Value);
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitemConsumption, CmbItemNameConsumption, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitemConsumption, CmbItemNameConsumption, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemNameConsumption).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id4 > 0)
				{
					CmbItemNameConsumption.Value = Id4;
				}
			}
			if (dtCrop.Rows.Count > 0)
			{
				int Id3 = Conversion.ToInt(CmbCropYearConsumption.Value);
				DDL.BindDDL(dtCrop, CmbCropYearConsumption, "Id", "name", "Crop Year", ZeroIndex: true);
				((UltraGridBase)CmbCropYearConsumption).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id3 > 0)
				{
					CmbCropYearConsumption.Value = Id3;
				}
			}
			if (dtJobLot.Rows.Count > 0)
			{
				int Id2 = Conversion.ToInt(CmbJobLotConsumption.Value);
				DDL.BindDDL(dtJobLot, CmbJobLotConsumption, "Id", "name", "JobLot", ZeroIndex: true);
				((UltraGridBase)CmbJobLotConsumption).DisplayLayout.Bands[0].Columns[1].Width = 275;
				if (Id2 > 0)
				{
					CmbJobLotConsumption.Value = Id2;
				}
			}
			if (dtPackType.Rows.Count > 0)
			{
				int Id = Conversion.ToInt(CmbPackingTypeConsumption.Value);
				DDL.BindDDL(dtPackType, CmbPackingTypeConsumption, "Id", "name", "Packing Type", ZeroIndex: true);
				((UltraGridBase)CmbPackingTypeConsumption).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					CmbPackingTypeConsumption.Value = Id;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUomForConsumtionItems()
	{
		try
		{
			if (dtuomlst.Rows.Count <= 0)
			{
				return;
			}
			string PackUom = ((Control)(object)cmbPackUomConsumption).Text;
			((Control)(object)cmbPackUomConsumption).Text = string.Empty;
			((Control)(object)CmbRateUomConsumption).Text = string.Empty;
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("PackUom");
			dtUom.Columns.Add("Equivalent");
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtuomlst.Rows[i]["ItemId"]) == Conversion.ToInt(CmbItemNameConsumption.Value))
				{
					dtUom.Rows.Add(dtuomlst.Rows[i]["Id"], dtuomlst.Rows[i]["UOMCode"], dtuomlst.Rows[i]["Equivalent"]);
				}
			}
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbPackUomConsumption, "Id", "PackUom", "Pack Uom", ZeroIndex: false);
				((UltraGridBase)cmbPackUomConsumption).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbPackUomConsumption).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtUom, CmbRateUomConsumption, "Id", "PackUom", "Rate Uom", ZeroIndex: false);
				((UltraGridBase)CmbRateUomConsumption).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbRateUomConsumption).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("PackUom") == PackUom))
				{
					((Control)(object)cmbPackUomConsumption).Text = PackUom;
				}
				else
				{
					((Control)(object)cmbPackUomConsumption).Text = string.Empty;
				}
				DataRow[] dr = dtUom.Select("Equivalent='" + Conversion.ToInt((object)40) + "'");
				if (dr.Length != 0)
				{
					CmbRateUomConsumption.Value = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				else
				{
					((Control)(object)CmbRateUomConsumption).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbJobOrderConsumption_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvProductionJobOrder Pro = new InvProductionJobOrder();
			Pro.OrganizationId = UserAccount.OrganizationId;
			Pro.CompanyId = UserAccount.CompanyId;
			Pro.Id = Conversion.ToInt(cmbJobOrderConsumption.Value);
			DataTable dt = InvProductionJobOrder.GetGlAccountsByJobOrderId(Pro);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbWIPAcConsumption, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dt, cmbWIPItemConsumption, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)cmbWIPAcConsumption).Rows[0].Activate();
				((UltraGridBase)cmbWIPItemConsumption).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)cmbWIPAcConsumption).DataSource = null;
				((UltraGridBase)cmbWIPItemConsumption).DataSource = null;
			}
			GetPlantFeeder(Pro.Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWarehouseConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForConsumption();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemNameConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			bindUomForConsumtionItems();
			GetAvailableStockForConsumption();
			GetAvgRateByItemAndJoblotForConsumption();
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
			if (dtitemConsumption.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemNameConsumption.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemNameConsumption.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitemConsumption, CmbItemNameConsumption, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitemConsumption, CmbItemNameConsumption, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemNameConsumption.Value = ItemId;
				CmbItemNameConsumption.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			ResetConsumptionMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 181);
			WagesActiveOrInActiveForConsumption = obj != null && obj.IsActive;
			ConsumptionDetailComboBind();
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetConsumptionMain()
	{
		try
		{
			RecIdConsumption = 0;
			VoucherHeadId = 0;
			txtRemarksHeaderConsumption.Text = string.Empty;
			btnSaveConsumption.Visible = true;
			BtnUpdateconsumption.Visible = false;
			UpdateMode = false;
			((Control)(object)cmbJobOrderConsumption).Enabled = true;
			tableConsumption.Clear();
			grdConsumption.ClearStructure();
			GenerateDocConsumption();
			((UltraGridBase)cmbJobOrderConsumption).DataSource = null;
			((Control)(object)cmbJobOrderConsumption).Text = string.Empty;
			JobOrderNoFill();
			((Control)(object)CmbPlantConsumption).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetConsumptionDetail()
	{
		try
		{
			((Control)(object)cmbWarehouseConsumption).Text = string.Empty;
			((Control)(object)CmbItemNameConsumption).Text = string.Empty;
			((Control)(object)cmbPackUomConsumption).Text = string.Empty;
			((UltraGridBase)cmbPackUomConsumption).DataSource = null;
			((Control)(object)CmbCropYearConsumption).Text = string.Empty;
			((Control)(object)CmbJobLotConsumption).Text = string.Empty;
			((Control)(object)CmbPackingTypeConsumption).Text = string.Empty;
			txtBalanceQtyConsumption.Text = string.Empty;
			txtBalanceWeightConsumption.Text = string.Empty;
			txtItemQtyConsumption.Text = string.Empty;
			txtNetWeightConsumption.Text = string.Empty;
			txtRemarksHeaderConsumption.Text = string.Empty;
			txtRateConsumption.Text = string.Empty;
			((Control)(object)CmbRateUomConsumption).Text = string.Empty;
			((UltraGridBase)CmbRateUomConsumption).DataSource = null;
			txtAmountConsumption.Text = string.Empty;
			BtnAddConsumption.Visible = true;
			BtnUpdateDetailConsumption.Visible = false;
			BtnCancelConsumption.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationConsumption()
	{
		if (txtDocNoConsumption.Text.Trim() == string.Empty || txtDocNoConsumption.Text == "0")
		{
			MessageBox.Show("document Number Field Required");
			txtDocNoConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobOrderConsumption).ActiveRow == null || Conversion.ToInt(cmbJobOrderConsumption.Value) == 0)
		{
			MessageBox.Show("Job Order Number Field Required");
			cmbJobOrderConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantConsumption).ActiveRow == null || Conversion.ToInt(CmbPlantConsumption.Value) == 0)
		{
			MessageBox.Show("Plant Name Field Required");
			CmbPlantConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWIPAcConsumption).ActiveRow == null || Conversion.ToInt(cmbWIPAcConsumption.Value) == 0)
		{
			MessageBox.Show("WorkInProcess Ac Field Required");
			cmbWIPAcConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWIPItemConsumption).ActiveRow == null || Conversion.ToInt(cmbWIPItemConsumption.Value) == 0)
		{
			MessageBox.Show("WIP Item Field Required");
			cmbWIPItemConsumption.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailConsumption()
	{
		if (((UltraGridBase)cmbWarehouseConsumption).ActiveRow == null || Conversion.ToInt(cmbWarehouseConsumption.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			cmbWarehouseConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemNameConsumption).ActiveRow == null || Conversion.ToInt(CmbItemNameConsumption.Value) == 0)
		{
			MessageBox.Show("ItemName Field Required");
			CmbItemNameConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUomConsumption).ActiveRow == null || Conversion.ToInt(cmbPackUomConsumption.Value) == 0)
		{
			MessageBox.Show("UOM Field Required");
			cmbPackUomConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYearConsumption).ActiveRow == null || Conversion.ToInt(CmbCropYearConsumption.Value) == 0)
		{
			MessageBox.Show("CropYear Field Required");
			CmbCropYearConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotConsumption).ActiveRow == null || Conversion.ToInt(CmbJobLotConsumption.Value) == 0)
		{
			MessageBox.Show("JobLot Field Required");
			CmbJobLotConsumption.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingTypeConsumption).ActiveRow == null || Conversion.ToInt(CmbPackingTypeConsumption.Value) == 0)
		{
			MessageBox.Show("Packing Type Field Required");
			CmbPackingTypeConsumption.Focus();
			return false;
		}
		if (txtItemQtyConsumption.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtItemQtyConsumption.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Qty Field Required");
			txtItemQtyConsumption.Focus();
			return false;
		}
		if (formright.DoHaveCanRateandAmount)
		{
			if (txtRateConsumption.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRateConsumption.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Rate Field Required");
				txtRateConsumption.Focus();
				return false;
			}
			if (((UltraGridBase)CmbRateUomConsumption).ActiveRow == null || Conversion.ToDouble(CmbRateUomConsumption.Value) == 0.0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUomConsumption.Focus();
				return false;
			}
			if (txtAmountConsumption.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountConsumption.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtAmountConsumption.Focus();
				return false;
			}
		}
		if (txtNetWeightConsumption.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetWeightConsumption.Text.Trim()) == 0.0)
		{
			MessageBox.Show("NetWeight Field Required");
			txtNetWeightConsumption.Focus();
			return false;
		}
		return true;
	}

	private void BtnAddConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOfDetailConsumption())
			{
				return;
			}
			if (tableConsumption.Rows.Count > 0)
			{
				for (int i = 0; i < tableConsumption.Rows.Count; i++)
				{
					if (Conversion.ToInt(tableConsumption.Rows[i]["RefDocumentTypeId"]) > 0)
					{
						throw new Exception("Record cannot be add in grid because record already add from loader");
					}
				}
			}
			tableConsumption.Rows.Add(0, 0, 0, 0, 0, 0, Conversion.ToInt(cmbWarehouseConsumption.Value), ((Control)(object)cmbWarehouseConsumption).Text, Conversion.ToInt(CmbItemNameConsumption.Value), ((Control)(object)CmbItemNameConsumption).Text, Conversion.ToString(((UltraDropDownBase)CmbItemNameConsumption).SelectedRow.Cells["ItemCode"].Value), Conversion.ToInt(cmbPackUomConsumption.Value), ((Control)(object)cmbPackUomConsumption).Text, Conversion.ToDouble(((UltraDropDownBase)cmbPackUomConsumption).SelectedRow.Cells[2].Value), ((Control)(object)CmbCropYearConsumption).Text, Conversion.ToInt(CmbJobLotConsumption.Value), ((Control)(object)CmbJobLotConsumption).Text, Conversion.ToInt(CmbPackingTypeConsumption.Value), ((Control)(object)CmbPackingTypeConsumption).Text, Conversion.ToDouble((object)txtBalanceQtyConsumption.Text), Conversion.ToDouble((object)txtBalanceWeightConsumption.Text), Conversion.ToDouble((object)txtItemQtyConsumption.Text), Conversion.ToDouble((object)txtNetWeightConsumption.Text), Conversion.ToDouble((object)txtRateConsumption.Text), Conversion.ToDouble((object)txtRateConsumption.Text), Conversion.ToInt(CmbRateUomConsumption.Value), Conversion.ToDouble((object)((Control)(object)CmbRateUomConsumption).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUomConsumption).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtAmountConsumption.Text), txtRemarksDetailConsumption.Text);
			grdConsumption.DataSource = tableConsumption;
			grdConsumption.RetrieveStructure();
			grdConsumptionsettings();
			ResetConsumptionDetail();
			cmbWarehouseConsumption.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumptionsettings()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdConsumption.RootTable.Columns["Rate"].Visible = false;
				grdConsumption.RootTable.Columns["RateUOM"].Visible = false;
				grdConsumption.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdConsumption.RootTable.Columns["Id"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["Item"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemEquivalent"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RateForCheck"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RateUOMForCheck"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["BalanceQty"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdConsumption.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["RefDocumentTypeId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["RefDocIdNo"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["RefDocSubIdNo"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["WarehouseId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["ItemUOMId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["BalanceQty"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["BalanceWeight"].FilterEditType = (FilterEditType)1;
			grdConsumption.RootTable.Columns["Id"].Visible = false;
			grdConsumption.RootTable.Columns["DocDate"].Visible = false;
			grdConsumption.RootTable.Columns["DocNo"].Visible = false;
			grdConsumption.RootTable.Columns["ItemId"].Visible = false;
			grdConsumption.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdConsumption.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdConsumption.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdConsumption.RootTable.Columns["JobLotId"].Visible = false;
			grdConsumption.RootTable.Columns["WarehouseId"].Visible = false;
			grdConsumption.RootTable.Columns["PackingTypeId"].Visible = false;
			grdConsumption.RootTable.Columns["ItemUOMId"].Visible = false;
			grdConsumption.RootTable.Columns["RateUOMId"].Visible = false;
			grdConsumption.RootTable.Columns["RateForCheck"].Visible = false;
			grdConsumption.RootTable.Columns["RateUOMForCheck"].Visible = false;
			grdConsumption.RootTable.Columns["ItemEquivalent"].Visible = false;
			grdConsumption.RootTable.Columns["WareHouse"].Width = 150;
			grdConsumption.RootTable.Columns["Item"].Width = 300;
			grdConsumption.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdConsumption.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdConsumption.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdConsumption.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdConsumption.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdConsumption.RootTable.Columns["BalanceQty"].FormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdConsumption.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdConsumption.RootTable.Columns["BalanceQty"].TotalFormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdConsumption.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdConsumption.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdConsumption.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdConsumption.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdConsumption.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdConsumption.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdConsumption);
			grdConsumption.RootTable.Columns.Add("Delete");
			grdConsumption.RootTable.Columns["Delete"].Caption = "X";
			grdConsumption.RootTable.Columns["Delete"].ButtonText = "X";
			grdConsumption.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdConsumption.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdConsumption.RootTable.Columns["Delete"].Width = 35;
			grdConsumption.RootTable.Columns["Delete"].Position = 0;
			grdConsumption.FrozenColumns = 1;
			ctrlGrdBar16_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumption_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdConsumption.CurrentRow;
			if (item != null)
			{
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					throw new Exception("The record was not updated as it was loaded directly from the Loader.");
				}
				updateDetailIndexConsumption = item.RowIndex;
				cmbWarehouseConsumption.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbItemNameConsumption.Value = item.Cells["ItemId"].Value.ToString();
				CmbItemNameConsumption_Leave(null, null);
				cmbPackUomConsumption.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbCropYearConsumption).Text = item.Cells["CropYear"].Text.ToString();
				CmbJobLotConsumption.Value = item.Cells["JobLotId"].Value.ToString();
				CmbPackingTypeConsumption.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtBalanceQtyConsumption.Text = item.Cells["BalanceQty"].Text.ToString();
				txtBalanceWeightConsumption.Text = item.Cells["BalanceWeight"].Text.ToString();
				txtItemQtyConsumption.Text = item.Cells["Quantity"].Text.ToString();
				txtNetWeightConsumption.Text = item.Cells["Weight"].Text.ToString();
				txtRateConsumption.Text = item.Cells["Rate"].Value.ToString();
				CmbRateUomConsumption.Value = item.Cells["RateUOMId"].Value.ToString();
				txtAmountConsumption.Text = item.Cells["ItemAmount"].Text.ToString();
				txtRemarksDetailConsumption.Text = item.Cells["Remarks"].Text.ToString();
				BtnAddConsumption.Visible = false;
				BtnUpdateDetailConsumption.Visible = true;
				BtnCancelConsumption.Visible = true;
				cmbWarehouseConsumption.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDetailConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailConsumption())
			{
				tableConsumption.Rows[updateDetailIndexConsumption]["WareHouseId"] = cmbWarehouseConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["WareHouse"] = ((Control)(object)cmbWarehouseConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["JobLotId"] = CmbJobLotConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["JobLot"] = ((Control)(object)CmbJobLotConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["PackingTypeId"] = CmbPackingTypeConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["PackingType"] = ((Control)(object)CmbPackingTypeConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemId"] = CmbItemNameConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["Item"] = ((Control)(object)CmbItemNameConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemNameConsumption).SelectedRow.Cells["ItemCode"].Value);
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemUOMId"] = cmbPackUomConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemUOM"] = ((Control)(object)cmbPackUomConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemEquivalent"] = ((Conversion.ToInt(cmbPackUomConsumption.Value) > 0) ? Conversion.ToDouble(((UltraDropDownBase)cmbPackUomConsumption).SelectedRow.Cells[2].Value) : 0.0);
				tableConsumption.Rows[updateDetailIndexConsumption]["CropYear"] = ((Control)(object)CmbCropYearConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["Quantity"] = txtItemQtyConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["BalanceQty"] = txtBalanceQtyConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["BalanceWeight"] = txtBalanceWeightConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["Weight"] = txtNetWeightConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["Rate"] = txtRateConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["RateForCheck"] = txtRateConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["RateUOM"] = ((Control)(object)CmbRateUomConsumption).Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["RateUOMForCheck"] = ((Conversion.ToInt(CmbRateUomConsumption.Value) > 0) ? Conversion.ToDouble(((UltraDropDownBase)CmbRateUomConsumption).SelectedRow.Cells[2].Value) : 0.0);
				tableConsumption.Rows[updateDetailIndexConsumption]["RateUOMId"] = CmbRateUomConsumption.Value;
				tableConsumption.Rows[updateDetailIndexConsumption]["ItemAmount"] = txtAmountConsumption.Text.Trim();
				tableConsumption.Rows[updateDetailIndexConsumption]["Remarks"] = txtRemarksDetailConsumption.Text.Trim();
				ResetConsumptionDetail();
				cmbWarehouseConsumption.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			BtnAddConsumption.Visible = true;
			BtnUpdateDetailConsumption.Visible = false;
			BtnCancelConsumption.Visible = false;
			ResetConsumptionDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumption_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grdConsumption.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToInt(item.Cells["Id"].Value) != 0)
				{
					MessageBox.Show("Can not Delete Saved Record...");
					return;
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				item.Delete();
			}
			grdConsumption.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumption_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		try
		{
			grdConsumption.UpdateData();
			GridEXRow item = grdConsumption.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble(item.Cells["ItemEquivalent"].Value);
				double Weight3 = 0.0;
				if (Conversion.ToDouble(item.Cells["BalanceQty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BalanceQty"].Value) < Qty && (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0))
				{
					item.Cells["Quantity"].Value = 0;
					item.Cells["Weight"].Value = 0;
					Qty = 0.0;
					MessageBox.Show("Qty cannot greater than BalanceQty Please check!");
				}
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					Weight3 = Conversion.ToDouble(item.Cells["BalanceWeight"].Value) / Conversion.ToDouble(item.Cells["BalanceQty"].Value) * Qty;
					if (Conversion.ToDouble(item.Cells["BalanceWeight"].Value) < Weight3)
					{
						Weight3 = 0.0;
						item.Cells["Weight"].Value = 0;
						MessageBox.Show("Weight cannot greater than Balance Weight Please check!");
					}
				}
				else
				{
					Weight3 = Qty * Uom;
				}
				item.Cells["Weight"].Value = Weight3;
				if (FIFOCGSFlag && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) <= 0)
				{
					double rate2 = CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
						WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value),
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						ItemQty = Conversion.ToDouble(item.Cells["Quantity"].Value),
						NetWeight = Conversion.ToDouble((object)item.Cells["Weight"].Text)
					});
					item.Cells["Rate"].Value = rate2 * 40.0;
				}
				double Rate3 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM3 = Conversion.ToDouble(item.Cells["RateUOMForCheck"].Value);
				double Amount3 = Weight3 / RateUOM3 * Rate3;
				item.Cells["ItemAmount"].Value = Amount3;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight2 = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				if (Conversion.ToDouble((object)item.Cells["BalanceWeight"].Value.ToString()) < Weight2 && (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0))
				{
					item.Cells["Weight"].Value = 0;
					Weight2 = 0.0;
					MessageBox.Show("Weight cannot greater than BalanceWeight Please check!");
				}
				if (FIFOCGSFlag && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) <= 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) <= 0)
				{
					double rate = CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
						WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value),
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						ItemQty = Conversion.ToDouble(item.Cells["Quantity"].Value),
						NetWeight = Conversion.ToDouble((object)item.Cells["Weight"].Text)
					});
					item.Cells["Rate"].Value = rate * 40.0;
				}
				double Rate2 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUOM2 = Conversion.ToDouble(item.Cells["RateUOMForCheck"].Value);
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Amount2;
			}
			if (e.Column.Key == "Rate" || e.Column.Key == "RateUOM")
			{
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0)
				{
					item.Cells["RateUOM"].Value = item.Cells["RateUOMForCheck"].Value;
					item.Cells["Rate"].Value = item.Cells["RateForCheck"].Value;
					throw new Exception("The item rate or rate unit of measure (RateUom) is not editable because this entry is loaded from the loader.");
				}
				double Amount = 0.0;
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				Amount = ((RateUOM != 0.0 && Rate != 0.0) ? (Weight / RateUOM * Rate) : 0.0);
				item.Cells["ItemAmount"].Value = Amount;
			}
			grdConsumption.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertConsumption()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		try
		{
			if (!FormValidationConsumption())
			{
				return;
			}
			InvFoodProduction invFoodProduction = new InvFoodProduction();
			if (RecIdConsumption > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.Id = RecIdConsumption;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invFoodProduction.DocumentTypeId = 181;
			invFoodProduction.DocCode = Conversion.ToInt((object)txtDocNoConsumption.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value);
			invFoodProduction.MainRemarks = txtRemarksHeaderConsumption.Text.ToString();
			invFoodProduction.EntryType = "Consumption";
			invFoodProduction.InvFoodProductionPlanId = Conversion.ToInt(CmbPlantConsumption.Value);
			invFoodProduction.WIPAccountId = Conversion.ToInt(cmbWIPAcConsumption.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(cmbWIPItemConsumption.Value);
			invFoodProduction.EBDepartmentId = Conversion.ToInt(CmbDepartmentConsumption.Value);
			invFoodProduction.InvJobOrderId = Conversion.ToInt(cmbJobOrderConsumption.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)cmbJobOrderConsumption).Text.Trim());
			if (Conversion.ToInt(((UltraDropDownBase)cmbJobOrderConsumption).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
			{
				invFoodProduction.BaseDocumentTypeId = 2;
			}
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantConsumption.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.InvFoodProductionConsumptionDetailslist = new List<InvFoodProductionConsumptionDetail>();
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			if (grdConsumption.RowCount > 0)
			{
				double GrossWeight = 0.0;
				GridEXRow[] rows = grdConsumption.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (!(Conversion.ToDouble((object)r.Cells["Quantity"].Text) > 0.0) || !(Conversion.ToDouble((object)r.Cells["Weight"].Text) > 0.0))
					{
						continue;
					}
					InvFoodProductionConsumptionDetail Conversiondetail = new InvFoodProductionConsumptionDetail();
					Conversiondetail.EntryType = Conversion.ToString((object)"Consumption");
					Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					Conversiondetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					Conversiondetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					if (r.Cells["WareHouseId"].Text == string.Empty || r.Cells["WareHouseId"].Text == "0")
					{
						MessageBox.Show("WareHouse Field Require in Consumption Detail");
						return;
					}
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || r.Cells["ItemId"].Text == "0")
					{
						MessageBox.Show("Item Field Require in Consumption Detail");
						return;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					if (r.Cells["CropYear"].Text == string.Empty || r.Cells["CropYear"].Text == "0")
					{
						MessageBox.Show("CropYear Field Require in Consumption Detail");
						return;
					}
					Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
					if (r.Cells["JobLotId"].Text == string.Empty)
					{
						MessageBox.Show("JobLot Field Require in Consumption Detail");
						return;
					}
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Conversiondetail.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					if (Conversiondetail.Qty == 0.0)
					{
						throw new Exception("ItemQty Field Required");
					}
					Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					GrossWeight += Conversiondetail.Weight;
					if (Conversiondetail.Weight == 0.0)
					{
						throw new Exception("ItemWeight Field Required");
					}
					Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					Conversiondetail.InvProductionJobOrderId = Conversion.ToInt((object)invFoodProduction.InvJobOrderId);
					if (!formright.DoHaveCanRateandAmount && Conversiondetail.RefDocumentTypeId == 0 && Conversiondetail.RefDocNoId == 0)
					{
						Conversiondetail.Rate = 0.0;
						Conversiondetail.Amount = 0.0;
					}
					else
					{
						Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						Conversiondetail.Amount = Conversion.ToInt(r.Cells["ItemAmount"].Value);
					}
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					if (Conversion.ToDouble(r.Cells["RateUOM"].Value) != 0.0)
					{
						DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
						if (dt.Rows.Count > 0)
						{
							DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r.Cells["RateUOM"].Value) + "'");
							if (dr.Length == 0)
							{
								MessageBox.Show("RateUOM On Row No " + (r.RowIndex + 1) + " is not define please check");
								return;
							}
							Conversiondetail.RateUOMId = Conversion.ToInt(dr[0].ItemArray[0]);
						}
					}
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					invFoodProduction.InvFoodProductionConsumptionDetailslist.Add(Conversiondetail);
				}
				if (invFoodProduction.InvFoodProductionConsumptionDetailslist.Count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}
				int success = InvFoodProduction.Save(invFoodProduction);
				if (RecIdConsumption > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				if (WagesStatus && WagesActiveOrInActiveForConsumption)
				{
					CommonServices.WagesDeleteByReferenceIds(181, success);
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 181;
					frmwagesBillHeader.RefDocId = success;
					frmwagesBillHeader.GrossWeightTotal = GrossWeight;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				ResetConsumptionMain();
			}
			else
			{
				MessageBox.Show("Enter Detail First ...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdConsumption(int ID)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			RecIdConsumption = ID;
			btnSaveConsumption.Visible = false;
			BtnUpdateconsumption.Visible = true;
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC != null && SC.EntryType == "Consumption")
			{
				if (SC.IsApproved)
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString((object)SC.PlanStatus) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString((object)SC.PlanStatus) + " JobPlanStatus");
				}
				txtDocNoConsumption.Text = Conversion.ToString((object)SC.DocCode);
				txtDocDateConsumption.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarksHeaderConsumption.Text = Conversion.ToString((object)SC.MainRemarks);
				CmbDepartmentConsumption.Value = SC.EBDepartmentId;
				cmbJobOrderConsumption.Value = SC.InvJobOrderId;
				cmbJobOrderConsumption_Leave(null, null);
				CmbPlantConsumption.Value = SC.PlantId;
				tableConsumption.Rows.Clear();
				for (int i = 0; i < SC.InvFoodProductionConsumptionDetailslist.Count; i++)
				{
					tabControl1.SelectedIndex = 2;
					tabControl6.SelectedIndex = 0;
					tableConsumption.Rows.Add(SC.InvFoodProductionConsumptionDetailslist[i].Id, SC.InvFoodProductionConsumptionDetailslist[i].RefDocumentTypeId, SC.InvFoodProductionConsumptionDetailslist[i].RefDocNoId, SC.InvFoodProductionConsumptionDetailslist[i].RefDocSubIdNo, Convert.ToDateTime(SC.InvFoodProductionConsumptionDetailslist[i].RefDocDate).ToString("dd-MMM-yy"), SC.InvFoodProductionConsumptionDetailslist[i].RefDocNo, SC.InvFoodProductionConsumptionDetailslist[i].WarehouseId, SC.InvFoodProductionConsumptionDetailslist[i].WareHouseName, SC.InvFoodProductionConsumptionDetailslist[i].ItemId, SC.InvFoodProductionConsumptionDetailslist[i].ItemName, SC.InvFoodProductionConsumptionDetailslist[i].ItemCode, SC.InvFoodProductionConsumptionDetailslist[i].ItemUomId, SC.InvFoodProductionConsumptionDetailslist[i].PackUom, SC.InvFoodProductionConsumptionDetailslist[i].Equivalent, SC.InvFoodProductionConsumptionDetailslist[i].CropBatch, SC.InvFoodProductionConsumptionDetailslist[i].JobLotId, SC.InvFoodProductionConsumptionDetailslist[i].JobLotDescription, SC.InvFoodProductionConsumptionDetailslist[i].PackingtypeId, SC.InvFoodProductionConsumptionDetailslist[i].PackTypeDesc, SC.InvFoodProductionConsumptionDetailslist[i].Qty, SC.InvFoodProductionConsumptionDetailslist[i].Weight, SC.InvFoodProductionConsumptionDetailslist[i].Qty, SC.InvFoodProductionConsumptionDetailslist[i].Weight, Math.Round(SC.InvFoodProductionConsumptionDetailslist[i].Rate, 2), Math.Round(SC.InvFoodProductionConsumptionDetailslist[i].Rate, 2), SC.InvFoodProductionConsumptionDetailslist[i].RateUOMId, SC.InvFoodProductionConsumptionDetailslist[i].RateUOM, SC.InvFoodProductionConsumptionDetailslist[i].RateEquivalent, SC.InvFoodProductionConsumptionDetailslist[i].Amount, SC.InvFoodProductionConsumptionDetailslist[i].Remarks);
				}
				grdConsumption.DataSource = tableConsumption;
				grdConsumption.RetrieveStructure();
				grdConsumptionsettings();
				GetVoucherHeadId(RecIdConsumption, 181);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdConsumption = 0;
			InsertConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateconsumption_Click(object sender, EventArgs e)
	{
		try
		{
			InsertConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintSlipConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			int Id = RecIdConsumption;
			GenerateSlipConsumption(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintVoucherConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(RecIdConsumption, 181));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlipConsumption(int PrintId)
	{
		try
		{
			CommonServices.ProductionConsumptionReport_623(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPendingForRatesFormConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			LoadConsumptionPendingforRate = new LoadConsumptionPendingforRates(UserAccount);
			LoadConsumptionPendingforRate.ShowDialog();
			LoadConsumptionPendingforRate.BringToFront();
			if (LoadConsumptionPendingforRate.OutPutDocId > 0)
			{
				ReadByIdConsumption(LoadConsumptionPendingforRate.OutPutDocId);
			}
			UpdateUomForLoadingPendingForRateEntriesConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadDataForConsumptionEntry_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTransactionOnConsumption = new LoadavailableTransactionsForIssuanceOnConsumption(UserAccount);
			LoadTransactionOnConsumption.ShowDialog();
			LoadTransactionOnConsumption.BringToFront();
			LoadDataDetailforConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailforConsumption()
	{
		try
		{
			DataTable dtConsump = LoadTransactionOnConsumption.dtIssuance;
			if (dtConsump.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtConsump.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grdConsumption.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtConsump.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtConsump.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtConsump.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						item.BeginEdit();
						double BalQty = Conversion.ToDouble(item.Cells["BalanceQty"].Value);
						double BalWeight = Conversion.ToDouble(item.Cells["BalanceWeight"].Value);
						item.Cells["BalanceWeight"].Value = Conversion.ToDouble((object)BalWeight) + Conversion.ToDouble(dtConsump.Rows[i]["Weight"]);
						item.Cells["BalanceQty"].Value = Conversion.ToDouble((object)BalQty) + Conversion.ToDouble(dtConsump.Rows[i]["ItemQty"]);
						item.EndEdit();
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					tableConsumption.Rows.Add(0, dtConsump.Rows[i]["RefDocumentTypeId"], dtConsump.Rows[i]["RefDocIdNo"], dtConsump.Rows[i]["RefDocSubIdNo"], Conversion.ToDateTime(dtConsump.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtConsump.Rows[i]["DocNo"], dtConsump.Rows[i]["WareHouseId"], dtConsump.Rows[i]["WareHouse"], dtConsump.Rows[i]["ItemId"], dtConsump.Rows[i]["ItemName"], dtConsump.Rows[i]["ItemCode"], dtConsump.Rows[i]["ItemUOMId"], dtConsump.Rows[i]["PackUom"], dtConsump.Rows[i]["ItemUOM"], dtConsump.Rows[i]["CropYear"], dtConsump.Rows[i]["JobLotId"], dtConsump.Rows[i]["JobLotCode"], dtConsump.Rows[i]["PackingTypeId"], dtConsump.Rows[i]["PackTypeCode"], dtConsump.Rows[i]["ItemQty"], dtConsump.Rows[i]["Weight"], dtConsump.Rows[i]["ItemQty"], dtConsump.Rows[i]["Weight"], dtConsump.Rows[i]["ItemRate"], dtConsump.Rows[i]["ItemRate"], dtConsump.Rows[i]["RateUOMId"], dtConsump.Rows[i]["RateUOM"], dtConsump.Rows[i]["Equivalent"], dtConsump.Rows[i]["ItemAmount"], "");
				}
			}
			grdConsumption.DataSource = tableConsumption;
			grdConsumption.RetrieveStructure();
			grdConsumptionsettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateUomForLoadingPendingForRateEntriesConsumption()
	{
		try
		{
			GridEXRow[] rows = grdConsumption.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["RateUom"].Value) == 0)
				{
					item.BeginEdit();
					item.Cells["RateUom"].Value = 40;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvailableStockForConsumption()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			DataTable dt = CommonServices.GetWeightCurrStockByItem(new ReportsParameters
			{
				WarehouseId = Conversion.ToInt(cmbWarehouseConsumption.Value),
				ItemId = Conversion.ToInt(CmbItemNameConsumption.Value),
				JobLotId = Conversion.ToInt(CmbJobLotConsumption.Value),
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropYearConsumption).Text.Trim()),
				ToDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtBalanceQtyConsumption.Text = Conversion.ToDouble(dt.Rows[0]["AvailableQty"]).ToString("#,##0.###");
				txtBalanceWeightConsumption.Text = Conversion.ToDouble(dt.Rows[0]["AvailableStock"]).ToString("#,##0.####");
			}
			else
			{
				txtBalanceQtyConsumption.Text = 0.ToString();
				txtBalanceWeightConsumption.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCalculationConsumption()
	{
		try
		{
			if (Conversion.ToDouble((object)txtNetWeightConsumption.Text) > 0.0 && ((Control)(object)CmbRateUomConsumption).Text != string.Empty && Conversion.ToDouble((object)txtRateConsumption.Text) > 0.0)
			{
				double num = Conversion.ToDouble((object)txtNetWeightConsumption.Text);
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomConsumption).SelectedRow.Cells[2].Value);
				double Rate = Conversion.ToDouble((object)txtRateConsumption.Text);
				double Amount = Conversion.ToDouble((object)(num / RateUom * Rate));
				if (Amount > 0.0)
				{
					txtAmountConsumption.Text = Amount.ToString();
				}
				else
				{
					txtAmountConsumption.Text = 0.ToString();
				}
			}
			else
			{
				txtAmountConsumption.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRateByItemAndJoblotForConsumption()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		try
		{
			if (formright.DoHaveCanRateandAmount)
			{
				double rate = 0.0;
				rate = ((!FIFOCGSFlag) ? CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(CmbItemNameConsumption.Value), Conversion.ToDateTime((object)txtDocDateConsumption.Value), 181, RecId, Conversion.ToInt(CmbJobLotConsumption.Value), Conversion.ToInt(CmbCropYearConsumption.Value), null, Conversion.ToInt(cmbWarehouseConsumption.Value)) : CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value),
					ItemId = Conversion.ToInt(CmbItemNameConsumption.Value),
					stockUOM = Conversion.ToInt(cmbPackUomConsumption.Value),
					WarehouseId = Conversion.ToInt(cmbWarehouseConsumption.Value),
					JobLotId = Conversion.ToInt(CmbJobLotConsumption.Value),
					PackingTypeId = Conversion.ToInt(CmbPackingTypeConsumption.Value),
					CropYear = Conversion.ToString((object)((Control)(object)CmbCropYearConsumption).Text),
					ItemQty = Conversion.ToDouble((object)txtItemQtyConsumption.Text),
					NetWeight = Conversion.ToDouble((object)txtNetWeightConsumption.Text)
				}));
				if (rate > 0.0)
				{
					rate *= 40.0;
					txtRateConsumption.Text = Conversion.ToString((object)rate);
				}
				else
				{
					txtRateConsumption.Text = "0";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCropYearConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForConsumption();
			if (FIFOCGSFlag)
			{
				GetAvailableStockForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobLotConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableStockForConsumption();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalWeightConsumption()
	{
		try
		{
			if (((UltraGridBase)cmbPackUomConsumption).ActiveRow != null && Conversion.ToInt(cmbPackUomConsumption.Value) > 0 && txtItemQtyConsumption.Text != string.Empty)
			{
				double UOM = 0.0;
				double NetWeight = 0.0;
				double num = Conversion.ToDouble((object)txtItemQtyConsumption.Text.Trim());
				UOM = Conversion.ToDouble(((UltraDropDownBase)cmbPackUomConsumption).SelectedRow.Cells[2].Value);
				txtNetWeightConsumption.Text = (num * UOM).ToString();
			}
			else
			{
				txtNetWeightConsumption.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingTypeConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPackUomConsumption_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalWeightConsumption();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQtyConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalWeightConsumption();
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQtyConsumption_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalWeightConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightConsumption_Leave(object sender, EventArgs e)
	{
		try
		{
			if (FIFOCGSFlag)
			{
				GetAvgRateByItemAndJoblotForConsumption();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightConsumption_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCalculationConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateConsumption_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCalculationConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUomConsumption_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCalculationConsumption();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowConsupmtionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GrdHistoryConsumptionMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshConsupmtionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			jobOrderConsumptionHCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistoryConsumption_Click(object sender, EventArgs e)
	{
		try
		{
			ResetConsumptionhistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void jobOrderConsumptionHCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "181",
				Activity = "JobOrder"
			});
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("JobOrder");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, CmbJobOrderNoConsupmtionHistory, "Id", "JobOrder", "Job Order", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbJobOrderNoConsupmtionHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoConsupmtionHistory).DataSource = null;
			}
			else
			{
				((Control)(object)CmbJobOrderNoConsupmtionHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoConsupmtionHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetConsumptionhistory()
	{
		try
		{
			txtDocNofromConsupmtionHistory.Text = string.Empty;
			txtDocNoToConsupmtionHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderNoConsupmtionHistory).Text = string.Empty;
			txtFromDateConsupmtionHistory.Value = DateTime.Now.AddDays(-3.0);
			txtFromDateConsupmtionHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl6_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 1)
			{
				txtFromDateConsupmtionHistory.Focus();
			}
			else
			{
				txtDocDateConsumption.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryConsumptionMain()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		try
		{
			DataTable dthis = InvFoodProduction.ProductionFormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDateConsupmtionHistory.Value),
				ToDate = Conversion.ToDateTime((object)txtToDateConsupmtionHistory.Value),
				FromDocNo = Conversion.ToInt((object)txtDocNofromConsupmtionHistory.Text),
				ToDocNo = Conversion.ToInt((object)txtDocNoToConsupmtionHistory.Text),
				JobOrderId = Conversion.ToInt(CmbJobOrderNoConsupmtionHistory.Value)
			});
			if (dthis.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtConsum = new DataTable();
			dtConsum.Columns.Add("Id");
			dtConsum.Columns.Add("DocSrNo", typeof(int));
			dtConsum.Columns.Add("DocDate", typeof(DateTime));
			dtConsum.Columns.Add("JobOrderNo");
			dtConsum.Columns.Add("PlantName");
			dtConsum.Columns.Add("EntryType");
			dtConsum.Columns.Add("Remarks");
			dtConsum.Columns.Add("IsApproved");
			dtConsum.Columns.Add("StartDate", typeof(DateTime));
			dtConsum.Columns.Add("EndDate", typeof(DateTime));
			dtConsum.Columns.Add("EntryDate", typeof(DateTime));
			dtConsum.Columns.Add("EntryUser", typeof(string));
			dtConsum.Columns.Add("ModifyDate", typeof(DateTime));
			dtConsum.Columns.Add("ModifyUser", typeof(string));
			dtConsum.Columns.Add("PlanStatus", typeof(string));
			dtConsum.Columns.Add("WagesId", typeof(int));
			dtConsum.Clear();
			for (int i = 0; i < dthis.Rows.Count; i++)
			{
				if (Conversion.ToInt(dthis.Rows[i]["DocumentTypeId"]) == 181)
				{
					dtConsum.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["PlantName"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
				}
			}
			grdHistoryConsumptionMai.DataSource = dtConsum;
			grdHistoryConsumptionMai.RetrieveStructure();
			HistoryConsumptionMainSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryConsumptionMainSetting()
	{
		try
		{
			grdHistoryConsumptionMai.RootTable.Columns["Id"].Visible = false;
			grdHistoryConsumptionMai.RootTable.Columns["WagesId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryConsumptionMai);
			if (formright.DoHaveUpdateRights)
			{
				grdHistoryConsumptionMai.RootTable.Columns.Add("Edit");
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].Key = "Edit";
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].Width = 40;
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].Position = 0;
				grdHistoryConsumptionMai.FrozenColumns = 1;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistoryConsumptionMai.RootTable.Columns.Add("WagesPrint");
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].Key = "WagesPrint";
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].Caption = "WagesPrint";
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].ButtonStyle = (ButtonStyle)5;
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].ButtonText = "WagesPrint";
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].Width = 60;
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].Position = 0;
				grdHistoryConsumptionMai.RootTable.Columns.Add("Print");
				grdHistoryConsumptionMai.RootTable.Columns["Print"].Key = "Print";
				grdHistoryConsumptionMai.RootTable.Columns["Print"].Caption = "Print";
				grdHistoryConsumptionMai.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryConsumptionMai.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistoryConsumptionMai.RootTable.Columns["Print"].ButtonText = "Print";
				grdHistoryConsumptionMai.RootTable.Columns["Print"].Width = 60;
				grdHistoryConsumptionMai.RootTable.Columns["Print"].Position = 1;
				grdHistoryConsumptionMai.FrozenColumns = 2;
			}
			if (formright.DoHavePrintRights && formright.DoHavePrintRights)
			{
				grdHistoryConsumptionMai.RootTable.Columns["Edit"].Position = 0;
				grdHistoryConsumptionMai.RootTable.Columns["Print"].Position = 1;
				grdHistoryConsumptionMai.RootTable.Columns["WagesPrint"].Position = 2;
				grdHistoryConsumptionMai.FrozenColumns = 3;
			}
			ctrlGrdBar17_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryConsumptionMai_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GrdHistoryConsumptionDetailHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryConsumptionDetailHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdHistoryConsumptionMai.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
			if (SC == null)
			{
				return;
			}
			DataTable dtConsumption = new DataTable();
			dtConsumption.Columns.Add("Id", typeof(int));
			dtConsumption.Columns.Add("RefDocumentTypeId", typeof(int));
			dtConsumption.Columns.Add("RefDocIdNo", typeof(int));
			dtConsumption.Columns.Add("RefDocSubIdNo", typeof(int));
			dtConsumption.Columns.Add("DocDate");
			dtConsumption.Columns.Add("DocNo", typeof(int));
			dtConsumption.Columns.Add("WareHouseId", typeof(int));
			dtConsumption.Columns.Add("WareHouse");
			dtConsumption.Columns.Add("ItemId", typeof(int));
			dtConsumption.Columns.Add("Item");
			dtConsumption.Columns.Add("ItemCode");
			dtConsumption.Columns.Add("ItemUOMId", typeof(int));
			dtConsumption.Columns.Add("ItemUOM");
			dtConsumption.Columns.Add("ItemEquivalent", typeof(double));
			dtConsumption.Columns.Add("CropYear");
			dtConsumption.Columns.Add("JobLotId", typeof(int));
			dtConsumption.Columns.Add("JobLot");
			dtConsumption.Columns.Add("PackingTypeId", typeof(int));
			dtConsumption.Columns.Add("PackingType");
			dtConsumption.Columns.Add("BalanceQty", typeof(double));
			dtConsumption.Columns.Add("BalanceWeight", typeof(double));
			dtConsumption.Columns.Add("Quantity", typeof(double));
			dtConsumption.Columns.Add("Weight", typeof(double));
			dtConsumption.Columns.Add("Rate", typeof(double));
			dtConsumption.Columns.Add("RateForCheck", typeof(double));
			dtConsumption.Columns.Add("RateUOMId", typeof(int));
			dtConsumption.Columns.Add("RateUOM");
			dtConsumption.Columns.Add("RateUOMForCheck");
			dtConsumption.Columns.Add("ItemAmount", typeof(decimal));
			dtConsumption.Columns.Add("Remarks");
			dtConsumption.Rows.Clear();
			for (int i = 0; i < SC.InvFoodProductionConsumptionDetailslist.Count; i++)
			{
				if (SC.InvFoodProductionConsumptionDetailslist[i].EntryType == "Consumption")
				{
					dtConsumption.Rows.Add(SC.InvFoodProductionConsumptionDetailslist[i].Id, SC.InvFoodProductionConsumptionDetailslist[i].RefDocumentTypeId, SC.InvFoodProductionConsumptionDetailslist[i].RefDocNoId, SC.InvFoodProductionConsumptionDetailslist[i].RefDocSubIdNo, Convert.ToDateTime(SC.InvFoodProductionConsumptionDetailslist[i].RefDocDate).ToString("dd-MMM-yy"), SC.InvFoodProductionConsumptionDetailslist[i].RefDocNo, SC.InvFoodProductionConsumptionDetailslist[i].WarehouseId, SC.InvFoodProductionConsumptionDetailslist[i].WareHouseName, SC.InvFoodProductionConsumptionDetailslist[i].ItemId, SC.InvFoodProductionConsumptionDetailslist[i].ItemName, SC.InvFoodProductionConsumptionDetailslist[i].ItemCode, SC.InvFoodProductionConsumptionDetailslist[i].ItemUomId, SC.InvFoodProductionConsumptionDetailslist[i].PackUom, SC.InvFoodProductionConsumptionDetailslist[i].Equivalent, SC.InvFoodProductionConsumptionDetailslist[i].CropBatch, SC.InvFoodProductionConsumptionDetailslist[i].JobLotId, SC.InvFoodProductionConsumptionDetailslist[i].JobLotDescription, SC.InvFoodProductionConsumptionDetailslist[i].PackingtypeId, SC.InvFoodProductionConsumptionDetailslist[i].PackTypeDesc, SC.InvFoodProductionConsumptionDetailslist[i].Qty, SC.InvFoodProductionConsumptionDetailslist[i].Weight, SC.InvFoodProductionConsumptionDetailslist[i].Qty, SC.InvFoodProductionConsumptionDetailslist[i].Weight, Math.Round(SC.InvFoodProductionConsumptionDetailslist[i].Rate, 2), Math.Round(SC.InvFoodProductionConsumptionDetailslist[i].Rate, 2), SC.InvFoodProductionConsumptionDetailslist[i].RateUOMId, SC.InvFoodProductionConsumptionDetailslist[i].RateUOM, SC.InvFoodProductionConsumptionDetailslist[i].RateEquivalent, SC.InvFoodProductionConsumptionDetailslist[i].Amount, SC.InvFoodProductionConsumptionDetailslist[i].Remarks);
				}
				grdHistoryConsumptionDetail.DataSource = dtConsumption;
				grdHistoryConsumptionDetail.RetrieveStructure();
				grdHistoryConsumptionDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryConsumptionDetailSettings()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdHistoryConsumptionDetail.RootTable.Columns["Rate"].Visible = false;
				grdHistoryConsumptionDetail.RootTable.Columns["RateUOM"].Visible = false;
				grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdHistoryConsumptionDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemEquivalent"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RateForCheck"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOMForCheck"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdHistoryConsumptionDetail.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOM"].EditType = (EditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocumentTypeId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocIdNo"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocSubIdNo"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["WarehouseId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemUOMId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].FilterEditType = (FilterEditType)1;
			grdHistoryConsumptionDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["DocDate"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["DocNo"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RateForCheck"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["RateUOMForCheck"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemEquivalent"].Visible = false;
			grdHistoryConsumptionDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdHistoryConsumptionDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].FormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].TotalFormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdHistoryConsumptionDetail.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryConsumptionDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdHistoryConsumptionDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdHistoryConsumptionDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdHistoryConsumptionDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryConsumptionMai_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["IsApproved"].Value) == "Approved")
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["PlanStatus"].Value) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["PlanStatus"].Value) + " JobPlanStatus");
				}
				RecIdConsumption = Conversion.ToInt((object)grdHistoryConsumptionMai.CurrentRow.Cells["Id"].Value.ToString());
				ReadByIdConsumption(RecIdConsumption);
			}
			if (e.Column.Key == "WagesPrint")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(grdHistoryConsumptionMai.CurrentRow.Cells["WagesId"].Value));
			}
			if (e.Column.Key == "Print")
			{
				GenerateSlipConsumption(Conversion.ToInt(grdHistoryConsumptionMai.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryConsumptionMai_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["PlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdHistoryConsumptionMai.CurrentRow.Cells["PlanStatus"].Value) + " JobPlanStatus");
			}
			RecIdConsumption = Conversion.ToInt((object)grdHistoryConsumptionMai.CurrentRow.Cells["Id"].Value.ToString());
			ReadByIdConsumption(RecIdConsumption);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar16_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdConsumption).Name);
			if (fs != null)
			{
				grdConsumption.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar17_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryConsumptionMai).Name);
			if (fs != null)
			{
				grdHistoryConsumptionMai.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CmbCropYearOutPut_Leave(object sender, EventArgs e)
	{
		try
		{
			GetLastItemRateByJobOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDailyPlantConsumedHours_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDailyPlantConsumedHours(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SettlementForm_OnPackingMaterialReadById(object sender, frmProductionSettlement.IdEventArgs e)
	{
		try
		{
			tabControl1.SelectedIndex = 3;
			PackingMaterialForm.tabControlPm.SelectedIndex = 0;
			PackingMaterialForm.ReadByIdPackingMaterial(e.Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SettlementForm_OnOverHeadReadById(object sender, frmProductionSettlement.IdEventArgs e)
	{
		try
		{
			tabControl1.SelectedIndex = 4;
			OverHeadForm.tabControl1.SelectedIndex = 0;
			OverHeadForm.ReadByIdOverHeadJobOrderWise(e.Id);
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
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Expected O, but got Unknown
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Expected O, but got Unknown
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Expected O, but got Unknown
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Expected O, but got Unknown
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Expected O, but got Unknown
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Expected O, but got Unknown
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Expected O, but got Unknown
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Expected O, but got Unknown
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Expected O, but got Unknown
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Expected O, but got Unknown
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Expected O, but got Unknown
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Expected O, but got Unknown
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Expected O, but got Unknown
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Expected O, but got Unknown
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Expected O, but got Unknown
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Expected O, but got Unknown
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Expected O, but got Unknown
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Expected O, but got Unknown
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Expected O, but got Unknown
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcd: Expected O, but got Unknown
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Expected O, but got Unknown
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Expected O, but got Unknown
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Expected O, but got Unknown
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Expected O, but got Unknown
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Expected O, but got Unknown
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1a: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c25: Expected O, but got Unknown
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Expected O, but got Unknown
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Expected O, but got Unknown
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Expected O, but got Unknown
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Expected O, but got Unknown
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Expected O, but got Unknown
		//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c72: Expected O, but got Unknown
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Expected O, but got Unknown
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Expected O, but got Unknown
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Expected O, but got Unknown
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Expected O, but got Unknown
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Expected O, but got Unknown
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbf: Expected O, but got Unknown
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Expected O, but got Unknown
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Expected O, but got Unknown
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Expected O, but got Unknown
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ceb: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Expected O, but got Unknown
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Expected O, but got Unknown
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Expected O, but got Unknown
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d38: Expected O, but got Unknown
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Expected O, but got Unknown
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Expected O, but got Unknown
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Expected O, but got Unknown
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Expected O, but got Unknown
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Expected O, but got Unknown
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Expected O, but got Unknown
		//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Expected O, but got Unknown
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Expected O, but got Unknown
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Expected O, but got Unknown
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db1: Expected O, but got Unknown
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc7: Expected O, but got Unknown
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Expected O, but got Unknown
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddd: Expected O, but got Unknown
		//IL_0ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Expected O, but got Unknown
		//IL_0dea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Expected O, but got Unknown
		//IL_0df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Expected O, but got Unknown
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Expected O, but got Unknown
		//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Expected O, but got Unknown
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Expected O, but got Unknown
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Expected O, but got Unknown
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Expected O, but got Unknown
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Expected O, but got Unknown
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Expected O, but got Unknown
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Expected O, but got Unknown
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Expected O, but got Unknown
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Expected O, but got Unknown
		//IL_0e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Expected O, but got Unknown
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e98: Expected O, but got Unknown
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Expected O, but got Unknown
		//IL_0ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Expected O, but got Unknown
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Expected O, but got Unknown
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Expected O, but got Unknown
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecf: Expected O, but got Unknown
		//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eda: Expected O, but got Unknown
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee5: Expected O, but got Unknown
		//IL_0ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Expected O, but got Unknown
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Expected O, but got Unknown
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Expected O, but got Unknown
		//IL_0f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f11: Expected O, but got Unknown
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Expected O, but got Unknown
		//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f27: Expected O, but got Unknown
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Expected O, but got Unknown
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Expected O, but got Unknown
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Expected O, but got Unknown
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Expected O, but got Unknown
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Expected O, but got Unknown
		//IL_0f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f95: Expected O, but got Unknown
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Expected O, but got Unknown
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Expected O, but got Unknown
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Expected O, but got Unknown
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc1: Expected O, but got Unknown
		//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcc: Expected O, but got Unknown
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Expected O, but got Unknown
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Expected O, but got Unknown
		//IL_0fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fed: Expected O, but got Unknown
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Expected O, but got Unknown
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1003: Expected O, but got Unknown
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_100e: Expected O, but got Unknown
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Expected O, but got Unknown
		//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e3: Expected O, but got Unknown
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Expected O, but got Unknown
		//IL_150d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Expected O, but got Unknown
		//IL_185c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1866: Expected O, but got Unknown
		//IL_1964: Unknown result type (might be due to invalid IL or missing references)
		//IL_196e: Expected O, but got Unknown
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_198f: Expected O, but got Unknown
		//IL_1c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c18: Expected O, but got Unknown
		//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c39: Expected O, but got Unknown
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Expected O, but got Unknown
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4f: Expected O, but got Unknown
		//IL_1c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5a: Expected O, but got Unknown
		//IL_1c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c65: Expected O, but got Unknown
		//IL_1c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c70: Expected O, but got Unknown
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7b: Expected O, but got Unknown
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c86: Expected O, but got Unknown
		//IL_1c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c91: Expected O, but got Unknown
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9c: Expected O, but got Unknown
		//IL_1c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca7: Expected O, but got Unknown
		//IL_1ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb2: Expected O, but got Unknown
		//IL_1cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbd: Expected O, but got Unknown
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc8: Expected O, but got Unknown
		//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd3: Expected O, but got Unknown
		//IL_1cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cde: Expected O, but got Unknown
		//IL_1cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce9: Expected O, but got Unknown
		//IL_1cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf4: Expected O, but got Unknown
		//IL_1cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cff: Expected O, but got Unknown
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0a: Expected O, but got Unknown
		//IL_1d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d15: Expected O, but got Unknown
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d20: Expected O, but got Unknown
		//IL_1d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2b: Expected O, but got Unknown
		//IL_1d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d36: Expected O, but got Unknown
		//IL_1d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d41: Expected O, but got Unknown
		//IL_1d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4c: Expected O, but got Unknown
		//IL_1d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d57: Expected O, but got Unknown
		//IL_1d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d62: Expected O, but got Unknown
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d78: Expected O, but got Unknown
		//IL_1d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d83: Expected O, but got Unknown
		//IL_1d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8e: Expected O, but got Unknown
		//IL_1d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d99: Expected O, but got Unknown
		//IL_1d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Expected O, but got Unknown
		//IL_1da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1daf: Expected O, but got Unknown
		//IL_1db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dba: Expected O, but got Unknown
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc5: Expected O, but got Unknown
		//IL_1dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd0: Expected O, but got Unknown
		//IL_3332: Unknown result type (might be due to invalid IL or missing references)
		//IL_333c: Expected O, but got Unknown
		//IL_5ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ae4: Expected O, but got Unknown
		//IL_5af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5afb: Expected O, but got Unknown
		//IL_6ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ced: Expected O, but got Unknown
		//IL_9dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_9dd6: Expected O, but got Unknown
		//IL_9de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_9ded: Expected O, but got Unknown
		//IL_9dfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e04: Expected O, but got Unknown
		//IL_b02e: Unknown result type (might be due to invalid IL or missing references)
		//IL_b038: Expected O, but got Unknown
		//IL_d575: Unknown result type (might be due to invalid IL or missing references)
		//IL_d57f: Expected O, but got Unknown
		//IL_d58c: Unknown result type (might be due to invalid IL or missing references)
		//IL_d596: Expected O, but got Unknown
		GridEXLayout grdHistoryConsumptionMai_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryConsumptionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdConsumption_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdinputhistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionConsumption));
		GridEXLayout grdStockConversionProductionDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance455 = new Appearance();
		Appearance appearance456 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance454 = new Appearance();
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
		Appearance appearance441 = new Appearance();
		Appearance appearance442 = new Appearance();
		Appearance appearance443 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance445 = new Appearance();
		Appearance appearance447 = new Appearance();
		Appearance appearance448 = new Appearance();
		Appearance appearance449 = new Appearance();
		Appearance appearance450 = new Appearance();
		Appearance appearance451 = new Appearance();
		Appearance appearance452 = new Appearance();
		Appearance appearance453 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance446 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.PanelOtherForms = new System.Windows.Forms.Panel();
		this.Consumption = new System.Windows.Forms.TabPage();
		this.tabControl6 = new System.Windows.Forms.TabControl();
		this.tabPage10 = new System.Windows.Forms.TabPage();
		this.panel61 = new System.Windows.Forms.Panel();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistoryConsumption = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshConsupmtionHistory = new System.Windows.Forms.ToolStripButton();
		this.label143 = new System.Windows.Forms.Label();
		this.panel62 = new System.Windows.Forms.Panel();
		this.groupBox13 = new System.Windows.Forms.GroupBox();
		this.txtDocNofromConsupmtionHistory = new System.Windows.Forms.TextBox();
		this.label162 = new System.Windows.Forms.Label();
		this.label161 = new System.Windows.Forms.Label();
		this.txtFromDateConsupmtionHistory = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoToConsupmtionHistory = new System.Windows.Forms.TextBox();
		this.label160 = new System.Windows.Forms.Label();
		this.label159 = new System.Windows.Forms.Label();
		this.txtToDateConsupmtionHistory = new System.Windows.Forms.DateTimePicker();
		this.label158 = new System.Windows.Forms.Label();
		this.BtnShowConsupmtionHistory = new System.Windows.Forms.Button();
		this.panel83 = new System.Windows.Forms.Panel();
		this.label157 = new System.Windows.Forms.Label();
		this.ctrlGrdBar17 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistoryConsumptionMai = new GridEX();
		this.panel82 = new System.Windows.Forms.Panel();
		this.label144 = new System.Windows.Forms.Label();
		this.grdHistoryConsumptionDetail = new GridEX();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.toolStripConsumption = new System.Windows.Forms.ToolStrip();
		this.BtnNewConsumption = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshConsumption = new System.Windows.Forms.ToolStripButton();
		this.btnSaveConsumption = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateconsumption = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrintSlipConsumption = new System.Windows.Forms.ToolStripButton();
		this.btnPrintVoucherConsumption = new System.Windows.Forms.ToolStripButton();
		this.BtnPendingForRatesFormConsumption = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadDataForConsumptionEntry = new System.Windows.Forms.ToolStripButton();
		this.panel48 = new System.Windows.Forms.Panel();
		this.panel80 = new System.Windows.Forms.Panel();
		this.label142 = new System.Windows.Forms.Label();
		this.panel79 = new System.Windows.Forms.Panel();
		this.groupBox10 = new System.Windows.Forms.GroupBox();
		this.txtRemarksHeaderConsumption = new System.Windows.Forms.TextBox();
		this.label141 = new System.Windows.Forms.Label();
		this.label140 = new System.Windows.Forms.Label();
		this.label139 = new System.Windows.Forms.Label();
		this.label138 = new System.Windows.Forms.Label();
		this.label137 = new System.Windows.Forms.Label();
		this.txtDocDateConsumption = new System.Windows.Forms.DateTimePicker();
		this.label136 = new System.Windows.Forms.Label();
		this.txtDocNoConsumption = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.panelManualEntryConsumption = new System.Windows.Forms.Panel();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.label134 = new System.Windows.Forms.Label();
		this.label133 = new System.Windows.Forms.Label();
		this.label132 = new System.Windows.Forms.Label();
		this.BtnUpdateDetailConsumption = new System.Windows.Forms.Button();
		this.label131 = new System.Windows.Forms.Label();
		this.BtnCancelConsumption = new System.Windows.Forms.Button();
		this.txtBalanceQtyConsumption = new System.Windows.Forms.TextBox();
		this.BtnAddConsumption = new System.Windows.Forms.Button();
		this.lblRateConsumption = new System.Windows.Forms.Label();
		this.label129 = new System.Windows.Forms.Label();
		this.txtRateConsumption = new System.Windows.Forms.TextBox();
		this.label128 = new System.Windows.Forms.Label();
		this.lblRateUomConsumption = new System.Windows.Forms.Label();
		this.label126 = new System.Windows.Forms.Label();
		this.label125 = new System.Windows.Forms.Label();
		this.lblAmountConsumption = new System.Windows.Forms.Label();
		this.txtAmountConsumption = new System.Windows.Forms.TextBox();
		this.txtRemarksDetailConsumption = new System.Windows.Forms.TextBox();
		this.label123 = new System.Windows.Forms.Label();
		this.txtNetWeightConsumption = new System.Windows.Forms.TextBox();
		this.txtBalanceWeightConsumption = new System.Windows.Forms.TextBox();
		this.txtItemQtyConsumption = new System.Windows.Forms.TextBox();
		this.label122 = new System.Windows.Forms.Label();
		this.label121 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.panel74 = new System.Windows.Forms.Panel();
		this.panel77 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar16 = new grid.User_Conrtols.CtrlGrdBar();
		this.label120 = new System.Windows.Forms.Label();
		this.panel76 = new System.Windows.Forms.Panel();
		this.grdConsumption = new GridEX();
		this.OutPut = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel60 = new System.Windows.Forms.Panel();
		this.toolStrip8 = new System.Windows.Forms.ToolStrip();
		this.BtnNewOutPuthistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefereshOutPutHistory = new System.Windows.Forms.ToolStripButton();
		this.grdOutPutDetail = new GridEX();
		this.panel81 = new System.Windows.Forms.Panel();
		this.groupBox12 = new System.Windows.Forms.GroupBox();
		this.txtDocNoFromOutPuthistory = new System.Windows.Forms.TextBox();
		this.label154 = new System.Windows.Forms.Label();
		this.label153 = new System.Windows.Forms.Label();
		this.txtFromDateOutPuthistory = new System.Windows.Forms.DateTimePicker();
		this.txtDocnotoOutPuthistory = new System.Windows.Forms.TextBox();
		this.label152 = new System.Windows.Forms.Label();
		this.label151 = new System.Windows.Forms.Label();
		this.txttoDateOutPuthistory = new System.Windows.Forms.DateTimePicker();
		this.label150 = new System.Windows.Forms.Label();
		this.BtnShowOutPuthistory = new System.Windows.Forms.Button();
		this.panel84 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar11 = new grid.User_Conrtols.CtrlGrdBar();
		this.label155 = new System.Windows.Forms.Label();
		this.panel85 = new System.Windows.Forms.Panel();
		this.label156 = new System.Windows.Forms.Label();
		this.grdOutputHistory = new GridEX();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.btnNewOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOutPut = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnPenForRateEntries = new System.Windows.Forms.ToolStripButton();
		this.btnDailyPlantConsumedHours = new System.Windows.Forms.ToolStripButton();
		this.panel30 = new System.Windows.Forms.Panel();
		this.label75 = new System.Windows.Forms.Label();
		this.txtFGWeight = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.txtFGRate = new System.Windows.Forms.TextBox();
		this.btnApplyRate = new System.Windows.Forms.Button();
		this.txtAvgFinishGoodRate = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtOutWeightTotal = new System.Windows.Forms.TextBox();
		this.txtOutQtyTotal = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtInPutTotalWeight = new System.Windows.Forms.TextBox();
		this.txtInPutTotalQty = new System.Windows.Forms.TextBox();
		this.txtRemarksHeaderOutPut = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtBalQtyTotal = new System.Windows.Forms.TextBox();
		this.txtBalWeightTotal = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.DocDateOutPut = new System.Windows.Forms.DateTimePicker();
		this.label63 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label170 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.txtDocNoOutPut = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtNetWeightOutPut = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.btnUpdateDetailOutPut = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.btnCancelOutPut = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.btnAddOutPut = new System.Windows.Forms.Button();
		this.txtQtyOutPut = new System.Windows.Forms.TextBox();
		this.lblRateOutPUt = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.txtRateOutPut = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblRateUOMOUtPut = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.lblAmountOUtPUt = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtAmountOutPut = new System.Windows.Forms.TextBox();
		this.txtRemarksOutPut = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label167 = new System.Windows.Forms.Label();
		this.label166 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label165 = new System.Windows.Forms.Label();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.panel64 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.InPut = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel57 = new System.Windows.Forms.Panel();
		this.toolStrip7 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshInputHistory = new System.Windows.Forms.ToolStripButton();
		this.label124 = new System.Windows.Forms.Label();
		this.panel58 = new System.Windows.Forms.Panel();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.label149 = new System.Windows.Forms.Label();
		this.label148 = new System.Windows.Forms.Label();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label147 = new System.Windows.Forms.Label();
		this.label146 = new System.Windows.Forms.Label();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label145 = new System.Windows.Forms.Label();
		this.BtnShowInputHistory = new System.Windows.Forms.Button();
		this.panel59 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar10 = new grid.User_Conrtols.CtrlGrdBar();
		this.label127 = new System.Windows.Forms.Label();
		this.grdInputDetail = new GridEX();
		this.panel78 = new System.Windows.Forms.Panel();
		this.label130 = new System.Windows.Forms.Label();
		this.grdinputhistory = new GridEX();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.btnPendingForRateInput = new System.Windows.Forms.ToolStripButton();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel63 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.panel72 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.btnLoadInvoices = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label112 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label83 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.PanelInPutManualEntry = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label67 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.btnUpdateDetailInput = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.btnCancelInput = new System.Windows.Forms.Button();
		this.txtBalanceQty = new System.Windows.Forms.TextBox();
		this.btnAddInput = new System.Windows.Forms.Button();
		this.lblRateInput = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.txtRateInput = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.lblRateUom = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.lblAmountInput = new System.Windows.Forms.Label();
		this.txtAmountInput = new System.Windows.Forms.TextBox();
		this.txtRemarksInput = new System.Windows.Forms.TextBox();
		this.label81 = new System.Windows.Forms.Label();
		this.txtNetWeightInput = new System.Windows.Forms.TextBox();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.txtQtyInput = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdStockConversionProductionDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.toolStripConsumptionHistory = new System.Windows.Forms.ToolStrip();
		this.CmbJobOrderNoConsupmtionHistory = new UltraCombo();
		this.cmbJobOrderConsumption = new UltraCombo();
		this.cmbWIPItemConsumption = new UltraCombo();
		this.CmbDepartmentConsumption = new UltraCombo();
		this.cmbWIPAcConsumption = new UltraCombo();
		this.CmbPlantConsumption = new UltraCombo();
		this.CmbPackingTypeConsumption = new UltraCombo();
		this.CmbRateUomConsumption = new UltraCombo();
		this.CmbCropYearConsumption = new UltraCombo();
		this.CmbItemNameConsumption = new UltraCombo();
		this.CmbJobLotConsumption = new UltraCombo();
		this.cmbPackUomConsumption = new UltraCombo();
		this.cmbWarehouseConsumption = new UltraCombo();
		this.CmbJobOrderOutPuthistory = new UltraCombo();
		this.CmbJobOrderNoOutput = new UltraCombo();
		this.CmbPlantFeederOutput = new UltraCombo();
		this.CmbExportContractSchedule = new UltraCombo();
		this.CmbEntryType = new UltraCombo();
		this.CmbCropYearOutPut = new UltraCombo();
		this.CmbItemOutput = new UltraCombo();
		this.CmbPackTypeOutPut = new UltraCombo();
		this.CmbRateUomOutPut = new UltraCombo();
		this.CmbWarehouseOutput = new UltraCombo();
		this.CmbUomOutPut = new UltraCombo();
		this.CmbLotOutPut = new UltraCombo();
		this.CmbJobOrderInputHistory = new UltraCombo();
		this.CmbJobOrderNo = new UltraCombo();
		this.CmbWIPItem = new UltraCombo();
		this.CmbProductionDepartment = new UltraCombo();
		this.CmbWorkInProcess = new UltraCombo();
		this.CmbPlantFeeder = new UltraCombo();
		this.cmbPackingTypeInput = new UltraCombo();
		this.cmbRateuomInput = new UltraCombo();
		this.cmbCropYearInput = new UltraCombo();
		this.cmbItemInput = new UltraCombo();
		this.cmbLotInput = new UltraCombo();
		this.cmbItemUomInput = new UltraCombo();
		this.cmbWareHouseInput = new UltraCombo();
		this.panel1.SuspendLayout();
		this.Consumption.SuspendLayout();
		this.tabControl6.SuspendLayout();
		this.tabPage10.SuspendLayout();
		this.panel61.SuspendLayout();
		this.panel62.SuspendLayout();
		this.groupBox13.SuspendLayout();
		this.panel83.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionMai).BeginInit();
		this.panel82.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionDetail).BeginInit();
		this.tabPage9.SuspendLayout();
		this.toolStripConsumption.SuspendLayout();
		this.panel48.SuspendLayout();
		this.panel80.SuspendLayout();
		this.panel79.SuspendLayout();
		this.groupBox10.SuspendLayout();
		this.panelManualEntryConsumption.SuspendLayout();
		this.groupBox9.SuspendLayout();
		this.panel74.SuspendLayout();
		this.panel77.SuspendLayout();
		this.panel76.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdConsumption).BeginInit();
		this.OutPut.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage6.SuspendLayout();
		this.panel60.SuspendLayout();
		this.toolStrip8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).BeginInit();
		this.panel81.SuspendLayout();
		this.groupBox12.SuspendLayout();
		this.panel84.SuspendLayout();
		this.panel85.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).BeginInit();
		this.tabPage5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.panel30.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel64.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.InPut.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel57.SuspendLayout();
		this.toolStrip7.SuspendLayout();
		this.panel58.SuspendLayout();
		this.groupBox11.SuspendLayout();
		this.panel59.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).BeginInit();
		this.panel78.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).BeginInit();
		this.tabPage1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel63.SuspendLayout();
		this.panel72.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.PanelInPutManualEntry.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.toolStripConsumptionHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoConsupmtionHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPItemConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPAcConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouseConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderOutPuthistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExportContractSchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeeder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateuomInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.PanelOtherForms);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1370, 687);
		this.panel1.TabIndex = 0;
		this.PanelOtherForms.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PanelOtherForms.Location = new System.Drawing.Point(0, 639);
		this.PanelOtherForms.Name = "PanelOtherForms";
		this.PanelOtherForms.Size = new System.Drawing.Size(1370, 48);
		this.PanelOtherForms.TabIndex = 173;
		this.PanelOtherForms.Visible = false;
		this.Consumption.Controls.Add(this.tabControl6);
		this.Consumption.Location = new System.Drawing.Point(4, 26);
		this.Consumption.Name = "Consumption";
		this.Consumption.Size = new System.Drawing.Size(1362, 609);
		this.Consumption.TabIndex = 7;
		this.Consumption.Text = "Consumption";
		this.Consumption.UseVisualStyleBackColor = true;
		this.tabControl6.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl6.Controls.Add(this.tabPage9);
		this.tabControl6.Controls.Add(this.tabPage10);
		this.tabControl6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl6.Location = new System.Drawing.Point(0, 0);
		this.tabControl6.Name = "tabControl6";
		this.tabControl6.SelectedIndex = 0;
		this.tabControl6.Size = new System.Drawing.Size(1362, 609);
		this.tabControl6.TabIndex = 5;
		this.tabControl6.SelectedIndexChanged += new System.EventHandler(tabControl6_SelectedIndexChanged);
		this.tabPage10.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail);
		this.tabPage10.Controls.Add(this.panel82);
		this.tabPage10.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai);
		this.tabPage10.Controls.Add(this.panel83);
		this.tabPage10.Controls.Add(this.panel62);
		this.tabPage10.Controls.Add(this.panel61);
		this.tabPage10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.tabPage10.Location = new System.Drawing.Point(4, 4);
		this.tabPage10.Name = "tabPage10";
		this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage10.Size = new System.Drawing.Size(1354, 580);
		this.tabPage10.TabIndex = 1;
		this.tabPage10.Text = "History";
		this.tabPage10.UseVisualStyleBackColor = true;
		this.panel61.BackColor = System.Drawing.Color.Teal;
		this.panel61.Controls.Add(this.label143);
		this.panel61.Controls.Add(this.toolStripConsumptionHistory);
		this.panel61.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel61.Location = new System.Drawing.Point(3, 3);
		this.panel61.Name = "panel61";
		this.panel61.Size = new System.Drawing.Size(1348, 52);
		this.panel61.TabIndex = 178;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.BackColor = System.Drawing.Color.White;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(148, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1348, 27);
		this.miniToolStrip.TabIndex = 4;
		this.BtnNewHistoryConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistoryConsumption.ForeColor = System.Drawing.Color.Black;
		this.BtnNewHistoryConsumption.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistoryConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistoryConsumption.Name = "BtnNewHistoryConsumption";
		this.BtnNewHistoryConsumption.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistoryConsumption.Text = "&New";
		this.BtnNewHistoryConsumption.Click += new System.EventHandler(BtnNewHistoryConsumption_Click);
		this.btnRefreshConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshConsupmtionHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshConsupmtionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshConsupmtionHistory.Name = "btnRefreshConsupmtionHistory";
		this.btnRefreshConsupmtionHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshConsupmtionHistory.Text = "Refresh";
		this.btnRefreshConsupmtionHistory.Click += new System.EventHandler(btnRefreshConsupmtionHistory_Click);
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label143.ForeColor = System.Drawing.Color.White;
		this.label143.Location = new System.Drawing.Point(3, 30);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(198, 19);
		this.label143.TabIndex = 211;
		this.label143.Text = "Consumption History    ";
		this.panel62.Controls.Add(this.groupBox13);
		this.panel62.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel62.Location = new System.Drawing.Point(3, 55);
		this.panel62.Name = "panel62";
		this.panel62.Size = new System.Drawing.Size(1348, 48);
		this.panel62.TabIndex = 179;
		this.groupBox13.Controls.Add(this.BtnShowConsupmtionHistory);
		this.groupBox13.Controls.Add(this.label158);
		this.groupBox13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory);
		this.groupBox13.Controls.Add(this.txtToDateConsupmtionHistory);
		this.groupBox13.Controls.Add(this.label159);
		this.groupBox13.Controls.Add(this.label160);
		this.groupBox13.Controls.Add(this.txtDocNoToConsupmtionHistory);
		this.groupBox13.Controls.Add(this.txtFromDateConsupmtionHistory);
		this.groupBox13.Controls.Add(this.label161);
		this.groupBox13.Controls.Add(this.label162);
		this.groupBox13.Controls.Add(this.txtDocNofromConsupmtionHistory);
		this.groupBox13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox13.Location = new System.Drawing.Point(4, 1);
		this.groupBox13.Name = "groupBox13";
		this.groupBox13.Size = new System.Drawing.Size(1038, 46);
		this.groupBox13.TabIndex = 1;
		this.groupBox13.TabStop = false;
		this.groupBox13.Text = "Filters";
		this.txtDocNofromConsupmtionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNofromConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNofromConsupmtionHistory.Location = new System.Drawing.Point(490, 16);
		this.txtDocNofromConsupmtionHistory.Name = "txtDocNofromConsupmtionHistory";
		this.txtDocNofromConsupmtionHistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNofromConsupmtionHistory.TabIndex = 2;
		this.label162.AutoSize = true;
		this.label162.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label162.Location = new System.Drawing.Point(429, 20);
		this.label162.Name = "label162";
		this.label162.Size = new System.Drawing.Size(60, 15);
		this.label162.TabIndex = 173;
		this.label162.Text = "Doc From";
		this.label161.AutoSize = true;
		this.label161.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label161.Location = new System.Drawing.Point(564, 20);
		this.label161.Name = "label161";
		this.label161.Size = new System.Drawing.Size(45, 15);
		this.label161.TabIndex = 2;
		this.label161.Text = "Doc To";
		this.txtFromDateConsupmtionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateConsupmtionHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateConsupmtionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateConsupmtionHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromDateConsupmtionHistory.Name = "txtFromDateConsupmtionHistory";
		this.txtFromDateConsupmtionHistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromDateConsupmtionHistory.TabIndex = 0;
		this.txtDocNoToConsupmtionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoToConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoToConsupmtionHistory.Location = new System.Drawing.Point(609, 16);
		this.txtDocNoToConsupmtionHistory.Name = "txtDocNoToConsupmtionHistory";
		this.txtDocNoToConsupmtionHistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNoToConsupmtionHistory.TabIndex = 3;
		this.label160.AutoSize = true;
		this.label160.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label160.Location = new System.Drawing.Point(2, 20);
		this.label160.Name = "label160";
		this.label160.Size = new System.Drawing.Size(63, 15);
		this.label160.TabIndex = 171;
		this.label160.Text = "Date From";
		this.label159.AutoSize = true;
		this.label159.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label159.Location = new System.Drawing.Point(217, 20);
		this.label159.Name = "label159";
		this.label159.Size = new System.Drawing.Size(48, 15);
		this.label159.TabIndex = 171;
		this.label159.Text = "Date To";
		this.txtToDateConsupmtionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateConsupmtionHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateConsupmtionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateConsupmtionHistory.Location = new System.Drawing.Point(268, 16);
		this.txtToDateConsupmtionHistory.Name = "txtToDateConsupmtionHistory";
		this.txtToDateConsupmtionHistory.Size = new System.Drawing.Size(148, 23);
		this.txtToDateConsupmtionHistory.TabIndex = 1;
		this.label158.AutoSize = true;
		this.label158.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label158.Location = new System.Drawing.Point(683, 20);
		this.label158.Name = "label158";
		this.label158.Size = new System.Drawing.Size(59, 15);
		this.label158.TabIndex = 3;
		this.label158.Text = "Job Order";
		this.BtnShowConsupmtionHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowConsupmtionHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowConsupmtionHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.BtnShowConsupmtionHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowConsupmtionHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowConsupmtionHistory.Location = new System.Drawing.Point(969, 15);
		this.BtnShowConsupmtionHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowConsupmtionHistory.Name = "BtnShowConsupmtionHistory";
		this.BtnShowConsupmtionHistory.Size = new System.Drawing.Size(61, 25);
		this.BtnShowConsupmtionHistory.TabIndex = 5;
		this.BtnShowConsupmtionHistory.Text = "Show";
		this.BtnShowConsupmtionHistory.UseVisualStyleBackColor = false;
		this.BtnShowConsupmtionHistory.Click += new System.EventHandler(BtnShowConsupmtionHistory_Click);
		this.panel83.BackColor = System.Drawing.Color.Teal;
		this.panel83.Controls.Add(this.ctrlGrdBar17);
		this.panel83.Controls.Add(this.label157);
		this.panel83.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel83.Location = new System.Drawing.Point(3, 103);
		this.panel83.Name = "panel83";
		this.panel83.Size = new System.Drawing.Size(1348, 23);
		this.panel83.TabIndex = 180;
		this.label157.AutoSize = true;
		this.label157.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label157.ForeColor = System.Drawing.Color.White;
		this.label157.Location = new System.Drawing.Point(3, 3);
		this.label157.Name = "label157";
		this.label157.Size = new System.Drawing.Size(40, 17);
		this.label157.TabIndex = 212;
		this.label157.Text = "Main";
		this.ctrlGrdBar17.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar17.FormName = this;
		this.ctrlGrdBar17.Location = new System.Drawing.Point(1308, 0);
		this.ctrlGrdBar17.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar17.MyGrid = null;
		this.ctrlGrdBar17.Name = "ctrlGrdBar17";
		this.ctrlGrdBar17.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar17.TabIndex = 174;
		this.ctrlGrdBar17.Load += new System.EventHandler(ctrlGrdBar17_Load);
		this.grdHistoryConsumptionMai.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryConsumptionMai.AlternatingColors = true;
		((ScrollableBase)this.grdHistoryConsumptionMai).BackColor = System.Drawing.Color.White;
		this.grdHistoryConsumptionMai.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryConsumptionMai_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryConsumptionMai.DesignTimeLayout = grdHistoryConsumptionMai_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryConsumptionMai.FilterMode = (FilterMode)2;
		this.grdHistoryConsumptionMai.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryConsumptionMai.GroupByBoxVisible = false;
		this.grdHistoryConsumptionMai.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).Location = new System.Drawing.Point(3, 126);
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).Name = "grdHistoryConsumptionMai";
		((ScrollableBase)this.grdHistoryConsumptionMai).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).Size = new System.Drawing.Size(1348, 317);
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).TabIndex = 181;
		this.grdHistoryConsumptionMai.TotalRow = (InheritableBoolean)1;
		this.grdHistoryConsumptionMai.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryConsumptionMai).VisualStyle = (VisualStyle)4;
		this.grdHistoryConsumptionMai.ColumnButtonClick += new ColumnActionEventHandler(grdHistoryConsumptionMai_ColumnButtonClick);
		this.grdHistoryConsumptionMai.SelectionChanged += new System.EventHandler(grdHistoryConsumptionMai_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionMai).DoubleClick += new System.EventHandler(grdHistoryConsumptionMai_DoubleClick);
		this.panel82.BackColor = System.Drawing.Color.Teal;
		this.panel82.Controls.Add(this.label144);
		this.panel82.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel82.Location = new System.Drawing.Point(3, 443);
		this.panel82.Name = "panel82";
		this.panel82.Size = new System.Drawing.Size(1348, 24);
		this.panel82.TabIndex = 182;
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label144.ForeColor = System.Drawing.Color.White;
		this.label144.Location = new System.Drawing.Point(3, 6);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(209, 14);
		this.label144.TabIndex = 211;
		this.label144.Text = "Detail of the above Selected row";
		this.grdHistoryConsumptionDetail.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryConsumptionDetail.AlternatingColors = true;
		((ScrollableBase)this.grdHistoryConsumptionDetail).BackColor = System.Drawing.Color.White;
		this.grdHistoryConsumptionDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryConsumptionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryConsumptionDetail.DesignTimeLayout = grdHistoryConsumptionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryConsumptionDetail.FilterMode = (FilterMode)2;
		this.grdHistoryConsumptionDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryConsumptionDetail.GroupByBoxVisible = false;
		this.grdHistoryConsumptionDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).Location = new System.Drawing.Point(3, 467);
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).Name = "grdHistoryConsumptionDetail";
		((ScrollableBase)this.grdHistoryConsumptionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).Size = new System.Drawing.Size(1348, 110);
		((System.Windows.Forms.Control)(object)this.grdHistoryConsumptionDetail).TabIndex = 183;
		this.grdHistoryConsumptionDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryConsumptionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryConsumptionDetail).VisualStyle = (VisualStyle)4;
		this.tabPage9.Controls.Add(this.panel48);
		this.tabPage9.Controls.Add(this.toolStripConsumption);
		this.tabPage9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage9.Location = new System.Drawing.Point(4, 4);
		this.tabPage9.Name = "tabPage9";
		this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage9.Size = new System.Drawing.Size(1354, 580);
		this.tabPage9.TabIndex = 0;
		this.tabPage9.Text = "Form";
		this.tabPage9.UseVisualStyleBackColor = true;
		this.toolStripConsumption.BackColor = System.Drawing.Color.White;
		this.toolStripConsumption.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripConsumption.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.BtnNewConsumption, this.BtnRefreshConsumption, this.btnSaveConsumption, this.BtnUpdateconsumption, this.toolStripSeparator7, this.BtnPrintSlipConsumption, this.btnPrintVoucherConsumption, this.BtnPendingForRatesFormConsumption, this.BtnLoadDataForConsumptionEntry });
		this.toolStripConsumption.Location = new System.Drawing.Point(3, 3);
		this.toolStripConsumption.Name = "toolStripConsumption";
		this.toolStripConsumption.Size = new System.Drawing.Size(1348, 27);
		this.toolStripConsumption.TabIndex = 1;
		this.toolStripConsumption.Text = "toolStrip2";
		this.BtnNewConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewConsumption.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewConsumption.Name = "BtnNewConsumption";
		this.BtnNewConsumption.Size = new System.Drawing.Size(59, 24);
		this.BtnNewConsumption.Text = "&New";
		this.BtnNewConsumption.Click += new System.EventHandler(BtnNewConsumption_Click);
		this.BtnRefreshConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshConsumption.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshConsumption.Name = "BtnRefreshConsumption";
		this.BtnRefreshConsumption.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshConsumption.Text = "Refresh";
		this.BtnRefreshConsumption.Click += new System.EventHandler(BtnRefreshConsumption_Click);
		this.btnSaveConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveConsumption.Image = (System.Drawing.Image)resources.GetObject("btnSaveConsumption.Image");
		this.btnSaveConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveConsumption.Name = "btnSaveConsumption";
		this.btnSaveConsumption.Size = new System.Drawing.Size(64, 24);
		this.btnSaveConsumption.Text = "&Save";
		this.btnSaveConsumption.Click += new System.EventHandler(btnSaveConsumption_Click);
		this.BtnUpdateconsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateconsumption.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateconsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateconsumption.Name = "BtnUpdateconsumption";
		this.BtnUpdateconsumption.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdateconsumption.Text = "&Update";
		this.BtnUpdateconsumption.Visible = false;
		this.BtnUpdateconsumption.Click += new System.EventHandler(BtnUpdateconsumption_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.BtnPrintSlipConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintSlipConsumption.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintSlipConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintSlipConsumption.Name = "BtnPrintSlipConsumption";
		this.BtnPrintSlipConsumption.Size = new System.Drawing.Size(92, 24);
		this.BtnPrintSlipConsumption.Text = "623-Print";
		this.BtnPrintSlipConsumption.Click += new System.EventHandler(BtnPrintSlipConsumption_Click);
		this.btnPrintVoucherConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintVoucherConsumption.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintVoucherConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintVoucherConsumption.Name = "btnPrintVoucherConsumption";
		this.btnPrintVoucherConsumption.Size = new System.Drawing.Size(115, 24);
		this.btnPrintVoucherConsumption.Text = "118-Voucher";
		this.btnPrintVoucherConsumption.Click += new System.EventHandler(btnPrintVoucherConsumption_Click);
		this.BtnPendingForRatesFormConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPendingForRatesFormConsumption.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPendingForRatesFormConsumption.Name = "BtnPendingForRatesFormConsumption";
		this.BtnPendingForRatesFormConsumption.Size = new System.Drawing.Size(135, 24);
		this.BtnPendingForRatesFormConsumption.Text = "Pending For Rates ";
		this.BtnPendingForRatesFormConsumption.Click += new System.EventHandler(BtnPendingForRatesFormConsumption_Click);
		this.BtnLoadDataForConsumptionEntry.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadDataForConsumptionEntry.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadDataForConsumptionEntry.Name = "BtnLoadDataForConsumptionEntry";
		this.BtnLoadDataForConsumptionEntry.Size = new System.Drawing.Size(230, 24);
		this.BtnLoadDataForConsumptionEntry.Text = "Load Data for Consumption Entry";
		this.BtnLoadDataForConsumptionEntry.Click += new System.EventHandler(BtnLoadDataForConsumptionEntry_Click);
		this.panel48.Controls.Add(this.panel74);
		this.panel48.Controls.Add(this.panelManualEntryConsumption);
		this.panel48.Controls.Add(this.panel79);
		this.panel48.Controls.Add(this.panel80);
		this.panel48.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel48.Location = new System.Drawing.Point(3, 30);
		this.panel48.Name = "panel48";
		this.panel48.Size = new System.Drawing.Size(1348, 547);
		this.panel48.TabIndex = 174;
		this.panel80.BackColor = System.Drawing.Color.Teal;
		this.panel80.Controls.Add(this.label142);
		this.panel80.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel80.Location = new System.Drawing.Point(0, 0);
		this.panel80.Name = "panel80";
		this.panel80.Size = new System.Drawing.Size(1348, 29);
		this.panel80.TabIndex = 2;
		this.label142.AutoSize = true;
		this.label142.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label142.ForeColor = System.Drawing.Color.White;
		this.label142.Location = new System.Drawing.Point(6, 5);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(170, 19);
		this.label142.TabIndex = 210;
		this.label142.Text = "Consumption           ";
		this.panel79.Controls.Add(this.groupBox10);
		this.panel79.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel79.Location = new System.Drawing.Point(0, 29);
		this.panel79.Name = "panel79";
		this.panel79.Size = new System.Drawing.Size(1348, 120);
		this.panel79.TabIndex = 3;
		this.groupBox10.Controls.Add(this.label135);
		this.groupBox10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantConsumption);
		this.groupBox10.Controls.Add(this.txtDocNoConsumption);
		this.groupBox10.Controls.Add(this.label136);
		this.groupBox10.Controls.Add(this.txtDocDateConsumption);
		this.groupBox10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption);
		this.groupBox10.Controls.Add(this.label137);
		this.groupBox10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption);
		this.groupBox10.Controls.Add(this.label138);
		this.groupBox10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption);
		this.groupBox10.Controls.Add(this.label139);
		this.groupBox10.Controls.Add(this.label140);
		this.groupBox10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption);
		this.groupBox10.Controls.Add(this.label141);
		this.groupBox10.Controls.Add(this.txtRemarksHeaderConsumption);
		this.groupBox10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox10.Location = new System.Drawing.Point(7, 3);
		this.groupBox10.Name = "groupBox10";
		this.groupBox10.Size = new System.Drawing.Size(1024, 106);
		this.groupBox10.TabIndex = 0;
		this.groupBox10.TabStop = false;
		this.groupBox10.Text = "Main";
		this.txtRemarksHeaderConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderConsumption.Location = new System.Drawing.Point(665, 45);
		this.txtRemarksHeaderConsumption.Multiline = true;
		this.txtRemarksHeaderConsumption.Name = "txtRemarksHeaderConsumption";
		this.txtRemarksHeaderConsumption.Size = new System.Drawing.Size(353, 55);
		this.txtRemarksHeaderConsumption.TabIndex = 7;
		this.label141.BackColor = System.Drawing.Color.LightGray;
		this.label141.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label141.Location = new System.Drawing.Point(665, 16);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(353, 26);
		this.label141.TabIndex = 6;
		this.label141.Text = "Remarks";
		this.label141.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label140.AutoSize = true;
		this.label140.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label140.Location = new System.Drawing.Point(4, 51);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(78, 15);
		this.label140.TabIndex = 151;
		this.label140.Text = "Job Order No";
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(351, 51);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(58, 15);
		this.label139.TabIndex = 173;
		this.label139.Text = "WIP Item";
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(351, 80);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(94, 15);
		this.label138.TabIndex = 173;
		this.label138.Text = "Pro_Department";
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label137.Location = new System.Drawing.Point(351, 22);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(78, 15);
		this.label137.TabIndex = 171;
		this.label137.Text = "WIP Account";
		this.txtDocDateConsumption.CustomFormat = "dd-MMM-yy";
		this.txtDocDateConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDateConsumption.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateConsumption.Location = new System.Drawing.Point(175, 18);
		this.txtDocDateConsumption.Name = "txtDocDateConsumption";
		this.txtDocDateConsumption.Size = new System.Drawing.Size(169, 23);
		this.txtDocDateConsumption.TabIndex = 1;
		this.label136.AutoSize = true;
		this.label136.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label136.Location = new System.Drawing.Point(4, 81);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(69, 15);
		this.label136.TabIndex = 209;
		this.label136.Text = "Plant Name";
		this.txtDocNoConsumption.BackColor = System.Drawing.Color.White;
		this.txtDocNoConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoConsumption.Location = new System.Drawing.Point(92, 18);
		this.txtDocNoConsumption.Name = "txtDocNoConsumption";
		this.txtDocNoConsumption.ReadOnly = true;
		this.txtDocNoConsumption.Size = new System.Drawing.Size(77, 23);
		this.txtDocNoConsumption.TabIndex = 0;
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(4, 22);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(88, 15);
		this.label135.TabIndex = 25;
		this.label135.Text = "Doc No && Date";
		this.panelManualEntryConsumption.Controls.Add(this.groupBox9);
		this.panelManualEntryConsumption.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelManualEntryConsumption.Location = new System.Drawing.Point(0, 149);
		this.panelManualEntryConsumption.Name = "panelManualEntryConsumption";
		this.panelManualEntryConsumption.Size = new System.Drawing.Size(1348, 141);
		this.panelManualEntryConsumption.TabIndex = 4;
		this.groupBox9.Controls.Add(this.rdSearchByCode);
		this.groupBox9.Controls.Add(this.rdSearchByName);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption);
		this.groupBox9.Controls.Add(this.label121);
		this.groupBox9.Controls.Add(this.label122);
		this.groupBox9.Controls.Add(this.txtItemQtyConsumption);
		this.groupBox9.Controls.Add(this.txtBalanceWeightConsumption);
		this.groupBox9.Controls.Add(this.txtNetWeightConsumption);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption);
		this.groupBox9.Controls.Add(this.label123);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption);
		this.groupBox9.Controls.Add(this.txtRemarksDetailConsumption);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption);
		this.groupBox9.Controls.Add(this.txtAmountConsumption);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption);
		this.groupBox9.Controls.Add(this.lblAmountConsumption);
		this.groupBox9.Controls.Add(this.label125);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption);
		this.groupBox9.Controls.Add(this.label126);
		this.groupBox9.Controls.Add(this.lblRateUomConsumption);
		this.groupBox9.Controls.Add(this.label128);
		this.groupBox9.Controls.Add(this.txtRateConsumption);
		this.groupBox9.Controls.Add(this.label129);
		this.groupBox9.Controls.Add(this.lblRateConsumption);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption);
		this.groupBox9.Controls.Add(this.BtnAddConsumption);
		this.groupBox9.Controls.Add(this.txtBalanceQtyConsumption);
		this.groupBox9.Controls.Add(this.BtnCancelConsumption);
		this.groupBox9.Controls.Add(this.label131);
		this.groupBox9.Controls.Add(this.BtnUpdateDetailConsumption);
		this.groupBox9.Controls.Add(this.label132);
		this.groupBox9.Controls.Add(this.label133);
		this.groupBox9.Controls.Add(this.label134);
		this.groupBox9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox9.Location = new System.Drawing.Point(7, 10);
		this.groupBox9.Name = "groupBox9";
		this.groupBox9.Size = new System.Drawing.Size(1024, 127);
		this.groupBox9.TabIndex = 0;
		this.groupBox9.TabStop = false;
		this.groupBox9.Text = "Detail";
		this.label134.AutoSize = true;
		this.label134.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label134.Location = new System.Drawing.Point(6, 22);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(69, 15);
		this.label134.TabIndex = 193;
		this.label134.Text = "WareHouse";
		this.label133.AutoSize = true;
		this.label133.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label133.Location = new System.Drawing.Point(189, 73);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(52, 15);
		this.label133.TabIndex = 200;
		this.label133.Text = "Remarks";
		this.label132.AutoSize = true;
		this.label132.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label132.Location = new System.Drawing.Point(833, 22);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(70, 15);
		this.label132.TabIndex = 197;
		this.label132.Text = "Balance Qty";
		this.BtnUpdateDetailConsumption.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDetailConsumption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetailConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateDetailConsumption.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetailConsumption.Location = new System.Drawing.Point(837, 91);
		this.BtnUpdateDetailConsumption.Name = "BtnUpdateDetailConsumption";
		this.BtnUpdateDetailConsumption.Size = new System.Drawing.Size(70, 27);
		this.BtnUpdateDetailConsumption.TabIndex = 15;
		this.BtnUpdateDetailConsumption.Text = "Update ";
		this.BtnUpdateDetailConsumption.UseVisualStyleBackColor = false;
		this.BtnUpdateDetailConsumption.Click += new System.EventHandler(BtnUpdateDetailConsumption_Click);
		this.label131.AutoSize = true;
		this.label131.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label131.Location = new System.Drawing.Point(739, 22);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(60, 15);
		this.label131.TabIndex = 196;
		this.label131.Text = "Pack Type";
		this.BtnCancelConsumption.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelConsumption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelConsumption.ForeColor = System.Drawing.Color.White;
		this.BtnCancelConsumption.Location = new System.Drawing.Point(906, 91);
		this.BtnCancelConsumption.Name = "BtnCancelConsumption";
		this.BtnCancelConsumption.Size = new System.Drawing.Size(67, 27);
		this.BtnCancelConsumption.TabIndex = 16;
		this.BtnCancelConsumption.Text = "Cancel";
		this.BtnCancelConsumption.UseVisualStyleBackColor = false;
		this.BtnCancelConsumption.Click += new System.EventHandler(BtnCancelConsumption_Click);
		this.txtBalanceQtyConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQtyConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQtyConsumption.Location = new System.Drawing.Point(834, 42);
		this.txtBalanceQtyConsumption.Name = "txtBalanceQtyConsumption";
		this.txtBalanceQtyConsumption.ReadOnly = true;
		this.txtBalanceQtyConsumption.Size = new System.Drawing.Size(69, 23);
		this.txtBalanceQtyConsumption.TabIndex = 6;
		this.txtBalanceQtyConsumption.TabStop = false;
		this.txtBalanceQtyConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.BtnAddConsumption.BackColor = System.Drawing.Color.Teal;
		this.BtnAddConsumption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddConsumption.ForeColor = System.Drawing.Color.White;
		this.BtnAddConsumption.Location = new System.Drawing.Point(833, 91);
		this.BtnAddConsumption.Margin = new System.Windows.Forms.Padding(2);
		this.BtnAddConsumption.Name = "BtnAddConsumption";
		this.BtnAddConsumption.Size = new System.Drawing.Size(35, 27);
		this.BtnAddConsumption.TabIndex = 14;
		this.BtnAddConsumption.Text = "+";
		this.BtnAddConsumption.UseVisualStyleBackColor = false;
		this.BtnAddConsumption.Click += new System.EventHandler(BtnAddConsumption_Click);
		this.lblRateConsumption.AutoSize = true;
		this.lblRateConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateConsumption.Location = new System.Drawing.Point(557, 73);
		this.lblRateConsumption.Name = "lblRateConsumption";
		this.lblRateConsumption.Size = new System.Drawing.Size(90, 15);
		this.lblRateConsumption.TabIndex = 201;
		this.lblRateConsumption.Text = "Avg Rate(40Kg)";
		this.label129.AutoSize = true;
		this.label129.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label129.Location = new System.Drawing.Point(183, 22);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(67, 15);
		this.label129.TabIndex = 191;
		this.label129.Text = "Item Name";
		this.txtRateConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateConsumption.Location = new System.Drawing.Point(566, 93);
		this.txtRateConsumption.Name = "txtRateConsumption";
		this.txtRateConsumption.Size = new System.Drawing.Size(74, 23);
		this.txtRateConsumption.TabIndex = 11;
		this.txtRateConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateConsumption.TextChanged += new System.EventHandler(txtRateConsumption_TextChanged);
		this.label128.AutoSize = true;
		this.label128.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label128.Location = new System.Drawing.Point(581, 22);
		this.label128.Name = "label128";
		this.label128.Size = new System.Drawing.Size(46, 15);
		this.label128.TabIndex = 195;
		this.label128.Text = "Job Lot";
		this.lblRateUomConsumption.AutoSize = true;
		this.lblRateUomConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateUomConsumption.Location = new System.Drawing.Point(642, 73);
		this.lblRateUomConsumption.Name = "lblRateUomConsumption";
		this.lblRateUomConsumption.Size = new System.Drawing.Size(61, 15);
		this.lblRateUomConsumption.TabIndex = 202;
		this.lblRateUomConsumption.Text = "Rate UOM";
		this.label126.AutoSize = true;
		this.label126.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label126.Location = new System.Drawing.Point(391, 22);
		this.label126.Name = "label126";
		this.label126.Size = new System.Drawing.Size(63, 15);
		this.label126.TabIndex = 192;
		this.label126.Text = "Pack UOM";
		this.label125.AutoSize = true;
		this.label125.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label125.Location = new System.Drawing.Point(476, 22);
		this.label125.Name = "label125";
		this.label125.Size = new System.Drawing.Size(57, 15);
		this.label125.TabIndex = 194;
		this.label125.Text = "Crop Year";
		this.lblAmountConsumption.AutoSize = true;
		this.lblAmountConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAmountConsumption.Location = new System.Drawing.Point(731, 73);
		this.lblAmountConsumption.Name = "lblAmountConsumption";
		this.lblAmountConsumption.Size = new System.Drawing.Size(51, 15);
		this.lblAmountConsumption.TabIndex = 203;
		this.lblAmountConsumption.Text = "Amount";
		this.txtAmountConsumption.BackColor = System.Drawing.Color.White;
		this.txtAmountConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountConsumption.Location = new System.Drawing.Point(730, 93);
		this.txtAmountConsumption.Name = "txtAmountConsumption";
		this.txtAmountConsumption.ReadOnly = true;
		this.txtAmountConsumption.Size = new System.Drawing.Size(102, 23);
		this.txtAmountConsumption.TabIndex = 13;
		this.txtAmountConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarksDetailConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetailConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetailConsumption.Location = new System.Drawing.Point(184, 93);
		this.txtRemarksDetailConsumption.Name = "txtRemarksDetailConsumption";
		this.txtRemarksDetailConsumption.Size = new System.Drawing.Size(380, 23);
		this.txtRemarksDetailConsumption.TabIndex = 10;
		this.label123.AutoSize = true;
		this.label123.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label123.Location = new System.Drawing.Point(92, 73);
		this.label123.Name = "label123";
		this.label123.Size = new System.Drawing.Size(68, 15);
		this.label123.TabIndex = 207;
		this.label123.Text = "Net Weight";
		this.txtNetWeightConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightConsumption.Location = new System.Drawing.Point(89, 93);
		this.txtNetWeightConsumption.Name = "txtNetWeightConsumption";
		this.txtNetWeightConsumption.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeightConsumption.TabIndex = 9;
		this.txtNetWeightConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightConsumption.TextChanged += new System.EventHandler(txtNetWeightConsumption_TextChanged);
		this.txtNetWeightConsumption.Leave += new System.EventHandler(txtNetWeightConsumption_Leave);
		this.txtBalanceWeightConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeightConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeightConsumption.Location = new System.Drawing.Point(904, 42);
		this.txtBalanceWeightConsumption.Name = "txtBalanceWeightConsumption";
		this.txtBalanceWeightConsumption.ReadOnly = true;
		this.txtBalanceWeightConsumption.Size = new System.Drawing.Size(93, 23);
		this.txtBalanceWeightConsumption.TabIndex = 7;
		this.txtBalanceWeightConsumption.TabStop = false;
		this.txtBalanceWeightConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQtyConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQtyConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyConsumption.Location = new System.Drawing.Point(6, 93);
		this.txtItemQtyConsumption.Name = "txtItemQtyConsumption";
		this.txtItemQtyConsumption.Size = new System.Drawing.Size(81, 23);
		this.txtItemQtyConsumption.TabIndex = 8;
		this.txtItemQtyConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQtyConsumption.TextChanged += new System.EventHandler(txtItemQtyConsumption_TextChanged);
		this.txtItemQtyConsumption.Leave += new System.EventHandler(txtItemQtyConsumption_Leave);
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label122.Location = new System.Drawing.Point(9, 73);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(53, 15);
		this.label122.TabIndex = 206;
		this.label122.Text = "Quantity";
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label121.Location = new System.Drawing.Point(904, 22);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(90, 15);
		this.label121.TabIndex = 199;
		this.label121.Text = "Balance Weight";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(262, 20);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 208;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(329, 20);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 209;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.panel74.Controls.Add(this.panel76);
		this.panel74.Controls.Add(this.panel77);
		this.panel74.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel74.Location = new System.Drawing.Point(0, 290);
		this.panel74.Name = "panel74";
		this.panel74.Size = new System.Drawing.Size(1348, 257);
		this.panel74.TabIndex = 3;
		this.panel77.BackColor = System.Drawing.Color.Teal;
		this.panel77.Controls.Add(this.label120);
		this.panel77.Controls.Add(this.ctrlGrdBar16);
		this.panel77.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel77.Location = new System.Drawing.Point(0, 0);
		this.panel77.Name = "panel77";
		this.panel77.Size = new System.Drawing.Size(1348, 28);
		this.panel77.TabIndex = 1;
		this.ctrlGrdBar16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar16.FormName = this;
		this.ctrlGrdBar16.Location = new System.Drawing.Point(1307, 3);
		this.ctrlGrdBar16.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar16.MyGrid = this.grdConsumption;
		this.ctrlGrdBar16.Name = "ctrlGrdBar16";
		this.ctrlGrdBar16.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar16.TabIndex = 20;
		this.ctrlGrdBar16.Load += new System.EventHandler(ctrlGrdBar16_Load);
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label120.ForeColor = System.Drawing.Color.White;
		this.label120.Location = new System.Drawing.Point(3, 4);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(169, 19);
		this.label120.TabIndex = 172;
		this.label120.Text = "Consumption Detail";
		this.panel76.Controls.Add((System.Windows.Forms.Control)(object)this.grdConsumption);
		this.panel76.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel76.Location = new System.Drawing.Point(0, 28);
		this.panel76.Name = "panel76";
		this.panel76.Size = new System.Drawing.Size(1348, 229);
		this.panel76.TabIndex = 2;
		((ScrollableBase)this.grdConsumption).BackColor = System.Drawing.Color.White;
		grdConsumption_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdConsumption.DesignTimeLayout = grdConsumption_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdConsumption).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdConsumption.FilterMode = (FilterMode)2;
		this.grdConsumption.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdConsumption).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdConsumption.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdConsumption).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdConsumption).Name = "grdConsumption";
		((ScrollableBase)this.grdConsumption).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdConsumption).Size = new System.Drawing.Size(1348, 229);
		((System.Windows.Forms.Control)(object)this.grdConsumption).TabIndex = 0;
		this.grdConsumption.TotalRow = (InheritableBoolean)1;
		this.grdConsumption.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdConsumption).VisualStyle = (VisualStyle)4;
		this.grdConsumption.CellUpdated += new ColumnActionEventHandler(grdConsumption_CellUpdated);
		this.grdConsumption.ColumnButtonClick += new ColumnActionEventHandler(grdConsumption_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdConsumption).DoubleClick += new System.EventHandler(grdConsumption_DoubleClick);
		this.OutPut.Controls.Add(this.panel3);
		this.OutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OutPut.Location = new System.Drawing.Point(4, 26);
		this.OutPut.Name = "OutPut";
		this.OutPut.Size = new System.Drawing.Size(1362, 609);
		this.OutPut.TabIndex = 5;
		this.OutPut.Text = "OutPut";
		this.OutPut.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.tabControl3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1362, 609);
		this.panel3.TabIndex = 0;
		this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1362, 609);
		this.tabControl3.TabIndex = 22;
		this.tabControl3.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistory);
		this.tabPage6.Controls.Add(this.panel85);
		this.tabPage6.Controls.Add(this.panel84);
		this.tabPage6.Controls.Add(this.panel81);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutDetail);
		this.tabPage6.Controls.Add(this.panel60);
		this.tabPage6.Location = new System.Drawing.Point(4, 4);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1354, 583);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "History";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel60.BackColor = System.Drawing.Color.Teal;
		this.panel60.Controls.Add(this.toolStrip8);
		this.panel60.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel60.Location = new System.Drawing.Point(3, 3);
		this.panel60.Name = "panel60";
		this.panel60.Size = new System.Drawing.Size(1348, 50);
		this.panel60.TabIndex = 6;
		this.toolStrip8.BackColor = System.Drawing.Color.White;
		this.toolStrip8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.toolStrip8.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewOutPuthistory, this.toolStripSeparator6, this.BtnRefereshOutPutHistory });
		this.toolStrip8.Location = new System.Drawing.Point(0, 0);
		this.toolStrip8.Name = "toolStrip8";
		this.toolStrip8.Size = new System.Drawing.Size(1348, 27);
		this.toolStrip8.TabIndex = 0;
		this.toolStrip8.Text = "toolStrip8";
		this.BtnNewOutPuthistory.BackColor = System.Drawing.Color.White;
		this.BtnNewOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewOutPuthistory.ForeColor = System.Drawing.Color.Black;
		this.BtnNewOutPuthistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewOutPuthistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewOutPuthistory.Name = "BtnNewOutPuthistory";
		this.BtnNewOutPuthistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewOutPuthistory.Text = "&New";
		this.BtnNewOutPuthistory.Click += new System.EventHandler(BtnNewOutPuthistory_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.BtnRefereshOutPutHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefereshOutPutHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefereshOutPutHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshOutPutHistory.Name = "BtnRefereshOutPutHistory";
		this.BtnRefereshOutPutHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshOutPutHistory.Text = "Refresh";
		this.BtnRefereshOutPutHistory.Click += new System.EventHandler(BtnRefereshOutPutHistory_Click);
		this.grdOutPutDetail.AllowEdit = (InheritableBoolean)2;
		this.grdOutPutDetail.AlternatingColors = true;
		((ScrollableBase)this.grdOutPutDetail).BackColor = System.Drawing.Color.White;
		this.grdOutPutDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOutPutDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutDetail.DesignTimeLayout = grdOutPutDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdOutPutDetail.FilterMode = (FilterMode)2;
		this.grdOutPutDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutDetail.GroupByBoxVisible = false;
		this.grdOutPutDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Location = new System.Drawing.Point(3, 393);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Name = "grdOutPutDetail";
		((ScrollableBase)this.grdOutPutDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Size = new System.Drawing.Size(1348, 187);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).TabIndex = 1;
		this.grdOutPutDetail.TotalRow = (InheritableBoolean)1;
		this.grdOutPutDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutDetail).VisualStyle = (VisualStyle)4;
		this.panel81.Controls.Add(this.groupBox12);
		this.panel81.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel81.Location = new System.Drawing.Point(3, 53);
		this.panel81.Name = "panel81";
		this.panel81.Size = new System.Drawing.Size(1348, 50);
		this.panel81.TabIndex = 176;
		this.groupBox12.Controls.Add(this.BtnShowOutPuthistory);
		this.groupBox12.Controls.Add(this.label150);
		this.groupBox12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory);
		this.groupBox12.Controls.Add(this.txttoDateOutPuthistory);
		this.groupBox12.Controls.Add(this.label151);
		this.groupBox12.Controls.Add(this.label152);
		this.groupBox12.Controls.Add(this.txtDocnotoOutPuthistory);
		this.groupBox12.Controls.Add(this.txtFromDateOutPuthistory);
		this.groupBox12.Controls.Add(this.label153);
		this.groupBox12.Controls.Add(this.label154);
		this.groupBox12.Controls.Add(this.txtDocNoFromOutPuthistory);
		this.groupBox12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox12.Location = new System.Drawing.Point(6, 2);
		this.groupBox12.Name = "groupBox12";
		this.groupBox12.Size = new System.Drawing.Size(1038, 46);
		this.groupBox12.TabIndex = 0;
		this.groupBox12.TabStop = false;
		this.groupBox12.Text = "Filters";
		this.txtDocNoFromOutPuthistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFromOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFromOutPuthistory.Location = new System.Drawing.Point(490, 16);
		this.txtDocNoFromOutPuthistory.Name = "txtDocNoFromOutPuthistory";
		this.txtDocNoFromOutPuthistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNoFromOutPuthistory.TabIndex = 2;
		this.txtDocNoFromOutPuthistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFromOutPuthistory_KeyPress);
		this.label154.AutoSize = true;
		this.label154.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label154.Location = new System.Drawing.Point(429, 20);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(60, 15);
		this.label154.TabIndex = 173;
		this.label154.Text = "Doc From";
		this.label153.AutoSize = true;
		this.label153.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label153.Location = new System.Drawing.Point(564, 20);
		this.label153.Name = "label153";
		this.label153.Size = new System.Drawing.Size(45, 15);
		this.label153.TabIndex = 2;
		this.label153.Text = "Doc To";
		this.txtFromDateOutPuthistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateOutPuthistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateOutPuthistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateOutPuthistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromDateOutPuthistory.Name = "txtFromDateOutPuthistory";
		this.txtFromDateOutPuthistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromDateOutPuthistory.TabIndex = 0;
		this.txtDocnotoOutPuthistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocnotoOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocnotoOutPuthistory.Location = new System.Drawing.Point(609, 16);
		this.txtDocnotoOutPuthistory.Name = "txtDocnotoOutPuthistory";
		this.txtDocnotoOutPuthistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocnotoOutPuthistory.TabIndex = 3;
		this.txtDocnotoOutPuthistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocnotoOutPuthistory_KeyPress);
		this.label152.AutoSize = true;
		this.label152.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label152.Location = new System.Drawing.Point(2, 20);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(63, 15);
		this.label152.TabIndex = 171;
		this.label152.Text = "Date From";
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label151.Location = new System.Drawing.Point(217, 20);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(48, 15);
		this.label151.TabIndex = 171;
		this.label151.Text = "Date To";
		this.txttoDateOutPuthistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDateOutPuthistory.CustomFormat = "dd-MMM-yy";
		this.txttoDateOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDateOutPuthistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttoDateOutPuthistory.Location = new System.Drawing.Point(268, 16);
		this.txttoDateOutPuthistory.Name = "txttoDateOutPuthistory";
		this.txttoDateOutPuthistory.Size = new System.Drawing.Size(148, 23);
		this.txttoDateOutPuthistory.TabIndex = 1;
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(683, 20);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(59, 15);
		this.label150.TabIndex = 3;
		this.label150.Text = "Job Order";
		this.BtnShowOutPuthistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowOutPuthistory.FlatAppearance.BorderSize = 0;
		this.BtnShowOutPuthistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowOutPuthistory.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.BtnShowOutPuthistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowOutPuthistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowOutPuthistory.Location = new System.Drawing.Point(969, 15);
		this.BtnShowOutPuthistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowOutPuthistory.Name = "BtnShowOutPuthistory";
		this.BtnShowOutPuthistory.Size = new System.Drawing.Size(61, 25);
		this.BtnShowOutPuthistory.TabIndex = 5;
		this.BtnShowOutPuthistory.Text = "Show";
		this.BtnShowOutPuthistory.UseVisualStyleBackColor = false;
		this.BtnShowOutPuthistory.Click += new System.EventHandler(BtnShowOutPuthistory_Click);
		this.panel84.BackColor = System.Drawing.Color.Teal;
		this.panel84.Controls.Add(this.label155);
		this.panel84.Controls.Add(this.ctrlGrdBar11);
		this.panel84.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel84.Location = new System.Drawing.Point(3, 103);
		this.panel84.Name = "panel84";
		this.panel84.Size = new System.Drawing.Size(1348, 24);
		this.panel84.TabIndex = 177;
		this.ctrlGrdBar11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar11.FormName = this;
		this.ctrlGrdBar11.Location = new System.Drawing.Point(1298, 0);
		this.ctrlGrdBar11.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar11.MyGrid = this.grdOutputHistory;
		this.ctrlGrdBar11.Name = "ctrlGrdBar11";
		this.ctrlGrdBar11.Size = new System.Drawing.Size(48, 22);
		this.ctrlGrdBar11.TabIndex = 175;
		this.ctrlGrdBar11.Load += new System.EventHandler(ctrlGrdBar11_Load);
		this.label155.AutoSize = true;
		this.label155.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label155.ForeColor = System.Drawing.Color.White;
		this.label155.Location = new System.Drawing.Point(3, 3);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(40, 17);
		this.label155.TabIndex = 211;
		this.label155.Text = "Main";
		this.panel85.BackColor = System.Drawing.Color.Teal;
		this.panel85.Controls.Add(this.label156);
		this.panel85.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel85.Location = new System.Drawing.Point(3, 373);
		this.panel85.Name = "panel85";
		this.panel85.Size = new System.Drawing.Size(1348, 20);
		this.panel85.TabIndex = 178;
		this.label156.AutoSize = true;
		this.label156.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label156.ForeColor = System.Drawing.Color.White;
		this.label156.Location = new System.Drawing.Point(3, 1);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(230, 17);
		this.label156.TabIndex = 211;
		this.label156.Text = "Detail of the Selected above row";
		this.grdOutputHistory.AllowEdit = (InheritableBoolean)2;
		this.grdOutputHistory.AlternatingColors = true;
		((ScrollableBase)this.grdOutputHistory).BackColor = System.Drawing.Color.White;
		this.grdOutputHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOutputHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutputHistory.DesignTimeLayout = grdOutputHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutputHistory.FilterMode = (FilterMode)2;
		this.grdOutputHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Location = new System.Drawing.Point(3, 127);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Name = "grdOutputHistory";
		((ScrollableBase)this.grdOutputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Size = new System.Drawing.Size(1348, 246);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).TabIndex = 0;
		this.grdOutputHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistory).VisualStyle = (VisualStyle)4;
		this.grdOutputHistory.ColumnButtonClick += new ColumnActionEventHandler(grdOutputHistory_ColumnButtonClick);
		this.grdOutputHistory.SelectionChanged += new System.EventHandler(grdOutputHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).DoubleClick += new System.EventHandler(grdOutputHistory_DoubleClick);
		this.tabPage5.Controls.Add(this.panel2);
		this.tabPage5.Location = new System.Drawing.Point(4, 4);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1354, 583);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "Form";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel24);
		this.panel2.Controls.Add(this.panel22);
		this.panel2.Controls.Add(this.panel30);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1348, 577);
		this.panel2.TabIndex = 22;
		this.panel4.Controls.Add(this.toolStrip6);
		this.panel4.Controls.Add(this.ctrlGrdBar4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1348, 31);
		this.panel4.TabIndex = 22;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar4.TabIndex = 3;
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNewOutPut, this.btnRefreshOutPut, this.btnSaveOutPut, this.btnUpdateOutPut, this.toolStripSeparator5, this.btnPrintOutPut, this.btnVoucherOutPut, this.btnPenForRateEntries, this.btnDailyPlantConsumedHours });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1348, 27);
		this.toolStrip6.TabIndex = 2;
		this.toolStrip6.Text = "toolStrip6";
		this.btnNewOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewOutPut.Name = "btnNewOutPut";
		this.btnNewOutPut.Size = new System.Drawing.Size(59, 24);
		this.btnNewOutPut.Text = "&New";
		this.btnNewOutPut.Click += new System.EventHandler(btnNewOutPut_Click);
		this.btnRefreshOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshOutPut.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshOutPut.Name = "btnRefreshOutPut";
		this.btnRefreshOutPut.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshOutPut.Text = "Refresh";
		this.btnRefreshOutPut.Click += new System.EventHandler(btnRefreshOutPut_Click);
		this.btnSaveOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveOutPut.Image = (System.Drawing.Image)resources.GetObject("btnSaveOutPut.Image");
		this.btnSaveOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveOutPut.Name = "btnSaveOutPut";
		this.btnSaveOutPut.Size = new System.Drawing.Size(64, 24);
		this.btnSaveOutPut.Text = "&Save";
		this.btnSaveOutPut.Click += new System.EventHandler(btnSaveOutPut_Click);
		this.btnUpdateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOutPut.Name = "btnUpdateOutPut";
		this.btnUpdateOutPut.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateOutPut.Text = "&Update";
		this.btnUpdateOutPut.Visible = false;
		this.btnUpdateOutPut.Click += new System.EventHandler(btnUpdateOutPut_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOutPut.Name = "btnPrintOutPut";
		this.btnPrintOutPut.Size = new System.Drawing.Size(92, 24);
		this.btnPrintOutPut.Text = "601-Print";
		this.btnPrintOutPut.Click += new System.EventHandler(btnPrintOutPut_Click);
		this.btnVoucherOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOutPut.Name = "btnVoucherOutPut";
		this.btnVoucherOutPut.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherOutPut.Text = "118-Voucher";
		this.btnVoucherOutPut.Click += new System.EventHandler(btnVoucherOutPut_Click);
		this.btnPenForRateEntries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnPenForRateEntries.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPenForRateEntries.Image = (System.Drawing.Image)resources.GetObject("btnPenForRateEntries.Image");
		this.btnPenForRateEntries.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPenForRateEntries.Name = "btnPenForRateEntries";
		this.btnPenForRateEntries.Size = new System.Drawing.Size(135, 24);
		this.btnPenForRateEntries.Text = "Pending For Rates ";
		this.btnPenForRateEntries.Click += new System.EventHandler(btnPenForRateEntries_Click);
		this.btnDailyPlantConsumedHours.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDailyPlantConsumedHours.Name = "btnDailyPlantConsumedHours";
		this.btnDailyPlantConsumedHours.Size = new System.Drawing.Size(163, 24);
		this.btnDailyPlantConsumedHours.Text = "Daily Plant Consumed Hours";
		this.btnDailyPlantConsumedHours.Click += new System.EventHandler(btnDailyPlantConsumedHours_Click);
		this.panel30.Controls.Add(this.panel64);
		this.panel30.Controls.Add(this.groupBox4);
		this.panel30.Controls.Add(this.groupBox3);
		this.panel30.Controls.Add(this.txtAvgFinishGoodRate);
		this.panel30.Controls.Add(this.btnApplyRate);
		this.panel30.Controls.Add(this.txtFGRate);
		this.panel30.Controls.Add(this.label76);
		this.panel30.Controls.Add(this.txtFGWeight);
		this.panel30.Controls.Add(this.label75);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 31);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1348, 276);
		this.panel30.TabIndex = 0;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(1592, 8);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(64, 15);
		this.label75.TabIndex = 175;
		this.label75.Text = "FG Weight";
		this.label75.Visible = false;
		this.txtFGWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGWeight.Location = new System.Drawing.Point(1590, 31);
		this.txtFGWeight.Name = "txtFGWeight";
		this.txtFGWeight.Size = new System.Drawing.Size(93, 21);
		this.txtFGWeight.TabIndex = 174;
		this.txtFGWeight.Visible = false;
		this.txtFGWeight.TextChanged += new System.EventHandler(txtFGWeight_TextChanged);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(1687, 10);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(52, 15);
		this.label76.TabIndex = 177;
		this.label76.Text = "FG Rate";
		this.label76.Visible = false;
		this.txtFGRate.BackColor = System.Drawing.Color.White;
		this.txtFGRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGRate.Location = new System.Drawing.Point(1685, 31);
		this.txtFGRate.Name = "txtFGRate";
		this.txtFGRate.ReadOnly = true;
		this.txtFGRate.Size = new System.Drawing.Size(93, 21);
		this.txtFGRate.TabIndex = 176;
		this.txtFGRate.Visible = false;
		this.btnApplyRate.BackColor = System.Drawing.Color.White;
		this.btnApplyRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplyRate.Location = new System.Drawing.Point(1690, 58);
		this.btnApplyRate.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplyRate.Name = "btnApplyRate";
		this.btnApplyRate.Size = new System.Drawing.Size(74, 24);
		this.btnApplyRate.TabIndex = 178;
		this.btnApplyRate.Text = "ApplyRate";
		this.btnApplyRate.UseVisualStyleBackColor = false;
		this.btnApplyRate.Visible = false;
		this.btnApplyRate.Click += new System.EventHandler(btnApplyRate_Click);
		this.txtAvgFinishGoodRate.BackColor = System.Drawing.Color.White;
		this.txtAvgFinishGoodRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFinishGoodRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFinishGoodRate.Location = new System.Drawing.Point(1590, 58);
		this.txtAvgFinishGoodRate.Name = "txtAvgFinishGoodRate";
		this.txtAvgFinishGoodRate.ReadOnly = true;
		this.txtAvgFinishGoodRate.Size = new System.Drawing.Size(93, 21);
		this.txtAvgFinishGoodRate.TabIndex = 179;
		this.txtAvgFinishGoodRate.Visible = false;
		this.groupBox3.Controls.Add(this.txtDocNoOutPut);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput);
		this.groupBox3.Controls.Add(this.label73);
		this.groupBox3.Controls.Add(this.label170);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label86);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.DocDateOutPut);
		this.groupBox3.Controls.Add(this.label71);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.txtBalWeightTotal);
		this.groupBox3.Controls.Add(this.txtBalQtyTotal);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.txtRemarksHeaderOutPut);
		this.groupBox3.Controls.Add(this.txtInPutTotalQty);
		this.groupBox3.Controls.Add(this.txtInPutTotalWeight);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.txtOutQtyTotal);
		this.groupBox3.Controls.Add(this.txtOutWeightTotal);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 30);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1028, 110);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.txtOutWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutWeightTotal.Location = new System.Drawing.Point(876, 49);
		this.txtOutWeightTotal.Name = "txtOutWeightTotal";
		this.txtOutWeightTotal.ReadOnly = true;
		this.txtOutWeightTotal.Size = new System.Drawing.Size(146, 23);
		this.txtOutWeightTotal.TabIndex = 10;
		this.txtOutWeightTotal.TabStop = false;
		this.txtOutQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutQtyTotal.Location = new System.Drawing.Point(693, 49);
		this.txtOutQtyTotal.Name = "txtOutQtyTotal";
		this.txtOutQtyTotal.ReadOnly = true;
		this.txtOutQtyTotal.Size = new System.Drawing.Size(106, 23);
		this.txtOutQtyTotal.TabIndex = 7;
		this.txtOutQtyTotal.TabStop = false;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(631, 23);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(40, 15);
		this.label53.TabIndex = 174;
		this.label53.Text = "In Qty";
		this.label53.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtInPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalWeight.Location = new System.Drawing.Point(876, 19);
		this.txtInPutTotalWeight.Name = "txtInPutTotalWeight";
		this.txtInPutTotalWeight.ReadOnly = true;
		this.txtInPutTotalWeight.Size = new System.Drawing.Size(146, 23);
		this.txtInPutTotalWeight.TabIndex = 9;
		this.txtInPutTotalWeight.TabStop = false;
		this.txtInPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalQty.Location = new System.Drawing.Point(693, 19);
		this.txtInPutTotalQty.Name = "txtInPutTotalQty";
		this.txtInPutTotalQty.ReadOnly = true;
		this.txtInPutTotalQty.Size = new System.Drawing.Size(106, 23);
		this.txtInPutTotalQty.TabIndex = 6;
		this.txtInPutTotalQty.TabStop = false;
		this.txtRemarksHeaderOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderOutPut.Location = new System.Drawing.Point(94, 76);
		this.txtRemarksHeaderOutPut.Name = "txtRemarksHeaderOutPut";
		this.txtRemarksHeaderOutPut.Size = new System.Drawing.Size(531, 23);
		this.txtRemarksHeaderOutPut.TabIndex = 3;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(631, 53);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(49, 15);
		this.label62.TabIndex = 174;
		this.label62.Text = "Out Qty";
		this.label62.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(3, 78);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(62, 19);
		this.label19.TabIndex = 138;
		this.label19.Text = "Remarks";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtBalQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQtyTotal.Location = new System.Drawing.Point(693, 78);
		this.txtBalQtyTotal.Name = "txtBalQtyTotal";
		this.txtBalQtyTotal.ReadOnly = true;
		this.txtBalQtyTotal.Size = new System.Drawing.Size(106, 23);
		this.txtBalQtyTotal.TabIndex = 8;
		this.txtBalQtyTotal.TabStop = false;
		this.txtBalWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeightTotal.Location = new System.Drawing.Point(876, 78);
		this.txtBalWeightTotal.Name = "txtBalWeightTotal";
		this.txtBalWeightTotal.ReadOnly = true;
		this.txtBalWeightTotal.Size = new System.Drawing.Size(146, 23);
		this.txtBalWeightTotal.TabIndex = 11;
		this.txtBalWeightTotal.TabStop = false;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(6, 53);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(78, 15);
		this.label60.TabIndex = 153;
		this.label60.Text = "Job Order No";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(805, 23);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(60, 15);
		this.label54.TabIndex = 174;
		this.label54.Text = "In Weight";
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(631, 82);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(45, 15);
		this.label71.TabIndex = 174;
		this.label71.Text = "Bal Qty";
		this.label71.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.DocDateOutPut.CustomFormat = "dd-MMM-yy";
		this.DocDateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDateOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateOutPut.Location = new System.Drawing.Point(174, 19);
		this.DocDateOutPut.Name = "DocDateOutPut";
		this.DocDateOutPut.Size = new System.Drawing.Size(122, 23);
		this.DocDateOutPut.TabIndex = 1;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(805, 53);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(69, 15);
		this.label63.TabIndex = 174;
		this.label63.Text = "Out Weight";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(302, 23);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(69, 15);
		this.label86.TabIndex = 213;
		this.label86.Text = "Plant Name";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(88, 15);
		this.label16.TabIndex = 134;
		this.label16.Text = "Doc No && Date";
		this.label170.AutoSize = true;
		this.label170.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label170.Location = new System.Drawing.Point(302, 53);
		this.label170.Name = "label170";
		this.label170.Size = new System.Drawing.Size(89, 15);
		this.label170.TabIndex = 213;
		this.label170.Text = "Export Contract";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(805, 82);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(65, 15);
		this.label73.TabIndex = 174;
		this.label73.Text = "Bal Weight";
		this.txtDocNoOutPut.BackColor = System.Drawing.Color.White;
		this.txtDocNoOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOutPut.Location = new System.Drawing.Point(94, 19);
		this.txtDocNoOutPut.Name = "txtDocNoOutPut";
		this.txtDocNoOutPut.ReadOnly = true;
		this.txtDocNoOutPut.Size = new System.Drawing.Size(79, 23);
		this.txtDocNoOutPut.TabIndex = 0;
		this.groupBox4.Controls.Add(this.txtEbTotal);
		this.groupBox4.Controls.Add(this.label165);
		this.groupBox4.Controls.Add(this.txtEbUnit);
		this.groupBox4.Controls.Add(this.label166);
		this.groupBox4.Controls.Add(this.label167);
		this.groupBox4.Controls.Add(this.txtGrossWeight);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotOutPut);
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Controls.Add(this.txtRemarksOutPut);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomOutPut);
		this.groupBox4.Controls.Add(this.txtAmountOutPut);
		this.groupBox4.Controls.Add(this.label70);
		this.groupBox4.Controls.Add(this.lblAmountOUtPUt);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut);
		this.groupBox4.Controls.Add(this.label45);
		this.groupBox4.Controls.Add(this.lblRateUOMOUtPut);
		this.groupBox4.Controls.Add(this.label36);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOutput);
		this.groupBox4.Controls.Add(this.txtRateOutPut);
		this.groupBox4.Controls.Add(this.label69);
		this.groupBox4.Controls.Add(this.lblRateOutPUt);
		this.groupBox4.Controls.Add(this.txtQtyOutPut);
		this.groupBox4.Controls.Add(this.btnAddOutPut);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.btnCancelOutPut);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryType);
		this.groupBox4.Controls.Add(this.btnUpdateDetailOutPut);
		this.groupBox4.Controls.Add(this.label43);
		this.groupBox4.Controls.Add(this.txtNetWeightOutPut);
		this.groupBox4.Controls.Add(this.label11);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(4, 142);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1028, 132);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(342, 75);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 149;
		this.label9.Text = "Remarks";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(248, 72);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 148;
		this.label11.Text = "Net Weight";
		this.txtNetWeightOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightOutPut.Enabled = false;
		this.txtNetWeightOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightOutPut.Location = new System.Drawing.Point(248, 95);
		this.txtNetWeightOutPut.Name = "txtNetWeightOutPut";
		this.txtNetWeightOutPut.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeightOutPut.TabIndex = 11;
		this.txtNetWeightOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightOutPut.TextChanged += new System.EventHandler(txtNetWeightOutPut_TextChanged);
		this.txtNetWeightOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightOutPut_KeyPress);
		this.txtNetWeightOutPut.Leave += new System.EventHandler(txtNetWeightOutPut_Leave);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(120, 17);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(69, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "WareHouse";
		this.btnUpdateDetailOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailOutPut.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailOutPut.Location = new System.Drawing.Point(885, 94);
		this.btnUpdateDetailOutPut.Name = "btnUpdateDetailOutPut";
		this.btnUpdateDetailOutPut.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateDetailOutPut.TabIndex = 17;
		this.btnUpdateDetailOutPut.Text = "Update ";
		this.btnUpdateDetailOutPut.UseVisualStyleBackColor = false;
		this.btnUpdateDetailOutPut.Click += new System.EventHandler(btnUpdateDetailOutPut_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(281, 17);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 135;
		this.label24.Text = "Item Name";
		this.btnCancelOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnCancelOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelOutPut.ForeColor = System.Drawing.Color.White;
		this.btnCancelOutPut.Location = new System.Drawing.Point(955, 94);
		this.btnCancelOutPut.Name = "btnCancelOutPut";
		this.btnCancelOutPut.Size = new System.Drawing.Size(67, 26);
		this.btnCancelOutPut.TabIndex = 18;
		this.btnCancelOutPut.Text = "Cancel";
		this.btnCancelOutPut.UseVisualStyleBackColor = false;
		this.btnCancelOutPut.Click += new System.EventHandler(btnCancelOutPut_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(502, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(63, 15);
		this.label20.TabIndex = 136;
		this.label20.Text = "Pack UOM";
		this.btnAddOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnAddOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddOutPut.ForeColor = System.Drawing.Color.White;
		this.btnAddOutPut.Location = new System.Drawing.Point(886, 94);
		this.btnAddOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddOutPut.Name = "btnAddOutPut";
		this.btnAddOutPut.Size = new System.Drawing.Size(32, 26);
		this.btnAddOutPut.TabIndex = 16;
		this.btnAddOutPut.Text = "+";
		this.btnAddOutPut.UseVisualStyleBackColor = false;
		this.btnAddOutPut.Click += new System.EventHandler(btnAddOutPut_Click);
		this.txtQtyOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOutPut.Location = new System.Drawing.Point(943, 38);
		this.txtQtyOutPut.Name = "txtQtyOutPut";
		this.txtQtyOutPut.Size = new System.Drawing.Size(79, 23);
		this.txtQtyOutPut.TabIndex = 7;
		this.txtQtyOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOutPut.TextChanged += new System.EventHandler(txtQtyOutPut_TextChanged);
		this.txtQtyOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyOutPut_KeyPress);
		this.txtQtyOutPut.Leave += new System.EventHandler(txtQtyOutPut_Leave);
		this.lblRateOutPUt.AutoSize = true;
		this.lblRateOutPUt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateOutPUt.Location = new System.Drawing.Point(642, 72);
		this.lblRateOutPUt.Name = "lblRateOutPUt";
		this.lblRateOutPUt.Size = new System.Drawing.Size(30, 15);
		this.lblRateOutPUt.TabIndex = 155;
		this.lblRateOutPUt.Text = "Rate";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(677, 17);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(46, 15);
		this.label69.TabIndex = 142;
		this.label69.Text = "Job Lot";
		this.txtRateOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateOutPut.Enabled = false;
		this.txtRateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateOutPut.Location = new System.Drawing.Point(642, 95);
		this.txtRateOutPut.Name = "txtRateOutPut";
		this.txtRateOutPut.Size = new System.Drawing.Size(70, 23);
		this.txtRateOutPut.TabIndex = 13;
		this.txtRateOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateOutPut.TextChanged += new System.EventHandler(txtRateOutPut_TextChanged);
		this.txtRateOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateOutPut_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(943, 17);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(54, 15);
		this.label36.TabIndex = 145;
		this.label36.Text = "Item Qty";
		this.lblRateUOMOUtPut.AutoSize = true;
		this.lblRateUOMOUtPut.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateUOMOUtPut.Location = new System.Drawing.Point(714, 72);
		this.lblRateUOMOUtPut.Name = "lblRateUOMOUtPut";
		this.lblRateUOMOUtPut.Size = new System.Drawing.Size(61, 15);
		this.lblRateUOMOUtPut.TabIndex = 157;
		this.lblRateUOMOUtPut.Text = "Rate UOM";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(837, 17);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(60, 15);
		this.label45.TabIndex = 143;
		this.label45.Text = "Pack Type";
		this.lblAmountOUtPUt.AutoSize = true;
		this.lblAmountOUtPUt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountOUtPUt.Location = new System.Drawing.Point(792, 72);
		this.lblAmountOUtPUt.Name = "lblAmountOUtPUt";
		this.lblAmountOUtPUt.Size = new System.Drawing.Size(51, 15);
		this.lblAmountOUtPUt.TabIndex = 159;
		this.lblAmountOUtPUt.Text = "Amount";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(584, 17);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(57, 15);
		this.label70.TabIndex = 141;
		this.label70.Text = "Crop Year";
		this.txtAmountOutPut.BackColor = System.Drawing.Color.White;
		this.txtAmountOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOutPut.Location = new System.Drawing.Point(792, 95);
		this.txtAmountOutPut.Name = "txtAmountOutPut";
		this.txtAmountOutPut.ReadOnly = true;
		this.txtAmountOutPut.Size = new System.Drawing.Size(92, 23);
		this.txtAmountOutPut.TabIndex = 15;
		this.txtAmountOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarksOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOutPut.Location = new System.Drawing.Point(342, 95);
		this.txtRemarksOutPut.Name = "txtRemarksOutPut";
		this.txtRemarksOutPut.Size = new System.Drawing.Size(298, 23);
		this.txtRemarksOutPut.TabIndex = 12;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 147;
		this.label13.Text = "Entry Type";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(4, 95);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(85, 23);
		this.txtGrossWeight.TabIndex = 8;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label167.AutoSize = true;
		this.label167.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label167.Location = new System.Drawing.Point(4, 75);
		this.label167.Name = "label167";
		this.label167.Size = new System.Drawing.Size(78, 15);
		this.label167.TabIndex = 163;
		this.label167.Text = "Gross Weight";
		this.label166.AutoSize = true;
		this.label166.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label166.Location = new System.Drawing.Point(90, 75);
		this.label166.Name = "label166";
		this.label166.Size = new System.Drawing.Size(42, 15);
		this.label166.TabIndex = 164;
		this.label166.Text = "EbUnit";
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(90, 95);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(70, 23);
		this.txtEbUnit.TabIndex = 9;
		this.txtEbUnit.Tag = "EbUnit";
		this.txtEbUnit.Text = "0";
		this.txtEbUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnit_KeyPress);
		this.label165.AutoSize = true;
		this.label165.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label165.Location = new System.Drawing.Point(161, 75);
		this.label165.Name = "label165";
		this.label165.Size = new System.Drawing.Size(46, 15);
		this.label165.TabIndex = 165;
		this.label165.Text = "EbTotal";
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(161, 95);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotal.TabIndex = 10;
		this.txtEbTotal.Tag = "EbTotal";
		this.txtEbTotal.Text = "0";
		this.txtEbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotal.TextChanged += new System.EventHandler(txtEbTotal_TextChanged);
		this.txtEbTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotal_KeyPress);
		this.panel64.BackColor = System.Drawing.Color.Teal;
		this.panel64.Controls.Add(this.label32);
		this.panel64.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel64.Location = new System.Drawing.Point(0, 0);
		this.panel64.Name = "panel64";
		this.panel64.Size = new System.Drawing.Size(1348, 29);
		this.panel64.TabIndex = 180;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(208, 19);
		this.label32.TabIndex = 210;
		this.label32.Text = "Production  Output         ";
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label15);
		this.panel22.Controls.Add(this.ctrlGrdBar7);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 307);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1348, 26);
		this.panel22.TabIndex = 23;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1307, 2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdByProduct;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 173;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(119, 19);
		this.label15.TabIndex = 174;
		this.label15.Text = "Output Detail";
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 333);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1348, 244);
		this.panel24.TabIndex = 24;
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProduct.FilterMode = (FilterMode)2;
		this.grdByProduct.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1348, 244);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellEdited += new ColumnActionEventHandler(grdByProduct_CellEdited);
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		this.InPut.BackColor = System.Drawing.Color.White;
		this.InPut.Controls.Add(this.tabControl2);
		this.InPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.InPut.Location = new System.Drawing.Point(4, 26);
		this.InPut.Name = "InPut";
		this.InPut.Padding = new System.Windows.Forms.Padding(3);
		this.InPut.Size = new System.Drawing.Size(1362, 609);
		this.InPut.TabIndex = 0;
		this.InPut.Text = "Input";
		this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(3, 3);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1356, 603);
		this.tabControl2.TabIndex = 4;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdinputhistory);
		this.tabPage4.Controls.Add(this.panel78);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputDetail);
		this.tabPage4.Controls.Add(this.panel59);
		this.tabPage4.Controls.Add(this.panel58);
		this.tabPage4.Controls.Add(this.panel57);
		this.tabPage4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.tabPage4.Location = new System.Drawing.Point(4, 4);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1348, 574);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "History";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel57.BackColor = System.Drawing.Color.Teal;
		this.panel57.Controls.Add(this.label124);
		this.panel57.Controls.Add(this.toolStrip7);
		this.panel57.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel57.Location = new System.Drawing.Point(3, 3);
		this.panel57.Name = "panel57";
		this.panel57.Size = new System.Drawing.Size(1342, 52);
		this.panel57.TabIndex = 6;
		this.toolStrip7.BackColor = System.Drawing.Color.White;
		this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton4, this.BtnRefereshInputHistory });
		this.toolStrip7.Location = new System.Drawing.Point(0, 0);
		this.toolStrip7.Name = "toolStrip7";
		this.toolStrip7.Size = new System.Drawing.Size(1342, 27);
		this.toolStrip7.TabIndex = 4;
		this.toolStrip7.Text = "toolStrip7";
		this.toolStripButton4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton4.ForeColor = System.Drawing.Color.Black;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton4.Text = "&New";
		this.toolStripButton4.Click += new System.EventHandler(BtnNewInputHistory_Click);
		this.BtnRefereshInputHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefereshInputHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefereshInputHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshInputHistory.Name = "BtnRefereshInputHistory";
		this.BtnRefereshInputHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshInputHistory.Text = "Refresh";
		this.BtnRefereshInputHistory.Click += new System.EventHandler(BtnRefereshInputHistory_Click);
		this.label124.AutoSize = true;
		this.label124.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label124.ForeColor = System.Drawing.Color.White;
		this.label124.Location = new System.Drawing.Point(2, 30);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(209, 19);
		this.label124.TabIndex = 211;
		this.label124.Text = "Production Input History";
		this.panel58.Controls.Add(this.groupBox11);
		this.panel58.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel58.Location = new System.Drawing.Point(3, 55);
		this.panel58.Name = "panel58";
		this.panel58.Size = new System.Drawing.Size(1342, 50);
		this.panel58.TabIndex = 0;
		this.groupBox11.Controls.Add(this.BtnShowInputHistory);
		this.groupBox11.Controls.Add(this.label145);
		this.groupBox11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory);
		this.groupBox11.Controls.Add(this.txtToDateHistory);
		this.groupBox11.Controls.Add(this.label146);
		this.groupBox11.Controls.Add(this.label147);
		this.groupBox11.Controls.Add(this.txtToDocNoHistory);
		this.groupBox11.Controls.Add(this.txtFromdateHistory);
		this.groupBox11.Controls.Add(this.label148);
		this.groupBox11.Controls.Add(this.label149);
		this.groupBox11.Controls.Add(this.txtFromNoHistory);
		this.groupBox11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox11.Location = new System.Drawing.Point(6, 2);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Size = new System.Drawing.Size(1038, 46);
		this.groupBox11.TabIndex = 1;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "Filters";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(490, 16);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(429, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(60, 15);
		this.label149.TabIndex = 173;
		this.label149.Text = "Doc From";
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(564, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(45, 15);
		this.label148.TabIndex = 173;
		this.label148.Text = "Doc To";
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(609, 16);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(2, 20);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 171;
		this.label147.Text = "Date From";
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(217, 20);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(48, 15);
		this.label146.TabIndex = 171;
		this.label146.Text = "Date To";
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(268, 16);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(148, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(683, 20);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(59, 15);
		this.label145.TabIndex = 178;
		this.label145.Text = "Job Order";
		this.BtnShowInputHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowInputHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowInputHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowInputHistory.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.BtnShowInputHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowInputHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowInputHistory.Location = new System.Drawing.Point(969, 15);
		this.BtnShowInputHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowInputHistory.Name = "BtnShowInputHistory";
		this.BtnShowInputHistory.Size = new System.Drawing.Size(61, 25);
		this.BtnShowInputHistory.TabIndex = 5;
		this.BtnShowInputHistory.Text = "Show";
		this.BtnShowInputHistory.UseVisualStyleBackColor = false;
		this.BtnShowInputHistory.Click += new System.EventHandler(BtnShowInputHistory_Click);
		this.panel59.BackColor = System.Drawing.Color.Teal;
		this.panel59.Controls.Add(this.label127);
		this.panel59.Controls.Add(this.ctrlGrdBar10);
		this.panel59.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel59.Location = new System.Drawing.Point(3, 105);
		this.panel59.Name = "panel59";
		this.panel59.Size = new System.Drawing.Size(1342, 24);
		this.panel59.TabIndex = 7;
		this.ctrlGrdBar10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar10.FormName = this;
		this.ctrlGrdBar10.Location = new System.Drawing.Point(1295, 1);
		this.ctrlGrdBar10.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar10.MyGrid = this.grdinputhistory;
		this.ctrlGrdBar10.Name = "ctrlGrdBar10";
		this.ctrlGrdBar10.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar10.TabIndex = 174;
		this.ctrlGrdBar10.Load += new System.EventHandler(ctrlGrdBar10_Load);
		this.label127.AutoSize = true;
		this.label127.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label127.ForeColor = System.Drawing.Color.White;
		this.label127.Location = new System.Drawing.Point(3, 3);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(40, 17);
		this.label127.TabIndex = 211;
		this.label127.Text = "Main";
		this.grdInputDetail.AllowEdit = (InheritableBoolean)2;
		this.grdInputDetail.AlternatingColors = true;
		((ScrollableBase)this.grdInputDetail).BackColor = System.Drawing.Color.White;
		this.grdInputDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdInputDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputDetail.DesignTimeLayout = grdInputDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdInputDetail.FilterMode = (FilterMode)2;
		this.grdInputDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputDetail.GroupByBoxVisible = false;
		this.grdInputDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Location = new System.Drawing.Point(3, 378);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Name = "grdInputDetail";
		((ScrollableBase)this.grdInputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1342, 193);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 2;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		this.panel78.BackColor = System.Drawing.Color.Teal;
		this.panel78.Controls.Add(this.label130);
		this.panel78.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel78.Location = new System.Drawing.Point(3, 358);
		this.panel78.Name = "panel78";
		this.panel78.Size = new System.Drawing.Size(1342, 20);
		this.panel78.TabIndex = 11;
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label130.ForeColor = System.Drawing.Color.White;
		this.label130.Location = new System.Drawing.Point(3, 1);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(230, 17);
		this.label130.TabIndex = 211;
		this.label130.Text = "Detail of the Selected above row";
		this.grdinputhistory.AllowEdit = (InheritableBoolean)2;
		this.grdinputhistory.AlternatingColors = true;
		((ScrollableBase)this.grdinputhistory).BackColor = System.Drawing.Color.White;
		this.grdinputhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdinputhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdinputhistory.DesignTimeLayout = grdinputhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdinputhistory.FilterMode = (FilterMode)2;
		this.grdinputhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdinputhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Location = new System.Drawing.Point(3, 129);
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Name = "grdinputhistory";
		((ScrollableBase)this.grdinputhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Size = new System.Drawing.Size(1342, 229);
		((System.Windows.Forms.Control)(object)this.grdinputhistory).TabIndex = 1;
		this.grdinputhistory.TotalRow = (InheritableBoolean)1;
		this.grdinputhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdinputhistory).VisualStyle = (VisualStyle)4;
		this.grdinputhistory.ColumnButtonClick += new ColumnActionEventHandler(grdinputhistory_ColumnButtonClick);
		this.grdinputhistory.SelectionChanged += new System.EventHandler(grdinputhistory_SelectionChanged);
		this.tabPage1.Controls.Add(this.panel28);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1348, 574);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnVoucherReport, this.btnJobOrderDefine, this.btnPendingForRateInput });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1342, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "601-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnJobOrderDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnJobOrderDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnJobOrderDefine.Name = "btnJobOrderDefine";
		this.btnJobOrderDefine.Size = new System.Drawing.Size(120, 24);
		this.btnJobOrderDefine.Text = "Define Job Order";
		this.btnJobOrderDefine.Click += new System.EventHandler(btnJobOrderDefine_Click);
		this.btnPendingForRateInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForRateInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPendingForRateInput.Name = "btnPendingForRateInput";
		this.btnPendingForRateInput.Size = new System.Drawing.Size(135, 24);
		this.btnPendingForRateInput.Text = "Pending For Rates ";
		this.btnPendingForRateInput.Click += new System.EventHandler(btnPendingForRateInput_Click);
		this.panel28.Controls.Add(this.panel5);
		this.panel28.Controls.Add(this.PanelInPutManualEntry);
		this.panel28.Controls.Add(this.panel72);
		this.panel28.Controls.Add(this.panel63);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(3, 30);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1342, 541);
		this.panel28.TabIndex = 174;
		this.panel63.BackColor = System.Drawing.Color.Teal;
		this.panel63.Controls.Add(this.label30);
		this.panel63.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel63.Location = new System.Drawing.Point(0, 0);
		this.panel63.Name = "panel63";
		this.panel63.Size = new System.Drawing.Size(1342, 29);
		this.panel63.TabIndex = 2;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(6, 5);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(206, 19);
		this.label30.TabIndex = 210;
		this.label30.Text = "Production Input            ";
		this.panel72.Controls.Add(this.groupBox1);
		this.panel72.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel72.Location = new System.Drawing.Point(0, 29);
		this.panel72.Name = "panel72";
		this.panel72.Size = new System.Drawing.Size(1342, 114);
		this.panel72.TabIndex = 3;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeeder);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.label83);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionDepartment);
		this.groupBox1.Controls.Add(this.label112);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPItem);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.groupBox1.Controls.Add(this.btnLoadInvoices);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1024, 106);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(665, 45);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(273, 55);
		this.txtRemarks.TabIndex = 7;
		this.label6.BackColor = System.Drawing.Color.LightGray;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(665, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(273, 26);
		this.label6.TabIndex = 6;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnLoadInvoices.BackColor = System.Drawing.Color.Teal;
		this.btnLoadInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadInvoices.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadInvoices.ForeColor = System.Drawing.Color.White;
		this.btnLoadInvoices.Location = new System.Drawing.Point(944, 51);
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(74, 49);
		this.btnLoadInvoices.TabIndex = 8;
		this.btnLoadInvoices.Text = "Load InPut";
		this.btnLoadInvoices.UseVisualStyleBackColor = false;
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 51);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 151;
		this.label23.Text = "Job Order No";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(351, 51);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(58, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "WIP Item";
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(351, 80);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(94, 15);
		this.label112.TabIndex = 173;
		this.label112.Text = "Pro_Department";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(351, 22);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 15);
		this.label27.TabIndex = 171;
		this.label27.Text = "WIP Account";
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(175, 18);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(169, 23);
		this.txtDocdate.TabIndex = 1;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(4, 81);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(69, 15);
		this.label83.TabIndex = 209;
		this.label83.Text = "Plant Name";
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(92, 18);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(77, 23);
		this.txtdocnumber.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Doc No && Date";
		this.PanelInPutManualEntry.Controls.Add(this.groupBox2);
		this.PanelInPutManualEntry.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelInPutManualEntry.Location = new System.Drawing.Point(0, 143);
		this.PanelInPutManualEntry.Name = "PanelInPutManualEntry";
		this.PanelInPutManualEntry.Size = new System.Drawing.Size(1342, 141);
		this.PanelInPutManualEntry.TabIndex = 4;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseInput);
		this.groupBox2.Controls.Add(this.label82);
		this.groupBox2.Controls.Add(this.label64);
		this.groupBox2.Controls.Add(this.txtQtyInput);
		this.groupBox2.Controls.Add(this.txtBalanceWeight);
		this.groupBox2.Controls.Add(this.txtNetWeightInput);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUomInput);
		this.groupBox2.Controls.Add(this.label81);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLotInput);
		this.groupBox2.Controls.Add(this.txtRemarksInput);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemInput);
		this.groupBox2.Controls.Add(this.txtAmountInput);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYearInput);
		this.groupBox2.Controls.Add(this.lblAmountInput);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateuomInput);
		this.groupBox2.Controls.Add(this.label78);
		this.groupBox2.Controls.Add(this.lblRateUom);
		this.groupBox2.Controls.Add(this.label65);
		this.groupBox2.Controls.Add(this.txtRateInput);
		this.groupBox2.Controls.Add(this.label74);
		this.groupBox2.Controls.Add(this.lblRateInput);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput);
		this.groupBox2.Controls.Add(this.btnAddInput);
		this.groupBox2.Controls.Add(this.txtBalanceQty);
		this.groupBox2.Controls.Add(this.btnCancelInput);
		this.groupBox2.Controls.Add(this.label66);
		this.groupBox2.Controls.Add(this.btnUpdateDetailInput);
		this.groupBox2.Controls.Add(this.label72);
		this.groupBox2.Controls.Add(this.label80);
		this.groupBox2.Controls.Add(this.label67);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(7, 10);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1024, 127);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(6, 22);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(69, 15);
		this.label67.TabIndex = 193;
		this.label67.Text = "WareHouse";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label80.Location = new System.Drawing.Point(189, 73);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(52, 15);
		this.label80.TabIndex = 200;
		this.label80.Text = "Remarks";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(845, 22);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(70, 15);
		this.label72.TabIndex = 197;
		this.label72.Text = "Balance Qty";
		this.btnUpdateDetailInput.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailInput.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailInput.Location = new System.Drawing.Point(868, 91);
		this.btnUpdateDetailInput.Name = "btnUpdateDetailInput";
		this.btnUpdateDetailInput.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetailInput.TabIndex = 15;
		this.btnUpdateDetailInput.Text = "Update ";
		this.btnUpdateDetailInput.UseVisualStyleBackColor = false;
		this.btnUpdateDetailInput.Click += new System.EventHandler(btnUpdateDetailInput_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(738, 22);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(60, 15);
		this.label66.TabIndex = 196;
		this.label66.Text = "Pack Type";
		this.btnCancelInput.BackColor = System.Drawing.Color.Teal;
		this.btnCancelInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelInput.ForeColor = System.Drawing.Color.White;
		this.btnCancelInput.Location = new System.Drawing.Point(939, 91);
		this.btnCancelInput.Name = "btnCancelInput";
		this.btnCancelInput.Size = new System.Drawing.Size(67, 27);
		this.btnCancelInput.TabIndex = 16;
		this.btnCancelInput.Text = "Cancel";
		this.btnCancelInput.UseVisualStyleBackColor = false;
		this.btnCancelInput.Click += new System.EventHandler(btnCancelInput_Click);
		this.txtBalanceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQty.Location = new System.Drawing.Point(845, 42);
		this.txtBalanceQty.Name = "txtBalanceQty";
		this.txtBalanceQty.ReadOnly = true;
		this.txtBalanceQty.Size = new System.Drawing.Size(69, 23);
		this.txtBalanceQty.TabIndex = 6;
		this.txtBalanceQty.TabStop = false;
		this.txtBalanceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.btnAddInput.BackColor = System.Drawing.Color.Teal;
		this.btnAddInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddInput.ForeColor = System.Drawing.Color.White;
		this.btnAddInput.Location = new System.Drawing.Point(827, 91);
		this.btnAddInput.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddInput.Name = "btnAddInput";
		this.btnAddInput.Size = new System.Drawing.Size(41, 27);
		this.btnAddInput.TabIndex = 14;
		this.btnAddInput.Text = "+";
		this.btnAddInput.UseVisualStyleBackColor = false;
		this.btnAddInput.Click += new System.EventHandler(btnAddInput_Click);
		this.lblRateInput.AutoSize = true;
		this.lblRateInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateInput.Location = new System.Drawing.Point(550, 73);
		this.lblRateInput.Name = "lblRateInput";
		this.lblRateInput.Size = new System.Drawing.Size(90, 15);
		this.lblRateInput.TabIndex = 201;
		this.lblRateInput.Text = "Avg Rate(40Kg)";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label74.Location = new System.Drawing.Point(180, 22);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(67, 15);
		this.label74.TabIndex = 191;
		this.label74.Text = "Item Name";
		this.txtRateInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateInput.Location = new System.Drawing.Point(567, 93);
		this.txtRateInput.Name = "txtRateInput";
		this.txtRateInput.Size = new System.Drawing.Size(70, 23);
		this.txtRateInput.TabIndex = 11;
		this.txtRateInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateInput.TextChanged += new System.EventHandler(txtRateInput_TextChanged);
		this.txtRateInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateInput_KeyPress_1);
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(574, 22);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(46, 15);
		this.label65.TabIndex = 195;
		this.label65.Text = "Job Lot";
		this.lblRateUom.AutoSize = true;
		this.lblRateUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateUom.Location = new System.Drawing.Point(640, 73);
		this.lblRateUom.Name = "lblRateUom";
		this.lblRateUom.Size = new System.Drawing.Size(61, 15);
		this.lblRateUom.TabIndex = 202;
		this.lblRateUom.Text = "Rate UOM";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label78.Location = new System.Drawing.Point(391, 22);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(63, 15);
		this.label78.TabIndex = 192;
		this.label78.Text = "Pack UOM";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(472, 22);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(57, 15);
		this.label61.TabIndex = 194;
		this.label61.Text = "Crop Year";
		this.lblAmountInput.AutoSize = true;
		this.lblAmountInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAmountInput.Location = new System.Drawing.Point(729, 73);
		this.lblAmountInput.Name = "lblAmountInput";
		this.lblAmountInput.Size = new System.Drawing.Size(51, 15);
		this.lblAmountInput.TabIndex = 203;
		this.lblAmountInput.Text = "Amount";
		this.txtAmountInput.BackColor = System.Drawing.Color.White;
		this.txtAmountInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountInput.Location = new System.Drawing.Point(728, 93);
		this.txtAmountInput.Name = "txtAmountInput";
		this.txtAmountInput.ReadOnly = true;
		this.txtAmountInput.Size = new System.Drawing.Size(98, 23);
		this.txtAmountInput.TabIndex = 13;
		this.txtAmountInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountInput_KeyPress_1);
		this.txtRemarksInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksInput.Location = new System.Drawing.Point(184, 93);
		this.txtRemarksInput.Name = "txtRemarksInput";
		this.txtRemarksInput.Size = new System.Drawing.Size(380, 23);
		this.txtRemarksInput.TabIndex = 10;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label81.Location = new System.Drawing.Point(92, 73);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(68, 15);
		this.label81.TabIndex = 207;
		this.label81.Text = "Net Weight";
		this.txtNetWeightInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightInput.Location = new System.Drawing.Point(89, 93);
		this.txtNetWeightInput.Name = "txtNetWeightInput";
		this.txtNetWeightInput.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeightInput.TabIndex = 9;
		this.txtNetWeightInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightInput.TextChanged += new System.EventHandler(txtNetWeightInput_TextChanged);
		this.txtNetWeightInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightInput_KeyPress);
		this.txtNetWeightInput.Leave += new System.EventHandler(txtNetWeightInput_Leave);
		this.txtBalanceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeight.Location = new System.Drawing.Point(915, 42);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.ReadOnly = true;
		this.txtBalanceWeight.Size = new System.Drawing.Size(93, 23);
		this.txtBalanceWeight.TabIndex = 7;
		this.txtBalanceWeight.TabStop = false;
		this.txtBalanceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceWeight_KeyPress);
		this.txtQtyInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyInput.Location = new System.Drawing.Point(7, 93);
		this.txtQtyInput.Name = "txtQtyInput";
		this.txtQtyInput.Size = new System.Drawing.Size(81, 23);
		this.txtQtyInput.TabIndex = 8;
		this.txtQtyInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyInput.TextChanged += new System.EventHandler(txtQtyInput_TextChanged);
		this.txtQtyInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInput_KeyPress);
		this.txtQtyInput.Leave += new System.EventHandler(txtQtyInput_Leave);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(7, 73);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(53, 15);
		this.label64.TabIndex = 206;
		this.label64.Text = "Quantity";
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label82.Location = new System.Drawing.Point(915, 22);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(90, 15);
		this.label82.TabIndex = 199;
		this.label82.Text = "Balance Weight";
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 284);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1342, 257);
		this.panel5.TabIndex = 3;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1342, 28);
		this.panel12.TabIndex = 1;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1302, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdStockConversionProductionDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(107, 19);
		this.label2.TabIndex = 172;
		this.label2.Text = "Input Detail";
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 28);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1342, 229);
		this.panel14.TabIndex = 2;
		((ScrollableBase)this.grdStockConversionProductionDetail).BackColor = System.Drawing.Color.White;
		grdStockConversionProductionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStockConversionProductionDetail.DesignTimeLayout = grdStockConversionProductionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdStockConversionProductionDetail.FilterMode = (FilterMode)2;
		this.grdStockConversionProductionDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStockConversionProductionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Name = "grdStockConversionProductionDetail";
		((ScrollableBase)this.grdStockConversionProductionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Size = new System.Drawing.Size(1342, 229);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).TabIndex = 0;
		this.grdStockConversionProductionDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockConversionProductionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).VisualStyle = (VisualStyle)4;
		this.grdStockConversionProductionDetail.CellUpdated += new ColumnActionEventHandler(grdStockConversionProductionDetail_CellUpdated);
		this.grdStockConversionProductionDetail.ColumnButtonClick += new ColumnActionEventHandler(grdStockConversionProductionDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).DoubleClick += new System.EventHandler(grdStockConversionProductionDetail_DoubleClick);
		this.tabControl1.Controls.Add(this.InPut);
		this.tabControl1.Controls.Add(this.OutPut);
		this.tabControl1.Controls.Add(this.Consumption);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 639);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.toolStripConsumptionHistory.BackColor = System.Drawing.Color.White;
		this.toolStripConsumptionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripConsumptionHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripConsumptionHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistoryConsumption, this.btnRefreshConsupmtionHistory });
		this.toolStripConsumptionHistory.Location = new System.Drawing.Point(0, 0);
		this.toolStripConsumptionHistory.Name = "toolStripConsumptionHistory";
		this.toolStripConsumptionHistory.Size = new System.Drawing.Size(1348, 27);
		this.toolStripConsumptionHistory.TabIndex = 4;
		this.toolStripConsumptionHistory.Text = "toolStrip9";
		this.CmbJobOrderNoConsupmtionHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoConsupmtionHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoConsupmtionHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoConsupmtionHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance394).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance395).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance395).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance395).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance396;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance397).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance397).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance397).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance397;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance398).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance398).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance398;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance399).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance399;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance400).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance400;
		((AppearanceBase)appearance401).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance401).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance402).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance402).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance402).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance402).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance402;
		((AppearanceBase)appearance404).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance404;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance405).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance405;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance406).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance406;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoConsupmtionHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Name = "CmbJobOrderNoConsupmtionHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).TabIndex = 4;
		this.cmbJobOrderConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).Location = new System.Drawing.Point(92, 45);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).Name = "cmbJobOrderConsumption";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderConsumption).Leave += new System.EventHandler(cmbJobOrderConsumption_Leave);
		this.cmbWIPItemConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbWIPItemConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWIPItemConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWIPItemConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance455).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance455).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance455;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance456).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance456).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance456).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance456).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance456;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance3;
		((SpecialBoxBase)((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWIPItemConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).Location = new System.Drawing.Point(450, 45);
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).Name = "cmbWIPItemConsumption";
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbWIPItemConsumption).TabStop = false;
		this.CmbDepartmentConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbDepartmentConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepartmentConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepartmentConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepartmentConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepartmentConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption).Location = new System.Drawing.Point(450, 74);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption).Name = "CmbDepartmentConsumption";
		((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentConsumption).TabIndex = 5;
		this.cmbWIPAcConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbWIPAcConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWIPAcConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWIPAcConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWIPAcConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWIPAcConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).Location = new System.Drawing.Point(450, 16);
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).Name = "cmbWIPAcConsumption";
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbWIPAcConsumption).TabStop = false;
		this.CmbPlantConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantConsumption).Location = new System.Drawing.Point(92, 75);
		((System.Windows.Forms.Control)(object)this.CmbPlantConsumption).Name = "CmbPlantConsumption";
		((System.Windows.Forms.Control)(object)this.CmbPlantConsumption).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantConsumption).TabIndex = 3;
		this.CmbPackingTypeConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance454).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance454).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance454;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).Location = new System.Drawing.Point(738, 40);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).Name = "CmbPackingTypeConsumption";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeConsumption).Leave += new System.EventHandler(CmbPackingTypeConsumption_Leave);
		this.CmbRateUomConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance441).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance441).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance441;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance442).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance442).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance442).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance442;
		((AppearanceBase)appearance443).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance443;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance444).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance444).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance444).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance444).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance444;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance445).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance445).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance445;
		((AppearanceBase)appearance447).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance447).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance447;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance448).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance448;
		((AppearanceBase)appearance449).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance449).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance449;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance450).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance450).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance450).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance450).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance450).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance450;
		((AppearanceBase)appearance451).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance451;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance452).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance452).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance452;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance453).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance453;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).Location = new System.Drawing.Point(643, 91);
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).Name = "CmbRateUomConsumption";
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbRateUomConsumption).TextChanged += new System.EventHandler(CmbRateUomConsumption_TextChanged);
		this.CmbCropYearConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance403).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance403;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance414).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance414).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance414).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance414;
		((AppearanceBase)appearance422).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance422;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance431).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance431).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance431).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance431;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance432).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance432).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance432;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance433).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance433;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance434;
		((AppearanceBase)appearance435).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance435).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance435;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance436).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance436).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance436).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance436).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance436).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance436;
		((AppearanceBase)appearance437).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance437;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance439).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance439).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance439;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance440).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance440;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).Location = new System.Drawing.Point(473, 40);
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).Name = "CmbCropYearConsumption";
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCropYearConsumption).Leave += new System.EventHandler(CmbCropYearConsumption_Leave);
		this.CmbItemNameConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance365).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance365).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance365).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance366;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance291).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance291).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance302).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance367;
		((AppearanceBase)appearance334).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance334).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance346).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance346).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance346).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance357).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance392).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance392;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).Location = new System.Drawing.Point(183, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).Name = "CmbItemNameConsumption";
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemNameConsumption).Leave += new System.EventHandler(CmbItemNameConsumption_Leave);
		this.CmbJobLotConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance413).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance415).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance415).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance415).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance415;
		((AppearanceBase)appearance416).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance416;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance279).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance361).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).Location = new System.Drawing.Point(576, 40);
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).Name = "CmbJobLotConsumption";
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbJobLotConsumption).Leave += new System.EventHandler(CmbJobLotConsumption_Leave);
		this.cmbPackUomConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUomConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUomConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUomConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance407).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance407;
		((AppearanceBase)appearance408).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance408).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance408;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance409).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance409).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance409).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance409).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance409).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance409;
		((AppearanceBase)appearance410).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance410;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance411).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance412;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUomConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).Location = new System.Drawing.Point(391, 40);
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).Name = "cmbPackUomConsumption";
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPackUomConsumption).TextChanged += new System.EventHandler(cmbPackUomConsumption_TextChanged);
		this.cmbWarehouseConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbWarehouseConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWarehouseConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWarehouseConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance345).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance345;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance430).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance430).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance430).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance430).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance430;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance438).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance438;
		((AppearanceBase)appearance446).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance446).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance446;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWarehouseConsumption.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).Location = new System.Drawing.Point(6, 40);
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).Name = "cmbWarehouseConsumption";
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbWarehouseConsumption).Leave += new System.EventHandler(cmbWarehouseConsumption_Leave);
		this.CmbJobOrderOutPuthistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderOutPuthistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderOutPuthistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderOutPuthistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance381).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance381).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance381).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance381;
		((AppearanceBase)appearance382).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance382;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance383).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance383).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance383;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance384).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance384;
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance385).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance386;
		((AppearanceBase)appearance387).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance387).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance388).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance388).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance388).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance388).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance388;
		((AppearanceBase)appearance389).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance389;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance390;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderOutPuthistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Name = "CmbJobOrderOutPuthistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).TabIndex = 4;
		this.CmbJobOrderNoOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance247;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Location = new System.Drawing.Point(94, 47);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Name = "CmbJobOrderNoOutput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Leave += new System.EventHandler(CmbJobOrderNoOutput_Leave);
		this.CmbPlantFeederOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance417).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance418).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance418).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance418).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance418;
		((AppearanceBase)appearance419).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance419;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance420).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance420).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance420;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance421).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance421).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance421;
		((AppearanceBase)appearance423).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance423).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance423;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance424).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance424;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance425).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance425;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance426).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance426).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance426).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance426).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance426;
		((AppearanceBase)appearance427).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance427;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance428).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance429;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Location = new System.Drawing.Point(390, 17);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Name = "CmbPlantFeederOutput";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Leave += new System.EventHandler(CmbPlantFeederOutput_Leave);
		this.CmbExportContractSchedule.AllowNull = (DefaultableBoolean)1;
		this.CmbExportContractSchedule.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExportContractSchedule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExportContractSchedule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance229;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance237).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExportContractSchedule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Location = new System.Drawing.Point(390, 47);
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Name = "CmbExportContractSchedule";
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Leave += new System.EventHandler(CmbPlantFeederOutput_Leave);
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance222).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Name = "CmbEntryType";
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).TabIndex = 0;
		this.CmbEntryType.ValueChanged += new System.EventHandler(CmbEntryType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Leave += new System.EventHandler(CmbEntryType_Leave);
		this.CmbCropYearOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance199;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Location = new System.Drawing.Point(584, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Name = "CmbCropYearOutPut";
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Size = new System.Drawing.Size(92, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Leave += new System.EventHandler(CmbCropYearOutPut_Leave);
		this.CmbItemOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance189).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance190).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Location = new System.Drawing.Point(281, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Name = "CmbItemOutput";
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Leave += new System.EventHandler(CmbItemOutput_Leave);
		this.CmbPackTypeOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbPackTypeOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackTypeOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackTypeOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackTypeOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Location = new System.Drawing.Point(837, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Name = "CmbPackTypeOutPut";
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).TabIndex = 6;
		this.CmbRateUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Location = new System.Drawing.Point(714, 93);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Name = "CmbRateUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Leave += new System.EventHandler(CmbRateUomOutPut_Leave);
		this.CmbWarehouseOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Location = new System.Drawing.Point(120, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Name = "CmbWarehouseOutput";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).TabIndex = 1;
		this.CmbUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Location = new System.Drawing.Point(502, 36);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Name = "CmbUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Leave += new System.EventHandler(CmbUomOutPut_Leave);
		this.CmbLotOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLotOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLotOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Location = new System.Drawing.Point(677, 36);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Name = "CmbLotOutPut";
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).TabIndex = 5;
		this.CmbJobOrderInputHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderInputHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderInputHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderInputHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance369).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance369).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance369).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance369;
		((AppearanceBase)appearance370).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance370;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance371).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance371).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance371).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance372).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance372;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance373).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance374;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance375).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance376).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance376).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance376).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance376).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance377).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance378).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance378;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderInputHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Name = "CmbJobOrderInputHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).TabIndex = 4;
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance348).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance349).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance349).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance349).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance349;
		((AppearanceBase)appearance350).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance350;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance351).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance351).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance351).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance352).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance352;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance353).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance354;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance355).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance356).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance356).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance356).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance356;
		((AppearanceBase)appearance358).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance359).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(92, 45);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.CmbWIPItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance335).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance335).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance335).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance336;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance337;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance341).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance342).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance342).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance342).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Location = new System.Drawing.Point(450, 45);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Name = "CmbWIPItem";
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabStop = false;
		this.CmbProductionDepartment.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance322).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance322).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance324).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance324).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance326).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance327;
		((AppearanceBase)appearance328).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance328).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance329).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance329).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance329).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance329;
		((AppearanceBase)appearance330).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Location = new System.Drawing.Point(450, 74);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Name = "CmbProductionDepartment";
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).TabIndex = 6;
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance309).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance309).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance310;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance316).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance317).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance317).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance317).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(450, 16);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabStop = false;
		this.CmbPlantFeeder.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeeder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeeder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeeder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance297;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance305).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeeder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Location = new System.Drawing.Point(92, 75);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Name = "CmbPlantFeeder";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).TabIndex = 3;
		this.cmbPackingTypeInput.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingTypeInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingTypeInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingTypeInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance284;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance289).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance292).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingTypeInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Location = new System.Drawing.Point(737, 40);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Name = "cmbPackingTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Size = new System.Drawing.Size(107, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Leave += new System.EventHandler(cmbPackingTypeInput_Leave);
		this.cmbRateuomInput.AllowNull = (DefaultableBoolean)1;
		this.cmbRateuomInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateuomInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateuomInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance266;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance272).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRateuomInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Location = new System.Drawing.Point(640, 91);
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Name = "cmbRateuomInput";
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).TextChanged += new System.EventHandler(cmbRateuomInput_TextChanged);
		this.cmbCropYearInput.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance233;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance256).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance261).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYearInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Location = new System.Drawing.Point(472, 40);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Name = "cmbCropYearInput";
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Leave += new System.EventHandler(cmbCropYearInput_Leave);
		this.cmbItemInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance219).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(183, 40);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TextChanged += new System.EventHandler(cmbItemInput_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Leave += new System.EventHandler(cmbItemInput_Leave);
		this.cmbLotInput.AllowNull = (DefaultableBoolean)1;
		this.cmbLotInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLotInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLotInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLotInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Location = new System.Drawing.Point(574, 40);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Name = "cmbLotInput";
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Leave += new System.EventHandler(cmbLotInput_Leave);
		this.cmbItemUomInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUomInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUomInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUomInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance120).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUomInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Location = new System.Drawing.Point(391, 40);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Name = "cmbItemUomInput";
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).TextChanged += new System.EventHandler(cmbItemUomInput_TextChanged);
		this.cmbWareHouseInput.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouseInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Location = new System.Drawing.Point(6, 40);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Name = "cmbWareHouseInput";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).TabIndex = 0;
		this.cmbWareHouseInput.ValueChanged += new System.EventHandler(cmbWareHouseInput_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Leave += new System.EventHandler(cmbWareHouseInput_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1370, 687);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionInput";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.Consumption.ResumeLayout(false);
		this.tabControl6.ResumeLayout(false);
		this.tabPage10.ResumeLayout(false);
		this.panel61.ResumeLayout(false);
		this.panel61.PerformLayout();
		this.panel62.ResumeLayout(false);
		this.groupBox13.ResumeLayout(false);
		this.groupBox13.PerformLayout();
		this.panel83.ResumeLayout(false);
		this.panel83.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionMai).EndInit();
		this.panel82.ResumeLayout(false);
		this.panel82.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionDetail).EndInit();
		this.tabPage9.ResumeLayout(false);
		this.tabPage9.PerformLayout();
		this.toolStripConsumption.ResumeLayout(false);
		this.toolStripConsumption.PerformLayout();
		this.panel48.ResumeLayout(false);
		this.panel80.ResumeLayout(false);
		this.panel80.PerformLayout();
		this.panel79.ResumeLayout(false);
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		this.panelManualEntryConsumption.ResumeLayout(false);
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		this.panel74.ResumeLayout(false);
		this.panel77.ResumeLayout(false);
		this.panel77.PerformLayout();
		this.panel76.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdConsumption).EndInit();
		this.OutPut.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		this.panel60.ResumeLayout(false);
		this.panel60.PerformLayout();
		this.toolStrip8.ResumeLayout(false);
		this.toolStrip8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).EndInit();
		this.panel81.ResumeLayout(false);
		this.groupBox12.ResumeLayout(false);
		this.groupBox12.PerformLayout();
		this.panel84.ResumeLayout(false);
		this.panel84.PerformLayout();
		this.panel85.ResumeLayout(false);
		this.panel85.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).EndInit();
		this.tabPage5.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel64.ResumeLayout(false);
		this.panel64.PerformLayout();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.InPut.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage4.ResumeLayout(false);
		this.panel57.ResumeLayout(false);
		this.panel57.PerformLayout();
		this.toolStrip7.ResumeLayout(false);
		this.toolStrip7.PerformLayout();
		this.panel58.ResumeLayout(false);
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		this.panel59.ResumeLayout(false);
		this.panel59.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).EndInit();
		this.panel78.ResumeLayout(false);
		this.panel78.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).EndInit();
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel28.ResumeLayout(false);
		this.panel63.ResumeLayout(false);
		this.panel63.PerformLayout();
		this.panel72.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.PanelInPutManualEntry.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.toolStripConsumptionHistory.ResumeLayout(false);
		this.toolStripConsumptionHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoConsupmtionHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPItemConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPAcConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouseConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderOutPuthistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExportContractSchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeeder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateuomInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).EndInit();
		base.ResumeLayout(false);
	}
}
