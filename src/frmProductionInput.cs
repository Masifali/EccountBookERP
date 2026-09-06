using System;
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

public class frmProductionInput : Form
{
	private bool UpdateMode;

	private int RecId;

	private int VoucherHeadId;

	private int updateDetailIndexInput = -1;

	private double AvgRate;

	private double AvgRateWithoutExp;

	private string InputDetailRowsRemoveIds = "";

	private string JoborderInputitems = "";

	public static bool ValuesShowRights;

	private DataTable dtBranch = new DataTable();

	private DataTable dtCropInPut = new DataTable();

	private DataTable dtJobInPut = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtuomlst = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private DataTable dtJobOrderInputItems = new DataTable();

	private bool LoaderStatus;

	private bool WagesStatus;

	private bool FIFOCGSFlag;

	private bool IsManualEntryOnInputNotAllowed;

	private bool JobOrderCreatewithoutRates;

	private bool WagesActiveOrInActiveForInput;

	private bool SaleCostingJobOrderWise;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadOutPutPendingforRates LoadPendingforRates;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private frmPendingMoveOrderDocuments LoadMoveOrder;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

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

	private ToolStrip miniToolStrip;

	private ToolStripButton BtnGenerateRates;

	internal UltraCombo CmbWipWarehouse;

	private Label label35;

	private GroupBox groupBox3;

	internal UltraCombo CmbMoveOrderTicket;

	private Label label3;

	private Button BtnLoadMoveOrder;

	private TextBox txtMoveOrderNetWeight;

	private Label label4;

	private TextBox txtMoveOrderBalWeight;

	private Label label7;

	private TextBox txtMoveOrderAlreadUsedWeight;

	private Label label5;

	private TextBox txtEbTotal;

	private Label label165;

	private TextBox txtEbUnit;

	private Label label166;

	private Label label167;

	private TextBox txtGrossWeight;

	private PictureBox BtnResetMoveOrder;

