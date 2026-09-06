// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
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

public class frmProductionPackingMaterial : Form
{
	private bool UpdateMode;

	private int VoucherHeadId;

	private int RecpackingMaterial;

	private int updateDetailIndexPM = -1;

	private double AvgRate;

	private double AvgRateWithoutExp;

	public static bool ValuesShowRights;

	private DataTable tablePackingMaterial = new DataTable();

	private DataTable dtScheduleData = new DataTable();

	private bool FIFOCGSFlag;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip miniToolStrip;

	public TabControl tabControlPm;

	private TabPage PmForm;

	private Panel panel66;

	internal GridEX grddetailPackingMaterial;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label100;

	private Panel panel42;

	private GroupBox groupBox8;

	private TextBox txtIdPMRec;

	private Button btnAddDetailPm;

	private Button btnCancelDetailPm;

	private Button btnUpdateDetailPm;

	internal UltraCombo CmbPlantFeederPackingMaterial;

	private Label label79;

	private TextBox txtremarksPackingMaterial;

	private Label label84;

	private Label lblRemarksPackingMaterial;

	internal UltraCombo CmbWarehousePackingMaterial;

	private TextBox txtPackingMaterialRate;

	internal UltraCombo cmbItemPackingMaterial;

	private Label label41;

	private Label label40;

	internal UltraCombo CmbBrandUomPackingMaterial;

	private Label lblRatePackingMaterial;

	internal UltraCombo CmbBrandNamePackingMaterial;

	private TextBox txtPackingMaterialQTY;

	private TextBox txtAmountPackingMaterial;

	private Label label4;

	private Label lblAmountPackingMaterial;

	private Label label33;

	private GroupBox groupBox5;

	private Label label104;

	private TextBox txtTotalFinishQtyPm;

	private Label label105;

	private TextBox txtTotalFinishWeightPm;

	private Label label106;

	private TextBox txtTotalInputQtyPm;

	private Label label107;

	private TextBox txtTotalInputWeightPm;

	private Label label108;

	private TextBox txtTotaloutputQtyPm;

	private Label label109;

	private TextBox txtTotalOutPutWeightPm;

	private DateTimePicker txtdocdatePackingMaterial;

	private TextBox txtDocNoPackingMaterial;

	private Label label37;

	private Label label35;

	internal UltraCombo cmbJobOrderPackingMaterial;

	private Panel panel41;

	private Label label96;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewpackingmaterial;

	private ToolStripButton btnRefreshPackingMaterial;

	private ToolStripButton btnsavepackingmaterial;

	private ToolStripButton btnUpdatepackingmaterial;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton btnPackingMaterialSlip;

	private ToolStripButton btnVoucherPM;

	private TabPage PmHistory;

	private Panel panel70;

	internal GridEX grddetailHistoryPM;

	private Panel panel73;

	private Label label111;

	private Label label110;

	private CtrlGrdBar ctrlGrdBar15;

	internal GridEX grdPackingMaterialHistory;

	private Panel panel71;

	private CheckBox chkPendingForRates;

	private Label label88;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnGenerateRate;

	private Label label1;

	internal UltraCombo cmbItemCondition;

	internal UltraCombo CmbInvoiceNo;

	private Label labelEXInv;

	internal UltraCombo CmbRackName;

