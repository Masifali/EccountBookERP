using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

public class FoodProductionWithValues : Form
{
	private bool UpdateMode;

	private int RecIdConsumption;

	private int RecIdOutPut;

	private int VoucherHeadId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexConsumption = -1;

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

	private DataTable tableConsumption = new DataTable();

	private DataTable dtItemOuPutGlobal = new DataTable();

	private DataTable dtitemConsumption = new DataTable();

	private DataTable OverHeadHistorydt = new DataTable();

	private DataTable dtItemOuPutReturnToGodownGlobal = new DataTable();

	private bool LoaderStatus;

	private bool WagesStatus;

	private bool FIFOCGSFlag;

	private bool IsManualEntryOnInputNotAllowed;

	private bool WagesActiveOrInActiveForOutPut;

	private bool WagesActiveOrInActiveForConsumption;

	private bool ByProductRateEditAbleOrNot;

	private bool SaleCostingJobOrderWise;

	private bool OutputItemsByJobOrderRateSchedule;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadOutPutPendingforRates LoadPendingforRates;

	private LoadConsumptionPendingforRates LoadConsumptionPendingforRate;

	private LoadavailableTransactionsForIssuanceOnConsumption LoadTransactionOnConsumption;

	private frmProductionInput InputForm;

	private frmProductionOutput OutputForm;

	private frmProductionOverhead OverHeadForm;

	private frmProductionPackingMaterial PackingMaterialForm;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabPage Summary;

	private Panel panel18;

	private TabPage InPut;

	private TabPage TransctionHistory;

	private Panel panel10;

	private Panel panel13;

	internal GridEX grdDetail;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Button btnSearchTransactionHistory;

	private Label label50;

	private TextBox txtDocNumberTo;

	private DateTimePicker txtToDateTransactionHistory;

	private Label label52;

	private DateTimePicker txtfromDateTransactionHistory;

	private Label label48;

	private Label label49;

	private TextBox txtDocNoFrom;

	private TabPage PackingMaterial;

	private Panel panel7;

	private TabPage OverHead;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TabPage OutPut;

	private TabPage Settlement;

	private Panel panel43;

	private Label label91;

	private Label label90;

	private Panel panel65;

	private Panel panel67;

	private TabPage Consumption;

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

	private TabPage tabPage10;

	private CtrlGrdBar ctrlGrdBar17;

	private ToolStrip toolStripConsumptionHistory;

	private ToolStripButton BtnNewHistoryConsumption;

	private ToolStripButton BtnLoadDataForConsumptionEntry;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private Panel panel61;

	private Panel panel83;

	private Label label157;

	private Panel panel62;

	private Label label143;

	private ToolStripButton btnRefreshConsupmtionHistory;

	internal GridEX grdHistoryConsumptionMai;

	internal GridEX grdHistoryConsumptionDetail;

	private Panel panel82;

	private Label label144;

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

	public TabControl tabControl1;

	public TabControl tabControlPm;

	public TabControl tabControl4;

	public TabControl tabControl6;

	private Label label164;

	internal UltraCombo CmbPlantTransactionHistory;

	private Label label163;

	internal UltraCombo CmbJobOrderTransactionHistory;

	private GroupBox groupBox14;

	private Panel panel16;

	private Panel panel29;

	private Panel panel17;

	private Panel panel11;

	private Panel panel33;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewTransactionHistory;

	private ToolStripButton btnRefreshTransactionHistory;

	private Panel PanelOtherForms;

	private ToolStripButton BtnGenerateRates;

