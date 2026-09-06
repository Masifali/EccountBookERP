using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.FixedAssets;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FixedAssets;
using Architecture.Model.Main;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
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
namespace Architecture.WinApp.StoreManagement
{

public class StoreReturn : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrAccount = new DataTable();

	private DataTable dtCurrentItemuom = new DataTable();

	private int RECID;

	private int DocumentTypeId = 140;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private bool StoreFinancialEffects;

	private bool FormInitialized;

	private bool ItemSearchByNameOrCode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private static bool CalculateByPerItemSecondaryQty = true;

	private static bool CalculateBySecondaryUomQty = false;

	private static bool CalculateBySecondaryUomRate = true;

	private static bool CalculateByItemRate = false;

	private bool _isUpdatingRates;

	private bool _isUpdatingSecondary;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControlGsIssuance;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Label label15;

	private DateTimePicker txtDocdate;

	private Label label2;

	private Label label1;

	private TextBox txtDocNo;

	internal GridEX grdDetail;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbAssetsRef;

	private Label label11;

	internal UltraCombo cmbUOM;

	internal UltraCombo cmbItem;

	private Label label44;

	private Label label9;

	private TextBox txtItemQty;

	private Label label18;

	private Label label7;

	private Label label6;

	internal UltraCombo CmbDepartment;

	internal UltraCombo CmbWarehouse;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button Add;

	private Label label13;

	private TextBox txtRemarksDetail;

	private TextBox txtItemAmount;

	private Label label12;

	private TextBox txtItemRate;

	private Label label10;

	private ToolStripButton btnVoucher;

	internal UltraCombo cmbCrAc;

	private Label labelCrAc;

	private ToolStripButton btnAttachment;

	private CheckBox ChkPrintPreview;

	private RadioButton rdSearchByName;

	private RadioButton rdSearchByCode;

	private GroupBox groupBox1;

	private GroupBox DetailGBox;

	internal GridEX gridHistory;

	private Panel panel8;

	internal GridEX DataGridHistory;

	private Panel panel7;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private Label label14;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label8;

	private Panel panel9;

	private Label label17;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel10;

	private GroupBox groupBox3;

	private Button btnShowHistory;

	private Label label55;

	private Label label56;

	private Label label57;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label26;

	internal UltraCombo CmbWarehouseHistory;

	private Label label16;

	internal UltraCombo CmbDepartmentHistory;

	private Label label5;

	internal UltraCombo CmbItemNameHistory;

	private Label label19;

	internal UltraCombo CmbAssetNameHistory;

	private Label label20;

	internal UltraCombo CmbCreditAccountHistory;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	internal UltraCombo CmbItemCondition;

	private Label label25;

	private Label label21;

	private TextBox txtIssuanceRate;

	private ToolStripButton btnLoadIssuance;

	private Label label3;

	private TextBox txtRemarks;

	internal UltraCombo CmbRackName;

	private Label label4;

	internal UltraCombo CmbSecondaryUom;

	private Label label23;

	private Label label22;

	private TextBox txtSecondaryUomQty;

	private Label label24;

	private TextBox txtPerItemWeight;

	private Label label27;

	private TextBox txtSecondaryUomRate;