	public frmProductionInput(UserAccount user)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
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
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnProduction"));
			LoaderStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IssuanceByLoader"));
			JobOrderCreatewithoutRates = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("JobOrderCreatewithoutRates"));
			IsManualEntryOnInputNotAllowed = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsManualEntryOnInputNotAllowed"));
			if (IsManualEntryOnInputNotAllowed)
			{
				PanelInPutManualEntry.Visible = false;
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
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar3.mGridChooseFielder.Enabled = false;
			}
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			if (FIFOCGSFlag)
			{
				label83.Visible = true;
				btnPendingForRateInput.Visible = false;
			}
			else
			{
				label83.Visible = false;
			}
			dtCropInPut.Columns.Add("Id", typeof(int));
			dtCropInPut.Columns.Add("CropYear");
			dtJobInPut.Columns.Add("Id", typeof(int));
			dtJobInPut.Columns.Add("JobLotDescription");
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
			table.Columns.Add("EbUnit", typeof(double));
			table.Columns.Add("EbTotal", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateForCheck", typeof(double));
			table.Columns.Add("RateUOMId");
			table.Columns.Add("RateUOM");
			table.Columns.Add("RateUOMForCheck");
			table.Columns.Add("ItemAmount", typeof(decimal));
			table.Columns.Add("Remarks");
			BranchesFill();
			GenerateDocNumberInput();
			UOMFill();
			JobLotInput();
			CropYearInput();
			bagType();
			CommondtForCombosFillInput();
			JobOrderNoFill();
			WarehouseInputFill();
			BindProductionDepartment();
			btnUpdateDetailInput.Visible = false;
			btnCancelInput.Visible = false;
			txtRemarksInput.Width = 527;
			if (!formright.DoHaveCanRateandAmount)
			{
				txtRemarksInput.Width = 793;
				txtRateInput.Visible = false;
				((Control)(object)cmbRateuomInput).Visible = false;
				txtAmountInput.Visible = false;
				lblRateInput.Visible = false;
				lblRateUom.Visible = false;
				lblAmountInput.Visible = false;
				BtnGenerateRates.Visible = false;
				btnVoucherReport.Visible = false;
			}
			txtFromdateHistory.Value = DateTime.Now.AddDays(-3.0);
			jobOrderInputHCombobind();
			if (FIFOCGSFlag)
			{
				txtRateInput.Enabled = false;
				((Control)(object)cmbRateuomInput).Enabled = false;
			}
			else
			{
				txtRateInput.Enabled = true;
				((Control)(object)cmbRateuomInput).Enabled = false;
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
				((UltraGridBase)CmbProductionDepartment).DisplayLayout.Bands[0].Columns[1].Width = 400;
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
			int JobInputId = Conversion.ToInt(CmbJobOrderNo.Value);
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
					((UltraGridBase)CmbJobOrderNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (JobInputId > 0)
				{
					CmbJobOrderNo.Value = JobInputId;
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			int JobInputId = Conversion.ToInt(CmbPlantFeeder.Value);
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
				if (JobInputId > 0)
				{
					CmbPlantFeeder.Value = JobInputId;
				}
			}
			else
			{
				((Control)(object)CmbPlantFeeder).Text = string.Empty;
				((UltraGridBase)CmbPlantFeeder).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void getJobOrderItems(int JobOrderId)
	{
		try
		{
			if (!JobOrderCreatewithoutRates)
			{
				return;
			}
			dtJobOrderInputItems.Rows.Clear();
			JoborderInputitems = string.Empty;
			dtJobOrderInputItems = InvProductionJobOrder.getJobOrderItemsById(JobOrderId, 1);
			if (dtJobOrderInputItems.Rows.Count > 0)
			{
				JoborderInputitems = string.Join(",", from x in dtJobOrderInputItems.AsEnumerable()
					select x.Field<int>("ItemId"));
				DDL.BindDDLNew(dtJobOrderInputItems, cmbItemInput, "ItemId", "ItemName", "Item", false);
			}
			else
			{
				((Control)(object)cmbItemInput).Text = string.Empty;
				((UltraGridBase)cmbItemInput).DataSource = null;
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
			if (tabControl2.SelectedIndex == 0)
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
			GetAvgRateByItemAndJoblot();
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
			GetAvgRateByItemAndJoblot();
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
			GetAvgRateByItemAndJoblot();
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
			GetAvgRateByItemAndJoblot();
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
				DDL.BindDDLNew(dt, CmbWipWarehouse, "WipWareHouseId", "WareHouseName", "WareHouse Name", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
				((UltraGridBase)CmbWipWarehouse).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)CmbWorkInProcess).DataSource = null;
				((UltraGridBase)CmbWIPItem).DataSource = null;
				((UltraGridBase)CmbWipWarehouse).DataSource = null;
			}
			GetPlantFeeder(Pro.Id);
			getJobOrderItems(Pro.Id);
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
			if (((UltraGridBase)cmbWareHouseInput).ActiveRow != null && !JobOrderCreatewithoutRates)
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
		if (((UltraGridBase)CmbWipWarehouse).ActiveRow == null || Conversion.ToInt(CmbWipWarehouse.Value) == 0)
		{
			MessageBox.Show("WIP WareHouse Field Required");
			CmbWipWarehouse.Focus();
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
		if (txtGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("GrossWeight Field Required");
			txtGrossWeight.Focus();
			return false;
		}
		return true;
	}

	private void ValidateMoveOrder()
	{
		try
		{
			if (Conversion.ToInt(CmbMoveOrderTicket.Value) > 0)
			{
				double gridTotalWeight = Conversion.ToDouble((object)GridEX_Helper.GetColumnSum(grdStockConversionProductionDetail, "GrossWeight"));
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

	private void InsertInput()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ValidateMoveOrder();
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
			invFoodProduction.WipWareHouseId = Conversion.ToInt(CmbWipWarehouse.Value);
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
					Conversiondetail.MoveOrderDocumentTypeId = 74;
					Conversiondetail.MoveOrderDocId = Conversion.ToInt(CmbMoveOrderTicket.Value);
					Conversiondetail.EbUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
					Conversiondetail.EbTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
					Conversiondetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					if (Conversiondetail.GrossWeight == 0.0)
					{
						MessageBox.Show("GrossWeight Field Required Input Detail");
						return;
					}
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
			List<InvFoodProductionDetail> invFoodProductionDetails = SC.invFoodProductionDetails;
			if (invFoodProductionDetails != null && invFoodProductionDetails.Count > 0)
			{
				InvFoodProductionDetail FirstRow = SC.invFoodProductionDetails[0];
				BindMoveOrderTicket(FirstRow.MoveOrderDocId, FirstRow.WbTicketNo);
				txtMoveOrderNetWeight.Text = FirstRow.TotalWbWeight.ToString("#,##");
				txtMoveOrderAlreadUsedWeight.Text = FirstRow.UsedWbWeight.ToString("#,##");
				txtMoveOrderBalWeight.Text = FirstRow.AvailableWbWeight.ToString("#,##");
			}
			CmbJobOrderNo_Leave(null, null);
			CmbPlantFeeder.Value = SC.PlantId;
			table.Rows.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType == "Issue")
				{
					tabControl2.SelectedIndex = 0;
					table.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].RefDocumentTypeId, SC.invFoodProductionDetails[i].RefDocumentType, SC.invFoodProductionDetails[i].RefDocNoId, SC.invFoodProductionDetails[i].RefDocSubIdNo, SC.invFoodProductionDetails[i].SupplierCustomerId, SC.invFoodProductionDetails[i].PartyName, Convert.ToDateTime(SC.invFoodProductionDetails[i].RefDocDate).ToString("dd-MMM-yy"), SC.invFoodProductionDetails[i].RefDocNo, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].ItemUomId, SC.invFoodProductionDetails[i].PackUom, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].EbUnit, SC.invFoodProductionDetails[i].EbTotal, SC.invFoodProductionDetails[i].GrossWeight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].RateEquivalent, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
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
			((UltraGridBase)CmbMoveOrderTicket).DataSource = null;
			((Control)(object)CmbMoveOrderTicket).Text = "";
			txtMoveOrderNetWeight.Text = string.Empty;
			txtMoveOrderAlreadUsedWeight.Text = string.Empty;
			txtMoveOrderBalWeight.Text = string.Empty;
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
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
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
				double Qty3 = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemEquivalent"].Value.ToString());
				double Weight3 = 0.0;
				if (Conversion.ToDouble(item.Cells["BalanceQty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BalanceQty"].Value) < Qty3 && (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0))
				{
					item.Cells["Quantity"].Value = 0;
					item.Cells["Weight"].Value = 0;
					Qty3 = 0.0;
					MessageBox.Show("Qty cannot greater than BalanceQty Please check!");
				}
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					Weight3 = Conversion.ToDouble(item.Cells["BalanceWeight"].Value) / Conversion.ToDouble(item.Cells["BalanceQty"].Value) * Qty3;
					if (Conversion.ToDouble(item.Cells["BalanceWeight"].Value) < Weight3)
					{
						Weight3 = 0.0;
						item.Cells["Weight"].Value = 0;
						MessageBox.Show("Weight cannot greater than Balance Weight Please check!");
					}
				}
				else
				{
					Weight3 = Qty3 * Uom;
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
				double EbTotal4 = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
				double GrossWeight4 = Weight3 + EbTotal4;
				item.Cells["GrossWeight"].Value = GrossWeight4;
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
				double EbTotal3 = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
				double GrossWeight3 = Weight2 + EbTotal3;
				item.Cells["GrossWeight"].Value = GrossWeight3;
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
			if (e.Column.Key == "EbUnit")
			{
				double num = Conversion.ToDouble((object)item.Cells["EbUnit"].Value.ToString());
				double Qty2 = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double EbTotal2 = num * Qty2;
				item.Cells["EbTotal"].Value = EbTotal2;
				double GrossWeight2 = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString()) + EbTotal2;
				item.Cells["GrossWeight"].Value = GrossWeight2;
			}
			if (e.Column.Key == "EbTotal")
			{
				double EbTotal = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double EbUnit = ((Qty > 0.0) ? (EbTotal / Qty) : 0.0);
				item.Cells["EbUnit"].Value = EbUnit;
				double GrossWeight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString()) + EbTotal;
				item.Cells["GrossWeight"].Value = GrossWeight;
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
				txtEbUnit.Text = item.Cells["EbUnit"].Text.ToString();
				txtEbTotal.Text = item.Cells["EbTotal"].Text.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Text.ToString();
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "WareHouseId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "SupplierCustomerId", "JobLotId", "WarehouseId", "PackingTypeId",
				"ItemUOMId", "RateUOMId", "RateForCheck", "RateUOMForCheck", "ItemEquivalent"
			};
			if (!formright.DoHaveCanRateandAmount)
			{
				hiddenColumns.AddRange(new string[3] { "Rate", "RateUOM", "ItemAmount" });
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Quantity",
					(EditType)1
				},
				{
					"Weight",
					(EditType)1
				},
				{
					"EbUnit",
					(EditType)1
				},
				{
					"EbTotal",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdStockConversionProductionDetail, 2, 3, hiddenColumns, editableColumns);
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].Width = 150;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].Width = 300;
			GridEX_Helper.AddButton(grdStockConversionProductionDetail, "Delete", "X", 20, 0);
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
				table.Rows.Add(0, 0, "", 0, 0, 0, "", 0, 0, cmbWareHouseInput.Value, ((Control)(object)cmbWareHouseInput).Text, cmbItemInput.Value, ((Control)(object)cmbItemInput).Text, cmbItemUomInput.Value, ((Control)(object)cmbItemUomInput).Text, Conversion.ToDouble(((UltraDropDownBase)cmbItemUomInput).SelectedRow.Cells["Equivalent"].Value), ((Control)(object)cmbCropYearInput).Text, cmbLotInput.Value, ((Control)(object)cmbLotInput).Text, cmbPackingTypeInput.Value, ((Control)(object)cmbPackingTypeInput).Text, Conversion.ToDouble((object)txtBalanceQty.Text), Conversion.ToDouble((object)txtBalanceWeight.Text), Conversion.ToDouble((object)txtQtyInput.Text), Conversion.ToDouble((object)txtNetWeightInput.Text), Conversion.ToDouble((object)txtEbUnit.Text), Conversion.ToDouble((object)txtEbTotal.Text), Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtRateInput.Text), Conversion.ToDouble((object)txtRateInput.Text), cmbRateuomInput.Value, ((Control)(object)cmbRateuomInput).Text, (Conversion.ToInt(cmbRateuomInput.Value) > 0) ? Conversion.ToInt(((UltraDropDownBase)cmbRateuomInput).SelectedRow.Cells["Equivalent"].Value) : 0, Conversion.ToDouble((object)txtAmountInput.Text), txtRemarksInput.Text);
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
				table.Rows[updateDetailIndexInput]["EbUnit"] = txtEbUnit.Text.Trim();
				table.Rows[updateDetailIndexInput]["EbTotal"] = txtEbTotal.Text.Trim();
				table.Rows[updateDetailIndexInput]["GrossWeight"] = txtGrossWeight.Text.Trim();
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

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			if (JobOrderCreatewithoutRates && (((UltraGridBase)CmbJobOrderNo).ActiveRow == null || Conversion.ToInt(CmbJobOrderNo.Value) == 0))
			{
				throw new Exception("Job Order Select First");
			}
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.dtjoborderItems = dtJobOrderInputItems;
			LoadTransactions.JobOrderItems = JoborderInputitems;
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
					table.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocumentType"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], dtpurchaseinvoice.Rows[i]["SupplierCustomerId"], dtpurchaseinvoice.Rows[i]["PartyName"], Conversion.ToDateTime(dtpurchaseinvoice.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpurchaseinvoice.Rows[i]["DocNo"], dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["PackUom"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], 0, 0, dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["RateUOM"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"], "");
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
				dtgrdinput.Columns.Add("WbTicketNo", typeof(int));
				dtgrdinput.Columns.Add("TotalWbWeight", typeof(decimal));
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
						dtgrdinput.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], dthis.Rows[i]["WagesNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["PlantName"], dthis.Rows[i]["WbTicketNo"], dthis.Rows[i]["TotalWbWeight"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["MainRemarks"], dthis.Rows[i]["JobOrderApprovedStatus"], Conversion.ToDateTime(dthis.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["EndDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["PlanStatus"], dthis.Rows[i]["WagesId"]);
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdinputhistory, 3, 2);
			grdinputhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdinputhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
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

	private void cmbItemUomInput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			GetAvgRateByItemAndJoblot();
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
				double Qty = Conversion.ToDouble((object)txtQtyInput.Text);
				double Weight = num * Qty;
				txtNetWeightInput.Text = Weight.ToString("#,##0.###");
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
				txtGrossWeight.Text = (Weight + EbTotal).ToString("#,##0.###");
			}
			else
			{
				txtNetWeightInput.Text = "0";
				txtGrossWeight.Text = "0";
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
			GetAvgRateByItemAndJoblot();
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
			GetAvgRateByItemAndJoblot();
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
			if (grdStockConversionProductionDetail.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdStockConversionProductionDetail.GetDataRows();
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
						DocDate = Conversion.ToDateTime((object)txtDocdate.Value),
						stockUOM = ItemUOMId,
						ItemId = ItemId,
						WarehouseId = WareHouseId,
						PackingTypeId = PackingTypeId,
						JobLotId = JobLotId,
						CropYear = CropYear,
						ItemQty = Qty,
						NetWeight = Weight
					};
					if (RecId > 0)
					{
						obj.DocumentTypeId = 80;
						obj.Id = RecId;
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
					rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)txtDocdate.Value), 80, RecId, JobLotId, 0, CropYear, WareHouseId) * 40.0;
					if (rate > 0.0)
					{
						r.Cells["Rate"].Value = rate;
						r.Cells["ItemAmount"].Value = Weight / RateUomEquivalent * rate;
					}
				}
				r.EndEdit();
			}
			grdStockConversionProductionDetail.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
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

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
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
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_107e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1088: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_7621: Unknown result type (might be due to invalid IL or missing references)
		//IL_762b: Expected O, but got Unknown
		GridEXLayout grdStockConversionProductionDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance130 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance203 = new Appearance();
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
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
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
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
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
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		GridEXLayout grdinputhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionInput));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdStockConversionProductionDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.PanelInPutManualEntry = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnAddInput = new System.Windows.Forms.Button();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.cmbWareHouseInput = new UltraCombo();
		this.label165 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.label166 = new System.Windows.Forms.Label();
		this.txtQtyInput = new System.Windows.Forms.TextBox();
		this.label167 = new System.Windows.Forms.Label();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtNetWeightInput = new System.Windows.Forms.TextBox();
		this.cmbItemUomInput = new UltraCombo();
		this.label81 = new System.Windows.Forms.Label();
		this.cmbLotInput = new UltraCombo();
		this.txtRemarksInput = new System.Windows.Forms.TextBox();
		this.cmbItemInput = new UltraCombo();
		this.txtAmountInput = new System.Windows.Forms.TextBox();
		this.cmbCropYearInput = new UltraCombo();
		this.lblAmountInput = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.cmbRateuomInput = new UltraCombo();
		this.label78 = new System.Windows.Forms.Label();
		this.lblRateUom = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.txtRateInput = new System.Windows.Forms.TextBox();
		this.label74 = new System.Windows.Forms.Label();
		this.lblRateInput = new System.Windows.Forms.Label();
		this.cmbPackingTypeInput = new UltraCombo();
		this.txtBalanceQty = new System.Windows.Forms.TextBox();
		this.btnCancelInput = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.btnUpdateDetailInput = new System.Windows.Forms.Button();
		this.label72 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.panel72 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbMoveOrderTicket = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.BtnLoadMoveOrder = new System.Windows.Forms.Button();
		this.txtMoveOrderBalWeight = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtMoveOrderAlreadUsedWeight = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtMoveOrderNetWeight = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbPlantFeeder = new UltraCombo();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.CmbJobOrderNo = new UltraCombo();
		this.CmbWipWarehouse = new UltraCombo();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.CmbWorkInProcess = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbProductionDepartment = new UltraCombo();
		this.label112 = new System.Windows.Forms.Label();
		this.CmbWIPItem = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.btnLoadInvoices = new System.Windows.Forms.Button();
		this.panel63 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.btnPendingForRateInput = new System.Windows.Forms.ToolStripButton();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdinputhistory = new GridEX();
		this.panel78 = new System.Windows.Forms.Panel();
		this.label130 = new System.Windows.Forms.Label();
		this.grdInputDetail = new GridEX();
		this.panel59 = new System.Windows.Forms.Panel();
		this.label127 = new System.Windows.Forms.Label();
		this.panel58 = new System.Windows.Forms.Panel();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.BtnShowInputHistory = new System.Windows.Forms.Button();
		this.label145 = new System.Windows.Forms.Label();
		this.CmbJobOrderInputHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label146 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label148 = new System.Windows.Forms.Label();
		this.label149 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel57 = new System.Windows.Forms.Panel();
		this.label124 = new System.Windows.Forms.Label();
		this.toolStrip7 = new System.Windows.Forms.ToolStrip();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.BtnResetMoveOrder = new System.Windows.Forms.PictureBox();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.BtnGenerateRates = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshInputHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar10 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.PanelInPutManualEntry.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateuomInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).BeginInit();
		this.panel72.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMoveOrderTicket).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeeder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).BeginInit();
		this.panel63.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).BeginInit();
		this.panel78.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).BeginInit();
		this.panel59.SuspendLayout();
		this.panel58.SuspendLayout();
		this.groupBox11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).BeginInit();
		this.panel57.SuspendLayout();
		this.toolStrip7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnResetMoveOrder).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 687);
		this.panel1.TabIndex = 0;
		this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1184, 687);
		this.tabControl2.TabIndex = 4;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel28);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 658);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel28.Controls.Add(this.panel5);
		this.panel28.Controls.Add(this.PanelInPutManualEntry);
		this.panel28.Controls.Add(this.panel72);
		this.panel28.Controls.Add(this.panel63);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(3, 30);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1170, 625);
		this.panel28.TabIndex = 174;
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 268);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 357);
		this.panel5.TabIndex = 3;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 28);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1170, 329);
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
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Size = new System.Drawing.Size(1170, 329);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).TabIndex = 0;
		this.grdStockConversionProductionDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockConversionProductionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).VisualStyle = (VisualStyle)4;
		this.grdStockConversionProductionDetail.CellUpdated += new ColumnActionEventHandler(grdStockConversionProductionDetail_CellUpdated);
		this.grdStockConversionProductionDetail.ColumnButtonClick += new ColumnActionEventHandler(grdStockConversionProductionDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).DoubleClick += new System.EventHandler(grdStockConversionProductionDetail_DoubleClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 28);
		this.panel12.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(107, 19);
		this.label2.TabIndex = 172;
		this.label2.Text = "Input Detail";
		this.PanelInPutManualEntry.Controls.Add(this.groupBox2);
		this.PanelInPutManualEntry.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelInPutManualEntry.Location = new System.Drawing.Point(0, 139);
		this.PanelInPutManualEntry.Name = "PanelInPutManualEntry";
		this.PanelInPutManualEntry.Size = new System.Drawing.Size(1170, 129);
		this.PanelInPutManualEntry.TabIndex = 4;
		this.groupBox2.Controls.Add(this.btnAddInput);
		this.groupBox2.Controls.Add(this.txtEbTotal);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseInput);
		this.groupBox2.Controls.Add(this.label165);
		this.groupBox2.Controls.Add(this.label82);
		this.groupBox2.Controls.Add(this.txtEbUnit);
		this.groupBox2.Controls.Add(this.label64);
		this.groupBox2.Controls.Add(this.label166);
		this.groupBox2.Controls.Add(this.txtQtyInput);
		this.groupBox2.Controls.Add(this.label167);
		this.groupBox2.Controls.Add(this.txtBalanceWeight);
		this.groupBox2.Controls.Add(this.txtGrossWeight);
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
		this.groupBox2.Controls.Add(this.txtBalanceQty);
		this.groupBox2.Controls.Add(this.btnCancelInput);
		this.groupBox2.Controls.Add(this.label66);
		this.groupBox2.Controls.Add(this.btnUpdateDetailInput);
		this.groupBox2.Controls.Add(this.label72);
		this.groupBox2.Controls.Add(this.label80);
		this.groupBox2.Controls.Add(this.label67);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1162, 124);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.btnAddInput.BackColor = System.Drawing.Color.Teal;
		this.btnAddInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddInput.ForeColor = System.Drawing.Color.White;
		this.btnAddInput.Location = new System.Drawing.Point(1022, 91);
		this.btnAddInput.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddInput.Name = "btnAddInput";
		this.btnAddInput.Size = new System.Drawing.Size(41, 27);
		this.btnAddInput.TabIndex = 14;
		this.btnAddInput.Text = "+";
		this.btnAddInput.UseVisualStyleBackColor = false;
		this.btnAddInput.Click += new System.EventHandler(btnAddInput_Click);
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(64, 93);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.Size = new System.Drawing.Size(70, 23);
		this.txtEbTotal.TabIndex = 168;
		this.txtEbTotal.Tag = "EbTotal";
		this.txtEbTotal.Text = "0";
		this.txtEbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotal.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotal_KeyPress);
		this.cmbWareHouseInput.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance203).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouseInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Location = new System.Drawing.Point(3, 40);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Name = "cmbWareHouseInput";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).TabIndex = 0;
		this.cmbWareHouseInput.ValueChanged += new System.EventHandler(cmbWareHouseInput_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseInput).Leave += new System.EventHandler(cmbWareHouseInput_Leave);
		this.label165.AutoSize = true;
		this.label165.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label165.Location = new System.Drawing.Point(64, 73);
		this.label165.Name = "label165";
		this.label165.Size = new System.Drawing.Size(46, 15);
		this.label165.TabIndex = 171;
		this.label165.Text = "EbTotal";
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label82.Location = new System.Drawing.Point(898, 22);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(90, 15);
		this.label82.TabIndex = 199;
		this.label82.Text = "Balance Weight";
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(3, 93);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(60, 23);
		this.txtEbUnit.TabIndex = 167;
		this.txtEbUnit.Tag = "EbUnit";
		this.txtEbUnit.Text = "0";
		this.txtEbUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnit_KeyPress);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(990, 22);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(53, 15);
		this.label64.TabIndex = 206;
		this.label64.Text = "Quantity";
		this.label166.AutoSize = true;
		this.label166.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label166.Location = new System.Drawing.Point(3, 73);
		this.label166.Name = "label166";
		this.label166.Size = new System.Drawing.Size(42, 15);
		this.label166.TabIndex = 170;
		this.label166.Text = "EbUnit";
		this.txtQtyInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyInput.Location = new System.Drawing.Point(990, 42);
		this.txtQtyInput.Name = "txtQtyInput";
		this.txtQtyInput.Size = new System.Drawing.Size(75, 23);
		this.txtQtyInput.TabIndex = 8;
		this.txtQtyInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyInput.TextChanged += new System.EventHandler(txtQtyInput_TextChanged);
		this.txtQtyInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInput_KeyPress);
		this.txtQtyInput.Leave += new System.EventHandler(txtQtyInput_Leave);
		this.label167.AutoSize = true;
		this.label167.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label167.Location = new System.Drawing.Point(135, 73);
		this.label167.Name = "label167";
		this.label167.Size = new System.Drawing.Size(78, 15);
		this.label167.TabIndex = 169;
		this.label167.Text = "Gross Weight";
		this.txtBalanceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeight.Location = new System.Drawing.Point(898, 42);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.ReadOnly = true;
		this.txtBalanceWeight.Size = new System.Drawing.Size(90, 23);
		this.txtBalanceWeight.TabIndex = 7;
		this.txtBalanceWeight.TabStop = false;
		this.txtBalanceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceWeight_KeyPress);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(135, 93);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(90, 23);
		this.txtGrossWeight.TabIndex = 166;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightInput.Location = new System.Drawing.Point(1066, 42);
		this.txtNetWeightInput.Name = "txtNetWeightInput";
		this.txtNetWeightInput.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeightInput.TabIndex = 9;
		this.txtNetWeightInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightInput.TextChanged += new System.EventHandler(txtNetWeightInput_TextChanged);
		this.txtNetWeightInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightInput_KeyPress);
		this.txtNetWeightInput.Leave += new System.EventHandler(txtNetWeightInput_Leave);
		this.cmbItemUomInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUomInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUomInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUomInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance213;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance218).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUomInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUomInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Location = new System.Drawing.Point(387, 40);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Name = "cmbItemUomInput";
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemUomInput).TextChanged += new System.EventHandler(cmbItemUomInput_TextChanged);
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label81.Location = new System.Drawing.Point(1066, 22);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(68, 15);
		this.label81.TabIndex = 207;
		this.label81.Text = "Net Weight";
		this.cmbLotInput.AllowNull = (DefaultableBoolean)1;
		this.cmbLotInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLotInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLotInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance225;
		((SpecialBoxBase)((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance230).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance231).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance232).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLotInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLotInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Location = new System.Drawing.Point(569, 40);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Name = "cmbLotInput";
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.cmbLotInput).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbLotInput).Leave += new System.EventHandler(cmbLotInput_Leave);
		this.txtRemarksInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksInput.Location = new System.Drawing.Point(226, 93);
		this.txtRemarksInput.Name = "txtRemarksInput";
		this.txtRemarksInput.Size = new System.Drawing.Size(527, 23);
		this.txtRemarksInput.TabIndex = 10;
		this.cmbItemInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance237;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance242).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(185, 40);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TextChanged += new System.EventHandler(cmbItemInput_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Leave += new System.EventHandler(cmbItemInput_Leave);
		this.txtAmountInput.BackColor = System.Drawing.Color.White;
		this.txtAmountInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountInput.Location = new System.Drawing.Point(919, 93);
		this.txtAmountInput.Name = "txtAmountInput";
		this.txtAmountInput.ReadOnly = true;
		this.txtAmountInput.Size = new System.Drawing.Size(100, 23);
		this.txtAmountInput.TabIndex = 13;
		this.txtAmountInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountInput_KeyPress_1);
		this.cmbCropYearInput.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance249;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance254).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance255).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance256).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYearInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Location = new System.Drawing.Point(468, 40);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Name = "cmbCropYearInput";
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbCropYearInput).Leave += new System.EventHandler(cmbCropYearInput_Leave);
		this.lblAmountInput.AutoSize = true;
		this.lblAmountInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAmountInput.Location = new System.Drawing.Point(920, 73);
		this.lblAmountInput.Name = "lblAmountInput";
		this.lblAmountInput.Size = new System.Drawing.Size(51, 15);
		this.lblAmountInput.TabIndex = 203;
		this.lblAmountInput.Text = "Amount";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(468, 22);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(57, 15);
		this.label61.TabIndex = 194;
		this.label61.Text = "Crop Year";
		this.cmbRateuomInput.AllowNull = (DefaultableBoolean)1;
		this.cmbRateuomInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateuomInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateuomInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance261;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance262).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance262).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance266).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateuomInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRateuomInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Location = new System.Drawing.Point(837, 91);
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Name = "cmbRateuomInput";
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbRateuomInput).TextChanged += new System.EventHandler(cmbRateuomInput_TextChanged);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label78.Location = new System.Drawing.Point(387, 22);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(63, 15);
		this.label78.TabIndex = 192;
		this.label78.Text = "Pack UOM";
		this.lblRateUom.AutoSize = true;
		this.lblRateUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateUom.Location = new System.Drawing.Point(837, 73);
		this.lblRateUom.Name = "lblRateUom";
		this.lblRateUom.Size = new System.Drawing.Size(61, 15);
		this.lblRateUom.TabIndex = 202;
		this.lblRateUom.Text = "Rate UOM";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(569, 22);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(46, 15);
		this.label65.TabIndex = 195;
		this.label65.Text = "Job Lot";
		this.txtRateInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateInput.Location = new System.Drawing.Point(755, 93);
		this.txtRateInput.Name = "txtRateInput";
		this.txtRateInput.Size = new System.Drawing.Size(80, 23);
		this.txtRateInput.TabIndex = 11;
		this.txtRateInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateInput.TextChanged += new System.EventHandler(txtRateInput_TextChanged);
		this.txtRateInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateInput_KeyPress_1);
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label74.Location = new System.Drawing.Point(185, 22);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(67, 15);
		this.label74.TabIndex = 191;
		this.label74.Text = "Item Name";
		this.lblRateInput.AutoSize = true;
		this.lblRateInput.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblRateInput.Location = new System.Drawing.Point(755, 73);
		this.lblRateInput.Name = "lblRateInput";
		this.lblRateInput.Size = new System.Drawing.Size(90, 15);
		this.lblRateInput.TabIndex = 201;
		this.lblRateInput.Text = "Avg Rate(40Kg)";
		this.cmbPackingTypeInput.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingTypeInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingTypeInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingTypeInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance278).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingTypeInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingTypeInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Location = new System.Drawing.Point(726, 40);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Name = "cmbPackingTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackingTypeInput).Leave += new System.EventHandler(cmbPackingTypeInput_Leave);
		this.txtBalanceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQty.Location = new System.Drawing.Point(828, 42);
		this.txtBalanceQty.Name = "txtBalanceQty";
		this.txtBalanceQty.ReadOnly = true;
		this.txtBalanceQty.Size = new System.Drawing.Size(69, 23);
		this.txtBalanceQty.TabIndex = 6;
		this.txtBalanceQty.TabStop = false;
		this.txtBalanceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.btnCancelInput.BackColor = System.Drawing.Color.Teal;
		this.btnCancelInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelInput.ForeColor = System.Drawing.Color.White;
		this.btnCancelInput.Location = new System.Drawing.Point(1092, 91);
		this.btnCancelInput.Name = "btnCancelInput";
		this.btnCancelInput.Size = new System.Drawing.Size(67, 27);
		this.btnCancelInput.TabIndex = 16;
		this.btnCancelInput.Text = "Cancel";
		this.btnCancelInput.UseVisualStyleBackColor = false;
		this.btnCancelInput.Click += new System.EventHandler(btnCancelInput_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(726, 22);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(60, 15);
		this.label66.TabIndex = 196;
		this.label66.Text = "Pack Type";
		this.btnUpdateDetailInput.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailInput.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailInput.Location = new System.Drawing.Point(1022, 91);
		this.btnUpdateDetailInput.Name = "btnUpdateDetailInput";
		this.btnUpdateDetailInput.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetailInput.TabIndex = 15;
		this.btnUpdateDetailInput.Text = "Update ";
		this.btnUpdateDetailInput.UseVisualStyleBackColor = false;
		this.btnUpdateDetailInput.Click += new System.EventHandler(btnUpdateDetailInput_Click);
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(828, 22);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(70, 15);
		this.label72.TabIndex = 197;
		this.label72.Text = "Balance Qty";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label80.Location = new System.Drawing.Point(226, 75);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(52, 15);
		this.label80.TabIndex = 200;
		this.label80.Text = "Remarks";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(3, 22);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(69, 15);
		this.label67.TabIndex = 193;
		this.label67.Text = "WareHouse";
		this.panel72.Controls.Add(this.groupBox3);
		this.panel72.Controls.Add(this.groupBox1);
		this.panel72.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel72.Location = new System.Drawing.Point(0, 29);
		this.panel72.Name = "panel72";
		this.panel72.Size = new System.Drawing.Size(1170, 110);
		this.panel72.TabIndex = 3;
		this.groupBox3.Controls.Add(this.BtnResetMoveOrder);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.BtnLoadMoveOrder);
		this.groupBox3.Controls.Add(this.txtMoveOrderBalWeight);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtMoveOrderAlreadUsedWeight);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.txtMoveOrderNetWeight);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(861, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(304, 104);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Link Any Move Order Here";
		this.CmbMoveOrderTicket.AllowNull = (DefaultableBoolean)1;
		this.CmbMoveOrderTicket.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMoveOrderTicket.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMoveOrderTicket.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMoveOrderTicket).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMoveOrderTicket.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Location = new System.Drawing.Point(3, 34);
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Name = "CmbMoveOrderTicket";
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.CmbMoveOrderTicket).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 170;
		this.label3.Text = "W.B Ticket No";
		this.BtnLoadMoveOrder.BackColor = System.Drawing.Color.Teal;
		this.BtnLoadMoveOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadMoveOrder.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadMoveOrder.ForeColor = System.Drawing.Color.White;
		this.BtnLoadMoveOrder.Location = new System.Drawing.Point(3, 60);
		this.BtnLoadMoveOrder.Name = "BtnLoadMoveOrder";
		this.BtnLoadMoveOrder.Size = new System.Drawing.Size(105, 39);
		this.BtnLoadMoveOrder.TabIndex = 1;
		this.BtnLoadMoveOrder.Text = "Load Move Order";
		this.BtnLoadMoveOrder.UseVisualStyleBackColor = false;
		this.BtnLoadMoveOrder.Click += new System.EventHandler(BtnLoadMoveOrder_Click);
		this.txtMoveOrderBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderBalWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderBalWeight.Location = new System.Drawing.Point(187, 74);
		this.txtMoveOrderBalWeight.Name = "txtMoveOrderBalWeight";
		this.txtMoveOrderBalWeight.ReadOnly = true;
		this.txtMoveOrderBalWeight.Size = new System.Drawing.Size(114, 26);
		this.txtMoveOrderBalWeight.TabIndex = 6;
		this.txtMoveOrderBalWeight.TabStop = false;
		this.txtMoveOrderBalWeight.Text = "0";
		this.txtMoveOrderBalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMoveOrderBalWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(113, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 15);
		this.label7.TabIndex = 197;
		this.label7.Text = "Bal Weight";
		this.txtMoveOrderAlreadUsedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderAlreadUsedWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderAlreadUsedWeight.Location = new System.Drawing.Point(187, 46);
		this.txtMoveOrderAlreadUsedWeight.Name = "txtMoveOrderAlreadUsedWeight";
		this.txtMoveOrderAlreadUsedWeight.ReadOnly = true;
		this.txtMoveOrderAlreadUsedWeight.Size = new System.Drawing.Size(114, 26);
		this.txtMoveOrderAlreadUsedWeight.TabIndex = 6;
		this.txtMoveOrderAlreadUsedWeight.TabStop = false;
		this.txtMoveOrderAlreadUsedWeight.Text = "0";
		this.txtMoveOrderAlreadUsedWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMoveOrderAlreadUsedWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(113, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 197;
		this.label5.Text = "Used Weight";
		this.txtMoveOrderNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoveOrderNetWeight.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoveOrderNetWeight.Location = new System.Drawing.Point(187, 18);
		this.txtMoveOrderNetWeight.Name = "txtMoveOrderNetWeight";
		this.txtMoveOrderNetWeight.ReadOnly = true;
		this.txtMoveOrderNetWeight.Size = new System.Drawing.Size(114, 26);
		this.txtMoveOrderNetWeight.TabIndex = 6;
		this.txtMoveOrderNetWeight.TabStop = false;
		this.txtMoveOrderNetWeight.Text = "0";
		this.txtMoveOrderNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMoveOrderNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceQty_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(113, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 15);
		this.label4.TabIndex = 197;
		this.label4.Text = "Net Weight";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeeder);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipWarehouse);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label83);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionDepartment);
		this.groupBox1.Controls.Add(this.label112);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPItem);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.btnLoadInvoices);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(857, 104);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbPlantFeeder.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeeder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeeder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeeder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeeder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeeder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Location = new System.Drawing.Point(81, 70);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Name = "CmbPlantFeeder";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeeder).TabIndex = 3;
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(81, 16);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(83, 23);
		this.txtdocnumber.TabIndex = 0;
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(166, 16);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(125, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.ValueChanged += new System.EventHandler(txtDocdate_ValueChanged);
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(81, 42);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.CmbWipWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWipWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Location = new System.Drawing.Point(616, 14);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Name = "CmbWipWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).TabIndex = 7;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(356, 72);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(440, 23);
		this.txtRemarks.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Transparent;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(295, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 6;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(547, 11);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(67, 30);
		this.label35.TabIndex = 170;
		this.label35.Text = "Wip\r\nWarehouse";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Doc No,Date";
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(4, 76);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(69, 15);
		this.label83.TabIndex = 209;
		this.label83.Text = "Plant Name";
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance164).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(356, 14);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabStop = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(295, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 30);
		this.label27.TabIndex = 171;
		this.label27.Text = "WIP\r\nAccount";
		this.CmbProductionDepartment.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance169;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance175).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Location = new System.Drawing.Point(616, 42);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Name = "CmbProductionDepartment";
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionDepartment).TabIndex = 8;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(547, 42);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(71, 30);
		this.label112.TabIndex = 173;
		this.label112.Text = "Production\r\nDepartment";
		this.CmbWIPItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance182;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Location = new System.Drawing.Point(356, 42);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Name = "CmbWIPItem";
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabStop = false;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(295, 48);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(58, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "WIP Item";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 48);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 151;
		this.label23.Text = "Job Order No";
		this.btnLoadInvoices.BackColor = System.Drawing.Color.Teal;
		this.btnLoadInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadInvoices.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadInvoices.ForeColor = System.Drawing.Color.White;
		this.btnLoadInvoices.Location = new System.Drawing.Point(798, 14);
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(59, 54);
		this.btnLoadInvoices.TabIndex = 9;
		this.btnLoadInvoices.Text = "Load InPut";
		this.btnLoadInvoices.UseVisualStyleBackColor = false;
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.panel63.BackColor = System.Drawing.Color.Teal;
		this.panel63.Controls.Add(this.label30);
		this.panel63.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel63.Location = new System.Drawing.Point(0, 0);
		this.panel63.Name = "panel63";
		this.panel63.Size = new System.Drawing.Size(1170, 29);
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
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnVoucherReport, this.btnJobOrderDefine, this.BtnGenerateRates, this.btnPendingForRateInput });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
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
		this.tabPage4.Size = new System.Drawing.Size(1176, 658);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "History";
		this.tabPage4.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdinputhistory).Size = new System.Drawing.Size(1170, 313);
		((System.Windows.Forms.Control)(object)this.grdinputhistory).TabIndex = 1;
		this.grdinputhistory.TotalRow = (InheritableBoolean)1;
		this.grdinputhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdinputhistory).VisualStyle = (VisualStyle)4;
		this.grdinputhistory.ColumnButtonClick += new ColumnActionEventHandler(grdinputhistory_ColumnButtonClick);
		this.grdinputhistory.SelectionChanged += new System.EventHandler(grdinputhistory_SelectionChanged);
		this.panel78.BackColor = System.Drawing.Color.Teal;
		this.panel78.Controls.Add(this.label130);
		this.panel78.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel78.Location = new System.Drawing.Point(3, 442);
		this.panel78.Name = "panel78";
		this.panel78.Size = new System.Drawing.Size(1170, 20);
		this.panel78.TabIndex = 11;
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label130.ForeColor = System.Drawing.Color.White;
		this.label130.Location = new System.Drawing.Point(3, 1);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(230, 17);
		this.label130.TabIndex = 211;
		this.label130.Text = "Detail of the Selected above row";
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
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Location = new System.Drawing.Point(3, 462);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Name = "grdInputDetail";
		((ScrollableBase)this.grdInputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1170, 193);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 2;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		this.panel59.BackColor = System.Drawing.Color.Teal;
		this.panel59.Controls.Add(this.label127);
		this.panel59.Controls.Add(this.ctrlGrdBar10);
		this.panel59.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel59.Location = new System.Drawing.Point(3, 105);
		this.panel59.Name = "panel59";
		this.panel59.Size = new System.Drawing.Size(1170, 24);
		this.panel59.TabIndex = 7;
		this.label127.AutoSize = true;
		this.label127.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label127.ForeColor = System.Drawing.Color.White;
		this.label127.Location = new System.Drawing.Point(3, 3);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(40, 17);
		this.label127.TabIndex = 211;
		this.label127.Text = "Main";
		this.panel58.Controls.Add(this.groupBox11);
		this.panel58.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel58.Location = new System.Drawing.Point(3, 55);
		this.panel58.Name = "panel58";
		this.panel58.Size = new System.Drawing.Size(1170, 50);
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
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(683, 20);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(59, 15);
		this.label145.TabIndex = 178;
		this.label145.Text = "Job Order";
		this.CmbJobOrderInputHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderInputHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderInputHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderInputHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance196;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderInputHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderInputHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Name = "CmbJobOrderInputHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderInputHistory).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(268, 16);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(148, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(217, 20);
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
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(609, 16);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(148, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(564, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(45, 15);
		this.label148.TabIndex = 173;
		this.label148.Text = "Doc To";
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(429, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(60, 15);
		this.label149.TabIndex = 173;
		this.label149.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(490, 16);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel57.BackColor = System.Drawing.Color.Teal;
		this.panel57.Controls.Add(this.label124);
		this.panel57.Controls.Add(this.toolStrip7);
		this.panel57.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel57.Location = new System.Drawing.Point(3, 3);
		this.panel57.Name = "panel57";
		this.panel57.Size = new System.Drawing.Size(1170, 52);
		this.panel57.TabIndex = 6;
		this.label124.AutoSize = true;
		this.label124.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label124.ForeColor = System.Drawing.Color.White;
		this.label124.Location = new System.Drawing.Point(2, 30);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(209, 19);
		this.label124.TabIndex = 211;
		this.label124.Text = "Production Input History";
		this.toolStrip7.BackColor = System.Drawing.Color.White;
		this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton4, this.BtnRefereshInputHistory });
		this.toolStrip7.Location = new System.Drawing.Point(0, 0);
		this.toolStrip7.Name = "toolStrip7";
		this.toolStrip7.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip7.TabIndex = 4;
		this.toolStrip7.Text = "toolStrip7";
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
		this.BtnResetMoveOrder.Image = Architecture.WinApp.Properties.Resources.reset__1_;
		this.BtnResetMoveOrder.Location = new System.Drawing.Point(88, 15);
		this.BtnResetMoveOrder.Name = "BtnResetMoveOrder";
		this.BtnResetMoveOrder.Size = new System.Drawing.Size(26, 18);
		this.BtnResetMoveOrder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.BtnResetMoveOrder.TabIndex = 210;
		this.BtnResetMoveOrder.TabStop = false;
		this.BtnResetMoveOrder.Click += new System.EventHandler(BtnResetMoveOrder_Click);
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
		this.BtnGenerateRates.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.BtnGenerateRates.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnGenerateRates.Name = "BtnGenerateRates";
		this.BtnGenerateRates.Size = new System.Drawing.Size(133, 24);
		this.BtnGenerateRates.Text = "Generate Rates";
		this.BtnGenerateRates.Click += new System.EventHandler(BtnGenerateRates_Click);
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
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1130, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdStockConversionProductionDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar10.FormName = this;
		this.ctrlGrdBar10.Location = new System.Drawing.Point(1123, 1);
		this.ctrlGrdBar10.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar10.MyGrid = this.grdinputhistory;
		this.ctrlGrdBar10.Name = "ctrlGrdBar10";
		this.ctrlGrdBar10.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar10.TabIndex = 174;
		this.ctrlGrdBar10.Load += new System.EventHandler(ctrlGrdBar10_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1184, 687);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionInput";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel28.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.PanelInPutManualEntry.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUomInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLotInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateuomInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingTypeInput).EndInit();
		this.panel72.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMoveOrderTicket).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeeder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).EndInit();
		this.panel63.ResumeLayout(false);
		this.panel63.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdinputhistory).EndInit();
		this.panel78.ResumeLayout(false);
		this.panel78.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).EndInit();
		this.panel59.ResumeLayout(false);
		this.panel59.PerformLayout();
		this.panel58.ResumeLayout(false);
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderInputHistory).EndInit();
		this.panel57.ResumeLayout(false);
		this.panel57.PerformLayout();
		this.toolStrip7.ResumeLayout(false);
		this.toolStrip7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnResetMoveOrder).EndInit();
		base.ResumeLayout(false);
	}
}

}
