using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.Steel.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.Model.Steel.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Production;

public class FoodProductionWithValuesNew_St : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int[] AryColWidth;

	private int RecId;

	private int RecOverHeadId;

	private int RecpackingMaterial;

	private int RecIdOutPut;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private int updateDetailIndexInput;

	private int updateDetailIndexPM;

	private int updateDetailIndexOH;

	private double AvgRate;

	private double AvgRateWithoutExp;

	private double FinishGoodsNetWeight;

	private double InputNetWeight;

	private double ByProductNetWeight;

	private double DiffNetWeight;

	private double ConsumedWeight;

	private double PreWeight;

	public static bool ValuesShowRights;

	private DataTable dtJob = new DataTable();

	private DataTable dtwarehouse = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable tableInput = new DataTable();

	private DataTable dtuomlst = new DataTable();

	private DataTable dtSummerReport = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable tableOverHeads = new DataTable();

	private DataTable dtEntryType = new DataTable();

	private DataTable OverHeadHistorydt = new DataTable();

	private DataTable dtStockAc = new DataTable();

	private DataTable dtpackingtype = new DataTable();

	private DataTable dtware = new DataTable();

	private DataTable dtjoblot = new DataTable();

	private DataTable dtUomOH = new DataTable();

	private LoadInputDataByJobOrder LoadInput;

	private LoadOutPutDataByJobOrder LoadOutPut;

	private LoadOverHeadsDataByJobOrder LoadOverheads;

	private List<InvFoodProductionDetail> RemovedOutputList = new List<InvFoodProductionDetail>();

	private List<InvFoodProductionDetail> RemovedInputList = new List<InvFoodProductionDetail>();

	private List<InvFoodProductionOverHeads> RemovedOverheadList = new List<InvFoodProductionOverHeads>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage Summary;

	private Panel panel18;

	private Panel panel21;

	private Panel panel19;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator2;

	private TabPage InPut;

	private Panel panel5;

	internal GridEX grdStockConversionProductionDetail;

	private DateTimePicker txtDocdate;

	private Label label1;

	private TextBox txtdocnumber;

	private TextBox txtRemarksInput;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private TabPage TransctionHistory;

	private Panel panel10;

	private Panel panel13;

	private Panel panel17;

	internal GridEX grdDetail;

	private Panel panel16;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Button btnSearch;

	private Label label50;

	private TextBox txtDocNumberTo;

	private DateTimePicker txtToDate;

	private Label label52;

	private DateTimePicker txtfromDate;

	private Label label48;

	private Label label49;

	private TextBox txtDocNoFrom;

	private Panel panel11;

	private TabPage OverHead;

	private DateTimePicker txtDocDateOverHead;

	private Label label56;

	private TextBox txtDocNoOverHead;

	private ToolStrip toolStrip5;

	private ToolStripButton btnNewOverHead;

	private ToolStripButton btnSaveOverHead;

	private ToolStripButton btnUpdateOverHead;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnPrintOverHead;

	internal GridEX grdOverHeadHistory;

	internal UltraCombo cmbJobOrderOverHead;

	private Label label39;

	internal UltraCombo cmbsummeryJobOrderNo;

	private Label label68;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TabPage OutPut;

	private Panel panel3;

	private DateTimePicker DocDateOutPut;

	private Label label16;

	private Label label19;

	private TextBox txtDocNoOutPut;

	private TextBox txtRemarksHeaderOutPut;

	private ToolStrip toolStrip6;

	private ToolStripButton btnNewOutPut;

	private ToolStripButton btnSaveOutPut;

	private ToolStripButton btnUpdateOutPut;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnPrintOutPut;

	internal GridEX grdByProduct;

	private Button btnLoadInvoices;

	private Label label23;

	internal UltraCombo CmbJobOrderNoInput;

	private Label label60;

	internal UltraCombo CmbJobOrderNoOutput;

	private Label label27;

	internal UltraCombo CmbWorkInProcess;

	private Label label77;

	internal UltraCombo CmbWIPItem;

	private ToolStripButton btnVoucherReport;

	private ToolStripButton btnVoucherOutPut;

	private ToolStripButton btnVoucherOH;

	private TextBox txtFGWeight;

	private Label label75;

	private TextBox txtFGRate;

	private Label label76;

	internal GridEX grdSummery;

	private Button btnApplyRate;

	private ToolStripButton btnProductionRecoveryReport;

	private Panel panel26;

	private Panel panel23;

	private Panel panel20;

	internal GridEX grdSummeryReport;

	private Panel panel28;

	private Panel panel30;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar6;

	private ToolStripButton btnJobOrderDefine;

	private TextBox txtAvgFinishGoodRate;

	private ToolStripButton btnRefresh;

	private TabPage Settlement;

	private Panel panel29;

	private Panel panel36;

	private Panel panel37;

	private Panel panel35;

	private Panel panel34;

	private Panel panel33;

	private Panel panel32;

	private Label label8;

	private Label label7;

	private Label label5;

	internal GridEX grdOverHeadSettlement;

	internal GridEX grdOutPutSettlement;

	internal GridEX grdInputsettlement;

	private Panel panel40;

	private Button btnGeneralSettlement;

	internal UltraCombo CmbJobOrderSettlement;

	private Label label12;

	private Button btnResetSettlement;

	private Label label18;

	private TextBox txtNetWeightRM;

	private Label label17;

	private Label label14;

	private TextBox txtItemQtyRM;

	private Label label51;

	private Label label21;

	private Label label22;

	private TextBox txtAmountBP;

	private TextBox txtAmountFG;

	private TextBox txtAmountRM;

	private TextBox txtNetWeightFG;

	private TextBox txtNetWeightBP;

	private Label label25;

	private Label label31;

	private Label label28;

	private TextBox txtItemQtyBP;

	private TextBox txtAvgRateFG;

	private TextBox txtItemQtyFG;

	private TextBox txtOverHeadsSettlement;

	private Button btnApplyAvgRateFG;

	private Button btnUpdateSettlement;

	private Label label54;

	private Label label53;

	private TextBox txtInPutTotalQty;

	private TextBox txtInPutTotalWeight;

	private ToolStripButton btnRefreshOutPut;

	private Button btnRecoveryReport;

	private Label label58;

	private TextBox txtFinishGoodRateWithOutExp;

	private Button btnApproveSettlement;

	private TextBox txtBPTotalAmountWithExpenses;

	private TextBox txtBPPMCharges;

	private Label label59;

	private TextBox txtBPOverHead;

	private Label label73;

	private Label label63;

	private Label label71;

	private Label label62;

	private TextBox txtBalQtyTotal;

	private TextBox txtOutQtyTotal;

	private TextBox txtBalWeightTotal;

	private TextBox txtOutWeightTotal;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private TabPage tabPage4;

	private TabControl tabControl3;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdinputhistory;

	private ToolStrip toolStrip8;

	private ToolStripButton toolStripButton9;

	internal GridEX grdOutputHistory;

	internal UltraCombo CmbPlantFeederInput;

	private Label label83;

	internal UltraCombo CmbPlantFeederOverHead;

	private Label label85;

	internal UltraCombo CmbPlantFeederOutput;

	private Label label86;

	private ToolStripButton btnRefreshOverHead;

	private ToolStripButton btnRefreshSummary;

	internal UltraCombo CmbPlantNameSummary;

	private Label label87;

	private Button btnShow;

	private GroupBox groupBox1;

	private Panel panel14;

	private Panel panel12;

	private ToolStrip toolStrip7;

	private ToolStripButton toolStripButton4;

	private GroupBox groupBox3;

	private Panel panel24;

	private Panel panel22;

	private Label label2;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar7;

	private GroupBox groupBox6;

	private Label label89;

	private CtrlGrdBar ctrlGrdBar9;

	private Panel panel44;

	private Panel panel43;

	private Label label91;

	private Label label90;

	private Panel panel48;

	private Panel panel47;

	private Panel panel46;

	private Panel panel45;

	private Label label93;

	private Label label92;

	private ToolStripSeparator toolStripSeparator1;

	private Panel panel50;

	private Panel panel49;

	private Panel panel53;

	private Panel panel54;

	private Label label94;

	private Panel panel51;

	private Panel panel52;

	private Panel panel55;

	private Panel panel56;

	private Label label26;

	private Panel panel57;

	private Panel panel59;

	internal GridEX grdInputDetail;

	private Panel panel58;

	private Panel panel60;

	private Panel panel62;

	internal GridEX grdOutPutDetail;

	private Panel panel61;

	private CtrlGrdBar ctrlGrdBar10;

	private CtrlGrdBar ctrlGrdBar11;

	private Panel panel63;

	private Label label30;

	private Panel panel64;

	private Label label32;

	private Panel panel67;

	private TabControl tabControl4;

	private TabPage tabPage2;

	private Panel panel68;

	private Panel panel69;

	private Label label44;

	private TabPage tabPage3;

	private Panel panel9;

	private Panel panel8;

	private Panel panel31;

	private Panel panel25;

	private Label label38;

	private TextBox txtOutputQtyTotalOH;

	private Label label46;

	private TextBox txtOutputWeightTotalOH;

	internal GridEX grdDetailOverhead;

	private CtrlGrdBar ctrlGrdBar12;

	private Label label47;

	private Label label97;

	private TextBox txtInputQtyTotalOH;

	private Label label98;

	private TextBox txtInputWeightTotalOH;

	internal GridEX grddetailOhHistory;

	private Panel panel27;

	private CtrlGrdBar ctrlGrdBar14;

	private Label label34;

	private Label label101;

	private Label label102;

	private TextBox txtfinishQtyOh;

	private Label label103;

	private TextBox txtfinishWeightOh;

	private ToolStripSeparator toolStripSeparator6;

	private Panel panel72;

	private ToolStripButton btnLoadOutPutFromjobOrder;

	private ToolStripButton btnLoadOverHeadsFromJobOrder;

	private Label label6;

	private Panel panel74;

	private TextBox txtDifferenceAmount;

	private Label label11;

	private Panel panel75;

	private Label label3;

	private TextBox txtFinishAmountAgainstJobOrder;

	private TextBox txtActualFinishGoodsAmount;

	private Label label4;

	private Label label9;

	internal UltraCombo CmbDifferenceAccountTitle;

	private Label label13;

	private Button btnSettlementVoucher;

	private ToolStripButton btnLoadAllOutputHistoy;

	public FoodProductionWithValuesNew_St(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFoodProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1090, 780);
			dtUomOH = CommonServices.GetStaticUOM();
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveOutPut.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnUpdateOutPut.Enabled = formright.DoHaveUpdateRights;
			Print.Enabled = formright.DoHavePrintRights;
			btnVoucherReport.Enabled = formright.DoHavePrintRights;
			btnVoucherOutPut.Enabled = formright.DoHavePrintRights;
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
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar3.mGridChooseFielder.Enabled = false;
				ctrlGrdBar4.mGridChooseFielder.Enabled = false;
			}
			if (!formright.DoHaveCanSettlementtab)
			{
				tabControl1.TabPages.Remove(Settlement);
				tabControl1.TabPages.Remove(OverHead);
			}
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("JobLotDescription");
			dtwarehouse.Columns.Add("Id");
			dtwarehouse.Columns.Add("WareHouseName");
			dtpacktype.Columns.Add("Id");
			dtpacktype.Columns.Add("PackingType");
			tableInput.Columns.Add("Id", typeof(int));
			tableInput.Columns.Add("JobOrderInPutId", typeof(int));
			tableInput.Columns.Add("RefDocumentTypeId", typeof(int));
			tableInput.Columns.Add("RefDocIdNo", typeof(int));
			tableInput.Columns.Add("RefDocSubIdNo", typeof(int));
			tableInput.Columns.Add("WareHouseId", typeof(int));
			tableInput.Columns.Add("WareHouse", typeof(string));
			tableInput.Columns.Add("JobLotId", typeof(int));
			tableInput.Columns.Add("JobLot", typeof(string));
			tableInput.Columns.Add("ItemId", typeof(int));
			tableInput.Columns.Add("Item", typeof(string));
			tableInput.Columns.Add("PackingTypeId", typeof(int));
			tableInput.Columns.Add("PackingType", typeof(string));
			tableInput.Columns.Add("PackUomId", typeof(int));
			tableInput.Columns.Add("PackUom", typeof(string));
			tableInput.Columns.Add("PackEquivalent", typeof(double));
			tableInput.Columns.Add("ItemQty", typeof(double));
			tableInput.Columns.Add("NetWeight", typeof(double));
			tableInput.Columns.Add("ItemRate", typeof(double));
			tableInput.Columns.Add("RateUomId", typeof(int));
			tableInput.Columns.Add("RateUom", typeof(string));
			tableInput.Columns.Add("RateEquivalent", typeof(double));
			tableInput.Columns.Add("ItemAmount", typeof(decimal));
			tableInput.Columns.Add("Remarks", typeof(string));
			tableByProduct.Columns.Add("Id", typeof(int));
			tableByProduct.Columns.Add("OutPutIdFromJobOrder", typeof(int));
			tableByProduct.Columns.Add("EntryTypeId", typeof(int));
			tableByProduct.Columns.Add("EntryType", typeof(string));
			tableByProduct.Columns.Add("WareHouseId", typeof(int));
			tableByProduct.Columns.Add("JobLotId", typeof(int));
			tableByProduct.Columns.Add("PackingTypeId", typeof(int));
			tableByProduct.Columns.Add("ItemId", typeof(int));
			tableByProduct.Columns.Add("Item", typeof(string));
			tableByProduct.Columns.Add("PackUomId", typeof(int));
			tableByProduct.Columns.Add("PackUom", typeof(string));
			tableByProduct.Columns.Add("PackEquivalent", typeof(double));
			tableByProduct.Columns.Add("ItemQty", typeof(double));
			tableByProduct.Columns.Add("NetWeight", typeof(double));
			tableByProduct.Columns.Add("ItemRate", typeof(double));
			tableByProduct.Columns.Add("RateUomId", typeof(int));
			tableByProduct.Columns.Add("RateUom", typeof(string));
			tableByProduct.Columns.Add("RateEquivalent", typeof(double));
			tableByProduct.Columns.Add("ItemAmount", typeof(double));
			tableByProduct.Columns.Add("NetRate", typeof(double));
			tableByProduct.Columns.Add("NetAmount", typeof(double));
			tableByProduct.Columns.Add("Remarks", typeof(string));
			tableOverHeads.Columns.Add("Id", typeof(int));
			tableOverHeads.Columns.Add("JobOrderOverHeadId");
			tableOverHeads.Columns.Add("DocDate", typeof(DateTime));
			tableOverHeads.Columns.Add("DocNo", typeof(int));
			tableOverHeads.Columns.Add("InvProductionJobOrderId", typeof(int));
			tableOverHeads.Columns.Add("ChartOfAccountId", typeof(int));
			tableOverHeads.Columns.Add("ChartOfAccount", typeof(string));
			tableOverHeads.Columns.Add("BrandId", typeof(int));
			tableOverHeads.Columns.Add("Brand", typeof(string));
			tableOverHeads.Columns.Add("BrandUomId", typeof(int));
			tableOverHeads.Columns.Add("BrandUom", typeof(string));
			tableOverHeads.Columns.Add("ItemQty", typeof(double));
			tableOverHeads.Columns.Add("ItemRate", typeof(double));
			tableOverHeads.Columns.Add("Amount", typeof(double));
			tableOverHeads.Columns.Add("Remarks", typeof(string));
			grdDetailOverhead.DataSource = tableOverHeads;
			grdDetailOverhead.RetrieveStructure();
			grdDetailOverheadSettings();
			EntryTypeFill();
			stockAccoutnFill();
			UOMFill();
			combojoblotfill();
			Warehouse();
			WarehouseInputFill();
			bagType();
			JobOrderNoFill();
			JobOrderNoFillForSummery();
			GenerateDocNumberInput();
			if (!formright.DoHaveCanRateandAmount)
			{
				btnVoucherReport.Visible = false;
				btnVoucherOutPut.Visible = false;
				btnVoucherOH.Visible = false;
			}
			txtDocdate.Focus();
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
			dtware = CommonServices.getActiveWareHouse();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void stockAccoutnFill()
	{
		try
		{
			dtStockAc.Rows.Clear();
			dtStockAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12");
			if (dtStockAc != null && dtStockAc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtStockAc, CmbDifferenceAccountTitle, "Id", "AccountTitle", "Difference Account", false);
				((UltraGridBase)CmbDifferenceAccountTitle).DisplayLayout.Bands[0].Columns[1].Width = 450;
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
			dtjoblot = CommonServices.JobLotGetAllService();
			dtJob.Clear();
			for (int i = 0; i < dtjoblot.Rows.Count; i++)
			{
				dtJob.Rows.Add(dtjoblot.Rows[i]["Id"], dtjoblot.Rows[i]["JobLotDescription"]);
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
			dtpackingtype = InvPackingType.Getall();
			dtpacktype.Clear();
			for (int i = 0; i < dtpackingtype.Rows.Count; i++)
			{
				dtpacktype.Rows.Add(dtpackingtype.Rows[i]["Id"], dtpackingtype.Rows[i]["PackTypeDesc"]);
			}
			_ = dtpacktype.Rows.Count;
			_ = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 1511,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvFoodProduction.GetJobOrderNoForInvFoodProduction(val);
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PlanCode");
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["ProductionJobOrderNo"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbJobOrderOverHead, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)cmbJobOrderOverHead).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					DDL.BindDDL(dt, CmbJobOrderNoInput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)CmbJobOrderNoInput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)CmbJobOrderNoOutput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFillForSummery()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvProductionJobOrder.GetJobOrderAll(val);
			if (dtPJO.Rows.Count > 0)
			{
				DDL.BindDDL(dtPJO, cmbsummeryJobOrderNo, "Id", "ProductionJobOrderNo", "Job Order No", ZeroIndex: false);
				DDL.BindDDL(dtPJO, CmbJobOrderSettlement, "Id", "ProductionJobOrderNo", "Job Order No", ZeroIndex: false);
				return;
			}
			((UltraGridBase)cmbsummeryJobOrderNo).DataSource = null;
			((UltraGridBase)CmbJobOrderSettlement).DataSource = null;
			((Control)(object)cmbsummeryJobOrderNo).Text = string.Empty;
			((Control)(object)CmbJobOrderSettlement).Text = string.Empty;
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
			dtEntryType.Columns.Add("Id");
			dtEntryType.Columns.Add("Name");
			dtEntryType.Rows.Add(1, "ManualRate");
			dtEntryType.Rows.Add(2, "AutoRate");
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			_ = Item.GetWarehouseFromInventoryStocksEvaluations(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			}).Rows.Count;
			_ = 0;
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
			Item.GetItemByWarehouseIdfromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WarehouseId
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadPlantDetailByJobOrderId(int Id)
	{
		try
		{
			DataTable dt = InvProductionJobOrder.ReadPlantDetailByJobOrderId(Id);
			if (dt.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbJobOrderNoInput).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeederInput, "PlantId", "PlantName", "Plant Name", false);
					((UltraGridBase)CmbPlantFeederInput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeederOutput, "PlantId", "PlantName", "Plant Name", false);
					((UltraGridBase)CmbPlantFeederOutput).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeederOverHead, "PlantId", "PlantName", "Plant Name", false);
					((UltraGridBase)CmbPlantFeederOverHead).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
			else
			{
				((Control)(object)CmbPlantFeederInput).Text = string.Empty;
				((Control)(object)CmbPlantFeederOutput).Text = string.Empty;
				((Control)(object)CmbPlantFeederOverHead).Text = string.Empty;
				((UltraGridBase)CmbPlantFeederInput).DataSource = null;
				((UltraGridBase)CmbPlantFeederOutput).DataSource = null;
				((UltraGridBase)CmbPlantFeederOverHead).DataSource = null;
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
			if (e.Control && e.KeyCode == Keys.NumPad4)
			{
				tabControl1.SelectedIndex = 2;
				txtDocDateOverHead.Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad5)
			{
				tabControl1.SelectedIndex = 3;
				((Control)(object)DatagridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad6)
			{
				tabControl1.SelectedIndex = 4;
				cmbsummeryJobOrderNo.Focus();
			}
			if (tabControl1.SelectedIndex == 2)
			{
				if (btnSaveOverHead.Visible && btnSaveOverHead.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnSaveOverHead_Click(null, null);
				}
				if (btnUpdateOverHead.Visible && btnUpdateOverHead.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdateOverHead_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewOverHead_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshOverHead_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 4 && e.Control && e.KeyCode == Keys.F5)
			{
				cmbsummeryJobOrderNo.Focus();
			}
			if (tabControl1.SelectedIndex == 3)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					if (DatagridHistory.CurrentRow.Cells[4].Value.ToString() == "Input")
					{
						((Control)(object)CmbJobOrderNoInput).Enabled = false;
						RecId = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString());
						ReadByIdInput(RecId);
						tabControl1.SelectedIndex = 0;
						txtDocdate.Focus();
					}
					else
					{
						((Control)(object)CmbJobOrderNoOutput).Enabled = false;
						RecIdOutPut = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString());
						ReadByIdOutPut(RecIdOutPut);
						tabControl1.SelectedIndex = 1;
						DocDateOutPut.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.D)
				{
					DatagridHistoryDetail();
				}
			}
			if (tabControl1.SelectedIndex == 0 && btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0 && btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.L)
			{
				btnLoadInvoices_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.F5)
			{
				txtDocdate.Focus();
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdStockConversionProductionDetail).Focus();
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1 && btnSaveOutPut.Visible && btnSaveOutPut.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSaveOutPut_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1 && btnUpdateOutPut.Visible && btnUpdateOutPut.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdateOutPut_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.F5)
			{
				DocDateOutPut.Focus();
			}
			if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdByProduct).Focus();
			}
			if (tabControl1.SelectedIndex == 1 && e.Control)
			{
				_ = e.KeyCode;
				_ = 13;
			}
			if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.N)
			{
				btnNewOutPut_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshOutPut_Click(null, null);
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
			if (tabControl1.SelectedIndex == 1 && btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
			{
				GenerateDocNumberOutPut();
				DocDateOutPut.Focus();
			}
			if (tabControl1.SelectedIndex == 2)
			{
				GenerateCodeofOverHead();
				txtDocDateOverHead.Focus();
			}
			if (tabControl1.SelectedIndex == 5)
			{
				panel29.AutoScroll = true;
				panel29.BorderStyle = BorderStyle.FixedSingle;
				CmbJobOrderSettlement.Focus();
			}
			GridHistory(50);
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

	private void GenerateDocNumberInput()
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
			int code = InvFoodProduction.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1512,
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

	private void CmbJobOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvProductionJobOrder Pro = new InvProductionJobOrder();
			Pro.OrganizationId = UserAccount.OrganizationId;
			Pro.CompanyId = UserAccount.CompanyId;
			Pro.Id = Conversion.ToInt(CmbJobOrderNoInput.Value);
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
			ReadPlantDetailByJobOrderId(Pro.Id);
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
		if (((UltraGridBase)CmbJobOrderNoInput).ActiveRow == null)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNoInput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeederInput).ActiveRow == null)
		{
			MessageBox.Show("Plant Name Field Required");
			CmbPlantFeederInput.Focus();
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

	private void InsertInput()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
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
			invFoodProduction.DocumentTypeId = 1512;
			invFoodProduction.DocCode = Conversion.ToInt((object)txtdocnumber.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invFoodProduction.MainRemarks = txtRemarksInput.Text.ToString();
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
			invFoodProduction.ProductionJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoInput).Text.Trim());
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantFeederInput.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchesId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.ApprovedDate = DateTime.Now;
			invFoodProduction.EntryUserId = UserAccount.ID;
			invFoodProduction.ModifyUserId = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			if (grdStockConversionProductionDetail.RowCount > 0)
			{
				decimal GrossWeight = default(decimal);
				GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble((object)r.Cells["ItemQty"].Text) > 0.0 && Conversion.ToDecimal((object)r.Cells["NetWeight"].Text) > 0m)
					{
						InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
						Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						Conversiondetail.ActionTypeId = ((Conversiondetail.Id == 0) ? 1 : 2);
						Conversiondetail.EntryTypeId = Conversion.ToInt((object)3);
						Conversiondetail.JobOrderInPutId = Conversion.ToInt(r.Cells["JobOrderInPutId"].Value);
						Conversiondetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
						Conversiondetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						Conversiondetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
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
						if (r.Cells["JobLotId"].Text == string.Empty)
						{
							MessageBox.Show("JobLot Field Require in Input Detail");
							return;
						}
						Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Conversiondetail.PackUnit = Conversion.ToInt(r.Cells["PackUom"].Value);
						Conversiondetail.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						Conversiondetail.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						if (Conversiondetail.Qty == 0m)
						{
							throw new Exception("ItemQty Field Required");
						}
						Conversiondetail.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						GrossWeight += Conversiondetail.Weight;
						if (Conversiondetail.Weight == 0m)
						{
							throw new Exception("ItemWeight Field Required");
						}
						Conversiondetail.Rate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						Conversiondetail.NetRate = Conversion.ToDecimal((object)Conversiondetail.Rate);
						Conversiondetail.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
						Conversiondetail.Amount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
						Conversiondetail.TotalAmount = Conversiondetail.Amount;
						Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
					}
					if (RemovedInputList.Count > 0)
					{
						for (int i = 0; i < RemovedInputList.Count; i++)
						{
							invFoodProduction.invFoodProductionDetails.Add(RemovedInputList[i]);
						}
					}
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}
				InvFoodProduction.Save(invFoodProduction);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
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

	public void ReadByIdInput(int ID)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		try
		{
			RecId = ID;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC != null)
			{
				txtdocnumber.Text = Conversion.ToString((object)SC.DocCode);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarksInput.Text = Conversion.ToString((object)SC.MainRemarks);
				CmbJobOrderNoInput.Value = SC.ProductionJobOrderId;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("JobOrderNo");
				dt.Rows.Add(SC.ProductionJobOrderId, SC.ProductionJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNoInput, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNoInput).Rows[0].Activate();
				CmbJobOrderNo_Leave(null, null);
				CmbPlantFeederInput.Value = SC.PlantId;
				DataTable dtac = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbJobOrderNoInput.Value)
				});
				if (dtac.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtac, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
					DDL.BindDDLNew(dtac, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
					((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
					((UltraGridBase)CmbWIPItem).Rows[0].Activate();
				}
			}
			tableInput.Rows.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				tabControl1.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				tableInput.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].JobOrderInPutId, SC.invFoodProductionDetails[i].RefDocumentTypeId, SC.invFoodProductionDetails[i].RefDocNoId, SC.invFoodProductionDetails[i].RefDocSubIdNo, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackingType, SC.invFoodProductionDetails[i].PackUomId, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].PackUomEquivalent, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].NetRate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].RateUomEquivalent, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
			}
			grdStockConversionProductionDetail.DataSource = tableInput;
			grdStockConversionProductionDetail.RetrieveStructure();
			gridsettings();
			GetVoucherHeadId(RecId, 1512);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void reset()
	{
		try
		{
			RemovedInputList.Clear();
			RecId = 0;
			VoucherHeadId = 0;
			txtRemarksInput.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbJobOrderNoInput).Enabled = true;
			tableInput.Clear();
			grdStockConversionProductionDetail.ClearStructure();
			GenerateDocNumberInput();
			((UltraGridBase)CmbJobOrderNoInput).DataSource = null;
			((Control)(object)CmbJobOrderNoInput).Text = string.Empty;
			JobOrderNoFill();
			((Control)(object)CmbPlantFeederInput).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		JobOrderNoFill();
	}

	private void grdStockConversionProductionDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdStockConversionProductionDetail.UpdateData();
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (e.Column.Key == "ItemQty")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["PackEquivalent"].Value.ToString());
				item.Cells["NetWeight"].Value = Qty * Uom;
				double num = Qty * Uom;
				double Rate2 = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount2 = num / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Amount2;
			}
			if (e.Column.Key == "NetWeight")
			{
				double Weight = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
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

	private void grdStockConversionProductionDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdStockConversionProductionDetail.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.EntryTypeId = Conversion.ToInt((object)3);
					Conversiondetail.JobOrderInPutId = Conversion.ToInt(r.Cells["JobOrderInPutId"].Value);
					Conversiondetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					Conversiondetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					Conversiondetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Conversiondetail.PackUnit = Conversion.ToInt(r.Cells["PackUom"].Value);
					Conversiondetail.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					Conversiondetail.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					Conversiondetail.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					Conversiondetail.Rate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					Conversiondetail.NetRate = Conversion.ToDecimal((object)Conversiondetail.Rate);
					Conversiondetail.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
					Conversiondetail.Amount = Conversion.ToInt(r.Cells["ItemAmount"].Value);
					Conversiondetail.TotalAmount = Conversiondetail.Amount;
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					Conversiondetail.ActionTypeId = 3;
					RemovedInputList.Add(Conversiondetail);
					tableInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					tableInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			if (grdStockConversionProductionDetail.GetRows().Count() == 0)
			{
				((Control)(object)CmbJobOrderNoInput).Enabled = true;
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
				grdStockConversionProductionDetail.RootTable.Columns["ItemRate"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["RateUom"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdStockConversionProductionDetail.RootTable.Columns["Id"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobOrderInPutId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobOrderInPutId"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdStockConversionProductionDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdStockConversionProductionDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
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

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbJobOrderNoInput).ActiveRow != null)
			{
				LoadInput = new LoadInputDataByJobOrder(UserAccount);
				LoadInput.JobOrderId = Conversion.ToInt(CmbJobOrderNoInput.Value);
				LoadInput.ShowDialog();
				LoadInput.BringToFront();
				LoadDataDetailfromPurchaseInvoivce();
				return;
			}
			CmbJobOrderNoInput.Focus();
			throw new Exception("Select Job Order No First...");
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
			DataTable dtpurchaseinvoice = LoadInput.dtIssuance;
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
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]) == Conversion.ToInt(item.Cells["JobOrderInPutId"].Value))
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					tableInput.Rows.Add(0, Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["WareHouseId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["WareHouseName"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["JobLotId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["JobLot"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["ItemName"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["PackingTypeId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackingType"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["UomSchIdQty"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackUom"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["PackEquivalent"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["BalQty"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["BalWeight"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemRate"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RateUomId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RateUom"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["RateEquivalent"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["BalAmount"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["Remarks"]));
				}
			}
			grdStockConversionProductionDetail.DataSource = tableInput;
			grdStockConversionProductionDetail.RetrieveStructure();
			gridsettings();
			((Control)(object)CmbJobOrderNoInput).Enabled = false;
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
			if (RecId > 0)
			{
				GenerateSlipInput_Output(RecId, 1512);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlipInput_Output(int PrintId, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionReports.FoodProductionSlip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1512-FoodProductionSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			InputGridHistory();
		}
	}

	private void InputGridHistory(int NoOfrecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			DataTable dtGridRegister = InvFoodProduction.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1512,
				NoOfRecords = NoOfrecords,
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights,
				EntryUser = UserAccount.ID
			});
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocCode", typeof(string));
				dtcol.Columns.Add("ProductionJobOrderId", typeof(int));
				dtcol.Columns.Add("ProductionJobOrderNo", typeof(string));
				dtcol.Columns.Add("PlantId", typeof(int));
				dtcol.Columns.Add("WorkInProcessAccountTitle", typeof(string));
				dtcol.Columns.Add("WipItemName", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(string));
				dtcol.Columns.Add("MainRemarks", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["RecordNo"], dtGridRegister.Rows[i]["Id"], dtGridRegister.Rows[i]["DocumentTypeId"], dtGridRegister.Rows[i]["DocDate"], dtGridRegister.Rows[i]["DocCode"], dtGridRegister.Rows[i]["ProductionJobOrderId"], dtGridRegister.Rows[i]["ProductionJobOrderNo"], dtGridRegister.Rows[i]["PlantId"], dtGridRegister.Rows[i]["WorkInProcessAccountTitle"], dtGridRegister.Rows[i]["WipItemName"], dtGridRegister.Rows[i]["EntryUserName"], dtGridRegister.Rows[i]["EntryDate"], dtGridRegister.Rows[i]["IsApproved"], dtGridRegister.Rows[i]["MainRemarks"]);
				}
				grdinputhistory.DataSource = dtcol;
				grdinputhistory.RetrieveStructure();
				GridInputSetting();
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
			grdinputhistory.RootTable.Columns.Add("Edit");
			grdinputhistory.RootTable.Columns["Edit"].Key = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdinputhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdinputhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdinputhistory.RootTable.Columns["Edit"].Width = 40;
			grdinputhistory.RootTable.Columns["Edit"].Position = 0;
			grdinputhistory.RootTable.Columns.Add("Print");
			grdinputhistory.RootTable.Columns["Print"].Key = "Print";
			grdinputhistory.RootTable.Columns["Print"].Caption = "Print";
			grdinputhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdinputhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdinputhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdinputhistory.RootTable.Columns["Print"].Width = 40;
			grdinputhistory.RootTable.Columns["Print"].Position = 1;
			grdinputhistory.FrozenColumns = 2;
			grdinputhistory.RootTable.Columns["Id"].Visible = false;
			grdinputhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdinputhistory.RootTable.Columns["ProductionJobOrderId"].Visible = false;
			grdinputhistory.RootTable.Columns["PlantId"].Visible = false;
			CommonServices.GridAutoAdjustment(AryColWidth, grdinputhistory);
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
			if (e.Column.Key == "Edit")
			{
				((Control)(object)CmbJobOrderNoInput).Enabled = false;
				if (!(Conversion.ToString(grdinputhistory.CurrentRow.Cells["IsApproved"].Value) != "True"))
				{
					throw new Exception("You Can't Update Approved Record");
				}
				RecId = Conversion.ToInt((object)grdinputhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadByIdInput(RecId);
			}
			if (e.Column.Key == "Print")
			{
				GenerateSlipInput_Output(Conversion.ToInt(grdinputhistory.CurrentRow.Cells["Id"].Value), 1512);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (grdinputhistory.CurrentRow != null && (int)grdinputhistory.CurrentRow.RowType == 0 && grdinputhistory.CurrentRow.Cells.Count > 0)
			{
				InvFoodProduction SC = new InvFoodProduction();
				SC = InvFoodProduction.GetByID(Conversion.ToInt(grdinputhistory.CurrentRow.Cells["Id"].Value));
				DataTable dt = new DataTable();
				dt.Columns.Add("WareHouseName");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("Weight", typeof(decimal));
				dt.Columns.Add("Rate", typeof(decimal));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Amount", typeof(decimal));
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackingType, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].Rate, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
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
				grdInputDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Qty"].FormatString = "#,#";
				grdInputDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#";
				grdInputDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdInputDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Weight"].FormatString = "#,#";
				grdInputDetail.RootTable.Columns["Weight"].TotalFormatString = "#,#";
				grdInputDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdInputDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdInputDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdInputDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
				grdInputDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
				grdInputDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				grdInputDetail.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
				CommonServices.GridAutoAdjustmentNew(grdInputDetail);
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

	private void button2_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnJobOrderDefine_Click(object sender, EventArgs e)
	{
		try
		{
			ProductionPreCostingAndJobOrder_St obj = new ProductionPreCostingAndJobOrder_St(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject(obj.Name);
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You don't have the right to view this form...");
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
	}

	private void btnRefreshOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			stockAccoutnFill();
			UOMFill();
			combojoblotfill();
			Warehouse();
			bagType();
			JobOrderNoFill();
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

	private void GenerateDocNumberOutPut()
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
			int code = InvFoodProduction.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1513,
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

	private void CmbJobOrderNoOutput_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InQty = 0.0;
				double InWeight = 0.0;
				double OutQty = 0.0;
				double OutWeight = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["EntryTypeId"]) == 3)
					{
						InQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						InWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					else
					{
						OutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						OutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
				}
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
			ReadPlantDetailByJobOrderId(Conversion.ToInt(CmbJobOrderNoOutput.Value));
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
				grdByProduct.RootTable.Columns["NeRate"].Visible = false;
				grdByProduct.RootTable.Columns["NetAmount"].Visible = false;
			}
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["PackUomId"].Visible = false;
			grdByProduct.RootTable.Columns["RateUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["EntryTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["PackEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["RateEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["OutPutIdFromJobOrder"].Visible = false;
			grdByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["OutPutIdFromJobOrder"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["NetRate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["NetAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackUomId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["NetWeight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemRate"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdByProduct.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			dtware = CommonServices.WareHouseGetAllService();
			grdByProduct.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			grdByProduct.RootTable.Columns["WareHouseId"].HasValueList = true;
			grdByProduct.RootTable.Columns["WareHouseId"].LimitToList = true;
			grdByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grdByProduct.RootTable.Columns["WareHouseId"].Caption = "WareHouse Name";
			grdByProduct.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtware.DefaultView, "Id", "WareHouseName");
			grdByProduct.RootTable.Columns["WareHouseId"].Width = 150;
			grdByProduct.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grdByProduct.RootTable.Columns["JobLotId"].HasValueList = true;
			grdByProduct.RootTable.Columns["JobLotId"].LimitToList = true;
			grdByProduct.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grdByProduct.RootTable.Columns["JobLotId"].Caption = "JobLot";
			grdByProduct.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtjoblot.DefaultView, "Id", "JobLotDescription");
			grdByProduct.RootTable.Columns["JobLotId"].Width = 150;
			grdByProduct.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdByProduct.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdByProduct.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdByProduct.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdByProduct.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdByProduct.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtpacktype.DefaultView, "Id", "PackingType");
			grdByProduct.RootTable.Columns["PackingTypeId"].Width = 150;
			CommonServices.GridAutoAdjustment(AryColWidth, grdByProduct);
			grdByProduct.RootTable.Columns.Add("Delete");
			grdByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Delete"].Width = 35;
			grdByProduct.RootTable.Columns["Delete"].Position = 0;
			grdByProduct.RootTable.Columns.Add("Add");
			grdByProduct.RootTable.Columns["Add"].Caption = "+";
			grdByProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdByProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Add"].Width = 35;
			grdByProduct.RootTable.Columns["Add"].Position = 1;
			grdByProduct.FrozenColumns = 2;
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
			if (e.Column.Key == "ItemQty")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["PackEquivalent"].Value.ToString());
				item.Cells["NetWeight"].Value = Qty * Uom;
				double Weight2 = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				double Rate2 = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Amount2;
				double NetRate2 = Conversion.ToDouble((object)item.Cells["NetRate"].Value.ToString());
				double NetAmount2 = Weight2 / RateUOM2 * NetRate2;
				item.Cells["NetAmount"].Value = NetAmount2;
			}
			if (e.Column.Key == "NetWeight")
			{
				Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
				Conversion.ToDouble((object)item.Cells["PackEquivalent"].Value.ToString());
				double Weight = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Value.ToString());
				double Amount = Weight / RateUOM * Rate;
				item.Cells["ItemAmount"].Value = Amount;
				double NetRate = Conversion.ToDouble((object)item.Cells["NetRate"].Value.ToString());
				double NetAmount = Weight / RateUOM * NetRate;
				item.Cells["NetAmount"].Value = NetAmount;
			}
			CommonServices.GridAutoAdjustmentNew(grdByProduct);
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
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				GridEXRow r2 = grdByProduct.CurrentRow;
				DataRow dr = tableByProduct.Rows[r2.RowIndex];
				tableByProduct.Rows.Add(dr.ItemArray);
				grdByProduct.DataSource = tableByProduct;
				if (!btnSaveOutPut.Visible || !btnSaveOutPut.Enabled)
				{
					MessageBox.Show("You can not add row in Update Mode");
					return;
				}
				tableByProduct.Rows[tableByProduct.Rows.Count - 1]["Id"] = 0;
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdByProduct.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					Conversiondetail.EntryTypeId = Conversion.ToInt(r.Cells["EntryTypeId"].Value);
					Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.JobOrderOutPutId = Conversion.ToInt(r.Cells["OutPutIdFromJobOrder"].Value);
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Conversiondetail.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Conversiondetail.Qty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					Conversiondetail.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					Conversiondetail.Rate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					Conversiondetail.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
					Conversiondetail.Amount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					Conversiondetail.NetRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
					Conversiondetail.TotalAmount = Conversion.ToDecimal(r.Cells["NetAmount"].Value);
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					Conversiondetail.ActionTypeId = 3;
					RemovedOutputList.Add(Conversiondetail);
					tableByProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					tableByProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdByProduct.UpdateData();
			if (grdByProduct.GetRows().Count() == 0)
			{
				((Control)(object)CmbJobOrderNoOutput).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdOutPut(int ID)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		try
		{
			if (ID <= 0)
			{
				return;
			}
			RecIdOutPut = ID;
			btnSaveOutPut.Visible = false;
			btnUpdateOutPut.Visible = true;
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC != null)
			{
				txtDocNoOutPut.Text = Conversion.ToString((object)SC.DocCode);
				DocDateOutPut.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarksHeaderOutPut.Text = Conversion.ToString((object)SC.MainRemarks);
				CmbJobOrderNoOutput.Value = SC.ProductionJobOrderId;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("JobOrderNo");
				dt.Rows.Add(SC.ProductionJobOrderId, SC.ProductionJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNoOutput).Rows[0].Activate();
				CmbJobOrderNoOutput_Leave(null, null);
				CmbPlantFeederOutput.Value = SC.PlantId;
			}
			tableByProduct.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				tabControl1.SelectedIndex = 1;
				tabControl3.SelectedIndex = 0;
				tableByProduct.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].JobOrderOutPutId, SC.invFoodProductionDetails[i].EntryTypeId, SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackUomId, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].PackUomEquivalent, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].RateUomEquivalent, SC.invFoodProductionDetails[i].Amount, Math.Round(SC.invFoodProductionDetails[i].NetRate, 2), SC.invFoodProductionDetails[i].TotalAmount, SC.invFoodProductionDetails[i].Remarks);
				if (SC.invFoodProductionDetails[i].EntryTypeId == 2 && SC.invFoodProductionDetails[i].NetRate > 0m)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString((object)SC.invFoodProductionDetails[i].NetRate);
				}
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
			GetVoucherHeadId(RecIdOutPut, 1513);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
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
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateDocNumberOutPut();
			}
			invFoodProduction.DocCode = Conversion.ToInt((object)txtDocNoOutPut.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)DocDateOutPut.Value);
			invFoodProduction.MainRemarks = txtRemarksHeaderOutPut.Text.ToString();
			invFoodProduction.DocumentTypeId = 1513;
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			invFoodProduction.ProductionJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoOutput).Text.Trim());
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchesId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.ApprovedDate = DateTime.Now;
			invFoodProduction.EntryUserId = UserAccount.ID;
			invFoodProduction.ModifyUserId = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			if (grdByProduct.RowCount > 0)
			{
				decimal GrossWeight = default(decimal);
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					if (Conversion.ToDouble(r.Cells["ItemQty"].Value) > 0.0 && Conversion.ToDouble(r.Cells["NetWeight"].Value) > 0.0)
					{
						if (Conversion.ToInt((object)r.Cells["EntryTypeId"].Text) == 0)
						{
							MessageBox.Show("EntryType Field Require in Output Detail");
							return;
						}
						Conversiondetail.EntryTypeId = Conversion.ToInt((object)r.Cells["EntryTypeId"].Text);
						Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						Conversiondetail.ActionTypeId = ((Conversiondetail.Id == 0) ? 1 : 2);
						Conversiondetail.JobOrderOutPutId = Conversion.ToInt(r.Cells["OutPutIdFromJobOrder"].Value);
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
						Conversiondetail.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						if (r.Cells["JobLotId"].Text == string.Empty || r.Cells["JobLotId"].Text == "0")
						{
							MessageBox.Show("JobLot Field Require in Output Detail");
							return;
						}
						Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Conversiondetail.Qty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						Conversiondetail.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						GrossWeight += Conversiondetail.Weight;
						Conversiondetail.Rate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						Conversiondetail.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
						Conversiondetail.Amount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
						Conversiondetail.NetRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
						Conversiondetail.TotalAmount = Conversion.ToDecimal(r.Cells["NetAmount"].Value);
						Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
					}
				}
				if (RemovedOutputList.Count > 0)
				{
					for (int i = 0; i < RemovedOutputList.Count; i++)
					{
						invFoodProduction.invFoodProductionDetails.Add(RemovedOutputList[i]);
					}
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("grid Record not found please check!");
				}
				InvFoodProduction.Save(invFoodProduction);
				if (RecIdOutPut > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				resetOutPut();
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

	private void resetOutPut()
	{
		try
		{
			RemovedOutputList.Clear();
			DiffNetWeight = 0.0;
			ConsumedWeight = 0.0;
			RecIdOutPut = 0;
			VoucherHeadId = 0;
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
			((UltraGridBase)CmbJobOrderNoOutput).DataSource = null;
			((Control)(object)CmbJobOrderNoOutput).Text = string.Empty;
			txtAvgFinishGoodRate.Text = string.Empty;
			JobOrderNoFill();
			((Control)(object)CmbPlantFeederOutput).Text = string.Empty;
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

	private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl3.SelectedIndex == 1)
		{
			OutputGridHistory(50);
		}
	}

	private void OutputGridHistory(int NoOfrecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			DataTable dtGridRegister = InvFoodProduction.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1513,
				NoOfRecords = NoOfrecords,
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights,
				EntryUser = UserAccount.ID
			});
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocCode", typeof(string));
				dtcol.Columns.Add("ProductionJobOrderId", typeof(int));
				dtcol.Columns.Add("ProductionJobOrderNo", typeof(string));
				dtcol.Columns.Add("PlantId", typeof(int));
				dtcol.Columns.Add("WorkInProcessAccountTitle", typeof(string));
				dtcol.Columns.Add("WipItemName", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(string));
				dtcol.Columns.Add("MainRemarks", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["RecordNo"], dtGridRegister.Rows[i]["Id"], dtGridRegister.Rows[i]["DocumentTypeId"], dtGridRegister.Rows[i]["DocDate"], dtGridRegister.Rows[i]["DocCode"], dtGridRegister.Rows[i]["ProductionJobOrderId"], dtGridRegister.Rows[i]["ProductionJobOrderNo"], dtGridRegister.Rows[i]["PlantId"], dtGridRegister.Rows[i]["WorkInProcessAccountTitle"], dtGridRegister.Rows[i]["WipItemName"], dtGridRegister.Rows[i]["EntryUserName"], dtGridRegister.Rows[i]["EntryDate"], dtGridRegister.Rows[i]["IsApproved"], dtGridRegister.Rows[i]["MainRemarks"]);
				}
				grdOutputHistory.DataSource = dtcol;
				grdOutputHistory.RetrieveStructure();
				GridOutputSetting();
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
			grdOutputHistory.RootTable.Columns.Add("Edit");
			grdOutputHistory.RootTable.Columns["Edit"].Key = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOutputHistory.RootTable.Columns["Edit"].Width = 40;
			grdOutputHistory.RootTable.Columns["Edit"].Position = 0;
			grdOutputHistory.RootTable.Columns.Add("Print");
			grdOutputHistory.RootTable.Columns["Print"].Key = "Print";
			grdOutputHistory.RootTable.Columns["Print"].Caption = "Print";
			grdOutputHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutputHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdOutputHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdOutputHistory.RootTable.Columns["Print"].Width = 40;
			grdOutputHistory.RootTable.Columns["Print"].Position = 1;
			grdOutputHistory.FrozenColumns = 2;
			grdOutputHistory.RootTable.Columns["Id"].Visible = false;
			grdOutputHistory.RootTable.Columns["ProductionJobOrderId"].Visible = false;
			grdOutputHistory.RootTable.Columns["PlantId"].Visible = false;
			grdOutputHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdOutputHistory);
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
				if (!(Conversion.ToString(grdOutputHistory.CurrentRow.Cells["IsApproved"].Value) != "True"))
				{
					throw new Exception("You Can't Update Approved Record");
				}
				RecIdOutPut = Conversion.ToInt((object)grdOutputHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadByIdOutPut(RecIdOutPut);
			}
			if (e.Column.Key == "Print")
			{
				GenerateSlipInput_Output(Conversion.ToInt(grdOutputHistory.CurrentRow.Cells["Id"].Value), 1513);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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
			if (RecIdOutPut > 0)
			{
				GenerateSlipInput_Output(RecIdOutPut, 1513);
				return;
			}
			throw new Exception("No Record Found");
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

	private void btnLoadOutPutFromjobOrder_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow != null)
			{
				LoadOutPut = new LoadOutPutDataByJobOrder(UserAccount);
				LoadOutPut.JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
				LoadOutPut.ShowDialog();
				LoadOutPut.BringToFront();
				LoadOutPutDataFromJobOrder();
				return;
			}
			CmbJobOrderNoOutput.Focus();
			throw new Exception("Select Job Order No First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadOutPutDataFromJobOrder()
	{
		try
		{
			DataTable dtLoadOutPutFromJobOrder = LoadOutPut.dtIssuance;
			if (dtLoadOutPutFromJobOrder.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtLoadOutPutFromJobOrder.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtLoadOutPutFromJobOrder.Rows[i]["Id"]) == Conversion.ToInt(item.Cells["OutPutIdFromJobOrder"].Value))
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					tableByProduct.Rows.Add(0, dtLoadOutPutFromJobOrder.Rows[i]["Id"], dtLoadOutPutFromJobOrder.Rows[i]["EntryTypeId"], dtLoadOutPutFromJobOrder.Rows[i]["EntryType"], dtLoadOutPutFromJobOrder.Rows[i]["WareHouseId"], dtLoadOutPutFromJobOrder.Rows[i]["JobLotId"], dtLoadOutPutFromJobOrder.Rows[i]["InvPackingTypeId"], dtLoadOutPutFromJobOrder.Rows[i]["ItemId"], dtLoadOutPutFromJobOrder.Rows[i]["ItemName"], dtLoadOutPutFromJobOrder.Rows[i]["PackUomId"], dtLoadOutPutFromJobOrder.Rows[i]["OuterPackUom"], dtLoadOutPutFromJobOrder.Rows[i]["OuterPackUomEquivalent"], dtLoadOutPutFromJobOrder.Rows[i]["BalQty"], dtLoadOutPutFromJobOrder.Rows[i]["BalWeight"], dtLoadOutPutFromJobOrder.Rows[i]["ItemRate"], dtLoadOutPutFromJobOrder.Rows[i]["RateUomId"], dtLoadOutPutFromJobOrder.Rows[i]["RateUOM"], dtLoadOutPutFromJobOrder.Rows[i]["RateUomEquivalent"], dtLoadOutPutFromJobOrder.Rows[i]["BalAmount"], dtLoadOutPutFromJobOrder.Rows[i]["NetRate"], dtLoadOutPutFromJobOrder.Rows[i]["BalNetAmount"], dtLoadOutPutFromJobOrder.Rows[i]["RemarksDetail"]);
				}
			}
			grdByProduct.DataSource = tableByProduct;
			grdByProduct.RetrieveStructure();
			gridsettingsOutPut();
			((Control)(object)CmbJobOrderNoOutput).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutDetailByHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdOutputHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				InvFoodProduction SC = new InvFoodProduction();
				SC = InvFoodProduction.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable dt = new DataTable();
				dt.Columns.Add("WareHouseName");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("Weight", typeof(decimal));
				dt.Columns.Add("ItemRate", typeof(decimal));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Amount", typeof(decimal));
				dt.Columns.Add("NetRate", typeof(decimal));
				dt.Columns.Add("NetAmount", typeof(decimal));
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackingType, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].Rate, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].NetRate, SC.invFoodProductionDetails[i].TotalAmount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdOutPutDetail.DataSource = dt;
				grdOutPutDetail.RetrieveStructure();
				if (!formright.DoHaveCanRateandAmount)
				{
					grdOutPutDetail.RootTable.Columns["ItemRate"].Visible = false;
					grdOutPutDetail.RootTable.Columns["RateUom"].Visible = false;
					grdOutPutDetail.RootTable.Columns["Amount"].Visible = false;
					grdOutPutDetail.RootTable.Columns["NetRate"].Visible = false;
					grdOutPutDetail.RootTable.Columns["NetAmount"].Visible = false;
				}
				grdOutPutDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["Qty"].FormatString = "#,#";
				grdOutPutDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#";
				grdOutPutDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["Weight"].FormatString = "#,#";
				grdOutPutDetail.RootTable.Columns["Weight"].TotalFormatString = "#,#";
				grdOutPutDetail.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
				grdOutPutDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				grdOutPutDetail.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
				grdOutPutDetail.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
				grdOutPutDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				grdOutPutDetail.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
				grdOutPutDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdOutPutDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdOutPutDetail.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
				grdOutPutDetail.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
				grdOutPutDetail.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdOutPutDetail.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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

	private void btnLoadAllOutputHistoy_Click(object sender, EventArgs e)
	{
	}

	private void GenerateCodeofOverHead()
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
			int code = InvFoodProduction.GenerateCodeOverHeaad(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1514,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoOverHead.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbJobOrderOverHead_Leave(object sender, EventArgs e)
	{
		try
		{
			ReadPlantDetailByJobOrderId(Conversion.ToInt(cmbJobOrderOverHead.Value));
			GetTotlInPutOut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotlInPutOut()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvJobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InQty = 0.0;
				double InWeight = 0.0;
				double OutQty = 0.0;
				double OutWeight = 0.0;
				double FinsishOutQty = 0.0;
				double FinishOutWeight = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["EntryTypeId"]) == 2)
					{
						FinsishOutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						FinishOutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					if (Conversion.ToInt(dt.Rows[i]["EntryTypeId"]) == 3)
					{
						InQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						InWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					else
					{
						OutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						OutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
				}
				txtInputQtyTotalOH.Text = InQty.ToString("#,#");
				txtInputWeightTotalOH.Text = InWeight.ToString("#,#");
				txtOutputQtyTotalOH.Text = OutQty.ToString("#,#");
				txtOutputWeightTotalOH.Text = OutWeight.ToString("#,#");
				txtfinishQtyOh.Text = FinsishOutQty.ToString("#,#");
				txtfinishWeightOh.Text = FinishOutWeight.ToString("#,#");
			}
			else
			{
				txtInputQtyTotalOH.Text = "0";
				txtInputWeightTotalOH.Text = "0";
				txtOutputQtyTotalOH.Text = "0";
				txtOutputWeightTotalOH.Text = "0";
				txtfinishQtyOh.Text = "0";
				txtfinishWeightOh.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		try
		{
			if (!FormValidationOfOverHead())
			{
				return;
			}
			InvFoodProductionOverHeads obj = new InvFoodProductionOverHeads();
			if (RecOverHeadId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecOverHeadId;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateCodeofOverHead();
			}
			obj.InvFoodProductionOverHeadsList = new List<InvFoodProductionOverHeads>();
			if (RemovedOverheadList.Count > 0)
			{
				for (int i = 0; i < RemovedOverheadList.Count; i++)
				{
					obj.InvFoodProductionOverHeadsList.Add(RemovedOverheadList[i]);
				}
			}
			if (grdDetailOverhead.RowCount > 0)
			{
				GridEXRow[] rows = grdDetailOverhead.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionOverHeads OverHeaddetail = new InvFoodProductionOverHeads();
					if (Conversion.ToInt(r.Cells["ChartOfAccountId"].Value) > 0 && Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
					{
						OverHeaddetail.PlantId = Conversion.ToInt(CmbPlantFeederOverHead.Value);
						OverHeaddetail.ProductionJobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value);
						OverHeaddetail.DocumentTypeId = 1514;
						OverHeaddetail.EntryDate = DateTime.Now;
						OverHeaddetail.ModifyDate = DateTime.Now;
						OverHeaddetail.ApprovedDate = DateTime.Now;
						OverHeaddetail.EntryUserId = UserAccount.ID;
						OverHeaddetail.ModifyUserId = UserAccount.ID;
						OverHeaddetail.OrganizationId = UserAccount.OrganizationId;
						OverHeaddetail.CompanyId = UserAccount.CompanyId;
						OverHeaddetail.BranchesId = UserAccount.BranchesId;
						OverHeaddetail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						OverHeaddetail.ActionId = ((OverHeaddetail.Id == 0) ? 1 : 2);
						OverHeaddetail.JobOrderOverHeadId = Conversion.ToInt(r.Cells["JobOrderOverHeadId"].Value);
						OverHeaddetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
						OverHeaddetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
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
						if (Conversion.ToDecimal(r.Cells["ItemQty"].Value) == 0m && Conversion.ToDouble(r.Cells["ItemRate"].Value) != 0.0)
						{
							MessageBox.Show("ItemQty Required in Detail");
							return;
						}
						OverHeaddetail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						if (Conversion.ToDouble(r.Cells["ItemRate"].Value) == 0.0 && Conversion.ToDouble(r.Cells["ItemQty"].Value) != 0.0)
						{
							MessageBox.Show("ItemRate Required in Detail");
							return;
						}
						OverHeaddetail.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						if (Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
						{
							MessageBox.Show("Amount Required in Detail");
							return;
						}
						OverHeaddetail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						OverHeaddetail.RemarksOH = Conversion.ToString(r.Cells["Remarks"].Value);
						obj.InvFoodProductionOverHeadsList.Add(OverHeaddetail);
					}
				}
				if (obj.InvFoodProductionOverHeadsList.Count == 0)
				{
					throw new Exception("Grid Record not found please check!");
				}
			}
			InvFoodProductionOverHeads.Save(obj);
			if (RecOverHeadId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			FormResetOverHead();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			RecOverHeadId = 0;
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdOverHeadJobOrderWise(int JobOrderID)
	{
		try
		{
			btnSaveOverHead.Visible = false;
			btnUpdateOverHead.Visible = true;
			DataTable dtSC = InvFoodProductionOverHeads.GetByJobOrderID(JobOrderID);
			tabControl1.SelectedIndex = 2;
			tabControl4.SelectedIndex = 0;
			cmbJobOrderOverHead.Value = Conversion.ToInt(dtSC.Rows[0]["ProductionJobOrderId"]);
			((Control)(object)cmbJobOrderOverHead).Enabled = false;
			cmbJobOrderOverHead_Leave(null, null);
			CmbPlantFeederOverHead.Value = Conversion.ToInt(dtSC.Rows[0]["PlantId"]);
			tableOverHeads.Clear();
			for (int i = 0; i < dtSC.Rows.Count; i++)
			{
				tableOverHeads.Rows.Add(dtSC.Rows[i]["Id"], dtSC.Rows[i]["JobOrderOverHeadId"], Conversion.ToDateTime(dtSC.Rows[i]["DocDate"]).ToShortDateString(), dtSC.Rows[i]["DocNo"].ToString(), dtSC.Rows[i]["ProductionJobOrderId"].ToString(), dtSC.Rows[i]["CharOfAccountId"], dtSC.Rows[i]["AccountTitle"], dtSC.Rows[i]["BrandId"], dtSC.Rows[i]["BrandName"], dtSC.Rows[i]["BrandUomId"], dtSC.Rows[i]["BrandUom"], dtSC.Rows[i]["ItemQty"], dtSC.Rows[i]["ItemRate"], dtSC.Rows[i]["Amount"], dtSC.Rows[i]["RemarksOH"]);
			}
			grdDetailOverhead.DataSource = tableOverHeads;
			grdDetailOverhead.RetrieveStructure();
			grdDetailOverheadSettings();
			GetVoucherHeadId(RecOverHeadId, 1514);
			cmbJobOrderOverHead.Focus();
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewOverHead_Click(object sender, EventArgs e)
	{
		FormResetOverHead();
	}

	private void FormResetOverHead()
	{
		try
		{
			RecOverHeadId = 0;
			VoucherHeadId = 0;
			btnUpdateOverHead.Visible = false;
			btnSaveOverHead.Visible = true;
			((Control)(object)cmbJobOrderOverHead).Enabled = true;
			UpdateMode = false;
			cmbJobOrderOverHead.Focus();
			txtOutputQtyTotalOH.Text = string.Empty;
			txtOutputWeightTotalOH.Text = string.Empty;
			txtInputQtyTotalOH.Text = string.Empty;
			txtInputWeightTotalOH.Text = string.Empty;
			((Control)(object)cmbJobOrderOverHead).Text = string.Empty;
			GenerateCodeofOverHead();
			((Control)(object)CmbPlantFeederOverHead).Text = string.Empty;
			tableOverHeads.Clear();
			grdDetailOverhead.ClearStructure();
			RemovedOverheadList.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderOverHeadLock()
	{
		try
		{
			if (tableOverHeads.Rows.Count > 0)
			{
				((Control)(object)cmbJobOrderOverHead).Enabled = false;
			}
			else
			{
				((Control)(object)cmbJobOrderOverHead).Enabled = true;
			}
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
			grdDetailOverhead.RootTable.Columns["Id"].Visible = false;
			grdDetailOverhead.RootTable.Columns["ChartOfAccountId"].Visible = false;
			grdDetailOverhead.RootTable.Columns["BrandUomId"].Visible = false;
			grdDetailOverhead.RootTable.Columns["BrandId"].Visible = false;
			grdDetailOverhead.RootTable.Columns["InvProductionJobOrderId"].Visible = false;
			grdDetailOverhead.RootTable.Columns["DocDate"].Visible = false;
			grdDetailOverhead.RootTable.Columns["DocNo"].Visible = false;
			grdDetailOverhead.RootTable.Columns["JobOrderOverHeadId"].Visible = false;
			grdDetailOverhead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["JobOrderOverHeadId"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["InvProductionJobOrderId"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["ChartOfAccountId"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["ChartOfAccount"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["BrandId"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["Brand"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["BrandUomId"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["BrandUom"].EditType = (EditType)0;
			grdDetailOverhead.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdDetailOverhead.RootTable.Columns["ItemRate"].EditType = (EditType)1;
			grdDetailOverhead.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdDetailOverhead.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdDetailOverhead.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdDetailOverhead.RootTable.Columns["BrandUom"].TextAlignment = (TextAlignment)3;
			grdDetailOverhead.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailOverhead.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailOverhead.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdDetailOverhead.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			grdDetailOverhead.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdDetailOverhead.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetailOverhead.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailOverhead.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailOverhead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetailOverhead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetailOverhead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailOverhead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailOverhead.RootTable.Columns.Add("Delete");
			grdDetailOverhead.RootTable.Columns["Delete"].Caption = "X";
			grdDetailOverhead.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetailOverhead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetailOverhead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetailOverhead.RootTable.Columns["Delete"].Width = 25;
			grdDetailOverhead.RootTable.Columns["Delete"].Position = 0;
			grdDetailOverhead.RootTable.Columns.Add("Add");
			grdDetailOverhead.RootTable.Columns["Add"].Caption = "+";
			grdDetailOverhead.RootTable.Columns["Add"].ButtonText = "+";
			grdDetailOverhead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetailOverhead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetailOverhead.RootTable.Columns["Add"].Width = 25;
			grdDetailOverhead.RootTable.Columns["Add"].Position = 1;
			grdDetailOverhead.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(grdDetailOverhead);
			ctrlGrdBar9_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailOverhead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				if (!btnSaveOverHead.Visible || !btnSaveOverHead.Enabled)
				{
					MessageBox.Show("You can not add row in Update Mode");
					return;
				}
				GridEXRow r2 = grdDetailOverhead.CurrentRow;
				DataRow dr = tableOverHeads.Rows[r2.RowIndex];
				tableOverHeads.Rows.Add(dr.ItemArray);
				grdDetailOverhead.DataSource = tableOverHeads;
				tableOverHeads.Rows[tableOverHeads.Rows.Count - 1]["Id"] = 0;
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdDetailOverhead.CurrentRow;
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					InvFoodProductionOverHeads OverHeaddetail = new InvFoodProductionOverHeads();
					OverHeaddetail.PlantId = Conversion.ToInt(CmbPlantFeederOverHead.Value);
					OverHeaddetail.JobOrderOverHeadId = Conversion.ToInt(cmbJobOrderOverHead.Value);
					OverHeaddetail.DocumentTypeId = 1514;
					OverHeaddetail.EntryDate = DateTime.Now;
					OverHeaddetail.ModifyDate = DateTime.Now;
					OverHeaddetail.ApprovedDate = DateTime.Now;
					OverHeaddetail.EntryUserId = UserAccount.ID;
					OverHeaddetail.ModifyUserId = UserAccount.ID;
					OverHeaddetail.OrganizationId = UserAccount.OrganizationId;
					OverHeaddetail.CompanyId = UserAccount.CompanyId;
					OverHeaddetail.BranchesId = UserAccount.BranchesId;
					OverHeaddetail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					OverHeaddetail.JobOrderOverHeadId = Conversion.ToInt(r.Cells["JobOrderOverHeadId"].Value);
					OverHeaddetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					OverHeaddetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					OverHeaddetail.CharOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
					OverHeaddetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					OverHeaddetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					OverHeaddetail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					OverHeaddetail.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					OverHeaddetail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					OverHeaddetail.RemarksOH = Conversion.ToString(r.Cells["Remarks"].Value);
					OverHeaddetail.ActionId = 3;
					RemovedOverheadList.Add(OverHeaddetail);
					tableOverHeads.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					tableOverHeads.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdDetailOverhead.UpdateData();
			JobOrderOverHeadLock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailOverhead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdDetailOverhead.UpdateData();
			GridEXRow item = grdDetailOverhead.CurrentRow;
			if (e.Column.Key == "ItemQty" || e.Column.Key == "ItemRate")
			{
				if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && Conversion.ToDouble(item.Cells["ItemRate"].Value) != 0.0)
				{
					double Amount = Conversion.ToDouble(item.Cells["ItemQty"].Value) * Conversion.ToDouble(item.Cells["ItemRate"].Value);
					item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			else if (e.Column.Key == "Amount")
			{
				item.Cells["ItemQty"].Value = 0;
				item.Cells["ItemRate"].Value = 0;
			}
			grdDetailOverhead.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdatePlantAndJobOrderInOHGrid()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetailOverhead.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int JobOrderNoId = Conversion.ToInt(cmbJobOrderOverHead.Value);
				int PlantId = Conversion.ToInt(CmbPlantFeederOverHead.Value);
				string Plant = Conversion.ToString((object)((Control)(object)CmbPlantFeederOverHead).Text);
				GridEXRow[] rows = grdDetailOverhead.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["InvProductionJobOrderId"].Value = JobOrderNoId;
					obj.Cells["PlantId"].Value = PlantId;
					obj.Cells["Plant"].Value = Plant;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHistoryOverHead(int NoOfRecords = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			OverHeadHistorydt.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1514;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			OverHeadHistorydt = InvFoodProductionOverHeads.FormHistory(obj);
			if (OverHeadHistorydt.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ProductionJobOrderId", typeof(int));
				dt.Columns.Add("ProductionJobOrder", typeof(string));
				dt.Columns.Add("TotalItemQty", typeof(double));
				dt.Columns.Add("TotalExpense", typeof(double));
				dt.Columns.Add("AverageItemRate", typeof(double));
				dt.Columns.Add("IsApproved");
				foreach (var row2 in (from row in OverHeadHistorydt.AsEnumerable()
					group row by row["ProductionJobOrderId"]).Select(delegate(IGrouping<object, DataRow> group)
				{
					double totalItemQty = group.Sum((DataRow row) => Conversion.ToDouble(row["ItemQty"]));
					double totalExpense = group.Sum((DataRow row) => Conversion.ToDouble(row["Amount"]));
					double averageItemRate = group.Average((DataRow row) => Conversion.ToDouble(row["ItemRate"]));
					return new
					{
						InvProductionJobOrderId = group.Key,
						ProductionJobOrder = group.First()["ProductionJobOrderNo"],
						TotalItemQty = totalItemQty,
						TotalExpense = totalExpense,
						AverageItemRate = averageItemRate,
						IsApproved = group.First()["JobOrderStatus"]
					};
				}))
				{
					dt.Rows.Add(row2.InvProductionJobOrderId, row2.ProductionJobOrder, row2.TotalItemQty, row2.TotalExpense, row2.AverageItemRate, row2.IsApproved);
				}
				grdOverHeadHistory.DataSource = dt;
				grdOverHeadHistory.RetrieveStructure();
				OverHeadGridSetting();
			}
			else
			{
				grdOverHeadHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadGridSetting()
	{
		try
		{
			grdOverHeadHistory.RootTable.Columns["ProductionJobOrderId"].Visible = false;
			grdOverHeadHistory.RootTable.Columns["ProductionJobOrder"].EditType = (EditType)0;
			grdOverHeadHistory.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			grdOverHeadHistory.RootTable.Columns["ProductionJobOrder"].FilterEditType = (FilterEditType)1;
			grdOverHeadHistory.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)1;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].FormatString = "#,##0.##";
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].TotalFormatString = "#,##0.##";
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHeadHistory.RootTable.Columns.Add("Edit");
			grdOverHeadHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOverHeadHistory.RootTable.Columns["Edit"].Width = 50;
			grdOverHeadHistory.RootTable.Columns["Edit"].Position = 0;
			grdOverHeadHistory.RootTable.Columns.Add("Print");
			grdOverHeadHistory.RootTable.Columns["Print"].Caption = "Print";
			grdOverHeadHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdOverHeadHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHeadHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdOverHeadHistory.RootTable.Columns["Print"].Width = 50;
			grdOverHeadHistory.RootTable.Columns["Print"].Position = 1;
			grdOverHeadHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(grdOverHeadHistory);
			ctrlGrdBar9_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl4.SelectedIndex == 1)
		{
			BindGridHistoryOverHead(50);
		}
	}

	private void grdOverHead_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["IsApproved"].Value) != "True")
			{
				btnSaveOverHead.Visible = false;
				btnUpdateOverHead.Visible = true;
				RecOverHeadId = Conversion.ToInt((object)grdOverHeadHistory.CurrentRow.Cells["ProductionJobOrderId"].Value.ToString());
				ReadByIdOverHeadJobOrderWise(RecOverHeadId);
				return;
			}
			throw new Exception("You Can't Update Approved Record");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdOverHeadHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow Item = grdOverHeadHistory.CurrentRow;
		if (Item == null || (int)Item.RowType == 5 || (int)Item.RowType != 0 || Item.Cells.Count <= 0)
		{
			return;
		}
		DataTable dtDetail = InvFoodProductionOverHeads.GetByJobOrderID(Conversion.ToInt(Item.Cells["ProductionJobOrderId"].Value));
		if (dtDetail.Rows.Count > 0)
		{
			DataTable dt = new DataTable();
			dt.Clear();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ProductionJobOrderId", typeof(int));
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("GLAccount", typeof(string));
			dt.Columns.Add("BrandName", typeof(string));
			dt.Columns.Add("BrandUom", typeof(string));
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("ItemRate", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks", typeof(string));
			for (int i = 0; i < dtDetail.Rows.Count; i++)
			{
				dt.Rows.Add(dtDetail.Rows[i]["Id"], dtDetail.Rows[i]["ProductionJobOrderId"], dtDetail.Rows[i]["DocNo"], Conversion.ToDateTime(dtDetail.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtDetail.Rows[i]["AccountTitle"], dtDetail.Rows[i]["BrandName"], dtDetail.Rows[i]["BrandUom"], dtDetail.Rows[i]["ItemQty"], dtDetail.Rows[i]["ItemRate"], dtDetail.Rows[i]["Amount"], dtDetail.Rows[i]["RemarksOH"]);
			}
			grddetailOhHistory.DataSource = dt;
			grddetailOhHistory.RetrieveStructure();
			GrddetailOhHistoryGridSetting();
		}
	}

	private void GrddetailOhHistoryGridSetting()
	{
		try
		{
			grddetailOhHistory.RootTable.Columns["Id"].Visible = false;
			grddetailOhHistory.RootTable.Columns["ProductionJobOrderId"].Visible = false;
			grddetailOhHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grddetailOhHistory.RootTable.Columns["GLAccount"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["BrandName"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["BrandUom"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grddetailOhHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grddetailOhHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grddetailOhHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (!(Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["IsApproved"].Value) != "True"))
				{
					throw new Exception("You Can't Update Approved Record");
				}
				btnSaveOverHead.Visible = false;
				btnUpdateOverHead.Visible = true;
				RecOverHeadId = Conversion.ToInt((object)grdOverHeadHistory.CurrentRow.Cells["ProductionJobOrderId"].Value.ToString());
				ReadByIdOverHeadJobOrderWise(RecOverHeadId);
			}
			else if (e.Column.Key == "Print")
			{
				GenerateOverHeadPrint(Conversion.ToInt(grdOverHeadHistory.CurrentRow.Cells["ProductionJobOrderId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPlantFeederOverHead_Leave(object sender, EventArgs e)
	{
	}

	private bool FormValidationOfOverHead()
	{
		if (txtDocNoOverHead.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Doc No. Field Required");
			txtDocNoOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow == null)
		{
			MessageBox.Show("Job Order No. Field Required");
			cmbJobOrderOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeederOverHead).ActiveRow == null)
		{
			MessageBox.Show("Plant Field Required");
			CmbPlantFeederOverHead.Focus();
			return false;
		}
		return true;
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnVoucherOH_Click(object sender, EventArgs e)
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

	private void btnVoucherOH_Click_1(object sender, EventArgs e)
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

	private void btnPrintOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow != null)
			{
				GenerateOverHeadPrint(Conversion.ToInt(cmbJobOrderOverHead.Value));
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateOverHeadPrint(int JobOrderId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionReports.InvFoodProductionOverHeadReportByJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = JobOrderId
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For DisPlay");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1514-FoodProductionOverHeadJobOrderWise_Slip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar9_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOverHeadHistory).Name);
			if (fs != null)
			{
				grdOverHeadHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadOverHeadsFromJobOrder_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow != null)
			{
				LoadOverheads = new LoadOverHeadsDataByJobOrder(UserAccount);
				LoadOverheads.JobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value);
				LoadOverheads.ShowDialog();
				LoadOverheads.BringToFront();
				LoadOverHeadsDataFromJobOrder();
				return;
			}
			cmbJobOrderOverHead.Focus();
			throw new Exception("Select Job Order No First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadOverHeadsDataFromJobOrder()
	{
		try
		{
			DataTable dtLoadOHFromJobOrder = LoadOverheads.dtIssuance;
			if (dtLoadOHFromJobOrder.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtLoadOHFromJobOrder.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grdDetailOverhead.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtLoadOHFromJobOrder.Rows[i]["Id"]) == Conversion.ToInt(item.Cells["JobOrderOverHeadId"].Value))
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					tableOverHeads.Rows.Add(0, dtLoadOHFromJobOrder.Rows[i]["Id"], DateTime.Now, 0, dtLoadOHFromJobOrder.Rows[i]["ProductionJobOrderId"], dtLoadOHFromJobOrder.Rows[i]["CharOfAccountId"], dtLoadOHFromJobOrder.Rows[i]["OverHeadAccount"], dtLoadOHFromJobOrder.Rows[i]["BrandId"], dtLoadOHFromJobOrder.Rows[i]["BrandName"], dtLoadOHFromJobOrder.Rows[i]["BrandUomId"], dtLoadOHFromJobOrder.Rows[i]["BrandUom"], dtLoadOHFromJobOrder.Rows[i]["BalQty"], Conversion.ToDouble(dtLoadOHFromJobOrder.Rows[i]["ItemRate"]), dtLoadOHFromJobOrder.Rows[i]["BalAmount"], dtLoadOHFromJobOrder.Rows[i]["RemarksOH"]);
				}
			}
			grdDetailOverhead.DataSource = tableOverHeads;
			grdDetailOverhead.RetrieveStructure();
			grdDetailOverheadSettings();
			((Control)(object)cmbJobOrderOverHead).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory(int NoOfRecord = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters stockConversion = new ReportsParameters();
			stockConversion.OrganizationId = UserAccount.OrganizationId;
			stockConversion.CompanyId = UserAccount.CompanyId;
			stockConversion.BranchesId = UserAccount.BranchesId;
			stockConversion.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			stockConversion.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!stockConversion.CanViewAllRecord)
			{
				stockConversion.EntryUser = UserAccount.ID;
			}
			stockConversion.NoOfRecords = NoOfRecord;
			DataTable dthis = InvFoodProduction.FormHistory(stockConversion);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId", typeof(int));
				dtHistory.Columns.Add("DocNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryType", typeof(string));
				dtHistory.Columns.Add("JobOrderNo");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryUser", typeof(string));
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser", typeof(string));
				dtHistory.Columns.Add("ApprovedStatus");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocumentTypeId"], dthis.Rows[i]["DocCode"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryType"], dthis.Rows[i]["ProductionJobOrderNo"], Conversion.ToDateTime(dthis.Rows[i]["EntryDate"]).ToShortDateString(), dthis.Rows[i]["EntryUserName"], Conversion.ToDateTime(dthis.Rows[i]["ModifyDate"]).ToShortDateString(), dthis.Rows[i]["ModifyUserName"], Conversion.ToBool(dthis.Rows[i]["IsApproved"]) ? "Approved" : "Not Approved", dthis.Rows[i]["MainRemarks"]);
				}
				DatagridHistory.DataSource = dtHistory;
				DatagridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
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
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (!(Conversion.ToString(DatagridHistory.CurrentRow.Cells["ApprovedStatus"].Value) != "Approved"))
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (DatagridHistory.CurrentRow.Cells["EntryType"].Value.ToString() == "Input")
				{
					((Control)(object)CmbJobOrderNoInput).Enabled = false;
					RecId = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadByIdInput(RecId);
				}
				else
				{
					((Control)(object)CmbJobOrderNoOutput).Enabled = false;
					RecIdOutPut = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadByIdOutPut(RecIdOutPut);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			DatagridHistoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetail()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (DatagridHistory.CurrentRow == null || (int)DatagridHistory.CurrentRow.RowType != 0 || DatagridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
			if (SC != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouseName");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("Weight", typeof(decimal));
				dt.Columns.Add("ItemRate", typeof(decimal));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Amount", typeof(decimal));
				dt.Columns.Add("NetRate", typeof(decimal));
				dt.Columns.Add("NetAmount", typeof(decimal));
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].JobLotCode, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].PackingType, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].Rate, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].NetRate, SC.invFoodProductionDetails[i].TotalAmount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
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
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbsummeryJobOrderNo).Text = string.Empty;
			cmbsummeryJobOrderNo.Focus();
			grdSummery.ClearStructure();
			grdSummeryReport.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSummary_Click(object sender, EventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			obj.PlantId = Conversion.ToInt(CmbPlantNameSummary.Value);
			dtSummerReport = InvFoodProduction.InvFoodProductionRecoverySummeryReport(obj);
			if (dtSummerReport.Rows.Count > 0)
			{
				DataTable dtcol2 = new DataTable();
				dtcol2.Columns.Add("DocNo", typeof(string));
				dtcol2.Columns.Add("DocDate", typeof(DateTime));
				dtcol2.Columns.Add("ProductionJobOrderNo", typeof(string));
				dtcol2.Columns.Add("PlantName", typeof(string));
				dtcol2.Columns.Add("SortNo", typeof(int));
				dtcol2.Columns.Add("TranType", typeof(string));
				dtcol2.Columns.Add("ItemName", typeof(string));
				dtcol2.Columns.Add("UOMCode", typeof(string));
				dtcol2.Columns.Add("Remarks", typeof(string));
				dtcol2.Columns.Add("JobLotCode", typeof(string));
				dtcol2.Columns.Add("NetQty", typeof(double));
				dtcol2.Columns.Add("NetWeight", typeof(double));
				dtcol2.Columns.Add("Amount", typeof(double));
				dtcol2.Columns.Add("TotalAmountBySortNo", typeof(double));
				dtcol2.Columns.Add("TranRate", typeof(double));
				dtcol2.Columns.Add("TotalWeightBySortNo", typeof(double));
				dtcol2.Columns.Add("WeightPrcnt", typeof(double));
				dtcol2.Columns.Add("OutPutPrcnt", typeof(double));
				dtcol2.Columns.Add("BpGgPrcnt", typeof(double));
				dtcol2.Columns.Add("RmWeightTotal", typeof(double));
				dtcol2.Columns.Add("RMAmountTotal", typeof(double));
				dtcol2.Columns.Add("RmAvgRate", typeof(double));
				dtcol2.Columns.Add("BpWeightTotal", typeof(double));
				dtcol2.Columns.Add("BpAmountTotal", typeof(double));
				dtcol2.Columns.Add("BpAvgRate", typeof(double));
				dtcol2.Columns.Add("FgWeightTotal", typeof(double));
				dtcol2.Columns.Add("FgItemRate", typeof(double));
				dtcol2.Columns.Add("FgAmountTotal", typeof(double));
				dtcol2.Columns.Add("BpFgQty", typeof(double));
				dtcol2.Columns.Add("BpFgWeight", typeof(double));
				dtcol2.Columns.Add("BpFgAmount", typeof(double));
				dtcol2.Columns.Add("OhAmount", typeof(double));
				dtcol2.Columns.Add("FgOhAmount", typeof(double));
				dtcol2.Columns.Add("FgOhRate", typeof(double));
				dtcol2.Columns.Add("NetRate", typeof(double));
				dtcol2.Rows.Clear();
				for (int j = 0; j < dtSummerReport.Rows.Count; j++)
				{
					dtcol2.Rows.Add(dtSummerReport.Rows[j]["DocNo"], Conversion.ToDateTime(dtSummerReport.Rows[j]["DocDate"]).ToShortDateString(), dtSummerReport.Rows[j]["ProductionJobOrderNo"], dtSummerReport.Rows[j]["PlantName"], dtSummerReport.Rows[j]["SortNo"], dtSummerReport.Rows[j]["TranType"], dtSummerReport.Rows[j]["ItemName"], dtSummerReport.Rows[j]["UOMCode"], dtSummerReport.Rows[j]["Remarks"], dtSummerReport.Rows[j]["JobLotCode"], dtSummerReport.Rows[j]["NetQty"], dtSummerReport.Rows[j]["NetWeight"], dtSummerReport.Rows[j]["Amount"], dtSummerReport.Rows[j]["TotalAmountBySortNo"], dtSummerReport.Rows[j]["TranRate"], dtSummerReport.Rows[j]["TotalWeightBySortNo"], dtSummerReport.Rows[j]["WeightPrcnt"], dtSummerReport.Rows[j]["OutPutPrcnt"], dtSummerReport.Rows[j]["BpGgPrcnt"], dtSummerReport.Rows[j]["RmWeightTotal"], dtSummerReport.Rows[j]["RMAmountTotal"], dtSummerReport.Rows[j]["RmAvgRate"], dtSummerReport.Rows[j]["BpWeightTotal"], dtSummerReport.Rows[j]["BpAmountTotal"], dtSummerReport.Rows[j]["BpAvgRate"], dtSummerReport.Rows[j]["FgWeightTotal"], dtSummerReport.Rows[j]["FgItemRate"], dtSummerReport.Rows[j]["FgAmountTotal"], dtSummerReport.Rows[j]["BpFgQty"], dtSummerReport.Rows[j]["BpFgWeight"], dtSummerReport.Rows[j]["BpFgAmount"], dtSummerReport.Rows[j]["OhAmount"], dtSummerReport.Rows[j]["FgOhAmount"], dtSummerReport.Rows[j]["FgOhRate"], dtSummerReport.Rows[j]["NetRate"]);
				}
				grdSummeryReport.DataSource = dtcol2;
				grdSummeryReport.RetrieveStructure();
				SummeryHistoryGridSetting();
			}
			DataTable dt = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("SortNo", typeof(int));
				dtcol.Columns.Add("TranType", typeof(string));
				dtcol.Columns.Add("NetQty", typeof(double));
				dtcol.Columns.Add("NetWeight", typeof(double));
				dtcol.Columns.Add("Amount", typeof(double));
				dtcol.Columns.Add("AvgRate", typeof(double));
				dtcol.Columns.Add("AvgRateWoExp", typeof(double));
				dtcol.Columns.Add("ByProductOverHeads", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["SortNo"], dt.Rows[i]["TranType"], dt.Rows[i]["NetQty"], dt.Rows[i]["NetWeight"], dt.Rows[i]["Amount"], dt.Rows[i]["AvgRate"], dt.Rows[i]["AvgRateWoExp"], dt.Rows[i]["ByProductOverHeads"]);
				}
				grdSummery.DataSource = dtcol;
				grdSummery.RetrieveStructure();
				grdSummery.RootTable.Columns["TranType"].Width = 130;
				grdSummery.RootTable.Columns["SortNo"].Width = 80;
				grdSummery.RootTable.Columns["NetQty"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["NetQty"].FormatString = "#,##";
				grdSummery.RootTable.Columns["NetQty"].TotalFormatString = "#,##";
				grdSummery.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["NetWeight"].FormatString = "#,##";
				grdSummery.RootTable.Columns["NetWeight"].TotalFormatString = "#,##";
				grdSummery.RootTable.Columns["ByProductOverHeads"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["ByProductOverHeads"].FormatString = "#,##";
				grdSummery.RootTable.Columns["ByProductOverHeads"].TotalFormatString = "#,##";
				grdSummery.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdSummery.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				if (!formright.DoHaveCanRateandAmount)
				{
					grdSummery.RootTable.Columns["AvgRate"].Visible = false;
					grdSummery.RootTable.Columns["Amount"].Visible = false;
				}
			}
			else
			{
				grdSummery.DataSource = null;
			}
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbsummeryJobOrderNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbsummeryJobOrderNo).ActiveRow != null)
			{
				DataTable dt = InvProductionJobOrder.ReadPlantDetailByJobOrderId(Conversion.ToInt(cmbsummeryJobOrderNo.Value));
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, CmbPlantNameSummary, "PlantId", "PlantName", "Plant Name", false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SummeryHistoryGridSetting()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdSummeryReport.RootTable.Columns["TranRate"].Visible = false;
				grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].Visible = false;
				grdSummeryReport.RootTable.Columns["Amount"].Visible = false;
			}
			grdSummeryReport.GroupByBoxVisible = true;
			grdSummeryReport.RootTable.Groups.Add("TranType");
			grdSummeryReport.RootTable.GroupTotals = (GroupTotals)2;
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].Visible = false;
			grdSummeryReport.RootTable.Columns["TranRate"].Visible = false;
			grdSummeryReport.RootTable.Columns["TotalWeightBySortNo"].Visible = false;
			grdSummeryReport.RootTable.Columns["RmWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["BpWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["FgWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["NetQty"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["NetQty"].FormatString = "#,##0.##";
			grdSummeryReport.RootTable.Columns["NetQty"].TotalFormatString = "#,##0.##";
			grdSummeryReport.RootTable.Columns["NetQty"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdSummeryReport.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdSummeryReport.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].FormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].TotalFormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["TranRate"].AggregateFunction = (AggregateFunction)3;
			grdSummeryReport.RootTable.Columns["TranRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["TranRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["TranRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["RMAmountTotal"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["RMAmountTotal"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["RMAmountTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["RMAmountTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["RmAvgRate"].AggregateFunction = (AggregateFunction)3;
			grdSummeryReport.RootTable.Columns["RmAvgRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["RmAvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["RmAvgRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["BpAmountTotal"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["BpAmountTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["BpAmountTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["BpAvgRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["BpAvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["BpAvgRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["FgItemRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["FgItemRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgItemRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgAmountTotal"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["FgAmountTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgAmountTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["BpFgAmount"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["BpFgAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["BpFgAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["OhAmount"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["OhAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["OhAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgOhAmount"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["FgOhAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgOhAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummeryReport.RootTable.Columns["FgOhRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["FgOhRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["FgOhRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSummeryReport.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdSummeryReport);
			ctrlGrdBar6_Load(null, null);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummery).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummery).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdSummery.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummeryReport).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummeryReport).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdSummeryReport.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnProductionRecoveryReport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionReports.InvFoodProduction_Summery_WithOutValue_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1513-FoodProductionSummery_WithOutValue.rpt");
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

	private void btnGeneralSettlement_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Expected O, but got Unknown
		try
		{
			DataTable dtSettlement = ProductionReports.FoodProductionSettlement(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				Activity = "InPutOutPut"
			});
			if (dtSettlement.Rows.Count > 0)
			{
				DataTable dtInput = new DataTable();
				dtInput.Columns.Add("Id");
				dtInput.Columns.Add("RefDocumentTypeId");
				dtInput.Columns.Add("RefDocIdNo");
				dtInput.Columns.Add("DocDate");
				dtInput.Columns.Add("DocNo");
				dtInput.Columns.Add("WareHouseId");
				dtInput.Columns.Add("WareHouse");
				dtInput.Columns.Add("ItemId");
				dtInput.Columns.Add("ItemName");
				dtInput.Columns.Add("PackUomId");
				dtInput.Columns.Add("PackUom");
				dtInput.Columns.Add("JobLotId");
				dtInput.Columns.Add("JobLot");
				dtInput.Columns.Add("PackingTypeId");
				dtInput.Columns.Add("PackingType");
				dtInput.Columns.Add("Quantity", typeof(double));
				dtInput.Columns.Add("Weight", typeof(double));
				dtInput.Columns.Add("Rate", typeof(double));
				dtInput.Columns.Add("RateUOMId");
				dtInput.Columns.Add("RateUOM");
				dtInput.Columns.Add("ItemAmount", typeof(double));
				dtInput.Columns.Add("Remarks");
				DataTable dtOutPut = new DataTable();
				dtOutPut.Columns.Add("Id");
				dtOutPut.Columns.Add("DetailId");
				dtOutPut.Columns.Add("WIPAccountId");
				dtOutPut.Columns.Add("EntryTypeId");
				dtOutPut.Columns.Add("EntryType");
				dtOutPut.Columns.Add("DocDate");
				dtOutPut.Columns.Add("DocNo");
				dtOutPut.Columns.Add("WareHouseId");
				dtOutPut.Columns.Add("WareHouse");
				dtOutPut.Columns.Add("ItemId");
				dtOutPut.Columns.Add("ItemName");
				dtOutPut.Columns.Add("PackUomId");
				dtOutPut.Columns.Add("PackUom");
				dtOutPut.Columns.Add("JobLotId");
				dtOutPut.Columns.Add("JobLot");
				dtOutPut.Columns.Add("PackingTypeId");
				dtOutPut.Columns.Add("PackingType");
				dtOutPut.Columns.Add("Quantity", typeof(double));
				dtOutPut.Columns.Add("Weight", typeof(double));
				dtOutPut.Columns.Add("Rate", typeof(double));
				dtOutPut.Columns.Add("ItemAmount", typeof(double));
				dtOutPut.Columns.Add("RateWithoutExp", typeof(double));
				dtOutPut.Columns.Add("AmountWithoutExp", typeof(double));
				dtOutPut.Columns.Add("NetRate", typeof(double));
				dtOutPut.Columns.Add("RateUOMId");
				dtOutPut.Columns.Add("RateUOM");
				dtOutPut.Columns.Add("ItemOhCost", typeof(double));
				dtOutPut.Columns.Add("GeneralOhCost", typeof(double));
				dtOutPut.Columns.Add("TotalAmount", typeof(double));
				dtOutPut.Columns.Add("Remarks");
				dtInput.Rows.Clear();
				for (int i = 0; i < dtSettlement.Rows.Count; i++)
				{
					if (Conversion.ToString(dtSettlement.Rows[i]["EntryType"]) == "Input")
					{
						dtInput.Rows.Add(dtSettlement.Rows[i]["Id"], dtSettlement.Rows[i]["RefDocumentTypeId"], dtSettlement.Rows[i]["RefDocNoId"], Conversion.ToDateTime(dtSettlement.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), dtSettlement.Rows[i]["DocCode"], dtSettlement.Rows[i]["WarehouseId"], dtSettlement.Rows[i]["WareHouseName"], dtSettlement.Rows[i]["ItemId"], dtSettlement.Rows[i]["ItemName"], dtSettlement.Rows[i]["PackUomId"], dtSettlement.Rows[i]["PackUom"], dtSettlement.Rows[i]["JobLotId"], dtSettlement.Rows[i]["JobLotDescription"], dtSettlement.Rows[i]["PackingtypeId"], dtSettlement.Rows[i]["PackTypeDesc"], dtSettlement.Rows[i]["Qty"], dtSettlement.Rows[i]["Weight"], dtSettlement.Rows[i]["Rate"], dtSettlement.Rows[i]["RateUOMId"], dtSettlement.Rows[i]["RateUom"], dtSettlement.Rows[i]["Amount"], dtSettlement.Rows[i]["Remarks"]);
					}
					if (Conversion.ToString(dtSettlement.Rows[i]["EntryType"]) == "Output")
					{
						dtOutPut.Rows.Add(dtSettlement.Rows[i]["Id"], dtSettlement.Rows[i]["DetailId"], dtSettlement.Rows[i]["WIPAccountId"], dtSettlement.Rows[i]["EntryTypeId"], dtSettlement.Rows[i]["EntryTypeDetail"], Conversion.ToDateTime(dtSettlement.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), dtSettlement.Rows[i]["DocCode"], dtSettlement.Rows[i]["WarehouseId"], dtSettlement.Rows[i]["WareHouseName"], dtSettlement.Rows[i]["ItemId"], dtSettlement.Rows[i]["ItemName"], dtSettlement.Rows[i]["PackUomId"], dtSettlement.Rows[i]["PackUom"], dtSettlement.Rows[i]["JobLotId"], dtSettlement.Rows[i]["JobLotDescription"], dtSettlement.Rows[i]["PackingtypeId"], dtSettlement.Rows[i]["PackTypeDesc"], dtSettlement.Rows[i]["Qty"], dtSettlement.Rows[i]["Weight"], dtSettlement.Rows[i]["Rate"], dtSettlement.Rows[i]["Amount"], dtSettlement.Rows[i]["Rate"], dtSettlement.Rows[i]["Amount"], dtSettlement.Rows[i]["NetRate"], dtSettlement.Rows[i]["RateUOMId"], dtSettlement.Rows[i]["RateUom"], dtSettlement.Rows[i]["ItemOhCost"], dtSettlement.Rows[i]["GeneralOhCost"], dtSettlement.Rows[i]["TotalAmount"], dtSettlement.Rows[i]["Remarks"]);
					}
				}
				grdInputsettlement.DataSource = dtInput;
				grdInputsettlement.RetrieveStructure();
				grdInputSettelmentSetting();
				grdOutPutSettlement.DataSource = dtOutPut;
				grdOutPutSettlement.RetrieveStructure();
				grdOutPutSettelmentSetting();
			}
			else
			{
				grdInputsettlement.ClearStructure();
				grdOutPutSettlement.ClearStructure();
			}
			DataTable dtOverHeads = ProductionReports.FoodProductionSettlement(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				Activity = "OverHeads"
			});
			if (dtOverHeads.Rows.Count > 0)
			{
				grdOverHeadSettlement.DataSource = dtOverHeads;
				grdOverHeadSettlement.RetrieveStructure();
				grdOverHeadsSettelmentSetting();
			}
			double NetWeight = 0.0;
			double FinishAmountAgainstJobOrderRate = 0.0;
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["EntryTypeId"].Value) == 2)
				{
					NetWeight += Conversion.ToDouble(item.Cells["Weight"].Value);
					FinishAmountAgainstJobOrderRate += Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				}
			}
			DataTable dtSummeryValue = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				NetWeight = NetWeight
			});
			if (dtSummeryValue.Rows.Count > 0 && dtSummeryValue != null)
			{
				double RMQty = 0.0;
				double RMWeight = 0.0;
				double RMAmount = 0.0;
				double BPQty = 0.0;
				double BPWeight = 0.0;
				double BPAmount = 0.0;
				double ByProductOverHeads = 0.0;
				double BpTotalAmountWithExpenses = 0.0;
				double FGQty = 0.0;
				double FGWeight = 0.0;
				double FGAmount = 0.0;
				double OverHeadAmount = 0.0;
				for (int j = 0; j < dtSummeryValue.Rows.Count; j++)
				{
					if (dtSummeryValue.Rows[j]["TranType"].ToString() == "Input Raw Material")
					{
						RMQty = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetQty"]);
						RMWeight = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetWeight"]);
						RMAmount = Conversion.ToDouble(dtSummeryValue.Rows[j]["Amount"]);
						txtItemQtyRM.Text = Math.Round(RMQty, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtNetWeightRM.Text = Math.Round(RMWeight, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtAmountRM.Text = Math.Round(RMAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
					}
					if (dtSummeryValue.Rows[j]["TranType"].ToString() == "Over Heads")
					{
						OverHeadAmount = Conversion.ToDouble(dtSummeryValue.Rows[j]["Amount"]);
						txtOverHeadsSettlement.Text = Math.Round(OverHeadAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
					}
					if (dtSummeryValue.Rows[j]["TranType"].ToString() == "OutPut ManualRate")
					{
						BPQty = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetQty"]);
						BPWeight = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetWeight"]);
						BPAmount = Conversion.ToDouble(dtSummeryValue.Rows[j]["Amount"]);
						ByProductOverHeads = Conversion.ToDouble(dtSummeryValue.Rows[j]["ByProductOverHeads"]);
						BpTotalAmountWithExpenses = BPAmount + ByProductOverHeads;
						txtItemQtyBP.Text = Math.Round(BPQty, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtNetWeightBP.Text = Math.Round(BPWeight, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtAmountBP.Text = Math.Round(BPAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtBPOverHead.Text = Math.Round(ByProductOverHeads, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtBPTotalAmountWithExpenses.Text = Math.Round(BpTotalAmountWithExpenses, 0, MidpointRounding.AwayFromZero).ToString("0,0");
					}
					if (dtSummeryValue.Rows[j]["TranType"].ToString() == "Output AutoRate")
					{
						FGQty = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetQty"]);
						FGWeight = Conversion.ToDouble(dtSummeryValue.Rows[j]["NetWeight"]);
						FGAmount = Conversion.ToDouble(dtSummeryValue.Rows[j]["Amount"]);
						AvgRate = Conversion.ToDouble(dtSummeryValue.Rows[j]["AvgRate"]);
						txtAvgRateFG.Text = Math.Round(Conversion.ToDouble(dtSummeryValue.Rows[j]["AvgRate"]), 3).ToString();
						txtFinishGoodRateWithOutExp.Text = Math.Round(Conversion.ToDouble(dtSummeryValue.Rows[j]["AvgRateWoExp"]), 3).ToString();
						txtItemQtyFG.Text = Math.Round(FGQty, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtNetWeightFG.Text = Math.Round(FGWeight, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtAmountFG.Text = Math.Round(FGAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtActualFinishGoodsAmount.Text = Math.Round(FGAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						double DifferanceAmount = FGAmount - FinishAmountAgainstJobOrderRate;
						txtFinishAmountAgainstJobOrder.Text = Math.Round(FinishAmountAgainstJobOrderRate, 0, MidpointRounding.AwayFromZero).ToString("0,0");
						txtDifferenceAmount.Text = Math.Round(DifferanceAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
					}
				}
			}
			if (Conversion.ToDouble((object)txtAmountFG.Text) > 0.0)
			{
				btnApproveSettlement.Visible = true;
			}
			else
			{
				btnApproveSettlement.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSettelmentSetting()
	{
		try
		{
			grdInputsettlement.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Id"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInputsettlement.RootTable.Columns["ItemId"].Visible = false;
			grdInputsettlement.RootTable.Columns["JobLotId"].Visible = false;
			grdInputsettlement.RootTable.Columns["WarehouseId"].Visible = false;
			grdInputsettlement.RootTable.Columns["PackingTypeId"].Visible = false;
			grdInputsettlement.RootTable.Columns["PackUomId"].Visible = false;
			grdInputsettlement.RootTable.Columns["RateUOMId"].Visible = false;
			grdInputsettlement.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdInputsettlement.RootTable.Columns["Quantity"].FormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Weight"].FormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Quantity"].TotalFormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputsettlement.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputsettlement.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputsettlement.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInputsettlement.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInputsettlement.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputsettlement.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdInputsettlement);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutSettelmentSetting()
	{
		try
		{
			grdOutPutSettlement.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WIPAccountId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["EntryTypeId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["NetRate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["EntryTypeId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["EntryType"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdOutPutSettlement.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackUomId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["NetRate"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Id"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["WIPAccountId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["DetailId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["ItemId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["EntryTypeId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["PackUomId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].AggregateFunction = (AggregateFunction)3;
			grdOutPutSettlement.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["Weight"].FormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["Quantity"].FormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutSettlement.RootTable.Columns["Weight"].TotalFormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["Quantity"].TotalFormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutSettlement.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdOutPutSettlement);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadsSettelmentSetting()
	{
		try
		{
			grdOverHeadSettlement.RootTable.Columns["Id"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["CharOfAccountId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["BrandId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["BrandUomId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadSettlement.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHeadSettlement.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHeadSettlement.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdOverHeadSettlement);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadSettlement_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GenerateOverHeadPrint(Conversion.ToInt(CmbJobOrderSettlement.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutSettlement_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdOutPutSettlement.UpdateData();
			GridEXRow item = grdOutPutSettlement.CurrentRow;
			if (!(e.Column.Key == "Rate"))
			{
				return;
			}
			double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
			double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomlst.Rows[i]["ItemId"]) == Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString()) && Conversion.ToInt(dtuomlst.Rows[i]["Equivalent"]) == 40)
				{
					item.Cells["RateUOMId"].Value = Conversion.ToInt(dtuomlst.Rows[i]["Id"]);
					item.Cells["RateUOM"].Value = Conversion.ToInt(dtuomlst.Rows[i]["Equivalent"]);
					break;
				}
			}
			double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
			double Amount = Weight / RateUOM * Rate;
			item.Cells["ItemAmount"].Value = Amount;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplyAvgRateFG_Click(object sender, EventArgs e)
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		try
		{
			double NetWeight = 0.0;
			double ItemAmount = 0.0;
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["EntryTypeId"].Value) == 2)
				{
					NetWeight += Conversion.ToDouble(item.Cells["Weight"].Value);
				}
				if (Conversion.ToDouble((object)txtAmountBP.Text.Trim()) == 0.0 && Conversion.ToInt(item.Cells["EntryTypeId"].Value) != 2)
				{
					ItemAmount += Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				}
			}
			if (ItemAmount > 0.0)
			{
				txtAmountBP.Text = Conversion.ToString((object)ItemAmount);
			}
			DataTable dt = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				NetWeight = NetWeight
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["TranType"].ToString() == "Output AutoRate")
					{
						AvgRate = Conversion.ToDouble(dt.Rows[i]["AvgRate"]);
						AvgRateWithoutExp = Conversion.ToDouble(dt.Rows[i]["AvgRateWoExp"]);
						txtAvgRateFG.Text = Math.Round(Conversion.ToDouble(dt.Rows[i]["AvgRate"]), 3).ToString();
					}
				}
				ProporationFinishGoodsSettlement();
			}
			else
			{
				txtAvgRateFG.Text = "0";
				txtFinishGoodRateWithOutExp.Text = "0";
			}
			if (Conversion.ToDouble((object)txtAmountFG.Text) > 0.0)
			{
				btnApproveSettlement.Visible = true;
			}
			else
			{
				btnApproveSettlement.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProporationFinishGoodsSettlement()
	{
		try
		{
			if (txtAvgRateFG.Text == string.Empty || txtAvgRateFG.Text == "0")
			{
				MessageBox.Show("Finish Goods AvgRate Not Found Please check!");
				return;
			}
			grdOutPutSettlement.UpdateData();
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				double Weight = 0.0;
				double Rate = 0.0;
				double RateUOM = 0.0;
				double ItemOhCost = 0.0;
				double GeneralOhCost = 0.0;
				double RiceAmountWithOutExp = 0.0;
				double FinishAmountWithExp = 0.0;
				double BrandRate = 0.0;
				double ByProductAmountWithExp = 0.0;
				double ByProductRate = 0.0;
				r.BeginEdit();
				if (Conversion.ToInt(r.Cells["RateUOMId"].Value) == 0)
				{
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt((object)40) + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("40KG RateUom not define please check");
							return;
						}
						r.Cells["RateUOMId"].Value = Conversion.ToInt(dr[0].ItemArray[0]);
						r.Cells["RateUOM"].Value = Conversion.ToInt(dr[0].ItemArray[2]);
					}
				}
				if (Conversion.ToInt((object)r.Cells["EntryTypeId"].Value.ToString()) == 2)
				{
					Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					Rate = Conversion.ToDouble((object)AvgRateWithoutExp);
					RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					ItemOhCost = Conversion.ToDouble((object)r.Cells["ItemOhCost"].Value.ToString());
					GeneralOhCost = Conversion.ToDouble((object)r.Cells["GeneralOhCost"].Value.ToString());
					RiceAmountWithOutExp = Weight * Rate;
					FinishAmountWithExp = RiceAmountWithOutExp + ItemOhCost + GeneralOhCost;
					BrandRate = FinishAmountWithExp / Weight * RateUOM;
					r.Cells["RateWithoutExp"].Value = Rate * RateUOM;
					r.Cells["AmountWithoutExp"].Value = RiceAmountWithOutExp.ToString();
					r.Cells["NetRate"].Value = BrandRate;
					r.Cells["TotalAmount"].Value = FinishAmountWithExp.ToString();
					r.EndEdit();
				}
				else if (Conversion.ToInt((object)r.Cells["EntryTypeId"].Value.ToString()) != 2)
				{
					Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					Rate = Conversion.ToDouble((object)r.Cells["Rate"].Value.ToString());
					RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					ItemOhCost = Conversion.ToDouble((object)r.Cells["ItemOhCost"].Value.ToString());
					GeneralOhCost = Conversion.ToDouble((object)r.Cells["GeneralOhCost"].Value.ToString());
					RiceAmountWithOutExp = Weight / RateUOM * Rate;
					ByProductAmountWithExp = RiceAmountWithOutExp + ItemOhCost + GeneralOhCost;
					ByProductRate = ByProductAmountWithExp / Weight * RateUOM;
					r.Cells["RateWithoutExp"].Value = Rate;
					r.Cells["AmountWithoutExp"].Value = RiceAmountWithOutExp.ToString();
					r.Cells["NetRate"].Value = ByProductRate;
					r.Cells["TotalAmount"].Value = ByProductAmountWithExp.ToString();
					r.EndEdit();
				}
			}
			grdOutPutSettelmentSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		grdOutPutSettlement.UpdateData();
	}

	private void ResetSettlement()
	{
		try
		{
			btnApproveSettlement.Visible = false;
			txtItemQtyRM.Text = string.Empty;
			txtItemQtyBP.Text = string.Empty;
			txtItemQtyFG.Text = string.Empty;
			txtNetWeightRM.Text = string.Empty;
			txtNetWeightBP.Text = string.Empty;
			txtNetWeightFG.Text = string.Empty;
			txtAmountRM.Text = string.Empty;
			txtAmountBP.Text = string.Empty;
			txtAmountFG.Text = string.Empty;
			txtActualFinishGoodsAmount.Text = string.Empty;
			txtFinishAmountAgainstJobOrder.Text = string.Empty;
			txtDifferenceAmount.Text = string.Empty;
			txtBPOverHead.Text = string.Empty;
			txtBPPMCharges.Text = string.Empty;
			txtBPTotalAmountWithExpenses.Text = string.Empty;
			txtOverHeadsSettlement.Text = string.Empty;
			txtAvgRateFG.Text = string.Empty;
			txtFinishGoodRateWithOutExp.Text = string.Empty;
			((Control)(object)CmbJobOrderSettlement).Text = string.Empty;
			grdInputsettlement.ClearStructure();
			grdOutPutSettlement.ClearStructure();
			grdOverHeadSettlement.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetSettlement_Click(object sender, EventArgs e)
	{
		try
		{
			ResetSettlement();
			JobOrderNoFillForSummery();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateSettlement()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			InvFoodProduction obj = new InvFoodProduction();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
				if (Conversion.ToDouble(r.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
				{
					Conversiondetail.Id = Conversion.ToInt(r.Cells["DetailId"].Value);
					Conversiondetail.FoodProductionId = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.EntryTypeId = Conversion.ToInt(r.Cells["EntryTypeId"].Value);
					Conversiondetail.RefDocNoId = Conversion.ToInt((object)0);
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
					Conversiondetail.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					if (r.Cells["JobLotId"].Text == string.Empty || r.Cells["JobLotId"].Text == "0")
					{
						MessageBox.Show("JobLot Field Require in Output Detail");
						return;
					}
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Conversiondetail.Qty = Conversion.ToDecimal((object)r.Cells["Quantity"].Text);
					Conversiondetail.Weight = Conversion.ToDecimal(r.Cells["Weight"].Value);
					if (Conversiondetail.Weight == 0m)
					{
						throw new Exception("Weight Field Required");
					}
					Conversiondetail.Rate = Conversion.ToDecimal(r.Cells["RateWithoutExp"].Value);
					Conversiondetail.NetRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
					if (Conversiondetail.Rate == 0m)
					{
						throw new Exception("Rate Field Required");
					}
					if (Conversiondetail.NetRate == 0m)
					{
						throw new Exception("NetRate Field Required");
					}
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					if (Conversiondetail.RateUOMId == 0)
					{
						throw new Exception("RateUom Field Required");
					}
					Conversiondetail.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
					Conversiondetail.Amount = Conversion.ToDecimal(r.Cells["AmountWithoutExp"].Value);
					Conversiondetail.ItemOhCost = Conversion.ToDecimal(r.Cells["ItemOhCost"].Value);
					Conversiondetail.GeneralOhCost = Conversion.ToDecimal(r.Cells["GeneralOhCost"].Value);
					Conversiondetail.TotalAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					if (Conversiondetail.Amount == 0m)
					{
						throw new Exception("ItemAmount mustbe greater than zero Please Check");
					}
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					obj.invFoodProductionDetails.Add(Conversiondetail);
				}
			}
			InvFoodProduction.UpdateSettlement(obj);
			MessageBox.Show("Record has updated Successfully");
			ResetSettlement();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SettlementFinancials()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbJobOrderSettlement).ActiveRow == null)
			{
				MessageBox.Show("JobOrder Settlement Field is Required");
				CmbJobOrderSettlement.Focus();
				return;
			}
			if (((UltraGridBase)CmbDifferenceAccountTitle).ActiveRow == null)
			{
				MessageBox.Show("Difference Account Field is Required");
				CmbDifferenceAccountTitle.Focus();
				return;
			}
			int WorkInProccessAcId = 0;
			DataTable dt = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderSettlement.Value)
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				WorkInProccessAcId = Conversion.ToInt(dt.Rows[0]["WorkInProccessAcId"]);
				int VoucherId = CommonServices.VoucherHeadIdGet(Conversion.ToInt(CmbJobOrderSettlement.Value), 178);
				VoucherHead obj = new VoucherHead();
				obj.Id = VoucherId;
				obj.DocumentTypeSrNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				obj.VoucherDate = DateTime.Now;
				obj.VoucherCode = obj.VoucherCode;
				obj.DocumentTypeId = 178;
				obj.RefAccountId = WorkInProccessAcId;
				obj.AgainstAccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.BranchId = UserAccount.BranchesId;
				obj.voucherDetailList = new List<VoucherDetail>();
				string remarks = "";
				remarks = "Finish Goods ActualAmount " + txtActualFinishGoodsAmount.Text + " Finish Goods Amount against JobOrder Rates " + txtFinishAmountAgainstJobOrder.Text + " Difference Amount " + txtDifferenceAmount.Text;
				double DifferenceAmount = Conversion.ToDouble((object)txtDifferenceAmount.Text.Trim());
				obj.VoucherAmount = Math.Abs(DifferenceAmount);
				VoucherDetail vd = new VoucherDetail();
				vd.AccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				vd.AgainstAccountId = Conversion.ToInt((object)WorkInProccessAcId);
				vd.Comments = Conversion.ToString((object)remarks);
				if (DifferenceAmount > 0.0)
				{
					vd.DebitAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				else
				{
					vd.CreditAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				vd.OrderNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				obj.voucherDetailList.Add(vd);
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = Conversion.ToInt((object)WorkInProccessAcId);
				vd2.AgainstAccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				vd2.Comments = Conversion.ToString((object)remarks);
				if (DifferenceAmount > 0.0)
				{
					vd2.CreditAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				else
				{
					vd2.DebitAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				vd2.OrderNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				obj.voucherDetailList.Add(vd2);
				if (InvFoodProduction.SetDataForSettlementFinancials(obj) > 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
				return;
			}
			throw new Exception("WIP Account Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateSettlement_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Are you sure to Update Settlement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				UpdateSettlement();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecoveryReport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtSummerReport = InvFoodProduction.InvFoodProductionRecoverySummeryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value)
			});
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "1513A_ProductionSummeryWithValues.rpt");
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

	private void btnApproveSettlement_Click(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Settlement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.BranchesId = UserAccount.BranchesId;
				detail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				detail.Id = Conversion.ToInt(CmbJobOrderSettlement.Value);
				detail.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(detail);
				InvProductionJobOrder.JobOrderApproveList(val);
				MessageBox.Show("Record Approved Successfully");
				ResetSettlement();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnSettlementVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			int VoucherId = CommonServices.VoucherHeadIdGet(Conversion.ToInt(CmbJobOrderSettlement.Value), 178);
			if (VoucherId > 0)
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherId, 178);
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

	private void FoodProductionWithValuesNew_St_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Expected O, but got Unknown
		//IL_0975: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Expected O, but got Unknown
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Expected O, but got Unknown
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Expected O, but got Unknown
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Expected O, but got Unknown
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b42: Expected O, but got Unknown
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb0: Expected O, but got Unknown
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Expected O, but got Unknown
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb8: Expected O, but got Unknown
		//IL_0cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Expected O, but got Unknown
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3c: Expected O, but got Unknown
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Expected O, but got Unknown
		//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Expected O, but got Unknown
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Expected O, but got Unknown
		//IL_1013: Unknown result type (might be due to invalid IL or missing references)
		//IL_101d: Expected O, but got Unknown
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1054: Expected O, but got Unknown
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Expected O, but got Unknown
		//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bea: Expected O, but got Unknown
		//IL_1bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c01: Expected O, but got Unknown
		//IL_3deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df5: Expected O, but got Unknown
		//IL_45da: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e4: Expected O, but got Unknown
		//IL_45f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_45fb: Expected O, but got Unknown
		//IL_4608: Unknown result type (might be due to invalid IL or missing references)
		//IL_4612: Expected O, but got Unknown
		//IL_69d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_69da: Expected O, but got Unknown
		//IL_72a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_72b0: Expected O, but got Unknown
		//IL_72bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_72c7: Expected O, but got Unknown
		//IL_9276: Unknown result type (might be due to invalid IL or missing references)
		//IL_9280: Expected O, but got Unknown
		//IL_9bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_9bdd: Expected O, but got Unknown
		//IL_d480: Unknown result type (might be due to invalid IL or missing references)
		//IL_d48a: Expected O, but got Unknown
		//IL_d6f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_d6fd: Expected O, but got Unknown
		GridEXLayout grdStockConversionProductionDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Production.FoodProductionWithValuesNew_St));
		GridEXLayout grdinputhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdOutputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailOverhead_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance57 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		GridEXLayout grddetailOhHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHeadHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummeryReport_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummery_DesignTimeLayout = new GridEXLayout();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
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
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		GridEXLayout grdOverHeadSettlement_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutSettlement_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputsettlement_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance56 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.InPut = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdStockConversionProductionDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel72 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbPlantFeederInput = new UltraCombo();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label83 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.CmbWorkInProcess = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbWIPItem = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoInput = new UltraCombo();
		this.btnLoadInvoices = new System.Windows.Forms.Button();
		this.txtRemarksInput = new System.Windows.Forms.TextBox();
		this.panel63 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar10 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdinputhistory = new GridEX();
		this.panel57 = new System.Windows.Forms.Panel();
		this.panel59 = new System.Windows.Forms.Panel();
		this.grdInputDetail = new GridEX();
		this.panel58 = new System.Windows.Forms.Panel();
		this.toolStrip7 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.OutPut = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel30 = new System.Windows.Forms.Panel();
		this.panel64 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtDocNoOutPut = new System.Windows.Forms.TextBox();
		this.CmbPlantFeederOutput = new UltraCombo();
		this.label73 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.DocDateOutPut = new System.Windows.Forms.DateTimePicker();
		this.label71 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoOutput = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.txtBalWeightTotal = new System.Windows.Forms.TextBox();
		this.txtBalQtyTotal = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtRemarksHeaderOutPut = new System.Windows.Forms.TextBox();
		this.txtInPutTotalQty = new System.Windows.Forms.TextBox();
		this.txtInPutTotalWeight = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtOutQtyTotal = new System.Windows.Forms.TextBox();
		this.txtOutWeightTotal = new System.Windows.Forms.TextBox();
		this.txtAvgFinishGoodRate = new System.Windows.Forms.TextBox();
		this.btnApplyRate = new System.Windows.Forms.Button();
		this.txtFGRate = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.txtFGWeight = new System.Windows.Forms.TextBox();
		this.label75 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.btnNewOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOutPut = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnLoadOutPutFromjobOrder = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar11 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdOutputHistory = new GridEX();
		this.panel60 = new System.Windows.Forms.Panel();
		this.panel62 = new System.Windows.Forms.Panel();
		this.grdOutPutDetail = new GridEX();
		this.panel61 = new System.Windows.Forms.Panel();
		this.toolStrip8 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadAllOutputHistoy = new System.Windows.Forms.ToolStripButton();
		this.OverHead = new System.Windows.Forms.TabPage();
		this.panel67 = new System.Windows.Forms.Panel();
		this.tabControl4 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel68 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.grdDetailOverhead = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar12 = new grid.User_Conrtols.CtrlGrdBar();
		this.label47 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label102 = new System.Windows.Forms.Label();
		this.txtfinishQtyOh = new System.Windows.Forms.TextBox();
		this.label103 = new System.Windows.Forms.Label();
		this.txtfinishWeightOh = new System.Windows.Forms.TextBox();
		this.label97 = new System.Windows.Forms.Label();
		this.CmbPlantFeederOverHead = new UltraCombo();
		this.txtInputQtyTotalOH = new System.Windows.Forms.TextBox();
		this.label98 = new System.Windows.Forms.Label();
		this.txtInputWeightTotalOH = new System.Windows.Forms.TextBox();
		this.label85 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtOutputQtyTotalOH = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtOutputWeightTotalOH = new System.Windows.Forms.TextBox();
		this.cmbJobOrderOverHead = new UltraCombo();
		this.txtDocNoOverHead = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDocDateOverHead = new System.Windows.Forms.DateTimePicker();
		this.label39 = new System.Windows.Forms.Label();
		this.panel69 = new System.Windows.Forms.Panel();
		this.label44 = new System.Windows.Forms.Label();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.btnNewOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOverHead = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOH = new System.Windows.Forms.ToolStripButton();
		this.btnLoadOverHeadsFromJobOrder = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grddetailOhHistory = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar14 = new grid.User_Conrtols.CtrlGrdBar();
		this.label34 = new System.Windows.Forms.Label();
		this.grdOverHeadHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label101 = new System.Windows.Forms.Label();
		this.ctrlGrdBar9 = new grid.User_Conrtols.CtrlGrdBar();
		this.label89 = new System.Windows.Forms.Label();
		this.TransctionHistory = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel44 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel43 = new System.Windows.Forms.Panel();
		this.label91 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel16 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDocNumberTo = new System.Windows.Forms.TextBox();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label52 = new System.Windows.Forms.Label();
		this.txtfromDate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label90 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.Summary = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel48 = new System.Windows.Forms.Panel();
		this.grdSummeryReport = new GridEX();
		this.panel47 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdSummery = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.panel46 = new System.Windows.Forms.Panel();
		this.panel45 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel20 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbPlantNameSummary = new UltraCombo();
		this.label87 = new System.Windows.Forms.Label();
		this.cmbsummeryJobOrderNo = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnProductionRecoveryReport = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.Settlement = new System.Windows.Forms.TabPage();
		this.panel29 = new System.Windows.Forms.Panel();
		this.panel40 = new System.Windows.Forms.Panel();
		this.panel74 = new System.Windows.Forms.Panel();
		this.txtDifferenceAmount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.panel75 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtFinishAmountAgainstJobOrder = new System.Windows.Forms.TextBox();
		this.txtActualFinishGoodsAmount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel55 = new System.Windows.Forms.Panel();
		this.panel56 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.txtAvgRateFG = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.txtFinishGoodRateWithOutExp = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.panel53 = new System.Windows.Forms.Panel();
		this.panel54 = new System.Windows.Forms.Panel();
		this.label94 = new System.Windows.Forms.Label();
		this.txtOverHeadsSettlement = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.panel51 = new System.Windows.Forms.Panel();
		this.panel52 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.txtItemQtyRM = new System.Windows.Forms.TextBox();
		this.txtItemQtyBP = new System.Windows.Forms.TextBox();
		this.txtBPOverHead = new System.Windows.Forms.TextBox();
		this.txtNetWeightRM = new System.Windows.Forms.TextBox();
		this.txtAmountFG = new System.Windows.Forms.TextBox();
		this.txtBPTotalAmountWithExpenses = new System.Windows.Forms.TextBox();
		this.txtNetWeightFG = new System.Windows.Forms.TextBox();
		this.txtNetWeightBP = new System.Windows.Forms.TextBox();
		this.txtAmountBP = new System.Windows.Forms.TextBox();
		this.txtBPPMCharges = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtAmountRM = new System.Windows.Forms.TextBox();
		this.txtItemQtyFG = new System.Windows.Forms.TextBox();
		this.panel36 = new System.Windows.Forms.Panel();
		this.grdOverHeadSettlement = new GridEX();
		this.panel37 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel35 = new System.Windows.Forms.Panel();
		this.grdOutPutSettlement = new GridEX();
		this.panel34 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel33 = new System.Windows.Forms.Panel();
		this.panel50 = new System.Windows.Forms.Panel();
		this.grdInputsettlement = new GridEX();
		this.panel49 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel32 = new System.Windows.Forms.Panel();
		this.btnSettlementVoucher = new System.Windows.Forms.Button();
		this.CmbDifferenceAccountTitle = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.btnApproveSettlement = new System.Windows.Forms.Button();
		this.btnRecoveryReport = new System.Windows.Forms.Button();
		this.btnUpdateSettlement = new System.Windows.Forms.Button();
		this.btnApplyAvgRateFG = new System.Windows.Forms.Button();
		this.btnResetSettlement = new System.Windows.Forms.Button();
		this.btnGeneralSettlement = new System.Windows.Forms.Button();
		this.CmbJobOrderSettlement = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.InPut.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel72.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoInput).BeginInit();
		this.panel63.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).BeginInit();
		this.panel57.SuspendLayout();
		this.panel59.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).BeginInit();
		this.panel58.SuspendLayout();
		this.toolStrip7.SuspendLayout();
		this.OutPut.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.panel22.SuspendLayout();
		this.panel30.SuspendLayout();
		this.panel64.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).BeginInit();
		this.panel60.SuspendLayout();
		this.panel62.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).BeginInit();
		this.panel61.SuspendLayout();
		this.toolStrip8.SuspendLayout();
		this.OverHead.SuspendLayout();
		this.panel67.SuspendLayout();
		this.tabControl4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel68.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailOverhead).BeginInit();
		this.panel25.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).BeginInit();
		this.panel69.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailOhHistory).BeginInit();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.TransctionHistory.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel44.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel43.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel11.SuspendLayout();
		this.Summary.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel48.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummeryReport).BeginInit();
		this.panel47.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummery).BeginInit();
		this.panel23.SuspendLayout();
		this.panel46.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).BeginInit();
		this.panel19.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.Settlement.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel40.SuspendLayout();
		this.panel74.SuspendLayout();
		this.panel75.SuspendLayout();
		this.panel55.SuspendLayout();
		this.panel56.SuspendLayout();
		this.panel53.SuspendLayout();
		this.panel54.SuspendLayout();
		this.panel51.SuspendLayout();
		this.panel52.SuspendLayout();
		this.panel36.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadSettlement).BeginInit();
		this.panel37.SuspendLayout();
		this.panel35.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutSettlement).BeginInit();
		this.panel34.SuspendLayout();
		this.panel33.SuspendLayout();
		this.panel50.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputsettlement).BeginInit();
		this.panel49.SuspendLayout();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderSettlement).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1073, 741);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.InPut);
		this.tabControl1.Controls.Add(this.OutPut);
		this.tabControl1.Controls.Add(this.OverHead);
		this.tabControl1.Controls.Add(this.TransctionHistory);
		this.tabControl1.Controls.Add(this.Summary);
		this.tabControl1.Controls.Add(this.Settlement);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1073, 741);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.InPut.BackColor = System.Drawing.Color.White;
		this.InPut.Controls.Add(this.tabControl2);
		this.InPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.InPut.Location = new System.Drawing.Point(4, 26);
		this.InPut.Name = "InPut";
		this.InPut.Padding = new System.Windows.Forms.Padding(3);
		this.InPut.Size = new System.Drawing.Size(1065, 711);
		this.InPut.TabIndex = 0;
		this.InPut.Text = "Input";
		this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 3);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1059, 705);
		this.tabControl2.TabIndex = 4;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel28);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1051, 679);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel28.Controls.Add(this.panel5);
		this.panel28.Controls.Add(this.panel72);
		this.panel28.Controls.Add(this.panel63);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(3, 30);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1045, 646);
		this.panel28.TabIndex = 174;
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 149);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1045, 497);
		this.panel5.TabIndex = 3;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 28);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1045, 469);
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
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Size = new System.Drawing.Size(1045, 469);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).TabIndex = 0;
		this.grdStockConversionProductionDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockConversionProductionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).VisualStyle = (VisualStyle)4;
		this.grdStockConversionProductionDetail.CellUpdated += new ColumnActionEventHandler(grdStockConversionProductionDetail_CellUpdated);
		this.grdStockConversionProductionDetail.ColumnButtonClick += new ColumnActionEventHandler(grdStockConversionProductionDetail_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1045, 28);
		this.panel12.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(107, 19);
		this.label2.TabIndex = 172;
		this.label2.Text = "Input Detail";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(996, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdStockConversionProductionDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel72.Controls.Add(this.groupBox1);
		this.panel72.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel72.Location = new System.Drawing.Point(0, 29);
		this.panel72.Name = "panel72";
		this.panel72.Size = new System.Drawing.Size(1045, 120);
		this.panel72.TabIndex = 3;
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.label83);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPItem);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput);
		this.groupBox1.Controls.Add(this.btnLoadInvoices);
		this.groupBox1.Controls.Add(this.txtRemarksInput);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1024, 106);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label6.BackColor = System.Drawing.Color.LightGray;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(612, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(296, 26);
		this.label6.TabIndex = 210;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Doc No && Date";
		this.CmbPlantFeederInput.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput).Location = new System.Drawing.Point(92, 75);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput).Name = "CmbPlantFeederInput";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederInput).TabIndex = 3;
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(92, 18);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(77, 23);
		this.txtdocnumber.TabIndex = 0;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(4, 81);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(34, 15);
		this.label83.TabIndex = 209;
		this.label83.Text = "Plant";
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(175, 18);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(147, 23);
		this.txtDocdate.TabIndex = 1;
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(427, 16);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabStop = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(328, 22);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 15);
		this.label27.TabIndex = 171;
		this.label27.Text = "WIP Account";
		this.CmbWIPItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance3;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Location = new System.Drawing.Point(427, 45);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Name = "CmbWIPItem";
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabStop = false;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(328, 51);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(58, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "WIP Item";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 51);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 151;
		this.label23.Text = "Job Order No";
		this.CmbJobOrderNoInput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Location = new System.Drawing.Point(92, 45);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Name = "CmbJobOrderNoInput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoInput).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.btnLoadInvoices.BackColor = System.Drawing.Color.Teal;
		this.btnLoadInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadInvoices.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadInvoices.ForeColor = System.Drawing.Color.White;
		this.btnLoadInvoices.Location = new System.Drawing.Point(911, 74);
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(107, 27);
		this.btnLoadInvoices.TabIndex = 7;
		this.btnLoadInvoices.Text = "Load InPut";
		this.btnLoadInvoices.UseVisualStyleBackColor = false;
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.txtRemarksInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksInput.Location = new System.Drawing.Point(612, 45);
		this.txtRemarksInput.Multiline = true;
		this.txtRemarksInput.Name = "txtRemarksInput";
		this.txtRemarksInput.Size = new System.Drawing.Size(296, 56);
		this.txtRemarksInput.TabIndex = 7;
		this.panel63.BackColor = System.Drawing.Color.Teal;
		this.panel63.Controls.Add(this.label30);
		this.panel63.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel63.Location = new System.Drawing.Point(0, 0);
		this.panel63.Name = "panel63";
		this.panel63.Size = new System.Drawing.Size(1045, 29);
		this.panel63.TabIndex = 2;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(6, 5);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(206, 19);
		this.label30.TabIndex = 210;
		this.label30.Text = "Production Input            ";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnJobOrderDefine, this.btnVoucherReport });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1045, 27);
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
		this.Print.Size = new System.Drawing.Size(100, 24);
		this.Print.Text = "1512-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnJobOrderDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnJobOrderDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnJobOrderDefine.Name = "btnJobOrderDefine";
		this.btnJobOrderDefine.Size = new System.Drawing.Size(120, 24);
		this.btnJobOrderDefine.Text = "Define Job Order";
		this.btnJobOrderDefine.Click += new System.EventHandler(btnJobOrderDefine_Click);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.tabPage4.Controls.Add(this.ctrlGrdBar10);
		this.tabPage4.Controls.Add(this.panel57);
		this.tabPage4.Controls.Add(this.toolStrip7);
		this.tabPage4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.tabPage4.Location = new System.Drawing.Point(4, 4);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1051, 679);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "History";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.ctrlGrdBar10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar10.FormName = this;
		this.ctrlGrdBar10.Location = new System.Drawing.Point(1006, 5);
		this.ctrlGrdBar10.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar10.MyGrid = this.grdinputhistory;
		this.ctrlGrdBar10.Name = "ctrlGrdBar10";
		this.ctrlGrdBar10.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar10.TabIndex = 174;
		this.ctrlGrdBar10.Load += new System.EventHandler(ctrlGrdBar10_Load);
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
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Name = "grdinputhistory";
		((ScrollableBase)this.grdinputhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Size = new System.Drawing.Size(1045, 331);
		((System.Windows.Forms.Control)(object)this.grdinputhistory).TabIndex = 5;
		this.grdinputhistory.TotalRow = (InheritableBoolean)1;
		this.grdinputhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdinputhistory).VisualStyle = (VisualStyle)4;
		this.grdinputhistory.ColumnButtonClick += new ColumnActionEventHandler(grdinputhistory_ColumnButtonClick);
		this.grdinputhistory.SelectionChanged += new System.EventHandler(grdinputhistory_SelectionChanged);
		this.panel57.Controls.Add(this.panel59);
		this.panel57.Controls.Add(this.panel58);
		this.panel57.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel57.Location = new System.Drawing.Point(3, 29);
		this.panel57.Name = "panel57";
		this.panel57.Size = new System.Drawing.Size(1045, 647);
		this.panel57.TabIndex = 6;
		this.panel59.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputDetail);
		this.panel59.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel59.Location = new System.Drawing.Point(0, 331);
		this.panel59.Name = "panel59";
		this.panel59.Size = new System.Drawing.Size(1045, 316);
		this.panel59.TabIndex = 7;
		this.grdInputDetail.AllowEdit = (InheritableBoolean)2;
		this.grdInputDetail.AlternatingColors = true;
		((ScrollableBase)this.grdInputDetail).BackColor = System.Drawing.Color.White;
		this.grdInputDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdInputDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputDetail.DesignTimeLayout = grdInputDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInputDetail.FilterMode = (FilterMode)2;
		this.grdInputDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputDetail.GroupByBoxVisible = false;
		this.grdInputDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Name = "grdInputDetail";
		((ScrollableBase)this.grdInputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1045, 316);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 6;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		this.panel58.Controls.Add((System.Windows.Forms.Control)(object)this.grdinputhistory);
		this.panel58.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel58.Location = new System.Drawing.Point(0, 0);
		this.panel58.Name = "panel58";
		this.panel58.Size = new System.Drawing.Size(1045, 331);
		this.panel58.TabIndex = 6;
		this.toolStrip7.BackColor = System.Drawing.Color.Teal;
		this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripButton4 });
		this.toolStrip7.Location = new System.Drawing.Point(3, 3);
		this.toolStrip7.Name = "toolStrip7";
		this.toolStrip7.Size = new System.Drawing.Size(1045, 26);
		this.toolStrip7.TabIndex = 4;
		this.toolStrip7.Text = "toolStrip7";
		this.toolStripButton4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton4.ForeColor = System.Drawing.Color.White;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(120, 23);
		this.toolStripButton4.Text = "Input History";
		this.OutPut.Controls.Add(this.panel3);
		this.OutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OutPut.Location = new System.Drawing.Point(4, 26);
		this.OutPut.Name = "OutPut";
		this.OutPut.Size = new System.Drawing.Size(1065, 711);
		this.OutPut.TabIndex = 5;
		this.OutPut.Text = "OutPut";
		this.OutPut.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.tabControl3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1065, 711);
		this.panel3.TabIndex = 0;
		this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1065, 711);
		this.tabControl3.TabIndex = 22;
		this.tabControl3.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabPage5.Controls.Add(this.panel2);
		this.tabPage5.Location = new System.Drawing.Point(4, 4);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1057, 685);
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
		this.panel2.Size = new System.Drawing.Size(1051, 679);
		this.panel2.TabIndex = 22;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 212);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1051, 467);
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
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1051, 467);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellEdited += new ColumnActionEventHandler(grdByProduct_CellEdited);
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label15);
		this.panel22.Controls.Add(this.ctrlGrdBar7);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 181);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1051, 31);
		this.panel22.TabIndex = 23;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(119, 19);
		this.label15.TabIndex = 174;
		this.label15.Text = "Output Detail";
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(999, 2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdByProduct;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar7.TabIndex = 173;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.panel30.Controls.Add(this.panel64);
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
		this.panel30.Size = new System.Drawing.Size(1051, 150);
		this.panel30.TabIndex = 1;
		this.panel64.BackColor = System.Drawing.Color.Teal;
		this.panel64.Controls.Add(this.label32);
		this.panel64.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel64.Location = new System.Drawing.Point(0, 0);
		this.panel64.Name = "panel64";
		this.panel64.Size = new System.Drawing.Size(1051, 29);
		this.panel64.TabIndex = 180;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(208, 19);
		this.label32.TabIndex = 210;
		this.label32.Text = "Production  Output         ";
		this.groupBox3.Controls.Add(this.txtDocNoOutPut);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput);
		this.groupBox3.Controls.Add(this.label73);
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
		this.groupBox3.Location = new System.Drawing.Point(16, 35);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1028, 110);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.txtDocNoOutPut.BackColor = System.Drawing.Color.White;
		this.txtDocNoOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOutPut.Location = new System.Drawing.Point(94, 19);
		this.txtDocNoOutPut.Name = "txtDocNoOutPut";
		this.txtDocNoOutPut.ReadOnly = true;
		this.txtDocNoOutPut.Size = new System.Drawing.Size(74, 23);
		this.txtDocNoOutPut.TabIndex = 0;
		this.CmbPlantFeederOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Location = new System.Drawing.Point(94, 76);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Name = "CmbPlantFeederOutput";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).TabIndex = 3;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(805, 82);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(65, 15);
		this.label73.TabIndex = 174;
		this.label73.Text = "Bal Weight";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(88, 15);
		this.label16.TabIndex = 134;
		this.label16.Text = "Doc No && Date";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(6, 82);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(34, 15);
		this.label86.TabIndex = 213;
		this.label86.Text = "Plant";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(805, 53);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(69, 15);
		this.label63.TabIndex = 174;
		this.label63.Text = "Out Weight";
		this.DocDateOutPut.CustomFormat = "dd-MMM-yy";
		this.DocDateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDateOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateOutPut.Location = new System.Drawing.Point(174, 19);
		this.DocDateOutPut.Name = "DocDateOutPut";
		this.DocDateOutPut.Size = new System.Drawing.Size(122, 23);
		this.DocDateOutPut.TabIndex = 1;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(631, 82);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(45, 15);
		this.label71.TabIndex = 174;
		this.label71.Text = "Bal Qty";
		this.label71.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.CmbJobOrderNoOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
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
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(805, 23);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(60, 15);
		this.label54.TabIndex = 174;
		this.label54.Text = "In Weight";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(6, 53);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(78, 15);
		this.label60.TabIndex = 153;
		this.label60.Text = "Job Order No";
		this.txtBalWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeightTotal.Location = new System.Drawing.Point(876, 78);
		this.txtBalWeightTotal.Name = "txtBalWeightTotal";
		this.txtBalWeightTotal.ReadOnly = true;
		this.txtBalWeightTotal.Size = new System.Drawing.Size(133, 23);
		this.txtBalWeightTotal.TabIndex = 10;
		this.txtBalWeightTotal.TabStop = false;
		this.txtBalQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQtyTotal.Location = new System.Drawing.Point(693, 78);
		this.txtBalQtyTotal.Name = "txtBalQtyTotal";
		this.txtBalQtyTotal.ReadOnly = true;
		this.txtBalQtyTotal.Size = new System.Drawing.Size(106, 23);
		this.txtBalQtyTotal.TabIndex = 7;
		this.txtBalQtyTotal.TabStop = false;
		this.label19.BackColor = System.Drawing.Color.Gainsboro;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(302, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(321, 23);
		this.label19.TabIndex = 138;
		this.label19.Text = "Remarks";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(631, 53);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(49, 15);
		this.label62.TabIndex = 174;
		this.label62.Text = "Out Qty";
		this.label62.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtRemarksHeaderOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderOutPut.Location = new System.Drawing.Point(302, 47);
		this.txtRemarksHeaderOutPut.Multiline = true;
		this.txtRemarksHeaderOutPut.Name = "txtRemarksHeaderOutPut";
		this.txtRemarksHeaderOutPut.Size = new System.Drawing.Size(321, 55);
		this.txtRemarksHeaderOutPut.TabIndex = 4;
		this.txtInPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalQty.Location = new System.Drawing.Point(693, 19);
		this.txtInPutTotalQty.Name = "txtInPutTotalQty";
		this.txtInPutTotalQty.ReadOnly = true;
		this.txtInPutTotalQty.Size = new System.Drawing.Size(106, 23);
		this.txtInPutTotalQty.TabIndex = 5;
		this.txtInPutTotalQty.TabStop = false;
		this.txtInPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalWeight.Location = new System.Drawing.Point(876, 19);
		this.txtInPutTotalWeight.Name = "txtInPutTotalWeight";
		this.txtInPutTotalWeight.ReadOnly = true;
		this.txtInPutTotalWeight.Size = new System.Drawing.Size(133, 23);
		this.txtInPutTotalWeight.TabIndex = 8;
		this.txtInPutTotalWeight.TabStop = false;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(631, 23);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(40, 15);
		this.label53.TabIndex = 174;
		this.label53.Text = "In Qty";
		this.label53.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutQtyTotal.Location = new System.Drawing.Point(693, 49);
		this.txtOutQtyTotal.Name = "txtOutQtyTotal";
		this.txtOutQtyTotal.ReadOnly = true;
		this.txtOutQtyTotal.Size = new System.Drawing.Size(106, 23);
		this.txtOutQtyTotal.TabIndex = 6;
		this.txtOutQtyTotal.TabStop = false;
		this.txtOutWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutWeightTotal.Location = new System.Drawing.Point(876, 49);
		this.txtOutWeightTotal.Name = "txtOutWeightTotal";
		this.txtOutWeightTotal.ReadOnly = true;
		this.txtOutWeightTotal.Size = new System.Drawing.Size(133, 23);
		this.txtOutWeightTotal.TabIndex = 9;
		this.txtOutWeightTotal.TabStop = false;
		this.txtAvgFinishGoodRate.BackColor = System.Drawing.Color.White;
		this.txtAvgFinishGoodRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFinishGoodRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFinishGoodRate.Location = new System.Drawing.Point(1590, 58);
		this.txtAvgFinishGoodRate.Name = "txtAvgFinishGoodRate";
		this.txtAvgFinishGoodRate.ReadOnly = true;
		this.txtAvgFinishGoodRate.Size = new System.Drawing.Size(93, 21);
		this.txtAvgFinishGoodRate.TabIndex = 179;
		this.txtAvgFinishGoodRate.Visible = false;
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
		this.txtFGRate.BackColor = System.Drawing.Color.White;
		this.txtFGRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGRate.Location = new System.Drawing.Point(1685, 31);
		this.txtFGRate.Name = "txtFGRate";
		this.txtFGRate.ReadOnly = true;
		this.txtFGRate.Size = new System.Drawing.Size(93, 21);
		this.txtFGRate.TabIndex = 176;
		this.txtFGRate.Visible = false;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(1687, 10);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(52, 15);
		this.label76.TabIndex = 177;
		this.label76.Text = "FG Rate";
		this.label76.Visible = false;
		this.txtFGWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGWeight.Location = new System.Drawing.Point(1590, 31);
		this.txtFGWeight.Name = "txtFGWeight";
		this.txtFGWeight.Size = new System.Drawing.Size(93, 21);
		this.txtFGWeight.TabIndex = 174;
		this.txtFGWeight.Visible = false;
		this.txtFGWeight.TextChanged += new System.EventHandler(txtFGWeight_TextChanged);
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(1592, 8);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(64, 15);
		this.label75.TabIndex = 175;
		this.label75.Text = "FG Weight";
		this.label75.Visible = false;
		this.panel4.Controls.Add(this.toolStrip6);
		this.panel4.Controls.Add(this.ctrlGrdBar4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1051, 31);
		this.panel4.TabIndex = 22;
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewOutPut, this.btnRefreshOutPut, this.btnSaveOutPut, this.btnUpdateOutPut, this.toolStripSeparator5, this.btnPrintOutPut, this.btnVoucherOutPut, this.btnLoadOutPutFromjobOrder });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1051, 27);
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
		this.btnPrintOutPut.Size = new System.Drawing.Size(100, 24);
		this.btnPrintOutPut.Text = "1512-Print";
		this.btnPrintOutPut.Click += new System.EventHandler(btnPrintOutPut_Click);
		this.btnVoucherOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOutPut.Name = "btnVoucherOutPut";
		this.btnVoucherOutPut.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherOutPut.Text = "118-Voucher";
		this.btnVoucherOutPut.Click += new System.EventHandler(btnVoucherOutPut_Click);
		this.btnLoadOutPutFromjobOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOutPutFromjobOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadOutPutFromjobOrder.Name = "btnLoadOutPutFromjobOrder";
		this.btnLoadOutPutFromjobOrder.Size = new System.Drawing.Size(93, 24);
		this.btnLoadOutPutFromjobOrder.Text = "Load OutPut";
		this.btnLoadOutPutFromjobOrder.Click += new System.EventHandler(btnLoadOutPutFromjobOrder_Click);
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar4.TabIndex = 3;
		this.tabPage6.Controls.Add(this.ctrlGrdBar11);
		this.tabPage6.Controls.Add(this.panel60);
		this.tabPage6.Controls.Add(this.toolStrip8);
		this.tabPage6.Location = new System.Drawing.Point(4, 4);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1057, 685);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "History";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.ctrlGrdBar11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar11.FormName = this;
		this.ctrlGrdBar11.Location = new System.Drawing.Point(994, 3);
		this.ctrlGrdBar11.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar11.MyGrid = this.grdOutputHistory;
		this.ctrlGrdBar11.Name = "ctrlGrdBar11";
		this.ctrlGrdBar11.Size = new System.Drawing.Size(60, 26);
		this.ctrlGrdBar11.TabIndex = 175;
		this.ctrlGrdBar11.Load += new System.EventHandler(ctrlGrdBar11_Load);
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
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Name = "grdOutputHistory";
		((ScrollableBase)this.grdOutputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Size = new System.Drawing.Size(1051, 331);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).TabIndex = 5;
		this.grdOutputHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistory).VisualStyle = (VisualStyle)4;
		this.grdOutputHistory.ColumnButtonClick += new ColumnActionEventHandler(grdOutputHistory_ColumnButtonClick);
		this.grdOutputHistory.SelectionChanged += new System.EventHandler(grdOutputHistory_SelectionChanged);
		this.panel60.Controls.Add(this.panel62);
		this.panel60.Controls.Add(this.panel61);
		this.panel60.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel60.Location = new System.Drawing.Point(3, 30);
		this.panel60.Name = "panel60";
		this.panel60.Size = new System.Drawing.Size(1051, 652);
		this.panel60.TabIndex = 6;
		this.panel62.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutDetail);
		this.panel62.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel62.Location = new System.Drawing.Point(0, 331);
		this.panel62.Name = "panel62";
		this.panel62.Size = new System.Drawing.Size(1051, 321);
		this.panel62.TabIndex = 7;
		this.grdOutPutDetail.AllowEdit = (InheritableBoolean)2;
		this.grdOutPutDetail.AlternatingColors = true;
		((ScrollableBase)this.grdOutPutDetail).BackColor = System.Drawing.Color.White;
		this.grdOutPutDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOutPutDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutDetail.DesignTimeLayout = grdOutPutDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPutDetail.FilterMode = (FilterMode)2;
		this.grdOutPutDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutDetail.GroupByBoxVisible = false;
		this.grdOutPutDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Name = "grdOutPutDetail";
		((ScrollableBase)this.grdOutPutDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Size = new System.Drawing.Size(1051, 321);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).TabIndex = 7;
		this.grdOutPutDetail.TotalRow = (InheritableBoolean)1;
		this.grdOutPutDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutDetail).VisualStyle = (VisualStyle)4;
		this.panel61.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistory);
		this.panel61.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel61.Location = new System.Drawing.Point(0, 0);
		this.panel61.Name = "panel61";
		this.panel61.Size = new System.Drawing.Size(1051, 331);
		this.panel61.TabIndex = 6;
		this.toolStrip8.BackColor = System.Drawing.Color.Teal;
		this.toolStrip8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.toolStrip8.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton9, this.toolStripSeparator6, this.btnLoadAllOutputHistoy });
		this.toolStrip8.Location = new System.Drawing.Point(3, 3);
		this.toolStrip8.Name = "toolStrip8";
		this.toolStrip8.Size = new System.Drawing.Size(1051, 27);
		this.toolStrip8.TabIndex = 4;
		this.toolStrip8.Text = "toolStrip8";
		this.toolStripButton9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.toolStripButton9.ForeColor = System.Drawing.Color.White;
		this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton9.Name = "toolStripButton9";
		this.toolStripButton9.Size = new System.Drawing.Size(133, 24);
		this.toolStripButton9.Text = "OutPut History";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnLoadAllOutputHistoy.ForeColor = System.Drawing.Color.White;
		this.btnLoadAllOutputHistoy.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAllOutputHistoy.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllOutputHistoy.Name = "btnLoadAllOutputHistoy";
		this.btnLoadAllOutputHistoy.Size = new System.Drawing.Size(99, 24);
		this.btnLoadAllOutputHistoy.Text = "Load All";
		this.btnLoadAllOutputHistoy.Click += new System.EventHandler(btnLoadAllOutputHistoy_Click);
		this.OverHead.Controls.Add(this.panel67);
		this.OverHead.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OverHead.Location = new System.Drawing.Point(4, 26);
		this.OverHead.Name = "OverHead";
		this.OverHead.Size = new System.Drawing.Size(1065, 711);
		this.OverHead.TabIndex = 4;
		this.OverHead.Text = "Over Head";
		this.OverHead.UseVisualStyleBackColor = true;
		this.panel67.Controls.Add(this.tabControl4);
		this.panel67.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel67.Location = new System.Drawing.Point(0, 0);
		this.panel67.Name = "panel67";
		this.panel67.Size = new System.Drawing.Size(1065, 711);
		this.panel67.TabIndex = 69;
		this.tabControl4.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl4.Controls.Add(this.tabPage2);
		this.tabControl4.Controls.Add(this.tabPage3);
		this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl4.Location = new System.Drawing.Point(0, 0);
		this.tabControl4.Name = "tabControl4";
		this.tabControl4.SelectedIndex = 0;
		this.tabControl4.Size = new System.Drawing.Size(1065, 711);
		this.tabControl4.TabIndex = 0;
		this.tabControl4.SelectedIndexChanged += new System.EventHandler(tabControl4_SelectedIndexChanged);
		this.tabPage2.Controls.Add(this.panel68);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1057, 682);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "Form";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel68.Controls.Add(this.panel31);
		this.panel68.Controls.Add(this.panel25);
		this.panel68.Controls.Add(this.panel9);
		this.panel68.Controls.Add(this.panel69);
		this.panel68.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel68.Location = new System.Drawing.Point(3, 3);
		this.panel68.Name = "panel68";
		this.panel68.Size = new System.Drawing.Size(1051, 676);
		this.panel68.TabIndex = 0;
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailOverhead);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel31.Location = new System.Drawing.Point(0, 189);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1051, 487);
		this.panel31.TabIndex = 5;
		((ScrollableBase)this.grdDetailOverhead).BackColor = System.Drawing.Color.White;
		grdDetailOverhead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailOverhead.DesignTimeLayout = grdDetailOverhead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailOverhead.FilterMode = (FilterMode)2;
		this.grdDetailOverhead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailOverhead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Name = "grdDetailOverhead";
		((ScrollableBase)this.grdDetailOverhead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Size = new System.Drawing.Size(1051, 487);
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).TabIndex = 1;
		this.grdDetailOverhead.TotalRow = (InheritableBoolean)1;
		this.grdDetailOverhead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailOverhead).VisualStyle = (VisualStyle)4;
		this.grdDetailOverhead.CellUpdated += new ColumnActionEventHandler(grdDetailOverhead_CellUpdated);
		this.grdDetailOverhead.ColumnButtonClick += new ColumnActionEventHandler(grdDetailOverhead_ColumnButtonClick);
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.ctrlGrdBar12);
		this.panel25.Controls.Add(this.label47);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 163);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1051, 26);
		this.panel25.TabIndex = 4;
		this.ctrlGrdBar12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar12.FormName = this;
		this.ctrlGrdBar12.Location = new System.Drawing.Point(1005, 2);
		this.ctrlGrdBar12.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar12.MyGrid = this.grdDetailOverhead;
		this.ctrlGrdBar12.Name = "ctrlGrdBar12";
		this.ctrlGrdBar12.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar12.TabIndex = 180;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label47.ForeColor = System.Drawing.Color.White;
		this.label47.Location = new System.Drawing.Point(3, 3);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(130, 17);
		this.label47.TabIndex = 179;
		this.label47.Text = "Over Heads Detail";
		this.panel9.Controls.Add(this.groupBox6);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 54);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1051, 109);
		this.panel9.TabIndex = 3;
		this.groupBox6.Controls.Add(this.label102);
		this.groupBox6.Controls.Add(this.txtfinishQtyOh);
		this.groupBox6.Controls.Add(this.label103);
		this.groupBox6.Controls.Add(this.txtfinishWeightOh);
		this.groupBox6.Controls.Add(this.label97);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead);
		this.groupBox6.Controls.Add(this.txtInputQtyTotalOH);
		this.groupBox6.Controls.Add(this.label98);
		this.groupBox6.Controls.Add(this.txtInputWeightTotalOH);
		this.groupBox6.Controls.Add(this.label85);
		this.groupBox6.Controls.Add(this.label38);
		this.groupBox6.Controls.Add(this.txtOutputQtyTotalOH);
		this.groupBox6.Controls.Add(this.label46);
		this.groupBox6.Controls.Add(this.txtOutputWeightTotalOH);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead);
		this.groupBox6.Controls.Add(this.txtDocNoOverHead);
		this.groupBox6.Controls.Add(this.label56);
		this.groupBox6.Controls.Add(this.txtDocDateOverHead);
		this.groupBox6.Controls.Add(this.label39);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(11, 6);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1001, 96);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Main";
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(742, 73);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(109, 15);
		this.label102.TabIndex = 188;
		this.label102.Text = "Total Finish Weight";
		this.label102.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtfinishQtyOh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfinishQtyOh.Location = new System.Drawing.Point(628, 69);
		this.txtfinishQtyOh.Name = "txtfinishQtyOh";
		this.txtfinishQtyOh.ReadOnly = true;
		this.txtfinishQtyOh.Size = new System.Drawing.Size(106, 23);
		this.txtfinishQtyOh.TabIndex = 186;
		this.txtfinishQtyOh.TabStop = false;
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.Location = new System.Drawing.Point(528, 73);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(89, 15);
		this.label103.TabIndex = 189;
		this.label103.Text = "Total Finish Qty";
		this.label103.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtfinishWeightOh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfinishWeightOh.Location = new System.Drawing.Point(862, 69);
		this.txtfinishWeightOh.Name = "txtfinishWeightOh";
		this.txtfinishWeightOh.ReadOnly = true;
		this.txtfinishWeightOh.Size = new System.Drawing.Size(106, 23);
		this.txtfinishWeightOh.TabIndex = 187;
		this.txtfinishWeightOh.TabStop = false;
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.Location = new System.Drawing.Point(742, 18);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(107, 15);
		this.label97.TabIndex = 184;
		this.label97.Text = "Total InPut Weight";
		this.label97.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.CmbPlantFeederOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Location = new System.Drawing.Point(85, 70);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Name = "CmbPlantFeederOverHead";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Leave += new System.EventHandler(CmbPlantFeederOverHead_Leave);
		this.txtInputQtyTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQtyTotalOH.Location = new System.Drawing.Point(628, 14);
		this.txtInputQtyTotalOH.Name = "txtInputQtyTotalOH";
		this.txtInputQtyTotalOH.ReadOnly = true;
		this.txtInputQtyTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtInputQtyTotalOH.TabIndex = 182;
		this.txtInputQtyTotalOH.TabStop = false;
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.Location = new System.Drawing.Point(526, 18);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(87, 15);
		this.label98.TabIndex = 185;
		this.label98.Text = "Total InPut Qty";
		this.label98.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtInputWeightTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputWeightTotalOH.Location = new System.Drawing.Point(862, 14);
		this.txtInputWeightTotalOH.Name = "txtInputWeightTotalOH";
		this.txtInputWeightTotalOH.ReadOnly = true;
		this.txtInputWeightTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtInputWeightTotalOH.TabIndex = 183;
		this.txtInputWeightTotalOH.TabStop = false;
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(4, 76);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(69, 15);
		this.label85.TabIndex = 213;
		this.label85.Text = "Plant Name";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(742, 46);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(116, 15);
		this.label38.TabIndex = 177;
		this.label38.Text = "Total OutPut Weight";
		this.label38.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutputQtyTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputQtyTotalOH.Location = new System.Drawing.Point(628, 42);
		this.txtOutputQtyTotalOH.Name = "txtOutputQtyTotalOH";
		this.txtOutputQtyTotalOH.ReadOnly = true;
		this.txtOutputQtyTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtOutputQtyTotalOH.TabIndex = 175;
		this.txtOutputQtyTotalOH.TabStop = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(526, 46);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(96, 15);
		this.label46.TabIndex = 178;
		this.label46.Text = "Total OutPut Qty";
		this.label46.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutputWeightTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputWeightTotalOH.Location = new System.Drawing.Point(862, 42);
		this.txtOutputWeightTotalOH.Name = "txtOutputWeightTotalOH";
		this.txtOutputWeightTotalOH.ReadOnly = true;
		this.txtOutputWeightTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtOutputWeightTotalOH.TabIndex = 176;
		this.txtOutputWeightTotalOH.TabStop = false;
		this.cmbJobOrderOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Location = new System.Drawing.Point(85, 40);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Name = "cmbJobOrderOverHead";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Leave += new System.EventHandler(cmbJobOrderOverHead_Leave);
		this.txtDocNoOverHead.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtDocNoOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOverHead.Location = new System.Drawing.Point(84, 14);
		this.txtDocNoOverHead.Name = "txtDocNoOverHead";
		this.txtDocNoOverHead.ReadOnly = true;
		this.txtDocNoOverHead.Size = new System.Drawing.Size(79, 23);
		this.txtDocNoOverHead.TabIndex = 0;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(4, 18);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 56;
		this.label56.Text = "Doc No";
		this.txtDocDateOverHead.CustomFormat = "dd-MMM-yy";
		this.txtDocDateOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDateOverHead.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateOverHead.Location = new System.Drawing.Point(166, 14);
		this.txtDocDateOverHead.Name = "txtDocDateOverHead";
		this.txtDocDateOverHead.Size = new System.Drawing.Size(174, 23);
		this.txtDocDateOverHead.TabIndex = 0;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(4, 46);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 116;
		this.label39.Text = "Job Order No";
		this.panel69.BackColor = System.Drawing.Color.Teal;
		this.panel69.Controls.Add(this.label44);
		this.panel69.Controls.Add(this.toolStrip5);
		this.panel69.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel69.Location = new System.Drawing.Point(0, 0);
		this.panel69.Name = "panel69";
		this.panel69.Size = new System.Drawing.Size(1051, 54);
		this.panel69.TabIndex = 2;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(3, 31);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(102, 19);
		this.label44.TabIndex = 178;
		this.label44.Text = "Over Heads";
		this.toolStrip5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewOverHead, this.btnRefreshOverHead, this.btnSaveOverHead, this.btnUpdateOverHead, this.toolStripSeparator4, this.btnPrintOverHead, this.btnVoucherOH, this.btnLoadOverHeadsFromJobOrder });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1051, 27);
		this.toolStrip5.TabIndex = 58;
		this.toolStrip5.Text = "toolStrip5";
		this.btnNewOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewOverHead.Name = "btnNewOverHead";
		this.btnNewOverHead.Size = new System.Drawing.Size(59, 24);
		this.btnNewOverHead.Text = "&New";
		this.btnNewOverHead.Click += new System.EventHandler(btnNewOverHead_Click);
		this.btnRefreshOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshOverHead.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshOverHead.Name = "btnRefreshOverHead";
		this.btnRefreshOverHead.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshOverHead.Text = "Refresh";
		this.btnRefreshOverHead.Click += new System.EventHandler(btnRefreshOverHead_Click);
		this.btnSaveOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveOverHead.Image = (System.Drawing.Image)resources.GetObject("btnSaveOverHead.Image");
		this.btnSaveOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveOverHead.Name = "btnSaveOverHead";
		this.btnSaveOverHead.Size = new System.Drawing.Size(64, 24);
		this.btnSaveOverHead.Text = "&Save";
		this.btnSaveOverHead.Click += new System.EventHandler(btnSaveOverHead_Click);
		this.btnUpdateOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOverHead.Name = "btnUpdateOverHead";
		this.btnUpdateOverHead.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateOverHead.Text = "&Update";
		this.btnUpdateOverHead.Visible = false;
		this.btnUpdateOverHead.Click += new System.EventHandler(btnUpdateOverHead_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOverHead.Name = "btnPrintOverHead";
		this.btnPrintOverHead.Size = new System.Drawing.Size(100, 24);
		this.btnPrintOverHead.Text = "&Print-1514";
		this.btnPrintOverHead.Click += new System.EventHandler(btnPrintOverHead_Click);
		this.btnVoucherOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherOH.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOH.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOH.Name = "btnVoucherOH";
		this.btnVoucherOH.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherOH.Text = "118-Voucher";
		this.btnVoucherOH.Click += new System.EventHandler(btnVoucherOH_Click_1);
		this.btnLoadOverHeadsFromJobOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOverHeadsFromJobOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadOverHeadsFromJobOrder.Name = "btnLoadOverHeadsFromJobOrder";
		this.btnLoadOverHeadsFromJobOrder.Size = new System.Drawing.Size(123, 24);
		this.btnLoadOverHeadsFromJobOrder.Text = "Load Over Heads";
		this.btnLoadOverHeadsFromJobOrder.Click += new System.EventHandler(btnLoadOverHeadsFromJobOrder_Click);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailOhHistory);
		this.tabPage3.Controls.Add(this.panel27);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHeadHistory);
		this.tabPage3.Controls.Add(this.panel8);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1057, 682);
		this.tabPage3.TabIndex = 1;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grddetailOhHistory.AllowEdit = (InheritableBoolean)2;
		grddetailOhHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailOhHistory.DesignTimeLayout = grddetailOhHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailOhHistory.DynamicFiltering = true;
		this.grddetailOhHistory.FilterMode = (FilterMode)2;
		this.grddetailOhHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailOhHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Location = new System.Drawing.Point(3, 389);
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Name = "grddetailOhHistory";
		((ScrollableBase)this.grddetailOhHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Size = new System.Drawing.Size(1051, 290);
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).TabIndex = 1;
		this.grddetailOhHistory.TotalRow = (InheritableBoolean)1;
		this.grddetailOhHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailOhHistory).VisualStyle = (VisualStyle)4;
		this.panel27.BackColor = System.Drawing.Color.Teal;
		this.panel27.Controls.Add(this.ctrlGrdBar14);
		this.panel27.Controls.Add(this.label34);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(3, 357);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1051, 32);
		this.panel27.TabIndex = 2;
		this.ctrlGrdBar14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar14.FormName = this;
		this.ctrlGrdBar14.Location = new System.Drawing.Point(1002, 2);
		this.ctrlGrdBar14.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar14.MyGrid = this.grddetailOhHistory;
		this.ctrlGrdBar14.Name = "ctrlGrdBar14";
		this.ctrlGrdBar14.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar14.TabIndex = 177;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 7);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(181, 17);
		this.label34.TabIndex = 178;
		this.label34.Text = "Selected Job Order Detail";
		this.grdOverHeadHistory.AllowEdit = (InheritableBoolean)2;
		grdOverHeadHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHeadHistory.DesignTimeLayout = grdOverHeadHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdOverHeadHistory.DynamicFiltering = true;
		this.grdOverHeadHistory.FilterMode = (FilterMode)2;
		this.grdOverHeadHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHeadHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Name = "grdOverHeadHistory";
		((ScrollableBase)this.grdOverHeadHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Size = new System.Drawing.Size(1051, 322);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).TabIndex = 0;
		this.grdOverHeadHistory.TotalRow = (InheritableBoolean)1;
		this.grdOverHeadHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHeadHistory).VisualStyle = (VisualStyle)4;
		this.grdOverHeadHistory.ColumnButtonClick += new ColumnActionEventHandler(grdOverHeadHistory_ColumnButtonClick);
		this.grdOverHeadHistory.SelectionChanged += new System.EventHandler(grdOverHeadHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).DoubleClick += new System.EventHandler(grdOverHead_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label101);
		this.panel8.Controls.Add(this.ctrlGrdBar9);
		this.panel8.Controls.Add(this.label89);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1051, 32);
		this.panel8.TabIndex = 0;
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label101.ForeColor = System.Drawing.Color.White;
		this.label101.Location = new System.Drawing.Point(174, 7);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(125, 17);
		this.label101.TabIndex = 179;
		this.label101.Text = "(Job Order Wise)";
		this.ctrlGrdBar9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar9.FormName = this;
		this.ctrlGrdBar9.Location = new System.Drawing.Point(1002, 2);
		this.ctrlGrdBar9.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar9.MyGrid = this.grdOverHeadHistory;
		this.ctrlGrdBar9.Name = "ctrlGrdBar9";
		this.ctrlGrdBar9.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar9.TabIndex = 177;
		this.ctrlGrdBar9.Load += new System.EventHandler(ctrlGrdBar9_Load);
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label89.ForeColor = System.Drawing.Color.White;
		this.label89.Location = new System.Drawing.Point(3, 6);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(165, 19);
		this.label89.TabIndex = 178;
		this.label89.Text = "Over Heads History";
		this.TransctionHistory.Controls.Add(this.panel10);
		this.TransctionHistory.Location = new System.Drawing.Point(4, 26);
		this.TransctionHistory.Name = "TransctionHistory";
		this.TransctionHistory.Padding = new System.Windows.Forms.Padding(3);
		this.TransctionHistory.Size = new System.Drawing.Size(1065, 711);
		this.TransctionHistory.TabIndex = 1;
		this.TransctionHistory.Text = "Transaction History";
		this.TransctionHistory.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1059, 705);
		this.panel10.TabIndex = 0;
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 30);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1059, 675);
		this.panel13.TabIndex = 2;
		this.panel17.Controls.Add(this.panel44);
		this.panel17.Controls.Add(this.panel43);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 294);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1059, 381);
		this.panel17.TabIndex = 2;
		this.panel44.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel44.Location = new System.Drawing.Point(0, 27);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(1059, 354);
		this.panel44.TabIndex = 47;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		this.grdDetail.AlternatingColors = true;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1059, 354);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel43.BackColor = System.Drawing.Color.Teal;
		this.panel43.Controls.Add(this.label91);
		this.panel43.Controls.Add(this.ctrlGrdBar2);
		this.panel43.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel43.Location = new System.Drawing.Point(0, 0);
		this.panel43.Name = "panel43";
		this.panel43.Size = new System.Drawing.Size(1059, 27);
		this.panel43.TabIndex = 46;
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label91.ForeColor = System.Drawing.Color.White;
		this.label91.Location = new System.Drawing.Point(5, 5);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(53, 18);
		this.label91.TabIndex = 177;
		this.label91.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1013, 3);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 10);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1059, 284);
		this.panel16.TabIndex = 1;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		this.DatagridHistory.AlternatingColors = true;
		this.DatagridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.ExpandableGroups = (InheritableBoolean)0;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1059, 284);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 44;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.panel15.Controls.Add(this.btnSearch);
		this.panel15.Controls.Add(this.label50);
		this.panel15.Controls.Add(this.txtDocNumberTo);
		this.panel15.Controls.Add(this.txtToDate);
		this.panel15.Controls.Add(this.label52);
		this.panel15.Controls.Add(this.txtfromDate);
		this.panel15.Controls.Add(this.label48);
		this.panel15.Controls.Add(this.label49);
		this.panel15.Controls.Add(this.txtDocNoFrom);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1059, 10);
		this.panel15.TabIndex = 0;
		this.btnSearch.Location = new System.Drawing.Point(864, 16);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(75, 23);
		this.btnSearch.TabIndex = 55;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = true;
		this.label50.AutoSize = true;
		this.label50.Location = new System.Drawing.Point(660, 23);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(71, 17);
		this.label50.TabIndex = 54;
		this.label50.Text = "Doc No To";
		this.txtDocNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNumberTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNumberTo.Location = new System.Drawing.Point(726, 18);
		this.txtDocNumberTo.Name = "txtDocNumberTo";
		this.txtDocNumberTo.Size = new System.Drawing.Size(113, 22);
		this.txtDocNumberTo.TabIndex = 53;
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(298, 19);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(153, 25);
		this.txtToDate.TabIndex = 51;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(246, 23);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(54, 17);
		this.label52.TabIndex = 52;
		this.label52.Text = "Date To";
		this.txtfromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtfromDate.Location = new System.Drawing.Point(87, 19);
		this.txtfromDate.Name = "txtfromDate";
		this.txtfromDate.Size = new System.Drawing.Size(153, 25);
		this.txtfromDate.TabIndex = 45;
		this.label48.AutoSize = true;
		this.label48.Location = new System.Drawing.Point(25, 23);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 17);
		this.label48.TabIndex = 47;
		this.label48.Text = "Date From";
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(462, 23);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(89, 17);
		this.label49.TabIndex = 49;
		this.label49.Text = "Doc No From";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(538, 18);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(113, 22);
		this.txtDocNoFrom.TabIndex = 46;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label90);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1059, 30);
		this.panel11.TabIndex = 0;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label90.ForeColor = System.Drawing.Color.White;
		this.label90.Location = new System.Drawing.Point(3, 5);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(168, 19);
		this.label90.TabIndex = 177;
		this.label90.Text = "Transaction History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1018, 0);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 4;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.Summary.Controls.Add(this.panel18);
		this.Summary.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Summary.Location = new System.Drawing.Point(4, 26);
		this.Summary.Name = "Summary";
		this.Summary.Size = new System.Drawing.Size(1065, 711);
		this.Summary.TabIndex = 2;
		this.Summary.Text = "Summery";
		this.Summary.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add(this.panel21);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1065, 711);
		this.panel18.TabIndex = 0;
		this.panel21.BackColor = System.Drawing.Color.White;
		this.panel21.Controls.Add(this.panel26);
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Controls.Add(this.panel20);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 30);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1065, 681);
		this.panel21.TabIndex = 2;
		this.panel26.Controls.Add(this.panel48);
		this.panel26.Controls.Add(this.panel47);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 292);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1065, 389);
		this.panel26.TabIndex = 122;
		this.panel48.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummeryReport);
		this.panel48.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel48.Location = new System.Drawing.Point(0, 29);
		this.panel48.Name = "panel48";
		this.panel48.Size = new System.Drawing.Size(1065, 360);
		this.panel48.TabIndex = 2;
		this.grdSummeryReport.AllowEdit = (InheritableBoolean)2;
		grdSummeryReport_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummeryReport.DesignTimeLayout = grdSummeryReport_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummeryReport.DynamicFiltering = true;
		this.grdSummeryReport.FilterMode = (FilterMode)2;
		this.grdSummeryReport.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummeryReport.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Name = "grdSummeryReport";
		((ScrollableBase)this.grdSummeryReport).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Size = new System.Drawing.Size(1065, 360);
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).TabIndex = 0;
		this.grdSummeryReport.TotalRow = (InheritableBoolean)1;
		this.grdSummeryReport.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummeryReport).VisualStyle = (VisualStyle)4;
		this.panel47.BackColor = System.Drawing.Color.Teal;
		this.panel47.Controls.Add(this.label93);
		this.panel47.Controls.Add(this.ctrlGrdBar5);
		this.panel47.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel47.Location = new System.Drawing.Point(0, 0);
		this.panel47.Name = "panel47";
		this.panel47.Size = new System.Drawing.Size(1065, 29);
		this.panel47.TabIndex = 1;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(5, 6);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(108, 16);
		this.label93.TabIndex = 178;
		this.label93.Text = "Summary Detail";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1016, 3);
		this.ctrlGrdBar5.MyGrid = this.grdSummery;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar5.TabIndex = 60;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.grdSummery.AllowEdit = (InheritableBoolean)2;
		grdSummery_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummery.DesignTimeLayout = grdSummery_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummery).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummery.DynamicFiltering = true;
		this.grdSummery.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummery).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummery.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummery).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummery).Name = "grdSummery";
		((ScrollableBase)this.grdSummery).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummery).Size = new System.Drawing.Size(1065, 207);
		((System.Windows.Forms.Control)(object)this.grdSummery).TabIndex = 0;
		this.grdSummery.TotalRow = (InheritableBoolean)1;
		this.grdSummery.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummery).VisualStyle = (VisualStyle)4;
		this.panel23.Controls.Add(this.panel46);
		this.panel23.Controls.Add(this.panel45);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 59);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1065, 233);
		this.panel23.TabIndex = 121;
		this.panel46.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummery);
		this.panel46.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel46.Location = new System.Drawing.Point(0, 26);
		this.panel46.Name = "panel46";
		this.panel46.Size = new System.Drawing.Size(1065, 207);
		this.panel46.TabIndex = 2;
		this.panel45.BackColor = System.Drawing.Color.Teal;
		this.panel45.Controls.Add(this.label92);
		this.panel45.Controls.Add(this.ctrlGrdBar6);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel45.Location = new System.Drawing.Point(0, 0);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1065, 26);
		this.panel45.TabIndex = 1;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(3, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(67, 16);
		this.label92.TabIndex = 177;
		this.label92.Text = "Summary";
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1016, 0);
		this.ctrlGrdBar6.MyGrid = this.grdSummeryReport;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar6.TabIndex = 59;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.btnShow);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary);
		this.panel20.Controls.Add(this.label87);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo);
		this.panel20.Controls.Add(this.label68);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1065, 59);
		this.panel20.TabIndex = 120;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(484, 25);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(63, 26);
		this.btnShow.TabIndex = 2;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbPlantNameSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantNameSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantNameSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantNameSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantNameSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Location = new System.Drawing.Point(245, 25);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Name = "CmbPlantNameSummary";
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).TabIndex = 1;
		this.label87.AutoSize = true;
		this.label87.BackColor = System.Drawing.Color.Transparent;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.Black;
		this.label87.Location = new System.Drawing.Point(247, 7);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(69, 15);
		this.label87.TabIndex = 119;
		this.label87.Text = "Plant Name";
		this.cmbsummeryJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbsummeryJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsummeryJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsummeryJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsummeryJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Location = new System.Drawing.Point(8, 25);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Name = "cmbsummeryJobOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Leave += new System.EventHandler(cmbsummeryJobOrderNo_TextChanged);
		this.label68.AutoSize = true;
		this.label68.BackColor = System.Drawing.Color.Transparent;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(6, 7);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(78, 15);
		this.label68.TabIndex = 117;
		this.label68.Text = "Job Order No";
		this.panel19.Controls.Add(this.toolStrip3);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1065, 30);
		this.panel19.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton1, this.btnRefreshSummary, this.toolStripSeparator2, this.btnProductionRecoveryReport, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1065, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnRefreshSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummary.Text = "Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnProductionRecoveryReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProductionRecoveryReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProductionRecoveryReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProductionRecoveryReport.Name = "btnProductionRecoveryReport";
		this.btnProductionRecoveryReport.Size = new System.Drawing.Size(179, 24);
		this.btnProductionRecoveryReport.Text = "1513-Recovery Report";
		this.btnProductionRecoveryReport.Click += new System.EventHandler(btnProductionRecoveryReport_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.Settlement.AccessibleName = "Settlement";
		this.Settlement.AutoScroll = true;
		this.Settlement.Controls.Add(this.panel29);
		this.Settlement.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Settlement.Location = new System.Drawing.Point(4, 26);
		this.Settlement.Name = "Settlement";
		this.Settlement.Size = new System.Drawing.Size(1065, 711);
		this.Settlement.TabIndex = 6;
		this.Settlement.Text = "Settlement";
		this.Settlement.UseVisualStyleBackColor = true;
		this.panel29.Controls.Add(this.panel40);
		this.panel29.Controls.Add(this.panel36);
		this.panel29.Controls.Add(this.panel35);
		this.panel29.Controls.Add(this.panel34);
		this.panel29.Controls.Add(this.panel33);
		this.panel29.Controls.Add(this.panel32);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 0);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1065, 711);
		this.panel29.TabIndex = 0;
		this.panel40.Controls.Add(this.panel74);
		this.panel40.Controls.Add(this.panel55);
		this.panel40.Controls.Add(this.panel53);
		this.panel40.Controls.Add(this.panel51);
		this.panel40.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel40.Location = new System.Drawing.Point(0, 539);
		this.panel40.Name = "panel40";
		this.panel40.Size = new System.Drawing.Size(1065, 172);
		this.panel40.TabIndex = 5;
		this.panel74.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel74.Controls.Add(this.txtDifferenceAmount);
		this.panel74.Controls.Add(this.label11);
		this.panel74.Controls.Add(this.panel75);
		this.panel74.Controls.Add(this.txtFinishAmountAgainstJobOrder);
		this.panel74.Controls.Add(this.txtActualFinishGoodsAmount);
		this.panel74.Controls.Add(this.label9);
		this.panel74.Controls.Add(this.label4);
		this.panel74.Location = new System.Drawing.Point(714, 5);
		this.panel74.Name = "panel74";
		this.panel74.Size = new System.Drawing.Size(260, 165);
		this.panel74.TabIndex = 12;
		this.txtDifferenceAmount.BackColor = System.Drawing.Color.White;
		this.txtDifferenceAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtDifferenceAmount.Location = new System.Drawing.Point(8, 136);
		this.txtDifferenceAmount.Name = "txtDifferenceAmount";
		this.txtDifferenceAmount.ReadOnly = true;
		this.txtDifferenceAmount.Size = new System.Drawing.Size(235, 25);
		this.txtDifferenceAmount.TabIndex = 12;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(8, 117);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(122, 17);
		this.label11.TabIndex = 13;
		this.label11.Text = "Difference Amount";
		this.panel75.BackColor = System.Drawing.Color.Teal;
		this.panel75.Controls.Add(this.label3);
		this.panel75.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel75.Location = new System.Drawing.Point(0, 0);
		this.panel75.Name = "panel75";
		this.panel75.Size = new System.Drawing.Size(258, 20);
		this.panel75.TabIndex = 11;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 17);
		this.label3.TabIndex = 6;
		this.label3.Text = "Difference";
		this.txtFinishAmountAgainstJobOrder.BackColor = System.Drawing.Color.White;
		this.txtFinishAmountAgainstJobOrder.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtFinishAmountAgainstJobOrder.Location = new System.Drawing.Point(8, 90);
		this.txtFinishAmountAgainstJobOrder.Name = "txtFinishAmountAgainstJobOrder";
		this.txtFinishAmountAgainstJobOrder.ReadOnly = true;
		this.txtFinishAmountAgainstJobOrder.Size = new System.Drawing.Size(235, 25);
		this.txtFinishAmountAgainstJobOrder.TabIndex = 0;
		this.txtActualFinishGoodsAmount.BackColor = System.Drawing.Color.White;
		this.txtActualFinishGoodsAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtActualFinishGoodsAmount.Location = new System.Drawing.Point(8, 41);
		this.txtActualFinishGoodsAmount.Name = "txtActualFinishGoodsAmount";
		this.txtActualFinishGoodsAmount.ReadOnly = true;
		this.txtActualFinishGoodsAmount.Size = new System.Drawing.Size(235, 25);
		this.txtActualFinishGoodsAmount.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(8, 23);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(139, 17);
		this.label9.TabIndex = 1;
		this.label9.Text = "Actual Finish Amount";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(8, 71);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(240, 17);
		this.label4.TabIndex = 1;
		this.label4.Text = "Finish Amount against Job Order Rate";
		this.panel55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel55.Controls.Add(this.panel56);
		this.panel55.Controls.Add(this.txtAvgRateFG);
		this.panel55.Controls.Add(this.label58);
		this.panel55.Controls.Add(this.txtFinishGoodRateWithOutExp);
		this.panel55.Controls.Add(this.label51);
		this.panel55.Location = new System.Drawing.Point(435, 73);
		this.panel55.Name = "panel55";
		this.panel55.Size = new System.Drawing.Size(274, 85);
		this.panel55.TabIndex = 11;
		this.panel56.BackColor = System.Drawing.Color.Teal;
		this.panel56.Controls.Add(this.label26);
		this.panel56.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel56.Location = new System.Drawing.Point(0, 0);
		this.panel56.Name = "panel56";
		this.panel56.Size = new System.Drawing.Size(272, 20);
		this.panel56.TabIndex = 12;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(6, 1);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(197, 17);
		this.label26.TabIndex = 6;
		this.label26.Text = "Finish Good Average Rate / Kg";
		this.txtAvgRateFG.BackColor = System.Drawing.Color.White;
		this.txtAvgRateFG.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtAvgRateFG.Location = new System.Drawing.Point(117, 53);
		this.txtAvgRateFG.Name = "txtAvgRateFG";
		this.txtAvgRateFG.ReadOnly = true;
		this.txtAvgRateFG.Size = new System.Drawing.Size(150, 27);
		this.txtAvgRateFG.TabIndex = 0;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(4, 26);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(90, 19);
		this.label58.TabIndex = 8;
		this.label58.Text = "WithOut Exp";
		this.txtFinishGoodRateWithOutExp.BackColor = System.Drawing.Color.White;
		this.txtFinishGoodRateWithOutExp.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtFinishGoodRateWithOutExp.Location = new System.Drawing.Point(117, 22);
		this.txtFinishGoodRateWithOutExp.Name = "txtFinishGoodRateWithOutExp";
		this.txtFinishGoodRateWithOutExp.ReadOnly = true;
		this.txtFinishGoodRateWithOutExp.Size = new System.Drawing.Size(150, 27);
		this.txtFinishGoodRateWithOutExp.TabIndex = 0;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(4, 57);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(66, 19);
		this.label51.TabIndex = 8;
		this.label51.Text = "With Exp";
		this.panel53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel53.Controls.Add(this.panel54);
		this.panel53.Controls.Add(this.txtOverHeadsSettlement);
		this.panel53.Controls.Add(this.label28);
		this.panel53.Location = new System.Drawing.Point(435, 5);
		this.panel53.Name = "panel53";
		this.panel53.Size = new System.Drawing.Size(274, 68);
		this.panel53.TabIndex = 10;
		this.panel54.BackColor = System.Drawing.Color.Teal;
		this.panel54.Controls.Add(this.label94);
		this.panel54.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel54.Location = new System.Drawing.Point(0, 0);
		this.panel54.Name = "panel54";
		this.panel54.Size = new System.Drawing.Size(272, 20);
		this.panel54.TabIndex = 11;
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.White;
		this.label94.Location = new System.Drawing.Point(6, 0);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(116, 17);
		this.label94.TabIndex = 6;
		this.label94.Text = "OverHeads Totals";
		this.txtOverHeadsSettlement.BackColor = System.Drawing.Color.White;
		this.txtOverHeadsSettlement.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtOverHeadsSettlement.Location = new System.Drawing.Point(117, 23);
		this.txtOverHeadsSettlement.Name = "txtOverHeadsSettlement";
		this.txtOverHeadsSettlement.ReadOnly = true;
		this.txtOverHeadsSettlement.Size = new System.Drawing.Size(150, 27);
		this.txtOverHeadsSettlement.TabIndex = 0;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(5, 27);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(81, 19);
		this.label28.TabIndex = 1;
		this.label28.Text = "Over Heads";
		this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel51.Controls.Add(this.panel52);
		this.panel51.Controls.Add(this.label14);
		this.panel51.Controls.Add(this.label25);
		this.panel51.Controls.Add(this.label59);
		this.panel51.Controls.Add(this.txtItemQtyRM);
		this.panel51.Controls.Add(this.txtItemQtyBP);
		this.panel51.Controls.Add(this.txtBPOverHead);
		this.panel51.Controls.Add(this.txtNetWeightRM);
		this.panel51.Controls.Add(this.txtAmountFG);
		this.panel51.Controls.Add(this.txtBPTotalAmountWithExpenses);
		this.panel51.Controls.Add(this.txtNetWeightFG);
		this.panel51.Controls.Add(this.txtNetWeightBP);
		this.panel51.Controls.Add(this.txtAmountBP);
		this.panel51.Controls.Add(this.txtBPPMCharges);
		this.panel51.Controls.Add(this.label31);
		this.panel51.Controls.Add(this.txtAmountRM);
		this.panel51.Controls.Add(this.txtItemQtyFG);
		this.panel51.Location = new System.Drawing.Point(5, 5);
		this.panel51.Name = "panel51";
		this.panel51.Size = new System.Drawing.Size(426, 164);
		this.panel51.TabIndex = 9;
		this.panel52.BackColor = System.Drawing.Color.Teal;
		this.panel52.Controls.Add(this.label21);
		this.panel52.Controls.Add(this.label17);
		this.panel52.Controls.Add(this.label18);
		this.panel52.Controls.Add(this.label22);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(424, 24);
		this.panel52.TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(2, 4);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(79, 17);
		this.label21.TabIndex = 5;
		this.label21.Text = "Description";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(113, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 17);
		this.label17.TabIndex = 2;
		this.label17.Text = "ItemQty";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(198, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(75, 17);
		this.label18.TabIndex = 4;
		this.label18.Text = "NetWeight";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(312, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 17);
		this.label22.TabIndex = 4;
		this.label22.Text = "Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(2, 41);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 1;
		this.label14.Text = "Raw Material";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(2, 73);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(95, 15);
		this.label25.TabIndex = 1;
		this.label25.Text = "By Product - Exp";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(2, 105);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(69, 15);
		this.label59.TabIndex = 1;
		this.label59.Text = "BP+OH Exp";
		this.txtItemQtyRM.BackColor = System.Drawing.Color.White;
		this.txtItemQtyRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyRM.Location = new System.Drawing.Point(109, 37);
		this.txtItemQtyRM.Name = "txtItemQtyRM";
		this.txtItemQtyRM.ReadOnly = true;
		this.txtItemQtyRM.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyRM.TabIndex = 0;
		this.txtItemQtyBP.BackColor = System.Drawing.Color.White;
		this.txtItemQtyBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyBP.Location = new System.Drawing.Point(109, 69);
		this.txtItemQtyBP.Name = "txtItemQtyBP";
		this.txtItemQtyBP.ReadOnly = true;
		this.txtItemQtyBP.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyBP.TabIndex = 0;
		this.txtBPOverHead.BackColor = System.Drawing.Color.White;
		this.txtBPOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOverHead.Location = new System.Drawing.Point(109, 101);
		this.txtBPOverHead.Name = "txtBPOverHead";
		this.txtBPOverHead.ReadOnly = true;
		this.txtBPOverHead.Size = new System.Drawing.Size(82, 23);
		this.txtBPOverHead.TabIndex = 0;
		this.txtNetWeightRM.BackColor = System.Drawing.Color.White;
		this.txtNetWeightRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightRM.Location = new System.Drawing.Point(195, 37);
		this.txtNetWeightRM.Name = "txtNetWeightRM";
		this.txtNetWeightRM.ReadOnly = true;
		this.txtNetWeightRM.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightRM.TabIndex = 3;
		this.txtAmountFG.BackColor = System.Drawing.Color.White;
		this.txtAmountFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountFG.Location = new System.Drawing.Point(305, 133);
		this.txtAmountFG.Name = "txtAmountFG";
		this.txtAmountFG.ReadOnly = true;
		this.txtAmountFG.Size = new System.Drawing.Size(116, 23);
		this.txtAmountFG.TabIndex = 3;
		this.txtBPTotalAmountWithExpenses.BackColor = System.Drawing.Color.White;
		this.txtBPTotalAmountWithExpenses.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPTotalAmountWithExpenses.Location = new System.Drawing.Point(305, 101);
		this.txtBPTotalAmountWithExpenses.Name = "txtBPTotalAmountWithExpenses";
		this.txtBPTotalAmountWithExpenses.ReadOnly = true;
		this.txtBPTotalAmountWithExpenses.Size = new System.Drawing.Size(116, 23);
		this.txtBPTotalAmountWithExpenses.TabIndex = 3;
		this.txtNetWeightFG.BackColor = System.Drawing.Color.White;
		this.txtNetWeightFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightFG.Location = new System.Drawing.Point(195, 133);
		this.txtNetWeightFG.Name = "txtNetWeightFG";
		this.txtNetWeightFG.ReadOnly = true;
		this.txtNetWeightFG.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightFG.TabIndex = 3;
		this.txtNetWeightBP.BackColor = System.Drawing.Color.White;
		this.txtNetWeightBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightBP.Location = new System.Drawing.Point(195, 69);
		this.txtNetWeightBP.Name = "txtNetWeightBP";
		this.txtNetWeightBP.ReadOnly = true;
		this.txtNetWeightBP.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightBP.TabIndex = 3;
		this.txtAmountBP.BackColor = System.Drawing.Color.White;
		this.txtAmountBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountBP.Location = new System.Drawing.Point(305, 69);
		this.txtAmountBP.Name = "txtAmountBP";
		this.txtAmountBP.ReadOnly = true;
		this.txtAmountBP.Size = new System.Drawing.Size(116, 23);
		this.txtAmountBP.TabIndex = 3;
		this.txtBPPMCharges.BackColor = System.Drawing.Color.White;
		this.txtBPPMCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPPMCharges.Location = new System.Drawing.Point(195, 101);
		this.txtBPPMCharges.Name = "txtBPPMCharges";
		this.txtBPPMCharges.ReadOnly = true;
		this.txtBPPMCharges.Size = new System.Drawing.Size(107, 23);
		this.txtBPPMCharges.TabIndex = 3;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(2, 137);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(75, 15);
		this.label31.TabIndex = 1;
		this.label31.Text = "Finish Goods";
		this.txtAmountRM.BackColor = System.Drawing.Color.White;
		this.txtAmountRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountRM.Location = new System.Drawing.Point(305, 37);
		this.txtAmountRM.Name = "txtAmountRM";
		this.txtAmountRM.ReadOnly = true;
		this.txtAmountRM.Size = new System.Drawing.Size(116, 23);
		this.txtAmountRM.TabIndex = 3;
		this.txtItemQtyFG.BackColor = System.Drawing.Color.White;
		this.txtItemQtyFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyFG.Location = new System.Drawing.Point(109, 133);
		this.txtItemQtyFG.Name = "txtItemQtyFG";
		this.txtItemQtyFG.ReadOnly = true;
		this.txtItemQtyFG.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyFG.TabIndex = 0;
		this.panel36.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement);
		this.panel36.Controls.Add(this.panel37);
		this.panel36.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel36.Location = new System.Drawing.Point(0, 396);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(1065, 143);
		this.panel36.TabIndex = 4;
		this.grdOverHeadSettlement.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOverHeadSettlement).BackColor = System.Drawing.Color.White;
		grdOverHeadSettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHeadSettlement.DesignTimeLayout = grdOverHeadSettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHeadSettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Name = "grdOverHeadSettlement";
		((ScrollableBase)this.grdOverHeadSettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Size = new System.Drawing.Size(1065, 120);
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).TabIndex = 2;
		this.grdOverHeadSettlement.TotalRow = (InheritableBoolean)1;
		this.grdOverHeadSettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHeadSettlement).VisualStyle = (VisualStyle)4;
		this.grdOverHeadSettlement.LinkClicked += new ColumnActionEventHandler(grdOverHeadSettlement_LinkClicked);
		this.panel37.BackColor = System.Drawing.Color.Teal;
		this.panel37.Controls.Add(this.label8);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 0);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1065, 23);
		this.panel37.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(10, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 16);
		this.label8.TabIndex = 3;
		this.label8.Text = "Over Heads";
		this.panel35.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutSettlement);
		this.panel35.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel35.Location = new System.Drawing.Point(0, 247);
		this.panel35.Name = "panel35";
		this.panel35.Size = new System.Drawing.Size(1065, 149);
		this.panel35.TabIndex = 3;
		((ScrollableBase)this.grdOutPutSettlement).BackColor = System.Drawing.Color.White;
		grdOutPutSettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutSettlement.DesignTimeLayout = grdOutPutSettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPutSettlement.FilterMode = (FilterMode)2;
		this.grdOutPutSettlement.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutSettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Name = "grdOutPutSettlement";
		((ScrollableBase)this.grdOutPutSettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Size = new System.Drawing.Size(1065, 149);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).TabIndex = 2;
		this.grdOutPutSettlement.TotalRow = (InheritableBoolean)1;
		this.grdOutPutSettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutSettlement).VisualStyle = (VisualStyle)4;
		this.grdOutPutSettlement.CellUpdated += new ColumnActionEventHandler(grdOutPutSettlement_CellUpdated);
		this.panel34.BackColor = System.Drawing.Color.Teal;
		this.panel34.Controls.Add(this.label7);
		this.panel34.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel34.Location = new System.Drawing.Point(0, 221);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(1065, 26);
		this.panel34.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(8, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(215, 16);
		this.label7.TabIndex = 2;
		this.label7.Text = "OutPut By-Product & Finish Goods";
		this.panel33.Controls.Add(this.panel50);
		this.panel33.Controls.Add(this.panel49);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(0, 64);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1065, 157);
		this.panel33.TabIndex = 1;
		this.panel50.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputsettlement);
		this.panel50.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel50.Location = new System.Drawing.Point(0, 29);
		this.panel50.Name = "panel50";
		this.panel50.Size = new System.Drawing.Size(1065, 128);
		this.panel50.TabIndex = 3;
		((ScrollableBase)this.grdInputsettlement).BackColor = System.Drawing.Color.White;
		grdInputsettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputsettlement.DesignTimeLayout = grdInputsettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputsettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Name = "grdInputsettlement";
		((ScrollableBase)this.grdInputsettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Size = new System.Drawing.Size(1065, 128);
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).TabIndex = 1;
		this.grdInputsettlement.TotalRow = (InheritableBoolean)1;
		this.grdInputsettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputsettlement).VisualStyle = (VisualStyle)4;
		this.panel49.BackColor = System.Drawing.Color.Teal;
		this.panel49.Controls.Add(this.label5);
		this.panel49.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel49.Location = new System.Drawing.Point(0, 0);
		this.panel49.Name = "panel49";
		this.panel49.Size = new System.Drawing.Size(1065, 29);
		this.panel49.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(131, 16);
		this.label5.TabIndex = 1;
		this.label5.Text = "Input Raw Material";
		this.panel32.BackColor = System.Drawing.Color.Transparent;
		this.panel32.Controls.Add(this.btnSettlementVoucher);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle);
		this.panel32.Controls.Add(this.label13);
		this.panel32.Controls.Add(this.btnApproveSettlement);
		this.panel32.Controls.Add(this.btnRecoveryReport);
		this.panel32.Controls.Add(this.btnUpdateSettlement);
		this.panel32.Controls.Add(this.btnApplyAvgRateFG);
		this.panel32.Controls.Add(this.btnResetSettlement);
		this.panel32.Controls.Add(this.btnGeneralSettlement);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement);
		this.panel32.Controls.Add(this.label12);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1065, 64);
		this.panel32.TabIndex = 0;
		this.btnSettlementVoucher.BackColor = System.Drawing.Color.Teal;
		this.btnSettlementVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSettlementVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSettlementVoucher.ForeColor = System.Drawing.Color.White;
		this.btnSettlementVoucher.Location = new System.Drawing.Point(981, 33);
		this.btnSettlementVoucher.Margin = new System.Windows.Forms.Padding(2);
		this.btnSettlementVoucher.Name = "btnSettlementVoucher";
		this.btnSettlementVoucher.Size = new System.Drawing.Size(79, 26);
		this.btnSettlementVoucher.TabIndex = 122;
		this.btnSettlementVoucher.Text = "Voucher";
		this.btnSettlementVoucher.UseVisualStyleBackColor = false;
		this.btnSettlementVoucher.Visible = false;
		this.btnSettlementVoucher.Click += new System.EventHandler(btnSettlementVoucher_Click);
		this.CmbDifferenceAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbDifferenceAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDifferenceAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDifferenceAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDifferenceAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Location = new System.Drawing.Point(237, 33);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Name = "CmbDifferenceAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(237, 13);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(117, 15);
		this.label13.TabIndex = 121;
		this.label13.Text = "Difference Account";
		this.label13.Visible = false;
		this.btnApproveSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnApproveSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproveSettlement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproveSettlement.ForeColor = System.Drawing.Color.White;
		this.btnApproveSettlement.Location = new System.Drawing.Point(902, 33);
		this.btnApproveSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproveSettlement.Name = "btnApproveSettlement";
		this.btnApproveSettlement.Size = new System.Drawing.Size(79, 26);
		this.btnApproveSettlement.TabIndex = 9;
		this.btnApproveSettlement.Text = "Approve";
		this.btnApproveSettlement.UseVisualStyleBackColor = false;
		this.btnApproveSettlement.Visible = false;
		this.btnApproveSettlement.Click += new System.EventHandler(btnApproveSettlement_Click);
		this.btnRecoveryReport.BackColor = System.Drawing.Color.Teal;
		this.btnRecoveryReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecoveryReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecoveryReport.ForeColor = System.Drawing.Color.White;
		this.btnRecoveryReport.Location = new System.Drawing.Point(794, 33);
		this.btnRecoveryReport.Margin = new System.Windows.Forms.Padding(2);
		this.btnRecoveryReport.Name = "btnRecoveryReport";
		this.btnRecoveryReport.Size = new System.Drawing.Size(107, 26);
		this.btnRecoveryReport.TabIndex = 6;
		this.btnRecoveryReport.Text = "1513A_Print";
		this.btnRecoveryReport.UseVisualStyleBackColor = false;
		this.btnRecoveryReport.Click += new System.EventHandler(btnRecoveryReport_Click);
		this.btnUpdateSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateSettlement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateSettlement.ForeColor = System.Drawing.Color.White;
		this.btnUpdateSettlement.Location = new System.Drawing.Point(714, 33);
		this.btnUpdateSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateSettlement.Name = "btnUpdateSettlement";
		this.btnUpdateSettlement.Size = new System.Drawing.Size(79, 26);
		this.btnUpdateSettlement.TabIndex = 5;
		this.btnUpdateSettlement.Text = "Update";
		this.btnUpdateSettlement.UseVisualStyleBackColor = false;
		this.btnUpdateSettlement.Click += new System.EventHandler(btnUpdateSettlement_Click);
		this.btnApplyAvgRateFG.BackColor = System.Drawing.Color.Teal;
		this.btnApplyAvgRateFG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplyAvgRateFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplyAvgRateFG.ForeColor = System.Drawing.Color.White;
		this.btnApplyAvgRateFG.Location = new System.Drawing.Point(618, 33);
		this.btnApplyAvgRateFG.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplyAvgRateFG.Name = "btnApplyAvgRateFG";
		this.btnApplyAvgRateFG.Size = new System.Drawing.Size(95, 26);
		this.btnApplyAvgRateFG.TabIndex = 4;
		this.btnApplyAvgRateFG.Text = "ApplyRate";
		this.btnApplyAvgRateFG.UseVisualStyleBackColor = false;
		this.btnApplyAvgRateFG.Click += new System.EventHandler(btnApplyAvgRateFG_Click);
		this.btnResetSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnResetSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetSettlement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetSettlement.ForeColor = System.Drawing.Color.White;
		this.btnResetSettlement.Location = new System.Drawing.Point(558, 33);
		this.btnResetSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnResetSettlement.Name = "btnResetSettlement";
		this.btnResetSettlement.Size = new System.Drawing.Size(59, 26);
		this.btnResetSettlement.TabIndex = 3;
		this.btnResetSettlement.Text = "Reset";
		this.btnResetSettlement.UseVisualStyleBackColor = false;
		this.btnResetSettlement.Click += new System.EventHandler(btnResetSettlement_Click);
		this.btnGeneralSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnGeneralSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGeneralSettlement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGeneralSettlement.ForeColor = System.Drawing.Color.White;
		this.btnGeneralSettlement.Location = new System.Drawing.Point(468, 33);
		this.btnGeneralSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnGeneralSettlement.Name = "btnGeneralSettlement";
		this.btnGeneralSettlement.Size = new System.Drawing.Size(89, 26);
		this.btnGeneralSettlement.TabIndex = 2;
		this.btnGeneralSettlement.Text = "Generate";
		this.btnGeneralSettlement.UseVisualStyleBackColor = false;
		this.btnGeneralSettlement.Click += new System.EventHandler(btnGeneralSettlement_Click);
		this.CmbJobOrderSettlement.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderSettlement.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderSettlement.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderSettlement.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderSettlement.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Location = new System.Drawing.Point(8, 33);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Name = "CmbJobOrderSettlement";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(8, 15);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(81, 15);
		this.label12.TabIndex = 119;
		this.label12.Text = "Job Order No";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1073, 741);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "FoodProductionWithValuesNew_St";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValuesNew_St_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.InPut.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel28.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel72.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoInput).EndInit();
		this.panel63.ResumeLayout(false);
		this.panel63.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.tabPage4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).EndInit();
		this.panel57.ResumeLayout(false);
		this.panel59.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).EndInit();
		this.panel58.ResumeLayout(false);
		this.toolStrip7.ResumeLayout(false);
		this.toolStrip7.PerformLayout();
		this.OutPut.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.panel64.ResumeLayout(false);
		this.panel64.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.tabPage6.ResumeLayout(false);
		this.tabPage6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).EndInit();
		this.panel60.ResumeLayout(false);
		this.panel62.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).EndInit();
		this.panel61.ResumeLayout(false);
		this.toolStrip8.ResumeLayout(false);
		this.toolStrip8.PerformLayout();
		this.OverHead.ResumeLayout(false);
		this.panel67.ResumeLayout(false);
		this.tabControl4.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel68.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailOverhead).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).EndInit();
		this.panel69.ResumeLayout(false);
		this.panel69.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailOhHistory).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.TransctionHistory.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel44.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel43.ResumeLayout(false);
		this.panel43.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.Summary.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel48.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummeryReport).EndInit();
		this.panel47.ResumeLayout(false);
		this.panel47.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummery).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel46.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel45.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.Settlement.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		this.panel40.ResumeLayout(false);
		this.panel74.ResumeLayout(false);
		this.panel74.PerformLayout();
		this.panel75.ResumeLayout(false);
		this.panel75.PerformLayout();
		this.panel55.ResumeLayout(false);
		this.panel55.PerformLayout();
		this.panel56.ResumeLayout(false);
		this.panel56.PerformLayout();
		this.panel53.ResumeLayout(false);
		this.panel53.PerformLayout();
		this.panel54.ResumeLayout(false);
		this.panel54.PerformLayout();
		this.panel51.ResumeLayout(false);
		this.panel51.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		this.panel36.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadSettlement).EndInit();
		this.panel37.ResumeLayout(false);
		this.panel37.PerformLayout();
		this.panel35.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutSettlement).EndInit();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		this.panel33.ResumeLayout(false);
		this.panel50.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputsettlement).EndInit();
		this.panel49.ResumeLayout(false);
		this.panel49.PerformLayout();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderSettlement).EndInit();
		base.ResumeLayout(false);
	}
}