	public FoodProductionWithValues(UserAccount user)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
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
			WagesRefDocumentsStatus obj2 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 181);
			WagesActiveOrInActiveForConsumption = obj2 != null && obj2.IsActive;
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
				panelManualEntryConsumption.Visible = false;
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSaveConsumption.Enabled = formright.DoHaveSaveRight;
			BtnUpdateconsumption.Enabled = formright.DoHaveUpdateRights;
			btnPrintVoucherConsumption.Enabled = formright.DoHavePrintRights;
			BtnPrintSlipConsumption.Enabled = formright.DoHavePrintRights;
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar16.mGridChooseFielder.Enabled = false;
				ctrlGrdBar17.mGridChooseFielder.Enabled = false;
			}
			if (!formright.DoHaveCanSettlementtab)
			{
				tabControl1.TabPages.Remove(Settlement);
				tabControl1.TabPages.Remove(OverHead);
			}
			else
			{
				tabControl1.TabPages.Remove(OverHead);
				FormHelper.AddTabPageToTabControl(tabControl1, "tabOverhead", "Overhead", 4);
				tabControl1.TabPages.Remove(Settlement);
				FormHelper.AddTabPageToTabControl(tabControl1, "tabSettlement", "Settlement", 7);
			}
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			if (FIFOCGSFlag)
			{
				BtnPendingForRatesFormConsumption.Visible = false;
			}
			else
			{
				BtnPendingForRatesFormConsumption.Visible = true;
			}
			dtitemConsumption.Columns.Add("Id", typeof(int));
			dtitemConsumption.Columns.Add("ItemName", typeof(string));
			dtitemConsumption.Columns.Add("ItemCode", typeof(string));
			dtItemOutPut.Columns.Add("Id", typeof(int));
			dtItemOutPut.Columns.Add("ItemName", typeof(string));
			dtCropOutPut.Columns.Add("Id", typeof(int));
			dtCropOutPut.Columns.Add("CropYear");
			dtJobOutPut.Columns.Add("Id", typeof(int));
			dtJobOutPut.Columns.Add("JobLotDescription");
			dtpacktype.Columns.Add("Id", typeof(int));
			dtpacktype.Columns.Add("PackingType");
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
			JobOrderNoFill();
			BindProductionDepartment();
			BtnAddConsumption.Visible = true;
			BtnUpdateDetailConsumption.Visible = false;
			BtnCancelConsumption.Visible = false;
			if (!formright.DoHaveCanRateandAmount)
			{
				txtRemarksDetailConsumption.Width = 640;
				txtRateConsumption.Visible = false;
				((Control)(object)CmbRateUomConsumption).Visible = false;
				txtAmountConsumption.Visible = false;
				lblRateConsumption.Visible = false;
				lblRateUomConsumption.Visible = false;
				lblAmountConsumption.Visible = false;
				BtnPendingForRatesFormConsumption.Visible = false;
				BtnGenerateRates.Visible = false;
				btnPrintVoucherConsumption.Visible = false;
			}
			txtFromDateConsupmtionHistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderConsumptionHCombobind();
			txtfromDateTransactionHistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderTransactionHistoryCombobind();
			if (FIFOCGSFlag)
			{
				txtRateConsumption.Enabled = false;
				((Control)(object)CmbRateUomConsumption).Enabled = false;
			}
			else
			{
				txtRateConsumption.Enabled = true;
				((Control)(object)CmbRateUomConsumption).Enabled = false;
			}
			tabControl1.TabPages.Remove(InPut);
			FormHelper.AddTabPageToTabControl(tabControl1, "tabInput", "Input", 0);
			tabControl1.TabPages.Remove(OutPut);
			FormHelper.AddTabPageToTabControl(tabControl1, "tabOutput", "Output", 1);
			tabControl1.TabPages.Remove(PackingMaterial);
			FormHelper.AddTabPageToTabControl(tabControl1, "tabPackingMaterial", "PackingMaterial", 3);
			tabControl1.TabPages.Remove(Summary);
			FormHelper.AddTabPageToTabControl(tabControl1, "tabSummary", "Summary", 6);
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
	}

	private void BindProductionDepartment()
	{
		try
		{
			DataTable dt = CommonServices.GetActiveWareHouseByWareHouseType(2);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDepartmentConsumption, "Id", "WareHouseName", "Production Department", ZeroIndex: false);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			Conversion.ToInt(cmbJobOrderConsumption.Value);
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
					DDL.BindDDL(dt, cmbJobOrderConsumption, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)cmbJobOrderConsumption).DisplayLayout.Bands[0].Columns[2].Hidden = true;
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

	private void GetPlantFeeder(int JobOrderId)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
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
				if (((UltraGridBase)cmbJobOrderConsumption).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantConsumption, "PlantId", "PlantName", "Plant Name", false);
				}
				if (JobConsumId > 0)
				{
					CmbPlantConsumption.Value = JobConsumId;
				}
			}
			else
			{
				((Control)(object)CmbPlantConsumption).Text = string.Empty;
				((UltraGridBase)CmbPlantConsumption).DataSource = null;
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
			if (e.Control && e.KeyCode == Keys.NumPad1)
			{
				tabControl1.SelectedIndex = 0;
			}
			if (e.Control && e.KeyCode == Keys.NumPad2)
			{
				tabControl1.SelectedIndex = 1;
			}
			if (e.Control && e.KeyCode == Keys.NumPad3)
			{
				tabControl1.SelectedIndex = 2;
				txtDocDateConsumption.Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad4)
			{
				tabControl1.SelectedIndex = 3;
			}
			if (e.Control && e.KeyCode == Keys.NumPad5)
			{
				tabControl1.SelectedIndex = 4;
			}
			if (e.Control && e.KeyCode == Keys.NumPad6)
			{
				tabControl1.SelectedIndex = 5;
				((Control)(object)DatagridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.NumPad7)
			{
				tabControl1.SelectedIndex = 6;
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
					((Control)(object)grdConsumption).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdConsumption_DoubleClick(null, null);
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
			if (tabControl1.SelectedIndex == 5)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					_ = DatagridHistory.CurrentRow.Cells[4].Value.ToString() == "Input";
				}
				if (e.Control && e.KeyCode == Keys.D)
				{
					DatagridHistoryDetail();
				}
			}
			if (tabControl1.SelectedIndex == 6 && e.Control)
			{
				_ = e.KeyCode;
				_ = 116;
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
			if (tabControl1.SelectedTab.Name == "tabInput")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				string screenName = "frmProductionInput";
				InputForm = FormHelper.OpenOrGetFormInPanel(screenName, () => new frmProductionInput(UserAccount), PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabOutput")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				string screenName2 = "frmProductionOutput";
				OutputForm = FormHelper.OpenOrGetFormInPanel(screenName2, () => new frmProductionOutput(UserAccount), PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabSummary")
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
				string screenName3 = "frmProductionPackingMaterial";
				PackingMaterialForm = FormHelper.OpenOrGetFormInPanel(screenName3, () => new frmProductionPackingMaterial(UserAccount), PanelOtherForms);
			}
			else if (tabControl1.SelectedTab.Name == "tabOverhead")
			{
				PanelOtherForms.Visible = true;
				tabControl1.Dock = DockStyle.Top;
				tabControl1.Height = 27;
				string screenName4 = "frmProductionOverhead";
				OverHeadForm = FormHelper.OpenOrGetFormInPanel(screenName4, () => new frmProductionOverhead(UserAccount), PanelOtherForms);
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
			if (tabControl1.SelectedIndex == 2)
			{
				if (btnSaveConsumption.Visible && btnSaveConsumption.Enabled)
				{
					GenerateDocConsumption();
				}
				ConsumptionDetailComboBind();
				txtDocDateConsumption.Focus();
			}
			if (tabControl1.SelectedIndex == 5)
			{
				txtfromDateTransactionHistory.Focus();
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

	private void jobOrderTransactionHistoryCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "80,112"
			});
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("JobOrder");
				DataTable dtPlant = new DataTable();
				dtPlant.Columns.Add("Id");
				dtPlant.Columns.Add("Plant");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "JobOrder")
					{
						dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
					{
						dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, CmbJobOrderTransactionHistory, "Id", "JobOrder", "Job Order", ZeroIndex: false);
				}
				else
				{
					((Control)(object)CmbJobOrderTransactionHistory).Text = string.Empty;
					((UltraGridBase)CmbJobOrderTransactionHistory).DataSource = null;
				}
				if (dtPlant.Rows.Count > 0)
				{
					DDL.BindDDL(dtPlant, CmbPlantTransactionHistory, "Id", "Plant", "Plant", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbPlantTransactionHistory).Text = string.Empty;
				((UltraGridBase)CmbPlantTransactionHistory).DataSource = null;
			}
			else
			{
				((Control)(object)CmbPlantTransactionHistory).Text = string.Empty;
				((UltraGridBase)CmbPlantTransactionHistory).DataSource = null;
			}
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
			if (txtfromDateTransactionHistory.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtfromDateTransactionHistory.Value);
			}
			if (txtToDateTransactionHistory.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtToDateTransactionHistory.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtDocNumberTo.Text);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderTransactionHistory.Value);
			obj.PlantId = Conversion.ToInt(CmbPlantTransactionHistory.Value);
			DataTable dthis = InvFoodProduction.ProductionFormHistory(obj);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("DocSrNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("JobOrderNo");
				dtHistory.Columns.Add("EntryType");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("JobOrderStatus");
				dtHistory.Columns.Add("StartDate", typeof(DateTime));
				dtHistory.Columns.Add("EndDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryUser", typeof(string));
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser", typeof(string));
				dtHistory.Columns.Add("PlanStatus", typeof(string));
				dtHistory.Columns.Add("WagesId", typeof(int));
				dtHistory.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
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
			DatagridHistory.RootTable.Columns["WagesId"].Visible = false;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns.Add("WagesPrint");
			DatagridHistory.RootTable.Columns["WagesPrint"].Key = "WagesPrint";
			DatagridHistory.RootTable.Columns["WagesPrint"].Caption = "WagesPrint";
			DatagridHistory.RootTable.Columns["WagesPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["WagesPrint"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["WagesPrint"].ButtonText = "WagesPrint";
			DatagridHistory.RootTable.Columns["WagesPrint"].Width = 60;
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
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
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (!(Conversion.ToString(r.Cells["JobOrderStatus"].Value) != "Approved"))
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (r.Cells[4].Value.ToString() == "Input")
				{
					tabControl1.SelectedIndex = 0;
					((Control)(object)InputForm.CmbJobOrderNo).Enabled = false;
					InputForm.ReadByIdInput(Id);
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)OutputForm.CmbJobOrderNoOutput).Enabled = false;
					OutputForm.ReadByIdOutPut(RecIdOutPut);
				}
			}
			if (e.Column.Key == "WagesPrint")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["WagesId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			SC = InvFoodProduction.GetByID(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			if (SC != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("Item");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CropBatch");
				dt.Columns.Add("PackType");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Order");
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].JobLotCode, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].PackTypeCode, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].InvProductionJobOrderNo, SC.invFoodProductionDetails[i].Remarks);
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
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,#";
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,#";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
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

	private void btnNewTransactionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtfromDateTransactionHistory.Value = DateTime.Now.AddDays(-3.0);
			txtToDateTransactionHistory.Value = DateTime.Now;
			txtDocNoFrom.Text = string.Empty;
			txtDocNumberTo.Text = string.Empty;
			((Control)(object)CmbJobOrderTransactionHistory).Text = string.Empty;
			((Control)(object)CmbPlantTransactionHistory).Text = string.Empty;
			DatagridHistory.ClearStructure();
			txtfromDateTransactionHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshTransactionHistory_Click(object sender, EventArgs e)
	{
		try
		{
			jobOrderTransactionHistoryCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearchTransactionHistory_Click(object sender, EventArgs e)
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
			GetAvgRateByItemAndJoblotForConsumption();
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
						DocDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value),
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
						DocDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value),
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
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
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
			invFoodProduction.BaseDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)cmbJobOrderConsumption).SelectedRow.Cells["DocumentTypeId"].Value);
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
				rate = ((!FIFOCGSFlag) ? CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(CmbItemNameConsumption.Value), Conversion.ToDateTime((object)txtDocDateConsumption.Value), 181, RecIdConsumption, Conversion.ToInt(CmbJobLotConsumption.Value), Conversion.ToInt(CmbCropYearConsumption.Value), null, Conversion.ToInt(cmbWarehouseConsumption.Value)) : CommonServices.GetAvgRateFromFIFOMethod(new ReportsParameters
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
			GetAvgRateByItemAndJoblotForConsumption();
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
			GetAvgRateByItemAndJoblotForConsumption();
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

	private void AvgRateUpdateOnDocDateChange()
	{
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		try
		{
			if (grdConsumption.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdConsumption.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
				int RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int ItemUOMId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				int PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				int WareHouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				string CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				double Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
				double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
				double RateUomEquivalent = Conversion.ToDouble(r.Cells["RateUOMForCheck"].Value);
				double rate = 0.0;
				if (RefDocumentTypeId != 0 || RefDocIdNo != 0 || RefDocSubIdNo != 0)
				{
					break;
				}
				if (FIFOCGSFlag)
				{
					ReportsParameters obj = new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtDocDateConsumption.Value),
						stockUOM = ItemUOMId,
						ItemId = ItemId,
						WarehouseId = WareHouseId,
						PackingTypeId = PackingTypeId,
						JobLotId = JobLotId,
						CropYear = CropYear,
						ItemQty = Qty,
						NetWeight = Weight
					};
					if (RecIdConsumption > 0)
					{
						obj.DocumentTypeId = 181;
						obj.Id = RecIdConsumption;
					}
					rate = Math.Round(CommonServices.GetAvgRateFromFIFOMethod(obj), 3) * 40.0;
					if (rate > 0.0)
					{
						r.Cells["Rate"].Value = rate;
						r.Cells["ItemAmount"].Value = Weight / RateUomEquivalent * rate;
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["ItemAmount"].Value = 0;
					}
				}
				else
				{
					rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)txtDocDateConsumption.Value), 181, RecIdConsumption, JobLotId, 0, CropYear, WareHouseId) * 40.0;
				}
				if (rate > 0.0)
				{
					r.Cells["Rate"].Value = rate;
					r.Cells["ItemAmount"].Value = Weight / RateUomEquivalent * rate;
				}
				r.EndEdit();
			}
			grdConsumption.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDocDateConsumption_ValueChanged(object sender, EventArgs e)
	{
	}

	private void BtnGenerateRates_Click(object sender, EventArgs e)
	{
		try
		{
			AvgRateUpdateOnDocDateChange();
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
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Expected O, but got Unknown
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Expected O, but got Unknown
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Expected O, but got Unknown
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Expected O, but got Unknown
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Expected O, but got Unknown
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_1595: Unknown result type (might be due to invalid IL or missing references)
		//IL_159f: Expected O, but got Unknown
		//IL_15ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b6: Expected O, but got Unknown
		//IL_6df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dfd: Expected O, but got Unknown
		//IL_88af: Unknown result type (might be due to invalid IL or missing references)
		//IL_88b9: Expected O, but got Unknown
		GridEXLayout grdConsumption_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
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
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.FoodProductionWithValues));
		GridEXLayout grdHistoryConsumptionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryConsumptionMai_DesignTimeLayout = new GridEXLayout();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.InPut = new System.Windows.Forms.TabPage();
		this.OutPut = new System.Windows.Forms.TabPage();
		this.Consumption = new System.Windows.Forms.TabPage();
		this.tabControl6 = new System.Windows.Forms.TabControl();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.panel48 = new System.Windows.Forms.Panel();
		this.panel74 = new System.Windows.Forms.Panel();
		this.panel76 = new System.Windows.Forms.Panel();
		this.grdConsumption = new GridEX();
		this.panel77 = new System.Windows.Forms.Panel();
		this.label120 = new System.Windows.Forms.Label();
		this.ctrlGrdBar16 = new grid.User_Conrtols.CtrlGrdBar();
		this.panelManualEntryConsumption = new System.Windows.Forms.Panel();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.cmbWarehouseConsumption = new UltraCombo();
		this.label121 = new System.Windows.Forms.Label();
		this.label122 = new System.Windows.Forms.Label();
		this.txtItemQtyConsumption = new System.Windows.Forms.TextBox();
		this.txtBalanceWeightConsumption = new System.Windows.Forms.TextBox();
		this.txtNetWeightConsumption = new System.Windows.Forms.TextBox();
		this.cmbPackUomConsumption = new UltraCombo();
		this.label123 = new System.Windows.Forms.Label();
		this.CmbJobLotConsumption = new UltraCombo();
		this.txtRemarksDetailConsumption = new System.Windows.Forms.TextBox();
		this.CmbItemNameConsumption = new UltraCombo();
		this.txtAmountConsumption = new System.Windows.Forms.TextBox();
		this.CmbCropYearConsumption = new UltraCombo();
		this.lblAmountConsumption = new System.Windows.Forms.Label();
		this.label125 = new System.Windows.Forms.Label();
		this.CmbRateUomConsumption = new UltraCombo();
		this.label126 = new System.Windows.Forms.Label();
		this.lblRateUomConsumption = new System.Windows.Forms.Label();
		this.label128 = new System.Windows.Forms.Label();
		this.txtRateConsumption = new System.Windows.Forms.TextBox();
		this.label129 = new System.Windows.Forms.Label();
		this.lblRateConsumption = new System.Windows.Forms.Label();
		this.CmbPackingTypeConsumption = new UltraCombo();
		this.BtnAddConsumption = new System.Windows.Forms.Button();
		this.txtBalanceQtyConsumption = new System.Windows.Forms.TextBox();
		this.BtnCancelConsumption = new System.Windows.Forms.Button();
		this.label131 = new System.Windows.Forms.Label();
		this.BtnUpdateDetailConsumption = new System.Windows.Forms.Button();
		this.label132 = new System.Windows.Forms.Label();
		this.label133 = new System.Windows.Forms.Label();
		this.label134 = new System.Windows.Forms.Label();
		this.panel79 = new System.Windows.Forms.Panel();
		this.groupBox10 = new System.Windows.Forms.GroupBox();
		this.label135 = new System.Windows.Forms.Label();
		this.CmbPlantConsumption = new UltraCombo();
		this.txtDocNoConsumption = new System.Windows.Forms.TextBox();
		this.label136 = new System.Windows.Forms.Label();
		this.txtDocDateConsumption = new System.Windows.Forms.DateTimePicker();
		this.cmbWIPAcConsumption = new UltraCombo();
		this.label137 = new System.Windows.Forms.Label();
		this.CmbDepartmentConsumption = new UltraCombo();
		this.label138 = new System.Windows.Forms.Label();
		this.cmbWIPItemConsumption = new UltraCombo();
		this.label139 = new System.Windows.Forms.Label();
		this.label140 = new System.Windows.Forms.Label();
		this.cmbJobOrderConsumption = new UltraCombo();
		this.label141 = new System.Windows.Forms.Label();
		this.txtRemarksHeaderConsumption = new System.Windows.Forms.TextBox();
		this.panel80 = new System.Windows.Forms.Panel();
		this.label142 = new System.Windows.Forms.Label();
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
		this.tabPage10 = new System.Windows.Forms.TabPage();
		this.grdHistoryConsumptionDetail = new GridEX();
		this.panel82 = new System.Windows.Forms.Panel();
		this.label144 = new System.Windows.Forms.Label();
		this.grdHistoryConsumptionMai = new GridEX();
		this.panel83 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar17 = new grid.User_Conrtols.CtrlGrdBar();
		this.label157 = new System.Windows.Forms.Label();
		this.panel62 = new System.Windows.Forms.Panel();
		this.groupBox13 = new System.Windows.Forms.GroupBox();
		this.BtnShowConsupmtionHistory = new System.Windows.Forms.Button();
		this.label158 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoConsupmtionHistory = new UltraCombo();
		this.txtToDateConsupmtionHistory = new System.Windows.Forms.DateTimePicker();
		this.label159 = new System.Windows.Forms.Label();
		this.label160 = new System.Windows.Forms.Label();
		this.txtDocNoToConsupmtionHistory = new System.Windows.Forms.TextBox();
		this.txtFromDateConsupmtionHistory = new System.Windows.Forms.DateTimePicker();
		this.label161 = new System.Windows.Forms.Label();
		this.label162 = new System.Windows.Forms.Label();
		this.txtDocNofromConsupmtionHistory = new System.Windows.Forms.TextBox();
		this.panel61 = new System.Windows.Forms.Panel();
		this.label143 = new System.Windows.Forms.Label();
		this.toolStripConsumptionHistory = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistoryConsumption = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshConsupmtionHistory = new System.Windows.Forms.ToolStripButton();
		this.PackingMaterial = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel65 = new System.Windows.Forms.Panel();
		this.tabControlPm = new System.Windows.Forms.TabControl();
		this.OverHead = new System.Windows.Forms.TabPage();
		this.panel67 = new System.Windows.Forms.Panel();
		this.tabControl4 = new System.Windows.Forms.TabControl();
		this.TransctionHistory = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel29 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel43 = new System.Windows.Forms.Panel();
		this.label91 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel16 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox14 = new System.Windows.Forms.GroupBox();
		this.txtfromDateTransactionHistory = new System.Windows.Forms.DateTimePicker();
		this.label164 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.CmbPlantTransactionHistory = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.label163 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.btnSearchTransactionHistory = new System.Windows.Forms.Button();
		this.label52 = new System.Windows.Forms.Label();
		this.CmbJobOrderTransactionHistory = new UltraCombo();
		this.txtToDateTransactionHistory = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDocNumberTo = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel33 = new System.Windows.Forms.Panel();
		this.label90 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewTransactionHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshTransactionHistory = new System.Windows.Forms.ToolStripButton();
		this.Summary = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.Settlement = new System.Windows.Forms.TabPage();
		this.PanelOtherForms = new System.Windows.Forms.Panel();
		this.BtnGenerateRates = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Consumption.SuspendLayout();
		this.tabControl6.SuspendLayout();
		this.tabPage9.SuspendLayout();
		this.panel48.SuspendLayout();
		this.panel74.SuspendLayout();
		this.panel76.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdConsumption).BeginInit();
		this.panel77.SuspendLayout();
		this.panelManualEntryConsumption.SuspendLayout();
		this.groupBox9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouseConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeConsumption).BeginInit();
		this.panel79.SuspendLayout();
		this.groupBox10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPAcConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPItemConsumption).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderConsumption).BeginInit();
		this.panel80.SuspendLayout();
		this.toolStripConsumption.SuspendLayout();
		this.tabPage10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionDetail).BeginInit();
		this.panel82.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionMai).BeginInit();
		this.panel83.SuspendLayout();
		this.panel62.SuspendLayout();
		this.groupBox13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoConsupmtionHistory).BeginInit();
		this.panel61.SuspendLayout();
		this.toolStripConsumptionHistory.SuspendLayout();
		this.PackingMaterial.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel65.SuspendLayout();
		this.OverHead.SuspendLayout();
		this.panel67.SuspendLayout();
		this.TransctionHistory.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel43.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.groupBox14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantTransactionHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderTransactionHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel33.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.Summary.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.PanelOtherForms);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1370, 687);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.InPut);
		this.tabControl1.Controls.Add(this.OutPut);
		this.tabControl1.Controls.Add(this.Consumption);
		this.tabControl1.Controls.Add(this.PackingMaterial);
		this.tabControl1.Controls.Add(this.OverHead);
		this.tabControl1.Controls.Add(this.TransctionHistory);
		this.tabControl1.Controls.Add(this.Summary);
		this.tabControl1.Controls.Add(this.Settlement);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 639);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.InPut.BackColor = System.Drawing.Color.White;
		this.InPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.InPut.Location = new System.Drawing.Point(4, 26);
		this.InPut.Name = "InPut";
		this.InPut.Padding = new System.Windows.Forms.Padding(3);
		this.InPut.Size = new System.Drawing.Size(1362, 609);
		this.InPut.TabIndex = 0;
		this.InPut.Text = "Input";
		this.OutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OutPut.Location = new System.Drawing.Point(4, 26);
		this.OutPut.Name = "OutPut";
		this.OutPut.Size = new System.Drawing.Size(1362, 609);
		this.OutPut.TabIndex = 5;
		this.OutPut.Text = "OutPut";
		this.OutPut.UseVisualStyleBackColor = true;
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
		this.panel48.Controls.Add(this.panel74);
		this.panel48.Controls.Add(this.panelManualEntryConsumption);
		this.panel48.Controls.Add(this.panel79);
		this.panel48.Controls.Add(this.panel80);
		this.panel48.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel48.Location = new System.Drawing.Point(3, 30);
		this.panel48.Name = "panel48";
		this.panel48.Size = new System.Drawing.Size(1348, 547);
		this.panel48.TabIndex = 174;
		this.panel74.Controls.Add(this.panel76);
		this.panel74.Controls.Add(this.panel77);
		this.panel74.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel74.Location = new System.Drawing.Point(0, 290);
		this.panel74.Name = "panel74";
		this.panel74.Size = new System.Drawing.Size(1348, 257);
		this.panel74.TabIndex = 3;
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
		this.panel77.BackColor = System.Drawing.Color.Teal;
		this.panel77.Controls.Add(this.label120);
		this.panel77.Controls.Add(this.ctrlGrdBar16);
		this.panel77.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel77.Location = new System.Drawing.Point(0, 0);
		this.panel77.Name = "panel77";
		this.panel77.Size = new System.Drawing.Size(1348, 28);
		this.panel77.TabIndex = 1;
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label120.ForeColor = System.Drawing.Color.White;
		this.label120.Location = new System.Drawing.Point(3, 4);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(169, 19);
		this.label120.TabIndex = 172;
		this.label120.Text = "Consumption Detail";
		this.ctrlGrdBar16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar16.FormName = this;
		this.ctrlGrdBar16.Location = new System.Drawing.Point(1307, 3);
		this.ctrlGrdBar16.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar16.MyGrid = this.grdConsumption;
		this.ctrlGrdBar16.Name = "ctrlGrdBar16";
		this.ctrlGrdBar16.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar16.TabIndex = 20;
		this.ctrlGrdBar16.Load += new System.EventHandler(ctrlGrdBar16_Load);
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
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(329, 20);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 209;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
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
		this.cmbWarehouseConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbWarehouseConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWarehouseConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWarehouseConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance170;
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
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWarehouseConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
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
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label121.Location = new System.Drawing.Point(904, 22);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(90, 15);
		this.label121.TabIndex = 199;
		this.label121.Text = "Balance Weight";
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label122.Location = new System.Drawing.Point(9, 73);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(53, 15);
		this.label122.TabIndex = 206;
		this.label122.Text = "Quantity";
		this.txtItemQtyConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQtyConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyConsumption.Location = new System.Drawing.Point(6, 93);
		this.txtItemQtyConsumption.Name = "txtItemQtyConsumption";
		this.txtItemQtyConsumption.Size = new System.Drawing.Size(81, 23);
		this.txtItemQtyConsumption.TabIndex = 8;
		this.txtItemQtyConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQtyConsumption.TextChanged += new System.EventHandler(txtItemQtyConsumption_TextChanged);
		this.txtItemQtyConsumption.Leave += new System.EventHandler(txtItemQtyConsumption_Leave);
		this.txtBalanceWeightConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeightConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeightConsumption.Location = new System.Drawing.Point(904, 42);
		this.txtBalanceWeightConsumption.Name = "txtBalanceWeightConsumption";
		this.txtBalanceWeightConsumption.ReadOnly = true;
		this.txtBalanceWeightConsumption.Size = new System.Drawing.Size(93, 23);
		this.txtBalanceWeightConsumption.TabIndex = 7;
		this.txtBalanceWeightConsumption.TabStop = false;
		this.txtBalanceWeightConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightConsumption.Location = new System.Drawing.Point(89, 93);
		this.txtNetWeightConsumption.Name = "txtNetWeightConsumption";
		this.txtNetWeightConsumption.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeightConsumption.TabIndex = 9;
		this.txtNetWeightConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightConsumption.TextChanged += new System.EventHandler(txtNetWeightConsumption_TextChanged);
		this.txtNetWeightConsumption.Leave += new System.EventHandler(txtNetWeightConsumption_Leave);
		this.cmbPackUomConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUomConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUomConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUomConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUomConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance148;
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
		this.label123.AutoSize = true;
		this.label123.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label123.Location = new System.Drawing.Point(92, 73);
		this.label123.Name = "label123";
		this.label123.Size = new System.Drawing.Size(68, 15);
		this.label123.TabIndex = 207;
		this.label123.Text = "Net Weight";
		this.CmbJobLotConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance109;
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
		this.txtRemarksDetailConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetailConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetailConsumption.Location = new System.Drawing.Point(184, 93);
		this.txtRemarksDetailConsumption.Name = "txtRemarksDetailConsumption";
		this.txtRemarksDetailConsumption.Size = new System.Drawing.Size(380, 23);
		this.txtRemarksDetailConsumption.TabIndex = 10;
		this.CmbItemNameConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance112;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
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
		this.txtAmountConsumption.BackColor = System.Drawing.Color.White;
		this.txtAmountConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountConsumption.Location = new System.Drawing.Point(730, 93);
		this.txtAmountConsumption.Name = "txtAmountConsumption";
		this.txtAmountConsumption.ReadOnly = true;
		this.txtAmountConsumption.Size = new System.Drawing.Size(102, 23);
		this.txtAmountConsumption.TabIndex = 13;
		this.txtAmountConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbCropYearConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
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
		this.lblAmountConsumption.AutoSize = true;
		this.lblAmountConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAmountConsumption.Location = new System.Drawing.Point(731, 73);
		this.lblAmountConsumption.Name = "lblAmountConsumption";
		this.lblAmountConsumption.Size = new System.Drawing.Size(51, 15);
		this.lblAmountConsumption.TabIndex = 203;
		this.lblAmountConsumption.Text = "Amount";
		this.label125.AutoSize = true;
		this.label125.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label125.Location = new System.Drawing.Point(476, 22);
		this.label125.Name = "label125";
		this.label125.Size = new System.Drawing.Size(57, 15);
		this.label125.TabIndex = 194;
		this.label125.Text = "Crop Year";
		this.CmbRateUomConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomConsumption).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
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
		this.label126.AutoSize = true;
		this.label126.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label126.Location = new System.Drawing.Point(391, 22);
		this.label126.Name = "label126";
		this.label126.Size = new System.Drawing.Size(63, 15);
		this.label126.TabIndex = 192;
		this.label126.Text = "Pack UOM";
		this.lblRateUomConsumption.AutoSize = true;
		this.lblRateUomConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateUomConsumption.Location = new System.Drawing.Point(642, 73);
		this.lblRateUomConsumption.Name = "lblRateUomConsumption";
		this.lblRateUomConsumption.Size = new System.Drawing.Size(61, 15);
		this.lblRateUomConsumption.TabIndex = 202;
		this.lblRateUomConsumption.Text = "Rate UOM";
		this.label128.AutoSize = true;
		this.label128.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label128.Location = new System.Drawing.Point(581, 22);
		this.label128.Name = "label128";
		this.label128.Size = new System.Drawing.Size(46, 15);
		this.label128.TabIndex = 195;
		this.label128.Text = "Job Lot";
		this.txtRateConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateConsumption.Location = new System.Drawing.Point(566, 93);
		this.txtRateConsumption.Name = "txtRateConsumption";
		this.txtRateConsumption.Size = new System.Drawing.Size(74, 23);
		this.txtRateConsumption.TabIndex = 11;
		this.txtRateConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateConsumption.TextChanged += new System.EventHandler(txtRateConsumption_TextChanged);
		this.label129.AutoSize = true;
		this.label129.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label129.Location = new System.Drawing.Point(183, 22);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(67, 15);
		this.label129.TabIndex = 191;
		this.label129.Text = "Item Name";
		this.lblRateConsumption.AutoSize = true;
		this.lblRateConsumption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateConsumption.Location = new System.Drawing.Point(557, 73);
		this.lblRateConsumption.Name = "lblRateConsumption";
		this.lblRateConsumption.Size = new System.Drawing.Size(90, 15);
		this.lblRateConsumption.TabIndex = 201;
		this.lblRateConsumption.Text = "Avg Rate(40Kg)";
		this.CmbPackingTypeConsumption.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
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
		this.txtBalanceQtyConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQtyConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQtyConsumption.Location = new System.Drawing.Point(834, 42);
		this.txtBalanceQtyConsumption.Name = "txtBalanceQtyConsumption";
		this.txtBalanceQtyConsumption.ReadOnly = true;
		this.txtBalanceQtyConsumption.Size = new System.Drawing.Size(69, 23);
		this.txtBalanceQtyConsumption.TabIndex = 6;
		this.txtBalanceQtyConsumption.TabStop = false;
		this.txtBalanceQtyConsumption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
		this.label131.AutoSize = true;
		this.label131.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label131.Location = new System.Drawing.Point(739, 22);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(60, 15);
		this.label131.TabIndex = 196;
		this.label131.Text = "Pack Type";
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
		this.label132.AutoSize = true;
		this.label132.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label132.Location = new System.Drawing.Point(833, 22);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(70, 15);
		this.label132.TabIndex = 197;
		this.label132.Text = "Balance Qty";
		this.label133.AutoSize = true;
		this.label133.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label133.Location = new System.Drawing.Point(189, 73);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(52, 15);
		this.label133.TabIndex = 200;
		this.label133.Text = "Remarks";
		this.label134.AutoSize = true;
		this.label134.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label134.Location = new System.Drawing.Point(6, 22);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(69, 15);
		this.label134.TabIndex = 193;
		this.label134.Text = "WareHouse";
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
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(4, 22);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(88, 15);
		this.label135.TabIndex = 25;
		this.label135.Text = "Doc No && Date";
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
		this.txtDocNoConsumption.BackColor = System.Drawing.Color.White;
		this.txtDocNoConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoConsumption.Location = new System.Drawing.Point(92, 18);
		this.txtDocNoConsumption.Name = "txtDocNoConsumption";
		this.txtDocNoConsumption.ReadOnly = true;
		this.txtDocNoConsumption.Size = new System.Drawing.Size(77, 23);
		this.txtDocNoConsumption.TabIndex = 0;
		this.label136.AutoSize = true;
		this.label136.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label136.Location = new System.Drawing.Point(4, 81);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(69, 15);
		this.label136.TabIndex = 209;
		this.label136.Text = "Plant Name";
		this.txtDocDateConsumption.CustomFormat = "dd-MMM-yy";
		this.txtDocDateConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDateConsumption.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateConsumption.Location = new System.Drawing.Point(175, 18);
		this.txtDocDateConsumption.Name = "txtDocDateConsumption";
		this.txtDocDateConsumption.Size = new System.Drawing.Size(169, 23);
		this.txtDocDateConsumption.TabIndex = 1;
		this.txtDocDateConsumption.ValueChanged += new System.EventHandler(txtDocDateConsumption_ValueChanged);
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
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label137.Location = new System.Drawing.Point(351, 22);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(78, 15);
		this.label137.TabIndex = 171;
		this.label137.Text = "WIP Account";
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
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(351, 80);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(94, 15);
		this.label138.TabIndex = 173;
		this.label138.Text = "Pro_Department";
		this.cmbWIPItemConsumption.AllowNull = (DefaultableBoolean)1;
		this.cmbWIPItemConsumption.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWIPItemConsumption.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWIPItemConsumption.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWIPItemConsumption).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
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
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(351, 51);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(58, 15);
		this.label139.TabIndex = 173;
		this.label139.Text = "WIP Item";
		this.label140.AutoSize = true;
		this.label140.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label140.Location = new System.Drawing.Point(4, 51);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(78, 15);
		this.label140.TabIndex = 151;
		this.label140.Text = "Job Order No";
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
		this.label141.BackColor = System.Drawing.Color.LightGray;
		this.label141.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label141.Location = new System.Drawing.Point(665, 16);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(353, 26);
		this.label141.TabIndex = 6;
		this.label141.Text = "Remarks";
		this.label141.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtRemarksHeaderConsumption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderConsumption.Location = new System.Drawing.Point(665, 45);
		this.txtRemarksHeaderConsumption.Multiline = true;
		this.txtRemarksHeaderConsumption.Name = "txtRemarksHeaderConsumption";
		this.txtRemarksHeaderConsumption.Size = new System.Drawing.Size(353, 55);
		this.txtRemarksHeaderConsumption.TabIndex = 7;
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
		this.toolStripConsumption.BackColor = System.Drawing.Color.White;
		this.toolStripConsumption.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripConsumption.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripConsumption.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.BtnNewConsumption, this.BtnRefreshConsumption, this.btnSaveConsumption, this.BtnUpdateconsumption, this.toolStripSeparator7, this.BtnPrintSlipConsumption, this.btnPrintVoucherConsumption, this.BtnGenerateRates, this.BtnPendingForRatesFormConsumption, this.BtnLoadDataForConsumptionEntry });
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
		this.panel83.BackColor = System.Drawing.Color.Teal;
		this.panel83.Controls.Add(this.ctrlGrdBar17);
		this.panel83.Controls.Add(this.label157);
		this.panel83.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel83.Location = new System.Drawing.Point(3, 103);
		this.panel83.Name = "panel83";
		this.panel83.Size = new System.Drawing.Size(1348, 23);
		this.panel83.TabIndex = 180;
		this.ctrlGrdBar17.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar17.FormName = this;
		this.ctrlGrdBar17.Location = new System.Drawing.Point(1308, 0);
		this.ctrlGrdBar17.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar17.MyGrid = null;
		this.ctrlGrdBar17.Name = "ctrlGrdBar17";
		this.ctrlGrdBar17.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar17.TabIndex = 174;
		this.ctrlGrdBar17.Load += new System.EventHandler(ctrlGrdBar17_Load);
		this.label157.AutoSize = true;
		this.label157.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label157.ForeColor = System.Drawing.Color.White;
		this.label157.Location = new System.Drawing.Point(3, 3);
		this.label157.Name = "label157";
		this.label157.Size = new System.Drawing.Size(40, 17);
		this.label157.TabIndex = 212;
		this.label157.Text = "Main";
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
		this.label158.AutoSize = true;
		this.label158.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label158.Location = new System.Drawing.Point(683, 20);
		this.label158.Name = "label158";
		this.label158.Size = new System.Drawing.Size(59, 15);
		this.label158.TabIndex = 3;
		this.label158.Text = "Job Order";
		this.CmbJobOrderNoConsupmtionHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoConsupmtionHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoConsupmtionHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoConsupmtionHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoConsupmtionHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoConsupmtionHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Name = "CmbJobOrderNoConsupmtionHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoConsupmtionHistory).TabIndex = 4;
		this.txtToDateConsupmtionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateConsupmtionHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateConsupmtionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateConsupmtionHistory.Location = new System.Drawing.Point(268, 16);
		this.txtToDateConsupmtionHistory.Name = "txtToDateConsupmtionHistory";
		this.txtToDateConsupmtionHistory.Size = new System.Drawing.Size(148, 23);
		this.txtToDateConsupmtionHistory.TabIndex = 1;
		this.label159.AutoSize = true;
		this.label159.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label159.Location = new System.Drawing.Point(217, 20);
		this.label159.Name = "label159";
		this.label159.Size = new System.Drawing.Size(48, 15);
		this.label159.TabIndex = 171;
		this.label159.Text = "Date To";
		this.label160.AutoSize = true;
		this.label160.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label160.Location = new System.Drawing.Point(2, 20);
		this.label160.Name = "label160";
		this.label160.Size = new System.Drawing.Size(63, 15);
		this.label160.TabIndex = 171;
		this.label160.Text = "Date From";
		this.txtDocNoToConsupmtionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoToConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoToConsupmtionHistory.Location = new System.Drawing.Point(609, 16);
		this.txtDocNoToConsupmtionHistory.Name = "txtDocNoToConsupmtionHistory";
		this.txtDocNoToConsupmtionHistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNoToConsupmtionHistory.TabIndex = 3;
		this.txtFromDateConsupmtionHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateConsupmtionHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateConsupmtionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateConsupmtionHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromDateConsupmtionHistory.Name = "txtFromDateConsupmtionHistory";
		this.txtFromDateConsupmtionHistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromDateConsupmtionHistory.TabIndex = 0;
		this.label161.AutoSize = true;
		this.label161.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label161.Location = new System.Drawing.Point(564, 20);
		this.label161.Name = "label161";
		this.label161.Size = new System.Drawing.Size(45, 15);
		this.label161.TabIndex = 2;
		this.label161.Text = "Doc To";
		this.label162.AutoSize = true;
		this.label162.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label162.Location = new System.Drawing.Point(429, 20);
		this.label162.Name = "label162";
		this.label162.Size = new System.Drawing.Size(60, 15);
		this.label162.TabIndex = 173;
		this.label162.Text = "Doc From";
		this.txtDocNofromConsupmtionHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNofromConsupmtionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNofromConsupmtionHistory.Location = new System.Drawing.Point(490, 16);
		this.txtDocNofromConsupmtionHistory.Name = "txtDocNofromConsupmtionHistory";
		this.txtDocNofromConsupmtionHistory.Size = new System.Drawing.Size(72, 23);
		this.txtDocNofromConsupmtionHistory.TabIndex = 2;
		this.panel61.BackColor = System.Drawing.Color.Teal;
		this.panel61.Controls.Add(this.label143);
		this.panel61.Controls.Add(this.toolStripConsumptionHistory);
		this.panel61.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel61.Location = new System.Drawing.Point(3, 3);
		this.panel61.Name = "panel61";
		this.panel61.Size = new System.Drawing.Size(1348, 52);
		this.panel61.TabIndex = 178;
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label143.ForeColor = System.Drawing.Color.White;
		this.label143.Location = new System.Drawing.Point(3, 30);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(198, 19);
		this.label143.TabIndex = 211;
		this.label143.Text = "Consumption History    ";
		this.toolStripConsumptionHistory.BackColor = System.Drawing.Color.White;
		this.toolStripConsumptionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripConsumptionHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripConsumptionHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistoryConsumption, this.btnRefreshConsupmtionHistory });
		this.toolStripConsumptionHistory.Location = new System.Drawing.Point(0, 0);
		this.toolStripConsumptionHistory.Name = "toolStripConsumptionHistory";
		this.toolStripConsumptionHistory.Size = new System.Drawing.Size(1348, 27);
		this.toolStripConsumptionHistory.TabIndex = 4;
		this.toolStripConsumptionHistory.Text = "toolStrip9";
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
		this.PackingMaterial.Controls.Add(this.panel7);
		this.PackingMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PackingMaterial.Location = new System.Drawing.Point(4, 26);
		this.PackingMaterial.Name = "PackingMaterial";
		this.PackingMaterial.Size = new System.Drawing.Size(1362, 609);
		this.PackingMaterial.TabIndex = 3;
		this.PackingMaterial.Text = "Packing Material";
		this.PackingMaterial.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel65);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1362, 609);
		this.panel7.TabIndex = 0;
		this.panel65.Controls.Add(this.tabControlPm);
		this.panel65.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel65.Location = new System.Drawing.Point(0, 0);
		this.panel65.Name = "panel65";
		this.panel65.Size = new System.Drawing.Size(1362, 609);
		this.panel65.TabIndex = 68;
		this.tabControlPm.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlPm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlPm.Location = new System.Drawing.Point(0, 0);
		this.tabControlPm.Name = "tabControlPm";
		this.tabControlPm.SelectedIndex = 0;
		this.tabControlPm.Size = new System.Drawing.Size(1362, 609);
		this.tabControlPm.TabIndex = 0;
		this.OverHead.Controls.Add(this.panel67);
		this.OverHead.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OverHead.Location = new System.Drawing.Point(4, 26);
		this.OverHead.Name = "OverHead";
		this.OverHead.Size = new System.Drawing.Size(1362, 609);
		this.OverHead.TabIndex = 4;
		this.OverHead.Text = "Over Head";
		this.OverHead.UseVisualStyleBackColor = true;
		this.panel67.Controls.Add(this.tabControl4);
		this.panel67.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel67.Location = new System.Drawing.Point(0, 0);
		this.panel67.Name = "panel67";
		this.panel67.Size = new System.Drawing.Size(1362, 609);
		this.panel67.TabIndex = 69;
		this.tabControl4.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl4.Location = new System.Drawing.Point(0, 0);
		this.tabControl4.Name = "tabControl4";
		this.tabControl4.SelectedIndex = 0;
		this.tabControl4.Size = new System.Drawing.Size(1362, 609);
		this.tabControl4.TabIndex = 0;
		this.TransctionHistory.Controls.Add(this.panel10);
		this.TransctionHistory.Location = new System.Drawing.Point(4, 26);
		this.TransctionHistory.Name = "TransctionHistory";
		this.TransctionHistory.Padding = new System.Windows.Forms.Padding(3);
		this.TransctionHistory.Size = new System.Drawing.Size(1362, 609);
		this.TransctionHistory.TabIndex = 1;
		this.TransctionHistory.Text = "Transaction History";
		this.TransctionHistory.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1356, 603);
		this.panel10.TabIndex = 0;
		this.panel13.Controls.Add(this.panel29);
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 54);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1356, 549);
		this.panel13.TabIndex = 2;
		this.panel29.Controls.Add(this.panel17);
		this.panel29.Controls.Add(this.panel43);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 357);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1356, 192);
		this.panel29.TabIndex = 46;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 27);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1356, 165);
		this.panel17.TabIndex = 47;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1356, 165);
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
		this.panel43.Size = new System.Drawing.Size(1356, 27);
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
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1315, 3);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 60);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1356, 297);
		this.panel16.TabIndex = 45;
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
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1356, 297);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 44;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.panel15.Controls.Add(this.groupBox14);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1356, 60);
		this.panel15.TabIndex = 0;
		this.groupBox14.Controls.Add(this.txtfromDateTransactionHistory);
		this.groupBox14.Controls.Add(this.label164);
		this.groupBox14.Controls.Add(this.txtDocNoFrom);
		this.groupBox14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory);
		this.groupBox14.Controls.Add(this.label49);
		this.groupBox14.Controls.Add(this.label163);
		this.groupBox14.Controls.Add(this.label48);
		this.groupBox14.Controls.Add(this.btnSearchTransactionHistory);
		this.groupBox14.Controls.Add(this.label52);
		this.groupBox14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory);
		this.groupBox14.Controls.Add(this.txtToDateTransactionHistory);
		this.groupBox14.Controls.Add(this.label50);
		this.groupBox14.Controls.Add(this.txtDocNumberTo);
		this.groupBox14.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox14.Location = new System.Drawing.Point(0, 0);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(972, 60);
		this.groupBox14.TabIndex = 58;
		this.groupBox14.TabStop = false;
		this.groupBox14.Text = "Filters";
		this.txtfromDateTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromDateTransactionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtfromDateTransactionHistory.Location = new System.Drawing.Point(6, 33);
		this.txtfromDateTransactionHistory.Name = "txtfromDateTransactionHistory";
		this.txtfromDateTransactionHistory.ShowCheckBox = true;
		this.txtfromDateTransactionHistory.Size = new System.Drawing.Size(133, 23);
		this.txtfromDateTransactionHistory.TabIndex = 0;
		this.label164.AutoSize = true;
		this.label164.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label164.Location = new System.Drawing.Point(701, 16);
		this.label164.Name = "label164";
		this.label164.Size = new System.Drawing.Size(34, 15);
		this.label164.TabIndex = 56;
		this.label164.Text = "Plant";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(274, 33);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(113, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.CmbPlantTransactionHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantTransactionHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantTransactionHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantTransactionHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantTransactionHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantTransactionHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory).Location = new System.Drawing.Point(701, 31);
		((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory).Name = "CmbPlantTransactionHistory";
		((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantTransactionHistory).TabIndex = 5;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(274, 16);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(79, 15);
		this.label49.TabIndex = 49;
		this.label49.Text = "Doc No From";
		this.label163.AutoSize = true;
		this.label163.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label163.Location = new System.Drawing.Point(502, 16);
		this.label163.Name = "label163";
		this.label163.Size = new System.Drawing.Size(59, 15);
		this.label163.TabIndex = 45;
		this.label163.Text = "Job Order";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 16);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(63, 15);
		this.label48.TabIndex = 47;
		this.label48.Text = "Date From";
		this.btnSearchTransactionHistory.BackColor = System.Drawing.Color.Teal;
		this.btnSearchTransactionHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchTransactionHistory.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSearchTransactionHistory.ForeColor = System.Drawing.Color.White;
		this.btnSearchTransactionHistory.Location = new System.Drawing.Point(900, 30);
		this.btnSearchTransactionHistory.Name = "btnSearchTransactionHistory";
		this.btnSearchTransactionHistory.Size = new System.Drawing.Size(65, 28);
		this.btnSearchTransactionHistory.TabIndex = 6;
		this.btnSearchTransactionHistory.Text = "Search";
		this.btnSearchTransactionHistory.UseVisualStyleBackColor = false;
		this.btnSearchTransactionHistory.Click += new System.EventHandler(btnSearchTransactionHistory_Click);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(140, 16);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 52;
		this.label52.Text = "Date To";
		this.CmbJobOrderTransactionHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderTransactionHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderTransactionHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderTransactionHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderTransactionHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderTransactionHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Location = new System.Drawing.Point(502, 31);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Name = "CmbJobOrderTransactionHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderTransactionHistory).TabIndex = 4;
		this.txtToDateTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateTransactionHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDateTransactionHistory.Location = new System.Drawing.Point(140, 33);
		this.txtToDateTransactionHistory.Name = "txtToDateTransactionHistory";
		this.txtToDateTransactionHistory.ShowCheckBox = true;
		this.txtToDateTransactionHistory.Size = new System.Drawing.Size(133, 23);
		this.txtToDateTransactionHistory.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(388, 16);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 54;
		this.label50.Text = "Doc No To";
		this.txtDocNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNumberTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNumberTo.Location = new System.Drawing.Point(388, 33);
		this.txtDocNumberTo.Name = "txtDocNumberTo";
		this.txtDocNumberTo.Size = new System.Drawing.Size(113, 23);
		this.txtDocNumberTo.TabIndex = 3;
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.panel33);
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1356, 54);
		this.panel11.TabIndex = 0;
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.label90);
		this.panel33.Controls.Add(this.ctrlGrdBar1);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel33.Location = new System.Drawing.Point(0, 27);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1356, 27);
		this.panel33.TabIndex = 6;
		this.label90.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label90.ForeColor = System.Drawing.Color.White;
		this.label90.Location = new System.Drawing.Point(5, 3);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(168, 19);
		this.label90.TabIndex = 177;
		this.label90.Text = "Transaction History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1315, 2);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 4;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewTransactionHistory, this.btnRefreshTransactionHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1356, 27);
		this.toolStrip2.TabIndex = 5;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewTransactionHistory.ForeColor = System.Drawing.Color.Black;
		this.btnNewTransactionHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewTransactionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewTransactionHistory.Name = "btnNewTransactionHistory";
		this.btnNewTransactionHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewTransactionHistory.Text = "&New";
		this.btnNewTransactionHistory.Click += new System.EventHandler(btnNewTransactionHistory_Click);
		this.btnRefreshTransactionHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshTransactionHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshTransactionHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshTransactionHistory.Name = "btnRefreshTransactionHistory";
		this.btnRefreshTransactionHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshTransactionHistory.Text = "Refresh";
		this.btnRefreshTransactionHistory.Click += new System.EventHandler(btnRefreshTransactionHistory_Click);
		this.Summary.Controls.Add(this.panel18);
		this.Summary.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Summary.Location = new System.Drawing.Point(4, 26);
		this.Summary.Name = "Summary";
		this.Summary.Size = new System.Drawing.Size(1362, 609);
		this.Summary.TabIndex = 2;
		this.Summary.Text = "Summary";
		this.Summary.UseVisualStyleBackColor = true;
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1362, 609);
		this.panel18.TabIndex = 0;
		this.Settlement.AccessibleName = "Settlement";
		this.Settlement.AutoScroll = true;
		this.Settlement.Font = new System.Drawing.Font("Cambria", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Settlement.Location = new System.Drawing.Point(4, 26);
		this.Settlement.Name = "Settlement";
		this.Settlement.Size = new System.Drawing.Size(1362, 609);
		this.Settlement.TabIndex = 6;
		this.Settlement.Text = "Settlement";
		this.Settlement.UseVisualStyleBackColor = true;
		this.PanelOtherForms.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PanelOtherForms.Location = new System.Drawing.Point(0, 639);
		this.PanelOtherForms.Name = "PanelOtherForms";
		this.PanelOtherForms.Size = new System.Drawing.Size(1370, 48);
		this.PanelOtherForms.TabIndex = 173;
		this.PanelOtherForms.Visible = false;
		this.BtnGenerateRates.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.BtnGenerateRates.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnGenerateRates.Name = "BtnGenerateRates";
		this.BtnGenerateRates.Size = new System.Drawing.Size(133, 24);
		this.BtnGenerateRates.Text = "Generate Rates";
		this.BtnGenerateRates.Click += new System.EventHandler(BtnGenerateRates_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1370, 687);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "FoodProductionWithValues";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Consumption.ResumeLayout(false);
		this.tabControl6.ResumeLayout(false);
		this.tabPage9.ResumeLayout(false);
		this.tabPage9.PerformLayout();
		this.panel48.ResumeLayout(false);
		this.panel74.ResumeLayout(false);
		this.panel76.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdConsumption).EndInit();
		this.panel77.ResumeLayout(false);
		this.panel77.PerformLayout();
		this.panelManualEntryConsumption.ResumeLayout(false);
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouseConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeConsumption).EndInit();
		this.panel79.ResumeLayout(false);
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPAcConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWIPItemConsumption).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderConsumption).EndInit();
		this.panel80.ResumeLayout(false);
		this.panel80.PerformLayout();
		this.toolStripConsumption.ResumeLayout(false);
		this.toolStripConsumption.PerformLayout();
		this.tabPage10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionDetail).EndInit();
		this.panel82.ResumeLayout(false);
		this.panel82.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryConsumptionMai).EndInit();
		this.panel83.ResumeLayout(false);
		this.panel83.PerformLayout();
		this.panel62.ResumeLayout(false);
		this.groupBox13.ResumeLayout(false);
		this.groupBox13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoConsupmtionHistory).EndInit();
		this.panel61.ResumeLayout(false);
		this.panel61.PerformLayout();
		this.toolStripConsumptionHistory.ResumeLayout(false);
		this.toolStripConsumptionHistory.PerformLayout();
		this.PackingMaterial.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel65.ResumeLayout(false);
		this.OverHead.ResumeLayout(false);
		this.panel67.ResumeLayout(false);
		this.TransctionHistory.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel43.ResumeLayout(false);
		this.panel43.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.groupBox14.ResumeLayout(false);
		this.groupBox14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantTransactionHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderTransactionHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel33.ResumeLayout(false);
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.Summary.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