	public StoreReturn(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void storeReturn_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(10, 10);
		base.AutoScrollMinSize = new Size(1150, 800);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			table.Columns.Add("IssuanceId", typeof(int));
			table.Columns.Add("IssuanceDetailId", typeof(int));
			table.Columns.Add("IssuanceNo", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouseName", typeof(string));
			table.Columns.Add("RackId", typeof(int));
			table.Columns.Add("RackName", typeof(string));
			table.Columns.Add("ItemConditionId", typeof(int));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom", typeof(string));
			table.Columns.Add("BalanceQty", typeof(double));
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("SecondaryUomId", typeof(int));
			table.Columns.Add("SecondaryUom", typeof(string));
			table.Columns.Add("SecondaryUomQty", typeof(double));
			table.Columns.Add("PerItemWeight", typeof(double));
			table.Columns.Add("SecondaryUomItemRate", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("IssuanceRate", typeof(double));
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("DepartmentId", typeof(int));
			table.Columns.Add("AssetId", typeof(int));
			table.Columns.Add("Remarks", typeof(string));
			table.Columns.Add("CrAccountId", typeof(int));
			table.Columns.Add("CreditAccount", typeof(string));
			table.Columns.Add("RecordNo", typeof(int));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			string documentNoData = GenerateDocNoDBCall();
			ItemdtFillForReturntoStoreDbCall();
			DataTable departmentData = DepartmentDbCall();
			DataTable assetsData = FixedAssetsDbCall();
			DataTable historyComboBindData = HistoryComboBindDbCall();
			AllCreditAccountsdtFillDbCall();
			return new
			{
				documentNoData = documentNoData,
				DepartmentData = departmentData,
				AssetsData = assetsData,
				HistoryComboBindData = historyComboBindData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else if (!base.IsDisposed)
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				Print.Enabled = formrights.DoHavePrintRights;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				ChkPrintPreview.Checked = formrights.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formrights);
				GetConfigurationsFromGlobal();
				if (ItemSearchByNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				if (RECID == 0)
				{
					txtDocNo.Text = result.documentNoData;
				}
				ItemNameBind();
				CommonBindings.ItemConditionBindFromGlobal(new DataTable(), CmbItemCondition);
				DepartmentBind(result.DepartmentData);
				FixedAssetsBind(result.AssetsData);
				BindGrids();
				HistoryComboBind(result.HistoryComboBindData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				((UltraGridBase)CmbItemCondition).Rows[4].Activate();
				FormInitialized = true;
				txtDocdate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			StoreFinancialEffects = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("StoreIssuanceFinancialEffect"));
			ItemSearchByNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			if (StoreFinancialEffects)
			{
				labelCrAc.Visible = true;
				((Control)(object)cmbCrAc).Visible = true;
				txtRemarksDetail.Size = new Size(390, 23);
			}
			else
			{
				labelCrAc.Visible = false;
				((Control)(object)cmbCrAc).Visible = false;
				txtRemarksDetail.Size = new Size(545, 23);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private string GenerateDocNoDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvStoreReturn.GenerateCode(new InvStoreReturn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemdtFillForReturntoStoreDbCall()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		try
		{
			DataTable dataTable = dtItem;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtItem.Columns.Add("Id", typeof(int));
				dtItem.Columns.Add("ItemName", typeof(string));
				dtItem.Columns.Add("ItemCode", typeof(string));
				dtItem.Columns.Add("ParentCategoryId", typeof(string));
			}
			dtItem.Clear();
			DataTable dtitem = InvGsStoreIssuanceHeader.GetItemsForReturntoStore(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtitem == null || dtitem.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow sourceRow in dtitem.Rows)
			{
				DataRow newRow = dtItem.NewRow();
				newRow["Id"] = sourceRow["Id"];
				newRow["ItemName"] = sourceRow["ItemName"];
				newRow["ItemCode"] = sourceRow["ItemCodeNew"];
				newRow["ParentCategoryId"] = sourceRow["ParentCategoryId"];
				dtItem.Rows.Add(newRow);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			cmbItem_Leave(null, null);
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
			int ItemId = Conversion.ToInt(cmbItem.Value);
			CreditAcDetailBind();
			CommonBindings.ItemUomFromGlobalBind(ItemId, cmbUOM, null, CmbSecondaryUom);
			GetLastIssuanceRateByItemId();
			txtItemRate.Text = GetAvgRateByDocDateItemAndCondition(ItemId, txtDocdate.Value, Conversion.ToInt(CmbItemCondition.Value)).ToString("#,##0.###");
			BindWarehouseDropdown(CmbWarehouse, ItemId);
			BindRacks(ItemId, Conversion.ToInt(CmbWarehouse.Value));
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
		InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Warehouse", ((Control)(object)combo).Tag?.ToString(), AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
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
			int parentCategoryId = 0;
			UltraCombo obj = cmbItem;
			if (((obj != null) ? ((UltraDropDownBase)obj).SelectedRow : null) != null)
			{
				parentCategoryId = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells[3].Value);
			}
			int configWarehouseId = 0;
			switch (parentCategoryId)
			{
			case 7:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
				break;
			case 8:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
				break;
			}
			if (configWarehouseId != 0 && dt.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["Id"]) == configWarehouseId))
			{
				combo.Value = configWarehouseId;
			}
		}
	}

	private void BindRacks(int itemId, int warehouseId)
	{
		try
		{
			List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
				where x.ItemId == itemId && (warehouseId == 0 || x.WarehouseId == warehouseId)
				group x by x.Id into g
				select g.First()).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("WarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Columns.Add("BaseRackId", typeof(int));
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem r in list)
			{
				dt.Rows.Add(r.Id, r.RackName, r.WarehouseId, r.WareHouseName, r.BaseRackId);
			}
			dt.EndLoadData();
			UltraCombo combo = CmbRackName;
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "RackName", "Rack", AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns["WarehouseId"].Hidden = true;
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns["BaseRackId"].Hidden = true;
			}
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
				DataRow baseRackRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["BaseRackId"]) > 0);
				if (baseRackRow != null)
				{
					combo.Value = baseRackRow["Id"];
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
			int currentWarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			RacksWithWarehouseAndItem rack = clsGlobalVariables.racksWithWarehouseAndItems.FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null && (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId))
			{
				DataTable dtW = (DataTable)((UltraGridBase)CmbWarehouse).DataSource;
				if (dtW != null && dtW.Rows.Count > 0)
				{
					CmbWarehouse.Value = rack.WarehouseId;
				}
			}
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
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable DepartmentDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return Department.GetAll(new Department
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

	private void DepartmentBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDepartment, dt, "Id", "DepartmentName", "Department Name", AllColumns: true, Conversion.ToInt(CmbDepartment.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable FixedAssetsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return FixedAssetsRegister.GetAll(new FixedAssetsRegister
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

	private void FixedAssetsBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbAssetsRef, dt, "Id", "AssetName", "Fixed Asset Name", AllColumns: true, Conversion.ToInt(cmbAssetsRef.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCreditAccountsdtFillDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtCrAccount = InvGsStoreIssuanceHeader.GetDebitAccountForStoreReturn(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditAcDetailBind()
	{
		try
		{
			EnumerableRowCollection<DataRow> rows = from r in dtCrAccount.AsEnumerable()
				where Conversion.ToInt(r["ItemId"]) == Conversion.ToInt(cmbItem.Value)
				select r;
			DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtCrAccount.Clone());
			InfragisticsHelper.BindAndRetainSelection(cmbCrAc, dt, "Id", "AccountTitle", "Credit Account", AllColumns: false, Conversion.ToInt(cmbCrAc.Value), insertDefaultRow: true);
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RECID, 140);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReset();
	}

	private void formReset()
	{
		try
		{
			AT = new Attachment();
			RECID = 0;
			VoucherHeadId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			DetailGBox.Visible = true;
			panel5.Height = 204;
			txtRemarks.Text = string.Empty;
			_isUpdatingRates = false;
			_isUpdatingSecondary = false;
			table.Rows.Clear();
			BindGrids();
			ResetDetail();
			GetConfigurationsFromGlobal();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			txtDocNo.Text = GenerateDocNoDBCall();
			txtDocNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			((UltraGridBase)CmbWarehouse).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			((Control)(object)CmbSecondaryUom).Text = string.Empty;
			txtSecondaryUomQty.Text = string.Empty;
			txtPerItemWeight.Text = string.Empty;
			txtSecondaryUomRate.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)cmbCrAc).Text = string.Empty;
			cmbItem.Focus();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			List<string> allServices = new List<string> { "WarehousesWithBranches", "RacksWithWarehouseAndItem", "ItemConditions", "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			ItemdtFillForReturntoStoreDbCall();
			ItemNameBind();
			CommonBindings.ItemConditionBindFromGlobal(new DataTable(), CmbItemCondition);
			DepartmentBind(DepartmentDbCall());
			FixedAssetsBind(FixedAssetsDbCall());
			AllCreditAccountsdtFillDbCall();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			if (grdDetail.RowCount > 0)
			{
				grdDetail.RootTable.Columns["CreditAccount"].Visible = StoreFinancialEffects;
			}
			if (gridHistory.RowCount > 0)
			{
				gridHistory.RootTable.Columns["CreditAccount"].Visible = StoreFinancialEffects;
			}
			GridComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StoreReturn_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BindGrids()
	{
		try
		{
			if (table != null && table.Columns.Count > 0)
			{
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				DetailGridSettings(grdDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailRow(DataRow row)
	{
		row["ItemId"] = Conversion.ToInt(cmbItem.Value);
		UltraCombo obj = cmbItem;
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)obj).SelectedRow;
			obj2 = ((selectedRow != null) ? selectedRow.Cells["ItemCode"].Value : null);
		}
		row["ItemCode"] = Conversion.ToString(obj2);
		UltraCombo obj3 = cmbItem;
		object obj4;
		if (obj3 == null)
		{
			obj4 = null;
		}
		else
		{
			UltraGridRow selectedRow2 = ((UltraDropDownBase)obj3).SelectedRow;
			obj4 = ((selectedRow2 != null) ? selectedRow2.Cells["ItemName"].Value : null);
		}
		row["ItemName"] = Conversion.ToString(obj4);
		row["WareHouseId"] = Conversion.ToInt(CmbWarehouse.Value);
		row["WareHouseName"] = Conversion.ToString((object)((Control)(object)CmbWarehouse).Text);
		row["RackId"] = Conversion.ToInt(CmbRackName.Value);
		row["RackName"] = Conversion.ToString((object)((Control)(object)CmbRackName).Text);
		row["ItemConditionId"] = Conversion.ToInt(CmbItemCondition.Value);
		double qty = Conversion.ToDouble((object)txtItemQty.Text.Trim());
		row["PackUomId"] = Conversion.ToInt(cmbUOM.Value);
		row["PackUom"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text.Trim());
		row["ItemQty"] = qty;
		row["BalanceQty"] = qty;
		row["SecondaryUomId"] = Conversion.ToInt(CmbSecondaryUom.Value);
		row["SecondaryUom"] = Conversion.ToString((object)((Control)(object)CmbSecondaryUom).Text);
		row["SecondaryUomQty"] = Conversion.ToDouble((object)txtSecondaryUomQty.Text.Trim());
		row["PerItemWeight"] = Conversion.ToDouble((object)txtPerItemWeight.Text.Trim());
		row["SecondaryUomItemRate"] = Conversion.ToDouble((object)txtSecondaryUomRate.Text.Trim());
		double itemRate = Conversion.ToDouble((object)txtItemRate.Text.Trim());
		double issuanceRate = Conversion.ToDouble((object)txtIssuanceRate.Text.Trim());
		row["ItemRate"] = itemRate;
		row["IssuanceRate"] = issuanceRate;
		row["ItemAmount"] = Conversion.ToDouble((object)txtItemAmount.Text.Trim());
		row["DepartmentId"] = Conversion.ToInt(CmbDepartment.Value);
		row["AssetId"] = Conversion.ToInt(cmbAssetsRef.Value);
		row["CrAccountId"] = Conversion.ToInt(cmbCrAc.Value);
		row["CreditAccount"] = Conversion.ToString((object)((Control)(object)cmbCrAc).Text);
		row["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text.Trim());
		row["RecordNo"] = 0;
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				DataRow newRow = table.NewRow();
				newRow["IssuanceId"] = Conversion.ToInt((object)0);
				newRow["IssuanceDetailId"] = Conversion.ToInt((object)0);
				newRow["IssuanceNo"] = Conversion.ToInt((object)0);
				FillDetailRow(newRow);
				table.Rows.Add(newRow);
				BindGrids();
				ResetDetail();
			}
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
			if (FormValidationDetail())
			{
				DataRow row = table.Rows[updateDetailIndex];
				FillDetailRow(row);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && Conversion.ToInt(item.Cells["IssuanceId"].Value) <= 0)
			{
				updateDetailIndex = item.RowIndex;
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				cmbItem_Leave(null, null);
				CmbWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbRackName.Value = item.Cells["RackId"].Value.ToString();
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				cmbUOM.Value = item.Cells["PackUomId"].Value.ToString();
				txtItemQty.Text = item.Cells["ItemQty"].Text.ToString();
				CmbSecondaryUom.Value = item.Cells["SecondaryUomId"].Value.ToString();
				txtSecondaryUomQty.Text = item.Cells["SecondaryUomQty"].Text.ToString();
				txtPerItemWeight.Text = item.Cells["PerItemWeight"].Text.ToString();
				txtSecondaryUomRate.Text = item.Cells["SecondaryUomItemRate"].Text.ToString();
				txtIssuanceRate.Text = Conversion.ToDouble(item.Cells["IssuanceRate"].Value).ToString("#,##0.###");
				txtItemRate.Text = item.Cells["ItemRate"].Text.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Text.ToString();
				CmbDepartment.Value = item.Cells["DepartmentId"].Value.ToString();
				cmbAssetsRef.Value = item.Cells["AssetId"].Value.ToString();
				cmbCrAc.Value = item.Cells["CrAccountId"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
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

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void GridComboBind()
	{
		try
		{
			DataTable dtCondition = (DataTable)((UltraGridBase)CmbItemCondition).DataSource;
			if (dtCondition != null && dtCondition.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grdDetail.RootTable.Columns["ItemConditionId"], dtCondition, "Id", "ItemCondition", "Item Condition", Constants.InventoryConstants.StringConstant, limitToList: true, editType: false);
			}
			DataTable dtDept = (DataTable)((UltraGridBase)CmbDepartment).DataSource;
			if (dtDept != null && dtDept.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grdDetail.RootTable.Columns["DepartmentId"], dtDept, "Id", "DepartmentName", "Department Name", Constants.InventoryConstants.WareHouseName, limitToList: true, editType: false);
			}
			DataTable dtAsset = (DataTable)((UltraGridBase)cmbAssetsRef).DataSource;
			if (dtAsset != null && dtAsset.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grdDetail.RootTable.Columns["AssetId"], dtAsset, "Id", "AssetName", "Asset Name", Constants.InventoryConstants.StringConstant, limitToList: true, editType: false);
			}
			GridItemUomComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridItemUomComboBind()
	{
	}

	private void DetailGridSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "IssuanceId", "IssuanceDetailId", "ItemId", "WareHouseId", "RackId", "PackUomId", "SecondaryUomId", "CrAccountId" };
			DataTable dt = (DataTable)gridEX.DataSource;
			bool IfAnyRowHasIssuance = false;
			if (dt != null && dt.Rows.Count > 0)
			{
				IfAnyRowHasIssuance = dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["IssuanceId"]) > 0);
				GridEX_Helper.SetColumnVisible(gridEX, "IssuanceNo", IfAnyRowHasIssuance);
				GridEX_Helper.SetColumnVisible(gridEX, "RecordNo", IfAnyRowHasIssuance);
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ItemConditionId",
					(EditType)4
				},
				{
					"ItemQty",
					(EditType)1
				},
				{
					"SecondaryUomQty",
					(EditType)1
				},
				{
					"PerItemWeight",
					(EditType)1
				},
				{
					"SecondaryUomItemRate",
					(EditType)1
				},
				{
					"ItemRate",
					(EditType)1
				},
				{
					"IssuanceRate",
					(EditType)1
				},
				{
					"DepartmentId",
					(EditType)4
				},
				{
					"AssetId",
					(EditType)4
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			if (((Control)(object)gridEX).Name == "grdDetail")
			{
				GridComboBind();
				GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				if (IfAnyRowHasIssuance)
				{
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					gridEX.FrozenColumns = 2;
				}
				else
				{
					gridEX.FrozenColumns = 1;
				}
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns);
			}
			gridEX.RootTable.Columns["ItemQty"].Caption = "Received Qty";
			gridEX.RootTable.Columns["SecondaryUomQty"].Caption = "Secondary Uom Weight";
			gridEX.RootTable.Columns["ItemAmount"].Caption = "Total Amount";
			GridEX_Helper.SetColumnWidth(gridEX, "IssuanceNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemCode", Constants.InventoryConstants.ItemCode);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(gridEX, "WareHouseName", Constants.InventoryConstants.WareHouseName);
			GridEX_Helper.SetColumnWidth(gridEX, "RackName", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemConditionId", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "PackUom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "BalanceQty", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemQty", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUom", Constants.InventoryConstants.UomConstant + 20);
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUomQty", Constants.InventoryConstants.WeightConstant + 10);
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUomItemRate", Constants.InventoryConstants.WeightConstant + 20);
			GridEX_Helper.SetColumnWidth(gridEX, "IssuanceRate", Constants.InventoryConstants.ExRateConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemRate", Constants.InventoryConstants.ExRateConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "DepartmentId", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "AssetId", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "CreditAccount", Constants.InventoryConstants.AccountTitle);
			GridEX_Helper.SetColumnWidth(gridEX, "Remarks", Constants.InventoryConstants.RemarksConstants);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdDetail.CurrentRow != null && (int)grdDetail.CurrentRow.RowType == 0)
			{
				GridEXRow r = grdDetail.CurrentRow;
				DataRow dr = table.Rows[r.RowIndex];
				int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int issuanceId = Conversion.ToInt(r.Cells["IssuanceId"].Value);
				int conditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				int recordNo = Conversion.ToInt(r.Cells["RecordNo"].Value);
				double balanceQty = Conversion.ToDouble(r.Cells["BalanceQty"].Value);
				double qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
				double rate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
				double perItemWeight = Conversion.ToDouble(r.Cells["PerItemWeight"].Value);
				double secondaryQty = Conversion.ToDouble(r.Cells["SecondaryUomQty"].Value);
				double secondaryRate = Conversion.ToDouble(r.Cells["SecondaryUomItemRate"].Value);
				if (e.Column.Key == "ItemQty" && issuanceId > 0)
				{
					ValidateQuantity(r, dr, recordNo, balanceQty);
					qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					rate = ((qty > 0.0) ? (secondaryRate * secondaryQty / qty) : 0.0);
					r.Cells["ItemRate"].Value = rate;
				}
				if (e.Column.Key == "ItemConditionId")
				{
					rate = Conversion.ToDouble((object)GetAvgRateByDocDateItemAndCondition(itemId, txtDocdate.Value, conditionId));
					r.Cells["ItemRate"].Value = rate;
				}
				if (e.Column.Key == "SecondaryUomQty")
				{
					perItemWeight = ((qty > 0.0) ? (secondaryQty / qty) : 0.0);
					r.Cells["PerItemWeight"].Value = perItemWeight;
				}
				if (e.Column.Key == "PerItemWeight")
				{
					secondaryQty = perItemWeight * qty;
					r.Cells["SecondaryUomQty"].Value = secondaryQty;
				}
				if (e.Column.Key == "SecondaryUomItemRate")
				{
					rate = ((qty > 0.0) ? (secondaryRate * secondaryQty / qty) : 0.0);
					r.Cells["ItemRate"].Value = rate;
				}
				if (e.Column.Key == "ItemRate")
				{
					secondaryRate = ((secondaryQty > 0.0) ? (rate * (qty / secondaryQty)) : 0.0);
					r.Cells["SecondaryUomItemRate"].Value = secondaryRate;
				}
				RecalculateRow(r);
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidateQuantity(GridEXRow r, DataRow dr, int recordNo, double balanceQty)
	{
		if (btnsave.Visible && btnsave.Enabled)
		{
			double enteredQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
			double otherRowsQty = (from row in table.AsEnumerable()
				where Conversion.ToInt(row["RecordNo"]) == recordNo && row != dr
				select row).Sum((DataRow row) => Conversion.ToDouble(row["ItemQty"]));
			double maxAllowed = balanceQty - otherRowsQty;
			if (enteredQty <= 0.0)
			{
				r.Cells["ItemQty"].Value = maxAllowed;
				MessageBox.Show($"Qty must be greater than 0. Reset to max allowed: {maxAllowed}", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (enteredQty > maxAllowed)
			{
				r.Cells["ItemQty"].Value = maxAllowed;
				MessageBox.Show($"Qty exceeds allowed balance for RecordNo {recordNo}.\n" + $"Already used: {otherRowsQty}, Max allowed: {maxAllowed}.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}

	private void RecalculateRow(GridEXRow r)
	{
		double qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		double rate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
		r.Cells["ItemAmount"].Value = qty * rate;
	}

	private void AddDetailrow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			DataRow dr = table.Rows[r.RowIndex];
			double BalQty = Conversion.ToDouble(dr["BalanceQty"]);
			double ItemQty = Conversion.ToDouble(dr["ItemQty"]);
			double QtyTobeAdded = 0.0;
			if (BalQty == ItemQty)
			{
				MessageBox.Show("Current Row can't be breakable. First Break The Qty of Row", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (table.AsEnumerable().Count((DataRow row) => row.Field<int>("RecordNo") == Conversion.ToInt(dr["RecordNo"])) == 1)
			{
				QtyTobeAdded = BalQty - ItemQty;
			}
			else
			{
				double totalQty = (from row in table.AsEnumerable()
					where row.Field<int>("RecordNo") == Conversion.ToInt(dr["RecordNo"])
					select row).Sum((DataRow row) => row.Field<double>("ItemQty"));
				if (totalQty > BalQty)
				{
					throw new Exception("Total Qty Of Record#" + dr["RecordNo"]?.ToString() + " can't be greater than balance Qty Which Is " + BalQty);
				}
				QtyTobeAdded = BalQty - totalQty;
				if (QtyTobeAdded == 0.0)
				{
					MessageBox.Show("Current Row can't be breakable. First Break The Qty of any Row of RecordNo" + dr["RecordNo"], "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			table.Rows.Add(dr.ItemArray);
			grdDetail.DataSource = table;
			double rate = Conversion.ToDouble(table.Rows[table.Rows.Count - 1]["ItemRate"]);
			table.Rows[table.Rows.Count - 1]["ItemQty"] = QtyTobeAdded;
			table.Rows[table.Rows.Count - 1]["ItemAmount"] = QtyTobeAdded * rate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				else if (e.Column.Key == "Add")
				{
					AddDetailrow(r);
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdDetail.CurrentColumn.Key == "Add")
				{
					AddDetailrow(r);
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddDetailrow(r);
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
				grdDetail.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			int WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
			if (grdDetail.CurrentColumn.Key == "CrAccountId" || grdDetail.CurrentColumn.Key == "CreditAccount")
			{
				EnumerableRowCollection<DataRow> rows = from rr in dtCrAccount.AsEnumerable()
					where Conversion.ToInt(rr["ItemId"]) == Conversion.ToInt(r.Cells["ItemId"].Value)
					select rr;
				GrdPopUp grdPopUp = new GrdPopUp(rows.Any() ? rows.CopyToDataTable() : dtCrAccount.Clone(), "Id", "AccountTitle");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				NameCode = Conversion.ToString((object)grdPopUp.ReturnName);
				r.Cells["CrAccountId"].Value = Id;
				r.Cells["CreditAccount"].Value = NameCode;
				grdDetail.UpdateData();
			}
			if (grdDetail.CurrentColumn.Key == "WareHouseId" || grdDetail.CurrentColumn.Key == "WareHouseName")
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
					r.Cells["WareHouseName"].Value = selectedName;
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
				grdDetail.UpdateData();
			}
			else if (grdDetail.CurrentColumn.Key == "RackId" || grdDetail.CurrentColumn.Key == "RackName")
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
				using (GrdPopUp grdPopUp2 = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
				{
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					int selectedId = Conversion.ToInt((object)grdPopUp2.ReturnId);
					string selectedName2 = Conversion.ToString((object)grdPopUp2.ReturnName);
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
				grdDetail.UpdateData();
			}
			if (grdDetail.CurrentColumn.Key == "SecondaryUomId" || grdDetail.CurrentColumn.Key == "SecondaryUom")
			{
				GrdPopUp grdPopUp3 = new GrdPopUp(CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId), "Id", "UOMCode", "Equivalent");
				grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp3.ShowDialog();
				grdPopUp3.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
				NameCode = Conversion.ToString((object)grdPopUp3.ReturnName);
				r.Cells["SecondaryUomId"].Value = Id;
				r.Cells["SecondaryUom"].Value = NameCode;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_SelectionChanged(object sender, EventArgs e)
	{
		GridItemUomComboBind();
	}

	private void grdDetail_Click(object sender, EventArgs e)
	{
		GridItemUomComboBind();
	}

	private void Insert()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			InvStoreReturn obj = new InvStoreReturn();
			obj.Id = RECID;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			obj.DocumentTypeId = 140;
			obj.Remarks = txtRemarks.Text;
			obj.EntryDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUser = UserAccount.ID;
			obj.ApprovedDate = DateTime.Now;
			obj.ApprovedUserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.IsApproved = false;
			obj.StoreRerurnDetails = new List<InvStoreReturnDetail>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ItemQty"].Value) <= 0)
				{
					continue;
				}
				InvStoreReturnDetail detail = new InvStoreReturnDetail();
				detail.LineId = r.RowIndex + 1;
				FillDetailListCommonForInsertAndDelete(detail, r);
				FormHelper.ValidateField(detail.ItemId, "Item Name", r.RowIndex);
				FormHelper.ValidateField(detail.Warehouseid, "Warehouse", r.RowIndex);
				FormHelper.ValidateField(detail.RackId, "Rack Name", r.RowIndex);
				FormHelper.ValidateField(detail.ItemCondition, "Item Condition", r.RowIndex);
				FormHelper.ValidateField(detail.ItemUomId, "Item Uom", r.RowIndex);
				FormHelper.ValidateField(detail.ItemQty, "Received Qty", r.RowIndex);
				FormHelper.ValidateField(detail.SecondaryUomId, "Secondary Uom", r.RowIndex);
				FormHelper.ValidateField(detail.SecondaryUomQty, "SecondaryUom Qty", r.RowIndex);
				FormHelper.ValidateField(detail.SecondaryUomItemRate, "SecondaryUom ItemRate", r.RowIndex);
				FormHelper.ValidateField(detail.ItemRate, "Item Rate", r.RowIndex);
				FormHelper.ValidateField(detail.ItemAmount, "Item Amount", r.RowIndex);
				FormHelper.ValidateField(detail.DepartmentId, "Department", r.RowIndex);
				FormHelper.ValidateField(detail.AssetsId, "Asset", r.RowIndex);
				FormHelper.ValidateField(detail.CrAccountId, "Credit Account", r.RowIndex);
				if (table.AsEnumerable().Count((DataRow row) => row.Field<int>("RecordNo") == Conversion.ToInt(r.Cells["RecordNo"].Value)) > 1 && detail.IssuanceDetailId > 0)
				{
					double BalQty = Conversion.ToDouble(r.Cells["BalanceQty"].Value);
					if ((from row in table.AsEnumerable()
						where row.Field<int>("RecordNo") == Conversion.ToInt(r.Cells["RecordNo"].Value)
						select row).Sum((DataRow row) => row.Field<double>("ItemQty")) > BalQty)
					{
						throw new Exception("Total Qty of RecordNo" + r.Cells["RecordNo"].Value?.ToString() + " Can't Be greater than balance qty which Is " + BalQty);
					}
				}
				obj.StoreRerurnDetails.Add(detail);
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvStoreReturn>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int code = InvStoreReturn.Save(obj);
			if (RECID == 0)
			{
				MessageBox.Show("Record Save SuccessFully " + obj.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully " + obj.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, 140, base.Name);
			}
			formReset();
			if (ChkPrintPreview.Checked)
			{
				SlipReturntoStore457(code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvStoreReturnDetail Pd, GridEXRow r)
	{
		Pd.IssuanceId = Conversion.ToInt(r.Cells["IssuanceId"].Value);
		Pd.IssuanceDetailId = Conversion.ToInt(r.Cells["IssuanceDetailId"].Value);
		Pd.IssuanceNo = Conversion.ToInt(r.Cells["IssuanceNo"].Value);
		Pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		Pd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		Pd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		Pd.Warehouseid = Conversion.ToInt(r.Cells["WareHouseId"].Value);
		Pd.WareHouseName = Conversion.ToString((object)r.Cells["WareHouseName"].Text);
		Pd.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
		Pd.rackName = Conversion.ToString(r.Cells["RackName"].Value);
		Pd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		Pd.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		Pd.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
		Pd.UomCode = Conversion.ToString(r.Cells["PackUom"].Value);
		Pd.BalQty = Conversion.ToDouble(r.Cells["BalanceQty"].Value);
		Pd.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		Pd.SecondaryUomId = Conversion.ToInt(r.Cells["SecondaryUomId"].Value);
		Pd.SecondaryUomCode = Conversion.ToString((object)r.Cells["SecondaryUom"].Text);
		Pd.SecondaryUomQty = Conversion.ToDecimal(r.Cells["SecondaryUomQty"].Value);
		Pd.PerItemSecondaryUomQty = Conversion.ToDouble(r.Cells["PerItemWeight"].Value);
		Pd.SecondaryUomItemRate = Conversion.ToDecimal(r.Cells["SecondaryUomItemRate"].Value);
		Pd.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
		Conversion.ToDouble(r.Cells["IssuanceRate"].Value);
		Pd.ItemAmount = Pd.ItemQty * Pd.ItemRate;
		Pd.DepartmentId = Conversion.ToInt(r.Cells["DepartmentId"].Value);
		Pd.DepartmentName = Conversion.ToString((object)r.Cells["DepartmentId"].Text);
		Pd.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
		Pd.AssetName = Conversion.ToString((object)r.Cells["AssetId"].Text);
		Pd.CrAccountId = Conversion.ToInt(r.Cells["CrAccountId"].Value);
		Pd.CreditAc = Conversion.ToString(r.Cells["CreditAccount"].Value);
		Pd.ReamarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			formReset();
			RECID = ID;
			InvStoreReturn head = InvStoreReturn.GetByID(ID);
			if (head == null)
			{
				MessageBox.Show("Record Not Found");
				return;
			}
			tabControlGsIssuance.SelectedIndex = 0;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			txtDocdate.Value = head.DocDate;
			txtDocNo.Text = head.DocNo.ToString();
			txtRemarks.Text = head.Remarks;
			VoucherHeadIdGet();
			table.Rows.Clear();
			FillDetailFromListCommonForReadById(head, table);
			BindGrids();
			if (table.AsEnumerable().Any((DataRow rr) => Conversion.ToInt(rr["IssuanceId"]) > 0))
			{
				DetailGBox.Visible = false;
				panel5.Height = 88;
			}
			FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(InvStoreReturn invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.StoreRerurnDetails == null || invGrn.StoreRerurnDetails.Count == 0)
		{
			return;
		}
		Dictionary<int, int> recordNoMap = new Dictionary<int, int>();
		int recordCounter = 0;
		foreach (InvStoreReturnDetail storeRerurnDetail in invGrn.StoreRerurnDetails)
		{
			int detailId = storeRerurnDetail.IssuanceDetailId;
			if (!recordNoMap.ContainsKey(detailId))
			{
				recordCounter = (recordNoMap[detailId] = recordCounter + 1);
			}
		}
		foreach (InvStoreReturnDetail detail in invGrn.StoreRerurnDetails)
		{
			DataRow dr = dt.NewRow();
			dr["IssuanceId"] = detail.IssuanceId;
			dr["IssuanceDetailId"] = detail.IssuanceDetailId;
			dr["IssuanceNo"] = detail.IssuanceNo;
			dr["ItemId"] = detail.ItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["WareHouseId"] = detail.Warehouseid;
			dr["WareHouseName"] = detail.WareHouseName;
			dr["RackId"] = detail.RackId;
			dr["RackName"] = detail.rackName;
			dr["ItemConditionId"] = detail.ItemConditionId;
			dr["PackUomId"] = detail.ItemUomId;
			dr["PackUom"] = detail.UomCode;
			dr["BalanceQty"] = detail.BalQty;
			dr["ItemQty"] = detail.ItemQty;
			dr["SecondaryUomId"] = detail.SecondaryUomId;
			dr["SecondaryUom"] = detail.SecondaryUomCode;
			dr["SecondaryUomQty"] = detail.SecondaryUomQty;
			dr["PerItemWeight"] = detail.PerItemSecondaryUomQty;
			dr["SecondaryUomItemRate"] = detail.SecondaryUomItemRate;
			dr["ItemRate"] = detail.ItemRate;
			dr["IssuanceRate"] = detail.IssuanceRate;
			dr["ItemAmount"] = detail.ItemAmount;
			dr["DepartmentId"] = detail.DepartmentId;
			dr["AssetId"] = detail.AssetsId;
			dr["Remarks"] = detail.ReamarksDetail;
			dr["CrAccountId"] = detail.CrAccountId;
			dr["CreditAccount"] = detail.CreditAc;
			dr["RecordNo"] = recordNoMap[detail.IssuanceDetailId];
			if (AddExtraColumns)
			{
				dr["ItemCondition"] = detail.ItemCondition;
				dr["DepartmentName"] = detail.DepartmentName;
				dr["AssetName"] = detail.AssetName;
			}
			dt.Rows.Add(dr);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty || Conversion.ToInt((object)txtDocNo.Text.Trim()) == 0)
		{
			MessageBox.Show("DocNo Field Required");
			txtDocNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("WareHouseName Field Required");
			CmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackName).ActiveRow == null || Conversion.ToInt(CmbRackName.Value) == 0)
		{
			MessageBox.Show("Rack Name Field Required");
			CmbRackName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
		{
			MessageBox.Show("Item Condition Field Required");
			CmbItemCondition.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUOM).ActiveRow == null)
		{
			MessageBox.Show("Item PackUom Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemQty Field Required");
			txtItemQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemRate Field Required");
			txtItemRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemAmount Field Required");
			txtItemAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDepartment).ActiveRow == null || Conversion.ToInt(CmbDepartment.Value) == 0)
		{
			MessageBox.Show("Department Field Required");
			CmbDepartment.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAssetsRef).ActiveRow == null || Conversion.ToInt(cmbAssetsRef.Value) == 0)
		{
			MessageBox.Show("Fixed Asset Field Required");
			cmbAssetsRef.Focus();
			return false;
		}
		if ((((UltraGridBase)cmbCrAc).ActiveRow == null || Conversion.ToInt(cmbCrAc.Value) == 0) && StoreFinancialEffects)
		{
			MessageBox.Show("Credit Account Field Required");
			cmbCrAc.Focus();
			return false;
		}
		return true;
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			SlipReturntoStore457(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SlipReturntoStore457(int PrintId)
	{
		try
		{
			CommonServices.StoreIssuanceReturnSlip457(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControlGsIssuance_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlGsIssuance.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill(int NoOfRecords = 0)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 140;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.WarehouseId = Conversion.ToInt(CmbWarehouseHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.DepartmentId = Conversion.ToInt(CmbDepartmentHistory.Value);
			obj.AssetId = Conversion.ToInt(CmbAssetNameHistory.Value);
			obj.AccountId = Conversion.ToInt(CmbCreditAccountHistory.Value);
			DataTable dt = InvStoreReturn.FormHistory(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("VoucherHeadId", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				int i;
				for (i = 0; i < dt.Rows.Count; i++)
				{
					if (!dtcol.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["Id"])))
					{
						dtcol.Rows.Add(dt.Rows[i]["RecordNo"], dt.Rows[i]["Id"], dt.Rows[i]["VoucherHeadId"], dt.Rows[i]["DocDate"], dt.Rows[i]["DocNo"], dt.Rows[i]["EntryDate"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["NoOfAttachments"]);
					}
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
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
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, null, null, null, ColumnSetting: false);
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants + 50;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants + 50;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			if (formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 40;
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.RootTable.Columns.Add("Voucher");
				DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
				DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
				DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
				DataGridHistory.RootTable.Columns["Voucher"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 35;
				DataGridHistory.RootTable.Columns["Edit"].Position = 2;
				DataGridHistory.FrozenColumns = 3;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RECID);
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
			if (e.Column.Key == "Edit" && formrights.DoHaveUpdateRights)
			{
				RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RECID);
			}
			if (e.Column.Key == "Print" && formrights.DoHavePrintRights)
			{
				SlipReturntoStore457(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher" && formrights.DoHavePrintRights)
			{
				CommonServices.VoucherReport_118(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), 140);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "NoOfAttachments" && Conversion.ToInt(DataGridHistory.CurrentRow.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 140);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			InvStoreReturn Obj = InvStoreReturn.GetByID(Id);
			if (Obj == null || Obj.StoreRerurnDetails.Count == 0)
			{
				gridHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = table.Clone();
			dtDetailHistory.Columns.Add("ItemCondition");
			dtDetailHistory.Columns.Add("DepartmentName");
			dtDetailHistory.Columns.Add("AssetName");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			gridHistory.DataSource = dtDetailHistory;
			gridHistory.RetrieveStructure();
			DataGridHistoryDetailSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			DetailGridSettings(gridHistory);
			gridHistory.RootTable.Columns["ItemConditionId"].Visible = false;
			gridHistory.RootTable.Columns["DepartmentId"].Visible = false;
			gridHistory.RootTable.Columns["AssetId"].Visible = false;
			gridHistory.RootTable.Columns["BalanceQty"].Visible = false;
			gridHistory.RootTable.Columns["RecordNo"].Visible = false;
			gridHistory.RootTable.Columns["ItemCondition"].Position = gridHistory.RootTable.Columns["ItemConditionId"].Position + 1;
			gridHistory.RootTable.Columns["DepartmentName"].Position = gridHistory.RootTable.Columns["DepartmentId"].Position + 1;
			gridHistory.RootTable.Columns["AssetName"].Position = gridHistory.RootTable.Columns["AssetId"].Position + 1;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetLastIssuanceRateByItemId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			double ItemRate = InvGsStoreIssuanceHeader.GetLastIssuanceRateByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbItem.Value)
			});
			if (ItemRate > 0.0)
			{
				txtIssuanceRate.Text = Conversion.ToString((object)ItemRate);
			}
			else
			{
				txtIssuanceRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetAvgRateByDocDateItemAndCondition(int ItemId, DateTime DocDate, int ConditionId)
	{
		try
		{
			double ItemRate = 0.0;
			DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate, ConditionId, RECID, (RECID > 0) ? 140 : 0);
			if (dtRate.Rows.Count > 0)
			{
				ItemRate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
			}
			return ItemRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemQty.Text);
			double itemRate = Conversion.ToDouble((object)txtItemRate.Text);
			double amount = num * itemRate;
			txtItemAmount.Text = ((amount > 0.0) ? amount.ToString(clsGlobalVariables.stringFormatsingle) : "0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
	{
		ItemRateAndSecondaryRateCalculations();
		CalculateAmount();
	}

	private void txtSecondaryUomQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SecondaryUomQtyAndPerItemSecondaryQtyCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSecondaryUomRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemRateAndSecondaryRateCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemRateAndSecondaryRateCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPerItemWeight_TextChanged(object sender, EventArgs e)
	{
		SecondaryUomQtyAndPerItemSecondaryQtyCalculations();
	}

	private void SecondaryUomQtyAndPerItemSecondaryQtyCalculations()
	{
		if (_isUpdatingSecondary)
		{
			return;
		}
		try
		{
			_isUpdatingSecondary = true;
			double perItem = Conversion.ToDouble((object)txtPerItemWeight.Text);
			double totalSecondary = Conversion.ToDouble((object)txtSecondaryUomQty.Text);
			double qty = Conversion.ToDouble((object)txtItemQty.Text);
			if (qty <= 0.0)
			{
				txtPerItemWeight.Text = "0";
				txtSecondaryUomQty.Text = "0";
				return;
			}
			string active = base.ActiveControl?.AccessibleName;
			if (active == txtPerItemWeight.AccessibleName)
			{
				double result = Math.Round(perItem * qty, 3);
				txtSecondaryUomQty.Text = result.ToString("#,##0.###");
				CalculateByPerItemSecondaryQty = true;
				CalculateBySecondaryUomQty = false;
			}
			else if (active == txtSecondaryUomQty.AccessibleName)
			{
				double result2 = Math.Round(totalSecondary / qty, 3);
				txtPerItemWeight.Text = result2.ToString("#,##0.###");
				CalculateByPerItemSecondaryQty = false;
				CalculateBySecondaryUomQty = true;
			}
			else if (CalculateBySecondaryUomQty)
			{
				double result3 = Math.Round(totalSecondary / qty, 3);
				txtPerItemWeight.Text = result3.ToString("#,##0.###");
			}
			else if (CalculateByPerItemSecondaryQty)
			{
				double result4 = Math.Round(perItem * qty, 3);
				txtSecondaryUomQty.Text = result4.ToString("#,##0.###");
			}
		}
		finally
		{
			_isUpdatingSecondary = false;
		}
	}

	private void ItemRateAndSecondaryRateCalculations()
	{
		if (_isUpdatingRates)
		{
			return;
		}
		try
		{
			_isUpdatingRates = true;
			double itemRate = Conversion.ToDouble((object)txtItemRate.Text);
			double secondaryRate = Conversion.ToDouble((object)txtSecondaryUomRate.Text);
			double secondaryQty = Conversion.ToDouble((object)txtSecondaryUomQty.Text);
			double qty = Conversion.ToDouble((object)txtItemQty.Text);
			if (qty <= 0.0 || secondaryQty <= 0.0)
			{
				txtItemRate.Text = "0";
				txtSecondaryUomRate.Text = "0";
				return;
			}
			string active = base.ActiveControl?.AccessibleName;
			if (active == txtItemRate.AccessibleName)
			{
				double result = Math.Round(itemRate * (qty / secondaryQty), 3);
				txtSecondaryUomRate.Text = result.ToString("#,##0.###");
				CalculateByItemRate = true;
				CalculateBySecondaryUomRate = false;
			}
			else if (active == txtSecondaryUomRate.AccessibleName)
			{
				double result2 = Math.Round(secondaryRate * secondaryQty / qty, 3);
				txtItemRate.Text = result2.ToString("#,##0.###");
				CalculateByItemRate = false;
				CalculateBySecondaryUomRate = true;
			}
			else if (CalculateBySecondaryUomRate)
			{
				double result3 = Math.Round(secondaryRate * secondaryQty / qty, 3);
				txtItemRate.Text = result3.ToString("#,##0.###");
			}
			else if (CalculateByItemRate)
			{
				double result4 = Math.Round(itemRate * (qty / secondaryQty), 3);
				txtSecondaryUomRate.Text = result4.ToString("#,##0.###");
			}
		}
		finally
		{
			_isUpdatingRates = false;
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId, 140);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void frmGSIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else
				{
					btnShowHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					BtnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else
				{
					BtnRefreshHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					tabControlGsIssuance.SelectedIndex = 0;
				}
				else
				{
					tabControlGsIssuance.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDocdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					CmbWarehouse.Focus();
				}
				else
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					((Control)(object)grdDetail).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RECID);
				}
				else
				{
					grdDetail_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Warehouse Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			dt.Rows.Add("F1", "Select Warehouse and Rack on F1 when focus on Warehouse or Rack Column in detail grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				int Id = int.Parse(r.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RECID);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print" && formrights.DoHavePrintRights)
				{
					SlipReturntoStore457(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Voucher" && formrights.DoHavePrintRights)
				{
					CommonServices.VoucherReport_118(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), 140);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 140);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			ToDateHistory.Text = string.Empty;
			((Control)(object)CmbWarehouseHistory).Text = string.Empty;
			((Control)(object)CmbItemNameHistory).Text = string.Empty;
			((Control)(object)CmbDepartmentHistory).Text = string.Empty;
			((Control)(object)CmbAssetNameHistory).Text = string.Empty;
			((Control)(object)CmbCreditAccountHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			gridHistory.DataSource = null;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboBindDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboBindDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			return InvStoreReturn.GetDataForDropDownFromInvStoreReturn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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
			DataTable dtWarehouse = new DataTable();
			dtWarehouse.Columns.Add("Id", typeof(int));
			dtWarehouse.Columns.Add("ReferenceName");
			DataTable dtItem = dtWarehouse.Clone();
			DataTable dtDepartment = dtWarehouse.Clone();
			DataTable dtAsset = dtWarehouse.Clone();
			DataTable dtAccount = dtWarehouse.Clone();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Warehouse")
					{
						dtWarehouse.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "ItemName")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Department")
					{
						dtDepartment.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Asset")
					{
						dtAsset.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "CreditAccount")
					{
						dtAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtWarehouse, CmbWarehouseHistory, "Id", "ReferenceName", "Warehouse", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemNameHistory, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtDepartment, CmbDepartmentHistory, "Id", "ReferenceName", "Department", ZeroIndex: false);
			DDL.BindDDL(dtAsset, CmbAssetNameHistory, "Id", "ReferenceName", "Asset Name", ZeroIndex: false);
			DDL.BindDDL(dtAccount, CmbCreditAccountHistory, "Id", "ReferenceName", "Credit Account", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnLoadIssuance_Click(object sender, EventArgs e)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				using (frmLoadIssuanceForReturn loader = new frmLoadIssuanceForReturn(UserAccount))
				{
					loader.ShowDialog();
					loader.BringToFront();
					LoadInGridDetail(loader.dtForLoad);
					return;
				}
			}
			GridEXRow r = grdDetail.GetRow(0);
			if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["IssuanceId"].Value) == 0)
			{
				throw new Exception("You Can't Load Because direct Entry Already Exist");
			}
			using frmLoadIssuanceForReturn frmLoadIssuanceForReturn2 = new frmLoadIssuanceForReturn(UserAccount);
			frmLoadIssuanceForReturn2.ShowDialog();
			frmLoadIssuanceForReturn2.BringToFront();
			LoadInGridDetail(frmLoadIssuanceForReturn2.dtForLoad);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadIssuanceByRecId(int IssuanceRecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dtData = InvGsStoreIssuanceHeader.PendingLoaderDataForStoreReturn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = IssuanceRecId
			});
			LoadInGridDetail(dtData, AsReplacement: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt, bool AsReplacement = false)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grdDetail.GetRows()
					select Conversion.ToInt(r.Cells["IssuanceDetailId"].Value));
				int flagMaxRowNoInDetail = (grdDetail.GetRows().Any() ? grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["RecordNo"].Value)) : 0);
				int newRecordCounter = 0;
				foreach (DataRow dr in dt.Rows)
				{
					int DetailId = Conversion.ToInt(dr["DetailId"]);
					if (existingDetailIds.Add(DetailId))
					{
						DataRow newRow = table.NewRow();
						newRow["IssuanceId"] = Conversion.ToInt(dr["Id"]);
						newRow["IssuanceDetailId"] = dr["DetailId"];
						newRow["IssuanceNo"] = dr["DocNo"];
						newRow["WareHouseId"] = dr["Warehouseid"];
						newRow["WareHouseName"] = dr["WareHouseName"];
						int itemId = Conversion.ToInt(dr["ItemId"]);
						newRow["ItemId"] = itemId;
						newRow["ItemName"] = ((itemId > 0) ? dr["ItemName"] : string.Empty);
						newRow["ItemCode"] = ((itemId > 0) ? dr["ItemCode"] : string.Empty);
						newRow["RackId"] = dr["RackId"];
						newRow["RackName"] = dr["rackName"];
						newRow["ItemConditionId"] = ((!AsReplacement) ? dr["ItemConditionId"] : ((clsGlobalVariables.globalItemConditions.Count > 0) ? ((object)4) : dr["ItemConditionId"]));
						newRow["PackUomId"] = dr["ItemUomId"];
						newRow["PackUom"] = dr["PackUom"];
						GloablUomSchedule obj = clsGlobalVariables.globalUomSchedule?.FirstOrDefault((GloablUomSchedule r) => r.ItemId == itemId && r.BaseSecondaryUom);
						int secondaryUomId = ((obj != null) ? obj.Id : 0);
						string secondaryUom = ((obj != null) ? obj.UOMCode : null) ?? "";
						if (secondaryUomId > 0)
						{
							newRow["SecondaryUomId"] = secondaryUomId;
						}
						if (secondaryUomId > 0)
						{
							newRow["SecondaryUom"] = secondaryUom;
						}
						newRow["BalanceQty"] = dr["BalanceQty"];
						newRow["ItemQty"] = dr["BalanceQty"];
						double ItemRate = GetAvgRateByDocDateItemAndCondition(itemId, txtDocdate.Value, Conversion.ToInt(dr["ItemConditionId"]));
						newRow["IssuanceRate"] = dr["ItemRate"];
						newRow["ItemRate"] = ItemRate;
						newRow["ItemAmount"] = Conversion.ToDouble(dr["BalanceQty"]) * ItemRate;
						newRow["DepartmentId"] = dr["DepartmentId"];
						newRow["AssetId"] = dr["AssetsId"];
						newRow["CrAccountId"] = dr["DrAccountId"];
						newRow["CreditAccount"] = dr["CreditAccount"];
						newRecordCounter++;
						newRow["RecordNo"] = flagMaxRowNoInDetail + newRecordCounter;
						table.Rows.Add(newRow);
					}
				}
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			DetailGridSettings(grdDetail);
			DetailGBox.Visible = false;
			panel5.Height = 88;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int conditionId = Conversion.ToInt(obj.Cells["ItemConditionId"].Value);
				double num = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
				double ItemRate = GetAvgRateByDocDateItemAndCondition(ItemId, txtDocdate.Value, conditionId);
				double Amount = num * ItemRate;
				obj.BeginEdit();
				obj.Cells["ItemAmount"].Value = Amount;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			txtItemRate.Text = GetAvgRateByDocDateItemAndCondition(Conversion.ToInt(cmbItem.Value), txtDocdate.Value, Conversion.ToInt(CmbItemCondition.Value)).ToString("#,##0.###");
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
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Expected O, but got Unknown
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Expected O, but got Unknown
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Expected O, but got Unknown
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Expected O, but got Unknown
		//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e97: Expected O, but got Unknown
		//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Expected O, but got Unknown
		//IL_5b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1e: Expected O, but got Unknown
		//IL_5b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b4c: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
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
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.StoreReturn));
		GridEXLayout gridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlGsIssuance = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DetailGBox = new System.Windows.Forms.GroupBox();
		this.CmbRackName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbSecondaryUom = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtPerItemWeight = new System.Windows.Forms.TextBox();
		this.txtIssuanceRate = new System.Windows.Forms.TextBox();
		this.CmbItemCondition = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbDepartment = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbCrAc = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtSecondaryUomQty = new System.Windows.Forms.TextBox();
		this.labelCrAc = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtSecondaryUomRate = new System.Windows.Forms.TextBox();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.label12 = new System.Windows.Forms.Label();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.Add = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbAssetsRef = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.btnLoadIssuance = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbCreditAccountHistory = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbAssetNameHistory = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbDepartmentHistory = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbItemNameHistory = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbWarehouseHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControlGsIssuance.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel5.SuspendLayout();
		this.DetailGBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSecondaryUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCrAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridHistory).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlGsIssuance);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1144, 661);
		this.panel1.TabIndex = 0;
		this.tabControlGsIssuance.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlGsIssuance.Controls.Add(this.tabPage1);
		this.tabControlGsIssuance.Controls.Add(this.tabPage2);
		this.tabControlGsIssuance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlGsIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlGsIssuance.Location = new System.Drawing.Point(0, 0);
		this.tabControlGsIssuance.Name = "tabControlGsIssuance";
		this.tabControlGsIssuance.SelectedIndex = 0;
		this.tabControlGsIssuance.Size = new System.Drawing.Size(1144, 661);
		this.tabControlGsIssuance.TabIndex = 0;
		this.tabControlGsIssuance.SelectedIndexChanged += new System.EventHandler(tabControlGsIssuance_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1136, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1130, 626);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 263);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1130, 363);
		this.panel6.TabIndex = 3;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1130, 335);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.grdDetail.SelectionChanged += new System.EventHandler(grdDetail_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdDetail).Click += new System.EventHandler(grdDetail_Click);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label17);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1130, 28);
		this.panel9.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1091, 1);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 6;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 19);
		this.label17.TabIndex = 1;
		this.label17.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.DetailGBox);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 57);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1130, 206);
		this.panel5.TabIndex = 2;
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.DetailGBox.Controls.Add(this.label4);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSecondaryUom);
		this.DetailGBox.Controls.Add(this.label23);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.DetailGBox.Controls.Add(this.label44);
		this.DetailGBox.Controls.Add(this.label24);
		this.DetailGBox.Controls.Add(this.label21);
		this.DetailGBox.Controls.Add(this.txtPerItemWeight);
		this.DetailGBox.Controls.Add(this.txtIssuanceRate);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.DetailGBox.Controls.Add(this.label25);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.DetailGBox.Controls.Add(this.label6);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepartment);
		this.DetailGBox.Controls.Add(this.label7);
		this.DetailGBox.Controls.Add(this.rdSearchByCode);
		this.DetailGBox.Controls.Add(this.rdSearchByName);
		this.DetailGBox.Controls.Add(this.label22);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCrAc);
		this.DetailGBox.Controls.Add(this.label18);
		this.DetailGBox.Controls.Add(this.txtSecondaryUomQty);
		this.DetailGBox.Controls.Add(this.labelCrAc);
		this.DetailGBox.Controls.Add(this.txtItemQty);
		this.DetailGBox.Controls.Add(this.label13);
		this.DetailGBox.Controls.Add(this.label27);
		this.DetailGBox.Controls.Add(this.label10);
		this.DetailGBox.Controls.Add(this.txtRemarksDetail);
		this.DetailGBox.Controls.Add(this.txtSecondaryUomRate);
		this.DetailGBox.Controls.Add(this.txtItemRate);
		this.DetailGBox.Controls.Add(this.btnCancelDetail);
		this.DetailGBox.Controls.Add(this.label12);
		this.DetailGBox.Controls.Add(this.txtItemAmount);
		this.DetailGBox.Controls.Add(this.Add);
		this.DetailGBox.Controls.Add(this.label9);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsRef);
		this.DetailGBox.Controls.Add(this.label11);
		this.DetailGBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.DetailGBox.Controls.Add(this.btnUpdateDetail);
		this.DetailGBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DetailGBox.Location = new System.Drawing.Point(4, 85);
		this.DetailGBox.Name = "DetailGBox";
		this.DetailGBox.Size = new System.Drawing.Size(1119, 119);
		this.DetailGBox.TabIndex = 1;
		this.DetailGBox.TabStop = false;
		this.DetailGBox.Text = "Detail Information";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance141).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(385, 40);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Leave += new System.EventHandler(CmbRackName_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(385, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 15);
		this.label4.TabIndex = 245;
		this.label4.Text = "Rack Name";
		this.CmbSecondaryUom.AllowNull = (DefaultableBoolean)1;
		this.CmbSecondaryUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSecondaryUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSecondaryUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSecondaryUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Location = new System.Drawing.Point(827, 40);
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Name = "CmbSecondaryUom";
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(836, 10);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(63, 30);
		this.label23.TabIndex = 107;
		this.label23.Text = "Secondary\r\nUom";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(668, 40);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 4;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(668, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(61, 15);
		this.label44.TabIndex = 107;
		this.label44.Text = "Pack Uom";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(988, 10);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(55, 30);
		this.label24.TabIndex = 123;
		this.label24.Text = "Per Item \r\nWeight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(171, 70);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(79, 15);
		this.label21.TabIndex = 123;
		this.label21.Text = "Issuance Rate";
		this.txtPerItemWeight.AccessibleName = "PerItemWeight";
		this.txtPerItemWeight.BackColor = System.Drawing.Color.White;
		this.txtPerItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPerItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPerItemWeight.Location = new System.Drawing.Point(985, 42);
		this.txtPerItemWeight.Name = "txtPerItemWeight";
		this.txtPerItemWeight.Size = new System.Drawing.Size(60, 23);
		this.txtPerItemWeight.TabIndex = 8;
		this.txtPerItemWeight.Tag = "double";
		this.txtPerItemWeight.TextChanged += new System.EventHandler(txtPerItemWeight_TextChanged);
		this.txtIssuanceRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtIssuanceRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIssuanceRate.Enabled = false;
		this.txtIssuanceRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIssuanceRate.Location = new System.Drawing.Point(171, 89);
		this.txtIssuanceRate.Name = "txtIssuanceRate";
		this.txtIssuanceRate.Size = new System.Drawing.Size(75, 23);
		this.txtIssuanceRate.TabIndex = 12;
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(526, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(526, 21);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(59, 15);
		this.label25.TabIndex = 121;
		this.label25.Text = "Condition";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(224, 40);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(224, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(101, 15);
		this.label6.TabIndex = 106;
		this.label6.Text = "WareHouseName";
		this.CmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.CmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Location = new System.Drawing.Point(248, 87);
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Name = "CmbDepartment";
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepartment).TabIndex = 13;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(248, 70);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(71, 15);
		this.label7.TabIndex = 106;
		this.label7.Text = "Department";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(107, 19);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(68, 19);
		this.rdSearchByCode.TabIndex = 119;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "By Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(36, 19);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(73, 19);
		this.rdSearchByName.TabIndex = 118;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "By Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(914, 10);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 30);
		this.label22.TabIndex = 110;
		this.label22.Text = "Secondary\r\nUom Weight";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbCrAc.AllowNull = (DefaultableBoolean)1;
		this.cmbCrAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCrAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCrAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCrAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbCrAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCrAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCrAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCrAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCrAc).Location = new System.Drawing.Point(833, 87);
		((System.Windows.Forms.Control)(object)this.cmbCrAc).Name = "cmbCrAc";
		((System.Windows.Forms.Control)(object)this.cmbCrAc).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.cmbCrAc).TabIndex = 16;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(760, 10);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(54, 30);
		this.label18.TabIndex = 110;
		this.label18.Text = "Recevied\r\nQty";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSecondaryUomQty.AccessibleName = "SecondaryUomQty";
		this.txtSecondaryUomQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondaryUomQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondaryUomQty.Location = new System.Drawing.Point(908, 42);
		this.txtSecondaryUomQty.Name = "txtSecondaryUomQty";
		this.txtSecondaryUomQty.Size = new System.Drawing.Size(75, 23);
		this.txtSecondaryUomQty.TabIndex = 7;
		this.txtSecondaryUomQty.Tag = "double";
		this.txtSecondaryUomQty.TextChanged += new System.EventHandler(txtSecondaryUomQty_TextChanged);
		this.labelCrAc.AutoSize = true;
		this.labelCrAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCrAc.Location = new System.Drawing.Point(833, 70);
		this.labelCrAc.Name = "labelCrAc";
		this.labelCrAc.Size = new System.Drawing.Size(86, 15);
		this.labelCrAc.TabIndex = 116;
		this.labelCrAc.Text = "Credit Account";
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(750, 42);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.Size = new System.Drawing.Size(75, 23);
		this.txtItemQty.TabIndex = 5;
		this.txtItemQty.Tag = "double";
		this.txtItemQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(552, 70);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 113;
		this.label13.Text = "Remarks";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1046, 10);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 30);
		this.label27.TabIndex = 110;
		this.label27.Text = "Secondary\r\nUom Rate";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 70);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 110;
		this.label10.Text = "Item Rate";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(552, 89);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(279, 23);
		this.txtRemarksDetail.TabIndex = 15;
		this.txtSecondaryUomRate.AccessibleName = "SecondaryUomRate";
		this.txtSecondaryUomRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondaryUomRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondaryUomRate.Location = new System.Drawing.Point(1046, 42);
		this.txtSecondaryUomRate.Name = "txtSecondaryUomRate";
		this.txtSecondaryUomRate.Size = new System.Drawing.Size(70, 23);
		this.txtSecondaryUomRate.TabIndex = 9;
		this.txtSecondaryUomRate.Tag = "double";
		this.txtSecondaryUomRate.TextChanged += new System.EventHandler(txtSecondaryUomRate_TextChanged);
		this.txtItemRate.AccessibleName = "ItemRate";
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(3, 89);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.Size = new System.Drawing.Size(75, 23);
		this.txtItemRate.TabIndex = 10;
		this.txtItemRate.Tag = "double";
		this.txtItemRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1057, 88);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(64, 25);
		this.btnCancelDetail.TabIndex = 19;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(79, 70);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 110;
		this.label12.Text = "Total Amount";
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(79, 89);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(90, 23);
		this.txtItemAmount.TabIndex = 11;
		this.txtItemAmount.TabStop = false;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(990, 87);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(37, 26);
		this.Add.TabIndex = 17;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 108;
		this.label9.Text = "Item";
		this.cmbAssetsRef.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsRef.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsRef.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsRef.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAssetsRef.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Location = new System.Drawing.Point(400, 87);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Name = "cmbAssetsRef";
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).TabIndex = 14;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(400, 70);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 111;
		this.label11.Text = "Assets Ref";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(3, 40);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(990, 87);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 26);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1119, 81);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(216, 27);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 15);
		this.label3.TabIndex = 125;
		this.label3.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(277, 23);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(839, 48);
		this.txtRemarks.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 27);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 79;
		this.label1.Text = "Doc No";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(86, 23);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(124, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 53);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 80;
		this.label2.Text = "Doc Date";
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(86, 49);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(124, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.ValueChanged += new System.EventHandler(txtDocdate_ValueChanged);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.ChkPrintPreview);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1130, 28);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(9, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(114, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Store Return";
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(1054, 6);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(72, 19);
		this.ChkPrintPreview.TabIndex = 5;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1130, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnAttachment, this.Print, this.btnVoucher, this.btnLoadIssuance, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1130, 27);
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
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "457-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher.Text = "118-Voucher";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.btnLoadIssuance.Font = new System.Drawing.Font("Verdana", 9.25f);
		this.btnLoadIssuance.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadIssuance.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadIssuance.Name = "btnLoadIssuance";
		this.btnLoadIssuance.Size = new System.Drawing.Size(126, 24);
		this.btnLoadIssuance.Text = "Load Issuance";
		this.btnLoadIssuance.Click += new System.EventHandler(btnLoadIssuance_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1136, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.gridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.gridHistory).BackColor = System.Drawing.Color.White;
		gridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridHistory.DesignTimeLayout = gridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.gridHistory).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.gridHistory).Location = new System.Drawing.Point(3, 474);
		((System.Windows.Forms.Control)(object)this.gridHistory).Name = "gridHistory";
		((ScrollableBase)this.gridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridHistory).Size = new System.Drawing.Size(1130, 155);
		((System.Windows.Forms.Control)(object)this.gridHistory).TabIndex = 4;
		this.gridHistory.TotalRow = (InheritableBoolean)1;
		this.gridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridHistory).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label14);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 445);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1130, 29);
		this.panel8.TabIndex = 3;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(4, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(251, 19);
		this.label14.TabIndex = 1;
		this.label14.Text = "Detail of Above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 164);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1130, 281);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 2;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel10.Controls.Add(this.groupBox3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1130, 107);
		this.panel10.TabIndex = 71;
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(754, 100);
		this.groupBox3.TabIndex = 4;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(497, 47);
		this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(86, 15);
		this.label20.TabIndex = 152;
		this.label20.Text = "Credit Account";
		this.CmbCreditAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Location = new System.Drawing.Point(585, 41);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Name = "CmbCreditAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).TabIndex = 8;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(497, 20);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(70, 15);
		this.label19.TabIndex = 150;
		this.label19.Text = "Asset Name";
		this.CmbAssetNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory).Location = new System.Drawing.Point(585, 14);
		((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory).Name = "CmbAssetNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetNameHistory).TabIndex = 7;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(249, 74);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(71, 15);
		this.label16.TabIndex = 148;
		this.label16.Text = "Department";
		this.CmbDepartmentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDepartmentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepartmentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepartmentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepartmentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Location = new System.Drawing.Point(326, 68);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Name = "CmbDepartmentHistory";
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(249, 47);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 146;
		this.label5.Text = "Item Name";
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(326, 41);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 5;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(249, 20);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(67, 15);
		this.label26.TabIndex = 144;
		this.label26.Text = "Warehouse";
		this.CmbWarehouseHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory).Location = new System.Drawing.Point(326, 14);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory).Name = "CmbWarehouseHistory";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseHistory).TabIndex = 4;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(688, 68);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 9;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(7, 66);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(63, 30);
		this.label55.TabIndex = 85;
		this.label55.Text = "From && To\r\nDoc No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(6, 47);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(6, 20);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(167, 70);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(88, 70);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(88, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(157, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(88, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(157, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 28);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1130, 29);
		this.panel7.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1090, 3);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 5;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(177, 19);
		this.label8.TabIndex = 0;
		this.label8.Text = "Store Return History";
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewHistory, this.BtnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1130, 25);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnLoadAll.AutoSize = false;
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(72, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1144, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "StoreReturn";
		this.Text = "Store Return";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StoreReturn_FormClosing);
		base.Load += new System.EventHandler(storeReturn_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGSIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlGsIssuance.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.DetailGBox.ResumeLayout(false);
		this.DetailGBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSecondaryUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCrAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
