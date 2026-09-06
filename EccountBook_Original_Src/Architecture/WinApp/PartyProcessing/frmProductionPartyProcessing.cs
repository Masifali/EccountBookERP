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
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Reports.Party_Processing;
using Architecture.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class frmProductionPartyProcessing : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecIdInput;

	private int RecIdOutPut;

	private int RecIdPM;

	private int updateDetailIndexOutPut = -1;

	private int updateDetailIndexinput = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	public static int StockPartyIdWIP;

	private bool WagesStatus;

	private bool WagesActiveOrInActiveForInput;

	private bool WagesActiveOrInActiveForOutPut;

	private DataTable dtInput = new DataTable();

	private DataTable dtByProduct = new DataTable();

	private DataTable dtEntryType = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtItemPM = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private List<InvFoodProductionPartyProcessingDetail> lstRemoveRecordInput = new List<InvFoodProductionPartyProcessingDetail>();

	private List<InvFoodProductionPartyProcessingDetail> lstRemoveRecordOutput = new List<InvFoodProductionPartyProcessingDetail>();

	private LoadavailableTransactionsForIssuancePartyProcessing LoadTransactions;

	private IContainer components;

	private Panel panel1;

	private TabPage InPut;

	internal GridEX grdInput;

	private DateTimePicker DocDateInput;

	private Label label1;

	private Label label6;

	private TextBox DocNoInput;

	private TextBox txtRemarks;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNewInput;

	private ToolStripButton btnSaveInput;

	private ToolStripButton btnUpdateInput;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton BtnPrintInput;

	private TabPage tabPage2;

	private Panel panel10;

	private Panel panel13;

	private Panel panel17;

	internal GridEX grdTransactionhistoryDetail;

	internal GridEX grdTransactionHistoryMain;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnSummeryReport;

	private TabPage PM;

	private Panel panel7;

	private Panel panel24;

	private Label label41;

	private TextBox txtPMQTY;

	internal UltraCombo cmbItemPM;

	private Label label40;

	private DateTimePicker txtdocdatePM;

	private TextBox txtDocNoPM;

	private Panel panel22;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewPM;

	private ToolStripButton btnSavePM;

	private ToolStripButton btnUpdatePM;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton BtnPrintPM;

	private Label label63;

	private TextBox txtremarksPM;

	private GroupBox groupBox2;

	internal GridEX grdPM;

	internal UltraCombo cmbJobOrderPM;

	private Label label35;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TabPage OutPut;

	private Panel panel3;

	private Button btnCancelOutPut;

	private Button btnUpdateDetailOutPut;

	private TextBox txtRemarksOutPut;

	private Label label9;

	internal UltraCombo CmbUomOutPut;

	private TextBox txtNetWeightOutPut;

	private Label label11;

	private Label label13;

	internal UltraCombo CmbWarehouseOutput;

	internal UltraCombo CmbLotOutPut;

	internal UltraCombo CmbItemOutput;

	internal UltraCombo CmbCropYearOutPut;

	private Button btnAddOutPut;

	private DateTimePicker DocDateOutPut;

	private Label label16;

	private Label label19;

	private Label label20;

	private Label label24;

	private TextBox txtDocNoOutPut;

	private TextBox txtQtyOutPut;

	private TextBox txtRemarksHeaderOutPut;

	private Label label36;

	private Label label43;

	private Label label45;

	internal UltraCombo CmbPackTypeOutPut;

	private Label label69;

	private Label label70;

	private Panel panel6;

	private ToolStrip toolStrip6;

	private ToolStripButton btnNewOutPut;

	private ToolStripButton btnSaveOutPut;

	private ToolStripButton btnUpdateOutPut;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnPrintOutPut;

	private Label label23;

	internal UltraCombo CmbJobOrderNoInput;

	private Label label60;

	internal UltraCombo CmbJobOrderNoOutput;

	internal UltraCombo CmbEntryType;

	private Label label77;

	internal UltraCombo CmbWIPItemInput;

	private TextBox txtFGWeight;

	private Label label75;

	private TextBox txtFGRate;

	private Label label76;

	private Button btnApplyRate;

	private Panel panel28;

	private Panel panel30;

	private Panel panel31;

	private ToolStripButton btnJobOrderDefine;

	private TextBox txtAvgFinishGoodRate;

	private ToolStripButton btnRefreshInput;

	private Label label54;

	private Label label53;

	private TextBox txtInPutTotalQty;

	private TextBox txtInPutTotalWeight;

	private ToolStripButton btnRefreshOutPut;

	private TextBox txtRemarksInput;

	private Label label61;

	private Label label65;

	internal UltraCombo cmbPackingTypeInput;

	private Label label66;

	private Label label67;

	private Label label72;

	private TextBox txtBalanceQtyInputDetail;

	private Label label74;

	private Label label78;

	private Button btnAddInput;

	private Button btnCancelInput;

	internal UltraCombo cmbCropYearInput;

	private Button btnUpdateDetailInput;

	internal UltraCombo cmbItemInput;

	internal UltraCombo cmbLotInput;

	private Label label80;

	internal UltraCombo cmbWareHouseInput;

	internal UltraCombo cmbItemUomInput;

	private TextBox txtBalanceWeightInputDetail;

	private Label label82;

	private Label label64;

	private TextBox txtQtyInput;

	private TextBox txtNetWeightInput;

	private Label label81;

	private Label label4;

	internal UltraCombo cmbReferencePartyInput;

	private Label label3;

	internal UltraCombo CmbStockPartyInput;

	private Label label5;

	internal UltraCombo cmbWarehouse;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnLoadInvoices;

	private Label label7;

	internal UltraCombo CmbStockPartyWIPInput;

	internal UltraCombo CmbJobOrderTransactionHistory;

	private ToolStripButton btn609ProductionIssuance;

	private Label label10;

	private Label label12;

	private TextBox txtOutPutTotalQty;

	private TextBox txtOutPutTotalWeight;

	private TabPage tabInputForm;

	private TabPage tabInputHistory;

	private TabPage tabOutputForm;

	private TabPage tabOutputHistory;

	internal GridEX grdInputHistoryMain;

	internal GridEX grdOutputHistoryMain;

	private Panel panel5;

	private ToolStrip toolStrip5;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdOutPut;

	private GroupBox groupBox1;

	private Label label14;

	private GroupBox groupBox3;

	private Panel panel8;

	private Panel panel59;

	private Label label127;

	private CtrlGrdBar ctrlGrdBar10;

	private Panel panel58;

	private GroupBox groupBox11;

	private Button BtnShowInputHistory;

	private Label label145;

	internal UltraCombo CmbJobOrderInputHistory;

	private DateTimePicker ToDateHistoryInput;

	private Label label146;

	private Label label147;

	private TextBox ToDocNoHistoryInput;

	private DateTimePicker FromDateHistoryInput;

	private Label label148;

	private Label label149;

	private TextBox FromNoHistoryInput;

	private Panel panel78;

	private Label label130;

	internal GridEX grdInputHistoryDetail;

	private ToolStripButton BtnRefreshInputHistory;

	private ToolStripButton BtnNewHistoryInput;

	private Label label124;

	private RadioButton RdModifyDate;

	private RadioButton RrDocDateInput;

	private RadioButton RdEntryDateInput;

	private Label label2;

	private Label label17;

	private GroupBox groupBox5;

	private GroupBox groupBox4;

	private Label label15;

	private Panel panel14;

	private Label label29;

	internal GridEX grdOutputHistoryDetail;

	private Panel panel4;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel9;

	private RadioButton RadModifyDateHistoryOutPut;

	private RadioButton RadDocDateHistoryOutPut;

	private RadioButton RadEntryDateHistoryOutPut;

	private GroupBox groupBox6;

	internal UltraCombo CmbJobOrderHistoryOutPut;

	private Button tnShowHistoryOutPut;

	private Label label21;

	private DateTimePicker ToDateHistoryOutPut;

	private Label label22;

	private Label label25;

	private TextBox ToNoHistoryOutPut;

	private DateTimePicker FromDateHistoryOutPut;

	private Label label26;

	private Label label27;

	private TextBox FromNoHistoryOutPut;

	private Panel panel12;

	private Label label28;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnNewHistoryOutPut;

	private ToolStripButton BtnRefreshHistoryOutPut;

	private Panel panel18;

	private Label label30;

	private Label label44;

	private Panel panel19;

	private RadioButton RadModifyDatePMHistory;

	private RadioButton RadDocDatePMHistory;

	private RadioButton RadEntrydatePMHistory;

	private GroupBox groupBox7;

	internal UltraCombo CmbJobOrderPMHistory;

	private Button BtnShowPMHistory;

	private Label label31;

	private DateTimePicker ToDatePMHistory;

	private Label label32;

	private Label label34;

	private TextBox ToDocPMHistory;

	private DateTimePicker FromDatePMHistory;

	private Label label37;

	private Label label38;

	private TextBox FromDocPMHistory;

	private Panel panel20;

	private Label label39;

	private ToolStrip toolStrip7;

	private ToolStripButton BtnNewPMHistory;

	private ToolStripButton BtnRefereshPMHistory;

	private Label label42;

	private RadioButton rdbtnItemCodeInput;

	private RadioButton rdbtnItemNameInput;

	private RadioButton rdbtnItemCodeOutPut;

	private RadioButton rdbtnItemNameOutPut;

	private RadioButton rdbtnItemCodePM;

	private RadioButton rdbtnItemNamePM;

	private CheckBox ChkPrintPreviewInput;

	private CheckBox ChkPrintPreviewOutPut;

	private ToolStripButton BtnDeleteInput;

	private ToolStripButton BtnDeleteOutput;

	private Panel panel21;

	private RadioButton RadModifyDateTransactionHistory;

	private RadioButton RadDocDateTransactionHistory;

	private RadioButton RadEntryDateTransactionHistory;

	private GroupBox groupBox8;

	private Button BtnShowTransactionHistory;

	private Label label33;

	private DateTimePicker ToDateTransactionHistory;

	private Label label46;

	private Label label47;

	private TextBox ToNoTransactionHistory;

	private DateTimePicker FromDateTransactionHistory;

	private Label label48;

	private Label label49;

	private TextBox FromNoTransactionHistory;

	private ToolStripButton BtnNewTransactionHistory;

	private ToolStripButton BtnRefreshTransactionHistory;

	private Label label8;

	private Label label51;

	private Label label50;

	private ToolStripButton BtnDeletePM;

	private ToolStripButton BtnRefreshPM;

	private GroupBox groupBox9;

	private CtrlGrdBar ctrlGrdBar6;

	private ToolStripButton btnShortcutKeysInput;

	private ToolStripButton btnShortcutKeysOutPut;

	private ToolStripButton btnShortcutKeysPM;

	private ToolStripButton btnShortcutKeysTransactionHistory;

	private TextBox txtEbTotal;

	private Label label165;

	private TextBox txtEbUnit;

	private Label label166;

	private Label label167;

	private TextBox txtGrossWeight;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	public TabControl tabControlMain;

	public TabControl tabControlInput;

	public TabControl tabControlOutPut;

	public frmProductionPartyProcessing(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFoodProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSaveInput.Enabled = formright.DoHaveSaveRight;
			btnSaveOutPut.Enabled = formright.DoHaveSaveRight;
			btnUpdateInput.Enabled = formright.DoHaveUpdateRights;
			BtnDeleteInput.Enabled = formright.DoHaveCanDelete;
			btnUpdateOutPut.Enabled = formright.DoHaveUpdateRights;
			BtnDeleteOutput.Enabled = formright.DoHaveCanDelete;
			BtnPrintInput.Enabled = formright.DoHavePrintRights;
			btnPrintOutPut.Enabled = formright.DoHavePrintRights;
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
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnProductionPartyProcessing"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 117);
			WagesActiveOrInActiveForInput = obj != null && obj.IsActive;
			WagesRefDocumentsStatus obj2 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 118);
			WagesActiveOrInActiveForOutPut = obj2 != null && obj2.IsActive;
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItemPM.Columns.Add("Id", typeof(int));
			dtItemPM.Columns.Add("ItemName");
			dtItemPM.Columns.Add("ItemCode");
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("RefDocumentTypeId", typeof(int));
			dtInput.Columns.Add("RefDocIdNo", typeof(int));
			dtInput.Columns.Add("RefDocSubIdNo", typeof(int));
			dtInput.Columns.Add("DocDate");
			dtInput.Columns.Add("DocNo", typeof(int));
			dtInput.Columns.Add("StockPartyId", typeof(int));
			dtInput.Columns.Add("StockParty");
			dtInput.Columns.Add("WareHouseId", typeof(int));
			dtInput.Columns.Add("WareHouse");
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemCode");
			dtInput.Columns.Add("Item");
			dtInput.Columns.Add("CropYearId", typeof(int));
			dtInput.Columns.Add("CropYear");
			dtInput.Columns.Add("JobLotId", typeof(int));
			dtInput.Columns.Add("JobLot");
			dtInput.Columns.Add("PackingTypeId", typeof(int));
			dtInput.Columns.Add("PackingType");
			dtInput.Columns.Add("ItemUOMId", typeof(int));
			dtInput.Columns.Add("ItemUOM");
			dtInput.Columns.Add("ItemUOMEquivalent", typeof(double));
			dtInput.Columns.Add("Quantity", typeof(double));
			dtInput.Columns.Add("Weight", typeof(double));
			dtInput.Columns.Add("BalanceQty", typeof(double));
			dtInput.Columns.Add("BalanceWeight", typeof(double));
			dtInput.Columns.Add("ReferencePartyId", typeof(int));
			dtInput.Columns.Add("ReferenceParty");
			dtInput.Columns.Add("Remarks");
			dtInput.Columns.Add("ActionId", typeof(int));
			grdInput.DataSource = dtInput;
			grdInput.RetrieveStructure();
			gridsettings();
			dtByProduct.Columns.Add("Id", typeof(int));
			dtByProduct.Columns.Add("EntryType");
			dtByProduct.Columns.Add("WareHouseId", typeof(int));
			dtByProduct.Columns.Add("WareHouse");
			dtByProduct.Columns.Add("ItemId", typeof(int));
			dtByProduct.Columns.Add("ItemCode");
			dtByProduct.Columns.Add("Item");
			dtByProduct.Columns.Add("CropYearId", typeof(int));
			dtByProduct.Columns.Add("CropYear");
			dtByProduct.Columns.Add("JobLotId", typeof(int));
			dtByProduct.Columns.Add("JobLot");
			dtByProduct.Columns.Add("PackingTypeId", typeof(int));
			dtByProduct.Columns.Add("PackingType");
			dtByProduct.Columns.Add("ItemUOMId", typeof(int));
			dtByProduct.Columns.Add("ItemUOM");
			dtByProduct.Columns.Add("ItemUOMEquivalent", typeof(double));
			dtByProduct.Columns.Add("Quantity", typeof(double));
			dtByProduct.Columns.Add("GrossWeight", typeof(double));
			dtByProduct.Columns.Add("EbUnit", typeof(double));
			dtByProduct.Columns.Add("EbTotal", typeof(double));
			dtByProduct.Columns.Add("Weight", typeof(double));
			dtByProduct.Columns.Add("Remarks");
			grdOutPut.DataSource = dtByProduct;
			grdOutPut.RetrieveStructure();
			gridsettingsOutPut();
			MultiLanguagesGetAll();
			GenerateDocNumberInput();
			GenerateDocNumberOutPut();
			JobOrderNoFill();
			EntryTypeFill();
			Warehouse();
			ItemFill();
			CropYear();
			combojoblotfill();
			PackingTypeFill();
			StockPartyFill();
			ReferencePartyFill();
			GenerateCodeOfPM();
			WareHousePM();
			ItemPMFill();
			JobOrderNoFillForSummery();
			btnAddInput.Visible = true;
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
			DocDateInput.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			InputHistoryComboFill();
			FromDateHistoryInput.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistoryInput.Value = DateTime.Now;
			OutPutHistoryComboFill();
			FromDateHistoryOutPut.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistoryOutPut.Value = DateTime.Now;
			PMHistoryComboFill();
			FromDatePMHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDatePMHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagesGetAll()
	{
		try
		{
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "Language Name", false);
				((UltraGridBase)CmbLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbJobOrderNoInput.Value);
			int ID2 = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			int ID3 = Conversion.ToInt(cmbJobOrderPM.Value);
			DataTable dtPJO = InvProductionJobOrderPartyProcessing.GetJobOrderNoForInvFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 120
			});
			if (dtPJO.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("PlanCode");
			for (int i = 0; i < dtPJO.Rows.Count; i++)
			{
				dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbJobOrderNoInput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
				DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
				DDL.BindDDL(dt, cmbJobOrderPM, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbJobOrderNoInput.Value = ID;
					}
					else
					{
						((Control)(object)CmbJobOrderNoInput).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbJobOrderNoOutput.Value = ID2;
					}
					else
					{
						((Control)(object)CmbJobOrderNoOutput).Text = string.Empty;
					}
				}
				if (ID3 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID3))
					{
						cmbJobOrderPM.Value = ID3;
					}
					else
					{
						((Control)(object)cmbJobOrderPM).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobOrderNoInput).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoInput).DataSource = null;
				((Control)(object)CmbJobOrderNoOutput).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoOutput).DataSource = null;
				((Control)(object)cmbJobOrderPM).Text = string.Empty;
				((UltraGridBase)cmbJobOrderPM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Warehouse()
	{
		try
		{
			int ID = Conversion.ToInt(CmbWarehouseOutput.Value);
			int ID2 = Conversion.ToInt(cmbWareHouseInput.Value);
			DataTable dt = CommonServices.WareHouseGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWarehouseOutput, "Id", "WareHouseName", "WareHouse", true);
				DDL.BindDDLNew(dt, cmbWareHouseInput, "Id", "WareHouseName", "WareHouse", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbWarehouseOutput.Value = ID;
					}
					else
					{
						((Control)(object)CmbWarehouseOutput).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						cmbWareHouseInput.Value = ID2;
					}
					else
					{
						((Control)(object)cmbWareHouseInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWarehouseOutput).Text = string.Empty;
				((UltraGridBase)CmbWarehouseOutput).DataSource = null;
				((Control)(object)cmbWareHouseInput).Text = string.Empty;
				((UltraGridBase)cmbWareHouseInput).DataSource = null;
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
			int id = Conversion.ToInt(cmbItemInput.Value);
			int id2 = Conversion.ToInt(CmbItemOutput.Value);
			DataTable dt = Item.ReadAllForItemsForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId, "");
			if (dt.Rows.Count > 0)
			{
				dtItem.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: true);
				DDL.BindDDL(dtItem, CmbItemOutput, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemInput).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbItemOutput).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (id > 0)
				{
					if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbItemInput.Value = id;
					}
					else
					{
						cmbItemInput.Value = string.Empty;
					}
				}
				if (id2 > 0)
				{
					if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
					{
						CmbItemOutput.Value = id2;
					}
					else
					{
						CmbItemOutput.Value = string.Empty;
					}
				}
			}
			else
			{
				cmbItemInput.Value = string.Empty;
				((UltraGridBase)cmbItemInput).DataSource = null;
				CmbItemOutput.Value = string.Empty;
				((UltraGridBase)CmbItemOutput).DataSource = null;
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
			int ID = Conversion.ToInt(cmbCropYearInput.Value);
			int ID2 = Conversion.ToInt(CmbCropYearOutPut.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCropYearInput, "Id", "CropYear", "Crop Year", true);
				DDL.BindDDLNew(dt, CmbCropYearOutPut, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)cmbCropYearInput).Rows[1].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbCropYearInput.Value = ID;
					}
					else
					{
						((Control)(object)cmbCropYearInput).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbCropYearOutPut.Value = ID2;
					}
					else
					{
						((Control)(object)CmbCropYearOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbCropYearInput).Text = string.Empty;
				((UltraGridBase)cmbCropYearInput).DataSource = null;
				((Control)(object)CmbCropYearOutPut).Text = string.Empty;
				((UltraGridBase)CmbCropYearOutPut).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbLotInput.Value);
			int ID2 = Conversion.ToInt(CmbLotOutPut.Value);
			DataTable dt = jobLot.AllJobLotForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLotInput, "Id", "JobLotDescription", "JobLot Description", true);
				DDL.BindDDLNew(dt, CmbLotOutPut, "Id", "JobLotDescription", "JobLot Description", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbLotInput.Value = ID;
					}
					else
					{
						((Control)(object)cmbLotInput).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbLotOutPut.Value = ID2;
					}
					else
					{
						((Control)(object)CmbLotOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLotInput).Text = string.Empty;
				((UltraGridBase)cmbLotInput).DataSource = null;
				((Control)(object)CmbLotOutPut).Text = string.Empty;
				((UltraGridBase)CmbLotOutPut).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbPackingTypeInput.Value);
			int ID2 = Conversion.ToInt(CmbPackTypeOutPut.Value);
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPackingTypeInput, "Id", "PackTypeDesc", "PackingType", true);
				DDL.BindDDLNew(dt, CmbPackTypeOutPut, "Id", "PackTypeDesc", "PackingType", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbPackingTypeInput.Value = ID;
					}
					else
					{
						((Control)(object)cmbPackingTypeInput).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbPackTypeOutPut.Value = ID2;
					}
					else
					{
						((Control)(object)CmbPackTypeOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbPackingTypeInput).Text = string.Empty;
				((UltraGridBase)cmbPackingTypeInput).DataSource = null;
				((Control)(object)CmbPackTypeOutPut).Text = string.Empty;
				((UltraGridBase)CmbPackTypeOutPut).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockPartyFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbStockPartyInput.Value);
			DataTable dt = CommonServices.GetSupplierustomerForPartyProcessing();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbStockPartyInput, "Id", "CompanyName", "Stock Party", ZeroIndex: true);
				((UltraGridBase)CmbStockPartyInput).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbStockPartyInput.Value = ID;
					}
					else
					{
						((Control)(object)CmbStockPartyInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbStockPartyInput).Text = string.Empty;
				((UltraGridBase)CmbStockPartyInput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmProductionPartyProcessing_FormClosing(object sender, FormClosingEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControlMain.SelectedIndex == 0)
		{
			DocDateInput.Focus();
		}
		else if (tabControlMain.SelectedIndex == 1)
		{
			DocDateOutPut.Focus();
		}
		else if (tabControlMain.SelectedIndex == 2)
		{
			txtdocdatePM.Focus();
		}
		else if (tabControlMain.SelectedIndex == 3)
		{
			FromDateTransactionHistory.Focus();
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
				DocDateInput.Focus();
				tabControlMain.SelectedIndex = 0;
			}
			if (e.Control && e.KeyCode == Keys.NumPad2)
			{
				tabControlMain.SelectedIndex = 1;
				DocDateOutPut.Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad3)
			{
				tabControlMain.SelectedIndex = 2;
				txtdocdatePM.Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad4)
			{
				FromDateTransactionHistory.Focus();
				tabControlMain.SelectedIndex = 3;
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControlMain.SelectedIndex == 0)
				{
					DocDateInput.Focus();
				}
				else if (tabControlMain.SelectedIndex == 1)
				{
					DocDateOutPut.Focus();
				}
				else if (tabControlMain.SelectedIndex == 2)
				{
					txtdocdatePM.Focus();
				}
				else if (tabControlMain.SelectedIndex == 3)
				{
					FromDateTransactionHistory.Focus();
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
			if (tabControlMain.SelectedIndex == 0)
			{
				if (tabControlInput.SelectedIndex == 0)
				{
					if (e.Control && e.KeyCode == Keys.S && btnSaveInput.Visible && btnSaveInput.Enabled)
					{
						btnsave_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.U && btnUpdateInput.Visible && btnUpdateInput.Enabled)
					{
						btnUpdate_Click(null, null);
					}
					if (e.Control && e.Shift && e.KeyCode == Keys.Delete && BtnDeleteInput.Visible && BtnDeleteInput.Enabled)
					{
						BtnDeleteInput_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.L)
					{
						btnLoadInvoices_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.F5)
					{
						DocDateInput.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Up)
					{
						CmbStockPartyInput.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Down)
					{
						((Control)(object)grdInput).Focus();
					}
					if (e.Control && e.KeyCode == Keys.N)
					{
						btnnew_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.R)
					{
						btnRefresh_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.P)
					{
						Print_Click(null, null);
					}
				}
				else
				{
					if (e.Control && e.KeyCode == Keys.S)
					{
						BtnShowInputHistory_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.F5)
					{
						FromDateHistoryInput.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Up)
					{
						FromDateHistoryInput.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Down)
					{
						((Control)(object)grdInputHistoryMain).Focus();
					}
					if (e.Control && e.KeyCode == Keys.N)
					{
						BtnNewHistoryInput_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.R)
					{
						BtnRefreshInputHistory_Click(null, null);
					}
				}
			}
			else if (tabControlMain.SelectedIndex == 1)
			{
				if (tabControlOutPut.SelectedIndex == 0)
				{
					if (e.Control && e.KeyCode == Keys.S && btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
					{
						btnSaveOutPut_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.U && btnUpdateOutPut.Visible && btnUpdateOutPut.Enabled)
					{
						btnUpdateOutPut_Click(null, null);
					}
					if (e.Control && e.Shift && e.KeyCode == Keys.Delete && BtnDeleteOutput.Visible && BtnDeleteOutput.Enabled)
					{
						BtnDeleteOutput_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.F5)
					{
						DocDateOutPut.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Up)
					{
						CmbEntryType.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Down)
					{
						((Control)(object)grdOutPut).Focus();
					}
					if (e.Control && e.KeyCode == Keys.N)
					{
						btnNewOutPut_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.R)
					{
						btnRefreshOutPut_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.P)
					{
						btnPrintOutPut_Click(null, null);
					}
				}
				else
				{
					if (e.Control && e.KeyCode == Keys.S)
					{
						tnShowHistoryOutPut_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.F5)
					{
						FromDateHistoryOutPut.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Up)
					{
						FromDateHistoryOutPut.Focus();
					}
					if (e.Control && e.KeyCode == Keys.Down)
					{
						((Control)(object)grdOutputHistoryMain).Focus();
					}
					if (e.Control && e.KeyCode == Keys.N)
					{
						BtnNewHistoryOutPut_Click(null, null);
					}
					if (e.Control && e.KeyCode == Keys.R)
					{
						BtnRefreshHistoryOutPut_Click(null, null);
					}
				}
			}
			else if (tabControlMain.SelectedIndex == 2)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSavePM.Visible && btnSavePM.Enabled)
				{
					btnsavePM_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdatePM.Visible && btnUpdatePM.Enabled)
				{
					btnUpdatePM_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && BtnDeletePM.Visible && BtnDeletePM.Enabled)
				{
					BtnDeletePM_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdatePM.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbItemPM.Focus();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewPM_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshPM_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					BtnPrintPM_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPM).Focus();
				}
				if (e.Control && e.KeyCode == Keys.H)
				{
					BtnShowPMHistory_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Up)
				{
					FromDatePMHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.B)
				{
					BtnNewPMHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F)
				{
					BtnRefereshPMHistory_Click(null, null);
				}
			}
			else if (tabControlMain.SelectedIndex == 3)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnShowTransactionHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateTransactionHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateTransactionHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdTransactionHistoryMain).Focus();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewTransactionHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshTransactionHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSummeryReport_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSummeryReport_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btn609ProductionIssuance_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					lblLanguage.Visible = true;
					((Control)(object)CmbLanguage).Visible = true;
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
			if (tabControlMain.SelectedIndex == 0 || tabControlMain.SelectedIndex == 1)
			{
				if (tabControlInput.SelectedIndex == 0)
				{
					dt.Rows.Add("Ctrl+S", "For Save");
					dt.Rows.Add("Ctrl+U", "For Update");
					dt.Rows.Add("Ctrl+E", "For Form Close");
					dt.Rows.Add("Ctrl+R", "For Refresh");
					dt.Rows.Add("Ctrl+N", "For New");
					dt.Rows.Add("Ctrl+P", "For Print");
					dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
					dt.Rows.Add("Ctrl+T", "For Tab Transfer");
					dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
					dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
					dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Stock Party Combo in Detail Box");
					dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
					dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
				}
				else
				{
					dt.Rows.Add("Ctrl+S", "For Show");
					dt.Rows.Add("Ctrl+E", "For Form Close");
					dt.Rows.Add("Ctrl+R", "For Refresh");
					dt.Rows.Add("Ctrl+N", "For New");
					dt.Rows.Add("Ctrl+F5", "For Focus on FromDate");
					dt.Rows.Add("Ctrl+T", "For Tab Transfer");
					dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
					dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
					dt.Rows.Add("Ctrl+ArrowUp", "For Focus On FromDate");
					dt.Rows.Add("Ctrl+ArrowRight", "For Focus From Main Grid To Detail Grid");
					dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
					dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
				}
			}
			else if (tabControlMain.SelectedIndex == 2)
			{
				dt.Rows.Add("Ctrl+S", "For Save");
				dt.Rows.Add("Ctrl+U", "For Update");
				dt.Rows.Add("Ctrl+E", "For Form Close");
				dt.Rows.Add("Ctrl+R", "For Refresh");
				dt.Rows.Add("Ctrl+N", "For New");
				dt.Rows.Add("Ctrl+P", "For Print");
				dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
				dt.Rows.Add("Ctrl+T", "For Tab Transfer");
				dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Name");
				dt.Rows.Add("", "");
				dt.Rows.Add("", "");
				dt.Rows.Add("Ctrl+H", "For Show");
				dt.Rows.Add("Ctrl+B", "For New History");
				dt.Rows.Add("Ctrl+F", "For Refresh History");
				dt.Rows.Add("Ctrl+Shift+F5", "For Focus on FromDate History");
				dt.Rows.Add("Ctrl+Shift+ArrowDown", "For Focus On Grid");
				dt.Rows.Add("Ctrl+Shift+ArrowUp", "For Focus On FromDate History");
				dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Grid ");
				dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Grid ");
			}
			else if (tabControlMain.SelectedIndex == 3)
			{
				dt.Rows.Add("Ctrl+S", "For Show");
				dt.Rows.Add("Ctrl+E", "For Form Close");
				dt.Rows.Add("Ctrl+R", "For Refresh");
				dt.Rows.Add("Ctrl+N", "For New");
				dt.Rows.Add("Ctrl+P", "For Print 608");
				dt.Rows.Add("Alt+1", "For Print 608");
				dt.Rows.Add("Alt+2", "For Print 609");
				dt.Rows.Add("Ctrl+F5", "For Focus on FromDate");
				dt.Rows.Add("Ctrl+T", "For Tab Transfer");
				dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On FromDate");
				dt.Rows.Add("Ctrl+ArrowRight", "For Focus From Main Grid To Detail Grid");
				dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
				dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			}
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeysInput_Click(object sender, EventArgs e)
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

	private void btnShortcutKeysOutPut_Click(object sender, EventArgs e)
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

	private void btnShortcutKeysPM_Click(object sender, EventArgs e)
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

	private void btnShortcutKeysTransactionHistory_Click(object sender, EventArgs e)
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

	private void GenerateDocNumberInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionPartyProcessing.GenerateCode(new InvFoodProductionPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 117
			});
			if (code > 0)
			{
				DocNoInput.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferencePartyFill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbReferencePartyInput.Value);
			DataTable dt = CommonServices.ReferencePartyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbReferencePartyInput, "Id", "ReferencePartyName", "Reference Party", true);
				((UltraGridBase)cmbReferencePartyInput).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbReferencePartyInput.Value = ID;
					}
					else
					{
						((Control)(object)cmbReferencePartyInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbReferencePartyInput).Text = string.Empty;
				((UltraGridBase)cmbReferencePartyInput).DataSource = null;
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
			string Uom = Conversion.ToString((object)((Control)(object)cmbItemUomInput).Text);
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItemUomInput.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbItemUomInput, "Id", "UOMCode", "UOM", ZeroIndex: true);
				if (!string.IsNullOrEmpty(Uom))
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Uom))
					{
						((Control)(object)cmbItemUomInput).Text = Uom;
					}
					else
					{
						((Control)(object)cmbItemUomInput).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItemUomInput).Text = string.Empty;
				((UltraGridBase)cmbItemUomInput).DataSource = null;
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrderPartyProcessing.GetGlAccountsByJobOrderId(new InvProductionJobOrderPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNoInput.Value),
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWIPItemInput, "WipItemId", "ItemName", "ItemName", false);
				DDL.BindDDLNew(dt, CmbStockPartyWIPInput, "StockPartyId", "StockParty", "StockParty", false);
				((UltraGridBase)CmbWIPItemInput).Rows[0].Activate();
				((UltraGridBase)CmbStockPartyWIPInput).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)CmbWIPItemInput).DataSource = null;
				((UltraGridBase)CmbStockPartyWIPInput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemInput_TextChanged(object sender, EventArgs e)
	{
	}

	private void cmbItemInput_Leave(object sender, EventArgs e)
	{
		try
		{
			bindUomInput();
			GetBalQtyAndWeightForPartyProcessing();
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
			GetBalQtyAndWeightForPartyProcessing();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemNameInput_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbItemInput.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbItemInput.Value);
				}
				if (rdbtnItemNameInput.Checked)
				{
					DDL.BindDDL(dtItem, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, cmbItemInput, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbItemInput.Value = ItemId;
				cmbItemInput.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (DocNoInput.Text.Trim() == string.Empty || DocNoInput.Text == "0")
		{
			MessageBox.Show("document Number Field Required");
			DocNoInput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNoInput).ActiveRow == null || Conversion.ToInt(CmbJobOrderNoInput.Value) == 0)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNoInput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWIPItemInput).ActiveRow == null || Conversion.ToInt(CmbWIPItemInput.Value) == 0)
		{
			MessageBox.Show("WIP Item Field Required");
			CmbWIPItemInput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStockPartyWIPInput).ActiveRow == null || Conversion.ToInt(CmbStockPartyWIPInput.Value) == 0)
		{
			MessageBox.Show("StockParty Field Required");
			CmbStockPartyWIPInput.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbStockPartyInput).ActiveRow == null || Conversion.ToInt(CmbStockPartyInput.Value) == 0)
		{
			MessageBox.Show("Stock Party Field Required");
			CmbStockPartyInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseInput).ActiveRow == null || Conversion.ToInt(cmbWareHouseInput.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			cmbWareHouseInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemInput).ActiveRow == null || Conversion.ToInt(cmbItemInput.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			cmbItemInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYearInput).ActiveRow == null || Conversion.ToInt(cmbCropYearInput.Value) == 0)
		{
			MessageBox.Show("CropYear Field Required");
			cmbCropYearInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLotInput).ActiveRow == null || Conversion.ToInt(cmbLotInput.Value) == 0)
		{
			MessageBox.Show("Job Lot Field Required");
			cmbLotInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingTypeInput).ActiveRow == null || Conversion.ToInt(cmbPackingTypeInput.Value) == 0)
		{
			MessageBox.Show("Packing Type Field Required");
			cmbPackingTypeInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemUomInput).ActiveRow == null || Conversion.ToInt(cmbItemUomInput.Value) == 0)
		{
			MessageBox.Show("Item Uom Field Required");
			cmbItemUomInput.Focus();
			return false;
		}
		if (txtQtyInput.Text.Trim() == string.Empty || txtQtyInput.Text == "0" || Conversion.ToDouble((object)txtQtyInput.Text) == 0.0)
		{
			MessageBox.Show("Qty Field Required");
			txtQtyInput.Focus();
			return false;
		}
		if (txtNetWeightInput.Text.Trim() == string.Empty || txtNetWeightInput.Text == "0" || Conversion.ToDouble((object)txtNetWeightInput.Text) == 0.0)
		{
			MessageBox.Show("Net Weight Field Required");
			txtNetWeightInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbReferencePartyInput).ActiveRow == null || Conversion.ToInt(cmbReferencePartyInput.Value) == 0)
		{
			MessageBox.Show("Reference Party Field Required");
			cmbReferencePartyInput.Focus();
			return false;
		}
		return true;
	}

	private void btnAddInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtInput.Rows.Add(0, 0, 0, 0, 0, 0, Conversion.ToInt(CmbStockPartyInput.Value), Conversion.ToString((object)((Control)(object)CmbStockPartyInput).Text), Conversion.ToInt(cmbWareHouseInput.Value), Conversion.ToString((object)((Control)(object)cmbWareHouseInput).Text), Conversion.ToInt(cmbItemInput.Value), Conversion.ToString(((UltraDropDownBase)cmbItemInput).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)cmbItemInput).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(cmbCropYearInput.Value), Conversion.ToString((object)((Control)(object)cmbCropYearInput).Text), Conversion.ToInt(cmbLotInput.Value), Conversion.ToString((object)((Control)(object)cmbLotInput).Text), Conversion.ToInt(cmbPackingTypeInput.Value), Conversion.ToString((object)((Control)(object)cmbPackingTypeInput).Text), Conversion.ToInt(cmbItemUomInput.Value), Conversion.ToString(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["UOMCode"].Value), Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQtyInput.Text), Conversion.ToDouble((object)txtNetWeightInput.Text), Conversion.ToDouble((object)txtBalanceQtyInputDetail.Text), Conversion.ToDouble((object)txtBalanceWeightInputDetail.Text), Conversion.ToInt(cmbReferencePartyInput.Value), Conversion.ToString((object)((Control)(object)cmbReferencePartyInput).Text), Conversion.ToString((object)txtRemarksInput.Text), 0);
				grdInput.DataSource = dtInput;
				grdInput.RetrieveStructure();
				gridsettings();
				ResetInputDetail();
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
			grdInput.RootTable.Columns["Id"].Visible = false;
			grdInput.RootTable.Columns["DocDate"].Visible = false;
			grdInput.RootTable.Columns["DocNo"].Visible = false;
			grdInput.RootTable.Columns["ItemId"].Visible = false;
			grdInput.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInput.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdInput.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdInput.RootTable.Columns["JobLotId"].Visible = false;
			grdInput.RootTable.Columns["WarehouseId"].Visible = false;
			grdInput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdInput.RootTable.Columns["ItemUOMId"].Visible = false;
			grdInput.RootTable.Columns["CropYearId"].Visible = false;
			grdInput.RootTable.Columns["StockPartyId"].Visible = false;
			grdInput.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdInput.RootTable.Columns["ActionId"].Visible = false;
			grdInput.RootTable.Columns["BalanceQty"].Visible = false;
			grdInput.RootTable.Columns["BalanceWeight"].Visible = false;
			grdInput.RootTable.Columns["ItemUOMEquivalent"].Visible = false;
			grdInput.RootTable.Columns["ActionId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdInput.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdInput.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdInput.RootTable.Columns["StockPartyId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["StockParty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Item"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemUOMEquivalent"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalanceQty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ReferencePartyId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ReferenceParty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdInput.RootTable.Columns["ActionId"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Quantity"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["Quantity"].TotalFormatString = "#,##.###";
			grdInput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Weight"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["Weight"].TotalFormatString = "#,##.###";
			grdInput.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["BalanceQty"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["BalanceQty"].TotalFormatString = "#,##.###";
			grdInput.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["BalanceWeight"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(grdInput);
			grdInput.RootTable.Columns["WareHouse"].Width = 200;
			grdInput.RootTable.Columns["Remarks"].Width = 300;
			grdInput.RootTable.Columns["Item"].Width = 300;
			grdInput.RootTable.Columns.Add("Delete");
			grdInput.RootTable.Columns["Delete"].Key = "Delete";
			grdInput.RootTable.Columns["Delete"].Caption = "X";
			grdInput.RootTable.Columns["Delete"].ButtonText = "X";
			grdInput.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInput.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInput.RootTable.Columns["Delete"].Width = 20;
			grdInput.RootTable.Columns["Delete"].Position = 0;
			grdInput.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
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
			if (FormValidationDetail())
			{
				dtInput.Rows[updateDetailIndexinput]["StockPartyId"] = Conversion.ToInt(CmbStockPartyInput.Value);
				dtInput.Rows[updateDetailIndexinput]["StockParty"] = Conversion.ToString((object)((Control)(object)CmbStockPartyInput).Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["WareHouseId"] = Conversion.ToInt(cmbWareHouseInput.Value);
				dtInput.Rows[updateDetailIndexinput]["WareHouse"] = Conversion.ToString((object)((Control)(object)cmbWareHouseInput).Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["ItemId"] = Conversion.ToInt(cmbItemInput.Value);
				dtInput.Rows[updateDetailIndexinput]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItemInput).SelectedRow.Cells["ItemCode"].Value);
				dtInput.Rows[updateDetailIndexinput]["Item"] = Conversion.ToString(((UltraDropDownBase)cmbItemInput).SelectedRow.Cells["ItemName"].Value);
				dtInput.Rows[updateDetailIndexinput]["CropYearId"] = Conversion.ToInt(cmbCropYearInput.Value);
				dtInput.Rows[updateDetailIndexinput]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbCropYearInput).Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["JobLotId"] = Conversion.ToInt(cmbLotInput.Value);
				dtInput.Rows[updateDetailIndexinput]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbLotInput).Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["PackingTypeId"] = Conversion.ToInt(cmbPackingTypeInput.Value);
				dtInput.Rows[updateDetailIndexinput]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbPackingTypeInput).Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["ItemUOMId"] = Conversion.ToInt(cmbItemUomInput.Value);
				dtInput.Rows[updateDetailIndexinput]["ItemUOM"] = Conversion.ToString(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["UOMCode"].Value);
				dtInput.Rows[updateDetailIndexinput]["ItemUOMEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["Equivalent"].Value);
				dtInput.Rows[updateDetailIndexinput]["Quantity"] = Conversion.ToDouble((object)txtQtyInput.Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["Weight"] = Conversion.ToDouble((object)txtNetWeightInput.Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["BalanceQty"] = Conversion.ToDouble((object)txtBalanceQtyInputDetail.Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["BalanceWeight"] = Conversion.ToDouble((object)txtBalanceWeightInputDetail.Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["Remarks"] = Conversion.ToString((object)txtRemarksInput.Text.Trim());
				dtInput.Rows[updateDetailIndexinput]["ReferencePartyId"] = Conversion.ToInt(cmbReferencePartyInput.Value);
				dtInput.Rows[updateDetailIndexinput]["ReferenceParty"] = Conversion.ToString((object)((Control)(object)cmbReferencePartyInput).Text.Trim());
				ResetInputDetail();
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
			ResetInputDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_DoubleClick(object sender, EventArgs e)
	{
	}

	private void grdInput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInput.UpdateData();
			GridEXRow r = grdInput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (btnSaveInput.Visible && btnSaveInput.Enabled)
			{
				if (e.Column.Key == "Quantity")
				{
					double Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Value.ToString());
					double BalanceQty = Conversion.ToDouble((object)r.Cells["BalanceQty"].Value.ToString());
					double BalanceWeight = Conversion.ToDouble((object)r.Cells["BalanceWeight"].Value.ToString());
					Conversion.ToDouble((object)r.Cells["ItemUOMEquivalent"].Value.ToString());
					double Weight2 = BalanceWeight / BalanceQty * Qty;
					r.Cells["Weight"].Value = Weight2;
					if (BalanceQty < Qty)
					{
						r.Cells["Quantity"].Value = BalanceQty;
						r.Cells["Weight"].Value = BalanceWeight;
						MessageBox.Show($"Qty cannot greater than BalanceQty {BalanceQty} Please check!");
					}
				}
				if (e.Column.Key == "Weight")
				{
					double Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					if (Conversion.ToDouble((object)r.Cells["BalanceWeight"].Value.ToString()) < Weight)
					{
						r.Cells["Weight"].Value = Conversion.ToDouble((object)r.Cells["BalanceWeight"].Value.ToString());
						MessageBox.Show(string.Format("Weight cannot greater than BalanceWeight {0} Please check!", Conversion.ToDouble((object)r.Cells["BalanceWeight"].Value.ToString())));
					}
				}
			}
			else
			{
				if (e.Column.Key == "Quantity")
				{
					double Qty2 = Conversion.ToDouble((object)r.Cells["Quantity"].Value.ToString());
					double Weight3 = Conversion.ToDouble((object)r.Cells["ItemUOMEquivalent"].Value.ToString()) * Qty2;
					r.Cells["Weight"].Value = Weight3;
				}
				if (e.Column.Key == "Weight")
				{
					Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
				}
			}
			grdInput.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdInput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndexinput != -1)
				{
					btnCancelInput.Focus();
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionPartyProcessingDetail objDetail = new InvFoodProductionPartyProcessingDetail();
					objDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					objDetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					objDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					objDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					objDetail.ActionTypeId = 3;
					objDetail.EntryType = Conversion.ToString((object)"Issue");
					objDetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					objDetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					objDetail.StockPartyId = Conversion.ToInt(r.Cells["StockPartyId"].Value);
					objDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					objDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					objDetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					objDetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					objDetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					objDetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					objDetail.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					objDetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					objDetail.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
					objDetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					objDetail.ActionId = Conversion.ToInt(r.Cells["ActionId"].Value);
					lstRemoveRecordInput.Add(objDetail);
					dtInput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
			}
			_ = e.Column.Key == "Edit";
			grdInput.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertInput()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		try
		{
			if (grdInput.RowCount == 0)
			{
				MessageBox.Show("Grd Record Not Found.\nEnter Detail First ...");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				InvFoodProductionPartyProcessing invFoodProduction = new InvFoodProductionPartyProcessing();
				if (RecIdInput > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					invFoodProduction.Id = RecIdInput;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.OrganizationId = UserAccount.OrganizationId;
				invFoodProduction.CompanyId = UserAccount.CompanyId;
				invFoodProduction.BranchId = UserAccount.BranchesId;
				invFoodProduction.ProjectId = 0;
				invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				invFoodProduction.DocumentTypeId = 117;
				invFoodProduction.EntryDate = DateTime.Now;
				invFoodProduction.ModifyDate = DateTime.Now;
				invFoodProduction.EntryUser = UserAccount.ID;
				invFoodProduction.ModifyUser = UserAccount.ID;
				invFoodProduction.ApprovedUserId = UserAccount.ID;
				invFoodProduction.ApprovedDate = DateTime.Now;
				invFoodProduction.DocNo = Conversion.ToInt((object)DocNoInput.Text);
				invFoodProduction.DocDate = Conversion.ToDateTime((object)DocDateInput.Value);
				invFoodProduction.EntryType = "Input";
				invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
				invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoInput).Text.Trim());
				invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItemInput.Value);
				invFoodProduction.ScreenName = "frmProductionPartyProcessing";
				invFoodProduction.MainRemarks = txtRemarks.Text.ToString();
				invFoodProduction.PartyProcessingDetailsList = new List<InvFoodProductionPartyProcessingDetail>();
				double GrossWeight = 0.0;
				GridEXRow[] rows = grdInput.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionPartyProcessingDetail d = new InvFoodProductionPartyProcessingDetail();
					if (!(Conversion.ToDouble((object)r.Cells["Quantity"].Text) > 0.0) || !(Conversion.ToDouble((object)r.Cells["Weight"].Text) > 0.0))
					{
						continue;
					}
					d.Id = ((RecIdInput > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					if (d.Id == 0)
					{
						d.ActionTypeId = 1;
					}
					else
					{
						d.ActionTypeId = 2;
					}
					d.EntryType = Conversion.ToString((object)"Issue");
					d.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					d.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					d.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					d.ActionId = Conversion.ToInt(r.Cells["ActionId"].Value);
					if (Conversion.ToInt((object)d.ActionId) == 1)
					{
						if (d.RefDocumentTypeId == 0)
						{
							MessageBox.Show("RefDocumentTypeId not Found");
							return;
						}
						if (d.RefDocNoId == 0)
						{
							MessageBox.Show("RefDocNoId not Found");
							return;
						}
						if (d.RefDocSubIdNo == 0)
						{
							MessageBox.Show("RefDocSubIdNo not Found");
							return;
						}
					}
					if (r.Cells["StockPartyId"].Text == string.Empty || Conversion.ToInt(r.Cells["StockPartyId"].Value) == 0)
					{
						MessageBox.Show("Stock Party Field Require in Input Detail");
						return;
					}
					if (r.Cells["WareHouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
					{
						MessageBox.Show("WareHouse Field Require in Input Detail");
						return;
					}
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Field Require in Input Detail");
						return;
					}
					if (r.Cells["CropYear"].Text == string.Empty || Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
					{
						MessageBox.Show("CropYear Field Require in Input Detail");
						return;
					}
					if (r.Cells["JobLotId"].Text == string.Empty || Conversion.ToInt(r.Cells["JobLotId"].Value) == 0)
					{
						MessageBox.Show("JobLot Field Require in Input Detail");
						return;
					}
					if (r.Cells["PackingTypeId"].Text == string.Empty || Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
					{
						MessageBox.Show("Packing Type Field Require in Input Detail");
						return;
					}
					if (r.Cells["ItemUOMId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemUOMId"].Value) == 0)
					{
						MessageBox.Show("ItemUOM Field Require in Input Detail");
						return;
					}
					if (r.Cells["Quantity"].Text == string.Empty || Conversion.ToDouble(r.Cells["Quantity"].Value) == 0.0)
					{
						throw new Exception("ItemQty Field Required");
					}
					if (r.Cells["Weight"].Text == string.Empty || Conversion.ToDouble(r.Cells["Weight"].Value) == 0.0)
					{
						throw new Exception("ItemWeight Field Required");
					}
					d.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
					d.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoInput).Text);
					d.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					d.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					d.StockPartyId = Conversion.ToInt(r.Cells["StockPartyId"].Value);
					d.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					d.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					d.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					d.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					d.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					d.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					GrossWeight += d.Weight;
					d.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					invFoodProduction.PartyProcessingDetailsList.Add(d);
				}
				if (lstRemoveRecordInput.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecordInput.Count; i++)
					{
						invFoodProduction.PartyProcessingDetailsList.Add(lstRemoveRecordInput[i]);
					}
				}
				if (invFoodProduction.PartyProcessingDetailsList.Count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}
				int success = InvFoodProductionPartyProcessing.Save(invFoodProduction);
				if (RecIdInput > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocNo);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocNo);
				}
				if (WagesStatus && WagesActiveOrInActiveForInput)
				{
					frmWagesBillPartyProcessing obj = new frmWagesBillPartyProcessing(UserAccount);
					obj.RefDocTypeId = 117;
					obj.RefDocId = success;
					obj.GrossWeightTotal = GrossWeight;
					obj.ShowDialog();
					obj.BringToFront();
				}
				ResetInputMain();
				if (ChkPrintPreviewInput.Checked)
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdInput = 0;
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
			if (RecIdInput == 0)
			{
				throw new Exception("Rec Id Not Found....");
			}
			InsertInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdInput(int ID)
	{
		try
		{
			InvFoodProductionPartyProcessing SC = InvFoodProductionPartyProcessing.GetByID(ID);
			if (SC != null && SC.EntryType == "Input")
			{
				btnSaveInput.Visible = false;
				((Control)(object)CmbJobOrderNoInput).Enabled = false;
				btnUpdateInput.Visible = true;
				BtnDeleteInput.Visible = true;
				tabControlMain.SelectedIndex = 0;
				tabControlInput.SelectedIndex = 0;
				DocNoInput.Text = Conversion.ToString((object)SC.DocNo);
				DocDateInput.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarks.Text = Conversion.ToString((object)SC.MainRemarks);
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("JobOrderNo", typeof(string));
				dt.Rows.Add(SC.InvJobOrderId, SC.InvJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNoInput, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNoInput).Rows[0].Activate();
				CmbJobOrderNo_Leave(null, null);
				dtInput.Rows.Clear();
				for (int i = 0; i < SC.PartyProcessingDetailsList.Count; i++)
				{
					if (SC.PartyProcessingDetailsList[i].EntryType == "Issue")
					{
						dtInput.Rows.Add(SC.PartyProcessingDetailsList[i].Id, SC.PartyProcessingDetailsList[i].RefDocumentTypeId, SC.PartyProcessingDetailsList[i].RefDocNoId, SC.PartyProcessingDetailsList[i].RefDocSubIdNo, Convert.ToDateTime(SC.PartyProcessingDetailsList[i].DocDate).ToString("dd-MMM-yy"), SC.PartyProcessingDetailsList[i].DocNo, SC.PartyProcessingDetailsList[i].StockPartyId, SC.PartyProcessingDetailsList[i].StockParty, SC.PartyProcessingDetailsList[i].WarehouseId, SC.PartyProcessingDetailsList[i].WareHouseName, SC.PartyProcessingDetailsList[i].ItemId, SC.PartyProcessingDetailsList[i].ItemCode, SC.PartyProcessingDetailsList[i].ItemName, SC.PartyProcessingDetailsList[i].CropYearId, SC.PartyProcessingDetailsList[i].CropBatch, SC.PartyProcessingDetailsList[i].JobLotId, SC.PartyProcessingDetailsList[i].JobLotDescription, SC.PartyProcessingDetailsList[i].PackingtypeId, SC.PartyProcessingDetailsList[i].PackTypeDesc, SC.PartyProcessingDetailsList[i].ItemUomId, SC.PartyProcessingDetailsList[i].PackUom, SC.PartyProcessingDetailsList[i].Equivalent, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].ReferencePartyId, SC.PartyProcessingDetailsList[i].ReferencePartyName, SC.PartyProcessingDetailsList[i].Remarks, SC.PartyProcessingDetailsList[i].ActionId);
					}
				}
				grdInput.DataSource = dtInput;
				grdInput.RetrieveStructure();
				gridsettings();
				return;
			}
			throw new Exception("Record Not Found...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDeleteInput_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecIdInput > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					new InvFoodProductionPartyProcessing().DeleteById(RecIdInput, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					ResetInputMain();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		ResetInputMain();
	}

	private void ResetInputMain()
	{
		try
		{
			RecIdInput = 0;
			updateDetailIndexinput = -1;
			((UltraGridBase)CmbJobOrderNoInput).DataSource = null;
			((Control)(object)CmbJobOrderNoInput).Text = string.Empty;
			((Control)(object)CmbWIPItemInput).Text = string.Empty;
			((Control)(object)CmbStockPartyWIPInput).Text = string.Empty;
			ResetInputDetail();
			dtInput.Clear();
			grdInput.ClearStructure();
			lstRemoveRecordInput.Clear();
			btnSaveInput.Visible = true;
			btnUpdateInput.Visible = false;
			BtnDeleteInput.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbJobOrderNoInput).Enabled = true;
			GenerateDocNumberInput();
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetInputDetail()
	{
		try
		{
			updateDetailIndexinput = -1;
			((Control)(object)CmbStockPartyInput).Text = string.Empty;
			((Control)(object)cmbWareHouseInput).Text = string.Empty;
			((Control)(object)cmbItemInput).Text = string.Empty;
			((Control)(object)cmbCropYearInput).Text = string.Empty;
			((Control)(object)cmbLotInput).Text = string.Empty;
			((Control)(object)cmbPackingTypeInput).Text = string.Empty;
			((Control)(object)cmbItemUomInput).Text = string.Empty;
			txtBalanceQtyInputDetail.Text = string.Empty;
			txtBalanceWeightInputDetail.Text = string.Empty;
			txtQtyInput.Text = string.Empty;
			txtNetWeightInput.Text = string.Empty;
			((Control)(object)cmbReferencePartyInput).Text = string.Empty;
			txtRemarksInput.Text = string.Empty;
			btnAddInput.Visible = true;
			btnUpdateDetailInput.Visible = false;
			btnCancelInput.Visible = false;
			CmbStockPartyInput.Focus();
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
			JobOrderNoFill();
			StockPartyFill();
			Warehouse();
			ItemFill();
			CropYear();
			combojoblotfill();
			PackingTypeFill();
			ReferencePartyFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbStockParty_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetBalQtyAndWeightForPartyProcessing();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBalQtyAndWeightForPartyProcessing()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbItemInput).ActiveRow != null && Conversion.ToInt(cmbItemInput.Value) > 0 && ((UltraGridBase)cmbWareHouseInput).ActiveRow != null && Conversion.ToInt(cmbWareHouseInput.Value) > 0 && ((UltraGridBase)CmbStockPartyInput).ActiveRow != null && Conversion.ToInt(CmbStockPartyInput.Value) > 0)
			{
				DataTable dt = GetAvgRatesAndStockInHand.GetBalQtyAndWeightForPartyProcessing(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = Conversion.ToInt(cmbItemInput.Value),
					StockPartyId = Conversion.ToInt(CmbStockPartyInput.Value),
					WarehouseId = Conversion.ToInt(cmbWareHouseInput.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtBalanceQtyInputDetail.Text = Conversion.ToDouble(dt.Rows[0]["BalQty"]).ToString();
					txtBalanceWeightInputDetail.Text = Conversion.ToDouble(dt.Rows[0]["BalStock"]).ToString();
				}
				else
				{
					txtBalanceQtyInputDetail.Text = string.Empty;
					txtBalanceWeightInputDetail.Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateBalQtyAndWeightForInputGrid()
	{
	}

	private void InputHistoryComboFill()
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
				DocumentTypeIds = "117"
			};
			DataTable dtjob = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"ReferenceName",
						typeof(string)
					}
				}
			};
			DataTable dt = InvFoodProductionPartyProcessing.GetDataForDropDownFromInvFoodProductionPartyProcessing(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvJobOrder")
					{
						dtjob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDL(dtjob, CmbJobOrderInputHistory, "Id", "ReferenceName", "JobOrder No", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbJobOrderInputHistory).DataSource = null;
			}
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
			if (tabControlInput.SelectedIndex == 1)
			{
				FromDateHistoryInput.Focus();
			}
			else
			{
				DocDateInput.Focus();
			}
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
			InputHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputHistory()
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
			obj.DocumentTypeId = 117;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (RrDocDateInput.Checked)
			{
				if (FromDateHistoryInput.Checked)
				{
					obj.FromDate = FromDateHistoryInput.Value;
				}
				if (ToDateHistoryInput.Checked)
				{
					obj.ToDate = ToDateHistoryInput.Value;
				}
			}
			else if (RdEntryDateInput.Checked)
			{
				if (FromDateHistoryInput.Checked)
				{
					obj.EntryDateFrom = FromDateHistoryInput.Value;
				}
				if (ToDateHistoryInput.Checked)
				{
					obj.EntryDateTo = ToDateHistoryInput.Value;
				}
			}
			else if (RdModifyDate.Checked)
			{
				if (FromDateHistoryInput.Checked)
				{
					obj.ModifyDateFrom = FromDateHistoryInput.Value;
				}
				if (ToDateHistoryInput.Checked)
				{
					obj.ModifyDateTo = ToDateHistoryInput.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromNoHistoryInput.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistoryInput.Text);
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderInputHistory.Value);
			DataTable dthis = InvFoodProductionPartyProcessing.FormHistory(obj);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocDate", typeof(DateTime));
				dtTarget.Columns.Add("DocNo", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("WIPItemId", typeof(int));
				dtTarget.Columns.Add("WIPItemCode", typeof(string));
				dtTarget.Columns.Add("WIPItemName", typeof(string));
				dtTarget.Columns.Add("InvJobOrderId", typeof(int));
				dtTarget.Columns.Add("InvJobOrderNo", typeof(string));
				dtTarget.Columns.Add("EntryType", typeof(string));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUserName", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUserName", typeof(string));
				foreach (DataRow row in dthis.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocDate"], row["DocNo"], row["DocumentTypeId"], row["WIPItemId"], row["WIPItemCode"], row["WIPItemName"], row["InvJobOrderId"], row["InvJobOrderNo"], row["EntryType"], row["MainRemarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
				}
				grdInputHistoryMain.DataSource = dtTarget;
				grdInputHistoryMain.RetrieveStructure();
				GridInputSetting();
			}
			else
			{
				grdInputHistoryMain.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridInputSetting()
	{
		try
		{
			grdInputHistoryMain.RootTable.Columns["Id"].Visible = false;
			grdInputHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdInputHistoryMain.RootTable.Columns["WIPItemId"].Visible = false;
			grdInputHistoryMain.RootTable.Columns["InvJobOrderId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdInputHistoryMain);
			grdInputHistoryMain.RootTable.Columns["Remarks"].Width = 250;
			grdInputHistoryMain.RootTable.Columns.Add("Print");
			grdInputHistoryMain.RootTable.Columns["Print"].Key = "Print";
			grdInputHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			grdInputHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdInputHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdInputHistoryMain.RootTable.Columns["Print"].Width = 50;
			grdInputHistoryMain.RootTable.Columns["Print"].Position = 0;
			grdInputHistoryMain.RootTable.Columns.Add("Edit");
			grdInputHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
			grdInputHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			grdInputHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdInputHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdInputHistoryMain.RootTable.Columns["Edit"].Width = 40;
			grdInputHistoryMain.RootTable.Columns["Edit"].Position = 1;
			grdInputHistoryMain.FrozenColumns = 2;
			ctrlGrdBar10_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdinputhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit" && r.Cells["EntryType"].Value.ToString() == "Input")
				{
					RecIdInput = CurrId;
					ReadByIdInput(RecIdInput);
				}
				if (e.Column.Key == "Print")
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputHistoryMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputHistoryMain.CurrentRow;
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

	private void grdInputHistoryMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				InputHistoryDetail(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputHistoryDetail(int CurrId)
	{
		try
		{
			InvFoodProductionPartyProcessing SC = InvFoodProductionPartyProcessing.GetByID(CurrId);
			if (SC == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RefDocumentTypeId", typeof(int));
			dt.Columns.Add("RefDocIdNo", typeof(int));
			dt.Columns.Add("RefDocSubIdNo", typeof(int));
			dt.Columns.Add("DocDate");
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("StockPartyId", typeof(int));
			dt.Columns.Add("StockParty");
			dt.Columns.Add("WareHouse");
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemCode");
			dt.Columns.Add("Item");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("ItemUOM");
			dt.Columns.Add("Quantity", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("ReferencePartyId", typeof(int));
			dt.Columns.Add("ReferenceParty");
			dt.Columns.Add("Remarks");
			dt.Rows.Clear();
			for (int i = 0; i < SC.PartyProcessingDetailsList.Count; i++)
			{
				if (SC.PartyProcessingDetailsList[i].EntryType == "Issue")
				{
					dt.Rows.Add(SC.PartyProcessingDetailsList[i].Id, SC.PartyProcessingDetailsList[i].RefDocumentTypeId, SC.PartyProcessingDetailsList[i].RefDocNoId, SC.PartyProcessingDetailsList[i].RefDocSubIdNo, Convert.ToDateTime(SC.PartyProcessingDetailsList[i].DocDate).ToString("dd-MMM-yy"), SC.PartyProcessingDetailsList[i].DocNo, SC.PartyProcessingDetailsList[i].StockPartyId, SC.PartyProcessingDetailsList[i].StockParty, SC.PartyProcessingDetailsList[i].WareHouseName, SC.PartyProcessingDetailsList[i].ItemId, SC.PartyProcessingDetailsList[i].ItemCode, SC.PartyProcessingDetailsList[i].ItemName, SC.PartyProcessingDetailsList[i].CropBatch, SC.PartyProcessingDetailsList[i].JobLotDescription, SC.PartyProcessingDetailsList[i].PackTypeDesc, SC.PartyProcessingDetailsList[i].PackUom, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].ReferencePartyId, SC.PartyProcessingDetailsList[i].ReferencePartyName, SC.PartyProcessingDetailsList[i].Remarks);
				}
			}
			grdInputHistoryDetail.DataSource = dt;
			grdInputHistoryDetail.RetrieveStructure();
			grdInputHistoryDetailsettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputHistoryDetailsettings()
	{
		try
		{
			grdInputHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["DocDate"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["DocNo"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["StockPartyId"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdInputHistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInputHistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInputHistoryDetail.RootTable.Columns["Quantity"].FormatString = "#,##.###";
			grdInputHistoryDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##.###";
			grdInputHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##.###";
			grdInputHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(grdInputHistoryDetail);
			grdInputHistoryDetail.RootTable.Columns["WareHouse"].Width = 200;
			grdInputHistoryDetail.RootTable.Columns["Item"].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistoryInput_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryInput.Value = DateTime.Now;
			ToDateHistoryInput.Value = DateTime.Now;
			FromNoHistoryInput.Text = string.Empty;
			ToDocNoHistoryInput.Text = string.Empty;
			((Control)(object)CmbJobOrderInputHistory).Text = string.Empty;
			grdInputHistoryMain.ClearStructure();
			grdInputHistoryDetail.ClearStructure();
			RrDocDateInput.Checked = true;
			FromDateHistoryInput.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshInputHistory_Click(object sender, EventArgs e)
	{
		try
		{
			InputHistoryComboFill();
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
			CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(RecIdInput);
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
			if (((UltraGridBase)CmbJobOrderNoInput).ActiveRow == null || Conversion.ToInt(CmbJobOrderNoInput.Value) == 0)
			{
				CmbJobOrderNoInput.Focus();
				throw new Exception("JobOrder select First");
			}
			if (((UltraGridBase)CmbStockPartyWIPInput).ActiveRow == null || Conversion.ToInt(CmbStockPartyWIPInput.Value) == 0)
			{
				CmbStockPartyWIPInput.Focus();
				throw new Exception("Stock Party Account select First");
			}
			LoadTransactions = new LoadavailableTransactionsForIssuancePartyProcessing(UserAccount);
			LoadTransactions.StockPartyId = Conversion.ToInt(CmbStockPartyWIPInput.Value);
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
			if (grdInput.RowCount > 0)
			{
				GridEXRow[] rows = grdInput.GetRows();
				for (int j = 0; j < rows.Length; j++)
				{
					if (Conversion.ToInt(rows[j].Cells["StockPartyId"].Value) != Conversion.ToInt(dtpurchaseinvoice.Rows[0]["StockPartyId"]))
					{
						throw new Exception("Data against another Stock Party Already Exist");
					}
				}
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grdInput.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					dtInput.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], Conversion.ToDateTime(dtpurchaseinvoice.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpurchaseinvoice.Rows[i]["DocCodeNo"], dtpurchaseinvoice.Rows[i]["StockPartyId"], dtpurchaseinvoice.Rows[i]["StockParty"], dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouseCode"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemCode"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["CropYearId"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["InvPackingTypeId"], dtpurchaseinvoice.Rows[i]["PackingType"], dtpurchaseinvoice.Rows[i]["ItemUomId"], dtpurchaseinvoice.Rows[i]["PackUom"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["QtyBalance"], dtpurchaseinvoice.Rows[i]["WeightBalance"], dtpurchaseinvoice.Rows[i]["QtyBalance"], dtpurchaseinvoice.Rows[i]["WeightBalance"], dtpurchaseinvoice.Rows[i]["SupplierCustomerId"], dtpurchaseinvoice.Rows[i]["ReferencePartyName"], "", 1);
				}
			}
			grdInput.DataSource = dtInput;
			grdInput.RetrieveStructure();
			gridsettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void FromNoHistoryInput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistoryInput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnJobOrderDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new ProductionJobOrderPartyProcessing(UserAccount).Show();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInput).Name);
			if (fs != null)
			{
				grdInput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar10_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInputHistoryMain).Name);
			if (fs != null)
			{
				grdInputHistoryMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdInput_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdInput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInput.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)grdInput.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInput.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndexinput != -1)
					{
						btnCancelInput.Focus();
						throw new Exception("Please Reset the Detail First...");
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						InvFoodProductionPartyProcessingDetail objDetail = new InvFoodProductionPartyProcessingDetail();
						objDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						objDetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
						objDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						objDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
						objDetail.ActionTypeId = 3;
						objDetail.EntryType = Conversion.ToString((object)"Issue");
						objDetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
						objDetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
						objDetail.StockPartyId = Conversion.ToInt(r.Cells["StockPartyId"].Value);
						objDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						objDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						objDetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						objDetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						objDetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						objDetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						objDetail.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
						objDetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
						objDetail.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
						objDetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						objDetail.ActionId = Conversion.ToInt(r.Cells["ActionId"].Value);
						lstRemoveRecordInput.Add(objDetail);
						dtInput.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
				}
				if (!(grdInput.CurrentColumn.Key == "Edit"))
				{
				}
			}
			else if ((!e.Control || e.KeyCode != Keys.Return) && e.Control && e.KeyCode == Keys.Delete)
			{
				if (updateDetailIndexinput != -1)
				{
					btnCancelInput.Focus();
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionPartyProcessingDetail objDetail2 = new InvFoodProductionPartyProcessingDetail();
					objDetail2.Id = Conversion.ToInt(r.Cells["Id"].Value);
					objDetail2.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					objDetail2.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					objDetail2.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					objDetail2.ActionTypeId = 3;
					objDetail2.EntryType = Conversion.ToString((object)"Issue");
					objDetail2.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					objDetail2.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					objDetail2.StockPartyId = Conversion.ToInt(r.Cells["StockPartyId"].Value);
					objDetail2.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					objDetail2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					objDetail2.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					objDetail2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					objDetail2.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					objDetail2.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					objDetail2.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					objDetail2.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					objDetail2.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
					objDetail2.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					objDetail2.ActionId = Conversion.ToInt(r.Cells["ActionId"].Value);
					lstRemoveRecordInput.Add(objDetail2);
					dtInput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputHistoryMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInputHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdInputHistoryMain.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInputHistoryMain.CurrentColumn.Key == "Edit" && r.Cells["EntryType"].Value.ToString() == "InPut")
				{
					RecIdInput = CurrId;
					ReadByIdInput(RecIdInput);
				}
				if (grdInputHistoryMain.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
				}
				if (grdInputHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights && r.Cells["EntryType"].Value.ToString() == "InPut")
				{
					RecIdInput = CurrId;
					ReadByIdInput(RecIdInput);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionPartyProcessing.GenerateCode(new InvFoodProductionPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 118
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
			string Uom = Conversion.ToString((object)((Control)(object)CmbUomOutPut).Text);
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemOutput.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbUomOutPut, "Id", "UOMCode", "UOM", ZeroIndex: true);
				if (!string.IsNullOrEmpty(Uom))
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Uom))
					{
						((Control)(object)CmbUomOutPut).Text = Uom;
					}
					else
					{
						((Control)(object)CmbUomOutPut).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbUomOutPut).Text = string.Empty;
				((UltraGridBase)CmbUomOutPut).DataSource = null;
			}
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
			dtEntryType.Columns.Add("Id");
			dtEntryType.Columns.Add("Name");
			dtEntryType.Rows.Add(1, "ByProduct");
			dtEntryType.Rows.Add(2, "FinishGoods");
			DDL.BindDDL(dtEntryType, CmbEntryType, "Id", "Name", "EntryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemNameOutPut_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemOutput.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemOutput.Value);
				}
				if (rdbtnItemNameOutPut.Checked)
				{
					DDL.BindDDL(dtItem, CmbItemOutput, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, CmbItemOutput, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemOutput.Value = ItemId;
				CmbItemOutput.Focus();
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
			bindUomOutPut();
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
		if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow == null || Conversion.ToInt(CmbJobOrderNoOutput.Value) == 0)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNoOutput.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortionOutPut()
	{
		if (((UltraGridBase)CmbEntryType).ActiveRow == null || Conversion.ToInt(CmbEntryType.Value) == 0)
		{
			MessageBox.Show("EntryType Field Required");
			CmbEntryType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouseOutput).ActiveRow == null || Conversion.ToInt(CmbWarehouseOutput.Value) == 0)
		{
			MessageBox.Show("Ware house Field Required");
			CmbWarehouseOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemOutput).ActiveRow == null || Conversion.ToInt(CmbItemOutput.Value) == 0)
		{
			MessageBox.Show("Item Field Field Required");
			CmbItemOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYearOutPut).ActiveRow == null || Conversion.ToInt(CmbCropYearOutPut.Value) == 0)
		{
			MessageBox.Show("CropYear Field Required");
			CmbCropYearOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLotOutPut).ActiveRow == null || Conversion.ToInt(CmbLotOutPut.Value) == 0)
		{
			MessageBox.Show("Job Lot Field Required");
			CmbLotOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackTypeOutPut).ActiveRow == null || Conversion.ToInt(CmbPackTypeOutPut.Value) == 0)
		{
			MessageBox.Show("Packing Type Field Required");
			CmbPackTypeOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbUomOutPut).ActiveRow == null || Conversion.ToInt(CmbUomOutPut.Value) == 0)
		{
			MessageBox.Show("UOM Field Required");
			CmbUomOutPut.Focus();
			return false;
		}
		if (txtQtyOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQtyOutPut.Focus();
			return false;
		}
		if (txtNetWeightOutPut.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Unit Weight Field Required");
			txtNetWeightOutPut.Focus();
			return false;
		}
		return true;
	}

	private void btnAddOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOutPut())
			{
				dtByProduct.Rows.Add(0, Conversion.ToString((object)((Control)(object)CmbEntryType).Text), Conversion.ToInt(CmbWarehouseOutput.Value), Conversion.ToString((object)((Control)(object)CmbWarehouseOutput).Text), Conversion.ToInt(CmbItemOutput.Value), Conversion.ToString(((UltraDropDownBase)CmbItemOutput).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemOutput).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropYearOutPut.Value), Conversion.ToString((object)((Control)(object)CmbCropYearOutPut).Text), Conversion.ToInt(CmbLotOutPut.Value), Conversion.ToString((object)((Control)(object)CmbLotOutPut).Text), Conversion.ToInt(CmbPackTypeOutPut.Value), Conversion.ToString((object)((Control)(object)CmbPackTypeOutPut).Text), Conversion.ToInt(CmbUomOutPut.Value), Conversion.ToString(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells["UOMCode"].Value), Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQtyOutPut.Text), Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtEbUnit.Text), Conversion.ToDouble((object)txtEbTotal.Text), Conversion.ToDouble((object)txtNetWeightOutPut.Text), Conversion.ToString((object)txtRemarksOutPut.Text));
				grdOutPut.DataSource = dtByProduct;
				grdOutPut.RetrieveStructure();
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
			grdOutPut.RootTable.Columns["Id"].Visible = false;
			grdOutPut.RootTable.Columns["WareHouseId"].Visible = false;
			grdOutPut.RootTable.Columns["ItemId"].Visible = false;
			grdOutPut.RootTable.Columns["CropYearId"].Visible = false;
			grdOutPut.RootTable.Columns["JobLotId"].Visible = false;
			grdOutPut.RootTable.Columns["PackingTypeId"].Visible = false;
			grdOutPut.RootTable.Columns["ItemUOMId"].Visible = false;
			grdOutPut.RootTable.Columns["ItemUOMEquivalent"].Visible = false;
			grdOutPut.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["Quantity"].FormatString = "#,##0.###";
			grdOutPut.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.###";
			grdOutPut.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdOutPut.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdOutPut.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["EbUnit"].FormatString = "#,##0.####";
			grdOutPut.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.####";
			grdOutPut.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["EbTotal"].FormatString = "#,##0.####";
			grdOutPut.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.####";
			grdOutPut.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdOutPut.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdOutPut);
			grdOutPut.RootTable.Columns["EntryType"].ColumnType = (ColumnType)0;
			grdOutPut.RootTable.Columns["EntryType"].HasValueList = true;
			grdOutPut.RootTable.Columns["EntryType"].LimitToList = true;
			grdOutPut.RootTable.Columns["EntryType"].EditType = (EditType)4;
			grdOutPut.RootTable.Columns["EntryType"].Caption = "EntryType";
			grdOutPut.RootTable.Columns["EntryType"].ValueList.PopulateValueList((IEnumerable)dtEntryType.DefaultView, "Id", "Name");
			grdOutPut.RootTable.Columns["EntryType"].Width = 150;
			grdOutPut.RootTable.Columns["WareHouse"].Width = 200;
			grdOutPut.RootTable.Columns["Remarks"].Width = 300;
			grdOutPut.RootTable.Columns["Item"].Width = 300;
			grdOutPut.RootTable.Columns.Add("Delete");
			grdOutPut.RootTable.Columns["Delete"].Key = "Delete";
			grdOutPut.RootTable.Columns["Delete"].Caption = "X";
			grdOutPut.RootTable.Columns["Delete"].ButtonText = "X";
			grdOutPut.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPut.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOutPut.RootTable.Columns["Delete"].Width = 20;
			grdOutPut.RootTable.Columns["Delete"].Position = 0;
			grdOutPut.RootTable.Columns.Add("Edit");
			grdOutPut.RootTable.Columns["Edit"].Key = "Edit";
			grdOutPut.RootTable.Columns["Edit"].Caption = "Edit";
			grdOutPut.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOutPut.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPut.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOutPut.RootTable.Columns["Edit"].Width = 50;
			grdOutPut.RootTable.Columns["Edit"].Position = 1;
			grdOutPut.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
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
			resetDetailOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPut.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailIndexOutPut = r.RowIndex;
				((Control)(object)CmbEntryType).Text = r.Cells["EntryType"].Text.ToString();
				CmbWarehouseOutput.Value = r.Cells["WareHouseId"].Value.ToString();
				((Control)(object)CmbWarehouseOutput).Text = r.Cells["WareHouse"].Text.ToString();
				CmbItemOutput.Value = r.Cells["ItemId"].Value.ToString();
				CmbItemOutput_Leave(null, null);
				((Control)(object)CmbCropYearOutPut).Text = r.Cells["CropYear"].Text.ToString();
				CmbLotOutPut.Value = r.Cells["JobLotId"].Value.ToString();
				((Control)(object)CmbLotOutPut).Text = r.Cells["JobLot"].Text.ToString();
				CmbPackTypeOutPut.Value = r.Cells["PackingTypeId"].Value.ToString();
				((Control)(object)CmbPackTypeOutPut).Text = r.Cells["PackingType"].Text.ToString();
				CmbUomOutPut.Value = r.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbUomOutPut).Text = r.Cells["ItemUOM"].Text.ToString();
				txtQtyOutPut.Text = r.Cells["Quantity"].Text.ToString();
				txtGrossWeight.Text = Conversion.ToDouble((object)r.Cells["GrossWeight"].Text).ToString("#,##0.###");
				txtEbUnit.Text = Conversion.ToDouble((object)r.Cells["EbUnit"].Text).ToString("#,##0.####");
				txtEbTotal.Text = Conversion.ToDouble((object)r.Cells["EbTotal"].Text).ToString("#,##0.####");
				txtNetWeightOutPut.Text = Conversion.ToDouble((object)r.Cells["Weight"].Text).ToString("#,##0.###");
				txtRemarksOutPut.Text = r.Cells["Remarks"].Text.ToString();
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
				dtByProduct.Rows[updateDetailIndexOutPut]["EntryType"] = Conversion.ToString((object)((Control)(object)CmbEntryType).Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["WareHouseId"] = Conversion.ToInt(CmbWarehouseOutput.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["WareHouse"] = Conversion.ToString((object)((Control)(object)CmbWarehouseOutput).Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["ItemId"] = Conversion.ToInt(CmbItemOutput.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemOutput).SelectedRow.Cells["ItemCode"].Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemOutput).SelectedRow.Cells["ItemName"].Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["CropYearId"] = Conversion.ToInt(CmbCropYearOutPut.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYearOutPut).Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["JobLotId"] = Conversion.ToInt(CmbLotOutPut.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbLotOutPut).Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["PackingTypeId"] = Conversion.ToInt(CmbPackTypeOutPut.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackTypeOutPut).Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["ItemUOMId"] = Conversion.ToInt(CmbUomOutPut.Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["ItemUOM"] = Conversion.ToString(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells["UOMCode"].Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["ItemUOMEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells["Equivalent"].Value);
				dtByProduct.Rows[updateDetailIndexOutPut]["Quantity"] = Conversion.ToDouble((object)txtQtyOutPut.Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["EbUnit"] = Conversion.ToDouble((object)txtEbUnit.Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["EbTotal"] = Conversion.ToDouble((object)txtEbTotal.Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["Weight"] = Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim());
				dtByProduct.Rows[updateDetailIndexOutPut]["Remarks"] = Conversion.ToString((object)txtRemarksOutPut.Text.Trim());
				resetDetailOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdOutPut.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndexOutPut != -1)
				{
					btnCancelOutPut.Focus();
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionPartyProcessingDetail obj = new InvFoodProductionPartyProcessingDetail();
					obj.ActionTypeId = 3;
					obj.Id = Conversion.ToInt(r.Cells["Id"].Value);
					obj.EntryType = r.Cells["EntryType"].Text.Trim();
					obj.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					obj.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					obj.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					obj.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					obj.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					obj.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					obj.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					obj.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
					lstRemoveRecordOutput.Add(obj);
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
			}
			if (e.Column.Key == "Edit")
			{
				grdByProduct_DoubleClick(null, null);
			}
			grdOutPut.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutInsert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		try
		{
			if (grdOutPut.RowCount == 0)
			{
				MessageBox.Show("Grd Record Not Found.\nEnter Detail First ...");
			}
			else
			{
				if (!FormValidationOutPut())
				{
					return;
				}
				InvFoodProductionPartyProcessing h = new InvFoodProductionPartyProcessing();
				if (RecIdOutPut > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					h.Id = RecIdOutPut;
				}
				else
				{
					if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					GenerateDocNumberOutPut();
				}
				h.OrganizationId = UserAccount.OrganizationId;
				h.CompanyId = UserAccount.CompanyId;
				h.BranchId = UserAccount.BranchesId;
				h.ProjectId = UserAccount.BranchesId;
				h.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				h.DocumentTypeId = 118;
				h.EntryDate = DateTime.Now;
				h.ModifyDate = DateTime.Now;
				h.EntryUser = UserAccount.ID;
				h.ModifyUser = UserAccount.ID;
				h.ApprovedUserId = UserAccount.ID;
				h.ApprovedDate = DateTime.Now;
				h.ScreenName = "frmProductionPartyProcessing";
				h.DocNo = Conversion.ToInt((object)txtDocNoOutPut.Text);
				h.DocDate = Conversion.ToDateTime((object)DocDateOutPut.Value);
				h.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
				h.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoOutput).Text.Trim());
				h.EntryType = "Output";
				h.MainRemarks = txtRemarksHeaderOutPut.Text.ToString();
				h.PartyProcessingDetailsList = new List<InvFoodProductionPartyProcessingDetail>();
				double GrossWeight = 0.0;
				if (lstRemoveRecordOutput.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecordOutput.Count; i++)
					{
						h.PartyProcessingDetailsList.Add(lstRemoveRecordOutput[i]);
					}
				}
				GridEXRow[] rows = grdOutPut.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionPartyProcessingDetail d = new InvFoodProductionPartyProcessingDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (d.Id > 0)
					{
						d.ActionTypeId = 2;
					}
					else
					{
						d.ActionTypeId = 1;
					}
					if (string.IsNullOrEmpty(Conversion.ToString((object)r.Cells["EntryType"].Text.Trim())))
					{
						MessageBox.Show("EntryType Field Require in Output Detail");
						return;
					}
					if (r.Cells["WareHouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
					{
						MessageBox.Show("WareHouse Field Require in Output Detail");
						return;
					}
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Field Require in Output Detail");
						return;
					}
					if (r.Cells["CropYearId"].Text == string.Empty || Conversion.ToInt((object)r.Cells["CropYearId"].Text) == 0)
					{
						MessageBox.Show("CropYear Field Required in Output Detail");
						return;
					}
					if (r.Cells["JobLotId"].Text == string.Empty || Conversion.ToInt((object)r.Cells["JobLotId"].Text) == 0)
					{
						MessageBox.Show("JobLot Field Require in Output Detail");
						return;
					}
					if (r.Cells["PackingTypeId"].Text == string.Empty || Conversion.ToInt((object)r.Cells["PackingTypeId"].Text) == 0)
					{
						MessageBox.Show("PackingType Field Required in Output Detail");
						return;
					}
					if (r.Cells["ItemUOMId"].Text == string.Empty || Conversion.ToInt((object)r.Cells["ItemUOMId"].Text) == 0)
					{
						MessageBox.Show("ItemUOM Field Required in Output Detail");
						return;
					}
					if (r.Cells["Quantity"].Text == string.Empty || Conversion.ToDouble((object)r.Cells["Quantity"].Text) == 0.0)
					{
						MessageBox.Show("Quantity Field Required in Output Detail");
						return;
					}
					if (r.Cells["Weight"].Text == string.Empty || Conversion.ToDouble((object)r.Cells["Weight"].Text) == 0.0)
					{
						MessageBox.Show("Weight Field Required in Output Detail");
						return;
					}
					d.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
					d.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
					d.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					d.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					d.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					d.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					d.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
					d.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					d.EbUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
					d.EbTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
					d.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					GrossWeight += d.Weight;
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					d.StockPartyId = 0;
					d.ActionId = 1;
					h.PartyProcessingDetailsList.Add(d);
				}
				if (h.PartyProcessingDetailsList.Count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}
				int success = InvFoodProductionPartyProcessing.Save(h);
				if (RecIdOutPut > 0)
				{
					MessageBox.Show("Record Update Successfully" + h.DocNo);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + h.DocNo);
				}
				if (WagesStatus && WagesActiveOrInActiveForOutPut)
				{
					frmWagesBillPartyProcessing obj = new frmWagesBillPartyProcessing(UserAccount);
					obj.RefDocTypeId = 118;
					obj.RefDocId = success;
					obj.GrossWeightTotal = GrossWeight;
					obj.ShowDialog();
					obj.BringToFront();
				}
				resetOutPut();
				resetDetailOutPut();
				if (ChkPrintPreviewOutPut.Checked)
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (RecIdOutPut == 0)
			{
				throw new Exception("Rec Id not found");
			}
			OutPutInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdOutPut(int ID)
	{
		try
		{
			InvFoodProductionPartyProcessing SC = InvFoodProductionPartyProcessing.GetByID(ID);
			if (SC != null && SC.EntryType != "Input")
			{
				btnSaveOutPut.Visible = false;
				btnUpdateOutPut.Visible = true;
				BtnDeleteOutput.Visible = true;
				tabControlMain.SelectedIndex = 1;
				tabControlOutPut.SelectedIndex = 0;
				txtDocNoOutPut.Text = Conversion.ToString((object)SC.DocNo);
				DocDateOutPut.Value = Conversion.ToDateTime((object)SC.DocDate);
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("JobOrderNo");
				dt.Rows.Add(SC.InvJobOrderId, SC.InvJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNoOutput).Rows[0].Activate();
				txtRemarksHeaderOutPut.Text = Conversion.ToString((object)SC.MainRemarks);
				CmbJobOrderNoOutput_Leave(null, null);
				dtByProduct.Clear();
				for (int i = 0; i < SC.PartyProcessingDetailsList.Count; i++)
				{
					if (SC.PartyProcessingDetailsList[i].EntryType != "Issue")
					{
						dtByProduct.Rows.Add(SC.PartyProcessingDetailsList[i].Id, SC.PartyProcessingDetailsList[i].EntryType, SC.PartyProcessingDetailsList[i].WarehouseId, SC.PartyProcessingDetailsList[i].WareHouseName, SC.PartyProcessingDetailsList[i].ItemId, SC.PartyProcessingDetailsList[i].ItemCode, SC.PartyProcessingDetailsList[i].ItemName, SC.PartyProcessingDetailsList[i].CropYearId, SC.PartyProcessingDetailsList[i].CropBatch, SC.PartyProcessingDetailsList[i].JobLotId, SC.PartyProcessingDetailsList[i].JobLotDescription, SC.PartyProcessingDetailsList[i].PackingtypeId, SC.PartyProcessingDetailsList[i].PackTypeDesc, SC.PartyProcessingDetailsList[i].ItemUomId, SC.PartyProcessingDetailsList[i].PackUom, SC.PartyProcessingDetailsList[i].Equivalent, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].GrossWeight, SC.PartyProcessingDetailsList[i].EbUnit, SC.PartyProcessingDetailsList[i].EbTotal, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].Remarks);
					}
				}
				grdOutPut.DataSource = dtByProduct;
				grdOutPut.RetrieveStructure();
				gridsettingsOutPut();
				return;
			}
			throw new Exception("Record Not Found...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDeleteOutput_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecIdOutPut > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					new InvFoodProductionPartyProcessing().DeleteById(RecIdOutPut, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					resetOutPut();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
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

	private void btnNewOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			resetOutPut();
			resetDetailOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			JobOrderNoFill();
			Warehouse();
			ItemFill();
			CropYear();
			combojoblotfill();
			PackingTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetOutPut()
	{
		try
		{
			RecIdOutPut = 0;
			updateDetailIndexOutPut = -1;
			lstRemoveRecordOutput.Clear();
			btnSaveOutPut.Visible = true;
			btnUpdateOutPut.Visible = false;
			BtnDeleteOutput.Visible = false;
			((Control)(object)CmbJobOrderNoOutput).Enabled = true;
			dtByProduct.Clear();
			grdOutPut.ClearStructure();
			GenerateDocNumberOutPut();
			((UltraGridBase)CmbJobOrderNoOutput).DataSource = null;
			((Control)(object)CmbJobOrderNoOutput).Text = string.Empty;
			JobOrderNoFill();
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
			updateDetailIndexOutPut = -1;
			((Control)(object)CmbEntryType).Text = string.Empty;
			((Control)(object)CmbWarehouseOutput).Text = string.Empty;
			((Control)(object)CmbItemOutput).Text = string.Empty;
			((Control)(object)CmbCropYearOutPut).Text = string.Empty;
			((Control)(object)CmbLotOutPut).Text = string.Empty;
			((Control)(object)CmbPackTypeOutPut).Text = string.Empty;
			((Control)(object)CmbUomOutPut).Text = string.Empty;
			txtQtyOutPut.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEbUnit.Text = string.Empty;
			txtEbTotal.Text = string.Empty;
			txtNetWeightOutPut.Text = string.Empty;
			txtRemarksOutPut.Text = string.Empty;
			btnAddOutPut.Visible = true;
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
			CmbEntryType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNoOutput_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProductionPartyProcessing.GetInPutTotalQtyandWeightByJobOrderId(new InvFoodProductionPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Input")
					{
						txtInPutTotalQty.Text = Conversion.ToString((object)dt.Rows[i]["ItemQty"].ToString());
						txtInPutTotalWeight.Text = Conversion.ToString((object)dt.Rows[i]["StockWeight"].ToString());
					}
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Output")
					{
						txtOutPutTotalQty.Text = Conversion.ToString(dt.Rows[i]["ItemQty"]);
						txtOutPutTotalWeight.Text = Conversion.ToString(dt.Rows[i]["StockWeight"]);
					}
				}
			}
			else
			{
				txtInPutTotalQty.Text = "0";
				txtInPutTotalWeight.Text = "0";
				txtOutPutTotalQty.Text = "0";
				txtOutPutTotalWeight.Text = "0";
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
			double Qty = Conversion.ToDouble((object)txtQtyOutPut.Text.Trim());
			double UOM = ((((UltraGridBase)CmbUomOutPut).ActiveRow != null && Conversion.ToInt(CmbUomOutPut.Value) > 0) ? Conversion.ToDouble(((UltraDropDownBase)CmbUomOutPut).SelectedRow.Cells["Equivalent"].Value) : 0.0);
			double GrossWeight = 0.0;
			if (btnSaveOutPut.Enabled && btnSaveOutPut.Visible)
			{
				txtGrossWeight.Text = (Qty * UOM).ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNetWeightOutPut()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtQtyOutPut.Text.Trim());
			double num = Conversion.ToDouble((object)txtGrossWeight.Text);
			double EbUnit = Conversion.ToDouble((object)txtEbUnit.Text);
			double EbTotal = Conversion.ToDouble((object)txtEbTotal.Text);
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
			double NetWeight = 0.0;
			NetWeight = num - EbTotal;
			txtNetWeightOutPut.Text = NetWeight.ToString("#,##.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUomOutPut_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyOutPut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutHistoryComboFill()
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
				DocumentTypeIds = "118"
			};
			DataTable dtjob = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"ReferenceName",
						typeof(string)
					}
				}
			};
			DataTable dt = InvFoodProductionPartyProcessing.GetDataForDropDownFromInvFoodProductionPartyProcessing(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvJobOrder")
					{
						dtjob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDL(dtjob, CmbJobOrderHistoryOutPut, "Id", "ReferenceName", "JobOrder No", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbJobOrderHistoryOutPut).DataSource = null;
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
			if (tabControlOutPut.SelectedIndex == 1)
			{
				FromDateHistoryOutPut.Focus();
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

	private void BtnNewHistoryOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryOutPut.Value = DateTime.Now;
			ToDateHistoryOutPut.Value = DateTime.Now;
			FromNoHistoryOutPut.Text = string.Empty;
			ToNoHistoryOutPut.Text = string.Empty;
			((Control)(object)CmbJobOrderHistoryOutPut).Text = string.Empty;
			grdOutputHistoryMain.ClearStructure();
			RadDocDateHistoryOutPut.Checked = true;
			FromDateHistoryOutPut.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistoryOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			OutPutHistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tnShowHistoryOutPut_Click(object sender, EventArgs e)
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

	private void OutputGridHistory()
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
			obj.DocumentTypeId = 118;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (RadDocDateHistoryOutPut.Checked)
			{
				if (FromDateHistoryOutPut.Checked)
				{
					obj.FromDate = FromDateHistoryOutPut.Value;
				}
				if (ToDateHistoryOutPut.Checked)
				{
					obj.ToDate = ToDateHistoryOutPut.Value;
				}
			}
			else if (RadEntryDateHistoryOutPut.Checked)
			{
				if (FromDateHistoryOutPut.Checked)
				{
					obj.EntryDateFrom = FromDateHistoryOutPut.Value;
				}
				if (ToDateHistoryOutPut.Checked)
				{
					obj.EntryDateTo = ToDateHistoryOutPut.Value;
				}
			}
			else if (RadModifyDateHistoryOutPut.Checked)
			{
				if (FromDateHistoryOutPut.Checked)
				{
					obj.ModifyDateFrom = FromDateHistoryOutPut.Value;
				}
				if (ToDateHistoryOutPut.Checked)
				{
					obj.ModifyDateTo = ToDateHistoryOutPut.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromNoHistoryOutPut.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDateHistoryOutPut.Text);
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderHistoryOutPut.Value);
			DataTable dthis = InvFoodProductionPartyProcessing.FormHistory(obj);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocDate", typeof(DateTime));
				dtTarget.Columns.Add("DocNo", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("InvJobOrderId", typeof(int));
				dtTarget.Columns.Add("InvJobOrderNo", typeof(string));
				dtTarget.Columns.Add("EntryType", typeof(string));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUserName", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUserName", typeof(string));
				foreach (DataRow row in dthis.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocDate"], row["DocNo"], row["DocumentTypeId"], row["InvJobOrderId"], row["InvJobOrderNo"], row["EntryType"], row["MainRemarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
				}
				grdOutputHistoryMain.DataSource = dtTarget;
				grdOutputHistoryMain.RetrieveStructure();
				GridOutputSetting();
			}
			else
			{
				grdOutputHistoryMain.ClearStructure();
			}
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
			grdOutputHistoryMain.RootTable.Columns["Id"].Visible = false;
			grdOutputHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdOutputHistoryMain.RootTable.Columns["InvJobOrderId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdOutputHistoryMain);
			grdOutputHistoryMain.RootTable.Columns["Remarks"].Width = 250;
			grdOutputHistoryMain.RootTable.Columns.Add("Print");
			grdOutputHistoryMain.RootTable.Columns["Print"].Key = "Print";
			grdOutputHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			grdOutputHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdOutputHistoryMain.RootTable.Columns["Print"].Width = 50;
			grdOutputHistoryMain.RootTable.Columns["Print"].Position = 0;
			grdOutputHistoryMain.RootTable.Columns.Add("Edit");
			grdOutputHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
			grdOutputHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			grdOutputHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOutputHistoryMain.RootTable.Columns["Edit"].Width = 40;
			grdOutputHistoryMain.RootTable.Columns["Edit"].Position = 1;
			grdOutputHistoryMain.FrozenColumns = 2;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutputHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit" && r.Cells["EntryType"].Value.ToString() == "Output")
				{
					RecIdOutPut = CurrId;
					ReadByIdOutPut(RecIdOutPut);
				}
				if (e.Column.Key == "Print")
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistoryMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutputHistoryMain.CurrentRow;
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

	private void grdOutputHistoryMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutputHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				OutputHistoryDetail(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutputHistoryDetail(int CurrId)
	{
		try
		{
			InvFoodProductionPartyProcessing SC = InvFoodProductionPartyProcessing.GetByID(CurrId);
			if (SC == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("EntryType");
			dt.Columns.Add("WareHouse");
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemCode");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("ItemUOM");
			dt.Columns.Add("Quantity", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("Remarks");
			dt.Rows.Clear();
			for (int i = 0; i < SC.PartyProcessingDetailsList.Count; i++)
			{
				if (SC.PartyProcessingDetailsList[i].EntryType != "Issue")
				{
					dt.Rows.Add(SC.PartyProcessingDetailsList[i].Id, SC.PartyProcessingDetailsList[i].EntryType, SC.PartyProcessingDetailsList[i].WareHouseName, SC.PartyProcessingDetailsList[i].ItemId, SC.PartyProcessingDetailsList[i].ItemCode, SC.PartyProcessingDetailsList[i].ItemName, SC.PartyProcessingDetailsList[i].CropBatch, SC.PartyProcessingDetailsList[i].JobLotDescription, SC.PartyProcessingDetailsList[i].PackTypeDesc, SC.PartyProcessingDetailsList[i].PackUom, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].Remarks);
				}
			}
			grdOutputHistoryDetail.DataSource = dt;
			grdOutputHistoryDetail.RetrieveStructure();
			grdOutputHistoryDetailsettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistoryDetailsettings()
	{
		try
		{
			grdOutputHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdOutputHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdOutputHistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdOutputHistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdOutputHistoryDetail.RootTable.Columns["Quantity"].FormatString = "#,##.###";
			grdOutputHistoryDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##.###";
			grdOutputHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutputHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutputHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##.###";
			grdOutputHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(grdOutputHistoryDetail);
			grdOutputHistoryDetail.RootTable.Columns["WareHouse"].Width = 200;
			grdOutputHistoryDetail.RootTable.Columns["ItemName"].Width = 300;
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
			CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(RecIdOutPut);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyOutPut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetWeightOutPut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromNoHistoryOutPut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToNoHistoryOutPut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutPut).Name);
			if (fs != null)
			{
				grdOutPut.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutputHistoryMain).Name);
			if (fs != null)
			{
				grdOutputHistoryMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdOutPut_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdOutPut.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPut.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)grdOutPut.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOutPut.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndexOutPut != -1)
					{
						btnCancelOutPut.Focus();
						throw new Exception("Please Reset the Detail First...");
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						InvFoodProductionPartyProcessingDetail obj = new InvFoodProductionPartyProcessingDetail();
						obj.ActionTypeId = 3;
						obj.Id = Conversion.ToInt(r.Cells["Id"].Value);
						obj.EntryType = r.Cells["EntryType"].Text.Trim();
						obj.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						obj.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						obj.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						obj.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						obj.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						obj.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
						obj.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
						obj.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
						lstRemoveRecordOutput.Add(obj);
						dtByProduct.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
				}
				if (grdOutPut.CurrentColumn.Key == "Edit")
				{
					grdByProduct_DoubleClick(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdByProduct_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (updateDetailIndexOutPut != -1)
				{
					btnCancelOutPut.Focus();
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionPartyProcessingDetail obj2 = new InvFoodProductionPartyProcessingDetail();
					obj2.ActionTypeId = 3;
					obj2.Id = Conversion.ToInt(r.Cells["Id"].Value);
					obj2.EntryType = r.Cells["EntryType"].Text.Trim();
					obj2.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					obj2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					obj2.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					obj2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					obj2.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					obj2.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					obj2.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					obj2.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					obj2.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
					lstRemoveRecordOutput.Add(obj2);
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistoryMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutputHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutputHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdOutputHistoryMain.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOutputHistoryMain.CurrentColumn.Key == "Edit" && r.Cells["EntryType"].Value.ToString() == "Output")
				{
					((Control)(object)CmbJobOrderNoOutput).Enabled = false;
					RecIdOutPut = CurrId;
					ReadByIdOutPut(RecIdOutPut);
					CmbJobOrderNoOutput_Leave(null, null);
				}
				if (grdOutputHistoryMain.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
				}
				if (grdOutputHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights && r.Cells["EntryType"].Value.ToString() == "Output")
				{
					((Control)(object)CmbJobOrderNoOutput).Enabled = false;
					RecIdOutPut = CurrId;
					ReadByIdOutPut(RecIdOutPut);
					CmbJobOrderNoOutput_Leave(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCodeOfPM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionPartyProcessingPackingMaterial.GenerateCode(new InvFoodProductionPartyProcessingPackingMaterial
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 119,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoPM.Text = code.ToString();
			}
			else
			{
				txtDocNoPM.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPMFill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbItemPM.Value);
			DataTable dt = Item.ReadAllForItemsForPartyProcessingPM(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				dtItemPM.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtItemPM.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtItemPM.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemPM, cmbItemPM, "Id", "ItemName", "ItemName", ZeroIndex: true);
				if (ID > 0)
				{
					if (dtItemPM.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbItemPM.Value = ID;
					}
					else
					{
						((Control)(object)cmbItemPM).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItemPM).Text = string.Empty;
				((UltraGridBase)cmbItemPM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemNamePM_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtItemPM.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbItemPM.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbItemPM.Value);
				}
				if (rdbtnItemNamePM.Checked)
				{
					DDL.BindDDL(dtItemPM, cmbItemPM, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItemPM, cmbItemPM, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbItemPM.Value = ItemId;
				cmbItemPM.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHousePM()
	{
		try
		{
			int ID = Conversion.ToInt(cmbWarehouse.Value);
			DataTable dt = CommonServices.GetActiveWareHouseByWareHouseType(2);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbWarehouse, "Id", "WareHouseName", "Warehouse", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbWarehouse.Value = ID;
					}
					else
					{
						((Control)(object)cmbWarehouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbWarehouse).Text = string.Empty;
				((UltraGridBase)cmbWarehouse).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbJobOrderPM_ValueChanged(object sender, EventArgs e)
	{
	}

	private bool FormValidationOfPM()
	{
		if (((UltraGridBase)cmbJobOrderPM).ActiveRow == null || Conversion.ToInt(cmbJobOrderPM.Value) == 0)
		{
			MessageBox.Show("Product No. Field Required");
			cmbJobOrderPM.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemPM).ActiveRow == null || Conversion.ToInt(cmbItemPM.Value) == 0)
		{
			MessageBox.Show(" Item Field Required");
			cmbItemPM.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWarehouse).ActiveRow == null || Conversion.ToInt(cmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse  Field Required");
			cmbWarehouse.Focus();
			return false;
		}
		if (txtPMQTY.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPMQTY.Text) == 0.0)
		{
			MessageBox.Show(" Qty Field Required");
			txtPMQTY.Focus();
			return false;
		}
		return true;
	}

	private void PMInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidationOfPM())
			{
				return;
			}
			InvFoodProductionPartyProcessingPackingMaterial h = new InvFoodProductionPartyProcessingPackingMaterial();
			if (RecIdPM > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				h.Id = RecIdPM;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateCodeOfPM();
			}
			h.CompanyId = UserAccount.CompanyId;
			h.OrganizationId = UserAccount.OrganizationId;
			h.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			h.DocumentTypeId = 119;
			h.EntryDate = DateTime.Now;
			h.EntryUser = UserAccount.ID;
			h.ModifyDate = DateTime.Now;
			h.ModifyUser = UserAccount.ID;
			h.ApprovedDate = DateTime.Now;
			h.ApprovedUserId = UserAccount.ID;
			h.IsApproved = false;
			h.DocDate = txtdocdatePM.Value;
			h.DocNo = Conversion.ToInt((object)txtDocNoPM.Text);
			h.InvJobOrderId = Conversion.ToInt(cmbJobOrderPM.Value);
			h.WarehouseId = Conversion.ToInt(cmbWarehouse.Value);
			h.ItemId = Conversion.ToInt(cmbItemPM.Value);
			h.Qty = Conversion.ToDouble((object)txtPMQTY.Text);
			h.Remarks = Conversion.ToString((object)txtremarksPM.Text);
			h.ScreenName = base.Name;
			InvFoodProductionPartyProcessingPackingMaterial.Save(h);
			if (RecIdPM > 0)
			{
				MessageBox.Show("Packing Material  Update Successfully");
			}
			else
			{
				MessageBox.Show("Packing Material Save Successfully");
			}
			FormResetOfPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsavePM_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdPM = 0;
			PMInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatePM_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecIdPM == 0)
			{
				throw new Exception("Rec Id not found");
			}
			PMInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdPM(int ID)
	{
		try
		{
			RecIdPM = ID;
			InvFoodProductionPartyProcessingPackingMaterial SC = InvFoodProductionPartyProcessingPackingMaterial.GetByID(RecIdPM);
			if (SC != null)
			{
				btnSavePM.Visible = false;
				btnUpdatePM.Visible = true;
				BtnDeletePM.Visible = true;
				tabControlMain.SelectedIndex = 2;
				txtdocdatePM.Value = SC.DocDate;
				txtDocNoPM.Text = SC.DocNo.ToString();
				cmbJobOrderPM.Value = SC.InvJobOrderId.ToString();
				cmbWarehouse.Value = SC.WarehouseId;
				cmbItemPM.Value = SC.ItemId;
				txtPMQTY.Text = SC.Qty.ToString();
				txtremarksPM.Text = Conversion.ToString((object)SC.Remarks);
				cmbWarehouse.Focus();
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDeletePM_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecIdPM > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					new InvFoodProductionPartyProcessingPackingMaterial().DeleteById(RecIdPM, UserAccount.ID);
					MessageBox.Show("Delete Record Seccessfully");
					FormResetOfPM();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormResetOfPM()
	{
		try
		{
			RecIdPM = 0;
			btnSavePM.Visible = true;
			btnUpdatePM.Visible = false;
			BtnDeletePM.Visible = false;
			UpdateMode = false;
			GenerateCodeOfPM();
			((Control)(object)cmbItemPM).Text = string.Empty;
			((Control)(object)cmbWarehouse).Text = string.Empty;
			cmbJobOrderPM.Focus();
			txtPMQTY.Text = string.Empty;
			txtremarksPM.Text = string.Empty;
			cmbJobOrderPM.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewPM_Click(object sender, EventArgs e)
	{
		try
		{
			FormResetOfPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshPM_Click(object sender, EventArgs e)
	{
		try
		{
			WareHousePM();
			ItemPMFill();
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPMRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CaluculatonofPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CaluculatonofPM()
	{
	}

	private void txtPMQTY_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CaluculatonofPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PMHistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtjob = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"ReferenceName",
						typeof(string)
					}
				}
			};
			DataTable dt = InvFoodProductionPartyProcessingPackingMaterial.GetDataForDropDownFromProductionPMPartyProcessing(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvJobOrder")
					{
						dtjob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDL(dtjob, CmbJobOrderPMHistory, "Id", "ReferenceName", "JobOrder No", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbJobOrderPMHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewPMHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDatePMHistory.Value = DateTime.Now;
			ToDatePMHistory.Value = DateTime.Now;
			FromDocPMHistory.Text = string.Empty;
			ToDocPMHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderPMHistory).Text = string.Empty;
			grdPM.ClearStructure();
			RadDocDatePMHistory.Checked = true;
			FromDatePMHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefereshPMHistory_Click(object sender, EventArgs e)
	{
		try
		{
			PMHistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowPMHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 119;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (RadDocDatePMHistory.Checked)
			{
				if (FromDatePMHistory.Checked)
				{
					obj.FromDate = FromDatePMHistory.Value;
				}
				if (ToDatePMHistory.Checked)
				{
					obj.ToDate = ToDatePMHistory.Value;
				}
			}
			else if (RadEntrydatePMHistory.Checked)
			{
				if (FromDatePMHistory.Checked)
				{
					obj.EntryDateFrom = FromDatePMHistory.Value;
				}
				if (ToDatePMHistory.Checked)
				{
					obj.EntryDateTo = ToDatePMHistory.Value;
				}
			}
			else if (RadModifyDatePMHistory.Checked)
			{
				if (FromDatePMHistory.Checked)
				{
					obj.ModifyDateFrom = FromDatePMHistory.Value;
				}
				if (ToDatePMHistory.Checked)
				{
					obj.ModifyDateTo = ToDatePMHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocPMHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDatePMHistory.Text);
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderPMHistory.Value);
			DataTable dtpm = InvFoodProductionPartyProcessingPackingMaterial.FormHistory(obj);
			if (dtpm.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("DocDate", typeof(DateTime));
				dtTarget.Columns.Add("DocNo", typeof(int));
				dtTarget.Columns.Add("InvJobOrderId", typeof(int));
				dtTarget.Columns.Add("JobOrderNo", typeof(string));
				dtTarget.Columns.Add("WarehouseId", typeof(int));
				dtTarget.Columns.Add("WareHouseName", typeof(string));
				dtTarget.Columns.Add("ItemId", typeof(int));
				dtTarget.Columns.Add("ItemName", typeof(string));
				dtTarget.Columns.Add("ItemCode", typeof(string));
				dtTarget.Columns.Add("Qty", typeof(decimal));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUserName", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUserName", typeof(string));
				foreach (DataRow row in dtpm.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocDate"], row["DocNo"], row["InvJobOrderId"], row["JobOrderNo"], row["WarehouseId"], row["WareHouseName"], row["ItemId"], row["ItemName"], row["ItemCode"], row["Qty"], row["Remarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
				}
				grdPM.DataSource = dtTarget;
				grdPM.RetrieveStructure();
				GridSettingOfPM();
			}
			else
			{
				grdPM.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingOfPM()
	{
		try
		{
			grdPM.RootTable.Columns["Id"].Visible = false;
			grdPM.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPM.RootTable.Columns["InvJobOrderId"].Visible = false;
			grdPM.RootTable.Columns["WarehouseId"].Visible = false;
			grdPM.RootTable.Columns["ItemId"].Visible = false;
			grdPM.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["Qty"].FormatString = "#,##.###";
			grdPM.RootTable.Columns["Qty"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(grdPM);
			grdPM.RootTable.Columns["Remarks"].Width = 250;
			grdPM.RootTable.Columns.Add("Print");
			grdPM.RootTable.Columns["Print"].Key = "Print";
			grdPM.RootTable.Columns["Print"].Caption = "Print";
			grdPM.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Print"].ButtonText = "Print";
			grdPM.RootTable.Columns["Print"].Width = 50;
			grdPM.RootTable.Columns["Print"].Position = 0;
			grdPM.RootTable.Columns.Add("Edit");
			grdPM.RootTable.Columns["Edit"].Key = "Edit";
			grdPM.RootTable.Columns["Edit"].Caption = "Edit";
			grdPM.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdPM.RootTable.Columns["Edit"].Width = 40;
			grdPM.RootTable.Columns["Edit"].Position = 1;
			grdPM.FrozenColumns = 2;
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPM_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				RecIdPM = CurrId;
				ReadByIdPM(RecIdPM);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPM_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					RecIdPM = CurrId;
					ReadByIdPM(RecIdPM);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReportPM(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintPM_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReportPM(RecIdPM);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportPM(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionPartyProcessingPackingMaterial.FoodProductionPartyProcessingPackingMaterial_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = RecId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "601_02-InvFoodPackingMaterialSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPM).Name);
			if (fs != null)
			{
				grdPM.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void FromDocPMHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocPMHistory_KeyPress(object sender, KeyPressEventArgs e)
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
			int Id = Conversion.ToInt((object)grdPM.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPM.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RecIdPM = Id;
					ReadByIdPM(RecIdPM);
				}
				if (grdPM.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					GenerateReportPM(Id);
				}
				if (!(grdPM.CurrentColumn.Key == "NoOfAttachments"))
				{
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				RecIdPM = Id;
				ReadByIdPM(RecIdPM);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFillForSummery()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbJobOrderTransactionHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtjob = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"ReferenceName",
						typeof(string)
					}
				}
			};
			DataTable dt = InvFoodProductionPartyProcessing.GetDataForDropDownFromInvFoodProductionPartyProcessing(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvJobOrder")
					{
						dtjob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDL(dtjob, CmbJobOrderTransactionHistory, "Id", "ReferenceName", "JobOrderNo", ZeroIndex: true);
				if (ID > 0)
				{
					if (dtjob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbJobOrderTransactionHistory.Value = ID;
					}
					else
					{
						((Control)(object)CmbJobOrderTransactionHistory).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobOrderTransactionHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderTransactionHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewTransactionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateTransactionHistory.Value = DateTime.Now;
			ToDateTransactionHistory.Value = DateTime.Now;
			FromNoTransactionHistory.Text = string.Empty;
			ToNoTransactionHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderTransactionHistory).Text = string.Empty;
			grdTransactionHistoryMain.ClearStructure();
			grdTransactionhistoryDetail.ClearStructure();
			RadDocDateTransactionHistory.Checked = true;
			FromDateTransactionHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshTransactionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			JobOrderNoFillForSummery();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowTransactionHistory_Click(object sender, EventArgs e)
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
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (RadDocDateTransactionHistory.Checked)
			{
				if (FromDateTransactionHistory.Checked)
				{
					obj.FromDate = FromDateTransactionHistory.Value;
				}
				if (ToDateTransactionHistory.Checked)
				{
					obj.ToDate = ToDateTransactionHistory.Value;
				}
			}
			else if (RadEntryDateTransactionHistory.Checked)
			{
				if (FromDateTransactionHistory.Checked)
				{
					obj.EntryDateFrom = FromDateTransactionHistory.Value;
				}
				if (ToDateTransactionHistory.Checked)
				{
					obj.EntryDateTo = ToDateTransactionHistory.Value;
				}
			}
			else if (RadModifyDateTransactionHistory.Checked)
			{
				if (FromDateTransactionHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateTransactionHistory.Value;
				}
				if (ToDateTransactionHistory.Checked)
				{
					obj.ModifyDateTo = ToDateTransactionHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromNoTransactionHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToNoTransactionHistory.Text);
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderTransactionHistory.Value);
			DataTable dthis = InvFoodProductionPartyProcessing.FormHistory(obj);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocDate", typeof(DateTime));
				dtTarget.Columns.Add("DocNo", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("InvJobOrderId", typeof(int));
				dtTarget.Columns.Add("InvJobOrderNo", typeof(string));
				dtTarget.Columns.Add("EntryType", typeof(string));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUserName", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUserName", typeof(string));
				foreach (DataRow row in dthis.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocDate"], row["DocNo"], row["DocumentTypeId"], row["InvJobOrderId"], row["InvJobOrderNo"], row["EntryType"], row["MainRemarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
				}
				grdTransactionHistoryMain.DataSource = dtTarget;
				grdTransactionHistoryMain.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdTransactionHistoryMain.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdTransactionHistoryMain.RootTable.Columns["Id"].Visible = false;
			grdTransactionHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdTransactionHistoryMain.RootTable.Columns["InvJobOrderId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdTransactionHistoryMain);
			grdTransactionHistoryMain.RootTable.Columns["Remarks"].Width = 250;
			grdTransactionHistoryMain.RootTable.Columns.Add("Print");
			grdTransactionHistoryMain.RootTable.Columns["Print"].Key = "Print";
			grdTransactionHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			grdTransactionHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdTransactionHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdTransactionHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdTransactionHistoryMain.RootTable.Columns["Print"].Width = 50;
			grdTransactionHistoryMain.RootTable.Columns["Print"].Position = 0;
			grdTransactionHistoryMain.RootTable.Columns.Add("Edit");
			grdTransactionHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
			grdTransactionHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			grdTransactionHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdTransactionHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdTransactionHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdTransactionHistoryMain.RootTable.Columns["Edit"].Width = 40;
			grdTransactionHistoryMain.RootTable.Columns["Edit"].Position = 1;
			grdTransactionHistoryMain.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdTransactionHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				if (r.Cells["EntryType"].Value.ToString() == "Input")
				{
					RecIdInput = CurrId;
					ReadByIdInput(RecIdInput);
				}
				else
				{
					RecIdOutPut = CurrId;
					ReadByIdOutPut(RecIdOutPut);
				}
			}
			if (e.Column.Key == "Print")
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdTransactionHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				DatagridHistoryDetail(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetail(int Id)
	{
		try
		{
			InvFoodProductionPartyProcessing SC = InvFoodProductionPartyProcessing.GetByID(Id);
			if (SC != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("StockPartyId", typeof(int));
				dt.Columns.Add("StockParty");
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("Item");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("ItemUOM");
				dt.Columns.Add("Quantity", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("ReferencePartyId", typeof(int));
				dt.Columns.Add("ReferenceParty");
				dt.Columns.Add("Remarks");
				dt.Rows.Clear();
				for (int i = 0; i < SC.PartyProcessingDetailsList.Count; i++)
				{
					dt.Rows.Add(SC.PartyProcessingDetailsList[i].Id, SC.PartyProcessingDetailsList[i].StockPartyId, SC.PartyProcessingDetailsList[i].StockParty, SC.PartyProcessingDetailsList[i].WareHouseName, SC.PartyProcessingDetailsList[i].ItemId, SC.PartyProcessingDetailsList[i].ItemCode, SC.PartyProcessingDetailsList[i].ItemName, SC.PartyProcessingDetailsList[i].CropBatch, SC.PartyProcessingDetailsList[i].JobLotDescription, SC.PartyProcessingDetailsList[i].PackTypeDesc, SC.PartyProcessingDetailsList[i].PackUom, SC.PartyProcessingDetailsList[i].Qty, SC.PartyProcessingDetailsList[i].Weight, SC.PartyProcessingDetailsList[i].ReferencePartyId, SC.PartyProcessingDetailsList[i].ReferencePartyName, SC.PartyProcessingDetailsList[i].Remarks);
				}
				grdTransactionhistoryDetail.DataSource = dt;
				grdTransactionhistoryDetail.RetrieveStructure();
				DetailHistoryGridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailHistoryGridSetting()
	{
		try
		{
			grdTransactionhistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdTransactionhistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdTransactionhistoryDetail.RootTable.Columns["StockPartyId"].Visible = false;
			grdTransactionhistoryDetail.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdTransactionhistoryDetail.RootTable.Columns["StockParty"].Visible = false;
			grdTransactionhistoryDetail.RootTable.Columns["ReferenceParty"].Visible = false;
			if (Conversion.ToString(grdTransactionHistoryMain.CurrentRow.Cells["EntryType"].Value) == "Input")
			{
				grdTransactionhistoryDetail.RootTable.Columns["StockParty"].Visible = true;
				grdTransactionhistoryDetail.RootTable.Columns["ReferenceParty"].Visible = true;
			}
			grdTransactionhistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdTransactionhistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdTransactionhistoryDetail.RootTable.Columns["Quantity"].FormatString = "#,##.###";
			grdTransactionhistoryDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##.###";
			grdTransactionhistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdTransactionhistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdTransactionhistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##.###";
			grdTransactionhistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(grdTransactionhistoryDetail);
			grdTransactionhistoryDetail.RootTable.Columns["WareHouse"].Width = 200;
			grdTransactionhistoryDetail.RootTable.Columns["Item"].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdTransactionHistoryMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnSummeryReport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtSummerReport = ProductionReports.InvFoodProductionPartyProcessingRecoverySummeryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderTransactionHistory.Value),
				LanguageId = Conversion.ToInt(CmbLanguage.Value)
			});
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "608-InvRptProductionSummeryPartyProcessing.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn609ProductionIssuance_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionReports.FoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_609(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderTransactionHistory.Value)
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For DisPlay");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdTransactionHistoryMain).Name);
			if (fs != null)
			{
				grdTransactionHistoryMain.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdTransactionhistoryDetail).Name);
			if (fs != null)
			{
				grdTransactionhistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromNoTransactionHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToNoTransactionHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdTransactionHistoryMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdTransactionHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdTransactionHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdTransactionHistoryMain.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdTransactionHistoryMain.CurrentColumn.Key == "Edit")
				{
					if (r.Cells["EntryType"].Value.ToString() == "Input")
					{
						RecIdInput = CurrId;
						ReadByIdInput(RecIdInput);
					}
					else
					{
						RecIdOutPut = CurrId;
						ReadByIdOutPut(RecIdOutPut);
					}
				}
				if (grdTransactionHistoryMain.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(CurrId);
				}
				if (!(grdTransactionHistoryMain.CurrentColumn.Key == "NoOfAttachments"))
				{
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				if (r.Cells["EntryType"].Value.ToString() == "Input")
				{
					RecIdInput = CurrId;
					ReadByIdInput(RecIdInput);
				}
				else
				{
					RecIdOutPut = CurrId;
					ReadByIdOutPut(RecIdOutPut);
				}
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

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetWeightOutPut();
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
			CalculateNetWeightOutPut();
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
			CalculateNetWeightOutPut();
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
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d87: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Expected O, but got Unknown
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103c: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Expected O, but got Unknown
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Expected O, but got Unknown
		//IL_1258: Unknown result type (might be due to invalid IL or missing references)
		//IL_1262: Expected O, but got Unknown
		//IL_1313: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Expected O, but got Unknown
		//IL_145d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1467: Expected O, but got Unknown
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_1493: Expected O, but got Unknown
		//IL_14d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e0: Expected O, but got Unknown
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1585: Expected O, but got Unknown
		//IL_15a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b1: Expected O, but got Unknown
		//IL_160a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1614: Expected O, but got Unknown
		//IL_168e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1698: Expected O, but got Unknown
		//IL_1f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f68: Expected O, but got Unknown
		//IL_1f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7f: Expected O, but got Unknown
		//IL_6e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e4c: Expected O, but got Unknown
		//IL_869b: Unknown result type (might be due to invalid IL or missing references)
		//IL_86a5: Expected O, but got Unknown
		//IL_d0c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_d0cb: Expected O, but got Unknown
		//IL_d0ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_d0f9: Expected O, but got Unknown
		//IL_e7c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_e7d0: Expected O, but got Unknown
		//IL_11afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b05: Expected O, but got Unknown
		//IL_11b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b33: Expected O, but got Unknown
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.frmProductionPartyProcessing));
		GridEXLayout grdInputHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputHistoryDetail_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdOutPut_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
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
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		GridEXLayout grdOutputHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutputHistoryDetail_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdPM_DesignTimeLayout = new GridEXLayout();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
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
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		GridEXLayout grdTransactionHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdTransactionhistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.InPut = new System.Windows.Forms.TabPage();
		this.tabControlInput = new System.Windows.Forms.TabControl();
		this.tabInputForm = new System.Windows.Forms.TabPage();
		this.grdInput = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel28 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdbtnItemCodeInput = new System.Windows.Forms.RadioButton();
		this.rdbtnItemNameInput = new System.Windows.Forms.RadioButton();
		this.btnAddInput = new System.Windows.Forms.Button();
		this.CmbStockPartyInput = new UltraCombo();
		this.cmbWareHouseInput = new UltraCombo();
		this.btnCancelInput = new System.Windows.Forms.Button();
		this.txtRemarksInput = new System.Windows.Forms.TextBox();
		this.btnUpdateDetailInput = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.cmbReferencePartyInput = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtQtyInput = new System.Windows.Forms.TextBox();
		this.cmbItemInput = new UltraCombo();
		this.txtNetWeightInput = new System.Windows.Forms.TextBox();
		this.label74 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.cmbCropYearInput = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.cmbLotInput = new UltraCombo();
		this.label72 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.txtBalanceQtyInputDetail = new System.Windows.Forms.TextBox();
		this.label66 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.cmbPackingTypeInput = new UltraCombo();
		this.cmbItemUomInput = new UltraCombo();
		this.txtBalanceWeightInputDetail = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.DocDateInput = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.DocNoInput = new System.Windows.Forms.TextBox();
		this.CmbJobOrderNoInput = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbWIPItemInput = new UltraCombo();
		this.CmbStockPartyWIPInput = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ChkPrintPreviewInput = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNewInput = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshInput = new System.Windows.Forms.ToolStripButton();
		this.btnSaveInput = new System.Windows.Forms.ToolStripButton();
		this.BtnDeleteInput = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateInput = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrintInput = new System.Windows.Forms.ToolStripButton();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.btnLoadInvoices = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeysInput = new System.Windows.Forms.ToolStripButton();
		this.label14 = new System.Windows.Forms.Label();
		this.tabInputHistory = new System.Windows.Forms.TabPage();
		this.grdInputHistoryMain = new GridEX();
		this.panel78 = new System.Windows.Forms.Panel();
		this.label130 = new System.Windows.Forms.Label();
		this.grdInputHistoryDetail = new GridEX();
		this.panel59 = new System.Windows.Forms.Panel();
		this.label127 = new System.Windows.Forms.Label();
		this.ctrlGrdBar10 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel58 = new System.Windows.Forms.Panel();
		this.RdModifyDate = new System.Windows.Forms.RadioButton();
		this.RrDocDateInput = new System.Windows.Forms.RadioButton();
		this.RdEntryDateInput = new System.Windows.Forms.RadioButton();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.CmbJobOrderInputHistory = new UltraCombo();
		this.BtnShowInputHistory = new System.Windows.Forms.Button();
		this.label145 = new System.Windows.Forms.Label();
		this.ToDateHistoryInput = new System.Windows.Forms.DateTimePicker();
		this.label146 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.ToDocNoHistoryInput = new System.Windows.Forms.TextBox();
		this.FromDateHistoryInput = new System.Windows.Forms.DateTimePicker();
		this.label148 = new System.Windows.Forms.Label();
		this.label149 = new System.Windows.Forms.Label();
		this.FromNoHistoryInput = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label124 = new System.Windows.Forms.Label();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistoryInput = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshInputHistory = new System.Windows.Forms.ToolStripButton();
		this.OutPut = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControlOutPut = new System.Windows.Forms.TabControl();
		this.tabOutputForm = new System.Windows.Forms.TabPage();
		this.grdOutPut = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel30 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.label165 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label166 = new System.Windows.Forms.Label();
		this.label167 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.rdbtnItemCodeOutPut = new System.Windows.Forms.RadioButton();
		this.rdbtnItemNameOutPut = new System.Windows.Forms.RadioButton();
		this.btnAddOutPut = new System.Windows.Forms.Button();
		this.txtRemarksOutPut = new System.Windows.Forms.TextBox();
		this.btnCancelOutPut = new System.Windows.Forms.Button();
		this.CmbEntryType = new UltraCombo();
		this.btnUpdateDetailOutPut = new System.Windows.Forms.Button();
		this.label70 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbPackTypeOutPut = new UltraCombo();
		this.txtQtyOutPut = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbWarehouseOutput = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.txtNetWeightOutPut = new System.Windows.Forms.TextBox();
		this.CmbItemOutput = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbUomOutPut = new UltraCombo();
		this.CmbCropYearOutPut = new UltraCombo();
		this.CmbLotOutPut = new UltraCombo();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtDocNoOutPut = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoOutput = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.DocDateOutPut = new System.Windows.Forms.DateTimePicker();
		this.txtOutPutTotalQty = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtOutPutTotalWeight = new System.Windows.Forms.TextBox();
		this.txtRemarksHeaderOutPut = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.txtInPutTotalWeight = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtInPutTotalQty = new System.Windows.Forms.TextBox();
		this.txtAvgFinishGoodRate = new System.Windows.Forms.TextBox();
		this.btnApplyRate = new System.Windows.Forms.Button();
		this.txtFGRate = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.txtFGWeight = new System.Windows.Forms.TextBox();
		this.label75 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ChkPrintPreviewOutPut = new System.Windows.Forms.CheckBox();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.btnNewOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOutPut = new System.Windows.Forms.ToolStripButton();
		this.BtnDeleteOutput = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeysOutPut = new System.Windows.Forms.ToolStripButton();
		this.label16 = new System.Windows.Forms.Label();
		this.tabOutputHistory = new System.Windows.Forms.TabPage();
		this.grdOutputHistoryMain = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.grdOutputHistoryDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.RadModifyDateHistoryOutPut = new System.Windows.Forms.RadioButton();
		this.RadDocDateHistoryOutPut = new System.Windows.Forms.RadioButton();
		this.RadEntryDateHistoryOutPut = new System.Windows.Forms.RadioButton();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.CmbJobOrderHistoryOutPut = new UltraCombo();
		this.tnShowHistoryOutPut = new System.Windows.Forms.Button();
		this.label21 = new System.Windows.Forms.Label();
		this.ToDateHistoryOutPut = new System.Windows.Forms.DateTimePicker();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.ToNoHistoryOutPut = new System.Windows.Forms.TextBox();
		this.FromDateHistoryOutPut = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.FromNoHistoryOutPut = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistoryOutPut = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistoryOutPut = new System.Windows.Forms.ToolStripButton();
		this.PM = new System.Windows.Forms.TabPage();
		this.grdPM = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label44 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.RadModifyDatePMHistory = new System.Windows.Forms.RadioButton();
		this.RadDocDatePMHistory = new System.Windows.Forms.RadioButton();
		this.RadEntrydatePMHistory = new System.Windows.Forms.RadioButton();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.CmbJobOrderPMHistory = new UltraCombo();
		this.BtnShowPMHistory = new System.Windows.Forms.Button();
		this.label31 = new System.Windows.Forms.Label();
		this.ToDatePMHistory = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.ToDocPMHistory = new System.Windows.Forms.TextBox();
		this.FromDatePMHistory = new System.Windows.Forms.DateTimePicker();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.FromDocPMHistory = new System.Windows.Forms.TextBox();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.toolStrip7 = new System.Windows.Forms.ToolStrip();
		this.BtnNewPMHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshPMHistory = new System.Windows.Forms.ToolStripButton();
		this.panel24 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbJobOrderPM = new UltraCombo();
		this.txtDocNoPM = new System.Windows.Forms.TextBox();
		this.txtdocdatePM = new System.Windows.Forms.DateTimePicker();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.cmbWarehouse = new UltraCombo();
		this.rdbtnItemCodePM = new System.Windows.Forms.RadioButton();
		this.txtremarksPM = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.rdbtnItemNamePM = new System.Windows.Forms.RadioButton();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.cmbItemPM = new UltraCombo();
		this.txtPMQTY = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label42 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNewPM = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshPM = new System.Windows.Forms.ToolStripButton();
		this.btnSavePM = new System.Windows.Forms.ToolStripButton();
		this.btnUpdatePM = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnDeletePM = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintPM = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeysPM = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdTransactionHistoryMain = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdTransactionhistoryDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label50 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel21 = new System.Windows.Forms.Panel();
		this.RadModifyDateTransactionHistory = new System.Windows.Forms.RadioButton();
		this.RadDocDateTransactionHistory = new System.Windows.Forms.RadioButton();
		this.RadEntryDateTransactionHistory = new System.Windows.Forms.RadioButton();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.CmbJobOrderTransactionHistory = new UltraCombo();
		this.BtnShowTransactionHistory = new System.Windows.Forms.Button();
		this.label33 = new System.Windows.Forms.Label();
		this.ToDateTransactionHistory = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.ToNoTransactionHistory = new System.Windows.Forms.TextBox();
		this.FromDateTransactionHistory = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.FromNoTransactionHistory = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnNewTransactionHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshTransactionHistory = new System.Windows.Forms.ToolStripButton();
		this.btnSummeryReport = new System.Windows.Forms.ToolStripButton();
		this.btn609ProductionIssuance = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeysTransactionHistory = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControlMain.SuspendLayout();
		this.InPut.SuspendLayout();
		this.tabControlInput.SuspendLayout();
		this.tabInputForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel8.SuspendLayout();
		this.panel28.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferencePartyInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyWIPInput).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabInputHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistoryMain).BeginInit();
		this.panel78.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistoryDetail).BeginInit();
		this.panel59.SuspendLayout();
		this.panel58.SuspendLayout();
		this.groupBox11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.OutPut.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControlOutPut.SuspendLayout();
		this.tabOutputForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).BeginInit();
		this.panel31.SuspendLayout();
		this.panel30.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.tabOutputHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistoryMain).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistoryDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistoryOutPut).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.PM.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPM).BeginInit();
		this.panel18.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel19.SuspendLayout();
		this.groupBox7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderPMHistory).BeginInit();
		this.panel20.SuspendLayout();
		this.toolStrip7.SuspendLayout();
		this.panel24.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPM).BeginInit();
		this.groupBox9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPM).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTransactionHistoryMain).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTransactionhistoryDetail).BeginInit();
		this.panel13.SuspendLayout();
		this.panel21.SuspendLayout();
		this.groupBox8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderTransactionHistory).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlMain);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 682);
		this.panel1.TabIndex = 0;
		this.tabControlMain.Controls.Add(this.InPut);
		this.tabControlMain.Controls.Add(this.OutPut);
		this.tabControlMain.Controls.Add(this.PM);
		this.tabControlMain.Controls.Add(this.tabPage2);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1084, 682);
		this.tabControlMain.TabIndex = 1;
		this.tabControlMain.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.InPut.BackColor = System.Drawing.Color.White;
		this.InPut.Controls.Add(this.tabControlInput);
		this.InPut.Location = new System.Drawing.Point(4, 25);
		this.InPut.Name = "InPut";
		this.InPut.Padding = new System.Windows.Forms.Padding(3);
		this.InPut.Size = new System.Drawing.Size(1076, 653);
		this.InPut.TabIndex = 0;
		this.InPut.Text = "Input";
		this.tabControlInput.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlInput.Controls.Add(this.tabInputForm);
		this.tabControlInput.Controls.Add(this.tabInputHistory);
		this.tabControlInput.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlInput.Location = new System.Drawing.Point(3, 3);
		this.tabControlInput.Name = "tabControlInput";
		this.tabControlInput.SelectedIndex = 0;
		this.tabControlInput.Size = new System.Drawing.Size(1070, 647);
		this.tabControlInput.TabIndex = 1;
		this.tabControlInput.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabInputForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.tabInputForm.Controls.Add(this.panel8);
		this.tabInputForm.Controls.Add(this.panel28);
		this.tabInputForm.Controls.Add(this.panel2);
		this.tabInputForm.Location = new System.Drawing.Point(4, 4);
		this.tabInputForm.Name = "tabInputForm";
		this.tabInputForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabInputForm.Size = new System.Drawing.Size(1062, 618);
		this.tabInputForm.TabIndex = 0;
		this.tabInputForm.Text = "Form";
		this.tabInputForm.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInput).BackColor = System.Drawing.Color.White;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInput.DynamicFiltering = true;
		this.grdInput.FilterMode = (FilterMode)2;
		this.grdInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		this.grdInput.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(3, 148);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(1056, 467);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.grdInput.CellUpdated += new ColumnActionEventHandler(grdInput_CellUpdated);
		this.grdInput.ColumnButtonClick += new ColumnActionEventHandler(grdInput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInput).DoubleClick += new System.EventHandler(grdInput_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdInput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInput_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 128);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1056, 20);
		this.panel8.TabIndex = 21;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(42, 14);
		this.label2.TabIndex = 212;
		this.label2.Text = "Detail";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1013, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdInput;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel28.Controls.Add(this.groupBox3);
		this.panel28.Controls.Add(this.groupBox1);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(3, 54);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1056, 74);
		this.panel28.TabIndex = 0;
		this.groupBox3.Controls.Add(this.rdbtnItemCodeInput);
		this.groupBox3.Controls.Add(this.rdbtnItemNameInput);
		this.groupBox3.Controls.Add(this.btnAddInput);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockPartyInput);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseInput);
		this.groupBox3.Controls.Add(this.btnCancelInput);
		this.groupBox3.Controls.Add(this.txtRemarksInput);
		this.groupBox3.Controls.Add(this.btnUpdateDetailInput);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.label67);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput);
		this.groupBox3.Controls.Add(this.label80);
		this.groupBox3.Controls.Add(this.label64);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtQtyInput);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemInput);
		this.groupBox3.Controls.Add(this.txtNetWeightInput);
		this.groupBox3.Controls.Add(this.label74);
		this.groupBox3.Controls.Add(this.label81);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYearInput);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLotInput);
		this.groupBox3.Controls.Add(this.label72);
		this.groupBox3.Controls.Add(this.label65);
		this.groupBox3.Controls.Add(this.txtBalanceQtyInputDetail);
		this.groupBox3.Controls.Add(this.label66);
		this.groupBox3.Controls.Add(this.label78);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUomInput);
		this.groupBox3.Controls.Add(this.txtBalanceWeightInputDetail);
		this.groupBox3.Controls.Add(this.label82);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 71);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1051, 114);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.groupBox3.Visible = false;
		this.rdbtnItemCodeInput.AutoSize = true;
		this.rdbtnItemCodeInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCodeInput.Location = new System.Drawing.Point(490, 16);
		this.rdbtnItemCodeInput.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCodeInput.Name = "rdbtnItemCodeInput";
		this.rdbtnItemCodeInput.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCodeInput.TabIndex = 213;
		this.rdbtnItemCodeInput.Text = "Code";
		this.rdbtnItemCodeInput.UseVisualStyleBackColor = true;
		this.rdbtnItemNameInput.AutoSize = true;
		this.rdbtnItemNameInput.Checked = true;
		this.rdbtnItemNameInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemNameInput.Location = new System.Drawing.Point(435, 16);
		this.rdbtnItemNameInput.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemNameInput.Name = "rdbtnItemNameInput";
		this.rdbtnItemNameInput.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemNameInput.TabIndex = 212;
		this.rdbtnItemNameInput.TabStop = true;
		this.rdbtnItemNameInput.Text = "Name";
		this.rdbtnItemNameInput.UseVisualStyleBackColor = true;
		this.rdbtnItemNameInput.CheckedChanged += new System.EventHandler(rdbtnItemNameInput_CheckedChanged);
		this.btnAddInput.BackColor = System.Drawing.Color.Teal;
		this.btnAddInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddInput.ForeColor = System.Drawing.Color.White;
		this.btnAddInput.Location = new System.Drawing.Point(899, 88);
		this.btnAddInput.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddInput.Name = "btnAddInput";
		this.btnAddInput.Size = new System.Drawing.Size(35, 26);
		this.btnAddInput.TabIndex = 13;
		this.btnAddInput.Text = "+";
		this.btnAddInput.UseVisualStyleBackColor = false;
		this.btnAddInput.Click += new System.EventHandler(btnAddInput_Click);
		this.CmbStockPartyInput.AllowNull = (DefaultableBoolean)1;
		this.CmbStockPartyInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockPartyInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockPartyInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockPartyInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStockPartyInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyInput).Location = new System.Drawing.Point(4, 35);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyInput).Name = "CmbStockPartyInput";
		((System.Windows.Forms.Control)(object)this.CmbStockPartyInput).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyInput).TabIndex = 0;
		this.CmbStockPartyInput.ValueChanged += new System.EventHandler(cmbStockParty_ValueChanged);
		this.cmbWareHouseInput.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance278).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance278;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance290).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance291).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance291).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance291).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance291;
		((AppearanceBase)appearance293).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Location = new System.Drawing.Point(201, 35);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Name = "cmbWareHouseInput";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).TabIndex = 1;
		this.cmbWareHouseInput.ValueChanged += new System.EventHandler(cmbWareHouseInput_ValueChanged);
		this.btnCancelInput.BackColor = System.Drawing.Color.Teal;
		this.btnCancelInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelInput.ForeColor = System.Drawing.Color.White;
		this.btnCancelInput.Location = new System.Drawing.Point(973, 88);
		this.btnCancelInput.Name = "btnCancelInput";
		this.btnCancelInput.Size = new System.Drawing.Size(69, 26);
		this.btnCancelInput.TabIndex = 15;
		this.btnCancelInput.Text = "Cancel";
		this.btnCancelInput.UseVisualStyleBackColor = false;
		this.btnCancelInput.Visible = false;
		this.btnCancelInput.Click += new System.EventHandler(btnCancelInput_Click);
		this.txtRemarksInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksInput.Location = new System.Drawing.Point(606, 90);
		this.txtRemarksInput.Name = "txtRemarksInput";
		this.txtRemarksInput.Size = new System.Drawing.Size(291, 23);
		this.txtRemarksInput.TabIndex = 12;
		this.btnUpdateDetailInput.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailInput.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailInput.Location = new System.Drawing.Point(899, 88);
		this.btnUpdateDetailInput.Name = "btnUpdateDetailInput";
		this.btnUpdateDetailInput.Size = new System.Drawing.Size(74, 26);
		this.btnUpdateDetailInput.TabIndex = 14;
		this.btnUpdateDetailInput.Text = "Update ";
		this.btnUpdateDetailInput.UseVisualStyleBackColor = false;
		this.btnUpdateDetailInput.Visible = false;
		this.btnUpdateDetailInput.Click += new System.EventHandler(btnUpdateDetailInput_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(398, 66);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 211;
		this.label4.Text = "Reference Party";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(201, 18);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(69, 15);
		this.label67.TabIndex = 193;
		this.label67.Text = "WareHouse";
		this.cmbReferencePartyInput.AllowNull = (DefaultableBoolean)1;
		this.cmbReferencePartyInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferencePartyInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferencePartyInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferencePartyInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReferencePartyInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput).Location = new System.Drawing.Point(398, 88);
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput).Name = "cmbReferencePartyInput";
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyInput).TabIndex = 11;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label80.Location = new System.Drawing.Point(606, 66);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(52, 15);
		this.label80.TabIndex = 200;
		this.label80.Text = "Remarks";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(201, 66);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(53, 15);
		this.label64.TabIndex = 206;
		this.label64.Text = "Quantity";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(4, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 209;
		this.label3.Text = "Stock Party";
		this.txtQtyInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyInput.Location = new System.Drawing.Point(201, 90);
		this.txtQtyInput.Name = "txtQtyInput";
		this.txtQtyInput.Size = new System.Drawing.Size(86, 23);
		this.txtQtyInput.TabIndex = 9;
		this.txtQtyInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInput_KeyPress);
		this.cmbItemInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance311;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance317).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance318).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance319).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(398, 35);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Leave += new System.EventHandler(cmbItemInput_Leave);
		this.txtNetWeightInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeightInput.Location = new System.Drawing.Point(289, 90);
		this.txtNetWeightInput.Name = "txtNetWeightInput";
		this.txtNetWeightInput.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightInput.TabIndex = 10;
		this.txtNetWeightInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightInput_KeyPress);
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label74.Location = new System.Drawing.Point(398, 18);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(32, 15);
		this.label74.TabIndex = 191;
		this.label74.Text = "Item";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label81.Location = new System.Drawing.Point(286, 66);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(68, 15);
		this.label81.TabIndex = 207;
		this.label81.Text = "Net Weight";
		this.cmbCropYearInput.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYearInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Location = new System.Drawing.Point(606, 35);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Name = "cmbCropYearInput";
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).TabIndex = 3;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(606, 18);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(57, 15);
		this.label61.TabIndex = 194;
		this.label61.Text = "Crop Year";
		this.cmbLotInput.AllowNull = (DefaultableBoolean)1;
		this.cmbLotInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLotInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLotInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance298;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance304).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance305).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance305).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance305).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance306).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLotInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Location = new System.Drawing.Point(707, 35);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Name = "cmbLotInput";
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).TabIndex = 4;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(4, 66);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(70, 15);
		this.label72.TabIndex = 197;
		this.label72.Text = "Balance Qty";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(707, 18);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(46, 15);
		this.label65.TabIndex = 195;
		this.label65.Text = "Job Lot";
		this.txtBalanceQtyInputDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQtyInputDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalanceQtyInputDetail.Location = new System.Drawing.Point(4, 90);
		this.txtBalanceQtyInputDetail.Name = "txtBalanceQtyInputDetail";
		this.txtBalanceQtyInputDetail.ReadOnly = true;
		this.txtBalanceQtyInputDetail.Size = new System.Drawing.Size(86, 23);
		this.txtBalanceQtyInputDetail.TabIndex = 7;
		this.txtBalanceQtyInputDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(872, 18);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(60, 15);
		this.label66.TabIndex = 196;
		this.label66.Text = "Pack Type";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label78.Location = new System.Drawing.Point(975, 18);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(35, 15);
		this.label78.TabIndex = 192;
		this.label78.Text = "UOM";
		this.cmbPackingTypeInput.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingTypeInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingTypeInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingTypeInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackingTypeInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Location = new System.Drawing.Point(872, 35);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Name = "cmbPackingTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).TabIndex = 5;
		this.cmbItemUomInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUomInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUomInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUomInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemUomInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Location = new System.Drawing.Point(975, 35);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Name = "cmbItemUomInput";
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Size = new System.Drawing.Size(74, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).TabIndex = 6;
		this.txtBalanceWeightInputDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeightInputDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalanceWeightInputDetail.Location = new System.Drawing.Point(92, 90);
		this.txtBalanceWeightInputDetail.Name = "txtBalanceWeightInputDetail";
		this.txtBalanceWeightInputDetail.ReadOnly = true;
		this.txtBalanceWeightInputDetail.Size = new System.Drawing.Size(107, 23);
		this.txtBalanceWeightInputDetail.TabIndex = 8;
		this.txtBalanceWeightInputDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceWeight_KeyPress);
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label82.Location = new System.Drawing.Point(96, 66);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(90, 15);
		this.label82.TabIndex = 199;
		this.label82.Text = "Balance Weight";
		this.groupBox1.Controls.Add(this.DocDateInput);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.DocNoInput);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPItemInput);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1047, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.DocDateInput.CustomFormat = "dd-MMM-yy";
		this.DocDateInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDateInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateInput.Location = new System.Drawing.Point(180, 16);
		this.DocDateInput.Name = "DocDateInput";
		this.DocDateInput.Size = new System.Drawing.Size(142, 23);
		this.DocDateInput.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Doc No,Date";
		this.DocNoInput.BackColor = System.Drawing.Color.White;
		this.DocNoInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.DocNoInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocNoInput.Location = new System.Drawing.Point(82, 16);
		this.DocNoInput.Name = "DocNoInput";
		this.DocNoInput.ReadOnly = true;
		this.DocNoInput.Size = new System.Drawing.Size(96, 23);
		this.DocNoInput.TabIndex = 0;
		this.CmbJobOrderNoInput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobOrderNoInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Location = new System.Drawing.Point(82, 42);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Name = "CmbJobOrderNoInput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 48);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 151;
		this.label23.Text = "Job Order No";
		this.CmbWIPItemInput.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWIPItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).Location = new System.Drawing.Point(394, 14);
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).Name = "CmbWIPItemInput";
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbWIPItemInput).TabStop = false;
		this.CmbStockPartyWIPInput.AllowNull = (DefaultableBoolean)1;
		this.CmbStockPartyWIPInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockPartyWIPInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockPartyWIPInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockPartyWIPInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStockPartyWIPInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).Location = new System.Drawing.Point(394, 42);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).Name = "CmbStockPartyWIPInput";
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyWIPInput).TabStop = false;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label77.Location = new System.Drawing.Point(325, 20);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(58, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "WIP Item";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(325, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 173;
		this.label7.Text = "Stock Party";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(683, 14);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(361, 54);
		this.txtRemarks.TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(632, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 59;
		this.label6.Text = "Remarks";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ChkPrintPreviewInput);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1056, 51);
		this.panel2.TabIndex = 0;
		this.ChkPrintPreviewInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreviewInput.AutoSize = true;
		this.ChkPrintPreviewInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreviewInput.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreviewInput.Location = new System.Drawing.Point(927, 29);
		this.ChkPrintPreviewInput.Name = "ChkPrintPreviewInput";
		this.ChkPrintPreviewInput.Size = new System.Drawing.Size(122, 20);
		this.ChkPrintPreviewInput.TabIndex = 26;
		this.ChkPrintPreviewInput.Text = "Print Preview";
		this.ChkPrintPreviewInput.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.BtnNewInput, this.btnRefreshInput, this.btnSaveInput, this.BtnDeleteInput, this.btnUpdateInput, this.toolStripSeparator, this.BtnPrintInput, this.btnJobOrderDefine, this.btnLoadInvoices, this.btnShortcutKeysInput });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1056, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNewInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnNewInput.Image = (System.Drawing.Image)resources.GetObject("BtnNewInput.Image");
		this.BtnNewInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewInput.Name = "BtnNewInput";
		this.BtnNewInput.Size = new System.Drawing.Size(59, 24);
		this.BtnNewInput.Text = "&New";
		this.BtnNewInput.Click += new System.EventHandler(btnnew_Click);
		this.btnRefreshInput.Image = (System.Drawing.Image)resources.GetObject("btnRefreshInput.Image");
		this.btnRefreshInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshInput.Name = "btnRefreshInput";
		this.btnRefreshInput.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshInput.Text = "Refresh";
		this.btnRefreshInput.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSaveInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSaveInput.Image = (System.Drawing.Image)resources.GetObject("btnSaveInput.Image");
		this.btnSaveInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveInput.Name = "btnSaveInput";
		this.btnSaveInput.Size = new System.Drawing.Size(64, 24);
		this.btnSaveInput.Text = "&Save";
		this.btnSaveInput.Click += new System.EventHandler(btnsave_Click);
		this.BtnDeleteInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnDeleteInput.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeleteInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeleteInput.Name = "BtnDeleteInput";
		this.BtnDeleteInput.Size = new System.Drawing.Size(73, 24);
		this.BtnDeleteInput.Text = "&Delete";
		this.BtnDeleteInput.Visible = false;
		this.BtnDeleteInput.Click += new System.EventHandler(BtnDeleteInput_Click);
		this.btnUpdateInput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdateInput.Image = (System.Drawing.Image)resources.GetObject("btnUpdateInput.Image");
		this.btnUpdateInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateInput.Name = "btnUpdateInput";
		this.btnUpdateInput.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateInput.Text = "&Update";
		this.btnUpdateInput.Visible = false;
		this.btnUpdateInput.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.BtnPrintInput.Image = (System.Drawing.Image)resources.GetObject("BtnPrintInput.Image");
		this.BtnPrintInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintInput.Name = "BtnPrintInput";
		this.BtnPrintInput.Size = new System.Drawing.Size(117, 24);
		this.BtnPrintInput.Text = "601_01_Print";
		this.BtnPrintInput.Click += new System.EventHandler(Print_Click);
		this.btnJobOrderDefine.Image = (System.Drawing.Image)resources.GetObject("btnJobOrderDefine.Image");
		this.btnJobOrderDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnJobOrderDefine.Name = "btnJobOrderDefine";
		this.btnJobOrderDefine.Size = new System.Drawing.Size(140, 24);
		this.btnJobOrderDefine.Text = "Define Job Order";
		this.btnJobOrderDefine.Click += new System.EventHandler(btnJobOrderDefine_Click);
		this.btnLoadInvoices.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnLoadInvoices.Image = (System.Drawing.Image)resources.GetObject("btnLoadInvoices.Image");
		this.btnLoadInvoices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(102, 24);
		this.btnLoadInvoices.Text = "Load Input";
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.btnShortcutKeysInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeysInput.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeysInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeysInput.Name = "btnShortcutKeysInput";
		this.btnShortcutKeysInput.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeysInput.Text = "ShortCut Keys";
		this.btnShortcutKeysInput.Click += new System.EventHandler(btnShortcutKeysInput_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 29);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(201, 19);
		this.label14.TabIndex = 25;
		this.label14.Text = "Production Input           ";
		this.tabInputHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputHistoryMain);
		this.tabInputHistory.Controls.Add(this.panel78);
		this.tabInputHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail);
		this.tabInputHistory.Controls.Add(this.panel59);
		this.tabInputHistory.Controls.Add(this.panel58);
		this.tabInputHistory.Controls.Add(this.panel5);
		this.tabInputHistory.Location = new System.Drawing.Point(4, 4);
		this.tabInputHistory.Name = "tabInputHistory";
		this.tabInputHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabInputHistory.Size = new System.Drawing.Size(1062, 618);
		this.tabInputHistory.TabIndex = 1;
		this.tabInputHistory.Text = "History";
		this.tabInputHistory.UseVisualStyleBackColor = true;
		this.grdInputHistoryMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdInputHistoryMain).BackColor = System.Drawing.Color.White;
		grdInputHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputHistoryMain.DesignTimeLayout = grdInputHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputHistoryMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).Location = new System.Drawing.Point(3, 127);
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).Name = "grdInputHistoryMain";
		((ScrollableBase)this.grdInputHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).Size = new System.Drawing.Size(1056, 235);
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).TabIndex = 1;
		this.grdInputHistoryMain.TotalRow = (InheritableBoolean)1;
		this.grdInputHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputHistoryMain).VisualStyle = (VisualStyle)4;
		this.grdInputHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(grdinputhistory_ColumnButtonClick);
		this.grdInputHistoryMain.SelectionChanged += new System.EventHandler(grdInputHistoryMain_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdInputHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputHistoryMain_KeyDown);
		this.panel78.BackColor = System.Drawing.Color.Teal;
		this.panel78.Controls.Add(this.label130);
		this.panel78.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel78.Location = new System.Drawing.Point(3, 362);
		this.panel78.Name = "panel78";
		this.panel78.Size = new System.Drawing.Size(1056, 20);
		this.panel78.TabIndex = 13;
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label130.ForeColor = System.Drawing.Color.White;
		this.label130.Location = new System.Drawing.Point(3, 1);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(230, 17);
		this.label130.TabIndex = 211;
		this.label130.Text = "Detail of the Selected above row";
		this.grdInputHistoryDetail.AllowEdit = (InheritableBoolean)2;
		this.grdInputHistoryDetail.AlternatingColors = true;
		((ScrollableBase)this.grdInputHistoryDetail).BackColor = System.Drawing.Color.White;
		this.grdInputHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdInputHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputHistoryDetail.DesignTimeLayout = grdInputHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdInputHistoryDetail.FilterMode = (FilterMode)2;
		this.grdInputHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputHistoryDetail.GroupByBoxVisible = false;
		this.grdInputHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).Location = new System.Drawing.Point(3, 382);
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).Name = "grdInputHistoryDetail";
		((ScrollableBase)this.grdInputHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).Size = new System.Drawing.Size(1056, 233);
		((System.Windows.Forms.Control)(object)this.grdInputHistoryDetail).TabIndex = 2;
		this.grdInputHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel59.BackColor = System.Drawing.Color.Teal;
		this.panel59.Controls.Add(this.label127);
		this.panel59.Controls.Add(this.ctrlGrdBar10);
		this.panel59.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel59.Location = new System.Drawing.Point(3, 103);
		this.panel59.Name = "panel59";
		this.panel59.Size = new System.Drawing.Size(1056, 24);
		this.panel59.TabIndex = 9;
		this.label127.AutoSize = true;
		this.label127.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label127.ForeColor = System.Drawing.Color.White;
		this.label127.Location = new System.Drawing.Point(3, 3);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(40, 17);
		this.label127.TabIndex = 211;
		this.label127.Text = "Main";
		this.ctrlGrdBar10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar10.FormName = null;
		this.ctrlGrdBar10.Location = new System.Drawing.Point(1009, 1);
		this.ctrlGrdBar10.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar10.MyGrid = this.grdInputHistoryMain;
		this.ctrlGrdBar10.Name = "ctrlGrdBar10";
		this.ctrlGrdBar10.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar10.TabIndex = 174;
		this.ctrlGrdBar10.Load += new System.EventHandler(ctrlGrdBar10_Load);
		this.panel58.Controls.Add(this.RdModifyDate);
		this.panel58.Controls.Add(this.RrDocDateInput);
		this.panel58.Controls.Add(this.RdEntryDateInput);
		this.panel58.Controls.Add(this.groupBox11);
		this.panel58.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel58.Location = new System.Drawing.Point(3, 53);
		this.panel58.Name = "panel58";
		this.panel58.Size = new System.Drawing.Size(1056, 50);
		this.panel58.TabIndex = 0;
		this.RdModifyDate.AutoSize = true;
		this.RdModifyDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdModifyDate.Location = new System.Drawing.Point(970, 31);
		this.RdModifyDate.Name = "RdModifyDate";
		this.RdModifyDate.Size = new System.Drawing.Size(89, 17);
		this.RdModifyDate.TabIndex = 3;
		this.RdModifyDate.Text = "Modify Date";
		this.RdModifyDate.UseVisualStyleBackColor = true;
		this.RrDocDateInput.AutoSize = true;
		this.RrDocDateInput.Checked = true;
		this.RrDocDateInput.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RrDocDateInput.Location = new System.Drawing.Point(970, 1);
		this.RrDocDateInput.Name = "RrDocDateInput";
		this.RrDocDateInput.Size = new System.Drawing.Size(72, 17);
		this.RrDocDateInput.TabIndex = 1;
		this.RrDocDateInput.TabStop = true;
		this.RrDocDateInput.Text = "Doc Date";
		this.RrDocDateInput.UseVisualStyleBackColor = true;
		this.RdEntryDateInput.AutoSize = true;
		this.RdEntryDateInput.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdEntryDateInput.Location = new System.Drawing.Point(970, 16);
		this.RdEntryDateInput.Name = "RdEntryDateInput";
		this.RdEntryDateInput.Size = new System.Drawing.Size(78, 17);
		this.RdEntryDateInput.TabIndex = 2;
		this.RdEntryDateInput.Text = "Entry Date";
		this.RdEntryDateInput.UseVisualStyleBackColor = true;
		this.groupBox11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory);
		this.groupBox11.Controls.Add(this.BtnShowInputHistory);
		this.groupBox11.Controls.Add(this.label145);
		this.groupBox11.Controls.Add(this.ToDateHistoryInput);
		this.groupBox11.Controls.Add(this.label146);
		this.groupBox11.Controls.Add(this.label147);
		this.groupBox11.Controls.Add(this.ToDocNoHistoryInput);
		this.groupBox11.Controls.Add(this.FromDateHistoryInput);
		this.groupBox11.Controls.Add(this.label148);
		this.groupBox11.Controls.Add(this.label149);
		this.groupBox11.Controls.Add(this.FromNoHistoryInput);
		this.groupBox11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox11.Location = new System.Drawing.Point(1, 2);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Size = new System.Drawing.Size(966, 46);
		this.groupBox11.TabIndex = 0;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "Filters";
		this.CmbJobOrderInputHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderInputHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderInputHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderInputHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderInputHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Location = new System.Drawing.Point(710, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Name = "CmbJobOrderInputHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).TabIndex = 4;
		this.BtnShowInputHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowInputHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowInputHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowInputHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowInputHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowInputHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowInputHistory.Location = new System.Drawing.Point(910, 15);
		this.BtnShowInputHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowInputHistory.Name = "BtnShowInputHistory";
		this.BtnShowInputHistory.Size = new System.Drawing.Size(54, 25);
		this.BtnShowInputHistory.TabIndex = 5;
		this.BtnShowInputHistory.Text = "Show";
		this.BtnShowInputHistory.UseVisualStyleBackColor = false;
		this.BtnShowInputHistory.Click += new System.EventHandler(BtnShowInputHistory_Click);
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(652, 20);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(59, 15);
		this.label145.TabIndex = 178;
		this.label145.Text = "Job Order";
		this.ToDateHistoryInput.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryInput.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryInput.Location = new System.Drawing.Point(257, 16);
		this.ToDateHistoryInput.Name = "ToDateHistoryInput";
		this.ToDateHistoryInput.ShowCheckBox = true;
		this.ToDateHistoryInput.Size = new System.Drawing.Size(137, 23);
		this.ToDateHistoryInput.TabIndex = 1;
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(206, 20);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(48, 15);
		this.label146.TabIndex = 171;
		this.label146.Text = "Date To";
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(2, 20);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 171;
		this.label147.Text = "Date From";
		this.ToDocNoHistoryInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistoryInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistoryInput.Location = new System.Drawing.Point(578, 16);
		this.ToDocNoHistoryInput.Name = "ToDocNoHistoryInput";
		this.ToDocNoHistoryInput.Size = new System.Drawing.Size(72, 23);
		this.ToDocNoHistoryInput.TabIndex = 3;
		this.ToDocNoHistoryInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistoryInput_KeyPress);
		this.FromDateHistoryInput.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryInput.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryInput.Location = new System.Drawing.Point(66, 16);
		this.FromDateHistoryInput.Name = "FromDateHistoryInput";
		this.FromDateHistoryInput.ShowCheckBox = true;
		this.FromDateHistoryInput.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistoryInput.TabIndex = 0;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(533, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(45, 15);
		this.label148.TabIndex = 173;
		this.label148.Text = "Doc To";
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(398, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(60, 15);
		this.label149.TabIndex = 173;
		this.label149.Text = "Doc From";
		this.FromNoHistoryInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromNoHistoryInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromNoHistoryInput.Location = new System.Drawing.Point(459, 16);
		this.FromNoHistoryInput.Name = "FromNoHistoryInput";
		this.FromNoHistoryInput.Size = new System.Drawing.Size(72, 23);
		this.FromNoHistoryInput.TabIndex = 2;
		this.FromNoHistoryInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromNoHistoryInput_KeyPress);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label124);
		this.panel5.Controls.Add(this.toolStrip5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.ForeColor = System.Drawing.Color.Black;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1056, 50);
		this.panel5.TabIndex = 4;
		this.label124.AutoSize = true;
		this.label124.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label124.ForeColor = System.Drawing.Color.White;
		this.label124.Location = new System.Drawing.Point(3, 29);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(209, 19);
		this.label124.TabIndex = 212;
		this.label124.Text = "Production Input History";
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistoryInput, this.BtnRefreshInputHistory });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1056, 27);
		this.toolStrip5.TabIndex = 3;
		this.toolStrip5.Text = "toolStrip5";
		this.BtnNewHistoryInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistoryInput.ForeColor = System.Drawing.Color.Black;
		this.BtnNewHistoryInput.Image = (System.Drawing.Image)resources.GetObject("BtnNewHistoryInput.Image");
		this.BtnNewHistoryInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistoryInput.Name = "BtnNewHistoryInput";
		this.BtnNewHistoryInput.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistoryInput.Text = "&New";
		this.BtnNewHistoryInput.Click += new System.EventHandler(BtnNewHistoryInput_Click);
		this.BtnRefreshInputHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshInputHistory.Image = (System.Drawing.Image)resources.GetObject("BtnRefreshInputHistory.Image");
		this.BtnRefreshInputHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshInputHistory.Name = "BtnRefreshInputHistory";
		this.BtnRefreshInputHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshInputHistory.Text = "Refresh";
		this.BtnRefreshInputHistory.Click += new System.EventHandler(BtnRefreshInputHistory_Click);
		this.OutPut.Controls.Add(this.panel3);
		this.OutPut.Location = new System.Drawing.Point(4, 25);
		this.OutPut.Name = "OutPut";
		this.OutPut.Size = new System.Drawing.Size(1076, 653);
		this.OutPut.TabIndex = 5;
		this.OutPut.Text = "OutPut";
		this.OutPut.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.tabControlOutPut);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1076, 653);
		this.panel3.TabIndex = 0;
		this.tabControlOutPut.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlOutPut.Controls.Add(this.tabOutputForm);
		this.tabControlOutPut.Controls.Add(this.tabOutputHistory);
		this.tabControlOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlOutPut.Location = new System.Drawing.Point(0, 0);
		this.tabControlOutPut.Name = "tabControlOutPut";
		this.tabControlOutPut.SelectedIndex = 0;
		this.tabControlOutPut.Size = new System.Drawing.Size(1076, 653);
		this.tabControlOutPut.TabIndex = 2;
		this.tabControlOutPut.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabOutputForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPut);
		this.tabOutputForm.Controls.Add(this.panel31);
		this.tabOutputForm.Controls.Add(this.panel30);
		this.tabOutputForm.Controls.Add(this.panel6);
		this.tabOutputForm.Location = new System.Drawing.Point(4, 4);
		this.tabOutputForm.Name = "tabOutputForm";
		this.tabOutputForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabOutputForm.Size = new System.Drawing.Size(1068, 624);
		this.tabOutputForm.TabIndex = 0;
		this.tabOutputForm.Text = "Form";
		this.tabOutputForm.UseVisualStyleBackColor = true;
		this.grdOutPut.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOutPut).BackColor = System.Drawing.Color.White;
		grdOutPut_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPut.DesignTimeLayout = grdOutPut_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPut.DynamicFiltering = true;
		this.grdOutPut.FilterMode = (FilterMode)2;
		this.grdOutPut.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPut.GroupByBoxVisible = false;
		this.grdOutPut.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Location = new System.Drawing.Point(3, 268);
		((System.Windows.Forms.Control)(object)this.grdOutPut).Name = "grdOutPut";
		((ScrollableBase)this.grdOutPut).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Size = new System.Drawing.Size(1062, 353);
		((System.Windows.Forms.Control)(object)this.grdOutPut).TabIndex = 0;
		this.grdOutPut.TotalRow = (InheritableBoolean)1;
		this.grdOutPut.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPut).VisualStyle = (VisualStyle)4;
		this.grdOutPut.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPut).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOutPut).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPut_KeyDown);
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label15);
		this.panel31.Controls.Add(this.ctrlGrdBar4);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 243);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1062, 25);
		this.panel31.TabIndex = 180;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(1, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(42, 14);
		this.label15.TabIndex = 213;
		this.label15.Text = "Detail";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1013, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdOutPut;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel30.Controls.Add(this.groupBox5);
		this.panel30.Controls.Add(this.groupBox4);
		this.panel30.Controls.Add(this.txtAvgFinishGoodRate);
		this.panel30.Controls.Add(this.btnApplyRate);
		this.panel30.Controls.Add(this.txtFGRate);
		this.panel30.Controls.Add(this.label76);
		this.panel30.Controls.Add(this.txtFGWeight);
		this.panel30.Controls.Add(this.label75);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(3, 54);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1062, 189);
		this.panel30.TabIndex = 179;
		this.groupBox5.Controls.Add(this.txtEbTotal);
		this.groupBox5.Controls.Add(this.label165);
		this.groupBox5.Controls.Add(this.txtEbUnit);
		this.groupBox5.Controls.Add(this.label166);
		this.groupBox5.Controls.Add(this.label167);
		this.groupBox5.Controls.Add(this.txtGrossWeight);
		this.groupBox5.Controls.Add(this.rdbtnItemCodeOutPut);
		this.groupBox5.Controls.Add(this.rdbtnItemNameOutPut);
		this.groupBox5.Controls.Add(this.btnAddOutPut);
		this.groupBox5.Controls.Add(this.txtRemarksOutPut);
		this.groupBox5.Controls.Add(this.btnCancelOutPut);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryType);
		this.groupBox5.Controls.Add(this.btnUpdateDetailOutPut);
		this.groupBox5.Controls.Add(this.label70);
		this.groupBox5.Controls.Add(this.label9);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.label36);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut);
		this.groupBox5.Controls.Add(this.txtQtyOutPut);
		this.groupBox5.Controls.Add(this.label45);
		this.groupBox5.Controls.Add(this.label20);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput);
		this.groupBox5.Controls.Add(this.label69);
		this.groupBox5.Controls.Add(this.txtNetWeightOutPut);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOutput);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add(this.label24);
		this.groupBox5.Controls.Add(this.label43);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomOutPut);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotOutPut);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(3, 74);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1051, 111);
		this.groupBox5.TabIndex = 1;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Detail";
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(336, 80);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotal.TabIndex = 10;
		this.txtEbTotal.Tag = "EbTotal";
		this.txtEbTotal.Text = "0";
		this.txtEbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotal.TextChanged += new System.EventHandler(txtEbTotal_TextChanged);
		this.txtEbTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotal_KeyPress);
		this.label165.AutoSize = true;
		this.label165.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label165.Location = new System.Drawing.Point(336, 60);
		this.label165.Name = "label165";
		this.label165.Size = new System.Drawing.Size(46, 15);
		this.label165.TabIndex = 188;
		this.label165.Text = "EbTotal";
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(265, 80);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(70, 23);
		this.txtEbUnit.TabIndex = 9;
		this.txtEbUnit.Tag = "EbUnit";
		this.txtEbUnit.Text = "0";
		this.txtEbUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnit_KeyPress);
		this.label166.AutoSize = true;
		this.label166.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label166.Location = new System.Drawing.Point(265, 60);
		this.label166.Name = "label166";
		this.label166.Size = new System.Drawing.Size(42, 15);
		this.label166.TabIndex = 187;
		this.label166.Text = "EbUnit";
		this.label167.AutoSize = true;
		this.label167.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label167.Location = new System.Drawing.Point(179, 60);
		this.label167.Name = "label167";
		this.label167.Size = new System.Drawing.Size(78, 15);
		this.label167.TabIndex = 186;
		this.label167.Text = "Gross Weight";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(179, 80);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(85, 23);
		this.txtGrossWeight.TabIndex = 8;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.rdbtnItemCodeOutPut.AutoSize = true;
		this.rdbtnItemCodeOutPut.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCodeOutPut.Location = new System.Drawing.Point(442, 12);
		this.rdbtnItemCodeOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCodeOutPut.Name = "rdbtnItemCodeOutPut";
		this.rdbtnItemCodeOutPut.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCodeOutPut.TabIndex = 182;
		this.rdbtnItemCodeOutPut.Text = "Code";
		this.rdbtnItemCodeOutPut.UseVisualStyleBackColor = true;
		this.rdbtnItemNameOutPut.AutoSize = true;
		this.rdbtnItemNameOutPut.Checked = true;
		this.rdbtnItemNameOutPut.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemNameOutPut.Location = new System.Drawing.Point(381, 12);
		this.rdbtnItemNameOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemNameOutPut.Name = "rdbtnItemNameOutPut";
		this.rdbtnItemNameOutPut.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemNameOutPut.TabIndex = 181;
		this.rdbtnItemNameOutPut.TabStop = true;
		this.rdbtnItemNameOutPut.Text = "Name";
		this.rdbtnItemNameOutPut.UseVisualStyleBackColor = true;
		this.rdbtnItemNameOutPut.CheckedChanged += new System.EventHandler(rdbtnItemNameOutPut_CheckedChanged);
		this.btnAddOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnAddOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddOutPut.ForeColor = System.Drawing.Color.White;
		this.btnAddOutPut.Location = new System.Drawing.Point(905, 78);
		this.btnAddOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddOutPut.Name = "btnAddOutPut";
		this.btnAddOutPut.Size = new System.Drawing.Size(31, 26);
		this.btnAddOutPut.TabIndex = 13;
		this.btnAddOutPut.Text = "+";
		this.btnAddOutPut.UseVisualStyleBackColor = false;
		this.btnAddOutPut.Click += new System.EventHandler(btnAddOutPut_Click);
		this.txtRemarksOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksOutPut.Location = new System.Drawing.Point(517, 80);
		this.txtRemarksOutPut.Name = "txtRemarksOutPut";
		this.txtRemarksOutPut.Size = new System.Drawing.Size(385, 23);
		this.txtRemarksOutPut.TabIndex = 12;
		this.btnCancelOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnCancelOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelOutPut.ForeColor = System.Drawing.Color.White;
		this.btnCancelOutPut.Location = new System.Drawing.Point(977, 78);
		this.btnCancelOutPut.Name = "btnCancelOutPut";
		this.btnCancelOutPut.Size = new System.Drawing.Size(69, 26);
		this.btnCancelOutPut.TabIndex = 15;
		this.btnCancelOutPut.Text = "Cancel";
		this.btnCancelOutPut.UseVisualStyleBackColor = false;
		this.btnCancelOutPut.Click += new System.EventHandler(btnCancelOutPut_Click);
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Location = new System.Drawing.Point(4, 33);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Name = "CmbEntryType";
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).TabIndex = 0;
		this.btnUpdateDetailOutPut.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailOutPut.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailOutPut.Location = new System.Drawing.Point(905, 78);
		this.btnUpdateDetailOutPut.Name = "btnUpdateDetailOutPut";
		this.btnUpdateDetailOutPut.Size = new System.Drawing.Size(72, 26);
		this.btnUpdateDetailOutPut.TabIndex = 14;
		this.btnUpdateDetailOutPut.Text = "Update ";
		this.btnUpdateDetailOutPut.UseVisualStyleBackColor = false;
		this.btnUpdateDetailOutPut.Click += new System.EventHandler(btnUpdateDetailOutPut_Click);
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(593, 14);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(57, 15);
		this.label70.TabIndex = 141;
		this.label70.Text = "Crop Year";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(517, 60);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 149;
		this.label9.Text = "Remarks";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(4, 14);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 147;
		this.label13.Text = "Entry Type";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(94, 60);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(53, 15);
		this.label36.TabIndex = 145;
		this.label36.Text = "Quantity";
		this.CmbPackTypeOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbPackTypeOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackTypeOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackTypeOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackTypeOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Location = new System.Drawing.Point(905, 33);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Name = "CmbPackTypeOutPut";
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).TabIndex = 5;
		this.txtQtyOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyOutPut.Location = new System.Drawing.Point(94, 80);
		this.txtQtyOutPut.Name = "txtQtyOutPut";
		this.txtQtyOutPut.Size = new System.Drawing.Size(83, 23);
		this.txtQtyOutPut.TabIndex = 7;
		this.txtQtyOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOutPut.TextChanged += new System.EventHandler(txtQtyOutPut_TextChanged);
		this.txtQtyOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyOutPut_KeyPress);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(902, 14);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(77, 15);
		this.label45.TabIndex = 143;
		this.label45.Text = "Packing Type";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(4, 60);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(35, 15);
		this.label20.TabIndex = 136;
		this.label20.Text = "UOM";
		this.CmbWarehouseOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance218;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Location = new System.Drawing.Point(125, 33);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Name = "CmbWarehouseOutput";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).TabIndex = 1;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(698, 14);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(46, 15);
		this.label69.TabIndex = 142;
		this.label69.Text = "Job Lot";
		this.txtNetWeightOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeightOutPut.Location = new System.Drawing.Point(424, 80);
		this.txtNetWeightOutPut.Name = "txtNetWeightOutPut";
		this.txtNetWeightOutPut.Size = new System.Drawing.Size(91, 23);
		this.txtNetWeightOutPut.TabIndex = 11;
		this.txtNetWeightOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightOutPut_KeyPress);
		this.CmbItemOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Location = new System.Drawing.Point(344, 33);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Name = "CmbItemOutput";
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Leave += new System.EventHandler(CmbItemOutput_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(424, 60);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 148;
		this.label11.Text = "Net Weight";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(344, 14);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(32, 15);
		this.label24.TabIndex = 135;
		this.label24.Text = "Item";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(125, 14);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(69, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "WareHouse";
		this.CmbUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance237).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Location = new System.Drawing.Point(4, 78);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Name = "CmbUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Size = new System.Drawing.Size(88, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Leave += new System.EventHandler(CmbUomOutPut_Leave);
		this.CmbCropYearOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYearOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Location = new System.Drawing.Point(593, 33);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Name = "CmbCropYearOutPut";
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Size = new System.Drawing.Size(103, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).TabIndex = 3;
		this.CmbLotOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLotOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance205;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance211).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLotOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Location = new System.Drawing.Point(698, 33);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Name = "CmbLotOutPut";
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).TabIndex = 4;
		this.groupBox4.Controls.Add(this.label17);
		this.groupBox4.Controls.Add(this.txtDocNoOutPut);
		this.groupBox4.Controls.Add(this.label60);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput);
		this.groupBox4.Controls.Add(this.label12);
		this.groupBox4.Controls.Add(this.DocDateOutPut);
		this.groupBox4.Controls.Add(this.txtOutPutTotalQty);
		this.groupBox4.Controls.Add(this.label19);
		this.groupBox4.Controls.Add(this.txtOutPutTotalWeight);
		this.groupBox4.Controls.Add(this.txtRemarksHeaderOutPut);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Controls.Add(this.txtInPutTotalWeight);
		this.groupBox4.Controls.Add(this.label53);
		this.groupBox4.Controls.Add(this.txtInPutTotalQty);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1051, 74);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Main";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(2, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 183;
		this.label17.Text = "Doc No,Date";
		this.txtDocNoOutPut.BackColor = System.Drawing.Color.White;
		this.txtDocNoOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOutPut.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOutPut.Location = new System.Drawing.Point(84, 16);
		this.txtDocNoOutPut.Name = "txtDocNoOutPut";
		this.txtDocNoOutPut.ReadOnly = true;
		this.txtDocNoOutPut.Size = new System.Drawing.Size(93, 23);
		this.txtDocNoOutPut.TabIndex = 0;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(2, 45);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(78, 15);
		this.label60.TabIndex = 153;
		this.label60.Text = "Job Order No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(851, 50);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(87, 15);
		this.label10.TabIndex = 181;
		this.label10.Text = "OutPut Weight";
		this.CmbJobOrderNoOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
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
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobOrderNoOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Location = new System.Drawing.Point(84, 44);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Name = "CmbJobOrderNoOutput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Leave += new System.EventHandler(CmbJobOrderNoOutput_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(698, 50);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 182;
		this.label12.Text = "OutPut Qty";
		this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.DocDateOutPut.CustomFormat = "dd-MMM-yy";
		this.DocDateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDateOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateOutPut.Location = new System.Drawing.Point(179, 17);
		this.DocDateOutPut.Name = "DocDateOutPut";
		this.DocDateOutPut.Size = new System.Drawing.Size(142, 23);
		this.DocDateOutPut.TabIndex = 1;
		this.txtOutPutTotalQty.Enabled = false;
		this.txtOutPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOutPutTotalQty.Location = new System.Drawing.Point(765, 46);
		this.txtOutPutTotalQty.Name = "txtOutPutTotalQty";
		this.txtOutPutTotalQty.Size = new System.Drawing.Size(83, 23);
		this.txtOutPutTotalQty.TabIndex = 6;
		this.txtOutPutTotalQty.TabStop = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(327, 21);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 138;
		this.label19.Text = "Remarks";
		this.txtOutPutTotalWeight.Enabled = false;
		this.txtOutPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOutPutTotalWeight.Location = new System.Drawing.Point(938, 46);
		this.txtOutPutTotalWeight.Name = "txtOutPutTotalWeight";
		this.txtOutPutTotalWeight.Size = new System.Drawing.Size(108, 23);
		this.txtOutPutTotalWeight.TabIndex = 7;
		this.txtOutPutTotalWeight.TabStop = false;
		this.txtRemarksHeaderOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksHeaderOutPut.Location = new System.Drawing.Point(385, 17);
		this.txtRemarksHeaderOutPut.Multiline = true;
		this.txtRemarksHeaderOutPut.Name = "txtRemarksHeaderOutPut";
		this.txtRemarksHeaderOutPut.Size = new System.Drawing.Size(311, 53);
		this.txtRemarksHeaderOutPut.TabIndex = 3;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(851, 21);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(78, 15);
		this.label54.TabIndex = 174;
		this.label54.Text = "InPut Weight";
		this.txtInPutTotalWeight.Enabled = false;
		this.txtInPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInPutTotalWeight.Location = new System.Drawing.Point(938, 17);
		this.txtInPutTotalWeight.Name = "txtInPutTotalWeight";
		this.txtInPutTotalWeight.Size = new System.Drawing.Size(108, 23);
		this.txtInPutTotalWeight.TabIndex = 5;
		this.txtInPutTotalWeight.TabStop = false;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(698, 21);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(58, 15);
		this.label53.TabIndex = 174;
		this.label53.Text = "InPut Qty";
		this.label53.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtInPutTotalQty.Enabled = false;
		this.txtInPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInPutTotalQty.Location = new System.Drawing.Point(765, 17);
		this.txtInPutTotalQty.Name = "txtInPutTotalQty";
		this.txtInPutTotalQty.Size = new System.Drawing.Size(83, 23);
		this.txtInPutTotalQty.TabIndex = 4;
		this.txtInPutTotalQty.TabStop = false;
		this.txtAvgFinishGoodRate.BackColor = System.Drawing.Color.White;
		this.txtAvgFinishGoodRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFinishGoodRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvgFinishGoodRate.Location = new System.Drawing.Point(1282, 4);
		this.txtAvgFinishGoodRate.Name = "txtAvgFinishGoodRate";
		this.txtAvgFinishGoodRate.ReadOnly = true;
		this.txtAvgFinishGoodRate.Size = new System.Drawing.Size(93, 23);
		this.txtAvgFinishGoodRate.TabIndex = 6;
		this.txtAvgFinishGoodRate.Visible = false;
		this.btnApplyRate.BackColor = System.Drawing.Color.White;
		this.btnApplyRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnApplyRate.Location = new System.Drawing.Point(1301, 32);
		this.btnApplyRate.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplyRate.Name = "btnApplyRate";
		this.btnApplyRate.Size = new System.Drawing.Size(74, 24);
		this.btnApplyRate.TabIndex = 178;
		this.btnApplyRate.Text = "ApplyRate";
		this.btnApplyRate.UseVisualStyleBackColor = false;
		this.btnApplyRate.Visible = false;
		this.txtFGRate.BackColor = System.Drawing.Color.White;
		this.txtFGRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFGRate.Location = new System.Drawing.Point(1188, 33);
		this.txtFGRate.Name = "txtFGRate";
		this.txtFGRate.ReadOnly = true;
		this.txtFGRate.Size = new System.Drawing.Size(93, 23);
		this.txtFGRate.TabIndex = 176;
		this.txtFGRate.Visible = false;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(1119, 37);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(47, 15);
		this.label76.TabIndex = 177;
		this.label76.Text = "FG Rate";
		this.label76.Visible = false;
		this.txtFGWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFGWeight.Location = new System.Drawing.Point(1188, 4);
		this.txtFGWeight.Name = "txtFGWeight";
		this.txtFGWeight.Size = new System.Drawing.Size(93, 23);
		this.txtFGWeight.TabIndex = 174;
		this.txtFGWeight.Visible = false;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(1119, 8);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(63, 15);
		this.label75.TabIndex = 175;
		this.label75.Text = "FG Weight";
		this.label75.Visible = false;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ChkPrintPreviewOutPut);
		this.panel6.Controls.Add(this.toolStrip6);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1062, 51);
		this.panel6.TabIndex = 0;
		this.ChkPrintPreviewOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreviewOutPut.AutoSize = true;
		this.ChkPrintPreviewOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreviewOutPut.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreviewOutPut.Location = new System.Drawing.Point(937, 29);
		this.ChkPrintPreviewOutPut.Name = "ChkPrintPreviewOutPut";
		this.ChkPrintPreviewOutPut.Size = new System.Drawing.Size(122, 20);
		this.ChkPrintPreviewOutPut.TabIndex = 135;
		this.ChkPrintPreviewOutPut.Text = "Print Preview";
		this.ChkPrintPreviewOutPut.UseVisualStyleBackColor = true;
		this.toolStrip6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewOutPut, this.btnRefreshOutPut, this.btnSaveOutPut, this.btnUpdateOutPut, this.BtnDeleteOutput, this.toolStripSeparator5, this.btnPrintOutPut, this.btnShortcutKeysOutPut });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1062, 27);
		this.toolStrip6.TabIndex = 2;
		this.toolStrip6.Text = "toolStrip6";
		this.btnNewOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewOutPut.Image = (System.Drawing.Image)resources.GetObject("btnNewOutPut.Image");
		this.btnNewOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewOutPut.Name = "btnNewOutPut";
		this.btnNewOutPut.Size = new System.Drawing.Size(59, 24);
		this.btnNewOutPut.Text = "&New";
		this.btnNewOutPut.Click += new System.EventHandler(btnNewOutPut_Click);
		this.btnRefreshOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshOutPut.Image = (System.Drawing.Image)resources.GetObject("btnRefreshOutPut.Image");
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
		this.btnUpdateOutPut.Image = (System.Drawing.Image)resources.GetObject("btnUpdateOutPut.Image");
		this.btnUpdateOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOutPut.Name = "btnUpdateOutPut";
		this.btnUpdateOutPut.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateOutPut.Text = "&Update";
		this.btnUpdateOutPut.Visible = false;
		this.btnUpdateOutPut.Click += new System.EventHandler(btnUpdateOutPut_Click);
		this.BtnDeleteOutput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnDeleteOutput.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeleteOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeleteOutput.Name = "BtnDeleteOutput";
		this.BtnDeleteOutput.Size = new System.Drawing.Size(73, 24);
		this.BtnDeleteOutput.Text = "&Delete";
		this.BtnDeleteOutput.Visible = false;
		this.BtnDeleteOutput.Click += new System.EventHandler(BtnDeleteOutput_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintOutPut.Image = (System.Drawing.Image)resources.GetObject("btnPrintOutPut.Image");
		this.btnPrintOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOutPut.Name = "btnPrintOutPut";
		this.btnPrintOutPut.Size = new System.Drawing.Size(117, 24);
		this.btnPrintOutPut.Text = "601_01_Print";
		this.btnPrintOutPut.Click += new System.EventHandler(btnPrintOutPut_Click);
		this.btnShortcutKeysOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeysOutPut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeysOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeysOutPut.Name = "btnShortcutKeysOutPut";
		this.btnShortcutKeysOutPut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeysOutPut.Text = "ShortCut Keys";
		this.btnShortcutKeysOutPut.Click += new System.EventHandler(btnShortcutKeysOutPut_Click);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 29);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(208, 19);
		this.label16.TabIndex = 134;
		this.label16.Text = "Production  Output         ";
		this.tabOutputHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain);
		this.tabOutputHistory.Controls.Add(this.panel14);
		this.tabOutputHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail);
		this.tabOutputHistory.Controls.Add(this.panel4);
		this.tabOutputHistory.Controls.Add(this.panel9);
		this.tabOutputHistory.Controls.Add(this.panel12);
		this.tabOutputHistory.Location = new System.Drawing.Point(4, 4);
		this.tabOutputHistory.Name = "tabOutputHistory";
		this.tabOutputHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabOutputHistory.Size = new System.Drawing.Size(1068, 624);
		this.tabOutputHistory.TabIndex = 1;
		this.tabOutputHistory.Text = "History";
		this.tabOutputHistory.UseVisualStyleBackColor = true;
		this.grdOutputHistoryMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOutputHistoryMain).BackColor = System.Drawing.Color.White;
		grdOutputHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutputHistoryMain.DesignTimeLayout = grdOutputHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutputHistoryMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).Location = new System.Drawing.Point(3, 127);
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).Name = "grdOutputHistoryMain";
		((ScrollableBase)this.grdOutputHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).Size = new System.Drawing.Size(1062, 258);
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).TabIndex = 2;
		this.grdOutputHistoryMain.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistoryMain).VisualStyle = (VisualStyle)4;
		this.grdOutputHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(grdOutputHistory_ColumnButtonClick);
		this.grdOutputHistoryMain.SelectionChanged += new System.EventHandler(grdOutputHistoryMain_SelectionChanged);
		this.grdOutputHistoryMain.LinkClicked += new ColumnActionEventHandler(grdOutputHistoryMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutputHistoryMain_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label29);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(3, 385);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1062, 20);
		this.panel14.TabIndex = 15;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(3, 1);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(230, 17);
		this.label29.TabIndex = 211;
		this.label29.Text = "Detail of the Selected above row";
		this.grdOutputHistoryDetail.AllowEdit = (InheritableBoolean)2;
		this.grdOutputHistoryDetail.AlternatingColors = true;
		((ScrollableBase)this.grdOutputHistoryDetail).BackColor = System.Drawing.Color.White;
		this.grdOutputHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOutputHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutputHistoryDetail.DesignTimeLayout = grdOutputHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdOutputHistoryDetail.FilterMode = (FilterMode)2;
		this.grdOutputHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutputHistoryDetail.GroupByBoxVisible = false;
		this.grdOutputHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).Location = new System.Drawing.Point(3, 405);
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).Name = "grdOutputHistoryDetail";
		((ScrollableBase)this.grdOutputHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).Size = new System.Drawing.Size(1062, 216);
		((System.Windows.Forms.Control)(object)this.grdOutputHistoryDetail).TabIndex = 14;
		this.grdOutputHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.ctrlGrdBar5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 103);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1062, 24);
		this.panel4.TabIndex = 12;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(40, 17);
		this.label18.TabIndex = 211;
		this.label18.Text = "Main";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1015, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdInputHistoryMain;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar5.TabIndex = 174;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.panel9.Controls.Add(this.RadModifyDateHistoryOutPut);
		this.panel9.Controls.Add(this.RadDocDateHistoryOutPut);
		this.panel9.Controls.Add(this.RadEntryDateHistoryOutPut);
		this.panel9.Controls.Add(this.groupBox6);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 53);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1062, 50);
		this.panel9.TabIndex = 10;
		this.RadModifyDateHistoryOutPut.AutoSize = true;
		this.RadModifyDateHistoryOutPut.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadModifyDateHistoryOutPut.Location = new System.Drawing.Point(974, 31);
		this.RadModifyDateHistoryOutPut.Name = "RadModifyDateHistoryOutPut";
		this.RadModifyDateHistoryOutPut.Size = new System.Drawing.Size(89, 17);
		this.RadModifyDateHistoryOutPut.TabIndex = 3;
		this.RadModifyDateHistoryOutPut.Text = "Modify Date";
		this.RadModifyDateHistoryOutPut.UseVisualStyleBackColor = true;
		this.RadDocDateHistoryOutPut.AutoSize = true;
		this.RadDocDateHistoryOutPut.Checked = true;
		this.RadDocDateHistoryOutPut.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadDocDateHistoryOutPut.Location = new System.Drawing.Point(974, 1);
		this.RadDocDateHistoryOutPut.Name = "RadDocDateHistoryOutPut";
		this.RadDocDateHistoryOutPut.Size = new System.Drawing.Size(72, 17);
		this.RadDocDateHistoryOutPut.TabIndex = 1;
		this.RadDocDateHistoryOutPut.TabStop = true;
		this.RadDocDateHistoryOutPut.Text = "Doc Date";
		this.RadDocDateHistoryOutPut.UseVisualStyleBackColor = true;
		this.RadEntryDateHistoryOutPut.AutoSize = true;
		this.RadEntryDateHistoryOutPut.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadEntryDateHistoryOutPut.Location = new System.Drawing.Point(974, 16);
		this.RadEntryDateHistoryOutPut.Name = "RadEntryDateHistoryOutPut";
		this.RadEntryDateHistoryOutPut.Size = new System.Drawing.Size(78, 17);
		this.RadEntryDateHistoryOutPut.TabIndex = 2;
		this.RadEntryDateHistoryOutPut.Text = "Entry Date";
		this.RadEntryDateHistoryOutPut.UseVisualStyleBackColor = true;
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut);
		this.groupBox6.Controls.Add(this.tnShowHistoryOutPut);
		this.groupBox6.Controls.Add(this.label21);
		this.groupBox6.Controls.Add(this.ToDateHistoryOutPut);
		this.groupBox6.Controls.Add(this.label22);
		this.groupBox6.Controls.Add(this.label25);
		this.groupBox6.Controls.Add(this.ToNoHistoryOutPut);
		this.groupBox6.Controls.Add(this.FromDateHistoryOutPut);
		this.groupBox6.Controls.Add(this.label26);
		this.groupBox6.Controls.Add(this.label27);
		this.groupBox6.Controls.Add(this.FromNoHistoryOutPut);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(6, 2);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(965, 46);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Filters";
		this.CmbJobOrderHistoryOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderHistoryOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderHistoryOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderHistoryOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderHistoryOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderHistoryOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut).Location = new System.Drawing.Point(710, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut).Name = "CmbJobOrderHistoryOutPut";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistoryOutPut).TabIndex = 4;
		this.tnShowHistoryOutPut.BackColor = System.Drawing.Color.Teal;
		this.tnShowHistoryOutPut.FlatAppearance.BorderSize = 0;
		this.tnShowHistoryOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.tnShowHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.tnShowHistoryOutPut.ForeColor = System.Drawing.Color.White;
		this.tnShowHistoryOutPut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tnShowHistoryOutPut.Location = new System.Drawing.Point(910, 15);
		this.tnShowHistoryOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.tnShowHistoryOutPut.Name = "tnShowHistoryOutPut";
		this.tnShowHistoryOutPut.Size = new System.Drawing.Size(53, 25);
		this.tnShowHistoryOutPut.TabIndex = 5;
		this.tnShowHistoryOutPut.Text = "Show";
		this.tnShowHistoryOutPut.UseVisualStyleBackColor = false;
		this.tnShowHistoryOutPut.Click += new System.EventHandler(tnShowHistoryOutPut_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(652, 20);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(59, 15);
		this.label21.TabIndex = 178;
		this.label21.Text = "Job Order";
		this.ToDateHistoryOutPut.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryOutPut.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryOutPut.Location = new System.Drawing.Point(257, 16);
		this.ToDateHistoryOutPut.Name = "ToDateHistoryOutPut";
		this.ToDateHistoryOutPut.ShowCheckBox = true;
		this.ToDateHistoryOutPut.Size = new System.Drawing.Size(137, 23);
		this.ToDateHistoryOutPut.TabIndex = 1;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(206, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(48, 15);
		this.label22.TabIndex = 171;
		this.label22.Text = "Date To";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(2, 20);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(63, 15);
		this.label25.TabIndex = 171;
		this.label25.Text = "Date From";
		this.ToNoHistoryOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToNoHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToNoHistoryOutPut.Location = new System.Drawing.Point(578, 16);
		this.ToNoHistoryOutPut.Name = "ToNoHistoryOutPut";
		this.ToNoHistoryOutPut.Size = new System.Drawing.Size(72, 23);
		this.ToNoHistoryOutPut.TabIndex = 3;
		this.ToNoHistoryOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToNoHistoryOutPut_KeyPress);
		this.FromDateHistoryOutPut.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryOutPut.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryOutPut.Location = new System.Drawing.Point(66, 16);
		this.FromDateHistoryOutPut.Name = "FromDateHistoryOutPut";
		this.FromDateHistoryOutPut.ShowCheckBox = true;
		this.FromDateHistoryOutPut.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistoryOutPut.TabIndex = 0;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(533, 20);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(45, 15);
		this.label26.TabIndex = 173;
		this.label26.Text = "Doc To";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(398, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(60, 15);
		this.label27.TabIndex = 173;
		this.label27.Text = "Doc From";
		this.FromNoHistoryOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromNoHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromNoHistoryOutPut.Location = new System.Drawing.Point(459, 16);
		this.FromNoHistoryOutPut.Name = "FromNoHistoryOutPut";
		this.FromNoHistoryOutPut.Size = new System.Drawing.Size(72, 23);
		this.FromNoHistoryOutPut.TabIndex = 2;
		this.FromNoHistoryOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromNoHistoryOutPut_KeyPress);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label28);
		this.panel12.Controls.Add(this.toolStrip3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.ForeColor = System.Drawing.Color.Black;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1062, 50);
		this.panel12.TabIndex = 11;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 29);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(221, 19);
		this.label28.TabIndex = 212;
		this.label28.Text = "Production Output History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistoryOutPut, this.BtnRefreshHistoryOutPut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1062, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnNewHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistoryOutPut.ForeColor = System.Drawing.Color.Black;
		this.BtnNewHistoryOutPut.Image = (System.Drawing.Image)resources.GetObject("BtnNewHistoryOutPut.Image");
		this.BtnNewHistoryOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistoryOutPut.Name = "BtnNewHistoryOutPut";
		this.BtnNewHistoryOutPut.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistoryOutPut.Text = "&New";
		this.BtnNewHistoryOutPut.Click += new System.EventHandler(BtnNewHistoryOutPut_Click);
		this.BtnRefreshHistoryOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistoryOutPut.Image = (System.Drawing.Image)resources.GetObject("BtnRefreshHistoryOutPut.Image");
		this.BtnRefreshHistoryOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistoryOutPut.Name = "BtnRefreshHistoryOutPut";
		this.BtnRefreshHistoryOutPut.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistoryOutPut.Text = "Refresh";
		this.BtnRefreshHistoryOutPut.Click += new System.EventHandler(BtnRefreshHistoryOutPut_Click);
		this.PM.Controls.Add((System.Windows.Forms.Control)(object)this.grdPM);
		this.PM.Controls.Add(this.panel18);
		this.PM.Controls.Add(this.panel22);
		this.PM.Controls.Add(this.panel24);
		this.PM.Controls.Add(this.panel7);
		this.PM.Location = new System.Drawing.Point(4, 25);
		this.PM.Name = "PM";
		this.PM.Size = new System.Drawing.Size(1076, 653);
		this.PM.TabIndex = 3;
		this.PM.Text = "Packing Material";
		this.PM.UseVisualStyleBackColor = true;
		this.grdPM.AllowEdit = (InheritableBoolean)2;
		grdPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPM.DesignTimeLayout = grdPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPM.DynamicFiltering = true;
		this.grdPM.FilterMode = (FilterMode)2;
		this.grdPM.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPM.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPM).Location = new System.Drawing.Point(0, 298);
		((System.Windows.Forms.Control)(object)this.grdPM).Name = "grdPM";
		((ScrollableBase)this.grdPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPM).Size = new System.Drawing.Size(1076, 355);
		((System.Windows.Forms.Control)(object)this.grdPM).TabIndex = 0;
		this.grdPM.TotalRow = (InheritableBoolean)1;
		this.grdPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPM).VisualStyle = (VisualStyle)4;
		this.grdPM.ColumnButtonClick += new ColumnActionEventHandler(grdPM_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPM).DoubleClick += new System.EventHandler(grdPM_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPM).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPM_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar6);
		this.panel18.Controls.Add(this.label44);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 277);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1076, 21);
		this.panel18.TabIndex = 3;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1024, 0);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.grdPM;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(48, 20);
		this.ctrlGrdBar6.TabIndex = 213;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(4, 2);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(112, 17);
		this.label44.TabIndex = 212;
		this.label44.Text = "Filtered Record";
		this.panel22.Controls.Add(this.panel19);
		this.panel22.Controls.Add(this.panel20);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 175);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1076, 102);
		this.panel22.TabIndex = 0;
		this.panel19.Controls.Add(this.RadModifyDatePMHistory);
		this.panel19.Controls.Add(this.RadDocDatePMHistory);
		this.panel19.Controls.Add(this.RadEntrydatePMHistory);
		this.panel19.Controls.Add(this.groupBox7);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 50);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1076, 50);
		this.panel19.TabIndex = 0;
		this.RadModifyDatePMHistory.AutoSize = true;
		this.RadModifyDatePMHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadModifyDatePMHistory.Location = new System.Drawing.Point(974, 31);
		this.RadModifyDatePMHistory.Name = "RadModifyDatePMHistory";
		this.RadModifyDatePMHistory.Size = new System.Drawing.Size(89, 17);
		this.RadModifyDatePMHistory.TabIndex = 3;
		this.RadModifyDatePMHistory.Text = "Modify Date";
		this.RadModifyDatePMHistory.UseVisualStyleBackColor = true;
		this.RadDocDatePMHistory.AutoSize = true;
		this.RadDocDatePMHistory.Checked = true;
		this.RadDocDatePMHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadDocDatePMHistory.Location = new System.Drawing.Point(974, 1);
		this.RadDocDatePMHistory.Name = "RadDocDatePMHistory";
		this.RadDocDatePMHistory.Size = new System.Drawing.Size(72, 17);
		this.RadDocDatePMHistory.TabIndex = 1;
		this.RadDocDatePMHistory.TabStop = true;
		this.RadDocDatePMHistory.Text = "Doc Date";
		this.RadDocDatePMHistory.UseVisualStyleBackColor = true;
		this.RadEntrydatePMHistory.AutoSize = true;
		this.RadEntrydatePMHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadEntrydatePMHistory.Location = new System.Drawing.Point(974, 16);
		this.RadEntrydatePMHistory.Name = "RadEntrydatePMHistory";
		this.RadEntrydatePMHistory.Size = new System.Drawing.Size(78, 17);
		this.RadEntrydatePMHistory.TabIndex = 2;
		this.RadEntrydatePMHistory.Text = "Entry Date";
		this.RadEntrydatePMHistory.UseVisualStyleBackColor = true;
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory);
		this.groupBox7.Controls.Add(this.BtnShowPMHistory);
		this.groupBox7.Controls.Add(this.label31);
		this.groupBox7.Controls.Add(this.ToDatePMHistory);
		this.groupBox7.Controls.Add(this.label32);
		this.groupBox7.Controls.Add(this.label34);
		this.groupBox7.Controls.Add(this.ToDocPMHistory);
		this.groupBox7.Controls.Add(this.FromDatePMHistory);
		this.groupBox7.Controls.Add(this.label37);
		this.groupBox7.Controls.Add(this.label38);
		this.groupBox7.Controls.Add(this.FromDocPMHistory);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(6, 2);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(965, 46);
		this.groupBox7.TabIndex = 0;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Filters";
		this.CmbJobOrderPMHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderPMHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderPMHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderPMHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance275;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance281).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderPMHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderPMHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory).Location = new System.Drawing.Point(710, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory).Name = "CmbJobOrderPMHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderPMHistory).TabIndex = 4;
		this.BtnShowPMHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowPMHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowPMHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowPMHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowPMHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowPMHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowPMHistory.Location = new System.Drawing.Point(910, 15);
		this.BtnShowPMHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowPMHistory.Name = "BtnShowPMHistory";
		this.BtnShowPMHistory.Size = new System.Drawing.Size(53, 25);
		this.BtnShowPMHistory.TabIndex = 5;
		this.BtnShowPMHistory.Text = "Show";
		this.BtnShowPMHistory.UseVisualStyleBackColor = false;
		this.BtnShowPMHistory.Click += new System.EventHandler(BtnShowPMHistory_Click);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(652, 20);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(59, 15);
		this.label31.TabIndex = 178;
		this.label31.Text = "Job Order";
		this.ToDatePMHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDatePMHistory.CustomFormat = "dd-MMM-yy";
		this.ToDatePMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDatePMHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDatePMHistory.Location = new System.Drawing.Point(257, 16);
		this.ToDatePMHistory.Name = "ToDatePMHistory";
		this.ToDatePMHistory.ShowCheckBox = true;
		this.ToDatePMHistory.Size = new System.Drawing.Size(137, 23);
		this.ToDatePMHistory.TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(206, 20);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(48, 15);
		this.label32.TabIndex = 171;
		this.label32.Text = "Date To";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(2, 20);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(63, 15);
		this.label34.TabIndex = 171;
		this.label34.Text = "Date From";
		this.ToDocPMHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocPMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocPMHistory.Location = new System.Drawing.Point(578, 16);
		this.ToDocPMHistory.Name = "ToDocPMHistory";
		this.ToDocPMHistory.Size = new System.Drawing.Size(72, 23);
		this.ToDocPMHistory.TabIndex = 3;
		this.ToDocPMHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocPMHistory_KeyPress);
		this.FromDatePMHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDatePMHistory.CustomFormat = "dd-MMM-yy";
		this.FromDatePMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDatePMHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDatePMHistory.Location = new System.Drawing.Point(66, 16);
		this.FromDatePMHistory.Name = "FromDatePMHistory";
		this.FromDatePMHistory.ShowCheckBox = true;
		this.FromDatePMHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDatePMHistory.TabIndex = 0;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(533, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(45, 15);
		this.label37.TabIndex = 173;
		this.label37.Text = "Doc To";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(398, 20);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(60, 15);
		this.label38.TabIndex = 173;
		this.label38.Text = "Doc From";
		this.FromDocPMHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocPMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocPMHistory.Location = new System.Drawing.Point(459, 16);
		this.FromDocPMHistory.Name = "FromDocPMHistory";
		this.FromDocPMHistory.Size = new System.Drawing.Size(72, 23);
		this.FromDocPMHistory.TabIndex = 2;
		this.FromDocPMHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocPMHistory_KeyPress);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label39);
		this.panel20.Controls.Add(this.toolStrip7);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.ForeColor = System.Drawing.Color.Black;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1076, 50);
		this.panel20.TabIndex = 13;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 1);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(190, 19);
		this.label39.TabIndex = 212;
		this.label39.Text = "Production PM History";
		this.toolStrip7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewPMHistory, this.BtnRefereshPMHistory });
		this.toolStrip7.Location = new System.Drawing.Point(0, 23);
		this.toolStrip7.Name = "toolStrip7";
		this.toolStrip7.Size = new System.Drawing.Size(1076, 27);
		this.toolStrip7.TabIndex = 3;
		this.toolStrip7.Text = "toolStrip7";
		this.BtnNewPMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewPMHistory.ForeColor = System.Drawing.Color.Black;
		this.BtnNewPMHistory.Image = (System.Drawing.Image)resources.GetObject("BtnNewPMHistory.Image");
		this.BtnNewPMHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewPMHistory.Name = "BtnNewPMHistory";
		this.BtnNewPMHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewPMHistory.Text = "&New";
		this.BtnNewPMHistory.Click += new System.EventHandler(BtnNewPMHistory_Click);
		this.BtnRefereshPMHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefereshPMHistory.Image = (System.Drawing.Image)resources.GetObject("BtnRefereshPMHistory.Image");
		this.BtnRefereshPMHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshPMHistory.Name = "BtnRefereshPMHistory";
		this.BtnRefereshPMHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshPMHistory.Text = "Refresh";
		this.BtnRefereshPMHistory.Click += new System.EventHandler(BtnRefereshPMHistory_Click);
		this.panel24.Controls.Add(this.groupBox2);
		this.panel24.Controls.Add(this.groupBox9);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 50);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1076, 125);
		this.panel24.TabIndex = 2;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderPM);
		this.groupBox2.Controls.Add(this.txtDocNoPM);
		this.groupBox2.Controls.Add(this.txtdocdatePM);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1072, 46);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(3, 19);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(76, 15);
		this.label30.TabIndex = 184;
		this.label30.Text = "Doc No,Date";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(328, 19);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(78, 15);
		this.label35.TabIndex = 116;
		this.label35.Text = "Job Order No";
		this.cmbJobOrderPM.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderPM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderPM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderPM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderPM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderPM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPM).Location = new System.Drawing.Point(406, 13);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPM).Name = "cmbJobOrderPM";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPM).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPM).TabIndex = 2;
		this.cmbJobOrderPM.ValueChanged += new System.EventHandler(cmbJobOrderPM_ValueChanged);
		this.txtDocNoPM.BackColor = System.Drawing.Color.White;
		this.txtDocNoPM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoPM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoPM.Location = new System.Drawing.Point(81, 15);
		this.txtDocNoPM.Name = "txtDocNoPM";
		this.txtDocNoPM.ReadOnly = true;
		this.txtDocNoPM.Size = new System.Drawing.Size(93, 23);
		this.txtDocNoPM.TabIndex = 0;
		this.txtdocdatePM.CustomFormat = "dd-MMM-yy";
		this.txtdocdatePM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdatePM.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdatePM.Location = new System.Drawing.Point(175, 15);
		this.txtdocdatePM.Name = "txtdocdatePM";
		this.txtdocdatePM.Size = new System.Drawing.Size(142, 23);
		this.txtdocdatePM.TabIndex = 1;
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWarehouse);
		this.groupBox9.Controls.Add(this.rdbtnItemCodePM);
		this.groupBox9.Controls.Add(this.txtremarksPM);
		this.groupBox9.Controls.Add(this.label63);
		this.groupBox9.Controls.Add(this.rdbtnItemNamePM);
		this.groupBox9.Controls.Add(this.label41);
		this.groupBox9.Controls.Add(this.label40);
		this.groupBox9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemPM);
		this.groupBox9.Controls.Add(this.txtPMQTY);
		this.groupBox9.Controls.Add(this.label5);
		this.groupBox9.Location = new System.Drawing.Point(3, 39);
		this.groupBox9.Name = "groupBox9";
		this.groupBox9.Size = new System.Drawing.Size(1070, 60);
		this.groupBox9.TabIndex = 1;
		this.groupBox9.TabStop = false;
		this.cmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance200).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Location = new System.Drawing.Point(2, 29);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Name = "cmbWarehouse";
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).TabIndex = 0;
		this.rdbtnItemCodePM.AutoSize = true;
		this.rdbtnItemCodePM.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdbtnItemCodePM.Location = new System.Drawing.Point(326, 10);
		this.rdbtnItemCodePM.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCodePM.Name = "rdbtnItemCodePM";
		this.rdbtnItemCodePM.Size = new System.Drawing.Size(52, 17);
		this.rdbtnItemCodePM.TabIndex = 186;
		this.rdbtnItemCodePM.Text = "Code";
		this.rdbtnItemCodePM.UseVisualStyleBackColor = true;
		this.txtremarksPM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksPM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksPM.Location = new System.Drawing.Point(585, 31);
		this.txtremarksPM.Name = "txtremarksPM";
		this.txtremarksPM.Size = new System.Drawing.Size(479, 23);
		this.txtremarksPM.TabIndex = 3;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(585, 11);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(52, 15);
		this.label63.TabIndex = 63;
		this.label63.Text = "Remarks";
		this.rdbtnItemNamePM.AutoSize = true;
		this.rdbtnItemNamePM.Checked = true;
		this.rdbtnItemNamePM.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdbtnItemNamePM.Location = new System.Drawing.Point(276, 10);
		this.rdbtnItemNamePM.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemNamePM.Name = "rdbtnItemNamePM";
		this.rdbtnItemNamePM.Size = new System.Drawing.Size(55, 17);
		this.rdbtnItemNamePM.TabIndex = 185;
		this.rdbtnItemNamePM.TabStop = true;
		this.rdbtnItemNamePM.Text = "Name";
		this.rdbtnItemNamePM.UseVisualStyleBackColor = true;
		this.rdbtnItemNamePM.CheckedChanged += new System.EventHandler(rdbtnItemNamePM_CheckedChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(479, 11);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(54, 15);
		this.label41.TabIndex = 59;
		this.label41.Text = "Item Qty";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(244, 11);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(32, 15);
		this.label40.TabIndex = 55;
		this.label40.Text = "Item";
		this.cmbItemPM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemPM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemPM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemPM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance210).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance232).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemPM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemPM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemPM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemPM).Location = new System.Drawing.Point(241, 29);
		((System.Windows.Forms.Control)(object)this.cmbItemPM).Name = "cmbItemPM";
		((System.Windows.Forms.Control)(object)this.cmbItemPM).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemPM).TabIndex = 1;
		this.txtPMQTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPMQTY.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMQTY.Location = new System.Drawing.Point(479, 31);
		this.txtPMQTY.Name = "txtPMQTY";
		this.txtPMQTY.Size = new System.Drawing.Size(104, 23);
		this.txtPMQTY.TabIndex = 2;
		this.txtPMQTY.TextChanged += new System.EventHandler(txtPMQTY_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(2, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 118;
		this.label5.Text = "WareHouse";
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label42);
		this.panel7.Controls.Add(this.toolStrip4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1076, 50);
		this.panel7.TabIndex = 0;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(2, 29);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(239, 19);
		this.label42.TabIndex = 212;
		this.label42.Text = "Production Packing Material";
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewPM, this.BtnRefreshPM, this.btnSavePM, this.btnUpdatePM, this.toolStripSeparator3, this.BtnDeletePM, this.BtnPrintPM, this.btnShortcutKeysPM });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1076, 27);
		this.toolStrip4.TabIndex = 58;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNewPM.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnNewPM.Image = (System.Drawing.Image)resources.GetObject("btnNewPM.Image");
		this.btnNewPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewPM.Name = "btnNewPM";
		this.btnNewPM.Size = new System.Drawing.Size(59, 24);
		this.btnNewPM.Text = "&New";
		this.btnNewPM.Click += new System.EventHandler(btnNewPM_Click);
		this.BtnRefreshPM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshPM.Image = (System.Drawing.Image)resources.GetObject("BtnRefreshPM.Image");
		this.BtnRefreshPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshPM.Name = "BtnRefreshPM";
		this.BtnRefreshPM.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshPM.Text = "Refresh";
		this.BtnRefreshPM.Click += new System.EventHandler(BtnRefreshPM_Click);
		this.btnSavePM.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSavePM.Image = (System.Drawing.Image)resources.GetObject("btnSavePM.Image");
		this.btnSavePM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSavePM.Name = "btnSavePM";
		this.btnSavePM.Size = new System.Drawing.Size(64, 24);
		this.btnSavePM.Text = "&Save";
		this.btnSavePM.Click += new System.EventHandler(btnsavePM_Click);
		this.btnUpdatePM.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdatePM.Image = (System.Drawing.Image)resources.GetObject("btnUpdatePM.Image");
		this.btnUpdatePM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdatePM.Name = "btnUpdatePM";
		this.btnUpdatePM.Size = new System.Drawing.Size(78, 24);
		this.btnUpdatePM.Text = "&Update";
		this.btnUpdatePM.Visible = false;
		this.btnUpdatePM.Click += new System.EventHandler(btnUpdatePM_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnDeletePM.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnDeletePM.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDeletePM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeletePM.Name = "BtnDeletePM";
		this.BtnDeletePM.Size = new System.Drawing.Size(73, 24);
		this.BtnDeletePM.Text = "&Delete";
		this.BtnDeletePM.Visible = false;
		this.BtnDeletePM.Click += new System.EventHandler(BtnDeletePM_Click);
		this.BtnPrintPM.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnPrintPM.Image = (System.Drawing.Image)resources.GetObject("BtnPrintPM.Image");
		this.BtnPrintPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintPM.Name = "BtnPrintPM";
		this.BtnPrintPM.Size = new System.Drawing.Size(117, 24);
		this.BtnPrintPM.Text = "601_02_Print";
		this.BtnPrintPM.Click += new System.EventHandler(BtnPrintPM_Click);
		this.btnShortcutKeysPM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeysPM.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeysPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeysPM.Name = "btnShortcutKeysPM";
		this.btnShortcutKeysPM.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeysPM.Text = "ShortCut Keys";
		this.btnShortcutKeysPM.Click += new System.EventHandler(btnShortcutKeysPM_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel21);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1076, 653);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Transaction History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdTransactionHistoryMain.AllowEdit = (InheritableBoolean)2;
		grdTransactionHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdTransactionHistoryMain.DesignTimeLayout = grdTransactionHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdTransactionHistoryMain.DynamicFiltering = true;
		this.grdTransactionHistoryMain.FilterMode = (FilterMode)2;
		this.grdTransactionHistoryMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdTransactionHistoryMain.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).Name = "grdTransactionHistoryMain";
		((ScrollableBase)this.grdTransactionHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).Size = new System.Drawing.Size(1070, 241);
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).TabIndex = 0;
		this.grdTransactionHistoryMain.TotalRow = (InheritableBoolean)1;
		this.grdTransactionHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdTransactionHistoryMain).VisualStyle = (VisualStyle)4;
		this.grdTransactionHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.grdTransactionHistoryMain.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.grdTransactionHistoryMain.LinkClicked += new ColumnActionEventHandler(grdTransactionHistoryMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdTransactionHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(grdTransactionHistoryMain_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label51);
		this.panel17.Controls.Add(this.ctrlGrdBar2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(3, 366);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1070, 21);
		this.panel17.TabIndex = 2;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(4, 2);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(236, 17);
		this.label51.TabIndex = 213;
		this.label51.Text = "Detail of the Above Selected Row";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1027, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar2.MyGrid = this.grdTransactionhistoryDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdTransactionhistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdTransactionhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdTransactionhistoryDetail.DesignTimeLayout = grdTransactionhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdTransactionhistoryDetail.DynamicFiltering = true;
		this.grdTransactionhistoryDetail.FilterMode = (FilterMode)2;
		this.grdTransactionhistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdTransactionhistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).Location = new System.Drawing.Point(3, 387);
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).Name = "grdTransactionhistoryDetail";
		((ScrollableBase)this.grdTransactionhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).Size = new System.Drawing.Size(1070, 263);
		((System.Windows.Forms.Control)(object)this.grdTransactionhistoryDetail).TabIndex = 1;
		this.grdTransactionhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdTransactionhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdTransactionhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label50);
		this.panel13.Controls.Add(this.ctrlGrdBar1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 103);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1070, 22);
		this.panel13.TabIndex = 2;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label50.ForeColor = System.Drawing.Color.White;
		this.label50.Location = new System.Drawing.Point(2, 1);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(132, 18);
		this.label50.TabIndex = 213;
		this.label50.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1027, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar1.MyGrid = this.grdTransactionHistoryMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar1.TabIndex = 4;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel21.Controls.Add(this.RadModifyDateTransactionHistory);
		this.panel21.Controls.Add(this.RadDocDateTransactionHistory);
		this.panel21.Controls.Add(this.RadEntryDateTransactionHistory);
		this.panel21.Controls.Add(this.groupBox8);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 53);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1070, 50);
		this.panel21.TabIndex = 46;
		this.RadModifyDateTransactionHistory.AutoSize = true;
		this.RadModifyDateTransactionHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadModifyDateTransactionHistory.Location = new System.Drawing.Point(970, 31);
		this.RadModifyDateTransactionHistory.Name = "RadModifyDateTransactionHistory";
		this.RadModifyDateTransactionHistory.Size = new System.Drawing.Size(89, 17);
		this.RadModifyDateTransactionHistory.TabIndex = 3;
		this.RadModifyDateTransactionHistory.Text = "Modify Date";
		this.RadModifyDateTransactionHistory.UseVisualStyleBackColor = true;
		this.RadDocDateTransactionHistory.AutoSize = true;
		this.RadDocDateTransactionHistory.Checked = true;
		this.RadDocDateTransactionHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadDocDateTransactionHistory.Location = new System.Drawing.Point(970, 1);
		this.RadDocDateTransactionHistory.Name = "RadDocDateTransactionHistory";
		this.RadDocDateTransactionHistory.Size = new System.Drawing.Size(72, 17);
		this.RadDocDateTransactionHistory.TabIndex = 1;
		this.RadDocDateTransactionHistory.TabStop = true;
		this.RadDocDateTransactionHistory.Text = "Doc Date";
		this.RadDocDateTransactionHistory.UseVisualStyleBackColor = true;
		this.RadEntryDateTransactionHistory.AutoSize = true;
		this.RadEntryDateTransactionHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadEntryDateTransactionHistory.Location = new System.Drawing.Point(970, 16);
		this.RadEntryDateTransactionHistory.Name = "RadEntryDateTransactionHistory";
		this.RadEntryDateTransactionHistory.Size = new System.Drawing.Size(78, 17);
		this.RadEntryDateTransactionHistory.TabIndex = 2;
		this.RadEntryDateTransactionHistory.Text = "Entry Date";
		this.RadEntryDateTransactionHistory.UseVisualStyleBackColor = true;
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory);
		this.groupBox8.Controls.Add(this.BtnShowTransactionHistory);
		this.groupBox8.Controls.Add(this.label33);
		this.groupBox8.Controls.Add(this.ToDateTransactionHistory);
		this.groupBox8.Controls.Add(this.label46);
		this.groupBox8.Controls.Add(this.label47);
		this.groupBox8.Controls.Add(this.ToNoTransactionHistory);
		this.groupBox8.Controls.Add(this.FromDateTransactionHistory);
		this.groupBox8.Controls.Add(this.label48);
		this.groupBox8.Controls.Add(this.label49);
		this.groupBox8.Controls.Add(this.FromNoTransactionHistory);
		this.groupBox8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox8.Location = new System.Drawing.Point(1, 2);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(966, 46);
		this.groupBox8.TabIndex = 0;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Filters";
		this.CmbJobOrderTransactionHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderTransactionHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderTransactionHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderTransactionHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderTransactionHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Location = new System.Drawing.Point(710, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Name = "CmbJobOrderTransactionHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).TabIndex = 4;
		this.BtnShowTransactionHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowTransactionHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowTransactionHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowTransactionHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowTransactionHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowTransactionHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowTransactionHistory.Location = new System.Drawing.Point(910, 15);
		this.BtnShowTransactionHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowTransactionHistory.Name = "BtnShowTransactionHistory";
		this.BtnShowTransactionHistory.Size = new System.Drawing.Size(54, 25);
		this.BtnShowTransactionHistory.TabIndex = 5;
		this.BtnShowTransactionHistory.Text = "Show";
		this.BtnShowTransactionHistory.UseVisualStyleBackColor = false;
		this.BtnShowTransactionHistory.Click += new System.EventHandler(BtnShowTransactionHistory_Click);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(652, 20);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(59, 15);
		this.label33.TabIndex = 178;
		this.label33.Text = "Job Order";
		this.ToDateTransactionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateTransactionHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateTransactionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateTransactionHistory.Location = new System.Drawing.Point(257, 16);
		this.ToDateTransactionHistory.Name = "ToDateTransactionHistory";
		this.ToDateTransactionHistory.ShowCheckBox = true;
		this.ToDateTransactionHistory.Size = new System.Drawing.Size(137, 23);
		this.ToDateTransactionHistory.TabIndex = 1;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(206, 20);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 15);
		this.label46.TabIndex = 171;
		this.label46.Text = "Date To";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(2, 20);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(63, 15);
		this.label47.TabIndex = 171;
		this.label47.Text = "Date From";
		this.ToNoTransactionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToNoTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToNoTransactionHistory.Location = new System.Drawing.Point(578, 16);
		this.ToNoTransactionHistory.Name = "ToNoTransactionHistory";
		this.ToNoTransactionHistory.Size = new System.Drawing.Size(72, 23);
		this.ToNoTransactionHistory.TabIndex = 3;
		this.ToNoTransactionHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToNoTransactionHistory_KeyPress);
		this.FromDateTransactionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateTransactionHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateTransactionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateTransactionHistory.Location = new System.Drawing.Point(66, 16);
		this.FromDateTransactionHistory.Name = "FromDateTransactionHistory";
		this.FromDateTransactionHistory.ShowCheckBox = true;
		this.FromDateTransactionHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateTransactionHistory.TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(533, 20);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(45, 15);
		this.label48.TabIndex = 173;
		this.label48.Text = "Doc To";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(398, 20);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(60, 15);
		this.label49.TabIndex = 173;
		this.label49.Text = "Doc From";
		this.FromNoTransactionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromNoTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromNoTransactionHistory.Location = new System.Drawing.Point(459, 16);
		this.FromNoTransactionHistory.Name = "FromNoTransactionHistory";
		this.FromNoTransactionHistory.Size = new System.Drawing.Size(72, 23);
		this.FromNoTransactionHistory.TabIndex = 2;
		this.FromNoTransactionHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromNoTransactionHistory_KeyPress);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel10.Controls.Add(this.lblLanguage);
		this.panel10.Controls.Add(this.label8);
		this.panel10.Controls.Add(this.toolStrip2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1070, 50);
		this.panel10.TabIndex = 0;
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(468, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 214;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.White;
		this.lblLanguage.Location = new System.Drawing.Point(399, 30);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(68, 17);
		this.lblLanguage.TabIndex = 215;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 28);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(376, 19);
		this.label8.TabIndex = 213;
		this.label8.Text = "Production Input_Output Transaction History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripSeparator1, this.BtnNewTransactionHistory, this.BtnRefreshTransactionHistory, this.btnSummeryReport, this.btn609ProductionIssuance, this.btnShortcutKeysTransactionHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnNewTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewTransactionHistory.ForeColor = System.Drawing.Color.Black;
		this.BtnNewTransactionHistory.Image = (System.Drawing.Image)resources.GetObject("BtnNewTransactionHistory.Image");
		this.BtnNewTransactionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewTransactionHistory.Name = "BtnNewTransactionHistory";
		this.BtnNewTransactionHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewTransactionHistory.Text = "&New";
		this.BtnNewTransactionHistory.Click += new System.EventHandler(BtnNewTransactionHistory_Click);
		this.BtnRefreshTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshTransactionHistory.Image = (System.Drawing.Image)resources.GetObject("BtnRefreshTransactionHistory.Image");
		this.BtnRefreshTransactionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshTransactionHistory.Name = "BtnRefreshTransactionHistory";
		this.BtnRefreshTransactionHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshTransactionHistory.Text = "Refresh";
		this.BtnRefreshTransactionHistory.Click += new System.EventHandler(BtnRefreshTransactionHistory_Click);
		this.btnSummeryReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSummeryReport.Image = (System.Drawing.Image)resources.GetObject("btnSummeryReport.Image");
		this.btnSummeryReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSummeryReport.Name = "btnSummeryReport";
		this.btnSummeryReport.Size = new System.Drawing.Size(165, 24);
		this.btnSummeryReport.Text = "608-SummeryReport";
		this.btnSummeryReport.ToolTipText = "This Report Will Show The Records of Selected \"Job Order\" Other Filters Will not apply On The Report";
		this.btnSummeryReport.Click += new System.EventHandler(btnSummeryReport_Click);
		this.btn609ProductionIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn609ProductionIssuance.Image = (System.Drawing.Image)resources.GetObject("btn609ProductionIssuance.Image");
		this.btn609ProductionIssuance.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn609ProductionIssuance.Name = "btn609ProductionIssuance";
		this.btn609ProductionIssuance.Size = new System.Drawing.Size(287, 24);
		this.btn609ProductionIssuance.Text = "609-ProductionIssuanceGrnWiseReport";
		this.btn609ProductionIssuance.ToolTipText = "This Report Will Show The Records of Selected \"Job Order\" Other Filters Will not apply On The Report";
		this.btn609ProductionIssuance.Click += new System.EventHandler(btn609ProductionIssuance_Click);
		this.btnShortcutKeysTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeysTransactionHistory.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeysTransactionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeysTransactionHistory.Name = "btnShortcutKeysTransactionHistory";
		this.btnShortcutKeysTransactionHistory.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeysTransactionHistory.Text = "ShortCut Keys";
		this.btnShortcutKeysTransactionHistory.Click += new System.EventHandler(btnShortcutKeysTransactionHistory_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(325, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1323, 27);
		this.miniToolStrip.TabIndex = 58;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 682);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionPartyProcessing";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductionPartyProcessing_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlMain.ResumeLayout(false);
		this.InPut.ResumeLayout(false);
		this.tabControlInput.ResumeLayout(false);
		this.tabInputForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel28.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferencePartyInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyWIPInput).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabInputHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputHistoryMain).EndInit();
		this.panel78.ResumeLayout(false);
		this.panel78.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistoryDetail).EndInit();
		this.panel59.ResumeLayout(false);
		this.panel59.PerformLayout();
		this.panel58.ResumeLayout(false);
		this.panel58.PerformLayout();
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.OutPut.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControlOutPut.ResumeLayout(false);
		this.tabOutputForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.tabOutputHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistoryMain).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistoryDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistoryOutPut).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.PM.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPM).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel22.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderPMHistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.toolStrip7.ResumeLayout(false);
		this.toolStrip7.PerformLayout();
		this.panel24.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPM).EndInit();
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPM).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdTransactionHistoryMain).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdTransactionhistoryDetail).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderTransactionHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
