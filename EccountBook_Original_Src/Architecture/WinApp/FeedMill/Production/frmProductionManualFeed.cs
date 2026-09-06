using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.FeedMill.Production;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.General;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.FeedMill.Production;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.EmployeeManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FeedMill.Production;

public class frmProductionManualFeed : Form
{
	private DataTable dtInput = new DataTable();

	private DataTable dtOutPutByProduct = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtConsupmtionDetail = new DataTable();

	private DataTable dtOutputItemCombo = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtBOMDataforCombo = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtWare = new DataTable();

	private DataTable lstConsumptionitem = new DataTable();

	private DataTable DtStock = new DataTable();

	private DataTable dtOutputBom = new DataTable();

	private DataTable dtOutputItem = new DataTable();

	private int updateDetailIndex = -1;

	private bool UpdateMode;

	private int RecId;

	private bool RestrictSingleBomInOutPut;

	private bool ProductionPackingMaterialRequired;

	private bool AdminRole;

	private WaitForm WF = new WaitForm();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<ProductionInputDetail> lstRemoveRecordInput = new List<ProductionInputDetail>();

	private List<ProductionOutPutDetail> lstRemoveRecordOutPut = new List<ProductionOutPutDetail>();

	private List<ProductionConsumptionDetail> lstRemoveRecordConsumption = new List<ProductionConsumptionDetail>();

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

	private TextBox txtWagesAmount;

	internal GridEX grdConsumptionHistory;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdConsupmtionDetail;

	private Panel panel5;

	private Label label33;

	internal GridEX grdOverHead;

	private Panel panel14;

	private Label label22;

	private Panel panel17;

	private Panel panel6;

	internal GridEX grdOutPutByProduct;

	private TextBox txtProductionNo;

	private Label label8;

	private Button BtnLoadInputFromBom;

	private ToolStripButton btnGenerateRatesOutput;

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

	internal UltraCombo CmbPlantName;

	private Label label26;

	private ToolStripButton btnattachment;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private GroupBox groupBox3;

	private Button BtnAddDetail;

	private Button BtnUpdateDetail;

	private Button BtnCancelDetail;

	private Button BtnUpdateWarehouseInConsumption;

	private Button btnClearInput;

	internal UltraCombo CmbWarehouseName;

	private Label label41;

	private Label label12;

	private Label label16;

	private TabControl tabControl2;

	private TabPage tabOutput;

	private TabPage tabConsumption;

	private TabPage tabOH;

	private TabControl tabControl3;

	private TabPage tabPage2;

	private TabPage tabPage4;

	private TabPage tabPage5;

	private ToolStripButton BtnFormulasOnForm;

	private ToolStripButton BtnSaveAs;

	private ToolStripButton btnRecordsUpdate;

	internal UltraCombo CmbProductionType;

	private Label label20;

	internal UltraCombo CmbProductionTypeHistory;

	private Label label27;

	private ToolStripButton btnReProcessing;

	private DateTimePicker datTodate;

	private DateTimePicker datFromDate;

	private Button BtnUpdateQtyByBom;

	internal UltraCombo CmbShift;

	private Label label29;

	private Button brnShift;

	internal UltraCombo CmbShiftNameHistory;

	private Label label36;

	private ToolStripButton btnRefreshHistory;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar7;

