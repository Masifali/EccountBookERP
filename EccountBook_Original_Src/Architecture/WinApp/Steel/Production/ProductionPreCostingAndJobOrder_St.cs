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
using Architecture.BLL.Production;
using Architecture.BLL.Steel.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.Model.Steel.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Production;

public class ProductionPreCostingAndJobOrder_St : Form
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

	private DataTable dtJobLot = new DataTable();

	private DataTable dtCropYear = new DataTable();

	private DataTable dtPlantdetail = new DataTable();

	private int updateDetailIndexoutput;

	private int updateDetailIndexOH;

	private int updateDetailIndexInput;

	private int updateDetailIndexPM;

	private int updatePlantDetailIndex;

	private DataTable table = new DataTable();

	private DataTable dtLabourAc = new DataTable();

	public Rightsobjects formright = new Rightsobjects();

	private int[] AryColWidth;

	private bool UpdateMode;

	private int RECID;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private List<InvProductionJobOrderOutput> RemovedOutputList = new List<InvProductionJobOrderOutput>();

	private List<InvProductionJobOrderInput> RemovedInputList = new List<InvProductionJobOrderInput>();

	private List<InvProductionJobOrderOverHeads> RemovedOverheadList = new List<InvProductionJobOrderOverHeads>();

	private List<InvProductionJobOrderPlant> RemovedPlantList = new List<InvProductionJobOrderPlant>();

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

	private Label label5;

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

	private Label label14;

	private Label label41;

	internal UltraCombo cmbJobLotOutput;

	private Label label42;

	internal UltraCombo cmbPackTypeInput;

	private ToolStripButton btnloadall;

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

	private RichTextBox txtProductionInstruction;

	private RichTextBox txtOtherInstruction;

	private Label label71;

	private RichTextBox txtDeliveryInstruction;

	private RichTextBox txtPlantInstruction;

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

	private Panel panel11;

	private Panel panel20;

	internal GridEX grdPackingMaterialHistory;

	private Panel panel21;

	private Label label83;

	private Panel panel13;

	internal GridEX grdOverheadHistory;

	private Panel panel19;

	private Label label82;

	internal GridEX grdOutPutHistory;

	private Panel panel5;

	private Label label81;

	internal GridEX grdInputHistory;

	private Panel panel12;

	private Label label80;

	private Label label77;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label84;

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

	private TabPage tabPage6;

	private ToolStripButton BtnGenerateInPutRates;

	public ProductionPreCostingAndJobOrder_St(UserAccount user)
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
			tabControl2.TabPages.Remove(tabPage6);
			panel13.Dock = DockStyle.Fill;
			dtViewAttachments.Columns.Add("AttchmentPath");
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
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
			dtPlannedOutput.Columns.Add("Id", typeof(int));
			dtPlannedOutput.Columns.Add("EntryTypeId", typeof(int));
			dtPlannedOutput.Columns.Add("EntryType", typeof(string));
			dtPlannedOutput.Columns.Add("WareHouseId", typeof(int));
			dtPlannedOutput.Columns.Add("WareHouseName", typeof(string));
			dtPlannedOutput.Columns.Add("JobLotId", typeof(int));
			dtPlannedOutput.Columns.Add("JobLotDescription", typeof(string));
			dtPlannedOutput.Columns.Add("ItemId", typeof(int));
			dtPlannedOutput.Columns.Add("ItemName", typeof(string));
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
			dtInputDetail.Columns.Add("WareHouseId", typeof(int));
			dtInputDetail.Columns.Add("WareHouse", typeof(string));
			dtInputDetail.Columns.Add("JobLotId", typeof(int));
			dtInputDetail.Columns.Add("JobLot", typeof(string));
			dtInputDetail.Columns.Add("ItemId", typeof(int));
			dtInputDetail.Columns.Add("ItemName", typeof(string));
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
			dtPlantdetail.Columns.Add("Id");
			dtPlantdetail.Columns.Add("PlantId");
			dtPlantdetail.Columns.Add("PlantName");
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, cmbPlantFeader, "Id", "Description", "Plant/Feader ", false);
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
			DataTable dtCoa = CommonServices.CoaAllocationGetForComboServiceBind();
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = InvProductionJobOrder.GetGenerateCode(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1511,
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
			DataTable dtItem = CommonServices.AllItemsBindForSteel();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, CmbWipItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			dtPlannedOutput.Rows.Clear();
			dtInputDetail.Rows.Clear();
			dtPlantdetail.Rows.Clear();
			RemovedOutputList.Clear();
			RemovedInputList.Clear();
			RemovedOverheadList.Clear();
			RemovedPlantList.Clear();
			dtOH.Rows.Clear();
			dtPM.Rows.Clear();
			txtPlanDate.Value = DateTime.Now;
			txtInvoiceReference.Text = string.Empty;
			txtOtherInstruction.Text = string.Empty;
			txtPlantInstruction.Text = string.Empty;
			txtProductionInstruction.Text = string.Empty;
			txtDeliveryInstruction.Text = string.Empty;
			((Control)(object)CmbWorkInProcess).Text = string.Empty;
			((Control)(object)CmbWipItem).Text = string.Empty;
			((Control)(object)CmbLotAc).Text = string.Empty;
			cmbProductionType.Value = 1;
			cmbStatus.Value = 1;
			cmbPlanType.Value = 1;
			grdInputDetail.ClearStructure();
			grdPlannedOutput.ClearStructure();
			grdOverHead.ClearStructure();
			grdPackingMaterial.ClearStructure();
			grdPlantdetail.ClearStructure();
			grdInputDetail.RetrieveStructure();
			grdPlannedOutput.RetrieveStructure();
			btnUpdate.Visible = false;
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemFill();
			JobLotBind();
			AccountFills();
			PlanTypeBind();
			PlantFeader();
			CropYear();
			WareHouseFill();
			ItemDetailFill();
			CmbPackingTypeFill();
			FilterTypeFill();
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
			grdInputHistory.ClearStructure();
			grdOutPutHistory.ClearStructure();
			grdOverheadHistory.ClearStructure();
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

	private void ProductionPreCostingAndJobOrder_St_FormClosing(object sender, FormClosingEventArgs e)
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
			MessageBox.Show("Mannual ReportNo Field Required");
			txtInvoiceReference.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProductionType).ActiveRow == null)
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
		if (((UltraGridBase)cmbPlanType).ActiveRow == null)
		{
			MessageBox.Show("Plan Type Field Required");
			cmbPlanType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Field Required");
			cmbStatus.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
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
			invProductionJobOrder.DocumentTypeId = 1511;
			invProductionJobOrder.DocDate = txtPlanDate.Value;
			invProductionJobOrder.DocNo = Conversion.ToInt((object)txtPlanCode.Text);
			invProductionJobOrder.PlanType = Conversion.ToString((object)((Control)(object)cmbPlanType).Text);
			invProductionJobOrder.ProductionJobOrderNo = Conversion.ToString((object)txtInvoiceReference.Text);
			invProductionJobOrder.ProductionType = Conversion.ToString((object)((Control)(object)cmbProductionType).Text);
			invProductionJobOrder.WorkInProccessAcId = Conversion.ToInt(CmbWorkInProcess.Value);
			invProductionJobOrder.WipItemId = Conversion.ToInt(CmbWipItem.Value);
			invProductionJobOrder.StartDate = txtstartDate.Value;
			invProductionJobOrder.EndDate = txtEndDate.Value;
			invProductionJobOrder.PlanStatus = Conversion.ToString((object)((Control)(object)cmbStatus).Text);
			invProductionJobOrder.PackingInstructions = txtPlantInstruction.Text;
			invProductionJobOrder.ProductionInsturction = txtProductionInstruction.Text;
			invProductionJobOrder.DeliveryInstructions = txtDeliveryInstruction.Text;
			invProductionJobOrder.OtherInstructions = txtOtherInstruction.Text;
			invProductionJobOrder.IsApproved = false;
			invProductionJobOrder.OrganizationId = UserAccount.OrganizationId;
			invProductionJobOrder.CompanyId = UserAccount.CompanyId;
			invProductionJobOrder.BranchesId = UserAccount.BranchesId;
			invProductionJobOrder.EntryUserId = UserAccount.ID;
			invProductionJobOrder.ModifyUserId = UserAccount.ID;
			invProductionJobOrder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invProductionJobOrder.FinishGoodRate = default(decimal);
			invProductionJobOrder.invProductionJobOrderOutput = new List<InvProductionJobOrderOutput>();
			invProductionJobOrder.invProductionJobOrderInput = new List<InvProductionJobOrderInput>();
			invProductionJobOrder.InvProductionJobOrderPlantslist = new List<InvProductionJobOrderPlant>();
			invProductionJobOrder.InvProductionJobOrderOverHeadsList = new List<InvProductionJobOrderOverHeads>();
			invProductionJobOrder.AttachmentsList = new List<DMSAttachments>();
			if (grdPlannedOutput.GetRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Record Not Found");
			}
			if (grdInputDetail.GetRows().Count() == 0)
			{
				throw new Exception("Input Grid Record Not Found");
			}
			if (RemovedOutputList.Count > 0)
			{
				for (int l = 0; l < RemovedOutputList.Count; l++)
				{
					invProductionJobOrder.invProductionJobOrderOutput.Add(RemovedOutputList[l]);
				}
			}
			GridEXRow[] rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvProductionJobOrderOutput jobOrderOutputdetail = new InvProductionJobOrderOutput();
				jobOrderOutputdetail.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				jobOrderOutputdetail.ActionTypeId = ((jobOrderOutputdetail.Id == 0) ? 1 : 2);
				jobOrderOutputdetail.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				jobOrderOutputdetail.InvPackingTypeId = Conversion.ToInt((object)r3.Cells["PackingTypeId"].Text);
				jobOrderOutputdetail.Qty = Conversion.ToDecimal((object)r3.Cells["QtyOuter"].Text);
				jobOrderOutputdetail.PackUomId = Conversion.ToInt((object)r3.Cells["OuterPackSizeId"].Text);
				jobOrderOutputdetail.NetWeight = Conversion.ToDecimal((object)r3.Cells["Weight"].Text);
				jobOrderOutputdetail.RemarksDetail = Conversion.ToString((object)r3.Cells["Remarks"].Text);
				if (r3.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show("WareHouse Field Required in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.WarehouseId = Conversion.ToInt((object)r3.Cells["WareHouseId"].Text);
				if (r3.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show("JobLot Field Required in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.JobLotId = Conversion.ToInt((object)r3.Cells["JobLotId"].Text);
				jobOrderOutputdetail.ItemRate = Conversion.ToDouble(r3.Cells["ItemRate"].Value);
				jobOrderOutputdetail.RateUomId = Conversion.ToInt(r3.Cells["RateUomId"].Value);
				jobOrderOutputdetail.ItemAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
				jobOrderOutputdetail.OHExpense = Conversion.ToDecimal(r3.Cells["OHExpense"].Value);
				jobOrderOutputdetail.PMExpense = Conversion.ToDecimal(r3.Cells["PMExpense"].Value);
				jobOrderOutputdetail.OHGeneralExpense = Conversion.ToDecimal(r3.Cells["OHGeneralExpense"].Value);
				jobOrderOutputdetail.PMGeneralExpense = Conversion.ToDecimal(r3.Cells["PMGeneralExpense"].Value);
				jobOrderOutputdetail.NetRate = Conversion.ToDecimal(r3.Cells["NetRate"].Value);
				jobOrderOutputdetail.NetAmount = Conversion.ToDecimal(r3.Cells["NetAmount"].Value);
				jobOrderOutputdetail.EntryTypeId = Conversion.ToInt(r3.Cells["EntryTypeId"].Value);
				invProductionJobOrder.invProductionJobOrderOutput.Add(jobOrderOutputdetail);
			}
			if (RemovedInputList.Count > 0)
			{
				for (int k = 0; k < RemovedInputList.Count; k++)
				{
					invProductionJobOrder.invProductionJobOrderInput.Add(RemovedInputList[k]);
				}
			}
			rows = grdInputDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvProductionJobOrderInput jobOrderInputdetail = new InvProductionJobOrderInput();
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0 || r2.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (Conversion.ToInt(r2.Cells["JobLotId"].Value) == 0 || r2.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show("JobLot Field Required in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				if (r2.Cells["Qty"].Text == string.Empty || r2.Cells["Qty"].Text == "0")
				{
					MessageBox.Show("Quantity Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.Qty = Conversion.ToDecimal((object)r2.Cells["Qty"].Text);
				if (r2.Cells["PackSizeId"].Text == string.Empty || Conversion.ToInt(r2.Cells["PackSizeId"].Value) == 0)
				{
					MessageBox.Show("PackSize Field Required in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.PackUomId = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
				if (r2.Cells["Weight"].Text == string.Empty || r2.Cells["Weight"].Text == "0")
				{
					MessageBox.Show("Weight Field Required in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.NetWeight = Conversion.ToDecimal((object)r2.Cells["Weight"].Text);
				if (Conversion.ToInt(r2.Cells["WareHouseId"].Value) == 0 || r2.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show("WareHouse Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) == 0 || r2.Cells["PackTypeId"].Text == string.Empty)
				{
					MessageBox.Show("Packing Type Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.PackingTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				jobOrderInputdetail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				jobOrderInputdetail.ActionTypeId = ((jobOrderInputdetail.Id == 0) ? 1 : 2);
				jobOrderInputdetail.ItemRate = Conversion.ToDecimal(r2.Cells["ItemRate"].Value);
				jobOrderInputdetail.RateUomId = Conversion.ToInt(r2.Cells["RateUomId"].Value);
				jobOrderInputdetail.ItemAmount = Conversion.ToDecimal(r2.Cells["ItemAmount"].Value);
				jobOrderInputdetail.RemarksDetail = Conversion.ToString((object)r2.Cells["Remarks"].Text);
				jobOrderInputdetail.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				jobOrderInputdetail.RefDocIdNo = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				jobOrderInputdetail.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				invProductionJobOrder.invProductionJobOrderInput.Add(jobOrderInputdetail);
			}
			if (RemovedOverheadList.Count > 0)
			{
				for (int j = 0; j < RemovedOverheadList.Count; j++)
				{
					invProductionJobOrder.InvProductionJobOrderOverHeadsList.Add(RemovedOverheadList[j]);
				}
			}
			if (grdOverHead.RowCount > 0)
			{
				rows = grdOverHead.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvProductionJobOrderOverHeads OverHeaddetail = new InvProductionJobOrderOverHeads();
					if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["ChartOfAccountId"].Value) > 0)
					{
						OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						OverHeaddetail.ActionTypeId = ((OverHeaddetail.Id == 0) ? 1 : 2);
						if (r.Cells["ChartOfAccountId"].Text == string.Empty || Conversion.ToInt(r.Cells["ChartOfAccountId"].Value) == 0)
						{
							MessageBox.Show("OverHead Account Field Require in Detail");
							return;
						}
						OverHeaddetail.CharOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
						if (Conversion.ToInt(r.Cells["BrandId"].Value) == 0 && Conversion.ToInt(r.Cells["BrandUomId"].Value) != 0)
						{
							MessageBox.Show("OutPut Item Field Require in Detail");
							return;
						}
						OverHeaddetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
						if (Conversion.ToInt(r.Cells["BrandUomId"].Value) == 0 && Conversion.ToInt(r.Cells["BrandId"].Value) != 0)
						{
							MessageBox.Show("OutPut UomField Require in Detail");
							return;
						}
						OverHeaddetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
						if ((r.Cells["ItemQty"].Text == string.Empty || Conversion.ToDouble(r.Cells["ItemQty"].Value) == 0.0) && (r.Cells["ItemRate"].Text != string.Empty || Conversion.ToDouble(r.Cells["ItemRate"].Value) != 0.0))
						{
							MessageBox.Show("ItemQty Required in Detail");
							return;
						}
						OverHeaddetail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						if ((r.Cells["ItemRate"].Text == string.Empty || Conversion.ToDouble(r.Cells["ItemRate"].Value) == 0.0) && (r.Cells["ItemQty"].Text != string.Empty || Conversion.ToDouble(r.Cells["ItemQty"].Value) != 0.0))
						{
							MessageBox.Show("ItemRate Required in Detail");
							return;
						}
						OverHeaddetail.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0)
						{
							MessageBox.Show("Amount Required in Detail");
							return;
						}
						OverHeaddetail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						OverHeaddetail.RemarksOH = Conversion.ToString(r.Cells["Remarks"].Value);
						invProductionJobOrder.InvProductionJobOrderOverHeadsList.Add(OverHeaddetail);
					}
				}
			}
			if (RemovedPlantList.Count > 0)
			{
				for (int i = 0; i < RemovedPlantList.Count; i++)
				{
					invProductionJobOrder.InvProductionJobOrderPlantslist.Add(RemovedPlantList[i]);
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
				plant.Id = Conversion.ToInt(item.Cells["Id"].Value);
				plant.ActionTypeId = ((plant.Id == 0) ? 1 : 2);
				plant.PlantId = Conversion.ToInt(item.Cells["PlantId"].Value);
				plant.RemarksDetail = Conversion.ToString(item.Cells["RemarksDetail"].Value);
				invProductionJobOrder.InvProductionJobOrderPlantslist.Add(plant);
			}
			invProductionJobOrder.AttachmentsList = new List<DMSAttachments>();
			invProductionJobOrder.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvProductionJobOrder>(invProductionJobOrder, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvProductionJobOrder.Save(invProductionJobOrder);
			if (RECID == 0)
			{
				MessageBox.Show("Save Successfully" + invProductionJobOrder.DocNo);
			}
			else
			{
				MessageBox.Show("Update Successfully" + invProductionJobOrder.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RECID, base.Name, 1511, 0);
				}
				UpdateMode = false;
			}
			Reset();
			if (chkPrint.Checked)
			{
				PrintJobOrder(success);
			}
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
			bindPackSizeInput(Conversion.ToInt(cmbItemInput.Value));
			cmbItemInput_TextChanged(null, null);
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
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(Conversion.ToInt((object)ItemId));
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, cmbpacksizeinput, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
				DDL.BindDDL(dtuomsch, CmbRateUomInPut, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
				((UltraGridBase)cmbpacksizeinput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbRateUomInPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (dtuomsch.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbpacksizeinput).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbpacksizeinput).Text = string.Empty;
				}
				if (dtuomsch.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUomInPut).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUomInPut).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)cmbpacksizeinput).Text = string.Empty;
				((Control)(object)CmbRateUomInPut).Text = string.Empty;
				((UltraGridBase)cmbpacksizeinput).DataSource = null;
				((UltraGridBase)CmbRateUomInPut).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetailInPut()
	{
		if (((UltraGridBase)cmbwarehouseinput).ActiveRow == null)
		{
			MessageBox.Show("Warehouse is Required");
			cmbwarehouseinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbjoblotinput).ActiveRow == null)
		{
			MessageBox.Show("Job Lot Field is Required");
			cmbjoblotinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemInput).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItemInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackTypeInput).ActiveRow == null)
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
		if (((UltraGridBase)cmbpacksizeinput).ActiveRow == null)
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
		if (Conversion.ToDouble((object)txtRateInPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemRate Field is Required");
			txtRateInPut.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmountInPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtAmountInPut.Focus();
			return false;
		}
		return true;
	}

	private void ResetDetailInPut()
	{
		try
		{
			cmbItemInput.Focus();
			((Control)(object)cmbItemInput).Text = string.Empty;
			((Control)(object)cmbjoblotinput).Text = string.Empty;
			((Control)(object)cmbpacksizeinput).Text = string.Empty;
			txtqtyinput.Text = string.Empty;
			txtweightinput.Text = string.Empty;
			((Control)(object)cmbwarehouseinput).Text = string.Empty;
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
		try
		{
			grdInputDetail.AllowDelete = (InheritableBoolean)2;
			grdInputDetail.AllowEdit = (InheritableBoolean)1;
			grdInputDetail.RootTable.Columns["Id"].Visible = false;
			grdInputDetail.RootTable.Columns["ItemId"].Visible = false;
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
			grdInputDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["PackType"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["BalanceQty"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["PackSizeId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["BalanceQty"].FormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["BalanceQty"].TotalFormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdInputDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInputDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputDetail.RootTable.Columns.Add("Delete");
			grdInputDetail.RootTable.Columns["Delete"].Caption = "X";
			grdInputDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputDetail.RootTable.Columns["Delete"].Width = 25;
			grdInputDetail.RootTable.Columns["Delete"].Position = 0;
			grdInputDetail.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grdInputDetail);
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
			if (FormValidationDetailInPut())
			{
				dtInputDetail.Rows.Add(0, Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt(cmbwarehouseinput.Value), Conversion.ToString((object)((Control)(object)cmbwarehouseinput).Text.Trim()), Conversion.ToInt(cmbjoblotinput.Value), Conversion.ToString((object)((Control)(object)cmbjoblotinput).Text.Trim()), Conversion.ToInt(cmbItemInput.Value), Conversion.ToString((object)((Control)(object)cmbItemInput).Text.Trim()), Conversion.ToInt(cmbPackTypeInput.Value), Conversion.ToString((object)((Control)(object)cmbPackTypeInput).Text.Trim()), Conversion.ToDouble((object)txtqtyinput.Text.Trim()), Conversion.ToDouble((object)txtweightinput.Text.Trim()), Conversion.ToDouble((object)txtqtyinput.Text.Trim()), Conversion.ToInt(cmbpacksizeinput.Value), Conversion.ToString((object)((Control)(object)cmbpacksizeinput).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)cmbpacksizeinput).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtweightinput.Text.Trim()), Conversion.ToDouble((object)txtRateInPut.Text.Trim()), Conversion.ToInt(CmbRateUomInPut.Value), Conversion.ToString((object)((Control)(object)CmbRateUomInPut).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbRateUomInPut).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtAmountInPut.Text.Trim()), Conversion.ToString((object)txtremarksinput.Text.Trim()));
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
				cmbwarehouseinput.Focus();
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
			if (FormValidationDetailInPut())
			{
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouseId"] = Conversion.ToInt(cmbwarehouseinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouse"] = Conversion.ToString((object)((Control)(object)cmbwarehouseinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["JobLotId"] = Conversion.ToInt(cmbjoblotinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbjoblotinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemId"] = Conversion.ToInt(cmbItemInput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItemInput).Text);
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
				if (e.Column.Key == "Qty")
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Uom = Conversion.ToDouble((object)item.Cells["PackSizeEquivalent"].Value.ToString());
					item.Cells["Weight"].Value = Qty * Uom;
					double num = Qty * Uom;
					if (Conversion.ToDouble((object)item.Cells["BalanceQty"].Value.ToString()) < Qty)
					{
						item.Cells["Qty"].Value = 0;
						item.Cells["Weight"].Value = 0;
						MessageBox.Show("Qty cannot greater than BalanceQty Please check!");
					}
					double Rate2 = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
					double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateUomEquivalent"].Value.ToString());
					double Amount2 = num / RateUOM2 * Rate2;
					item.Cells["ItemAmount"].Value = Amount2;
				}
				if (e.Column.Key == "Weight")
				{
					double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
					if (Conversion.ToDouble((object)item.Cells["BalanceWeight"].Value.ToString()) < Weight)
					{
						item.Cells["Weight"].Value = 0;
						MessageBox.Show("Weight cannot greater than BalanceWeight Please check!");
					}
					double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
					double RateUOM = Conversion.ToDouble((object)item.Cells["RateUomEquivalent"].Value.ToString());
					double Amount = Weight / RateUOM * Rate;
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
				else
				{
					txtweightinput.Text = "0";
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

	private void GetAvgRate()
	{
		try
		{
			double rate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(cmbItemInput.Value), Conversion.ToDateTime((object)txtPlanDate.Value), 1511, RECID, Conversion.ToInt(cmbjoblotinput.Value), 0, null, Conversion.ToInt(cmbwarehouseinput.Value), UserAccount.BranchesId);
			if (rate > 0.0)
			{
				txtRateInPut.Text = Conversion.ToDouble((object)(Conversion.ToDouble((object)rate) * 1.0)).ToString();
				DataRow foundRow = CommonServices.GetUomScheduleByItemId(Conversion.ToInt((object)Conversion.ToInt(cmbItemInput.Value))).AsEnumerable().FirstOrDefault((DataRow row) => row.Field<double>("Equivalent") == 1.0);
				int Rate = ((foundRow != null) ? Conversion.ToInt(foundRow["Id"]) : 0);
				CmbRateUomInPut.Value = Rate;
				((Control)(object)CmbRateUomInPut).Enabled = false;
			}
			else
			{
				txtRateInPut.Text = 0.ToString();
				((Control)(object)CmbRateUomInPut).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbItemInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetAvgRate();
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
			dtentryType.Rows.Add(1, "ManualRate");
			dtentryType.Rows.Add(2, "AutoRate");
			DDL.BindDDL(dtentryType, CmbEntryTypeOP, "Id", "name", "EntryType", ZeroIndex: false);
			((UltraGridBase)CmbEntryTypeOP).Rows[0].Activate();
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
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtOuterqty.Text = string.Empty;
			((Control)(object)cmbouteruom).Text = string.Empty;
			txtweight.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtItemRateOutPut.Text = string.Empty;
			((Control)(object)CmbRateUomOutPut).Text = string.Empty;
			txtItemAmountOutPut.Text = string.Empty;
			cmbItem.Focus();
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
		if (((UltraGridBase)CmbEntryTypeOP).ActiveRow == null)
		{
			MessageBox.Show("EntryType Field is Required");
			CmbEntryTypeOP.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseOutPut).ActiveRow == null)
		{
			MessageBox.Show("WareHouse is Required");
			cmbWareHouseOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLotOutput).ActiveRow == null)
		{
			MessageBox.Show("JobLot is Required");
			cmbJobLotOutput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("Item Pack Unit Field is Required");
			cmbPackingType.Focus();
			return false;
		}
		if (txtOuterqty.Text.Trim() == string.Empty || txtOuterqty.Text.Trim() == "0")
		{
			MessageBox.Show("Outer Qty Field is Required");
			txtOuterqty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbouteruom).ActiveRow == null)
		{
			MessageBox.Show("Outer UOM Field is Required");
			cmbouteruom.Focus();
			return false;
		}
		if (txtweight.Text.Trim() == string.Empty || txtweight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight is Required");
			txtweight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemRateOutPut.Text.Trim()) == 0.0 && ((Control)(object)CmbEntryTypeOP).Text != "AutoRate")
		{
			MessageBox.Show("ItemRate Field is Required");
			txtItemRateOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUomOutPut).ActiveRow == null)
		{
			MessageBox.Show("Item Rate Uom Field is Required");
			CmbRateUomOutPut.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()) == 0.0 && ((Control)(object)CmbEntryTypeOP).Text != "AutoRate")
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtItemAmountOutPut.Focus();
			return false;
		}
		return true;
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
				else
				{
					txtweight.Text = "0";
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
				dtPlannedOutput.Rows.Add(0, Conversion.ToInt(CmbEntryTypeOP.Value), Conversion.ToString((object)((Control)(object)CmbEntryTypeOP).Text), Conversion.ToInt(cmbWareHouseOutPut.Value), Conversion.ToString((object)((Control)(object)cmbWareHouseOutPut).Text.Trim()), Conversion.ToInt(cmbJobLotOutput.Value), Conversion.ToString((object)((Control)(object)cmbJobLotOutput).Text.Trim()), Conversion.ToInt(cmbItem.Value), Conversion.ToString((object)((Control)(object)cmbItem).Text.Trim()), Conversion.ToInt(cmbPackingType.Value), Conversion.ToString((object)((Control)(object)cmbPackingType).Text.Trim()), Conversion.ToDouble((object)txtOuterqty.Text.Trim()), Conversion.ToInt(cmbouteruom.Value), Conversion.ToString((object)((Control)(object)cmbouteruom).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)cmbouteruom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtItemRateOutPut.Text), Conversion.ToInt(CmbRateUomOutPut.Value), Conversion.ToString((object)((Control)(object)CmbRateUomOutPut).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()), 0, 0, 0, 0, Conversion.ToDouble((object)txtItemRateOutPut.Text), Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()), Conversion.ToString((object)txtremarks.Text.Trim()));
				grdPlannedOutput.DataSource = dtPlannedOutput;
				grdPlannedOutput.RetrieveStructure();
				PlannedOutputGridSetting();
				ResetDetailOutPut();
				CostingCalculations();
				InsertPMInOutPutGrid();
				InsertOhInOutPutGrid();
				CalculateNEtRateAndamount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlannedOutput_DoubleClick(object sender, EventArgs e)
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
				DocumentTypeId = 112,
				JobOrderId = RECID
			}) > 0)
			{
				throw new Exception("Record Not Update Because Record has exist in Production OutPut");
			}
			GridEXRow item = grdPlannedOutput.CurrentRow;
			if (item != null)
			{
				updateDetailIndexoutput = item.RowIndex;
				CmbEntryTypeOP.Value = item.Cells["EntryTypeId"].Value.ToString();
				cmbWareHouseOutPut.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbJobLotOutput.Value = item.Cells["JobLotId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
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
				dtPlannedOutput.Rows[updateDetailIndexoutput]["EquivalentRate"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells["Equivalent"].Value);
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
		try
		{
			grdPlannedOutput.AllowDelete = (InheritableBoolean)2;
			grdPlannedOutput.AllowEdit = (InheritableBoolean)2;
			grdPlannedOutput.RootTable.Columns["Id"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EntryTypeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["ItemId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["WareHouseId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["JobLotId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EquivalentPack"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EquivalentRate"].Visible = false;
			grdPlannedOutput.RootTable.Columns["RateUomId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdPlannedOutput.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPlannedOutput.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdPlannedOutput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["Weight"].FormatString = "#,#.##";
			grdPlannedOutput.RootTable.Columns["Weight"].TotalFormatString = "#,#.##";
			grdPlannedOutput.RootTable.Columns["QtyOuter"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].FormatString = "#,#.##";
			grdPlannedOutput.RootTable.Columns["QtyOuter"].TotalFormatString = "#,#.##";
			grdPlannedOutput.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["OHExpense"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["OHExpense"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["OHExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["OHExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["PMExpense"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["PMExpense"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["PMExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["PMExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			grdPlannedOutput.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPlannedOutput.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdPlannedOutput.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPlannedOutput.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPlannedOutput);
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
				if (Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 2)
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

	private void BindBrandItemsFromOutPutGrid()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdPlannedOutput.CurrentRow != null && (int)grdPlannedOutput.CurrentRow.RowType == 0 && grdPlannedOutput.CurrentRow.Cells.Count > 0)
			{
				DataTable dtBrandItem = new DataTable();
				dtBrandItem.Columns.Add("BrandItemId");
				dtBrandItem.Columns.Add("BrandItem");
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item in rows)
				{
					dtBrandItem.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Value);
				}
				if (dtBrandItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtBrandItem, CmbBrandNameOverHead, "BrandItemId", "BrandItem", "Brand Item", ZeroIndex: false);
					DDL.BindDDL(dtBrandItem, CmbBrandNamePackingMaterial, "BrandItemId", "BrandItem", "Brand Item", ZeroIndex: false);
					return;
				}
				((UltraGridBase)CmbBrandNameOverHead).DataSource = null;
				((Control)(object)CmbBrandNameOverHead).Text = string.Empty;
				((UltraGridBase)CmbBrandNamePackingMaterial).DataSource = null;
				((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
			}
			else
			{
				((UltraGridBase)CmbBrandNameOverHead).DataSource = null;
				((Control)(object)CmbBrandNameOverHead).Text = string.Empty;
				((UltraGridBase)CmbBrandNamePackingMaterial).DataSource = null;
				((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
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
		if (((UltraGridBase)cmbChartofAccountOverHead).ActiveRow == null)
		{
			MessageBox.Show("OverHead Account Field Required");
			cmbChartofAccountOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNameOverHead).ActiveRow != null && ((UltraGridBase)CmbBrandUomOverHead).ActiveRow == null)
		{
			MessageBox.Show("outPut ItemUOM Field Required");
			CmbBrandUomOverHead.Focus();
			return false;
		}
		if (txtExpAmountOverHead.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()) == 0.0)
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
				CostingCalculations();
				InsertOhInOutPutGrid();
				CalculateNEtRateAndamount();
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
				CommonServices.GridAutoAdjustment(AryColWidth, grdOverHead);
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
			grdOverHead.RootTable.Columns["BrandUomId"].Visible = false;
			grdOverHead.RootTable.Columns["BrandId"].Visible = false;
			grdOverHead.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["BrandUom"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Amount"].FormatString = "#,#";
			grdOverHead.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverHead.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].Width = 25;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grdOverHead);
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdOverHead.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvProductionJobOrderOverHeads OverHeaddetail = new InvProductionJobOrderOverHeads();
					OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					OverHeaddetail.CharOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
					OverHeaddetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					OverHeaddetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					OverHeaddetail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					OverHeaddetail.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					OverHeaddetail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					OverHeaddetail.RemarksOH = Conversion.ToString(r.Cells["Remarks"].Value);
					OverHeaddetail.ActionTypeId = 3;
					RemovedOverheadList.Add(OverHeaddetail);
					dtOH.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					dtOH.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdOverHead.UpdateData();
			CostingCalculations();
			InsertOhInOutPutGrid();
			CalculateNEtRateAndamount();
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
			if (Conversion.ToDouble((object)txtItemQtyOverHead.Text) > 0.0 && Conversion.ToDouble((object)txtItemRateOverHeads.Text) > 0.0)
			{
				double Amount = Conversion.ToDouble((object)txtItemQtyOverHead.Text) * Conversion.ToDouble((object)txtItemRateOverHeads.Text);
				txtExpAmountOverHead.Text = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtExpAmountOverHead.Text = "";
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
		if (((UltraGridBase)CmbWarehousePackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehousePackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Packing Material Item Field Required");
			cmbItemPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNamePackingMaterial).ActiveRow != null && ((UltraGridBase)CmbBrandUomPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("outPut ItemUOM Field Required");
			CmbBrandUomPackingMaterial.Focus();
			return false;
		}
		if (txtPackingMaterialRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Field Required");
			txtPackingMaterialRate.Focus();
			return false;
		}
		if (txtAmountPackingMaterial.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim()) == 0.0)
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
				CommonServices.GridAutoAdjustment(AryColWidth, grdPackingMaterial);
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
			grdPackingMaterial.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["BrandUom"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = "#,#";
			grdPackingMaterial.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterial.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["ItemRate"].FormatString = "#,#";
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 25;
			grdPackingMaterial.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterial.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPackingMaterial);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (!btnsave.Enabled || !btnsave.Visible)
				{
					MessageBox.Show("You Can not Delete Record In Update Mode");
					return;
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdPackingMaterial.CurrentRow;
				dtPM.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdPackingMaterial.UpdateData();
			InsertPMInOutPutGrid();
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			BindHistoryGrid(50);
		}
	}

	private void btnloadall_Click(object sender, EventArgs e)
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

	private void BindHistoryGrid(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1511,
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights,
				NoOfRecords = NoOfRecords
			};
			DataTable dtGetAll = new DataTable();
			dtGetAll = InvProductionJobOrder.FormHistory(val);
			if (dtGetAll.Rows.Count > 0)
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
				dt.Columns.Add("ProductionInsturction", typeof(string));
				dt.Columns.Add("OtherInstruction", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtGetAll.Rows.Count; i++)
				{
					dt.Rows.Add(dtGetAll.Rows[i]["Id"], Conversion.ToDateTime(dtGetAll.Rows[i]["DocDate"]).ToString("dd-MMM-yyy"), dtGetAll.Rows[i]["DocNo"], dtGetAll.Rows[i]["PlanType"], dtGetAll.Rows[i]["ProductionJobOrderNo"], dtGetAll.Rows[i]["PlanType"], Conversion.ToDateTime(dtGetAll.Rows[i]["StartDate"]).ToString("dd-MMM-yyy"), Conversion.ToDateTime(dtGetAll.Rows[i]["EndDate"]).ToString("dd-MMM-yyy"), dtGetAll.Rows[i]["PlanStatus"], dtGetAll.Rows[i]["PackingInstructions"], dtGetAll.Rows[i]["DeliveryInstructions"], dtGetAll.Rows[i]["ProductionInsturction"], dtGetAll.Rows[i]["OtherInstructions"], dtGetAll.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HgridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grdInputHistory.ClearStructure();
				grdOutPutHistory.ClearStructure();
				grdOverheadHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HgridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Key = "Print";
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			Reset();
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			dtPlannedOutput.Clear();
			dtInputDetail.Clear();
			grdPlannedOutput.ClearStructure();
			grdInputDetail.ClearStructure();
			InvProductionJobOrder head = new InvProductionJobOrder();
			RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			head = InvProductionJobOrder.GetByID(RECID);
			tabControl1.SelectedIndex = 0;
			head.Id = RECID;
			txtPlanDate.Value = head.DocDate;
			txtPlanCode.Text = Conversion.ToString((object)head.DocNo);
			CmbWorkInProcess.Value = Conversion.ToInt((object)head.WorkInProccessAcId);
			CmbWipItem.Value = Conversion.ToInt((object)head.WipItemId);
			((Control)(object)cmbPlanType).Text = Conversion.ToString((object)head.PlanType);
			txtInvoiceReference.Text = Conversion.ToString((object)head.ProductionJobOrderNo);
			((Control)(object)cmbProductionType).Text = Conversion.ToString((object)head.ProductionType);
			txtstartDate.Value = Conversion.ToDateTime((object)head.StartDate);
			txtEndDate.Value = Conversion.ToDateTime((object)head.EndDate);
			((Control)(object)cmbStatus).Text = Conversion.ToString((object)head.PlanStatus);
			txtProductionInstruction.Text = head.ProductionInsturction;
			txtDeliveryInstruction.Text = head.DeliveryInstructions;
			txtPlantInstruction.Text = head.PackingInstructions;
			txtOtherInstruction.Text = head.OtherInstructions;
			for (int l = 0; l < head.invProductionJobOrderInput.Count; l++)
			{
				dtInputDetail.Rows.Add(head.invProductionJobOrderInput[l].Id, head.invProductionJobOrderInput[l].RefDocumentTypeId, head.invProductionJobOrderInput[l].RefDocIdNo, head.invProductionJobOrderInput[l].RefDocSubIdNo, head.invProductionJobOrderInput[l].WarehouseId, head.invProductionJobOrderInput[l].WareHouseName, head.invProductionJobOrderInput[l].JobLotId, head.invProductionJobOrderInput[l].JobLotCode, head.invProductionJobOrderInput[l].ItemId, head.invProductionJobOrderInput[l].ItemName, head.invProductionJobOrderInput[l].PackingTypeId, head.invProductionJobOrderInput[l].PackingType, head.invProductionJobOrderInput[l].Qty, head.invProductionJobOrderInput[l].NetWeight, head.invProductionJobOrderInput[l].Qty, head.invProductionJobOrderInput[l].PackUomId, head.invProductionJobOrderInput[l].PackUom, head.invProductionJobOrderInput[l].PackUomEquivalent, head.invProductionJobOrderInput[l].NetWeight, head.invProductionJobOrderInput[l].ItemRate, head.invProductionJobOrderInput[l].RateUomId, head.invProductionJobOrderInput[l].RateUom, head.invProductionJobOrderInput[l].RateUomEquivalent, head.invProductionJobOrderInput[l].ItemAmount, head.invProductionJobOrderInput[l].RemarksDetail);
			}
			for (int k = 0; k < head.invProductionJobOrderOutput.Count; k++)
			{
				dtPlannedOutput.Rows.Add(head.invProductionJobOrderOutput[k].Id, head.invProductionJobOrderOutput[k].EntryTypeId, head.invProductionJobOrderOutput[k].EntryType, head.invProductionJobOrderOutput[k].WarehouseId, head.invProductionJobOrderOutput[k].WareHouseName, head.invProductionJobOrderOutput[k].JobLotId, head.invProductionJobOrderOutput[k].JobLotDescription, head.invProductionJobOrderOutput[k].ItemId, head.invProductionJobOrderOutput[k].ItemName, head.invProductionJobOrderOutput[k].InvPackingTypeId, head.invProductionJobOrderOutput[k].PackTypeDesc, head.invProductionJobOrderOutput[k].Qty, head.invProductionJobOrderOutput[k].PackUomId, head.invProductionJobOrderOutput[k].PackUomCode, head.invProductionJobOrderOutput[k].PackUomCodeEquivalent, head.invProductionJobOrderOutput[k].NetWeight, head.invProductionJobOrderOutput[k].ItemRate, head.invProductionJobOrderOutput[k].RateUomId, head.invProductionJobOrderOutput[k].RateUom, head.invProductionJobOrderOutput[k].Equivalent, head.invProductionJobOrderOutput[k].ItemAmount, Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].OHExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].PMExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].OHGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].PMGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].NetRate), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].NetAmount), head.invProductionJobOrderOutput[k].RemarksDetail);
			}
			for (int j = 0; j < head.InvProductionJobOrderOverHeadsList.Count; j++)
			{
				dtOH.Rows.Add(head.InvProductionJobOrderOverHeadsList[j].Id, head.InvProductionJobOrderOverHeadsList[j].CharOfAccountId, head.InvProductionJobOrderOverHeadsList[j].AccountTitle, head.InvProductionJobOrderOverHeadsList[j].BrandId, head.InvProductionJobOrderOverHeadsList[j].BrandName, head.InvProductionJobOrderOverHeadsList[j].BrandUomId, head.InvProductionJobOrderOverHeadsList[j].BrandUom, head.InvProductionJobOrderOverHeadsList[j].ItemQty, head.InvProductionJobOrderOverHeadsList[j].ItemRate, head.InvProductionJobOrderOverHeadsList[j].Amount, head.InvProductionJobOrderOverHeadsList[j].RemarksOH);
			}
			for (int i = 0; i < head.InvProductionJobOrderPlantslist.Count; i++)
			{
				dtPlantdetail.Rows.Add(head.InvProductionJobOrderPlantslist[i].Id, head.InvProductionJobOrderPlantslist[i].PlantId, head.InvProductionJobOrderPlantslist[i].PlantName, head.InvProductionJobOrderPlantslist[i].RemarksDetail);
			}
			UpdateMode = true;
			grdInputDetail.DataSource = dtInputDetail;
			grdInputDetail.RetrieveStructure();
			InputDetailGridSetting();
			grdPlannedOutput.DataSource = dtPlannedOutput;
			grdPlannedOutput.RetrieveStructure();
			PlannedOutputGridSetting();
			grdOverHead.DataSource = dtOH;
			grdOverHead.RetrieveStructure();
			grdDetailOverheadSettings();
			grdPackingMaterial.DataSource = dtPM;
			grdPackingMaterial.RetrieveStructure();
			grdDetailPackingMaterialSettings();
			grdPlantdetail.DataSource = dtPlantdetail;
			grdPlantdetail.RetrieveStructure();
			grdPlantdetailSetting();
			FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
			CostingCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				DataGridHistory_DoubleClick(null, null);
			}
			if (e.Column.Key == "Print")
			{
				PrintJobOrder(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || Conversion.ToInt((object)item.Cells.Count) <= 0)
			{
				return;
			}
			DataTable dtOutput = new DataTable();
			dtOutput.Columns.Add("EntryType", typeof(string));
			dtOutput.Columns.Add("WareHouseName", typeof(string));
			dtOutput.Columns.Add("JobLotDescription", typeof(string));
			dtOutput.Columns.Add("ItemName", typeof(string));
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
			dtinput.Columns.Add("WareHouse", typeof(string));
			dtinput.Columns.Add("JobLot", typeof(string));
			dtinput.Columns.Add("ItemName", typeof(string));
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
			InvProductionJobOrder head = new InvProductionJobOrder();
			int id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (id > 0)
			{
				head = InvProductionJobOrder.GetByID(id);
				for (int k = 0; k < head.invProductionJobOrderInput.Count; k++)
				{
					dtinput.Rows.Add(head.invProductionJobOrderInput[k].WareHouseName, head.invProductionJobOrderInput[k].JobLotCode, head.invProductionJobOrderInput[k].ItemName, head.invProductionJobOrderInput[k].PackingType, head.invProductionJobOrderInput[k].Qty, head.invProductionJobOrderInput[k].PackUom, head.invProductionJobOrderInput[k].NetWeight, head.invProductionJobOrderInput[k].ItemRate, head.invProductionJobOrderInput[k].RateUom, head.invProductionJobOrderInput[k].ItemAmount, head.invProductionJobOrderInput[k].RemarksDetail);
				}
				for (int j = 0; j < head.invProductionJobOrderOutput.Count; j++)
				{
					dtOutput.Rows.Add(head.invProductionJobOrderOutput[j].EntryType, head.invProductionJobOrderOutput[j].WareHouseName, head.invProductionJobOrderOutput[j].JobLotDescription, head.invProductionJobOrderOutput[j].ItemName, head.invProductionJobOrderOutput[j].PackTypeDesc, head.invProductionJobOrderOutput[j].Qty, head.invProductionJobOrderOutput[j].PackUomCode, head.invProductionJobOrderOutput[j].NetWeight, head.invProductionJobOrderOutput[j].ItemRate, head.invProductionJobOrderOutput[j].RateUom, head.invProductionJobOrderOutput[j].ItemAmount, Conversion.ToDecimal((object)head.invProductionJobOrderOutput[j].OHExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[j].PMExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[j].OHGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[j].PMGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[j].NetRate), Conversion.ToDouble((object)head.invProductionJobOrderOutput[j].NetAmount), head.invProductionJobOrderOutput[j].RemarksDetail);
				}
				for (int i = 0; i < head.InvProductionJobOrderOverHeadsList.Count; i++)
				{
					dtOverHead.Rows.Add(head.InvProductionJobOrderOverHeadsList[i].AccountTitle, head.InvProductionJobOrderOverHeadsList[i].BrandName, head.InvProductionJobOrderOverHeadsList[i].BrandUom, head.InvProductionJobOrderOverHeadsList[i].ItemQty, head.InvProductionJobOrderOverHeadsList[i].ItemRate, head.InvProductionJobOrderOverHeadsList[i].Amount, head.InvProductionJobOrderOverHeadsList[i].RemarksOH);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void GridInputHistorySetting()
	{
		try
		{
			grdInputHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInputHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInputHistory.RootTable.Columns["Qty"].FormatString = "0,0";
			grdInputHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInputHistory.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputHistory.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputHistory.RootTable.Columns["Weight"].FormatString = "0,0";
			grdInputHistory.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdInputHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdInputHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInputHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdInputHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustment(AryColWidth, grdInputHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridOutPutHistorySetting()
	{
		try
		{
			grdOutPutHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOutPutHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutHistory.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutHistory.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["Weight"].FormatString = "#,#.##";
			grdOutPutHistory.RootTable.Columns["Weight"].TotalFormatString = "#,#.##";
			grdOutPutHistory.RootTable.Columns["QtyOuter"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["QtyOuter"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["QtyOuter"].FormatString = "#,#.##";
			grdOutPutHistory.RootTable.Columns["QtyOuter"].TotalFormatString = "#,#.##";
			grdOutPutHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["OHExpense"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["OHExpense"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["OHExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["OHExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["PMExpense"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["PMExpense"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["PMExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["PMExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["OHGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["OHGeneralExpense"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["OHGeneralExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["OHGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["PMGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["PMGeneralExpense"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["PMGeneralExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["PMGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutHistory.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutHistory.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutHistory.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdOutPutHistory.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutHistory.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdOutPutHistory);
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
			grdOverheadHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverheadHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOverheadHistory.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdOverheadHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverheadHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverheadHistory.RootTable.Columns["BrandUom"].TextAlignment = (TextAlignment)3;
			grdOverheadHistory.RootTable.Columns["Amount"].FormatString = "#,#";
			grdOverheadHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverheadHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdOverheadHistory.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grdOverheadHistory);
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
			grdPackingMaterialHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialHistory.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdPackingMaterialHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialHistory.RootTable.Columns["BrandUom"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialHistory.RootTable.Columns["Amount"].FormatString = "#,#";
			grdPackingMaterialHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterialHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialHistory.RootTable.Columns["ItemRate"].FormatString = "#,#";
			CommonServices.GridAutoAdjustment(AryColWidth, grdPackingMaterialHistory);
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
			DataTable dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, cmbwarehouseinput, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
				DDL.BindDDL(dtWh, cmbWareHouseOutPut, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
				DDL.BindDDL(dtWh, CmbWarehousePackingMaterial, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
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

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
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
			dtJobLot = CommonServices.JobLotGetAllService();
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLot, cmbjoblotinput, "Id", "JobLotDescription", "JobLotDescription", false);
				DDL.BindDDLNew(dtJobLot, cmbJobLotOutput, "Id", "JobLotDescription", "JobLotDescription", false);
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
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(Conversion.ToInt((object)ItemId));
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, cmbouteruom, "Id", "UOMCode", "Outer Uom", ZeroIndex: false);
				DDL.BindDDL(dtuomsch, CmbRateUomOutPut, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
				((UltraGridBase)cmbouteruom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbRateUomOutPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
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
		try
		{
			dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackingType, cmbPackingType, "Id", "PackTypeDesc", "PackType", false);
				DDL.BindDDLNew(dtPackingType, cmbPackTypeInput, "Id", "PackTypeDesc", "PackType", false);
				((UltraGridBase)cmbPackingType).Rows[0].Activate();
				((UltraGridBase)cmbPackTypeInput).Rows[0].Activate();
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
				((UltraGridBase)cmbPlanType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionTypeBind()
	{
		try
		{
			DataTable dtprot = new DataTable();
			dtprot.Columns.Add("Id");
			dtprot.Columns.Add("ProductionType");
			dtprot.Rows.Add(1, "Husking");
			dtprot.Rows.Add(2, "Processing");
			dtprot.Rows.Add(3, "Other");
			if (dtprot.Rows.Count > 0)
			{
				DDL.BindDDL(dtprot, cmbProductionType, "Id", "ProductionType", "Production Type ", ZeroIndex: false);
				((UltraGridBase)cmbProductionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
		try
		{
			dtLabourAc = CommonServices.GetWagesAccount(null, 0, 1);
			if (dtLabourAc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLabourAc, CmbLabourItemInPut, "Id", "WagesAccountName", "Labour Item", false);
				DDL.BindDDLNew(dtLabourAc, CmbLabourItemOutPut, "Id", "WagesAccountName", "Labour Item", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialItemBind()
	{
		try
		{
			DataTable dt = CommonServices.GetItemByItemTypeId("14");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPackingMaterial, "Id", "ItemName", "Packing Item", false);
				DDL.BindDDLNew(dt, cmbItemPackingMaterial, "Id", "ItemName", "Packing Item", false);
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
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			else
			{
				btnnew_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
			}
			else
			{
				tabControl1.SelectedIndex = 1;
			}
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.U && UpdateMode)
		{
			btnUpdate_Click(null, null);
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
			if (RECID > 0)
			{
				PrintJobOrder(RECID);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintJobOrder(int PrintId)
	{
		try
		{
			CommonServices.ProductionJobOrderSlipSteel1563(PrintId);
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
					dtInputDetail.Rows.Add(0, Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["WareHouseId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["WareHouse"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["JobLotId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["JobLotCode"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["ItemName"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["PackingTypeId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackTypeCode"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemUOMId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackUom"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemUOM"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemRate"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RateUOMId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RateUOM"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Equivalent"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["ItemAmount"]), Conversion.ToString((object)""));
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
					BindBrandItemsFromOutPutGrid();
					ChartOfAccountOverHead();
				}
				else if (tabControl2.SelectedIndex == 3)
				{
					BindBrandItemsFromOutPutGrid();
				}
				else if (tabControl2.SelectedIndex == 4)
				{
					PlantFeader();
					cmbPlantFeader.Focus();
				}
			}
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
						item.Cells["ItemRate"].Value = RateUom * AvgRateWithoutExp;
						item.Cells["ItemAmount"].Value = Weight / RateUom * Conversion.ToDouble(item.Cells["ItemRate"].Value);
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
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				Conversion.ToDouble(obj.Cells["ItemRate"].Value);
				double ItemAmount = Conversion.ToDouble(obj.Cells["ItemAmount"].Value);
				double ItemWeight = Conversion.ToDouble(obj.Cells["Weight"].Value);
				double RateUom = Conversion.ToDouble(obj.Cells["EquivalentRate"].Value);
				double OHExpenseItem = Conversion.ToDouble(obj.Cells["OHExpense"].Value);
				double PMExpenseItem = Conversion.ToDouble(obj.Cells["PMExpense"].Value);
				double OHExpenseItemGeneral = Conversion.ToDouble(obj.Cells["OHGeneralExpense"].Value);
				double PMExpenseItemGeneral = Conversion.ToDouble(obj.Cells["PMGeneralExpense"].Value);
				NetAmount = ItemAmount + OHExpenseItem + OHExpenseItemGeneral + PMExpenseItem + PMExpenseItemGeneral;
				NetRate = NetAmount / ItemWeight * RateUom;
				obj.Cells["NetAmount"].Value = NetAmount;
				obj.Cells["NetRate"].Value = NetRate;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlannedOutput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdPlannedOutput.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvProductionJobOrderOutput jobOrderOutputdetail = new InvProductionJobOrderOutput();
					jobOrderOutputdetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					jobOrderOutputdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					jobOrderOutputdetail.InvPackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					jobOrderOutputdetail.Qty = Conversion.ToDecimal((object)r.Cells["QtyOuter"].Text);
					jobOrderOutputdetail.PackUomId = Conversion.ToInt((object)r.Cells["OuterPackSizeId"].Text);
					jobOrderOutputdetail.NetWeight = Conversion.ToDecimal((object)r.Cells["Weight"].Text);
					jobOrderOutputdetail.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
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
					RemovedOutputList.Add(jobOrderOutputdetail);
					dtPlannedOutput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					dtPlannedOutput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdPlannedOutput.UpdateData();
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

	private void grdInputDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdInputDetail.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvProductionJobOrderInput jobOrderInputdetail = new InvProductionJobOrderInput();
					jobOrderInputdetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					jobOrderInputdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					jobOrderInputdetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					jobOrderInputdetail.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					jobOrderInputdetail.PackUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					jobOrderInputdetail.NetWeight = Conversion.ToDecimal((object)r.Cells["Weight"].Text);
					jobOrderInputdetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					jobOrderInputdetail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					jobOrderInputdetail.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					jobOrderInputdetail.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					jobOrderInputdetail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					jobOrderInputdetail.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
					jobOrderInputdetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					jobOrderInputdetail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					jobOrderInputdetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					jobOrderInputdetail.ActionTypeId = 3;
					RemovedInputList.Add(jobOrderInputdetail);
					dtInputDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					dtInputDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdInputDetail.UpdateData();
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
			dtPlantdetail.Rows.Add(0, cmbPlantFeader.Value, ((Control)(object)cmbPlantFeader).Text.Trim(), txtRemarksPlantDetail.Text.Trim());
			grdPlantdetail.DataSource = dtPlantdetail;
			grdPlantdetail.RetrieveStructure();
			grdPlantdetailSetting();
			((Control)(object)cmbPlantFeader).Text = string.Empty;
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
			dtPlantdetail.Rows[updatePlantDetailIndex]["RemarksDetail"] = txtRemarksPlantDetail.Text.Trim();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)cmbPlantFeader).Text = string.Empty;
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
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPlantdetail.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
				else
				{
					if (InvProductionJobOrder.CheckPlantExistinProductionagaintJobOrder(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						JobOrderId = RECID,
						PlantId = Conversion.ToInt(item.Cells["PlantId"].Value)
					}))
					{
						throw new Exception("You cannot be delete " + Conversion.ToString(item.Cells["PlantName"].Value) + " because " + Conversion.ToString(item.Cells["PlantName"].Value) + " Has been refered to production");
					}
					InvProductionJobOrderPlant plant = new InvProductionJobOrderPlant();
					plant.Id = Conversion.ToInt(item.Cells["Id"].Value);
					plant.ActionTypeId = 3;
					plant.PlantId = Conversion.ToInt(item.Cells["PlantId"].Value);
					plant.RemarksDetail = Conversion.ToString(item.Cells["RemarksDetail"].Value);
					RemovedPlantList.Add(plant);
					dtPlantdetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
			}
			grdPlantdetail.UpdateData();
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

	private void cmbwarehouseinput_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbjoblotinput_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvgRate();
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
				int WareHouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				r.BeginEdit();
				double rate = CommonServices.AvgRateOnlyForCGS(ItemId, DocDate, 401, RECID, JobLotId, 0, null, WareHouseId, UserAccount.BranchesId);
				if (rate > 0.0)
				{
					double Rateuom = 1.0;
					double Rate = 0.0;
					if (clsGlobalVariables.globalUomSchedule != null && clsGlobalVariables.globalUomSchedule.Count > 0)
					{
						GloablUomSchedule Row = clsGlobalVariables.globalUomSchedule.AsEnumerable().FirstOrDefault((GloablUomSchedule d) => d.Equivalent == 1.0 && d.ItemId == ItemId);
						if (Row == null)
						{
							Rateuom = 0.0;
							r.Cells["RateUomId"].Value = 0;
							r.Cells["RateUom"].Value = "";
							r.Cells["RateUomEquivalent"].Value = Rateuom;
						}
						if (Row != null)
						{
							Rateuom = Row.Equivalent;
							r.Cells["RateUomId"].Value = Row.Id;
							r.Cells["RateUom"].Value = Row.UOMCode;
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
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df5: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_107f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1089: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Expected O, but got Unknown
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1186: Expected O, but got Unknown
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Expected O, but got Unknown
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Expected O, but got Unknown
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Expected O, but got Unknown
		//IL_1263: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Expected O, but got Unknown
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_1283: Expected O, but got Unknown
		//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Expected O, but got Unknown
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1349: Expected O, but got Unknown
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Expected O, but got Unknown
		//IL_1397: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a1: Expected O, but got Unknown
		//IL_13a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ac: Expected O, but got Unknown
		//IL_147e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1488: Expected O, but got Unknown
		//IL_149f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a9: Expected O, but got Unknown
		//IL_14b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bf: Expected O, but got Unknown
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ca: Expected O, but got Unknown
		//IL_14d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e0: Expected O, but got Unknown
		//IL_16af: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b9: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_1728: Unknown result type (might be due to invalid IL or missing references)
		//IL_1732: Expected O, but got Unknown
		//IL_1733: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Expected O, but got Unknown
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Expected O, but got Unknown
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1824: Expected O, but got Unknown
		//IL_183b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1845: Expected O, but got Unknown
		//IL_185c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1866: Expected O, but got Unknown
		//IL_187d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1887: Expected O, but got Unknown
		//IL_2374: Unknown result type (might be due to invalid IL or missing references)
		//IL_237e: Expected O, but got Unknown
		//IL_238b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2395: Expected O, but got Unknown
		//IL_512f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5139: Expected O, but got Unknown
		//IL_5146: Unknown result type (might be due to invalid IL or missing references)
		//IL_5150: Expected O, but got Unknown
		//IL_7a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a18: Expected O, but got Unknown
		//IL_9466: Unknown result type (might be due to invalid IL or missing references)
		//IL_9470: Expected O, but got Unknown
		//IL_b393: Unknown result type (might be due to invalid IL or missing references)
		//IL_b39d: Expected O, but got Unknown
		//IL_138c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_138d3: Expected O, but got Unknown
		//IL_138f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13901: Expected O, but got Unknown
		GridEXLayout grdPlannedOutput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
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
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
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
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
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
		Appearance appearance338 = new Appearance();
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
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		GridEXLayout grdPlantdetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
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
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
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
		Appearance appearance231 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
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
		Appearance appearance260 = new Appearance();
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
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
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
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Production.ProductionPreCostingAndJobOrder_St));
		GridEXLayout grdPackingMaterialHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverheadHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		this.txtItemRateOutPut = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.CmbRateUomOutPut = new UltraCombo();
		this.cmbWareHouseOutPut = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
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
		this.label34 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
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
		this.btnAddPlantDetail = new System.Windows.Forms.Button();
		this.btnCanclePlantdetail = new System.Windows.Forms.Button();
		this.btnUpdatePlantDetail = new System.Windows.Forms.Button();
		this.txtRemarksPlantDetail = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.cmbPlantFeader = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.btnProductionPlant = new System.Windows.Forms.Button();
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
		this.label84 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.txtProductionInstruction = new System.Windows.Forms.RichTextBox();
		this.txtOtherInstruction = new System.Windows.Forms.RichTextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.txtDeliveryInstruction = new System.Windows.Forms.RichTextBox();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.txtPlantInstruction = new System.Windows.Forms.RichTextBox();
		this.txtPlanDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.txtPlanCode = new System.Windows.Forms.TextBox();
		this.txtInvoiceReference = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbProductionType = new UltraCombo();
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
		this.label5 = new System.Windows.Forms.Label();
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
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnLoadInvoices = new System.Windows.Forms.ToolStripButton();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdPackingMaterialHistory = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label83 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdOverheadHistory = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label82 = new System.Windows.Forms.Label();
		this.grdOutPutHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.grdInputHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label80 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label77 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnloadall = new System.Windows.Forms.ToolStripButton();
		this.BtnGenerateInPutRates = new System.Windows.Forms.ToolStripButton();
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
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).BeginInit();
		this.groupBox2.SuspendLayout();
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
		this.panel11.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverheadHistory).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutHistory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1064, 733);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1064, 733);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1056, 707);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1050, 701);
		this.panel2.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.tabControl2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 411);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1050, 290);
		this.panel6.TabIndex = 61;
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Controls.Add(this.tabPage7);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.ShowToolTips = true;
		this.tabControl2.Size = new System.Drawing.Size(1050, 290);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel14);
		this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage3.Location = new System.Drawing.Point(4, 29);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1042, 257);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Planned Output Requirnment";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlannedOutput);
		this.panel14.Controls.Add(this.panel9);
		this.panel14.Controls.Add(this.paneloutputgrd);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1036, 251);
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
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Size = new System.Drawing.Size(1036, 118);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).TabIndex = 1;
		this.grdPlannedOutput.TotalRow = (InheritableBoolean)1;
		this.grdPlannedOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlannedOutput).VisualStyle = (VisualStyle)4;
		this.grdPlannedOutput.CellUpdated += new ColumnActionEventHandler(grdPlannedOutput_CellUpdated);
		this.grdPlannedOutput.ColumnButtonClick += new ColumnActionEventHandler(grdPlannedOutput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).DoubleClick += new System.EventHandler(grdPlannedOutput_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 110);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1036, 23);
		this.panel9.TabIndex = 206;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(991, -1);
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
		this.paneloutputgrd.Controls.Add(this.txtItemRateOutPut);
		this.paneloutputgrd.Controls.Add(this.label44);
		this.paneloutputgrd.Controls.Add(this.label46);
		this.paneloutputgrd.Controls.Add(this.label47);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut);
		this.paneloutputgrd.Controls.Add(this.label14);
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
		this.paneloutputgrd.Size = new System.Drawing.Size(1036, 110);
		this.paneloutputgrd.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(4, 7);
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
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance354).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance354;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance365).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance365;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance380).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance386;
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
		this.txtItemAmountOutPut.Location = new System.Drawing.Point(245, 79);
		this.txtItemAmountOutPut.Name = "txtItemAmountOutPut";
		this.txtItemAmountOutPut.ReadOnly = true;
		this.txtItemAmountOutPut.Size = new System.Drawing.Size(89, 23);
		this.txtItemAmountOutPut.TabIndex = 13;
		this.txtItemRateOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemRateOutPut.Location = new System.Drawing.Point(100, 79);
		this.txtItemRateOutPut.Name = "txtItemRateOutPut";
		this.txtItemRateOutPut.Size = new System.Drawing.Size(70, 23);
		this.txtItemRateOutPut.TabIndex = 11;
		this.txtItemRateOutPut.TextChanged += new System.EventHandler(txtItemRateOutPut_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(245, 57);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 211;
		this.label44.Text = "Amount";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(100, 57);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(30, 15);
		this.label46.TabIndex = 212;
		this.label46.Text = "Rate";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(172, 57);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(59, 15);
		this.label47.TabIndex = 210;
		this.label47.Text = "Rate Uom";
		this.CmbRateUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Location = new System.Drawing.Point(172, 77);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Name = "CmbRateUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Size = new System.Drawing.Size(71, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TextChanged += new System.EventHandler(CmbRateUomOutPut_TextChanged);
		this.cmbWareHouseOutPut.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance186).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Location = new System.Drawing.Point(114, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Name = "cmbWareHouseOutPut";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(114, 7);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 206;
		this.label14.Text = "WareHouse";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(299, 7);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(48, 15);
		this.label41.TabIndex = 205;
		this.label41.Text = "Job/Lot";
		this.cmbJobLotOutput.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance355).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance355).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance355;
		((AppearanceBase)appearance356).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance356;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance357).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance357).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance357).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance358).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance358;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance360;
		((AppearanceBase)appearance361).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance361).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance362).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance362).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance362).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance362;
		((AppearanceBase)appearance363).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLotOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Location = new System.Drawing.Point(299, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Name = "cmbJobLotOutput";
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).TabIndex = 2;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance367).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance368).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance368).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance368;
		((AppearanceBase)appearance369).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance369;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance370).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance370).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance370).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance371).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance371;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance372).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance373;
		((AppearanceBase)appearance374).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance374).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance375).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance375).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance375).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance375;
		((AppearanceBase)appearance377).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance378).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance378;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(474, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(475, 7);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 178;
		this.label37.Text = "Item Name";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(951, 7);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(68, 15);
		this.label25.TabIndex = 180;
		this.label25.Text = "Outer UOM";
		this.cmbouteruom.AllowNull = (DefaultableBoolean)1;
		this.cmbouteruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbouteruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbouteruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance381).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance381;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance382).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance382).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance383;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance384).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance384).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance384).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance385).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance385;
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance387).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance388;
		((AppearanceBase)appearance389).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance389).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance389;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance390).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance390).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance390).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance390;
		((AppearanceBase)appearance391).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance392).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance392;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbouteruom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Location = new System.Drawing.Point(951, 26);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Name = "cmbouteruom";
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).TabIndex = 9;
		this.cmbouteruom.ValueChanged += new System.EventHandler(cmbouteruom_ValueChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(766, 7);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 184;
		this.label35.Text = "PackingType";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance394).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance395).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance395).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance395).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance396;
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
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(765, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 5;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(877, 7);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(59, 15);
		this.label26.TabIndex = 186;
		this.label26.Text = "Outer Qty";
		this.txtOuterqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOuterqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOuterqty.Location = new System.Drawing.Point(877, 28);
		this.txtOuterqty.Name = "txtOuterqty";
		this.txtOuterqty.Size = new System.Drawing.Size(72, 23);
		this.txtOuterqty.TabIndex = 8;
		this.txtOuterqty.TextChanged += new System.EventHandler(txtOuterqty_TextChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(4, 57);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 188;
		this.label39.Text = "Weight";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(4, 79);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(95, 23);
		this.txtweight.TabIndex = 10;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(334, 57);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 188;
		this.label27.Text = "Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(335, 79);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(540, 23);
		this.txtremarks.TabIndex = 14;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(876, 77);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(28, 26);
		this.btnplus.TabIndex = 15;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(879, 77);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(949, 77);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.tabPage4.Controls.Add(this.panel15);
		this.tabPage4.Location = new System.Drawing.Point(4, 29);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1042, 257);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Input Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputDetail);
		this.panel15.Controls.Add(this.panel7);
		this.panel15.Controls.Add(this.panelinputgrd);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1036, 251);
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
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1036, 118);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 1;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		this.grdInputDetail.CellUpdated += new ColumnActionEventHandler(grdInputDetail_CellUpdated);
		this.grdInputDetail.ColumnButtonClick += new ColumnActionEventHandler(grdInputDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).DoubleClick += new System.EventHandler(grdInputDetail_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 110);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1036, 23);
		this.panel7.TabIndex = 207;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(992, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdInputDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panelinputgrd.BackColor = System.Drawing.Color.White;
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
		this.panelinputgrd.Controls.Add(this.label34);
		this.panelinputgrd.Controls.Add(this.label9);
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
		this.panelinputgrd.Size = new System.Drawing.Size(1036, 110);
		this.panelinputgrd.TabIndex = 0;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(671, 9);
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
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Location = new System.Drawing.Point(671, 28);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Name = "cmbPackTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).TabIndex = 4;
		this.txtAmountInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmountInPut.Location = new System.Drawing.Point(268, 79);
		this.txtAmountInPut.Name = "txtAmountInPut";
		this.txtAmountInPut.ReadOnly = true;
		this.txtAmountInPut.Size = new System.Drawing.Size(101, 23);
		this.txtAmountInPut.TabIndex = 10;
		this.txtRateInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateInPut.Location = new System.Drawing.Point(104, 79);
		this.txtRateInPut.Name = "txtRateInPut";
		this.txtRateInPut.ReadOnly = true;
		this.txtRateInPut.Size = new System.Drawing.Size(78, 23);
		this.txtRateInPut.TabIndex = 8;
		this.txtRateInPut.TextChanged += new System.EventHandler(txtRateInPut_TextChanged);
		this.txtweightinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweightinput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweightinput.Location = new System.Drawing.Point(7, 79);
		this.txtweightinput.Name = "txtweightinput";
		this.txtweightinput.Size = new System.Drawing.Size(96, 23);
		this.txtweightinput.TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(436, 9);
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
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(436, 28);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TextChanged += new System.EventHandler(cmbItemInput_TextChanged);
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
		this.txtremarksinput.Location = new System.Drawing.Point(370, 79);
		this.txtremarksinput.Name = "txtremarksinput";
		this.txtremarksinput.Size = new System.Drawing.Size(479, 23);
		this.txtremarksinput.TabIndex = 11;
		this.btnupdategrdinput.BackColor = System.Drawing.Color.Teal;
		this.btnupdategrdinput.FlatAppearance.BorderSize = 0;
		this.btnupdategrdinput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnupdategrdinput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdategrdinput.ForeColor = System.Drawing.Color.White;
		this.btnupdategrdinput.Location = new System.Drawing.Point(852, 77);
		this.btnupdategrdinput.Name = "btnupdategrdinput";
		this.btnupdategrdinput.Size = new System.Drawing.Size(68, 26);
		this.btnupdategrdinput.TabIndex = 13;
		this.btnupdategrdinput.Text = "Update";
		this.btnupdategrdinput.UseVisualStyleBackColor = false;
		this.btnupdategrdinput.Visible = false;
		this.btnupdategrdinput.Click += new System.EventHandler(btnupdategrdinput_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(373, 60);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(52, 15);
		this.label36.TabIndex = 202;
		this.label36.Text = "Remarks";
		this.btncancelgrdinput.BackColor = System.Drawing.Color.Teal;
		this.btncancelgrdinput.FlatAppearance.BorderSize = 0;
		this.btncancelgrdinput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btncancelgrdinput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btncancelgrdinput.ForeColor = System.Drawing.Color.White;
		this.btncancelgrdinput.Location = new System.Drawing.Point(922, 77);
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
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
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
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(8, 9);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(69, 15);
		this.label34.TabIndex = 200;
		this.label34.Text = "WareHouse";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(268, 60);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(51, 15);
		this.label9.TabIndex = 198;
		this.label9.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(104, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 15);
		this.label3.TabIndex = 198;
		this.label3.Text = "Rate / kg";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(202, 9);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 192;
		this.label45.Text = "Job/Lot";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(7, 60);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 198;
		this.label33.Text = "Weight";
		this.cmbjoblotinput.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblotinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblotinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblotinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbjoblotinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Location = new System.Drawing.Point(200, 28);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Name = "cmbjoblotinput";
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Leave += new System.EventHandler(cmbjoblotinput_Leave);
		this.txtqtyinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqtyinput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqtyinput.Location = new System.Drawing.Point(797, 30);
		this.txtqtyinput.Name = "txtqtyinput";
		this.txtqtyinput.Size = new System.Drawing.Size(101, 23);
		this.txtqtyinput.TabIndex = 5;
		this.txtqtyinput.TextChanged += new System.EventHandler(txtqtyinput_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(183, 60);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(59, 15);
		this.label19.TabIndex = 195;
		this.label19.Text = "Rate Uom";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(899, 10);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(61, 15);
		this.label32.TabIndex = 195;
		this.label32.Text = "Pack Uom";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(797, 9);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(26, 15);
		this.label31.TabIndex = 196;
		this.label31.Text = "Qty";
		this.CmbRateUomInPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomInPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomInPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomInPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUomInPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Location = new System.Drawing.Point(183, 77);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Name = "CmbRateUomInPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).TabIndex = 9;
		this.CmbRateUomInPut.ValueChanged += new System.EventHandler(cmbpacksizeinput_ValueChanged);
		this.cmbpacksizeinput.AllowNull = (DefaultableBoolean)1;
		this.cmbpacksizeinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpacksizeinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpacksizeinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpacksizeinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Location = new System.Drawing.Point(899, 27);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Name = "cmbpacksizeinput";
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).TabIndex = 6;
		this.cmbpacksizeinput.ValueChanged += new System.EventHandler(cmbpacksizeinput_ValueChanged);
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.tabPage5.Controls.Add(this.panel17);
		this.tabPage5.Controls.Add(this.panel10);
		this.tabPage5.Location = new System.Drawing.Point(4, 29);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1042, 257);
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
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(1042, 117);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 0;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).DoubleClick += new System.EventHandler(grdOverHead_DoubleClick);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar2);
		this.panel17.Controls.Add(this.label67);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 120);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1042, 20);
		this.panel17.TabIndex = 13;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1002, -1);
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
		this.panel10.Size = new System.Drawing.Size(1042, 120);
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
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
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
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
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
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
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
		this.tabPage6.Size = new System.Drawing.Size(1042, 257);
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
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(1042, 114);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 14;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).DoubleClick += new System.EventHandler(grdPackingMaterial_DoubleClick);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar4);
		this.panel18.Controls.Add(this.label68);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 123);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1042, 20);
		this.panel18.TabIndex = 13;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(999, -2);
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
		this.panel16.Size = new System.Drawing.Size(1042, 123);
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
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance307;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance312).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance313).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance313).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance313).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance314).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance316;
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
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance319).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance319;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance320).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance320).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance321;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance324).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance325).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance325).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance325).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance325).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance326).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance328;
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
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance330).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance330).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance331;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance332).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance333;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance336).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance337).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance338;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance339).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance340;
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
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance342).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance342).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance343;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance344).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance344).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance344).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance345).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance345;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance348).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance349).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance349).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance349).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance349).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance349;
		((AppearanceBase)appearance350).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance350;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance351).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance352;
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
		this.tabPage7.Size = new System.Drawing.Size(1042, 257);
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
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Size = new System.Drawing.Size(1042, 160);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).TabIndex = 0;
		this.grdPlantdetail.TotalRow = (InheritableBoolean)1;
		this.grdPlantdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlantdetail).VisualStyle = (VisualStyle)4;
		this.grdPlantdetail.ColumnButtonClick += new ColumnActionEventHandler(grdPlantdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).DoubleClick += new System.EventHandler(grdPlantdetail_DoubleClick);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.ctrlGrdBar6);
		this.panel22.Controls.Add(this.label85);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 77);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1042, 20);
		this.panel22.TabIndex = 16;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(999, -2);
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
		this.panel23.Size = new System.Drawing.Size(1042, 77);
		this.panel23.TabIndex = 15;
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
		this.btnAddPlantDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddPlantDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddPlantDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddPlantDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddPlantDetail.Location = new System.Drawing.Point(857, 36);
		this.btnAddPlantDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddPlantDetail.Name = "btnAddPlantDetail";
		this.btnAddPlantDetail.Size = new System.Drawing.Size(38, 26);
		this.btnAddPlantDetail.TabIndex = 3;
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
		this.btnCanclePlantdetail.TabIndex = 5;
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
		this.btnUpdatePlantDetail.TabIndex = 4;
		this.btnUpdatePlantDetail.Text = "Update ";
		this.btnUpdatePlantDetail.UseVisualStyleBackColor = false;
		this.btnUpdatePlantDetail.Click += new System.EventHandler(btnUpdatePlantDetail_Click);
		this.txtRemarksPlantDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksPlantDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPlantDetail.Location = new System.Drawing.Point(159, 38);
		this.txtRemarksPlantDetail.Name = "txtRemarksPlantDetail";
		this.txtRemarksPlantDetail.Size = new System.Drawing.Size(693, 23);
		this.txtRemarksPlantDetail.TabIndex = 2;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(159, 20);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(52, 15);
		this.label87.TabIndex = 63;
		this.label87.Text = "Remarks";
		this.cmbPlantFeader.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantFeader.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantFeader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantFeader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
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
		this.panel4.Controls.Add(this.label5);
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
		this.panel4.Size = new System.Drawing.Size(1050, 357);
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
		this.groupBox3.Location = new System.Drawing.Point(7, 211);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(194, 142);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters For Item";
		this.cmbentrytype.AllowNull = (DefaultableBoolean)1;
		this.cmbentrytype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbentrytype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbentrytype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
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
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
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
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance178;
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
		this.groupBox2.Controls.Add(this.label84);
		this.groupBox2.Controls.Add(this.label72);
		this.groupBox2.Controls.Add(this.txtProductionInstruction);
		this.groupBox2.Controls.Add(this.txtOtherInstruction);
		this.groupBox2.Controls.Add(this.label71);
		this.groupBox2.Controls.Add(this.txtDeliveryInstruction);
		this.groupBox2.Controls.Add(this.chkPrint);
		this.groupBox2.Controls.Add(this.txtPlantInstruction);
		this.groupBox2.Controls.Add(this.txtPlanDate);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtPlanCode);
		this.groupBox2.Controls.Add(this.txtInvoiceReference);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionType);
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
		this.groupBox2.Controls.Add(this.txtstartDate);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1041, 207);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label84.Location = new System.Drawing.Point(4, 52);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(57, 15);
		this.label84.TabIndex = 214;
		this.label84.Text = "Doc Date";
		this.label72.BackColor = System.Drawing.Color.LightGray;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(544, 142);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(409, 20);
		this.label72.TabIndex = 213;
		this.label72.Text = "Other Instruction";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtProductionInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProductionInstruction.Location = new System.Drawing.Point(104, 164);
		this.txtProductionInstruction.Name = "txtProductionInstruction";
		this.txtProductionInstruction.Size = new System.Drawing.Size(434, 37);
		this.txtProductionInstruction.TabIndex = 5;
		this.txtProductionInstruction.Text = "";
		this.txtOtherInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherInstruction.Location = new System.Drawing.Point(544, 164);
		this.txtOtherInstruction.Name = "txtOtherInstruction";
		this.txtOtherInstruction.Size = new System.Drawing.Size(409, 37);
		this.txtOtherInstruction.TabIndex = 13;
		this.txtOtherInstruction.Text = "";
		this.label71.BackColor = System.Drawing.Color.LightGray;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(545, 80);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(409, 20);
		this.label71.TabIndex = 210;
		this.label71.Text = "Delivery Instruction";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtDeliveryInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryInstruction.Location = new System.Drawing.Point(544, 104);
		this.txtDeliveryInstruction.Name = "txtDeliveryInstruction";
		this.txtDeliveryInstruction.Size = new System.Drawing.Size(409, 37);
		this.txtDeliveryInstruction.TabIndex = 12;
		this.txtDeliveryInstruction.Text = "";
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkPrint.Location = new System.Drawing.Point(959, 177);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(67, 19);
		this.chkPrint.TabIndex = 14;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.txtPlantInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlantInstruction.Location = new System.Drawing.Point(544, 39);
		this.txtPlantInstruction.Name = "txtPlantInstruction";
		this.txtPlantInstruction.Size = new System.Drawing.Size(409, 37);
		this.txtPlantInstruction.TabIndex = 11;
		this.txtPlantInstruction.Text = "";
		this.txtPlanDate.CustomFormat = "dd-MMM-yy";
		this.txtPlanDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPlanDate.Location = new System.Drawing.Point(104, 48);
		this.txtPlanDate.Name = "txtPlanDate";
		this.txtPlanDate.Size = new System.Drawing.Size(204, 23);
		this.txtPlanDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Doc No";
		this.txtPlanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanCode.Enabled = false;
		this.txtPlanCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlanCode.Location = new System.Drawing.Point(104, 18);
		this.txtPlanCode.Name = "txtPlanCode";
		this.txtPlanCode.Size = new System.Drawing.Size(204, 23);
		this.txtPlanCode.TabIndex = 0;
		this.txtInvoiceReference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceReference.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInvoiceReference.Location = new System.Drawing.Point(104, 77);
		this.txtInvoiceReference.Name = "txtInvoiceReference";
		this.txtInvoiceReference.Size = new System.Drawing.Size(204, 23);
		this.txtInvoiceReference.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(4, 81);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 73;
		this.label1.Text = "Job Order No";
		this.cmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Location = new System.Drawing.Point(104, 104);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Name = "cmbProductionType";
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).TabIndex = 3;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(4, 110);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(94, 15);
		this.label13.TabIndex = 157;
		this.label13.Text = "Production Type";
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(4, 164);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(100, 37);
		this.label4.TabIndex = 155;
		this.label4.Text = "Production Instruction";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWipItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWipItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWipItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Location = new System.Drawing.Point(395, 16);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Name = "CmbWipItem";
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(313, 22);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(58, 15);
		this.label23.TabIndex = 176;
		this.label23.Text = "WIP Item";
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance213).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(104, 134);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 4;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance221;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance226).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(395, 134);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 10;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(4, 140);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 168;
		this.label15.Text = "WIP A/c";
		this.cmbPlanType.AllowNull = (DefaultableBoolean)1;
		this.cmbPlanType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlanType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlanType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance251).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPlanType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Location = new System.Drawing.Point(395, 46);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Name = "cmbPlanType";
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).TabIndex = 7;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(313, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 71;
		this.label2.Text = "Plan Type";
		this.label6.BackColor = System.Drawing.Color.LightGray;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(544, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(409, 20);
		this.label6.TabIndex = 79;
		this.label6.Text = "Plant Instruction";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(395, 106);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(144, 23);
		this.txtEndDate.TabIndex = 9;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(313, 140);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 151;
		this.label10.Text = "Status";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(313, 110);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(55, 15);
		this.label18.TabIndex = 163;
		this.label18.Text = "End Date";
		this.txtstartDate.CustomFormat = "dd-MMM-yy";
		this.txtstartDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtstartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtstartDate.Location = new System.Drawing.Point(395, 77);
		this.txtstartDate.Name = "txtstartDate";
		this.txtstartDate.Size = new System.Drawing.Size(144, 23);
		this.txtstartDate.TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(313, 81);
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
		this.groupBox1.Location = new System.Drawing.Point(207, 211);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(839, 142);
		this.groupBox1.TabIndex = 208;
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
		this.label94.Location = new System.Drawing.Point(137, 0);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(116, 17);
		this.label94.TabIndex = 6;
		this.label94.Tag = "OverHeads && Packing Mateial Totals";
		this.label94.Text = "OverHeads Totals";
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
		this.label57.Visible = false;
		this.txtPackingMaterialAmount.Enabled = false;
		this.txtPackingMaterialAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingMaterialAmount.Location = new System.Drawing.Point(265, 23);
		this.txtPackingMaterialAmount.Name = "txtPackingMaterialAmount";
		this.txtPackingMaterialAmount.Size = new System.Drawing.Size(125, 23);
		this.txtPackingMaterialAmount.TabIndex = 0;
		this.txtPackingMaterialAmount.Visible = false;
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
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance258;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance263).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance264).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance264).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance264).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance267;
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
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(34, 169);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(32, 15);
		this.label5.TabIndex = 63;
		this.label5.Text = "Date";
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
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance270;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance276).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance277).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance277).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance277).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance280;
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
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance283;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance288).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance289).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance289).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance289).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance292;
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
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance295;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance300).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance301).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance301).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance301).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance302).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance304;
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
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1050, 54);
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnsave, this.btnUpdate, this.btnattachment, this.btnRefresh, this.btnLoadInvoices, this.BtnGenerateInPutRates, this.btnGenerate, this.Print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1050, 27);
		this.toolStrip1.TabIndex = 57;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnLoadInvoices.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadInvoices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(122, 24);
		this.btnLoadInvoices.Text = "Load Invoices";
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.btnGenerate.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(91, 24);
		this.btnGenerate.Text = "Generate";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(100, 24);
		this.Print.Text = "1563-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutHistory);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1056, 707);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add(this.panel20);
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 590);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1050, 114);
		this.panel11.TabIndex = 67;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory);
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(476, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(574, 114);
		this.panel20.TabIndex = 67;
		this.panel20.Visible = false;
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
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Size = new System.Drawing.Size(574, 92);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).TabIndex = 67;
		this.grdPackingMaterialHistory.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialHistory).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label83);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(574, 22);
		this.panel21.TabIndex = 66;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.White;
		this.label83.Location = new System.Drawing.Point(4, 3);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(321, 17);
		this.label83.TabIndex = 10;
		this.label83.Text = "Packing Material Detail Of the above Selected Row";
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverheadHistory);
		this.panel13.Controls.Add(this.panel19);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(476, 114);
		this.panel13.TabIndex = 66;
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
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Size = new System.Drawing.Size(476, 92);
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).TabIndex = 67;
		this.grdOverheadHistory.TotalRow = (InheritableBoolean)1;
		this.grdOverheadHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverheadHistory).VisualStyle = (VisualStyle)4;
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.label82);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(476, 22);
		this.panel19.TabIndex = 66;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.White;
		this.label82.Location = new System.Drawing.Point(3, 3);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(285, 17);
		this.label82.TabIndex = 9;
		this.label82.Text = "OverHeads Detail Of the above Selected Row";
		this.grdOutPutHistory.AllowEdit = (InheritableBoolean)2;
		grdOutPutHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutHistory.DesignTimeLayout = grdOutPutHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdOutPutHistory.DynamicFiltering = true;
		this.grdOutPutHistory.FilterMode = (FilterMode)2;
		this.grdOutPutHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Location = new System.Drawing.Point(3, 437);
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Name = "grdOutPutHistory";
		((ScrollableBase)this.grdOutPutHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Size = new System.Drawing.Size(1050, 153);
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).TabIndex = 66;
		this.grdOutPutHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutPutHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutHistory).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label81);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 414);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1050, 23);
		this.panel5.TabIndex = 65;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.White;
		this.label81.Location = new System.Drawing.Point(4, 3);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(262, 17);
		this.label81.TabIndex = 8;
		this.label81.Text = "OutPut Detail Of the above Selected Row";
		this.grdInputHistory.AllowEdit = (InheritableBoolean)2;
		grdInputHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputHistory.DesignTimeLayout = grdInputHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdInputHistory.DynamicFiltering = true;
		this.grdInputHistory.FilterMode = (FilterMode)2;
		this.grdInputHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Location = new System.Drawing.Point(3, 269);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Name = "grdInputHistory";
		((ScrollableBase)this.grdInputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Size = new System.Drawing.Size(1050, 145);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).TabIndex = 64;
		this.grdInputHistory.TotalRow = (InheritableBoolean)1;
		this.grdInputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label80);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 248);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1050, 21);
		this.panel12.TabIndex = 63;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.White;
		this.label80.Location = new System.Drawing.Point(5, 2);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(251, 17);
		this.label80.TabIndex = 7;
		this.label80.Text = "Input Detail Of the above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 59);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1050, 189);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 61;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar5);
		this.panel8.Controls.Add(this.label77);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1050, 56);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = null;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1010, 31);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(7);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar5.TabIndex = 211;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.White;
		this.label77.Location = new System.Drawing.Point(5, 31);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(244, 19);
		this.label77.TabIndex = 210;
		this.label77.Text = "Production Job Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnloadall });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1050, 27);
		this.toolStrip2.TabIndex = 58;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnloadall.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnloadall.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnloadall.Name = "btnloadall";
		this.btnloadall.Size = new System.Drawing.Size(82, 24);
		this.btnloadall.Text = "Load All";
		this.btnloadall.Click += new System.EventHandler(btnloadall_Click);
		this.BtnGenerateInPutRates.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.BtnGenerateInPutRates.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnGenerateInPutRates.Name = "BtnGenerateInPutRates";
		this.BtnGenerateInPutRates.Size = new System.Drawing.Size(173, 24);
		this.BtnGenerateInPutRates.Text = "Generate Input Rates";
		this.BtnGenerateInPutRates.Click += new System.EventHandler(BtnGenerateInPutRates_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1064, 733);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProductionPreCostingAndJobOrder_St";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProductionPreCostingAndJobOrder_St_FormClosing);
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
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
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
		this.panel11.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverheadHistory).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
