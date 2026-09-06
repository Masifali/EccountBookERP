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
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class invfrmStockConversionProduction : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable dtPackingMaterial = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtStiching = new DataTable();

	private DataTable dtwagesacc = new DataTable();

	private DataTable dtStichingItems = new DataTable();

	private DataTable dtcus = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtUomlst = new DataTable();

	private DataTable dtChargeTo = new DataTable();

	private DataTable dtMoistureSlab = new DataTable();

	private DataTable dtWagesTypeIdsAgainstDocumentType = new DataTable();

	private DataTable dtScheduleData = new DataTable();

	private DataTable dtBrandItem = new DataTable();

	private DataTable dtBrandUom = new DataTable();

	private int RecId;

	private int updateDetailIndex = -1;

	private bool UpdateMode;

	private int VoucherHeadId;

	private bool DetailEditMood;

	private double PercentageForRateAddLess;

	private bool ContractorWageComparisonbyActivityForStockConversion;

	private bool IsFormInitialized;

	private bool MultiBranchFeatur;

	private bool CheckExpenseAmountTotal;

	private bool checkPackingMaterialAmountTotal;

	private bool ContractWagesChargetoProductForStockConversion;

	private bool WagesAmountCalculateOnQty;

	private bool WagesStatus;

	private bool FIFOCgs;

	private bool IsRegularOrFreeOfCost;

	private double GainLossTolerance;

	private bool WagesActiveOrInActive;

	private string InputDetailRowsRemoveIds = "";

	private bool PmCompulsoryForWarning;

	private bool PmCompulsoryForStop;

	private bool SaleMinusAllowedAgainstFifo;

	private bool StichingWagesCompulsory;

	private bool EnableAddLessOnWagesRegular;

	private bool StockReleaseFromFumigation;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private frmLoadStockShortFallForSales LoadOutPut;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Panel panel4;

	private Panel panel10;

	private Panel panel11;

	internal GridEX grdHistoryMain;

	internal GridEX grdhistoryInputOutputDetail;

	private Panel panel7;

	internal GridEX grdHistoryOH;

	internal GridEX grdHistoryPM;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbEntryType;

	private CheckBox chkPrint;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private TextBox txtdeailRemarks;

	private Label label21;

	internal UltraCombo cmbRateUom;

	private Label label47;

	internal UltraCombo cmbUOM;

	private Label label32;

	private Label label23;

	private Label label30;

	private TextBox txtAverageRate;

	private Label label28;

	private TextBox txtStock;

	internal UltraCombo cmbGodown;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

	internal UltraCombo CmbCropyr;

	private Button Add;

	private DateTimePicker txtDocdate;

	private Label label3;

	private TextBox txtAmount;

	private Label label1;

	private Label label26;

	private Label label6;

	private TextBox txtRate;

	private Label label24;

	private Label label44;

	private Label label7;

	private TextBox txtUnitWeight;

	private TextBox txtdocnumber;

	private TextBox txtProductionNo;

	private TextBox txtQty;

	private Label label18;

	private Label label5;

	private Label label12;

	internal UltraCombo cmbBagType;

	private Label label10;

	private Label label8;

	private GroupBox groupBox1;

	private Label label27;

	internal UltraCombo cmbParentCategory;

	private Label label4;

	private TextBox txtMoisture;

	internal UltraCombo cmbMoistureSlab;

	private Label label9;

	private TabControl tabControl2;

	private TabPage tabInput;

	private Panel panel22;

	internal GridEX grdInput;

	private TabPage tabOutPut;

	private Panel panel23;

	internal GridEX grdByProduct;

	private TabPage tabPm;

	private TabPage tabOh;

	private Panel panel6;

	private Panel panel12;

	internal GridEX grdOH;

	internal GridEX grdPM;

	private ToolStripButton btnGenerate;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnIssuanceLoad;

	private GroupBox groupBox3;

	private Panel panel3;

	private Label label16;

	private Label label19;

	private Label label17;

	private Panel panel8;

	private GroupBox groupBox2;

	private Panel panel24;

	private Panel panel25;

	private Label label33;

	private Label label34;

	private Label label35;

	private Label label36;

	private Label label37;

	private Label label38;

	private Label label39;

	private Label label40;

	private TextBox txtDifference;

	private TextBox txtOverHeadQty;

	private TextBox txtAvgRate;

	private TextBox txtPackingMaterialAmount;

	private TextBox txtPackingMaterialQty;

	private TextBox txtPackingMaterialRate;

	private TextBox txtOverHeadAmount;

	private TextBox txtOverHeadWeight;

	private TextBox txtPackingMaterialWeight;

	private TextBox txtOverHeadRate;

	private Panel panel51;

	private Panel panel52;

	private Label label43;

	private Label label11;

	private Label label13;

	private Label label14;

	private Label label15;

	private Label label25;

	private Label label29;

	private Label label31;

	private TextBox txtOutputByProductQty;

	private TextBox txtFinishGoodsQty;

	private TextBox txtInputQuantity;

	private TextBox txtInputWeight;

	private TextBox txtOutputByProductWeight;

	private TextBox txtFinishGoodsWeight;

	private TextBox txtInputRate;

	private TextBox txtOutputByProductRate;

	private TextBox txtFinishGoodsRate;

	private TextBox txtInputAmount;

	private TextBox txtFinishGoodsAmount;

	private TextBox txtOutputByProductAmount;

	private Panel panel5;

	internal UltraCombo CmbProductionDepartment;

	private Label label112;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label2;

	private Panel panel13;

	private Panel panel16;

	private Panel panel15;

	private Panel panel14;

	private Label label20;

	private Panel panel18;

	private Label label41;

	private Panel panel17;

	private Label label22;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private DateTimePicker ToDateHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label42;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private Label label45;

	private Button btnshowHistory;

	private DateTimePicker FromDateHistory;

	private Label label46;

	private Label label48;

	private RadioButton RdApproveDate;

	private Button BtnForwardRowsFromInputToOutPut;

	private ToolStripButton btnshortcutkeys;

	private Label label49;

	internal UltraCombo CmbConversionType;

	private Label label51;

	private Label label50;

	private TextBox txtRemarks;

	internal UltraCombo CmbDifferenceAccount;

	private Label label52;

	private TextBox txtInputOutputValueDiff;

	private Label labelInputOutputValueDiff;

	internal UltraCombo CmbJobLotForGrid;

	private Label label66;

	private Button BtnUpdateComboValueIngrid;

	private ToolStripButton BtnLoadOutPut;

	private ToolStripButton btnDelete;

	private ToolStripButton btnVoucher;

	private TabPage tabwages;

	private TabControl tabControl3;

	private TabPage tabPage3;

	private Panel panel19;

	internal GridEX grdwagesDetail;

	private Panel panel20;

	private Label label53;

	private Button btnAddContractorValuesForAllDetailWages;

	private Label label54;

	private TabPage tabPage4;

	private Panel panel21;

	internal GridEX grdStiching;

	private Panel panel26;

	private Label label55;

	private Button btnAddContractorValuesForAllSticingWages;

	private Label label56;

	private Label labelWagesAmt;

	private TextBox txtWagesAmount;

	private ToolStripButton btnWagesSchedule;

	private Button btnResetRegularWagesForOutPut;

	private Button btnResetRegularWagesForInput;

	private Button btnResetOtherWagesForInput;

	private Button btnResetOtherWagesForOutPut;

	private ToolStripButton btnWagesExempt;

	private Label label57;

	private Button btnGenerateItemsAndUom;

	private ToolStripButton btnStockReleaseFromFumigation;

	private CheckBox chkFumigationOnHold;

	public invfrmStockConversionProduction(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void invfrmStockConversionProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 720);
			FIFOCgs = CommonServices.GetERPFeatureById(5);
			MultiBranchFeatur = CommonServices.GetERPFeatureById(11);
			SaleMinusAllowedAgainstFifo = CommonServices.GetERPFeatureById(14);
			StockReleaseFromFumigation = CommonServices.GetERPFeatureById(24);
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnStockConversion"));
			PmCompulsoryForWarning = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialCompulsoryOnStockConversionForWarning"));
			PmCompulsoryForStop = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialCompulsoryOnStockConversionForStop"));
			GainLossTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ToleranceForInPutMinusOutputForConversionInPercent"));
			PercentageForRateAddLess = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PercentageForRateAddLess"));
			ContractWagesChargetoProductForStockConversion = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ContractWagesChargetoProductForStockConversion"));
			EnableAddLessOnWagesRegular = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EnableAddLessOnWagesRegular"));
			ContractorWageComparisonbyActivityForStockConversion = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ContractorWageComparisonbyActivityForStockConversion"));
			WagesAmountCalculateOnQty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesAmountCalculateOnQty"));
			bool IssuanceByLoader = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IssuanceByLoader"));
			btnIssuanceLoad.Visible = IssuanceByLoader;
			chkFumigationOnHold.Visible = IssuanceByLoader && StockReleaseFromFumigation;
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 66);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			StichingWagesCompulsory = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OtherWagesCompulsoryForStockConversion"));
			dtWagesTypeIdsAgainstDocumentType = InvContractorWagesBillHeader.WagesTypeIdsAgainstDocumentType_GetAll();
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = false;
			tabControl2.TabPages.Remove(tabwages);
			labelWagesAmt.Visible = ContractWagesChargetoProductForStockConversion;
			txtWagesAmount.Visible = ContractWagesChargetoProductForStockConversion;
			btnWagesSchedule.Visible = ContractWagesChargetoProductForStockConversion;
			btnWagesExempt.Visible = ContractWagesChargetoProductForStockConversion;
			if (ContractWagesChargetoProductForStockConversion)
			{
				tabControl2.TabPages.Add(tabwages);
				suppliercustomer();
				accountName();
			}
			ScheduleNoDbCall();
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("ItemUOM");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocNoId", typeof(int));
			table.Columns.Add("RefDocSubId", typeof(int));
			table.Columns.Add("EntryType");
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse");
			table.Columns.Add("CropYear");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("UOM");
			table.Columns.Add("PackEquivalent", typeof(double));
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("BalQty", typeof(double));
			table.Columns.Add("BalWeight", typeof(double));
			table.Columns.Add("Quantity", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateUOMId", typeof(int));
			table.Columns.Add("RateUOM");
			table.Columns.Add("RateEquivalent", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("Moisture");
			table.Columns.Add("MoistureSlabId", typeof(int));
			table.Columns.Add("Remarks");
			table.Columns.Add("LineId", typeof(int));
			table.Columns.Add("labIPmActivityLogId", typeof(int));
			grdInput.DataSource = table;
			grdInput.RetrieveStructure();
			gridsettings();
			tableByProduct.Columns.Add("Id", typeof(int));
			tableByProduct.Columns.Add("EntryType");
			tableByProduct.Columns.Add("WareHouseId", typeof(int));
			tableByProduct.Columns.Add("WareHouse");
			tableByProduct.Columns.Add("CropYear");
			tableByProduct.Columns.Add("ItemId", typeof(int));
			tableByProduct.Columns.Add("Item");
			tableByProduct.Columns.Add("JobLotId", typeof(int));
			tableByProduct.Columns.Add("JobLot");
			tableByProduct.Columns.Add("ItemUOMId", typeof(int));
			tableByProduct.Columns.Add("UOM");
			tableByProduct.Columns.Add("PackEquivalent", typeof(double));
			tableByProduct.Columns.Add("PackingTypeId", typeof(int));
			tableByProduct.Columns.Add("PackingType");
			tableByProduct.Columns.Add("Quantity", typeof(double));
			tableByProduct.Columns.Add("Weight", typeof(double));
			tableByProduct.Columns.Add("Rate", typeof(double));
			tableByProduct.Columns.Add("RateUOMId", typeof(int));
			tableByProduct.Columns.Add("RateUOM");
			tableByProduct.Columns.Add("RateEquivalent", typeof(double));
			tableByProduct.Columns.Add("AmountWithoutExpenses", typeof(double));
			tableByProduct.Columns.Add("Moisture");
			tableByProduct.Columns.Add("MoistureSlabId", typeof(int));
			tableByProduct.Columns.Add("MoistureSlabDescription");
			tableByProduct.Columns.Add("PMAmount", typeof(double));
			tableByProduct.Columns.Add("ItemPMAmount", typeof(double));
			tableByProduct.Columns.Add("ExpAmount", typeof(double));
			tableByProduct.Columns.Add("ItemExpAmount", typeof(double));
			tableByProduct.Columns.Add("Amount", typeof(double));
			tableByProduct.Columns.Add("Remarks");
			tableByProduct.Columns.Add("IssueWeight", typeof(double));
			tableByProduct.Columns.Add("LineId", typeof(int));
			tableByProduct.Columns.Add("WagesAmount", typeof(double));
			tableByProduct.Columns.Add("labIPmActivityLogId", typeof(int));
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsByProduct();
			cmbChargeTo();
			dtPackingMaterial.Columns.Add("ItemId");
			dtPackingMaterial.Columns.Add("WareHouseId", typeof(int));
			dtPackingMaterial.Columns.Add("WareHouse");
			dtPackingMaterial.Columns.Add("RackId", typeof(int));
			dtPackingMaterial.Columns.Add("RackName");
			dtPackingMaterial.Columns.Add("ItemConditionId", typeof(int));
			dtPackingMaterial.Columns.Add("BrandItemId", typeof(int));
			dtPackingMaterial.Columns.Add("BrandItem", typeof(string));
			dtPackingMaterial.Columns.Add("BrandUomId", typeof(int));
			dtPackingMaterial.Columns.Add("BrandUom", typeof(string));
			dtPackingMaterial.Columns.Add("ItemQTY", typeof(double));
			dtPackingMaterial.Columns.Add("ItemRate", typeof(double));
			dtPackingMaterial.Columns.Add("Amount", typeof(double));
			dtPackingMaterial.Columns.Add("ChargeTo");
			dtPackingMaterial.Columns.Add("ContractScheduleId", typeof(int));
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			dtExpense.Columns.Add("ChartofAccount");
			dtExpense.Columns.Add("BrandItemId", typeof(int));
			dtExpense.Columns.Add("BrandItem", typeof(string));
			dtExpense.Columns.Add("BrandUomId", typeof(int));
			dtExpense.Columns.Add("BrandUom", typeof(string));
			dtExpense.Columns.Add("LedgerRemarks");
			dtExpense.Columns.Add("ExpAmount", typeof(double));
			dtExpense.Columns.Add("ChargeTo");
			AddRowInExpenseGrid();
			grdOH.RetrieveStructure();
			grdConversionExpenseSettings();
			dtdetail.Columns.Add("SupplierId");
			dtdetail.Columns.Add("ContractorName");
			dtdetail.Columns.Add("WagesId", typeof(int));
			dtdetail.Columns.Add("WagesAccount");
			dtdetail.Columns.Add("WagesType");
			dtdetail.Columns.Add("Date", typeof(DateTime));
			dtdetail.Columns.Add("packingTypeId");
			dtdetail.Columns.Add("packingType");
			dtdetail.Columns.Add("Weight", typeof(double));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("Quantity", typeof(double));
			dtdetail.Columns.Add("WeightCut", typeof(double));
			dtdetail.Columns.Add("BillQty", typeof(double));
			dtdetail.Columns.Add("BillWeight", typeof(double));
			dtdetail.Columns.Add("RateWithoutAddLess", typeof(double));
			dtdetail.Columns.Add("RateAddLess", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("jobLotId");
			dtdetail.Columns.Add("jobLot");
			dtdetail.Columns.Add("Crop");
			dtdetail.Columns.Add("MoveFromId");
			dtdetail.Columns.Add("MoveFrom");
			dtdetail.Columns.Add("MoveToId");
			dtdetail.Columns.Add("MoveTo");
			dtdetail.Columns.Add("PurchaseGLAC");
			dtdetail.Columns.Add("WarehouseType");
			dtdetail.Columns.Add("RefDocQty", typeof(double));
			dtdetail.Columns.Add("RefDocWeight", typeof(double));
			dtdetail.Columns.Add("RefLineId", typeof(int));
			dtdetail.Columns.Add("WagesScheduleId", typeof(int));
			dtdetail.Columns.Add("TransactionType");
			dtStiching = dtdetail.Clone();
			GenerateDocNumber();
			ParentCategoryFill();
			BindProductionDepartment();
			BindProductionType();
			CmbEntryTypeFill();
			Warehouse();
			ItemFill();
			BindItemCombo(Conversion.ToInt(cmbParentCategory.Value));
			CropYear();
			combojoblotfill();
			bagType();
			MoistureSlabFill();
			AccountFills();
			CmbConversionType_Leave(null, null);
			txtProductionNo.Focus();
			txtDocdate.Tag = txtDocdate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumber()
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
			int code = InvStockConversion.GetGenerateCode(new InvStockConversion
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchedId = UserAccount.BranchesId,
				DocTypeId = 66,
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

	private void CmbEntryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EntryType");
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "IssuanceByLoader").ToList();
			if (list.Count > 0 && !bool.Parse(list[0].ConfigKey.ToString()))
			{
				dt.Rows.Add(1, "Issue");
			}
			dt.Rows.Add(2, "Recovery By Product");
			dt.Rows.Add(3, "Recovery Head Rice");
			DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType", "EntryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbChargeTo()
	{
		try
		{
			dtChargeTo.Columns.Add("Id");
			dtChargeTo.Columns.Add("ChargeTo");
			dtChargeTo.Rows.Add(1, "Recovery By Product");
			dtChargeTo.Rows.Add(2, "Recovery Head Rice");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindProductionDepartment()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbProductionDepartment.Value);
			DataTable dt = InvWareHouse.GetActiveWareHouseByWareHouseType(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				ActivityId = 2
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbProductionDepartment, "Id", "WareHouseName", "Production Department", ZeroIndex: false);
				((UltraGridBase)CmbProductionDepartment).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbProductionDepartment.Value = Id;
					}
					else
					{
						((Control)(object)CmbProductionDepartment).Text = "";
					}
				}
			}
			else
			{
				((UltraGridBase)CmbProductionDepartment).DataSource = null;
				((Control)(object)CmbProductionDepartment).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindProductionType()
	{
		try
		{
			DataTable dtType = CommonServices.StaticColumnsService("ConversionType");
			if (dtType.Rows.Count > 0 && !StockReleaseFromFumigation)
			{
				dtType.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["Id"]) == 5)?.Delete();
				dtType.AcceptChanges();
			}
			InfragisticsHelper.BindAndRetainSelection(CmbConversionType, dtType, "Id", "type", "Conversion Type", AllColumns: false, Conversion.ToInt(CmbConversionType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtType.Rows.Count > 0)
			{
				((UltraGridBase)CmbConversionType).DisplayLayout.Bands[0].Columns[1].Width = 200;
				((UltraGridBase)CmbConversionType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Warehouse()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbGodown).Text = string.Empty;
			((UltraGridBase)cmbGodown).DataSource = null;
			DataTable dtwh = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, cmbGodown, "Id", "WareHouseName", "Issue From ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			Item obj = new Item();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtitem = Item.ReadAllItems(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindItemCombo(int ParentCategoryId = 0)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				DataTable dtFilteredItems = new DataTable();
				if (ParentCategoryId > 0)
				{
					EnumerableRowCollection<DataRow> rows = from r in dtitem.AsEnumerable()
						where r.Field<int>("InventoryParentCategoriesId") == ParentCategoryId
						select r;
					dtFilteredItems = (rows.Any() ? rows.CopyToDataTable() : dtitem.Clone());
					DDL.BindDDLNew(dtFilteredItems, cmbItem, "Id", "ItemName", "Item Name", false);
				}
				else
				{
					DDL.BindDDLNew(dtitem, cmbItem, "Id", "ItemName", "Item Name", false);
				}
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbLot).Text = string.Empty;
			((UltraGridBase)cmbLot).DataSource = null;
			DataTable dtJob = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, cmbLot, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
				DDL.BindDDL(dtJob, CmbJobLotForGrid, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
			}
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
			((Control)(object)CmbCropyr).Text = string.Empty;
			((UltraGridBase)CmbCropyr).DataSource = null;
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", false);
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
			DataTable dtpt = new DataTable();
			dtpt = InvPackingType.Getall();
			if (dtpt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpt, cmbBagType, "Id", "PackTypeDesc", "Packing Type", false);
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
			DataTable dtCoa = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12,10,9");
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbDifferenceAccount, "Id", "AccountTitle", "Difference Account", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string Packuom = ((Control)(object)cmbUOM).Text;
			string Rateuom = ((Control)(object)cmbRateUom).Text;
			((Control)(object)cmbUOM).Text = string.Empty;
			((Control)(object)cmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbUOM, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, cmbRateUom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)cmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)cmbRateUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbUOM).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbUOM).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)cmbRateUom).Text = Rateuom;
				}
				else
				{
					((Control)(object)cmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)cmbUOM).Text = string.Empty;
				((Control)(object)cmbRateUom).Text = string.Empty;
				((UltraGridBase)cmbUOM).DataSource = null;
				((UltraGridBase)cmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GetAvgRate();
			}
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRate()
	{
		try
		{
			double rate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(cmbItem.Value), Conversion.ToDateTime((object)txtDocdate.Value), 66, RecId, Conversion.ToInt(cmbLot.Value), Conversion.ToInt(CmbCropyr.Value), null, Conversion.ToInt(cmbGodown.Value));
			if (rate > 0.0)
			{
				txtAverageRate.Text = Conversion.ToString((object)rate);
				if (!DetailEditMood)
				{
					txtRate.Text = Conversion.ToString((object)rate);
				}
			}
			else
			{
				txtAverageRate.Text = "0";
				txtRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 66);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.InventoryParentCategories(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbParentCategory, "Id", "InvParentCateDescription", "InvParentCateDescription", ZeroIndex: false);
				((UltraGridBase)cmbParentCategory).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getItemAndJobLotDataFromProductionOutPut()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			int prevJobLot = Conversion.ToInt(cmbLot.Value);
			int prevItem = Conversion.ToInt(cmbItem.Value);
			DataTable dtcombo = InvFoodProduction.GetDataFromProductionForStockConversion(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "112"
			});
			if (dtcombo == null || dtcombo.Rows.Count == 0)
			{
				InfragisticsHelper.ClearCombo(CmbJobLotForGrid);
				InfragisticsHelper.ClearCombo(cmbItem);
				return;
			}
			DataTable dtjob = new DataView(dtcombo)
			{
				RowFilter = "Activity = 'JobLot'"
			}.ToTable(false, "Id", "ReferenceName");
			dtjob.Columns["ReferenceName"].ColumnName = "JobLot";
			DataTable dtitem = new DataView(dtcombo)
			{
				RowFilter = "Activity = 'ItemName'"
			}.ToTable(false, "Id", "ReferenceName");
			dtitem.Columns["ReferenceName"].ColumnName = "ItemName";
			InfragisticsHelper.BindAndRetainSelection(cmbLot, dtjob, "Id", "JobLot", "Job Lot", AllColumns: true, prevJobLot, insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtitem, "Id", "ItemName", "Item Name", AllColumns: true, prevItem, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading Production Output data:\n" + ex.Message);
		}
	}

	private void WareHouseFillByParentCategoryId()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbGodown).Text = string.Empty;
			((UltraGridBase)cmbGodown).DataSource = null;
			DataTable dtWarehouse = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetAllWarehousesByParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Conversion.ToInt(cmbParentCategory.Value)
			});
			if (dtWarehouse.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Name");
				for (int i = 0; i < dtWarehouse.Rows.Count; i++)
				{
					dt.Rows.Add(dtWarehouse.Rows[i]["Id"], dtWarehouse.Rows[i]["WareHouseName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbGodown, "Id", "Name", "Warehouse", ZeroIndex: false);
					((UltraGridBase)cmbGodown).DisplayLayout.Bands[0].Columns[1].Width = 151;
					((UltraGridBase)cmbGodown).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearbyWarehouseIdandParentCategoryId(int WarehouseId, int ParentCategoryId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbCropyr).Text = string.Empty;
			((UltraGridBase)CmbCropyr).DataSource = null;
			DataTable dtCropYear = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetAllCropYearsByByParentCategoryWarehouses",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId
			});
			if (dtCropYear.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Name");
				for (int i = 0; i < dtCropYear.Rows.Count; i++)
				{
					dt.Rows.Add(dtCropYear.Rows[i]["CropYear"], dtCropYear.Rows[i]["CropYear"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbCropyr, "Id", "Name", "CropYear", ZeroIndex: false);
					((UltraGridBase)CmbCropyr).DisplayLayout.Bands[0].Columns[1].Width = 84;
					GetCurrentItemStockByWarehouseByJobLotAndCropYear();
					((UltraGridBase)CmbCropyr).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllItemsByCropByWarehouseByInventoryParentCategory(int ParentCategoryId, int WarehouseId, string CropYear)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			DataTable dtItem = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetAllItemsByCropByWarehouseByInventoryParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId,
				CropYear = CropYear
			});
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Name");
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbItem, "Id", "Name", "Item Name", ZeroIndex: false);
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 200;
					GetCurrentItemStockByWarehouseByJobLotAndCropYear();
					((UltraGridBase)cmbItem).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllLotsByItemByCropByWarehouseByInventoryParentCategory(int ParentCategoryId, int WarehouseId, string CropYear, int ItemId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbLot).Text = string.Empty;
			((UltraGridBase)cmbLot).DataSource = null;
			DataTable dtLot = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				Activity = "GetAllLotsByItemByCropByWarehouseByInventoryParentCategory",
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId,
				ItemCategoryId = ParentCategoryId,
				CropYear = CropYear,
				ItemId = ItemId
			});
			if (dtLot.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Name");
				for (int i = 0; i < dtLot.Rows.Count; i++)
				{
					dt.Rows.Add(dtLot.Rows[i]["Id"], dtLot.Rows[i]["JobLotDescription"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbLot, "Id", "Name", "Job Lot", ZeroIndex: false);
					((UltraGridBase)cmbLot).DisplayLayout.Bands[0].Columns[1].Width = 200;
					GetCurrentItemStockByWarehouseByJobLotAndCropYear();
					((UltraGridBase)cmbLot).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCurrentItemStockByWarehouseByJobLotAndCropYear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		try
		{
			DataTable dtGetCurrentItemStock = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Conversion.ToInt(cmbParentCategory.Value),
				WarehouseId = Conversion.ToInt(cmbGodown.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				JobLotId = Conversion.ToInt(cmbLot.Value),
				ToDate = DateTime.Now,
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropyr).Text),
				Activity = "GetCurrentItemStockByWarehouseByJobLotAndCropYear"
			});
			if (dtGetCurrentItemStock.Rows.Count > 0)
			{
				double stockAvailable = Math.Round(Conversion.ToDouble(dtGetCurrentItemStock.Rows[0]["AvailableItemStock"]), 2);
				txtStock.Text = stockAvailable.ToString();
			}
			else
			{
				txtStock.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MoistureSlabFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtMoistureSlab = MoistureSlab.MoistureSlabGetAll(obj);
			if (dtMoistureSlab.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtMoistureSlab, cmbMoistureSlab, "Id", "MoistureSlabDescription", "Moisture Slab", false);
				((UltraGridBase)cmbMoistureSlab).DisplayLayout.Bands[0].Columns[1].Width = 130;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEntryType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbParentCategory).Text == string.Empty)
			{
				MessageBox.Show("Please Select Parent Category First!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbParentCategory.Focus();
				return;
			}
			if (((Control)(object)cmbEntryType).Text == string.Empty)
			{
				MessageBox.Show("Please Select Entry Type First!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbEntryType.Focus();
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Recovery Head Rice")
			{
				txtRate.ReadOnly = true;
				txtRate.TabStop = false;
			}
			else
			{
				txtRate.ReadOnly = false;
				txtRate.TabStop = true;
			}
			if (((Control)(object)cmbEntryType).Text != "Issue")
			{
				BindItemCombo(Conversion.ToInt(cmbParentCategory.Value));
				combojoblotfill();
				txtAverageRate.Text = 0.ToString();
			}
			CalculateWeight();
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbConversionType_Leave(object sender, EventArgs e)
	{
		try
		{
			int num = Conversion.ToInt(CmbConversionType.Value);
			bool isId4 = num == 5;
			bool isId3 = num == 3;
			((Control)(object)CmbDifferenceAccount).Visible = isId3;
			label52.Visible = isId3;
			txtInputOutputValueDiff.Visible = isId3;
			labelInputOutputValueDiff.Visible = isId3;
			txtRemarks.Size = (isId3 ? new Size(272, 26) : new Size(272, 54));
			BtnLoadOutPut.Visible = !isId3 && SaleMinusAllowedAgainstFifo;
			btnIssuanceLoad.Visible = !isId4;
			btnStockReleaseFromFumigation.Visible = isId4;
			if (isId4)
			{
				GridPmDropdownBind();
				if (tabControl2.TabPages.Contains(tabwages))
				{
					tabControl2.TabPages.Remove(tabwages);
				}
				labelWagesAmt.Visible = false;
				txtWagesAmount.Visible = false;
				btnWagesSchedule.Visible = false;
				btnWagesExempt.Visible = false;
				chkFumigationOnHold.Visible = false;
				return;
			}
			labelWagesAmt.Visible = ContractWagesChargetoProductForStockConversion;
			txtWagesAmount.Visible = ContractWagesChargetoProductForStockConversion;
			btnWagesSchedule.Visible = ContractWagesChargetoProductForStockConversion;
			btnWagesExempt.Visible = ContractWagesChargetoProductForStockConversion;
			if (ContractWagesChargetoProductForStockConversion)
			{
				if (!tabControl2.TabPages.Contains(tabwages))
				{
					tabControl2.TabPages.Add(tabwages);
				}
				suppliercustomer();
				accountName();
			}
			else if (tabControl2.TabPages.Contains(tabwages))
			{
				tabControl2.TabPages.Remove(tabwages);
			}
			bool IssuanceByLoader = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IssuanceByLoader"));
			chkFumigationOnHold.Visible = IssuanceByLoader;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbParentCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			BindItemCombo(Conversion.ToInt(cmbParentCategory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbConversionType).ActiveRow == null || Conversion.ToInt(CmbConversionType.Value) == 0)
		{
			MessageBox.Show("Conversion Type Field Required");
			CmbConversionType.Focus();
			return false;
		}
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text == "0" || txtdocnumber.Text == null)
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Production NO. Field Required");
			txtProductionNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((UltraGridBase)CmbConversionType).ActiveRow == null || Conversion.ToInt(CmbConversionType.Value) == 0)
		{
			MessageBox.Show("Conversion Type Field Required");
			CmbConversionType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEntryType).ActiveRow == null)
		{
			MessageBox.Show("Entry Type Field Required");
			cmbEntryType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGodown).ActiveRow == null)
		{
			MessageBox.Show("Ware house Field Required");
			cmbGodown.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropyr).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropyr.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Field Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLot).ActiveRow == null)
		{
			MessageBox.Show("Lot Field Required");
			cmbLot.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUOM).ActiveRow == null)
		{
			MessageBox.Show("UOM Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBagType).ActiveRow == null)
		{
			MessageBox.Show("Bag Type Field Required");
			cmbBagType.Focus();
			return false;
		}
		if (Conversion.ToString((object)((Control)(object)cmbEntryType).Text) != "Issue" && (txtQty.Text.Trim() == string.Empty || txtQty.Text.Trim() == "0"))
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQty.Focus();
			return false;
		}
		if (txtUnitWeight.Text.Trim() == string.Empty || txtUnitWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Unit Weight Field Required");
			txtUnitWeight.Focus();
			return false;
		}
		if (((Control)(object)cmbEntryType).Text != "Recovery Head Rice")
		{
			if (txtRate.Text == string.Empty)
			{
				MessageBox.Show("Rate  Field Required");
				txtRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtAmount.Text) == 0.0)
			{
				MessageBox.Show("Amount  Field Required");
				txtAmount.Focus();
				return false;
			}
		}
		if (((UltraGridBase)cmbRateUom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM Field Required");
			cmbRateUom.Focus();
			return false;
		}
		if (txtMoisture.Text == string.Empty)
		{
			MessageBox.Show("Moisture Field Required");
			txtMoisture.Focus();
			return false;
		}
		return true;
	}

	private void RefreshForm()
	{
		try
		{
			InputDetailRowsRemoveIds = "";
			RecId = 0;
			VoucherHeadId = 0;
			txtWagesAmount.Text = "0";
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			txtProductionNo.Text = string.Empty;
			((Control)(object)CmbProductionDepartment).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtUnitWeight.Text = string.Empty;
			btnDelete.Visible = false;
			table.Clear();
			GenerateDocNumber();
			grdInput.ClearStructure();
			grdByProduct.ClearStructure();
			tableByProduct.Rows.Clear();
			grdOH.ClearStructure();
			grdPM.ClearStructure();
			dtExpense.Clear();
			dtPackingMaterial.Clear();
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			AddRowInExpenseGrid();
			grdOH.RetrieveStructure();
			grdConversionExpenseSettings();
			GainLossTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ToleranceForInPutMinusOutputForConversionInPercent"));
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
			CmbConversionType_Leave(null, null);
			chkFumigationOnHold.Checked = false;
			dtdetail.Rows.Clear();
			grdwagesDetail.ClearStructure();
			dtStiching.Rows.Clear();
			grdStiching.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		updateDetailIndex = -1;
		cmbItem.Value = string.Empty;
		cmbUOM.Value = string.Empty;
		cmbGodown.Value = string.Empty;
		CmbCropyr.Value = string.Empty;
		cmbLot.Value = string.Empty;
		cmbBagType.Value = string.Empty;
		((Control)(object)cmbRateUom).Text = string.Empty;
		((Control)(object)cmbMoistureSlab).Text = string.Empty;
		txtUnitWeight.Text = string.Empty;
		txtAverageRate.Text = string.Empty;
		txtMoisture.Text = string.Empty;
		txtQty.Text = string.Empty;
		txtStock.Text = 0.ToString();
		txtRate.Text = string.Empty;
		txtAmount.Text = string.Empty;
		txtdeailRemarks.Text = string.Empty;
		CheckExpenseAmountTotal = false;
		checkPackingMaterialAmountTotal = false;
		GainLossTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ToleranceForInPutMinusOutputForConversionInPercent"));
		((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
	}

	private void summeryreset()
	{
		txtInputAmount.Text = string.Empty;
		txtInputQuantity.Text = string.Empty;
		txtInputWeight.Text = string.Empty;
		txtInputRate.Text = string.Empty;
		txtOutputByProductAmount.Text = string.Empty;
		txtOutputByProductQty.Text = string.Empty;
		txtOutputByProductRate.Text = string.Empty;
		txtOutputByProductWeight.Text = string.Empty;
		txtFinishGoodsAmount.Text = string.Empty;
		txtFinishGoodsQty.Text = string.Empty;
		txtFinishGoodsRate.Text = string.Empty;
		txtFinishGoodsWeight.Text = string.Empty;
		txtOverHeadAmount.Text = string.Empty;
		txtOverHeadQty.Text = string.Empty;
		txtOverHeadRate.Text = string.Empty;
		txtOverHeadWeight.Text = string.Empty;
		txtPackingMaterialAmount.Text = string.Empty;
		txtPackingMaterialQty.Text = string.Empty;
		txtPackingMaterialRate.Text = string.Empty;
		txtPackingMaterialWeight.Text = string.Empty;
		labelInputOutputValueDiff.Text = "None";
		labelInputOutputValueDiff.ForeColor = Color.Black;
		txtInputOutputValueDiff.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
		txtInputOutputValueDiff.ForeColor = Color.Black;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		RefreshForm();
		resetDetail();
		summeryreset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BindProductionDepartment();
			BindProductionType();
			ItemFill();
			BindItemCombo(Conversion.ToInt(cmbParentCategory.Value));
			combojoblotfill();
			CropYear();
			Warehouse();
			bagType();
			AccountFills();
			ScheduleNoDbCall();
			GridPmDropdownBind();
			GridOverHeadDropdownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void invfrmStockConversionProduction_FormClosing(object sender, FormClosingEventArgs e)
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

	private void AddInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) == 5)
			{
				throw new Exception("Can't add manual entry in case of Release stock from fumigation");
			}
			if (!FormValidationOfDetailPortion())
			{
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				if (Conversion.ToInt(CmbConversionType.Value) == 2 && grdInput.RowCount > 0)
				{
					int ItemId = Conversion.ToInt(cmbItem.Value);
					if (!grdInput.GetRows().All((GridEXRow r) => Conversion.ToInt(r.Cells["ItemId"].Value) == ItemId))
					{
						MessageBox.Show("Please add same Item,another Item Already Exists in Input grid.");
						return;
					}
				}
				if (FIFOCgs)
				{
					MessageBox.Show("You cannot add a row manually when FIFO is on.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				int maxLine = 0;
				if (grdInput.GetRows().Count() > 0)
				{
					maxLine = Conversion.ToInt((object)grdInput.GetRows().Max((GridEXRow row) => Conversion.ToInt(row.Cells["LineId"].Value)));
				}
				table.Rows.Add(0, 0, 0, 0, ((Control)(object)cmbEntryType).Text, cmbGodown.Value, ((Control)(object)cmbGodown).Text, ((Control)(object)CmbCropyr).Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbLot.Value, ((Control)(object)cmbLot).Text, cmbUOM.Value, ((Control)(object)cmbUOM).Text, ((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value, cmbBagType.Value, ((Control)(object)cmbBagType).Text, Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtUnitWeight.Text), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtUnitWeight.Text), Conversion.ToDouble((object)txtRate.Text), cmbRateUom.Value, ((Control)(object)cmbRateUom).Text, ((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value, Conversion.ToDouble((object)txtAmount.Text), txtMoisture.Text.Trim(), cmbMoistureSlab.Value, txtdeailRemarks.Text, maxLine + 1);
				grdInput.DataSource = table;
				grdInput.RetrieveStructure();
				gridsettings();
				if (ContractWagesChargetoProductForStockConversion)
				{
					WagesGridFillFromInputOutput(grdInput, InputGrid: true);
				}
				resetDetail();
				DetailEditMood = false;
			}
			else
			{
				tableByProduct.Rows.Add(0, ((Control)(object)cmbEntryType).Text, cmbGodown.Value, ((Control)(object)cmbGodown).Text, ((Control)(object)CmbCropyr).Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbLot.Value, ((Control)(object)cmbLot).Text, cmbUOM.Value, ((Control)(object)cmbUOM).Text, ((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value, cmbBagType.Value, ((Control)(object)cmbBagType).Text, Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtUnitWeight.Text), Conversion.ToDouble((object)txtRate.Text), cmbRateUom.Value, ((Control)(object)cmbRateUom).Text, ((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value, Conversion.ToDouble((object)txtAmount.Text), txtMoisture.Text.Trim(), cmbMoistureSlab.Value, ((Control)(object)cmbMoistureSlab).Text, 0, 0, 0, 0, Conversion.ToDouble((object)txtAmount.Text), txtdeailRemarks.Text, 0);
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				gridsettingsByProduct();
				if (ContractWagesChargetoProductForStockConversion)
				{
					WagesGridFillFromInputOutput(grdByProduct);
				}
				resetDetail();
			}
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
			cmbEntryType.Focus();
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
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
			if (!FormValidationOfDetailPortion())
			{
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				if (Conversion.ToInt(CmbConversionType.Value) == 2 && grdInput.RowCount > 0)
				{
					int ItemId = Conversion.ToInt(cmbItem.Value);
					if (!grdInput.GetRows().All((GridEXRow r) => Conversion.ToInt(r.Cells["ItemId"].Value) == ItemId && r.RowIndex != updateDetailIndex))
					{
						MessageBox.Show("Please add same Item,another Item Already Exists in Input grid.");
						return;
					}
				}
				if (FIFOCgs)
				{
					MessageBox.Show("You cannot add a row manually when FIFO is on.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				table.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)cmbEntryType).Text;
				table.Rows[updateDetailIndex]["WareHouseId"] = cmbGodown.Value;
				table.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)cmbGodown).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = cmbLot.Value;
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbLot).Text;
				table.Rows[updateDetailIndex]["PackingTypeId"] = cmbBagType.Value;
				table.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)cmbBagType).Text;
				table.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
				table.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
				table.Rows[updateDetailIndex]["ItemUOMId"] = cmbUOM.Value;
				table.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbUOM).Text;
				table.Rows[updateDetailIndex]["PackEquivalent"] = ((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
				table.Rows[updateDetailIndex]["Quantity"] = txtQty.Text;
				table.Rows[updateDetailIndex]["Weight"] = txtUnitWeight.Text;
				table.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
				table.Rows[updateDetailIndex]["RateUOMId"] = cmbRateUom.Value;
				table.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbRateUom).Text;
				table.Rows[updateDetailIndex]["RateEquivalent"] = ((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value;
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				table.Rows[updateDetailIndex]["Moisture"] = txtMoisture.Text;
				table.Rows[updateDetailIndex]["MoistureSlabId"] = cmbMoistureSlab.Value;
				table.Rows[updateDetailIndex]["Remarks"] = txtdeailRemarks.Text;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				Add.Visible = true;
				if (ContractWagesChargetoProductForStockConversion)
				{
					WagesGridFillFromInputOutput(grdInput, InputGrid: true);
				}
			}
			else
			{
				tableByProduct.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)cmbEntryType).Text;
				tableByProduct.Rows[updateDetailIndex]["WareHouseId"] = cmbGodown.Value;
				tableByProduct.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)cmbGodown).Text;
				tableByProduct.Rows[updateDetailIndex]["JobLotId"] = cmbLot.Value;
				tableByProduct.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbLot).Text;
				tableByProduct.Rows[updateDetailIndex]["PackingTypeId"] = cmbBagType.Value;
				tableByProduct.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)cmbBagType).Text;
				tableByProduct.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
				tableByProduct.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
				tableByProduct.Rows[updateDetailIndex]["ItemUOMId"] = cmbUOM.Value;
				tableByProduct.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbUOM).Text;
				tableByProduct.Rows[updateDetailIndex]["PackEquivalent"] = ((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value;
				tableByProduct.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
				tableByProduct.Rows[updateDetailIndex]["Quantity"] = txtQty.Text;
				tableByProduct.Rows[updateDetailIndex]["Weight"] = txtUnitWeight.Text;
				tableByProduct.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
				tableByProduct.Rows[updateDetailIndex]["RateUOMId"] = cmbRateUom.Value;
				tableByProduct.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbRateUom).Text;
				tableByProduct.Rows[updateDetailIndex]["RateEquivalent"] = ((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value;
				tableByProduct.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				tableByProduct.Rows[updateDetailIndex]["AmountWithoutExpenses"] = Conversion.ToDouble((object)txtAmount.Text);
				tableByProduct.Rows[updateDetailIndex]["Remarks"] = txtdeailRemarks.Text;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				Add.Visible = true;
				if (ContractWagesChargetoProductForStockConversion)
				{
					WagesGridFillFromInputOutput(grdByProduct);
				}
			}
			resetDetail();
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
			cmbEntryType.Focus();
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelDetail.Visible = false;
		resetDetail();
	}

	private void grdInput_DoubleClick(object sender, EventArgs e)
	{
	}

	private void gridsettings(bool FromFumigation = false)
	{
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInput.RootTable.Columns["Id"].Visible = false;
			grdInput.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInput.RootTable.Columns["RefDocNoId"].Visible = false;
			grdInput.RootTable.Columns["RefDocSubId"].Visible = false;
			grdInput.RootTable.Columns["WareHouseId"].Visible = false;
			grdInput.RootTable.Columns["ItemId"].Visible = false;
			grdInput.RootTable.Columns["ItemUOMId"].Visible = false;
			grdInput.RootTable.Columns["JobLotId"].Visible = false;
			grdInput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdInput.RootTable.Columns["RateUOMId"].Visible = false;
			grdInput.RootTable.Columns["PackEquivalent"].Visible = false;
			grdInput.RootTable.Columns["RateEquivalent"].Visible = false;
			grdInput.RootTable.Columns["LineId"].Visible = false;
			grdInput.RootTable.Columns["labIPmActivityLogId"].Visible = false;
			if (!Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IssuanceByLoader")))
			{
				grdInput.RootTable.Columns["Weight"].EditType = (EditType)0;
				grdInput.RootTable.Columns["Quantity"].EditType = (EditType)0;
			}
			grdInput.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInput.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdInput.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocNoId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocSubId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Item"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInput.RootTable.Columns["labIPmActivityLogId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.DiscountType;
			grdInput.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdInput.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdInput.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdInput.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdInput.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdInput.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdInput.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdInput.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInput.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdInput.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInput.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdInput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInput.RootTable.Columns["Moisture"].Width = Constants.InventoryConstants.DocNoConstant;
			grdInput.RootTable.Columns["MoistureSlabId"].Width = Constants.InventoryConstants.StringConstant + 20;
			grdInput.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdInput.RootTable.Columns["MoistureSlabId"].ColumnType = (ColumnType)0;
			grdInput.RootTable.Columns["MoistureSlabId"].HasValueList = true;
			grdInput.RootTable.Columns["MoistureSlabId"].LimitToList = true;
			grdInput.RootTable.Columns["MoistureSlabId"].EditType = (EditType)4;
			grdInput.RootTable.Columns["MoistureSlabId"].Caption = "Moisture Slab Description";
			grdInput.RootTable.Columns["MoistureSlabId"].ValueList.PopulateValueList((IEnumerable)dtMoistureSlab.DefaultView, "Id", "MoistureSlabDescription");
			grdInput.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grdInput.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdInput.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			grdInput.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			grdInput.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Quantity"].FormatString = "#,##0.###";
			grdInput.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.###";
			grdInput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdInput.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (!FromFumigation)
			{
				grdInput.RootTable.Columns.Add("Delete");
				grdInput.RootTable.Columns["Delete"].Key = "Delete";
				grdInput.RootTable.Columns["Delete"].Caption = "X";
				grdInput.RootTable.Columns["Delete"].ButtonText = "X";
				grdInput.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdInput.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdInput.RootTable.Columns["Delete"].Width = 20;
				grdInput.RootTable.Columns["Delete"].Position = 0;
			}
			grdInput.RootTable.Columns.Add("Select");
			grdInput.RootTable.Columns["Select"].ActAsSelector = true;
			grdInput.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdInput.RootTable.Columns["Select"].Width = 30;
			grdInput.RootTable.Columns["Select"].Position = 0;
			grdInput.FrozenColumns = 2;
			((Control)(object)grdInput).Refresh();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
				double Uom = Conversion.ToDouble(item.Cells["PackEquivalent"].Value);
				item.Cells["Weight"].Value = Qty * Uom;
				double num = Qty * Uom;
				double Rate2 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUOM2 = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
				double Amount2 = num / RateUOM2 * Rate2;
				item.Cells["Amount"].Value = Amount2;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
				if (Conversion.ToDouble((object)item.Cells["BalWeight"].Value.ToString()) < Weight && btnsave.Enabled && btnsave.Visible)
				{
					item.Cells["Weight"].Value = 0;
					throw new Exception("Weight cannot greater than Balance Weight Please check!");
				}
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUOM = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
				double Amount = Weight / RateUOM * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdInput.UpdateData();
			GenerateSummaryForUser();
			WagesGridFillFromInputOutput(grdInput, InputGrid: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
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
						InputDetailRowsRemoveIds = InputDetailRowsRemoveIds + "," + Conversion.ToString(item.Cells["Id"].Value);
						item.Delete();
					}
				}
				proportionateOverheadGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
				GenerateSummaryForUser();
				WagesGridFillFromInputOutput(grdInput, InputGrid: true);
			}
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettingsByProduct()
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["MoistureSlabId"].Visible = false;
			grdByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["JobLotId"].Visible = false;
			grdByProduct.RootTable.Columns["PackingTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["RateUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["PackEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["RateEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["LineId"].Visible = false;
			grdByProduct.RootTable.Columns["labIPmActivityLogId"].Visible = false;
			grdByProduct.RootTable.Columns["WagesAmount"].Visible = ContractWagesChargetoProductForStockConversion;
			grdByProduct.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdByProduct.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdByProduct.RootTable.Columns["PMAmount"].Caption = "General Packing Material Amount";
			grdByProduct.RootTable.Columns["ExpAmount"].Caption = "General Overhead Amount";
			grdByProduct.RootTable.Columns["ItemPMAmount"].Caption = "Item Packing Material Amount";
			grdByProduct.RootTable.Columns["ItemExpAmount"].Caption = "Item Overhead Amount";
			grdByProduct.RootTable.Columns["IssueWeight"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Moisture"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["MoistureSlabDescription"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["MoistureSlabId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Quantity"].EditType = (EditType)1;
			grdByProduct.RootTable.Columns["Weight"].EditType = (EditType)1;
			grdByProduct.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ExpAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PMAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemExpAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemPMAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["labIPmActivityLogId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Moisture"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["MoistureSlabDescription"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EntryType"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ExpAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PMAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemExpAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemPMAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.DiscountType + 50;
			grdByProduct.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdByProduct.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdByProduct.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdByProduct.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdByProduct.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdByProduct.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdByProduct.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdByProduct.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdByProduct.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdByProduct.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["Moisture"].Width = Constants.InventoryConstants.DocNoConstant;
			grdByProduct.RootTable.Columns["MoistureSlabDescription"].Width = Constants.InventoryConstants.StringConstant + 30;
			grdByProduct.RootTable.Columns["PMAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdByProduct.RootTable.Columns["ExpAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["ItemPMAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["ItemExpAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdByProduct.RootTable.Columns["IssueWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdByProduct.RootTable.Columns["WagesAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["AmountWithoutExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Quantity"].FormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdByProduct.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdByProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["PMAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["PMAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["PMAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["PMAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemPMAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemPMAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemPMAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemPMAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemExpAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["WagesAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["WagesAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["WagesAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["WagesAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns.Add("Delete");
			grdByProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Delete"].Width = 35;
			grdByProduct.RootTable.Columns["Delete"].Position = 0;
			grdByProduct.FrozenColumns = 1;
			((Control)(object)grdByProduct).Refresh();
			ctrlGrdBar1_Load(null, null);
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
			if (Conversion.ToInt(CmbConversionType.Value) == 5)
			{
				return;
			}
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToDouble(item.Cells["IssueWeight"].Value) > 0.0)
				{
					throw new Exception("Record Not Update because record has exist another Form");
				}
				((Control)(object)cmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				cmbGodown.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbBagType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
				cmbUOM.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbCropyr).Text = item.Cells["CropYear"].Text.ToString();
				cmbRateUom.Value = item.Cells["RateUOMId"].Value.ToString();
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtUnitWeight.Text = item.Cells["Weight"].Text.ToString();
				txtRate.Text = item.Cells["Rate"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Text.ToString();
				txtMoisture.Text = item.Cells["Moisture"].Text.ToString();
				cmbMoistureSlab.Value = item.Cells["MoistureSlabId"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletebyProductRow(GridEXRow item)
	{
		if (Conversion.ToInt(CmbConversionType.Value) != 5)
		{
			if (updateDetailIndex != -1)
			{
				throw new Exception("Reset the Detail First...");
			}
			if (btnsave.Visible && btnsave.Enabled)
			{
				item.Delete();
			}
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
			WagesGridFillFromInputOutput(grdByProduct);
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
		}
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				DeletebyProductRow(item);
			}
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
			GridEXRow item = grdByProduct.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["PackEquivalent"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
				double num = Qty * Uom;
				double Rate3 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM3 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount3 = num / RateUOM3 * Rate3;
				item.Cells["Amount"].Value = Amount3;
				item.Cells["AmountWithoutExpenses"].Value = Amount3;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight2 = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double Rate2 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["Amount"].Value = Amount2;
				item.Cells["AmountWithoutExpenses"].Value = Amount2;
			}
			if (e.Column.Key == "Rate")
			{
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount = Weight / RateUOM * Rate;
				item.Cells["Amount"].Value = Amount;
				item.Cells["AmountWithoutExpenses"].Value = Amount;
			}
			grdByProduct.UpdateData();
			proportionatedPackingMaterialAmountInOutputGrid();
			proportionateOverheadGrid();
			GenerateSummaryForUser();
			WagesGridFillFromInputOutput(grdByProduct);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdByProduct.GetRows().Count() > 0)
			{
				HashSet<string> allowEditFields = new HashSet<string> { "Quantity", "Weight" };
				if (Conversion.ToInt(CmbConversionType.Value) == 3)
				{
					allowEditFields.Add("Rate");
				}
				SetEditableGridFields(grdByProduct, allowEditFields);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SetEditableGridFields(GridEX grid, HashSet<string> editableFields)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) != 5)
			{
				foreach (GridEXColumn item in (JanusCollectionBase)grid.RootTable.Columns)
				{
					GridEXColumn column = item;
					column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
					column.EditType = (EditType)(editableFields.Contains(column.Key) ? 1 : 0);
				}
				return;
			}
			grdByProduct.AllowEdit = (InheritableBoolean)2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InputRowsForwardToOutput()
	{
		try
		{
			if (grdInput.RowCount == 0)
			{
				throw new Exception("Input Grid Have no Rows..");
			}
			if (grdInput.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check any Row First...");
			}
			GridEXRow[] checkedRows = grdInput.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				if (!tableByProduct.AsEnumerable().Any(delegate(DataRow row)
				{
					object obj = row.Field<object>("LineId");
					return obj != DBNull.Value && Conversion.ToInt(obj) == Conversion.ToInt(r.Cells["LineId"].Value);
				}))
				{
					tableByProduct.Rows.Add(0, "Recovery Head Rice", Conversion.ToInt(r.Cells["WareHouseId"].Value), Conversion.ToString(r.Cells["WareHouse"].Value), Conversion.ToString((object)r.Cells["CropYear"].Text), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["Item"].Value), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToString(r.Cells["JobLot"].Value), Conversion.ToInt(r.Cells["ItemUOMId"].Value), Conversion.ToString(r.Cells["UOM"].Value), Conversion.ToDouble(r.Cells["PackEquivalent"].Value), Conversion.ToInt(r.Cells["PackingTypeId"].Value), Conversion.ToString(r.Cells["PackingType"].Value), Conversion.ToDouble(r.Cells["Quantity"].Value), Conversion.ToDouble(r.Cells["Weight"].Value), Conversion.ToDouble(r.Cells["Rate"].Value), Conversion.ToInt(r.Cells["RateUOMId"].Value), Conversion.ToString(r.Cells["RateUOM"].Value), Conversion.ToDouble(r.Cells["RateEquivalent"].Value), Conversion.ToDouble(r.Cells["Amount"].Value), Conversion.ToDouble(r.Cells["Moisture"].Value), Conversion.ToInt(r.Cells["MoistureSlabId"].Value), Conversion.ToString((object)r.Cells["MoistureSlabId"].Text), 0, 0, 0, 0, Conversion.ToDouble(r.Cells["Amount"].Value), Conversion.ToString((object)r.Cells["Remarks"].Text), 0, Conversion.ToInt((object)r.Cells["LineId"].Text), 0, Conversion.ToInt(r.Cells["labIPmActivityLogId"].Value));
				}
			}
			if (tableByProduct.Rows.Count > 0)
			{
				tabControl2.SelectedIndex = 1;
				((Control)(object)grdByProduct).Focus();
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				gridsettingsByProduct();
			}
			if (ContractWagesChargetoProductForStockConversion && Conversion.ToInt(CmbConversionType.Value) != 5)
			{
				WagesGridFillFromInputOutput(grdByProduct);
				WagesGridFillFromInputOutput(grdInput, InputGrid: true);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnForwardRowsFromInputToOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) != 5)
			{
				InputRowsForwardToOutput();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInExpenseGrid()
	{
		dtExpense.Rows.Add(0, 0, "", 0, "", 0, 0);
		grdOH.DataSource = dtExpense;
	}

	private void GridOverHeadDropdownBind()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			List<COAAllocation> lst = new List<COAAllocation>();
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					COAAllocation cOAAllocation = new COAAllocation();
					cOAAllocation.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
					cOAAllocation.AccountTitle = dt.Rows[i]["AccountTitle"].ToString();
					lst.Add(cOAAllocation);
				}
				grdOH.RootTable.Columns["ChartofAccount"].ColumnType = (ColumnType)0;
				grdOH.RootTable.Columns["ChartofAccount"].HasValueList = true;
				grdOH.RootTable.Columns["ChartofAccount"].LimitToList = true;
				grdOH.RootTable.Columns["ChartofAccount"].EditType = (EditType)4;
				grdOH.RootTable.Columns["ChartofAccount"].Caption = "Item";
				grdOH.RootTable.Columns["ChartofAccount"].ValueList.PopulateValueList((IEnumerable)lst, "Id", "AccountTitle");
				grdOH.RootTable.Columns["ChargeTo"].ColumnType = (ColumnType)0;
				grdOH.RootTable.Columns["ChargeTo"].HasValueList = true;
				grdOH.RootTable.Columns["ChargeTo"].LimitToList = true;
				grdOH.RootTable.Columns["ChargeTo"].EditType = (EditType)4;
				grdOH.RootTable.Columns["ChargeTo"].Caption = "ChargeTo";
				grdOH.RootTable.Columns["ChargeTo"].ValueList.PopulateValueList((IEnumerable)dtChargeTo.DefaultView, "Id", "ChargeTo");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Exception from Overhead dropdown bind: " + ex.Message);
		}
	}

	private void grdConversionExpenseSettings()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOH.RootTable.Columns["BrandItemId"].Visible = false;
			grdOH.RootTable.Columns["BrandUomId"].Visible = false;
			grdOH.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOH.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdOH.RootTable.Columns["ChartofAccount"].Width = Constants.InventoryConstants.AccountTitle + 50;
			grdOH.RootTable.Columns["BrandItem"].Width = Constants.InventoryConstants.ItemName;
			grdOH.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant + 40;
			grdOH.RootTable.Columns["LedgerRemarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdOH.RootTable.Columns["ExpAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOH.RootTable.Columns["ChargeTo"].Width = Constants.InventoryConstants.AccountTitle;
			GridOverHeadDropdownBind();
			grdOH.RootTable.Columns["BrandItemId"].EditType = (EditType)0;
			grdOH.RootTable.Columns["BrandItem"].EditType = (EditType)0;
			grdOH.RootTable.Columns["BrandUomId"].EditType = (EditType)0;
			grdOH.RootTable.Columns["BrandUom"].EditType = (EditType)0;
			grdOH.RootTable.Columns["ExpAmount"].EditType = (EditType)1;
			grdOH.RootTable.Columns["LedgerRemarks"].EditType = (EditType)1;
			grdOH.RootTable.Columns["BrandItem"].FilterEditType = (FilterEditType)1;
			grdOH.RootTable.Columns["BrandUom"].FilterEditType = (FilterEditType)1;
			grdOH.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdOH.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdOH.RootTable.Columns["ExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOH.RootTable.Columns["ExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOH.RootTable.Columns.Add("Delete");
			grdOH.RootTable.Columns["Delete"].Key = "Delete";
			grdOH.RootTable.Columns["Delete"].Caption = "X";
			grdOH.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOH.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOH.RootTable.Columns["Delete"].ButtonText = "X";
			grdOH.RootTable.Columns["Delete"].Width = 20;
			grdOH.RootTable.Columns["Delete"].Position = 0;
			grdOH.RootTable.Columns.Add("Add");
			grdOH.RootTable.Columns["Add"].Key = "Add";
			grdOH.RootTable.Columns["Add"].Caption = "+";
			grdOH.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOH.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOH.RootTable.Columns["Add"].ButtonText = "+";
			grdOH.RootTable.Columns["Add"].Width = 20;
			grdOH.RootTable.Columns["Add"].Position = 1;
			grdOH.FrozenColumns = 2;
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
			GridEXRow item = grdOH.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				item.Delete();
				if (dtExpense.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				proportionateOverheadGrid();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInExpenseGrid();
				proportionateOverheadGrid();
			}
			GenerateSummaryForUser();
			grdOH.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOH.UpdateData();
			GridEXRow item = grdOH.CurrentRow;
			double Amount = 0.0;
			if (item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["ExpAmount"].Value) != 0.0)
			{
				Amount = Conversion.ToDouble((object)item.Cells["ExpAmount"].Value.ToString());
				item.Cells["ExpAmount"].Value = Amount;
			}
			proportionateOverheadGrid();
			GenerateSummaryForUser();
			grdOH.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPackingMaterialGrid()
	{
		DataRow dr = dtPackingMaterial.NewRow();
		dtPackingMaterial.Rows.Add(dr);
		grdPM.DataSource = dtPackingMaterial;
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPM.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				item.Delete();
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPackingMaterialGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			grdPM.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPM.UpdateData();
			GridEXRow item = grdPM.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "ItemQTY" || e.Column.Key == "ItemId" || e.Column.Key == "WareHouseId" || e.Column.Key == "ItemConditionId")
				{
					double rate = 0.0;
					int ItemConditionId = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
					DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToDateTime((object)txtDocdate.Value), ItemConditionId, RecId, (RecId > 0) ? 66 : 0);
					if (dtRate.Rows.Count > 0)
					{
						rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
					}
					item.Cells["ItemRate"].Value = rate;
				}
				if (e.Column.Key == "ItemQTY" || e.Column.Key == "ItemRate" || e.Column.Key == "ItemId" || e.Column.Key == "WareHouseId" || e.Column.Key == "ItemConditionId")
				{
					if (item.Cells["ItemQTY"].Value.ToString() != string.Empty && item.Cells["ItemRate"].Value.ToString() != string.Empty)
					{
						double num = Conversion.ToDouble((object)item.Cells["ItemQTY"].Value.ToString());
						double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
						double Amount = num * Rate;
						item.Cells["Amount"].Value = Amount;
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
			}
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
			grdPM.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScheduleNoDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtScheduleData.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.RecId = RecId;
			dtScheduleData = ExportContractSchedule.ExportContractSchedule_GetPending(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridPmDropdownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.GetItemByItemTypeId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = ((Conversion.ToInt(CmbConversionType.Value) == 5) ? "14,17" : "14")
			});
			grdPM.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPM.RootTable.Columns["ItemId"].HasValueList = true;
			grdPM.RootTable.Columns["ItemId"].LimitToList = true;
			grdPM.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPM.RootTable.Columns["ItemId"].Caption = "Item";
			grdPM.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdPM.RootTable.Columns["ChargeTo"].ColumnType = (ColumnType)0;
			grdPM.RootTable.Columns["ChargeTo"].HasValueList = true;
			grdPM.RootTable.Columns["ChargeTo"].LimitToList = true;
			grdPM.RootTable.Columns["ChargeTo"].EditType = (EditType)4;
			grdPM.RootTable.Columns["ChargeTo"].Caption = "Charge To";
			grdPM.RootTable.Columns["ChargeTo"].ValueList.PopulateValueList((IEnumerable)dtChargeTo.DefaultView, "Id", "ChargeTo");
			DataTable dtItemCondition = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			GridEX_Helper.ComboBind(grdPM.RootTable.Columns["ItemConditionId"], dtItemCondition, "Id", "Description", "Item Condition", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.ComboBind(grdPM.RootTable.Columns["ContractScheduleId"], dtScheduleData, "Id", "ScheduleCode", "Schedule / Invoice No", Constants.InventoryConstants.JobLotConstant);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPackingMaterialSettings()
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (FIFOCgs)
			{
				grdPM.RootTable.Columns["ItemRate"].EditType = (EditType)0;
				grdPM.RootTable.Columns["Amount"].EditType = (EditType)0;
			}
			GridPmDropdownBind();
			grdPM.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPM.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdPM.RootTable.Columns["WareHouseId"].Visible = false;
			grdPM.RootTable.Columns["RackId"].Visible = false;
			grdPM.RootTable.Columns["BrandItemId"].Visible = false;
			grdPM.RootTable.Columns["BrandUomId"].Visible = false;
			grdPM.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdPM.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdPM.RootTable.Columns["RackId"].EditType = (EditType)0;
			grdPM.RootTable.Columns["RackName"].EditType = (EditType)0;
			grdPM.RootTable.Columns["BrandItem"].EditType = (EditType)0;
			grdPM.RootTable.Columns["BrandUomId"].EditType = (EditType)0;
			grdPM.RootTable.Columns["BrandUom"].EditType = (EditType)0;
			grdPM.RootTable.Columns["ContractScheduleId"].EditType = (EditType)4;
			grdPM.RootTable.Columns["BrandItem"].FilterEditType = (FilterEditType)1;
			grdPM.RootTable.Columns["BrandUom"].FilterEditType = (FilterEditType)1;
			grdPM.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdPM.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdPM.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName + 30;
			grdPM.RootTable.Columns["BrandItem"].Width = Constants.InventoryConstants.ItemName + 50;
			grdPM.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant;
			grdPM.RootTable.Columns["ItemQTY"].Width = Constants.InventoryConstants.QtyConstant;
			grdPM.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPM.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPM.RootTable.Columns["ChargeTo"].Width = Constants.InventoryConstants.AccountTitle;
			grdPM.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["ItemQTY"].FormatString = "#,##0.###";
			grdPM.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPM.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPM.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPM.RootTable.Columns.Add("Delete");
			grdPM.RootTable.Columns["Delete"].Key = "Delete";
			grdPM.RootTable.Columns["Delete"].Caption = "X";
			grdPM.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Delete"].ButtonText = "X";
			grdPM.RootTable.Columns["Delete"].Width = 20;
			grdPM.RootTable.Columns["Delete"].Position = 0;
			grdPM.RootTable.Columns.Add("Add");
			grdPM.RootTable.Columns["Add"].Key = "Add";
			grdPM.RootTable.Columns["Add"].Caption = "+";
			grdPM.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Add"].ButtonText = "+";
			grdPM.RootTable.Columns["Add"].Width = 20;
			grdPM.RootTable.Columns["Add"].Position = 1;
			grdPM.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellEdited(object sender, ColumnActionEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPM.CurrentRow;
			if ((e.Column.Key == "ItemQTY" || e.Column.Key == "ItemRate") && item != null && (int)item.RowType == 0 && item.Cells["ItemQTY"].Value.ToString() != string.Empty && item.Cells["ItemRate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["ItemQTY"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidationforRefRowWeight(DataTable dt, string GridName)
	{
		try
		{
			foreach (var item in (from row in dt.AsEnumerable()
				group row by new
				{
					WagesId = ((!row.IsNull("WagesId")) ? Conversion.ToInt((object)row.Field<int>("WagesId")) : 0),
					RefLineId = ((!row.IsNull("RefLineId")) ? Conversion.ToInt((object)row.Field<int>("RefLineId")) : 0),
					RefDocWeight = (row.IsNull("RefDocWeight") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("RefDocWeight")))
				} into @group
				select new
				{
					WagesId = @group.Key.WagesId,
					RefLineId = @group.Key.RefLineId,
					RefDocWeight = @group.Key.RefDocWeight,
					TotalBillWeight = @group.Sum((DataRow row) => (!row.IsNull("BillWeight")) ? row.Field<double>("BillWeight") : 0.0)
				}).ToList())
			{
				if (item.TotalBillWeight > item.RefDocWeight)
				{
					string wagesAccountName = (from row in dtwagesacc.AsEnumerable()
						where row.Field<int>("Id") == item.WagesId
						select row.Field<string>("WagesAccountName")).FirstOrDefault() ?? "";
					throw new Exception("TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n" + $"Here TotalBillWeight ({item.TotalBillWeight}) exceeds Reference Row Weight ({item.RefDocWeight}) for WagesAccount ({wagesAccountName}) and RowNo {item.RefLineId}  in {GridName} Grid");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void ValidationforRefRowQty(DataTable dt, string GridName)
	{
		try
		{
			foreach (var item in (from row in dt.AsEnumerable()
				group row by new
				{
					WagesId = ((!row.IsNull("WagesId")) ? Conversion.ToInt((object)row.Field<int>("WagesId")) : 0),
					RefLineId = ((!row.IsNull("RefLineId")) ? Conversion.ToInt((object)row.Field<int>("RefLineId")) : 0),
					RefDocQty = (row.IsNull("RefDocQty") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("RefDocQty")))
				} into @group
				select new
				{
					WagesId = @group.Key.WagesId,
					RefLineId = @group.Key.RefLineId,
					RefDocQty = @group.Key.RefDocQty,
					TotalQuantity = @group.Sum((DataRow row) => (!row.IsNull("Quantity")) ? row.Field<double>("Quantity") : 0.0)
				}).ToList())
			{
				if (item.TotalQuantity > item.RefDocQty)
				{
					string wagesAccountName = (from row in dtwagesacc.AsEnumerable()
						where row.Field<int>("Id") == item.WagesId
						select row.Field<string>("WagesAccountName")).FirstOrDefault() ?? "";
					throw new Exception("TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n" + $"Here TotalQty ({item.TotalQuantity}) exceeds Reference Row Qty ({item.RefDocQty}) for WagesAccount ({wagesAccountName}) and RowNo {item.RefLineId} in {GridName} Grid");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void btnResetWagesFromInput_Click(object sender, EventArgs e)
	{
		try
		{
			(from DataRow r in dtdetail.Rows
				where Convert.ToString(r["TransactionType"]) == "Issue"
				select r).ToList().ForEach(delegate(DataRow r)
			{
				dtdetail.Rows.Remove(r);
			});
			WagesGridFillFromInputOutput(grdInput, InputGrid: true, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetWagesForOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			(from DataRow r in dtdetail.Rows
				where Convert.ToString(r["TransactionType"]) == "Recovery"
				select r).ToList().ForEach(delegate(DataRow r)
			{
				dtdetail.Rows.Remove(r);
			});
			WagesGridFillFromInputOutput(grdByProduct, InputGrid: false, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetOtherWagesForInput_Click(object sender, EventArgs e)
	{
	}

	private void btnResetOtherWagesForOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			(from DataRow r in dtStiching.Rows
				where Convert.ToString(r["TransactionType"]) == "Recovery"
				select r).ToList().ForEach(delegate(DataRow r)
			{
				dtStiching.Rows.Remove(r);
			});
			WagesGridFillFromInputOutput(grdByProduct, InputGrid: false, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		try
		{
			if (!MultiBranchFeatur)
			{
				SupplierCustomer supplier = new SupplierCustomer();
				supplier.OrganizationId = UserAccount.OrganizationId;
				supplier.CompanyId = UserAccount.CompanyId;
				dtcus = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			}
			else
			{
				ReportsParameters supplier2 = new ReportsParameters();
				supplier2.OrganizationId = UserAccount.OrganizationId;
				supplier2.CompanyId = UserAccount.CompanyId;
				supplier2.BranchesId = UserAccount.BranchesId;
				dtcus = ContractorsAllocationToBranch.GetContractorsAllocatedToBranch(supplier2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void accountName()
	{
		try
		{
			string Ids = null;
			DataRow[] matchingRows = dtWagesTypeIdsAgainstDocumentType.Select("DocumentTypeId = '" + 66 + "'");
			if (matchingRows.Length != 0)
			{
				Ids = Conversion.ToString(matchingRows[0]["WagesActivityIds"]);
			}
			dtwagesacc = CommonServices.GetWagesAccount(Ids, 0, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesGridFillFromInputOutput(GridEX grid, bool InputGrid = false, int TypeId = 0)
	{
		try
		{
			string transactionType = (InputGrid ? "Issue" : "Recovery");
			if (grid.RowCount > 0)
			{
				var groupedData = (from r in grid.GetDataRows()
					group r by new
					{
						ItemId = Convert.ToInt32(r.Cells["ItemId"].Value),
						Item = Convert.ToString(r.Cells["Item"].Value),
						WarehouseId = Convert.ToInt32(r.Cells["WareHouseId"].Value),
						Warehouse = Convert.ToString(r.Cells["WareHouse"].Value),
						Crop = Convert.ToString(r.Cells["CropYear"].Value),
						JobLotId = Convert.ToInt32(r.Cells["JobLotId"].Value),
						JobLot = Convert.ToString(r.Cells["JobLot"].Value),
						PackingTypeId = Convert.ToInt32(r.Cells["PackingTypeId"].Value),
						PackingType = Convert.ToString(r.Cells["PackingType"].Value),
						PackEquivalent = Convert.ToDouble(r.Cells["PackEquivalent"].Value)
					} into g
					select new
					{
						ItemId = g.Key.ItemId,
						Item = g.Key.Item,
						WarehouseId = g.Key.WarehouseId,
						Warehouse = g.Key.Warehouse,
						Crop = g.Key.Crop,
						JobLotId = g.Key.JobLotId,
						JobLot = g.Key.JobLot,
						PackingTypeId = g.Key.PackingTypeId,
						PackingType = g.Key.PackingType,
						PackEquivalent = g.Key.PackEquivalent,
						Quantity = g.Sum((GridEXRow x) => Convert.ToDouble(x.Cells["Quantity"].Value)),
						Weight = g.Sum((GridEXRow x) => Convert.ToDouble(x.Cells["Weight"].Value)),
						TransactionType = transactionType
					}).ToList();
				switch (TypeId)
				{
				case 0:
					UpdateSummaryTable(dtdetail, groupedData, transactionType);
					if (transactionType != "Issue")
					{
						UpdateSummaryTable(dtStiching, groupedData, transactionType);
					}
					break;
				case 1:
					UpdateSummaryTable(dtdetail, groupedData, transactionType);
					break;
				case 2:
					if (transactionType != "Issue")
					{
						UpdateSummaryTable(dtStiching, groupedData, transactionType);
					}
					break;
				}
			}
			else
			{
				switch (TypeId)
				{
				case 0:
					RemoveRowsByTransactionType(dtdetail, transactionType);
					RemoveRowsByTransactionType(dtStiching, transactionType);
					break;
				case 1:
					RemoveRowsByTransactionType(dtdetail, transactionType);
					break;
				case 2:
					RemoveRowsByTransactionType(dtStiching, transactionType);
					break;
				}
			}
			switch (TypeId)
			{
			case 0:
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				DetailGridSettings();
				grdStiching.DataSource = dtStiching;
				grdStiching.RetrieveStructure();
				GridStichingSettings();
				break;
			case 1:
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				DetailGridSettings();
				break;
			case 2:
				grdStiching.DataSource = dtStiching;
				grdStiching.RetrieveStructure();
				GridStichingSettings();
				break;
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			throw new Exception("Error in WagesGridFillFromInputOutput: " + ex.Message);
		}
	}

	private void UpdateSummaryTable(DataTable table, IEnumerable<dynamic> groupedData, string transactionType)
	{
		if (table == null)
		{
			return;
		}
		foreach (dynamic g2 in groupedData)
		{
			DataRow existing = table.AsEnumerable().FirstOrDefault((DataRow r) => Convert.ToInt32(r["ItemId"]) == g2.ItemId && Convert.ToInt32(r["MoveFromId"]) == g2.WarehouseId && Convert.ToString(r["Crop"]) == g2.Crop && Convert.ToInt32(r["jobLotId"]) == g2.JobLotId && Convert.ToInt32(r["packingTypeId"]) == g2.PackingTypeId && Convert.ToDouble(r["PackSize"]) == g2.PackEquivalent && Convert.ToString(r["TransactionType"]) == g2.TransactionType);
			if (existing == null)
			{
				int maxRowNo = (from r in table.AsEnumerable()
					select r.Field<int?>("RefLineId").GetValueOrDefault()).DefaultIfEmpty(0).Max();
				bool isFree = CommonServices.CheckItemsFreeofcostforWages(Convert.ToDateTime(txtDocdate.Value), 66, g2.ItemId, 0);
				DataRow newRow = table.NewRow();
				newRow["ItemId"] = (object)g2.ItemId;
				newRow["Item"] = (object)g2.Item;
				newRow["WagesType"] = (isFree ? "Free Of Cost" : "Regular");
				newRow["MoveFromId"] = (object)g2.WarehouseId;
				newRow["MoveFrom"] = (object)g2.Warehouse;
				newRow["MoveToId"] = (object)g2.WarehouseId;
				newRow["MoveTo"] = (object)g2.Warehouse;
				newRow["Crop"] = (object)g2.Crop;
				newRow["jobLotId"] = (object)g2.JobLotId;
				newRow["jobLot"] = (object)g2.JobLot;
				newRow["packingTypeId"] = (object)g2.PackingTypeId;
				newRow["packingType"] = (object)g2.PackingType;
				newRow["PackSize"] = (object)g2.PackEquivalent;
				newRow["Quantity"] = (object)g2.Quantity;
				newRow["Weight"] = (object)g2.Weight;
				newRow["BillQty"] = (object)g2.Quantity;
				newRow["BillWeight"] = (object)g2.Weight;
				newRow["TransactionType"] = (object)g2.TransactionType;
				newRow["RefDocQty"] = (object)g2.Quantity;
				newRow["RefDocWeight"] = (object)g2.Weight;
				newRow["RefLineId"] = maxRowNo + 1;
				table.Rows.Add(newRow);
			}
			else
			{
				existing["Quantity"] = (object)g2.Quantity;
				existing["BillQty"] = (object)g2.Quantity;
				existing["BillWeight"] = (object)g2.Weight;
				existing["Weight"] = (object)g2.Weight;
				existing["RefDocQty"] = (object)g2.Quantity;
				existing["RefDocWeight"] = (object)g2.Weight;
				double Rate = Conversion.ToDouble(existing["Rate"]);
				double Amount = ((!WagesAmountCalculateOnQty) ? ((double)((g2.PackEquivalent > 0) ? (g2.Weight / g2.PackEquivalent * Rate) : ((object)0))) : ((double)(g2.Quantity * Rate)));
				existing["Amount"] = Math.Round(Amount, 2);
			}
		}
		var keysInGroupedData = groupedData.Select((dynamic g) => new
		{
			ItemId = (object)g.ItemId,
			WarehouseId = (object)g.WarehouseId,
			Crop = (object)g.Crop,
			JobLotId = (object)g.JobLotId,
			PackingTypeId = (object)g.PackingTypeId,
			PackEquivalent = (object)g.PackEquivalent,
			TransactionType = (object)g.TransactionType
		}).ToList();
		foreach (DataRow row in (from r in table.AsEnumerable()
			where Convert.ToString(r["TransactionType"]) == transactionType
			where !keysInGroupedData.Any(g => g.ItemId == Convert.ToInt32(r["ItemId"]) && g.WarehouseId == Convert.ToInt32(r["MoveFromId"]) && g.Crop == Convert.ToString(r["Crop"]) && g.JobLotId == Convert.ToInt32(r["jobLotId"]) && g.PackingTypeId == Convert.ToInt32(r["packingTypeId"]) && g.PackEquivalent == Convert.ToDouble(r["PackSize"]) && g.TransactionType == Convert.ToString(r["TransactionType"]))
			select r).ToList())
		{
			table.Rows.Remove(row);
		}
	}

	private void RemoveRowsByTransactionType(DataTable table, string transactionType)
	{
		if (table == null || table.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in (from r in table.AsEnumerable()
			where Convert.ToString(r["TransactionType"]) == transactionType
			select r).ToList())
		{
			table.Rows.Remove(row);
		}
	}

	private void DetailGridSettings()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1002: Expected O, but got Unknown
		try
		{
			WagesItemAndContractorRefresh();
			grdwagesDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdwagesDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdwagesDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grdwagesDetail.RootTable.Columns["WarehouseType"].Visible = false;
			grdwagesDetail.RootTable.Columns["ContractorName"].Visible = false;
			grdwagesDetail.RootTable.Columns["WagesAccount"].Visible = false;
			grdwagesDetail.RootTable.Columns["ItemId"].Visible = false;
			grdwagesDetail.RootTable.Columns["jobLotId"].Visible = false;
			grdwagesDetail.RootTable.Columns["packingTypeId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveFromId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveToId"].Visible = false;
			grdwagesDetail.RootTable.Columns["BillQty"].Visible = false;
			grdwagesDetail.RootTable.Columns["RefDocQty"].Visible = false;
			grdwagesDetail.RootTable.Columns["Date"].Visible = false;
			grdwagesDetail.RootTable.Columns["RefDocWeight"].Visible = false;
			grdwagesDetail.RootTable.Columns["WagesScheduleId"].Visible = false;
			grdwagesDetail.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			int DocTypeValue = 66;
			if (DocTypeValue != 68 || DocTypeValue != 806)
			{
				grdwagesDetail.RootTable.Columns["MoveTo"].Visible = false;
			}
			grdwagesDetail.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["jobLotId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveToId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["BillQty"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefDocQty"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefDocWeight"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefLineId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesScheduleId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefLineId"].Visible = true;
			grdwagesDetail.RootTable.Columns["RefLineId"].Caption = "RowNo";
			grdwagesDetail.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			grdwagesDetail.RootTable.Columns["Date"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["jobLot"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveTo"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Crop"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["BillWeight"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RateAddLess"].EditType = (EditType)(EnableAddLessOnWagesRegular ? 1 : 0);
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Quantity"].EditType = (EditType)1;
			grdwagesDetail.RootTable.Groups.Add("TransactionType");
			grdwagesDetail.RootTable.Columns["TransactionType"].HideWhenGrouped = (InheritableBoolean)1;
			grdwagesDetail.RootTable.Columns["SupplierId"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdwagesDetail.RootTable.Columns["ContractorName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdwagesDetail.RootTable.Columns["WagesId"].Width = Constants.InventoryConstants.WagesActity;
			grdwagesDetail.RootTable.Columns["WagesAccount"].Width = Constants.InventoryConstants.WagesActity;
			grdwagesDetail.RootTable.Columns["WagesType"].Width = Constants.InventoryConstants.DiscountType;
			grdwagesDetail.RootTable.Columns["Date"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdwagesDetail.RootTable.Columns["packingType"].Width = Constants.InventoryConstants.PackingType;
			grdwagesDetail.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdwagesDetail.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdwagesDetail.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdwagesDetail.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			grdwagesDetail.RootTable.Columns["BillQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdwagesDetail.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdwagesDetail.RootTable.Columns["RateAddLess"].Width = Constants.InventoryConstants.ExRateConstant;
			grdwagesDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdwagesDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdwagesDetail.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName + 20;
			grdwagesDetail.RootTable.Columns["jobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdwagesDetail.RootTable.Columns["Crop"].Width = Constants.InventoryConstants.CropYearConstant;
			grdwagesDetail.RootTable.Columns["MoveFrom"].Width = Constants.InventoryConstants.WareHouseName;
			grdwagesDetail.RootTable.Columns["MoveTo"].Width = Constants.InventoryConstants.WareHouseName;
			grdwagesDetail.RootTable.Columns["WarehouseType"].Width = Constants.InventoryConstants.CastingType;
			grdwagesDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Quantity"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			grdwagesDetail.RootTable.Columns["BillQty"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["BillQty"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["BillQty"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillQty"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Weight"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["BillWeight"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateAddLess"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["RateAddLess"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Rate"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Amount"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns.Add("Add");
			grdwagesDetail.RootTable.Columns["Add"].Key = "Add";
			grdwagesDetail.RootTable.Columns["Add"].Caption = "Add";
			grdwagesDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdwagesDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdwagesDetail.RootTable.Columns["Add"].ButtonText = "Add";
			grdwagesDetail.RootTable.Columns["Add"].Width = 40;
			grdwagesDetail.RootTable.Columns.Add("Delete");
			grdwagesDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdwagesDetail.RootTable.Columns["Delete"].Caption = "X";
			grdwagesDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdwagesDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdwagesDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdwagesDetail.RootTable.Columns["Delete"].Width = 20;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdwagesDetail.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			grdwagesDetail.RootTable.FormatConditions.Add(fc);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesItemAndContractorRefresh()
	{
		try
		{
			grdwagesDetail.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["SupplierId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["SupplierId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["SupplierId"].Caption = "Contractor Name";
			grdwagesDetail.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtcus.DefaultView, "Id", "CompanyName");
			grdwagesDetail.RootTable.Columns["SupplierId"].Width = 200;
			grdwagesDetail.RootTable.Columns["WagesId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["WagesId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["WagesId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["WagesId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["WagesId"].Caption = "Labour / Wages Activity";
			grdwagesDetail.RootTable.Columns["WagesId"].ValueList.PopulateValueList((IEnumerable)dtwagesacc.DefaultView, "Id", "WagesAccountName");
			grdwagesDetail.RootTable.Columns["WagesId"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow selectedRow = grdwagesDetail.CurrentRow;
			double WagesRate = 0.0;
			int WagesScheduleId = 0;
			double AllQtyExceptSelected = (from GridEXRow row in grdwagesDetail.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["Quantity"].Value).Sum();
			double AllNetWeightExceptSelected = (from GridEXRow row in grdwagesDetail.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["BillWeight"].Value).Sum();
			double AllGrossWeightExceptSelected = (from GridEXRow row in grdwagesDetail.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["Weight"].Value).Sum();
			GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
			double TotalQty = GridEX_Helper.GetColumnSum(grdByProduct, "Quantity");
			grdwagesDetail.UpdateData();
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				Conversion.ToDouble(item.Cells["Rate"].Value);
				item.BeginEdit();
				if (e.Column.Key == "ContractorName" || e.Column.Key == "WagesId" || e.Column.Key == "WagesAccount")
				{
					IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime((object)txtDocdate.Value), 66, Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToInt(item.Cells["WagesId"].Value));
					if (IsRegularOrFreeOfCost)
					{
						item.Cells["Wagestype"].Value = "Free Of Cost";
					}
					else
					{
						item.Cells["Wagestype"].Value = "Regular";
					}
				}
				if (e.Column.Key == "Quantity" && Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double Qty2 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					item.Cells["Quantity"].Value = Math.Round(Qty2, 2);
					double WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double PackSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Weight = Qty2 * PackSize;
					item.Cells["Weight"].Value = Weight.ToString();
					double WeightCutTotal2 = Qty2 * WeightCut;
					double BillWeight3 = Weight - Math.Abs(WeightCutTotal2);
					item.Cells["BillWeight"].Value = Math.Round(BillWeight3, 2);
				}
				if ((e.Column.Key == "Weight" || e.Column.Key == "WeightCut") && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double num = Conversion.ToDouble(item.Cells["Weight"].Value);
					double b = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty = num / b;
					item.Cells["Quantity"].Value = Math.Round(Qty, 2);
					double num2 = Conversion.ToDouble(item.Cells["Weight"].Value);
					double WeightCutTotal = Conversion.ToDouble(item.Cells["WeightCut"].Value) * Qty;
					double BillWeight2 = num2 - Math.Abs(WeightCutTotal);
					item.Cells["BillWeight"].Value = Math.Round(BillWeight2, 2);
				}
				if (e.Column.Key == "WagesId" || e.Column.Key == "SupplierId" || e.Column.Key == "Quantity" || e.Column.Key == "Weight" || e.Column.Key == "WeightCut" || e.Column.Key == "RateAddLess")
				{
					if (Conversion.ToString((object)item.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)item.Cells["WarehouseType"].Text) == "Dryer")
					{
						item.Cells["WagesScheduleId"].Value = 0;
						item.Cells["Rate"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
					else
					{
						GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToDouble(item.Cells["PackSize"].Value), Conversion.ToInt(item.Cells["WagesId"].Value), Conversion.ToInt(item.Cells["SupplierId"].Value));
						if (d != null)
						{
							WagesRate = d.WagesRate;
							WagesScheduleId = d.ScheduleId;
						}
					}
				}
				if (WagesRate > 0.0)
				{
					item.Cells["Rate"].Value = WagesRate;
					item.Cells["WagesScheduleId"].Value = WagesScheduleId;
					if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
					{
						double Quantity = Conversion.ToDouble(item.Cells["Quantity"].Value);
						double Packsize = Conversion.ToDouble(item.Cells["PackSize"].Value);
						double BillWeight = Conversion.ToDouble(item.Cells["BillWeight"].Value);
						double RateWithoutAddLess = Conversion.ToDouble(item.Cells["Rate"].Value);
						item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess;
						double RateAddLess = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
						if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
						{
							if (PercentageForRateAddLess > 0.0)
							{
								double RateAddLessByConfig = PercentageForRateAddLess * RateWithoutAddLess / 100.0;
								bool RateAddlessInMinus = !(RateAddLess > 0.0);
								if (Math.Abs(RateAddLess) > RateAddLessByConfig)
								{
									RateAddLess = (RateAddlessInMinus ? Conversion.ToDouble((object)("-" + RateAddLessByConfig)) : RateAddLessByConfig);
									item.Cells["RateAddLess"].Value = RateAddLess;
									MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig}");
								}
							}
							else
							{
								RateAddLess = 0.0;
								item.Cells["RateAddLess"].Value = RateAddLess;
								MessageBox.Show("Please set RateAddLess Percentage in config first...");
							}
						}
						double Rate = RateWithoutAddLess + RateAddLess;
						item.Cells["Rate"].Value = Rate;
						double Amount = ((!WagesAmountCalculateOnQty) ? (BillWeight / Packsize * Rate) : (Quantity * Rate));
						item.Cells["Amount"].Value = Math.Round(Amount, 2);
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					item.Cells["WagesScheduleId"].Value = 0;
					item.Cells["Rate"].Value = 0;
					item.Cells["RateWithoutAddLess"].Value = 0;
					item.Cells["Amount"].Value = 0;
				}
				item.EndEdit();
			}
			grdwagesDetail.UpdateData();
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					if (dtdetail.Rows.Count <= 1)
					{
						MessageBox.Show("You Can Not Delete All rows");
						return;
					}
					item.Delete();
				}
				if (e.Column.Key == "Add")
				{
					AddRowInGLGrid(item);
				}
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid(GridEXRow rrr)
	{
		try
		{
			double TotalWeight = 0.0;
			double TotalQty = 0.0;
			string TransactionType = Conversion.ToString(rrr.Cells["TransactionType"].Value);
			if (TransactionType == "Issue")
			{
				TotalWeight = GridEX_Helper.GetColumnSum(grdInput, "Weight");
				TotalQty = GridEX_Helper.GetColumnSum(grdInput, "Quantity");
			}
			else if (TransactionType == "Recovery")
			{
				TotalWeight = GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
				TotalQty = GridEX_Helper.GetColumnSum(grdByProduct, "Quantity");
			}
			double Weight = 0.0;
			double WeightCut = 0.0;
			double BillWeight = 0.0;
			double Qty = 0.0;
			if (grdwagesDetail.GetDataRows().Count() > 0)
			{
				foreach (GridEXRow item in from oo in grdwagesDetail.GetDataRows()
					where Conversion.ToString(oo.Cells["TransactionType"].Value) == TransactionType
					select oo)
				{
					Weight += Conversion.ToDouble(item.Cells["Weight"].Value);
					WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					BillWeight += Conversion.ToDouble(item.Cells["BillWeight"].Value);
					Qty += Conversion.ToDouble(item.Cells["Quantity"].Value);
				}
			}
			double GrossWeight = TotalWeight - Weight;
			double GrossQty = TotalQty - Qty;
			int DocTypeValue = 66;
			if (GrossWeight > 0.0 || GrossQty > 0.0 || DocTypeValue == 112 || DocTypeValue == 66)
			{
				bool OtherCondition = DocTypeValue == 112 || DocTypeValue == 66;
				if (WagesAmountCalculateOnQty)
				{
					if (GrossQty > 0.0)
					{
						double ItemWeight = GrossQty * Conversion.ToDouble(rrr.Cells["PackSize"].Value);
						double BillQtyPartal = GrossQty;
						double Amount = BillQtyPartal * Conversion.ToDouble(rrr.Cells["Rate"].Value);
						dtdetail.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, rrr.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, Math.Round(ItemWeight, 2), rrr.Cells["PackSize"].Value, Math.Round(BillQtyPartal, 2), rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, ItemWeight, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, Math.Round(Amount, 2), rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
						grdwagesDetail.DataSource = dtdetail;
					}
					else if (OtherCondition)
					{
						dtdetail.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, rrr.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, rrr.Cells["Weight"].Value, rrr.Cells["PackSize"].Value, rrr.Cells["Quantity"].Value, rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, rrr.Cells["BillWeight"].Value, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, rrr.Cells["Amount"].Value, rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
						grdwagesDetail.DataSource = dtdetail;
					}
					else
					{
						MessageBox.Show("Please Check Grid Qty and TotalQty");
					}
				}
				else if (GrossWeight > 0.0)
				{
					double ItemQty = GrossWeight / Conversion.ToDouble(rrr.Cells["PackSize"].Value);
					double BillWeightPartal = GrossWeight - ItemQty * WeightCut;
					double Amount2 = BillWeightPartal / Conversion.ToDouble(rrr.Cells["PackSize"].Value) * Conversion.ToDouble(rrr.Cells["Rate"].Value);
					dtdetail.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, rrr.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, GrossWeight, rrr.Cells["PackSize"].Value, Math.Round(ItemQty, 2), rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, BillWeightPartal, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, Math.Round(Amount2, 2), rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
					grdwagesDetail.DataSource = dtdetail;
				}
				else if (OtherCondition)
				{
					dtdetail.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, rrr.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, rrr.Cells["Weight"].Value, rrr.Cells["PackSize"].Value, rrr.Cells["Quantity"].Value, rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, rrr.Cells["BillWeight"].Value, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, rrr.Cells["Amount"].Value, rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
					grdwagesDetail.DataSource = dtdetail;
				}
				else
				{
					MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_EditingCell(object sender, EditingCellEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdwagesDetail.CurrentRow;
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

	private void AddRowInStichingGrid(GridEXRow rrr)
	{
		try
		{
			double TotalWeight = 0.0;
			double TotalQty = 0.0;
			string TransactionType = Conversion.ToString(rrr.Cells["TransactionType"].Value);
			if (TransactionType == "Issue")
			{
				TotalWeight = GridEX_Helper.GetColumnSum(grdInput, "Weight");
				TotalQty = GridEX_Helper.GetColumnSum(grdInput, "Quantity");
			}
			else if (TransactionType == "Recovery")
			{
				TotalWeight = GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
				TotalQty = GridEX_Helper.GetColumnSum(grdByProduct, "Quantity");
			}
			double Weight = 0.0;
			double WeightCut = 0.0;
			double BillWeight = 0.0;
			double Qty = 0.0;
			if (grdStiching.GetDataRows().Count() > 0)
			{
				foreach (GridEXRow item in from oo in grdStiching.GetDataRows()
					where Conversion.ToString(oo.Cells["TransactionType"].Value) == TransactionType
					select oo)
				{
					Weight += Conversion.ToDouble(item.Cells["Weight"].Value);
					WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					BillWeight += Conversion.ToDouble(item.Cells["BillWeight"].Value);
					Qty += Conversion.ToDouble(item.Cells["Quantity"].Value);
				}
			}
			double GrossWeight = TotalWeight - Weight;
			double GrossQty = TotalQty - Qty;
			if (WagesAmountCalculateOnQty)
			{
				if (GrossQty > 0.0)
				{
					double ItemWeight = GrossQty * Conversion.ToDouble(rrr.Cells["PackSize"].Value);
					double BillQtyPartal = GrossQty;
					double Amount = BillQtyPartal * Conversion.ToDouble(rrr.Cells["Rate"].Value);
					dtStiching.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, ItemWeight, rrr.Cells["PackSize"].Value, Math.Round(BillQtyPartal, 2), rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, ItemWeight, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, Math.Round(Amount, 2), rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
					grdStiching.DataSource = dtStiching;
				}
				else
				{
					dtStiching.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, rrr.Cells["Weight"].Value, rrr.Cells["PackSize"].Value, rrr.Cells["Quantity"].Value, rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, rrr.Cells["BillWeight"].Value, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, rrr.Cells["Amount"].Value, rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
					grdStiching.DataSource = dtStiching;
				}
			}
			else if (GrossWeight > 0.0)
			{
				double ItemQty = GrossWeight / Conversion.ToDouble(rrr.Cells["PackSize"].Value);
				double BillWeightPartal = GrossWeight - ItemQty * WeightCut;
				double Amount2 = BillWeightPartal / Conversion.ToDouble(rrr.Cells["PackSize"].Value) * Conversion.ToDouble(rrr.Cells["Rate"].Value);
				dtStiching.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, GrossWeight, rrr.Cells["PackSize"].Value, Math.Round(ItemQty, 2), rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, BillWeightPartal, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, Math.Round(Amount2, 2), rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
				grdStiching.DataSource = dtStiching;
			}
			else
			{
				dtStiching.Rows.Add(rrr.Cells["SupplierId"].Value, rrr.Cells["ContractorName"].Value, rrr.Cells["WagesId"].Value, rrr.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, rrr.Cells["Date"].Value, rrr.Cells["packingTypeId"].Value, rrr.Cells["packingType"].Value, rrr.Cells["Weight"].Value, rrr.Cells["PackSize"].Value, rrr.Cells["Quantity"].Value, rrr.Cells["WeightCut"].Value, rrr.Cells["BillQty"].Value, rrr.Cells["BillWeight"].Value, rrr.Cells["RateWithoutAddLess"].Value, rrr.Cells["RateAddLess"].Value, rrr.Cells["Rate"].Value, rrr.Cells["Amount"].Value, rrr.Cells["ItemId"].Value, rrr.Cells["Item"].Value, rrr.Cells["jobLotId"].Value, rrr.Cells["jobLot"].Value, rrr.Cells["Crop"].Value, rrr.Cells["MoveFromId"].Value, rrr.Cells["MoveFrom"].Value, rrr.Cells["MoveToId"].Value, rrr.Cells["MoveTo"].Value, rrr.Cells["PurchaseGLAC"].Value, rrr.Cells["WarehouseType"].Value, rrr.Cells["RefDocQty"].Value, rrr.Cells["RefDocWeight"].Value, rrr.Cells["RefLineId"].Value, rrr.Cells["WagesScheduleId"].Value, TransactionType);
				grdStiching.DataSource = dtStiching;
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStiching_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdStiching.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					if (dtStiching.Rows.Count <= 1)
					{
						MessageBox.Show("You Can Not Delete All rows....");
						return;
					}
					item.Delete();
				}
				if (e.Column.Key == "Add")
				{
					AddRowInStichingGrid(item);
				}
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStiching_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdStiching.UpdateData();
			GridEXRow selectedRow = grdStiching.CurrentRow;
			double AllQtyExceptSelected = (from GridEXRow row in grdStiching.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["Quantity"].Value).Sum();
			double AllNetWeightExceptSelected = (from GridEXRow row in grdStiching.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["BillWeight"].Value).Sum();
			double AllGrossWeightExceptSelected = (from GridEXRow row in grdStiching.GetDataRows()
				where row != selectedRow
				select (double)row.Cells["Weight"].Value).Sum();
			GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
			double TotalQty = GridEX_Helper.GetColumnSum(grdByProduct, "Quantity");
			GridEXRow item = grdStiching.CurrentRow;
			if (e.Column.Key == "ContractorName" || e.Column.Key == "WagesId" || e.Column.Key == "WagesAccount")
			{
				IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime((object)txtDocdate.Value), 66, Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToInt(item.Cells["WagesId"].Value));
				if (IsRegularOrFreeOfCost)
				{
					item.Cells["Wagestype"].Value = "Free Of Cost";
				}
				else
				{
					item.Cells["Wagestype"].Value = "Regular";
				}
			}
			if (e.Column.Key == "WagesId" || e.Column.Key == "SupplierId" || e.Column.Key == "Quantity" || e.Column.Key == "Weight" || e.Column.Key == "WeightCut" || e.Column.Key == "RateAddLess")
			{
				if (Conversion.ToString((object)item.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString(item.Cells["WarehouseType"].Value) == "Dryer")
				{
					item.Cells["Rate"].Value = 0;
					item.Cells["Amount"].Value = 0;
				}
				else
				{
					Conversion.ToDouble(item.Cells["Rate"].Value);
					double WagesRate = 0.0;
					int WagesScheduleId = 0;
					GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToDouble(item.Cells["PackSize"].Value), Conversion.ToInt(item.Cells["WagesId"].Value), Conversion.ToInt(item.Cells["SupplierId"].Value));
					if (d != null)
					{
						WagesRate = d.WagesRate;
						WagesScheduleId = d.ScheduleId;
					}
					if (WagesRate > 0.0)
					{
						item.Cells["WagesScheduleId"].Value = WagesScheduleId;
						item.Cells["Rate"].Value = WagesRate;
						if (Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
						{
							double num = Conversion.ToDouble(item.Cells["Weight"].Value);
							double b2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
							_ = num / b2;
							double Weight3 = Conversion.ToDouble(item.Cells["Weight"].Value);
							if (!WagesAmountCalculateOnQty)
							{
								item.Cells["BillWeight"].Value = Math.Round(Weight3, 2);
							}
						}
						if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
						{
							double num2 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
							double Qty5 = Conversion.ToDouble(item.Cells["Quantity"].Value);
							double WeightCut4 = num2 * Qty5;
							double BillWeight4 = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut4);
							if (!WagesAmountCalculateOnQty)
							{
								item.Cells["BillWeight"].Value = Math.Round(BillWeight4, 2);
							}
							else
							{
								item.Cells["Quantity"].Value = Math.Round(Qty5, 2);
							}
						}
						if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
						{
							double Packsize3 = Conversion.ToDouble(item.Cells["PackSize"].Value);
							double num3 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
							double RateWithoutAddLess3 = Conversion.ToDouble(item.Cells["Rate"].Value);
							item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess3;
							double RateAddLess3 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
							if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
							{
								if (PercentageForRateAddLess > 0.0)
								{
									double RateAddLessByConfig3 = PercentageForRateAddLess * RateWithoutAddLess3 / 100.0;
									bool RateAddlessInMinus3 = !(RateAddLess3 > 0.0);
									if (Math.Abs(RateAddLess3) > RateAddLessByConfig3)
									{
										RateAddLess3 = (RateAddlessInMinus3 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig3)) : RateAddLessByConfig3);
										item.Cells["RateAddLess"].Value = RateAddLess3;
										MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig3}");
									}
								}
								else
								{
									RateAddLess3 = 0.0;
									item.Cells["RateAddLess"].Value = RateAddLess3;
									MessageBox.Show("Please set RateAddLess Percentage in config first...");
								}
							}
							double Rate3 = RateWithoutAddLess3 + RateAddLess3;
							item.Cells["Rate"].Value = Rate3;
							double Amount3 = num3 / Packsize3 * Rate3;
							item.Cells["Amount"].Value = Math.Round(Amount3, 2);
						}
						else
						{
							item.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						item.Cells["Rate"].Value = 0;
						item.Cells["WagesScheduleId"].Value = 0;
					}
				}
			}
			if (e.Column.Key == "Weight" || e.Column.Key == "WeightCut")
			{
				if (Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double num4 = Conversion.ToDouble(item.Cells["Weight"].Value);
					double b = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty4 = num4 / b;
					double Weight2 = Conversion.ToDouble(item.Cells["Weight"].Value);
					item.Cells["BillWeight"].Value = Math.Round(Weight2, 2);
					item.Cells["Quantity"].Value = Math.Round(Qty4, 2);
				}
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double num5 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double Qty3 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double WeightCut3 = num5 * Qty3;
					double BillWeight3 = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut3);
					item.Cells["BillWeight"].Value = Math.Round(BillWeight3, 2);
					item.Cells["Quantity"].Value = Math.Round(Qty3, 2);
				}
				if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
				{
					double Packsize2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double num6 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
					double RateWithoutAddLess2 = Conversion.ToDouble(item.Cells["Rate"].Value);
					item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess2;
					double RateAddLess2 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
					if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
					{
						if (PercentageForRateAddLess > 0.0)
						{
							double RateAddLessByConfig2 = PercentageForRateAddLess * RateWithoutAddLess2 / 100.0;
							bool RateAddlessInMinus2 = !(RateAddLess2 > 0.0);
							if (Math.Abs(RateAddLess2) > RateAddLessByConfig2)
							{
								RateAddLess2 = (RateAddlessInMinus2 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig2)) : RateAddLessByConfig2);
								item.Cells["RateAddLess"].Value = RateAddLess2;
								MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig2}");
							}
						}
						else
						{
							RateAddLess2 = 0.0;
							item.Cells["RateAddLess"].Value = RateAddLess2;
							MessageBox.Show("Please set RateAddLess Percentage in config first...");
						}
					}
					double Rate2 = RateWithoutAddLess2 + RateAddLess2;
					item.Cells["Rate"].Value = Rate2;
					double Amount2 = num6 / Packsize2 * Rate2;
					item.Cells["Amount"].Value = Math.Round(Amount2, 2);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			if (e.Column.Key == "Quantity" || e.Column.Key == "WeightCut")
			{
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double WeightCut2 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double PackSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty2 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double Weight = Qty2 * PackSize;
					double WeightCutTotal = Qty2 * Math.Abs(WeightCut2);
					double BillWeight2 = Weight - Math.Abs(WeightCutTotal);
					if (!WagesAmountCalculateOnQty)
					{
						item.Cells["Weight"].Value = Weight.ToString();
						item.Cells["BillWeight"].Value = Math.Round(BillWeight2, 2);
					}
					else
					{
						item.Cells["Quantity"].Value = Math.Round(Qty2, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double num7 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double WeightCut = num7 * Qty;
					double BillWeight = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut);
					if (!WagesAmountCalculateOnQty)
					{
						item.Cells["BillWeight"].Value = Math.Round(BillWeight, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
				{
					double Packsize = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double num8 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
					double RateWithoutAddLess = Conversion.ToDouble(item.Cells["Rate"].Value);
					double RateAddLess = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
					if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
					{
						if (PercentageForRateAddLess > 0.0)
						{
							double RateAddLessByConfig = PercentageForRateAddLess * RateWithoutAddLess / 100.0;
							bool RateAddlessInMinus = !(RateAddLess > 0.0);
							if (Math.Abs(RateAddLess) > RateAddLessByConfig)
							{
								RateAddLess = (RateAddlessInMinus ? Conversion.ToDouble((object)("-" + RateAddLessByConfig)) : RateAddLessByConfig);
								item.Cells["RateAddLess"].Value = RateAddLess;
								MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig}");
							}
						}
						else
						{
							RateAddLess = 0.0;
							item.Cells["RateAddLess"].Value = RateAddLess;
							MessageBox.Show("Please set RateAddLess Percentage in config first...");
						}
					}
					double Rate = RateWithoutAddLess + RateAddLess;
					item.Cells["Rate"].Value = Rate;
					double Amount = num8 / Packsize * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, 2);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdStiching.UpdateData();
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridStichingSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee2: Expected O, but got Unknown
		try
		{
			grdStiching.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdStiching.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdStiching.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grdStiching.RootTable.Columns["WarehouseType"].Visible = false;
			grdStiching.RootTable.Columns["ContractorName"].Visible = false;
			grdStiching.RootTable.Columns["WagesAccount"].Visible = false;
			grdStiching.RootTable.Columns["ItemId"].Visible = false;
			grdStiching.RootTable.Columns["jobLotId"].Visible = false;
			grdStiching.RootTable.Columns["packingTypeId"].Visible = false;
			grdStiching.RootTable.Columns["MoveFromId"].Visible = false;
			grdStiching.RootTable.Columns["MoveToId"].Visible = false;
			grdStiching.RootTable.Columns["MoveToId"].Visible = false;
			grdStiching.RootTable.Columns["MoveToId"].Visible = false;
			grdStiching.RootTable.Columns["Date"].Visible = false;
			grdStiching.RootTable.Columns["BillQty"].Visible = false;
			grdStiching.RootTable.Columns["RefDocWeight"].Visible = false;
			grdStiching.RootTable.Columns["RefDocQty"].Visible = false;
			grdStiching.RootTable.Columns["WagesScheduleId"].Visible = false;
			grdStiching.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdStiching.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdStiching.RootTable.Columns["MoveTo"].Visible = false;
			grdStiching.RootTable.Groups.Add("TransactionType");
			grdStiching.RootTable.Columns["TransactionType"].HideWhenGrouped = (InheritableBoolean)1;
			grdStiching.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			grdStiching.RootTable.Columns["SupplierId"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdStiching.RootTable.Columns["ContractorName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdStiching.RootTable.Columns["WagesId"].Width = Constants.InventoryConstants.WagesActity;
			grdStiching.RootTable.Columns["WagesAccount"].Width = Constants.InventoryConstants.WagesActity;
			grdStiching.RootTable.Columns["WagesType"].Width = Constants.InventoryConstants.DiscountType;
			grdStiching.RootTable.Columns["Date"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdStiching.RootTable.Columns["packingType"].Width = Constants.InventoryConstants.PackingType;
			grdStiching.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdStiching.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdStiching.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdStiching.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			grdStiching.RootTable.Columns["BillQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdStiching.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdStiching.RootTable.Columns["RateWithoutAddLess"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdStiching.RootTable.Columns["RateAddLess"].Width = Constants.InventoryConstants.ExRateConstant;
			grdStiching.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdStiching.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdStiching.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName + 20;
			grdStiching.RootTable.Columns["jobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdStiching.RootTable.Columns["Crop"].Width = Constants.InventoryConstants.CropYearConstant;
			grdStiching.RootTable.Columns["MoveFrom"].Width = Constants.InventoryConstants.WareHouseName;
			grdStiching.RootTable.Columns["MoveTo"].Width = Constants.InventoryConstants.WareHouseName;
			grdStiching.RootTable.Columns["WarehouseType"].Width = Constants.InventoryConstants.CastingType;
			grdStiching.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["jobLotId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveToId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RateAddLess"].EditType = (EditType)(EnableAddLessOnWagesRegular ? 1 : 0);
			grdStiching.RootTable.Columns["RateWithoutAddLess"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefLineId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefDocQty"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefDocWeight"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefLineId"].Visible = true;
			grdStiching.RootTable.Columns["RefLineId"].Caption = "RowNo";
			grdStiching.RootTable.Columns["Date"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["packingType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["jobLot"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveTo"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Crop"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["BillWeight"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RateAddLess"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["BillQty"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdStiching.RootTable.Columns["SupplierId"].HasValueList = true;
			grdStiching.RootTable.Columns["SupplierId"].LimitToList = true;
			grdStiching.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdStiching.RootTable.Columns["SupplierId"].Caption = "Contractor Name";
			grdStiching.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtcus.DefaultView, "Id", "CompanyName");
			string wagesId = "35";
			DataRow[] matchingRows = dtWagesTypeIdsAgainstDocumentType.Select("DocumentTypeId = '" + 66 + "'");
			if (matchingRows.Length != 0)
			{
				wagesId = Conversion.ToString(matchingRows[0]["WagesActivityIds"]);
			}
			dtStichingItems = CommonServices.GetWagesAccount(wagesId, 0, 1);
			grdStiching.RootTable.Columns["WagesId"].ColumnType = (ColumnType)0;
			grdStiching.RootTable.Columns["WagesId"].HasValueList = true;
			grdStiching.RootTable.Columns["WagesId"].LimitToList = true;
			grdStiching.RootTable.Columns["WagesId"].EditType = (EditType)4;
			grdStiching.RootTable.Columns["WagesId"].Caption = "Labour / Wages Activity";
			grdStiching.RootTable.Columns["WagesId"].ValueList.PopulateValueList((IEnumerable)dtStichingItems.DefaultView, "Id", "WagesAccountName");
			grdStiching.RootTable.Columns.Add("Add");
			grdStiching.RootTable.Columns["Add"].Key = "Add";
			grdStiching.RootTable.Columns["Add"].Caption = "Add";
			grdStiching.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdStiching.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdStiching.RootTable.Columns["Add"].ButtonText = "Add";
			grdStiching.RootTable.Columns["Add"].Width = 40;
			grdStiching.RootTable.Columns.Add("Delete");
			grdStiching.RootTable.Columns["Delete"].Key = "Delete";
			grdStiching.RootTable.Columns["Delete"].Caption = "X";
			grdStiching.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdStiching.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdStiching.RootTable.Columns["Delete"].ButtonText = "X";
			grdStiching.RootTable.Columns["Delete"].Width = 20;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdStiching.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			grdStiching.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdStiching_EditingCell(object sender, EditingCellEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdStiching.CurrentRow;
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

	private void Insert()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Expected O, but got Unknown
		//IL_0f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f87: Expected O, but got Unknown
		//IL_137f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1386: Expected O, but got Unknown
		try
		{
			if (grdInput.GetRows().Count() == 0)
			{
				throw new Exception("InPut Grid Not Found");
			}
			if (grdByProduct.GetRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvStockConversion stockConversion = new InvStockConversion();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				stockConversion.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToInt(CmbConversionType.Value) != 3)
			{
				handleAverageRateCalculation();
			}
			if (txtFinishGoodsAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFinishGoodsAmount.Text) == 0.0)
			{
				throw new Exception("Finish Goods Receod Not Found in Grid Please Check!");
			}
			GridEXRow[] rows = grdPM.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDouble(r5.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			rows = grdOH.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["ExpAmount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ChartofAccount"].Value) == 0)
				{
					MessageBox.Show("Please Select Account Title First");
					return;
				}
			}
			bool hasIssueEntry = false;
			int firstItemId = -1;
			bool allItemsSame = true;
			rows = grdInput.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				if (row2.Cells["EntryType"].Text == "Issue")
				{
					hasIssueEntry = true;
				}
				if (Conversion.ToInt(CmbConversionType.Value) == 2)
				{
					int itemId = Conversion.ToInt(row2.Cells["ItemId"].Value);
					if (firstItemId == -1)
					{
						firstItemId = itemId;
					}
					if (itemId != firstItemId)
					{
						allItemsSame = false;
					}
				}
			}
			if (!hasIssueEntry)
			{
				MessageBox.Show("Issue is Required in Detail Grid");
				return;
			}
			if (Conversion.ToInt(CmbConversionType.Value) == 2 && !allItemsSame)
			{
				MessageBox.Show("Rows in Input Grid should have the same Item when Conversion Type is " + ((Control)(object)CmbConversionType).Text);
				return;
			}
			double InputNetWeight = Conversion.ToDouble((object)txtInputWeight.Text);
			double ByProductNetWeight = Conversion.ToDouble((object)txtOutputByProductWeight.Text);
			double FinishNetWeight = Conversion.ToDouble((object)txtFinishGoodsWeight.Text);
			double DiffWeight = InputNetWeight - (ByProductNetWeight + FinishNetWeight);
			if (Conversion.ToInt(CmbConversionType.Value) == 3 && Math.Abs(Conversion.ToDouble((object)txtInputAmount.Text) - (double)Conversion.ToInt((object)txtFinishGoodsAmount.Text)) > 0.0)
			{
				if (((UltraGridBase)CmbDifferenceAccount).ActiveRow == null || Conversion.ToInt(CmbDifferenceAccount.Value) == 0)
				{
					CmbDifferenceAccount.Focus();
					MessageBox.Show("Difference A/c required when Conversion Type is " + ((Control)(object)CmbConversionType).Text);
					return;
				}
				stockConversion.DifferenceAccountId = Conversion.ToInt(CmbDifferenceAccount.Value);
			}
			if (DiffWeight > 0.0)
			{
				double DiffPercentage = Math.Abs(DiffWeight) * 100.0 / InputNetWeight;
				double ToleranceWeight = InputNetWeight * GainLossTolerance / 100.0;
				stockConversion.GainLossId = 1;
				if (DiffPercentage > GainLossTolerance)
				{
					DiffWeight = Math.Abs(Math.Round(DiffWeight, 4));
					ToleranceWeight = Math.Round(ToleranceWeight, 4);
					throw new Exception("The difference between Input Weight and Output Weight" + $" is greater than the configured tolerance.\nDifference Weight: {DiffWeight} |" + $" Tolerance Weight: {ToleranceWeight}.\nThe entry is in Loss.");
				}
				if (DiffPercentage < GainLossTolerance && MessageBox.Show("There is difference between Input Weight and Output Weight.\n" + $" Input Weight is: {InputNetWeight} |" + $" OutPut Weight is: {ByProductNetWeight + FinishNetWeight} and.\n" + $"Difference Weight is {DiffWeight}.The entry is in Loss. Are you sure you want to save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (DiffWeight < 0.0)
			{
				double DiffPercentage2 = Math.Abs(DiffWeight) * 100.0 / InputNetWeight;
				double ToleranceWeight2 = InputNetWeight * GainLossTolerance / 100.0;
				stockConversion.GainLossId = 2;
				if (DiffPercentage2 > GainLossTolerance)
				{
					DiffWeight = Math.Abs(Math.Round(DiffWeight, 4));
					ToleranceWeight2 = Math.Round(ToleranceWeight2, 4);
					throw new Exception("The difference between Input Weight and Output Weight" + $" is greater than the configured tolerance. \nDifference Weight: {DiffWeight} |" + $" Tolerance Weight: {ToleranceWeight2}.\nThe entry is in Gain.");
				}
				if (DiffPercentage2 < GainLossTolerance && MessageBox.Show("There is difference between Input Weight and Output Weight.\n" + $" Input Weight is: {InputNetWeight} |" + $" OutPut Weight is: {ByProductNetWeight + FinishNetWeight} and.\n" + $"Difference Weight is {DiffWeight}.The entry is in Gain. Are you sure you want to save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			stockConversion.OrganizationId = UserAccount.OrganizationId;
			stockConversion.CompanyId = UserAccount.CompanyId;
			stockConversion.BranchedId = UserAccount.BranchesId;
			stockConversion.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			stockConversion.DocTypeId = 66;
			stockConversion.EntryUser = UserAccount.ID;
			stockConversion.ModifyUser = UserAccount.ID;
			stockConversion.EntryDate = DateTime.Now;
			stockConversion.ModifyDate = DateTime.Now;
			stockConversion.PostDate = DateTime.Now;
			stockConversion.ConversionTypeId = Conversion.ToInt(CmbConversionType.Value);
			stockConversion.DocSrNo = Conversion.ToInt((object)txtdocnumber.Text);
			stockConversion.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			stockConversion.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			stockConversion.parentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			stockConversion.EBDepartmentId = Conversion.ToInt(CmbProductionDepartment.Value);
			stockConversion.DocManualRef = string.Empty;
			stockConversion.Remarks = Conversion.ToString((object)txtRemarks.Text);
			stockConversion.invStockConversionDetails = new List<InvStockConversionDetail>();
			stockConversion.invStockConversionAddExpenses = new List<InvStockConversionAddExpense>();
			stockConversion.invStockConversionPackings = new List<InvStockConversionPackingMaterial>();
			stockConversion.InvContractorWagesBillHeaderList = new List<InvContractorWagesBillHeader>();
			if (InputDetailRowsRemoveIds != null && InputDetailRowsRemoveIds != string.Empty)
			{
				stockConversion.InputDetailRowsRemoveIds = InputDetailRowsRemoveIds;
			}
			rows = grdInput.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionDetail Conversiondetail3 = new InvStockConversionDetail();
					Conversiondetail3.Id = Conversion.ToInt(r4.Cells["Id"].Value);
					Conversiondetail3.RefDocumentTypeId = Conversion.ToInt(r4.Cells["RefDocumentTypeId"].Value);
					Conversiondetail3.RefDocNoId = Conversion.ToInt(r4.Cells["RefDocNoId"].Value);
					Conversiondetail3.RefDocSubId = Conversion.ToInt(r4.Cells["RefDocSubId"].Value);
					Conversiondetail3.EntryType = Conversion.ToString((object)r4.Cells["EntryType"].Text);
					Conversiondetail3.WarehouseId = Conversion.ToInt(r4.Cells["WareHouseId"].Value);
					Conversiondetail3.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					Conversiondetail3.ItemUomId = Conversion.ToInt(r4.Cells["ItemUOMId"].Value);
					Conversiondetail3.CropBatch = Conversion.ToString((object)r4.Cells["CropYear"].Text);
					Conversiondetail3.JobLotId = Conversion.ToInt(r4.Cells["JobLotId"].Value);
					Conversiondetail3.PackingtypeId = Conversion.ToInt(r4.Cells["PackingTypeId"].Value);
					Conversiondetail3.Qty = Conversion.ToDouble(r4.Cells["Quantity"].Value);
					Conversiondetail3.Weight = Conversion.ToDouble(r4.Cells["Weight"].Value);
					if (Conversiondetail3.Weight == 0.0)
					{
						MessageBox.Show("Weight field Required");
						return;
					}
					Conversiondetail3.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
					if (Conversiondetail3.Rate == 0.0)
					{
						MessageBox.Show("ItemRate field Required In Row#" + (r4.RowIndex + 1) + "In InPut Grid");
						return;
					}
					Conversiondetail3.RateUOMId = Conversion.ToInt(r4.Cells["RateUOMId"].Value);
					Conversiondetail3.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
					if (Conversiondetail3.Amount == 0.0)
					{
						MessageBox.Show("Amount field Required In Row#" + (r4.RowIndex + 1) + "In InPut Grid");
						return;
					}
					Conversiondetail3.Moisture = Conversion.ToDouble(r4.Cells["Moisture"].Value);
					Conversiondetail3.MoistureSlabId = Conversion.ToInt(r4.Cells["MoistureSlabId"].Value);
					Conversiondetail3.ProjectId = 0;
					Conversiondetail3.VoucherHeadId = 0;
					Conversiondetail3.PackUnit = 0;
					Conversiondetail3.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text);
					Conversiondetail3.LineId = Conversion.ToInt((object)r4.Cells["LineId"].Text);
					Conversiondetail3.labIPmActivityLogId = Conversion.ToInt((object)r4.Cells["labIPmActivityLogId"].Text);
					stockConversion.invStockConversionDetails.Add(Conversiondetail3);
				}
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionDetail Conversiondetail2 = new InvStockConversionDetail();
					Conversiondetail2.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					Conversiondetail2.EntryType = Conversion.ToString((object)r3.Cells["EntryType"].Text);
					Conversiondetail2.WarehouseId = Conversion.ToInt(r3.Cells["WareHouseId"].Value);
					Conversiondetail2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Conversiondetail2.ItemUomId = Conversion.ToInt(r3.Cells["ItemUOMId"].Value);
					Conversiondetail2.CropBatch = Conversion.ToString((object)r3.Cells["CropYear"].Text);
					Conversiondetail2.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Conversiondetail2.PackingtypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					Conversiondetail2.Qty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
					Conversiondetail2.Weight = Conversion.ToDouble(r3.Cells["Weight"].Value);
					Conversiondetail2.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					if (Conversiondetail2.Rate == 0.0)
					{
						MessageBox.Show("ItemRate field Required In Row#" + (r3.RowIndex + 1) + "In Output Grid");
						return;
					}
					Conversiondetail2.RateUOMId = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
					Conversiondetail2.Moisture = Conversion.ToInt((object)r3.Cells["Moisture"].Text);
					Conversiondetail2.MoistureSlabId = Conversion.ToInt((object)r3.Cells["MoistureSlabId"].Text);
					Conversiondetail2.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					if (Conversiondetail2.Amount == 0.0)
					{
						MessageBox.Show("Amount field Required In Row#" + (r3.RowIndex + 1) + "In Output Grid");
						return;
					}
					Conversiondetail2.ExpenseAmount = Conversion.ToSingle(r3.Cells["ExpAmount"].Value);
					Conversiondetail2.ItemOhCost = Conversion.ToSingle(r3.Cells["ItemExpAmount"].Value);
					Conversiondetail2.PackingMaterialAmount = Conversion.ToDouble(r3.Cells["PMAmount"].Value);
					Conversiondetail2.ItemPmCost = Conversion.ToDouble(r3.Cells["ItemPMAmount"].Value);
					Conversiondetail2.ProjectId = 0;
					Conversiondetail2.VoucherHeadId = 0;
					Conversiondetail2.PackUnit = 0;
					Conversiondetail2.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text);
					Conversiondetail2.LineId = Conversion.ToInt((object)r3.Cells["LineId"].Text);
					Conversiondetail2.WagesAmount = Conversion.ToDouble(r3.Cells["WagesAmount"].Value);
					Conversiondetail2.labIPmActivityLogId = Conversion.ToInt(r3.Cells["labIPmActivityLogId"].Value);
					Conversiondetail2.IsOnHold = StockReleaseFromFumigation && chkFumigationOnHold.Checked;
					stockConversion.invStockConversionDetails.Add(Conversiondetail2);
				}
			}
			rows = grdPM.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvStockConversionPackingMaterial Conversiondetail = new InvStockConversionPackingMaterial();
					Conversiondetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					Conversiondetail.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
					Conversiondetail.RackId = Conversion.ToInt(r2.Cells["RackId"].Value);
					Conversiondetail.ItemConditionId = Conversion.ToInt(r2.Cells["ItemConditionId"].Value);
					if (Conversiondetail.ItemConditionId == 0)
					{
						throw new Exception($"Item Condition field required in Packing grid at row#{r2.RowIndex + 1}");
					}
					Conversiondetail.ItemCondition = Conversion.ToString((object)r2.Cells["ItemConditionId"].Text);
					Conversiondetail.BrandItemId = Conversion.ToInt(r2.Cells["BrandItemId"].Value);
					Conversiondetail.BrandItemUomId = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
					Conversiondetail.ItemQty = Conversion.ToDouble(r2.Cells["ItemQTY"].Value);
					FormHelper.ValidateField(Conversiondetail.ItemId, "Item", r2.RowIndex, "Packing Grid");
					FormHelper.ValidateField(Conversiondetail.WarehouseId, "Warehouse", r2.RowIndex, "Packing Grid");
					FormHelper.ValidateField(Conversiondetail.RackId, "RackName", r2.RowIndex, "Packing Grid");
					FormHelper.ValidateField(Conversiondetail.ItemQty, "Qty", r2.RowIndex, "Packing Grid");
					if (Conversiondetail.BrandItemId > 0 && Conversiondetail.BrandItemUomId == 0)
					{
						MessageBox.Show("Brand Uom Field Require for Brand Item:" + Conversion.ToString(r2.Cells["BrandItem"].Value));
						return;
					}
					Conversiondetail.ItemRate = Conversion.ToDouble((object)r2.Cells["ItemRate"].Text);
					Conversiondetail.ItemAmount = Conversion.ToDouble((object)r2.Cells["Amount"].Text);
					FormHelper.ValidateField(Conversiondetail.ItemAmount, "Amount", r2.RowIndex, "Packing Grid");
					Conversiondetail.ChargeTo = Conversion.ToString((object)r2.Cells["ChargeTo"].Text);
					FormHelper.ValidateField(Conversiondetail.ChargeTo, "Charge To", r2.RowIndex, "Packing Grid");
					Conversiondetail.ContractScheduleId = Conversion.ToInt(r2.Cells["ContractScheduleId"].Value);
					Conversiondetail.ContractScheduleNo = Conversion.ToString((object)r2.Cells["ContractScheduleId"].Text);
					DataRow row = dtScheduleData.AsEnumerable().FirstOrDefault((DataRow rrr) => rrr.Field<int>("Id") == Conversiondetail.ContractScheduleId);
					if (row != null)
					{
						Conversiondetail.ExImInvoiceId = row.Field<int>("InvoiceId");
					}
					stockConversion.invStockConversionPackings.Add(Conversiondetail);
				}
			}
			rows = grdOH.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ChartofAccount"].Value.ToString()) != 0)
				{
					InvStockConversionAddExpense Expensedetail = new InvStockConversionAddExpense();
					if (!(r.Cells["ChartofAccount"].Text != string.Empty))
					{
						MessageBox.Show("ChartofAccount Field Require ");
						return;
					}
					Expensedetail.ChartOfAccountId = Conversion.ToInt(r.Cells["ChartofAccount"].Value);
					Expensedetail.BrandItemId = Conversion.ToInt(r.Cells["BrandItemId"].Value);
					Expensedetail.BrandItemUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					if (Expensedetail.BrandItemId > 0 && Expensedetail.BrandItemUomId == 0)
					{
						MessageBox.Show("Brand Uom Field Require for Brand Item:" + Conversion.ToString(r.Cells["BrandItem"].Value));
						return;
					}
					Expensedetail.LedgerRemarks = Conversion.ToString((object)r.Cells["LedgerRemarks"].Text);
					if (!(r.Cells["ExpAmount"].Text != string.Empty))
					{
						MessageBox.Show("ExpAmount Field Require ");
						return;
					}
					Expensedetail.ExpAmount = Conversion.ToDouble((object)r.Cells["ExpAmount"].Text);
					if (!(r.Cells["ChargeTo"].Text != string.Empty))
					{
						MessageBox.Show("Charge To is Required");
						return;
					}
					Expensedetail.ChargeTo = Conversion.ToString((object)r.Cells["ChargeTo"].Text);
					stockConversion.invStockConversionAddExpenses.Add(Expensedetail);
				}
			}
			if (stockConversion.invStockConversionPackings.Count == 0)
			{
				if (PmCompulsoryForStop)
				{
					MessageBox.Show("Packing Material Detail is Compulsory", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (PmCompulsoryForWarning)
				{
					if (RecId > 0)
					{
						if (MessageBox.Show("Are you sure to Update?No Packing Material Detail Added", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							return;
						}
					}
					else if (MessageBox.Show("Are you sure to Save?No Packing Material Detail Added", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
				}
			}
			CheckExpenseandPackingMaterialAmountTotal();
			if (checkPackingMaterialAmountTotal)
			{
				MessageBox.Show("Packing Material Amount not Equal By Product PMAmount");
				return;
			}
			if (CheckExpenseAmountTotal)
			{
				MessageBox.Show("Expenses Amount not Equal ByProduct Expense Amount");
				return;
			}
			if (ContractWagesChargetoProductForStockConversion && stockConversion.ConversionTypeId != 5)
			{
				stockConversion.InvContractorWagesBillHeaderList.Add(AddWagesListInInsert());
				stockConversion.InvContractorWagesBillHeaderList.Add(AddWagesListInInsert(Input: true));
			}
			int code = InvStockConversion.Save(stockConversion);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully " + stockConversion.DocSrNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully " + stockConversion.DocSrNo);
				UpdateMode = false;
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int i = 0; i < AT.RemovedAttachmentListInUpdateCase.Count; i++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[i].Path, FileMode.Open, FileAccess.ReadWrite))
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
								File.Delete(AT.RemovedAttachmentListInUpdateCase[i].Path);
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
			if (!ContractWagesChargetoProductForStockConversion && stockConversion.ConversionTypeId != 5 && WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 66;
				frmwagesBillHeader.RefDocId = code;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			if (chkPrint.Checked)
			{
				CommonServices.StockConversionSummary_605(code);
			}
			RefreshForm();
			grdInput.ClearStructure();
			summeryreset();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
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

	public void ReadById(int Id)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			RefreshForm();
			resetDetail();
			summeryreset();
			InvStockConversion SC = new InvStockConversion();
			RecId = Id;
			InputDetailRowsRemoveIds = "";
			SC = InvStockConversion.GetById(RecId);
			tabControl1.SelectedIndex = 0;
			if (SC != null)
			{
				btnDelete.Visible = true;
				txtdocnumber.Text = Conversion.ToString((object)SC.DocSrNo);
				txtProductionNo.Text = Conversion.ToString((object)SC.ProductionNo);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtDocdate.Tag = SC.DocDate;
				txtRemarks.Text = Conversion.ToString((object)SC.Remarks);
				cmbParentCategory.Value = Conversion.ToInt((object)SC.parentCategoryId);
				CmbConversionType.Value = Conversion.ToInt((object)SC.ConversionTypeId);
				if (Conversion.ToInt(CmbConversionType.Value) == 0)
				{
					CmbConversionType.Value = 1;
				}
				if (Conversion.ToInt(CmbConversionType.Value) == 3)
				{
					CmbDifferenceAccount.Value = SC.DifferenceAccountId;
				}
				if (SC.EBDepartmentId > 0)
				{
					CmbProductionDepartment.Value = SC.EBDepartmentId;
				}
				CmbConversionType_Leave(null, null);
				dtPackingMaterial.Clear();
				if (SC.invStockConversionPackings.Count > 0)
				{
					for (int k = 0; k < SC.invStockConversionPackings.Count; k++)
					{
						dtPackingMaterial.Rows.Add(SC.invStockConversionPackings[k].ItemId, SC.invStockConversionPackings[k].WarehouseId, SC.invStockConversionPackings[k].WarehouseName, SC.invStockConversionPackings[k].RackId, SC.invStockConversionPackings[k].rackName, SC.invStockConversionPackings[k].ItemConditionId, SC.invStockConversionPackings[k].BrandItemId, SC.invStockConversionPackings[k].BrandName, SC.invStockConversionPackings[k].BrandItemUomId, SC.invStockConversionPackings[k].BrandUom, SC.invStockConversionPackings[k].ItemQty, SC.invStockConversionPackings[k].ItemRate, SC.invStockConversionPackings[k].ItemAmount, SC.invStockConversionPackings[k].ChargeTo, SC.invStockConversionPackings[k].ContractScheduleId);
					}
				}
				else
				{
					AddRowInPackingMaterialGrid();
				}
				grdPM.DataSource = dtPackingMaterial;
				grdPM.RetrieveStructure();
				grdPackingMaterialSettings();
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				dtExpense.Clear();
				if (SC.invStockConversionAddExpenses.Count > 0)
				{
					for (int j = 0; j < SC.invStockConversionAddExpenses.Count; j++)
					{
						dtExpense.Rows.Add(SC.invStockConversionAddExpenses[j].ChartOfAccountId, SC.invStockConversionAddExpenses[j].BrandItemId, SC.invStockConversionAddExpenses[j].BrandName, SC.invStockConversionAddExpenses[j].BrandItemUomId, SC.invStockConversionAddExpenses[j].BrandUom, SC.invStockConversionAddExpenses[j].LedgerRemarks, SC.invStockConversionAddExpenses[j].ExpAmount, SC.invStockConversionAddExpenses[j].ChargeTo);
					}
				}
				else
				{
					AddRowInExpenseGrid();
				}
				grdOH.DataSource = dtExpense;
				grdOH.RetrieveStructure();
				grdConversionExpenseSettings();
				table.Clear();
				tableByProduct.Clear();
				double AmountWithoutExpense = 0.0;
				int issueLineId = 1;
				int outputLineId = 1;
				for (int i = 0; i < SC.invStockConversionDetails.Count; i++)
				{
					int lineId = SC.invStockConversionDetails[i].LineId;
					if (Conversion.ToInt((object)SC.ConversionTypeId) == 5)
					{
						lineId = ((!(SC.invStockConversionDetails[i].EntryType == "Issue")) ? outputLineId++ : issueLineId++);
					}
					if (SC.invStockConversionDetails[i].EntryType == "Issue")
					{
						table.Rows.Add(SC.invStockConversionDetails[i].Id, SC.invStockConversionDetails[i].RefDocumentTypeId, SC.invStockConversionDetails[i].RefDocNoId, SC.invStockConversionDetails[i].RefDocSubId, SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].CropBatch, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UomCode, SC.invStockConversionDetails[i].UOMDescription, SC.invStockConversionDetails[i].PackingtypeId, SC.invStockConversionDetails[i].PackTypeDesc, 0, 0, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Weight, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].RateUOMId, SC.invStockConversionDetails[i].RateUom, SC.invStockConversionDetails[i].Equivalent, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Moisture, SC.invStockConversionDetails[i].MoistureSlabId, SC.invStockConversionDetails[i].Remarks, lineId, SC.invStockConversionDetails[i].labIPmActivityLogId);
					}
					else if (SC.invStockConversionDetails[i].EntryType != "Issue")
					{
						AmountWithoutExpense = SC.invStockConversionDetails[i].Amount - (SC.invStockConversionDetails[i].PackingMaterialAmount + SC.invStockConversionDetails[i].ExpenseAmount);
						chkFumigationOnHold.Checked = SC.invStockConversionDetails[i].IsOnHold;
						tableByProduct.Rows.Add(SC.invStockConversionDetails[i].Id, SC.invStockConversionDetails[i].EntryType, SC.invStockConversionDetails[i].WarehouseId, SC.invStockConversionDetails[i].WareHouseName, SC.invStockConversionDetails[i].CropBatch, SC.invStockConversionDetails[i].ItemId, SC.invStockConversionDetails[i].ItemName, SC.invStockConversionDetails[i].JobLotId, SC.invStockConversionDetails[i].JobLotDescription, SC.invStockConversionDetails[i].ItemUomId, SC.invStockConversionDetails[i].UomCode, SC.invStockConversionDetails[i].UOMDescription, SC.invStockConversionDetails[i].PackingtypeId, SC.invStockConversionDetails[i].PackTypeDesc, SC.invStockConversionDetails[i].Qty, SC.invStockConversionDetails[i].Weight, SC.invStockConversionDetails[i].Rate, SC.invStockConversionDetails[i].RateUOMId, SC.invStockConversionDetails[i].RateUom, SC.invStockConversionDetails[i].Equivalent, AmountWithoutExpense, SC.invStockConversionDetails[i].Moisture, SC.invStockConversionDetails[i].MoistureSlabId, SC.invStockConversionDetails[i].MoistureSlabDescription, SC.invStockConversionDetails[i].PackingMaterialAmount, SC.invStockConversionDetails[i].ItemPmCost, SC.invStockConversionDetails[i].ExpenseAmount, SC.invStockConversionDetails[i].ItemOhCost, SC.invStockConversionDetails[i].Amount, SC.invStockConversionDetails[i].Remarks, Conversion.ToDouble((object)SC.invStockConversionDetails[i].IssueWeight), lineId, SC.invStockConversionDetails[i].WagesAmount, SC.invStockConversionDetails[i].labIPmActivityLogId);
					}
				}
				grdInput.DataSource = table;
				grdInput.RetrieveStructure();
				gridsettings();
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				gridsettingsByProduct();
				proportionatedPackingMaterialAmountInOutputGrid();
				proportionateOverheadGrid();
				if (Conversion.ToInt(CmbConversionType.Value) != 3)
				{
					btnGenerate_Click(null, null);
				}
				VoucherHeadIdGet();
				GenerateSummaryForUser();
				if (ContractWagesChargetoProductForStockConversion)
				{
					WagesDetailReadbyId(RecId);
					WagesGridFillFromInputOutput(grdByProduct);
					WagesGridFillFromInputOutput(grdInput, InputGrid: true);
				}
			}
			else
			{
				dtExpense.Clear();
				grdOH.ClearStructure();
				grdhistoryInputOutputDetail.ClearStructure();
				table.Clear();
				grdPM.ClearStructure();
				dtPackingMaterial.Clear();
				summeryreset();
			}
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
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
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			if (formright.DoHaveCanDelete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice val = new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 66
					};
					if (RecId <= 0)
					{
						throw new Exception("RecordId Not Found.....");
					}
					val.Id = RecId;
					val.EntryUser = UserAccount.ID;
					InvPurchaseInvoice.RemoveByID(val);
					MessageBox.Show("Delete Record Successfully");
					RefreshForm();
				}
				return;
			}
			throw new Exception("Record cannot be delete because you don't have right....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private InvContractorWagesBillHeader AddWagesListInInsert(bool Input = false)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Expected O, but got Unknown
		try
		{
			InvContractorWagesBillHeader obj = new InvContractorWagesBillHeader();
			if (WagesAmountCalculateOnQty)
			{
				ValidationforRefRowQty(dtdetail, "Regular_Wages");
				ValidationforRefRowQty(dtStiching, "Other_Wages");
			}
			else
			{
				ValidationforRefRowWeight(dtdetail, "Regular_Wages");
				ValidationforRefRowWeight(dtStiching, "Other_Wages");
			}
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			obj.DocDate = txtDocdate.Value;
			obj.DocumentTypeId = 101;
			obj.RefDocumentTypeId = 66;
			obj.RefDocNoId = RecId;
			obj.RefDocNo = Conversion.ToInt((object)txtdocnumber.Text);
			obj.OtherRemarks = Conversion.ToString((object)txtRemarks.Text);
			obj.EntryUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ModifyUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ScaleSlipNo = 0;
			obj.RefDocument = (Input ? "Issue" : "Recovery");
			obj.ModifyDate = DateTime.Now;
			obj.EntryDate = DateTime.Now;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.IsAproved = false;
			obj.invContractWagesBillDateil = new List<InvContractorWagesBillDetail>();
			double Qty = 0.0;
			double weight = 0.0;
			IEnumerable<GridEXRow> enumerable = from r in grdwagesDetail.GetDataRows()
				where Convert.ToString(r.Cells["TransactionType"].Value) == Convert.ToString(obj.RefDocument)
				select r;
			enumerable.Count();
			foreach (GridEXRow r3 in enumerable)
			{
				InvContractorWagesBillDetail wd = new InvContractorWagesBillDetail();
				obj.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
				wd.RefDocDate = Conversion.ToDateTime((object)txtDocdate.Value);
				if (obj.DocDate.Date < wd.RefDocDate.Date)
				{
					throw new Exception("Doc Date Can't be " + obj.DocDate.Date.ToString() + " Because Detail Date " + wd.RefDocDate.ToString() + " is Greater");
				}
				if (Conversion.ToInt(r3.Cells["WagesId"].Value) == 0 || r3.Cells["WagesId"].Text.ToString() == string.Empty)
				{
					throw new Exception("WagesAccount Field required In Regular Wages Grid...");
				}
				wd.InvConractorWagesAccountsId = Conversion.ToInt(r3.Cells["WagesId"].Value);
				if (Conversion.ToInt(r3.Cells["Quantity"].Value) == 0 || r3.Cells["Quantity"].Text.ToString() == string.Empty)
				{
					throw new Exception("Quantity Field required In Regular Wages Grid...");
				}
				wd.Qty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
				wd.BillQty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
				Qty += wd.Qty;
				if (Conversion.ToDouble(r3.Cells["Weight"].Value) == 0.0 || r3.Cells["Weight"].Text.ToString() == string.Empty)
				{
					throw new Exception("Weight Field required In Regular Wages Grid...");
				}
				wd.Weight = Conversion.ToDouble((object)r3.Cells["Weight"].Text);
				wd.BillWeight = Conversion.ToDouble(r3.Cells["Weight"].Value);
				weight += wd.Weight;
				if (Conversion.ToDouble(r3.Cells["PackSize"].Value) == 0.0 || r3.Cells["PackSize"].Text.ToString() == string.Empty)
				{
					throw new Exception("PackSize Field required In Regular Wages Grid...");
				}
				wd.PackSize = Conversion.ToDouble((object)r3.Cells["PackSize"].Text);
				if (Conversion.ToString((object)r3.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)r3.Cells["WarehouseType"].Text) == "Dryer")
				{
					wd.FreeOfCost = true;
					wd.WageRate = 0.0;
					wd.RateAddLess = 0.0;
					wd.WagesAmount = 0.0;
					wd.InvContractorWagesScheduleId = 0;
				}
				else
				{
					wd.WageRate = Conversion.ToDouble((object)r3.Cells["Rate"].Text);
					wd.InvContractorWagesScheduleId = Conversion.ToInt(r3.Cells["WagesScheduleId"].Value);
					wd.RateAddLess = Conversion.ToDouble((object)r3.Cells["RateAddLess"].Text);
					if (wd.WageRate == 0.0)
					{
						throw new Exception("Rate Field required In Regular Wages Grid...");
					}
				}
				if (Conversion.ToString((object)r3.Cells["WagesType"].Text) == "Free Of Cost")
				{
					wd.FreeOfCost = true;
				}
				else
				{
					wd.FreeOfCost = false;
				}
				if (wd.FreeOfCost)
				{
					wd.WagesAmount = 0.0;
				}
				else if (WagesAmountCalculateOnQty)
				{
					wd.WagesAmount = Math.Round(wd.Qty * wd.WageRate, 2);
				}
				else
				{
					wd.WagesAmount = Math.Round(wd.Weight / wd.PackSize * wd.WageRate, 2);
				}
				if (Conversion.ToInt(r3.Cells["SupplierId"].Value) == 0 || r3.Cells["SupplierId"].Text.ToString() == string.Empty)
				{
					throw new Exception("ContractorAccount Field required In Regular Wages Grid...");
				}
				wd.ContractorId = Conversion.ToInt(r3.Cells["SupplierId"].Value);
				if (Conversion.ToInt(r3.Cells["ItemId"].Value) == 0 || r3.Cells["ItemId"].Text.ToString() == string.Empty)
				{
					throw new Exception("Item Name Field required In Regular Wages Grid...");
				}
				wd.ItemId = Conversion.ToInt((object)r3.Cells["ItemId"].Text);
				wd.ItemName = Conversion.ToString(r3.Cells["Item"].Value);
				wd.CompanyName = Conversion.ToString((object)r3.Cells["SupplierId"].Text);
				wd.WagesAccountName = Conversion.ToString((object)r3.Cells["WagesId"].Text);
				wd.WagesTypeId = Conversion.ToInt(r3.Cells["WagesType"].Value);
				wd.JobLotId = Conversion.ToInt((object)r3.Cells["JobLotId"].Text);
				wd.Crop = Conversion.ToString((object)r3.Cells["Crop"].Text);
				wd.InvPackingTypeId = Conversion.ToInt((object)r3.Cells["packingTypeId"].Text);
				wd.WareHouseFromId = Conversion.ToInt((object)r3.Cells["MoveFromId"].Text);
				wd.WareHouseToId = Conversion.ToInt((object)r3.Cells["MoveToId"].Text);
				wd.BillQty = Conversion.ToDouble((object)r3.Cells["BillQty"].Text);
				wd.WeightCut = Conversion.ToDouble((object)r3.Cells["WeightCut"].Text);
				wd.RefDocQty = Conversion.ToDouble((object)r3.Cells["RefDocQty"].Text);
				wd.RefDocWeight = Conversion.ToDouble((object)r3.Cells["RefDocWeight"].Text);
				wd.RefLineId = Conversion.ToInt((object)r3.Cells["RefLineId"].Text);
				obj.invContractWagesBillDateil.Add(wd);
			}
			IEnumerable<GridEXRow> filterStichingGrid = from r in grdStiching.GetDataRows()
				where Convert.ToString(r.Cells["TransactionType"].Value) == Convert.ToString(obj.RefDocument)
				select r;
			if (!filterStichingGrid.Any() && StichingWagesCompulsory && Conversion.ToString((object)obj.RefDocument) != "Issue")
			{
				bool RowExistWithUomLessthan100 = false;
				GridEXRow[] rows = grdwagesDetail.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToDouble((object)rows[i].Cells["PackSize"].Text) < 100.0)
					{
						RowExistWithUomLessthan100 = true;
						break;
					}
				}
				if (RowExistWithUomLessthan100)
				{
					throw new Exception("Stitching Wages Grid... Record Not Found\n Stitching Wages Compulsory Configuration is On");
				}
			}
			if (filterStichingGrid.Any())
			{
				foreach (GridEXRow r2 in filterStichingGrid)
				{
					if (Conversion.ToString((object)obj.RefDocument) == "Issue" || (!StichingWagesCompulsory && (Conversion.ToInt(r2.Cells["WagesId"].Value) == 0 || Conversion.ToString((object)r2.Cells["WagesId"].Text.Trim()) == string.Empty) && (Conversion.ToInt(r2.Cells["SupplierId"].Value) == 0 || Conversion.ToString((object)r2.Cells["SupplierId"].Text.Trim()) == string.Empty)))
					{
						continue;
					}
					string SecondGridName = "Other Wages Grid ...";
					InvContractorWagesBillDetail wds = new InvContractorWagesBillDetail();
					if (Conversion.ToInt(r2.Cells["WagesId"].Value) == 0 || r2.Cells["WagesId"].Text.ToString() == string.Empty)
					{
						throw new Exception("WagesAccount Field required In " + SecondGridName);
					}
					wds.InvConractorWagesAccountsId = Conversion.ToInt(r2.Cells["WagesId"].Value);
					if (Conversion.ToInt(r2.Cells["Quantity"].Value) == 0 || r2.Cells["Quantity"].Text.ToString() == string.Empty)
					{
						throw new Exception("Quantity Field required In " + SecondGridName);
					}
					wds.Qty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
					wds.BillQty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
					Qty += wds.Qty;
					if (Conversion.ToDouble(r2.Cells["Weight"].Value) == 0.0 || r2.Cells["Weight"].Text.ToString() == string.Empty)
					{
						throw new Exception("Weight Field required In " + SecondGridName);
					}
					wds.Weight = Conversion.ToDouble((object)r2.Cells["Weight"].Text);
					wds.BillWeight = Conversion.ToDouble(r2.Cells["Weight"].Value);
					weight += wds.Weight;
					if (Conversion.ToDouble(r2.Cells["PackSize"].Value) == 0.0 || r2.Cells["PackSize"].Text.ToString() == string.Empty)
					{
						throw new Exception("PackSize Field Required In " + SecondGridName);
					}
					wds.PackSize = Conversion.ToDouble((object)r2.Cells["PackSize"].Text);
					if (Conversion.ToString((object)r2.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)r2.Cells["WarehouseType"].Text) == "Dryer")
					{
						wds.FreeOfCost = true;
						wds.WageRate = 0.0;
						wds.RateAddLess = 0.0;
						wds.WagesAmount = 0.0;
						wds.InvContractorWagesScheduleId = 0;
					}
					else
					{
						wds.WageRate = Conversion.ToDouble((object)r2.Cells["Rate"].Text);
						wds.InvContractorWagesScheduleId = Conversion.ToInt(r2.Cells["WagesScheduleId"].Value);
						wds.RateAddLess = Conversion.ToDouble((object)r2.Cells["RateAddLess"].Text);
						if (wds.WageRate == 0.0)
						{
							throw new Exception("Rate Field required In " + SecondGridName);
						}
						if (WagesAmountCalculateOnQty)
						{
							wds.WagesAmount = Math.Round(wds.Qty * wds.WageRate, 2);
						}
						else
						{
							wds.WagesAmount = Math.Round(wds.Weight / wds.PackSize * wds.WageRate, 2);
						}
					}
					if (Conversion.ToInt(r2.Cells["SupplierId"].Value) == 0 || r2.Cells["SupplierId"].Text.ToString() == string.Empty)
					{
						throw new Exception("ContractorAccount Field required In " + SecondGridName);
					}
					wds.ContractorId = Conversion.ToInt(r2.Cells["SupplierId"].Value);
					if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0 || r2.Cells["ItemId"].Text.ToString() == string.Empty)
					{
						throw new Exception("Item Name Field required In " + SecondGridName);
					}
					wds.ItemId = Conversion.ToInt((object)r2.Cells["ItemId"].Text);
					wds.ItemName = Conversion.ToString(r2.Cells["Item"].Value);
					wds.CompanyName = Conversion.ToString((object)r2.Cells["SupplierId"].Text);
					wds.WagesAccountName = Conversion.ToString((object)r2.Cells["WagesId"].Text);
					wds.RefDocDate = Conversion.ToDateTime(r2.Cells["Date"].Value);
					wds.JobLotId = Conversion.ToInt((object)r2.Cells["JobLotId"].Text);
					wds.Crop = Conversion.ToString((object)r2.Cells["Crop"].Text);
					wds.InvPackingTypeId = Conversion.ToInt((object)r2.Cells["packingTypeId"].Text);
					wds.WareHouseFromId = Conversion.ToInt((object)r2.Cells["MoveFromId"].Text);
					wds.WareHouseToId = Conversion.ToInt((object)r2.Cells["MoveToId"].Text);
					wds.BillQty = Conversion.ToInt((object)r2.Cells["BillQty"].Text);
					wds.WeightCut = Conversion.ToDouble((object)r2.Cells["WeightCut"].Text);
					wds.RefDocQty = Conversion.ToDouble((object)r2.Cells["RefDocQty"].Text);
					wds.RefDocWeight = Conversion.ToDouble((object)r2.Cells["RefDocWeight"].Text);
					wds.RefLineId = Conversion.ToInt((object)r2.Cells["RefLineId"].Text);
					if (Conversion.ToString((object)r2.Cells["WagesType"].Text) == "Free Of Cost")
					{
						wds.FreeOfCost = true;
					}
					else
					{
						wds.FreeOfCost = false;
					}
					wds.WagesTypeId = 2;
					obj.invContractWagesBillDateil.Add(wds);
				}
			}
			if (Input)
			{
				obj.QtyTotal = GridEX_Helper.GetColumnSum(grdInput, "Quantity");
				obj.WeightTotal = GridEX_Helper.GetColumnSum(grdInput, "Weight");
			}
			else
			{
				obj.QtyTotal = GridEX_Helper.GetColumnSum(grdByProduct, "Quantity");
				obj.WeightTotal = GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
			}
			return obj;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WagesDetailReadbyId(int id)
	{
		try
		{
			DataTable dt = InvContractorWagesBillHeader.InvContractorWagesBillHeader_DetailByRefDocument(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, 66, id);
			if (dt == null || dt.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow row in dt.Rows)
			{
				int num = Conversion.ToInt(row["WagesTypeId"]);
				DataRow targetRow = ((num == 2) ? dtStiching.NewRow() : dtdetail.NewRow());
				targetRow["SupplierId"] = row["ContractorId"];
				targetRow["ContractorName"] = row["CompanyName"];
				targetRow["WagesId"] = row["InvConractorWagesAccountsId"];
				targetRow["WagesAccount"] = row["WagesAccountName"];
				targetRow["WagesType"] = (Conversion.ToBool(row["FreeOfCost"]) ? "Free Of Cost" : "Regular");
				targetRow["Date"] = row["RefDocDate"];
				targetRow["packingTypeId"] = row["InvPackingTypeId"];
				targetRow["packingType"] = row["PackTypeDesc"];
				targetRow["Weight"] = Conversion.ToDouble(row["Weight"]);
				targetRow["PackSize"] = row["PackSize"];
				targetRow["Quantity"] = Conversion.ToDouble(row["Qty"]);
				targetRow["WeightCut"] = row["WeightCut"];
				targetRow["BillQty"] = Conversion.ToDouble(row["BillQty"]);
				targetRow["BillWeight"] = Conversion.ToDouble(row["BillWeight"]);
				targetRow["RateWithoutAddLess"] = row["WageRate"];
				targetRow["RateAddLess"] = Conversion.ToDouble(row["RateAddLess"]);
				targetRow["Rate"] = row["WageRate"];
				targetRow["Amount"] = Conversion.ToDouble(row["WagesAmount"]);
				targetRow["ItemId"] = row["ItemId"];
				targetRow["Item"] = row["ItemName"];
				targetRow["jobLotId"] = row["JobLotId"];
				targetRow["jobLot"] = row["JobLotDescription"];
				targetRow["Crop"] = row["Crop"];
				targetRow["MoveFromId"] = row["WareHouseFromId"];
				targetRow["MoveFrom"] = row["WareHouseFrom"];
				targetRow["MoveToId"] = row["WareHouseToId"];
				targetRow["MoveTo"] = row["WareHouseTo"];
				targetRow["PurchaseGLAC"] = row["PurchaseGLAC"];
				targetRow["WarehouseType"] = row["WarehouseType"];
				targetRow["RefDocQty"] = Conversion.ToDouble(row["RefDocQty"]);
				targetRow["RefDocWeight"] = Conversion.ToDouble(row["RefDocWeight"]);
				targetRow["RefLineId"] = Conversion.ToInt(row["RefLineId"]);
				targetRow["WagesScheduleId"] = row["InvContractorWagesScheduleId"];
				targetRow["TransactionType"] = row["TransactionType"];
				if (num == 2)
				{
					dtStiching.Rows.Add(targetRow);
				}
				else
				{
					dtdetail.Rows.Add(targetRow);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception($"Error loading Wages details for ID {id}", ex);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			drdocdate.Checked = true;
			grdHistoryMain.ClearStructure();
			grdhistoryInputOutputDetail.ClearStructure();
			grdHistoryPM.ClearStructure();
			grdHistoryOH.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 66;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
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
			else if (RdApproveDate.Checked)
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
			DataTable dth = InvStockConversion.FormHistoryNew(obj);
			if (dth.Rows.Count > 0)
			{
				DataTable dataTable = dtHistory;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtHistory.Columns.Add("DocNo", typeof(int));
					dtHistory.Columns.Add("DocDate", typeof(DateTime));
					dtHistory.Columns.Add("ProductionNo", typeof(string));
					dtHistory.Columns.Add("Remarks", typeof(string));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Clear();
				foreach (DataRow row in dth.Rows)
				{
					dtHistory.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["Remarks"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["PostDate"], row["ApprovedUser"], row["NoOfAttachments"]);
				}
				grdHistoryMain.DataSource = dtHistory;
				grdHistoryMain.RetrieveStructure();
				GridHistoryMainSetting();
			}
			else
			{
				grdHistoryMain.ClearStructure();
				grdhistoryInputOutputDetail.ClearStructure();
				grdHistoryOH.ClearStructure();
				grdHistoryPM.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryMainSetting()
	{
		try
		{
			grdHistoryMain.RootTable.Columns["Id"].Visible = false;
			grdHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistoryMain.RootTable.Columns["NoOfAttachments"].Visible = false;
			grdHistoryMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryMain, 2, 3, null, null, null, ColumnSetting: false);
			grdHistoryMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistoryMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdHistoryMain.RootTable.Columns["ProductionNo"].Width = Constants.InventoryConstants.ProductionNo;
			grdHistoryMain.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdHistoryMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistoryMain.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistoryMain.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistoryMain.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistoryMain.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistoryMain.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistoryMain.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdHistoryMain.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdHistoryMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistoryMain.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistoryMain.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistoryMain.RootTable.Columns.Add("Edit");
			grdHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
			grdHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistoryMain.RootTable.Columns["Edit"].Width = 35;
			grdHistoryMain.RootTable.Columns["Edit"].Position = 1;
			grdHistoryMain.RootTable.Columns.Add("Print");
			grdHistoryMain.RootTable.Columns["Print"].Key = "Print";
			grdHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			grdHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistoryMain.RootTable.Columns["Print"].Width = 45;
			grdHistoryMain.RootTable.Columns["Print"].Position = 0;
			grdHistoryMain.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryMain_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.StockConversionSummary_605(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				BindDetailsByHeaderId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailsByHeaderId(int Id)
	{
		try
		{
			InvStockConversion SC = InvStockConversion.GetById(Id);
			if (SC != null && SC.invStockConversionDetails.Count > 0)
			{
				DataTable dt = new DataTable();
				DataTable dtpack = new DataTable();
				DataTable dtExp = new DataTable();
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("CropBatch");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("PackType");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("PMAmount", typeof(double));
				dt.Columns.Add("ItemPMAmount", typeof(double));
				dt.Columns.Add("ExpAmount", typeof(double));
				dt.Columns.Add("ItemExpAmount", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks");
				for (int k = 0; k < SC.invStockConversionDetails.Count; k++)
				{
					dt.Rows.Add(SC.invStockConversionDetails[k].EntryType, SC.invStockConversionDetails[k].WareHouseName, SC.invStockConversionDetails[k].CropBatch, SC.invStockConversionDetails[k].ItemName, SC.invStockConversionDetails[k].JobLotDescription, SC.invStockConversionDetails[k].UomCode, SC.invStockConversionDetails[k].PackTypeDesc, SC.invStockConversionDetails[k].Qty, SC.invStockConversionDetails[k].Weight, SC.invStockConversionDetails[k].Rate, SC.invStockConversionDetails[k].RateUom, SC.invStockConversionDetails[k].PackingMaterialAmount, SC.invStockConversionDetails[k].ItemPmCost, SC.invStockConversionDetails[k].ExpenseAmount, SC.invStockConversionDetails[k].ItemOhCost, SC.invStockConversionDetails[k].Amount, SC.invStockConversionDetails[k].Remarks);
				}
				grdhistoryInputOutputDetail.DataSource = dt;
				grdhistoryInputOutputDetail.RetrieveStructure();
				GridHistoryInputOutputDetailSetting();
				dtpack.Columns.Add("WarehouseName");
				dtpack.Columns.Add("Item");
				dtpack.Columns.Add("BrandItem");
				dtpack.Columns.Add("BrandUom");
				dtpack.Columns.Add("ItemQTY", typeof(double));
				dtpack.Columns.Add("Rate", typeof(double));
				dtpack.Columns.Add("Amount", typeof(double));
				dtpack.Columns.Add("ChargeTo");
				dtpack.Columns.Add("ContractScheduleNo");
				for (int j = 0; j < SC.invStockConversionPackings.Count; j++)
				{
					dtpack.Rows.Add(SC.invStockConversionPackings[j].WarehouseName, SC.invStockConversionPackings[j].ItemName, SC.invStockConversionPackings[j].BrandName, SC.invStockConversionPackings[j].BrandUom, SC.invStockConversionPackings[j].ItemQty, SC.invStockConversionPackings[j].ItemRate, SC.invStockConversionPackings[j].ItemAmount, SC.invStockConversionPackings[j].ChargeTo, SC.invStockConversionPackings[j].ContractScheduleNo);
				}
				grdHistoryPM.DataSource = dtpack;
				grdHistoryPM.RetrieveStructure();
				GridHistoryPMSetting();
				dtExp.Columns.Add("ChartofAccount");
				dtExp.Columns.Add("BrandItem");
				dtExp.Columns.Add("BrandUom");
				dtExp.Columns.Add("ExpAmount", typeof(double));
				dtExp.Columns.Add("LedgerRemarks");
				for (int i = 0; i < SC.invStockConversionAddExpenses.Count; i++)
				{
					dtExp.Rows.Add(SC.invStockConversionAddExpenses[i].AccountTitle, SC.invStockConversionAddExpenses[i].BrandName, SC.invStockConversionAddExpenses[i].BrandUom, SC.invStockConversionAddExpenses[i].ExpAmount, SC.invStockConversionAddExpenses[i].LedgerRemarks);
				}
				grdHistoryOH.DataSource = dtExp;
				grdHistoryOH.RetrieveStructure();
				GridHistoryOHSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryOHSetting()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryOH, 2, 3, null, null, null, ColumnSetting: false);
			grdHistoryOH.RootTable.Columns["ChartofAccount"].Width = Constants.InventoryConstants.AccountTitle + 40;
			grdHistoryOH.RootTable.Columns["BrandItem"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryOH.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant + 50;
			grdHistoryOH.RootTable.Columns["ExpAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryOH.RootTable.Columns["LedgerRemarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdHistoryOH.RootTable.Columns["ChartofAccount"].Width = 150;
			grdHistoryOH.RootTable.Columns["ExpAmount"].Width = 80;
			grdHistoryOH.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOH.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryOH.RootTable.Columns["ExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOH.RootTable.Columns["ExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryPMSetting()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryPM, 2, 3, null, null, null, ColumnSetting: false);
			grdHistoryPM.RootTable.Columns["ContractScheduleNo"].Caption = "Schedule / Invoice No";
			grdHistoryPM.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdHistoryPM.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryPM.RootTable.Columns["BrandItem"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryPM.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant + 30;
			grdHistoryPM.RootTable.Columns["ItemQTY"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryPM.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryPM.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryPM.RootTable.Columns["ChargeTo"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistoryPM.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdHistoryPM.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["ItemQTY"].FormatString = "#,##0.###";
			grdHistoryPM.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##0.###";
			grdHistoryPM.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryPM.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryPM.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryPM.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryInputOutputDetailSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdhistoryInputOutputDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistoryInputOutputDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdhistoryInputOutputDetail.RootTable.Columns["EntryType"].Width = 100;
			grdhistoryInputOutputDetail.RootTable.Columns["WareHouse"].Width = 90;
			grdhistoryInputOutputDetail.RootTable.Columns["CropBatch"].Width = 65;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemName"].Width = 130;
			grdhistoryInputOutputDetail.RootTable.Columns["JobLot"].Width = 100;
			grdhistoryInputOutputDetail.RootTable.Columns["PackUom"].Width = 60;
			grdhistoryInputOutputDetail.RootTable.Columns["PackType"].Width = 65;
			grdhistoryInputOutputDetail.RootTable.Columns["Qty"].Width = 70;
			grdhistoryInputOutputDetail.RootTable.Columns["Weight"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["Rate"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["RateUom"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["ExpAmount"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["PMAmount"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemExpAmount"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemPMAmount"].Width = 75;
			grdhistoryInputOutputDetail.RootTable.Columns["Amount"].Width = 100;
			grdhistoryInputOutputDetail.RootTable.Columns["Remarks"].Width = 150;
			grdhistoryInputOutputDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdhistoryInputOutputDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdhistoryInputOutputDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdhistoryInputOutputDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdhistoryInputOutputDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["Rate"].FormatString = "#,##0.###";
			grdhistoryInputOutputDetail.RootTable.Columns["ExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["ExpAmount"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["ExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["ExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["PMAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["PMAmount"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["PMAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["PMAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemExpAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemExpAmount"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemExpAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemExpAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemPMAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemPMAmount"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemPMAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["ItemPMAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryInputOutputDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdhistoryInputOutputDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryInputOutputDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCalculation()
	{
		try
		{
			if (((UltraGridBase)cmbRateUom).ActiveRow != null)
			{
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value);
				double rate = Conversion.ToDouble((object)txtRate.Text);
				if (rate > 0.0 && RateUom > 0.0 && Conversion.ToDouble((object)txtUnitWeight.Text) > 0.0)
				{
					double NetAmount = Conversion.ToDouble((object)txtUnitWeight.Text) / RateUom * rate;
					txtAmount.Text = Conversion.ToDouble((object)NetAmount).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtAmount.Text = 0.ToString();
				}
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbUOM_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			AmountCalculation();
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
			if (((UltraGridBase)cmbUOM).ActiveRow != null)
			{
				double ItemUom = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				double qty = Conversion.ToDouble((object)txtQty.Text);
				if (ItemUom > 0.0 && qty > 0.0)
				{
					double NetWeight = ItemUom * qty;
					txtUnitWeight.Text = Conversion.ToDouble((object)NetWeight).ToString("#,##0.###");
				}
				else
				{
					txtUnitWeight.Text = 0.ToString();
				}
			}
			else
			{
				txtUnitWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			AmountCalculation();
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
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtUnitWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void proportionatedPackingMaterialAmountInOutputGrid()
	{
		try
		{
			grdByProduct.UpdateData();
			Dictionary<string, double> weightOld = new Dictionary<string, double>();
			Dictionary<string, double> amountOld = new Dictionary<string, double>();
			Dictionary<string, double> weightBrand = new Dictionary<string, double>();
			Dictionary<string, double> amountBrand = new Dictionary<string, double>();
			if (grdByProduct.GetRows().Count() > 0 && grdPM.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow obj in rows)
				{
					string entryType3 = obj.Cells["EntryType"].Text;
					int itemId3 = Conversion.ToInt(obj.Cells["ItemId"].Value);
					int uomId3 = Conversion.ToInt(obj.Cells["ItemUOMId"].Value);
					double weight2 = Conversion.ToDouble(obj.Cells["Weight"].Value);
					string oldKey3 = entryType3 ?? "";
					string brandKey3 = $"{entryType3}_{itemId3}_{uomId3}";
					if (!weightOld.ContainsKey(oldKey3))
					{
						weightOld[oldKey3] = 0.0;
					}
					weightOld[oldKey3] += weight2;
					if (!weightBrand.ContainsKey(brandKey3))
					{
						weightBrand[brandKey3] = 0.0;
					}
					weightBrand[brandKey3] += weight2;
				}
				rows = grdPM.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					string entryType2 = r2.Cells["ChargeTo"].Text;
					int itemId2 = Conversion.ToInt(r2.Cells["BrandItemId"].Value);
					int uomId2 = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
					double amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					if (itemId2 == 0)
					{
						string oldKey2 = entryType2 ?? "";
						if (!amountOld.ContainsKey(oldKey2))
						{
							amountOld[oldKey2] = 0.0;
						}
						amountOld[oldKey2] += amount;
						continue;
					}
					if (uomId2 == 0)
					{
						throw new Exception("Packing Material grid row#:" + (r2.RowIndex + 1) + " must have Brand Uom");
					}
					string brandKey2 = $"{entryType2}_{itemId2}_{uomId2}";
					if (!amountBrand.ContainsKey(brandKey2))
					{
						amountBrand[brandKey2] = 0.0;
					}
					amountBrand[brandKey2] += amount;
				}
				rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					string entryType = r.Cells["EntryType"].Text;
					int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int uomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					double weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					string oldKey = entryType ?? "";
					string brandKey = $"{entryType}_{itemId}_{uomId}";
					double pmAmount = 0.0;
					double itemPMAmount = 0.0;
					if (amountBrand.ContainsKey(brandKey))
					{
						double totalAmt2 = amountBrand[brandKey];
						double totalWgt2 = (weightBrand.ContainsKey(brandKey) ? weightBrand[brandKey] : 0.0);
						if (totalWgt2 > 0.0)
						{
							itemPMAmount = totalAmt2 / totalWgt2 * weight;
						}
						r.Cells["ItemPMAmount"].Value = itemPMAmount;
					}
					else
					{
						r.Cells["ItemPMAmount"].Value = 0;
					}
					if (amountOld.ContainsKey(oldKey))
					{
						double totalAmt = amountOld[oldKey];
						double totalWgt = (weightOld.ContainsKey(oldKey) ? weightOld[oldKey] : 0.0);
						if (totalWgt > 0.0)
						{
							pmAmount = totalAmt / totalWgt * weight;
						}
						r.Cells["PMAmount"].Value = pmAmount;
					}
					else
					{
						r.Cells["PMAmount"].Value = 0;
					}
					r.EndEdit();
				}
			}
			else if (grdByProduct.GetRows().Count() > 0 && grdPM.GetRows().Count() == 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["PMAmount"].Value = 0.ToString();
					obj2.Cells["ItemPMAmount"].Value = 0.ToString();
					obj2.EndEdit();
				}
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckExpenseandPackingMaterialAmountTotal()
	{
		try
		{
			checkPackingMaterialAmountTotal = false;
			double PackingMaterialAmount = 0.0;
			double ExpenseAmount = 0.0;
			double ExpAmountBP = 0.0;
			double PMAmount = 0.0;
			PackingMaterialAmount = Conversion.ToDouble((object)grdPM.GetTotal(grdPM.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			ExpenseAmount = Conversion.ToDouble((object)grdOH.GetTotal(grdOH.RootTable.Columns["ExpAmount"], (AggregateFunction)2).ToString());
			ExpAmountBP = Conversion.ToDouble((object)grdByProduct.GetTotal(grdByProduct.RootTable.Columns["ExpAmount"], (AggregateFunction)2).ToString());
			PMAmount = Conversion.ToDouble((object)grdByProduct.GetTotal(grdByProduct.RootTable.Columns["PMAmount"], (AggregateFunction)2).ToString());
			double ItemExpAmount = Conversion.ToDouble((object)GridEX_Helper.GetColumnSum(grdByProduct, "ItemExpAmount"));
			double ItemPMAmount = Conversion.ToDouble((object)GridEX_Helper.GetColumnSum(grdByProduct, "ItemPMAmount"));
			ExpAmountBP += ItemExpAmount;
			PMAmount += ItemPMAmount;
			if (PackingMaterialAmount > 0.0 && PMAmount > 0.0 && PackingMaterialAmount != PMAmount)
			{
				if (PackingMaterialAmount > PMAmount)
				{
					if (PackingMaterialAmount - PMAmount > 5.0)
					{
						checkPackingMaterialAmountTotal = true;
					}
				}
				else if (PMAmount > PackingMaterialAmount && PMAmount - PackingMaterialAmount > 5.0)
				{
					checkPackingMaterialAmountTotal = true;
				}
			}
			if (!(ExpenseAmount > 0.0) || !(ExpAmountBP > 0.0) || ExpenseAmount == ExpAmountBP)
			{
				return;
			}
			if (ExpenseAmount > ExpAmountBP)
			{
				if (ExpenseAmount - ExpAmountBP > 5.0)
				{
					CheckExpenseAmountTotal = true;
				}
			}
			else if (ExpAmountBP > ExpenseAmount && ExpAmountBP - ExpenseAmount > 5.0)
			{
				CheckExpenseAmountTotal = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void proportionateOverheadGrid()
	{
		try
		{
			grdByProduct.UpdateData();
			Dictionary<string, double> weightOld = new Dictionary<string, double>();
			Dictionary<string, double> amountOld = new Dictionary<string, double>();
			Dictionary<string, double> weightBrand = new Dictionary<string, double>();
			Dictionary<string, double> amountBrand = new Dictionary<string, double>();
			if (grdByProduct.GetRows().Count() > 0 && grdOH.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow obj in rows)
				{
					string entryType3 = obj.Cells["EntryType"].Text;
					int itemId3 = Conversion.ToInt(obj.Cells["ItemId"].Value);
					int uomId3 = Conversion.ToInt(obj.Cells["ItemUOMId"].Value);
					double weight2 = Conversion.ToDouble(obj.Cells["Weight"].Value);
					string oldKey3 = entryType3 ?? "";
					string brandKey3 = $"{entryType3}_{itemId3}_{uomId3}";
					if (!weightOld.ContainsKey(oldKey3))
					{
						weightOld[oldKey3] = 0.0;
					}
					weightOld[oldKey3] += weight2;
					if (!weightBrand.ContainsKey(brandKey3))
					{
						weightBrand[brandKey3] = 0.0;
					}
					weightBrand[brandKey3] += weight2;
				}
				rows = grdOH.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					string entryType2 = r2.Cells["ChargeTo"].Text;
					int itemId2 = Conversion.ToInt(r2.Cells["BrandItemId"].Value);
					int uomId2 = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
					double amount = Conversion.ToDouble(r2.Cells["ExpAmount"].Value);
					if (itemId2 == 0)
					{
						string oldKey2 = entryType2 ?? "";
						if (!amountOld.ContainsKey(oldKey2))
						{
							amountOld[oldKey2] = 0.0;
						}
						amountOld[oldKey2] += amount;
						continue;
					}
					if (uomId2 == 0)
					{
						throw new Exception("Over Head grid row#:" + (r2.RowIndex + 1) + " must have Brand Uom");
					}
					string brandKey2 = $"{entryType2}_{itemId2}_{uomId2}";
					if (!amountBrand.ContainsKey(brandKey2))
					{
						amountBrand[brandKey2] = 0.0;
					}
					amountBrand[brandKey2] += amount;
				}
				rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					string entryType = r.Cells["EntryType"].Text;
					int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int uomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					double weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					string oldKey = entryType ?? "";
					string brandKey = $"{entryType}_{itemId}_{uomId}";
					double pmAmount = 0.0;
					double itemPMAmount = 0.0;
					if (amountBrand.ContainsKey(brandKey))
					{
						double totalAmt2 = amountBrand[brandKey];
						double totalWgt2 = (weightBrand.ContainsKey(brandKey) ? weightBrand[brandKey] : 0.0);
						if (totalWgt2 > 0.0)
						{
							itemPMAmount = totalAmt2 / totalWgt2 * weight;
						}
						r.Cells["ItemExpAmount"].Value = itemPMAmount;
					}
					else
					{
						r.Cells["ItemExpAmount"].Value = 0;
					}
					if (amountOld.ContainsKey(oldKey))
					{
						double totalAmt = amountOld[oldKey];
						double totalWgt = (weightOld.ContainsKey(oldKey) ? weightOld[oldKey] : 0.0);
						if (totalWgt > 0.0)
						{
							pmAmount = totalAmt / totalWgt * weight;
						}
						r.Cells["ExpAmount"].Value = pmAmount;
					}
					else
					{
						r.Cells["ExpAmount"].Value = 0;
					}
					r.EndEdit();
				}
			}
			else if (grdByProduct.GetRows().Count() > 0 && grdOH.GetRows().Count() == 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["ExpAmount"].Text = 0.ToString();
					obj2.Cells["ItemExpAmount"].Text = 0.ToString();
					obj2.EndEdit();
				}
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void handleAverageRateCalculation()
	{
		try
		{
			double totalAmountInInput = 0.0;
			double totalByProductAmountInOutput = 0.0;
			double totalPmAmount = 0.0;
			double totalExpAmount = 0.0;
			double totalItemPmAmount = 0.0;
			double totalItemExpAmount = 0.0;
			double totalHeadRiceWeightinOutput = 0.0;
			GridEXRow[] rows = grdInput.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				r3.BeginEdit();
				totalAmountInInput += Conversion.ToDouble(r3.Cells["Amount"].Value);
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				r2.BeginEdit();
				if (r2.Cells["EntryType"].Text == "Recovery By Product")
				{
					totalByProductAmountInOutput += Conversion.ToDouble(r2.Cells["Amount"].Value);
				}
				else if (r2.Cells["EntryType"].Text == "Recovery Head Rice")
				{
					totalPmAmount += Conversion.ToDouble(r2.Cells["PMAmount"].Value);
					totalExpAmount += Conversion.ToDouble(r2.Cells["ExpAmount"].Value);
					totalItemPmAmount += Conversion.ToDouble(r2.Cells["ItemPMAmount"].Value);
					totalItemExpAmount += Conversion.ToDouble(r2.Cells["ItemExpAmount"].Value);
					totalHeadRiceWeightinOutput += Conversion.ToDouble(r2.Cells["Weight"].Value);
				}
			}
			_ = Conversion.ToDouble((object)(Conversion.ToDouble((object)totalAmountInInput) + totalPmAmount + totalExpAmount + totalItemPmAmount + totalItemExpAmount - Conversion.ToDouble((object)totalByProductAmountInOutput))) / Conversion.ToDouble((object)totalHeadRiceWeightinOutput);
			double headRiceRateFor1KgWithoutExpese = Conversion.ToDouble((object)(Conversion.ToDouble((object)totalAmountInInput) - Conversion.ToDouble((object)totalByProductAmountInOutput))) / Conversion.ToDouble((object)totalHeadRiceWeightinOutput);
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (r.Cells["EntryType"].Text == "Recovery By Product")
				{
					double totalAmount = Conversion.ToDouble((object)r.Cells["AmountWithoutExpenses"].Text) + Conversion.ToDouble((object)r.Cells["PMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ExpAmount"].Text) + Conversion.ToDouble((object)r.Cells["ItemPMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ItemExpAmount"].Text) + Conversion.ToDouble((object)r.Cells["WagesAmount"].Text);
					r.Cells["Amount"].Value = Conversion.ToString((object)totalAmount);
				}
				else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
				{
					double rateWiehtoutExpeseAgainstUom = Conversion.ToDouble((object)headRiceRateFor1KgWithoutExpese) * Conversion.ToDouble((object)r.Cells["RateEquivalent"].Text);
					double amountWithoutExpense = Conversion.ToDouble((object)r.Cells["Weight"].Text) / Conversion.ToDouble((object)r.Cells["RateEquivalent"].Text) * Conversion.ToDouble((object)rateWiehtoutExpeseAgainstUom);
					r.Cells["AmountWithoutExpenses"].Value = Conversion.ToString((object)amountWithoutExpense);
					double amountWithExpenses = Conversion.ToDouble((object)amountWithoutExpense) + Conversion.ToDouble((object)r.Cells["PMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ExpAmount"].Text) + Conversion.ToDouble((object)r.Cells["ItemPMAmount"].Text) + Conversion.ToDouble((object)r.Cells["ItemExpAmount"].Text) + Conversion.ToDouble((object)r.Cells["WagesAmount"].Text);
					r.Cells["Amount"].Value = Conversion.ToString((object)amountWithExpenses);
					double FinishRatewithexpenses = Conversion.ToDouble((object)amountWithExpenses) / Conversion.ToDouble((object)r.Cells["Weight"].Text) * Conversion.ToDouble((object)r.Cells["RateEquivalent"].Text);
					r.Cells["Rate"].Value = Conversion.ToString((object)FinishRatewithexpenses);
				}
			}
			grdByProduct.UpdateData();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCropyr_Leave(object sender, EventArgs e)
	{
		if (((Control)(object)cmbEntryType).Text == "Issue")
		{
			GetAvgRate();
		}
	}

	private void cmbLot_Leave(object sender, EventArgs e)
	{
		if (((Control)(object)cmbEntryType).Text == "Issue")
		{
			GetCurrentItemStockByWarehouseByJobLotAndCropYear();
			GetAvgRate();
		}
	}

	private void cmbGodown_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GetAvgRate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateWagesAmountInOutputGrid()
	{
		try
		{
			grdByProduct.UpdateData();
			double totalWagesAmount = GridEX_Helper.GetColumnSumWithWhere(grdwagesDetail, "Amount", "WagesType", "Regular") + GridEX_Helper.GetColumnSumWithWhere(grdStiching, "Amount", "WagesType", "Regular");
			txtWagesAmount.Text = totalWagesAmount.ToString("#,##.##");
			double totalWeightInOutput = GridEX_Helper.GetColumnSum(grdByProduct, "Weight");
			GridEXRow[] byProductRows = grdByProduct.GetRows();
			int wagesDetailCount = grdwagesDetail.GetRows().Count();
			int stitchingCount = grdStiching.GetRows().Count();
			if (byProductRows.Count() == 0)
			{
				return;
			}
			GridEXRow[] array;
			if ((wagesDetailCount == 0 && stitchingCount == 0) || totalWeightInOutput <= 0.0)
			{
				array = byProductRows;
				foreach (GridEXRow obj in array)
				{
					obj.BeginEdit();
					obj.Cells["WagesAmount"].Value = "0.00";
					obj.EndEdit();
				}
				grdByProduct.UpdateData();
				return;
			}
			array = byProductRows;
			foreach (GridEXRow obj2 in array)
			{
				double weight = Conversion.ToDouble(obj2.Cells["Weight"].Value);
				double wagesAmount = totalWagesAmount / totalWeightInOutput * weight;
				obj2.BeginEdit();
				obj2.Cells["WagesAmount"].Value = wagesAmount.ToString();
				obj2.EndEdit();
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while proportionating wages: " + ex.Message, "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.StockConversionSummary_605(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(RecId, 66));
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
			if (Conversion.ToInt(CmbConversionType.Value) != 3 || MessageBox.Show("Are you sure to Generate When ConversionType [" + Conversion.ToString((object)((Control)(object)CmbConversionType).Text) + "]?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				handleAverageRateCalculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSummaryForUser()
	{
		try
		{
			summeryreset();
			if (grdInput.GetRows().Count() > 0)
			{
				double QtyInput = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Quantity"], (AggregateFunction)2));
				txtInputQuantity.Text = QtyInput.ToString("#,##0.###");
				double WeightInput = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Weight"], (AggregateFunction)2));
				txtInputWeight.Text = WeightInput.ToString("#,##0.###");
				double InputRate = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Rate"], (AggregateFunction)2));
				txtInputRate.Text = InputRate.ToString(clsGlobalVariables.DecimalRateFormate);
				double InputAmount = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Amount"], (AggregateFunction)2));
				txtInputAmount.Text = InputAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (grdByProduct.GetRows().Count() == 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						txtOutputByProductQty.Text = Conversion.ToDouble((object)r.Cells["Quantity"].Text).ToString("#,##0.###");
						txtOutputByProductWeight.Text = Conversion.ToDouble((object)r.Cells["Weight"].Text).ToString("#,##0.###");
						txtOutputByProductRate.Text = Conversion.ToDouble((object)r.Cells["Rate"].Text).ToString(clsGlobalVariables.DecimalRateFormate);
						txtOutputByProductAmount.Text = Conversion.ToDouble((object)r.Cells["Amount"].Text).ToString(clsGlobalVariables.stringFormatsingle);
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						txtFinishGoodsQty.Text = Conversion.ToDouble((object)r.Cells["Quantity"].Text).ToString("#,##0.###");
						txtFinishGoodsWeight.Text = Conversion.ToDouble((object)r.Cells["Weight"].Text).ToString("#,##0.###");
						txtFinishGoodsRate.Text = Conversion.ToDouble((object)r.Cells["Rate"].Text).ToString(clsGlobalVariables.DecimalRateFormate);
						txtFinishGoodsAmount.Text = Conversion.ToDouble((object)r.Cells["Amount"].Text).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
				else if (grdByProduct.GetRows().Count() > 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						double OutputByProductQty = Conversion.ToDouble((object)txtOutputByProductQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtOutputByProductQty.Text = OutputByProductQty.ToString("#,##0.###");
						double OutputByProductWeight = Conversion.ToDouble((object)txtOutputByProductWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtOutputByProductWeight.Text = OutputByProductWeight.ToString("#,##0.###");
						double OutputByProductRate = Conversion.ToDouble((object)txtOutputByProductRate.Text) + Conversion.ToDouble(r.Cells["Rate"].Value);
						txtOutputByProductRate.Text = OutputByProductRate.ToString(clsGlobalVariables.DecimalRateFormate);
						double OutputByProductAmount = Conversion.ToDouble((object)txtOutputByProductAmount.Text) + Conversion.ToDouble(r.Cells["Amount"].Value);
						txtOutputByProductAmount.Text = OutputByProductAmount.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						double FinishGoodsQty = Conversion.ToDouble((object)txtFinishGoodsQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtFinishGoodsQty.Text = FinishGoodsQty.ToString("#,##0.###");
						double FinishGoodsWeight = Conversion.ToDouble((object)txtFinishGoodsWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtFinishGoodsWeight.Text = FinishGoodsWeight.ToString("#,##0.###");
						double FinishGoodsRate = Conversion.ToDouble((object)txtFinishGoodsRate.Text) + Conversion.ToDouble(r.Cells["Rate"].Value);
						txtFinishGoodsRate.Text = FinishGoodsRate.ToString(clsGlobalVariables.DecimalRateFormate);
						double FinishGoodsAmount = Conversion.ToDouble((object)txtFinishGoodsAmount.Text) + Conversion.ToDouble(r.Cells["Amount"].Value);
						txtFinishGoodsAmount.Text = FinishGoodsAmount.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			double gainLoss = Conversion.ToDouble((object)txtFinishGoodsAmount.Text) - Conversion.ToDouble((object)txtInputAmount.Text);
			bool isGain = gainLoss > 0.0;
			Color resultBrush = (isGain ? Color.Green : Color.Red);
			labelInputOutputValueDiff.Text = (isGain ? "Gain" : "Loss");
			labelInputOutputValueDiff.ForeColor = resultBrush;
			txtInputOutputValueDiff.Text = gainLoss.ToString(clsGlobalVariables.stringFormatsingle);
			txtInputOutputValueDiff.ForeColor = resultBrush;
			double QtyPackingMaterial = Conversion.ToDouble(grdPM.GetTotal(grdPM.RootTable.Columns["ItemQTY"], (AggregateFunction)2));
			txtPackingMaterialQty.Text = QtyPackingMaterial.ToString("#,##0.###");
			double RatePackingMaterial = Conversion.ToDouble(grdPM.GetTotal(grdPM.RootTable.Columns["ItemRate"], (AggregateFunction)2));
			txtPackingMaterialRate.Text = RatePackingMaterial.ToString(clsGlobalVariables.DecimalRateFormate);
			double AmountPackingMaterial = Conversion.ToDouble(grdPM.GetTotal(grdPM.RootTable.Columns["Amount"], (AggregateFunction)2));
			txtPackingMaterialAmount.Text = AmountPackingMaterial.ToString(clsGlobalVariables.stringFormatsingle);
			double AmountOverheads = Conversion.ToDouble(grdOH.GetTotal(grdOH.RootTable.Columns["ExpAmount"], (AggregateFunction)2));
			txtOverHeadAmount.Text = AmountOverheads.ToString(clsGlobalVariables.stringFormatsingle);
			double WagesAmount = GridEX_Helper.GetColumnSumWithWhere(grdwagesDetail, "Amount", "WagesType", "Regular") + GridEX_Helper.GetColumnSumWithWhere(grdStiching, "Amount", "WagesType", "Regular");
			txtWagesAmount.Text = WagesAmount.ToString(clsGlobalVariables.stringFormatsingle);
			if (Conversion.ToDouble((object)txtInputAmount.Text) > 0.0 && Conversion.ToDouble((object)txtInputWeight.Text) > 0.0)
			{
				double AverageRateInput = Conversion.ToDouble((object)txtInputAmount.Text) / Conversion.ToDouble((object)txtInputWeight.Text) * 40.0;
				txtInputRate.Text = AverageRateInput.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			if (Conversion.ToDouble((object)txtOutputByProductAmount.Text) > 0.0 && Conversion.ToDouble((object)txtOutputByProductWeight.Text) > 0.0)
			{
				double AverageRateOutput = Conversion.ToDouble((object)txtOutputByProductAmount.Text) / Conversion.ToDouble((object)txtOutputByProductWeight.Text) * 40.0;
				txtOutputByProductRate.Text = AverageRateOutput.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			if (Conversion.ToDouble((object)txtFinishGoodsAmount.Text) > 0.0 && Conversion.ToDouble((object)txtFinishGoodsWeight.Text) > 0.0)
			{
				double AverageRateFinishGoods = Conversion.ToDouble((object)txtFinishGoodsAmount.Text) / Conversion.ToDouble((object)txtFinishGoodsWeight.Text) * 40.0;
				txtFinishGoodsRate.Text = AverageRateFinishGoods.ToString(clsGlobalVariables.DecimalRateFormate);
			}
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (IsFormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInput, table);
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
			if (IsFormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdByProduct, tableByProduct);
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
			if (IsFormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistoryMain, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnIssuanceLoad_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) == 0)
			{
				CmbConversionType.Focus();
				throw new Exception("Please Select Conversion Type");
			}
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.CanLoadOnlyPurchaseAndSingleItemRows = Conversion.ToInt(CmbConversionType.Value) == 2;
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
			if (ContractWagesChargetoProductForStockConversion)
			{
				WagesGridFillFromInputOutput(grdInput, InputGrid: true);
			}
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
			if (dtpurchaseinvoice == null || dtpurchaseinvoice.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingRefDocSubIds = new HashSet<int>();
			if (grdInput.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdInput.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					int refDocSubId2 = Conversion.ToInt(rows[i].Cells["RefDocSubId"].Value);
					if (!existingRefDocSubIds.Contains(refDocSubId2))
					{
						existingRefDocSubIds.Add(refDocSubId2);
					}
				}
			}
			int maxLineId = 0;
			if (table.Rows.Count > 0)
			{
				maxLineId = table.AsEnumerable().Max((DataRow r) => Conversion.ToInt(r["LineId"]));
			}
			foreach (DataRow dr in dtpurchaseinvoice.Rows)
			{
				int refDocSubId = Conversion.ToInt(dr["RefDocSubIdNo"]);
				if (!existingRefDocSubIds.Contains(refDocSubId))
				{
					maxLineId++;
					table.Rows.Add(0, dr["RefDocumentTypeId"], dr["RefDocIdNo"], dr["RefDocSubIdNo"], "Issue", dr["WareHouseId"], dr["WareHouse"], dr["CropYear"], dr["ItemId"], dr["ItemName"], dr["JobLotId"], dr["JobLotCode"], dr["ItemUOMId"], dr["PackUom"], dr["ItemUOM"], dr["PackingTypeId"], dr["PackTypeCode"], dr["ItemQty"], dr["Weight"], dr["ItemQty"], dr["Weight"], dr["ItemRate"], dr["RateUOMId"], dr["RateUOM"], dr["Equivalent"], dr["ItemAmount"], "", 0, "", maxLineId, 0);
					existingRefDocSubIds.Add(refDocSubId);
				}
			}
			grdInput.DataSource = table;
			grdInput.RetrieveStructure();
			gridsettings();
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void invfrmStockConversionProduction_KeyDown(object sender, KeyEventArgs e)
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
					txtDocdate.Focus();
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
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtProductionNo.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grdInput).Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 121;
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdInput).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdByProduct).Focus();
					}
					else if (((Control)(object)grdByProduct).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPM).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdInput).Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistoryMain).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryMain).Focused)
				{
					((Control)(object)grdhistoryInputOutputDetail).Focus();
				}
				else if (((Control)(object)grdhistoryInputOutputDetail).Focused)
				{
					((Control)(object)grdHistoryPM).Focus();
				}
				else
				{
					((Control)(object)grdHistoryMain).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print Slip");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Ctrl+F5", "For Focus on Production No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void GrdHistoryMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistoryMain.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (grdHistoryMain.CurrentColumn.Key == "Print")
				{
					CommonServices.StockConversionSummary_605(CurrId);
				}
				if (grdHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.StockConversionSummary_605(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInput.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdInput.CurrentColumn.Key == "Delete")
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
				GenerateSummaryForUser();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
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
				GenerateSummaryForUser();
			}
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdByProduct.CurrentColumn != null)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control && e.KeyCode == Keys.Space && grdByProduct.CurrentColumn.Key == "Delete")
				{
					DeletebyProductRow(item);
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					DeletebyProductRow(item);
				}
				grdByProduct_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPM_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPM.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPM.CurrentColumn != null && grdPM.CurrentColumn.Key == "Delete")
				{
					r.Delete();
					if (dtPackingMaterial.Rows.Count == 0)
					{
						AddRowInPackingMaterialGrid();
					}
					proportionatedPackingMaterialAmountInOutputGrid();
				}
				if (grdPM.CurrentColumn != null && grdPM.CurrentColumn.Key == "Add")
				{
					AddRowInPackingMaterialGrid();
					proportionatedPackingMaterialAmountInOutputGrid();
				}
				grdPM.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				r.Delete();
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPackingMaterialGrid();
				proportionatedPackingMaterialAmountInOutputGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				int BrandItemId = Conversion.ToInt(r.Cells["BrandItemId"].Value);
				int BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
				int WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
				if (grdPM.CurrentColumn.Key == "WareHouseId" || grdPM.CurrentColumn.Key == "WareHouse")
				{
					List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
						where x.ItemId == ItemId
						group x by x.WarehouseId into g
						select g.First()).ToList();
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("WareHouseName", typeof(string));
					dt.BeginLoadData();
					foreach (RacksWithWarehouseAndItem w in list)
					{
						dt.Rows.Add(w.WarehouseId, w.WareHouseName);
					}
					dt.EndLoadData();
					using (GrdPopUp obj = new GrdPopUp(dt, "Id", "WareHouseName"))
					{
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						int selectedId2 = Conversion.ToInt((object)obj.ReturnId);
						string selectedName = Conversion.ToString((object)obj.ReturnName);
						r.Cells["WareHouseId"].Value = selectedId2;
						r.Cells["WareHouse"].Value = selectedName;
						List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId2).ToList();
						if (RackId > 0)
						{
							if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
							{
								if (racks.Count > 1)
								{
									r.Cells["RackId"].Value = 0;
									r.Cells["RackName"].Value = "";
								}
								else if (racks.Count == 1)
								{
									RacksWithWarehouseAndItem singleRack = racks.First();
									r.Cells["RackId"].Value = singleRack.Id;
									r.Cells["RackName"].Value = singleRack.RackName;
								}
								else
								{
									r.Cells["RackId"].Value = 0;
									r.Cells["RackName"].Value = "";
								}
							}
						}
						else if (racks.Count == 1)
						{
							RacksWithWarehouseAndItem singleRack2 = racks.First();
							r.Cells["RackId"].Value = singleRack2.Id;
							r.Cells["RackName"].Value = singleRack2.RackName;
						}
					}
					grdPM.UpdateData();
				}
				else if (grdPM.CurrentColumn.Key == "RackId" || grdPM.CurrentColumn.Key == "RackName")
				{
					List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
						where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
						select rr into row
						group row by row.Id into g
						select g.First()).ToList();
					if (filteredRacks.Count == 0)
					{
						return;
					}
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id", typeof(int));
					dt2.Columns.Add("RackName", typeof(string));
					dt2.Columns.Add("WarehouseId", typeof(int));
					dt2.Columns.Add("WarehouseName", typeof(string));
					dt2.BeginLoadData();
					foreach (RacksWithWarehouseAndItem item in filteredRacks)
					{
						dt2.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
					}
					dt2.EndLoadData();
					using (GrdPopUp grdPopUp = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
					{
						grdPopUp.StartPosition = FormStartPosition.CenterScreen;
						grdPopUp.ShowDialog();
						int selectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
						string selectedName2 = Conversion.ToString((object)grdPopUp.ReturnName);
						r.Cells["RackId"].Value = selectedId;
						r.Cells["RackName"].Value = selectedName2;
						if (WarehouseId == 0)
						{
							RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId);
							if (selected != null)
							{
								r.Cells["WareHouseId"].Value = selected.WarehouseId;
								r.Cells["WareHouseName"].Value = selected.WareHouseName;
							}
						}
					}
					grdPM.UpdateData();
				}
				else if (grdPM.CurrentColumn != null && (grdPM.CurrentColumn.Key == "BrandItemId" || grdPM.CurrentColumn.Key == "BrandItem"))
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtBrandItem, "ItemId", "Item");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					BrandItemId = Id;
					Name = Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["BrandItemId"].Value = Id;
					r.Cells["BrandItem"].Value = Name;
					BrandItemId = Id;
					if (Id > 0)
					{
						DataTable dtUom = new DataView(dtBrandUom)
						{
							RowFilter = $"ItemId = {BrandItemId}"
						}.ToTable();
						if (dtUom.Rows.Count == 1)
						{
							BrandUomId = Conversion.ToInt(dtUom.Rows[0]["ItemUOMId"]);
							r.Cells["BrandUomId"].Value = BrandUomId;
							r.Cells["BrandUom"].Value = dtUom.Rows[0]["UOM"];
						}
						if (BrandItemId == 0)
						{
							BrandUomId = 0;
							r.Cells["BrandUomId"].Value = 0;
							r.Cells["BrandUom"].Value = "";
						}
					}
				}
				else if (grdPM.CurrentColumn != null && (grdPM.CurrentColumn.Key == "BrandUomId" || grdPM.CurrentColumn.Key == "BrandUom"))
				{
					if (BrandItemId <= 0)
					{
						throw new Exception("Please Select Brand Item first!");
					}
					GrdPopUp grdPopUp3 = new GrdPopUp(new DataView(dtBrandUom)
					{
						RowFilter = $"ItemId = {BrandItemId}"
					}.ToTable(), "ItemUOMId", "UOM");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					BrandUomId = Id;
					Name = Conversion.ToString((object)grdPopUp3.ReturnName);
					r.Cells["BrandUomId"].Value = Id;
					r.Cells["BrandUom"].Value = Name;
				}
				if (BrandItemId > 0 && BrandUomId > 0)
				{
					proportionatedPackingMaterialAmountInOutputGrid();
				}
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOH_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOH.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOH.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOH.CurrentColumn != null && grdOH.CurrentColumn.Key == "Delete")
				{
					r.Delete();
					if (dtPackingMaterial.Rows.Count == 0)
					{
						AddRowInExpenseGrid();
					}
					proportionateOverheadGrid();
				}
				if (grdOH.CurrentColumn != null && grdOH.CurrentColumn.Key == "Add")
				{
					AddRowInExpenseGrid();
					proportionateOverheadGrid();
				}
				grdOH.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				r.Delete();
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				proportionateOverheadGrid();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInExpenseGrid();
				proportionateOverheadGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				int BrandItemId = Conversion.ToInt(r.Cells["BrandItemId"].Value);
				int BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
				if (grdOH.CurrentColumn != null && (grdOH.CurrentColumn.Key == "BrandItemId" || grdOH.CurrentColumn.Key == "BrandItem"))
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtBrandItem, "ItemId", "Item");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					BrandItemId = Id;
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["BrandItemId"].Value = Id;
					r.Cells["BrandItem"].Value = Name;
					DataTable dtUom = new DataView(dtBrandUom)
					{
						RowFilter = $"ItemId = {BrandItemId}"
					}.ToTable();
					if (dtUom.Rows.Count == 1)
					{
						BrandUomId = Conversion.ToInt(dtUom.Rows[0]["ItemUOMId"]);
						r.Cells["BrandUomId"].Value = BrandUomId;
						r.Cells["BrandUom"].Value = dtUom.Rows[0]["UOM"];
					}
					if (BrandItemId == 0)
					{
						BrandUomId = 0;
						r.Cells["BrandUomId"].Value = 0;
						r.Cells["BrandUom"].Value = "";
					}
				}
				if (grdOH.CurrentColumn != null && (grdOH.CurrentColumn.Key == "BrandUomId" || grdOH.CurrentColumn.Key == "BrandUom"))
				{
					BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					if (BrandItemId <= 0)
					{
						throw new Exception("Please Select Brand Item first!");
					}
					GrdPopUp grdPopUp2 = new GrdPopUp(new DataView(dtBrandUom)
					{
						RowFilter = $"ItemId = {BrandItemId}"
					}.ToTable(), "ItemUOMId", "UOM");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					BrandUomId = Id;
					Name = Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["BrandUomId"].Value = Id;
					r.Cells["BrandUom"].Value = Name;
				}
				if (BrandItemId > 0 && BrandUomId > 0)
				{
					proportionateOverheadGrid();
				}
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateComboValueIngrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) != 5)
			{
				UpdatejobLotInGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdatejobLotInGrid()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdByProduct.GetDataRows().Count() > 0)
			{
				int JobId = Conversion.ToInt(CmbJobLotForGrid.Value);
				string jobName = "";
				if (((UltraGridBase)CmbJobLotForGrid).ActiveRow != null && JobId > 0)
				{
					jobName = Conversion.ToString((object)((Control)(object)CmbJobLotForGrid).Text);
				}
				else
				{
					GridEXRow rowsToValidate = (from rr in grdByProduct.GetDataRows()
						where Conversion.ToInt(rr.Cells["JobLotId"].Value) > 0
						select rr).FirstOrDefault();
					if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
					{
						if (Conversion.ToInt(rowsToValidate.Cells["JobLotId"].Value) == 0)
						{
							throw new Exception("JobLot Field is Empty in Output Grid Row No: 01");
						}
						JobId = Conversion.ToInt(rowsToValidate.Cells["JobLotId"].Value);
						jobName = Conversion.ToString(rowsToValidate.Cells["JobLot"].Value);
					}
				}
				if (JobId == 0)
				{
					CmbJobLotForGrid.Focus();
					throw new Exception("Please Select JobLot");
				}
				GridEXRow[] dataRows = grdByProduct.GetDataRows();
				foreach (GridEXRow obj in dataRows)
				{
					obj.BeginEdit();
					obj.Cells["JobLotId"].Value = JobId;
					obj.Cells["JobLot"].Value = jobName;
					obj.EndEdit();
				}
				return;
			}
			throw new Exception("Output Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) == 0 || Conversion.ToInt(CmbConversionType.Value) == 3)
			{
				CmbConversionType.Focus();
				throw new Exception("You Can't Select Conversion Type [Value Gain/Loss]");
			}
			LoadOutPut = new frmLoadStockShortFallForSales(UserAccount);
			LoadOutPut.ShowDialog();
			LoadOutPut.BringToFront();
			LoadOutPutDataFromStockShortFallForSales();
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
			if (ContractWagesChargetoProductForStockConversion)
			{
				WagesGridFillFromInputOutput(grdByProduct);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadOutPutDataFromStockShortFallForSales()
	{
		try
		{
			if (LoadOutPut.dtLoader.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in LoadOutPut.dtLoader.Rows)
			{
				tableByProduct.Rows.Add(0, "Recovery Head Rice", Conversion.ToInt(dr["WarehouseId"]), Conversion.ToString(dr["WareHouseName"]), Conversion.ToString(dr["CropYear"]), Conversion.ToInt(dr["ItemId"]), Conversion.ToString(dr["ItemName"]), Conversion.ToInt(dr["JobLotId"]), Conversion.ToString(dr["JobLot"]), Conversion.ToInt(dr["ItemUomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), Conversion.ToInt(dr["PackingTypeId"]), Conversion.ToString(dr["PackingType"]), Conversion.ToDouble(dr["RequiredQty"]), Conversion.ToDouble(dr["RequiredWeight"]), 0, Conversion.ToInt(dr["ItemUomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), Conversion.ToDouble(dr["RequiredAmount"]), 0, 0, "", 0, 0, Conversion.ToDouble(dr["RequiredAmount"]), "", 0, 0);
			}
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsByProduct();
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddContractorValuesForAllDetailWages_Click(object sender, EventArgs e)
	{
		try
		{
			AddContractorValuesForAllInGrid(grdwagesDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddContractorValuesForAllInGrid(GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grid.CurrentRow != null && (int)grid.CurrentRow.RowType == 0 && grid.CurrentRow.Cells.Count > 0 && grid.GetDataRows().Length != 0)
			{
				GridEXRow obj = grid.GetDataRows().FirstOrDefault();
				int ContractorValue = Conversion.ToInt(obj.Cells["SupplierId"].Value);
				int WagesAccountValue = Conversion.ToInt(obj.Cells["WagesId"].Value);
				if (ContractorValue == 0 && WagesAccountValue == 0)
				{
					MessageBox.Show("First row contains No values Of Contractor And Wages Account.");
					return;
				}
				GridEXRow[] dataRows = grid.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					r.BeginEdit();
					r.Cells["SupplierId"].Value = ContractorValue;
					r.Cells["WagesId"].Value = WagesAccountValue;
					r.Cells["WagesId"].Value = WagesAccountValue;
					double WagesPackSize = Conversion.ToDouble(r.Cells["PackSize"].Value);
					double WagesRate = 0.0;
					int WagesScheduleId = 0;
					GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtDocdate.Value), WagesPackSize, Conversion.ToInt(r.Cells["WagesId"].Value), Conversion.ToInt(r.Cells["SupplierId"].Value));
					if (d != null)
					{
						WagesRate = d.WagesRate;
						WagesScheduleId = d.ScheduleId;
					}
					if (WagesRate > 0.0)
					{
						r.Cells["WagesScheduleId"].Value = WagesScheduleId;
						r.Cells["Rate"].Value = WagesRate;
						if (Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0)
						{
							double num = Conversion.ToDouble(r.Cells["Weight"].Value);
							double b = Conversion.ToDouble(r.Cells["PackSize"].Value);
							_ = num / b;
							double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
							r.Cells["BillWeight"].Value = Math.Round(Weight, 2);
						}
						if (Conversion.ToDouble(r.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(r.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
						{
							double num2 = Conversion.ToDouble(r.Cells["WeightCut"].Value);
							double Qty2 = Conversion.ToDouble(r.Cells["Quantity"].Value);
							double WeightCut = num2 * Qty2;
							double BillWeight2 = Conversion.ToDouble(r.Cells["Weight"].Value) - Math.Abs(WeightCut);
							r.Cells["BillWeight"].Value = Math.Round(BillWeight2, 2);
						}
						if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(r.Cells["BillWeight"].Value) > 0.0)
						{
							double Packsize = Conversion.ToDouble(r.Cells["PackSize"].Value);
							double BillWeight = Conversion.ToDouble(r.Cells["BillWeight"].Value);
							double RateWithoutAddLess = Conversion.ToDouble(r.Cells["Rate"].Value);
							r.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess;
							double RateAddLess = Conversion.ToDouble(r.Cells["RateAddLess"].Value);
							if (EnableAddLessOnWagesRegular)
							{
								if (PercentageForRateAddLess > 0.0)
								{
									double RateAddLessByConfig = PercentageForRateAddLess * RateWithoutAddLess / 100.0;
									bool RateAddlessInMinus = !(RateAddLess > 0.0);
									if (Math.Abs(RateAddLess) > RateAddLessByConfig)
									{
										RateAddLess = (RateAddlessInMinus ? Conversion.ToDouble((object)("-" + RateAddLessByConfig)) : RateAddLessByConfig);
										r.Cells["RateAddLess"].Value = RateAddLess;
									}
								}
								else
								{
									RateAddLess = 0.0;
									r.Cells["RateAddLess"].Value = RateAddLess;
								}
							}
							double Rate = RateWithoutAddLess + RateAddLess;
							double Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
							double Amount = ((!WagesAmountCalculateOnQty) ? (BillWeight / Packsize * Rate) : (Qty * Rate));
							r.Cells["Amount"].Value = Math.Round(Amount, 2);
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["WagesScheduleId"].Value = 0;
					}
					r.EndEdit();
				}
			}
			ProportionateWagesAmountInOutputGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddContractorValuesForAllSticingWages_Click(object sender, EventArgs e)
	{
		try
		{
			AddContractorValuesForAllInGrid(grdStiching);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			frmContractWagesSchedule frmContractWagesSchedule = new frmContractWagesSchedule(UserAccount);
			frmContractWagesSchedule.Show();
			frmContractWagesSchedule.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesExempt_Click(object sender, EventArgs e)
	{
		try
		{
			new WagesExemptItemSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateItemsAndUom_Click(object sender, EventArgs e)
	{
		try
		{
			dtBrandItem.Rows.Clear();
			dtBrandUom.Rows.Clear();
			if (dtBrandItem.Columns.Count == 0)
			{
				dtBrandItem.Columns.Add("ItemId", typeof(int));
				dtBrandItem.Columns.Add("Item", typeof(string));
			}
			if (dtBrandUom.Columns.Count == 0)
			{
				dtBrandUom.Columns.Add("ItemId", typeof(int));
				dtBrandUom.Columns.Add("ItemUOMId", typeof(int));
				dtBrandUom.Columns.Add("UOM", typeof(string));
			}
			if (grdByProduct.RowCount <= 0)
			{
				return;
			}
			foreach (DataRow r2 in tableByProduct.DefaultView.ToTable(true, "ItemId", "Item").Rows)
			{
				DataRow newRow2 = dtBrandItem.NewRow();
				newRow2["ItemId"] = r2["ItemId"];
				newRow2["Item"] = r2["Item"];
				dtBrandItem.Rows.Add(newRow2);
			}
			foreach (DataRow r in tableByProduct.DefaultView.ToTable(true, "ItemId", "ItemUOMId", "UOM").Rows)
			{
				DataRow newRow = dtBrandUom.NewRow();
				newRow["ItemId"] = r["ItemId"];
				newRow["ItemUOMId"] = r["ItemUOMId"];
				newRow["UOM"] = r["UOM"];
				dtBrandUom.Rows.Add(newRow);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateUpdateOnDocDateChangeForInput()
	{
		try
		{
			if (grdInput.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdInput.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
				int RefDocSubId = Conversion.ToInt(r.Cells["RefDocSubId"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				Conversion.ToInt(r.Cells["WareHouseId"].Value);
				string CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
				double RateUomEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				double rate = 0.0;
				if (RefDocumentTypeId != 0 || RefDocNoId != 0 || RefDocSubId != 0)
				{
					break;
				}
				if (!FIFOCgs)
				{
					rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)txtDocdate.Value), 66, RecId, JobLotId, 0, CropYear);
				}
				if (rate > 0.0)
				{
					r.Cells["Rate"].Value = rate;
					r.Cells["Amount"].Value = Weight / RateUomEquivalent * rate;
				}
				r.EndEdit();
			}
			grdInput.UpdateData();
			handleAverageRateCalculation();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AvgRateUpdateOnDocDateChangeForPm()
	{
		try
		{
			if (grdPM.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdPM.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				obj.BeginEdit();
				double ItemQty = Conversion.ToDouble(obj.Cells["ItemQTY"].Value);
				double rate = 0.0;
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(ItemConditionId: Conversion.ToInt(obj.Cells["ItemConditionId"].Value), ItemId: ItemId, DocDate: Conversion.ToDateTime((object)txtDocdate.Value), RecId: RecId, DocumentTypeId: (RecId > 0) ? 66 : 0);
				if (dtRate.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
				}
				obj.Cells["ItemRate"].Value = rate;
				obj.Cells["Amount"].Value = ItemQty * rate;
				obj.EndEdit();
			}
			grdPM.UpdateData();
			handleAverageRateCalculation();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime previousDate = ((txtDocdate.Tag != null) ? Conversion.ToDateTime(txtDocdate.Tag) : txtDocdate.Value);
			DateTime currentDate = txtDocdate.Value;
			if (previousDate.Date == currentDate.Date)
			{
				return;
			}
			txtDocdate.Tag = txtDocdate.Value;
			if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPM, "Amount")) > 0m)
			{
				if (MessageBox.Show("By changing doc date the rates in Packing Material grid will be change, so that amount change.Are you sure to change doc date ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					txtDocdate.Tag = previousDate;
					txtDocdate.Value = previousDate;
				}
				else
				{
					AvgRateUpdateOnDocDateChangeForPm();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStockReleaseFromFumigation_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbConversionType.Value) == 0)
			{
				CmbConversionType.Focus();
				throw new Exception("Please Select Conversion Type");
			}
			using (LoadavailableTransactionsForStockReleaseFromFumigation loadFumigation = new LoadavailableTransactionsForStockReleaseFromFumigation(UserAccount))
			{
				loadFumigation.ShowDialog();
				loadFumigation.BringToFront();
				LoadDataDetailfromFumigation(loadFumigation.dtIssuance);
			}
			((Control)(object)CmbConversionType).Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromFumigation(DataTable dt)
	{
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingSubIds = new HashSet<int>(from r in grdInput.GetRows()
				where (int)r.RowType == 0
				select Conversion.ToInt(r.Cells["RefDocSubId"].Value));
			int maxLineId = (from r in grdInput.GetRows()
				where (int)r.RowType == 0
				select Conversion.ToInt(r.Cells["LineId"].Value)).DefaultIfEmpty(0).Max();
			table.BeginLoadData();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				int refDocSubIdNo = Conversion.ToInt(dt.Rows[i]["RefDocSubIdNo"]);
				if (!existingSubIds.Contains(refDocSubIdNo))
				{
					maxLineId++;
					table.Rows.Add(0, dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["RefDocIdNo"], dt.Rows[i]["RefDocSubIdNo"], "Issue", dt.Rows[i]["WareHouseId"], dt.Rows[i]["WareHouse"], dt.Rows[i]["CropYear"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotCode"], dt.Rows[i]["ItemUOMId"], dt.Rows[i]["PackUom"], dt.Rows[i]["ItemUOM"], dt.Rows[i]["PackingTypeId"], dt.Rows[i]["PackTypeCode"], dt.Rows[i]["ItemQty"], dt.Rows[i]["Weight"], dt.Rows[i]["ItemQty"], dt.Rows[i]["Weight"], dt.Rows[i]["ItemRate"], dt.Rows[i]["RateUOMId"], dt.Rows[i]["RateUOM"], dt.Rows[i]["Equivalent"], dt.Rows[i]["ItemAmount"], "", 0, "", maxLineId, dt.Rows[i]["labIPmActivityLogId"]);
					existingSubIds.Add(refDocSubIdNo);
				}
			}
			table.EndLoadData();
			grdInput.DataSource = table;
			grdInput.RetrieveStructure();
			gridsettings(FromFumigation: true);
			GridEXRow[] rows = grdInput.GetRows();
			foreach (GridEXRow row in rows)
			{
				if ((int)row.RowType == 0)
				{
					row.IsChecked = true;
				}
			}
			InputRowsForwardToOutput();
			proportionateOverheadGrid();
			proportionatedPackingMaterialAmountInOutputGrid();
			GenerateSummaryForUser();
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
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Expected O, but got Unknown
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Expected O, but got Unknown
		//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1e: Expected O, but got Unknown
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Expected O, but got Unknown
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Expected O, but got Unknown
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab8: Expected O, but got Unknown
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Expected O, but got Unknown
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Expected O, but got Unknown
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Expected O, but got Unknown
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Expected O, but got Unknown
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baa: Expected O, but got Unknown
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Expected O, but got Unknown
		//IL_0c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c91: Expected O, but got Unknown
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb2: Expected O, but got Unknown
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Expected O, but got Unknown
		//IL_188a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1894: Expected O, but got Unknown
		//IL_18a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ab: Expected O, but got Unknown
		//IL_236e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2378: Expected O, but got Unknown
		//IL_2385: Unknown result type (might be due to invalid IL or missing references)
		//IL_238f: Expected O, but got Unknown
		//IL_26ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b6: Expected O, but got Unknown
		//IL_26c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cd: Expected O, but got Unknown
		//IL_26da: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e4: Expected O, but got Unknown
		//IL_28cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d7: Expected O, but got Unknown
		//IL_28e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ee: Expected O, but got Unknown
		//IL_2c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2d: Expected O, but got Unknown
		//IL_2c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c44: Expected O, but got Unknown
		//IL_2c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5b: Expected O, but got Unknown
		//IL_32da: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e4: Expected O, but got Unknown
		//IL_32f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fb: Expected O, but got Unknown
		//IL_3308: Unknown result type (might be due to invalid IL or missing references)
		//IL_3312: Expected O, but got Unknown
		//IL_b1f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_b1fc: Expected O, but got Unknown
		//IL_b220: Unknown result type (might be due to invalid IL or missing references)
		//IL_b22a: Expected O, but got Unknown
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPM_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOH_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdwagesDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdStiching_DesignTimeLayout = new GridEXLayout();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
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
		Appearance appearance64 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
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
		Appearance appearance69 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.invfrmStockConversionProduction));
		GridEXLayout grdHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryInputOutputDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryPM_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOH_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabInput = new System.Windows.Forms.TabPage();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.BtnForwardRowsFromInputToOutPut = new System.Windows.Forms.Button();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdInput = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label19 = new System.Windows.Forms.Label();
		this.tabOutPut = new System.Windows.Forms.TabPage();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.btnGenerateItemsAndUom = new System.Windows.Forms.Button();
		this.CmbJobLotForGrid = new UltraCombo();
		this.label66 = new System.Windows.Forms.Label();
		this.BtnUpdateComboValueIngrid = new System.Windows.Forms.Button();
		this.grdByProduct = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.tabPm = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdPM = new GridEX();
		this.tabOh = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdOH = new GridEX();
		this.tabwages = new System.Windows.Forms.TabPage();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdwagesDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.btnResetRegularWagesForInput = new System.Windows.Forms.Button();
		this.btnResetRegularWagesForOutPut = new System.Windows.Forms.Button();
		this.btnAddContractorValuesForAllDetailWages = new System.Windows.Forms.Button();
		this.label54 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdStiching = new GridEX();
		this.panel26 = new System.Windows.Forms.Panel();
		this.btnResetOtherWagesForInput = new System.Windows.Forms.Button();
		this.btnResetOtherWagesForOutPut = new System.Windows.Forms.Button();
		this.label55 = new System.Windows.Forms.Label();
		this.btnAddContractorValuesForAllSticingWages = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.panel24 = new System.Windows.Forms.Panel();
		this.labelWagesAmt = new System.Windows.Forms.Label();
		this.txtPackingMaterialQty = new System.Windows.Forms.TextBox();
		this.txtInputOutputValueDiff = new System.Windows.Forms.TextBox();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.labelInputOutputValueDiff = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtDifference = new System.Windows.Forms.TextBox();
		this.txtOverHeadQty = new System.Windows.Forms.TextBox();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialAmount = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialRate = new System.Windows.Forms.TextBox();
		this.txtWagesAmount = new System.Windows.Forms.TextBox();
		this.txtOverHeadAmount = new System.Windows.Forms.TextBox();
		this.txtOverHeadWeight = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialWeight = new System.Windows.Forms.TextBox();
		this.txtOverHeadRate = new System.Windows.Forms.TextBox();
		this.panel51 = new System.Windows.Forms.Panel();
		this.panel52 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtOutputByProductQty = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsQty = new System.Windows.Forms.TextBox();
		this.txtInputQuantity = new System.Windows.Forms.TextBox();
		this.txtInputWeight = new System.Windows.Forms.TextBox();
		this.txtOutputByProductWeight = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsWeight = new System.Windows.Forms.TextBox();
		this.txtInputRate = new System.Windows.Forms.TextBox();
		this.txtOutputByProductRate = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsRate = new System.Windows.Forms.TextBox();
		this.txtInputAmount = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsAmount = new System.Windows.Forms.TextBox();
		this.txtOutputByProductAmount = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkFumigationOnHold = new System.Windows.Forms.CheckBox();
		this.label16 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbConversionType = new UltraCombo();
		this.CmbDifferenceAccount = new UltraCombo();
		this.CmbProductionDepartment = new UltraCombo();
		this.label112 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.cmbParentCategory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbMoistureSlab = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.cmbEntryType = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtAverageRate = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtStock = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbBagType = new UltraCombo();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtdeailRemarks = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbCropyr = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbRateUom = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtUnitWeight = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.cmbGodown = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.cmbLot = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.btnIssuanceLoad = new System.Windows.Forms.ToolStripButton();
		this.btnStockReleaseFromFumigation = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnWagesSchedule = new System.Windows.Forms.ToolStripButton();
		this.btnWagesExempt = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryMain = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label41 = new System.Windows.Forms.Label();
		this.grdhistoryInputOutputDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdHistoryPM = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdHistoryOH = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.RdApproveDate = new System.Windows.Forms.RadioButton();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label42 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabInput.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.tabOutPut.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotForGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.tabPm.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPM).BeginInit();
		this.tabOh.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOH).BeginInit();
		this.tabwages.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStiching).BeginInit();
		this.panel26.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.panel51.SuspendLayout();
		this.panel52.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbConversionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMoistureSlab).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMain).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryInputOutputDetail).BeginInit();
		this.panel13.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).BeginInit();
		this.panel17.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOH).BeginInit();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1151, 681);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1151, 681);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1143, 652);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 271);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1137, 225);
		this.panel5.TabIndex = 4;
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabInput);
		this.tabControl2.Controls.Add(this.tabOutPut);
		this.tabControl2.Controls.Add(this.tabPm);
		this.tabControl2.Controls.Add(this.tabOh);
		this.tabControl2.Controls.Add(this.tabwages);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1137, 225);
		this.tabControl2.TabIndex = 0;
		this.tabInput.BackColor = System.Drawing.Color.White;
		this.tabInput.Controls.Add(this.panel22);
		this.tabInput.Location = new System.Drawing.Point(4, 26);
		this.tabInput.Name = "tabInput";
		this.tabInput.Padding = new System.Windows.Forms.Padding(3);
		this.tabInput.Size = new System.Drawing.Size(1129, 195);
		this.tabInput.TabIndex = 0;
		this.tabInput.Text = "Input";
		this.panel22.Controls.Add(this.label49);
		this.panel22.Controls.Add(this.BtnForwardRowsFromInputToOutPut);
		this.panel22.Controls.Add(this.panel8);
		this.panel22.Controls.Add(this.ctrlGrdBar2);
		this.panel22.Controls.Add(this.label19);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1123, 189);
		this.panel22.TabIndex = 0;
		this.label49.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label49.AutoSize = true;
		this.label49.BackColor = System.Drawing.Color.Teal;
		this.label49.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(507, 5);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(536, 17);
		this.label49.TabIndex = 0;
		this.label49.Text = "Click the Forward_Button to Transfer Selected Input Rows Into Output Grid";
		this.BtnForwardRowsFromInputToOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnForwardRowsFromInputToOutPut.BackColor = System.Drawing.Color.Transparent;
		this.BtnForwardRowsFromInputToOutPut.FlatAppearance.BorderSize = 0;
		this.BtnForwardRowsFromInputToOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnForwardRowsFromInputToOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnForwardRowsFromInputToOutPut.ForeColor = System.Drawing.Color.Transparent;
		this.BtnForwardRowsFromInputToOutPut.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnForwardRowsFromInputToOutPut.Location = new System.Drawing.Point(1043, 1);
		this.BtnForwardRowsFromInputToOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.BtnForwardRowsFromInputToOutPut.Name = "BtnForwardRowsFromInputToOutPut";
		this.BtnForwardRowsFromInputToOutPut.Size = new System.Drawing.Size(35, 25);
		this.BtnForwardRowsFromInputToOutPut.TabIndex = 17;
		this.BtnForwardRowsFromInputToOutPut.UseVisualStyleBackColor = false;
		this.BtnForwardRowsFromInputToOutPut.Click += new System.EventHandler(BtnForwardRowsFromInputToOutPut_Click);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 26);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1123, 163);
		this.panel8.TabIndex = 22;
		((ScrollableBase)this.grdInput).BackColor = System.Drawing.Color.White;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(1123, 163);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.grdInput.CellUpdated += new ColumnActionEventHandler(grdInput_CellUpdated);
		this.grdInput.ColumnButtonClick += new ColumnActionEventHandler(grdInput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInput).DoubleClick += new System.EventHandler(grdInput_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdInput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInput_KeyDown);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1083, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInput;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Dock = System.Windows.Forms.DockStyle.Top;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(1123, 26);
		this.label19.TabIndex = 2;
		this.tabOutPut.Controls.Add(this.panel23);
		this.tabOutPut.Location = new System.Drawing.Point(4, 26);
		this.tabOutPut.Name = "tabOutPut";
		this.tabOutPut.Padding = new System.Windows.Forms.Padding(3);
		this.tabOutPut.Size = new System.Drawing.Size(1489, 195);
		this.tabOutPut.TabIndex = 1;
		this.tabOutPut.Text = "Output";
		this.tabOutPut.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add(this.label57);
		this.panel23.Controls.Add(this.btnGenerateItemsAndUom);
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid);
		this.panel23.Controls.Add(this.label66);
		this.panel23.Controls.Add(this.BtnUpdateComboValueIngrid);
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel23.Controls.Add(this.ctrlGrdBar1);
		this.panel23.Controls.Add(this.label17);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(3, 3);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1483, 189);
		this.panel23.TabIndex = 0;
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.Teal;
		this.label57.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(2, 5);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(622, 17);
		this.label57.TabIndex = 163;
		this.label57.Text = "Click the Forward_Button to generate Output items based on Uom For PM and Overhead";
		this.btnGenerateItemsAndUom.BackColor = System.Drawing.Color.Transparent;
		this.btnGenerateItemsAndUom.FlatAppearance.BorderSize = 0;
		this.btnGenerateItemsAndUom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerateItemsAndUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateItemsAndUom.ForeColor = System.Drawing.Color.Transparent;
		this.btnGenerateItemsAndUom.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.btnGenerateItemsAndUom.Location = new System.Drawing.Point(625, 1);
		this.btnGenerateItemsAndUom.Margin = new System.Windows.Forms.Padding(2);
		this.btnGenerateItemsAndUom.Name = "btnGenerateItemsAndUom";
		this.btnGenerateItemsAndUom.Size = new System.Drawing.Size(35, 25);
		this.btnGenerateItemsAndUom.TabIndex = 164;
		this.btnGenerateItemsAndUom.UseVisualStyleBackColor = false;
		this.btnGenerateItemsAndUom.Click += new System.EventHandler(btnGenerateItemsAndUom_Click);
		this.CmbJobLotForGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotForGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotForGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotForGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotForGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotForGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid).Location = new System.Drawing.Point(760, 0);
		((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid).Name = "CmbJobLotForGrid";
		((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotForGrid).TabIndex = 160;
		this.label66.AutoSize = true;
		this.label66.BackColor = System.Drawing.Color.Teal;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(703, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(55, 19);
		this.label66.TabIndex = 162;
		this.label66.Text = "Job lot";
		this.BtnUpdateComboValueIngrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateComboValueIngrid.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateComboValueIngrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateComboValueIngrid.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateComboValueIngrid.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateComboValueIngrid.Location = new System.Drawing.Point(939, 0);
		this.BtnUpdateComboValueIngrid.Name = "BtnUpdateComboValueIngrid";
		this.BtnUpdateComboValueIngrid.Size = new System.Drawing.Size(72, 26);
		this.BtnUpdateComboValueIngrid.TabIndex = 161;
		this.BtnUpdateComboValueIngrid.Text = "Update";
		this.BtnUpdateComboValueIngrid.UseVisualStyleBackColor = false;
		this.BtnUpdateComboValueIngrid.Click += new System.EventHandler(BtnUpdateComboValueIngrid_Click);
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProduct.FilterMode = (FilterMode)2;
		this.grdByProduct.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1483, 163);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Click += new System.EventHandler(grdByProduct_Click);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdByProduct_KeyDown);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1443, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdByProduct;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(1483, 26);
		this.label17.TabIndex = 1;
		this.tabPm.Controls.Add(this.panel6);
		this.tabPm.Location = new System.Drawing.Point(4, 26);
		this.tabPm.Name = "tabPm";
		this.tabPm.Size = new System.Drawing.Size(1129, 195);
		this.tabPm.TabIndex = 2;
		this.tabPm.Text = "Packing Material / Consumeable Items";
		this.tabPm.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdPM);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1129, 195);
		this.panel6.TabIndex = 0;
		this.grdPM.AutomaticSort = false;
		((ScrollableBase)this.grdPM).BackColor = System.Drawing.Color.White;
		grdPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPM.DesignTimeLayout = grdPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPM.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPM).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPM).Name = "grdPM";
		((ScrollableBase)this.grdPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPM).Size = new System.Drawing.Size(1129, 195);
		((System.Windows.Forms.Control)(object)this.grdPM).TabIndex = 13;
		this.grdPM.TotalRow = (InheritableBoolean)1;
		this.grdPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPM).VisualStyle = (VisualStyle)4;
		this.grdPM.CellEdited += new ColumnActionEventHandler(grdPackingMaterial_CellEdited);
		this.grdPM.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPM.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPM).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPM_KeyDown);
		this.tabOh.Controls.Add(this.panel12);
		this.tabOh.Location = new System.Drawing.Point(4, 26);
		this.tabOh.Name = "tabOh";
		this.tabOh.Size = new System.Drawing.Size(1489, 195);
		this.tabOh.TabIndex = 3;
		this.tabOh.Text = "Overheads";
		this.tabOh.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdOH);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1489, 195);
		this.panel12.TabIndex = 0;
		this.grdOH.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdOH).BackColor = System.Drawing.Color.White;
		grdOH_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOH.DesignTimeLayout = grdOH_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOH).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOH).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOH.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOH).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOH).Name = "grdOH";
		((ScrollableBase)this.grdOH).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOH).Size = new System.Drawing.Size(1489, 195);
		((System.Windows.Forms.Control)(object)this.grdOH).TabIndex = 15;
		this.grdOH.TotalRow = (InheritableBoolean)1;
		this.grdOH.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOH).VisualStyle = (VisualStyle)4;
		this.grdOH.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOH.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOH).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOH_KeyDown);
		this.tabwages.Controls.Add(this.tabControl3);
		this.tabwages.Location = new System.Drawing.Point(4, 26);
		this.tabwages.Name = "tabwages";
		this.tabwages.Size = new System.Drawing.Size(1489, 195);
		this.tabwages.TabIndex = 4;
		this.tabwages.Text = "Contractor Wages";
		this.tabwages.UseVisualStyleBackColor = true;
		this.tabControl3.Controls.Add(this.tabPage3);
		this.tabControl3.Controls.Add(this.tabPage4);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1489, 195);
		this.tabControl3.TabIndex = 1;
		this.tabPage3.Controls.Add(this.panel19);
		this.tabPage3.Controls.Add(this.panel20);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1481, 166);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Regular Wages";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdwagesDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 30);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1475, 133);
		this.panel19.TabIndex = 7;
		((ScrollableBase)this.grdwagesDetail).BackColor = System.Drawing.Color.White;
		grdwagesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdwagesDetail.DesignTimeLayout = grdwagesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdwagesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Name = "grdwagesDetail";
		((ScrollableBase)this.grdwagesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Size = new System.Drawing.Size(1475, 133);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Tag = "WagesGrid";
		this.grdwagesDetail.TotalRow = (InheritableBoolean)1;
		this.grdwagesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdwagesDetail).VisualStyle = (VisualStyle)4;
		this.grdwagesDetail.EditingCell += new EditingCellEventHandler(grdwagesDetail_EditingCell);
		this.grdwagesDetail.CellUpdated += new ColumnActionEventHandler(grdwagesDetail_CellUpdated);
		this.grdwagesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdwagesDetail_ColumnButtonClick);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label53);
		this.panel20.Controls.Add(this.btnResetRegularWagesForInput);
		this.panel20.Controls.Add(this.btnResetRegularWagesForOutPut);
		this.panel20.Controls.Add(this.btnAddContractorValuesForAllDetailWages);
		this.panel20.Controls.Add(this.label54);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1475, 27);
		this.panel20.TabIndex = 6;
		this.label53.AutoSize = true;
		this.label53.BackColor = System.Drawing.Color.Teal;
		this.label53.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label53.Location = new System.Drawing.Point(320, 1);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(181, 26);
		this.label53.TabIndex = 68;
		this.label53.Text = "Click this button To Apply First \r\nRow Values for All other Rows";
		this.btnResetRegularWagesForInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnResetRegularWagesForInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnResetRegularWagesForInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetRegularWagesForInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetRegularWagesForInput.ForeColor = System.Drawing.Color.White;
		this.btnResetRegularWagesForInput.Location = new System.Drawing.Point(1146, 0);
		this.btnResetRegularWagesForInput.Name = "btnResetRegularWagesForInput";
		this.btnResetRegularWagesForInput.Size = new System.Drawing.Size(155, 26);
		this.btnResetRegularWagesForInput.TabIndex = 68;
		this.btnResetRegularWagesForInput.Text = "Reset Input Wages";
		this.btnResetRegularWagesForInput.UseVisualStyleBackColor = false;
		this.btnResetRegularWagesForInput.Click += new System.EventHandler(btnResetWagesFromInput_Click);
		this.btnResetRegularWagesForOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnResetRegularWagesForOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnResetRegularWagesForOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetRegularWagesForOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetRegularWagesForOutPut.ForeColor = System.Drawing.Color.White;
		this.btnResetRegularWagesForOutPut.Location = new System.Drawing.Point(1303, 0);
		this.btnResetRegularWagesForOutPut.Name = "btnResetRegularWagesForOutPut";
		this.btnResetRegularWagesForOutPut.Size = new System.Drawing.Size(168, 26);
		this.btnResetRegularWagesForOutPut.TabIndex = 70;
		this.btnResetRegularWagesForOutPut.Text = "Reset Output Wages";
		this.btnResetRegularWagesForOutPut.UseVisualStyleBackColor = false;
		this.btnResetRegularWagesForOutPut.Click += new System.EventHandler(btnResetWagesForOutPut_Click);
		this.btnAddContractorValuesForAllDetailWages.BackColor = System.Drawing.Color.Green;
		this.btnAddContractorValuesForAllDetailWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddContractorValuesForAllDetailWages.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddContractorValuesForAllDetailWages.ForeColor = System.Drawing.Color.White;
		this.btnAddContractorValuesForAllDetailWages.Location = new System.Drawing.Point(502, 1);
		this.btnAddContractorValuesForAllDetailWages.Name = "btnAddContractorValuesForAllDetailWages";
		this.btnAddContractorValuesForAllDetailWages.Size = new System.Drawing.Size(84, 26);
		this.btnAddContractorValuesForAllDetailWages.TabIndex = 67;
		this.btnAddContractorValuesForAllDetailWages.Text = "Apply All";
		this.btnAddContractorValuesForAllDetailWages.UseVisualStyleBackColor = false;
		this.btnAddContractorValuesForAllDetailWages.Click += new System.EventHandler(btnAddContractorValuesForAllDetailWages_Click);
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.Teal;
		this.label54.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label54.Location = new System.Drawing.Point(3, 6);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(266, 16);
		this.label54.TabIndex = 0;
		this.label54.Text = "In this Grid User Will add Regular Wages";
		this.tabPage4.Controls.Add(this.panel21);
		this.tabPage4.Controls.Add(this.panel26);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1481, 166);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Wages";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdStiching);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(3, 28);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1475, 135);
		this.panel21.TabIndex = 9;
		((ScrollableBase)this.grdStiching).BackColor = System.Drawing.Color.White;
		grdStiching_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStiching.DesignTimeLayout = grdStiching_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStiching).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdStiching).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStiching.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStiching).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStiching).Name = "grdStiching";
		((ScrollableBase)this.grdStiching).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStiching).Size = new System.Drawing.Size(1475, 135);
		((System.Windows.Forms.Control)(object)this.grdStiching).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdStiching).Tag = "StichingGrid";
		this.grdStiching.TotalRow = (InheritableBoolean)1;
		this.grdStiching.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStiching).VisualStyle = (VisualStyle)4;
		this.grdStiching.EditingCell += new EditingCellEventHandler(grdStiching_EditingCell);
		this.grdStiching.CellUpdated += new ColumnActionEventHandler(grdStiching_CellUpdated);
		this.grdStiching.ColumnButtonClick += new ColumnActionEventHandler(grdStiching_ColumnButtonClick);
		this.panel26.BackColor = System.Drawing.Color.Teal;
		this.panel26.Controls.Add(this.btnResetOtherWagesForInput);
		this.panel26.Controls.Add(this.btnResetOtherWagesForOutPut);
		this.panel26.Controls.Add(this.label55);
		this.panel26.Controls.Add(this.btnAddContractorValuesForAllSticingWages);
		this.panel26.Controls.Add(this.label56);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(3, 3);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1475, 25);
		this.panel26.TabIndex = 8;
		this.btnResetOtherWagesForInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnResetOtherWagesForInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnResetOtherWagesForInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetOtherWagesForInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetOtherWagesForInput.ForeColor = System.Drawing.Color.White;
		this.btnResetOtherWagesForInput.Location = new System.Drawing.Point(1150, -2);
		this.btnResetOtherWagesForInput.Name = "btnResetOtherWagesForInput";
		this.btnResetOtherWagesForInput.Size = new System.Drawing.Size(155, 26);
		this.btnResetOtherWagesForInput.TabIndex = 71;
		this.btnResetOtherWagesForInput.Text = "Reset Input Wages";
		this.btnResetOtherWagesForInput.UseVisualStyleBackColor = false;
		this.btnResetOtherWagesForInput.Visible = false;
		this.btnResetOtherWagesForInput.Click += new System.EventHandler(btnResetOtherWagesForInput_Click);
		this.btnResetOtherWagesForOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnResetOtherWagesForOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnResetOtherWagesForOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetOtherWagesForOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetOtherWagesForOutPut.ForeColor = System.Drawing.Color.White;
		this.btnResetOtherWagesForOutPut.Location = new System.Drawing.Point(1307, -2);
		this.btnResetOtherWagesForOutPut.Name = "btnResetOtherWagesForOutPut";
		this.btnResetOtherWagesForOutPut.Size = new System.Drawing.Size(168, 26);
		this.btnResetOtherWagesForOutPut.TabIndex = 72;
		this.btnResetOtherWagesForOutPut.Text = "Reset Output Wages";
		this.btnResetOtherWagesForOutPut.UseVisualStyleBackColor = false;
		this.btnResetOtherWagesForOutPut.Click += new System.EventHandler(btnResetOtherWagesForOutPut_Click);
		this.label55.AutoSize = true;
		this.label55.BackColor = System.Drawing.Color.Teal;
		this.label55.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label55.Location = new System.Drawing.Point(378, -1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(178, 26);
		this.label55.TabIndex = 70;
		this.label55.Text = "Click this button To Apply First\r\nRow Values for All other Rows";
		this.btnAddContractorValuesForAllSticingWages.BackColor = System.Drawing.Color.Green;
		this.btnAddContractorValuesForAllSticingWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddContractorValuesForAllSticingWages.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddContractorValuesForAllSticingWages.ForeColor = System.Drawing.Color.White;
		this.btnAddContractorValuesForAllSticingWages.Location = new System.Drawing.Point(563, -2);
		this.btnAddContractorValuesForAllSticingWages.Name = "btnAddContractorValuesForAllSticingWages";
		this.btnAddContractorValuesForAllSticingWages.Size = new System.Drawing.Size(84, 27);
		this.btnAddContractorValuesForAllSticingWages.TabIndex = 69;
		this.btnAddContractorValuesForAllSticingWages.Text = "Apply All";
		this.btnAddContractorValuesForAllSticingWages.UseVisualStyleBackColor = false;
		this.btnAddContractorValuesForAllSticingWages.Click += new System.EventHandler(btnAddContractorValuesForAllSticingWages_Click);
		this.label56.AutoSize = true;
		this.label56.BackColor = System.Drawing.Color.Teal;
		this.label56.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label56.Location = new System.Drawing.Point(3, 3);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(320, 16);
		this.label56.TabIndex = 0;
		this.label56.Text = "In this Grid User Will add Other Wages (Stiching)";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(3, 496);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1137, 153);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.panel24);
		this.groupBox2.Controls.Add(this.panel51);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 10.25f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1063, 147);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Summery Calculations";
		this.panel24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel24.Controls.Add(this.labelWagesAmt);
		this.panel24.Controls.Add(this.txtPackingMaterialQty);
		this.panel24.Controls.Add(this.txtInputOutputValueDiff);
		this.panel24.Controls.Add(this.panel25);
		this.panel24.Controls.Add(this.label38);
		this.panel24.Controls.Add(this.label39);
		this.panel24.Controls.Add(this.labelInputOutputValueDiff);
		this.panel24.Controls.Add(this.label40);
		this.panel24.Controls.Add(this.txtDifference);
		this.panel24.Controls.Add(this.txtOverHeadQty);
		this.panel24.Controls.Add(this.txtAvgRate);
		this.panel24.Controls.Add(this.txtPackingMaterialAmount);
		this.panel24.Controls.Add(this.txtPackingMaterialRate);
		this.panel24.Controls.Add(this.txtWagesAmount);
		this.panel24.Controls.Add(this.txtOverHeadAmount);
		this.panel24.Controls.Add(this.txtOverHeadWeight);
		this.panel24.Controls.Add(this.txtPackingMaterialWeight);
		this.panel24.Controls.Add(this.txtOverHeadRate);
		this.panel24.Location = new System.Drawing.Point(524, 19);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(538, 113);
		this.panel24.TabIndex = 12;
		this.labelWagesAmt.BackColor = System.Drawing.Color.Silver;
		this.labelWagesAmt.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelWagesAmt.ForeColor = System.Drawing.Color.Black;
		this.labelWagesAmt.Location = new System.Drawing.Point(436, 30);
		this.labelWagesAmt.Name = "labelWagesAmt";
		this.labelWagesAmt.Size = new System.Drawing.Size(99, 37);
		this.labelWagesAmt.TabIndex = 6;
		this.labelWagesAmt.Text = "Wages Amount";
		this.labelWagesAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPackingMaterialQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQty.Enabled = false;
		this.txtPackingMaterialQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQty.Location = new System.Drawing.Point(93, 57);
		this.txtPackingMaterialQty.Name = "txtPackingMaterialQty";
		this.txtPackingMaterialQty.Size = new System.Drawing.Size(74, 23);
		this.txtPackingMaterialQty.TabIndex = 16;
		this.txtPackingMaterialQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputOutputValueDiff.Enabled = false;
		this.txtInputOutputValueDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputOutputValueDiff.Location = new System.Drawing.Point(333, 84);
		this.txtInputOutputValueDiff.Name = "txtInputOutputValueDiff";
		this.txtInputOutputValueDiff.Size = new System.Drawing.Size(100, 23);
		this.txtInputOutputValueDiff.TabIndex = 116;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label33);
		this.panel25.Controls.Add(this.label34);
		this.panel25.Controls.Add(this.label35);
		this.panel25.Controls.Add(this.label36);
		this.panel25.Controls.Add(this.label37);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(536, 24);
		this.panel25.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(356, 4);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(58, 17);
		this.label33.TabIndex = 6;
		this.label33.Text = "Amount";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(2, 4);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 17);
		this.label34.TabIndex = 5;
		this.label34.Text = "Description";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(101, 4);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(58, 17);
		this.label35.TabIndex = 2;
		this.label35.Text = "ItemQty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(175, 4);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(75, 17);
		this.label36.TabIndex = 4;
		this.label36.Text = "NetWeight";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(276, 4);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(35, 17);
		this.label37.TabIndex = 4;
		this.label37.Text = "Rate";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(2, 31);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(81, 19);
		this.label38.TabIndex = 1;
		this.label38.Text = "Over Heads";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(2, 60);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(95, 15);
		this.label39.TabIndex = 1;
		this.label39.Text = "Packing Material";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.labelInputOutputValueDiff.AutoSize = true;
		this.labelInputOutputValueDiff.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold);
		this.labelInputOutputValueDiff.Location = new System.Drawing.Point(256, 85);
		this.labelInputOutputValueDiff.Name = "labelInputOutputValueDiff";
		this.labelInputOutputValueDiff.Size = new System.Drawing.Size(81, 21);
		this.labelInputOutputValueDiff.TabIndex = 1;
		this.labelInputOutputValueDiff.Text = "Gain/Loss";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(2, 86);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(74, 19);
		this.label40.TabIndex = 1;
		this.label40.Text = "Difference";
		this.txtDifference.Enabled = false;
		this.txtDifference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDifference.Location = new System.Drawing.Point(93, 84);
		this.txtDifference.Name = "txtDifference";
		this.txtDifference.Size = new System.Drawing.Size(74, 23);
		this.txtDifference.TabIndex = 116;
		this.txtOverHeadQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadQty.Enabled = false;
		this.txtOverHeadQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadQty.Location = new System.Drawing.Point(93, 29);
		this.txtOverHeadQty.Name = "txtOverHeadQty";
		this.txtOverHeadQty.Size = new System.Drawing.Size(74, 23);
		this.txtOverHeadQty.TabIndex = 12;
		this.txtOverHeadQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgRate.Enabled = false;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRate.Location = new System.Drawing.Point(171, 84);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.Size = new System.Drawing.Size(82, 23);
		this.txtAvgRate.TabIndex = 118;
		this.txtPackingMaterialAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialAmount.Enabled = false;
		this.txtPackingMaterialAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialAmount.Location = new System.Drawing.Point(333, 57);
		this.txtPackingMaterialAmount.Name = "txtPackingMaterialAmount";
		this.txtPackingMaterialAmount.Size = new System.Drawing.Size(100, 23);
		this.txtPackingMaterialAmount.TabIndex = 19;
		this.txtPackingMaterialAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialRate.Enabled = false;
		this.txtPackingMaterialRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialRate.Location = new System.Drawing.Point(257, 57);
		this.txtPackingMaterialRate.Name = "txtPackingMaterialRate";
		this.txtPackingMaterialRate.Size = new System.Drawing.Size(72, 23);
		this.txtPackingMaterialRate.TabIndex = 18;
		this.txtPackingMaterialRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWagesAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmount.Enabled = false;
		this.txtWagesAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWagesAmount.Location = new System.Drawing.Point(436, 70);
		this.txtWagesAmount.Name = "txtWagesAmount";
		this.txtWagesAmount.Size = new System.Drawing.Size(99, 22);
		this.txtWagesAmount.TabIndex = 15;
		this.txtWagesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHeadAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadAmount.Enabled = false;
		this.txtOverHeadAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadAmount.Location = new System.Drawing.Point(333, 29);
		this.txtOverHeadAmount.Name = "txtOverHeadAmount";
		this.txtOverHeadAmount.Size = new System.Drawing.Size(100, 23);
		this.txtOverHeadAmount.TabIndex = 15;
		this.txtOverHeadAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHeadWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadWeight.Enabled = false;
		this.txtOverHeadWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadWeight.Location = new System.Drawing.Point(171, 29);
		this.txtOverHeadWeight.Name = "txtOverHeadWeight";
		this.txtOverHeadWeight.Size = new System.Drawing.Size(82, 23);
		this.txtOverHeadWeight.TabIndex = 13;
		this.txtOverHeadWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialWeight.Enabled = false;
		this.txtPackingMaterialWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialWeight.Location = new System.Drawing.Point(171, 57);
		this.txtPackingMaterialWeight.Name = "txtPackingMaterialWeight";
		this.txtPackingMaterialWeight.Size = new System.Drawing.Size(82, 23);
		this.txtPackingMaterialWeight.TabIndex = 17;
		this.txtPackingMaterialWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHeadRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHeadRate.Enabled = false;
		this.txtOverHeadRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverHeadRate.Location = new System.Drawing.Point(257, 29);
		this.txtOverHeadRate.Name = "txtOverHeadRate";
		this.txtOverHeadRate.Size = new System.Drawing.Size(72, 23);
		this.txtOverHeadRate.TabIndex = 14;
		this.txtOverHeadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel51.Controls.Add(this.panel52);
		this.panel51.Controls.Add(this.label25);
		this.panel51.Controls.Add(this.label29);
		this.panel51.Controls.Add(this.label31);
		this.panel51.Controls.Add(this.txtOutputByProductQty);
		this.panel51.Controls.Add(this.txtFinishGoodsQty);
		this.panel51.Controls.Add(this.txtInputQuantity);
		this.panel51.Controls.Add(this.txtInputWeight);
		this.panel51.Controls.Add(this.txtOutputByProductWeight);
		this.panel51.Controls.Add(this.txtFinishGoodsWeight);
		this.panel51.Controls.Add(this.txtInputRate);
		this.panel51.Controls.Add(this.txtOutputByProductRate);
		this.panel51.Controls.Add(this.txtFinishGoodsRate);
		this.panel51.Controls.Add(this.txtInputAmount);
		this.panel51.Controls.Add(this.txtFinishGoodsAmount);
		this.panel51.Controls.Add(this.txtOutputByProductAmount);
		this.panel51.Location = new System.Drawing.Point(4, 19);
		this.panel51.Name = "panel51";
		this.panel51.Size = new System.Drawing.Size(518, 113);
		this.panel51.TabIndex = 10;
		this.panel52.BackColor = System.Drawing.Color.Teal;
		this.panel52.Controls.Add(this.label43);
		this.panel52.Controls.Add(this.label11);
		this.panel52.Controls.Add(this.label13);
		this.panel52.Controls.Add(this.label14);
		this.panel52.Controls.Add(this.label15);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(516, 24);
		this.panel52.TabIndex = 0;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(422, 4);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(58, 17);
		this.label43.TabIndex = 6;
		this.label43.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(2, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(79, 17);
		this.label11.TabIndex = 5;
		this.label11.Text = "Description";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(113, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 17);
		this.label13.TabIndex = 2;
		this.label13.Text = "ItemQty";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(198, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 17);
		this.label14.TabIndex = 4;
		this.label14.Text = "NetWeight";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(330, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(35, 17);
		this.label15.TabIndex = 4;
		this.label15.Text = "Rate";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(2, 31);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(92, 19);
		this.label25.TabIndex = 1;
		this.label25.Text = "Raw Material";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(2, 59);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(77, 19);
		this.label29.TabIndex = 1;
		this.label29.Text = "By Product";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(2, 86);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(90, 19);
		this.label31.TabIndex = 1;
		this.label31.Text = "Finish Goods";
		this.txtOutputByProductQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductQty.Enabled = false;
		this.txtOutputByProductQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductQty.Location = new System.Drawing.Point(109, 57);
		this.txtOutputByProductQty.Name = "txtOutputByProductQty";
		this.txtOutputByProductQty.Size = new System.Drawing.Size(82, 23);
		this.txtOutputByProductQty.TabIndex = 4;
		this.txtOutputByProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsQty.Enabled = false;
		this.txtFinishGoodsQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsQty.Location = new System.Drawing.Point(109, 84);
		this.txtFinishGoodsQty.Name = "txtFinishGoodsQty";
		this.txtFinishGoodsQty.Size = new System.Drawing.Size(82, 23);
		this.txtFinishGoodsQty.TabIndex = 8;
		this.txtFinishGoodsQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputQuantity.Enabled = false;
		this.txtInputQuantity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQuantity.Location = new System.Drawing.Point(109, 29);
		this.txtInputQuantity.Name = "txtInputQuantity";
		this.txtInputQuantity.Size = new System.Drawing.Size(82, 23);
		this.txtInputQuantity.TabIndex = 0;
		this.txtInputQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputWeight.Enabled = false;
		this.txtInputWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputWeight.Location = new System.Drawing.Point(194, 29);
		this.txtInputWeight.Name = "txtInputWeight";
		this.txtInputWeight.Size = new System.Drawing.Size(107, 23);
		this.txtInputWeight.TabIndex = 1;
		this.txtInputWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductWeight.Enabled = false;
		this.txtOutputByProductWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductWeight.Location = new System.Drawing.Point(194, 57);
		this.txtOutputByProductWeight.Name = "txtOutputByProductWeight";
		this.txtOutputByProductWeight.Size = new System.Drawing.Size(107, 23);
		this.txtOutputByProductWeight.TabIndex = 5;
		this.txtOutputByProductWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsWeight.Enabled = false;
		this.txtFinishGoodsWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsWeight.Location = new System.Drawing.Point(194, 84);
		this.txtFinishGoodsWeight.Name = "txtFinishGoodsWeight";
		this.txtFinishGoodsWeight.Size = new System.Drawing.Size(107, 23);
		this.txtFinishGoodsWeight.TabIndex = 9;
		this.txtFinishGoodsWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputRate.Enabled = false;
		this.txtInputRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputRate.Location = new System.Drawing.Point(305, 29);
		this.txtInputRate.Name = "txtInputRate";
		this.txtInputRate.Size = new System.Drawing.Size(84, 23);
		this.txtInputRate.TabIndex = 2;
		this.txtInputRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductRate.Enabled = false;
		this.txtOutputByProductRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductRate.Location = new System.Drawing.Point(305, 57);
		this.txtOutputByProductRate.Name = "txtOutputByProductRate";
		this.txtOutputByProductRate.Size = new System.Drawing.Size(84, 23);
		this.txtOutputByProductRate.TabIndex = 6;
		this.txtOutputByProductRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsRate.Enabled = false;
		this.txtFinishGoodsRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsRate.Location = new System.Drawing.Point(305, 84);
		this.txtFinishGoodsRate.Name = "txtFinishGoodsRate";
		this.txtFinishGoodsRate.Size = new System.Drawing.Size(84, 23);
		this.txtFinishGoodsRate.TabIndex = 10;
		this.txtFinishGoodsRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputAmount.Enabled = false;
		this.txtInputAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputAmount.Location = new System.Drawing.Point(393, 29);
		this.txtInputAmount.Name = "txtInputAmount";
		this.txtInputAmount.Size = new System.Drawing.Size(116, 23);
		this.txtInputAmount.TabIndex = 3;
		this.txtInputAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinishGoodsAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsAmount.Enabled = false;
		this.txtFinishGoodsAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsAmount.Location = new System.Drawing.Point(393, 84);
		this.txtFinishGoodsAmount.Name = "txtFinishGoodsAmount";
		this.txtFinishGoodsAmount.Size = new System.Drawing.Size(116, 23);
		this.txtFinishGoodsAmount.TabIndex = 11;
		this.txtFinishGoodsAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductAmount.Enabled = false;
		this.txtOutputByProductAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductAmount.Location = new System.Drawing.Point(393, 57);
		this.txtOutputByProductAmount.Name = "txtOutputByProductAmount";
		this.txtOutputByProductAmount.Size = new System.Drawing.Size(116, 23);
		this.txtOutputByProductAmount.TabIndex = 7;
		this.txtOutputByProductAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 33);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1137, 238);
		this.panel4.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.chkFumigationOnHold);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1137, 28);
		this.panel3.TabIndex = 178;
		this.chkFumigationOnHold.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkFumigationOnHold.AutoSize = true;
		this.chkFumigationOnHold.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkFumigationOnHold.ForeColor = System.Drawing.Color.White;
		this.chkFumigationOnHold.Location = new System.Drawing.Point(829, 3);
		this.chkFumigationOnHold.Name = "chkFumigationOnHold";
		this.chkFumigationOnHold.Size = new System.Drawing.Size(217, 20);
		this.chkFumigationOnHold.TabIndex = 6;
		this.chkFumigationOnHold.Text = "Stock Hold For Fumigation";
		this.chkFumigationOnHold.UseVisualStyleBackColor = true;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(7, 3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(242, 19);
		this.label16.TabIndex = 0;
		this.label16.Text = "Stock Conversion Production";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1051, 3);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(84, 20);
		this.chkPrint.TabIndex = 5;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbConversionType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionDepartment);
		this.groupBox3.Controls.Add(this.label112);
		this.groupBox3.Controls.Add(this.txtdocnumber);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.txtProductionNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.txtDocdate);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtRemarks);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(7, 30);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1062, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.CmbConversionType.AllowNull = (DefaultableBoolean)1;
		this.CmbConversionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbConversionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbConversionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbConversionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbConversionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbConversionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbConversionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbConversionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbConversionType).Location = new System.Drawing.Point(275, 13);
		((System.Windows.Forms.Control)(object)this.CmbConversionType).Name = "CmbConversionType";
		((System.Windows.Forms.Control)(object)this.CmbConversionType).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbConversionType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbConversionType).Leave += new System.EventHandler(CmbConversionType_Leave);
		this.CmbDifferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDifferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDifferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDifferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDifferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Location = new System.Drawing.Point(781, 41);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Name = "CmbDifferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Size = new System.Drawing.Size(272, 26);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).TabIndex = 7;
		this.CmbProductionDepartment.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Location = new System.Drawing.Point(542, 41);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Name = "CmbProductionDepartment";
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).TabIndex = 5;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(443, 47);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(94, 15);
		this.label112.TabIndex = 178;
		this.label112.Text = "Pro_Department";
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(62, 15);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(122, 23);
		this.txtdocnumber.TabIndex = 0;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(185, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(94, 15);
		this.label51.TabIndex = 0;
		this.label51.Text = "Conversion Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(443, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 176;
		this.label27.Text = "Parent Category";
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(275, 43);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(166, 23);
		this.txtProductionNo.TabIndex = 3;
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(542, 13);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Leave += new System.EventHandler(cmbParentCategory_Leave);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(724, 47);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(49, 15);
		this.label52.TabIndex = 149;
		this.label52.Text = "Diff A/c";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(724, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 149;
		this.label6.Text = "Remarks";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(3, 47);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(57, 15);
		this.label50.TabIndex = 140;
		this.label50.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 140;
		this.label1.Text = "Doc No";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(62, 43);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(122, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.Leave += new System.EventHandler(txtDocdate_ValueChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(185, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 147;
		this.label3.Text = "Production No";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(781, 13);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(272, 26);
		this.txtRemarks.TabIndex = 6;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMoistureSlab);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtMoisture);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEntryType);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtAverageRate);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtStock);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBagType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.txtdeailRemarks);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateUom);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtUnitWeight);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGodown);
		this.groupBox1.Controls.Add(this.Add);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 102);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1063, 129);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.cmbMoistureSlab.AllowNull = (DefaultableBoolean)1;
		this.cmbMoistureSlab.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMoistureSlab.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMoistureSlab.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMoistureSlab).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbMoistureSlab.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Location = new System.Drawing.Point(504, 95);
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Name = "cmbMoistureSlab";
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).Size = new System.Drawing.Size(106, 26);
		((System.Windows.Forms.Control)(object)this.cmbMoistureSlab).TabIndex = 15;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(504, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(80, 15);
		this.label9.TabIndex = 178;
		this.label9.Text = "Moisture Slab";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(430, 76);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 15);
		this.label4.TabIndex = 176;
		this.label4.Text = "Moisture (%)";
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(430, 97);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(72, 23);
		this.txtMoisture.TabIndex = 14;
		this.cmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.cmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Location = new System.Drawing.Point(3, 40);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Name = "cmbEntryType";
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Leave += new System.EventHandler(cmbEntryType_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(171, 76);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(79, 15);
		this.label30.TabIndex = 165;
		this.label30.Text = "Avg Rate (Kg)";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(280, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(57, 15);
		this.label8.TabIndex = 153;
		this.label8.Text = "Crop Year";
		this.txtAverageRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAverageRate.Enabled = false;
		this.txtAverageRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAverageRate.Location = new System.Drawing.Point(171, 97);
		this.txtAverageRate.Name = "txtAverageRate";
		this.txtAverageRate.Size = new System.Drawing.Size(75, 23);
		this.txtAverageRate.TabIndex = 11;
		this.txtAverageRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(684, 21);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(37, 15);
		this.label28.TabIndex = 163;
		this.label28.Text = "Stock";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(553, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(46, 15);
		this.label10.TabIndex = 155;
		this.label10.Text = "Job Lot";
		this.txtStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStock.Enabled = false;
		this.txtStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStock.Location = new System.Drawing.Point(684, 42);
		this.txtStock.Name = "txtStock";
		this.txtStock.Size = new System.Drawing.Size(100, 23);
		this.txtStock.TabIndex = 5;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(988, 96);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(66, 26);
		this.btnCancelDetail.TabIndex = 19;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(916, 96);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbBagType.AllowNull = (DefaultableBoolean)1;
		this.cmbBagType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBagType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBagType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbBagType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBagType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Location = new System.Drawing.Point(857, 40);
		((System.Windows.Forms.Control)(object)this.cmbBagType).Name = "cmbBagType";
		((System.Windows.Forms.Control)(object)this.cmbBagType).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbBagType).TabIndex = 7;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(786, 40);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbUOM).TextChanged += new System.EventHandler(cmbUOM_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(786, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 142;
		this.label44.Text = "UOM";
		this.txtdeailRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeailRemarks.Location = new System.Drawing.Point(614, 97);
		this.txtdeailRemarks.Name = "txtdeailRemarks";
		this.txtdeailRemarks.Size = new System.Drawing.Size(258, 23);
		this.txtdeailRemarks.TabIndex = 16;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(614, 80);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(52, 15);
		this.label21.TabIndex = 172;
		this.label21.Text = "Remarks";
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(280, 40);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Leave += new System.EventHandler(CmbCropyr_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(857, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 15);
		this.label12.TabIndex = 157;
		this.label12.Text = "Packing Type";
		this.cmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.cmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance145;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance151).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Location = new System.Drawing.Point(248, 95);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Name = "cmbRateUom";
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).TextChanged += new System.EventHandler(cmbRateUom_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(119, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 151;
		this.label5.Text = "WareHouse";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(978, 19);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(53, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Quantity";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(248, 76);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(61, 15);
		this.label47.TabIndex = 170;
		this.label47.Text = "Rate UOM";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(978, 42);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(80, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 76);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 169;
		this.label32.Text = "Weight";
		this.txtUnitWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUnitWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUnitWeight.Location = new System.Drawing.Point(3, 97);
		this.txtUnitWeight.Name = "txtUnitWeight";
		this.txtUnitWeight.Size = new System.Drawing.Size(90, 23);
		this.txtUnitWeight.TabIndex = 9;
		this.txtUnitWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtUnitWeight.TextChanged += new System.EventHandler(txtUnitWeight_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(3, 21);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 15);
		this.label23.TabIndex = 167;
		this.label23.Text = "Entry Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(371, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(32, 15);
		this.label7.TabIndex = 141;
		this.label7.Text = "Item";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(96, 76);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(30, 15);
		this.label24.TabIndex = 161;
		this.label24.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(95, 97);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(75, 23);
		this.txtRate.TabIndex = 10;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(329, 76);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 162;
		this.label26.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(329, 97);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 13;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbGodown.AllowNull = (DefaultableBoolean)1;
		this.cmbGodown.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGodown.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGodown.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbGodown).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGodown.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Location = new System.Drawing.Point(119, 40);
		((System.Windows.Forms.Control)(object)this.cmbGodown).Name = "cmbGodown";
		((System.Windows.Forms.Control)(object)this.cmbGodown).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbGodown).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Leave += new System.EventHandler(cmbGodown_Leave);
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(882, 96);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(34, 26);
		this.Add.TabIndex = 17;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(AddInGrid_Click);
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(553, 40);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbLot).Leave += new System.EventHandler(cmbLot_Leave);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(371, 40);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1137, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.Print, this.btnVoucher, this.btnGenerate, this.btnIssuanceLoad, this.btnStockReleaseFromFumigation,
			this.BtnLoadOutPut, this.btnshortcutkeys, this.btnWagesSchedule, this.btnWagesExempt
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1137, 27);
		this.toolStrip1.TabIndex = 1;
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "605-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher.Text = "Voucher-118";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.btnGenerate.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(91, 24);
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.btnIssuanceLoad.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnIssuanceLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIssuanceLoad.Name = "btnIssuanceLoad";
		this.btnIssuanceLoad.Size = new System.Drawing.Size(126, 24);
		this.btnIssuanceLoad.Text = "Issuance Load";
		this.btnIssuanceLoad.Click += new System.EventHandler(btnIssuanceLoad_Click);
		this.btnStockReleaseFromFumigation.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnStockReleaseFromFumigation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStockReleaseFromFumigation.Name = "btnStockReleaseFromFumigation";
		this.btnStockReleaseFromFumigation.Size = new System.Drawing.Size(200, 24);
		this.btnStockReleaseFromFumigation.Text = "Release Fumigation Stock";
		this.btnStockReleaseFromFumigation.Visible = false;
		this.btnStockReleaseFromFumigation.Click += new System.EventHandler(btnStockReleaseFromFumigation_Click);
		this.BtnLoadOutPut.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadOutPut.Name = "BtnLoadOutPut";
		this.BtnLoadOutPut.Size = new System.Drawing.Size(113, 24);
		this.BtnLoadOutPut.Text = "Load OutPut";
		this.BtnLoadOutPut.Click += new System.EventHandler(BtnLoadOutPut_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnWagesSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesSchedule.Name = "btnWagesSchedule";
		this.btnWagesSchedule.Size = new System.Drawing.Size(119, 24);
		this.btnWagesSchedule.Text = "Wages Schedule";
		this.btnWagesSchedule.Click += new System.EventHandler(btnWagesSchedule_Click);
		this.btnWagesExempt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesExempt.Name = "btnWagesExempt";
		this.btnWagesExempt.Size = new System.Drawing.Size(142, 24);
		this.btnWagesExempt.Text = "Wages Exempt Item";
		this.btnWagesExempt.Click += new System.EventHandler(btnWagesExempt_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryMain);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1503, 652);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryMain.AllowEdit = (InheritableBoolean)2;
		grdHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryMain.DesignTimeLayout = grdHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryMain.DynamicFiltering = true;
		this.grdHistoryMain.FilterMode = (FilterMode)2;
		this.grdHistoryMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryMain.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Location = new System.Drawing.Point(3, 107);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Name = "grdHistoryMain";
		((ScrollableBase)this.grdHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Size = new System.Drawing.Size(1497, 160);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).TabIndex = 1;
		this.grdHistoryMain.TotalRow = (InheritableBoolean)1;
		this.grdHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryMain).VisualStyle = (VisualStyle)4;
		this.grdHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(grdHistoryMain_ColumnButtonClick);
		this.grdHistoryMain.SelectionChanged += new System.EventHandler(grdHistoryMain_SelectionChanged);
		this.grdHistoryMain.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).DoubleClick += new System.EventHandler(grdHistoryMain_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistoryMain_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label41);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel18.Location = new System.Drawing.Point(3, 267);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1497, 21);
		this.panel18.TabIndex = 49;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(4, 2);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(313, 17);
		this.label41.TabIndex = 71;
		this.label41.Text = "Input and Out Detail of Above Selected Row";
		this.grdhistoryInputOutputDetail.AllowEdit = (InheritableBoolean)2;
		grdhistoryInputOutputDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryInputOutputDetail.DesignTimeLayout = grdhistoryInputOutputDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistoryInputOutputDetail.DynamicFiltering = true;
		this.grdhistoryInputOutputDetail.FilterMode = (FilterMode)2;
		this.grdhistoryInputOutputDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryInputOutputDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).Location = new System.Drawing.Point(3, 288);
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).Name = "grdhistoryInputOutputDetail";
		((ScrollableBase)this.grdhistoryInputOutputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).Size = new System.Drawing.Size(1497, 209);
		((System.Windows.Forms.Control)(object)this.grdhistoryInputOutputDetail).TabIndex = 2;
		this.grdhistoryInputOutputDetail.TotalRow = (InheritableBoolean)1;
		this.grdhistoryInputOutputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryInputOutputDetail).VisualStyle = (VisualStyle)4;
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 497);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1497, 152);
		this.panel13.TabIndex = 48;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryPM);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(522, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(975, 152);
		this.panel16.TabIndex = 4;
		this.grdHistoryPM.AllowEdit = (InheritableBoolean)2;
		grdHistoryPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryPM.DesignTimeLayout = grdHistoryPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryPM.DynamicFiltering = true;
		this.grdHistoryPM.FilterMode = (FilterMode)2;
		this.grdHistoryPM.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Name = "grdHistoryPM";
		((ScrollableBase)this.grdHistoryPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Size = new System.Drawing.Size(975, 131);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).TabIndex = 0;
		this.grdHistoryPM.TotalRow = (InheritableBoolean)1;
		this.grdHistoryPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryPM).VisualStyle = (VisualStyle)4;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label22);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(975, 21);
		this.panel17.TabIndex = 46;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(4, 2);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(234, 17);
		this.label22.TabIndex = 71;
		this.label22.Text = "PM Detail of Above Selected Row";
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOH);
		this.panel15.Controls.Add(this.panel14);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(522, 152);
		this.panel15.TabIndex = 3;
		this.grdHistoryOH.AllowEdit = (InheritableBoolean)2;
		grdHistoryOH_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOH.DesignTimeLayout = grdHistoryOH_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOH.DynamicFiltering = true;
		this.grdHistoryOH.FilterMode = (FilterMode)2;
		this.grdHistoryOH.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOH.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).Name = "grdHistoryOH";
		((ScrollableBase)this.grdHistoryOH).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).Size = new System.Drawing.Size(522, 131);
		((System.Windows.Forms.Control)(object)this.grdHistoryOH).TabIndex = 0;
		this.grdHistoryOH.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOH.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOH).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label20);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(522, 21);
		this.panel14.TabIndex = 3;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(4, 2);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(235, 17);
		this.label20.TabIndex = 71;
		this.label20.Text = "OH Detail of Above Selected Row";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Controls.Add(this.label2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 85);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1497, 22);
		this.panel9.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1446, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdHistoryMain;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(48, 22);
		this.ctrlGrdBar3.TabIndex = 72;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(132, 18);
		this.label2.TabIndex = 71;
		this.label2.Text = "Filtered Records";
		this.panel10.Controls.Add(this.RdApproveDate);
		this.panel10.Controls.Add(this.ToDateHistory);
		this.panel10.Controls.Add(this.rdmodifydate);
		this.panel10.Controls.Add(this.drdocdate);
		this.panel10.Controls.Add(this.rdentrydate);
		this.panel10.Controls.Add(this.label42);
		this.panel10.Controls.Add(this.txtToDocNoHistory);
		this.panel10.Controls.Add(this.txtFromDocNoHistory);
		this.panel10.Controls.Add(this.label45);
		this.panel10.Controls.Add(this.btnshowHistory);
		this.panel10.Controls.Add(this.FromDateHistory);
		this.panel10.Controls.Add(this.label46);
		this.panel10.Controls.Add(this.label48);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 53);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1497, 32);
		this.panel10.TabIndex = 0;
		this.RdApproveDate.AutoSize = true;
		this.RdApproveDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdApproveDate.Location = new System.Drawing.Point(931, 7);
		this.RdApproveDate.Name = "RdApproveDate";
		this.RdApproveDate.Size = new System.Drawing.Size(96, 17);
		this.RdApproveDate.TabIndex = 8;
		this.RdApproveDate.Text = "Approve Date";
		this.RdApproveDate.UseVisualStyleBackColor = true;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(251, 4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(135, 23);
		this.ToDateHistory.TabIndex = 1;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(839, 6);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(681, 6);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(756, 6);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(502, 8);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(39, 15);
		this.label42.TabIndex = 627;
		this.label42.Text = "To No";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(541, 4);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(440, 4);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(389, 8);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(54, 15);
		this.label45.TabIndex = 626;
		this.label45.Text = "From No";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(605, 2);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(206, 8);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 15);
		this.label46.TabIndex = 624;
		this.label46.Text = "Date To";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(2, 8);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(63, 15);
		this.label48.TabIndex = 625;
		this.label48.Text = "Date From";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1497, 50);
		this.panel11.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.btnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1497, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1151, 681);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "invfrmStockConversionProduction";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(invfrmStockConversionProduction_FormClosing);
		base.Load += new System.EventHandler(invfrmStockConversionProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(invfrmStockConversionProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabInput.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.tabOutPut.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotForGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.tabPm.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPM).EndInit();
		this.tabOh.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOH).EndInit();
		this.tabwages.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStiching).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel51.ResumeLayout(false);
		this.panel51.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbConversionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMoistureSlab).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMain).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryInputOutputDetail).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOH).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
