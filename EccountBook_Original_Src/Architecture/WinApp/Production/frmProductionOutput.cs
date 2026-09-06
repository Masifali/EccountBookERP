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
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmProductionOutput : Form
{
	private bool UpdateMode;

	private int RecIdOutPut;

	private int VoucherHeadId;

	private int updateDetailIndex = -1;

	private double AvgRate;

	private double AvgRateWithoutExp;

	private string OutputDetailRowsRemoveIds = "";

	private int ScheduleId;

	public static bool ValuesShowRights;

	private DataTable dtBranch = new DataTable();

	private DataTable dtCropGlobal = new DataTable();

	private DataTable dtCropOutPut = new DataTable();

	private DataTable dtJobGlobal = new DataTable();

	private DataTable dtJobOutPut = new DataTable();

	private DataTable dtuomlst = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private DataTable dtEntryType = new DataTable();

	private DataTable dtItemOutPut = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable dtReturnToGodown = new DataTable();

	private DataTable dtItemOuPutGlobal = new DataTable();

	private DataTable dtItemOuPutReturnToGodownGlobal = new DataTable();

	private bool WagesStatus;

	private bool FIFOCGSFlag;

	private bool WagesActiveOrInActiveForOutPut;

	private bool ByProductRateEditAbleOrNot;

	private bool SaleCostingJobOrderWise;

	private bool OutputItemsByJobOrderRateSchedule;

	private bool JobOrderCreatewithoutRates;

	private bool ProductionOutputLabApprovalRequired;

	private bool StockHoldForLabApprovalAutoChecked;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadOutPutPendingforRates LoadPendingforRates;

	private frmPendingMoveOrderDocuments LoadMoveOrder;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	public TabControl tabControl1;

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

	private ToolStrip miniToolStrip;

	private TextBox txtWIPAccount;

	private Label label1;

	private TextBox txtWIPItem;

	private Label label2;

	private GroupBox groupBox1;

	internal UltraCombo CmbMoveOrderTicket;

	private Label label3;

	private Button BtnLoadMoveOrder;

	private TextBox txtMoveOrderBalWeight;

	private Label label7;

	private TextBox txtMoveOrderAlreadUsedWeight;

	private Label label5;

	private TextBox txtMoveOrderNetWeight;

	private Label label4;

	private PictureBox BtnResetMoveOrder;

	private CheckBox chkStockHoldForLabApproval;

	public frmProductionOutput(UserAccount user)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
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
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 112);
			WagesActiveOrInActiveForOutPut = obj != null && obj.IsActive;
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			OutputItemsByJobOrderRateSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OutputItemsByJobOrderRateSchedule"));
			JobOrderCreatewithoutRates = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("JobOrderCreatewithoutRates"));
			StockHoldForLabApprovalAutoChecked = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("StockHoldForLabApprovalAutoChecked"));
			chkStockHoldForLabApproval.Checked = ProductionOutputLabApprovalRequired && StockHoldForLabApprovalAutoChecked;
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("WagesCompulsoryOnProduction,ByProductRateEditableIsAllow");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "WagesCompulsoryOnProduction")
					{
						WagesStatus = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "ByProductRateEditableIsAllow")
					{
						ByProductRateEditAbleOrNot = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (clsGlobalVariables.ScreenViewReights.Any((tblUserRights dd) => dd.ScreenName == "FoodProductionWithValues"))
			{
				formright = CommonServices.SetRightsValueInRightsObject("FoodProductionWithValues");
			}
			else
			{
				formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			}
			btnSaveOutPut.Enabled = formright.DoHaveSaveRight;
			btnUpdateOutPut.Enabled = formright.DoHaveUpdateRights;
			btnVoucherOutPut.Enabled = formright.DoHavePrintRights;
			btnPrintOutPut.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			formright.DoHaveCanRateandAmount = !JobOrderCreatewithoutRates && formright.DoHaveCanRateandAmount;
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar4.mGridChooseFielder.Enabled = false;
			}
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			dtItemOutPut.Columns.Add("Id", typeof(int));
			dtItemOutPut.Columns.Add("ItemName", typeof(string));
			dtCropOutPut.Columns.Add("Id", typeof(int));
			dtCropOutPut.Columns.Add("CropYear");
			dtJobOutPut.Columns.Add("Id", typeof(int));
			dtJobOutPut.Columns.Add("JobLotDescription");
			dtpacktype.Columns.Add("Id", typeof(int));
			dtpacktype.Columns.Add("PackingType");
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
			GenerateDocNumberOutPut();
			CommondtForCombosFillOutPut();
			BranchesFill();
			UOMFill();
			JobLotOutPut();
			CropYearOutPut();
			EntryTypeFill();
			bagType();
			Warehouse();
			JobOrderNoFill();
			if (!OutputItemsByJobOrderRateSchedule)
			{
				AllItemForOutPut();
			}
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
			if (!formright.DoHaveCanRateandAmount)
			{
				txtRemarksOutPut.Width = 542;
				txtRateOutPut.Visible = false;
				((Control)(object)CmbRateUomOutPut).Visible = false;
				txtAmountOutPut.Visible = false;
				lblRateOutPUt.Visible = false;
				lblRateUOMOUtPut.Visible = false;
				lblAmountOUtPUt.Visible = false;
				btnVoucherOutPut.Visible = false;
			}
			txtFromDateOutPuthistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderOutputHCombobind();
			DocDateOutPut.Focus();
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int JobId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
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
				dt.Columns.Add("WorkInProcessAcId", typeof(int));
				dt.Columns.Add("WorkInProcessAc");
				dt.Columns.Add("WipItemId", typeof(int));
				dt.Columns.Add("ItemName");
				dt.Columns.Add("FinishGoodRate", typeof(double));
				dt.Columns.Add("WipWareHouseId", typeof(int));
				dt.Columns.Add("WareHouseName");
				dt.Columns.Add("DocumentTypeId", typeof(int));
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"], dtPJO.Rows[i]["WorkInProcessAcId"], dtPJO.Rows[i]["WorkInProcessAc"], dtPJO.Rows[i]["WipItemId"], dtPJO.Rows[i]["ItemName"], dtPJO.Rows[i]["FinishGoodRate"], dtPJO.Rows[i]["WipWareHouseId"], dtPJO.Rows[i]["WareHouseName"], dtPJO.Rows[i]["DocumentTypeId"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "PlanCode", "Job Order No", ZeroIndex: false);
					((UltraGridBase)CmbJobOrderNoOutput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbJobOrderNoOutput).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)CmbJobOrderNoOutput).DisplayLayout.Bands[0].Columns["DocumentTypeId"].Hidden = true;
				}
				if (JobId > 0)
				{
					CmbJobOrderNoOutput.Value = JobId;
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl3.SelectedIndex == 0)
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
				if (OutputItemsByJobOrderRateSchedule || JobOrderCreatewithoutRates)
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
		if (Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WorkInProcessAcId"].Value) == 0)
		{
			MessageBox.Show("WorkInProcess account not bind against selected job order");
			CmbJobOrderNoOutput.Focus();
			return false;
		}
		if (Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WipItemId"].Value) == 0)
		{
			MessageBox.Show("WIP Item not bind against selected job order");
			CmbJobOrderNoOutput.Focus();
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
			DataTable dt = InvProductionJobOrder.ExportContract_GetForProduction(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, JobOrderId, RecIdOutPut);
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
				if (OutputItemsByJobOrderRateSchedule || JobOrderCreatewithoutRates)
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
			if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow != null || Conversion.ToInt(CmbJobOrderNoOutput.Value) > 0)
			{
				if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["FinishGoodRate"].Value);
				}
				else if (Conversion.ToDouble((object)txtAvgFinishGoodRate.Text.Trim()) == 0.0)
				{
					txtAvgFinishGoodRate.Text = Conversion.ToString(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["FinishGoodRate"].Value);
				}
				txtWIPAccount.Text = Conversion.ToString(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WorkInProcessAc"].Value);
				txtWIPItem.Text = Conversion.ToString(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["ItemName"].Value);
			}
			else
			{
				txtAvgFinishGoodRate.Text = "0";
			}
			if (OutputItemsByJobOrderRateSchedule || JobOrderCreatewithoutRates)
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

	private void ValidateMoveOrder()
	{
		try
		{
			if (Conversion.ToInt(CmbMoveOrderTicket.Value) > 0)
			{
				double gridTotalWeight = Conversion.ToDouble((object)GridEX_Helper.GetColumnSum(grdByProduct, "GrossWeight"));
				double availableWeight = Conversion.ToDouble((object)txtMoveOrderBalWeight.Text);
				if (gridTotalWeight > availableWeight)
				{
					throw new Exception($"The total weight in the grid '{gridTotalWeight}' exceeds the available Move Order balance weight '{availableWeight}'.");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void ReadByIdOutPut(int ID)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
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
			chkStockHoldForLabApproval.Checked = Conversion.ToBool((object)SC.StockHoldForLabApproval);
			List<InvFoodProductionDetail> invFoodProductionDetails = SC.invFoodProductionDetails;
			if (invFoodProductionDetails != null && invFoodProductionDetails.Count > 0)
			{
				InvFoodProductionDetail FirstRow = SC.invFoodProductionDetails[0];
				BindMoveOrderTicket(FirstRow.MoveOrderDocId, FirstRow.WbTicketNo);
				txtMoveOrderNetWeight.Text = FirstRow.TotalWbWeight.ToString("#,##");
				txtMoveOrderAlreadUsedWeight.Text = FirstRow.UsedWbWeight.ToString("#,##");
				txtMoveOrderBalWeight.Text = FirstRow.AvailableWbWeight.ToString("#,##");
			}
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		try
		{
			if (!FormValidationOutPut())
			{
				return;
			}
			ValidateMoveOrder();
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
			invFoodProduction.WIPAccountId = Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WorkInProcessAcId"].Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WipItemId"].Value);
			invFoodProduction.WipWareHouseId = Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["WipWareHouseId"].Value);
			if (Conversion.ToInt(((UltraDropDownBase)CmbJobOrderNoOutput).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
			{
				invFoodProduction.BaseDocumentTypeId = 2;
			}
			if (invFoodProduction.WipWareHouseId == 0)
			{
				throw new Exception("Please Add WareHouse In JobOrder");
			}
			invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoOutput).Text.Trim());
			invFoodProduction.PlantId = Conversion.ToInt(CmbPlantFeederOutput.Value);
			invFoodProduction.ContractScheduleId = Conversion.ToInt(CmbExportContractSchedule.Value);
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.BranchId = UserAccount.BranchesId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.StockHoldForLabApproval = chkStockHoldForLabApproval.Checked;
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
						Conversiondetail.MoveOrderDocumentTypeId = 74;
						Conversiondetail.MoveOrderDocId = Conversion.ToInt(CmbMoveOrderTicket.Value);
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
			txtWIPAccount.Text = "";
			txtWIPItem.Text = "";
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
			JobOrderNoFill();
			txtAvgFinishGoodRate.Text = string.Empty;
			CmbJobOrderNoOutput_Leave(null, null);
			((UltraGridBase)CmbMoveOrderTicket).DataSource = null;
			((Control)(object)CmbMoveOrderTicket).Text = "";
			txtMoveOrderNetWeight.Text = string.Empty;
			txtMoveOrderAlreadUsedWeight.Text = string.Empty;
			txtMoveOrderBalWeight.Text = string.Empty;
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
			if (tabControl3.SelectedIndex == 1)
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
			dtgrdoutput.Columns.Add("WbTicketNo", typeof(int));
			dtgrdoutput.Columns.Add("TotalWbWeight", typeof(decimal));
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
					dtgrdoutput.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], dthis.Rows[i]["WagesNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["PlantName"], dthis.Rows[i]["WbTicketNo"], dthis.Rows[i]["TotalWbWeight"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["ScheduleCode"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdOutputHistory, 3, 2);
			grdOutputHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdOutputHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
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
				btnNewOutPut_Click(null, null);
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
			btnNewOutPut_Click(null, null);
			RecIdOutPut = Conversion.ToInt(grdOutputHistory.CurrentRow.Cells["Id"].Value);
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
				btnNewOutPut_Click(null, null);
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
					else if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "ByProduct" || Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "FinishGoods" || Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "Wastage")
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

	private void BtnLoadMoveOrder_Click(object sender, EventArgs e)
	{
		try
		{
			LoadMoveOrder = new frmPendingMoveOrderDocuments(UserAccount);
			LoadMoveOrder.ShowDialog();
			LoadMoveOrder.BringToFront();
			LoadInGridDetail(LoadMoveOrder.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		if (dt != null && dt.Rows.Count != 0)
		{
			int incomingMOId = Conversion.ToInt(dt.Rows[0]["Id"]);
			int currentMOId = Conversion.ToInt(CmbMoveOrderTicket.Value);
			if (currentMOId <= 0 || currentMOId == incomingMOId || MessageBox.Show("There is already a ticket selected. Are you sure you want to clear it and proceed with a new one?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				int ticketNo = Conversion.ToInt(dt.Rows[0]["TicketNo"]);
				txtMoveOrderNetWeight.Text = Convert.ToDouble(dt.Rows[0]["NetWbWeight"]).ToString("#,##");
				txtMoveOrderAlreadUsedWeight.Text = Convert.ToDouble(dt.Rows[0]["UsedWeight"]).ToString("#,##");
				txtMoveOrderBalWeight.Text = Convert.ToDouble(dt.Rows[0]["BalWeight"]).ToString("#,##");
				BindMoveOrderTicket(incomingMOId, ticketNo);
				CmbMoveOrderTicket.Value = incomingMOId;
			}
		}
	}

	private void BindMoveOrderTicket(int Id, int Name)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("TicketNo", typeof(int));
		dt.Rows.Add(Id, Name);
		InfragisticsHelper.BindAndRetainSelection(CmbMoveOrderTicket, dt, "Id", "TicketNo", "Ticket No", AllColumns: true, Conversion.ToInt(CmbMoveOrderTicket.Value));
	}

	private void BtnResetMoveOrder_Click(object sender, EventArgs e)
	{
		try
		{
			((UltraGridBase)CmbMoveOrderTicket).DataSource = null;
			((Control)(object)CmbMoveOrderTicket).Text = "";
			txtMoveOrderNetWeight.Text = string.Empty;
			txtMoveOrderAlreadUsedWeight.Text = string.Empty;
			txtMoveOrderBalWeight.Text = string.Empty;
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
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Expected O, but got Unknown
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Expected O, but got Unknown
		//IL_0f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9c: Expected O, but got Unknown
		//IL_0fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Expected O, but got Unknown
		//IL_0fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fca: Expected O, but got Unknown
		//IL_7b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b0c: Expected O, but got Unknown
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		Appearance appearance127 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
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
		Appearance appearance111 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionOutput));
		GridEXLayout grdOutputHistory_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdOutPutDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel30 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.BtnResetMoveOrder = new System.Windows.Forms.PictureBox();
		this.CmbMoveOrderTicket = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.BtnLoadMoveOrder = new System.Windows.Forms.Button();
		this.txtMoveOrderBalWeight = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtMoveOrderAlreadUsedWeight = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtMoveOrderNetWeight = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.panel64 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.label165 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label166 = new System.Windows.Forms.Label();
		this.label167 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.CmbLotOutPut = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtRemarksOutPut = new System.Windows.Forms.TextBox();
		this.CmbUomOutPut = new UltraCombo();
		this.txtAmountOutPut = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.lblAmountOUtPUt = new System.Windows.Forms.Label();
		this.CmbWarehouseOutput = new UltraCombo();
		this.CmbRateUomOutPut = new UltraCombo();
		this.CmbPackTypeOutPut = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.lblRateUOMOUtPut = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbItemOutput = new UltraCombo();
		this.txtRateOutPut = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.lblRateOutPUt = new System.Windows.Forms.Label();
		this.txtQtyOutPut = new System.Windows.Forms.TextBox();
		this.btnAddOutPut = new System.Windows.Forms.Button();
		this.CmbCropYearOutPut = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.btnCancelOutPut = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbEntryType = new UltraCombo();
		this.btnUpdateDetailOutPut = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetWeightOutPut = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtWIPItem = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtWIPAccount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNoOutPut = new System.Windows.Forms.TextBox();
		this.CmbExportContractSchedule = new UltraCombo();
		this.CmbPlantFeederOutput = new UltraCombo();
		this.label73 = new System.Windows.Forms.Label();
		this.label170 = new System.Windows.Forms.Label();
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
		this.btnPenForRateEntries = new System.Windows.Forms.ToolStripButton();
		this.btnDailyPlantConsumedHours = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdOutputHistory = new GridEX();
		this.panel85 = new System.Windows.Forms.Panel();
		this.label156 = new System.Windows.Forms.Label();
		this.panel84 = new System.Windows.Forms.Panel();
		this.label155 = new System.Windows.Forms.Label();
		this.ctrlGrdBar11 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel81 = new System.Windows.Forms.Panel();
		this.groupBox12 = new System.Windows.Forms.GroupBox();
		this.BtnShowOutPuthistory = new System.Windows.Forms.Button();
		this.label150 = new System.Windows.Forms.Label();
		this.CmbJobOrderOutPuthistory = new UltraCombo();
		this.txttoDateOutPuthistory = new System.Windows.Forms.DateTimePicker();
		this.label151 = new System.Windows.Forms.Label();
		this.label152 = new System.Windows.Forms.Label();
		this.txtDocnotoOutPuthistory = new System.Windows.Forms.TextBox();
		this.txtFromDateOutPuthistory = new System.Windows.Forms.DateTimePicker();
		this.label153 = new System.Windows.Forms.Label();
		this.label154 = new System.Windows.Forms.Label();
		this.txtDocNoFromOutPuthistory = new System.Windows.Forms.TextBox();
		this.grdOutPutDetail = new GridEX();
		this.panel60 = new System.Windows.Forms.Panel();
		this.toolStrip8 = new System.Windows.Forms.ToolStrip();
		this.BtnNewOutPuthistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefereshOutPutHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.chkStockHoldForLabApproval = new System.Windows.Forms.CheckBox();
		this.panel1.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.panel22.SuspendLayout();
		this.panel30.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnResetMoveOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMoveOrderTicket).BeginInit();
		this.panel64.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportContractSchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).BeginInit();
		this.panel85.SuspendLayout();
		this.panel84.SuspendLayout();
		this.panel81.SuspendLayout();
		this.groupBox12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderOutPuthistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).BeginInit();
		this.panel60.SuspendLayout();
		this.toolStrip8.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl3);
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1182, 687);
		this.panel1.TabIndex = 0;
		this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1182, 687);
		this.tabControl3.TabIndex = 22;
		this.tabControl3.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabPage5.Controls.Add(this.panel2);
		this.tabPage5.Location = new System.Drawing.Point(4, 4);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1174, 661);
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
		this.panel2.Size = new System.Drawing.Size(1168, 655);
		this.panel2.TabIndex = 22;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 344);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1168, 311);
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
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1168, 311);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellEdited += new ColumnActionEventHandler(grdByProduct_CellEdited);
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label15);
		this.panel22.Controls.Add(this.ctrlGrdBar7);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 318);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1168, 26);
		this.panel22.TabIndex = 23;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(119, 19);
		this.label15.TabIndex = 174;
		this.label15.Text = "Output Detail";
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1127, 2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdByProduct;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 173;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.panel30.Controls.Add(this.groupBox1);
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
		this.panel30.Size = new System.Drawing.Size(1168, 287);
		this.panel30.TabIndex = 0;
		this.groupBox1.Controls.Add(this.BtnResetMoveOrder);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.BtnLoadMoveOrder);
		this.groupBox1.Controls.Add(this.txtMoveOrderBalWeight);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtMoveOrderAlreadUsedWeight);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtMoveOrderNetWeight);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(876, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(290, 126);
		this.groupBox1.TabIndex = 181;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Link Any Move Order Here";
		this.BtnResetMoveOrder.Image = Architecture.WinApp.Properties.Resources.reset__1_;
		this.BtnResetMoveOrder.Location = new System.Drawing.Point(69, 30);
		this.BtnResetMoveOrder.Name = "BtnResetMoveOrder";
		this.BtnResetMoveOrder.Size = new System.Drawing.Size(28, 22);
		this.BtnResetMoveOrder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.BtnResetMoveOrder.TabIndex = 211;
		this.BtnResetMoveOrder.TabStop = false;
		this.BtnResetMoveOrder.Click += new System.EventHandler(BtnResetMoveOrder_Click);
		this.CmbMoveOrderTicket.AllowNull = (DefaultableBoolean)1;
		this.CmbMoveOrderTicket.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMoveOrderTicket.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMoveOrderTicket.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMoveOrderTicket.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Location = new System.Drawing.Point(3, 54);
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Name = "CmbMoveOrderTicket";
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 30);
		this.label3.TabIndex = 170;
		this.label3.Text = "W.B \r\nTicket No";
		this.BtnLoadMoveOrder.BackColor = System.Drawing.Color.Teal;
		this.BtnLoadMoveOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadMoveOrder.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadMoveOrder.ForeColor = System.Drawing.Color.White;
		this.BtnLoadMoveOrder.Location = new System.Drawing.Point(3, 80);
		this.BtnLoadMoveOrder.Name = "BtnLoadMoveOrder";
		this.BtnLoadMoveOrder.Size = new System.Drawing.Size(94, 39);
		this.BtnLoadMoveOrder.TabIndex = 1;
		this.BtnLoadMoveOrder.Text = "Load Move Order";
		this.BtnLoadMoveOrder.UseVisualStyleBackColor = false;
		this.BtnLoadMoveOrder.Click += new System.EventHandler(BtnLoadMoveOrder_Click);
		this.txtMoveOrderBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderBalWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderBalWeight.Location = new System.Drawing.Point(176, 74);
		this.txtMoveOrderBalWeight.Name = "txtMoveOrderBalWeight";
		this.txtMoveOrderBalWeight.ReadOnly = true;
		this.txtMoveOrderBalWeight.Size = new System.Drawing.Size(110, 26);
		this.txtMoveOrderBalWeight.TabIndex = 6;
		this.txtMoveOrderBalWeight.TabStop = false;
		this.txtMoveOrderBalWeight.Text = "0";
		this.txtMoveOrderBalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(102, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 15);
		this.label7.TabIndex = 197;
		this.label7.Text = "Bal Weight";
		this.txtMoveOrderAlreadUsedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderAlreadUsedWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderAlreadUsedWeight.Location = new System.Drawing.Point(176, 46);
		this.txtMoveOrderAlreadUsedWeight.Name = "txtMoveOrderAlreadUsedWeight";
		this.txtMoveOrderAlreadUsedWeight.ReadOnly = true;
		this.txtMoveOrderAlreadUsedWeight.Size = new System.Drawing.Size(110, 26);
		this.txtMoveOrderAlreadUsedWeight.TabIndex = 6;
		this.txtMoveOrderAlreadUsedWeight.TabStop = false;
		this.txtMoveOrderAlreadUsedWeight.Text = "0";
		this.txtMoveOrderAlreadUsedWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(102, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 197;
		this.label5.Text = "Used Weight";
		this.txtMoveOrderNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderNetWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderNetWeight.Location = new System.Drawing.Point(176, 18);
		this.txtMoveOrderNetWeight.Name = "txtMoveOrderNetWeight";
		this.txtMoveOrderNetWeight.ReadOnly = true;
		this.txtMoveOrderNetWeight.Size = new System.Drawing.Size(110, 26);
		this.txtMoveOrderNetWeight.TabIndex = 6;
		this.txtMoveOrderNetWeight.TabStop = false;
		this.txtMoveOrderNetWeight.Text = "0";
		this.txtMoveOrderNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(105, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 15);
		this.label4.TabIndex = 197;
		this.label4.Text = "Net Weight";
		this.panel64.BackColor = System.Drawing.Color.Teal;
		this.panel64.Controls.Add(this.chkStockHoldForLabApproval);
		this.panel64.Controls.Add(this.label32);
		this.panel64.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel64.Location = new System.Drawing.Point(0, 0);
		this.panel64.Name = "panel64";
		this.panel64.Size = new System.Drawing.Size(1168, 29);
		this.panel64.TabIndex = 180;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(208, 19);
		this.label32.TabIndex = 210;
		this.label32.Text = "Production  Output         ";
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
		this.groupBox4.Location = new System.Drawing.Point(4, 158);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1028, 125);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail";
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
		this.label165.AutoSize = true;
		this.label165.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label165.Location = new System.Drawing.Point(161, 75);
		this.label165.Name = "label165";
		this.label165.Size = new System.Drawing.Size(46, 15);
		this.label165.TabIndex = 165;
		this.label165.Text = "EbTotal";
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
		this.label166.AutoSize = true;
		this.label166.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label166.Location = new System.Drawing.Point(90, 75);
		this.label166.Name = "label166";
		this.label166.Size = new System.Drawing.Size(42, 15);
		this.label166.TabIndex = 164;
		this.label166.Text = "EbUnit";
		this.label167.AutoSize = true;
		this.label167.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label167.Location = new System.Drawing.Point(4, 75);
		this.label167.Name = "label167";
		this.label167.Size = new System.Drawing.Size(78, 15);
		this.label167.TabIndex = 163;
		this.label167.Text = "Gross Weight";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(4, 95);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(85, 23);
		this.txtGrossWeight.TabIndex = 8;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.CmbLotOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLotOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLotOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Location = new System.Drawing.Point(677, 36);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Name = "CmbLotOutPut";
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 147;
		this.label13.Text = "Entry Type";
		this.txtRemarksOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOutPut.Location = new System.Drawing.Point(342, 95);
		this.txtRemarksOutPut.Name = "txtRemarksOutPut";
		this.txtRemarksOutPut.Size = new System.Drawing.Size(298, 23);
		this.txtRemarksOutPut.TabIndex = 12;
		this.CmbUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance1).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance1).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance1).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
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
		this.txtAmountOutPut.BackColor = System.Drawing.Color.White;
		this.txtAmountOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOutPut.Location = new System.Drawing.Point(792, 95);
		this.txtAmountOutPut.Name = "txtAmountOutPut";
		this.txtAmountOutPut.ReadOnly = true;
		this.txtAmountOutPut.Size = new System.Drawing.Size(92, 23);
		this.txtAmountOutPut.TabIndex = 15;
		this.txtAmountOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(584, 17);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(57, 15);
		this.label70.TabIndex = 141;
		this.label70.Text = "Crop Year";
		this.lblAmountOUtPUt.AutoSize = true;
		this.lblAmountOUtPUt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountOUtPUt.Location = new System.Drawing.Point(792, 72);
		this.lblAmountOUtPUt.Name = "lblAmountOUtPUt";
		this.lblAmountOUtPUt.Size = new System.Drawing.Size(51, 15);
		this.lblAmountOUtPUt.TabIndex = 159;
		this.lblAmountOUtPUt.Text = "Amount";
		this.CmbWarehouseOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Location = new System.Drawing.Point(120, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Name = "CmbWarehouseOutput";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).TabIndex = 1;
		this.CmbRateUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
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
		this.CmbPackTypeOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbPackTypeOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackTypeOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackTypeOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackTypeOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Location = new System.Drawing.Point(837, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Name = "CmbPackTypeOutPut";
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).TabIndex = 6;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(837, 17);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(60, 15);
		this.label45.TabIndex = 143;
		this.label45.Text = "Pack Type";
		this.lblRateUOMOUtPut.AutoSize = true;
		this.lblRateUOMOUtPut.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateUOMOUtPut.Location = new System.Drawing.Point(714, 72);
		this.lblRateUOMOUtPut.Name = "lblRateUOMOUtPut";
		this.lblRateUOMOUtPut.Size = new System.Drawing.Size(61, 15);
		this.lblRateUOMOUtPut.TabIndex = 157;
		this.lblRateUOMOUtPut.Text = "Rate UOM";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(943, 17);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(54, 15);
		this.label36.TabIndex = 145;
		this.label36.Text = "Item Qty";
		this.CmbItemOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
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
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(677, 17);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(46, 15);
		this.label69.TabIndex = 142;
		this.label69.Text = "Job Lot";
		this.lblRateOutPUt.AutoSize = true;
		this.lblRateOutPUt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateOutPUt.Location = new System.Drawing.Point(642, 72);
		this.lblRateOutPUt.Name = "lblRateOutPUt";
		this.lblRateOutPUt.Size = new System.Drawing.Size(30, 15);
		this.lblRateOutPUt.TabIndex = 155;
		this.lblRateOutPUt.Text = "Rate";
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
		this.CmbCropYearOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
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
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(502, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(63, 15);
		this.label20.TabIndex = 136;
		this.label20.Text = "Pack UOM";
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
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(281, 17);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 135;
		this.label24.Text = "Item Name";
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
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
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(120, 17);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(69, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "WareHouse";
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
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(248, 72);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 148;
		this.label11.Text = "Net Weight";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(342, 75);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 149;
		this.label9.Text = "Remarks";
		this.groupBox3.Controls.Add(this.txtWIPItem);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtWIPAccount);
		this.groupBox3.Controls.Add(this.label1);
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
		this.groupBox3.Size = new System.Drawing.Size(871, 126);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.txtWIPItem.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWIPItem.Location = new System.Drawing.Point(378, 73);
		this.txtWIPItem.Name = "txtWIPItem";
		this.txtWIPItem.ReadOnly = true;
		this.txtWIPItem.Size = new System.Drawing.Size(180, 23);
		this.txtWIPItem.TabIndex = 216;
		this.txtWIPItem.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(287, 77);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 217;
		this.label2.Text = "WIP Item";
		this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtWIPAccount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWIPAccount.Location = new System.Drawing.Point(378, 46);
		this.txtWIPAccount.Name = "txtWIPAccount";
		this.txtWIPAccount.ReadOnly = true;
		this.txtWIPAccount.Size = new System.Drawing.Size(180, 23);
		this.txtWIPAccount.TabIndex = 214;
		this.txtWIPAccount.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(287, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 215;
		this.label1.Text = "WIP Account";
		this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtDocNoOutPut.BackColor = System.Drawing.Color.White;
		this.txtDocNoOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOutPut.Location = new System.Drawing.Point(82, 19);
		this.txtDocNoOutPut.Name = "txtDocNoOutPut";
		this.txtDocNoOutPut.ReadOnly = true;
		this.txtDocNoOutPut.Size = new System.Drawing.Size(79, 23);
		this.txtDocNoOutPut.TabIndex = 0;
		this.CmbExportContractSchedule.AllowNull = (DefaultableBoolean)1;
		this.CmbExportContractSchedule.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExportContractSchedule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExportContractSchedule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExportContractSchedule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExportContractSchedule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Location = new System.Drawing.Point(378, 17);
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Name = "CmbExportContractSchedule";
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbExportContractSchedule).Leave += new System.EventHandler(CmbPlantFeederOutput_Leave);
		this.CmbPlantFeederOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Location = new System.Drawing.Point(82, 71);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Name = "CmbPlantFeederOutput";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOutput).Leave += new System.EventHandler(CmbPlantFeederOutput_Leave);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(696, 77);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(65, 15);
		this.label73.TabIndex = 174;
		this.label73.Text = "Bal Weight";
		this.label170.AutoSize = true;
		this.label170.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label170.Location = new System.Drawing.Point(287, 23);
		this.label170.Name = "label170";
		this.label170.Size = new System.Drawing.Size(89, 15);
		this.label170.TabIndex = 213;
		this.label170.Text = "Export Contract";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(76, 15);
		this.label16.TabIndex = 134;
		this.label16.Text = "Doc No,Date";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(6, 77);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(69, 15);
		this.label86.TabIndex = 213;
		this.label86.Text = "Plant Name";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(696, 50);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(69, 15);
		this.label63.TabIndex = 174;
		this.label63.Text = "Out Weight";
		this.DocDateOutPut.CustomFormat = "dd-MMM-yy";
		this.DocDateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDateOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateOutPut.Location = new System.Drawing.Point(162, 19);
		this.DocDateOutPut.Name = "DocDateOutPut";
		this.DocDateOutPut.Size = new System.Drawing.Size(122, 23);
		this.DocDateOutPut.TabIndex = 1;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(562, 77);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(45, 15);
		this.label71.TabIndex = 174;
		this.label71.Text = "Bal Qty";
		this.label71.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.CmbJobOrderNoOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Location = new System.Drawing.Point(82, 44);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Name = "CmbJobOrderNoOutput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Leave += new System.EventHandler(CmbJobOrderNoOutput_Leave);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(696, 23);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(60, 15);
		this.label54.TabIndex = 174;
		this.label54.Text = "In Weight";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(6, 50);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(78, 15);
		this.label60.TabIndex = 153;
		this.label60.Text = "Job Order No";
		this.txtBalWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeightTotal.Location = new System.Drawing.Point(767, 73);
		this.txtBalWeightTotal.Name = "txtBalWeightTotal";
		this.txtBalWeightTotal.ReadOnly = true;
		this.txtBalWeightTotal.Size = new System.Drawing.Size(100, 23);
		this.txtBalWeightTotal.TabIndex = 11;
		this.txtBalWeightTotal.TabStop = false;
		this.txtBalWeightTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQtyTotal.Location = new System.Drawing.Point(612, 73);
		this.txtBalQtyTotal.Name = "txtBalQtyTotal";
		this.txtBalQtyTotal.ReadOnly = true;
		this.txtBalQtyTotal.Size = new System.Drawing.Size(80, 23);
		this.txtBalQtyTotal.TabIndex = 8;
		this.txtBalQtyTotal.TabStop = false;
		this.txtBalQtyTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(6, 102);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 138;
		this.label19.Text = "Remarks";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(562, 50);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(49, 15);
		this.label62.TabIndex = 174;
		this.label62.Text = "Out Qty";
		this.label62.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtRemarksHeaderOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderOutPut.Location = new System.Drawing.Point(82, 98);
		this.txtRemarksHeaderOutPut.Name = "txtRemarksHeaderOutPut";
		this.txtRemarksHeaderOutPut.Size = new System.Drawing.Size(785, 23);
		this.txtRemarksHeaderOutPut.TabIndex = 4;
		this.txtInPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalQty.Location = new System.Drawing.Point(612, 19);
		this.txtInPutTotalQty.Name = "txtInPutTotalQty";
		this.txtInPutTotalQty.ReadOnly = true;
		this.txtInPutTotalQty.Size = new System.Drawing.Size(80, 23);
		this.txtInPutTotalQty.TabIndex = 6;
		this.txtInPutTotalQty.TabStop = false;
		this.txtInPutTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInPutTotalWeight.Location = new System.Drawing.Point(767, 19);
		this.txtInPutTotalWeight.Name = "txtInPutTotalWeight";
		this.txtInPutTotalWeight.ReadOnly = true;
		this.txtInPutTotalWeight.Size = new System.Drawing.Size(100, 23);
		this.txtInPutTotalWeight.TabIndex = 9;
		this.txtInPutTotalWeight.TabStop = false;
		this.txtInPutTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(562, 23);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(40, 15);
		this.label53.TabIndex = 174;
		this.label53.Text = "In Qty";
		this.label53.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutQtyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutQtyTotal.Location = new System.Drawing.Point(612, 46);
		this.txtOutQtyTotal.Name = "txtOutQtyTotal";
		this.txtOutQtyTotal.ReadOnly = true;
		this.txtOutQtyTotal.Size = new System.Drawing.Size(80, 23);
		this.txtOutQtyTotal.TabIndex = 7;
		this.txtOutQtyTotal.TabStop = false;
		this.txtOutQtyTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutWeightTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutWeightTotal.Location = new System.Drawing.Point(767, 46);
		this.txtOutWeightTotal.Name = "txtOutWeightTotal";
		this.txtOutWeightTotal.ReadOnly = true;
		this.txtOutWeightTotal.Size = new System.Drawing.Size(100, 23);
		this.txtOutWeightTotal.TabIndex = 10;
		this.txtOutWeightTotal.TabStop = false;
		this.txtOutWeightTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
		this.panel4.Size = new System.Drawing.Size(1168, 31);
		this.panel4.TabIndex = 22;
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNewOutPut, this.btnRefreshOutPut, this.btnSaveOutPut, this.btnUpdateOutPut, this.toolStripSeparator5, this.btnPrintOutPut, this.btnVoucherOutPut, this.btnPenForRateEntries, this.btnDailyPlantConsumedHours });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1168, 27);
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
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar4.TabIndex = 3;
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistory);
		this.tabPage6.Controls.Add(this.panel85);
		this.tabPage6.Controls.Add(this.panel84);
		this.tabPage6.Controls.Add(this.panel81);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutDetail);
		this.tabPage6.Controls.Add(this.panel60);
		this.tabPage6.Location = new System.Drawing.Point(4, 4);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1176, 661);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "History";
		this.tabPage6.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Size = new System.Drawing.Size(1170, 324);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).TabIndex = 0;
		this.grdOutputHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistory).VisualStyle = (VisualStyle)4;
		this.grdOutputHistory.ColumnButtonClick += new ColumnActionEventHandler(grdOutputHistory_ColumnButtonClick);
		this.grdOutputHistory.SelectionChanged += new System.EventHandler(grdOutputHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).DoubleClick += new System.EventHandler(grdOutputHistory_DoubleClick);
		this.panel85.BackColor = System.Drawing.Color.Teal;
		this.panel85.Controls.Add(this.label156);
		this.panel85.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel85.Location = new System.Drawing.Point(3, 451);
		this.panel85.Name = "panel85";
		this.panel85.Size = new System.Drawing.Size(1170, 20);
		this.panel85.TabIndex = 178;
		this.label156.AutoSize = true;
		this.label156.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label156.ForeColor = System.Drawing.Color.White;
		this.label156.Location = new System.Drawing.Point(3, 1);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(230, 17);
		this.label156.TabIndex = 211;
		this.label156.Text = "Detail of the Selected above row";
		this.panel84.BackColor = System.Drawing.Color.Teal;
		this.panel84.Controls.Add(this.label155);
		this.panel84.Controls.Add(this.ctrlGrdBar11);
		this.panel84.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel84.Location = new System.Drawing.Point(3, 103);
		this.panel84.Name = "panel84";
		this.panel84.Size = new System.Drawing.Size(1170, 24);
		this.panel84.TabIndex = 177;
		this.label155.AutoSize = true;
		this.label155.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label155.ForeColor = System.Drawing.Color.White;
		this.label155.Location = new System.Drawing.Point(3, 3);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(40, 17);
		this.label155.TabIndex = 211;
		this.label155.Text = "Main";
		this.ctrlGrdBar11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar11.FormName = this;
		this.ctrlGrdBar11.Location = new System.Drawing.Point(1120, 0);
		this.ctrlGrdBar11.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar11.MyGrid = this.grdOutputHistory;
		this.ctrlGrdBar11.Name = "ctrlGrdBar11";
		this.ctrlGrdBar11.Size = new System.Drawing.Size(48, 22);
		this.ctrlGrdBar11.TabIndex = 175;
		this.ctrlGrdBar11.Load += new System.EventHandler(ctrlGrdBar11_Load);
		this.panel81.Controls.Add(this.groupBox12);
		this.panel81.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel81.Location = new System.Drawing.Point(3, 53);
		this.panel81.Name = "panel81";
		this.panel81.Size = new System.Drawing.Size(1170, 50);
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
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(683, 20);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(59, 15);
		this.label150.TabIndex = 3;
		this.label150.Text = "Job Order";
		this.CmbJobOrderOutPuthistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderOutPuthistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderOutPuthistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderOutPuthistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderOutPuthistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderOutPuthistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Name = "CmbJobOrderOutPuthistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderOutPuthistory).TabIndex = 4;
		this.txttoDateOutPuthistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDateOutPuthistory.CustomFormat = "dd-MMM-yy";
		this.txttoDateOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDateOutPuthistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttoDateOutPuthistory.Location = new System.Drawing.Point(268, 16);
		this.txttoDateOutPuthistory.Name = "txttoDateOutPuthistory";
		this.txttoDateOutPuthistory.Size = new System.Drawing.Size(148, 23);
		this.txttoDateOutPuthistory.TabIndex = 1;
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label151.Location = new System.Drawing.Point(217, 20);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(48, 15);
		this.label151.TabIndex = 171;
		this.label151.Text = "Date To";
		this.label152.AutoSize = true;
		this.label152.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label152.Location = new System.Drawing.Point(2, 20);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(63, 15);
		this.label152.TabIndex = 171;
		this.label152.Text = "Date From";
		this.txtDocnotoOutPuthistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocnotoOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocnotoOutPuthistory.Location = new System.Drawing.Point(609, 16);
		this.txtDocnotoOutPuthistory.Name = "txtDocnotoOutPuthistory";
		this.txtDocnotoOutPuthistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocnotoOutPuthistory.TabIndex = 3;
		this.txtDocnotoOutPuthistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocnotoOutPuthistory_KeyPress);
		this.txtFromDateOutPuthistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateOutPuthistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateOutPuthistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateOutPuthistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromDateOutPuthistory.Name = "txtFromDateOutPuthistory";
		this.txtFromDateOutPuthistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromDateOutPuthistory.TabIndex = 0;
		this.label153.AutoSize = true;
		this.label153.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label153.Location = new System.Drawing.Point(564, 20);
		this.label153.Name = "label153";
		this.label153.Size = new System.Drawing.Size(45, 15);
		this.label153.TabIndex = 2;
		this.label153.Text = "Doc To";
		this.label154.AutoSize = true;
		this.label154.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label154.Location = new System.Drawing.Point(429, 20);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(60, 15);
		this.label154.TabIndex = 173;
		this.label154.Text = "Doc From";
		this.txtDocNoFromOutPuthistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFromOutPuthistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFromOutPuthistory.Location = new System.Drawing.Point(490, 16);
		this.txtDocNoFromOutPuthistory.Name = "txtDocNoFromOutPuthistory";
		this.txtDocNoFromOutPuthistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNoFromOutPuthistory.TabIndex = 2;
		this.txtDocNoFromOutPuthistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFromOutPuthistory_KeyPress);
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
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Location = new System.Drawing.Point(3, 471);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Name = "grdOutPutDetail";
		((ScrollableBase)this.grdOutPutDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).Size = new System.Drawing.Size(1170, 187);
		((System.Windows.Forms.Control)(object)this.grdOutPutDetail).TabIndex = 1;
		this.grdOutPutDetail.TotalRow = (InheritableBoolean)1;
		this.grdOutPutDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutDetail).VisualStyle = (VisualStyle)4;
		this.panel60.BackColor = System.Drawing.Color.Teal;
		this.panel60.Controls.Add(this.toolStrip8);
		this.panel60.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel60.Location = new System.Drawing.Point(3, 3);
		this.panel60.Name = "panel60";
		this.panel60.Size = new System.Drawing.Size(1170, 50);
		this.panel60.TabIndex = 6;
		this.toolStrip8.BackColor = System.Drawing.Color.White;
		this.toolStrip8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.toolStrip8.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewOutPuthistory, this.toolStripSeparator6, this.BtnRefereshOutPutHistory });
		this.toolStrip8.Location = new System.Drawing.Point(0, 0);
		this.toolStrip8.Name = "toolStrip8";
		this.toolStrip8.Size = new System.Drawing.Size(1170, 27);
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
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 421);
		this.tabControl1.TabIndex = 1;
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
		this.chkStockHoldForLabApproval.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkStockHoldForLabApproval.AutoSize = true;
		this.chkStockHoldForLabApproval.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkStockHoldForLabApproval.ForeColor = System.Drawing.Color.White;
		this.chkStockHoldForLabApproval.Location = new System.Drawing.Point(983, 5);
		this.chkStockHoldForLabApproval.Name = "chkStockHoldForLabApproval";
		this.chkStockHoldForLabApproval.Size = new System.Drawing.Size(183, 19);
		this.chkStockHoldForLabApproval.TabIndex = 211;
		this.chkStockHoldForLabApproval.Text = "Stock Hold For Lab Approval";
		this.chkStockHoldForLabApproval.UseVisualStyleBackColor = true;
		this.chkStockHoldForLabApproval.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1182, 687);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionOutput";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnResetMoveOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMoveOrderTicket).EndInit();
		this.panel64.ResumeLayout(false);
		this.panel64.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportContractSchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).EndInit();
		this.panel85.ResumeLayout(false);
		this.panel85.PerformLayout();
		this.panel84.ResumeLayout(false);
		this.panel84.PerformLayout();
		this.panel81.ResumeLayout(false);
		this.groupBox12.ResumeLayout(false);
		this.groupBox12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderOutPuthistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutDetail).EndInit();
		this.panel60.ResumeLayout(false);
		this.panel60.PerformLayout();
		this.toolStrip8.ResumeLayout(false);
		this.toolStrip8.PerformLayout();
		base.ResumeLayout(false);
	}
}
