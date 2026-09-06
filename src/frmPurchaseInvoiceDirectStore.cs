// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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

public class frmPurchaseInvoiceDirectStore : Form
{
	private int Id;

	private int VoucherHeadId;

	private int DocumentTypeId;

	private int updateDetailIndex = -1;

	private bool Approved;

	private bool IsTaxable;

	private bool BranchFeature;

	private bool FormInitialized;

	private bool BranchImplemented;

	private bool SubsidiaryAccountAllownOnVouchers;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable dtGrid = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	public DataTable dtitem = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private Label label7;

	private Label label6;

	private TextBox txtbillno;

	private GroupBox groupBox1;

	private TextBox txtAmount;

	private Label label14;

	private Label label42;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label33;

	private Label label31;

	private Label label22;

	internal UltraCombo comsupplier;

	internal UltraCombo comjobLot;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	private Label label5;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label24;

	private Button btnAdd;

	private TextBox txtBillAmount;

	private Label label51;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CheckBox ChkBok;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnRefresh;

	private CheckBox ChkPrintSlip;

	private TextBox txtVehicleNo;

	private Label label3;

	private TextBox txtGpNo;

	private Label label2;

	private CtrlGrdBar ctrlGrdBar2;

	private Label lblTaxInvoice;

	private TextBox txtTaxInvoiceNo;

	internal UltraCombo CmbTaxAccount;

	private Label lblTaxAccount;

	internal GridEX grd;

	private Panel panel6;

	private Panel panel8;

	private Panel panel7;

	internal GridEX grdFreight;

	private Panel panel11;

	private Panel panel5;

	internal GridEX grdGLedger;

	private Panel panel12;

	private GroupBox groupBox2;

	private TextBox txtRemarksdetail;

	internal UltraCombo cmbPackUom;

	private Label label8;

	private Label lblStockQty;

	private RadioButton rdbtnItemCode;

	private Label label36;

	private RadioButton rdbtnItemName;

	private Label label9;

	private Label label10;

	private ToolStripButton BtnShortCutkeys;

	private Label label15;

	private Label label12;

	private Label label11;

	internal GridEX grdDetail;

	private Label label16;

	private Label label17;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel9;

	private Panel panel10;

	private Panel panel14;

	private Panel panel13;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label19;

	private Label label57;

	private Label label58;

	private Label label60;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TextBox txtremarks;

	private TextBox txtBranchSrNo;

	private Label label1;

	internal UltraCombo cmbBranchName;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbItemCondition;

	private Label label4;

	internal UltraCombo CmbRackName;

	private Label label30;

