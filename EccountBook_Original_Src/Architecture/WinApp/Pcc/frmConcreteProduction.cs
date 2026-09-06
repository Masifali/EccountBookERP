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
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Pcc;
using Architecture.Model.Production;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmConcreteProduction : Form
{
	private WaitForm WF = new WaitForm();

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInput = new DataTable();

	private DataTable dtOutPutByProduct = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtContractorWages = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtBOMDataforCombo = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtHistoryFromDb = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable lstitem = new DataTable();

	private DataTable dtAttributes = new DataTable();

	private DataTable dtWareForInput = new DataTable();

	private DataTable dtWareForOutPut = new DataTable();

	private DataTable dtJob = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable dtOrders = new DataTable();

	private DataTable dtType = new DataTable();

	private DataTable DtStock = new DataTable();

	private DataTable dtOutputBom = new DataTable();

	private DataTable dtOutputItem = new DataTable();

	private DataTable dtWorkOrderData = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtRefParty = new DataTable();

	private int updateDetailIndex = -1;

	private bool FifoMethod;

	private bool UpdateMode;

	private int RecId;

	private int PreviousWorkOrderId;

	private bool CheckWarehouseExists = true;

	private bool ContractorWagesDebitTotExpenseAcOnProductionConcrete = true;

	private List<ProductionInputDetail> lstRemoveRecordInput = new List<ProductionInputDetail>();

	private List<ProductionOutPutDetail> lstRemoveRecordOutPut = new List<ProductionOutPutDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	internal UltraCombo cmbItemBOM;

	private Label label7;

	private TextBox txtTotalOutPutAutoRatedAmount;

	private Label label10;

	private TextBox txtOverHead;

	private Label label9;

	private TextBox txtTotalInputs;

	private Label label6;

	internal UltraCombo comCompany;

	private Label label4;

	internal UltraCombo combranches;

	private Label label45;

	internal UltraCombo comproject;

	private Label label46;

	private Label label11;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private Panel panel4;

	private Panel panel20;

	private Label label5;

	private Label label25;

	private Label label3;

	private GroupBox groupBox2;

	private Label label2;

	private Label label21;

	internal UltraCombo cmbBOMNo;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Panel panel10;

	internal GridEX grdInputMaterial;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Label label30;

	internal UltraCombo cmbProductionNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreviewSlip;

	internal GridEX grdHistoryOutput;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	private TextBox txtRemarksHeader;

	private Panel panel11;

	private Label label23;

	private Panel panel7;

	private Label label1;

	private TextBox txtOutPutAutoRatePerQtyAmount;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label38;

	private TextBox txtWagesAmount;

	internal GridEX grdWagesHistory;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdContractorWages;

	private Panel panel5;

	private Label label33;

	internal GridEX grdOverHead;

	private Panel panel14;

	private Label label22;

	private Label label12;

	internal UltraCombo CmbWorkOrderNo;

	private Panel panel17;

	private Panel panel6;

	private TabControl tabControl2;

	private TabPage TabOutPut;

	internal GridEX grdOutPutByProduct;

	private TabPage TabWages;

	private TabPage TabOverheads;

	private TextBox txtProductionNo;

	private Label label8;

	private Button BtnLoadInputFromBom;

	private ToolStripButton btnGenerateRatesOutput;

	private TextBox txtTotalOutPutManualRatedAmount;

	private Label label39;

	private Label label13;

	private Label label24;

	private Button BtnUpdateWarehouseInput;

	private Button BtnUpdateWarehouseOutPut;

	private Label label14;

	private Panel panel18;

	private Label label40;

	private Label label15;

	internal GridEX grdHistoryOverHead;

	private Panel panel42;

	private Label label34;

	private ToolStripButton btnVoucherReport;

	private CheckBox chkPrintVoucher;

	internal UltraCombo CmbProductionType;

	private Label label16;

	internal UltraCombo CmbPlantName;

	private Label label26;

	private Button btnDefinePlant;

	internal UltraCombo CmbWIPAccount;

	private Label label36;

	internal UltraCombo CmbWIPWarehouse;

	private Label label27;

	private ToolStripButton btnattachment;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private GroupBox groupBox3;

	internal UltraCombo CmbWarehouseName;

	private Label label41;

	private TextBox txtVarientEquivalent;

	private Label label43;

	internal UltraCombo CmbAttribute;

	private Label label42;

	private Button BtnAddDetail;

	private Button BtnUpdateDetail;

	private Button BtnCancelDetail;

	internal UltraCombo CmbContractorForUpdateInWagesGrid;

	private Button BtnUpdateConctratorInWages;

	private Label label66;

	private Label label65;

	internal UltraCombo CmbActivityforContractorUpdateInWagesGrid;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private Button btnClearInput;

	private ToolStripButton btnUpdateInputWagesOH;

	internal UltraCombo CmbWagesExpenseAccount;

	private Label labelWagesExpAc;

	internal UltraCombo CmbCustomerName;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private Panel panel9;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private Button BtnUpdateInOhGrid;

	private TextBox txtPercentToupdateInOhGrid;

	private Label label29;

	private Panel panel16;

	private RadioButton RadCustomers;

	private RadioButton RadRefParty;

	private Label label20;

	private Button BtnReferenceParty;

	private TextBox txtReferenceNoDetail;

	private Label label47;

	public frmConcreteProduction(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 750);
			WF.Show();
			FifoMethod = CommonServices.GetERPFeatureById(5);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
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
			ContractorWagesDebitTotExpenseAcOnProductionConcrete = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ContractorWagesDebitTotExpenseAcOnProductionConcrete"));
			labelWagesExpAc.Visible = ContractorWagesDebitTotExpenseAcOnProductionConcrete;
			((Control)(object)CmbWagesExpenseAccount).Visible = ContractorWagesDebitTotExpenseAcOnProductionConcrete;
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("ItemCategoryId", typeof(int));
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("ItemTypeId", typeof(int));
			dtItem.Columns.Add("ItemType");
			dtItem.Columns.Add("ParentTypeId", typeof(int));
			dtItem.Columns.Add("ParentType");
			dtOutputBom.Columns.Add("BomHeaderId", typeof(int));
			dtOutputBom.Columns.Add("BomNo", typeof(string));
			dtOutputItem.Columns.Add("OutItemId", typeof(int));
			dtOutputItem.Columns.Add("OutItemName", typeof(string));
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtType.Columns.Add("EntryTypeId", typeof(int));
			dtType.Columns.Add("EntryType", typeof(string));
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			ItemdtFillFromBomDbCall();
			ItemNameBind();
			dtFillsForGrids();
			WIPWarehouseFill();
			WIPAccountFill();
			WagesExpenseAccountFill();
			BOMNoAndItemFill();
			Datetypefill();
			AllCombobind();
			DocumentNoFill();
			ProductionTypeFill();
			PlantBind(PlantFeaderDbCall());
			WorkOrderDbCall(null);
			WorkOrderNoFill();
			RefPartydtDbCall();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("BomHeaderId", typeof(int));
			dtInput.Columns.Add("BomNo", typeof(string));
			dtInput.Columns.Add("BomInputId", typeof(int));
			dtInput.Columns.Add("WarehouseId", typeof(int));
			dtInput.Columns.Add("WarehouseName", typeof(string));
			dtInput.Columns.Add("JobLotId", typeof(int));
			dtInput.Columns.Add("JobLot", typeof(string));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemName", typeof(string));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("BalanceStock", typeof(decimal));
			dtInput.Columns.Add("BomQty", typeof(decimal));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("BomRate", typeof(decimal));
			dtInput.Columns.Add("Rate", typeof(decimal));
			dtInput.Columns.Add("RateUomId", typeof(int));
			dtInput.Columns.Add("RateUom", typeof(decimal));
			dtInput.Columns.Add("RateUomCode", typeof(string));
			dtInput.Columns.Add("Amount", typeof(decimal));
			dtInput.Columns.Add("BomMinQty", typeof(decimal));
			dtInput.Columns.Add("BomMaxQty", typeof(decimal));
			dtInput.Columns.Add("MinQty", typeof(decimal));
			dtInput.Columns.Add("MaxQty", typeof(decimal));
			dtInput.Columns.Add("MainItemId", typeof(int));
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOutPutByProduct.Columns.Add("Id", typeof(int));
			dtOutPutByProduct.Columns.Add("BomHeaderId", typeof(int));
			dtOutPutByProduct.Columns.Add("BomNo", typeof(string));
			dtOutPutByProduct.Columns.Add("WorkOrderId", typeof(int));
			dtOutPutByProduct.Columns.Add("WorkOrderDetailId", typeof(int));
			dtOutPutByProduct.Columns.Add("WorkOrderNo", typeof(string));
			dtOutPutByProduct.Columns.Add("EntryTypeId", typeof(int));
			dtOutPutByProduct.Columns.Add("EntryType", typeof(string));
			dtOutPutByProduct.Columns.Add("CustomerTypeId", typeof(int));
			dtOutPutByProduct.Columns.Add("CustomerType", typeof(string));
			dtOutPutByProduct.Columns.Add("CustomerId", typeof(int));
			dtOutPutByProduct.Columns.Add("CustomerName", typeof(string));
			dtOutPutByProduct.Columns.Add("ReferenceNo", typeof(string));
			dtOutPutByProduct.Columns.Add("SaleOrderId", typeof(int));
			dtOutPutByProduct.Columns.Add("SaleOrderDetailId", typeof(int));
			dtOutPutByProduct.Columns.Add("SaleOrder", typeof(int));
			dtOutPutByProduct.Columns.Add("WarehouseId", typeof(int));
			dtOutPutByProduct.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProduct.Columns.Add("JobLotId", typeof(int));
			dtOutPutByProduct.Columns.Add("JobLot", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemId", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtOutPutByProduct.Columns.Add("AttributeVarient", typeof(string));
			dtOutPutByProduct.Columns.Add("VarientEquivalent", typeof(decimal));
			dtOutPutByProduct.Columns.Add("ItemUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("Uom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("UomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("VarientQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProduct.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("RateUom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("Amount", typeof(decimal));
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			dtOverHead.Columns.Add("Id", typeof(int));
			dtOverHead.Columns.Add("BomHeaderId", typeof(int));
			dtOverHead.Columns.Add("BomNo", typeof(string));
			dtOverHead.Columns.Add("BomOverHeadId", typeof(int));
			dtOverHead.Columns.Add("OverHeadTypeId", typeof(int));
			dtOverHead.Columns.Add("OverHeadAccountId", typeof(int));
			dtOverHead.Columns.Add("PercentOfTotalInput", typeof(decimal));
			dtOverHead.Columns.Add("BomQty", typeof(decimal));
			dtOverHead.Columns.Add("Qty", typeof(decimal));
			dtOverHead.Columns.Add("Rate", typeof(decimal));
			dtOverHead.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("OhRemarks", typeof(string));
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			dtContractorWages.Columns.Add("Id", typeof(int));
			dtContractorWages.Columns.Add("BomHeaderId", typeof(int));
			dtContractorWages.Columns.Add("BomNo", typeof(string));
			dtContractorWages.Columns.Add("BomWagesId", typeof(int));
			dtContractorWages.Columns.Add("OutPutItemId", typeof(int));
			dtContractorWages.Columns.Add("OutPutItemName");
			dtContractorWages.Columns.Add("ContractorId", typeof(int));
			dtContractorWages.Columns.Add("ContractorName");
			dtContractorWages.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtContractorWages.Columns.Add("ServiceActivity");
			dtContractorWages.Columns.Add("BomQty", typeof(decimal));
			dtContractorWages.Columns.Add("Qty", typeof(decimal));
			dtContractorWages.Columns.Add("Rate", typeof(decimal));
			dtContractorWages.Columns.Add("Amount", typeof(decimal));
			dtContractorWages.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			txtdocdate.Focus();
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

	private void dtFillsForGrids()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		try
		{
			dtType.Clear();
			dtType.Rows.Add(1, "Auto Rate");
			dtType.Rows.Add(1, "Manual Rate");
			dtJob.Clear();
			dtJob = CommonServices.JobLotGetAllService();
			lstitem.Clear();
			lstitem = CommonServices.GetItemByItemTypeId("45,46,47");
			dtAttributes.Clear();
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbItemBOM.Value));
			dtOrders.Clear();
			SaleOrder obj = new SaleOrder();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOrders = SaleOrder.GetSaleOrderNo(obj);
			dtContr.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtAccounts.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 11 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 12 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 13 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 14 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 20 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 21)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumentNoFill()
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
			int code = ProductionHeader.GenerateCode(new ProductionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1850,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WarehouseFill(DataTable dtWare)
	{
		try
		{
			int ItId = Conversion.ToInt(CmbWarehouseName.Value);
			if (dtWare.Rows.Count > 0)
			{
				DDL.BindDDL(dtWare, CmbWarehouseName, "Id", "WareHouseName", "WareHouse", ZeroIndex: true);
				((UltraGridBase)CmbWarehouseName).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ItId > 0)
				{
					if (dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbWarehouseName.Value = ItId;
					}
					else
					{
						((Control)(object)CmbWarehouseName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWarehouseName).DataSource = null;
				((UltraGridBase)CmbWarehouseName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemdtFillFromBomDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			dtItem.Clear();
			DataTable dt = BomHeader.GetAllBomItems(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1851,
				ItemTypeId = Conversion.ToInt(CmbProductionType.Value)
			});
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in dt.Rows)
			{
				dtItem.Rows.Add(dr["ItemId"], dr["ItemName"], dr["ItemCode"], dr["ItemCategoryId"], dr["ItemCategory"], dr["ItemTypeId"], dr["ItemType"], dr["ItemTypeOfTypeId"], dr["ItemTypeOfType"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItemBOM.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(cmbItemBOM, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[7].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AttributeVarientFill(int ItemId)
	{
		try
		{
			dtAttributes.Clear();
			int BomId = Conversion.ToInt(CmbAttribute.Value);
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtAttributes.Rows.Count > 0)
			{
				DDL.BindDDL(dtAttributes, CmbAttribute, "ItemAttributeVarientId", "ItemAttribute", "ItemAttribute", ZeroIndex: true);
				if (BomId > 0)
				{
					if (dtAttributes.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeVarientId") == BomId))
					{
						CmbAttribute.Value = BomId;
					}
					else
					{
						((Control)(object)CmbAttribute).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbAttribute).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BOMNobyItemDbCall(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return BomHeader.GetBomNoByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ItemId = ItemId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1851
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BOMNoBind(DataTable dtBomNo)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbBOMNo, dtBomNo, "Id", "BomName", "Bom No", AllColumns: true, Conversion.ToInt(cmbBOMNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBomNo != null && dtBomNo.Rows.Count > 0)
			{
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BOMNoAndItemFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtBOMDataforCombo.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1851;
			dtBOMDataforCombo = BomHeader.GetAllBomNoAndItems(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindHeaderItemUom(int BomId)
	{
		try
		{
			if (BomId > 0)
			{
				int Id = Conversion.ToInt(cmbUOM.Value);
				((Control)(object)cmbUOM).Text = string.Empty;
				DataTable dtuomheader = new DataTable();
				dtuomheader.Columns.Add("Id", typeof(int));
				dtuomheader.Columns.Add("ItemUOM");
				dtuomheader.Columns.Add("ItemUOMEquivalent", typeof(double));
				for (int i = 0; i < dtBOMDataforCombo.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtBOMDataforCombo.Rows[i]["Id"]) == BomId)
					{
						dtuomheader.Rows.Add(dtBOMDataforCombo.Rows[i]["ItemUomId"], dtBOMDataforCombo.Rows[i]["PackUom"], dtBOMDataforCombo.Rows[i]["PackUomEquivalent"]);
					}
				}
				if (dtuomheader.Rows.Count > 0)
				{
					DDL.BindDDL(dtuomheader, cmbUOM, "Id", "ItemUOM", "UOM", ZeroIndex: true);
					((UltraGridBase)cmbUOM).Rows[1].Activate();
					if (Id > 0 && dtuomheader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbUOM.Value = Id;
					}
				}
			}
			else
			{
				((Control)(object)cmbUOM).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbBOMNo_Leave(object sender, EventArgs e)
	{
		try
		{
			BindHeaderItemUom(Conversion.ToInt(cmbBOMNo.Value));
			DataRow foundRow = dtBOMDataforCombo.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(cmbBOMNo.Value));
			txtQty.Text = ((foundRow != null) ? Conversion.ToString(foundRow["BomBatchQty"]) : "");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[6] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal };
				dtSupplier.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefPartydtDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtRefParty.Clear();
			ReferenceParties obj = new ReferenceParties();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ReferencePartyTypeId = 1;
			dtRefParty = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierdtFillFromRefPartydt()
	{
		try
		{
			dtSupplier.Clear();
			DataTable dataTable = dtRefParty;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow item in dtRefParty.Rows)
			{
				dtSupplier.Rows.Add(item["Id"], item["ReferencePartyName"]);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerName, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, Conversion.ToInt(CmbCustomerName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			if (!RadCustomers.Checked)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkOrderDbCall(string WorkOrderDetailIds)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtWorkOrderData.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.RecId = RecId;
			obj.OrderDetailIds = WorkOrderDetailIds;
			dtWorkOrderData = WorkOrderHeader.PendingWorkOrderForProduction(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WorkOrderNoFill()
	{
		try
		{
			DataTable dtWO = new DataTable();
			dtWO.Columns.Add("Id", typeof(int));
			dtWO.Columns.Add("WorkOrderNo", typeof(string));
			dtWO.Columns.Add("DocNo", typeof(int));
			dtWO.Columns.Add("DocDate", typeof(DateTime));
			dtWO.Columns.Add("CustomerId", typeof(int));
			dtWO.Columns.Add("CustomerName", typeof(string));
			dtWO.Columns.Add("OrderId", typeof(int));
			dtWO.Columns.Add("OrderNo", typeof(int));
			dtWO.Columns.Add("PlantId", typeof(int));
			dtWO.Columns.Add("PlantName", typeof(string));
			DataTable dataTable = dtWorkOrderData;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow dr in dtWorkOrderData.Rows)
				{
					int Id = Conversion.ToInt(dr["WorkOrderHeaderId"]);
					if (Ids.Add(Id))
					{
						dtWO.Rows.Add(Id, dr["WorkOrderNo"], dr["DocNo"], dr["DocDate"], dr["CustomerId"], dr["CustomerName"], dr["SaleOrderId"], dr["OrderNo"], dr["PlantId"], dr["PlantName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbWorkOrderNo, dtWO, "Id", "WorkOrderNo", "WorkOrder No", AllColumns: true, Conversion.ToInt(CmbWorkOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtWO != null && dtWO.Rows.Count > 0)
			{
				((UltraGridBase)CmbWorkOrderNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbWorkOrderNo).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbWorkOrderNo).DisplayLayout.Bands[0].Columns[8].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ProductionTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbProductionType.Value);
			DataTable dt = InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 6
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dttype = new DataTable();
				dttype.Columns.Add("Id", typeof(int));
				dttype.Columns.Add("ProductionName", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 45 || Conversion.ToInt(dt.Rows[i]["Id"]) == 46 || Conversion.ToInt(dt.Rows[i]["Id"]) == 47)
					{
						dttype.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LookupName"]);
					}
				}
				if (dttype.Rows.Count > 0)
				{
					DDL.BindDDL(dttype, CmbProductionType, "Id", "ProductionName", "Production Type", ZeroIndex: true);
					((UltraGridBase)CmbProductionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbProductionType).Rows[1].Activate();
					if (BomId > 0)
					{
						if (dttype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
						{
							CmbProductionType.Value = BomId;
						}
						else
						{
							((Control)(object)CmbProductionType).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbProductionType).Text = string.Empty;
					((UltraGridBase)CmbProductionType).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbProductionType).Text = string.Empty;
				((UltraGridBase)CmbProductionType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable PlantFeaderDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PlantBind(DataTable dtProductionplant)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPlantName, dtProductionplant, "Id", "Description", "Plant", AllColumns: false, Conversion.ToInt(CmbPlantName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WIPWarehouseFill()
	{
		try
		{
			int BomId = Conversion.ToInt(CmbWIPWarehouse.Value);
			DataTable dtProductionplant = CommonServices.GetActiveWareHouseByWareHouseType(5);
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, CmbWIPWarehouse, "Id", "WareHouseName", "WareHouseName", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbWIPWarehouse.Value = BomId;
					}
					else
					{
						((Control)(object)CmbWIPWarehouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWIPWarehouse).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WIPAccountFill()
	{
		try
		{
			int BomId = Conversion.ToInt(CmbWIPAccount.Value);
			DataTable dtCoa = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12");
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbWIPAccount, "Id", "AccountTitle", "WorkInProcess", ZeroIndex: true);
				if (BomId > 0)
				{
					if (dtCoa.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbWIPAccount.Value = BomId;
					}
					else
					{
						((Control)(object)CmbWIPAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWIPAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WagesExpenseAccountFill()
	{
		try
		{
			int BomId = Conversion.ToInt(CmbWagesExpenseAccount.Value);
			DataTable dtCoa = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21");
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbWagesExpenseAccount, "Id", "AccountTitle", "WagesExpenseAccount", ZeroIndex: true);
				if (BomId > 0)
				{
					if (dtCoa.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbWagesExpenseAccount.Value = BomId;
					}
					else
					{
						((Control)(object)CmbWagesExpenseAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWagesExpenseAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbAttribute_Leave(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttribute.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttribute).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
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
			BOMNoBind(BOMNobyItemDbCall(Conversion.ToInt(cmbItemBOM.Value)));
			AttributeVarientFill(Conversion.ToInt(cmbItemBOM.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPlantName_Leave(object sender, EventArgs e)
	{
		try
		{
			BOMNoBind(BOMNobyItemDbCall(Conversion.ToInt(cmbItemBOM.Value)));
			dtWareForInput = CommonServices.WareHouseAllocationToPlantsByProductionPlantId(Conversion.ToInt(CmbPlantName.Value));
			if (dtWareForInput.Rows.Count > 0)
			{
				int warehouseId = 0;
				WarehouseFill(dtWareForInput);
				DataRow firstRow = (from row in dtWareForInput.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).FirstOrDefault();
				if (firstRow != null)
				{
					warehouseId = Conversion.ToInt(firstRow["Id"]);
				}
				if (Conversion.ToInt(CmbWarehouseName.Value) == 0)
				{
					CmbWarehouseName.Value = warehouseId;
				}
			}
			EnableDisablePlant();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWorkOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		try
		{
			int WorkOrderId = Conversion.ToInt(CmbWorkOrderNo.Value);
			int count = (from r in grdOutPutByProduct.GetDataRows()
				where Conversion.ToInt(r.Cells["ItemId"].Value) > 0
				select r).Count();
			if (WorkOrderId == 0 && PreviousWorkOrderId == 0)
			{
				IEnumerable<GridEXRow> Rows = from rr in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(rr.Cells["WorkOrderDetailId"].Value) > 0
					select rr;
				if (Rows.Count() > 0)
				{
					foreach (GridEXRow r2 in Rows)
					{
						int Id = Conversion.ToInt(r2.Cells["Id"].Value);
						if (Id > 0)
						{
							ProductionOutPutDetail d1 = new ProductionOutPutDetail();
							d1.Id = Id;
							FillOutPutDetailListCommonForInsertAndDelete(d1, r2);
							d1.ActionTypeId = 3;
							lstRemoveRecordOutPut.Add(d1);
							dtOutPutByProduct.Rows[r2.RowIndex].Delete();
							r2.Delete();
						}
						else
						{
							r2.Delete();
						}
					}
					if (dtOutPutByProduct.Rows.Count == 0)
					{
						AddRowInOutPutByProductGrid();
					}
				}
			}
			else if (WorkOrderId > 0 && PreviousWorkOrderId == 0)
			{
				PreviousWorkOrderId = WorkOrderId;
				FillOutPutGridFromWorkOrder(WorkOrderId);
			}
			else if (WorkOrderId != PreviousWorkOrderId && count > 0)
			{
				if (!FormHelper.ConfirmAction("Are you Sure To change WorkOrder??? Previous Data Of OutPut grid Will be Reset"))
				{
					CmbWorkOrderNo.Value = PreviousWorkOrderId;
					return;
				}
				IEnumerable<GridEXRow> Rows2 = from rr in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(rr.Cells["WorkOrderDetailId"].Value) > 0
					select rr;
				if (Rows2.Count() > 0)
				{
					foreach (GridEXRow r3 in Rows2)
					{
						int Id2 = Conversion.ToInt(r3.Cells["Id"].Value);
						if (Id2 > 0)
						{
							ProductionOutPutDetail d2 = new ProductionOutPutDetail();
							d2.Id = Id2;
							FillOutPutDetailListCommonForInsertAndDelete(d2, r3);
							d2.ActionTypeId = 3;
							lstRemoveRecordOutPut.Add(d2);
							dtOutPutByProduct.Rows[r3.RowIndex].Delete();
							r3.Delete();
						}
						else
						{
							r3.Delete();
						}
					}
				}
				PreviousWorkOrderId = WorkOrderId;
				if (WorkOrderId > 0)
				{
					FillOutPutGridFromWorkOrder(WorkOrderId);
				}
			}
			else if (count == 0)
			{
				FillOutPutGridFromWorkOrder(WorkOrderId);
				PreviousWorkOrderId = WorkOrderId;
			}
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOutPutGridFromWorkOrder(int WorkOrderId)
	{
		try
		{
			if (lstRemoveRecordOutPut.Count > 0)
			{
				string Ids = string.Join(",", lstRemoveRecordOutPut.Select((ProductionOutPutDetail rr) => rr.WorkOrderDetailId).Distinct());
				WorkOrderDbCall(Ids);
			}
			DataTable dtData = (from rr in dtWorkOrderData.AsEnumerable()
				where rr.Field<int>("WorkOrderHeaderId") == WorkOrderId
				select rr).CopyToDataTable();
			if (dtData.Rows.Count == 0)
			{
				if (grdOutPutByProduct.GetDataRows().Count() == 0)
				{
					AddRowInOutPutByProductGrid();
				}
			}
			else
			{
				int PlantId = Conversion.ToInt(dtData.Rows[0]["PlantId"]);
				if (PlantId > 0 && ((UltraGridBase)CmbPlantName).DataSource != null)
				{
					CmbPlantName.Value = PlantId;
				}
				GetDefaultJob(out var JobLotId, out var JobLotName);
				HashSet<int> WorkOrderDetailIds = (from row in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(row.Cells["WorkOrderDetailId"].Value) > 0
					select Conversion.ToInt(row.Cells["WorkOrderDetailId"].Value)).ToHashSet();
				foreach (DataRow dr in dtData.Rows)
				{
					double VariantQty = Conversion.ToDouble(dr["BalQty"]) * Conversion.ToDouble(dr["VarientEquivalent"]);
					int detailId = Conversion.ToInt(dr["WorkOrderCustomerDetailId"]);
					if (WorkOrderDetailIds.Add(detailId))
					{
						dtOutPutByProduct.Rows.Add(0, 0, "", dr["WorkOrderHeaderId"], dr["WorkOrderCustomerDetailId"], dr["WorkOrderNo"], 1, "Auto Rate", dr["CustomerTypeId"], dr["CustomerType"], dr["CustomerId"], dr["CustomerName"], dr["ReferenceNo"], dr["SaleOrderId"], dr["SaleOrderDetailId"], dr["OrderNo"], 0, "", JobLotId, JobLotName, dr["ItemId"], dr["ItemName"], dr["ItemAttributeVarientId"], dr["ItemAttribute"], dr["VarientEquivalent"], dr["ItemUomId"], dr["ItemUomEquivalent"], dr["ItemUomCode"], dr["BalQty"], VariantQty, dr["BalWeight"], 0, 0, 0, "", 0);
					}
				}
			}
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadInputFromBom_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPlantName.Value) == 0)
			{
				CmbPlantName.Focus();
				throw new Exception("Please Select Plant Name First");
			}
			FillInputDetailForMultiBom();
			FillWagesGridFilterCombo();
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

	private void FillInputDetailForMultiBom()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0 && MessageBox.Show("Input Grid already have Records.Do you want To Refresh Data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Record Not Found...");
			}
			if (grdContractorWages.GetDataRows().Count() == 1)
			{
				GridEXRow r = grdContractorWages.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Conversion.ToInt(r.Cells["BomHeaderId"].Value) == 0 && Conversion.ToInt(r.Cells["OutPutItemId"].Value) == 0)
				{
					dtContractorWages.Clear();
					grdContractorWages.ClearStructure();
				}
			}
			if (grdOverHead.GetDataRows().Count() == 1)
			{
				GridEXRow r2 = grdOverHead.GetDataRows().FirstOrDefault();
				if (r2 != null && (int)r2.RowType == 0 && r2.Cells.Count > 0 && Conversion.ToInt(r2.Cells["BomHeaderId"].Value) == 0 && Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) == 0)
				{
					dtOverHead.Clear();
					grdOverHead.ClearStructure();
				}
			}
			int warehouseId = 0;
			string warehouseName = "";
			if (dtWareForInput.Rows.Count > 0)
			{
				DataRow firstRow2 = (from row in dtWareForInput.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).FirstOrDefault();
				if (firstRow2 != null)
				{
					warehouseId = Conversion.ToInt(firstRow2["Id"]);
					warehouseName = Conversion.ToString(firstRow2["WarehouseName"]);
				}
			}
			int JobLotId = 0;
			string JobLotName = "";
			if (dtJob.Rows.Count > 0)
			{
				DataRow firstRow = dtJob.AsEnumerable().FirstOrDefault();
				if (firstRow != null)
				{
					JobLotId = Conversion.ToInt(firstRow["Id"]);
					JobLotName = Conversion.ToString(firstRow["JobLotDescription"]);
				}
			}
			BomHeader bOMHeader = new BomHeader();
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				int BOMId = Conversion.ToInt(obj.Cells["BomHeaderId"].Value);
				int OutPutItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				string OutPutItemName = Conversion.ToString(obj.Cells["ItemName"].Value);
				if (BOMId <= 0)
				{
					continue;
				}
				bOMHeader = BomHeader.ReadById(BOMId);
				if (bOMHeader == null)
				{
					continue;
				}
				if (bOMHeader.BomInputDetailList.Count > 0)
				{
					bool flag3 = false;
					for (int k = 0; k < bOMHeader.BomInputDetailList.Count; k++)
					{
						if (bOMHeader.BomInputDetailList[k].MainItemId != 0)
						{
							continue;
						}
						if (grdInputMaterial.GetDataRows().Length != 0)
						{
							GridEXRow[] dataRows2 = grdInputMaterial.GetDataRows();
							for (int m = 0; m < dataRows2.Length; m++)
							{
								if (Conversion.ToInt(dataRows2[m].Cells["BomInputId"].Value) == Conversion.ToInt((object)bOMHeader.BomInputDetailList[k].Id))
								{
									flag3 = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							dtInput.Rows.Add(0, bOMHeader.BomInputDetailList[k].BomHeaderId, bOMHeader.BomInputDetailList[k].BomHeaderNo, bOMHeader.BomInputDetailList[k].Id, warehouseId, warehouseName, JobLotId, JobLotName, bOMHeader.BomInputDetailList[k].ItemId, bOMHeader.BomInputDetailList[k].ItemName, bOMHeader.BomInputDetailList[k].ItemUomId, bOMHeader.BomInputDetailList[k].PackUomEquivalent, bOMHeader.BomInputDetailList[k].PackUom, 0, bOMHeader.BomInputDetailList[k].ItemQty, bOMHeader.BomInputDetailList[k].ItemQty, bOMHeader.BomInputDetailList[k].WeightKg, bOMHeader.BomInputDetailList[k].Rate, 0, bOMHeader.BomInputDetailList[k].RateUomId, bOMHeader.BomInputDetailList[k].RateUomEquivalent, bOMHeader.BomInputDetailList[k].RateUom, bOMHeader.BomInputDetailList[k].Amount, bOMHeader.BomInputDetailList[k].MinQty, bOMHeader.BomInputDetailList[k].MaxQty, bOMHeader.BomInputDetailList[k].MinQty, bOMHeader.BomInputDetailList[k].MaxQty, bOMHeader.BomInputDetailList[k].ItemId);
						}
						flag3 = false;
					}
					grdInputMaterial.DataSource = dtInput;
					grdInputMaterial.RetrieveStructure();
					grdInputSettings();
				}
				else
				{
					grdInputMaterial.ClearStructure();
				}
				if (bOMHeader.BomFohDetailList.Count > 0)
				{
					bool flag2 = false;
					for (int j = 0; j < bOMHeader.BomFohDetailList.Count; j++)
					{
						if (grdOverHead.GetDataRows().Length != 0)
						{
							GridEXRow[] dataRows2 = grdOverHead.GetDataRows();
							for (int m = 0; m < dataRows2.Length; m++)
							{
								if (Conversion.ToInt(dataRows2[m].Cells["BomOverHeadId"].Value) == Conversion.ToInt((object)bOMHeader.BomFohDetailList[j].Id))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							dtOverHead.Rows.Add(0, bOMHeader.BomFohDetailList[j].BomHeaderId, bOMHeader.BomFohDetailList[j].BomHeaderNo, bOMHeader.BomFohDetailList[j].Id, bOMHeader.BomFohDetailList[j].OverHeadTypeId, bOMHeader.BomFohDetailList[j].GLAccountId, bOMHeader.BomFohDetailList[j].PercentOfTotalInput, bOMHeader.BomFohDetailList[j].Qty, bOMHeader.BomFohDetailList[j].Rate, bOMHeader.BomFohDetailList[j].Amount, bOMHeader.BomFohDetailList[j].OhRemarks);
						}
						flag2 = false;
					}
				}
				else
				{
					AddRowInOverHeadGrid();
				}
				List<BomWagesDetail> lstBomWagesDetail = BomHeader.BomHeader_BomWagesDetailWithUpdatedRatesByHeaderId(BOMId);
				if (lstBomWagesDetail.Count > 0)
				{
					bool flag = false;
					for (int i = 0; i < lstBomWagesDetail.Count; i++)
					{
						if (grdContractorWages.GetDataRows().Length != 0)
						{
							GridEXRow[] dataRows2 = grdContractorWages.GetDataRows();
							for (int m = 0; m < dataRows2.Length; m++)
							{
								if (Conversion.ToInt(dataRows2[m].Cells["BomWagesId"].Value) == Conversion.ToInt((object)lstBomWagesDetail[i].Id))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							dtContractorWages.Rows.Add(0, lstBomWagesDetail[i].BomHeaderId, lstBomWagesDetail[i].BomHeaderNo, lstBomWagesDetail[i].Id, OutPutItemId, OutPutItemName, 0, "", lstBomWagesDetail[i].ContractorWagesRateScheduleId, lstBomWagesDetail[i].ServiceActivity, lstBomWagesDetail[i].ItemQty, lstBomWagesDetail[i].ItemQty, lstBomWagesDetail[i].Rate, lstBomWagesDetail[i].Amount, lstBomWagesDetail[i].Remarks);
						}
						flag = false;
					}
				}
				else
				{
					AddRowInContractorWagesGrid();
				}
				grdOverHead.DataSource = dtOverHead;
				grdOverHead.RetrieveStructure();
				grdOverHeadGridSetting();
				grdContractorWages.DataSource = dtContractorWages;
				grdContractorWages.RetrieveStructure();
				grdContractorWagesGridSetting();
				UpdateWagesRates();
				EnableDisableItemName();
				EnableDisableProductionType();
				EnableDisablePlant();
				Calculation();
			}
			WF.Close();
		}
		catch (Exception ex)
		{
			WF.Close();
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("Doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (((Control)(object)CmbProductionType).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionType.Value) == 0)
		{
			MessageBox.Show("Production Type Feild Required");
			CmbProductionType.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == string.Empty || txtProductionNo.Text.Trim() == null)
		{
			MessageBox.Show("Production No Field Required");
			txtProductionNo.Focus();
			return false;
		}
		if (((Control)(object)CmbPlantName).Text.Trim() == string.Empty || Conversion.ToInt(CmbPlantName.Value) == 0)
		{
			MessageBox.Show("Plant Name Feild Required");
			CmbPlantName.Focus();
			return false;
		}
		if (((Control)(object)CmbWIPWarehouse).Text.Trim() == string.Empty || Conversion.ToInt(CmbWIPWarehouse.Value) == 0)
		{
			MessageBox.Show("WIPWarehouse Field Required");
			CmbWIPWarehouse.Focus();
			return false;
		}
		if (((Control)(object)CmbWIPAccount).Text.Trim() == string.Empty || Conversion.ToInt(CmbWIPAccount.Value) == 0)
		{
			MessageBox.Show("WIPAccount Field Required");
			CmbWIPAccount.Focus();
			return false;
		}
		if (ContractorWagesDebitTotExpenseAcOnProductionConcrete && (((Control)(object)CmbWagesExpenseAccount).Text.Trim() == string.Empty || Conversion.ToInt(CmbWagesExpenseAccount.Value) == 0))
		{
			MessageBox.Show("Wages Expense Account Field Required");
			CmbWagesExpenseAccount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOutPutGrid()
	{
		if (((Control)(object)cmbItemBOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbItemBOM.Value) == 0)
		{
			MessageBox.Show("ItemBOM Feild Required");
			cmbItemBOM.Focus();
			return false;
		}
		if (((Control)(object)cmbBOMNo).Text.Trim() == string.Empty || Conversion.ToInt(cmbBOMNo.Value) == 0)
		{
			MessageBox.Show("BOM No Feild Required");
			cmbBOMNo.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text == "0" || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		if (((Control)(object)CmbAttribute).Text.Trim() == string.Empty || Conversion.ToInt(CmbAttribute.Value) == 0)
		{
			MessageBox.Show("Item Attribute Field Required");
			CmbAttribute.Focus();
			return false;
		}
		if (txtVarientEquivalent.Text.Trim() == string.Empty || txtVarientEquivalent.Text == "0" || Conversion.ToDouble((object)txtVarientEquivalent.Text) == 0.0)
		{
			MessageBox.Show("Varient Equivalent Field Required");
			txtVarientEquivalent.Focus();
			return false;
		}
		if (((Control)(object)CmbWarehouseName).Text.Trim() == string.Empty || Conversion.ToInt(CmbWarehouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehouseName.Focus();
			return false;
		}
		return true;
	}

	private void CmbProductionType_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemdtFillFromBomDbCall();
			ItemNameBind();
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableProductionType()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				GridEXRow r = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r.Cells["ItemQty"].Value) > 0)
				{
					((Control)(object)CmbProductionType).Enabled = false;
				}
				else
				{
					((Control)(object)CmbProductionType).Enabled = true;
				}
			}
			else
			{
				((Control)(object)CmbProductionType).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void EnableDisablePlant()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow r = grdInputMaterial.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["WareHouseId"].Value) > 0)
				{
					((Control)(object)CmbPlantName).Enabled = false;
				}
				else
				{
					((Control)(object)CmbPlantName).Enabled = true;
				}
			}
			else
			{
				((Control)(object)CmbPlantName).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		try
		{
			grdInputMaterial.RootTable.Columns["Id"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomInputId"].Visible = false;
			grdInputMaterial.RootTable.Columns["WarehouseId"].Visible = false;
			grdInputMaterial.RootTable.Columns["JobLotId"].Visible = false;
			grdInputMaterial.RootTable.Columns["JobLot"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["Uom"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomHeaderId"].Visible = false;
			grdInputMaterial.RootTable.Columns["MainItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUom"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Visible = false;
			grdInputMaterial.RootTable.Columns["NetWeight"].Visible = false;
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdInputMaterial.RootTable.Columns["Rate"].Caption = "Avg Rate";
			grdInputMaterial.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdInputMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomInputId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BalanceStock"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomRate"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomMinQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomMaxQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MinQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MaxQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MainItemId"].EditType = (EditType)0;
			grdInputMaterial.GroupByBoxVisible = true;
			grdInputMaterial.RootTable.Groups.Add("BomNo");
			grdInputMaterial.RootTable.Columns["BomNo"].Visible = false;
			grdInputMaterial.RootTable.GroupTotals = (GroupTotals)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["BomQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["BomQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BomQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomMinQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["BomMinQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BomMinQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomMinQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomMaxQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["BomMaxQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BomMaxQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomMaxQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MinQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["MinQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["MinQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MinQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MaxQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["MaxQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["MaxQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MaxQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BalanceStock"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["BalanceStock"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BalanceStock"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BalanceStock"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Uom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["RateUom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BomRate"].AggregateFunction = (AggregateFunction)3;
			grdInputMaterial.RootTable.Columns["BomRate"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BomRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["BomRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdInputMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdInputMaterial);
			grdInputMaterial.RootTable.Columns.Add("Delete");
			grdInputMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdInputMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdInputMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputMaterial.RootTable.Columns["Delete"].Width = 20;
			grdInputMaterial.RootTable.Columns["Delete"].Position = 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				item.Cells["ItemUomId"].Value = 0;
				item.Cells["Uom"].Value = 0;
				item.Cells["UomCode"].Value = "";
				item.Cells["RateUom"].Value = 0;
				item.Cells["RateUomId"].Value = 0;
				item.Cells["RateUomCode"].Value = "";
				item.Cells["NetWeight"].Value = 0;
				item.Cells["Amount"].Value = 0;
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId") && item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight = num * quom;
					item.Cells["NetWeight"].Value = Weight;
					if (!FifoMethod)
					{
						Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
					}
					else
					{
						ReportsParameters obj1 = new ReportsParameters();
						obj1.OrganizationId = UserAccount.OrganizationId;
						obj1.CompanyId = UserAccount.CompanyId;
						obj1.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						obj1.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
						obj1.WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
						obj1.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						obj1.Id = RecId;
						if (obj1.Id > 0)
						{
							obj1.DocumentTypeId = 1850;
						}
						obj1.ItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						obj1.NetWeight = Conversion.ToDouble(item.Cells["NetWeight"].Value);
						double AvgRat = CommonServices.GetAvgRateFromFIFOMethodConcrete(obj1);
						Conversion.ToDecimal((object)AvgRat);
						item.Cells["Rate"].Value = AvgRat;
					}
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
				}
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					decimal Iuom2 = default(decimal);
					decimal num2 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom2 = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate2 = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom2 = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount2 = num2 * Iuom2 / Ruom2 * rate2;
					item.Cells["Amount"].Value = Amount2;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			if ((e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom") && item != null && (int)item.RowType == 0)
			{
				decimal Qty = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
				{
					throw new Exception("ItemQty And ItemUom Should be greater than zero...");
				}
				Qty = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
				_ = Qty * Iuom;
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					Qty = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (Qty * Iuom / Ruom * rate) : 0m);
					item.Cells["Amount"].Value = Amount;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdInputMaterial.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdInputMaterial);
			Calculation();
			EnableDisablePlant();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				grdInputMaterial.UpdateData();
				Calculation();
				EnableDisablePlant();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteInputDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProductionInputDetail d1 = new ProductionInputDetail();
					d1.Id = Id;
					FillInPutDetailListCommonForInsertAndDelete(d1, r);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					r.Delete();
					grdInputMaterial.UpdateData();
				}
			}
			else
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsDuplicateRecord(int CustomerId, string ReferenceNo, int bomId, int itemId, int warehouseId, int? skipRowIndex = null)
	{
		GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			if (!skipRowIndex.HasValue || row.RowIndex != skipRowIndex.Value)
			{
				int rCustomerId = Conversion.ToInt(row.Cells["CustomerId"].Value);
				string rReferenceNo = Conversion.ToString(row.Cells["ReferenceNo"].Value);
				int rBomId = Conversion.ToInt(row.Cells["BomHeaderId"].Value);
				int rItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
				int rWarehouseId = Conversion.ToInt(row.Cells["WareHouseId"].Value);
				if (CustomerId == rCustomerId && ReferenceNo == rReferenceNo && bomId == rBomId && itemId == rItemId && warehouseId == rWarehouseId)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void GetDefaultJob(out int jobLotId, out string jobLotName)
	{
		jobLotId = 0;
		jobLotName = "";
		if (dtJob.Rows.Count > 0)
		{
			DataRow first = dtJob.AsEnumerable().First();
			jobLotId = Conversion.ToInt(first["Id"]);
			jobLotName = Conversion.ToString(first["JobLotDescription"]);
		}
	}

	private void grdOutPutByProduct_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (Conversion.ToInt(r.Cells["WorkOrderId"].Value) > 0)
				{
					throw new Exception("Manual update is not allowed when a Work Order is present.");
				}
				updateDetailIndex = r.RowIndex;
				RadCustomers.Checked = Conversion.ToInt(r.Cells["CustomerTypeId"].Value) == 1;
				CmbCustomerName.Value = r.Cells["CustomerId"].Value.ToString();
				txtReferenceNoDetail.Text = Conversion.ToString(r.Cells["ReferenceNo"].Value);
				cmbItemBOM.Value = r.Cells["ItemId"].Value.ToString();
				cmbItem_Leave(null, null);
				cmbBOMNo.Value = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
				cmbBOMNo_Leave(null, null);
				cmbUOM.Value = Conversion.ToInt(r.Cells["ItemUomId"].Value);
				CmbAttribute.Value = r.Cells["ItemAttributeVarientId"].Value.ToString();
				CmbAttribute_Leave(null, null);
				CmbWarehouseName.Value = r.Cells["WareHouseId"].Value.ToString();
				BtnAddDetail.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				cmbItemBOM.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOutPutGrid())
			{
				return;
			}
			int bomId = Conversion.ToInt(cmbBOMNo.Value);
			int itemId = Conversion.ToInt(cmbItemBOM.Value);
			int warehouseId = Conversion.ToInt(CmbWarehouseName.Value);
			int CustomerTypeId = (RadCustomers.Checked ? 1 : 2);
			string CustomerType = (RadCustomers.Checked ? "Customer" : "Ref Party");
			int CustomerId = Conversion.ToInt(CmbCustomerName.Value);
			string ReferenceNo = Conversion.ToString((object)txtReferenceNoDetail.Text);
			if (IsDuplicateRecord(CustomerId, ReferenceNo, bomId, itemId, warehouseId))
			{
				MessageBox.Show("Duplicate record not added. Same Customer,ReferenceNo, BOM, Item and Warehouse already exists.");
				return;
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 1)
			{
				GridEXRow r = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (r != null && (Conversion.ToInt(r.Cells["BomHeaderId"].Value) == 0 || Conversion.ToInt(r.Cells["ItemId"].Value) == 0))
				{
					dtOutPutByProduct.Clear();
					grdOutPutByProduct.ClearStructure();
				}
			}
			GetDefaultJob(out var jobLotId, out var jobLotName);
			dtOutPutByProduct.Rows.Add(0, bomId, Conversion.ToString((object)((Control)(object)cmbBOMNo).Text), 0, 0, "", 1, "Auto Rate", CustomerTypeId, CustomerType, Conversion.ToInt(CmbCustomerName.Value), Conversion.ToString((object)((Control)(object)CmbCustomerName).Text), ReferenceNo, 0, 0, 0, warehouseId, Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text), jobLotId, jobLotName, itemId, Conversion.ToString((object)((Control)(object)cmbItemBOM).Text), Conversion.ToInt(CmbAttribute.Value), Conversion.ToString((object)((Control)(object)CmbAttribute).Text), Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToInt(cmbUOM.Value), Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value), Conversion.ToString((object)((Control)(object)cmbUOM).Text), 0, 0, 0, 0, Conversion.ToInt(cmbUOM.Value), Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value), Conversion.ToString((object)((Control)(object)cmbUOM).Text), 0);
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			ResetDetailOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOutPutGrid())
			{
				int bomId = Conversion.ToInt(cmbBOMNo.Value);
				int itemId = Conversion.ToInt(cmbItemBOM.Value);
				int warehouseId = Conversion.ToInt(CmbWarehouseName.Value);
				int CustomerId = Conversion.ToInt(CmbCustomerName.Value);
				string ReferenceNo = Conversion.ToString((object)txtReferenceNoDetail.Text);
				if (IsDuplicateRecord(CustomerId, ReferenceNo, bomId, itemId, warehouseId, updateDetailIndex))
				{
					MessageBox.Show("Duplicate record not added. Same Customer,ReferenceNo,BOM, Item and Warehouse already exists.");
					return;
				}
				GetDefaultJob(out var jobLotId, out var jobLotName);
				DataRow dataRow = dtOutPutByProduct.Rows[updateDetailIndex];
				dataRow["BomHeaderId"] = bomId;
				dataRow["BomNo"] = Conversion.ToString((object)((Control)(object)cmbBOMNo).Text);
				dataRow["CustomerTypeId"] = (RadCustomers.Checked ? 1 : 2);
				dataRow["CustomerType"] = (RadCustomers.Checked ? "Customer" : "Ref Party");
				dataRow["CustomerId"] = CustomerId;
				dataRow["CustomerName"] = Conversion.ToString((object)((Control)(object)CmbCustomerName).Text);
				dataRow["ReferenceNo"] = ReferenceNo;
				dataRow["EntryTypeId"] = 1;
				dataRow["EntryType"] = "Auto Rate";
				dataRow["WarehouseId"] = warehouseId;
				dataRow["WarehouseName"] = Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text);
				dataRow["JobLotId"] = jobLotId;
				dataRow["JobLot"] = jobLotName;
				dataRow["ItemId"] = itemId;
				dataRow["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItemBOM).Text);
				dataRow["ItemAttributeVarientId"] = Conversion.ToInt(CmbAttribute.Value);
				dataRow["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttribute).Text);
				dataRow["VarientEquivalent"] = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				dataRow["ItemUomId"] = Conversion.ToInt(cmbUOM.Value);
				dataRow["Uom"] = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				dataRow["UomCode"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
				dataRow["RateUomId"] = Conversion.ToInt(cmbUOM.Value);
				dataRow["RateUom"] = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				dataRow["RateUomCode"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
				BtnAddDetail.Visible = true;
				BtnUpdateDetail.Visible = false;
				BtnCancelDetail.Visible = false;
				ResetDetailOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetailOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOutPutByProductGrid()
	{
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				dtOutPutByProduct.Rows.Add(0, 0, "", 0, 0, "", 1, "Auto Rate", 0, "", 0, "", "", 0, 0, 0, 0, "", 0, "", 0, "", 0, "", 0, 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0);
			}
			else
			{
				int BomHeaderId = 0;
				string BomNo = "";
				int WorkOrderId = 0;
				int WorkOrderDetailId = 0;
				string WorkOrderNo = "";
				int CustomerId = 0;
				string CustomerName = "";
				int CustomerTypeId = 0;
				string CustomerType = "";
				string ReferenceNo = "";
				int EntryTypeId = 1;
				string EntryType = "Auto Rate";
				int SaleOrderId = 0;
				int SaleOrderDetailId = 0;
				int SaleOrder = 0;
				int WarehouseId = 0;
				string WarehouseName = "";
				int JobLotId = 0;
				string JobLot = "";
				int ItemId = 0;
				string ItemName = "";
				int ItemAttributeVarientId = 0;
				string AttributeVarient = "";
				decimal VarientEquivalent = default(decimal);
				int ItemUomId = 0;
				decimal Uom = default(decimal);
				string UomCode = "";
				decimal ItemQty = default(decimal);
				decimal VarientQty = default(decimal);
				decimal NetWeight = default(decimal);
				decimal Rate = default(decimal);
				GridEXRow r = grdOutPutByProduct.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (BomHeaderId == 0 && ItemId == 0)
					{
						GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
						foreach (GridEXRow row in dataRows)
						{
							BomHeaderId = Conversion.ToInt(row.Cells["BomHeaderId"].Value);
							ItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
							if (BomHeaderId != 0 && ItemId != 0)
							{
								SaleOrderId = Conversion.ToInt(row.Cells["SaleOrderId"].Value);
								SaleOrderDetailId = Conversion.ToInt(row.Cells["SaleOrderDetailId"].Value);
								SaleOrder = Conversion.ToInt(row.Cells["SaleOrder"].Value);
								BomNo = Conversion.ToString(row.Cells["BomNo"].Value);
								ItemName = Conversion.ToString(row.Cells["ItemName"].Value);
								CustomerTypeId = Conversion.ToInt(row.Cells["CustomerTypeId"].Value);
								CustomerType = Conversion.ToString(row.Cells["CustomerType"].Value);
								ReferenceNo = Conversion.ToString(row.Cells["ReferenceNo"].Value);
								CustomerId = Conversion.ToInt(row.Cells["CustomerId"].Value);
								CustomerName = Conversion.ToString(row.Cells["CustomerName"].Value);
								WarehouseId = Conversion.ToInt(row.Cells["WarehouseId"].Value);
								WarehouseName = Conversion.ToString(row.Cells["WarehouseName"].Value);
								JobLotId = Conversion.ToInt(row.Cells["JobLotId"].Value);
								JobLot = Conversion.ToString(row.Cells["JobLot"].Value);
								ItemAttributeVarientId = Conversion.ToInt(row.Cells["ItemAttributeVarientId"].Value);
								AttributeVarient = Conversion.ToString(row.Cells["AttributeVarient"].Value);
								VarientEquivalent = Conversion.ToDecimal(row.Cells["VarientEquivalent"].Value);
								ItemUomId = Conversion.ToInt(row.Cells["ItemUomId"].Value);
								Uom = Conversion.ToDecimal(row.Cells["Uom"].Value);
								UomCode = Conversion.ToString(row.Cells["UomCode"].Value);
								ItemQty = Conversion.ToDecimal(row.Cells["ItemQty"].Value);
								VarientQty = Conversion.ToDecimal(row.Cells["VarientQty"].Value);
								NetWeight = Conversion.ToDecimal(row.Cells["NetWeight"].Value);
								Rate = Conversion.ToDecimal(row.Cells["Rate"].Value);
								Conversion.ToInt(row.Cells["RateUomId"].Value);
								break;
							}
						}
					}
					else
					{
						SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
						SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
						SaleOrder = Conversion.ToInt(r.Cells["SaleOrder"].Value);
						BomNo = Conversion.ToString(r.Cells["BomNo"].Value);
						ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
						CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
						CustomerName = Conversion.ToString(r.Cells["CustomerName"].Value);
						CustomerTypeId = Conversion.ToInt(r.Cells["CustomerTypeId"].Value);
						CustomerType = Conversion.ToString(r.Cells["CustomerType"].Value);
						ReferenceNo = Conversion.ToString(r.Cells["ReferenceNo"].Value);
						WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						WarehouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
						JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						JobLot = Conversion.ToString(r.Cells["JobLot"].Value);
						ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
						AttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
						VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
						ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						Uom = Conversion.ToDecimal(r.Cells["Uom"].Value);
						UomCode = Conversion.ToString(r.Cells["UomCode"].Value);
						ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						VarientQty = Conversion.ToDecimal(r.Cells["VarientQty"].Value);
						NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						Conversion.ToInt(r.Cells["RateUomId"].Value);
					}
				}
				GetDefaultJob(out JobLotId, out JobLot);
				dtOutPutByProduct.Rows.Add(0, BomHeaderId, BomNo, WorkOrderId, WorkOrderDetailId, WorkOrderNo, EntryTypeId, EntryType, CustomerTypeId, CustomerType, CustomerId, CustomerName, ReferenceNo, SaleOrderId, SaleOrderDetailId, SaleOrder, WarehouseId, WarehouseName, JobLotId, JobLot, ItemId, ItemName, ItemAttributeVarientId, AttributeVarient, VarientEquivalent, ItemUomId, Uom, UomCode, ItemQty, VarientQty, NetWeight, Rate, ItemUomId, Uom, UomCode, 0);
			}
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOutPutGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "BomHeaderId", "WorkOrderId", "WorkOrderDetailId", "EntryTypeId", "EntryType", "CustomerTypeId", "CustomerId", "SaleOrderId", "SaleOrderDetailId",
				"RateUomId", "ItemUomId", "UomCode", "RateUom", "RateUomCode", "NetWeight", "ItemId", "WarehouseId", "JobLotId", "JobLot",
				"ItemAttributeVarientId", "Uom"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ReferenceNo",
					(EditType)1
				},
				{
					"ItemQty",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdOutPutByProduct, 2, 4, hiddenColumns, editableColumns);
			grdOutPutByProduct.GroupByBoxVisible = true;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Caption = "Uom";
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdOutPutByProduct.RootTable.Columns["VarientQty"].Caption = "T.Qty";
			grdOutPutByProduct.RootTable.Columns["SaleOrder"].Caption = "Order";
			grdOutPutByProduct.RootTable.Columns["SaleOrder"].Caption = "Order";
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].Caption = "Varient";
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].Caption = "VarientUnit";
			grdOutPutByProduct.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdOutPutByProduct.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdOutPutByProduct.RootTable.Columns["BomNo"].Width = 70;
			grdOutPutByProduct.RootTable.Columns["WorkOrderNo"].Width = 80;
			grdOutPutByProduct.RootTable.Columns["EntryType"].Width = 100;
			grdOutPutByProduct.RootTable.Columns["CustomerType"].Width = 75;
			grdOutPutByProduct.RootTable.Columns["CustomerName"].Width = 130;
			grdOutPutByProduct.RootTable.Columns["ReferenceNo"].Width = 70;
			grdOutPutByProduct.RootTable.Columns["SaleOrder"].Width = 55;
			grdOutPutByProduct.RootTable.Columns["WarehouseName"].Width = 110;
			grdOutPutByProduct.RootTable.Columns["JobLot"].Width = 100;
			grdOutPutByProduct.RootTable.Columns["ItemName"].Width = 110;
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].Width = 55;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].Width = 55;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Width = 60;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].Width = 55;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].Width = 60;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Width = 90;
			grdOutPutByProduct.RootTable.Columns["Rate"].Width = 65;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Width = 60;
			grdOutPutByProduct.RootTable.Columns["Amount"].Width = 80;
			GridEX_Helper.AddButton(grdOutPutByProduct, "Delete", "X", 30, 0);
			GridEX_Helper.AddButton(grdOutPutByProduct, "Add", "+", 30, 1);
			grdOutPutByProduct.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOutPutByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPutByProduct.UpdateData();
			GridEXRow item = grdOutPutByProduct.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToInt(item.Cells["ItemId"].Value) == 0)
				{
					if (Conversion.ToInt(cmbItemBOM.Value) != 0)
					{
						item.Cells["ItemId"].Value = Conversion.ToInt(cmbItemBOM.Value);
						item.Cells["ItemName"].Value = Conversion.ToString((object)((Control)(object)cmbItemBOM).Text);
					}
					if (Conversion.ToInt(cmbUOM.Value) != 0)
					{
						item.Cells["ItemUomId"].Value = Conversion.ToInt(cmbUOM.Value);
						item.Cells["Uom"].Value = Conversion.ToInt(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
						item.Cells["UomCode"].Value = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[1].Value);
						item.Cells["RateUomId"].Value = Conversion.ToInt(cmbUOM.Value);
						item.Cells["RateUom"].Value = Conversion.ToInt(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
						item.Cells["RateUomCode"].Value = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[1].Value);
					}
				}
				if (Conversion.ToInt(item.Cells["ItemId"].Value) > 0 && Conversion.ToInt(item.Cells["ItemAttributeVarientId"].Value) == 0)
				{
					dtAttributes.Clear();
					int ItemAttributeVarientId = 0;
					dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(item.Cells["ItemId"].Value));
					if (dtAttributes.Rows.Count > 0)
					{
						DataRow firstRow = dtAttributes.AsEnumerable().FirstOrDefault();
						if (firstRow != null)
						{
							ItemAttributeVarientId = Conversion.ToInt(firstRow["ItemAttributeVarientId"]);
						}
						DataRow foundRow = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == ItemAttributeVarientId);
						string AttributeVarient = ((foundRow != null) ? Conversion.ToString(foundRow["ItemAttribute"]) : "");
						decimal VarientEquivalent2 = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
						item.Cells["ItemAttributeVarientId"].Value = ItemAttributeVarientId;
						item.Cells["AttributeVarient"].Value = AttributeVarient;
						item.Cells["VarientEquivalent"].Value = VarientEquivalent2;
					}
				}
				if (e.Column.Key == "ItemQty")
				{
					if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
						decimal VarientEquivalent = Conversion.ToDecimal((object)item.Cells["VarientEquivalent"].Value.ToString());
						decimal VarientQty = num * VarientEquivalent;
						item.Cells["VarientQty"].Value = VarientQty;
						decimal CurrentRowAmount = Conversion.ToDecimal(item.Cells["Rate"].Value) * VarientQty;
						item.Cells["Amount"].Value = CurrentRowAmount;
					}
					else
					{
						item.Cells["VarientQty"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				grdOutPutByProduct.UpdateData();
			}
			EnableDisableProductionType();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInOutPutByProductGrid();
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
				EnableDisableProductionType();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
	}

	private void GetOutPutGridBomNos()
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				dtOutputBom.Clear();
				GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (!dtOutputBom.AsEnumerable().Any((DataRow row) => row.Field<int>("BomHeaderId") == Conversion.ToInt(r.Cells["BomHeaderId"].Value)))
					{
						dtOutputBom.Rows.Add(Conversion.ToInt(r.Cells["BomHeaderId"].Value), Conversion.ToString(r.Cells["BomNo"].Value));
					}
				}
				return;
			}
			throw new Exception("Out put Grid Record No Found...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetOutPutGridItemsForWages(int BomId)
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				dtOutputItem.Clear();
				{
					foreach (GridEXRow r in from rr in grdOutPutByProduct.GetDataRows()
						where Conversion.ToInt(rr.Cells["BomHeaderId"].Value) == BomId
						select rr)
					{
						if (!dtOutputItem.AsEnumerable().Any((DataRow row) => row.Field<int>("OutItemId") == Conversion.ToInt(r.Cells["ItemId"].Value)))
						{
							dtOutputItem.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value));
						}
					}
					return;
				}
			}
			throw new Exception("Out put Grid Record No Found...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionOutPutDetail d1 = new ProductionOutPutDetail();
					d1.Id = Id;
					FillOutPutDetailListCommonForInsertAndDelete(d1, r);
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
				}
				return;
			}
			throw new Exception("Please Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadGridSetting()
	{
		try
		{
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Visible = false;
			grdOverHead.RootTable.Columns["BomOverHeadId"].Visible = false;
			grdOverHead.RootTable.Columns["BomHeaderId"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
			grdOverHead.GroupByBoxVisible = true;
			grdOverHead.RootTable.Groups.Add("BomNo");
			grdOverHead.RootTable.GroupTotals = (GroupTotals)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].FormatString = "##,#.####";
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TotalFormatString = "##,#.####";
			grdOverHead.RootTable.Columns["BomQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["BomQty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["BomQty"].FormatString = "##,#.####";
			grdOverHead.RootTable.Columns["BomQty"].TotalFormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (dtAccounts.Rows.Count > 0)
			{
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ColumnType = (ColumnType)0;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].LimitToList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].HasValueList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].EditType = (EditType)4;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Caption = "AccountTitle";
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 220;
			}
			CommonServices.GridAutoAdjustmentNew(grdOverHead);
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 20;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 20;
			grdOverHead.RootTable.Columns["Add"].Position = 1;
			grdOverHead.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			if (e.Column.Key == "PercentOfTotalInput")
			{
				double TotalPercentage = 0.0;
				int BomId = Conversion.ToInt(item.Cells["BomHeaderId"].Value);
				GridEXRow[] dataRows = grdOverHead.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (BomId == Conversion.ToInt(r.Cells["BomHeaderId"].Value))
					{
						TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
					}
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(item.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					item.Cells["PercentOfTotalInput"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...For Bom No: " + Conversion.ToString(item.Cells["BomNo"].Value));
				}
				double num2 = (from row in grdInputMaterial.GetDataRows()
					where Conversion.ToInt(row.Cells["BomHeaderId"].Value) == BomId
					select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["Amount"].Value));
				double Percentage = Conversion.ToDouble((object)item.Cells["PercentOfTotalInput"].Value.ToString());
				double TotlaPercentage = num2 / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Amount"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdOverHead.UpdateData();
			Calculation();
			CommonServices.GridAutoAdjustmentNew(grdOverHead);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOverHead.CurrentRow;
				dtOverHead.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdContractorWages.GetDataRows().Count() == 0)
			{
				dtContractorWages.Rows.Add(0, 0, "", 0, 0, "", 0, "", 0, "", 0, 0, 0, 0, "");
			}
			else
			{
				int BomHeaderId = 0;
				string BomNo = "";
				int OutPutItemId = 0;
				string OutPutItemName = "";
				int ContractorId = 0;
				string ContractorName = "";
				GridEXRow r = grdContractorWages.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (BomHeaderId == 0 && OutPutItemId == 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow row in dataRows)
						{
							BomHeaderId = Conversion.ToInt(row.Cells["BomHeaderId"].Value);
							OutPutItemId = Conversion.ToInt(row.Cells["OutPutItemId"].Value);
							if (BomHeaderId != 0 && OutPutItemId != 0)
							{
								BomNo = Conversion.ToString(row.Cells["BomNo"].Value);
								OutPutItemName = Conversion.ToString(row.Cells["OutPutItemName"].Value);
								ContractorId = Conversion.ToInt(row.Cells["ContractorId"].Value);
								ContractorName = Conversion.ToString(row.Cells["ContractorName"].Value);
								break;
							}
						}
					}
					else
					{
						BomNo = Conversion.ToString(r.Cells["BomNo"].Value);
						OutPutItemName = Conversion.ToString(r.Cells["OutPutItemName"].Value);
						ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
						ContractorName = Conversion.ToString(r.Cells["ContractorName"].Value);
					}
				}
				dtContractorWages.Rows.Add(0, BomHeaderId, BomNo, 0, OutPutItemId, OutPutItemName, ContractorId, ContractorName, 0, "", 0, 0, 0, 0, "");
			}
			grdContractorWages.DataSource = dtContractorWages;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdContractorWagesComboFill()
	{
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWagesComboFill();
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["BomWagesId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["BomHeaderId"].Visible = false;
			grdContractorWages.RootTable.Columns["OutPutItemId"].Visible = false;
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomWagesId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OutPutItemId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OutPutItemName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdContractorWages.GroupByBoxVisible = true;
			grdContractorWages.RootTable.Groups.Add("BomNo");
			grdContractorWages.RootTable.GroupTotals = (GroupTotals)2;
			grdContractorWages.RootTable.Columns["BomQty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["BomQty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["BomQty"].FormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["BomQty"].TotalFormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 20;
			grdContractorWages.RootTable.Columns["Delete"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 20;
			grdContractorWages.RootTable.Columns["Add"].Position = 1;
			grdContractorWages.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			grdContractorWages.UpdateData();
			Calculation();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdContractorWages.CurrentRow;
				dtContractorWages.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			EnableDisableItemName();
			Calculation();
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableItemName()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0)
			{
				GridEXRow r = grdContractorWages.GetRow(0);
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					((Control)(object)cmbItemBOM).Enabled = false;
				}
				else
				{
					((Control)(object)cmbItemBOM).Enabled = true;
				}
			}
			else
			{
				((Control)(object)cmbItemBOM).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillWagesGridFilterCombo()
	{
		try
		{
			int Id1 = Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value);
			string Id2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
			if (grdContractorWages.GetDataRows().Count() > 0 && dtContractorWages.Rows.Count > 0)
			{
				DataTable distinctRowsActivity = (from row in dtContractorWages.AsEnumerable()
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
			throw new Exception(ex.Message);
		}
	}

	private void ResetDetailOutPut()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbItemBOM).Text = "";
			((UltraGridBase)cmbBOMNo).DataSource = null;
			((UltraGridBase)cmbUOM).DataSource = null;
			((Control)(object)cmbBOMNo).Text = "";
			((Control)(object)cmbUOM).Text = "";
			txtQty.Text = "";
			((UltraGridBase)CmbAttribute).DataSource = null;
			txtVarientEquivalent.Text = "";
			((Control)(object)CmbAttribute).Text = "";
			((Control)(object)CmbWarehouseName).Text = "";
			((Control)(object)CmbAttribute).Visible = true;
			txtVarientEquivalent.Visible = true;
			((Control)(object)CmbWarehouseName).Visible = true;
			BtnAddDetail.Visible = true;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			CmbWarehouseName.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	public void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			DocumentNoFill();
			((Control)(object)cmbBOMNo).Text = string.Empty;
			PreviousWorkOrderId = 0;
			((Control)(object)CmbWorkOrderNo).Text = string.Empty;
			((Control)(object)cmbBOMNo).Text = string.Empty;
			((Control)(object)cmbItemBOM).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			dtInput.Rows.Clear();
			grdInputMaterial.ClearStructure();
			dtOverHead.Rows.Clear();
			grdOverHead.ClearStructure();
			dtContractorWages.Rows.Clear();
			grdContractorWages.ClearStructure();
			lstRemoveRecordInput.Clear();
			lstRemoveRecordOutPut.Clear();
			dtOutPutByProduct.Rows.Clear();
			grdOutPutByProduct.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			txtTotalInputs.Text = string.Empty;
			txtOverHead.Text = string.Empty;
			txtWagesAmount.Text = string.Empty;
			txtTotalOutPutAutoRatedAmount.Text = string.Empty;
			txtOutPutAutoRatePerQtyAmount.Text = string.Empty;
			EnableDisableProductionType();
			EnableDisableItemName();
			FillWagesGridFilterCombo();
			EnableDisablePlant();
			WorkOrderDbCall("");
			WorkOrderNoFill();
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "UomSchedule", "SupplierCustomerLists", "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			ItemdtFillFromBomDbCall();
			ItemNameBind();
			BOMNoAndItemFill();
			dtFillsForGrids();
			PlantBind(PlantFeaderDbCall());
			WIPWarehouseFill();
			WIPAccountFill();
			WagesExpenseAccountFill();
			FillWagesGridFilterCombo();
			string Ids = GridEX_Helper.GetAllValuesOfColumn<int>(grdOutPutByProduct, "WorkOrderDetailId", null, null, distinct: true);
			WorkOrderDbCall(Ids);
			WorkOrderNoFill();
			SupplierDtFillFromGlobal();
			BindSupplierName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmConcreteProduction_FormClosed(object sender, FormClosedEventArgs e)
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

	public void Calculation()
	{
		try
		{
			double sumoftotalInputs = GridEX_Helper.GetColumnSum(grdInputMaterial, "Amount");
			double sumofOverhead = GridEX_Helper.GetColumnSum(grdOverHead, "Amount");
			double ManualRatedAmount = Conversion.ToDouble((object)(from GridEXRow row in grdOutPutByProduct.GetDataRows()
				where Conversion.ToInt(row.Cells["EntryTypeId"].Value) != 1
				select (decimal)row.Cells["Amount"].Value).Sum());
			double sumofContractorWages = GridEX_Helper.GetColumnSum(grdContractorWages, "Amount");
			txtTotalInputs.Text = Math.Round(sumoftotalInputs, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalOutPutManualRatedAmount.Text = Math.Round(ManualRatedAmount, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtOverHead.Text = Math.Round(sumofOverhead, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtWagesAmount.Text = Math.Round(sumofContractorWages, 2).ToString(clsGlobalVariables.stringFormatsingle);
			double NetValue = sumoftotalInputs + sumofOverhead + sumofContractorWages - ManualRatedAmount;
			txtTotalOutPutAutoRatedAmount.Text = NetValue.ToString(clsGlobalVariables.stringFormatsingle);
			double OutPutAutoRateItemQty = Conversion.ToDouble((object)(from GridEXRow row in grdOutPutByProduct.GetDataRows()
				where Conversion.ToInt(row.Cells["EntryTypeId"].Value) == 1
				select (decimal)row.Cells["VarientQty"].Value).Sum());
			txtQty.Text = Math.Round(OutPutAutoRateItemQty, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtOutPutAutoRatePerQtyAmount.Text = Math.Round(NetValue / OutPutAutoRateItemQty, 2).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void IsDuplicateRowExists()
	{
		try
		{
			Dictionary<string, GridEXRow> uniqueKeys = new Dictionary<string, GridEXRow>();
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int bomHeaderId = Conversion.ToInt(item.Cells["BomHeaderId"].Value);
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int itemAttributeVarientId = Conversion.ToInt(item.Cells["ItemAttributeVarientId"].Value);
				int warehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				int rCustomerId = Conversion.ToInt(item.Cells["CustomerId"].Value);
				string rReferenceNo = Conversion.ToString(item.Cells["ReferenceNo"].Value);
				string key = $"{bomHeaderId}-{itemId}-{itemAttributeVarientId}-{warehouseId}-{rCustomerId}-{rReferenceNo}";
				if (uniqueKeys.TryGetValue(key, out var existingRow))
				{
					string itemName = Conversion.ToString((object)item.Cells["ItemName"].Text);
					string attributeVarient = Conversion.ToString((object)item.Cells["AttributeVarient"].Text);
					string bomNo = Conversion.ToString((object)item.Cells["BomNo"].Text);
					string warehouseName = Conversion.ToString((object)item.Cells["WarehouseName"].Text);
					string Customer = Conversion.ToString((object)item.Cells["CustomerName"].Text);
					throw new Exception("Duplicate Record not added to the grid.\nItem " + itemName + " with BOM No " + bomNo + ", Variant " + attributeVarient + ", Warehouse " + warehouseName + ", Customer " + Customer + ", Reference No " + rReferenceNo + " " + $"exists in row No {item.RowIndex + 1} and Row No {existingRow.RowIndex + 1}");
				}
				uniqueKeys[key] = item;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void CheckBomHeaderIds(DataTable dtOutput, DataTable dtInput, string TargetGridName = "Output", string SourceGridName = "Input", int CheckSpecific = 0)
	{
		try
		{
			HashSet<int> TargetIds = (from row in dtOutput.AsEnumerable()
				where row.Field<int>("BomHeaderId") > 0
				select row.Field<int>("BomHeaderId")).ToHashSet();
			HashSet<int> SourceIds = (from row in dtInput.AsEnumerable()
				where row.Field<int>("BomHeaderId") > 0
				select row.Field<int>("BomHeaderId")).ToHashSet();
			if (CheckSpecific == 0 || CheckSpecific == 1)
			{
				List<int> missingInTargetButExistsInSource = SourceIds.Except(TargetIds).ToList();
				if (missingInTargetButExistsInSource.Any())
				{
					IEnumerable<string> missingBomNos = (from row in dtInput.AsEnumerable()
						where missingInTargetButExistsInSource.Contains(row.Field<int>("BomHeaderId"))
						select row.Field<string>("BomNo")).ToList().Distinct();
					throw new Exception("BomHeaderNo " + string.Join(", ", missingBomNos) + " found in " + SourceGridName + " Grid but not in " + TargetGridName + " Grid.");
				}
			}
			if (CheckSpecific != 0 && CheckSpecific != 2)
			{
				return;
			}
			List<int> missingInSourceBuExistsInTarget = TargetIds.Except(SourceIds).ToList();
			if (missingInSourceBuExistsInTarget.Any())
			{
				List<string> missingBomNos2 = (from row in dtOutput.AsEnumerable()
					where missingInSourceBuExistsInTarget.Contains(row.Field<int>("BomHeaderId"))
					select row.Field<string>("BomNo")).ToList();
				throw new Exception("BomHeaderNo " + string.Join(", ", missingBomNos2) + " found in " + TargetGridName + " Grid but not in " + SourceGridName + " Grid.");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Expected O, but got Unknown
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInputMaterial.GetDataRows().Count() == 0 || grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");
			}
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ValidateItemTypeOfType(dtOutPutByProduct);
			IsDuplicateRowExists();
			CheckBomHeaderIds(dtOutPutByProduct, dtInput);
			CheckBomHeaderIds(dtInput, dtOverHead, "InPut", "Over Head", 1);
			CheckBomHeaderIds(dtInput, dtContractorWages, "InPut", "Wages", 1);
			btnGenerateRatesOutput_Click(null, null);
			if (!ContractorWagesDebitTotExpenseAcOnProductionConcrete)
			{
				decimal sumoftotalOutPut2 = Conversion.ToDecimal((object)grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				decimal TotalOutPutAutoRatedAmount2 = Conversion.ToDecimal((object)txtTotalOutPutAutoRatedAmount.Text);
				if (Math.Round(TotalOutPutAutoRatedAmount2, 2) != Math.Round(sumoftotalOutPut2, 2))
				{
					throw new Exception($"Net Vale {TotalOutPutAutoRatedAmount2} In Header and Total OutPut Amount {sumoftotalOutPut2} are not equal");
				}
			}
			else
			{
				decimal sumoftotalOutPut = Conversion.ToDecimal((object)grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				decimal TotalOutPutAutoRatedAmount = Conversion.ToDecimal((object)txtTotalOutPutAutoRatedAmount.Text) - Conversion.ToDecimal((object)txtWagesAmount.Text);
				if (Math.Round(TotalOutPutAutoRatedAmount, 2) != Math.Round(sumoftotalOutPut, 2))
				{
					throw new Exception($"Net Vale {TotalOutPutAutoRatedAmount} In Header and Total OutPut Amount {sumoftotalOutPut} are not equal");
				}
			}
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader.Id = RecId;
			ProductionHeader.CompanyId = UserAccount.CompanyId;
			ProductionHeader.BranchesId = UserAccount.BranchesId;
			ProductionHeader.ProjectsId = UserAccount.BranchesId;
			ProductionHeader.OrganizationId = UserAccount.OrganizationId;
			ProductionHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ProductionHeader.DocumentTypeId = 1850;
			ProductionHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			ProductionHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ProductionHeader.WorkOrderId = Conversion.ToInt(CmbWorkOrderNo.Value);
			ProductionHeader.ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			ProductionHeader.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			ProductionHeader.PlantId = Conversion.ToInt(CmbPlantName.Value);
			GridEXRow rrr = grdOutPutByProduct.CurrentRow;
			if (rrr != null && (int)rrr.RowType == 0 && rrr.Cells.Count > 0)
			{
				ProductionHeader.ItemId = Conversion.ToInt(rrr.Cells["ItemId"].Value);
				ProductionHeader.ItemUomId = Conversion.ToInt(rrr.Cells["ItemUomId"].Value);
				ProductionHeader.BomHeaderId = Conversion.ToInt(rrr.Cells["BomHeaderId"].Value);
				ProductionHeader.ItemQty = Conversion.ToDecimal(rrr.Cells["ItemQty"].Value);
			}
			ProductionHeader.WIPWarehouseId = Conversion.ToInt(CmbWIPWarehouse.Value);
			ProductionHeader.WIPGLAccountID = Conversion.ToInt(CmbWIPAccount.Value);
			ProductionHeader.WagesExpenseAccountId = Conversion.ToInt(CmbWagesExpenseAccount.Value);
			ProductionHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ProductionHeader.EntryUserId = UserAccount.ID;
			ProductionHeader.EntryDate = DateTime.Now;
			ProductionHeader.ModifyDate = DateTime.Now;
			ProductionHeader.ModifyUserId = UserAccount.ID;
			ProductionHeader.ApprovedUserId = UserAccount.ID;
			ProductionHeader.ApprovedDate = DateTime.Now;
			ProductionHeader.IsApproved = false;
			ProductionHeader.ProductionInputDetailList = new List<ProductionInputDetail>();
			ProductionHeader.ProductionOverHeadsDetailList = new List<ProductionOverHeadsDetail>();
			ProductionHeader.ProductionOutPutDetailList = new List<ProductionOutPutDetail>();
			ProductionHeader.ProductionWagesDetailList = new List<ProductionWagesDetail>();
			ProductionHeader.AttachmentsList = new List<DMSAttachments>();
			if (lstRemoveRecordInput.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecordInput.Count; j++)
				{
					ProductionHeader.ProductionInputDetailList.Add(lstRemoveRecordInput[j]);
				}
			}
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r4 in dataRows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					ProductionInputDetail d3 = new ProductionInputDetail();
					d3.Id = ((RecId != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
					d3.ActionTypeId = ((d3.Id <= 0) ? 1 : 2);
					FillInPutDetailListCommonForInsertAndDelete(d3, r4);
					FormHelper.ValidateField(d3.WareHouseId, "Warehouse", r4.RowIndex);
					FormHelper.ValidateField(d3.JobLotId, "JobLotId", r4.RowIndex);
					FormHelper.ValidateField(d3.ItemId, "Item", r4.RowIndex);
					FormHelper.ValidateField(d3.ItemUomId, "ItemUom", r4.RowIndex);
					FormHelper.ValidateField(d3.ItemQty, "Qty", r4.RowIndex);
					FormHelper.ValidateField(d3.Rate, "Rate", r4.RowIndex);
					FormHelper.ValidateField(d3.RateUomId, "RateUom", r4.RowIndex);
					FormHelper.ValidateField(d3.Amount, "Amount", r4.RowIndex);
					ProductionHeader.ProductionInputDetailList.Add(d3);
				}
			}
			if (lstRemoveRecordOutPut.Count > 0 && RecId > 0)
			{
				for (int k = 0; k < lstRemoveRecordOutPut.Count; k++)
				{
					ProductionHeader.ProductionOutPutDetailList.Add(lstRemoveRecordOutPut[k]);
				}
			}
			bool WorkOrderIdExists = false;
			dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					ProductionOutPutDetail vd = new ProductionOutPutDetail();
					vd.Id = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
					vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
					FillOutPutDetailListCommonForInsertAndDelete(vd, r3);
					FormHelper.ValidateField(vd.BOMHeaderId, "Bom No", r3.RowIndex);
					if (vd.WorkOrderId > 0)
					{
						FormHelper.ValidateField(vd.CustomerName, "Customer", r3.RowIndex);
					}
					FormHelper.ValidateField(vd.WareHouseId, "Warehouse", r3.RowIndex);
					FormHelper.ValidateField(vd.JobLotId, "JobLotId", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemId, "Item", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemAttributeVarientId, "Item Attribute Varient", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemAttributeVarientId, "AttributeVarient", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemQty, "Qty", r3.RowIndex);
					FormHelper.ValidateField(vd.VarientQty, "VarientQty", r3.RowIndex);
					FormHelper.ValidateField(vd.Rate, "Rate", r3.RowIndex);
					FormHelper.ValidateField(vd.Amount, "Amount", r3.RowIndex);
					ProductionHeader.ProductionOutPutDetailList.Add(vd);
					if (!WorkOrderIdExists)
					{
						WorkOrderIdExists = vd.WorkOrderDetailId > 0;
					}
					continue;
				}
				MessageBox.Show("Item Field Required in Grid output Row No: " + (r3.RowIndex + 1));
				return;
			}
			if (!WorkOrderIdExists)
			{
				ProductionHeader.WorkOrderId = 0;
			}
			dataRows = grdOverHead.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				if (Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) > 0)
				{
					ProductionOverHeadsDetail d2 = new ProductionOverHeadsDetail();
					if (r2.Cells["OverHeadAccountId"].Text == string.Empty || Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) == 0)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					d2.BomOverHeadId = Conversion.ToInt(r2.Cells["BomOverHeadId"].Value);
					d2.OverHeadTypeId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					d2.OverHeadAccountId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					if (r2.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.PercentOfTotalInput = Conversion.ToDecimal((object)r2.Cells["PercentOfTotalInput"].Text);
					d2.Qty = Conversion.ToDecimal((object)r2.Cells["Qty"].Text);
					d2.BomQty = Conversion.ToDecimal((object)r2.Cells["BomQty"].Text);
					d2.Rate = Conversion.ToDecimal((object)r2.Cells["Rate"].Text);
					d2.Amount = Conversion.ToDecimal((object)r2.Cells["Amount"].Text);
					d2.OhRemarks = Conversion.ToString((object)r2.Cells["OhRemarks"].Text);
					d2.BomHeaderId = Conversion.ToInt(r2.Cells["BomHeaderId"].Value);
					ProductionHeader.ProductionOverHeadsDetailList.Add(d2);
				}
			}
			dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					ProductionWagesDetail d = new ProductionWagesDetail();
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.BomWagesId = Conversion.ToInt(r.Cells["BomWagesId"].Value);
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.BomQty = Conversion.ToDecimal((object)r.Cells["BomQty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					d.BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					d.OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					ProductionHeader.ProductionWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				ProductionHeader.AttachmentsList.Add(attach);
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
			int success = ProductionHeader.Save(ProductionHeader);
			ProductionHeader.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ProductionHeader.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int l = 0; l < AT.RemovedAttachmentListInUpdateCase.Count; l++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[l].Path, FileMode.Open, FileAccess.ReadWrite))
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
								File.Delete(AT.RemovedAttachmentListInUpdateCase[l].Path);
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
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ProductionHeader.DocNo);
			}
			FromReset();
			if (ChkPreviewSlip.Checked && formright.DoHavePrintRights)
			{
				CommonServices.ProductionSlipConcrete(ProductionHeader.Id);
			}
			if (chkPrintVoucher.Checked && formright.DoHavePrintRights)
			{
				VoucherReprot(ProductionHeader.Id);
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
			WF.Close();
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
			WF.Close();
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

	public void ReadById(int Id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			FromReset();
			ProductionHeader ProductionHeader = new ProductionHeader();
			RecId = Id;
			ProductionHeader = ProductionHeader.ReadById(RecId);
			if (ProductionHeader == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = ProductionHeader.DocDate;
			txtdocno.Text = Conversion.ToString((object)ProductionHeader.DocNo);
			CmbWorkOrderNo.Value = ProductionHeader.WorkOrderId;
			PreviousWorkOrderId = ProductionHeader.WorkOrderId;
			CmbProductionType.Value = ProductionHeader.ProductionTypeId;
			CmbProductionType_Leave(null, null);
			CmbPlantName.Value = ProductionHeader.PlantId;
			CmbPlantName_Leave(null, null);
			txtProductionNo.Text = ProductionHeader.ProductionNo;
			txtRemarksHeader.Text = Conversion.ToString((object)ProductionHeader.RemarksHeader);
			cmbItemBOM.Value = ProductionHeader.ItemId;
			cmbItem_Leave(null, null);
			cmbBOMNo.Value = ProductionHeader.BomHeaderId;
			cmbBOMNo_Leave(null, null);
			cmbUOM.Value = ProductionHeader.ItemUomId;
			txtQty.Text = ProductionHeader.ItemQty.ToString();
			CmbWIPWarehouse.Value = Conversion.ToInt((object)ProductionHeader.WIPWarehouseId);
			CmbWIPAccount.Value = Conversion.ToInt((object)ProductionHeader.WIPGLAccountID);
			CmbWagesExpenseAccount.Value = Conversion.ToInt((object)ProductionHeader.WagesExpenseAccountId);
			if (ProductionHeader.ProductionOutPutDetailList.Count > 0)
			{
				dtOutPutByProduct.Rows.Clear();
				FillOutputDetailFromListCommonForReadById(ProductionHeader, dtOutPutByProduct);
			}
			else
			{
				AddRowInOutPutByProductGrid();
			}
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				dtInput.Clear();
				FillInputDetailFromListCommonForReadById(ProductionHeader, dtInput);
			}
			else
			{
				grdInputMaterial.ClearStructure();
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			if (ProductionHeader.ProductionOverHeadsDetailList.Count > 0)
			{
				dtOverHead.Clear();
				FillOhDetailFromListCommonForReadById(ProductionHeader, dtOverHead);
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			if (ProductionHeader.ProductionWagesDetailList.Count > 0)
			{
				dtContractorWages.Clear();
				FillWagesDetailFromListCommonForReadById(ProductionHeader, dtContractorWages);
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.DataSource = dtContractorWages;
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			EnableDisableItemName();
			EnableDisableProductionType();
			Calculation();
			FillWagesGridFilterCombo();
			EnableDisablePlant();
			string Ids = GridEX_Helper.GetAllValuesOfColumn<int>(grdOutPutByProduct, "WorkOrderDetailId", null, null, distinct: true);
			WorkOrderDbCall(Ids);
			WorkOrderNoFill();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProductionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FromReset();
				}
				return;
			}
			throw new Exception("No record found to Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOutPutDetailListCommonForInsertAndDelete(ProductionOutPutDetail vd, GridEXRow r)
	{
		vd.BOMHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
		vd.BOMHeaderNo = Conversion.ToString(r.Cells["BomNo"].Value);
		vd.WorkOrderId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
		vd.WorkOrderDetailId = Conversion.ToInt(r.Cells["WorkOrderDetailId"].Value);
		vd.WorkOrderNo = Conversion.ToString(r.Cells["WorkOrderNo"].Value);
		vd.EntryTypeId = 1;
		vd.EntryType = "Auto Rate";
		vd.CustomerTypeId = Conversion.ToInt(r.Cells["CustomerTypeId"].Value);
		vd.CustomerType = Conversion.ToString(r.Cells["CustomerType"].Value);
		vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
		vd.CustomerName = Conversion.ToString(r.Cells["CustomerName"].Value);
		vd.ReferenceNo = Conversion.ToString(r.Cells["ReferenceNo"].Value);
		vd.SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
		vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
		vd.SaleOrderNo = Conversion.ToInt((object)r.Cells["SaleOrder"].Text);
		vd.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		vd.WareHouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLotDescription = Conversion.ToString(r.Cells["JobLot"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
		vd.ItemAttribute = Conversion.ToString((object)r.Cells["AttributeVarient"].Text);
		vd.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
		vd.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
		vd.ItemUomCode = Conversion.ToString(r.Cells["UomCode"].Value);
		vd.ItemUomEquivalent = Conversion.ToDouble(r.Cells["Uom"].Value);
		vd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
		vd.VarientQty = Conversion.ToDecimal(r.Cells["VarientQty"].Value);
		vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
		vd.RateUomCode = Conversion.ToString(r.Cells["RateUomCode"].Value);
		vd.RateUomEquivalent = Conversion.ToDouble(r.Cells["RateUom"].Value);
		vd.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.Remarks = "";
	}

	private void FillInPutDetailListCommonForInsertAndDelete(ProductionInputDetail vd, GridEXRow r)
	{
		vd.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
		vd.BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
		vd.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		vd.WareHouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLotDescription = Conversion.ToString(r.Cells["JobLot"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
		if (dt2.Rows.Count > 0)
		{
			DataRow[] dr2 = dt2.Select("Id='" + Conversion.ToInt(r.Cells["ItemUomId"].Value) + "'");
			if (dr2.Length == 0)
			{
				MessageBox.Show("UOM in Input Grid Row No: " + (r.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + ". Please Check...");
				return;
			}
			vd.ItemUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
		}
		vd.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
		vd.MinQty = Conversion.ToDecimal((object)r.Cells["MinQty"].Text);
		vd.MaxQty = Conversion.ToDecimal((object)r.Cells["MaxQty"].Text);
		vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		DataTable dt1 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
		if (dt1.Rows.Count > 0)
		{
			DataRow[] dr = dt1.Select("Id='" + Conversion.ToInt(r.Cells["RateUomId"].Value) + "'");
			if (dr.Length == 0)
			{
				MessageBox.Show("RateUom in Input Grid Row No: " + (r.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + ". Please Check...");
				return;
			}
			vd.RateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
		}
		vd.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.Remarks = "";
	}

	private void FillOutputDetailFromListCommonForReadById(ProductionHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.ProductionOutPutDetailList?.Count == 0)
		{
			return;
		}
		foreach (ProductionOutPutDetail detail in invGrn.ProductionOutPutDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BOMHeaderId, detail.BOMHeaderNo, detail.WorkOrderId, detail.WorkOrderDetailId, detail.WorkOrderNo, detail.EntryTypeId, detail.EntryType, detail.CustomerTypeId, detail.CustomerType,
				detail.CustomerId, detail.CustomerName, detail.ReferenceNo, detail.SaleOrderId, detail.SaleOrderDetailId, detail.SaleOrderNo, detail.WareHouseId, detail.WareHouseName, detail.JobLotId, detail.JobLotDescription,
				detail.ItemId, detail.ItemName, detail.ItemAttributeVarientId, detail.ItemAttribute, detail.VarientEquivalent, detail.ItemUomId, detail.ItemUomEquivalent, detail.ItemUomCode, detail.ItemQty, detail.VarientQty,
				detail.NetWeight, detail.Rate, detail.RateUomId, detail.RateUomEquivalent, detail.RateUomCode, detail.Amount
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillInputDetailFromListCommonForReadById(ProductionHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.ProductionInputDetailList?.Count == 0)
		{
			return;
		}
		foreach (ProductionInputDetail detail in invGrn.ProductionInputDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BomHeaderId, detail.BomHeaderNo, detail.BomInputId, detail.WareHouseId, detail.WareHouseName, detail.JobLotId, detail.JobLotDescription, detail.ItemId, detail.ItemName,
				detail.ItemUomId, detail.ItemUomEquivalent, detail.ItemUomCode, detail.ItemQty, detail.BomQty, detail.ItemQty, detail.NetWeight, detail.BomRate, detail.Rate, detail.RateUomId,
				detail.RateUomEquivalent, detail.RateUomCode, detail.Amount, detail.BomMinQty, detail.BomMaxQty, detail.MinQty, detail.MaxQty, detail.BomMainItemId
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillOhDetailFromListCommonForReadById(ProductionHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.ProductionOverHeadsDetailList?.Count == 0)
		{
			return;
		}
		foreach (ProductionOverHeadsDetail detail in invGrn.ProductionOverHeadsDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BomHeaderId, detail.BomHeaderNo, detail.BomOverHeadId, detail.OverHeadTypeId, detail.OverHeadAccountId, detail.PercentOfTotalInput, detail.BomQty, detail.Qty, detail.Rate,
				detail.Amount, detail.OhRemarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillWagesDetailFromListCommonForReadById(ProductionHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.ProductionWagesDetailList?.Count == 0)
		{
			return;
		}
		foreach (ProductionWagesDetail detail in invGrn.ProductionWagesDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BomHeaderId, detail.BomHeaderNo, detail.BomWagesId, detail.OutPutItemId, detail.OutPutItemName, detail.ContractorId, detail.ContractorName, detail.ContractorWagesRateScheduleId, detail.ServiceActivity,
				detail.BomQty, detail.Qty, detail.Rate, detail.Amount, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
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

	private void txtPercentToupdateInOhGrid_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProductionSlipConcrete(RecId);
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
			VoucherReprot(RecId);
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
			CommonServices.VoucherReport_118(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1850)), 1850);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Idtype = Conversion.ToInt(cmbProductionNoHistory.Value);
			DataTable dtcombo = ProductionHeader.GetDataForDropDownFromProductionHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtProductionNo = new DataTable();
			dtProductionNo.Columns.Add("Id", typeof(int));
			dtProductionNo.Columns.Add("ProductionNo");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionNo")
				{
					dtProductionNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtProductionNo.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtProductionNo, cmbProductionNoHistory, "Id", "ProductionNo", "Production No", ZeroIndex: true);
			((UltraGridBase)cmbProductionNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Idtype > 0)
			{
				if (dtProductionNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idtype))
				{
					cmbProductionNoHistory.Value = Idtype;
				}
				else
				{
					((Control)(object)cmbProductionNoHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				DateTime now = DateTime.Now;
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			Datetypefill();
			AllCombobind();
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbProductionNoHistory).Text = string.Empty;
			grdHistoryOverHead.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
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
			WF.Show();
			HistoryGridFill();
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

	private void HistoryGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistoryFromDb.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1850;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			if (txtFromdateHistory.Checked)
			{
				header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			header.Id = Conversion.ToInt(cmbProductionNoHistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (RadReffered.Checked)
			{
				header.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				header.ActionId = 2;
			}
			else if (RadAll.Checked)
			{
				header.ActionId = 0;
			}
			dtHistoryFromDb = ProductionHeader.FormHistory(header);
			if (dtHistoryFromDb.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("RecordNo", typeof(int));
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("DocNo", typeof(string));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ProductionNo", typeof(string));
					dtHistoryGrid.Columns.Add("ReleaseNo", typeof(int));
					dtHistoryGrid.Columns.Add("ProductionType", typeof(string));
					dtHistoryGrid.Columns.Add("PlantName", typeof(string));
					dtHistoryGrid.Columns.Add("WorkOrderId", typeof(int));
					dtHistoryGrid.Columns.Add("BomHeaderId", typeof(int));
					dtHistoryGrid.Columns.Add("BomHeaderNo", typeof(string));
					dtHistoryGrid.Columns.Add("ItemId", typeof(int));
					dtHistoryGrid.Columns.Add("ItemName", typeof(string));
					dtHistoryGrid.Columns.Add("ItemCode", typeof(string));
					dtHistoryGrid.Columns.Add("UOM", typeof(string));
					dtHistoryGrid.Columns.Add("ItemQty", typeof(decimal));
					dtHistoryGrid.Columns.Add("WipWareHouseName", typeof(string));
					dtHistoryGrid.Columns.Add("WIPAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryGrid.Rows.Clear();
				foreach (DataRow row in dtHistoryFromDb.Rows)
				{
					dtHistoryGrid.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["ReleaseNo"], row["ProductionType"], row["PlantName"], row["WorkOrderId"], row["BomHeaderId"], row["BomHeaderNo"], row["ItemId"], row["ItemName"], row["ItemCode"], row["UOM"], row["ItemQty"], row["WareHouseName"], row["WIPAccountTitle"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtHistoryGrid;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "DocumentTypeId", "WorkOrderId", "BomHeaderId", "ItemId", "BomHeaderId", "BomHeaderNo", "ItemId", "ItemName", "ItemCode",
				"UOM", "ItemQty"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistory, 3, 2, hiddenColumns);
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(DatagridHistory, "Print", "Print", 50, 0);
			GridEX_Helper.AddButton(DatagridHistory, "Voucher", "Voucher", 50, 1);
			GridEX_Helper.AddButton(DatagridHistory, "Edit", "Edit", 50, 2);
			DatagridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value);
				int ReleaseNo = Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value);
				if (!formright.DoHaveUpdateRights)
				{
					WF.Close();
					throw new Exception("you don't have update rights...");
				}
				if (ReleaseNo > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				WF.Show();
				ReadById(CurrId);
			}
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

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				CommonServices.ProductionSlipConcrete(CurrId);
			}
			if (e.Column.Key == "Voucher")
			{
				VoucherReprot(CurrId);
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					WF.Close();
					throw new Exception("ypu don't have updae rights...");
				}
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					WF.Close();
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					WF.Show();
					ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
			}
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

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1850);
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
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			grdHistoryInput.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryOverHead.ClearStructure();
			grdWagesHistory.ClearStructure();
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader = ProductionHeader.ReadById(Id);
			if (ProductionHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			DataTable dtWagesHistory = new DataTable();
			new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("BomNo", typeof(string));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("JobLot", typeof(string));
			dtInputHistory.Columns.Add("ItemId", typeof(int));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("BomNo", typeof(string));
			dtOutPutByProductHistory.Columns.Add("EntryType", typeof(string));
			dtOutPutByProductHistory.Columns.Add("SaleOrderId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("SaleOrder", typeof(int));
			dtOutPutByProductHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("JobLot", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("ItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("AttributeVarient", typeof(string));
			dtOutPutByProductHistory.Columns.Add("VarientUnit", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("VarientQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Id", typeof(int));
			dtOverHeadHistory.Columns.Add("BomNo", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadType", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOverHeadHistory.Columns.Add("Percentage", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Qty", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Rate", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("OhRemarks", typeof(string));
			dtWagesHistory.Columns.Add("Id", typeof(int));
			dtWagesHistory.Columns.Add("BomNo");
			dtWagesHistory.Columns.Add("ContractorName");
			dtWagesHistory.Columns.Add("ItemName");
			dtWagesHistory.Columns.Add("ServiceActivity");
			dtWagesHistory.Columns.Add("Qty", typeof(decimal));
			dtWagesHistory.Columns.Add("Rate", typeof(decimal));
			dtWagesHistory.Columns.Add("Amount", typeof(decimal));
			dtWagesHistory.Columns.Add("Remarks");
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				for (int l = 0; l < ProductionHeader.ProductionInputDetailList.Count; l++)
				{
					dtInputHistory.Rows.Add(ProductionHeader.ProductionInputDetailList[l].Id, ProductionHeader.ProductionInputDetailList[l].BomHeaderNo, ProductionHeader.ProductionInputDetailList[l].WareHouseName, ProductionHeader.ProductionInputDetailList[l].JobLotDescription, ProductionHeader.ProductionInputDetailList[l].ItemId, ProductionHeader.ProductionInputDetailList[l].ItemName, ProductionHeader.ProductionInputDetailList[l].ItemUomCode, ProductionHeader.ProductionInputDetailList[l].ItemQty, ProductionHeader.ProductionInputDetailList[l].NetWeight, ProductionHeader.ProductionInputDetailList[l].Rate, ProductionHeader.ProductionInputDetailList[l].RateUomCode, ProductionHeader.ProductionInputDetailList[l].Amount);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (ProductionHeader.ProductionOutPutDetailList.Count > 0)
			{
				for (int k = 0; k < ProductionHeader.ProductionOutPutDetailList.Count; k++)
				{
					dtOutPutByProductHistory.Rows.Add(ProductionHeader.ProductionOutPutDetailList[k].Id, ProductionHeader.ProductionOutPutDetailList[k].BOMHeaderNo, ProductionHeader.ProductionOutPutDetailList[k].EntryType, ProductionHeader.ProductionOutPutDetailList[k].SaleOrderId, ProductionHeader.ProductionOutPutDetailList[k].SaleOrderNo, ProductionHeader.ProductionOutPutDetailList[k].WareHouseName, ProductionHeader.ProductionOutPutDetailList[k].JobLotDescription, ProductionHeader.ProductionOutPutDetailList[k].ItemId, ProductionHeader.ProductionOutPutDetailList[k].ItemName, ProductionHeader.ProductionOutPutDetailList[k].ItemAttribute, ProductionHeader.ProductionOutPutDetailList[k].VarientEquivalent, ProductionHeader.ProductionOutPutDetailList[k].ItemUomCode, ProductionHeader.ProductionOutPutDetailList[k].ItemQty, ProductionHeader.ProductionOutPutDetailList[k].VarientQty, ProductionHeader.ProductionOutPutDetailList[k].NetWeight, ProductionHeader.ProductionOutPutDetailList[k].Rate, ProductionHeader.ProductionOutPutDetailList[k].RateUomCode, ProductionHeader.ProductionOutPutDetailList[k].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			if (ProductionHeader.ProductionOverHeadsDetailList.Count > 0)
			{
				for (int j = 0; j < ProductionHeader.ProductionOverHeadsDetailList.Count; j++)
				{
					dtOverHeadHistory.Rows.Add(ProductionHeader.ProductionOverHeadsDetailList[j].Id, ProductionHeader.ProductionOverHeadsDetailList[j].BomHeaderNo, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadTypeId, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadAccountTitle, ProductionHeader.ProductionOverHeadsDetailList[j].PercentOfTotalInput, ProductionHeader.ProductionOverHeadsDetailList[j].Qty, ProductionHeader.ProductionOverHeadsDetailList[j].Rate, ProductionHeader.ProductionOverHeadsDetailList[j].Amount, ProductionHeader.ProductionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			if (ProductionHeader.ProductionWagesDetailList.Count > 0)
			{
				for (int i = 0; i < ProductionHeader.ProductionWagesDetailList.Count; i++)
				{
					dtWagesHistory.Rows.Add(ProductionHeader.ProductionWagesDetailList[i].Id, ProductionHeader.ProductionWagesDetailList[i].BomHeaderNo, ProductionHeader.ProductionWagesDetailList[i].ContractorName, ProductionHeader.ProductionWagesDetailList[i].ItemName, ProductionHeader.ProductionWagesDetailList[i].ServiceActivity, ProductionHeader.ProductionWagesDetailList[i].Qty, ProductionHeader.ProductionWagesDetailList[i].Rate, ProductionHeader.ProductionWagesDetailList[i].Amount, ProductionHeader.ProductionWagesDetailList[i].Remarks);
				}
			}
			else
			{
				grdWagesHistory.ClearStructure();
			}
			grdHistoryInput.DataSource = dtInputHistory;
			grdHistoryInput.RetrieveStructure();
			grdHistoryInputSetting();
			grdHistoryOutput.DataSource = dtOutPutByProductHistory;
			grdHistoryOutput.RetrieveStructure();
			grdHistoryOutputSetting();
			grdHistoryOverHead.DataSource = dtOverHeadHistory;
			grdHistoryOverHead.RetrieveStructure();
			grdHistoryOverHeadSetting();
			grdWagesHistory.DataSource = dtWagesHistory;
			grdWagesHistory.RetrieveStructure();
			grdWagesHistorySetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryInput);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["EntryType"].Visible = false;
			grdHistoryOutput.RootTable.Columns["SaleOrderId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["Uom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VarientQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VarientQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VarientQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VarientQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOutput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOutput);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Percentage"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Percentage"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOverHead);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdWagesHistorySetting()
	{
		try
		{
			grdWagesHistory.RootTable.Columns["Id"].Visible = false;
			grdWagesHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdWagesHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdWagesHistory);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formright.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					BtnLoadInputFromBom_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 123;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdInputMaterial).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdOutPutByProduct).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdContractorWages).Focus();
					}
					else if (((Control)(object)grdContractorWages).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdOverHead).Focus();
					}
					else if (((Control)(object)grdOverHead).Focused)
					{
						((Control)(object)grdInputMaterial).Focus();
					}
					else if (((Control)(object)grdInputMaterial).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdOutPutByProduct).Focus();
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
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistoryOutput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryOutput).Focused)
					{
						((Control)(object)grdHistoryInput).Focus();
					}
					else if (((Control)(object)grdHistoryInput).Focused)
					{
						((Control)(object)grdWagesHistory).Focus();
					}
					else if (((Control)(object)grdWagesHistory).Focused)
					{
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else if (((Control)(object)grdHistoryOverHead).Focused)
					{
						((Control)(object)grdHistoryOutput).Focus();
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
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
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInputMaterial.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				_ = grdInputMaterial.CurrentColumn.Key == "Add";
				if (grdInputMaterial.CurrentColumn.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 68;
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteInputDetailRow(r);
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string NameCode = "";
				if (grdInputMaterial.CurrentColumn.Key == "ItemId" || grdInputMaterial.CurrentColumn.Key == "ItemName")
				{
					DataTable dtAlter = BomHeader.GetAlternativeItemsByMainItemId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["MainItemId"].Value), Conversion.ToInt(r.Cells["BomHeaderId"].Value));
					if (dtAlter.Rows.Count <= 0)
					{
						throw new Exception("No Alternative Item found Against this Item " + Conversion.ToString(r.Cells["ItemName"].Value));
					}
					GrdPopUp obj3 = new GrdPopUp(dtAlter, "ItemId", "ItemName");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					Id = Conversion.ToInt((object)obj3.ReturnId);
					NameCode = Conversion.ToString((object)obj3.ReturnName);
					r.Cells["ItemId"].Value = Id;
					r.Cells["ItemName"].Value = NameCode;
					DataRow foundRow = dtAlter.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemId") == Id);
					int UomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ItemUomId"]) : 0);
					string ItemUom = ((foundRow != null) ? Conversion.ToString(foundRow["ItemUom"]) : "");
					decimal ItemUomEquivalent = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemUomEquivalent"]) : 0m);
					decimal MinQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["MinQty"]) : 0m);
					decimal MaxQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["MaxQty"]) : 0m);
					decimal ItemQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemQty"]) : 0m);
					decimal WeightKg = ((foundRow != null) ? Conversion.ToDecimal(foundRow["WeightKg"]) : 0m);
					r.Cells["ItemUomId"].Value = UomId;
					r.Cells["UomCode"].Value = ItemUom;
					r.Cells["Uom"].Value = ItemUomEquivalent;
					r.Cells["BomQty"].Value = ItemQty;
					r.Cells["BomMinQty"].Value = MinQty;
					r.Cells["BomMaxQty"].Value = MaxQty;
					r.Cells["ItemQty"].Value = ItemQty;
					r.Cells["MinQty"].Value = MinQty;
					r.Cells["MaxQty"].Value = MaxQty;
					r.Cells["NetWeight"].Value = WeightKg;
					int JobId2 = 0;
					string jobName2 = "";
					if (dtJob.Rows.Count > 0)
					{
						JobId2 = Conversion.ToInt(dtJob.Rows[0]["Id"]);
						jobName2 = Conversion.ToString(dtJob.Rows[0]["JobLotDescription"]);
					}
					r.Cells["JobLotId"].Value = JobId2;
					r.Cells["JobLot"].Value = jobName2;
					DtStock.Clear();
					int wareId2 = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId2 = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId2 = UserAccount.OrganizationId;
					int CompanyId2 = UserAccount.CompanyId;
					DateTime DocDate2 = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId2, CompanyId2, ItemId2, DocDate2, wareId2, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate2 = default(decimal);
						decimal Stock2 = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate2 = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj5 = new ReportsParameters();
							obj5.OrganizationId = UserAccount.OrganizationId;
							obj5.CompanyId = UserAccount.CompanyId;
							obj5.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj5.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj5.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj5.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
							obj5.Id = RecId;
							if (obj5.Id > 0)
							{
								obj5.DocumentTypeId = 1850;
							}
							obj5.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj5.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
							AvgRate2 = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj5));
						}
						r.Cells["BalanceStock"].Value = Stock2;
						r.Cells["Rate"].Value = AvgRate2;
						decimal RateUom2 = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom2 > 0m && AvgRate2 > 0m)
						{
							decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal Iuom2 = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount2 = num * Iuom2 / RateUom2 * AvgRate2;
							r.Cells["Amount"].Value = Amount2;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				if (grdInputMaterial.CurrentColumn.Key == "WarehouseName")
				{
					GrdPopUp obj2 = new GrdPopUp(CommonServices.WareHouseAllocationToPlantsByProductionPlantId(Conversion.ToInt(CmbPlantName.Value)), "Id", "WarehouseName");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
					r.Cells["WarehouseName"].Value = NameCode;
					int JobId = 0;
					string jobName = "";
					if (dtJob.Rows.Count > 0)
					{
						JobId = Conversion.ToInt(dtJob.Rows[0]["Id"]);
						jobName = Conversion.ToString(dtJob.Rows[0]["JobLotDescription"]);
					}
					r.Cells["JobLotId"].Value = JobId;
					r.Cells["JobLot"].Value = jobName;
					DtStock.Clear();
					int wareId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					DateTime DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, DocDate, wareId, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate = default(decimal);
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj4 = new ReportsParameters();
							obj4.OrganizationId = UserAccount.OrganizationId;
							obj4.CompanyId = UserAccount.CompanyId;
							obj4.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj4.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj4.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj4.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
							obj4.Id = RecId;
							if (obj4.Id > 0)
							{
								obj4.DocumentTypeId = 1850;
							}
							obj4.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj4.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
							AvgRate = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj4));
						}
						r.Cells["BalanceStock"].Value = Stock;
						r.Cells["Rate"].Value = AvgRate;
						decimal RateUom = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom > 0m && AvgRate > 0m)
						{
							decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount = num2 * Iuom / RateUom * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				if (grdInputMaterial.CurrentColumn.Key == "JobLot")
				{
					GrdPopUp obj = new GrdPopUp(dtJob, "Id", "JobLotDescription");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					r.Cells["JobLotId"].Value = Id;
					r.Cells["JobLot"].Value = NameCode;
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			EnableDisablePlant();
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
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPutByProduct.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int WorkOrderId = Conversion.ToInt((object)r.Cells["WorkOrderId"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOutPutByProduct.CurrentColumn.Key == "Add")
				{
					AddRowInOutPutByProductGrid();
				}
				if (grdOutPutByProduct.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOutPutByProductGrid();
				grdOutPutByProduct.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			decimal Name = default(decimal);
			string NameCode = "";
			if (grdOutPutByProduct.CurrentColumn.Key == "ItemName" && WorkOrderId == 0 && Conversion.ToInt(CmbProductionType.Value) == 45)
			{
				lstitem.Clear();
				lstitem = CommonServices.GetItemByItemTypeId(Conversion.ToString(CmbProductionType.Value));
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj7 = new GrdPopUp(lstitem, "Id", "ItemName");
				obj7.StartPosition = FormStartPosition.CenterScreen;
				obj7.ShowDialog();
				obj7.BringToFront();
				Id = Conversion.ToInt((object)obj7.ReturnId);
				NameCode = Conversion.ToString((object)obj7.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = NameCode;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["Uom"].Value = Name;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["NetWeight"].Value = 0;
				}
				if (Conversion.ToInt((object)r.Cells["ItemAttributeVarientId"]) == 0)
				{
					dtAttributes.Clear();
					int ItemAttributeVarientId = 0;
					dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dtAttributes.Rows.Count > 0)
					{
						DataRow firstRow = dtAttributes.AsEnumerable().FirstOrDefault();
						if (firstRow != null)
						{
							ItemAttributeVarientId = Conversion.ToInt(firstRow["ItemAttributeVarientId"]);
						}
						DataRow foundRow2 = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == ItemAttributeVarientId);
						string AttributeVarient = ((foundRow2 != null) ? Conversion.ToString(foundRow2["ItemAttribute"]) : "");
						decimal VarientEquivalent2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["VarientEquivalent"]) : 0m);
						r.Cells["ItemAttributeVarientId"].Value = Id;
						r.Cells["AttributeVarient"].Value = AttributeVarient;
						r.Cells["VarientEquivalent"].Value = VarientEquivalent2;
						decimal VarientQty2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString()) * VarientEquivalent2;
						r.Cells["VarientQty"].Value = VarientQty2;
						decimal CurrentRowAmount2 = Conversion.ToDecimal(r.Cells["Rate"].Value) * VarientQty2;
						r.Cells["Amount"].Value = CurrentRowAmount2;
					}
				}
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "WarehouseName")
			{
				GrdPopUp obj6 = new GrdPopUp(CommonServices.WareHouseAllocationToPlantsByProductionPlantId(Conversion.ToInt(CmbPlantName.Value)), "Id", "WarehouseName");
				obj6.StartPosition = FormStartPosition.CenterScreen;
				obj6.ShowDialog();
				obj6.BringToFront();
				Id = Conversion.ToInt((object)obj6.ReturnId);
				NameCode = Conversion.ToString((object)obj6.ReturnName);
				r.Cells["WarehouseId"].Value = Id;
				r.Cells["WarehouseName"].Value = NameCode;
				int JobId = 0;
				string jobName = "";
				GetDefaultJob(out JobId, out jobName);
				r.Cells["JobLotId"].Value = JobId;
				r.Cells["JobLot"].Value = jobName;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "EntryType")
			{
				GrdPopUp obj5 = new GrdPopUp(dtType, "EntryTypeId", "EntryType");
				obj5.StartPosition = FormStartPosition.CenterScreen;
				obj5.ShowDialog();
				obj5.BringToFront();
				Id = Conversion.ToInt((object)obj5.ReturnId);
				NameCode = Conversion.ToString((object)obj5.ReturnName);
				r.Cells["EntryTypeId"].Value = Id;
				r.Cells["EntryType"].Value = NameCode;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "JobLot")
			{
				GrdPopUp obj4 = new GrdPopUp(dtJob, "Id", "JobLotDescription");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				NameCode = Conversion.ToString((object)obj4.ReturnName);
				r.Cells["JobLotId"].Value = Id;
				r.Cells["JobLot"].Value = Name;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "AttributeVarient" && WorkOrderId == 0)
			{
				dtAttributes.Clear();
				dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj3 = new GrdPopUp(dtAttributes, "ItemAttributeVarientId", "ItemAttribute");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				NameCode = Conversion.ToString((object)obj3.ReturnName);
				DataRow foundRow = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
				r.Cells["ItemAttributeVarientId"].Value = Id;
				r.Cells["AttributeVarient"].Value = NameCode;
				r.Cells["VarientEquivalent"].Value = Name;
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				decimal VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Value.ToString());
				decimal VarientQty = num * VarientEquivalent;
				r.Cells["VarientQty"].Value = VarientQty;
				decimal CurrentRowAmount = Conversion.ToDecimal(r.Cells["Rate"].Value) * VarientQty;
				r.Cells["Amount"].Value = CurrentRowAmount;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "SaleOrder" && WorkOrderId == 0)
			{
				GrdPopUp obj2 = new GrdPopUp(dtOrders, "Id", "DocNo");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["SaleOrderId"].Value = Id;
				r.Cells["SaleOrder"].Value = Conversion.ToInt((object)NameCode);
			}
			if ((grdOutPutByProduct.CurrentColumn.Key == "BomHeaderId" || grdOutPutByProduct.CurrentColumn.Key == "BomNo") && WorkOrderId > 0)
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				ValidateItemTypeOfType(dtOutPutByProduct);
				GrdPopUp obj = new GrdPopUp(BOMNobyItemDbCall(ItemId), "Id", "BomName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				foreach (GridEXRow item in from rrr in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(rrr.Cells["WorkOrderId"].Value) == WorkOrderId
					select rrr)
				{
					item.BeginEdit();
					item.Cells["BomHeaderId"].Value = Id;
					item.Cells["BomNo"].Value = NameCode;
					item.EndEdit();
				}
			}
			grdOutPutByProduct.UpdateData();
			EnableDisableProductionType();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOverHead.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOverHead.CurrentColumn.Key == "Add")
				{
					AddRowInOverHeadGrid();
				}
				if (grdOverHead.CurrentColumn.Key == "Delete")
				{
					dtOverHead.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOverHead.Rows.Count == 0)
					{
						AddRowInOverHeadGrid();
					}
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOverHeadGrid();
				grdOverHead.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOverHead.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			if (grdOverHead.CurrentColumn != null && grdOverHead.CurrentColumn.Key == "OverHeadAccountId")
			{
				GrdPopUp obj2 = new GrdPopUp(dtAccounts, "Id", "AccountTitle");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				r.Cells["OverHeadAccountId"].Value = obj2.ReturnId;
			}
			if (grdOverHead.CurrentColumn != null && (grdOverHead.CurrentColumn.Key == "BomHeaderId" || grdOverHead.CurrentColumn.Key == "BomNo"))
			{
				GetOutPutGridBomNos();
				if (dtOutputBom.Rows.Count > 0)
				{
					GrdPopUp obj = new GrdPopUp(dtOutputBom, "BomHeaderId", "BomNo");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["BomHeaderId"].Value = obj.ReturnId;
					r.Cells["BomNo"].Value = obj.ReturnName;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
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
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtContractorWages.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtContractorWages.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtContractorWages.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
				{
					int OutPutItemId3 = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (OutPutItemId3 == 0)
					{
						throw new Exception("Please Select OutPut Item for Wages Entry...");
					}
					GrdPopUp obj4 = new GrdPopUp(dtContr, "Id", "CompanyName");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					Name = Conversion.ToString((object)obj4.ReturnName);
					r.Cells["ContractorId"].Value = Id;
					r.Cells["ContractorName"].Value = Name;
					string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
					DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, OutPutItemId3, Conversion.ToDateTime((object)txtdocdate.Value), 0, Id);
					if (dt2.Rows.Count > 0)
					{
						DataRow foundRow3 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
						decimal Rate2 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Rate"]) : 0m);
						int ContractorWagesRateScheduleId = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ContractorWagesRateScheduleId"]) : 0);
						r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
						decimal Qty2 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Qty"]) : 0m);
						decimal RatePerQty2 = ((Qty2 > 0m) ? (Rate2 / Qty2) : 0m);
						r.Cells["Rate"].Value = RatePerQty2;
						decimal CurrRowQty2 = Conversion.ToInt(r.Cells["Qty"].Value);
						decimal Amount2 = RatePerQty2 * CurrRowQty2;
						r.Cells["Amount"].Value = Amount2;
					}
				}
				if (grdContractorWages.CurrentColumn != null && grdContractorWages.CurrentColumn.Key == "ServiceActivity")
				{
					int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					int OutPutItemId2 = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (OutPutItemId2 == 0)
					{
						throw new Exception("Please Select OutPut Item for Wages Entry...");
					}
					DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, OutPutItemId2, Conversion.ToDateTime((object)txtdocdate.Value), 0, ContrId);
					GrdPopUp obj3 = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					Id = Conversion.ToInt((object)obj3.ReturnId);
					Name = Conversion.ToString((object)obj3.ReturnName);
					r.Cells["ContractorWagesRateScheduleId"].Value = Id;
					r.Cells["ServiceActivity"].Value = Name;
					if (Id > 0)
					{
						DataRow foundRow2 = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
						decimal Qty = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Qty"]) : 0m);
						decimal Rate = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Rate"]) : 0m);
						decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
						r.Cells["Rate"].Value = RatePerQty;
						decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
						decimal Amount = RatePerQty * CurrRowQty;
						r.Cells["Amount"].Value = Amount;
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "BomHeaderId" || grdContractorWages.CurrentColumn.Key == "BomNo"))
				{
					if (Conversion.ToInt((object)grdOutPutByProduct.GetDataRows().Count()) == 0)
					{
						throw new Exception("Out Put Grid Record not founds...");
					}
					if (Conversion.ToInt(r.Cells["BomWagesId"].Value) != 0)
					{
						throw new Exception("You can't Update Bom No of row Against Bill Of Material");
					}
					GetOutPutGridBomNos();
					if (dtOutputBom.Rows.Count > 0)
					{
						GrdPopUp obj2 = new GrdPopUp(dtOutputBom, "BomHeaderId", "BomNo");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						r.Cells["BomHeaderId"].Value = obj2.ReturnId;
						r.Cells["BomNo"].Value = obj2.ReturnName;
						Id = Conversion.ToInt((object)obj2.ReturnId);
						Name = Conversion.ToString((object)obj2.ReturnName);
						if (Id > 0)
						{
							GetOutPutGridItemsForWages(Id);
							DataRow foundRow = dtOutputItem.AsEnumerable().FirstOrDefault();
							int OutPutItemId = ((foundRow != null) ? Conversion.ToInt(foundRow["OutItemId"]) : 0);
							string OutPutItemName = ((foundRow != null) ? Conversion.ToString(foundRow["OutItemName"]) : "");
							r.Cells["OutPutItemId"].Value = OutPutItemId;
							r.Cells["OutPutItemName"].Value = OutPutItemName;
						}
						else
						{
							r.Cells["OutPutItemId"].Value = 0;
							r.Cells["OutPutItemName"].Value = "";
						}
					}
				}
				if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "OutPutItemId" || grdContractorWages.CurrentColumn.Key == "OutPutItemName"))
				{
					if (Conversion.ToInt((object)grdOutPutByProduct.GetDataRows().Count()) == 0)
					{
						throw new Exception("Out Put Grid Record not founds...");
					}
					if (Conversion.ToInt(r.Cells["BomWagesId"].Value) != 0)
					{
						throw new Exception("You can't Update Bom No of row Against Bill Of Material");
					}
					int BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					if (BomHeaderId == 0)
					{
						throw new Exception("Please Select Bom No first...");
					}
					GetOutPutGridItemsForWages(BomHeaderId);
					if (dtOutputItem.Rows.Count > 0)
					{
						GrdPopUp obj = new GrdPopUp(dtOutputItem, "OutItemId", "OutItemName");
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						obj.BringToFront();
						r.Cells["OutPutItemId"].Value = obj.ReturnId;
						r.Cells["OutPutItemName"].Value = obj.ReturnName;
					}
				}
				grdContractorWages.UpdateData();
				Calculation();
			}
			EnableDisableItemName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						WF.Close();
						throw new Exception("you don't have update rights...");
					}
					if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
					{
						WF.Close();
						MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					WF.Show();
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ProductionSlipConcrete(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1850);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					WF.Close();
					throw new Exception("you don't have update rights...");
				}
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				WF.Show();
				ReadById(Conversion.ToInt((object)Id));
			}
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

	private void ValidateItemTypeOfType(DataTable dtToValidate)
	{
		int parentTypeId = Conversion.ToInt(CmbProductionType.Value);
		HashSet<int> validItemIds = (from item in clsGlobalVariables.getGlobalAllItems
			where item.Id != 0 && item.ItemTypeOfTypeId == parentTypeId
			select Conversion.ToInt((object)item.Id)).ToHashSet();
		string selectedTypeName = Conversion.ToString((object)((Control)(object)CmbProductionType).Text);
		foreach (DataRow dr in dtToValidate.Rows)
		{
			int itemId = Conversion.ToInt(dr["ItemId"]);
			if (itemId > 0 && !validItemIds.Contains(itemId))
			{
				string itemName = Conversion.ToString(dr["ItemName"]);
				throw new Exception("Item '" + itemName + "' in Output Grid is not valid for Type '" + selectedTypeName + "' in ItemList.");
			}
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DatagridHistory, dtHistoryFromDb);
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInputMaterial, dtInput);
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdOutPutByProduct, dtOutPutByProduct);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateRatesOutput_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbProductionType).ActiveRow == null || Conversion.ToInt(CmbProductionType.Value) == 0)
			{
				WF.Close();
				CmbProductionType.Focus();
				throw new Exception("ProductionType Field Required");
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				WF.Close();
				throw new Exception("Output Grid Record not Found");
			}
			WF.Show();
			UpdateRatesOutput();
			WF.Close();
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

	private void UpdateInputQtyIncaseOfSlabProduction(bool isSlab)
	{
		try
		{
			if (grdInputMaterial.GetDataRows().Count() == 0)
			{
				throw new Exception("Input Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by (!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0 into @group
				select new
				{
					BomHeaderId = @group.Key,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int outBomId = item.BomHeaderId;
				decimal totalOutQtyOfCurrBom = Conversion.ToDecimal((object)item.TotalItemQty);
				foreach (GridEXRow item2 in from r in grdInputMaterial.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == outBomId
					select r)
				{
					item2.BeginEdit();
					decimal avgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal rateUom = Conversion.ToDecimal(item2.Cells["Rateuom"].Value);
					decimal qty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString());
					decimal finalQty = (isSlab ? (qty * totalOutQtyOfCurrBom) : qty);
					item2.Cells["ItemQty"].Value = finalQty;
					decimal BomMinQty = Conversion.ToDecimal((object)item2.Cells["BomMinQty"].Value.ToString());
					decimal BomMaxQty = Conversion.ToDecimal((object)item2.Cells["BomMaxQty"].Value.ToString());
					item2.Cells["MinQty"].Value = BomMinQty * (isSlab ? totalOutQtyOfCurrBom : 1m);
					item2.Cells["MaxQty"].Value = BomMaxQty * (isSlab ? totalOutQtyOfCurrBom : 1m);
					decimal iuom = Conversion.ToDecimal((object)item2.Cells["Uom"].Value.ToString());
					item2.Cells["NetWeight"].Value = finalQty * iuom;
					item2.Cells["Amount"].Value = ((rateUom > 0m && avgRate > 0m) ? (finalQty * iuom / rateUom * avgRate) : 0m);
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateWagesQtyIncaseOfSlabProduction(bool IsSlab)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() == 0)
			{
				throw new Exception("Wages Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by new
				{
					BomHeaderId = ((!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0)
				} into @group
				select new
				{
					BomHeaderId = @group.Key.BomHeaderId,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int OutBomId = item.BomHeaderId;
				decimal TotalOutQtyOfCurrBom = (IsSlab ? Conversion.ToDecimal((object)item.TotalItemQty) : 1m);
				foreach (GridEXRow item2 in from r in grdContractorWages.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == OutBomId
					select r)
				{
					item2.BeginEdit();
					decimal AvgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal FinalQty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString()) * TotalOutQtyOfCurrBom;
					item2.Cells["Qty"].Value = FinalQty;
					decimal Amount = ((FinalQty > 0m && AvgRate > 0m) ? (FinalQty * AvgRate) : 0m);
					item2.Cells["Amount"].Value = Amount;
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateOHQtyIncaseOfSlabProduction(bool IsSlab)
	{
		try
		{
			if (grdOverHead.GetDataRows().Count() == 0)
			{
				throw new Exception("Overhead Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by new
				{
					BomHeaderId = ((!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0)
				} into @group
				select new
				{
					BomHeaderId = @group.Key.BomHeaderId,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int OutBomId = item.BomHeaderId;
				decimal TotalOutQtyOfCurrBom = (IsSlab ? Conversion.ToDecimal((object)item.TotalItemQty) : 1m);
				foreach (GridEXRow item2 in from r in grdOverHead.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == OutBomId
					select r)
				{
					item2.BeginEdit();
					decimal AvgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal FinalQty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString()) * TotalOutQtyOfCurrBom;
					item2.Cells["Qty"].Value = FinalQty;
					decimal Amount = ((FinalQty > 0m && AvgRate > 0m) ? (FinalQty * AvgRate) : 0m);
					item2.Cells["Amount"].Value = Amount;
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateInputCurrentStockAndAvgRate()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				DtStock.Clear();
				GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					int wareId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					DateTime DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, DocDate, wareId, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate = default(decimal);
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj = new ReportsParameters();
							obj.OrganizationId = UserAccount.OrganizationId;
							obj.CompanyId = UserAccount.CompanyId;
							obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
							obj.Id = RecId;
							if (obj.Id > 0)
							{
								obj.DocumentTypeId = 1850;
							}
							obj.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
							AvgRate = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj));
						}
						r.BeginEdit();
						r.Cells["BalanceStock"].Value = Stock;
						r.Cells["Rate"].Value = AvgRate;
						decimal RateUom = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom > 0m && AvgRate > 0m)
						{
							decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount = num * Iuom / RateUom * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
						r.EndEdit();
					}
					else
					{
						r.BeginEdit();
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.EndEdit();
					}
				}
				return;
			}
			if (grdInputMaterial.GetDataRows().Count() == 1 && Conversion.ToInt(grdInputMaterial.GetRow(0).Cells["WarehouseId"].Value) == 0)
			{
				throw new Exception("Warehouse Field is Empty in Input Grid Row No: 01");
			}
			throw new Exception("Input Grid Record not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateWagesRates()
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				int ServiceActivityId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				r.BeginEdit();
				if (ServiceActivityId > 0)
				{
					DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(r.Cells["OutPutItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), ServiceActivityId, ContrId);
					if (dt.Rows.Count > 0)
					{
						DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == ServiceActivityId);
						decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
						decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
						decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
						r.Cells["Rate"].Value = RatePerQty;
						decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
						decimal Amount = RatePerQty * CurrRowQty;
						r.Cells["Amount"].Value = Amount;
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateRatesOutput()
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 1 && Conversion.ToInt((from rr in grdOutPutByProduct.GetDataRows()
				where Conversion.ToInt(rr.Cells["BomHeaderId"].Value) > 0
				select rr).FirstOrDefault().Cells["ItemId"].Value) == 0)
			{
				WF.Close();
				throw new Exception("Output Grid Record not Filled Completely");
			}
			Calculation();
			foreach (var item in (from row in dtInput.AsEnumerable()
				group row by (!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0 into @group
				select new
				{
					BomHeaderId = @group.Key,
					TotalAmount = @group.Sum((DataRow row) => (!row.IsNull("Amount")) ? Conversion.ToDouble((object)row.Field<decimal>("Amount")) : 0.0)
				}).ToList())
			{
				int inputBomId = item.BomHeaderId;
				decimal totalInputAmountOfCurrBom = Conversion.ToDecimal((object)item.TotalAmount);
				decimal sumOfOverhead = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grdOverHead, "Amount", "BomHeaderId", inputBomId.ToString(), CheckValue: true));
				decimal netValue = default(decimal);
				if (!ContractorWagesDebitTotExpenseAcOnProductionConcrete)
				{
					decimal sumOfContractorWages = (from GridEXRow row in grdContractorWages.GetDataRows()
						where Conversion.ToInt(row.Cells["BomHeaderId"].Value) == inputBomId
						select (decimal)row.Cells["Amount"].Value).Sum();
					netValue = totalInputAmountOfCurrBom + sumOfOverhead + sumOfContractorWages;
				}
				else
				{
					netValue = totalInputAmountOfCurrBom + sumOfOverhead;
				}
				List<GridEXRow> list = (from rr in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(rr.Cells["BomHeaderId"].Value) == inputBomId && Conversion.ToInt(rr.Cells["EntryTypeId"].Value) == 1
					select rr).ToList();
				decimal totalVarientQty = list.Sum((GridEXRow row) => Conversion.ToDecimal(row.Cells["VarientQty"].Value));
				foreach (GridEXRow item2 in list)
				{
					item2.BeginEdit();
					decimal totalCurrentRowVarientQty = Conversion.ToDecimal(item2.Cells["VarientQty"].Value);
					decimal currentRowAmount = ((totalVarientQty > 0m) ? (netValue / totalVarientQty * totalCurrentRowVarientQty) : 0m);
					item2.Cells["Amount"].Value = currentRowAmount;
					decimal currentRowRate = ((totalCurrentRowVarientQty > 0m) ? (currentRowAmount / totalCurrentRowVarientQty) : 0m);
					item2.Cells["Rate"].Value = currentRowRate;
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateInputWagesOH_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbProductionType).ActiveRow == null || Conversion.ToInt(CmbProductionType.Value) == 0)
			{
				CmbProductionType.Focus();
				throw new Exception("ProductionType Field Required");
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				throw new Exception("Output Grid Record not Found");
			}
			if (Conversion.ToInt(CmbProductionType.Value) == 46 || Conversion.ToInt(CmbProductionType.Value) == 47)
			{
				UpdateInputQtyIncaseOfSlabProduction(isSlab: true);
				UpdateWagesQtyIncaseOfSlabProduction(IsSlab: true);
				UpdateOHQtyIncaseOfSlabProduction(IsSlab: true);
			}
			else if (Conversion.ToInt(CmbProductionType.Value) == 45)
			{
				UpdateInputQtyIncaseOfSlabProduction(isSlab: false);
				UpdateWagesQtyIncaseOfSlabProduction(IsSlab: false);
				UpdateOHQtyIncaseOfSlabProduction(IsSlab: false);
			}
			WF.Show();
			UpdateInputCurrentStockAndAvgRate();
			UpdateWagesRates();
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

	private void BtnUpdateInOhGrid_Click(object sender, EventArgs e)
	{
		try
		{
			double percent = Conversion.ToDouble((object)txtPercentToupdateInOhGrid.Text);
			if (percent <= 0.0)
			{
				throw new Exception("Percentage must be greater than 0.");
			}
			List<GridEXRow> ohRows = grdOverHead.GetDataRows().ToList();
			Dictionary<int, double> inputAmountTotals = (from r in grdInputMaterial.GetDataRows().ToList()
				group r by Conversion.ToInt(r.Cells["BomHeaderId"].Value)).ToDictionary((IGrouping<int, GridEXRow> g) => g.Key, (IGrouping<int, GridEXRow> g) => g.Sum((GridEXRow x) => Conversion.ToDouble(x.Cells["Amount"].Value)));
			int SkipBomId = 0;
			foreach (GridEXRow r2 in ohRows)
			{
				r2.BeginEdit();
				int bomId = Conversion.ToInt(r2.Cells["BomHeaderId"].Value);
				if (SkipBomId > 0 && SkipBomId == bomId)
				{
					continue;
				}
				int bomRowCount = ohRows.Count((GridEXRow x) => Conversion.ToInt(x.Cells["BomHeaderId"].Value) == bomId);
				double newTotalPercent = percent * (double)bomRowCount;
				if (newTotalPercent > 100.0)
				{
					MessageBox.Show(string.Format("Total percentage would become {0}% for BOM No: {1}.\n", newTotalPercent, Conversion.ToString(r2.Cells["BomNo"].Value)) + "This exceeds the allowed 100% limit.\n\n" + $"Rows in BOM: {bomRowCount}, Each with {percent}%");
					r2.EndEdit();
					SkipBomId = bomId;
					continue;
				}
				r2.Cells["PercentOfTotalInput"].Value = percent;
				double updatedAmount = (inputAmountTotals.ContainsKey(bomId) ? inputAmountTotals[bomId] : 0.0) * percent / 100.0;
				if (updatedAmount > 0.0)
				{
					r2.Cells["Amount"].Value = Math.Round(updatedAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else
				{
					r2.Cells["Amount"].Value = 0;
				}
				r2.Cells["Qty"].Value = 0;
				r2.Cells["Rate"].Value = 0;
				r2.EndEdit();
			}
			grdOverHead.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateWarehouseInput_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in Input Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					int JobId = 0;
					string jobName = "";
					if (dtJob.Rows.Count > 0)
					{
						JobId = Conversion.ToInt(dtJob.Rows[0]["Id"]);
						jobName = Conversion.ToString(dtJob.Rows[0]["JobLotDescription"]);
					}
					(from r in grdInputMaterial.GetDataRows()
						where r.RowIndex == 0
						select r).FirstOrDefault();
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.Cells["JobLotId"].Value = JobId;
						obj.Cells["JobLot"].Value = jobName;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("Input Grid Record not Found");
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

	private void BtnUpdateWarehouseOutPut_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in OutPut Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					int JobId = 0;
					string jobName = "";
					if (dtJob.Rows.Count > 0)
					{
						JobId = Conversion.ToInt(dtJob.Rows[0]["Id"]);
						jobName = Conversion.ToString(dtJob.Rows[0]["JobLotDescription"]);
					}
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.Cells["JobLotId"].Value = JobId;
						obj.Cells["JobLot"].Value = jobName;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("OutPut Grid Record not Found");
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

	private void BtnUpdateConctratorInWages_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0)
			{
				int ContrId = 0;
				string ContrName = string.Empty;
				string ActivtyName = string.Empty;
				if (((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) != 0)
				{
					ContrId = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
					ContrName = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
				}
				if (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) != 0)
				{
					ActivtyName = Conversion.ToString((object)((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text);
				}
				GridEXRow[] dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (ContrId == 0 && string.IsNullOrEmpty(ActivtyName))
					{
						if (Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
						{
							throw new Exception("ContractorName Field is Empty in Wages Grid Row No: 01");
						}
						ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
						ContrName = Conversion.ToString(r.Cells["ContractorName"].Value);
					}
					if (!string.IsNullOrEmpty(ActivtyName) && Conversion.ToString(r.Cells["ServiceActivity"].Value) != ActivtyName)
					{
						continue;
					}
					r.BeginEdit();
					r.Cells["ContractorId"].Value = ((ContrId == 0) ? Conversion.ToInt(r.Cells["ContractorId"].Value) : ContrId);
					r.Cells["ContractorName"].Value = ContrName ?? Conversion.ToString(r.Cells["ContractorName"].Value);
					int ServiceActivityId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (ServiceActivityId > 0)
					{
						DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(r.Cells["OutPutItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), ServiceActivityId, ContrId);
						if (dt.Rows.Count > 0)
						{
							DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == ServiceActivityId);
							decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
							decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
							decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
							r.Cells["Rate"].Value = RatePerQty;
							decimal CurrRowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
							r.Cells["Amount"].Value = RatePerQty * CurrRowQty;
						}
						else
						{
							r.Cells["Rate"].Value = 0;
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
					r.EndEdit();
				}
				if (ContrId == 0)
				{
					throw new Exception("ContractorName Field is Empty");
				}
				return;
			}
			throw new Exception("Wages Grid Record not Found");
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

	private void btnClearInput_Click(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("are you sure to reset Input Detail?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id > 0)
				{
					ProductionInputDetail d1 = new ProductionInputDetail();
					d1.Id = Id;
					FillInPutDetailListCommonForInsertAndDelete(d1, r);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					r.Delete();
					grdInputMaterial.UpdateData();
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

	private void btnDefinePlant_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlant(UserAccount).Show();
			PlantBind(PlantFeaderDbCall());
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
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Expected O, but got Unknown
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Expected O, but got Unknown
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Expected O, but got Unknown
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Expected O, but got Unknown
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Expected O, but got Unknown
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Expected O, but got Unknown
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Expected O, but got Unknown
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Expected O, but got Unknown
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Expected O, but got Unknown
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Expected O, but got Unknown
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d1: Expected O, but got Unknown
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Expected O, but got Unknown
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Expected O, but got Unknown
		//IL_0b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b31: Expected O, but got Unknown
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Expected O, but got Unknown
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Expected O, but got Unknown
		//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Expected O, but got Unknown
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Expected O, but got Unknown
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Expected O, but got Unknown
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf4: Expected O, but got Unknown
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Expected O, but got Unknown
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Expected O, but got Unknown
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Expected O, but got Unknown
		//IL_15e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ea: Expected O, but got Unknown
		//IL_15f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1601: Expected O, but got Unknown
		//IL_1a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a64: Expected O, but got Unknown
		//IL_1a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7b: Expected O, but got Unknown
		//IL_1a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a92: Expected O, but got Unknown
		//IL_1ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efe: Expected O, but got Unknown
		//IL_1f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f15: Expected O, but got Unknown
		//IL_2d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d84: Expected O, but got Unknown
		//IL_2d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d9b: Expected O, but got Unknown
		//IL_af00: Unknown result type (might be due to invalid IL or missing references)
		//IL_af0a: Expected O, but got Unknown
		//IL_af2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_af38: Expected O, but got Unknown
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance202 = new Appearance();
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
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
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
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
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
		Appearance appearance78 = new Appearance();
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
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
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
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
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
		Appearance appearance152 = new Appearance();
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
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
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
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWagesHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseInput = new System.Windows.Forms.Button();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.TabOutPut = new System.Windows.Forms.TabPage();
		this.grdOutPutByProduct = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseOutPut = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.TabWages = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.CmbContractorForUpdateInWagesGrid = new UltraCombo();
		this.BtnUpdateConctratorInWages = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbActivityforContractorUpdateInWagesGrid = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.TabOverheads = new System.Windows.Forms.TabPage();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateInOhGrid = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.txtPercentToupdateInOhGrid = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtReferenceNoDetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.BtnReferenceParty = new System.Windows.Forms.Button();
		this.panel16 = new System.Windows.Forms.Panel();
		this.RadCustomers = new System.Windows.Forms.RadioButton();
		this.RadRefParty = new System.Windows.Forms.RadioButton();
		this.label20 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.CmbCustomerName = new UltraCombo();
		this.BtnAddDetail = new System.Windows.Forms.Button();
		this.cmbItemBOM = new UltraCombo();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbBOMNo = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbAttribute = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbWarehouseName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.btnClearInput = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbProductionType = new UltraCombo();
		this.CmbWorkOrderNo = new UltraCombo();
		this.CmbWIPAccount = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbWIPWarehouse = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbPlantName = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.BtnLoadInputFromBom = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtOutPutAutoRatePerQtyAmount = new System.Windows.Forms.TextBox();
		this.txtTotalOutPutManualRatedAmount = new System.Windows.Forms.TextBox();
		this.txtTotalOutPutAutoRatedAmount = new System.Windows.Forms.TextBox();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.txtWagesAmount = new System.Windows.Forms.TextBox();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdateInputWagesOH = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateRatesOutput = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.chkPrintVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreviewSlip = new System.Windows.Forms.CheckBox();
		this.labelWagesExpAc = new System.Windows.Forms.Label();
		this.CmbWagesExpenseAccount = new UltraCombo();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdWagesHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbProductionNoHistory = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.TabOutPut.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).BeginInit();
		this.panel11.SuspendLayout();
		this.TabWages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel5.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).BeginInit();
		this.TabOverheads.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).BeginInit();
		this.panel4.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesExpenseAccount).BeginInit();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1197, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1197, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1189, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel6);
		this.panel17.Controls.Add(this.panel10);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 278);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1183, 401);
		this.panel17.TabIndex = 4;
		this.panel6.Controls.Add(this.BtnUpdateWarehouseInput);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(765, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(418, 401);
		this.panel6.TabIndex = 4;
		this.BtnUpdateWarehouseInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseInput.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseInput.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseInput.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseInput.Location = new System.Drawing.Point(189, 3);
		this.BtnUpdateWarehouseInput.Name = "BtnUpdateWarehouseInput";
		this.BtnUpdateWarehouseInput.Size = new System.Drawing.Size(180, 21);
		this.BtnUpdateWarehouseInput.TabIndex = 9;
		this.BtnUpdateWarehouseInput.Text = "Update Same WareHouse In All Rows";
		this.BtnUpdateWarehouseInput.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseInput.Click += new System.EventHandler(BtnUpdateWarehouseInput_Click);
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(418, 375);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 0;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(418, 26);
		this.panel7.TabIndex = 3;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(370, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(46, 25);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(222, 16);
		this.label1.TabIndex = 126;
		this.label1.Text = "Input Material (Consumed Items)";
		this.panel10.Controls.Add(this.tabControl2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(765, 401);
		this.panel10.TabIndex = 3;
		this.tabControl2.Controls.Add(this.TabOutPut);
		this.tabControl2.Controls.Add(this.TabWages);
		this.tabControl2.Controls.Add(this.TabOverheads);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(765, 401);
		this.tabControl2.TabIndex = 0;
		this.TabOutPut.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutByProduct);
		this.TabOutPut.Controls.Add(this.panel11);
		this.TabOutPut.Location = new System.Drawing.Point(4, 25);
		this.TabOutPut.Name = "TabOutPut";
		this.TabOutPut.Padding = new System.Windows.Forms.Padding(3);
		this.TabOutPut.Size = new System.Drawing.Size(757, 372);
		this.TabOutPut.TabIndex = 0;
		this.TabOutPut.Text = "OutPut (Produced Items)";
		this.TabOutPut.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOutPutByProduct).BackColor = System.Drawing.Color.White;
		grdOutPutByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutByProduct.DesignTimeLayout = grdOutPutByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Name = "grdOutPutByProduct";
		((ScrollableBase)this.grdOutPutByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Size = new System.Drawing.Size(751, 339);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).TabIndex = 0;
		this.grdOutPutByProduct.TotalRow = (InheritableBoolean)1;
		this.grdOutPutByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutByProduct).VisualStyle = (VisualStyle)4;
		this.grdOutPutByProduct.CellValueChanged += new ColumnActionEventHandler(grdOutPutByProduct_CellValueChanged);
		this.grdOutPutByProduct.CellUpdated += new ColumnActionEventHandler(grdOutPutByProduct_CellUpdated);
		this.grdOutPutByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdOutPutByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).DoubleClick += new System.EventHandler(grdOutPutByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutByProduct_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.BtnUpdateWarehouseOutPut);
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(751, 27);
		this.panel11.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseOutPut.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseOutPut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseOutPut.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseOutPut.Location = new System.Drawing.Point(515, 2);
		this.BtnUpdateWarehouseOutPut.Name = "BtnUpdateWarehouseOutPut";
		this.BtnUpdateWarehouseOutPut.Size = new System.Drawing.Size(185, 24);
		this.BtnUpdateWarehouseOutPut.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Text = "Update Same WareHouse In All Rows";
		this.BtnUpdateWarehouseOutPut.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseOutPut.Click += new System.EventHandler(BtnUpdateWarehouseOutPut_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(703, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOutPutByProduct;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(170, 16);
		this.label23.TabIndex = 126;
		this.label23.Text = "OutPut (Produced Items)";
		this.TabWages.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.TabWages.Controls.Add(this.panel5);
		this.TabWages.Location = new System.Drawing.Point(4, 25);
		this.TabWages.Name = "TabWages";
		this.TabWages.Padding = new System.Windows.Forms.Padding(3);
		this.TabWages.Size = new System.Drawing.Size(757, 372);
		this.TabWages.TabIndex = 1;
		this.TabWages.Text = "Contractor Wages";
		this.TabWages.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(3, 34);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(751, 335);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 2;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.PanelMessage);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid);
		this.panel5.Controls.Add(this.BtnUpdateConctratorInWages);
		this.panel5.Controls.Add(this.label66);
		this.panel5.Controls.Add(this.label65);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(751, 31);
		this.panel5.TabIndex = 13;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(724, 5);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(25, 23);
		this.PanelMessage.TabIndex = 127;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(25, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.CmbContractorForUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorForUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorForUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorForUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorForUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Location = new System.Drawing.Point(472, 2);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Name = "CmbContractorForUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).TabIndex = 157;
		this.BtnUpdateConctratorInWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateConctratorInWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateConctratorInWages.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateConctratorInWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.Location = new System.Drawing.Point(648, 2);
		this.BtnUpdateConctratorInWages.Name = "BtnUpdateConctratorInWages";
		this.BtnUpdateConctratorInWages.Size = new System.Drawing.Size(72, 26);
		this.BtnUpdateConctratorInWages.TabIndex = 158;
		this.BtnUpdateConctratorInWages.Text = "Update";
		this.BtnUpdateConctratorInWages.UseVisualStyleBackColor = false;
		this.BtnUpdateConctratorInWages.Click += new System.EventHandler(BtnUpdateConctratorInWages_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(393, 6);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(81, 19);
		this.label66.TabIndex = 160;
		this.label66.Text = "Contractor";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(198, 6);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(60, 19);
		this.label65.TabIndex = 161;
		this.label65.Text = "Activity";
		this.CmbActivityforContractorUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivityforContractorUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivityforContractorUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Location = new System.Drawing.Point(259, 2);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Name = "CmbActivityforContractorUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).TabIndex = 156;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(3, 7);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(130, 16);
		this.label33.TabIndex = 126;
		this.label33.Text = "Contractor Wages ";
		this.TabOverheads.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.TabOverheads.Controls.Add(this.panel14);
		this.TabOverheads.Location = new System.Drawing.Point(4, 25);
		this.TabOverheads.Name = "TabOverheads";
		this.TabOverheads.Size = new System.Drawing.Size(757, 372);
		this.TabOverheads.TabIndex = 2;
		this.TabOverheads.Text = "Over Heads";
		this.TabOverheads.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(757, 347);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 1;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.panel9);
		this.panel14.Controls.Add(this.BtnUpdateInOhGrid);
		this.panel14.Controls.Add(this.label22);
		this.panel14.Controls.Add(this.txtPercentToupdateInOhGrid);
		this.panel14.Controls.Add(this.label29);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(757, 25);
		this.panel14.TabIndex = 12;
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.toolStrip3);
		this.panel9.Location = new System.Drawing.Point(730, 1);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(25, 23);
		this.panel9.TabIndex = 159;
		this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripButton1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(25, 25);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripButton1.ToolTipText = "Click to update % in all rows of OverHead grid";
		this.BtnUpdateInOhGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateInOhGrid.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateInOhGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateInOhGrid.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateInOhGrid.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateInOhGrid.Location = new System.Drawing.Point(657, -2);
		this.BtnUpdateInOhGrid.Name = "BtnUpdateInOhGrid";
		this.BtnUpdateInOhGrid.Size = new System.Drawing.Size(72, 26);
		this.BtnUpdateInOhGrid.TabIndex = 160;
		this.BtnUpdateInOhGrid.Text = "Update";
		this.BtnUpdateInOhGrid.UseVisualStyleBackColor = false;
		this.BtnUpdateInOhGrid.Click += new System.EventHandler(BtnUpdateInOhGrid_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(3, 3);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
		this.txtPercentToupdateInOhGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPercentToupdateInOhGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPercentToupdateInOhGrid.Location = new System.Drawing.Point(551, 1);
		this.txtPercentToupdateInOhGrid.Name = "txtPercentToupdateInOhGrid";
		this.txtPercentToupdateInOhGrid.Size = new System.Drawing.Size(105, 23);
		this.txtPercentToupdateInOhGrid.TabIndex = 4;
		this.txtPercentToupdateInOhGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPercentToupdateInOhGrid_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(457, 6);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(92, 15);
		this.label29.TabIndex = 89;
		this.label29.Text = "Percentage (%)";
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Controls.Add(this.btnClearInput);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.BtnLoadInputFromBom);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1183, 220);
		this.panel2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.txtReferenceNoDetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.BtnReferenceParty);
		this.groupBox3.Controls.Add(this.panel16);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox3.Controls.Add(this.BtnAddDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemBOM);
		this.groupBox3.Controls.Add(this.BtnCancelDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateDetail);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBOMNo);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.txtVarientEquivalent);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.txtQty);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttribute);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseName);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 106);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(723, 110);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtReferenceNoDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReferenceNoDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReferenceNoDetail.Location = new System.Drawing.Point(225, 34);
		this.txtReferenceNoDetail.Name = "txtReferenceNoDetail";
		this.txtReferenceNoDetail.Size = new System.Drawing.Size(108, 23);
		this.txtReferenceNoDetail.TabIndex = 2;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(225, 14);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(78, 15);
		this.label47.TabIndex = 501;
		this.label47.Text = "Reference No";
		this.BtnReferenceParty.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnReferenceParty.Location = new System.Drawing.Point(198, 32);
		this.BtnReferenceParty.Name = "BtnReferenceParty";
		this.BtnReferenceParty.Size = new System.Drawing.Size(26, 26);
		this.BtnReferenceParty.TabIndex = 1;
		this.BtnReferenceParty.TabStop = false;
		this.BtnReferenceParty.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add(this.RadCustomers);
		this.panel16.Controls.Add(this.RadRefParty);
		this.panel16.Location = new System.Drawing.Point(3, 12);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(156, 18);
		this.panel16.TabIndex = 498;
		this.RadCustomers.AutoSize = true;
		this.RadCustomers.Checked = true;
		this.RadCustomers.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCustomers.Location = new System.Drawing.Point(2, 0);
		this.RadCustomers.Name = "RadCustomers";
		this.RadCustomers.Size = new System.Drawing.Size(76, 19);
		this.RadCustomers.TabIndex = 493;
		this.RadCustomers.TabStop = true;
		this.RadCustomers.Text = "Customer";
		this.RadCustomers.UseVisualStyleBackColor = true;
		this.RadRefParty.AutoSize = true;
		this.RadRefParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadRefParty.Location = new System.Drawing.Point(82, 0);
		this.RadRefParty.Name = "RadRefParty";
		this.RadRefParty.Size = new System.Drawing.Size(72, 19);
		this.RadRefParty.TabIndex = 494;
		this.RadRefParty.Text = "Ref Party";
		this.RadRefParty.UseVisualStyleBackColor = true;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(162, 14);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(39, 15);
		this.label20.TabIndex = 497;
		this.label20.Text = "Name";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(594, 12);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 496;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(538, 12);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 495;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(2, 32);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 0;
		this.BtnAddDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnAddDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnAddDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddDetail.Location = new System.Drawing.Point(650, 82);
		this.BtnAddDetail.Name = "BtnAddDetail";
		this.BtnAddDetail.Size = new System.Drawing.Size(35, 23);
		this.BtnAddDetail.TabIndex = 10;
		this.BtnAddDetail.Text = "+";
		this.BtnAddDetail.UseVisualStyleBackColor = false;
		this.BtnAddDetail.Click += new System.EventHandler(BtnAddDetail_Click);
		this.cmbItemBOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemBOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemBOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemBOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemBOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Location = new System.Drawing.Point(508, 32);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Name = "cmbItemBOM";
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Leave += new System.EventHandler(cmbItem_Leave);
		this.BtnCancelDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(650, 60);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(69, 21);
		this.BtnCancelDetail.TabIndex = 12;
		this.BtnCancelDetail.Text = "Cancel";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(650, 82);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(69, 23);
		this.BtnUpdateDetail.TabIndex = 11;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(508, 14);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item Name";
		this.cmbBOMNo.AllowNull = (DefaultableBoolean)1;
		this.cmbBOMNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBOMNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBOMNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBOMNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Location = new System.Drawing.Point(2, 80);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Name = "cmbBOMNo";
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Leave += new System.EventHandler(cmbBOMNo_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 62);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "BOM No";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(198, 80);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 6;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(198, 62);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(540, 82);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(106, 23);
		this.txtVarientEquivalent.TabIndex = 9;
		this.txtVarientEquivalent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVarientEquivalent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(540, 62);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(62, 15);
		this.label43.TabIndex = 89;
		this.label43.Text = "Equivalent";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Enabled = false;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(300, 82);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(94, 23);
		this.txtQty.TabIndex = 7;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(300, 62);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.CmbAttribute.AllowNull = (DefaultableBoolean)1;
		this.CmbAttribute.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttribute.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttribute.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttribute.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Location = new System.Drawing.Point(397, 80);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Name = "CmbAttribute";
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Leave += new System.EventHandler(CmbAttribute_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(397, 62);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(54, 15);
		this.label42.TabIndex = 85;
		this.label42.Text = "Attribute";
		this.CmbWarehouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Location = new System.Drawing.Point(335, 32);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Name = "CmbWarehouseName";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).TabIndex = 3;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(335, 14);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(102, 15);
		this.label41.TabIndex = 85;
		this.label41.Text = "Warehouse Name";
		this.btnClearInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnClearInput.FlatAppearance.BorderSize = 0;
		this.btnClearInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearInput.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnClearInput.ForeColor = System.Drawing.Color.White;
		this.btnClearInput.Location = new System.Drawing.Point(731, 167);
		this.btnClearInput.Name = "btnClearInput";
		this.btnClearInput.Size = new System.Drawing.Size(93, 40);
		this.btnClearInput.TabIndex = 3;
		this.btnClearInput.Text = "Clear Input";
		this.btnClearInput.UseVisualStyleBackColor = false;
		this.btnClearInput.Click += new System.EventHandler(btnClearInput_Click);
		this.groupBox2.Controls.Add(this.btnDefinePlant);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPAccount);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantName);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtProductionNo);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(825, 104);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(504, 46);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(28, 26);
		this.btnDefinePlant.TabIndex = 7;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.btnDefinePlant.Click += new System.EventHandler(btnDefinePlant_Click);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 23);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(88, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No && Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(159, 19);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(122, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(96, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(62, 23);
		this.txtdocno.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(2, 52);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(88, 15);
		this.label12.TabIndex = 130;
		this.label12.Text = "Work Order No";
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(96, 74);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Leave += new System.EventHandler(CmbProductionType_Leave);
		this.CmbWorkOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).Location = new System.Drawing.Point(96, 46);
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).Name = "CmbWorkOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNo).Leave += new System.EventHandler(CmbWorkOrderNo_Leave);
		this.CmbWIPAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).Location = new System.Drawing.Point(629, 46);
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).Name = "CmbWIPAccount";
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbWIPAccount).Leave += new System.EventHandler(cmbItem_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(535, 52);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(78, 15);
		this.label36.TabIndex = 85;
		this.label36.Text = "WIP Account";
		this.CmbWIPWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).Location = new System.Drawing.Point(629, 17);
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).Name = "CmbWIPWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbWIPWarehouse).Leave += new System.EventHandler(cmbItem_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(535, 23);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(93, 15);
		this.label27.TabIndex = 85;
		this.label27.Text = "WIP Warehouse";
		this.CmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Location = new System.Drawing.Point(367, 46);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Name = "CmbPlantName";
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Leave += new System.EventHandler(CmbPlantName_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(284, 52);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(69, 15);
		this.label26.TabIndex = 85;
		this.label26.Text = "Plant Name";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(367, 76);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(451, 23);
		this.txtRemarksHeader.TabIndex = 7;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(367, 19);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(165, 23);
		this.txtProductionNo.TabIndex = 4;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(2, 80);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(94, 15);
		this.label16.TabIndex = 89;
		this.label16.Text = "Production Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(284, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(85, 15);
		this.label8.TabIndex = 89;
		this.label8.Text = "Production No";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(284, 80);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 87;
		this.label2.Text = "Remarks";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.BtnLoadInputFromBom.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnLoadInputFromBom.FlatAppearance.BorderSize = 0;
		this.BtnLoadInputFromBom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadInputFromBom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadInputFromBom.ForeColor = System.Drawing.Color.White;
		this.BtnLoadInputFromBom.Location = new System.Drawing.Point(731, 123);
		this.BtnLoadInputFromBom.Name = "BtnLoadInputFromBom";
		this.BtnLoadInputFromBom.Size = new System.Drawing.Size(93, 40);
		this.BtnLoadInputFromBom.TabIndex = 2;
		this.BtnLoadInputFromBom.Text = "Load Input From BOM";
		this.BtnLoadInputFromBom.UseVisualStyleBackColor = false;
		this.BtnLoadInputFromBom.Click += new System.EventHandler(BtnLoadInputFromBom_Click);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.txtOutPutAutoRatePerQtyAmount);
		this.panel4.Controls.Add(this.txtTotalOutPutManualRatedAmount);
		this.panel4.Controls.Add(this.txtTotalOutPutAutoRatedAmount);
		this.panel4.Controls.Add(this.txtTotalInputs);
		this.panel4.Controls.Add(this.txtWagesAmount);
		this.panel4.Controls.Add(this.txtOverHead);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Location = new System.Drawing.Point(830, 1);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(349, 215);
		this.panel4.TabIndex = 2;
		this.txtOutPutAutoRatePerQtyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutPutAutoRatePerQtyAmount.Enabled = false;
		this.txtOutPutAutoRatePerQtyAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOutPutAutoRatePerQtyAmount.Location = new System.Drawing.Point(195, 165);
		this.txtOutPutAutoRatePerQtyAmount.Name = "txtOutPutAutoRatePerQtyAmount";
		this.txtOutPutAutoRatePerQtyAmount.ReadOnly = true;
		this.txtOutPutAutoRatePerQtyAmount.Size = new System.Drawing.Size(149, 25);
		this.txtOutPutAutoRatePerQtyAmount.TabIndex = 5;
		this.txtOutPutAutoRatePerQtyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalOutPutManualRatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalOutPutManualRatedAmount.Enabled = false;
		this.txtTotalOutPutManualRatedAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalOutPutManualRatedAmount.Location = new System.Drawing.Point(195, 106);
		this.txtTotalOutPutManualRatedAmount.Name = "txtTotalOutPutManualRatedAmount";
		this.txtTotalOutPutManualRatedAmount.ReadOnly = true;
		this.txtTotalOutPutManualRatedAmount.Size = new System.Drawing.Size(149, 25);
		this.txtTotalOutPutManualRatedAmount.TabIndex = 3;
		this.txtTotalOutPutManualRatedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalOutPutManualRatedAmount.Visible = false;
		this.txtTotalOutPutAutoRatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalOutPutAutoRatedAmount.Enabled = false;
		this.txtTotalOutPutAutoRatedAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalOutPutAutoRatedAmount.Location = new System.Drawing.Point(195, 138);
		this.txtTotalOutPutAutoRatedAmount.Name = "txtTotalOutPutAutoRatedAmount";
		this.txtTotalOutPutAutoRatedAmount.ReadOnly = true;
		this.txtTotalOutPutAutoRatedAmount.Size = new System.Drawing.Size(149, 25);
		this.txtTotalOutPutAutoRatedAmount.TabIndex = 4;
		this.txtTotalOutPutAutoRatedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Enabled = false;
		this.txtTotalInputs.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalInputs.Location = new System.Drawing.Point(195, 25);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(149, 25);
		this.txtTotalInputs.TabIndex = 0;
		this.txtTotalInputs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWagesAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmount.Enabled = false;
		this.txtWagesAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtWagesAmount.Location = new System.Drawing.Point(195, 79);
		this.txtWagesAmount.Name = "txtWagesAmount";
		this.txtWagesAmount.ReadOnly = true;
		this.txtWagesAmount.Size = new System.Drawing.Size(149, 25);
		this.txtWagesAmount.TabIndex = 2;
		this.txtWagesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Enabled = false;
		this.txtOverHead.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOverHead.Location = new System.Drawing.Point(195, 52);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(149, 25);
		this.txtOverHead.TabIndex = 1;
		this.txtOverHead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(195, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs (Consumed Items)";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(3, 168);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(177, 17);
		this.label31.TabIndex = 97;
		this.label31.Text = "(Produced Item) Value / Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 141);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(175, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Total (Produced Item) Value";
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.label5);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(347, 23);
		this.panel20.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(141, 16);
		this.label5.TabIndex = 33;
		this.label5.Text = "Calculation Summery";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(3, 110);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(173, 17);
		this.label39.TabIndex = 95;
		this.label39.Text = "Total (Manual Rate) Output";
		this.label39.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 83);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(117, 17);
		this.label13.TabIndex = 95;
		this.label13.Text = "Total Wages Value";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(148, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Total Over Heads Value";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(115, 126);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(15, 16);
		this.label25.TabIndex = 93;
		this.label25.Text = "=";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(115, 100);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(12, 16);
		this.label38.TabIndex = 93;
		this.label38.Text = "-";
		this.label38.Visible = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(115, 71);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(15, 16);
		this.label24.TabIndex = 93;
		this.label24.Text = "+";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(115, 42);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(15, 16);
		this.label3.TabIndex = 93;
		this.label3.Text = "+";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Enabled = false;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(1343, 18);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label4.AutoSize = true;
		this.label4.Enabled = false;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1273, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 17);
		this.label4.TabIndex = 126;
		this.label4.Text = "Company";
		this.label4.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1343, 48);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label45.AutoSize = true;
		this.label45.Enabled = false;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1289, 53);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(47, 17);
		this.label45.TabIndex = 127;
		this.label45.Text = "Branch";
		this.label45.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Enabled = false;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1343, 79);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label46.AutoSize = true;
		this.label46.Enabled = false;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1289, 84);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 17);
		this.label46.TabIndex = 128;
		this.label46.Text = "Project";
		this.label46.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.chkPrintVoucher);
		this.panel3.Controls.Add(this.ChkPreviewSlip);
		this.panel3.Controls.Add(this.labelWagesExpAc);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1183, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnVoucherReport, this.btnPrint, this.toolStripSeparator5,
			this.btnattachment, this.btnshortcutkeys, this.toolStripSeparator2, this.btnUpdateInputWagesOH, this.btnGenerateRatesOutput
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1183, 27);
		this.toolStrip1.TabIndex = 2;
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
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1850-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnUpdateInputWagesOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateInputWagesOH.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.btnUpdateInputWagesOH.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateInputWagesOH.Name = "btnUpdateInputWagesOH";
		this.btnUpdateInputWagesOH.Size = new System.Drawing.Size(190, 24);
		this.btnUpdateInputWagesOH.Text = "Update Input,Wages,OH";
		this.btnUpdateInputWagesOH.Click += new System.EventHandler(btnUpdateInputWagesOH_Click);
		this.btnGenerateRatesOutput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateRatesOutput.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.btnGenerateRatesOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRatesOutput.Name = "btnGenerateRatesOutput";
		this.btnGenerateRatesOutput.Size = new System.Drawing.Size(177, 24);
		this.btnGenerateRatesOutput.Text = "Generate Output Rate";
		this.btnGenerateRatesOutput.Click += new System.EventHandler(btnGenerateRatesOutput_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(97, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Production";
		this.chkPrintVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintVoucher.AutoSize = true;
		this.chkPrintVoucher.Checked = true;
		this.chkPrintVoucher.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintVoucher.ForeColor = System.Drawing.Color.White;
		this.chkPrintVoucher.Location = new System.Drawing.Point(1052, 29);
		this.chkPrintVoucher.Name = "chkPrintVoucher";
		this.chkPrintVoucher.Size = new System.Drawing.Size(124, 20);
		this.chkPrintVoucher.TabIndex = 15;
		this.chkPrintVoucher.Text = "Voucher Print";
		this.chkPrintVoucher.UseVisualStyleBackColor = true;
		this.ChkPreviewSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreviewSlip.AutoSize = true;
		this.ChkPreviewSlip.Checked = true;
		this.ChkPreviewSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreviewSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreviewSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPreviewSlip.Location = new System.Drawing.Point(960, 29);
		this.ChkPreviewSlip.Name = "ChkPreviewSlip";
		this.ChkPreviewSlip.Size = new System.Drawing.Size(90, 20);
		this.ChkPreviewSlip.TabIndex = 16;
		this.ChkPreviewSlip.Text = "Slip Print";
		this.ChkPreviewSlip.UseVisualStyleBackColor = true;
		this.labelWagesExpAc.AutoSize = true;
		this.labelWagesExpAc.BackColor = System.Drawing.Color.Transparent;
		this.labelWagesExpAc.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelWagesExpAc.ForeColor = System.Drawing.Color.White;
		this.labelWagesExpAc.Location = new System.Drawing.Point(535, 26);
		this.labelWagesExpAc.Name = "labelWagesExpAc";
		this.labelWagesExpAc.Size = new System.Drawing.Size(93, 30);
		this.labelWagesExpAc.TabIndex = 85;
		this.labelWagesExpAc.Text = "Wages Expense\r\nAccount";
		this.labelWagesExpAc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWagesExpenseAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesExpenseAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesExpenseAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesExpenseAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesExpenseAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesExpenseAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).Location = new System.Drawing.Point(633, 28);
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).Name = "CmbWagesExpenseAccount";
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbWagesExpenseAccount).Leave += new System.EventHandler(cmbItem_Leave);
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1189, 682);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 356);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1189, 326);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.panel8.Controls.Add(this.panel42);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(544, 326);
		this.panel8.TabIndex = 50;
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(544, 111);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 1;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 196);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(544, 19);
		this.panel42.TabIndex = 58;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(78, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(544, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label14);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(544, 19);
		this.panel44.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 1);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(222, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Input Material (Consumed Items)";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesHistory);
		this.panel19.Controls.Add(this.panel18);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 326);
		this.panel19.TabIndex = 4;
		this.grdWagesHistory.AllowEdit = (InheritableBoolean)2;
		grdWagesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesHistory.DesignTimeLayout = grdWagesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesHistory.DynamicFiltering = true;
		this.grdWagesHistory.FilterMode = (FilterMode)2;
		this.grdWagesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Name = "grdWagesHistory";
		((ScrollableBase)this.grdWagesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Size = new System.Drawing.Size(645, 111);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).TabIndex = 1;
		this.grdWagesHistory.TotalRow = (InheritableBoolean)1;
		this.grdWagesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label40);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 196);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(645, 19);
		this.panel18.TabIndex = 55;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(3, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(126, 16);
		this.label40.TabIndex = 127;
		this.label40.Text = "Contractor Wages";
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 0;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(645, 19);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(170, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut (Produced Items)";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 140);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1189, 216);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 117);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1189, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1142, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(54, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 51);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1189, 66);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1181, 64);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(1074, 35);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 9;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(995, 35);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 8;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(950, 35);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 7;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(885, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(673, 15);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Production No";
		this.cmbProductionNoHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Location = new System.Drawing.Point(679, 33);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Name = "cmbProductionNoHistory";
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).TabIndex = 5;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 15);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 33);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(327, 35);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(152, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(167, 15);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(579, 35);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(173, 35);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(152, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(475, 15);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(481, 35);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1189, 51);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1189, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(61, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 29);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(170, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Production History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1197, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmConcreteProduction";
		this.Text = "Concrete Production";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmConcreteProduction_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.TabOutPut.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.TabWages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).EndInit();
		this.TabOverheads.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesExpenseAccount).EndInit();
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