	public frmProductionManualFeed(UserAccount user)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
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
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			BtnSaveAs.Enabled = formright.DoHaveSaveRight;
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation r) => r.ConfigDescription == "RestrictSingleBomInOutPut").FirstOrDefault();
			RestrictSingleBomInOutPut = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation r) => r.ConfigDescription == "ProductionPackingMaterialRequired").FirstOrDefault();
			ProductionPackingMaterialRequired = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			BtnUpdateQtyByBom.Visible = RestrictSingleBomInOutPut;
			AdminRole = UserAccount.RoleName == "Admin";
			WF.Show(this);
			dtOutputBom.Columns.Add("BomHeaderId", typeof(int));
			dtOutputBom.Columns.Add("BomNo", typeof(string));
			dtOutputItem.Columns.Add("OutItemId", typeof(int));
			dtOutputItem.Columns.Add("OutItemName", typeof(string));
			dtOutputItemCombo.Columns.Add("Id", typeof(int));
			dtOutputItemCombo.Columns.Add("ItemName", typeof(string));
			dtOutputItemCombo.Columns.Add("ItemCode", typeof(string));
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtAccounts.Columns.Add("AccountCode");
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			dtFillsForGrids();
			BOMNoAndItemFill();
			DocumentNoFill();
			ShiftComboFill();
			ProductionTypeFill();
			PlantFeader();
			ItemFillFromBom();
			uomgetAll();
			WarehouseFill();
			Datetypefill();
			AllCombobind();
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("BomHeaderId", typeof(int));
			dtInput.Columns.Add("BomNo", typeof(string));
			dtInput.Columns.Add("BomInputId", typeof(int));
			dtInput.Columns.Add("WarehouseId", typeof(int));
			dtInput.Columns.Add("WarehouseName", typeof(string));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemName", typeof(string));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("BalanceStock", typeof(decimal));
			dtInput.Columns.Add("BomQty", typeof(decimal));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("PrctOfTotalQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("BomRate", typeof(decimal));
			dtInput.Columns.Add("Rate", typeof(decimal));
			dtInput.Columns.Add("RateUomId", typeof(int));
			dtInput.Columns.Add("RateUom", typeof(decimal));
			dtInput.Columns.Add("RateUomCode", typeof(string));
			dtInput.Columns.Add("Amount", typeof(decimal));
			dtInput.Columns.Add("BomMinQty", typeof(decimal));
			dtInput.Columns.Add("BomMaxQty", typeof(decimal));
			grdInputMaterial.DataSource = dtInput;
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOutPutByProduct.Columns.Add("Id", typeof(int));
			dtOutPutByProduct.Columns.Add("BomHeaderId", typeof(int));
			dtOutPutByProduct.Columns.Add("BomNo", typeof(string));
			dtOutPutByProduct.Columns.Add("WarehouseId", typeof(int));
			dtOutPutByProduct.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemId", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("Uom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("UomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("ConsumptionQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProduct.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("RateUom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProduct.Columns.Add("BomQty", typeof(decimal));
			dtOutPutByProduct.Rows.Add(0, 0, 0, 0, "", 0, "", 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0);
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
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
			dtOverHead.Columns.Add("BomAmount", typeof(decimal));
			dtOverHead.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("OhRemarks", typeof(string));
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			dtConsupmtionDetail.Columns.Add("Id", typeof(int));
			dtConsupmtionDetail.Columns.Add("BomHeaderId", typeof(int));
			dtConsupmtionDetail.Columns.Add("BomNo", typeof(string));
			dtConsupmtionDetail.Columns.Add("BomConsumptionId", typeof(int));
			dtConsupmtionDetail.Columns.Add("WarehouseId", typeof(int));
			dtConsupmtionDetail.Columns.Add("WarehouseName", typeof(string));
			dtConsupmtionDetail.Columns.Add("ItemId", typeof(int));
			dtConsupmtionDetail.Columns.Add("ItemName", typeof(string));
			dtConsupmtionDetail.Columns.Add("ParentCategory", typeof(string));
			dtConsupmtionDetail.Columns.Add("ItemUomId", typeof(int));
			dtConsupmtionDetail.Columns.Add("Uom", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("UomCode", typeof(string));
			dtConsupmtionDetail.Columns.Add("BalanceStock", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("BomQty", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("ItemQty", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("NetWeight", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("BomRate", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("Rate", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("RateUomId", typeof(int));
			dtConsupmtionDetail.Columns.Add("RateUom", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("RateUomCode", typeof(string));
			dtConsupmtionDetail.Columns.Add("Amount", typeof(decimal));
			dtConsupmtionDetail.Columns.Add("OutPutItemId", typeof(int));
			AddRowInConsumptionGrid();
			grdConsupmtionDetail.RetrieveStructure();
			grdConsumptionGridSetting();
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

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = ProductionHeader.GenerateCode(new ProductionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1122
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ProductionTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbProductionType.Value);
			DataTable dt = ProductionType.GetAll(new ProductionType());
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbProductionType, "Id", "ProductionTypeDescription", "Production Type", false);
			((UltraGridBase)CmbProductionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbProductionType.Value = Id;
				}
				else
				{
					((Control)(object)CmbProductionType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ShiftComboFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbShift.Value);
			DataTable dt = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbShift, "ShiftId", "ShiftName", "Shift", false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ShiftId") == Id))
					{
						CmbShift.Value = Id;
					}
					else
					{
						((Control)(object)CmbShift).Text = string.Empty;
					}
				}
				else
				{
					((UltraGridBase)CmbShift).Rows[0].Activate();
				}
			}
			else
			{
				((Control)(object)CmbShift).Text = string.Empty;
				((UltraGridBase)CmbShift).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbPlantName.Value);
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, CmbPlantName, "Id", "Description", "Plant", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbPlantName.Value = BomId;
					}
					else
					{
						((Control)(object)CmbPlantName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPlantName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void dtFillsForGrids()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dtWare.Clear();
			dtWare = CommonServices.getActiveWareHouse();
			dtItem.Clear();
			dtItem = CommonServices.ReadAllItems();
			new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				dtAccounts.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 11 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 12 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 13 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 14 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 20 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 21)
					{
						dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["AccountCode"]);
					}
				}
			}
			lstConsumptionitem.Clear();
			lstConsumptionitem = CommonServices.GetItemByItemTypeId("14,17");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemFillFromBom()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			dtOutputItemCombo.Rows.Clear();
			int ItId = Conversion.ToInt(cmbItemBOM.Value);
			DataTable dt = BomHeader.GetAllBomItems(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1120
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtOutputItemCombo.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
				if (dtOutputItemCombo.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtOutputItemCombo, cmbItemBOM, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ItId > 0)
				{
					if (dtOutputItemCombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						cmbItemBOM.Value = ItId;
					}
					else
					{
						((Control)(object)cmbItemBOM).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbItemBOM).DataSource = null;
				((UltraGridBase)cmbBOMNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbProductionType.Value) != 4 && Conversion.ToInt(CmbProductionType.Value) != 5)
			{
				BOMNobyItemFill(Conversion.ToInt(cmbItemBOM.Value));
			}
			else if (Conversion.ToInt(CmbProductionType.Value) == 4 || Conversion.ToInt(CmbProductionType.Value) == 5)
			{
				string Uom = ((Control)(object)cmbUOM).Text;
				((Control)(object)cmbUOM).Text = string.Empty;
				((Control)(object)cmbBOMNo).Text = string.Empty;
				((UltraGridBase)cmbBOMNo).DataSource = null;
				bindUom(Conversion.ToInt(cmbItemBOM.Value));
				DataTable dtuomheader = new DataTable();
				dtuomheader.Columns.Add("Id", typeof(int));
				dtuomheader.Columns.Add("ItemUOM");
				dtuomheader.Columns.Add("ItemUOMEquivalent", typeof(double));
				if (dtUom.Rows.Count > 0)
				{
					for (int i = 0; i < dtUom.Rows.Count; i++)
					{
						dtuomheader.Rows.Add(dtUom.Rows[i]["Id"], dtUom.Rows[i]["ItemUOMCode"], dtUom.Rows[i]["ItemUOM"]);
					}
					DDL.BindDDL(dtuomheader, cmbUOM, "Id", "ItemUOM", "UOM", ZeroIndex: true);
					((UltraGridBase)cmbUOM).Rows[1].Activate();
					if (Uom != string.Empty)
					{
						if (dtuomheader.AsEnumerable().Any((DataRow row) => row.Field<string>("ItemUOM") == Uom))
						{
							((Control)(object)cmbUOM).Text = Uom;
						}
						else
						{
							((Control)(object)cmbUOM).Text = string.Empty;
						}
					}
				}
			}
			if (dtWare.Rows.Count > 0 && ((UltraGridBase)CmbWarehouseName).DataSource != null && Conversion.ToInt(CmbWarehouseName.Value) == 0)
			{
				int warehouseId = 0;
				DataRow firstRow = (from row in dtWare.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).FirstOrDefault();
				if (firstRow != null)
				{
					warehouseId = Conversion.ToInt(firstRow["Id"]);
				}
				CmbWarehouseName.Value = warehouseId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BOMNobyItemFill(int ItemId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(cmbBOMNo.Value);
			DataTable dtBomNo = BomHeader.GetBomNoByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ItemId = ItemId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1120
			});
			if (dtBomNo.Rows.Count > 0)
			{
				if (dtBomNo.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtBomNo, cmbBOMNo, "Id", "BomName", "Bom No", ZeroIndex: true);
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbBOMNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (BomId > 0)
				{
					if (dtBomNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						cmbBOMNo.Value = BomId;
					}
					else
					{
						((Control)(object)cmbBOMNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbBOMNo).DataSource = null;
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
			if (Conversion.ToInt(CmbProductionType.Value) != 4 && Conversion.ToInt(CmbProductionType.Value) != 5)
			{
				BindHeaderItemUom(Conversion.ToInt(cmbBOMNo.Value));
				DataRow foundRow = dtBOMDataforCombo.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(cmbBOMNo.Value));
				txtQty.Text = ((foundRow != null) ? Conversion.ToString(foundRow["BomBatchQty"]) : "");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			obj.DocumentTypeId = 1120;
			dtBOMDataforCombo = BomHeader.GetAllBomNoAndItems(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WarehouseFill()
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
			}
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

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindUom(int ItemId)
	{
		try
		{
			dtUom.Clear();
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUom(int ItemId)
	{
		try
		{
			dtRateUom.Clear();
			if (dtuomGetAll.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtRateUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnClearInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("are you sure to reset Input Detail?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				grdInputMaterial.ClearStructure();
				dtInput.Clear();
				AddRowInInputGrid();
				grdInputMaterial.DataSource = dtInput;
				grdInputMaterial.RetrieveStructure();
				grdInputSettings();
				Calculation();
			}
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
			FillInputDetailForMultiBom();
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow r3 = grdInputMaterial.GetDataRows().FirstOrDefault();
				if (r3 != null && (int)r3.RowType == 0 && r3.Cells.Count > 0)
				{
					if (Conversion.ToInt(r3.Cells["ItemId"].Value) > 0 || Conversion.ToInt(r3.Cells["ItemUomId"].Value) > 0 || Conversion.ToInt(r3.Cells["RateUomId"].Value) > 0)
					{
						if (MessageBox.Show("Input Grid already have Records.Do you want To Refresh Data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							return;
						}
					}
					else
					{
						dtInput.Clear();
						grdInputMaterial.ClearStructure();
					}
				}
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Record Not Found...");
			}
			int warehouseId = 0;
			string warehouseName = "";
			if (RestrictSingleBomInOutPut)
			{
				int BomId = Conversion.ToInt((object)((grdOutPutByProduct.GetDataRows().Count() > 0) ? grdOutPutByProduct.GetDataRows()[0] : null).Cells["BomHeaderId"].Value.ToString());
				DataTable InputData = BomHeader.getBomdataBySingleBomId(UserAccount.OrganizationId, UserAccount.CompanyId, BomId);
				if (InputData.Rows.Count > 0)
				{
					if (dtWare.Rows.Count > 0)
					{
						DataRow firstRow = (from row in dtWare.AsEnumerable()
							where row.Field<int>("Id") > 0
							select row).FirstOrDefault();
						if (firstRow != null)
						{
							warehouseId = Conversion.ToInt(firstRow["Id"]);
							warehouseName = Conversion.ToString(firstRow["WarehouseName"]);
						}
					}
					foreach (DataRow r in InputData.Rows)
					{
						bool flag = false;
						if (grdInputMaterial.GetDataRows().Length != 0)
						{
							GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
							for (int i = 0; i < dataRows.Length; i++)
							{
								if (Conversion.ToInt(dataRows[i].Cells["BomInputId"].Value) == Conversion.ToInt(r["BomInputId"]))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							dtInput.Rows.Add(0, r["BomHeaderId"], r["BomHeaderNo"], r["BomInputId"], warehouseId, warehouseName, r["ItemId"], r["ItemName"], r["ItemUomId"], r["PUomEquivalent"], r["PackUom"], 0, r["ItemQty"], r["ItemQty"], 0, Conversion.ToDouble(r["ItemQty"]) * Conversion.ToDouble(r["PUomEquivalent"]), 0, 0, (Conversion.ToInt(r["RateUomId"]) > 0) ? Conversion.ToInt(r["RateUomId"]) : Conversion.ToInt(r["ItemUomId"]), (Conversion.ToInt(r["RateUomId"]) > 0) ? Conversion.ToInt(r["RateEquivalent"]) : Conversion.ToInt(r["PUomEquivalent"]), (Conversion.ToInt(r["RateUomId"]) > 0) ? Conversion.ToInt(r["RateUom"]) : Conversion.ToInt(r["PackUom"]), 0, 0, 0);
						}
					}
				}
				else if (grdInputMaterial.GetDataRows().Count() == 0)
				{
					AddRowInInputGrid();
				}
			}
			else
			{
				string BomIds = "";
				GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
				foreach (GridEXRow r4 in dataRows)
				{
					if (Conversion.ToInt(r4.Cells["BomHeaderId"].Value) > 0 && !BomIds.Contains(Conversion.ToString(r4.Cells["BomHeaderId"].Value)))
					{
						BomIds = BomIds + "," + Conversion.ToInt(r4.Cells["BomHeaderId"].Value);
					}
				}
				DataTable InputData2 = BomHeader.GetBomdataByIds(UserAccount.OrganizationId, UserAccount.CompanyId, BomIds);
				if (InputData2.Rows.Count > 0)
				{
					if (dtWare.Rows.Count > 0)
					{
						DataRow firstRow2 = (from row in dtWare.AsEnumerable()
							where row.Field<int>("Id") > 0
							select row).FirstOrDefault();
						if (firstRow2 != null)
						{
							warehouseId = Conversion.ToInt(firstRow2["Id"]);
							warehouseName = Conversion.ToString(firstRow2["WarehouseName"]);
						}
					}
					foreach (DataRow r2 in InputData2.Rows)
					{
						bool flag2 = false;
						if (grdInputMaterial.GetDataRows().Length != 0)
						{
							dataRows = grdInputMaterial.GetDataRows();
							foreach (GridEXRow item in dataRows)
							{
								if (Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(r2["ItemId"]) && Conversion.ToInt(item.Cells["ItemUomId"].Value) == Conversion.ToInt(r2["ItemUomId"]) && Conversion.ToInt(item.Cells["RateUomId"].Value) == Conversion.ToInt(r2["RateUomId"]))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							dtInput.Rows.Add(0, 0, 0, 0, warehouseId, warehouseName, r2["ItemId"], r2["ItemName"], r2["ItemUomId"], r2["PUomEquivalent"], r2["PackUom"], 0, 0, 0, 0, 0, 0, 0, (Conversion.ToInt(r2["RateUomId"]) > 0) ? Conversion.ToInt(r2["RateUomId"]) : Conversion.ToInt(r2["ItemUomId"]), (Conversion.ToInt(r2["RateUomId"]) > 0) ? Conversion.ToInt(r2["RateEquivalent"]) : Conversion.ToInt(r2["PUomEquivalent"]), (Conversion.ToInt(r2["RateUomId"]) > 0) ? Conversion.ToInt(r2["RateUom"]) : Conversion.ToInt(r2["PackUom"]), 0, 0, 0);
						}
					}
				}
				else if (grdInputMaterial.GetDataRows().Count() == 0)
				{
					AddRowInInputGrid();
				}
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			Calculation();
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
		if (((Control)(object)CmbPlantName).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionType.Value) == 0)
		{
			MessageBox.Show("Production Type Field Required");
			CmbProductionType.Focus();
			return false;
		}
		if (((Control)(object)CmbShift).Text.Trim() == string.Empty || Conversion.ToInt(CmbShift.Value) == 0)
		{
			MessageBox.Show("Shift Field Required");
			CmbShift.Focus();
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
			MessageBox.Show("Plant Name Field Required");
			CmbPlantName.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOutPutGrid()
	{
		if (((Control)(object)CmbWarehouseName).Text.Trim() == string.Empty || Conversion.ToInt(CmbWarehouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehouseName.Focus();
			return false;
		}
		if (((Control)(object)cmbItemBOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbItemBOM.Value) == 0)
		{
			MessageBox.Show("ItemBOM Field Required");
			cmbItemBOM.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbProductionType.Value) != 4 && Conversion.ToInt(CmbProductionType.Value) != 5 && (((Control)(object)cmbBOMNo).Text.Trim() == string.Empty || Conversion.ToInt(cmbBOMNo.Value) == 0))
		{
			MessageBox.Show("BOM No Field Required");
			cmbBOMNo.Focus();
			return false;
		}
		if (((Control)(object)cmbUOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("BOM Item Uom No Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text == "0" || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		return true;
	}

	private void AddRowInInputGrid()
	{
		try
		{
			dtInput.Rows.Add(0, 0, "", 0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdInputMaterial.DataSource = dtInput;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputMaterial.AllowEdit = (InheritableBoolean)1;
			grdInputMaterial.RootTable.Columns["Id"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomInputId"].Visible = false;
			grdInputMaterial.RootTable.Columns["WarehouseId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["Uom"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomHeaderId"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomHeaderId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUom"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomMinQty"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomMaxQty"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomRate"].Visible = false;
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomQty"].Visible = false;
			grdInputMaterial.RootTable.Columns["BomNo"].Visible = false;
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdInputMaterial.RootTable.Columns["Rate"].Caption = "Avg Rate";
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].Caption = "%";
			grdInputMaterial.RootTable.Columns["BalanceStock"].Caption = "Stock";
			grdInputMaterial.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdInputMaterial.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputMaterial.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdInputMaterial.RootTable.Columns["BomHeaderId"].Width = 100;
			grdInputMaterial.RootTable.Columns["BomNo"].Width = 70;
			grdInputMaterial.RootTable.Columns["WarehouseId"].Width = 120;
			grdInputMaterial.RootTable.Columns["WarehouseName"].Width = 100;
			grdInputMaterial.RootTable.Columns["ItemId"].Width = 100;
			grdInputMaterial.RootTable.Columns["ItemName"].Width = 100;
			grdInputMaterial.RootTable.Columns["UomCode"].Width = 50;
			grdInputMaterial.RootTable.Columns["BalanceStock"].Width = 70;
			grdInputMaterial.RootTable.Columns["BomRate"].Width = 70;
			grdInputMaterial.RootTable.Columns["Rate"].Width = 70;
			grdInputMaterial.RootTable.Columns["Amount"].Width = 80;
			grdInputMaterial.RootTable.Columns["BomQty"].Width = 80;
			grdInputMaterial.RootTable.Columns["ItemQty"].Width = 80;
			grdInputMaterial.RootTable.Columns["BomMinQty"].Width = 80;
			grdInputMaterial.RootTable.Columns["BomMaxQty"].Width = 80;
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].Width = 60;
			grdInputMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomInputId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
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
			grdInputMaterial.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].EditType = (EditType)0;
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
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["PrctOfTotalQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns.Add("Delete");
			grdInputMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdInputMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdInputMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputMaterial.RootTable.Columns["Delete"].Width = 20;
			grdInputMaterial.RootTable.Columns["Delete"].Position = 0;
			grdInputMaterial.RootTable.Columns.Add("Add");
			grdInputMaterial.RootTable.Columns["Add"].Key = "Add";
			grdInputMaterial.RootTable.Columns["Add"].Caption = "+";
			grdInputMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Add"].ButtonText = "+";
			grdInputMaterial.RootTable.Columns["Add"].Width = 20;
			grdInputMaterial.RootTable.Columns["Add"].Position = 2;
			grdInputMaterial.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId")
				{
					if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
						decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
						decimal Weight = num * quom;
						item.Cells["NetWeight"].Value = Weight;
						double AvgRat = CommonServices.getAvgRateByItemId(Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), 1122, RecId);
						Conversion.ToDecimal((object)AvgRat);
						item.Cells["Rate"].Value = AvgRat;
						decimal TotalGridQty = ((grdInputMaterial.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["ItemQty"], (AggregateFunction)2)) : 0m);
						decimal BomQty = Conversion.ToDecimal((object)item.Cells["BomQty"].Value.ToString());
						decimal PrctOftotal = ((TotalGridQty > 0m) ? (BomQty / TotalGridQty * 100m) : 0m);
						item.Cells["PrctOfTotalQty"].Value = PrctOftotal;
						decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
						decimal Amount2 = ((Weight > 0m) ? (Weight * rate) : 0m);
						item.Cells["Amount"].Value = Amount2;
					}
					else
					{
						item.Cells["NetWeight"].Value = 0;
						item.Cells["Rate"].Value = 0;
						item.Cells["PrctOfTotalQty"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				if (e.Column.Key == "Rate")
				{
					if (Conversion.ToDouble(item.Cells["Rate"].Value) != 0.0 && item.Cells["RateUomId"].Value.ToString() != string.Empty)
					{
						decimal NetWeight = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
						decimal AvgRate = Conversion.ToDecimal(item.Cells["Rate"].Value);
						decimal Amount = ((NetWeight > 0m) ? (NetWeight * AvgRate) : 0m);
						item.Cells["Amount"].Value = Amount;
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionInputDetail d1 = new ProductionInputDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
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
			if (e.Column.Key == "Add")
			{
				AddRowInInputGrid();
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInConsumptionGrid()
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdConsupmtionDetail.GetDataRows().Count() == 0)
			{
				dtConsupmtionDetail.Rows.Add(0, 0, "", 0, 0, "", 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			}
			else
			{
				int BomHeaderId = 0;
				string BomNo = "";
				int OutPutItemId = 0;
				GridEXRow r = grdConsupmtionDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (BomHeaderId == 0 && OutPutItemId == 0)
					{
						GridEXRow[] dataRows = grdConsupmtionDetail.GetDataRows();
						foreach (GridEXRow row in dataRows)
						{
							BomHeaderId = Conversion.ToInt(row.Cells["BomHeaderId"].Value);
							OutPutItemId = Conversion.ToInt(row.Cells["OutPutItemId"].Value);
							if (BomHeaderId != 0 && OutPutItemId != 0)
							{
								BomNo = Conversion.ToString(row.Cells["BomNo"].Value);
								break;
							}
						}
					}
					else
					{
						BomNo = Conversion.ToString(r.Cells["BomNo"].Value);
					}
				}
				dtConsupmtionDetail.Rows.Add(0, BomHeaderId, BomNo, 0, 0, "", 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, OutPutItemId);
			}
			grdConsupmtionDetail.DataSource = dtConsupmtionDetail;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdConsumptionGridSetting()
	{
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdConsupmtionDetail.AllowEdit = (InheritableBoolean)1;
			grdConsupmtionDetail.RootTable.Columns["Id"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomConsumptionId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomHeaderId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["ItemUomId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["Uom"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["NetWeight"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["RateUom"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["RateUomCode"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["OutPutItemId"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["BomNo"].Visible = false;
			grdConsupmtionDetail.RootTable.Columns["UomCode"].Caption = "Uom";
			grdConsupmtionDetail.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].Caption = "Stock";
			grdConsupmtionDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BomConsumptionId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["OutPutItemId"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.Columns["ParentCategory"].EditType = (EditType)0;
			grdConsupmtionDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdConsupmtionDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdConsupmtionDetail.RootTable.Columns["BomHeaderId"].Width = 100;
			grdConsupmtionDetail.RootTable.Columns["BomNo"].Width = 70;
			grdConsupmtionDetail.RootTable.Columns["WarehouseId"].Width = 120;
			grdConsupmtionDetail.RootTable.Columns["WarehouseName"].Width = 100;
			grdConsupmtionDetail.RootTable.Columns["ItemId"].Width = 100;
			grdConsupmtionDetail.RootTable.Columns["ItemName"].Width = 100;
			grdConsupmtionDetail.RootTable.Columns["ParentCategory"].Width = 100;
			grdConsupmtionDetail.RootTable.Columns["UomCode"].Width = 60;
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].Width = 80;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].Width = 80;
			grdConsupmtionDetail.RootTable.Columns["Rate"].Width = 70;
			grdConsupmtionDetail.RootTable.Columns["Amount"].Width = 90;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].Width = 80;
			grdConsupmtionDetail.RootTable.Columns["ItemQty"].Width = 80;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].AggregateFunction = (AggregateFunction)2;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["BomQty"].FormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["BomQty"].TotalFormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdConsupmtionDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].AggregateFunction = (AggregateFunction)2;
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].FormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["BalanceStock"].TotalFormatString = "##,#.####";
			grdConsupmtionDetail.RootTable.Columns["BomRate"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdConsupmtionDetail.RootTable.Columns["BomRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdConsupmtionDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdConsupmtionDetail.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdConsupmtionDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdConsupmtionDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdConsupmtionDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdConsupmtionDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdConsupmtionDetail.RootTable.Columns.Add("Delete");
			grdConsupmtionDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdConsupmtionDetail.RootTable.Columns["Delete"].Caption = "X";
			grdConsupmtionDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdConsupmtionDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdConsupmtionDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdConsupmtionDetail.RootTable.Columns["Delete"].Width = 20;
			grdConsupmtionDetail.RootTable.Columns["Delete"].Position = 0;
			grdConsupmtionDetail.RootTable.Columns.Add("Add");
			grdConsupmtionDetail.RootTable.Columns["Add"].Key = "Add";
			grdConsupmtionDetail.RootTable.Columns["Add"].Caption = "+";
			grdConsupmtionDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdConsupmtionDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdConsupmtionDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdConsupmtionDetail.RootTable.Columns["Add"].Width = 20;
			grdConsupmtionDetail.RootTable.Columns["Add"].Position = 1;
			grdConsupmtionDetail.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdConsumption_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdConsupmtionDetail.UpdateData();
			GridEXRow item = grdConsupmtionDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId")
				{
					if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
						decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
						decimal Weight = num * quom;
						item.Cells["NetWeight"].Value = Weight;
						double AvgRat = CommonServices.getAvgRateByItemId(Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), 1122, RecId);
						Conversion.ToDecimal((object)AvgRat);
						item.Cells["Rate"].Value = AvgRat;
						decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
						decimal Amount2 = ((Weight > 0m) ? (Weight * rate) : 0m);
						item.Cells["Amount"].Value = Amount2;
					}
					else
					{
						item.Cells["NetWeight"].Value = 0;
						item.Cells["Rate"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				if (e.Column.Key == "Rate")
				{
					if (Conversion.ToDouble(item.Cells["Rate"].Value) != 0.0 && item.Cells["RateUomId"].Value.ToString() != string.Empty)
					{
						decimal NetWeight = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
						decimal AvgRate = Conversion.ToDecimal(item.Cells["Rate"].Value);
						decimal Amount = ((NetWeight > 0m) ? (NetWeight * AvgRate) : 0m);
						item.Cells["Amount"].Value = Amount;
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
			}
			grdConsupmtionDetail.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumption_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdConsupmtionDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionConsumptionDetail d1 = new ProductionConsumptionDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.BomConsumptionId = Conversion.ToInt(r.Cells["BomConsumptionId"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordConsumption.Add(d1);
					dtConsupmtionDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdConsupmtionDetail.UpdateData();
				}
				else
				{
					r.Delete();
				}
				if (dtConsupmtionDetail.Rows.Count == 0)
				{
					AddRowInConsumptionGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInConsumptionGrid();
			}
			grdConsupmtionDetail.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				updateDetailIndex = r.RowIndex;
				CmbWarehouseName.Value = r.Cells["WareHouseId"].Value.ToString();
				cmbItemBOM.Value = r.Cells["ItemId"].Value.ToString();
				cmbItem_Leave(null, null);
				cmbBOMNo.Value = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
				cmbBOMNo_Leave(null, null);
				cmbUOM.Value = Conversion.ToInt(r.Cells["ItemUomId"].Value);
				txtQty.Text = r.Cells["ItemQty"].Value.ToString();
				BtnAddDetail.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				tabControl2.SelectedIndex = 0;
				CmbWarehouseName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidationOutPutGrid())
			{
				return;
			}
			GridEXRow[] dataRows;
			if (RestrictSingleBomInOutPut)
			{
				dataRows = grdOutPutByProduct.GetDataRows();
				for (int i = 0; i < dataRows.Length; i++)
				{
					int BomHeaderId2 = Conversion.ToInt(dataRows[i].Cells["BomHeaderId"].Value);
					if (BomHeaderId2 > 0 && Conversion.ToInt(cmbBOMNo.Value) != BomHeaderId2)
					{
						MessageBox.Show("You can Only Added Single BOM in Output");
						return;
					}
				}
			}
			dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				int BomHeaderId = Conversion.ToInt(obj.Cells["BomHeaderId"].Value);
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int WareHouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(cmbBOMNo.Value) == BomHeaderId && Conversion.ToInt(cmbItemBOM.Value) == ItemId && Conversion.ToInt(CmbWarehouseName.Value) == WareHouseId)
				{
					MessageBox.Show("Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists");
					return;
				}
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 1)
			{
				GridEXRow r = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && (Conversion.ToInt(r.Cells["BomHeaderId"].Value) == 0 || Conversion.ToInt(r.Cells["ItemId"].Value) == 0))
				{
					dtOutPutByProduct.Clear();
					grdOutPutByProduct.ClearStructure();
				}
			}
			int UomId = 0;
			string UomCode = "";
			decimal Uom = default(decimal);
			if (((UltraGridBase)cmbUOM).ActiveRow != null && Conversion.ToInt(cmbUOM.Value) > 0)
			{
				UomId = Conversion.ToInt(cmbUOM.Value);
				Uom = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				UomCode = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
			}
			dtOutPutByProduct.Rows.Add(0, Conversion.ToInt(cmbBOMNo.Value), Conversion.ToString((object)((Control)(object)cmbBOMNo).Text), Conversion.ToInt(CmbWarehouseName.Value), Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text), Conversion.ToInt(cmbItemBOM.Value), Conversion.ToString((object)((Control)(object)cmbItemBOM).Text), UomId, Uom, UomCode, Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtQty.Text) * Uom, 0, UomId, Uom, UomCode, 0, Conversion.ToDecimal((object)txtQty.Text));
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			ResetDetailOutPut();
			tabControl2.SelectedIndex = 0;
			if (grdOutPutByProduct.GetRows().Count() > 0)
			{
				((Control)(object)CmbProductionType).Enabled = false;
			}
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
			if (!FormValidationOutPutGrid())
			{
				return;
			}
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int BomHeaderId = Conversion.ToInt(item.Cells["BomHeaderId"].Value);
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int WareHouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(cmbBOMNo.Value) == BomHeaderId && Conversion.ToInt(cmbItemBOM.Value) == ItemId && Conversion.ToInt(CmbWarehouseName.Value) == WareHouseId && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists");
					return;
				}
			}
			int UomId = 0;
			string UomCode = "";
			decimal Uom = default(decimal);
			if (((UltraGridBase)cmbUOM).ActiveRow != null && Conversion.ToInt(cmbUOM.Value) > 0)
			{
				UomId = Conversion.ToInt(cmbUOM.Value);
				Uom = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
				UomCode = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
			}
			dtOutPutByProduct.Rows[updateDetailIndex]["BomHeaderId"] = Conversion.ToInt(cmbBOMNo.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["BomNo"] = Conversion.ToString((object)((Control)(object)cmbBOMNo).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWarehouseName.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["WarehouseName"] = Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItemBOM.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItemBOM).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemUomId"] = UomId;
			dtOutPutByProduct.Rows[updateDetailIndex]["Uom"] = Uom;
			dtOutPutByProduct.Rows[updateDetailIndex]["UomCode"] = UomCode;
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtQty.Text) * Uom;
			dtOutPutByProduct.Rows[updateDetailIndex]["RateUomId"] = UomId;
			dtOutPutByProduct.Rows[updateDetailIndex]["RateUom"] = Uom;
			dtOutPutByProduct.Rows[updateDetailIndex]["RateUomCode"] = UomCode;
			dtOutPutByProduct.Rows[updateDetailIndex]["BomQty"] = Conversion.ToDecimal((object)txtQty.Text);
			BtnAddDetail.Visible = true;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			ResetDetailOutPut();
			tabControl2.SelectedIndex = 0;
			Calculation();
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
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				dtOutPutByProduct.Rows.Add(0, 0, 0, 0, "", 0, "", 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0);
			}
			else
			{
				int BomHeaderId = 0;
				string BomNo = "";
				int WarehouseId = 0;
				string WarehouseName = "";
				int ItemId = 0;
				string ItemName = "";
				int ItemUomId = 0;
				decimal Uom = default(decimal);
				string UomCode = "";
				decimal ItemQty = default(decimal);
				decimal ConsumptionQty = default(decimal);
				decimal BomQty = default(decimal);
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
								BomNo = Conversion.ToString(row.Cells["BomNo"].Value);
								ItemName = Conversion.ToString(row.Cells["ItemName"].Value);
								WarehouseId = Conversion.ToInt(row.Cells["WarehouseId"].Value);
								WarehouseName = Conversion.ToString(row.Cells["WarehouseName"].Value);
								ItemUomId = Conversion.ToInt(row.Cells["ItemUomId"].Value);
								Uom = Conversion.ToDecimal(row.Cells["Uom"].Value);
								UomCode = Conversion.ToString(row.Cells["UomCode"].Value);
								ConsumptionQty = Conversion.ToDecimal(row.Cells["ConsumptionQty"].Value);
								ItemQty = Conversion.ToDecimal(row.Cells["ItemQty"].Value);
								BomQty = Conversion.ToDecimal(row.Cells["BomQty"].Value);
								NetWeight = Conversion.ToDecimal(row.Cells["NetWeight"].Value);
								Rate = Conversion.ToDecimal(row.Cells["Rate"].Value);
								Conversion.ToInt(row.Cells["RateUomId"].Value);
								break;
							}
						}
					}
					else
					{
						BomNo = Conversion.ToString(r.Cells["BomNo"].Value);
						ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
						WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						WarehouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
						ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						Uom = Conversion.ToDecimal(r.Cells["Uom"].Value);
						UomCode = Conversion.ToString(r.Cells["UomCode"].Value);
						ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						ConsumptionQty = Conversion.ToDecimal(r.Cells["ConsumptionQty"].Value);
						BomQty = Conversion.ToDecimal(r.Cells["BomQty"].Value);
						NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						Conversion.ToInt(r.Cells["RateUomId"].Value);
					}
				}
				dtOutPutByProduct.Rows.Add(0, BomHeaderId, BomNo, WarehouseId, WarehouseName, ItemId, ItemName, ItemUomId, Uom, UomCode, ConsumptionQty, ItemQty, NetWeight, Rate, ItemUomId, Uom, UomCode, 0, BomQty);
			}
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OutPutGridSetWidth(GridEX grdOutPut)
	{
		grdOutPut.RootTable.Columns["UomCode"].Caption = "Uom";
		grdOutPut.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
		grdOutPut.RootTable.Columns["ItemQty"].Caption = (RestrictSingleBomInOutPut ? "Actual OutPut Qty" : "Qty");
		grdOutPut.RootTable.Columns["BomNo"].Width = 90;
		grdOutPut.RootTable.Columns["WarehouseName"].Width = 90;
		grdOutPut.RootTable.Columns["ItemName"].Width = 120;
		grdOutPut.RootTable.Columns["Uom"].Width = 65;
		grdOutPut.RootTable.Columns["UomCode"].Width = 65;
		grdOutPut.RootTable.Columns["ConsumptionQty"].Width = 90;
		grdOutPut.RootTable.Columns["ItemQty"].Width = 90;
		grdOutPut.RootTable.Columns["NetWeight"].Width = 80;
		grdOutPut.RootTable.Columns["Rate"].Width = 90;
		grdOutPut.RootTable.Columns["RateUom"].Width = 65;
		grdOutPut.RootTable.Columns["RateUomCode"].Width = 75;
		grdOutPut.RootTable.Columns["Amount"].Width = 100;
	}

	private void grdOutPutGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "RateUomId", "ItemUomId", "WarehouseId", "UomCode", "RateUom", "RateUomCode", "ItemId", "WareHouseId", "BomHeaderId",
				"Uom", "BomQty", "BomNo"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"ItemQty",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdOutPutByProduct, 2, 4, hiddenColumns, editableColumns);
			grdOutPutByProduct.RootTable.Columns["ConsumptionQty"].Visible = RestrictSingleBomInOutPut;
			grdOutPutByProduct.RootTable.Columns["ConsumptionQty"].EditType = (EditType)(RestrictSingleBomInOutPut ? 1 : 0);
			grdOutPutByProduct.GroupByBoxVisible = true;
			grdOutPutByProduct.RootTable.GroupTotals = (GroupTotals)2;
			OutPutGridSetWidth(grdOutPutByProduct);
			GridEX_Helper.AddButton(grdOutPutByProduct, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdOutPutByProduct, "Add", "+", 20, 1);
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
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "ItemQty")
			{
				if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal Uom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal RateUom = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal NetWeight = num * Uom;
					item.Cells["NetWeight"].Value = NetWeight;
					decimal CurrentRowRate = Conversion.ToDecimal(item.Cells["Rate"].Value);
					decimal CurrentRowAmount = ((RateUom > 0m) ? (CurrentRowRate * (NetWeight / RateUom)) : 0m);
					item.Cells["Amount"].Value = CurrentRowAmount;
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
					item.Cells["Amount"].Value = 0;
				}
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
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
					ProductionOutPutDetail d1 = new ProductionOutPutDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = 0;
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = 0m;
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = 0;
					d1.Amount = 0m;
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
					grdOutPutByProduct.UpdateData();
				}
				else
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
					((Control)(object)CmbProductionType).Enabled = true;
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOutPutByProductGrid();
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
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

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadGridSetting()
	{
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Visible = false;
			grdOverHead.RootTable.Columns["BomOverHeadId"].Visible = false;
			grdOverHead.RootTable.Columns["BomHeaderId"].Visible = false;
			grdOverHead.RootTable.Columns["BomNo"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomNo"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["BomAmount"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
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
			grdOverHead.RootTable.Columns["BomAmount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["BomAmount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["BomAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHead.RootTable.Columns["BomAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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
			grdOverHead.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOverHead.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdOverHead.RootTable.Columns["BomNo"].Width = 80;
			grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 180;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Width = 80;
			grdOverHead.RootTable.Columns["BomQty"].Width = 80;
			grdOverHead.RootTable.Columns["Qty"].Width = 80;
			grdOverHead.RootTable.Columns["Rate"].Width = 80;
			grdOverHead.RootTable.Columns["BomAmount"].Width = 95;
			grdOverHead.RootTable.Columns["Amount"].Width = 95;
			grdOverHead.RootTable.Columns["OhRemarks"].Width = 200;
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 30;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 30;
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
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
			if (e.Column.Key == "PercentOfTotalInput" && item != null && (int)item.RowType == 0)
			{
				double TotalPercentage = 0.0;
				GridEXRow[] dataRows = grdOverHead.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
				}
				if (item.Cells["PercentOfTotalInput"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
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
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdOverHead.UpdateData();
			Calculation();
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
			((Control)(object)CmbWarehouseName).Text = "";
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
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			RecId = 0;
			DocumentNoFill();
			((Control)(object)cmbBOMNo).Text = string.Empty;
			((Control)(object)cmbBOMNo).Text = string.Empty;
			((Control)(object)cmbItemBOM).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)CmbProductionType).Enabled = true;
			dtInput.Rows.Clear();
			grdInputMaterial.ClearStructure();
			dtOverHead.Rows.Clear();
			grdOverHead.ClearStructure();
			dtConsupmtionDetail.Rows.Clear();
			grdConsupmtionDetail.ClearStructure();
			lstRemoveRecordInput.Clear();
			lstRemoveRecordOutPut.Clear();
			dtOutPutByProduct.Rows.Clear();
			grdOutPutByProduct.ClearStructure();
			btnsave.Visible = true;
			BtnSaveAs.Visible = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			btnRecordsUpdate.Visible = AdminRole;
			btnReProcessing.Visible = AdminRole;
			datFromDate.Visible = AdminRole;
			datTodate.Visible = AdminRole;
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			AddRowInConsumptionGrid();
			grdConsupmtionDetail.RetrieveStructure();
			grdConsumptionGridSetting();
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			txtTotalInputs.Text = string.Empty;
			txtOverHead.Text = string.Empty;
			txtWagesAmount.Text = string.Empty;
			txtTotalOutPutAutoRatedAmount.Text = string.Empty;
			txtOutPutAutoRatePerQtyAmount.Text = string.Empty;
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation r) => r.ConfigDescription == "ProductionPackingMaterialRequired").FirstOrDefault();
			ProductionPackingMaterialRequired = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ShiftComboFill();
			ItemFillFromBom();
			BOMNoAndItemFill();
			dtFillsForGrids();
			uomgetAll();
			PlantFeader();
			WarehouseFill();
			ProductionTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionFeed_FormClosed(object sender, FormClosedEventArgs e)
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
			decimal sumoftotalInputs = ((grdInputMaterial.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0m);
			decimal sumofOverhead = ((grdOverHead.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0m);
			decimal sumofConsumption = ((grdConsupmtionDetail.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdConsupmtionDetail.GetTotal(grdConsupmtionDetail.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0m);
			decimal OutPutAutoRateItemQty = ((grdOutPutByProduct.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["ItemQty"], (AggregateFunction)2)) : 0m);
			decimal NetValue = sumoftotalInputs + sumofOverhead + sumofConsumption;
			txtTotalInputs.Text = Math.Round(sumoftotalInputs, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtOverHead.Text = Math.Round(sumofOverhead, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtWagesAmount.Text = Math.Round(sumofConsumption, 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalOutPutAutoRatedAmount.Text = NetValue.ToString(clsGlobalVariables.stringFormatsingle);
			if (OutPutAutoRateItemQty > 0m)
			{
				txtOutPutAutoRatePerQtyAmount.Text = Math.Round(NetValue / OutPutAutoRateItemQty, 2).ToString(clsGlobalVariables.stringFormatsingle);
			}
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
			if (RestrictSingleBomInOutPut && (from row in grdOutPutByProduct.GetDataRows()
				select row.Cells["BomHeaderId"].Value into val
				where val != null
				select val).Distinct().Count() > 1)
			{
				throw new Exception("OutPut have More than One BOM");
			}
			Dictionary<string, GridEXRow> uniqueKeys = new Dictionary<string, GridEXRow>();
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int bomHeaderId = Conversion.ToInt(item.Cells["BomHeaderId"].Value);
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int ItemUomId = Conversion.ToInt(item.Cells["ItemUomId"].Value);
				int WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				string key = $"{bomHeaderId}-{itemId}-{ItemUomId}-{WarehouseId}";
				if (uniqueKeys.TryGetValue(key, out var existingRow))
				{
					string ItemName = Conversion.ToString((object)item.Cells["ItemName"].Text);
					string ItemUom = Conversion.ToString((object)item.Cells["UomCode"].Text);
					string BomNo = Conversion.ToString((object)item.Cells["BomNo"].Text);
					string WarehouseName = Conversion.ToString((object)item.Cells["WarehouseName"].Text);
					throw new Exception("Duplicate Record not added to the grid.\n" + $"Item {ItemName} with BOM No {BomNo} , Uom {ItemUom} and Warehouse {WarehouseName} exists in row No {item.RowIndex + 1} and Row No {existingRow.RowIndex + 1}");
				}
				uniqueKeys[key] = item;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void CheckBomHeaderIds(DataTable dtOutput, DataTable dtInput, string OutputCaption, string InputCaption, bool ValidateOnlyOutput)
	{
		try
		{
			HashSet<int> outputIds = (from row in dtOutput.AsEnumerable()
				where row.Field<int>("BomHeaderId") > 0
				select row.Field<int>("BomHeaderId")).ToHashSet();
			HashSet<int> inputIds = (from row in dtInput.AsEnumerable()
				where row.Field<int>("BomHeaderId") > 0
				select row.Field<int>("BomHeaderId")).ToHashSet();
			List<int> missingInOutput = inputIds.Except(outputIds).ToList();
			List<int> missingInInput = outputIds.Except(inputIds).ToList();
			if (missingInOutput.Any())
			{
				IEnumerable<string> missingBomNos = (from row in dtInput.AsEnumerable()
					where missingInOutput.Contains(row.Field<int>("BomHeaderId"))
					select row.Field<string>("BomNo")).ToList().Distinct();
				throw new Exception("BomHeaderNo " + string.Join(", ", missingBomNos) + " found in " + InputCaption + " Grid but not in " + OutputCaption + " Grid.");
			}
			if (missingInInput.Any() && !ValidateOnlyOutput)
			{
				IEnumerable<string> missingBomNos2 = (from row in dtOutput.AsEnumerable()
					where missingInInput.Contains(row.Field<int>("BomHeaderId"))
					select row.Field<string>("BomNo")).ToList().Distinct();
				throw new Exception("BomHeaderNo " + string.Join(", ", missingBomNos2) + " found in " + OutputCaption + " Grid but not in " + InputCaption + " Grid.");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Expected O, but got Unknown
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Expected O, but got Unknown
		//IL_1621: Unknown result type (might be due to invalid IL or missing references)
		//IL_1628: Expected O, but got Unknown
		//IL_1d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d44: Expected O, but got Unknown
		//IL_1de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dee: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInputMaterial.GetDataRows().Count() == 0 || grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");
				return;
			}
			ProductionHeader ProductionHeader = new ProductionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ProductionHeader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			IsDuplicateRowExists();
			decimal d5 = Conversion.ToDecimal((object)grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			if (!decimal.TryParse(txtTotalOutPutAutoRatedAmount.Text, out var TotalOutPutAutoRatedAmount))
			{
				throw new Exception("Invalid input for Total Output Auto Rated Amount.");
			}
			TotalOutPutAutoRatedAmount = Math.Round(TotalOutPutAutoRatedAmount, 0);
			decimal roundedSumOfTotalOutput = Math.Round(d5, 0, MidpointRounding.AwayFromZero);
			if (roundedSumOfTotalOutput - TotalOutPutAutoRatedAmount > 1m)
			{
				throw new Exception($"Net Value {TotalOutPutAutoRatedAmount} in Header and Total Output Amount {roundedSumOfTotalOutput} are not equal");
			}
			ProductionHeader.CompanyId = UserAccount.CompanyId;
			ProductionHeader.BranchesId = UserAccount.BranchesId;
			ProductionHeader.ProjectsId = UserAccount.BranchesId;
			ProductionHeader.OrganizationId = UserAccount.OrganizationId;
			ProductionHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ProductionHeader.DocumentTypeId = 1122;
			ProductionHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			ProductionHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ProductionHeader.ShiftId = Conversion.ToInt(CmbShift.Value);
			ProductionHeader.WorkOrderId = 0;
			ProductionHeader.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			ProductionHeader.PlantId = Conversion.ToInt(CmbPlantName.Value);
			ProductionHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ProductionHeader.WIPWarehouseId = 0;
			ProductionHeader.ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			ProductionHeader.WIPGLAccountID = 0;
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
			ProductionHeader.ProductionConsumptionDetailList = new List<ProductionConsumptionDetail>();
			ProductionHeader.AttachmentsList = new List<DMSAttachments>();
			int LineId = 0;
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				ProductionInputDetail d3 = new ProductionInputDetail();
				d3.Id = ((RecId > 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				if (d3.Id > 0)
				{
					d3.ActionTypeId = 2;
				}
				else
				{
					d3.ActionTypeId = 1;
				}
				d3.BomInputId = Conversion.ToInt(r2.Cells["BomInputId"].Value);
				d3.BomHeaderId = Conversion.ToInt(r2.Cells["BomHeaderId"].Value);
				if (r2.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r2.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.WareHouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
				if (r2.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (r2.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r2.Cells["ItemUomId"].Value) == 0)
				{
					MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				DataTable dt3 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
				if (dt3.Rows.Count > 0)
				{
					DataRow[] dr3 = dt3.Select("Id='" + Conversion.ToInt(r2.Cells["ItemUomId"].Value) + "'");
					if (dr3.Length == 0)
					{
						MessageBox.Show("UOM in Input Grid Row No: " + (LineId + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.ItemUomId = Conversion.ToInt(dr3[0].ItemArray[0]);
				}
				if (r2.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.ItemQty = Conversion.ToDecimal((object)r2.Cells["ItemQty"].Text);
				if (r2.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["NetWeight"].Value) == 0m)
				{
					MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.NetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
				if (r2.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Rate"].Value) == 0m)
				{
					MessageBox.Show("Rate Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
				if (r2.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r2.Cells["RateUomId"].Value) == 0)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				DataTable dt4 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
				if (dt4.Rows.Count > 0)
				{
					DataRow[] dr4 = dt4.Select("Id='" + Conversion.ToInt(r2.Cells["RateUomId"].Value) + "'");
					if (dr4.Length == 0)
					{
						MessageBox.Show("RateUom in Input Grid Row No: " + (LineId + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.RateUomId = Conversion.ToInt(dr4[0].ItemArray[0]);
				}
				if (r2.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Amount"].Value) == 0m)
				{
					MessageBox.Show("Amount Field Required in Grid Input Row No: " + (LineId + 1));
					return;
				}
				d3.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
				d3.PrctOfTotalQty = Conversion.ToDecimal(r2.Cells["PrctOfTotalQty"].Value);
				ProductionHeader.ProductionInputDetailList.Add(d3);
				LineId++;
			}
			if (lstRemoveRecordInput.Count > 0 && RecId > 0)
			{
				for (int l = 0; l < lstRemoveRecordInput.Count; l++)
				{
					ProductionHeader.ProductionInputDetailList.Add(lstRemoveRecordInput[l]);
				}
			}
			dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow r5 in dataRows)
			{
				if (Conversion.ToInt((object)r5.Cells["ItemId"].Value.ToString()) != 0)
				{
					ProductionOutPutDetail d4 = new ProductionOutPutDetail();
					d4.Id = ((RecId > 0) ? Conversion.ToInt(r5.Cells["Id"].Value) : 0);
					if (d4.Id > 0)
					{
						d4.ActionTypeId = 2;
					}
					else
					{
						d4.ActionTypeId = 1;
					}
					d4.EntryTypeId = 1;
					d4.BOMHeaderId = Conversion.ToInt(r5.Cells["BomHeaderId"].Value);
					if (ProductionHeader.ProductionTypeId != 4 && ProductionHeader.ProductionTypeId != 5)
					{
						if (r5.Cells["BomHeaderId"].Text == string.Empty || Conversion.ToInt(r5.Cells["BomHeaderId"].Value) == 0)
						{
							MessageBox.Show("BomHeader Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
							return;
						}
						d4.BOMHeaderId = Conversion.ToInt(r5.Cells["BomHeaderId"].Value);
					}
					if (r5.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r5.Cells["WarehouseId"].Value) == 0)
					{
						MessageBox.Show("Warehouse Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.WareHouseId = Conversion.ToInt(r5.Cells["WarehouseId"].Value);
					if (r5.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					if (r5.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r5.Cells["ItemQty"].Value) == 0m)
					{
						MessageBox.Show("ItemQty Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.ItemQty = Conversion.ToDecimal((object)r5.Cells["ItemQty"].Text);
					if (RestrictSingleBomInOutPut)
					{
						if (r5.Cells["ConsumptionQty"].Text == string.Empty || Conversion.ToDecimal(r5.Cells["ConsumptionQty"].Value) == 0m)
						{
							MessageBox.Show("ConsumptionQty Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
							return;
						}
						d4.ConsumptionQty = Conversion.ToDecimal((object)r5.Cells["ConsumptionQty"].Text);
					}
					else
					{
						d4.ConsumptionQty = Conversion.ToDecimal((object)r5.Cells["ItemQty"].Text);
					}
					if (r5.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r5.Cells["Rate"].Value) == 0m)
					{
						MessageBox.Show("Rate Field Required in Grid Output Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.Rate = Conversion.ToDecimal(r5.Cells["Rate"].Value);
					if (r5.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r5.Cells["Rate"].Value) == 0m)
					{
						MessageBox.Show("AmountField Required in Grid Output Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.Amount = Conversion.ToDecimal(r5.Cells["Amount"].Value);
					d4.NetWeight = d4.ItemQty;
					if (r5.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r5.Cells["ItemUomId"].Value) == 0)
					{
						MessageBox.Show("UOM  Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
						return;
					}
					DataTable dt5 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r5.Cells["ItemId"].Value));
					if (dt5.Rows.Count > 0)
					{
						DataRow[] dr5 = dt5.Select("Id='" + Conversion.ToInt(r5.Cells["ItemUomId"].Value) + "'");
						if (dr5.Length == 0)
						{
							MessageBox.Show("UOM in Output Grid Row No: " + (r5.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r5.Cells["ItemId"].Text) + ". Please Check...");
							return;
						}
						d4.ItemUomId = Conversion.ToInt(dr5[0].ItemArray[0]);
					}
					if (r5.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r5.Cells["NetWeight"].Value) == 0m)
					{
						MessageBox.Show("NetWeight Field Required in Grid OutPut Row No: " + (r5.RowIndex + 1));
						return;
					}
					d4.NetWeight = Conversion.ToDecimal(r5.Cells["NetWeight"].Value);
					if (r5.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r5.Cells["RateUomId"].Value) == 0)
					{
						MessageBox.Show("Rate UOM Field Required in Grid Output Row No: " + (r5.RowIndex + 1));
						return;
					}
					DataTable dt6 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r5.Cells["ItemId"].Value));
					if (dt6.Rows.Count > 0)
					{
						DataRow[] dr6 = dt6.Select("Id='" + Conversion.ToInt(r5.Cells["RateUomId"].Value) + "'");
						if (dr6.Length == 0)
						{
							MessageBox.Show("RateUom in output Grid Row No: " + (r5.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r5.Cells["ItemId"].Text) + ". Please Check...");
							return;
						}
						d4.RateUomId = Conversion.ToInt(dr6[0].ItemArray[0]);
					}
					ProductionHeader.ProductionOutPutDetailList.Add(d4);
					continue;
				}
				MessageBox.Show("Item Field Required in Grid output Row No: " + (r5.RowIndex + 1));
				return;
			}
			if (lstRemoveRecordOutPut.Count > 0)
			{
				for (int k = 0; k < lstRemoveRecordOutPut.Count; k++)
				{
					ProductionHeader.ProductionOutPutDetailList.Add(lstRemoveRecordOutPut[k]);
				}
			}
			dataRows = grdOverHead.GetDataRows();
			foreach (GridEXRow r4 in dataRows)
			{
				if (Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value) > 0)
				{
					ProductionOverHeadsDetail d = new ProductionOverHeadsDetail();
					if (r4.Cells["OverHeadAccountId"].Text == string.Empty || Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value) == 0)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r4.RowIndex + 1));
						return;
					}
					d.Id = ((RecId > 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
					d.BomOverHeadId = Conversion.ToInt(r4.Cells["BomOverHeadId"].Value);
					d.OverHeadTypeId = Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value);
					d.OverHeadAccountId = Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value);
					if (r4.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r4.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r4.RowIndex + 1));
						return;
					}
					d.PercentOfTotalInput = Conversion.ToDecimal((object)r4.Cells["PercentOfTotalInput"].Text);
					d.Qty = Conversion.ToDecimal((object)r4.Cells["Qty"].Text);
					d.BomQty = Conversion.ToDecimal((object)r4.Cells["BomQty"].Text);
					d.Rate = Conversion.ToDecimal((object)r4.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r4.Cells["Amount"].Text);
					d.OhRemarks = Conversion.ToString((object)r4.Cells["OhRemarks"].Text);
					d.BomHeaderId = Conversion.ToInt(r4.Cells["BomHeaderId"].Value);
					ProductionHeader.ProductionOverHeadsDetailList.Add(d);
				}
			}
			if (ProductionPackingMaterialRequired)
			{
				DataTable dtConsumption = (DataTable)grdConsupmtionDetail.DataSource;
				if (dtConsumption == null || dtConsumption.Rows.Count <= 0)
				{
					throw new Exception("Consumption grid don't have entry of Packing Material. Please Check!");
				}
				if (!dtConsumption.AsEnumerable().Any((DataRow r) => r.Field<string>("ParentCategory") == "Packing Material"))
				{
					tabControl2.SelectedIndex = 1;
					throw new Exception("Consumption grid don't have entry of Packing Material. Please Check!");
				}
			}
			dataRows = grdConsupmtionDetail.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				ProductionConsumptionDetail d2 = new ProductionConsumptionDetail();
				d2.Id = ((RecId > 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				if (d2.Id > 0)
				{
					d2.ActionTypeId = 2;
				}
				else
				{
					d2.ActionTypeId = 1;
				}
				d2.BomConsumptionId = Conversion.ToInt(r3.Cells["BomConsumptionId"].Value);
				d2.BomHeaderId = Conversion.ToInt(r3.Cells["BomHeaderId"].Value);
				if (r3.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r3.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.WareHouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
				if (r3.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r3.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				if (r3.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r3.Cells["ItemUomId"].Value) == 0)
				{
					MessageBox.Show("UOM  Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt2.Rows.Count > 0)
				{
					DataRow[] dr2 = dt2.Select("Id='" + Conversion.ToInt(r3.Cells["ItemUomId"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("UOM in Consumption Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d2.ItemUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
				if (r3.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.ItemQty = Conversion.ToDecimal((object)r3.Cells["ItemQty"].Text);
				if (r3.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["NetWeight"].Value) == 0m)
				{
					MessageBox.Show("NetWeight Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.NetWeight = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				if (r3.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["Rate"].Value) == 0m)
				{
					MessageBox.Show("Rate Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.Rate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
				if (r3.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r3.Cells["RateUomId"].Value) == 0)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Id='" + Conversion.ToInt(r3.Cells["RateUomId"].Value) + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("RateUom in Consumption Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d2.RateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				if (r3.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["Amount"].Value) == 0m)
				{
					MessageBox.Show("AmountField Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return;
				}
				d2.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
				d2.OutPutItemId = Conversion.ToInt(r3.Cells["OutPutItemId"].Value);
				ProductionHeader.ProductionConsumptionDetailList.Add(d2);
			}
			if (lstRemoveRecordConsumption.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecordConsumption.Count; j++)
				{
					ProductionHeader.ProductionConsumptionDetailList.Add(lstRemoveRecordConsumption[j]);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.FileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				ProductionHeader.AttachmentsList.Add(attach);
			}
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				ProductionHeader.DeleteAttachmentsList = new List<DMSAttachments>();
				for (int m = 0; m < AT.RemovedAttachmentListInUpdateCase.Count; m++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[m].Path);
					delete.ScreenName = base.Name;
					ProductionHeader.DeleteAttachmentsList.Add(delete);
				}
			}
			int success = ProductionHeader.Save(ProductionHeader);
			ProductionHeader.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ProductionHeader.DocNo);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ProductionHeader.DocNo);
			}
			FromReset();
			if (ChkPreviewSlip.Checked && formright.DoHavePrintRights)
			{
				CommonServices.ProductionManualSlipFeedMill(ProductionHeader.Id);
			}
			if (chkPrintVoucher.Checked && formright.DoHavePrintRights)
			{
				VoucherReprot(ProductionHeader.Id);
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

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			FromReset();
			dtInput.Clear();
			grdInputMaterial.ClearStructure();
			dtOutPutByProduct.Clear();
			grdOutPutByProduct.ClearStructure();
			dtOverHead.Clear();
			grdOverHead.ClearStructure();
			dtConsupmtionDetail.Clear();
			grdConsupmtionDetail.ClearStructure();
			ProductionHeader ProductionHeader = new ProductionHeader();
			RecId = Id;
			ProductionHeader = ProductionHeader.ReadById(RecId);
			if (ProductionHeader == null)
			{
				return;
			}
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = ProductionHeader.DocDate;
			txtdocno.Text = Conversion.ToString((object)ProductionHeader.DocNo);
			if (ProductionHeader.ProductionTypeId > 0)
			{
				CmbProductionType.Value = ProductionHeader.ProductionTypeId;
				((Control)(object)CmbProductionType).Enabled = false;
			}
			CmbProductionType_Leave(null, null);
			if (Conversion.ToInt((object)ProductionHeader.ShiftId) > 0)
			{
				CmbShift.Value = ProductionHeader.ShiftId;
			}
			CmbPlantName.Value = ProductionHeader.PlantId;
			txtProductionNo.Text = ProductionHeader.ProductionNo;
			txtRemarksHeader.Text = Conversion.ToString((object)ProductionHeader.RemarksHeader);
			if (ProductionHeader.ProductionOutPutDetailList.Count > 0)
			{
				for (int l = 0; l < ProductionHeader.ProductionOutPutDetailList.Count; l++)
				{
					dtOutPutByProduct.Rows.Add(ProductionHeader.ProductionOutPutDetailList[l].Id, ProductionHeader.ProductionOutPutDetailList[l].BOMHeaderId, ProductionHeader.ProductionOutPutDetailList[l].BOMHeaderNo, ProductionHeader.ProductionOutPutDetailList[l].WareHouseId, ProductionHeader.ProductionOutPutDetailList[l].WareHouseName, ProductionHeader.ProductionOutPutDetailList[l].ItemId, ProductionHeader.ProductionOutPutDetailList[l].ItemName, ProductionHeader.ProductionOutPutDetailList[l].ItemUomId, ProductionHeader.ProductionOutPutDetailList[l].ItemUomEquivalent, ProductionHeader.ProductionOutPutDetailList[l].ItemUomCode, ProductionHeader.ProductionOutPutDetailList[l].ConsumptionQty, ProductionHeader.ProductionOutPutDetailList[l].ItemQty, ProductionHeader.ProductionOutPutDetailList[l].NetWeight, ProductionHeader.ProductionOutPutDetailList[l].Rate, ProductionHeader.ProductionOutPutDetailList[l].RateUomId, ProductionHeader.ProductionOutPutDetailList[l].RateUomEquivalent, ProductionHeader.ProductionOutPutDetailList[l].RateUomCode, ProductionHeader.ProductionOutPutDetailList[l].Amount, ProductionHeader.ProductionOutPutDetailList[l].BomQty);
				}
			}
			else
			{
				AddRowInOutPutByProductGrid();
			}
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				for (int k = 0; k < ProductionHeader.ProductionInputDetailList.Count; k++)
				{
					dtInput.Rows.Add(ProductionHeader.ProductionInputDetailList[k].Id, ProductionHeader.ProductionInputDetailList[k].BomHeaderId, ProductionHeader.ProductionInputDetailList[k].BomHeaderNo, ProductionHeader.ProductionInputDetailList[k].BomInputId, ProductionHeader.ProductionInputDetailList[k].WareHouseId, ProductionHeader.ProductionInputDetailList[k].WareHouseName, ProductionHeader.ProductionInputDetailList[k].ItemId, ProductionHeader.ProductionInputDetailList[k].ItemName, ProductionHeader.ProductionInputDetailList[k].ItemUomId, ProductionHeader.ProductionInputDetailList[k].ItemUomEquivalent, ProductionHeader.ProductionInputDetailList[k].ItemUomCode, ProductionHeader.ProductionInputDetailList[k].ItemQty, ProductionHeader.ProductionInputDetailList[k].BomQty, ProductionHeader.ProductionInputDetailList[k].ItemQty, ProductionHeader.ProductionInputDetailList[k].PrctOfTotalQty, ProductionHeader.ProductionInputDetailList[k].NetWeight, ProductionHeader.ProductionInputDetailList[k].BomRate, ProductionHeader.ProductionInputDetailList[k].Rate, ProductionHeader.ProductionInputDetailList[k].RateUomId, ProductionHeader.ProductionInputDetailList[k].RateUomEquivalent, ProductionHeader.ProductionInputDetailList[k].RateUomCode, ProductionHeader.ProductionInputDetailList[k].Amount, ProductionHeader.ProductionInputDetailList[k].BomMinQty, ProductionHeader.ProductionInputDetailList[k].BomMaxQty);
				}
			}
			else
			{
				grdInputMaterial.ClearStructure();
			}
			if (ProductionHeader.ProductionOverHeadsDetailList.Count > 0)
			{
				for (int j = 0; j < ProductionHeader.ProductionOverHeadsDetailList.Count; j++)
				{
					dtOverHead.Rows.Add(ProductionHeader.ProductionOverHeadsDetailList[j].Id, ProductionHeader.ProductionOverHeadsDetailList[j].BomHeaderId, ProductionHeader.ProductionOverHeadsDetailList[j].BomHeaderNo, ProductionHeader.ProductionOverHeadsDetailList[j].BomOverHeadId, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadTypeId, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadAccountId, ProductionHeader.ProductionOverHeadsDetailList[j].PercentOfTotalInput, ProductionHeader.ProductionOverHeadsDetailList[j].BomQty, ProductionHeader.ProductionOverHeadsDetailList[j].Qty, ProductionHeader.ProductionOverHeadsDetailList[j].Rate, ProductionHeader.ProductionOverHeadsDetailList[j].BomAmount, ProductionHeader.ProductionOverHeadsDetailList[j].Amount, ProductionHeader.ProductionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			if (ProductionHeader.ProductionConsumptionDetailList.Count > 0)
			{
				for (int i = 0; i < ProductionHeader.ProductionConsumptionDetailList.Count; i++)
				{
					dtConsupmtionDetail.Rows.Add(ProductionHeader.ProductionConsumptionDetailList[i].Id, ProductionHeader.ProductionConsumptionDetailList[i].BomHeaderId, ProductionHeader.ProductionConsumptionDetailList[i].BomHeaderNo, ProductionHeader.ProductionConsumptionDetailList[i].BomConsumptionId, ProductionHeader.ProductionConsumptionDetailList[i].WareHouseId, ProductionHeader.ProductionConsumptionDetailList[i].WareHouseName, ProductionHeader.ProductionConsumptionDetailList[i].ItemId, ProductionHeader.ProductionConsumptionDetailList[i].ItemName, ProductionHeader.ProductionConsumptionDetailList[i].ParentCategory, ProductionHeader.ProductionConsumptionDetailList[i].ItemUomId, ProductionHeader.ProductionConsumptionDetailList[i].ItemUomEquivalent, ProductionHeader.ProductionConsumptionDetailList[i].ItemUomCode, ProductionHeader.ProductionConsumptionDetailList[i].ItemQty, ProductionHeader.ProductionConsumptionDetailList[i].BomQty, ProductionHeader.ProductionConsumptionDetailList[i].ItemQty, ProductionHeader.ProductionConsumptionDetailList[i].NetWeight, ProductionHeader.ProductionConsumptionDetailList[i].BomRate, ProductionHeader.ProductionConsumptionDetailList[i].Rate, ProductionHeader.ProductionConsumptionDetailList[i].RateUomId, ProductionHeader.ProductionConsumptionDetailList[i].RateUomEquivalent, ProductionHeader.ProductionConsumptionDetailList[i].RateUomCode, ProductionHeader.ProductionConsumptionDetailList[i].Amount, ProductionHeader.ProductionConsumptionDetailList[i].OutPutItemId);
				}
			}
			else
			{
				AddRowInConsumptionGrid();
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			grdConsupmtionDetail.DataSource = dtConsupmtionDetail;
			grdConsupmtionDetail.RetrieveStructure();
			grdConsumptionGridSetting();
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.FileCustomName;
				TA.ActionId = 2;
				AT.lst.Add(TA);
			}
			Calculation();
			txtdocdate.Focus();
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
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProductionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
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

	private void BtnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
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

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyBOM_KeyPress(object sender, KeyPressEventArgs e)
	{
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProductionManualSlipFeedMill(RecId);
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1122)), 1122);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		try
		{
			int Idtype = Conversion.ToInt(cmbProductionNoHistory.Value);
			int PTypeId = Conversion.ToInt(CmbProductionTypeHistory.Value);
			int shiftId = Conversion.ToInt(CmbShiftNameHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ApprovedFilter = "All"
			};
			DataTable dtShift = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"Name"
				}
			};
			DataTable dtcombo = ProductionHeader.GetDataForDropDownFromProductionHeader(val);
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtProductionNo = new DataTable();
				dtProductionNo.Columns.Add("Id", typeof(int));
				dtProductionNo.Columns.Add("ProductionNo");
				DataTable dtProductionType = new DataTable();
				dtProductionType.Columns.Add("Id", typeof(int));
				dtProductionType.Columns.Add("Type");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionNo")
					{
						dtProductionNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionType")
					{
						dtProductionType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ShiftName")
					{
						dtShift.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtProductionNo.Rows.Count > 0)
				{
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
				DDL.BindDDL(dtProductionType, CmbProductionTypeHistory, "Id", "Type", "Production Type", ZeroIndex: true);
				((UltraGridBase)CmbProductionTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idtype > 0)
				{
					if (dtProductionType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PTypeId))
					{
						CmbProductionTypeHistory.Value = PTypeId;
					}
					else
					{
						((Control)(object)CmbProductionTypeHistory).Text = string.Empty;
					}
				}
			}
			DDL.BindDDL(dtShift, CmbShiftNameHistory, "Id", "Name", "Shift Name", ZeroIndex: true);
			((UltraGridBase)CmbShiftNameHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (shiftId > 0)
			{
				if (dtShift.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == shiftId))
				{
					CmbShiftNameHistory.Value = shiftId;
				}
				else
				{
					((Control)(object)CmbShiftNameHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				cmbDateTypeHistory.Focus();
				Datetypefill();
			}
			else
			{
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbProductionNoHistory).Text = string.Empty;
			((Control)(object)CmbProductionTypeHistory).Text = string.Empty;
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
			WF.Show(this);
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
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1122;
			if (txtFromdateHistory.Checked)
			{
				header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
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
			header.ProductTypeId = Conversion.ToInt(CmbProductionTypeHistory.Value);
			dtHistory = ProductionHeader.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("ProductionType", typeof(string));
				dtcol.Columns.Add("ShiftName", typeof(string));
				dtcol.Columns.Add("ProductionNo", typeof(string));
				dtcol.Columns.Add("PlantName", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionTypeDescription"], row["ShiftName"], row["ProductionNo"], row["PlantName"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DatagridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DatagridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["DocNo"].Width = 60;
			DatagridHistory.RootTable.Columns["DocDate"].Width = 80;
			DatagridHistory.RootTable.Columns["ProductionType"].Width = 80;
			DatagridHistory.RootTable.Columns["ShiftName"].Width = 100;
			DatagridHistory.RootTable.Columns["ProductionNo"].Width = 75;
			DatagridHistory.RootTable.Columns["PlantName"].Width = 100;
			DatagridHistory.RootTable.Columns["RemarksHeader"].Width = 140;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = 145;
			DatagridHistory.RootTable.Columns["EntryUserName"].Width = 125;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = 145;
			DatagridHistory.RootTable.Columns["ModifyUserName"].Width = 125;
			DatagridHistory.RootTable.Columns["IsApproved"].Width = 80;
			DatagridHistory.RootTable.Columns["ApprovedDate"].Width = 145;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = 85;
			DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 40;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Voucher");
			DatagridHistory.RootTable.Columns["Voucher"].Key = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Width = 60;
			DatagridHistory.RootTable.Columns["Voucher"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 2;
			DatagridHistory.RootTable.Columns.Add("SaveAs");
			DatagridHistory.RootTable.Columns["SaveAs"].Key = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DatagridHistory.RootTable.Columns["SaveAs"].Position = 3;
			DatagridHistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
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
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.ProductionManualSlipFeedMill(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Voucher")
			{
				VoucherReprot(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (!(e.Column.Key == "SaveAs"))
			{
				return;
			}
			if (formright.DoHaveSaveRight)
			{
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
				btnsave.Visible = false;
				btnUpdate.Visible = false;
				btnDelete.Visible = false;
				BtnSaveAs.Visible = true;
				if (grdOutPutByProduct.GetDataRows().Count() > 0)
				{
					GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
				if (grdConsupmtionDetail.GetDataRows().Count() > 0)
				{
					GridEXRow[] dataRows = grdConsupmtionDetail.GetDataRows();
					foreach (GridEXRow obj2 in dataRows)
					{
						obj2.BeginEdit();
						obj2.Cells["Id"].Value = 0;
						obj2.EndEdit();
					}
				}
				if (grdInputMaterial.GetDataRows().Count() > 0)
				{
					GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
					foreach (GridEXRow obj3 in dataRows)
					{
						obj3.BeginEdit();
						obj3.Cells["Id"].Value = 0;
						obj3.EndEdit();
					}
				}
				if (grdOverHead.GetDataRows().Count() > 0)
				{
					GridEXRow[] dataRows = grdOverHead.GetDataRows();
					foreach (GridEXRow obj4 in dataRows)
					{
						obj4.BeginEdit();
						obj4.Cells["Id"].Value = 0;
						obj4.EndEdit();
					}
				}
				return;
			}
			throw new Exception("you don't have save rights...");
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1122);
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
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
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
			grdConsumptionHistory.ClearStructure();
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader = ProductionHeader.ReadById(Id);
			if (ProductionHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			DataTable dtConsumptionHistory = new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("PrctOfTotalQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("BomNo", typeof(string));
			dtOutPutByProductHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ConsumptionQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Id", typeof(int));
			dtOverHeadHistory.Columns.Add("OverHeadType", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOverHeadHistory.Columns.Add("Percentage", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Qty", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Rate", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("OhRemarks", typeof(string));
			dtConsumptionHistory.Columns.Add("Id", typeof(int));
			dtConsumptionHistory.Columns.Add("WarehouseName", typeof(string));
			dtConsumptionHistory.Columns.Add("ItemName", typeof(string));
			dtConsumptionHistory.Columns.Add("Uom", typeof(string));
			dtConsumptionHistory.Columns.Add("ItemQty", typeof(decimal));
			dtConsumptionHistory.Columns.Add("NetWeight", typeof(decimal));
			dtConsumptionHistory.Columns.Add("Rate", typeof(decimal));
			dtConsumptionHistory.Columns.Add("RateUom", typeof(string));
			dtConsumptionHistory.Columns.Add("Amount", typeof(decimal));
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				for (int l = 0; l < ProductionHeader.ProductionInputDetailList.Count; l++)
				{
					dtInputHistory.Rows.Add(ProductionHeader.ProductionInputDetailList[l].Id, ProductionHeader.ProductionInputDetailList[l].WareHouseName, ProductionHeader.ProductionInputDetailList[l].ItemName, ProductionHeader.ProductionInputDetailList[l].ItemUomCode, ProductionHeader.ProductionInputDetailList[l].ItemQty, ProductionHeader.ProductionInputDetailList[l].PrctOfTotalQty, ProductionHeader.ProductionInputDetailList[l].NetWeight, ProductionHeader.ProductionInputDetailList[l].Rate, ProductionHeader.ProductionInputDetailList[l].RateUomCode, ProductionHeader.ProductionInputDetailList[l].Amount);
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
					dtOutPutByProductHistory.Rows.Add(ProductionHeader.ProductionOutPutDetailList[k].Id, ProductionHeader.ProductionOutPutDetailList[k].BOMHeaderNo, ProductionHeader.ProductionOutPutDetailList[k].WareHouseName, ProductionHeader.ProductionOutPutDetailList[k].ItemName, ProductionHeader.ProductionOutPutDetailList[k].ItemUomCode, ProductionHeader.ProductionOutPutDetailList[k].ConsumptionQty, ProductionHeader.ProductionOutPutDetailList[k].ItemQty, ProductionHeader.ProductionOutPutDetailList[k].NetWeight, ProductionHeader.ProductionOutPutDetailList[k].Rate, ProductionHeader.ProductionOutPutDetailList[k].RateUomCode, ProductionHeader.ProductionOutPutDetailList[k].Amount);
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
					dtOverHeadHistory.Rows.Add(ProductionHeader.ProductionOverHeadsDetailList[j].Id, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadTypeId, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadAccountTitle, ProductionHeader.ProductionOverHeadsDetailList[j].PercentOfTotalInput, ProductionHeader.ProductionOverHeadsDetailList[j].Qty, ProductionHeader.ProductionOverHeadsDetailList[j].Rate, ProductionHeader.ProductionOverHeadsDetailList[j].Amount, ProductionHeader.ProductionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			if (ProductionHeader.ProductionConsumptionDetailList.Count > 0)
			{
				for (int i = 0; i < ProductionHeader.ProductionConsumptionDetailList.Count; i++)
				{
					dtConsumptionHistory.Rows.Add(ProductionHeader.ProductionConsumptionDetailList[i].Id, ProductionHeader.ProductionConsumptionDetailList[i].WareHouseName, ProductionHeader.ProductionConsumptionDetailList[i].ItemName, ProductionHeader.ProductionConsumptionDetailList[i].ItemUomCode, ProductionHeader.ProductionConsumptionDetailList[i].ItemQty, ProductionHeader.ProductionConsumptionDetailList[i].NetWeight, ProductionHeader.ProductionConsumptionDetailList[i].Rate, ProductionHeader.ProductionConsumptionDetailList[i].RateUomCode, ProductionHeader.ProductionConsumptionDetailList[i].Amount);
				}
			}
			else
			{
				grdConsumptionHistory.ClearStructure();
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
			grdConsumptionHistory.DataSource = dtConsumptionHistory;
			grdConsumptionHistory.RetrieveStructure();
			grdConsumptionHistorySetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryInput.RootTable.Columns["PrctOfTotalQty"].Caption = "%";
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryInput, 2, 4);
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["Uom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ConsumptionQty"].Visible = RestrictSingleBomInOutPut;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryOutput, 2, 4);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryOverHead, 2, 4);
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdConsumptionHistorySetting()
	{
		try
		{
			grdConsumptionHistory.RootTable.Columns["Id"].Visible = false;
			grdConsumptionHistory.RootTable.Columns["NetWeight"].Visible = false;
			grdConsumptionHistory.RootTable.Columns["RateUom"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdConsumptionHistory, 2, 4);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = AdminRole;
			btnReProcessing.Visible = AdminRole;
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
			if (e.Control && e.KeyCode == Keys.E)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.A)
			{
				btnRecordsUpdate.Visible = true;
				datFromDate.Visible = true;
				datTodate.Visible = true;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.B)
			{
				btnReProcessing.Visible = true;
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
				if (e.Control && e.KeyCode == Keys.F12 && BtnSaveAs.Visible && BtnSaveAs.Enabled)
				{
					BtnSaveAs_Click(null, null);
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
					tabControl2.SelectedIndex = 0;
					((Control)(object)grdOutPutByProduct).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWarehouseName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdOutPutByProduct).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdConsupmtionDetail).Focus();
					}
					else if (((Control)(object)grdConsupmtionDetail).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdOverHead).Focus();
					}
					else if (((Control)(object)grdOverHead).Focused)
					{
						tabControl2.SelectedIndex = 0;
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
				else if (e.Control && e.KeyCode == Keys.Left)
				{
					tabControl3.SelectedIndex = 0;
					((Control)(object)grdHistoryInput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryOutput).Focused)
					{
						tabControl3.SelectedIndex = 1;
						((Control)(object)grdConsumptionHistory).Focus();
					}
					else if (((Control)(object)grdConsumptionHistory).Focused)
					{
						tabControl3.SelectedIndex = 2;
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else if (((Control)(object)grdHistoryOverHead).Focused)
					{
						tabControl3.SelectedIndex = 0;
						((Control)(object)grdHistoryInput).Focus();
					}
					else if (((Control)(object)grdHistoryInput).Focused)
					{
						tabControl3.SelectedIndex = 0;
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
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Warehouse ");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Expected O, but got Unknown
		//IL_1009: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
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
				if (grdInputMaterial.CurrentColumn.Key == "Add")
				{
					AddRowInInputGrid();
				}
				if (grdInputMaterial.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ProductionInputDetail d2 = new ProductionInputDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
						d2.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordInput.Add(d2);
						dtInput.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdInputMaterial.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					_ = dtInput.Rows.Count;
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInInputGrid();
				grdInputMaterial.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionInputDetail d1 = new ProductionInputDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
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
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			if (grdInputMaterial.CurrentColumn != null && (grdInputMaterial.CurrentColumn.Key == "WarehouseId" || grdInputMaterial.CurrentColumn.Key == "WarehouseName"))
			{
				GrdPopUp obj3 = new GrdPopUp(dtWare, "Id", "WarehouseName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				NameCode = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["WarehouseId"].Value = Id;
				r.Cells["WarehouseName"].Value = NameCode;
				DtStock.Clear();
				ReportsParameters objj3 = new ReportsParameters();
				objj3.OrganizationId = UserAccount.CompanyId;
				objj3.CompanyId = UserAccount.OrganizationId;
				objj3.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				objj3.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				objj3.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj3);
				if (DtStock.Rows.Count > 0)
				{
					decimal Stock3 = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
					if (Stock3 > 0m)
					{
						r.Cells["BalanceStock"].Value = Stock3;
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
				}
				else
				{
					r.Cells["BalanceStock"].Value = 0;
				}
				double AvgRateFromDb3 = CommonServices.getAvgRateByItemId(objj3.ItemId, objj3.DocDate, 1122, RecId);
				if (AvgRateFromDb3 > 0.0)
				{
					decimal AvgRate3 = Conversion.ToDecimal((object)AvgRateFromDb3);
					r.Cells["Rate"].Value = AvgRate3;
					decimal NetWeight4 = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					if (NetWeight4 > 0m && AvgRate3 > 0m)
					{
						decimal Amount3 = NetWeight4 * AvgRate3;
						r.Cells["Amount"].Value = Amount3;
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdInputMaterial.CurrentColumn != null && grdInputMaterial.CurrentColumn.Key == "ItemName")
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj2 = new GrdPopUp(dtItem, "Id", "ItemName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = NameCode;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = 0;
					r.Cells["Uom"].Value = 0;
					r.Cells["UomCode"].Value = "";
					r.Cells["RateUom"].Value = 0;
					r.Cells["RateUomId"].Value = 0;
					r.Cells["RateUomCode"].Value = "";
					r.Cells["NetWeight"].Value = 0;
				}
				if (Conversion.ToInt((object)r.Cells["ItemUomId"]) == 0)
				{
					dtUom.Clear();
					bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dtUom.Rows.Count > 0)
					{
						int ItemUomId = 0;
						string UomCode = "";
						decimal Uom2 = default(decimal);
						DataRow firstRow2 = dtUom.AsEnumerable().FirstOrDefault();
						if (firstRow2 != null)
						{
							ItemUomId = Conversion.ToInt(firstRow2["Id"]);
							Uom2 = Conversion.ToDecimal(firstRow2["ItemUOM"]);
							UomCode = Conversion.ToString(firstRow2["ItemUOMCode"]);
						}
						r.Cells["ItemUomId"].Value = ItemUomId;
						r.Cells["UomCode"].Value = UomCode;
						r.Cells["Uom"].Value = Uom2;
						r.Cells["RateUomId"].Value = ItemUomId;
						r.Cells["RateUomCode"].Value = UomCode;
						r.Cells["RateUom"].Value = Uom2;
						decimal NetWeight3 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString()) * Uom2;
						r.Cells["NetWeight"].Value = NetWeight3;
						decimal CurrentRowAmount = Conversion.ToDecimal(r.Cells["Rate"].Value) * (NetWeight3 / Uom2);
						r.Cells["Amount"].Value = CurrentRowAmount;
					}
				}
				DtStock.Clear();
				ReportsParameters objj2 = new ReportsParameters();
				objj2.OrganizationId = UserAccount.CompanyId;
				objj2.CompanyId = UserAccount.OrganizationId;
				objj2.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				objj2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				objj2.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj2);
				if (DtStock.Rows.Count > 0)
				{
					decimal Stock2 = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
					if (Stock2 > 0m)
					{
						r.Cells["BalanceStock"].Value = Stock2;
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
				}
				else
				{
					r.Cells["BalanceStock"].Value = 0;
				}
				double AvgRateFromDb2 = CommonServices.getAvgRateByItemId(objj2.ItemId, objj2.DocDate, 1122, RecId);
				if (AvgRateFromDb2 > 0.0)
				{
					decimal AvgRate2 = Conversion.ToDecimal((object)AvgRateFromDb2);
					r.Cells["Rate"].Value = AvgRate2;
					decimal NetWeight2 = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					if (NetWeight2 > 0m && AvgRate2 > 0m)
					{
						decimal Amount2 = NetWeight2 * AvgRate2;
						r.Cells["Amount"].Value = Amount2;
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdInputMaterial.CurrentColumn != null && (grdInputMaterial.CurrentColumn.Key == "ItemUomId" || grdInputMaterial.CurrentColumn.Key == "Uom" || grdInputMaterial.CurrentColumn.Key == "UomCode"))
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (ItemId == 0)
				{
					throw new Exception("Please Select an Item First...");
				}
				dtUom.Clear();
				bindUom(ItemId);
				if (dtUom.Rows.Count > 0)
				{
					GrdPopUp obj = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["UomCode"].Value = NameCode;
					decimal Uom = default(decimal);
					DataRow firstRow = (from row in dtUom.AsEnumerable()
						where row.Field<int>("Id") == Id
						select row).FirstOrDefault();
					if (firstRow != null)
					{
						Uom = Conversion.ToDecimal(firstRow["ItemUOM"]);
					}
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["Uom"].Value = Uom;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Uom;
					DtStock.Clear();
					ReportsParameters objj = new ReportsParameters();
					objj.OrganizationId = UserAccount.CompanyId;
					objj.CompanyId = UserAccount.OrganizationId;
					objj.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					objj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					objj.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj);
					if (DtStock.Rows.Count > 0)
					{
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
						if (Stock > 0m)
						{
							r.Cells["BalanceStock"].Value = Stock;
						}
						else
						{
							r.Cells["BalanceStock"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
					double AvgRateFromDb = CommonServices.getAvgRateByItemId(objj.ItemId, objj.DocDate, 1122, RecId);
					if (AvgRateFromDb > 0.0)
					{
						decimal AvgRate = Conversion.ToDecimal((object)AvgRateFromDb);
						r.Cells["Rate"].Value = AvgRate;
						decimal NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						if (NetWeight > 0m && AvgRate > 0m)
						{
							decimal Amount = NetWeight * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPutByProduct.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOutPutByProduct.CurrentColumn.Key == "Add")
				{
					AddRowInOutPutByProductGrid();
				}
				if (grdOutPutByProduct.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ProductionOutPutDetail d2 = new ProductionOutPutDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordOutPut.Add(d2);
						dtOutPutByProduct.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdOutPutByProduct.UpdateData();
					}
					else
					{
						r.Delete();
					}
					if (dtOutPutByProduct.Rows.Count == 0)
					{
						AddRowInOutPutByProductGrid();
					}
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
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionOutPutDetail d1 = new ProductionOutPutDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
					grdOutPutByProduct.UpdateData();
				}
				else
				{
					r.Delete();
				}
				_ = dtOutPutByProduct.Rows.Count;
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string NameCode = "";
				if (grdOutPutByProduct.CurrentColumn.Key == "WarehouseName")
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtWare, "Id", "WarehouseName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					NameCode = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
					r.Cells["WarehouseName"].Value = NameCode;
				}
				_ = grdOutPutByProduct.CurrentColumn.Key == "ItemName";
				if (!(grdOutPutByProduct.CurrentColumn.Key == "ItemUomId") && !(grdOutPutByProduct.CurrentColumn.Key == "Uom"))
				{
					_ = grdOutPutByProduct.CurrentColumn.Key == "UomCode";
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
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
				GrdPopUp obj2 = new GrdPopUp(dtAccounts, "Id", "AccountTitle", "AccountCode");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				if (obj2.ReturnId > 0)
				{
					r.Cells["OverHeadAccountId"].Value = obj2.ReturnId;
				}
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

	private void grdConsumption_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_1086: Unknown result type (might be due to invalid IL or missing references)
		//IL_108d: Expected O, but got Unknown
		//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdConsupmtionDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdConsupmtionDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdConsupmtionDetail.CurrentColumn.Key == "Add")
				{
					AddRowInConsumptionGrid();
				}
				if (grdConsupmtionDetail.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ProductionConsumptionDetail d2 = new ProductionConsumptionDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.BomConsumptionId = Conversion.ToInt(r.Cells["BomConsumptionId"].Value);
						d2.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordConsumption.Add(d2);
						dtConsupmtionDetail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdConsupmtionDetail.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtConsupmtionDetail.Rows.Count == 0)
					{
						AddRowInConsumptionGrid();
					}
				}
				grdConsupmtionDetail.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInConsumptionGrid();
				grdConsupmtionDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionConsumptionDetail d1 = new ProductionConsumptionDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.BomConsumptionId = Conversion.ToInt(r.Cells["BomConsumptionId"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordConsumption.Add(d1);
					dtConsupmtionDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdConsupmtionDetail.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdConsupmtionDetail.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			if (grdConsupmtionDetail.CurrentColumn != null && (grdConsupmtionDetail.CurrentColumn.Key == "WarehouseId" || grdConsupmtionDetail.CurrentColumn.Key == "WarehouseName"))
			{
				GrdPopUp obj4 = new GrdPopUp(dtWare, "Id", "WarehouseName");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				NameCode = Conversion.ToString((object)obj4.ReturnName);
				r.Cells["WarehouseId"].Value = Id;
				r.Cells["WarehouseName"].Value = NameCode;
				DtStock.Clear();
				ReportsParameters objj3 = new ReportsParameters();
				objj3.OrganizationId = UserAccount.CompanyId;
				objj3.CompanyId = UserAccount.OrganizationId;
				objj3.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				objj3.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				objj3.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj3);
				if (DtStock.Rows.Count > 0)
				{
					decimal Stock3 = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
					if (Stock3 > 0m)
					{
						r.Cells["BalanceStock"].Value = Stock3;
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
				}
				else
				{
					r.Cells["BalanceStock"].Value = 0;
				}
				double AvgRateFromDb3 = CommonServices.getAvgRateByItemId(objj3.ItemId, objj3.DocDate, 1122, RecId);
				if (AvgRateFromDb3 > 0.0)
				{
					decimal AvgRate3 = Conversion.ToDecimal((object)AvgRateFromDb3);
					r.Cells["Rate"].Value = AvgRate3;
					decimal NetWeight4 = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					if (NetWeight4 > 0m && AvgRate3 > 0m)
					{
						decimal Amount3 = NetWeight4 * AvgRate3;
						r.Cells["Amount"].Value = Amount3;
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdConsupmtionDetail.CurrentColumn != null && grdConsupmtionDetail.CurrentColumn.Key == "ItemName")
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(lstConsumptionitem, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				NameCode = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = NameCode;
				if (Id != PreviousItemId)
				{
					r.Cells["ParentCategory"].Value = "";
					r.Cells["ItemUomId"].Value = 0;
					r.Cells["Uom"].Value = 0;
					r.Cells["UomCode"].Value = "";
					r.Cells["RateUom"].Value = 0;
					r.Cells["RateUomId"].Value = 0;
					r.Cells["RateUomCode"].Value = "";
					r.Cells["NetWeight"].Value = 0;
				}
				string parentCategory = lstConsumptionitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id)?.Field<string>("ParentCategory");
				r.Cells["ParentCategory"].Value = parentCategory;
				if (Conversion.ToInt((object)r.Cells["ItemUomId"]) == 0)
				{
					dtUom.Clear();
					bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dtUom.Rows.Count > 0)
					{
						int ItemUomId = 0;
						string UomCode = "";
						decimal Uom2 = default(decimal);
						DataRow firstRow2 = dtUom.AsEnumerable().FirstOrDefault();
						if (firstRow2 != null)
						{
							ItemUomId = Conversion.ToInt(firstRow2["Id"]);
							Uom2 = Conversion.ToDecimal(firstRow2["ItemUOM"]);
							UomCode = Conversion.ToString(firstRow2["ItemUOMCode"]);
						}
						r.Cells["ItemUomId"].Value = ItemUomId;
						r.Cells["UomCode"].Value = UomCode;
						r.Cells["Uom"].Value = Uom2;
						r.Cells["RateUomId"].Value = ItemUomId;
						r.Cells["RateUomCode"].Value = UomCode;
						r.Cells["RateUom"].Value = Uom2;
						decimal NetWeight3 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString()) * Uom2;
						r.Cells["NetWeight"].Value = NetWeight3;
						decimal CurrentRowAmount = Conversion.ToDecimal(r.Cells["Rate"].Value) * (NetWeight3 / Uom2);
						r.Cells["Amount"].Value = CurrentRowAmount;
					}
				}
				DtStock.Clear();
				ReportsParameters objj2 = new ReportsParameters();
				objj2.OrganizationId = UserAccount.CompanyId;
				objj2.CompanyId = UserAccount.OrganizationId;
				objj2.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				objj2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				objj2.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj2);
				if (DtStock.Rows.Count > 0)
				{
					decimal Stock2 = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
					if (Stock2 > 0m)
					{
						r.Cells["BalanceStock"].Value = Stock2;
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
				}
				else
				{
					r.Cells["BalanceStock"].Value = 0;
				}
				double AvgRateFromDb2 = CommonServices.getAvgRateByItemId(objj2.ItemId, objj2.DocDate, 1122, RecId);
				if (AvgRateFromDb2 > 0.0)
				{
					decimal AvgRate2 = Conversion.ToDecimal((object)AvgRateFromDb2);
					r.Cells["Rate"].Value = AvgRate2;
					decimal NetWeight2 = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					if (NetWeight2 > 0m && AvgRate2 > 0m)
					{
						decimal Amount2 = NetWeight2 * AvgRate2;
						r.Cells["Amount"].Value = Amount2;
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdConsupmtionDetail.CurrentColumn != null && (grdConsupmtionDetail.CurrentColumn.Key == "ItemUomId" || grdConsupmtionDetail.CurrentColumn.Key == "Uom" || grdConsupmtionDetail.CurrentColumn.Key == "UomCode"))
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (ItemId == 0)
				{
					throw new Exception("Please Select an Item First...");
				}
				dtUom.Clear();
				bindUom(ItemId);
				if (dtUom.Rows.Count > 0)
				{
					GrdPopUp obj2 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["UomCode"].Value = NameCode;
					decimal Uom = default(decimal);
					DataRow firstRow = (from row in dtUom.AsEnumerable()
						where row.Field<int>("Id") == Id
						select row).FirstOrDefault();
					if (firstRow != null)
					{
						Uom = Conversion.ToDecimal(firstRow["ItemUOM"]);
					}
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["Uom"].Value = Uom;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Uom;
					DtStock.Clear();
					ReportsParameters objj = new ReportsParameters();
					objj.OrganizationId = UserAccount.CompanyId;
					objj.CompanyId = UserAccount.OrganizationId;
					objj.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					objj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					objj.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj);
					if (DtStock.Rows.Count > 0)
					{
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]);
						if (Stock > 0m)
						{
							r.Cells["BalanceStock"].Value = Stock;
						}
						else
						{
							r.Cells["BalanceStock"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
					}
					double AvgRateFromDb = CommonServices.getAvgRateByItemId(objj.ItemId, objj.DocDate, 1122, RecId);
					if (AvgRateFromDb > 0.0)
					{
						decimal AvgRate = Conversion.ToDecimal((object)AvgRateFromDb);
						r.Cells["Rate"].Value = AvgRate;
						decimal NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						if (NetWeight > 0m && AvgRate > 0m)
						{
							decimal Amount = NetWeight * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
			}
			if (grdConsupmtionDetail.CurrentColumn != null && (grdConsupmtionDetail.CurrentColumn.Key == "BomHeaderId" || grdConsupmtionDetail.CurrentColumn.Key == "BomNo"))
			{
				if (Conversion.ToInt((object)grdOutPutByProduct.GetDataRows().Count()) == 0)
				{
					throw new Exception("Out Put Grid Record not founds...");
				}
				if (Conversion.ToInt(r.Cells["BomConsumptionId"].Value) != 0)
				{
					throw new Exception("You can't Update Bom No of row Against Bill Of Material");
				}
				GetOutPutGridBomNos();
				if (dtOutputBom.Rows.Count > 0)
				{
					GrdPopUp obj = new GrdPopUp(dtOutputBom, "BomHeaderId", "BomNo");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["BomHeaderId"].Value = obj.ReturnId;
					r.Cells["BomNo"].Value = obj.ReturnName;
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					if (Id > 0)
					{
						GetOutPutGridItemsForWages(Id);
						DataRow foundRow = dtOutputItem.AsEnumerable().FirstOrDefault();
						int OutPutItemId = ((foundRow != null) ? Conversion.ToInt(foundRow["OutItemId"]) : 0);
						r.Cells["OutPutItemId"].Value = OutPutItemId;
					}
					else
					{
						r.Cells["OutPutItemId"].Value = 0;
					}
				}
			}
			if (grdConsupmtionDetail.CurrentColumn != null && !(grdConsupmtionDetail.CurrentColumn.Key == "OutPutItemId"))
			{
				_ = grdConsupmtionDetail.CurrentColumn.Key == "OutPutItemName";
			}
			grdConsupmtionDetail.UpdateData();
			Calculation();
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
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ProductionManualSlipFeedMill(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1122);
				}
				if (DatagridHistory.CurrentColumn.Key == "SaveAs")
				{
					if (!formright.DoHaveSaveRight)
					{
						throw new Exception("you don't have save rights...");
					}
					ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					BtnSaveAs.Visible = true;
					if (grdOutPutByProduct.GetDataRows().Count() > 0)
					{
						GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
						foreach (GridEXRow obj in dataRows)
						{
							obj.BeginEdit();
							obj.Cells["Id"].Value = 0;
							obj.EndEdit();
						}
					}
					if (grdConsupmtionDetail.GetDataRows().Count() > 0)
					{
						GridEXRow[] dataRows = grdConsupmtionDetail.GetDataRows();
						foreach (GridEXRow obj2 in dataRows)
						{
							obj2.BeginEdit();
							obj2.Cells["Id"].Value = 0;
							obj2.EndEdit();
						}
					}
					if (grdInputMaterial.GetDataRows().Count() > 0)
					{
						GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
						foreach (GridEXRow obj3 in dataRows)
						{
							obj3.BeginEdit();
							obj3.Cells["Id"].Value = 0;
							obj3.EndEdit();
						}
					}
					if (grdOverHead.GetDataRows().Count() > 0)
					{
						GridEXRow[] dataRows = grdOverHead.GetDataRows();
						foreach (GridEXRow obj4 in dataRows)
						{
							obj4.BeginEdit();
							obj4.Cells["Id"].Value = 0;
							obj4.EndEdit();
						}
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)Id));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInputMaterial).Name);
			if (fs != null)
			{
				grdInputMaterial.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutPutByProduct).Name);
			if (fs != null)
			{
				grdOutPutByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryOutput).Name);
			if (fs != null)
			{
				grdHistoryOutput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdConsumptionHistory).Name);
			if (fs != null)
			{
				grdConsumptionHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryOverHead).Name);
			if (fs != null)
			{
				grdHistoryOverHead.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryInput).Name);
			if (fs != null)
			{
				grdHistoryInput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			WF.Show(this);
			UpdateInputCurrentStockAndAvgRate();
			UpdateConsumptionCurrentStockAndAvgRate();
			UpdateOverHeadByOutput();
			UpdateOutputRates();
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

	private void UpdateInputCurrentStockAndAvgRate()
	{
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				DtStock.Clear();
				GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					row.BeginEdit();
					UpdateRowStockAndRate(row);
					row.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			throw new Exception(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void UpdateConsumptionCurrentStockAndAvgRate()
	{
		try
		{
			if (grdConsupmtionDetail.GetDataRows().Count() > 0)
			{
				DtStock.Clear();
				GridEXRow[] dataRows = grdConsupmtionDetail.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					row.BeginEdit();
					UpdateRowStockAndRate(row);
					row.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateOverHeadByOutput()
	{
		try
		{
			if (grdOverHead.GetDataRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdOverHead.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				if (Conversion.ToInt(row.Cells["BomOverHeadId"].Value) > 0)
				{
					row.BeginEdit();
					int bomHeaderId = Conversion.ToInt(row.Cells["BomHeaderId"].Value);
					DataRow foundRow = dtBOMDataforCombo.AsEnumerable().FirstOrDefault((DataRow rrr) => rrr.Field<int>("Id") == bomHeaderId);
					decimal bomFinishQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["BomBatchQty"]) : 0m);
					decimal productionFinishQty = (from rrr in dtOutPutByProduct.AsEnumerable()
						where rrr.Field<int>("BomHeaderId") == bomHeaderId
						select rrr).Sum((DataRow rrr) => Conversion.ToDecimal(rrr["ItemQty"]));
					decimal bomQty = Conversion.ToDecimal(row.Cells["BomQty"].Value);
					decimal bomAmount = Conversion.ToDecimal(row.Cells["BomAmount"].Value);
					if (bomQty > 0m)
					{
						decimal qty = ((bomFinishQty > 0m) ? (bomQty / bomFinishQty * productionFinishQty) : 0m);
						row.Cells["Qty"].Value = qty;
						decimal rate = Conversion.ToDecimal(row.Cells["Rate"].Value);
						row.Cells["Amount"].Value = qty * rate;
					}
					else if (bomAmount > 0m)
					{
						row.Cells["Rate"].Value = 0;
						row.Cells["Qty"].Value = 0;
						row.Cells["Amount"].Value = ((bomFinishQty > 0m) ? (bomAmount / bomFinishQty * productionFinishQty) : 0m);
					}
					else
					{
						row.Cells["Qty"].Value = 0;
						row.Cells["Amount"].Value = 0;
					}
					row.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateRowStockAndRate(GridEXRow row)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		try
		{
			ReportsParameters objj = new ReportsParameters
			{
				OrganizationId = UserAccount.CompanyId,
				CompanyId = UserAccount.OrganizationId,
				WarehouseId = Conversion.ToInt(row.Cells["WarehouseId"].Value),
				ItemId = Conversion.ToInt(row.Cells["ItemId"].Value),
				DocDate = Conversion.ToDateTime((object)txtdocdate.Value)
			};
			DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation(objj);
			decimal stock = ((DtStock.Rows.Count > 0) ? Conversion.ToDecimal(DtStock.Rows[0]["StockInHand"]) : 0m);
			row.Cells["BalanceStock"].Value = stock;
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.CompanyId,
				CompanyId = UserAccount.OrganizationId,
				WarehouseId = Conversion.ToInt(row.Cells["WarehouseId"].Value),
				ItemId = Conversion.ToInt(row.Cells["ItemId"].Value),
				DocDate = Conversion.ToDateTime((object)txtdocdate.Value)
			};
			double avgRateFromDb = CommonServices.getAvgRateByItemId(obj.ItemId, obj.DocDate, 1122, RecId);
			decimal avgRate = ((avgRateFromDb > 0.0) ? Conversion.ToDecimal((object)avgRateFromDb) : 0m);
			row.Cells["Rate"].Value = avgRate;
			decimal netWeight = Conversion.ToDecimal(row.Cells["NetWeight"].Value);
			decimal amount = ((netWeight > 0m && avgRate > 0m) ? (netWeight * avgRate) : 0m);
			row.Cells["Amount"].Value = amount;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateOutputRates()
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				WF.Close();
				throw new Exception("Output Grid Record not Found");
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 1)
			{
				GridEXRow rowsToValidate = (from rr in grdOutPutByProduct.GetDataRows()
					where Conversion.ToInt(rr.Cells["BomHeaderId"].Value) > 0
					select rr).FirstOrDefault();
				if (rowsToValidate != null && Conversion.ToInt(rowsToValidate.Cells["ItemId"].Value) == 0)
				{
					WF.Close();
					throw new Exception("Output Grid Record not Filled Completely");
				}
			}
			Calculation();
			decimal num = (from GridEXRow row in grdInputMaterial.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal sumOfOverhead = (from GridEXRow row in grdOverHead.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal sumOfConsumption = (from GridEXRow row in grdConsupmtionDetail.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal netValue = num + sumOfOverhead + sumOfConsumption;
			decimal ItemQty = (from GridEXRow row in grdOutPutByProduct.GetDataRows()
				select (decimal)row.Cells["ItemQty"].Value).Sum();
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				obj.BeginEdit();
				decimal RowQty = Conversion.ToDecimal(obj.Cells["ItemQty"].Value);
				decimal currentRowAmount = ((ItemQty > 0m) ? (netValue / ItemQty * RowQty) : 0m);
				obj.Cells["Amount"].Value = currentRowAmount;
				decimal currentRowRate = ((RowQty > 0m) ? (currentRowAmount / RowQty) : 0m);
				obj.Cells["Rate"].Value = currentRowRate;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			throw new Exception(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void BtnUpdateWarehouseInput_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdInputMaterial.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						WF.Close();
						throw new Exception("Warehouse Field is Empty in Input Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					WF.Show(this);
					GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.EndEdit();
					}
				}
				return;
			}
			WF.Close();
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
						WF.Close();
						throw new Exception("Warehouse Field is Empty in OutPut Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					WF.Show(this);
					GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.EndEdit();
					}
				}
				return;
			}
			WF.Close();
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

	private void BtnUpdateWarehouseInConsumption_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdConsupmtionDetail.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdConsupmtionDetail.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						WF.Close();
						throw new Exception("Warehouse Field is Empty in Consumption Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					WF.Show(this);
					GridEXRow[] dataRows = grdConsupmtionDetail.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.EndEdit();
					}
				}
				return;
			}
			WF.Close();
			throw new Exception("Consumption Grid Record not Found");
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

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
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

	private void BtnFormulasOnForm_Click(object sender, EventArgs e)
	{
	}

	private bool AutoUpdatedRecords(int RecordId)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Expected O, but got Unknown
		//IL_1241: Unknown result type (might be due to invalid IL or missing references)
		//IL_1248: Expected O, but got Unknown
		//IL_1500: Unknown result type (might be due to invalid IL or missing references)
		//IL_1507: Expected O, but got Unknown
		//IL_1c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3c: Expected O, but got Unknown
		//IL_1cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce6: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return false;
			}
			if (grdInputMaterial.GetDataRows().Count() == 0 || grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");
				return false;
			}
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader.Id = RecordId;
			IsDuplicateRowExists();
			decimal sumoftotalOutPut = Conversion.ToDecimal((object)grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			if (!decimal.TryParse(txtTotalOutPutAutoRatedAmount.Text, out var TotalOutPutAutoRatedAmount))
			{
				MessageBox.Show("Invalid input for Total Output Auto Rated Amount.");
				return false;
			}
			TotalOutPutAutoRatedAmount = Math.Round(TotalOutPutAutoRatedAmount, 0);
			decimal roundedSumOfTotalOutput = Math.Round(sumoftotalOutPut, 0);
			if (Math.Abs(TotalOutPutAutoRatedAmount - roundedSumOfTotalOutput) > 1m)
			{
				MessageBox.Show($"Net Value {TotalOutPutAutoRatedAmount} in Header and Total Output Amount {roundedSumOfTotalOutput} are not equal");
				return false;
			}
			ProductionHeader.CompanyId = UserAccount.CompanyId;
			ProductionHeader.BranchesId = UserAccount.BranchesId;
			ProductionHeader.ProjectsId = UserAccount.BranchesId;
			ProductionHeader.OrganizationId = UserAccount.OrganizationId;
			ProductionHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ProductionHeader.DocumentTypeId = 1122;
			ProductionHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			ProductionHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ProductionHeader.ShiftId = Conversion.ToInt(CmbShift.Value);
			ProductionHeader.WorkOrderId = 0;
			ProductionHeader.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			ProductionHeader.PlantId = Conversion.ToInt(CmbPlantName.Value);
			ProductionHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ProductionHeader.WIPWarehouseId = 0;
			ProductionHeader.ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			ProductionHeader.WIPGLAccountID = 0;
			ProductionHeader.EntryUserId = UserAccount.ID;
			ProductionHeader.EntryDate = DateTime.Now;
			ProductionHeader.ModifyDate = DateTime.Now;
			ProductionHeader.ModifyUserId = UserAccount.ID;
			ProductionHeader.ApprovedUserId = UserAccount.ID;
			ProductionHeader.ApprovedDate = DateTime.Now;
			ProductionHeader.IsApproved = false;
			ProductionHeader.AutoUpdate = true;
			ProductionHeader.ProductionInputDetailList = new List<ProductionInputDetail>();
			ProductionHeader.ProductionOverHeadsDetailList = new List<ProductionOverHeadsDetail>();
			ProductionHeader.ProductionOutPutDetailList = new List<ProductionOutPutDetail>();
			ProductionHeader.ProductionConsumptionDetailList = new List<ProductionConsumptionDetail>();
			ProductionHeader.AttachmentsList = new List<DMSAttachments>();
			int LineId = 0;
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				ProductionInputDetail d2 = new ProductionInputDetail();
				d2.Id = ((RecordId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				if (d2.Id > 0)
				{
					d2.ActionTypeId = 2;
				}
				else
				{
					d2.ActionTypeId = 1;
				}
				d2.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
				d2.BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
				if (r.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (r.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemUomId"].Value) == 0)
				{
					MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Id='" + Conversion.ToInt(r.Cells["ItemUomId"].Value) + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("UOM in Input Grid Row No: " + (LineId + 1) + " Is not Define Against Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + ". Please Check...");
						return false;
					}
					d2.ItemUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				if (r.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
				if (r.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r.Cells["NetWeight"].Value) == 0m)
				{
					MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				if (r.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Rate"].Value) == 0m)
				{
					MessageBox.Show("Rate Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
				if (r.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r.Cells["RateUomId"].Value) == 0)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt2.Rows.Count > 0)
				{
					DataRow[] dr2 = dt2.Select("Id='" + Conversion.ToInt(r.Cells["RateUomId"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("RateUom in Input Grid Row No: " + (LineId + 1) + " Is not Define Against Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + ". Please Check...");
						return false;
					}
					d2.RateUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
				if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
				{
					MessageBox.Show("Amount Field Required in Grid Input Row No: " + (LineId + 1));
					return false;
				}
				d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				d2.PrctOfTotalQty = Conversion.ToDecimal(r.Cells["PrctOfTotalQty"].Value);
				ProductionHeader.ProductionInputDetailList.Add(d2);
				LineId++;
			}
			if (lstRemoveRecordInput.Count > 0)
			{
				for (int k = 0; k < lstRemoveRecordInput.Count; k++)
				{
					ProductionHeader.ProductionInputDetailList.Add(lstRemoveRecordInput[k]);
				}
			}
			dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					ProductionOutPutDetail d3 = new ProductionOutPutDetail();
					d3.Id = ((RecordId > 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					if (d3.Id > 0)
					{
						d3.ActionTypeId = 2;
					}
					else
					{
						d3.ActionTypeId = 1;
					}
					d3.EntryTypeId = 1;
					if (ProductionHeader.ProductionTypeId != 4 && ProductionHeader.ProductionTypeId != 5)
					{
						if (r2.Cells["BomHeaderId"].Text == string.Empty || Conversion.ToInt(r2.Cells["BomHeaderId"].Value) == 0)
						{
							MessageBox.Show("BomHeader Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
							return false;
						}
						d3.BOMHeaderId = Conversion.ToInt(r2.Cells["BomHeaderId"].Value);
					}
					d3.BOMHeaderId = Conversion.ToInt(r2.Cells["BomHeaderId"].Value);
					if (r2.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r2.Cells["WarehouseId"].Value) == 0)
					{
						MessageBox.Show("Warehouse Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.WareHouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
					if (r2.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					if (r2.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["ItemQty"].Value) == 0m)
					{
						MessageBox.Show("ItemQty Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.ItemQty = Conversion.ToDecimal((object)r2.Cells["ItemQty"].Text);
					if (r2.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Rate"].Value) == 0m)
					{
						MessageBox.Show("Rate Field Required in Grid Output Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					if (r2.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Rate"].Value) == 0m)
					{
						MessageBox.Show("AmountField Required in Grid Output Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					d3.NetWeight = d3.ItemQty;
					if (r2.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r2.Cells["ItemUomId"].Value) == 0)
					{
						MessageBox.Show("UOM  Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
						return false;
					}
					DataTable dt4 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
					if (dt4.Rows.Count > 0)
					{
						DataRow[] dr5 = dt4.Select("Id='" + Conversion.ToInt(r2.Cells["ItemUomId"].Value) + "'");
						if (dr5.Length == 0)
						{
							MessageBox.Show("UOM in Output Grid Row No: " + (r2.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
							return false;
						}
						d3.ItemUomId = Conversion.ToInt(dr5[0].ItemArray[0]);
					}
					if (r2.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["NetWeight"].Value) == 0m)
					{
						MessageBox.Show("NetWeight Field Required in Grid OutPut Row No: " + (r2.RowIndex + 1));
						return false;
					}
					d3.NetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					if (r2.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r2.Cells["RateUomId"].Value) == 0)
					{
						MessageBox.Show("Rate UOM Field Required in Grid Output Row No: " + (r2.RowIndex + 1));
						return false;
					}
					DataTable dt6 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
					if (dt6.Rows.Count > 0)
					{
						DataRow[] dr6 = dt6.Select("Id='" + Conversion.ToInt(r2.Cells["RateUomId"].Value) + "'");
						if (dr6.Length == 0)
						{
							MessageBox.Show("RateUom in output Grid Row No: " + (r2.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
							return false;
						}
						d3.RateUomId = Conversion.ToInt(dr6[0].ItemArray[0]);
					}
					ProductionHeader.ProductionOutPutDetailList.Add(d3);
					continue;
				}
				MessageBox.Show("Item Field Required in Grid output Row No: " + (r2.RowIndex + 1));
				return false;
			}
			if (lstRemoveRecordOutPut.Count > 0)
			{
				for (int l = 0; l < lstRemoveRecordOutPut.Count; l++)
				{
					ProductionHeader.ProductionOutPutDetailList.Add(lstRemoveRecordOutPut[l]);
				}
			}
			dataRows = grdOverHead.GetDataRows();
			foreach (GridEXRow r4 in dataRows)
			{
				if (Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value) > 0)
				{
					ProductionOverHeadsDetail d = new ProductionOverHeadsDetail();
					if (r4.Cells["OverHeadAccountId"].Text == string.Empty || Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value) == 0)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r4.RowIndex + 1));
						return false;
					}
					d.Id = ((RecordId > 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
					d.BomOverHeadId = Conversion.ToInt(r4.Cells["BomOverHeadId"].Value);
					d.OverHeadTypeId = Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value);
					d.OverHeadAccountId = Conversion.ToInt(r4.Cells["OverHeadAccountId"].Value);
					if (r4.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r4.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r4.RowIndex + 1));
						return false;
					}
					d.PercentOfTotalInput = Conversion.ToDecimal((object)r4.Cells["PercentOfTotalInput"].Text);
					d.Qty = Conversion.ToDecimal((object)r4.Cells["Qty"].Text);
					d.BomQty = Conversion.ToDecimal((object)r4.Cells["BomQty"].Text);
					d.Rate = Conversion.ToDecimal((object)r4.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r4.Cells["Amount"].Text);
					d.OhRemarks = Conversion.ToString((object)r4.Cells["OhRemarks"].Text);
					d.BomHeaderId = Conversion.ToInt(r4.Cells["BomHeaderId"].Value);
					ProductionHeader.ProductionOverHeadsDetailList.Add(d);
				}
			}
			dataRows = grdConsupmtionDetail.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				ProductionConsumptionDetail d4 = new ProductionConsumptionDetail();
				d4.Id = ((RecordId > 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				if (d4.Id > 0)
				{
					d4.ActionTypeId = 2;
				}
				else
				{
					d4.ActionTypeId = 1;
				}
				d4.BomConsumptionId = Conversion.ToInt(r3.Cells["BomConsumptionId"].Value);
				d4.BomHeaderId = Conversion.ToInt(r3.Cells["BomHeaderId"].Value);
				if (r3.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r3.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.WareHouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
				if (r3.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r3.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				if (r3.Cells["ItemUomId"].Text == string.Empty || Conversion.ToInt(r3.Cells["ItemUomId"].Value) == 0)
				{
					MessageBox.Show("UOM  Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				DataTable dt5 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt5.Rows.Count > 0)
				{
					DataRow[] dr4 = dt5.Select("Id='" + Conversion.ToInt(r3.Cells["ItemUomId"].Value) + "'");
					if (dr4.Length == 0)
					{
						MessageBox.Show("UOM in Consumption Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return false;
					}
					d4.ItemUomId = Conversion.ToInt(dr4[0].ItemArray[0]);
				}
				if (r3.Cells["ItemQty"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.ItemQty = Conversion.ToDecimal((object)r3.Cells["ItemQty"].Text);
				if (r3.Cells["NetWeight"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["NetWeight"].Value) == 0m)
				{
					MessageBox.Show("NetWeight Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.NetWeight = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				if (r3.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["Rate"].Value) == 0m)
				{
					MessageBox.Show("Rate Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.Rate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
				if (r3.Cells["RateUomId"].Text == string.Empty || Conversion.ToInt(r3.Cells["RateUomId"].Value) == 0)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				DataTable dt3 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt3.Rows.Count > 0)
				{
					DataRow[] dr3 = dt3.Select("Id='" + Conversion.ToInt(r3.Cells["RateUomId"].Value) + "'");
					if (dr3.Length == 0)
					{
						MessageBox.Show("RateUom in Consumption Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return false;
					}
					d4.RateUomId = Conversion.ToInt(dr3[0].ItemArray[0]);
				}
				if (r3.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r3.Cells["Amount"].Value) == 0m)
				{
					MessageBox.Show("AmountField Required in Grid Consumption Row No: " + (r3.RowIndex + 1));
					return false;
				}
				d4.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
				d4.OutPutItemId = Conversion.ToInt(r3.Cells["OutPutItemId"].Value);
				ProductionHeader.ProductionConsumptionDetailList.Add(d4);
			}
			if (lstRemoveRecordConsumption.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecordConsumption.Count; j++)
				{
					ProductionHeader.ProductionConsumptionDetailList.Add(lstRemoveRecordConsumption[j]);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.FileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				ProductionHeader.AttachmentsList.Add(attach);
			}
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				ProductionHeader.DeleteAttachmentsList = new List<DMSAttachments>();
				for (int m = 0; m < AT.RemovedAttachmentListInUpdateCase.Count; m++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[m].Path);
					delete.ScreenName = base.Name;
					ProductionHeader.DeleteAttachmentsList.Add(delete);
				}
			}
			int success = ProductionHeader.Save(ProductionHeader);
			ProductionHeader.Id = success;
			FromReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
		return true;
	}

	private async void btnAutoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DateTime fromDate = datFromDate.Value;
			DateTime toDate = datTodate.Value;
			CommonServices.DatelockValidationAndStatusOpenForCgsRun(UserAccount.OrganizationId, UserAccount.CompanyId, fromDate, toDate);
			DateTime currentDate = fromDate;
			while (currentDate <= toDate)
			{
				DataTable dtIds = CommonServices.GetIdsOfDocumentsDateWiseForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 1122, currentDate);
				if (dtIds.Rows.Count > 0 && dtIds != null)
				{
					foreach (DataRow row in dtIds.Rows)
					{
						int recId = Conversion.ToInt(row["Id"]);
						ReadById(recId);
						UpdateInputCurrentStockAndAvgRate();
						UpdateConsumptionCurrentStockAndAvgRate();
						UpdateOverHeadByOutput();
						UpdateOutputRates();
						await Task.Delay(500);
						if (!AutoUpdatedRecords(recId))
						{
							MessageBox.Show("Auto-update process stopped.");
							return;
						}
					}
				}
				DataTable dtSaleItemIds = CommonServices.getItemIdsByDate(UserAccount.OrganizationId, UserAccount.CompanyId, currentDate, "Sale");
				if (dtSaleItemIds.Rows.Count > 0)
				{
					foreach (DataRow row2 in dtSaleItemIds.Rows)
					{
						int ItemId = Conversion.ToInt(row2["ItemId"]);
						if (ItemId > 0)
						{
							GeneralReprots.SaleCGS_Update(new ReportsParameters
							{
								OrganizationId = UserAccount.OrganizationId,
								CompanyId = UserAccount.CompanyId,
								DocDate = currentDate,
								ItemId = ItemId,
								EntryUser = UserAccount.ID
							});
						}
					}
				}
				btnRecordsUpdate.Visible = AdminRole;
				datFromDate.Visible = AdminRole;
				datTodate.Visible = AdminRole;
				currentDate = currentDate.AddDays(1.0);
			}
			btnRecordsUpdate.Visible = AdminRole;
			datFromDate.Visible = AdminRole;
			datTodate.Visible = AdminRole;
			MessageBox.Show("Records Updated Successfully");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = AdminRole;
			MessageBox.Show("An error occurred: " + ex.Message);
		}
	}

	private async void btnReProcessing_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = CommonServices.GetIdsOfDocumentsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 1122, 2);
			if (dtIds == null || dtIds.Rows.Count == 0)
			{
				MessageBox.Show("Ids Not Found For Update");
				return;
			}
			foreach (DataRow row in dtIds.Rows)
			{
				int recId = Conversion.ToInt(row["Id"]);
				ReadById(recId);
				UpdateOutputRates();
				await Task.Delay(500);
				if (!AutoUpdatedRecords(recId))
				{
					MessageBox.Show("Auto-update process stopped.");
					return;
				}
			}
			btnReProcessing.Visible = AdminRole;
			MessageBox.Show("Records Updated Successfully");
		}
		catch (Exception ex)
		{
			btnReProcessing.Visible = AdminRole;
			MessageBox.Show("An error occurred: " + ex.Message);
		}
	}

	private void CmbProductionType_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItId = Conversion.ToInt(cmbItemBOM.Value);
			if (Conversion.ToInt(CmbProductionType.Value) == 4 || Conversion.ToInt(CmbProductionType.Value) == 5)
			{
				if (dtItem.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("ItemName");
					dt.Columns.Add("ItemCode");
					for (int i = 0; i < dtItem.Rows.Count; i++)
					{
						dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"], dtItem.Rows[i]["ItemCode"]);
					}
					DDL.BindDDL(dt, cmbItemBOM, "Id", "ItemName", "Item Name", ZeroIndex: true);
					((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[1].Width = 350;
					if (ItId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
						{
							cmbItemBOM.Value = ItId;
						}
						else
						{
							((Control)(object)cmbItemBOM).Text = string.Empty;
						}
					}
				}
				((Control)(object)cmbUOM).Enabled = true;
				txtQty.Enabled = true;
				return;
			}
			if (dtOutputItemCombo.Rows.Count > 0)
			{
				DDL.BindDDL(dtOutputItemCombo, cmbItemBOM, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ItId > 0)
				{
					if (dtOutputItemCombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						cmbItemBOM.Value = ItId;
					}
					else
					{
						((Control)(object)cmbItemBOM).Text = string.Empty;
					}
				}
			}
			((Control)(object)cmbUOM).Enabled = false;
			txtQty.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateQtyByBom_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdInputMaterial.RowCount == 0)
			{
				throw new Exception("InPut have No Row");
			}
			if ((grdOutPutByProduct.GetDataRows().Any() ? grdOutPutByProduct.GetDataRows()[0] : null) == null)
			{
				throw new Exception("OutPut have No Row With BOM");
			}
			UpdateQtyByBom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateQtyByBom()
	{
		try
		{
			decimal OutPutRowItemQty = default(decimal);
			decimal OutPutRowBomQty = default(decimal);
			List<GridEXRow> validRows = (from r in grdOutPutByProduct.GetDataRows()
				where Conversion.ToInt(r.Cells["BomHeaderId"].Value) > 0
				select r).ToList();
			OutPutRowItemQty = validRows.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["ConsumptionQty"].Value));
			OutPutRowBomQty = validRows.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["BomQty"].Value));
			if (validRows.Any())
			{
				Conversion.ToInt(validRows.First().Cells["BomHeaderId"].Value);
			}
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				decimal InputBomQty = Conversion.ToDecimal(r2.Cells["BomQty"].Value);
				if (InputBomQty > 0m)
				{
					decimal Qty = ((OutPutRowBomQty > 0m && OutPutRowItemQty > 0m) ? (OutPutRowItemQty / OutPutRowBomQty * InputBomQty) : 0m);
					r2.BeginEdit();
					r2.Cells["ItemQty"].Value = Math.Round(Qty, 2);
					decimal quom = Conversion.ToDecimal((object)r2.Cells["Uom"].Value.ToString());
					decimal Weight = Qty * quom;
					r2.Cells["NetWeight"].Value = Math.Round(Weight, 2);
					double AvgRat = CommonServices.getAvgRateByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), 1122, RecId);
					Conversion.ToDecimal((object)AvgRat);
					r2.Cells["Rate"].Value = AvgRat;
					decimal TotalGridQty = ((grdInputMaterial.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["ItemQty"], (AggregateFunction)2)) : 0m);
					decimal BomQty = Conversion.ToDecimal((object)r2.Cells["BomQty"].Value.ToString());
					decimal PrctOftotal = ((TotalGridQty > 0m) ? (BomQty / TotalGridQty * 100m) : 0m);
					r2.Cells["PrctOfTotalQty"].Value = PrctOftotal;
					decimal rate = Conversion.ToDecimal((object)r2.Cells["Rate"].Value.ToString());
					decimal Amount = ((Weight > 0m) ? (Weight * rate) : 0m);
					r2.Cells["Amount"].Value = Amount;
					r2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void brnShift_Click(object sender, EventArgs e)
	{
		try
		{
			new frmGenShift(UserAccount).Show();
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
			AllCombobind();
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
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Expected O, but got Unknown
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Expected O, but got Unknown
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Expected O, but got Unknown
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Expected O, but got Unknown
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Expected O, but got Unknown
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Expected O, but got Unknown
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Expected O, but got Unknown
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8a: Expected O, but got Unknown
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Expected O, but got Unknown
		//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac1: Expected O, but got Unknown
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Expected O, but got Unknown
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1349: Expected O, but got Unknown
		//IL_1356: Unknown result type (might be due to invalid IL or missing references)
		//IL_1360: Expected O, but got Unknown
		//IL_18bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c9: Expected O, but got Unknown
		//IL_18d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e0: Expected O, but got Unknown
		//IL_18ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Expected O, but got Unknown
		//IL_1d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d63: Expected O, but got Unknown
		//IL_1d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7a: Expected O, but got Unknown
		//IL_20ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2109: Expected O, but got Unknown
		//IL_2116: Unknown result type (might be due to invalid IL or missing references)
		//IL_2120: Expected O, but got Unknown
		//IL_830e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8318: Expected O, but got Unknown
		//IL_833c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8346: Expected O, but got Unknown
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdConsupmtionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
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
		Appearance appearance124 = new Appearance();
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
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdConsumptionHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseInput = new System.Windows.Forms.Button();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.BtnUpdateQtyByBom = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabOutput = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdOutPutByProduct = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseOutPut = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.tabConsumption = new System.Windows.Forms.TabPage();
		this.grdConsupmtionDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseInConsumption = new System.Windows.Forms.Button();
		this.label33 = new System.Windows.Forms.Label();
		this.tabOH = new System.Windows.Forms.TabPage();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BtnAddDetail = new System.Windows.Forms.Button();
		this.cmbItemBOM = new UltraCombo();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbBOMNo = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbWarehouseName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.btnClearInput = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.brnShift = new System.Windows.Forms.Button();
		this.CmbShift = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbProductionType = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbPlantName = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.BtnLoadInputFromBom = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtOutPutAutoRatePerQtyAmount = new System.Windows.Forms.TextBox();
		this.txtTotalOutPutAutoRatedAmount = new System.Windows.Forms.TextBox();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.txtWagesAmount = new System.Windows.Forms.TextBox();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.datTodate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnGenerateRatesOutput = new System.Windows.Forms.ToolStripButton();
		this.BtnFormulasOnForm = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRecordsUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnReProcessing = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.chkPrintVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreviewSlip = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdConsumptionHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbShiftNameHistory = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbProductionTypeHistory = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbProductionNoHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.cmbDateTypeHistory = new UltraCombo();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.label30 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabOutput.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).BeginInit();
		this.panel11.SuspendLayout();
		this.tabConsumption.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdConsupmtionDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.tabOH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).BeginInit();
		this.panel4.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdConsumptionHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShiftNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1426, 733);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1426, 733);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1418, 704);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel6);
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 224);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1412, 477);
		this.panel17.TabIndex = 4;
		this.panel6.Controls.Add(this.BtnUpdateWarehouseInput);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(664, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(748, 477);
		this.panel6.TabIndex = 4;
		this.BtnUpdateWarehouseInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseInput.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseInput.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseInput.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseInput.Location = new System.Drawing.Point(518, 3);
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
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(748, 451);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 0;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.BtnUpdateQtyByBom);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(748, 26);
		this.panel7.TabIndex = 3;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(700, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(46, 25);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.BtnUpdateQtyByBom.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateQtyByBom.FlatAppearance.BorderSize = 0;
		this.BtnUpdateQtyByBom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateQtyByBom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateQtyByBom.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateQtyByBom.Location = new System.Drawing.Point(169, 2);
		this.BtnUpdateQtyByBom.Name = "BtnUpdateQtyByBom";
		this.BtnUpdateQtyByBom.Size = new System.Drawing.Size(190, 23);
		this.BtnUpdateQtyByBom.TabIndex = 3;
		this.BtnUpdateQtyByBom.Text = "Update Qty if Single BOM";
		this.BtnUpdateQtyByBom.UseVisualStyleBackColor = false;
		this.BtnUpdateQtyByBom.Click += new System.EventHandler(BtnUpdateQtyByBom_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(165, 16);
		this.label1.TabIndex = 126;
		this.label1.Text = "Input (Consumed Items)";
		this.tabControl2.Controls.Add(this.tabOutput);
		this.tabControl2.Controls.Add(this.tabConsumption);
		this.tabControl2.Controls.Add(this.tabOH);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Left;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(664, 477);
		this.tabControl2.TabIndex = 6;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabOutput.Controls.Add(this.panel10);
		this.tabOutput.Location = new System.Drawing.Point(4, 25);
		this.tabOutput.Name = "tabOutput";
		this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
		this.tabOutput.Size = new System.Drawing.Size(656, 448);
		this.tabOutput.TabIndex = 0;
		this.tabOutput.Text = "Output";
		this.tabOutput.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutByProduct);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(650, 442);
		this.panel10.TabIndex = 3;
		((ScrollableBase)this.grdOutPutByProduct).BackColor = System.Drawing.Color.White;
		grdOutPutByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutByProduct.DesignTimeLayout = grdOutPutByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Name = "grdOutPutByProduct";
		((ScrollableBase)this.grdOutPutByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Size = new System.Drawing.Size(650, 415);
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
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(650, 27);
		this.panel11.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseOutPut.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseOutPut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseOutPut.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseOutPut.Location = new System.Drawing.Point(414, 2);
		this.BtnUpdateWarehouseOutPut.Name = "BtnUpdateWarehouseOutPut";
		this.BtnUpdateWarehouseOutPut.Size = new System.Drawing.Size(185, 24);
		this.BtnUpdateWarehouseOutPut.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Text = "Update Same WareHouse In All Rows";
		this.BtnUpdateWarehouseOutPut.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseOutPut.Click += new System.EventHandler(BtnUpdateWarehouseOutPut_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(602, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOutPutByProduct;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(170, 16);
		this.label23.TabIndex = 126;
		this.label23.Text = "OutPut (Produced Items)";
		this.tabConsumption.Controls.Add((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail);
		this.tabConsumption.Controls.Add(this.panel5);
		this.tabConsumption.Location = new System.Drawing.Point(4, 25);
		this.tabConsumption.Name = "tabConsumption";
		this.tabConsumption.Padding = new System.Windows.Forms.Padding(3);
		this.tabConsumption.Size = new System.Drawing.Size(656, 448);
		this.tabConsumption.TabIndex = 1;
		this.tabConsumption.Text = "Consumption";
		this.tabConsumption.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdConsupmtionDetail).BackColor = System.Drawing.Color.White;
		grdConsupmtionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdConsupmtionDetail.DesignTimeLayout = grdConsupmtionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdConsupmtionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).Name = "grdConsupmtionDetail";
		((ScrollableBase)this.grdConsupmtionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).Size = new System.Drawing.Size(650, 416);
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).TabIndex = 0;
		this.grdConsupmtionDetail.TotalRow = (InheritableBoolean)1;
		this.grdConsupmtionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdConsupmtionDetail).VisualStyle = (VisualStyle)4;
		this.grdConsupmtionDetail.CellUpdated += new ColumnActionEventHandler(grdConsumption_CellUpdated);
		this.grdConsupmtionDetail.ColumnButtonClick += new ColumnActionEventHandler(grdConsumption_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdConsupmtionDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdConsumption_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.BtnUpdateWarehouseInConsumption);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(650, 26);
		this.panel5.TabIndex = 13;
		this.BtnUpdateWarehouseInConsumption.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseInConsumption.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseInConsumption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseInConsumption.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseInConsumption.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseInConsumption.Location = new System.Drawing.Point(466, 0);
		this.BtnUpdateWarehouseInConsumption.Name = "BtnUpdateWarehouseInConsumption";
		this.BtnUpdateWarehouseInConsumption.Size = new System.Drawing.Size(180, 25);
		this.BtnUpdateWarehouseInConsumption.TabIndex = 158;
		this.BtnUpdateWarehouseInConsumption.Text = "Update Same Warehouse";
		this.BtnUpdateWarehouseInConsumption.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseInConsumption.Click += new System.EventHandler(BtnUpdateWarehouseInConsumption_Click);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(3, 7);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(131, 16);
		this.label33.TabIndex = 126;
		this.label33.Text = "Consupmtion Detail";
		this.tabOH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.tabOH.Controls.Add(this.panel14);
		this.tabOH.Location = new System.Drawing.Point(4, 25);
		this.tabOH.Name = "tabOH";
		this.tabOH.Size = new System.Drawing.Size(656, 448);
		this.tabOH.TabIndex = 2;
		this.tabOH.Text = "OverHeads";
		this.tabOH.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(656, 426);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 1;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label22);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(656, 22);
		this.panel14.TabIndex = 12;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(3, 3);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
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
		this.panel2.Location = new System.Drawing.Point(3, 53);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1412, 171);
		this.panel2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.BtnAddDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemBOM);
		this.groupBox3.Controls.Add(this.BtnCancelDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateDetail);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBOMNo);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.txtQty);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseName);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 102);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(740, 64);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.BtnAddDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnAddDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnAddDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddDetail.Location = new System.Drawing.Point(664, 34);
		this.BtnAddDetail.Name = "BtnAddDetail";
		this.BtnAddDetail.Size = new System.Drawing.Size(35, 23);
		this.BtnAddDetail.TabIndex = 5;
		this.BtnAddDetail.Text = "+";
		this.BtnAddDetail.UseVisualStyleBackColor = false;
		this.BtnAddDetail.Click += new System.EventHandler(BtnAddDetail_Click);
		this.cmbItemBOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemBOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemBOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemBOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemBOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Location = new System.Drawing.Point(195, 31);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Name = "cmbItemBOM";
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Leave += new System.EventHandler(cmbItem_Leave);
		this.BtnCancelDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(664, 10);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(69, 21);
		this.BtnCancelDetail.TabIndex = 7;
		this.BtnCancelDetail.Text = "Cancel";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(664, 34);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(69, 23);
		this.BtnUpdateDetail.TabIndex = 6;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(195, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item Name";
		this.cmbBOMNo.AllowNull = (DefaultableBoolean)1;
		this.cmbBOMNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBOMNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBOMNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBOMNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Location = new System.Drawing.Point(373, 31);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Name = "cmbBOMNo";
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Leave += new System.EventHandler(cmbBOMNo_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(373, 15);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "BOM No";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(516, 31);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 3;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(516, 15);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Enabled = false;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(595, 33);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(65, 23);
		this.txtQty.TabIndex = 4;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(595, 15);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.CmbWarehouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Name = "CmbWarehouseName";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).TabIndex = 0;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(3, 15);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(102, 15);
		this.label41.TabIndex = 85;
		this.label41.Text = "Warehouse Name";
		this.btnClearInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnClearInput.FlatAppearance.BorderSize = 0;
		this.btnClearInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearInput.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnClearInput.ForeColor = System.Drawing.Color.White;
		this.btnClearInput.Location = new System.Drawing.Point(773, 141);
		this.btnClearInput.Name = "btnClearInput";
		this.btnClearInput.Size = new System.Drawing.Size(90, 23);
		this.btnClearInput.TabIndex = 3;
		this.btnClearInput.Text = "Clear Input";
		this.btnClearInput.UseVisualStyleBackColor = false;
		this.btnClearInput.Click += new System.EventHandler(btnClearInput_Click);
		this.groupBox2.Controls.Add(this.brnShift);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShift);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantName);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtProductionNo);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(860, 101);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.brnShift.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.brnShift.Location = new System.Drawing.Point(276, 71);
		this.brnShift.Name = "brnShift";
		this.brnShift.Size = new System.Drawing.Size(23, 26);
		this.brnShift.TabIndex = 4;
		this.brnShift.TabStop = false;
		this.brnShift.UseVisualStyleBackColor = true;
		this.brnShift.Click += new System.EventHandler(brnShift_Click);
		this.CmbShift.AllowNull = (DefaultableBoolean)1;
		this.CmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShift).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShift).Location = new System.Drawing.Point(98, 71);
		((System.Windows.Forms.Control)(object)this.CmbShift).Name = "CmbShift";
		((System.Windows.Forms.Control)(object)this.CmbShift).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbShift).TabIndex = 3;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(5, 77);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 142;
		this.label29.Text = "Shift Name";
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(98, 44);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Leave += new System.EventHandler(CmbProductionType_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(94, 15);
		this.label20.TabIndex = 140;
		this.label20.Text = "Production Type";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(5, 23);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(88, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No && Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(166, 19);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(132, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(98, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(66, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Location = new System.Drawing.Point(651, 17);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Name = "CmbPlantName";
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).TabIndex = 5;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(577, 23);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(69, 15);
		this.label26.TabIndex = 85;
		this.label26.Text = "Plant Name";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(387, 44);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(470, 53);
		this.txtRemarksHeader.TabIndex = 6;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(387, 19);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(178, 23);
		this.txtProductionNo.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(302, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(85, 15);
		this.label8.TabIndex = 89;
		this.label8.Text = "Production No";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(302, 49);
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
		this.BtnLoadInputFromBom.Location = new System.Drawing.Point(773, 101);
		this.BtnLoadInputFromBom.Name = "BtnLoadInputFromBom";
		this.BtnLoadInputFromBom.Size = new System.Drawing.Size(90, 37);
		this.BtnLoadInputFromBom.TabIndex = 2;
		this.BtnLoadInputFromBom.Text = "Load Input From BOM";
		this.BtnLoadInputFromBom.UseVisualStyleBackColor = false;
		this.BtnLoadInputFromBom.Click += new System.EventHandler(BtnLoadInputFromBom_Click);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.txtOutPutAutoRatePerQtyAmount);
		this.panel4.Controls.Add(this.txtTotalOutPutAutoRatedAmount);
		this.panel4.Controls.Add(this.txtTotalInputs);
		this.panel4.Controls.Add(this.txtWagesAmount);
		this.panel4.Controls.Add(this.txtOverHead);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Location = new System.Drawing.Point(866, 1);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(349, 165);
		this.panel4.TabIndex = 2;
		this.txtOutPutAutoRatePerQtyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutPutAutoRatePerQtyAmount.Enabled = false;
		this.txtOutPutAutoRatePerQtyAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOutPutAutoRatePerQtyAmount.Location = new System.Drawing.Point(195, 136);
		this.txtOutPutAutoRatePerQtyAmount.Name = "txtOutPutAutoRatePerQtyAmount";
		this.txtOutPutAutoRatePerQtyAmount.ReadOnly = true;
		this.txtOutPutAutoRatePerQtyAmount.Size = new System.Drawing.Size(149, 25);
		this.txtOutPutAutoRatePerQtyAmount.TabIndex = 5;
		this.txtOutPutAutoRatePerQtyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalOutPutAutoRatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalOutPutAutoRatedAmount.Enabled = false;
		this.txtTotalOutPutAutoRatedAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalOutPutAutoRatedAmount.Location = new System.Drawing.Point(195, 109);
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
		this.label31.Location = new System.Drawing.Point(3, 139);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(177, 17);
		this.label31.TabIndex = 97;
		this.label31.Text = "(Produced Item) Value / Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 112);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(175, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Total (Produced Item) Value";
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label5);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(347, 23);
		this.panel20.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(141, 16);
		this.label5.TabIndex = 33;
		this.label5.Text = "Calculation Summery";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 83);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(159, 17);
		this.label13.TabIndex = 95;
		this.label13.Text = "Total Consumption Value";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(148, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Total Over Heads Value";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(115, 97);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(15, 16);
		this.label25.TabIndex = 93;
		this.label25.Text = "=";
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
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
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
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
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
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
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
		this.panel3.Controls.Add(this.datTodate);
		this.panel3.Controls.Add(this.datFromDate);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.chkPrintVoucher);
		this.panel3.Controls.Add(this.ChkPreviewSlip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1412, 50);
		this.panel3.TabIndex = 0;
		this.datTodate.CustomFormat = "dd-MMM-yy";
		this.datTodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datTodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datTodate.Location = new System.Drawing.Point(955, 28);
		this.datTodate.Name = "datTodate";
		this.datTodate.Size = new System.Drawing.Size(122, 23);
		this.datTodate.TabIndex = 142;
		this.datTodate.Visible = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(833, 28);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(122, 23);
		this.datFromDate.TabIndex = 141;
		this.datFromDate.Visible = false;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnsave, this.BtnSaveAs, this.btnUpdate, this.toolStripSeparator, this.btnVoucherReport, this.btnPrint, this.toolStripSeparator5,
			this.btnattachment, this.btnshortcutkeys, this.toolStripSeparator2, this.btnGenerateRatesOutput, this.BtnFormulasOnForm, this.btnDelete, this.btnRecordsUpdate, this.btnReProcessing
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1412, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(79, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(62, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.BtnSaveAs.Font = new System.Drawing.Font("Verdana", 9f);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(77, 24);
		this.BtnSaveAs.Text = "&SaveAs";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(77, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(110, 24);
		this.btnVoucherReport.Text = "103-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(97, 24);
		this.btnPrint.Text = "1122-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(103, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(121, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnGenerateRatesOutput.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnGenerateRatesOutput.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.btnGenerateRatesOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRatesOutput.Name = "btnGenerateRatesOutput";
		this.btnGenerateRatesOutput.Size = new System.Drawing.Size(319, 24);
		this.btnGenerateRatesOutput.Text = "Update Input,Consumption.OH && Output Rate";
		this.btnGenerateRatesOutput.Click += new System.EventHandler(btnGenerateRatesOutput_Click);
		this.BtnFormulasOnForm.Font = new System.Drawing.Font("Verdana", 9f);
		this.BtnFormulasOnForm.Image = Architecture.WinApp.Properties.Resources.Fixed_Assets;
		this.BtnFormulasOnForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnFormulasOnForm.Name = "BtnFormulasOnForm";
		this.BtnFormulasOnForm.Size = new System.Drawing.Size(88, 24);
		this.BtnFormulasOnForm.Text = "&Formulas";
		this.BtnFormulasOnForm.Click += new System.EventHandler(BtnFormulasOnForm_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(72, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRecordsUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(90, 24);
		this.btnRecordsUpdate.Text = "Auto Update";
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnAutoUpdate_Click);
		this.btnReProcessing.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReProcessing.Name = "btnReProcessing";
		this.btnReProcessing.Size = new System.Drawing.Size(183, 24);
		this.btnReProcessing.Text = "Auto Update Re-Processing";
		this.btnReProcessing.Visible = false;
		this.btnReProcessing.Click += new System.EventHandler(btnReProcessing_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 28);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(161, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Production Manual";
		this.chkPrintVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintVoucher.AutoSize = true;
		this.chkPrintVoucher.Checked = true;
		this.chkPrintVoucher.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintVoucher.ForeColor = System.Drawing.Color.White;
		this.chkPrintVoucher.Location = new System.Drawing.Point(1281, 29);
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
		this.ChkPreviewSlip.Location = new System.Drawing.Point(1189, 29);
		this.ChkPreviewSlip.Name = "ChkPreviewSlip";
		this.ChkPreviewSlip.Size = new System.Drawing.Size(90, 20);
		this.ChkPreviewSlip.TabIndex = 16;
		this.ChkPreviewSlip.Text = "Slip Print";
		this.ChkPreviewSlip.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1418, 704);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.tabControl3);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 354);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1418, 350);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(650, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(768, 350);
		this.panel8.TabIndex = 50;
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(768, 324);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.ctrlGrdBar7);
		this.panel44.Controls.Add(this.label40);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(768, 26);
		this.panel44.TabIndex = 49;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(4, 4);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(165, 16);
		this.label40.TabIndex = 127;
		this.label40.Text = "Input (Consumed Items)";
		this.tabControl3.Controls.Add(this.tabPage2);
		this.tabControl3.Controls.Add(this.tabPage4);
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Left;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(650, 350);
		this.tabControl3.TabIndex = 57;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(642, 321);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "OutPut (Produced Items)";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(636, 290);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 0;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar4);
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(636, 25);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(170, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut (Produced Items)";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdConsumptionHistory);
		this.tabPage4.Controls.Add(this.panel18);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(642, 321);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Consumption";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdConsumptionHistory.AllowEdit = (InheritableBoolean)2;
		grdConsumptionHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdConsumptionHistory.DesignTimeLayout = grdConsumptionHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdConsumptionHistory.DynamicFiltering = true;
		this.grdConsumptionHistory.FilterMode = (FilterMode)2;
		this.grdConsumptionHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdConsumptionHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).Name = "grdConsumptionHistory";
		((ScrollableBase)this.grdConsumptionHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).Size = new System.Drawing.Size(636, 290);
		((System.Windows.Forms.Control)(object)this.grdConsumptionHistory).TabIndex = 0;
		this.grdConsumptionHistory.TotalRow = (InheritableBoolean)1;
		this.grdConsumptionHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdConsumptionHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar5);
		this.panel18.Controls.Add(this.label14);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(636, 25);
		this.panel18.TabIndex = 55;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(4, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(131, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Consupmtion Detail";
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.tabPage5.Controls.Add(this.panel42);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(642, 321);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Overheads";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(642, 297);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 2;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.ctrlGrdBar6);
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 0);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(642, 24);
		this.panel42.TabIndex = 58;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(4, 3);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(78, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 138);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1418, 216);
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
		this.panel15.Location = new System.Drawing.Point(0, 115);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1418, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1371, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(5, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(54, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 51);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1418, 64);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1185, 60);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbShiftNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbShiftNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShiftNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShiftNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShiftNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShiftNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory).Location = new System.Drawing.Point(875, 31);
		((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory).Name = "CmbShiftNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbShiftNameHistory).TabIndex = 7;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(875, 13);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 182;
		this.label36.Text = "Shift Name";
		this.CmbProductionTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Location = new System.Drawing.Point(702, 31);
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Name = "CmbProductionTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).TabIndex = 6;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(703, 13);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(94, 15);
		this.label27.TabIndex = 180;
		this.label27.Text = "Production Type";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1027, 32);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbProductionNoHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Location = new System.Drawing.Point(535, 31);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Name = "cmbProductionNoHistory";
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).TabIndex = 5;
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(471, 33);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(268, 33);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(139, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(128, 33);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(139, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(1086, 43);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(91, 17);
		this.RadNotReffered.TabIndex = 11;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadNotReffered.Visible = false;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(1086, 26);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(69, 17);
		this.RadReffered.TabIndex = 10;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadReffered.Visible = false;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(1086, 9);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(38, 17);
		this.RadAll.TabIndex = 9;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadAll.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(536, 13);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Production No";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 13);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(271, 13);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 15);
		this.label12.TabIndex = 171;
		this.label12.Text = "Date To";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(129, 13);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(472, 13);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(45, 15);
		this.label16.TabIndex = 173;
		this.label16.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(408, 13);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(408, 33);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(62, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1418, 51);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewHistory, this.toolStripSeparator1, this.btnRefreshHistory, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1418, 27);
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
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(79, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
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
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(596, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(12, 7, 12, 7);
		this.ctrlGrdBar4.MyGrid = this.grdHistoryOutput;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 129;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(596, 2);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(16, 9, 16, 9);
		this.ctrlGrdBar5.MyGrid = this.grdConsumptionHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar5.TabIndex = 130;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(603, 1);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(16, 9, 16, 9);
		this.ctrlGrdBar6.MyGrid = this.grdHistoryOverHead;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar6.TabIndex = 130;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(727, 2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(16, 9, 16, 9);
		this.ctrlGrdBar7.MyGrid = this.grdHistoryInput;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 130;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1426, 733);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionManualFeed";
		this.Text = "Production";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductionFeed_FormClosed);
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
		this.tabControl2.ResumeLayout(false);
		this.tabOutput.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabConsumption.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdConsupmtionDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabOH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
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
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.tabControl3.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdConsumptionHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShiftNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