	public frmPurchaseInvoiceDirectStore(UserAccount user)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 750);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			FormInitialized = true;
			((Control)(object)CmbTaxAccount).Visible = false;
			txtTaxInvoiceNo.Visible = false;
			lblTaxInvoice.Visible = false;
			lblTaxAccount.Visible = false;
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			label10.Text = "Purchase Invoice Direct";
			AccountsFill();
			if (Conversion.ToString(base.Tag) == "PurchaseInvoiceDirectWithTax")
			{
				IsTaxable = true;
				label10.Text = "Purchase Invoice Direct With Tax";
				((Control)(object)CmbTaxAccount).Visible = true;
				txtTaxInvoiceNo.Visible = true;
				lblTaxInvoice.Visible = true;
				lblTaxAccount.Visible = true;
				DocumentTypeId = 131;
				GenerateSalesTaxNo();
				TaxItemsBind();
				TaxAccountFill();
			}
			else
			{
				DocumentTypeId = 61;
				ItemDtsFillFromGlobal();
				ItemNameBind();
			}
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			ChkPrintSlip.Enabled = formrights.DoHavePrintRights;
			ChkPrintSlip.Checked = formrights.DoHavePrintRights;
			ChkBok.Enabled = formrights.DoHavePrintRights;
			ChkBok.Checked = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			BranchFeature = CommonServices.GetERPFeatureById(11);
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("RackId", typeof(int));
			dtGrid.Columns.Add("RackName");
			dtGrid.Columns.Add("ItemConditionId", typeof(int));
			dtGrid.Columns.Add("ItemCondition");
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("UOMId", typeof(int));
			dtGrid.Columns.Add("UOM");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("TaxNameId", typeof(int));
			dtGrid.Columns.Add("TaxName");
			dtGrid.Columns.Add("TaxPercent", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("RemarksDetail");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage");
			dtGrdGL.Columns.Add("Qty");
			dtGrdGL.Columns.Add("Rate");
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Remarks");
			dtFreight.Columns.Add("GlAccountId", typeof(int));
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			bindWareHouse();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			Branchlot();
			ConditiontFillFromGlobalAndBind();
			BranchSrNoFill();
			DocumentNo();
			DocDate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnCancelUpdateDetial.Visible = false;
			btnUpdateDetail.Visible = false;
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			HistoryBranchComboFill();
			HistoryComboFill(ValidateBranch: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (IsTaxable)
		{
			if (txtTaxInvoiceNo.Text.Trim() == "" || txtTaxInvoiceNo.Text.Trim() == "0" || txtTaxInvoiceNo.Text.Trim() == null)
			{
				MessageBox.Show("TaxInvoiceNo Field is Required");
				txtTaxInvoiceNo.Focus();
				return false;
			}
			if (((UltraGridBase)CmbTaxAccount).ActiveRow == null)
			{
				MessageBox.Show("TaxAccount Field is Required");
				CmbTaxAccount.Focus();
				return false;
			}
		}
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackName).ActiveRow == null || Conversion.ToInt(CmbRackName.Value) == 0)
		{
			MessageBox.Show("Rack Name Field is Required");
			CmbRackName.Focus();
			return false;
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUom).ActiveRow == null || Conversion.ToInt(cmbPackUom.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			cmbPackUom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
		{
			MessageBox.Show("Item Condition Field is Required");
			CmbItemCondition.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRate.Text) == 0.0)
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			comRateUOM.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void BranchSrNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int code = InvPurchaseInvoice.GenerateBranchCode(new InvPurchaseInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[i].Id), Conversion.ToString((object)obj[i].CompanyName), Conversion.ToString((object)obj[i].PartyCode), Conversion.ToInt((object)obj[i].GlAccountId), Conversion.ToInt((object)obj[i].CityId), Conversion.ToString((object)obj[i].CityName), Conversion.ToString((object)obj[i].MobilePersonal));
				}
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
			int SupId = Conversion.ToInt(comsupplier.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(comsupplier, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, SupId, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)comsupplier).Text = string.Empty;
				((UltraGridBase)comsupplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Branchlot()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(comjobLot.Value);
			DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = ((BranchFeature && BranchImplemented) ? UserAccount.BranchesId : 0)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comjobLot, "Id", "JobLotDescription", "Job Lot ", ZeroIndex: true);
				((UltraGridBase)comjobLot).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comjobLot).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comjobLot.Value = Id;
				}
				else
				{
					((Control)(object)comjobLot).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)comjobLot).DataSource = null;
				((Control)(object)comjobLot).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindWareHouse()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches.Where((GlobalWarehousesWithBranch r) => r.IsActive).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("WareHouseName", typeof(string));
			dt.Columns.Add("BranchId", typeof(int));
			dt.Columns.Add("BranchName", typeof(string));
			DatatableHelper.PopulateDataTable(dt, lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName, (GlobalWarehousesWithBranch x) => x.BranchId, (GlobalWarehousesWithBranch x) => x.BranchName);
			InfragisticsHelper.BindAndRetainSelection(comWarehouse, dt, "Id", "WareHouseName", "Warehouse Name", AllColumns: true, Conversion.ToInt(comWarehouse.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)comWarehouse).Rows[1].Activate();
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(1);
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
					((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[1].Width = 400;
					((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtSupplier, comsupplier, "Id", "CompanyName", "Supplier Name", false);
					((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[1].Width = 400;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtitem.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || allItems.Count == 0)
			{
				return;
			}
			HashSet<int> allowedTypes = new HashSet<int>();
			allowedTypes.Add(14);
			allowedTypes.Add(17);
			dtitem.BeginLoadData();
			foreach (getGlobalAllItems item in allItems)
			{
				if (allowedTypes.Contains(item.ItemTypeOfTypeId))
				{
					dtitem.Rows.Add(item.Id, item.ItemName, item.ItemCode);
				}
			}
			dtitem.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			string DisplayMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string Caption = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(comItem, dtitem, "Id", DisplayMember, Caption, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtitem;
			if (dataTable != null && dataTable.Rows.Count > 0 && ((UltraGridBase)comItem).DataSource != null)
			{
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void item()
	{
		try
		{
			dtitem.Rows.Clear();
			DataTable dt = CommonServices.GetItemByItemTypeId("14,17");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Items Not Found");
		}
	}

	private void TaxItemsBind()
	{
		try
		{
			dtitem.Rows.Clear();
			DataTable dt = CommonServices.GetItemsFromItemTaxSchedule();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Items Not Found");
		}
	}

	private void AccountsFill()
	{
		try
		{
			List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
			dtAccountlst.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtAccountlst.Rows.Add(lst[i].GlAccountId, lst[i].Id, lst[i].CompanyName);
					}
				}
				return;
			}
			int[] include = new int[11]
			{
				3, 4, 6, 7, 8, 11, 12, 13, 14, 20,
				21
			};
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => include.Contains(r.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtAccountlst.Rows.Add(acc.ChartOfAccountId, acc.ChartOfAccountId, acc.AccountTitle);
				}
				existingIds.Add(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxAccountFill()
	{
		try
		{
			DataTable dt = DatatableHelper.GetTaxAccountsFromGlobal();
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbTaxAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOM()
	{
		try
		{
			string Packuom = ((Control)(object)cmbPackUom).Text;
			string RateUom = ((Control)(object)comRateUOM).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(comItem.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbPackUom, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			InfragisticsHelper.BindAndRetainSelection(comRateUOM, dtUOM, "Id", "UOMCode", "Rate Uom", AllColumns: true, RateUom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			if (Conversion.ToInt(cmbPackUom.Value) == 0)
			{
				int rowCount2 = dtUOM?.Rows.Count ?? 0;
				if (rowCount2 == 1)
				{
					cmbPackUom.Value = dtUOM.Rows[0]["Id"];
				}
				else if (rowCount2 > 1)
				{
					DataRow eqRow2 = dtUOM.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToDecimal(r["Equivalent"]) == 1m);
					if (eqRow2 != null)
					{
						int eqId2 = Conversion.ToInt(eqRow2["Id"]);
						UltraGridRow obj = ((IEnumerable)((UltraGridBase)cmbPackUom).Rows).Cast<UltraGridRow>().FirstOrDefault((UltraGridRow r) => Conversion.ToInt(r.Cells["Id"].Value) == eqId2);
						if (obj != null)
						{
							obj.Activate();
						}
					}
				}
			}
			if (Conversion.ToInt(comRateUOM.Value) != 0)
			{
				return;
			}
			int rowCount = dtUOM?.Rows.Count ?? 0;
			if (rowCount == 1)
			{
				comRateUOM.Value = dtUOM.Rows[0]["Id"];
			}
			else
			{
				if (rowCount <= 1)
				{
					return;
				}
				DataRow eqRow = dtUOM.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToDecimal(r["Equivalent"]) == 1m);
				if (eqRow != null)
				{
					int eqId = Conversion.ToInt(eqRow["Id"]);
					UltraGridRow obj2 = ((IEnumerable)((UltraGridBase)comRateUOM).Rows).Cast<UltraGridRow>().FirstOrDefault((UltraGridRow r) => Conversion.ToInt(r.Cells["Id"].Value) == eqId);
					if (obj2 != null)
					{
						obj2.Activate();
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		try
		{
			int Code = CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId);
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSalesTaxNo()
	{
		try
		{
			int Code = CommonServices.GenerateSalesTaxNo(IsTaxable);
			if (Code > 0)
			{
				txtTaxInvoiceNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConditiontFillFromGlobalAndBind()
	{
		try
		{
			DataTable dtCondition = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dtCondition, "Id", "Description", "Item Condition", AllColumns: true, Conversion.ToInt(CmbItemCondition.Value));
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		PackUOM();
		BalanceStock();
		int ItemId = Conversion.ToInt(comItem.Value);
		int WareHouseId = Conversion.ToInt(comWarehouse.Value);
		WareHouseBindFromGlobalRacksByItemId(ItemId);
		RackBindFromGlobalRacksByItemId(ItemId, WareHouseId);
	}

	private void WareHouseBindFromGlobalRacksByItemId(int ItemId)
	{
		try
		{
			List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
				where x.ItemId == ItemId
				group x by x.WarehouseId into g
				select g.First()).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("WareHouseName", typeof(string));
			dt.Columns.Add("BranchId", typeof(int));
			dt.Columns.Add("BranchName", typeof(string));
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem w in list)
			{
				dt.Rows.Add(w.WarehouseId, w.WareHouseName, w.BranchId, w.BranchName);
			}
			dt.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(comWarehouse, dt, "Id", "WareHouseName", "WareHouseName", AllColumns: true, Conversion.ToInt(comWarehouse.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
			}
			if (Conversion.ToInt(CmbRackName.Value) != 0)
			{
				return;
			}
			if (dt.Rows.Count == 1)
			{
				comWarehouse.Value = dt.Rows[0]["Id"];
			}
			else if (dt.Rows.Count > 1)
			{
				int ConfigurationWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
				if (ConfigurationWarehouseId != 0 && dt.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["Id"]) == ConfigurationWarehouseId))
				{
					comWarehouse.Value = ConfigurationWarehouseId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading WareHouse: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem item in list)
			{
				dt.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
			}
			dt.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbRackName, dt, "Id", "RackName", "RackName", AllColumns: true, Conversion.ToInt(CmbRackName.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbRackName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			if (Conversion.ToInt(CmbRackName.Value) == 0 && dt.Rows.Count == 1)
			{
				CmbRackName.Value = dt.Rows[0]["Id"];
				CmbRackName_Leave(null, null);
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
			int currentWarehouseId = Conversion.ToInt(comWarehouse.Value);
			RacksWithWarehouseAndItem rack = clsGlobalVariables.racksWithWarehouseAndItems.FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null)
			{
				if (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId)
				{
					comWarehouse.Value = rack.WarehouseId;
				}
				BalanceStock();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceStock()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			lblStockQty.Text = "0";
			int ItemId = Conversion.ToInt(comItem.Value);
			if (ItemId != 0)
			{
				DataTable dt = GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					ItemId = ItemId,
					DocDate = Conversion.ToDateTime((object)DocDate.Value),
					ItemConditionId = Conversion.ToInt(CmbItemCondition.Value),
					WarehouseId = Conversion.ToInt(comWarehouse.Value),
					JobLotId = Conversion.ToInt(comjobLot.Value),
					StoreRackId = Conversion.ToInt(CmbRackName.Value)
				});
				if (dt.Rows.Count > 0 && dt != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dt.Rows[0]["QtyInHand"]), 2);
					lblStockQty.Text = Conversion.ToString((object)ItemQty);
					lblStockQty.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Expected O, but got Unknown
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				purchaseInvoice.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDouble(r5.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r5.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if ((Conversion.ToDouble(r4.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r4.Cells["Debit"].Value) > 0) && Conversion.ToInt(r4.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			BillAmount();
			FreightProportion();
			purchaseInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			purchaseInvoice.SalesTaxNo = Conversion.ToInt((object)txtTaxInvoiceNo.Text);
			purchaseInvoice.DocumentTypeId = DocumentTypeId;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.RemarksHeader = txtremarks.Text.Trim();
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			purchaseInvoice.ReferencePartyId = Conversion.ToInt(CmbTaxAccount.Value);
			purchaseInvoice.SupplierInvoiceDate = DateTime.Now;
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.DueDate = DateTime.Now;
			purchaseInvoice.IsTaxable = IsTaxable;
			purchaseInvoice.ScreenName = Conversion.ToString((object)base.Name);
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					Pd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Pd.WarehouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
					Pd.RackId = Conversion.ToInt(r3.Cells["RackId"].Value);
					Pd.rackName = Conversion.ToString(r3.Cells["RackName"].Value);
					Pd.ItemConditionId = Conversion.ToInt(r3.Cells["ItemConditionId"].Value);
					Pd.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Pd.JobLotDescription = Conversion.ToString(r3.Cells["JobLot"].Value);
					Pd.ItemUOMId = Conversion.ToInt(r3.Cells["UOMId"].Value);
					Pd.UOMCodeItem = Conversion.ToString(r3.Cells["UOM"].Value);
					Pd.ItemQty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
					Pd.GrossWeight = Pd.ItemQty;
					Pd.NetBillWeight = Pd.ItemQty;
					Pd.NetStockWeight = Pd.ItemQty;
					Pd.UomScheduleIdRate = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
					Pd.ItemRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					Pd.ItemAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
					Pd.TaxNameId = Conversion.ToInt(r3.Cells["TaxNameId"].Value);
					Pd.TaxDescriptions = Conversion.ToString(r3.Cells["TaxName"].Value);
					Pd.TaxPercent = Conversion.ToDouble(r3.Cells["TaxPercent"].Value);
					Pd.TaxAmount = Conversion.ToDouble(r3.Cells["TaxAmount"].Value);
					Pd.GpDate = DateTime.Now;
					Pd.GpNo = Conversion.ToInt(r3.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
					Pd.RemarksDetail = Conversion.ToString(r3.Cells["RemarksDetail"].Value);
					Pd.BillAmount = Conversion.ToDouble(r3.Cells["BillAmount"].Value);
					Pd.FreightAmount = Conversion.ToDouble(r3.Cells["Freights"].Value);
					Pd.BranchId = Conversion.ToInt(r3.Cells["BranchId"].Value);
					Pd.BranchName = Conversion.ToString(r3.Cells["BranchName"].Value);
					FormHelper.ValidateField(Pd.WarehouseId, "Warehouse", r3.RowIndex);
					FormHelper.ValidateField(Pd.RackId, "Rack Name", r3.RowIndex);
					FormHelper.ValidateField(Pd.ItemConditionId, "ItemCondition", r3.RowIndex);
					FormHelper.ValidateField(Pd.JobLotId, "Job Lot", r3.RowIndex);
					FormHelper.ValidateField(Pd.ItemUOMId, "Pack Uom", r3.RowIndex);
					FormHelper.ValidateField(Pd.ItemQty, "Item Qty", r3.RowIndex);
					FormHelper.ValidateField(Pd.UomScheduleIdRate, "Rate Uom", r3.RowIndex);
					FormHelper.ValidateField(Pd.ItemRate, "Item Rate", r3.RowIndex);
					FormHelper.ValidateField(Pd.ItemAmount, "Item Amount", r3.RowIndex);
					if (IsTaxable && Pd.TaxNameId == 0)
					{
						throw new Exception("TaxNameId Not Found Please Check");
					}
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.TansporterId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PF.SupplierCustomerId = Conversion.ToInt(r2.Cells["Transporter"].Value);
						PF.FreightAmount = Conversion.ToInt(r2.Cells["Freight"].Value);
						PF.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
						PJ.SupplierCustomerId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r.Cells["Credit"].Value);
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				purchaseInvoice.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, Id, DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Record Saved Successfully");
				}
				Reset();
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.VoucherReport_118(VoucherHeadId, DocumentTypeId);
				}
				if (ChkPrintSlip.Checked)
				{
					if (DocumentTypeId == 131)
					{
						CommonServices.PurchaseInvoiceStoreDirectSlip_237(sucess);
					}
					else
					{
						CommonServices.PurchaseInvoiceStoreDirectSlip_233(sucess);
					}
				}
				return;
			}
			throw new Exception("Grid Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void saveToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
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
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Id);
			if (invPI != null)
			{
				txtdocno.Text = invPI.DocNo.ToString();
				txtBranchSrNo.Text = invPI.BranchSrNo.ToString();
				txtTaxInvoiceNo.Text = Conversion.ToString((object)invPI.SalesTaxNo);
				DocDate.Value = invPI.DocDate;
				tabControl1.SelectedIndex = 0;
				comsupplier.Value = invPI.SupplierCustomerId;
				CmbTaxAccount.Value = invPI.ReferencePartyId;
				txtbillno.Text = invPI.ManualBillNo;
				txtremarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
				txtBillAmount.Text = invPI.BillAmount.ToString();
				AttachmentsValues = invPI.AttachmentsValues;
				CustomAttachmentsValues = invPI.CustomAttachmentsValues;
				Approved = invPI.IsApproved;
				VoucherHeadIdGet();
				dtGrid.Rows.Clear();
				for (int k = 0; k < invPI.invPurchaseInvoiceDetailList.Count; k++)
				{
					dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[k].Id, invPI.invPurchaseInvoiceDetailList[k].ItemId, invPI.invPurchaseInvoiceDetailList[k].ItemCode, invPI.invPurchaseInvoiceDetailList[k].ItemName, invPI.invPurchaseInvoiceDetailList[k].WarehouseId, invPI.invPurchaseInvoiceDetailList[k].WareHouseName, invPI.invPurchaseInvoiceDetailList[k].RackId, invPI.invPurchaseInvoiceDetailList[k].rackName, invPI.invPurchaseInvoiceDetailList[k].ItemConditionId, invPI.invPurchaseInvoiceDetailList[k].ItemCondition, invPI.invPurchaseInvoiceDetailList[k].JobLotId, invPI.invPurchaseInvoiceDetailList[k].JobLotDescription, invPI.invPurchaseInvoiceDetailList[k].ItemUOMId, invPI.invPurchaseInvoiceDetailList[k].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[k].ItemQty, invPI.invPurchaseInvoiceDetailList[k].ItemRate, invPI.invPurchaseInvoiceDetailList[k].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[k].RateUom, invPI.invPurchaseInvoiceDetailList[k].ItemAmount, invPI.invPurchaseInvoiceDetailList[k].TaxNameId, invPI.invPurchaseInvoiceDetailList[k].TaxDescriptions, invPI.invPurchaseInvoiceDetailList[k].TaxPercent, invPI.invPurchaseInvoiceDetailList[k].TaxAmount, invPI.invPurchaseInvoiceDetailList[k].BillAmount, invPI.invPurchaseInvoiceDetailList[k].FreightAmount, invPI.invPurchaseInvoiceDetailList[k].RemarksDetail, invPI.invPurchaseInvoiceDetailList[k].GpNo, invPI.invPurchaseInvoiceDetailList[k].VehicleNo, invPI.invPurchaseInvoiceDetailList[k].BranchId, invPI.invPurchaseInvoiceDetailList[k].BranchName);
				}
				dtGrdGL.Rows.Clear();
				for (int j = 0; j < invPI.InvPurchaseInvoicejournalList.Count; j++)
				{
					dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[j].SupplierCustomerId, invPI.InvPurchaseInvoicejournalList[j].JvRemarks, invPI.InvPurchaseInvoicejournalList[j].JvPrcnt, invPI.InvPurchaseInvoicejournalList[j].JvQty, invPI.InvPurchaseInvoicejournalList[j].JvRate, invPI.InvPurchaseInvoicejournalList[j].JvDebit, invPI.InvPurchaseInvoicejournalList[j].JvCredit, invPI.InvPurchaseInvoicejournalList[j].ChartofAccountId);
				}
				dtFreight.Rows.Clear();
				for (int i = 0; i < invPI.InvPurchaseInvoiceFreightList.Count; i++)
				{
					dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[i].SupplierCustomerId, invPI.InvPurchaseInvoiceFreightList[i].FreightAmount, invPI.InvPurchaseInvoiceFreightList[i].Remarks, invPI.InvPurchaseInvoiceFreightList[i].TansporterId);
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				gridGLSettings();
				grdFreightSettings();
				tabControl1.SelectedIndex = 0;
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				BillProportion();
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			Id = 0;
			VoucherHeadId = 0;
			Approved = false;
			((Control)(object)comsupplier).Text = string.Empty;
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)comWarehouse).Text = string.Empty;
			((UltraGridBase)comWarehouse).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			((Control)(object)comjobLot).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)comWarehouse).Text = string.Empty;
			txtGpNo.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			DocDate.Focus();
			dtFreight.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			DocumentNo();
			BranchSrNoFill();
			if (IsTaxable)
			{
				GenerateSalesTaxNo();
			}
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comsupplier).Enabled = true;
			txtRate.Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)comWarehouse).Text = string.Empty;
			((UltraGridBase)comWarehouse).DataSource = null;
			((Control)(object)CmbRackName).Text = string.Empty;
			((UltraGridBase)CmbRackName).DataSource = null;
			txtQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AccountsWithCustomGroupId", "AllItems", "RacksWithWarehouseAndItem", "UomSchedule", "WarehousesWithBranches", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			AccountsFill();
			TaxAccountFill();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			if (IsTaxable)
			{
				TaxItemsBind();
			}
			else
			{
				ItemDtsFillFromGlobal();
				ItemNameBind();
			}
			Branchlot();
			ConditiontFillFromGlobalAndBind();
			GridcomboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseInvoiceDirectStore_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double TaxAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (int.Parse(r2.Cells["AccountId"].Value.ToString()) > 0)
				{
					JournalDebit += Conversion.ToDouble((object)r2.Cells["Debit"].Value.ToString());
					JournalCredit += Conversion.ToDouble((object)r2.Cells["Credit"].Value.ToString());
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["Transporter"].Value) > 0 && txtSupplierGLId.Text.ToString() == r.Cells["Transporter"].Value.ToString())
				{
					TransporterCredit += Conversion.ToDouble((object)r.Cells["Freight"].Value.ToString());
				}
			}
			BillAmount = ItemAmount + TaxAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount += TransporterCredit;
			txtBillAmount.Text = Math.Round(BillAmount).ToString();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridcomboBind()
	{
		try
		{
			if (dtAccountlst.Rows.Count > 0)
			{
				grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
				grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
				grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
				grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
				grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
				grdFreight.RootTable.Columns["Transporter"].Width = 250;
				grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
				grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
				grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
				grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
				grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
				grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0);
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SuppliercustomerId", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 200;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns["Delete"].Position = 0;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Add"].Position = 1;
			grdFreight.FrozenColumns = 2;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFreight.CurrentRow;
				dtFreight.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtFreight.Rows.Add(item.Cells["Transporter"].Value, 0, item.Cells["Remarks"].Value);
					grdFreight.DataSource = dtFreight;
				}
			}
			grdFreight.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdFreight.UpdateData();
			_ = grdFreight.CurrentRow;
			if (e.Column.Key == "Transporter")
			{
				SupCustIdUpdateforFrieghtGrid();
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforFrieghtGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["Transporter"].Text);
					if (selectedAcId != string.Empty && selectedAcId != "0")
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdFreight.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SuppliercustomerId", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 200;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns["Delete"].Position = 0;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
			grdGLedger.RootTable.Columns["Add"].Position = 1;
			grdGLedger.FrozenColumns = 2;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Qty * Rate;
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
			{
				item.Cells["Credit"].Value = 0;
				MessageBox.Show("Debit Side is aleady added");
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
			{
				item.Cells["Debit"].Value = 0;
				MessageBox.Show("Credit Side is aleady added");
			}
			if (e.Column.Key == "AccountId")
			{
				if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Supplier Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid();
			}
			grdGLedger.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforGLGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow r = grdGLedger.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["AccountId"].Text);
					if (selectedAcId != "0" && selectedAcId != string.Empty)
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			int BranchId = 0;
			string BranchName = "";
			int JobBranchId = 0;
			string JobBranchName = "";
			if (((UltraGridBase)comWarehouse).ActiveRow != null && Conversion.ToInt(comWarehouse.Value) > 0)
			{
				BranchId = Conversion.ToInt(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchId"].Value);
				BranchName = Conversion.ToString(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchName"].Value);
			}
			if (((UltraGridBase)comjobLot).ActiveRow != null && Conversion.ToInt(comjobLot.Value) > 0)
			{
				JobBranchId = Conversion.ToInt(((UltraDropDownBase)comjobLot).SelectedRow.Cells["BranchId"].Value);
				JobBranchName = Conversion.ToString(((UltraDropDownBase)comjobLot).SelectedRow.Cells["BranchName"].Value);
			}
			if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
			{
				MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			int TaxNameId = 0;
			string TaxName = "";
			double TaxPercent = 0.0;
			double TaxAmount = 0.0;
			double TotalAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)txtAmount.Text.Trim());
			if (DocumentTypeId == 131)
			{
				DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(comItem.Value), DocDate.Value);
				if (dt.Rows.Count > 0)
				{
					TaxNameId = Conversion.ToInt(dt.Rows[0]["TaxNameId"]);
					TaxName = Conversion.ToString(dt.Rows[0]["TaxName"]);
					TaxPercent = Conversion.ToDouble(dt.Rows[0]["TaxPercent"]);
					TaxAmount = ItemAmount * TaxPercent / 100.0;
				}
			}
			TotalAmount = ItemAmount + TaxAmount;
			dtGrid.Rows.Add(0, Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString((object)((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Text), Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToInt(CmbRackName.Value), Conversion.ToString((object)((Control)(object)CmbRackName).Text), Conversion.ToInt(CmbItemCondition.Value), Conversion.ToString((object)((Control)(object)CmbItemCondition).Text.Trim()), Conversion.ToInt(comjobLot.Value), Conversion.ToString((object)((Control)(object)comjobLot).Text), Conversion.ToInt(cmbPackUom.Value), Conversion.ToString((object)((Control)(object)cmbPackUom).Text.Trim()), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtRate.Text.Trim()), Conversion.ToInt(comRateUOM.Value), Conversion.ToString((object)((Control)(object)comRateUOM).Text.Trim()), Conversion.ToDouble((object)txtAmount.Text.Trim()), TaxNameId, TaxName, TaxPercent, TaxAmount, TotalAmount, 0, Conversion.ToString((object)txtRemarksdetail.Text.Trim()), Conversion.ToInt((object)txtGpNo.Text), Conversion.ToString((object)txtVehicleNo.Text.Trim()), BranchId, BranchName);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			FreightProportion();
			BillAmount();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "WarehouseId", "RackId", "ItemConditionId", "JobLotId", "UOMId", "RateUOMId", "TaxNameId", "BranchId" };
			if (!IsTaxable)
			{
				hiddenColumns.AddRange(new string[3] { "TaxName", "TaxPercent", "TaxAmount" });
			}
			if (!BranchFeature || BranchImplemented)
			{
				hiddenColumns.AddRange(new string[1] { "BranchName" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TaxName"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["TaxPercent"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["Freights"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Freights"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["TaxAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				comItem.Value = item.Cells["ItemId"].Value.ToString();
				comItem_Leave(null, null);
				comWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbRackName.Value = item.Cells["RackId"].Value.ToString();
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				comjobLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbPackUom.Value = item.Cells["UOMId"].Value.ToString();
				txtQty.Text = item.Cells["ItemQty"].Value.ToString();
				txtRate.Text = item.Cells["Rate"].Text.ToString();
				comRateUOM.Value = item.Cells["RateUOMId"].Value.ToString();
				((Control)(object)comRateUOM).Text = item.Cells["RateUOM"].Value.ToString();
				txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRemarksdetail.Text = item.Cells["RemarksDetail"].Value.ToString();
				txtGpNo.Text = item.Cells["GpNo"].Value.ToString();
				txtVehicleNo.Text = item.Cells["VehicleNo"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				comWarehouse.Focus();
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
			if (!FormValidationDetail())
			{
				return;
			}
			int BranchId = 0;
			string BranchName = "";
			int JobBranchId = 0;
			string JobBranchName = "";
			if (((UltraGridBase)comWarehouse).ActiveRow != null && Conversion.ToInt(comWarehouse.Value) > 0)
			{
				BranchId = Conversion.ToInt(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchId"].Value);
				BranchName = Conversion.ToString(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchName"].Value);
			}
			if (((UltraGridBase)comjobLot).ActiveRow != null && Conversion.ToInt(comjobLot.Value) > 0)
			{
				JobBranchId = Conversion.ToInt(((UltraDropDownBase)comjobLot).SelectedRow.Cells["BranchId"].Value);
				JobBranchName = Conversion.ToString(((UltraDropDownBase)comjobLot).SelectedRow.Cells["BranchName"].Value);
			}
			if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
			{
				MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			int TaxNameId = 0;
			string TaxName = "";
			double TaxPercent = 0.0;
			double TaxAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)txtAmount.Text.Trim());
			if (DocumentTypeId == 131)
			{
				DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(comItem.Value), DocDate.Value);
				if (dt.Rows.Count > 0)
				{
					TaxNameId = Conversion.ToInt(dt.Rows[0]["TaxNameId"]);
					TaxName = Conversion.ToString(dt.Rows[0]["TaxName"]);
					TaxPercent = Conversion.ToDouble(dt.Rows[0]["TaxPercent"]);
					TaxAmount = ItemAmount * TaxPercent / 100.0;
				}
			}
			dtGrid.Rows[updateDetailIndex]["ItemId"] = comItem.Value;
			dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
			dtGrid.Rows[updateDetailIndex]["Item"] = Conversion.ToString((object)((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Text);
			dtGrid.Rows[updateDetailIndex]["WarehouseId"] = comWarehouse.Value;
			dtGrid.Rows[updateDetailIndex]["Warehouse"] = ((Control)(object)comWarehouse).Text;
			dtGrid.Rows[updateDetailIndex]["RackId"] = CmbRackName.Value;
			dtGrid.Rows[updateDetailIndex]["RackName"] = ((Control)(object)CmbRackName).Text;
			dtGrid.Rows[updateDetailIndex]["ItemConditionId"] = CmbItemCondition.Value;
			dtGrid.Rows[updateDetailIndex]["ItemCondition"] = ((Control)(object)CmbItemCondition).Text;
			dtGrid.Rows[updateDetailIndex]["JobLotId"] = comjobLot.Value;
			dtGrid.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)comjobLot).Text;
			dtGrid.Rows[updateDetailIndex]["UOMId"] = cmbPackUom.Value;
			dtGrid.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbPackUom).Text;
			dtGrid.Rows[updateDetailIndex]["ItemQty"] = txtQty.Text;
			dtGrid.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
			dtGrid.Rows[updateDetailIndex]["RateUOMId"] = comRateUOM.Value;
			dtGrid.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)comRateUOM).Text;
			dtGrid.Rows[updateDetailIndex]["ItemAmount"] = txtAmount.Text;
			dtGrid.Rows[updateDetailIndex]["TaxNameId"] = TaxNameId;
			dtGrid.Rows[updateDetailIndex]["TaxName"] = TaxName;
			dtGrid.Rows[updateDetailIndex]["TaxPercent"] = TaxPercent;
			dtGrid.Rows[updateDetailIndex]["TaxAmount"] = TaxAmount;
			dtGrid.Rows[updateDetailIndex]["RemarksDetail"] = txtRemarksdetail.Text;
			dtGrid.Rows[updateDetailIndex]["GpNo"] = txtGpNo.Text;
			dtGrid.Rows[updateDetailIndex]["VehicleNo"] = txtVehicleNo.Text;
			dtGrid.Rows[updateDetailIndex]["BranchId"] = BranchId;
			dtGrid.Rows[updateDetailIndex]["BranchName"] = BranchName;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			FreightProportion();
			BillAmount();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailrow(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailrow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int DetailId = Conversion.ToInt(r.Cells["Id"].Value);
			if (DetailId > 0)
			{
				if (MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				InvPurchaseInvoice.StockInReferenceValidationReferredOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId, Id, DetailId);
			}
			r.Delete();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCaluculation()
	{
		try
		{
			double ItemQty = 0.0;
			double RateUOM = 0.0;
			double Rate = 0.0;
			double ItemAmount = 0.0;
			if (((UltraGridBase)comRateUOM).ActiveRow != null && Conversion.ToInt(comRateUOM.Value) > 0)
			{
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
			}
			if (txtRate.Text.Trim() != string.Empty)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (txtQty.Text.Trim() != string.Empty)
			{
				ItemQty = Conversion.ToDouble((object)txtQty.Text);
			}
			if (ItemQty > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = ItemQty / RateUOM * Rate;
				txtAmount.Text = Math.Round(ItemAmount, 2).ToString("#,##0.###");
			}
			else
			{
				txtAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
	{
		AmountCaluculation();
		BillAmount();
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		AmountCaluculation();
		BillAmount();
	}

	private void FreightProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				double TotalDiffernce = CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					obj.Cells["Freights"].Value = Math.Round(TotalDiffernce) / NetWeight * Weight;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freights"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Freights"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["TaxAmount"].Value.ToString());
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(comsupplier.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(comsupplier.Value));
				if (dr.Length != 0)
				{
					txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
		PackUOM();
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		AmountCaluculation();
	}

	private void HistoryBranchComboFill()
	{
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill(bool ValidateBranch = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			string BranchIds = "";
			if (dtBranch.Rows.Count > 0)
			{
				if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
				{
					string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
					obj.BranchesIds = BranchIds;
				}
				else
				{
					if (ValidateBranch)
					{
						cmbBranchName.Focus();
						throw new Exception("Select branch first");
					}
					obj.BranchesIds = UserAccount.BranchesId.ToString();
				}
			}
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
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
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
				if (dthistory.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("VoucherHeadId", typeof(int));
					dt.Columns.Add("DocumentTypeId", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("DueDays", typeof(int));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("ManualBillNo", typeof(string));
					dt.Columns.Add("SupplierName", typeof(string));
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("ApprovedStatus", typeof(string));
					dt.Columns.Add("EntryUser", typeof(string));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("ModifyUser", typeof(string));
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("NoOfAttachments", typeof(int));
					dt.Columns.Add("Remarks", typeof(string));
					for (int i = 0; i < dthistory.Rows.Count; i++)
					{
						dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["DocNo"], dthistory.Rows[i]["BranchSrNo"], dthistory.Rows[i]["BranchName"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["ApprovedStatus"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
					}
					grdHistory.DataSource = dt;
					grdHistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdHistory.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.ColumnAutoResize = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DueDays"].Visible = false;
			grdHistory.RootTable.Columns["DueDate"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3);
			grdHistory.RootTable.Columns["DocDate"].Width = 85;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 15;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			if (formrights.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Slip");
				grdHistory.RootTable.Columns["Slip"].Caption = "Slip";
				grdHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
				grdHistory.RootTable.Columns["Slip"].Width = 35;
				grdHistory.RootTable.Columns["Slip"].Position = 0;
				grdHistory.RootTable.Columns.Add("Voucher");
				grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].Width = 60;
				grdHistory.RootTable.Columns["Voucher"].Position = 1;
				grdHistory.FrozenColumns = 2;
			}
			if (formrights.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 35;
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			if (formrights.DoHavePrintRights && formrights.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Slip"].Position = 0;
				grdHistory.RootTable.Columns["Voucher"].Position = 1;
				grdHistory.RootTable.Columns["Edit"].Position = 2;
				grdHistory.FrozenColumns = 3;
			}
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 110);
			((Control)(object)grdHistory).Refresh();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int rId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Voucher")
			{
				if (formrights.DoHavePrintRights)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
				}
			}
			else if (e.Column.Key == "Slip")
			{
				if (formrights.DoHavePrintRights)
				{
					CommonServices.PurchaseInvoiceStoreDirectSlip(rId, DocumentTypeId);
				}
			}
			else if (e.Column.Key == "Edit")
			{
				if (!formrights.DoHaveUpdateRights)
				{
					throw new Exception("You don't Have Update Rights...");
				}
				Id = rId;
				ReadById(Id);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, rId, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formrights.DoHaveUpdateRights)
			{
				Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
				return;
			}
			throw new Exception("You don't Have Update Rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			InvPurchaseInvoice obj = new InvPurchaseInvoice();
			obj = InvPurchaseInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj == null)
			{
				return;
			}
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("Warehouse");
			dtdetail.Columns.Add("RackName");
			dtdetail.Columns.Add("ItemCondition");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("TaxName");
			dtdetail.Columns.Add("TaxPercent", typeof(double));
			dtdetail.Columns.Add("TaxAmount", typeof(double));
			dtdetail.Columns.Add("BillAmount", typeof(double));
			dtdetail.Columns.Add("Freights", typeof(double));
			dtdetail.Columns.Add("RemarksDetail");
			dtdetail.Columns.Add("GpNo", typeof(int));
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("BranchName", typeof(string));
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.invPurchaseInvoiceDetailList.Count; i++)
				{
					dtdetail.Rows.Add(obj.invPurchaseInvoiceDetailList[i].ItemId, obj.invPurchaseInvoiceDetailList[i].ItemCode, obj.invPurchaseInvoiceDetailList[i].ItemName, obj.invPurchaseInvoiceDetailList[i].WareHouseName, obj.invPurchaseInvoiceDetailList[i].rackName, obj.invPurchaseInvoiceDetailList[i].ItemCondition, obj.invPurchaseInvoiceDetailList[i].JobLotDescription, obj.invPurchaseInvoiceDetailList[i].UOMCodeItem, obj.invPurchaseInvoiceDetailList[i].ItemQty, obj.invPurchaseInvoiceDetailList[i].ItemRate, obj.invPurchaseInvoiceDetailList[i].RateUom, obj.invPurchaseInvoiceDetailList[i].ItemAmount, obj.invPurchaseInvoiceDetailList[i].TaxDescriptions, obj.invPurchaseInvoiceDetailList[i].TaxPercent, obj.invPurchaseInvoiceDetailList[i].TaxAmount, obj.invPurchaseInvoiceDetailList[i].BillAmount, obj.invPurchaseInvoiceDetailList[i].FreightAmount, obj.invPurchaseInvoiceDetailList[i].RemarksDetail, obj.invPurchaseInvoiceDetailList[i].GpNo, obj.invPurchaseInvoiceDetailList[i].VehicleNo, obj.invPurchaseInvoiceDetailList[i].BranchName);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, null, null, null, ColumnSetting: false);
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName + 20;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TaxName"].Width = 85;
			grdDetail.RootTable.Columns["TaxPercent"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			if (!IsTaxable)
			{
				grdDetail.RootTable.Columns["TaxName"].Visible = false;
				grdDetail.RootTable.Columns["TaxPercent"].Visible = false;
				grdDetail.RootTable.Columns["TaxAmount"].Visible = false;
			}
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "grdHistory")
			{
				FromDateHistory.Focus();
			}
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
			GetAll();
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
			HistoryBranchComboFill();
			HistoryComboFill();
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoiceStoreDirectSlip(Id, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ChangedTaxPercentFromGridByDatechange(string ItemIds)
	{
		DataTable dt = new DataTable();
		try
		{
			dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!IsTaxable || grd.RowCount <= 0)
			{
				return;
			}
			string ItemIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
			}
			DataTable dt = ChangedTaxPercentFromGridByDatechange(ItemIds);
			rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double TaxPercent = 0.0;
				double ItemAmount = 0.0;
				double TaxAmount = 0.0;
				double TotalAmount = 0.0;
				bool flag = false;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
					{
						flag = true;
						item.Cells["TaxNameId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
						item.Cells["TaxName"].Value = Conversion.ToString(dt.Rows[i]["TaxName"]);
						TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
						item.Cells["TaxPercent"].Value = TaxPercent;
						ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
						if (ItemAmount > 0.0 && TaxPercent > 0.0)
						{
							TaxAmount = ItemAmount * TaxPercent / 100.0;
							item.Cells["TaxAmount"].Value = TaxAmount;
						}
						TotalAmount = ItemAmount + TaxAmount + Conversion.ToDouble(item.Cells["Freights"].Value);
						item.Cells["BillAmount"].Value = TotalAmount;
					}
				}
				if (!flag)
				{
					item.Cells["TaxNameId"].Value = 0;
					item.Cells["TaxName"].Value = 0;
					item.Cells["TaxPercent"].Value = 0;
					item.Cells["TaxAmount"].Value = 0;
					item.Cells["BillAmount"].Value = Conversion.ToDouble(item.Cells["ItemAmount"].Value) + Conversion.ToDouble(item.Cells["Freights"].Value);
				}
				item.EndEdit();
			}
			BillAmount();
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

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				saveToolStripButton_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnNew_Click(null, null);
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
			{
				if (!formrights.DoHavePrintRights)
				{
					throw new Exception("You don't Have Print Right");
				}
				btnPrint_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
			{
				if (!formrights.DoHavePrintRights)
				{
					throw new Exception("You don't Have Print Right");
				}
				btnSlip_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				comItem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)grd).Focused)
				{
					((Control)(object)grdFreight).Focus();
				}
				else if (((Control)(object)grdFreight).Focused)
				{
					((Control)(object)grdGLedger).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				comWarehouse.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					if (formrights.DoHaveUpdateRights)
					{
						Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
						ReadById(Id);
					}
				}
				else
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (((Control)(object)grdFreight).Focused)
				{
					GridEXRow item = grdFreight.CurrentRow;
					dtFreight.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				else if (((Control)(object)grdGLedger).Focused)
				{
					GridEXRow item2 = grdGLedger.CurrentRow;
					dtGrdGL.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				if (((Control)(object)grdFreight).Focused)
				{
					AddRowInFreightGrid();
				}
				else if (((Control)(object)grdGLedger).Focused)
				{
					AddRowInGLGrid();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+Delete", "To delete Row of Grid Freight/Ledger Which have the Focus");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+1", "For Print Slip");
			dt.Rows.Add("alt+2", "For PrintVoucher");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				DeleteDetailrow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdFreight.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdFreight.CurrentColumn == null)
			{
				return;
			}
			if (grdFreight.CurrentColumn.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item2 = grdFreight.CurrentRow;
				dtFreight.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (grdFreight.CurrentColumn.Key == "Add")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtFreight.Rows.Add(item.Cells["Transporter"].Value, 0, item.Cells["Remarks"].Value);
					grdFreight.DataSource = dtFreight;
				}
			}
			grdFreight.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGLedger.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGLedger.CurrentColumn == null || !e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (grdGLedger.CurrentColumn.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (grdGLedger.CurrentColumn.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (formrights.DoHavePrintRights)
			{
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "Slip")
				{
					if (DocumentTypeId == 61)
					{
						Reporting rpt2 = new Reporting();
						if (UserAccount.CompName.ToString() == string.Empty)
						{
							rpt2.RptPerameter("@CompanyName", (string)null);
						}
						else
						{
							rpt2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						}
						rpt2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						rpt2.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
						rpt2.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
						rpt2.RptPerameter("@Id", Conversion.ToString(grdHistory.CurrentRow.Cells["Id"].Value));
						rpt2.RptPerameter("@PihId", Conversion.ToString(grdHistory.CurrentRow.Cells["Id"].Value));
						rpt2.ShowReport("233-SpInvPurchaseInvoice_StoreBillDirect.rpt");
					}
					else
					{
						Reporting rpt = new Reporting();
						if (UserAccount.CompName.ToString() == string.Empty)
						{
							rpt.RptPerameter("@CompanyName", (string)null);
						}
						else
						{
							rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						}
						rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
						rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
						rpt.RptPerameter("@Id", Conversion.ToString(grdHistory.CurrentRow.Cells[1].Value));
						rpt.RptPerameter("@PihId", Conversion.ToString(grdHistory.CurrentRow.Cells[1].Value));
						rpt.ShowReport("237-InvPurchaseInvoice_StoreBillDirectWithTax.rpt");
					}
				}
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formrights.DoHaveUpdateRights)
					{
						throw new Exception("You don't Have Update Rights...");
					}
					Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
					ReadById(Id);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
				}
				return;
			}
			throw new Exception("You don't Have Print Rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grd.DataSource;
				if (dt != null && dt.Rows.Count > 0)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dt);
				}
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
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grdHistory.DataSource;
				if (dt != null && dt.Rows.Count > 0)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dt);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("Record Id Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void comWarehouse_Leave(object sender, EventArgs e)
	{
		int ItemId = Conversion.ToInt(comItem.Value);
		int WareHouseId = Conversion.ToInt(comWarehouse.Value);
		RackBindFromGlobalRacksByItemId(ItemId, WareHouseId);
		BalanceStock();
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		BalanceStock();
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		BalanceStock();
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
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Expected O, but got Unknown
		//IL_0cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Expected O, but got Unknown
		//IL_10df: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e9: Expected O, but got Unknown
		//IL_10f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1100: Expected O, but got Unknown
		//IL_1388: Unknown result type (might be due to invalid IL or missing references)
		//IL_1392: Expected O, but got Unknown
		//IL_139f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Expected O, but got Unknown
		//IL_639c: Unknown result type (might be due to invalid IL or missing references)
		//IL_63a6: Expected O, but got Unknown
		//IL_63ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_63d4: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdFreight = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbRackName = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbItemCondition = new UltraCombo();
		this.cmbPackUom = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.comWarehouse = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.lblStockQty = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.label36 = new System.Windows.Forms.Label();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtRemarksdetail = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.comRateUOM = new UltraCombo();
		this.comjobLot = new UltraCombo();
		this.comItem = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtGpNo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.comsupplier = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.lblTaxInvoice = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.CmbTaxAccount = new UltraCombo();
		this.lblTaxAccount = new System.Windows.Forms.Label();
		this.txtTaxInvoiceNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel12.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1218, 711);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1210, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 467);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1204, 212);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 442);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1204, 25);
		this.panel6.TabIndex = 34;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1164, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 66;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(46, 17);
		this.label15.TabIndex = 65;
		this.label15.Text = "Detail";
		this.panel8.Controls.Add(this.panel5);
		this.panel8.Controls.Add(this.panel7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 282);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1204, 160);
		this.panel8.TabIndex = 33;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(496, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(708, 160);
		this.panel5.TabIndex = 33;
		this.grdGLedger.AutomaticSort = false;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(708, 136);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label12);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(708, 24);
		this.panel12.TabIndex = 35;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(6, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(134, 17);
		this.label12.TabIndex = 65;
		this.label12.Text = "Supplier Add/Less";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(496, 160);
		this.panel7.TabIndex = 32;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(496, 136);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label11);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(496, 24);
		this.panel11.TabIndex = 35;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(5, 2);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(204, 17);
		this.label11.TabIndex = 64;
		this.label11.Text = "Freight (Charge To Product)";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 164);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1204, 118);
		this.panel4.TabIndex = 1;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.lblStockQty);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add(this.txtRemarksdetail);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtGpNo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(941, 111);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(343, 36);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 2;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(343, 17);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 239;
		this.label30.Text = "Rack Name";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(465, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(716, 36);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(716, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(35, 15);
		this.label8.TabIndex = 57;
		this.label8.Text = "UOM";
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(215, 36);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(215, 18);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(104, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(466, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(59, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "Condition";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(797, 82);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(39, 25);
		this.btnAdd.TabIndex = 13;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(184, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 128;
		this.label9.Text = "Remarks";
		this.lblStockQty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStockQty.Location = new System.Drawing.Point(141, 17);
		this.lblStockQty.Name = "lblStockQty";
		this.lblStockQty.Size = new System.Drawing.Size(71, 15);
		this.lblStockQty.TabIndex = 125;
		this.lblStockQty.Text = "0";
		this.lblStockQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(90, 16);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(4, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(32, 15);
		this.label36.TabIndex = 124;
		this.label36.Text = "Item";
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(36, 16);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtRemarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksdetail.Location = new System.Drawing.Point(184, 83);
		this.txtRemarksdetail.Name = "txtRemarksdetail";
		this.txtRemarksdetail.Size = new System.Drawing.Size(441, 23);
		this.txtRemarksdetail.TabIndex = 10;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(866, 82);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 25);
		this.btnCancelUpdateDetial.TabIndex = 15;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(797, 82);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(69, 25);
		this.btnUpdateDetail.TabIndex = 14;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(2, 81);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(597, 36);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 0;
		this.comItem.ValueChanged += new System.EventHandler(comItem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(868, 18);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(30, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(868, 38);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(70, 23);
		this.txtRate.TabIndex = 7;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(683, 83);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(113, 23);
		this.txtVehicleNo.TabIndex = 12;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(683, 66);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 47;
		this.label3.Text = "Vehicle No";
		this.txtGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.Location = new System.Drawing.Point(627, 83);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(55, 23);
		this.txtGpNo.TabIndex = 11;
		this.txtGpNo.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(627, 66);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(38, 15);
		this.label2.TabIndex = 47;
		this.label2.Text = "GpNo";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(797, 38);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(797, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(599, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(2, 66);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(83, 83);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(83, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1204, 161);
		this.panel1.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.txtSupplierGLId);
		this.groupBox2.Controls.Add(this.txtCommAgentId);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.lblTaxInvoice);
		this.groupBox2.Controls.Add(this.txtBranchSrNo);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtBillAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.groupBox2.Controls.Add(this.lblTaxAccount);
		this.groupBox2.Controls.Add(this.txtTaxInvoiceNo);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 55);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(940, 102);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(89, 72);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(529, 23);
		this.txtremarks.TabIndex = 4;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(646, 70);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.TabStop = false;
		this.txtSupplierGLId.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(624, 70);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.TabStop = false;
		this.txtCommAgentId.Visible = false;
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(89, 43);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 3;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(89, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(74, 23);
		this.txtdocno.TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 49);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Supplier Name";
		this.lblTaxInvoice.AutoSize = true;
		this.lblTaxInvoice.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblTaxInvoice.Location = new System.Drawing.Point(621, 21);
		this.lblTaxInvoice.Name = "lblTaxInvoice";
		this.lblTaxInvoice.Size = new System.Drawing.Size(86, 15);
		this.lblTaxInvoice.TabIndex = 68;
		this.lblTaxInvoice.Text = "Tax Invoice No";
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(164, 17);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(68, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(3, 74);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(62, 19);
		this.label24.TabIndex = 57;
		this.label24.Text = "Remarks";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(234, 17);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(124, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbillno.Location = new System.Drawing.Point(453, 17);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(165, 23);
		this.txtbillno.TabIndex = 5;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(362, 46);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(89, 20);
		this.label51.TabIndex = 60;
		this.label51.Text = "Bill Amount";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(76, 15);
		this.label5.TabIndex = 24;
		this.label5.Text = "Doc No,Date";
		this.txtBillAmount.BackColor = System.Drawing.Color.White;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f);
		this.txtBillAmount.Location = new System.Drawing.Point(453, 43);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(165, 27);
		this.txtBillAmount.TabIndex = 6;
		this.txtBillAmount.TabStop = false;
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(712, 43);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 8;
		this.lblTaxAccount.AutoSize = true;
		this.lblTaxAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblTaxAccount.Location = new System.Drawing.Point(621, 49);
		this.lblTaxAccount.Name = "lblTaxAccount";
		this.lblTaxAccount.Size = new System.Drawing.Size(73, 15);
		this.lblTaxAccount.TabIndex = 65;
		this.lblTaxAccount.Text = "Tax Account";
		this.txtTaxInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxInvoiceNo.Location = new System.Drawing.Point(712, 17);
		this.txtTaxInvoiceNo.Name = "txtTaxInvoiceNo";
		this.txtTaxInvoiceNo.ReadOnly = true;
		this.txtTaxInvoiceNo.Size = new System.Drawing.Size(219, 23);
		this.txtTaxInvoiceNo.TabIndex = 7;
		this.txtTaxInvoiceNo.TabStop = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(362, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual Bill No";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.ChkPrintSlip);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1204, 26);
		this.panel3.TabIndex = 29;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(2, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(277, 19);
		this.label10.TabIndex = 63;
		this.label10.Text = "PURCHASE DIRECT INVOICE       ";
		this.ChkPrintSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Checked = true;
		this.ChkPrintSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintSlip.Location = new System.Drawing.Point(980, 4);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(90, 20);
		this.ChkPrintSlip.TabIndex = 0;
		this.ChkPrintSlip.Text = "Print Slip";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1072, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(124, 20);
		this.ChkBok.TabIndex = 1;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.btnAttachment, this.btnPrint, this.btnSlip, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1204, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "118-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(81, 22);
		this.btnSlip.Text = "233-Slip";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(122, 22);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1210, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 506);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1204, 173);
		this.panel10.TabIndex = 54;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1204, 173);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label17);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 483);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1204, 23);
		this.panel14.TabIndex = 53;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(3, 2);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(211, 17);
		this.label17.TabIndex = 1;
		this.label17.Text = "Detail Of Above Selected Row";
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 138);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1204, 345);
		this.panel13.TabIndex = 52;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1204, 345);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel9.Controls.Add(this.groupBox3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 56);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1204, 82);
		this.panel9.TabIndex = 51;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.label58);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(-2, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(950, 74);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(84, 13);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(267, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(3, 47);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(79, 15);
		this.label60.TabIndex = 13;
		this.label60.Text = "From,To Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(408, 43);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(408, 15);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.txtFromDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(216, 43);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(135, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(84, 43);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(131, 23);
		this.FromDateHistory.TabIndex = 1;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(354, 47);
		this.label57.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(39, 15);
		this.label57.TabIndex = 141;
		this.label57.Text = "To No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(354, 19);
		this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(54, 15);
		this.label58.TabIndex = 140;
		this.label58.Text = "From No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 19);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 146;
		this.label1.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(850, 38);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(850, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(769, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(769, 38);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(699, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(64, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(491, 19);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(58, 15);
		this.label19.TabIndex = 142;
		this.label19.Text = "Customer";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(554, 13);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 5;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1204, 53);
		this.panel2.TabIndex = 46;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(5, 29);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(289, 19);
		this.label16.TabIndex = 65;
		this.label16.Text = "PURCHASE INVOICE HISTORY       ";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1155, 26);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1204, 25);
		this.toolStrip2.TabIndex = 28;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(64, 22);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(73, 22);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1218, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmPurchaseInvoiceDirectStore";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPurchaseInvoiceDirectStore_FormClosed);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