	private Label label13;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel15;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	internal UltraCombo CmbJobOrder;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	public frmProductionPackingMaterial(UserAccount user)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
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
			if (clsGlobalVariables.ScreenViewReights.Any((tblUserRights dd) => dd.ScreenName == "FoodProductionWithValues"))
			{
				formright = CommonServices.SetRightsValueInRightsObject("FoodProductionWithValues");
			}
			else
			{
				formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			}
			btnsavepackingmaterial.Enabled = formright.DoHaveSaveRight;
			btnUpdatepackingmaterial.Enabled = formright.DoHaveUpdateRights;
			btnVoucherPM.Enabled = formright.DoHavePrintRights;
			btnPackingMaterialSlip.Enabled = formright.DoHavePrintRights;
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			tablePackingMaterial.Columns.Add("Id", typeof(int));
			tablePackingMaterial.Columns.Add("DocDate", typeof(DateTime));
			tablePackingMaterial.Columns.Add("DocNo", typeof(int));
			tablePackingMaterial.Columns.Add("InvProductionJobOrderId", typeof(int));
			tablePackingMaterial.Columns.Add("PlantId", typeof(int));
			tablePackingMaterial.Columns.Add("Plant", typeof(string));
			tablePackingMaterial.Columns.Add("PackingItemId", typeof(int));
			tablePackingMaterial.Columns.Add("PackingItem", typeof(string));
			tablePackingMaterial.Columns.Add("WarehouseId", typeof(int));
			tablePackingMaterial.Columns.Add("Warehouse", typeof(string));
			tablePackingMaterial.Columns.Add("RackId", typeof(int));
			tablePackingMaterial.Columns.Add("RackName", typeof(string));
			tablePackingMaterial.Columns.Add("ItemConditionId", typeof(int));
			tablePackingMaterial.Columns.Add("ItemCondition", typeof(string));
			tablePackingMaterial.Columns.Add("BrandId", typeof(int));
			tablePackingMaterial.Columns.Add("Brand", typeof(string));
			tablePackingMaterial.Columns.Add("BrandUomId", typeof(int));
			tablePackingMaterial.Columns.Add("BrandUom", typeof(string));
			tablePackingMaterial.Columns.Add("ItemQty", typeof(double));
			tablePackingMaterial.Columns.Add("ItemRate", typeof(double));
			tablePackingMaterial.Columns.Add("Amount", typeof(double));
			tablePackingMaterial.Columns.Add("Remarks", typeof(string));
			tablePackingMaterial.Columns.Add("ContractScheduleId", typeof(int));
			tablePackingMaterial.Columns.Add("ContractScheduleNo", typeof(string));
			grddetailPackingMaterial.DataSource = tablePackingMaterial;
			grddetailPackingMaterial.RetrieveStructure();
			grdDetailPackingMaterialSettings();
			GenerateCodeOfPackingMaterial();
			JobOrderNoFill();
			ItemFillPackingMaterial();
			ItemConditionBindFromGlobal();
			ScheduleNoDbCall();
			ScheduleNoBind();
			if (!formright.DoHaveCanRateandAmount)
			{
				txtremarksPackingMaterial.Width = 607;
				txtremarksPackingMaterial.Location = new Point(191, txtremarksPackingMaterial.Location.Y);
				lblRemarksPackingMaterial.Location = new Point(191, lblRemarksPackingMaterial.Location.Y);
				lblRemarksPackingMaterial.Visible = true;
				txtPackingMaterialRate.Visible = false;
				txtAmountPackingMaterial.Visible = false;
				lblRatePackingMaterial.Visible = false;
				lblAmountPackingMaterial.Visible = false;
				btnVoucherPM.Visible = false;
			}
			txtdocdatePackingMaterial.Focus();
			HistoryComboBind(HistoryComboDBCall());
			btnUpdateDetailPm.Visible = false;
			btnCancelDetailPm.Visible = false;
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
			int JobPMId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
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
					DDL.BindDDL(dt, cmbJobOrderPackingMaterial, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)cmbJobOrderPackingMaterial).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (JobPMId > 0)
				{
					cmbJobOrderPackingMaterial.Value = JobPMId;
				}
			}
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
			int JobPMId = Conversion.ToInt(CmbPlantFeederPackingMaterial.Value);
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
				if (((UltraGridBase)cmbJobOrderPackingMaterial).ActiveRow != null)
				{
					DDL.BindDDLNew(dt, CmbPlantFeederPackingMaterial, "PlantId", "PlantName", "Plant Name", false);
				}
				if (JobPMId > 0)
				{
					CmbPlantFeederPackingMaterial.Value = JobPMId;
				}
			}
			else
			{
				((Control)(object)CmbPlantFeederPackingMaterial).Text = string.Empty;
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
			if (tabControlPm.SelectedIndex == 0)
			{
				if (btnsavepackingmaterial.Visible && btnsavepackingmaterial.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnsavepackingmaterial_Click(null, null);
				}
				if (btnUpdatepackingmaterial.Visible && btnUpdatepackingmaterial.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdatepackingmaterial_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewpackingmaterial_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshPackingMaterial_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOfPackingMaterial()
	{
		if (txtDocNoPackingMaterial.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Doc No. Field Required");
			txtDocNoPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobOrderPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Job Order No. Field Required");
			cmbJobOrderPackingMaterial.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortionPackingMaterial()
	{
		if (((UltraGridBase)cmbJobOrderPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("job Order Field Required");
			cmbJobOrderPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeederPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Plant Field Required");
			CmbPlantFeederPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Packing Material Item Field Required");
			cmbItemPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehousePackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehousePackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackName).ActiveRow == null)
		{
			MessageBox.Show("Rack Name Field Required");
			CmbRackName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemCondition).ActiveRow != null && Conversion.ToInt(cmbItemCondition.Value) <= 0)
		{
			MessageBox.Show("Item Condition Field Required");
			cmbItemCondition.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNamePackingMaterial).ActiveRow != null && Conversion.ToInt(CmbBrandUomPackingMaterial.Value) <= 0)
		{
			MessageBox.Show("ItemUOM Field Required");
			CmbBrandUomPackingMaterial.Focus();
			return false;
		}
		if ((txtPackingMaterialRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim()) == 0.0) && formright.DoHaveCanRateandAmount)
		{
			MessageBox.Show("Rate Field Required");
			txtPackingMaterialRate.Focus();
			return false;
		}
		if ((txtAmountPackingMaterial.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim()) == 0.0) && formright.DoHaveCanRateandAmount)
		{
			MessageBox.Show("Amount Field Required");
			txtAmountPackingMaterial.Focus();
			return false;
		}
		return true;
	}

	private void BrandUomForPackMaterialAgainstPmItemAndBrandItem()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
			obj.ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
			obj.BrandId = Conversion.ToInt(CmbBrandNamePackingMaterial.Value);
			obj.Id = Conversion.ToInt((object)txtIdPMRec.Text);
			DataTable lst = InvFoodProduction.GetBrandItemsAndUomForPackMaterialAgainstPmItem(obj);
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemUomId", typeof(int));
				dt.Columns.Add("PackUom");
				dt.Clear();
				int i;
				for (i = 0; i < lst.Rows.Count; i++)
				{
					bool num = dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemUomId") == Conversion.ToInt(lst.Rows[i]["ItemUomId"]));
					Conversion.ToDouble(lst.Rows[i]["PmQty"]);
					Conversion.ToDouble(lst.Rows[i]["BalQty"]);
					double ItemId = Conversion.ToInt(lst.Rows[i]["ItemId"]);
					if (!num && ItemId == (double)Conversion.ToInt(CmbBrandNamePackingMaterial.Value))
					{
						dt.Rows.Add(lst.Rows[i]["ItemUomId"], lst.Rows[i]["PackUom"]);
					}
				}
				DDL.BindDDL(dt, CmbBrandUomPackingMaterial, "ItemUomId", "PackUom", "PackUom", ZeroIndex: false);
				if (dt.Rows.Count == 1)
				{
					CmbBrandUomPackingMaterial.Value = Conversion.ToInt(dt.Rows[0]["ItemUomId"]);
					CmbBrandUomPackingMaterial_Leave(null, null);
				}
			}
			else
			{
				((Control)(object)CmbBrandUomPackingMaterial).Text = string.Empty;
				((UltraGridBase)CmbBrandUomPackingMaterial).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrandItemsForPackMaterialAgainstPmItem()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
			obj.ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
			obj.Id = Conversion.ToInt((object)txtIdPMRec.Text);
			DataTable lst = InvFoodProduction.GetBrandItemsAndUomForPackMaterialAgainstPmItem(obj);
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName");
				dt.Clear();
				int i;
				for (i = 0; i < lst.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == Conversion.ToInt(lst.Rows[i]["ItemId"])))
					{
						dt.Rows.Add(lst.Rows[i]["ItemId"], lst.Rows[i]["ItemName"]);
					}
				}
				DDL.BindDDL(dt, CmbBrandNamePackingMaterial, "ItemId", "ItemName", "ItemName", ZeroIndex: false);
			}
			else
			{
				((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
				((UltraGridBase)CmbBrandNamePackingMaterial).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemConditionBindFromGlobal()
	{
		try
		{
			List<GlobalItemCondition> list = clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList();
			int Id = Conversion.ToInt(cmbItemCondition.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(list, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(cmbItemCondition, dt, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCodeOfPackingMaterial()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionPackingMaterial.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoPackingMaterial.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFillPackingMaterial()
	{
		try
		{
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			foreach (getGlobalAllItems row in clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId == 14))
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode);
			}
			InfragisticsHelper.BindAndRetainSelection(cmbItemPackingMaterial, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Conversion.ToInt(cmbItemPackingMaterial.Value), insertDefaultRow: true);
			cmbItemPackingMaterial_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindWarehouseDropdown(UltraCombo combo, int itemId)
	{
		List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
			where x.ItemId == itemId
			group x by x.WarehouseId into g
			select g.First()).ToList();
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("Warehouse", typeof(string));
		dt.BeginLoadData();
		foreach (RacksWithWarehouseAndItem w in list)
		{
			dt.Rows.Add(w.WarehouseId, w.WareHouseName);
		}
		dt.EndLoadData();
		InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Warehouse", "Warehouse", AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
		if (Conversion.ToInt(combo.Value) != 0)
		{
			return;
		}
		if (dt.Rows.Count == 1)
		{
			combo.Value = dt.Rows[0]["Id"];
		}
		else if (dt.Rows.Count > 1)
		{
			int configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
			if (configWarehouseId != 0 && dt.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["Id"]) == configWarehouseId))
			{
				combo.Value = configWarehouseId;
			}
		}
	}

	private void RackBindFromGlobalRacksByItemId(int ItemId, int WareHouseId = 0)
	{
		try
		{
			List<RacksWithWarehouseAndItem> list = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
				where rr.ItemId == ItemId && (WareHouseId == 0 || rr.WarehouseId == WareHouseId)
				select rr into row
				group row by row.Id into g
				select g.First()).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("WarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Columns.Add("BaseRackId", typeof(int));
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem item in list)
			{
				dt.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName, item.BaseRackId);
			}
			dt.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbRackName, dt, "Id", "RackName", "Rack Name", AllColumns: true, Conversion.ToInt(CmbRackName.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbRackName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			if (Conversion.ToInt(CmbRackName.Value) != 0)
			{
				return;
			}
			if (dt.Rows.Count == 1)
			{
				CmbRackName.Value = dt.Rows[0]["Id"];
				CmbRackName_Leave(null, null);
			}
			else if (dt.Rows.Count > 1)
			{
				DataRow baseRackRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["BaseRackId"]) > 0);
				if (baseRackRow != null)
				{
					CmbRackName.Value = baseRackRow["Id"];
					CmbRackName_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading Rack: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbRackName_Leave(object sender, EventArgs e)
	{
		try
		{
			int rackId = Conversion.ToInt(CmbRackName.Value);
			int currentWarehouseId = Conversion.ToInt(CmbWarehousePackingMaterial.Value);
			RacksWithWarehouseAndItem rack = clsGlobalVariables.racksWithWarehouseAndItems.FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null && (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId))
			{
				CmbWarehousePackingMaterial.Value = rack.WarehouseId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbJobOrderPackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbJobOrderPackingMaterial.Value) > 0)
			{
				GetPlantFeeder(Conversion.ToInt(cmbJobOrderPackingMaterial.Value));
				GetTotalInPutOutOnPM();
				BrandItemsForPackMaterialAgainstPmItem();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotalInPutOutOnPM()
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
				JobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value),
				PlantId = Conversion.ToInt(CmbPlantFeederPackingMaterial.Value)
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
					if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "FinishGoods")
					{
						FinsishOutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						FinishOutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Input")
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
				txtTotalInputQtyPm.Text = InQty.ToString("#,#");
				txtTotalInputWeightPm.Text = InWeight.ToString("#,#");
				txtTotaloutputQtyPm.Text = OutQty.ToString("#,#");
				txtTotalOutPutWeightPm.Text = OutWeight.ToString("#,#");
				txtTotalFinishQtyPm.Text = FinsishOutQty.ToString("#,#");
				txtTotalFinishWeightPm.Text = FinishOutWeight.ToString("#,#");
			}
			else
			{
				txtTotalInputQtyPm.Text = "0";
				txtTotalInputWeightPm.Text = "0";
				txtTotaloutputQtyPm.Text = "0";
				txtTotalOutPutWeightPm.Text = "0";
				txtTotalFinishQtyPm.Text = "0";
				txtTotalFinishWeightPm.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandNamePackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			BrandUomForPackMaterialAgainstPmItemAndBrandItem();
			if (((UltraGridBase)CmbBrandUomPackingMaterial).ActiveRow != null)
			{
				GetOutPutBalQtyForPMQty();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetOutPutBalQtyForPMQty()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			if (!btnsavepackingmaterial.Visible || !btnsavepackingmaterial.Enabled)
			{
				return;
			}
			ReportsParameters OBJ = new ReportsParameters();
			OBJ.OrganizationId = UserAccount.OrganizationId;
			OBJ.CompanyId = UserAccount.CompanyId;
			OBJ.JobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
			OBJ.ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
			DataTable lst = InvFoodProduction.GetBrandItemsAndUomForPackMaterialAgainstPmItem(OBJ);
			if (lst.Rows.Count <= 0 || ((UltraGridBase)CmbBrandUomPackingMaterial).ActiveRow == null)
			{
				return;
			}
			txtPackingMaterialQTY.Text = "0";
			DataTable dt = new DataTable();
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemUomId", typeof(int));
			dt.Columns.Add("BalQty", typeof(double));
			dt.Clear();
			int i;
			for (i = 0; i < lst.Rows.Count; i++)
			{
				bool ItemExists = dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == Conversion.ToInt(lst.Rows[i]["ItemId"]));
				bool UomExists = dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemUomId") == Conversion.ToInt(lst.Rows[i]["ItemUomId"]));
				double balQty = Conversion.ToDouble(lst.Rows[i]["BalQty"]);
				double PmQty = Conversion.ToDouble(lst.Rows[i]["PmQty"]);
				if (!ItemExists && !UomExists && balQty > PmQty && Conversion.ToInt(CmbBrandUomPackingMaterial.Value) == Conversion.ToInt(lst.Rows[i]["ItemUomId"]))
				{
					dt.Rows.Add(lst.Rows[i]["ItemId"], lst.Rows[i]["ItemUomId"], lst.Rows[i]["BalQty"]);
				}
			}
			double OutQty = Conversion.ToDouble(dt.Rows[0]["BalQty"]);
			txtPackingMaterialQTY.Text = OutQty.ToString();
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
			obj.RecId = RecpackingMaterial;
			dtScheduleData = ExportContractSchedule.ExportContractSchedule_GetPending(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ScheduleNoBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNo, dtScheduleData, "Id", "ScheduleCode", "Schedule / Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNo.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtScheduleData;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)CmbInvoiceNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbInvoiceNo).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshPackingMaterial_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AllItems", "RacksWithWarehouseAndItem", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			JobOrderNoFill();
			ItemFillPackingMaterial();
			ItemConditionBindFromGlobal();
			ScheduleNoDbCall();
			ScheduleNoBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormResetOfPackingMaterial()
	{
		try
		{
			RecpackingMaterial = 0;
			VoucherHeadId = 0;
			btnsavepackingmaterial.Visible = true;
			btnUpdatepackingmaterial.Visible = false;
			((Control)(object)cmbJobOrderPackingMaterial).Visible = true;
			((Control)(object)cmbJobOrderPackingMaterial).Enabled = true;
			UpdateMode = false;
			GenerateCodeOfPackingMaterial();
			((Control)(object)CmbPlantFeederPackingMaterial).Text = string.Empty;
			((Control)(object)cmbItemPackingMaterial).Text = string.Empty;
			((Control)(object)CmbWarehousePackingMaterial).Text = string.Empty;
			((UltraGridBase)CmbWarehousePackingMaterial).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
			((Control)(object)CmbBrandUomPackingMaterial).Text = string.Empty;
			txtPackingMaterialQTY.Text = string.Empty;
			txtPackingMaterialRate.Text = string.Empty;
			txtAmountPackingMaterial.Text = string.Empty;
			txtremarksPackingMaterial.Text = string.Empty;
			((Control)(object)cmbJobOrderPackingMaterial).Text = string.Empty;
			btnAddDetailPm.Visible = true;
			btnUpdateDetailPm.Visible = false;
			btnCancelDetailPm.Visible = false;
			tablePackingMaterial.Clear();
			grddetailPackingMaterial.ClearStructure();
			txtTotalFinishQtyPm.Text = "";
			txtTotalFinishWeightPm.Text = "";
			txtTotaloutputQtyPm.Text = "";
			txtTotalOutPutWeightPm.Text = "";
			txtTotalInputQtyPm.Text = "";
			txtTotalInputWeightPm.Text = "";
			txtdocdatePackingMaterial.Value = DateTime.Now;
			cmbJobOrderPackingMaterial.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewpackingmaterial_Click(object sender, EventArgs e)
	{
		FormResetOfPackingMaterial();
	}

	private void ResetdetailPackingMaterial()
	{
		try
		{
			txtIdPMRec.Text = string.Empty;
			((Control)(object)CmbPlantFeederPackingMaterial).Text = string.Empty;
			((Control)(object)cmbItemPackingMaterial).Text = string.Empty;
			((Control)(object)CmbWarehousePackingMaterial).Text = string.Empty;
			((UltraGridBase)CmbWarehousePackingMaterial).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
			((Control)(object)CmbBrandUomPackingMaterial).Text = string.Empty;
			txtPackingMaterialQTY.Text = string.Empty;
			txtPackingMaterialRate.Text = string.Empty;
			txtAmountPackingMaterial.Text = string.Empty;
			txtremarksPackingMaterial.Text = string.Empty;
			btnAddDetailPm.Visible = true;
			btnUpdateDetailPm.Visible = false;
			btnCancelDetailPm.Visible = false;
			CmbPlantFeederPackingMaterial.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			if (!FormValidationOfPackingMaterial())
			{
				return;
			}
			InvFoodProductionPackingMaterial obj = new InvFoodProductionPackingMaterial();
			obj.invFoodProductionPackingMaterialList = new List<InvFoodProductionPackingMaterial>();
			if (RecpackingMaterial > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecpackingMaterial;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grddetailPackingMaterial.RowCount > 0)
			{
				GridEXRow[] rows = grddetailPackingMaterial.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionPackingMaterial PackingMaterialDetail = new InvFoodProductionPackingMaterial();
					if (Conversion.ToInt((object)r.Cells["PlantId"].Text.Trim()) == 0 && Conversion.ToString((object)r.Cells["PlantId"].Text.Trim()) == string.Empty)
					{
						MessageBox.Show("Plant Field Required in Detail");
						return;
					}
					PackingMaterialDetail.PlantId = Conversion.ToInt(r.Cells["PlantId"].Value);
					PackingMaterialDetail.InvProductionJobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
					if (Conversion.ToInt(((UltraDropDownBase)cmbJobOrderPackingMaterial).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
					{
						PackingMaterialDetail.BaseDocumentTypeId = 2;
					}
					PackingMaterialDetail.DocumentTypeId = 111;
					PackingMaterialDetail.EntryDate = DateTime.Now;
					PackingMaterialDetail.EntryUser = UserAccount.ID;
					PackingMaterialDetail.ModifyDate = DateTime.Now;
					PackingMaterialDetail.ModifyUser = UserAccount.ID;
					PackingMaterialDetail.OrganizationId = UserAccount.OrganizationId;
					PackingMaterialDetail.CompanyId = UserAccount.CompanyId;
					PackingMaterialDetail.BranchesId = UserAccount.BranchesId;
					PackingMaterialDetail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					PackingMaterialDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					PackingMaterialDetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					PackingMaterialDetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					PackingMaterialDetail.ItemId = Conversion.ToInt(r.Cells["PackingItemId"].Value);
					PackingMaterialDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					PackingMaterialDetail.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
					PackingMaterialDetail.rackName = Conversion.ToString(r.Cells["RackName"].Value);
					if (PackingMaterialDetail.RackId == 0)
					{
						throw new Exception($"Rack Name field required in grid at row#{r.RowIndex + 1}");
					}
					PackingMaterialDetail.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					if (PackingMaterialDetail.ItemConditionId == 0)
					{
						throw new Exception($"Item Condition field required in grid at row#{r.RowIndex + 1}");
					}
					PackingMaterialDetail.ItemCondition = Conversion.ToString(r.Cells["ItemCondition"].Value);
					PackingMaterialDetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					PackingMaterialDetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					FormHelper.ValidateField(PackingMaterialDetail.ItemId, "Packing Item", r.RowIndex);
					if (PackingMaterialDetail.BrandId == 0 && PackingMaterialDetail.BrandUomId != 0)
					{
						MessageBox.Show($"OutPut Item Field Require in Detail grid at row#{r.RowIndex}");
						return;
					}
					if (PackingMaterialDetail.BrandUomId == 0 && PackingMaterialDetail.BrandId != 0)
					{
						MessageBox.Show("OutPut Uom Field Require in Detail");
						return;
					}
					if ((r.Cells["ItemQty"].Text == string.Empty || Conversion.ToDouble(r.Cells["ItemQty"].Value) == 0.0) && (r.Cells["ItemRate"].Text != string.Empty || Conversion.ToDouble(r.Cells["ItemRate"].Value) != 0.0))
					{
						MessageBox.Show("ItemQty Required in Detail");
						return;
					}
					PackingMaterialDetail.Qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					if ((r.Cells["ItemRate"].Text == string.Empty || Conversion.ToDouble(r.Cells["ItemRate"].Value) == 0.0) && (r.Cells["ItemQty"].Text != string.Empty || Conversion.ToDouble(r.Cells["ItemQty"].Value) != 0.0) && formright.DoHaveCanRateandAmount)
					{
						MessageBox.Show("ItemRate Required in Detail");
						return;
					}
					PackingMaterialDetail.Rate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					if ((r.Cells["Amount"].Text == string.Empty || Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0) && formright.DoHaveCanRateandAmount)
					{
						MessageBox.Show("Amount Required in Detail");
						return;
					}
					PackingMaterialDetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PackingMaterialDetail.ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
					PackingMaterialDetail.ContractScheduleNo = Conversion.ToString((object)r.Cells["ContractScheduleNo"].Text);
					PackingMaterialDetail.pmRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					DataRow row = dtScheduleData.AsEnumerable().FirstOrDefault((DataRow rrr) => rrr.Field<int>("Id") == PackingMaterialDetail.ContractScheduleId);
					if (row != null)
					{
						PackingMaterialDetail.ExImInvoiceId = row.Field<int>("InvoiceId");
					}
					obj.invFoodProductionPackingMaterialList.Add(PackingMaterialDetail);
				}
				if (obj.invFoodProductionPackingMaterialList.Count == 0)
				{
					throw new Exception("Grid Record not found please check!");
				}
			}
			InvFoodProductionPackingMaterial.Save(obj);
			if (RecpackingMaterial > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			FormResetOfPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsavepackingmaterial_Click(object sender, EventArgs e)
	{
		try
		{
			RecpackingMaterial = 0;
			PackingMaterialInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatepackingmaterial_Click(object sender, EventArgs e)
	{
		try
		{
			PackingMaterialInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdPackingMaterial(int ID)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			RecpackingMaterial = ID;
			btnsavepackingmaterial.Visible = false;
			btnUpdatepackingmaterial.Visible = true;
			InvFoodProductionPackingMaterial obj = new InvFoodProductionPackingMaterial();
			obj = InvFoodProductionPackingMaterial.GetByID(ID);
			if (obj != null)
			{
				if (obj.IsApproved)
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString((object)obj.PlanStatus) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString((object)obj.PlanStatus) + " JobPlanStatus");
				}
				tabControlPm.SelectedIndex = 0;
				tablePackingMaterial.Clear();
				txtdocdatePackingMaterial.Value = obj.DocDate;
				txtDocNoPackingMaterial.Text = Conversion.ToString((object)obj.DocNo);
				cmbJobOrderPackingMaterial.Value = obj.InvProductionJobOrderId;
				cmbJobOrderPackingMaterial_Leave(null, null);
				((Control)(object)cmbJobOrderPackingMaterial).Enabled = false;
				if (tablePackingMaterial.Columns.Count == 0)
				{
					tablePackingMaterial.Columns.Add("Id", typeof(int));
					tablePackingMaterial.Columns.Add("DocDate", typeof(DateTime));
					tablePackingMaterial.Columns.Add("DocNo", typeof(int));
					tablePackingMaterial.Columns.Add("InvProductionJobOrderId", typeof(int));
					tablePackingMaterial.Columns.Add("PlantId", typeof(int));
					tablePackingMaterial.Columns.Add("Plant", typeof(string));
					tablePackingMaterial.Columns.Add("PackingItemId", typeof(int));
					tablePackingMaterial.Columns.Add("PackingItem", typeof(string));
					tablePackingMaterial.Columns.Add("WarehouseId", typeof(int));
					tablePackingMaterial.Columns.Add("Warehouse", typeof(string));
					tablePackingMaterial.Columns.Add("RackId", typeof(int));
					tablePackingMaterial.Columns.Add("RackName", typeof(string));
					tablePackingMaterial.Columns.Add("ItemConditionId", typeof(int));
					tablePackingMaterial.Columns.Add("ItemCondition", typeof(string));
					tablePackingMaterial.Columns.Add("BrandId", typeof(int));
					tablePackingMaterial.Columns.Add("Brand", typeof(string));
					tablePackingMaterial.Columns.Add("BrandUomId", typeof(int));
					tablePackingMaterial.Columns.Add("BrandUom", typeof(string));
					tablePackingMaterial.Columns.Add("ItemQty", typeof(double));
					tablePackingMaterial.Columns.Add("ItemRate", typeof(double));
					tablePackingMaterial.Columns.Add("Amount", typeof(double));
					tablePackingMaterial.Columns.Add("Remarks", typeof(string));
					tablePackingMaterial.Columns.Add("ContractScheduleId", typeof(int));
					tablePackingMaterial.Columns.Add("ContractScheduleNo", typeof(string));
				}
				tablePackingMaterial.Rows.Add(obj.Id, Conversion.ToDateTime((object)obj.DocDate).ToShortDateString(), obj.DocNo, obj.InvProductionJobOrderId, obj.PlantId, obj.PlantName, obj.ItemId, obj.ItemName, obj.WarehouseId, obj.WareHouseName, obj.RackId, obj.rackName, obj.ItemConditionId, obj.ItemCondition, obj.BrandId, obj.BrandName, obj.BrandUomId, obj.BrandUom, obj.Qty, obj.Rate, obj.Amount, Conversion.ToString((object)obj.pmRemarks), Conversion.ToInt((object)obj.ContractScheduleId), Conversion.ToString((object)obj.ContractScheduleNo));
				grddetailPackingMaterial.DataSource = tablePackingMaterial;
				grddetailPackingMaterial.RetrieveStructure();
				grdDetailPackingMaterialSettings();
				cmbItemPackingMaterial.Focus();
				UpdateMode = true;
				GetVoucherHeadId(RecpackingMaterial, 111);
				ResetdetailPackingMaterial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryPMGrid()
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
			if (chkPendingForRates.Checked)
			{
				obj.ActionId = 1;
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
			else if (rdapproveddate.Checked)
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
			obj.JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			DataTable dtpm = InvFoodProductionPackingMaterial.GetAll(obj);
			if (dtpm.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Clear();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("JobOrderId", typeof(int));
				dt.Columns.Add("JobOrder", typeof(string));
				dt.Columns.Add("PlantName", typeof(string));
				dt.Columns.Add("Item", typeof(string));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("RackName", typeof(string));
				dt.Columns.Add("ItemCondition", typeof(string));
				dt.Columns.Add("BrandName", typeof(string));
				dt.Columns.Add("BrandUom", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("ContractScheduleNo", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("JobPlanStatus", typeof(string));
				for (int i = 0; i < dtpm.Rows.Count; i++)
				{
					dt.Rows.Add(dtpm.Rows[i]["Id"], dtpm.Rows[i]["DocNo"], Conversion.ToDateTime(dtpm.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpm.Rows[i]["InvProductionJobOrderId"], dtpm.Rows[i]["RefInvoiceNo"], dtpm.Rows[i]["PlantName"], dtpm.Rows[i]["ItemName"], dtpm.Rows[i]["WareHouseName"], dtpm.Rows[i]["rackName"], dtpm.Rows[i]["ItemCondition"], dtpm.Rows[i]["BrandName"], dtpm.Rows[i]["BrandUom"], dtpm.Rows[i]["Qty"], dtpm.Rows[i]["Rate"], dtpm.Rows[i]["Amount"], dtpm.Rows[i]["pmRemarks"], dtpm.Rows[i]["ContractScheduleNo"], dtpm.Rows[i]["JobOrderStatus"], dtpm.Rows[i]["JobPlanStatus"]);
				}
				grdPackingMaterialHistory.DataSource = dt;
				grdPackingMaterialHistory.RetrieveStructure();
				GridSettingOfPackingmaterial();
			}
			else
			{
				grdPackingMaterialHistory.DataSource = null;
				grddetailHistoryPM.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingOfPackingmaterial()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdPackingMaterialHistory.RootTable.Columns["Rate"].Visible = false;
				grdPackingMaterialHistory.RootTable.Columns["Amount"].Visible = false;
			}
			grdPackingMaterialHistory.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterialHistory.RootTable.Columns["JobOrderId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdPackingMaterialHistory, 2, 3);
			grdPackingMaterialHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdPackingMaterialHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdPackingMaterialHistory.RootTable.Columns["JobOrder"].Width = Constants.InventoryConstants.JobLotConstant + 50;
			grdPackingMaterialHistory.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			grdPackingMaterialHistory.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdPackingMaterialHistory.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdPackingMaterialHistory.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdPackingMaterialHistory.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdPackingMaterialHistory.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPackingMaterialHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPackingMaterialHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPackingMaterialHistory.RootTable.Columns["BrandName"].Width = Constants.InventoryConstants.ItemName;
			grdPackingMaterialHistory.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant;
			grdPackingMaterialHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdPackingMaterialHistory.RootTable.Columns["ContractScheduleNo"].Width = Constants.InventoryConstants.JobLotConstant + 20;
			grdPackingMaterialHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdPackingMaterialHistory.RootTable.Columns["JobPlanStatus"].Width = Constants.InventoryConstants.StatusConstant;
			GridEX_Helper.AddButton(grdPackingMaterialHistory, "Edit", "Edit", 40, 0);
			grdPackingMaterialHistory.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnsavepackingmaterial.Visible = false;
			btnUpdatepackingmaterial.Visible = true;
			if (Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["JobPlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["JobPlanStatus"].Value) + " JobPlanStatus");
			}
			RecpackingMaterial = Conversion.ToInt((object)grdPackingMaterialHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadByIdPackingMaterial(RecpackingMaterial);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPackingMaterialSlip_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(grddetailPackingMaterial.CurrentRow.Cells["InvProductionJobOrderId"].Value),
				ReportType = "PackingMaterial"
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For DisPlay");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialRate_TextChanged(object sender, EventArgs e)
	{
		CaluculatonofPackingMaterial();
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
			else
			{
				txtAmountPackingMaterial.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialQTY_TextChanged(object sender, EventArgs e)
	{
		GetAvgRateByItemAndWarehouseForPackingMAterial();
		CaluculatonofPackingMaterial();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grddetailPackingMaterial).Name);
			if (fs != null)
			{
				grddetailPackingMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingMaterialHistory).Name);
			if (fs != null)
			{
				grdPackingMaterialHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControlPm_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControlPm.SelectedIndex == 1)
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				chkPendingForRates.Visible = false;
			}
			else
			{
				chkPendingForRates.Visible = true;
			}
			chkPendingForRates.Checked = false;
		}
	}

	private void grdPackingMaterialHistory_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void btnAddDetailPm_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionPackingMaterial())
			{
				tablePackingMaterial.Rows.Add(0, Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value).ToShortDateString(), Conversion.ToInt((object)txtDocNoPackingMaterial.Text.Trim()), Conversion.ToInt(cmbJobOrderPackingMaterial.Value), Conversion.ToInt(CmbPlantFeederPackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbPlantFeederPackingMaterial).Text.Trim()), Conversion.ToInt(cmbItemPackingMaterial.Value), Conversion.ToString((object)((Control)(object)cmbItemPackingMaterial).Text.Trim()), Conversion.ToInt(CmbWarehousePackingMaterial.Value), (Conversion.ToInt(CmbWarehousePackingMaterial.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbWarehousePackingMaterial).Text.Trim()) : "", Conversion.ToInt(CmbRackName.Value), Conversion.ToString((object)((Control)(object)CmbRackName).Text.Trim()), Conversion.ToInt(cmbItemCondition.Value), Conversion.ToString((object)((Control)(object)cmbItemCondition).Text.Trim()), Conversion.ToInt(CmbBrandNamePackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbBrandNamePackingMaterial).Text.Trim()), Conversion.ToInt(CmbBrandUomPackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomPackingMaterial).Text.Trim()), Conversion.ToDouble((object)txtPackingMaterialQTY.Text.Trim()), Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim()), Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim()), Conversion.ToString((object)txtremarksPackingMaterial.Text.Trim()), Conversion.ToInt(CmbInvoiceNo.Value), Conversion.ToString((object)((Control)(object)CmbInvoiceNo).Text));
				grddetailPackingMaterial.DataSource = tablePackingMaterial;
				grddetailPackingMaterial.RetrieveStructure();
				grdDetailPackingMaterialSettings();
				ResetdetailPackingMaterial();
				JobOrderPackingMaterialLock();
				CmbPlantFeederPackingMaterial.Focus();
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
			grddetailPackingMaterial.AllowEdit = (InheritableBoolean)1;
			List<string> hiddenColumns = new List<string>
			{
				"Id", "PackingItemId", "WarehouseId", "RackId", "ItemConditionId", "PlantId", "BrandUomId", "BrandId", "InvProductionJobOrderId", "ContractScheduleId",
				"DocDate", "DocNo"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"DocDate",
				(EditType)2
			} };
			if (!formright.DoHaveCanRateandAmount)
			{
				hiddenColumns.AddRange(new string[2] { "ItemRate", "Amount" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailPackingMaterial, 2, 3, hiddenColumns, editableColumns);
			grddetailPackingMaterial.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grddetailPackingMaterial.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grddetailPackingMaterial.RootTable.Columns["Plant"].Width = Constants.InventoryConstants.PlantName;
			grddetailPackingMaterial.RootTable.Columns["PackingItem"].Width = Constants.InventoryConstants.ItemName;
			grddetailPackingMaterial.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grddetailPackingMaterial.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grddetailPackingMaterial.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grddetailPackingMaterial.RootTable.Columns["Brand"].Width = Constants.InventoryConstants.ItemName;
			grddetailPackingMaterial.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant;
			grddetailPackingMaterial.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grddetailPackingMaterial.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grddetailPackingMaterial.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grddetailPackingMaterial.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grddetailPackingMaterial.RootTable.Columns["ContractScheduleNo"].Width = Constants.ExportConstants.ScheduleCode;
			grddetailPackingMaterial.RootTable.Columns["ContractScheduleNo"].Caption = "Schedule / Invoice No";
			GridEX_Helper.AddButton(grddetailPackingMaterial, "Delete", "X", 20, 0);
			grddetailPackingMaterial.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderPackingMaterialLock()
	{
		try
		{
			if (tablePackingMaterial.Rows.Count > 0)
			{
				((Control)(object)cmbJobOrderPackingMaterial).Enabled = false;
				cmbJobOrderPackingMaterial.Value = tablePackingMaterial.Rows[0]["InvProductionJobOrderId"];
			}
			else
			{
				((Control)(object)cmbJobOrderPackingMaterial).Enabled = true;
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
				tablePackingMaterial.Rows[updateDetailIndexPM]["PlantId"] = Conversion.ToInt(CmbPlantFeederPackingMaterial.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["WarehouseId"] = Conversion.ToInt(CmbWarehousePackingMaterial.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["Warehouse"] = ((Conversion.ToInt(CmbWarehousePackingMaterial.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbWarehousePackingMaterial).Text.Trim()) : "");
				tablePackingMaterial.Rows[updateDetailIndexPM]["PackingItemId"] = Conversion.ToInt(cmbItemPackingMaterial.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["PackingItem"] = Conversion.ToString((object)((Control)(object)cmbItemPackingMaterial).Text);
				tablePackingMaterial.Rows[updateDetailIndexPM]["BrandId"] = Conversion.ToInt(CmbBrandNamePackingMaterial.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["Brand"] = Conversion.ToString((object)((Control)(object)CmbBrandNamePackingMaterial).Text);
				CmbBrandNamePackingMaterial_Leave(null, null);
				tablePackingMaterial.Rows[updateDetailIndexPM]["BrandUomId"] = Conversion.ToInt(CmbBrandUomPackingMaterial.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomPackingMaterial).Text);
				tablePackingMaterial.Rows[updateDetailIndexPM]["ItemConditionId"] = Conversion.ToInt(cmbItemCondition.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["ItemCondition"] = Conversion.ToString((object)((Control)(object)cmbItemCondition).Text);
				tablePackingMaterial.Rows[updateDetailIndexPM]["ItemQty"] = Conversion.ToDouble((object)txtPackingMaterialQTY.Text.Trim());
				tablePackingMaterial.Rows[updateDetailIndexPM]["ItemRate"] = Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim());
				tablePackingMaterial.Rows[updateDetailIndexPM]["Amount"] = Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim());
				tablePackingMaterial.Rows[updateDetailIndexPM]["Remarks"] = Conversion.ToString((object)txtremarksPackingMaterial.Text.Trim());
				tablePackingMaterial.Rows[updateDetailIndexPM]["ContractScheduleId"] = Conversion.ToInt(CmbInvoiceNo.Value);
				tablePackingMaterial.Rows[updateDetailIndexPM]["ContractScheduleNo"] = ((Conversion.ToInt(CmbInvoiceNo.Value) > 0) ? ((Control)(object)CmbInvoiceNo).Text : "");
				ResetdetailPackingMaterial();
				JobOrderPackingMaterialLock();
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

	private void grddetailPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (!btnsavepackingmaterial.Enabled || !btnsavepackingmaterial.Visible)
				{
					MessageBox.Show("You Can not Delete Record In Update Mode");
					return;
				}
				GridEXRow item = grddetailPackingMaterial.CurrentRow;
				tablePackingMaterial.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grddetailPackingMaterial.UpdateData();
			JobOrderPackingMaterialLock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailPackingMaterial_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grddetailPackingMaterial.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPM = item.RowIndex;
				txtdocdatePackingMaterial.Value = Conversion.ToDateTime(item.Cells["DocDate"].Value);
				txtDocNoPackingMaterial.Text = item.Cells["DocNo"].Text.ToString();
				txtIdPMRec.Text = item.Cells["Id"].Value.ToString();
				cmbJobOrderPackingMaterial.Value = item.Cells["InvProductionJobOrderId"].Value;
				cmbJobOrderPackingMaterial_Leave(null, null);
				CmbPlantFeederPackingMaterial.Value = item.Cells["PlantId"].Value;
				((Control)(object)CmbPlantFeederPackingMaterial).Text = item.Cells["Plant"].Text.ToString();
				cmbItemPackingMaterial.Value = Conversion.ToInt(item.Cells["PackingItemId"].Value);
				cmbItemPackingMaterial_Leave(null, null);
				CmbWarehousePackingMaterial.Value = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				CmbRackName.Value = Conversion.ToInt(item.Cells["RackId"].Value);
				cmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				CmbBrandNamePackingMaterial.Value = item.Cells["BrandId"].Value;
				CmbBrandNamePackingMaterial_Leave(null, null);
				CmbBrandUomPackingMaterial.Value = Conversion.ToInt(item.Cells["BrandUomId"].Value);
				txtPackingMaterialQTY.Text = item.Cells["ItemQty"].Text.ToString();
				txtPackingMaterialRate.Text = item.Cells["ItemRate"].Text.ToString();
				txtAmountPackingMaterial.Text = item.Cells["Amount"].Text.ToString();
				txtremarksPackingMaterial.Text = item.Cells["Remarks"].Text.ToString();
				CmbInvoiceNo.Value = item.Cells["ContractScheduleId"].Text.ToString();
				btnAddDetailPm.Visible = false;
				btnUpdateDetailPm.Visible = true;
				btnCancelDetailPm.Visible = true;
				CmbPlantFeederPackingMaterial.Focus();
				JobOrderPackingMaterialLock();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkPendingForRates_CheckedChanged(object sender, EventArgs e)
	{
		BindHistoryPMGrid();
	}

	private void cmbItemPackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
			GetOutPutBalQtyForPMQty();
			BrandItemsForPackMaterialAgainstPmItem();
			GetAvgRateByItemAndWarehouseForPackingMAterial();
			CaluculatonofPackingMaterial();
			BindWarehouseDropdown(CmbWarehousePackingMaterial, ItemId);
			RackBindFromGlobalRacksByItemId(ItemId, Conversion.ToInt(CmbWarehousePackingMaterial.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRateByItemAndWarehouseForPackingMAterial()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			txtPackingMaterialRate.Text = "0";
			if (!formright.DoHaveCanRateandAmount)
			{
				return;
			}
			txtPackingMaterialRate.Enabled = true;
			double rate = Conversion.ToDouble((object)txtPackingMaterialRate.Text);
			if (FIFOCGSFlag)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.DocDate = Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value);
				obj.ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
				obj.WarehouseId = Conversion.ToInt(CmbWarehousePackingMaterial.Value);
				obj.ItemQty = Conversion.ToDouble((object)txtPackingMaterialQTY.Text);
				obj.NetWeight = Conversion.ToDouble((object)txtPackingMaterialQTY.Text);
				if (RecpackingMaterial > 0)
				{
					obj.DocumentTypeId = 111;
					obj.Id = RecpackingMaterial;
				}
				rate = Math.Round(CommonServices.GetAvgRateFromFIFOMethod(obj), 3);
				txtPackingMaterialRate.Enabled = false;
			}
			else
			{
				DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItemPackingMaterial.Value), Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value), Conversion.ToInt(cmbItemCondition.Value), RecpackingMaterial, (RecpackingMaterial > 0) ? 111 : 0);
				if (dtRate.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
				}
			}
			if (rate > 0.0)
			{
				txtPackingMaterialRate.Text = Conversion.ToString((object)rate);
				txtPackingMaterialRate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandUomPackingMaterial_Leave(object sender, EventArgs e)
	{
		GetAvgRateByItemAndWarehouseForPackingMAterial();
		GetOutPutBalQtyForPMQty();
	}

	private void grdPackingMaterialHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			btnsavepackingmaterial.Visible = false;
			btnUpdatepackingmaterial.Visible = true;
			if (Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["JobPlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdPackingMaterialHistory.CurrentRow.Cells["JobPlanStatus"].Value) + " JobPlanStatus");
			}
			RecpackingMaterial = Conversion.ToInt((object)grdPackingMaterialHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadByIdPackingMaterial(RecpackingMaterial);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void txtPackingMaterialQTY_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvgRateByItemAndWarehouseForPackingMAterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocdatePackingMaterial_ValueChanged(object sender, EventArgs e)
	{
	}

	private void CmbPlantFeederPackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			GetTotalInPutOutOnPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWarehousePackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			RackBindFromGlobalRacksByItemId(Conversion.ToInt(cmbItemPackingMaterial.Value), Conversion.ToInt(CmbWarehousePackingMaterial.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AverageCalculate()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		try
		{
			if (grddetailPackingMaterial.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grddetailPackingMaterial.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["DocDate"].Value = Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value);
				int ItemId = Conversion.ToInt(r.Cells["PackingItemId"].Value);
				int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				int WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				double Qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
				double rate = 0.0;
				if (FIFOCGSFlag)
				{
					ReportsParameters obj = new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value),
						ItemId = ItemId,
						WarehouseId = WareHouseId,
						ItemQty = Qty,
						NetWeight = Qty
					};
					if (RecpackingMaterial > 0)
					{
						obj.DocumentTypeId = 111;
						obj.Id = RecpackingMaterial;
					}
					rate = Math.Round(CommonServices.GetAvgRateFromFIFOMethod(obj), 3);
					if (rate > 0.0)
					{
						r.Cells["ItemRate"].Value = rate;
						r.Cells["Amount"].Value = Qty * rate;
					}
					else
					{
						r.Cells["ItemRate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, Conversion.ToDateTime((object)txtdocdatePackingMaterial.Value), ItemConditionId, RecpackingMaterial, (RecpackingMaterial > 0) ? 111 : 0);
					if (dtRate.Rows.Count > 0)
					{
						rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
					}
					if (rate > 0.0)
					{
						r.Cells["ItemRate"].Value = rate;
						r.Cells["Amount"].Value = Qty * rate;
					}
				}
				r.EndEdit();
			}
			grddetailPackingMaterial.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateRate_Click(object sender, EventArgs e)
	{
		try
		{
			AverageCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvgRateByItemAndWarehouseForPackingMAterial();
			CaluculatonofPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherPM_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			return InvFoodProductionPackingMaterial.GetDataForDropDownFromInvFoodProductionPackingMaterial(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Activity = "ProductionJobOrder"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbJobOrder, dt, "Id", "ReferenceName", "Job Order", AllColumns: true, Conversion.ToInt(CmbJobOrder.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbJobOrder).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Clear();
			txtToDocNoHistory.Clear();
			((Control)(object)CmbJobOrder).Text = string.Empty;
			grdPackingMaterialHistory.DataSource = null;
			grddetailHistoryPM.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboDBCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistoryPMGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Expected O, but got Unknown
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Expected O, but got Unknown
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Expected O, but got Unknown
		//IL_54e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_54ef: Expected O, but got Unknown
		GridEXLayout grddetailPackingMaterial_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionPackingMaterial));
		GridEXLayout grdPackingMaterialHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance115 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		GridEXLayout grddetailHistoryPM_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlPm = new System.Windows.Forms.TabControl();
		this.PmForm = new System.Windows.Forms.TabPage();
		this.panel66 = new System.Windows.Forms.Panel();
		this.grddetailPackingMaterial = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label100 = new System.Windows.Forms.Label();
		this.panel42 = new System.Windows.Forms.Panel();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.CmbRackName = new UltraCombo();
		this.txtIdPMRec = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.CmbBrandUomPackingMaterial = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbBrandNamePackingMaterial = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.labelEXInv = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnAddDetailPm = new System.Windows.Forms.Button();
		this.CmbWarehousePackingMaterial = new UltraCombo();
		this.cmbItemCondition = new UltraCombo();
		this.label79 = new System.Windows.Forms.Label();
		this.btnCancelDetailPm = new System.Windows.Forms.Button();
		this.btnUpdateDetailPm = new System.Windows.Forms.Button();
		this.CmbPlantFeederPackingMaterial = new UltraCombo();
		this.txtremarksPackingMaterial = new System.Windows.Forms.TextBox();
		this.label84 = new System.Windows.Forms.Label();
		this.lblRemarksPackingMaterial = new System.Windows.Forms.Label();
		this.txtPackingMaterialRate = new System.Windows.Forms.TextBox();
		this.cmbItemPackingMaterial = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.lblRatePackingMaterial = new System.Windows.Forms.Label();
		this.txtPackingMaterialQTY = new System.Windows.Forms.TextBox();
		this.txtAmountPackingMaterial = new System.Windows.Forms.TextBox();
		this.lblAmountPackingMaterial = new System.Windows.Forms.Label();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label104 = new System.Windows.Forms.Label();
		this.txtTotalFinishQtyPm = new System.Windows.Forms.TextBox();
		this.label105 = new System.Windows.Forms.Label();
		this.txtTotalFinishWeightPm = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.txtTotalInputQtyPm = new System.Windows.Forms.TextBox();
		this.label107 = new System.Windows.Forms.Label();
		this.txtTotalInputWeightPm = new System.Windows.Forms.TextBox();
		this.label108 = new System.Windows.Forms.Label();
		this.txtTotaloutputQtyPm = new System.Windows.Forms.TextBox();
		this.label109 = new System.Windows.Forms.Label();
		this.txtTotalOutPutWeightPm = new System.Windows.Forms.TextBox();
		this.txtdocdatePackingMaterial = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoPackingMaterial = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbJobOrderPackingMaterial = new UltraCombo();
		this.panel41 = new System.Windows.Forms.Panel();
		this.label96 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNewpackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshPackingMaterial = new System.Windows.Forms.ToolStripButton();
		this.btnsavepackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.btnUpdatepackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPackingMaterialSlip = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherPM = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateRate = new System.Windows.Forms.ToolStripButton();
		this.PmHistory = new System.Windows.Forms.TabPage();
		this.panel70 = new System.Windows.Forms.Panel();
		this.grdPackingMaterialHistory = new GridEX();
		this.panel73 = new System.Windows.Forms.Panel();
		this.label111 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.ctrlGrdBar15 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbJobOrder = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel71 = new System.Windows.Forms.Panel();
		this.chkPendingForRates = new System.Windows.Forms.CheckBox();
		this.label88 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grddetailHistoryPM = new GridEX();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControlPm.SuspendLayout();
		this.PmForm.SuspendLayout();
		this.panel66.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailPackingMaterial).BeginInit();
		this.panel6.SuspendLayout();
		this.panel42.SuspendLayout();
		this.groupBox8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPackingMaterial).BeginInit();
		this.panel41.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.PmHistory.SuspendLayout();
		this.panel70.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).BeginInit();
		this.panel73.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		this.panel71.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailHistoryPM).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlPm);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1299, 687);
		this.panel1.TabIndex = 0;
		this.tabControlPm.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlPm.Controls.Add(this.PmForm);
		this.tabControlPm.Controls.Add(this.PmHistory);
		this.tabControlPm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlPm.Location = new System.Drawing.Point(0, 0);
		this.tabControlPm.Name = "tabControlPm";
		this.tabControlPm.SelectedIndex = 0;
		this.tabControlPm.Size = new System.Drawing.Size(1299, 687);
		this.tabControlPm.TabIndex = 0;
		this.tabControlPm.SelectedIndexChanged += new System.EventHandler(tabControlPm_SelectedIndexChanged);
		this.PmForm.Controls.Add(this.panel66);
		this.PmForm.Location = new System.Drawing.Point(4, 4);
		this.PmForm.Name = "PmForm";
		this.PmForm.Padding = new System.Windows.Forms.Padding(3);
		this.PmForm.Size = new System.Drawing.Size(1291, 658);
		this.PmForm.TabIndex = 0;
		this.PmForm.Text = "Form";
		this.PmForm.UseVisualStyleBackColor = true;
		this.panel66.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial);
		this.panel66.Controls.Add(this.panel6);
		this.panel66.Controls.Add(this.panel42);
		this.panel66.Controls.Add(this.panel41);
		this.panel66.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel66.Location = new System.Drawing.Point(3, 3);
		this.panel66.Name = "panel66";
		this.panel66.Size = new System.Drawing.Size(1285, 652);
		this.panel66.TabIndex = 0;
		this.grddetailPackingMaterial.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailPackingMaterial).BackColor = System.Drawing.Color.White;
		grddetailPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailPackingMaterial.DesignTimeLayout = grddetailPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailPackingMaterial.FilterMode = (FilterMode)2;
		this.grddetailPackingMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).Location = new System.Drawing.Point(0, 271);
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).Name = "grddetailPackingMaterial";
		((ScrollableBase)this.grddetailPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).Size = new System.Drawing.Size(1285, 381);
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).TabIndex = 5;
		this.grddetailPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grddetailPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grddetailPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grddetailPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grddetailPackingMaterial).DoubleClick += new System.EventHandler(grddetailPackingMaterial_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label100);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 245);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1285, 26);
		this.panel6.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1244, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grddetailPackingMaterial;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 180;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label100.ForeColor = System.Drawing.Color.White;
		this.label100.Location = new System.Drawing.Point(3, 3);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(161, 17);
		this.label100.TabIndex = 179;
		this.label100.Text = "Packing Material Detail";
		this.panel42.Controls.Add(this.groupBox8);
		this.panel42.Controls.Add(this.groupBox5);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 54);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(1285, 191);
		this.panel42.TabIndex = 0;
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox8.Controls.Add(this.txtIdPMRec);
		this.groupBox8.Controls.Add(this.label13);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial);
		this.groupBox8.Controls.Add(this.label33);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial);
		this.groupBox8.Controls.Add(this.label4);
		this.groupBox8.Controls.Add(this.labelEXInv);
		this.groupBox8.Controls.Add(this.label1);
		this.groupBox8.Controls.Add(this.btnAddDetailPm);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCondition);
		this.groupBox8.Controls.Add(this.label79);
		this.groupBox8.Controls.Add(this.btnCancelDetailPm);
		this.groupBox8.Controls.Add(this.btnUpdateDetailPm);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial);
		this.groupBox8.Controls.Add(this.txtremarksPackingMaterial);
		this.groupBox8.Controls.Add(this.label84);
		this.groupBox8.Controls.Add(this.lblRemarksPackingMaterial);
		this.groupBox8.Controls.Add(this.txtPackingMaterialRate);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial);
		this.groupBox8.Controls.Add(this.label41);
		this.groupBox8.Controls.Add(this.label40);
		this.groupBox8.Controls.Add(this.lblRatePackingMaterial);
		this.groupBox8.Controls.Add(this.txtPackingMaterialQTY);
		this.groupBox8.Controls.Add(this.txtAmountPackingMaterial);
		this.groupBox8.Controls.Add(this.lblAmountPackingMaterial);
		this.groupBox8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox8.Location = new System.Drawing.Point(3, 76);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(1016, 111);
		this.groupBox8.TabIndex = 1;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Detail";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(538, 35);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Leave += new System.EventHandler(CmbRackName_Leave);
		this.txtIdPMRec.BackColor = System.Drawing.Color.White;
		this.txtIdPMRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIdPMRec.Enabled = false;
		this.txtIdPMRec.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIdPMRec.Location = new System.Drawing.Point(1002, 12);
		this.txtIdPMRec.Name = "txtIdPMRec";
		this.txtIdPMRec.ReadOnly = true;
		this.txtIdPMRec.Size = new System.Drawing.Size(10, 23);
		this.txtIdPMRec.TabIndex = 212;
		this.txtIdPMRec.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(538, 16);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 243;
		this.label13.Text = "Rack Name";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(801, 79);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 11;
		this.CmbBrandUomPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Location = new System.Drawing.Point(8, 79);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Name = "CmbBrandUomPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Leave += new System.EventHandler(CmbBrandUomPackingMaterial_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 63);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(61, 15);
		this.label33.TabIndex = 120;
		this.label33.Text = "Item Uom";
		this.CmbBrandNamePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNamePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNamePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNamePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNamePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Location = new System.Drawing.Point(801, 35);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Name = "CmbBrandNamePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Leave += new System.EventHandler(CmbBrandNamePackingMaterial_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(801, 16);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(111, 15);
		this.label4.TabIndex = 118;
		this.label4.Text = "OutPut Item  Name";
		this.labelEXInv.AutoSize = true;
		this.labelEXInv.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelEXInv.Location = new System.Drawing.Point(801, 63);
		this.labelEXInv.Name = "labelEXInv";
		this.labelEXInv.Size = new System.Drawing.Size(125, 15);
		this.labelEXInv.TabIndex = 594;
		this.labelEXInv.Text = "Schedule / Invoice No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(671, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(87, 15);
		this.label1.TabIndex = 226;
		this.label1.Text = "Item Condition";
		this.btnAddDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetailPm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btnAddDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnAddDetailPm.Location = new System.Drawing.Point(936, 85);
		this.btnAddDetailPm.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddDetailPm.Name = "btnAddDetailPm";
		this.btnAddDetailPm.Size = new System.Drawing.Size(38, 22);
		this.btnAddDetailPm.TabIndex = 12;
		this.btnAddDetailPm.Text = "+";
		this.btnAddDetailPm.UseVisualStyleBackColor = false;
		this.btnAddDetailPm.Click += new System.EventHandler(btnAddDetailPm_Click);
		this.CmbWarehousePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehousePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehousePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehousePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehousePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Location = new System.Drawing.Point(394, 35);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Name = "CmbWarehousePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Leave += new System.EventHandler(CmbWarehousePackingMaterial_Leave);
		this.cmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance1).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Location = new System.Drawing.Point(671, 35);
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Name = "cmbItemCondition";
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Size = new System.Drawing.Size(129, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItemCondition).Leave += new System.EventHandler(cmbItemCondition_Leave);
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label79.Location = new System.Drawing.Point(394, 16);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(67, 15);
		this.label79.TabIndex = 122;
		this.label79.Text = "Warehouse";
		this.btnCancelDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetailPm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetailPm.Location = new System.Drawing.Point(936, 63);
		this.btnCancelDetailPm.Name = "btnCancelDetailPm";
		this.btnCancelDetailPm.Size = new System.Drawing.Size(67, 24);
		this.btnCancelDetailPm.TabIndex = 14;
		this.btnCancelDetailPm.Text = "Cancel";
		this.btnCancelDetailPm.UseVisualStyleBackColor = false;
		this.btnCancelDetailPm.Click += new System.EventHandler(btnCancelDetailPm_Click);
		this.btnUpdateDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailPm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailPm.Location = new System.Drawing.Point(936, 85);
		this.btnUpdateDetailPm.Name = "btnUpdateDetailPm";
		this.btnUpdateDetailPm.Size = new System.Drawing.Size(67, 22);
		this.btnUpdateDetailPm.TabIndex = 13;
		this.btnUpdateDetailPm.Text = "Update ";
		this.btnUpdateDetailPm.UseVisualStyleBackColor = false;
		this.btnUpdateDetailPm.Click += new System.EventHandler(btnUpdateDetailPm_Click);
		this.CmbPlantFeederPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Location = new System.Drawing.Point(6, 35);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Name = "CmbPlantFeederPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Leave += new System.EventHandler(CmbPlantFeederPackingMaterial_Leave);
		this.txtremarksPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksPackingMaterial.Location = new System.Drawing.Point(394, 81);
		this.txtremarksPackingMaterial.Name = "txtremarksPackingMaterial";
		this.txtremarksPackingMaterial.Size = new System.Drawing.Size(406, 23);
		this.txtremarksPackingMaterial.TabIndex = 10;
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(6, 16);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(69, 15);
		this.label84.TabIndex = 211;
		this.label84.Text = "Plant Name";
		this.lblRemarksPackingMaterial.AutoSize = true;
		this.lblRemarksPackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarksPackingMaterial.Location = new System.Drawing.Point(393, 63);
		this.lblRemarksPackingMaterial.Name = "lblRemarksPackingMaterial";
		this.lblRemarksPackingMaterial.Size = new System.Drawing.Size(52, 15);
		this.lblRemarksPackingMaterial.TabIndex = 63;
		this.lblRemarksPackingMaterial.Text = "Remarks";
		this.txtPackingMaterialRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialRate.Location = new System.Drawing.Point(191, 81);
		this.txtPackingMaterialRate.Name = "txtPackingMaterialRate";
		this.txtPackingMaterialRate.Size = new System.Drawing.Size(91, 23);
		this.txtPackingMaterialRate.TabIndex = 8;
		this.txtPackingMaterialRate.TextChanged += new System.EventHandler(txtPackingMaterialRate_TextChanged);
		this.cmbItemPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbItemPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Location = new System.Drawing.Point(177, 35);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Name = "cmbItemPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Leave += new System.EventHandler(cmbItemPackingMaterial_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(110, 63);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(54, 15);
		this.label41.TabIndex = 59;
		this.label41.Text = "Item Qty";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(177, 16);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(158, 15);
		this.label40.TabIndex = 55;
		this.label40.Text = "Packing Material Item Name";
		this.lblRatePackingMaterial.AutoSize = true;
		this.lblRatePackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRatePackingMaterial.Location = new System.Drawing.Point(191, 63);
		this.lblRatePackingMaterial.Name = "lblRatePackingMaterial";
		this.lblRatePackingMaterial.Size = new System.Drawing.Size(58, 15);
		this.lblRatePackingMaterial.TabIndex = 59;
		this.lblRatePackingMaterial.Text = "Item Rate";
		this.txtPackingMaterialQTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQTY.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQTY.Location = new System.Drawing.Point(110, 81);
		this.txtPackingMaterialQTY.Name = "txtPackingMaterialQTY";
		this.txtPackingMaterialQTY.Size = new System.Drawing.Size(80, 23);
		this.txtPackingMaterialQTY.TabIndex = 7;
		this.txtPackingMaterialQTY.TextChanged += new System.EventHandler(txtPackingMaterialQTY_TextChanged);
		this.txtPackingMaterialQTY.Leave += new System.EventHandler(txtPackingMaterialQTY_Leave);
		this.txtAmountPackingMaterial.BackColor = System.Drawing.Color.White;
		this.txtAmountPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountPackingMaterial.Enabled = false;
		this.txtAmountPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountPackingMaterial.Location = new System.Drawing.Point(283, 81);
		this.txtAmountPackingMaterial.Name = "txtAmountPackingMaterial";
		this.txtAmountPackingMaterial.ReadOnly = true;
		this.txtAmountPackingMaterial.Size = new System.Drawing.Size(110, 23);
		this.txtAmountPackingMaterial.TabIndex = 9;
		this.lblAmountPackingMaterial.AutoSize = true;
		this.lblAmountPackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountPackingMaterial.Location = new System.Drawing.Point(283, 63);
		this.lblAmountPackingMaterial.Name = "lblAmountPackingMaterial";
		this.lblAmountPackingMaterial.Size = new System.Drawing.Size(51, 15);
		this.lblAmountPackingMaterial.TabIndex = 61;
		this.lblAmountPackingMaterial.Text = "Amount";
		this.groupBox5.Controls.Add(this.label104);
		this.groupBox5.Controls.Add(this.txtTotalFinishQtyPm);
		this.groupBox5.Controls.Add(this.label105);
		this.groupBox5.Controls.Add(this.txtTotalFinishWeightPm);
		this.groupBox5.Controls.Add(this.label106);
		this.groupBox5.Controls.Add(this.txtTotalInputQtyPm);
		this.groupBox5.Controls.Add(this.label107);
		this.groupBox5.Controls.Add(this.txtTotalInputWeightPm);
		this.groupBox5.Controls.Add(this.label108);
		this.groupBox5.Controls.Add(this.txtTotaloutputQtyPm);
		this.groupBox5.Controls.Add(this.label109);
		this.groupBox5.Controls.Add(this.txtTotalOutPutWeightPm);
		this.groupBox5.Controls.Add(this.txtdocdatePackingMaterial);
		this.groupBox5.Controls.Add(this.txtDocNoPackingMaterial);
		this.groupBox5.Controls.Add(this.label37);
		this.groupBox5.Controls.Add(this.label35);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(3, 1);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1016, 74);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Main";
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.Location = new System.Drawing.Point(788, 48);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(109, 15);
		this.label104.TabIndex = 200;
		this.label104.Text = "Total Finish Weight";
		this.label104.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotalFinishQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFinishQtyPm.Location = new System.Drawing.Point(899, 18);
		this.txtTotalFinishQtyPm.Name = "txtTotalFinishQtyPm";
		this.txtTotalFinishQtyPm.ReadOnly = true;
		this.txtTotalFinishQtyPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotalFinishQtyPm.TabIndex = 198;
		this.txtTotalFinishQtyPm.TabStop = false;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label105.Location = new System.Drawing.Point(788, 22);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(89, 15);
		this.label105.TabIndex = 201;
		this.label105.Text = "Total Finish Qty";
		this.label105.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotalFinishWeightPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFinishWeightPm.Location = new System.Drawing.Point(899, 44);
		this.txtTotalFinishWeightPm.Name = "txtTotalFinishWeightPm";
		this.txtTotalFinishWeightPm.ReadOnly = true;
		this.txtTotalFinishWeightPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotalFinishWeightPm.TabIndex = 199;
		this.txtTotalFinishWeightPm.TabStop = false;
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.Location = new System.Drawing.Point(348, 48);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(107, 15);
		this.label106.TabIndex = 196;
		this.label106.Text = "Total InPut Weight";
		this.label106.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotalInputQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalInputQtyPm.Location = new System.Drawing.Point(455, 18);
		this.txtTotalInputQtyPm.Name = "txtTotalInputQtyPm";
		this.txtTotalInputQtyPm.ReadOnly = true;
		this.txtTotalInputQtyPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotalInputQtyPm.TabIndex = 194;
		this.txtTotalInputQtyPm.TabStop = false;
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.Location = new System.Drawing.Point(348, 22);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(87, 15);
		this.label107.TabIndex = 197;
		this.label107.Text = "Total InPut Qty";
		this.label107.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotalInputWeightPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalInputWeightPm.Location = new System.Drawing.Point(455, 44);
		this.txtTotalInputWeightPm.Name = "txtTotalInputWeightPm";
		this.txtTotalInputWeightPm.ReadOnly = true;
		this.txtTotalInputWeightPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotalInputWeightPm.TabIndex = 195;
		this.txtTotalInputWeightPm.TabStop = false;
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.Location = new System.Drawing.Point(563, 48);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(116, 15);
		this.label108.TabIndex = 192;
		this.label108.Text = "Total OutPut Weight";
		this.label108.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotaloutputQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotaloutputQtyPm.Location = new System.Drawing.Point(680, 18);
		this.txtTotaloutputQtyPm.Name = "txtTotaloutputQtyPm";
		this.txtTotaloutputQtyPm.ReadOnly = true;
		this.txtTotaloutputQtyPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotaloutputQtyPm.TabIndex = 190;
		this.txtTotaloutputQtyPm.TabStop = false;
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.Location = new System.Drawing.Point(563, 22);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(96, 15);
		this.label109.TabIndex = 193;
		this.label109.Text = "Total OutPut Qty";
		this.label109.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtTotalOutPutWeightPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalOutPutWeightPm.Location = new System.Drawing.Point(680, 44);
		this.txtTotalOutPutWeightPm.Name = "txtTotalOutPutWeightPm";
		this.txtTotalOutPutWeightPm.ReadOnly = true;
		this.txtTotalOutPutWeightPm.Size = new System.Drawing.Size(106, 23);
		this.txtTotalOutPutWeightPm.TabIndex = 191;
		this.txtTotalOutPutWeightPm.TabStop = false;
		this.txtdocdatePackingMaterial.CustomFormat = "dd-MMM-yy";
		this.txtdocdatePackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdatePackingMaterial.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdatePackingMaterial.Location = new System.Drawing.Point(178, 18);
		this.txtdocdatePackingMaterial.Name = "txtdocdatePackingMaterial";
		this.txtdocdatePackingMaterial.Size = new System.Drawing.Size(166, 23);
		this.txtdocdatePackingMaterial.TabIndex = 1;
		this.txtdocdatePackingMaterial.ValueChanged += new System.EventHandler(txtdocdatePackingMaterial_ValueChanged);
		this.txtDocNoPackingMaterial.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtDocNoPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoPackingMaterial.Location = new System.Drawing.Point(93, 18);
		this.txtDocNoPackingMaterial.Name = "txtDocNoPackingMaterial";
		this.txtDocNoPackingMaterial.ReadOnly = true;
		this.txtDocNoPackingMaterial.Size = new System.Drawing.Size(84, 23);
		this.txtDocNoPackingMaterial.TabIndex = 0;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(5, 22);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(88, 15);
		this.label37.TabIndex = 49;
		this.label37.Text = "Doc No && Date";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(5, 48);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(78, 15);
		this.label35.TabIndex = 116;
		this.label35.Text = "Job Order No";
		this.cmbJobOrderPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Location = new System.Drawing.Point(93, 42);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Name = "cmbJobOrderPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Leave += new System.EventHandler(cmbJobOrderPackingMaterial_Leave);
		this.panel41.BackColor = System.Drawing.Color.Teal;
		this.panel41.Controls.Add(this.label96);
		this.panel41.Controls.Add(this.toolStrip4);
		this.panel41.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel41.Location = new System.Drawing.Point(0, 0);
		this.panel41.Name = "panel41";
		this.panel41.Size = new System.Drawing.Size(1285, 54);
		this.panel41.TabIndex = 0;
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label96.ForeColor = System.Drawing.Color.White;
		this.label96.Location = new System.Drawing.Point(3, 30);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(146, 19);
		this.label96.TabIndex = 179;
		this.label96.Text = "Packing Material";
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewpackingmaterial, this.btnRefreshPackingMaterial, this.btnsavepackingmaterial, this.btnUpdatepackingmaterial, this.toolStripSeparator3, this.btnPackingMaterialSlip, this.btnVoucherPM, this.btnGenerateRate });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1285, 27);
		this.toolStrip4.TabIndex = 58;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNewpackingmaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewpackingmaterial.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewpackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewpackingmaterial.Name = "btnNewpackingmaterial";
		this.btnNewpackingmaterial.Size = new System.Drawing.Size(59, 24);
		this.btnNewpackingmaterial.Text = "&New";
		this.btnNewpackingmaterial.Click += new System.EventHandler(btnNewpackingmaterial_Click);
		this.btnRefreshPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshPackingMaterial.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshPackingMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshPackingMaterial.Name = "btnRefreshPackingMaterial";
		this.btnRefreshPackingMaterial.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshPackingMaterial.Text = "Refresh";
		this.btnRefreshPackingMaterial.Click += new System.EventHandler(btnRefreshPackingMaterial_Click);
		this.btnsavepackingmaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsavepackingmaterial.Image = (System.Drawing.Image)resources.GetObject("btnsavepackingmaterial.Image");
		this.btnsavepackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsavepackingmaterial.Name = "btnsavepackingmaterial";
		this.btnsavepackingmaterial.Size = new System.Drawing.Size(64, 24);
		this.btnsavepackingmaterial.Text = "&Save";
		this.btnsavepackingmaterial.Click += new System.EventHandler(btnsavepackingmaterial_Click);
		this.btnUpdatepackingmaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatepackingmaterial.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdatepackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdatepackingmaterial.Name = "btnUpdatepackingmaterial";
		this.btnUpdatepackingmaterial.Size = new System.Drawing.Size(78, 24);
		this.btnUpdatepackingmaterial.Text = "&Update";
		this.btnUpdatepackingmaterial.Visible = false;
		this.btnUpdatepackingmaterial.Click += new System.EventHandler(btnUpdatepackingmaterial_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnPackingMaterialSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingMaterialSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPackingMaterialSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPackingMaterialSlip.Name = "btnPackingMaterialSlip";
		this.btnPackingMaterialSlip.Size = new System.Drawing.Size(92, 24);
		this.btnPackingMaterialSlip.Text = "&Print-606";
		this.btnPackingMaterialSlip.Click += new System.EventHandler(btnPackingMaterialSlip_Click);
		this.btnVoucherPM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherPM.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherPM.Name = "btnVoucherPM";
		this.btnVoucherPM.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherPM.Text = "118-Voucher";
		this.btnVoucherPM.Click += new System.EventHandler(btnVoucherPM_Click);
		this.btnGenerateRate.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.btnGenerateRate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRate.Name = "btnGenerateRate";
		this.btnGenerateRate.Size = new System.Drawing.Size(126, 24);
		this.btnGenerateRate.Text = "Generate Rate";
		this.btnGenerateRate.Click += new System.EventHandler(btnGenerateRate_Click);
		this.PmHistory.Controls.Add(this.panel70);
		this.PmHistory.Controls.Add(this.toolStrip2);
		this.PmHistory.Location = new System.Drawing.Point(4, 4);
		this.PmHistory.Name = "PmHistory";
		this.PmHistory.Padding = new System.Windows.Forms.Padding(3);
		this.PmHistory.Size = new System.Drawing.Size(1291, 658);
		this.PmHistory.TabIndex = 1;
		this.PmHistory.Text = "History";
		this.PmHistory.UseVisualStyleBackColor = true;
		this.panel70.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory);
		this.panel70.Controls.Add(this.panel73);
		this.panel70.Controls.Add(this.panel15);
		this.panel70.Controls.Add(this.panel71);
		this.panel70.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailHistoryPM);
		this.panel70.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel70.Location = new System.Drawing.Point(3, 30);
		this.panel70.Name = "panel70";
		this.panel70.Size = new System.Drawing.Size(1285, 625);
		this.panel70.TabIndex = 3;
		this.grdPackingMaterialHistory.AllowEdit = (InheritableBoolean)2;
		grdPackingMaterialHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialHistory.DesignTimeLayout = grdPackingMaterialHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterialHistory.DynamicFiltering = true;
		this.grdPackingMaterialHistory.FilterMode = (FilterMode)2;
		this.grdPackingMaterialHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Location = new System.Drawing.Point(0, 95);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Name = "grdPackingMaterialHistory";
		((ScrollableBase)this.grdPackingMaterialHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Size = new System.Drawing.Size(1285, 295);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).TabIndex = 0;
		this.grdPackingMaterialHistory.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialHistory).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialHistory.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterialHistory_ColumnButtonClick);
		this.grdPackingMaterialHistory.SelectionChanged += new System.EventHandler(grdPackingMaterialHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).DoubleClick += new System.EventHandler(grdPackingMaterial_DoubleClick);
		this.panel73.BackColor = System.Drawing.Color.Teal;
		this.panel73.Controls.Add(this.label111);
		this.panel73.Controls.Add(this.label110);
		this.panel73.Controls.Add(this.ctrlGrdBar15);
		this.panel73.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel73.Location = new System.Drawing.Point(0, 390);
		this.panel73.Name = "panel73";
		this.panel73.Size = new System.Drawing.Size(1285, 26);
		this.panel73.TabIndex = 3;
		this.panel73.Visible = false;
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label111.ForeColor = System.Drawing.Color.White;
		this.label111.Location = new System.Drawing.Point(231, 4);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(125, 17);
		this.label111.TabIndex = 177;
		this.label111.Text = "(Job Order Wise)";
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label110.ForeColor = System.Drawing.Color.White;
		this.label110.Location = new System.Drawing.Point(4, 3);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(228, 17);
		this.label110.TabIndex = 176;
		this.label110.Text = "Detail of the above selected row";
		this.ctrlGrdBar15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar15.FormName = this;
		this.ctrlGrdBar15.Location = new System.Drawing.Point(1571, 2);
		this.ctrlGrdBar15.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar15.MyGrid = this.grdPackingMaterialHistory;
		this.ctrlGrdBar15.Name = "ctrlGrdBar15";
		this.ctrlGrdBar15.Size = new System.Drawing.Size(80, 39);
		this.ctrlGrdBar15.TabIndex = 175;
		this.panel15.Controls.Add(this.groupBox3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 27);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1285, 68);
		this.panel15.TabIndex = 47;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, -1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(973, 66);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(863, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 146;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(863, 22);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 145;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(782, 22);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 143;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(782, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 144;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(721, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(479, 19);
		this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(59, 15);
		this.label49.TabIndex = 142;
		this.label49.Text = "Job Order";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(378, 19);
		this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 141;
		this.label50.Text = "To Doc No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(276, 19);
		this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(79, 15);
		this.label51.TabIndex = 140;
		this.label51.Text = "From Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(143, 19);
		this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 139;
		this.label52.Text = "To Date";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(5, 20);
		this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(63, 15);
		this.label53.TabIndex = 13;
		this.label53.Text = "From Date";
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(378, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(276, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel71.BackColor = System.Drawing.Color.Teal;
		this.panel71.Controls.Add(this.chkPendingForRates);
		this.panel71.Controls.Add(this.label88);
		this.panel71.Controls.Add(this.ctrlGrdBar2);
		this.panel71.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel71.Location = new System.Drawing.Point(0, 0);
		this.panel71.Name = "panel71";
		this.panel71.Size = new System.Drawing.Size(1285, 27);
		this.panel71.TabIndex = 2;
		this.chkPendingForRates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPendingForRates.AutoSize = true;
		this.chkPendingForRates.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPendingForRates.ForeColor = System.Drawing.Color.White;
		this.chkPendingForRates.Location = new System.Drawing.Point(1003, 5);
		this.chkPendingForRates.Name = "chkPendingForRates";
		this.chkPendingForRates.Size = new System.Drawing.Size(231, 20);
		this.chkPendingForRates.TabIndex = 177;
		this.chkPendingForRates.Text = "Show Only Zero-Rated Entry";
		this.chkPendingForRates.UseVisualStyleBackColor = true;
		this.chkPendingForRates.CheckedChanged += new System.EventHandler(chkPendingForRates_CheckedChanged);
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label88.ForeColor = System.Drawing.Color.White;
		this.label88.Location = new System.Drawing.Point(2, 3);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(209, 19);
		this.label88.TabIndex = 176;
		this.label88.Text = "Packing Material History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1243, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdPackingMaterialHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 175;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grddetailHistoryPM.AllowEdit = (InheritableBoolean)2;
		grddetailHistoryPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailHistoryPM.DesignTimeLayout = grddetailHistoryPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grddetailHistoryPM.DynamicFiltering = true;
		this.grddetailHistoryPM.FilterMode = (FilterMode)2;
		this.grddetailHistoryPM.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailHistoryPM.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Location = new System.Drawing.Point(0, 416);
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Name = "grddetailHistoryPM";
		((ScrollableBase)this.grddetailHistoryPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Size = new System.Drawing.Size(1285, 209);
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).TabIndex = 4;
		this.grddetailHistoryPM.TotalRow = (InheritableBoolean)1;
		this.grddetailHistoryPM.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grddetailHistoryPM).Visible = false;
		((ScrollableBase)this.grddetailHistoryPM).VisualStyle = (VisualStyle)4;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1285, 27);
		this.toolStrip2.TabIndex = 25;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.BackColor = System.Drawing.Color.White;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(148, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1356, 27);
		this.miniToolStrip.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1299, 687);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionPackingMaterial";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlPm.ResumeLayout(false);
		this.PmForm.ResumeLayout(false);
		this.panel66.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailPackingMaterial).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel42.ResumeLayout(false);
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).EndInit();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPackingMaterial).EndInit();
		this.panel41.ResumeLayout(false);
		this.panel41.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.PmHistory.ResumeLayout(false);
		this.PmHistory.PerformLayout();
		this.panel70.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).EndInit();
		this.panel73.ResumeLayout(false);
		this.panel73.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		this.panel71.ResumeLayout(false);
		this.panel71.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailHistoryPM).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
