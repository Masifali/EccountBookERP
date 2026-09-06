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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class InvfrmInvSaleInvoiceDirectFlour : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private double SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int Id;

	private bool Approved;

	private bool GetItemagainstwarehouseId;

	private int OutwardFreightAccountId;

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

	private Label label3;

	internal UltraCombo comstockparty;

	internal UltraCombo compartyreference;

	private Label label27;

	private DateTimePicker deliverystartdate;

	private DateTimePicker validate;

	private Label label44;

	private Label label45;

	private TextBox txtdeliverydays;

	internal UltraCombo comborderno;

	private Panel panel5;

	private Panel panel7;

	private Panel panel6;

	private Panel panel8;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grd;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private ToolStripButton btnSlip;

	private CheckBox ChkBok;

	private Label label51;

	private TextBox txtBillAmount;

	private TextBox txtremarks;

	private Label label24;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	private Label label43;

	internal UltraCombo comsupplier;

	private Label label7;

	private TextBox txtSupplierReference;

	private Label label6;

	private Label label12;

	private TextBox txtbillno;

	private Label label5;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private ToolStripLabel toolStripLabel1;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combdeliverytrm;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripLabel toolStripLabel2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdDetail;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	private Label label1;

	internal UltraCombo CmbTransporterAc;

	private Label label2;

	private TextBox txtFreightAmount;

	internal UltraCombo combCommType;

	internal UltraCombo combcommUOM;

	internal UltraCombo combcommAgent;

	private Label label20;

	private Label label26;

	private Label label11;

	private TextBox txtCommissionRemarks;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private GroupBox groupBox1;

	private Label lblBalance;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private TextBox txtWeightCutTotal;

	private Label label52;

	private TextBox txtgatepassno;

	private Button btnAdd;

	private Label label48;

	private TextBox txtvehicleno;

	private Label label47;

	private TextBox txtwtcut;

	private Label label46;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	internal UltraCombo comPackUOM;

	internal UltraCombo comjobLot;

	internal UltraCombo comCropYear;

	internal UltraCombo comPackingType;

	internal UltraCombo comItem;

	private DateTimePicker txtgpdate;

	private Label label42;

	private Label label41;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtGrossWeight;

	private Label label39;

	private TextBox txtQty;

	private Label label38;

	private Label label35;

	private TextBox txtNetBillWeight;

	private Label label36;

	private TextBox txtlabanalysisno;

	private Label label37;

	private Label label32;

	private Label label33;

	private TextBox txtStockWeight;

	private Label label34;

	private TextBox txtAddLss;

	private Label label31;

	private Label label30;

	private Label label28;

	private Label label22;

	private TextBox txtEmptyBagsTotal;

	private Label label16;

	private TextBox txtEmptybagsUnit;

	private Label label15;

	private TextBox txtAmount;

	private Label label14;

	private Label label9;

	public InvfrmInvSaleInvoiceDirectFlour(UserAccount user)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)comsupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtduedays.Text.Trim() == "" || txtduedays.Text.Trim() == null)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetila()
	{
		if (((UltraGridBase)comItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)comCropYear).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field is Required");
			comCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)comPackingType).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			comPackingType.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (((UltraGridBase)comPackUOM).ActiveRow == null)
		{
			MessageBox.Show("Pack Unit Field is Required");
			comPackUOM.Focus();
			return false;
		}
		if (txtNetBillWeight.Text.Trim() == "" || txtNetBillWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetBillWeight.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)comRateUOM).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM Field is Required");
			comRateUOM.Focus();
			return false;
		}
		if (((UltraGridBase)comWarehouse).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		return true;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			string ItemStatus = CommonServices.GetConfigurationByOrgCompandConfigDescription("GetItemsagainstWarehouse");
			if (ItemStatus != string.Empty)
			{
				GetItemagainstwarehouseId = Conversion.ToBool((object)ItemStatus);
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightOutwardAc").ToList();
			if (list.Count > 0)
			{
				int value = int.Parse(list[0].ConfigKey.ToString());
				if (value > 0)
				{
					OutwardFreightAccountId = value;
				}
			}
			dtViewAttachments.Columns.Add("AttchmentPath");
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			AccountsFill();
			OtherItemsBind();
			dtGrid.Columns.Add("SaleOrderId");
			dtGrid.Columns.Add("SaleOrderNo");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("PackUOMId");
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("EmptyBags", typeof(double));
			dtGrid.Columns.Add("EmptyBagsTotal", typeof(double));
			dtGrid.Columns.Add("WeightCut", typeof(double));
			dtGrid.Columns.Add("WeightCutTotal", typeof(double));
			dtGrid.Columns.Add("AddLss");
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("Rate");
			dtGrid.Columns.Add("RateUOMId");
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("WarehouseId");
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("LabSampleNo");
			dtGrid.Columns.Add("GpDate");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("Freight", typeof(double));
			dtGrid.Columns.Add("PurchaseGLAC", typeof(double));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty");
			dtInvExp.Columns.Add("Rate");
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage");
			dtGrdGL.Columns.Add("Qty");
			dtGrdGL.Columns.Add("Rate");
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			bindWareHouse();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			item();
			PackingType();
			Branchlot();
			cropyear();
			DocumentNo();
			TransporterAcFill();
			DocDate.Focus();
			ChkPrintslip.Checked = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			DataTable dt = CommonServices.getActiveWareHouse();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comWarehouse, "Id", "WareHouseName", "Ware House ", ZeroIndex: false);
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void suppliercustomer()
	{
		try
		{
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, comsupplier, "Id", "CompanyName", "Suplier ", false);
				DDL.BindDDLNew(dtSupplier, combcommAgent, "Id", "CompanyName", "Commission Agent", false);
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void item()
	{
		try
		{
			if (GetItemagainstwarehouseId)
			{
				DataTable dt = CommonServices.GetItemsFromWarehouseAllocationByWareHouseId(Conversion.ToInt(comWarehouse.Value));
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, comItem, "Id", "ItemName", "Item", ZeroIndex: false);
					((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				}
				else
				{
					((Control)(object)comItem).Text = string.Empty;
					((UltraGridBase)comItem).DataSource = null;
				}
			}
			else
			{
				DataTable dt2 = CommonServices.ReadAllItems();
				if (dt2.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt2, comItem, "Id", "ItemName", "Item", false);
					((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Supplier Name", false);
				((UltraGridBase)combpttrm).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, combCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)combCommType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, combcommUOM, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)combcommUOM).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingType()
	{
		try
		{
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, comPackingType, "Id", "PackTypeDesc", "Pack Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Branchlot()
	{
		try
		{
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, comjobLot, "Id", "JobLotDescription", "Job Lot ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt(comItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dtCOMMUOM = new DataTable();
				dtCOMMUOM.Columns.Add("Id");
				dtCOMMUOM.Columns.Add("UOMCode");
				dtCOMMUOM.Columns.Add("Equivalent");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtCOMMUOM.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["UOMCode"], lst.Rows[i]["Equivalent"]);
				}
				if (dtCOMMUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtCOMMUOM, comPackUOM, "Id", "UOMCode", "Unit Of Measure ", ZeroIndex: false);
					DDL.BindDDL(dtCOMMUOM, comRateUOM, "Id", "UOMCode", "Rate Unit Of Measure ", ZeroIndex: false);
					((UltraGridBase)comPackUOM).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)comRateUOM).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cropyear()
	{
		try
		{
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, comCropYear, "Id", "CropYear", "Crop Year", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 99,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
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

	private void ItemNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(comborderno.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, comItem, "Id", "ItemName", "Item Name", false);
			}
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 99);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			if (dtAccountlst.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAccountlst, CmbTransporterAc, "Id", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		try
		{
			double CurrStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(comWarehouse.Value),
				ItemId = Conversion.ToInt(comItem.Value),
				JobLotId = Conversion.ToInt(comjobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)comCropYear).Text.Trim()),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				PackingTypeId = Conversion.ToInt(comPackingType.Value),
				stockUOM = Conversion.ToInt(comPackUOM.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		PackUOM();
		AvailableStockGetByItem();
	}

	private void GridComboBind()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
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
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["SaleOrderNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["GrossWeight"].Visible = false;
			grd.RootTable.Columns["EmptyBags"].Visible = false;
			grd.RootTable.Columns["EmptyBagsTotal"].Visible = false;
			grd.RootTable.Columns["WeightCut"].Visible = false;
			grd.RootTable.Columns["WeightCutTotal"].Visible = false;
			grd.RootTable.Columns["AddLss"].Visible = false;
			grd.RootTable.Columns["StockWeight"].Visible = false;
			grd.RootTable.Columns["LabSampleNo"].Visible = false;
			grd.RootTable.Columns["GpDate"].Visible = false;
			grd.RootTable.Columns["GpNo"].Visible = false;
			grd.RootTable.Columns["VehicleNo"].Visible = false;
			grd.RootTable.Columns["BillAmount"].Visible = false;
			grd.RootTable.Columns["Commission"].Visible = false;
			grd.RootTable.Columns["Freight"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,#";
			grd.RootTable.Columns["StockWeight"].FormatString = "#,#";
			grd.RootTable.Columns["Rate"].FormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			grd.RootTable.Columns["BillAmount"].FormatString = "#,#";
			grd.RootTable.Columns["Expense"].FormatString = "#,#";
			grd.RootTable.Columns["Journal"].FormatString = "#,#";
			grd.RootTable.Columns["Commission"].FormatString = "#,#";
			grd.RootTable.Columns["Freight"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Rate"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BillAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Journal"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Commission"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Freight"].TotalFormatString = "#,#";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Expected O, but got Unknown
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToDouble((object)txtFreightAmount.Text.Trim()) > 0.0 && ((UltraGridBase)CmbTransporterAc).ActiveRow == null)
			{
				throw new Exception("Transporter Account field Required");
			}
			InvSaleInvoice purchaseInvoice = new InvSaleInvoice();
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
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if ((Conversion.ToDouble(r5.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r5.Cells["Debit"].Value) > 0) && Conversion.ToInt(r5.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToDouble(r4.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r4.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				return;
			}
			purchaseInvoice.BranchesId = 0;
			purchaseInvoice.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			purchaseInvoice.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			purchaseInvoice.CommissionType = ((Control)(object)combCommType).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			purchaseInvoice.UomScheduleIdCmRate = Conversion.ToString((object)((Control)(object)combcommUOM).Text);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocumentTypeId = 99;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.ProjectsId = 0;
			purchaseInvoice.ReferencePartyId = Conversion.ToInt(compartyreference.Value);
			purchaseInvoice.RemarksHeader = txtremarks.Text.Trim();
			purchaseInvoice.StockPartyId = Conversion.ToInt(comstockparty.Value);
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			purchaseInvoice.TransporterId = Conversion.ToInt(CmbTransporterAc.Value);
			purchaseInvoice.FreightAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			purchaseInvoice.SupplierInvoiceDate = duedate.Value;
			purchaseInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtduedays.Text);
			purchaseInvoice.SupplierReferenceNo = Conversion.ToString((object)txtSupplierReference.Text.Trim());
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			if (grd.RowCount > 0)
			{
				int LineId = 1;
				rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					InvSaleInvoiceDetail Pd = new InvSaleInvoiceDetail();
					Pd.LineId = LineId;
					Pd.SaleOrderId = Conversion.ToInt(r3.Cells["SaleOrderId"].Value);
					Pd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Pd.CropYear = Conversion.ToString(r3.Cells["CropYear"].Value);
					Pd.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Pd.PackingTypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					Pd.ItemUOMId = Conversion.ToInt(r3.Cells["PackUOMId"].Value);
					Pd.ItemQty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
					Pd.GrossWeight = Conversion.ToDouble(r3.Cells["GrossWeight"].Value);
					Pd.EBWeight = Conversion.ToDouble(r3.Cells["EmptyBags"].Value);
					Pd.EBTotalWt = Conversion.ToDouble(r3.Cells["EmptyBagsTotal"].Value);
					Pd.WeightCut = Conversion.ToSingle(r3.Cells["WeightCut"].Value);
					Pd.WeightCutTotal = Conversion.ToSingle(r3.Cells["WeightCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDouble(r3.Cells["AddLss"].Value);
					Pd.NetBillWeight = Conversion.ToDouble(r3.Cells["NetBillWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDouble(r3.Cells["StockWeight"].Value);
					Pd.ItemRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					Pd.UomScheduleIdRate = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
					Pd.ItemAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
					Pd.RateUOM = Conversion.ToDouble(r3.Cells["RateUOM"].Value);
					Pd.WarehouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
					Pd.LabAnalisysNo = Conversion.ToString(r3.Cells["LabSampleNo"].Value);
					Pd.GpDate = Conversion.ToDateTime(r3.Cells["GpDate"].Value);
					Pd.GpNo = Conversion.ToInt(r3.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
					Pd.BillAmount = Conversion.ToDouble(r3.Cells["BillAmount"].Value);
					Pd.ExpenseAmount = Conversion.ToDouble(r3.Cells["Expense"].Value);
					Pd.CommissionAmount = Conversion.ToDouble(r3.Cells["Commission"].Value);
					Pd.FreightAmount = Conversion.ToDouble(r3.Cells["Freight"].Value);
					purchaseInvoice.InvSaleInvoiceDetaillist.Add(Pd);
					LineId++;
				}
				purchaseInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
						PE.InvRevExpItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						purchaseInvoice.InvSaleInvoiceExpenseList.Add(PE);
					}
				}
				purchaseInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r.Cells["Credit"].Value);
						purchaseInvoice.InvSaleInvoiceJournalList.Add(PJ);
					}
				}
				purchaseInvoice.AttachmentsList = new List<DMSAttachments>();
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.ScreenName = base.Name;
					purchaseInvoice.AttachmentsList.Add(attach);
				}
				int sucess = InvSaleInvoice.Save(purchaseInvoice);
				if (Id == 0)
				{
					MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				Reset();
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.VoucherReport_118(VoucherHeadId, 99);
				}
				if (ChkPrintslip.Checked)
				{
					CommonServices.SaleInvoiceDirectItemSlip_294(sucess);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
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
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
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
			Id = 0;
			VoucherHeadId = 0;
			Approved = false;
			AT = new Attachment();
			((Control)(object)comsupplier).Text = string.Empty;
			((Control)(object)comstockparty).Text = string.Empty;
			((Control)(object)compartyreference).Text = string.Empty;
			txtSupplierReference.Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)combcommAgent).Text = string.Empty;
			((Control)(object)combCommType).Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtFreightAmount.Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)comPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEmptybagsUnit.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtwtcut.Text = string.Empty;
			txtWeightCutTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			DocDate.Focus();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			DocumentNo();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
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
			((Control)(object)comPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEmptybagsUnit.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtwtcut.Text = string.Empty;
			txtWeightCutTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comborderno).Text = string.Empty;
		((Control)(object)comItem).Text = string.Empty;
		((Control)(object)comCropYear).Text = string.Empty;
		((Control)(object)comjobLot).Text = string.Empty;
		((Control)(object)comPackingType).Text = string.Empty;
		((Control)(object)comWarehouse).Text = string.Empty;
		txtlabanalysisno.Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		comItem.Focus();
	}

	public void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			ItemAmount = Math.Round(ItemAmount);
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (int.Parse(r.Cells["AccountId"].Value.ToString()) > 0)
				{
					JournalDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
					JournalCredit += Conversion.ToDouble((object)r.Cells["Credit"].Value.ToString());
					SupplierGLDebitAmount = JournalDebit;
					SupplierGLCreditAmount = JournalCredit;
				}
			}
			double FreightAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			BillAmount = ItemAmount + ExpAmount - FreightAmount;
			BillAmount = BillAmount + JournalCredit - JournalDebit;
			if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
			{
				BillAmount -= Conversion.ToDouble((object)txtcommamount.Text.Trim());
				SupplierCommissionAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			else
			{
				SupplierCommissionAmount = 0.0;
			}
			txtBillAmount.Text = Math.Round(BillAmount).ToString();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "Delete";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "Add New";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "Add";
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "Delete";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "Add New";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "Add";
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

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			grdInvExp.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			grdGLedger.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
				obj.Cells["Expense"].Value = (TotalAmount / NetWeight * Weight).ToString();
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
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
				obj.Cells["Commission"].Value = (TotalAmount / NetWeight * Weight).ToString();
				obj.EndEdit();
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			if (TotalAmount > 0.0)
			{
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Freight"].Value = (TotalAmount / NetWeight * Weight).ToString();
				}
				BillProportion();
			}
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
			if (FormValidationDetila())
			{
				double Qty = 0.0;
				if (txtQty.Text != string.Empty && txtQty.Text != null)
				{
					Qty = Convert.ToDouble(txtQty.Text.Trim());
				}
				double EbU = 0.0;
				if (txtEmptybagsUnit.Text != string.Empty && txtEmptybagsUnit.Text != null)
				{
					EbU = Convert.ToDouble(txtEmptybagsUnit.Text.Trim());
				}
				double EBT = 0.0;
				if (txtEmptyBagsTotal.Text != string.Empty && txtEmptyBagsTotal.Text != null)
				{
					EBT = Convert.ToDouble(txtEmptyBagsTotal.Text.Trim());
				}
				string UOM = ((Control)(object)comPackUOM).Text.Trim().ToString();
				double UOMId = 0.0;
				if (((Control)(object)comPackUOM).Text != string.Empty && ((Control)(object)comPackUOM).Text != null)
				{
					UOMId = double.Parse(comPackUOM.Value.ToString());
				}
				if (txtGrossWeight.Text == string.Empty)
				{
					txtGrossWeight.Text = "0";
				}
				if (txtwtcut.Text == string.Empty)
				{
					txtwtcut.Text = "0";
				}
				if (txtWeightCutTotal.Text == string.Empty)
				{
					txtWeightCutTotal.Text = "0";
				}
				if (txtAddLss.Text == string.Empty)
				{
					txtAddLss.Text = "0";
				}
				if (txtNetBillWeight.Text == string.Empty)
				{
					txtNetBillWeight.Text = "0";
				}
				if (txtStockWeight.Text == string.Empty)
				{
					txtStockWeight.Text = "0";
				}
				if (txtRate.Text == string.Empty)
				{
					txtRate.Text = "0";
				}
				if (txtAmount.Text == string.Empty)
				{
					txtAmount.Text = "0";
				}
				if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
				{
					throw new Exception("Please Check Item Amount");
				}
				dtGrid.Rows.Add(comborderno.Value, ((Control)(object)comborderno).Text, comItem.Value, ((Control)(object)comItem).Text, ((Control)(object)comCropYear).Text, comjobLot.Value, ((Control)(object)comjobLot).Text, comPackingType.Value, ((Control)(object)comPackingType).Text, UOMId, UOM, Qty, txtGrossWeight.Text, EbU, EBT, txtwtcut.Text, txtWeightCutTotal.Text, txtAddLss.Text, txtNetBillWeight.Text, txtStockWeight.Text, txtRate.Text, comRateUOM.Value, ((Control)(object)comRateUOM).Text, txtAmount.Text, comWarehouse.Value, ((Control)(object)comWarehouse).Text, txtlabanalysisno.Text, txtgpdate.Value, txtgatepassno.Text, txtvehicleno.Text);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				TotalCommissionAmount();
				CommissionProportion();
				ExpProportion();
				FreightProportion();
				BillAmount();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvSaleInvoice invPI = InvSaleInvoice.GetByID(Id);
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			comsupplier.Value = invPI.SupplierCustomerId;
			compartyreference.Value = invPI.ReferencePartyId;
			comstockparty.Value = invPI.StockPartyId;
			CmbTransporterAc.Value = invPI.TransporterId;
			txtFreightAmount.Text = Conversion.ToString((object)invPI.FreightAmount);
			txtSupplierReference.Text = invPI.SupplierReferenceNo.ToString();
			txtbillno.Text = invPI.ManualBillNo;
			combcommAgent.Value = invPI.CommissionAgentId;
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = Conversion.ToString((object)invPI.CommRate);
			((Control)(object)combcommUOM).Text = Conversion.ToString((object)invPI.UomScheduleIdCmRate);
			txtcommamount.Text = Conversion.ToString((object)invPI.CommAmount);
			txtCommissionRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			txtremarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
			txtBillAmount.Text = Conversion.ToString((object)invPI.BillAmount);
			txtduedays.Text = Conversion.ToString((object)invPI.SupplierInvoiceNo);
			duedate.Value = invPI.SupplierInvoiceDate;
			Approved = invPI.IsApproved;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int k = 0; k < invPI.InvSaleInvoiceDetaillist.Count; k++)
			{
				dtGrid.Rows.Add("", "", invPI.InvSaleInvoiceDetaillist[k].ItemId, invPI.InvSaleInvoiceDetaillist[k].ItemName, invPI.InvSaleInvoiceDetaillist[k].CropYear, invPI.InvSaleInvoiceDetaillist[k].JobLotId, invPI.InvSaleInvoiceDetaillist[k].JobLotDescription, invPI.InvSaleInvoiceDetaillist[k].PackingTypeId, invPI.InvSaleInvoiceDetaillist[k].PackTypeDesc, invPI.InvSaleInvoiceDetaillist[k].ItemUOMId, invPI.InvSaleInvoiceDetaillist[k].UOMCodeItem, invPI.InvSaleInvoiceDetaillist[k].ItemQty, invPI.InvSaleInvoiceDetaillist[k].GrossWeight, invPI.InvSaleInvoiceDetaillist[k].EBWeight, Math.Round(invPI.InvSaleInvoiceDetaillist[k].EBTotalWt), invPI.InvSaleInvoiceDetaillist[k].WeightCut, Math.Round(invPI.InvSaleInvoiceDetaillist[k].WeightCutTotal), invPI.InvSaleInvoiceDetaillist[k].AdLsWeight, invPI.InvSaleInvoiceDetaillist[k].NetBillWeight, invPI.InvSaleInvoiceDetaillist[k].NetStockWeight, invPI.InvSaleInvoiceDetaillist[k].ItemRate, invPI.InvSaleInvoiceDetaillist[k].UomScheduleIdRate, invPI.InvSaleInvoiceDetaillist[k].RateUOM, invPI.InvSaleInvoiceDetaillist[k].ItemAmount, invPI.InvSaleInvoiceDetaillist[k].WarehouseId, invPI.InvSaleInvoiceDetaillist[k].WareHouseName, invPI.InvSaleInvoiceDetaillist[k].LabAnalisysNo, invPI.InvSaleInvoiceDetaillist[k].GpDate, invPI.InvSaleInvoiceDetaillist[k].GpNo, invPI.InvSaleInvoiceDetaillist[k].VehicleNo, invPI.InvSaleInvoiceDetaillist[k].BillAmount, invPI.InvSaleInvoiceDetaillist[k].ExpenseAmount, invPI.InvSaleInvoiceDetaillist[k].JournalAmount, invPI.InvSaleInvoiceDetaillist[k].CommissionAmount, invPI.InvSaleInvoiceDetaillist[k].FreightAmount, invPI.InvSaleInvoiceDetaillist[k].SaleGLAC);
			}
			dtInvExp.Rows.Clear();
			for (int j = 0; j < invPI.InvSaleInvoiceExpenseList.Count; j++)
			{
				dtInvExp.Rows.Add(invPI.InvSaleInvoiceExpenseList[j].InvRevExpItemId, invPI.InvSaleInvoiceExpenseList[j].Qty, invPI.InvSaleInvoiceExpenseList[j].Rate, invPI.InvSaleInvoiceExpenseList[j].Amount, invPI.InvSaleInvoiceExpenseList[j].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int i = 0; i < invPI.InvSaleInvoiceJournalList.Count; i++)
			{
				dtGrdGL.Rows.Add(invPI.InvSaleInvoiceJournalList[i].ChartofAccountId, invPI.InvSaleInvoiceJournalList[i].JvRemarks, invPI.InvSaleInvoiceJournalList[i].JvPrcnt, invPI.InvSaleInvoiceJournalList[i].JvQty, invPI.InvSaleInvoiceJournalList[i].JvRate, invPI.InvSaleInvoiceJournalList[i].JvDebit, invPI.InvSaleInvoiceJournalList[i].JvCredit);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			gridGLSettings();
			CommissionProportion();
			BillAmount();
			tabControl1.SelectedIndex = 0;
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			VoucherHeadIdGet();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			BillProportion();
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
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = 0.0;
			double WeightCutTotal = 0.0;
			string AddLess = "0";
			if (((Control)(object)comPackUOM).Text != string.Empty)
			{
				Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				Qty = double.Parse(txtQty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = double.Parse(txtGrossWeight.Text.Trim().ToString());
			}
			if (txtEmptybagsUnit.Text != string.Empty && txtEmptybagsUnit.Text != null)
			{
				EBUnit = double.Parse(txtEmptybagsUnit.Text.Trim().ToString());
			}
			if (EBUnit > 0.0)
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString();
			}
			else
			{
				EBTotal = 0.0;
			}
			if (txtNetBillWeight.Text != string.Empty && txtNetBillWeight.Text != null)
			{
				double.Parse(txtNetBillWeight.Text.Trim().ToString());
			}
			if (txtStockWeight.Text != string.Empty && txtStockWeight.Text != null)
			{
				double.Parse(txtStockWeight.Text.Trim().ToString());
			}
			if (txtwtcut.Text != string.Empty && txtwtcut.Text != null)
			{
				WeightCut = double.Parse(txtwtcut.Text.Trim().ToString());
				WeightCutTotal = Qty * WeightCut;
				txtWeightCutTotal.Text = WeightCutTotal.ToString();
			}
			else
			{
				txtWeightCutTotal.Text = "0";
			}
			if (txtAddLss.Text != string.Empty && txtAddLss.Text != null)
			{
				AddLess = txtAddLss.Text.Trim().ToString();
			}
			if (AddLess == "-")
			{
				double Weight = GrossWeight - EBTotal - WeightCutTotal;
				txtNetBillWeight.Text = Math.Round(Weight, 2).ToString();
				txtStockWeight.Text = Math.Round(Weight, 2).ToString();
			}
			else
			{
				double Weight2 = GrossWeight - EBTotal - WeightCutTotal + double.Parse(AddLess);
				txtNetBillWeight.Text = Math.Round(Weight2, 2).ToString();
				txtStockWeight.Text = Math.Round(Weight2, 2).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			if (((Control)(object)comPackUOM).Text != string.Empty)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty)
			{
				Qty = double.Parse(txtQty.Text.ToString());
			}
			if (txtGrossWeight.Text != string.Empty)
			{
				GrossWeight = double.Parse(txtGrossWeight.Text.ToString());
			}
			txtGrossWeight.Text = (PackUOM * Qty).ToString();
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
			double RateUOM = 0.0;
			double Rate = 0.0;
			double RateCut = 0.0;
			double NetWeight = 0.0;
			double ItemAmount = 0.0;
			double RateCutTotal = 0.0;
			RateUOM = ((!(((Control)(object)comRateUOM).Text != string.Empty)) ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value));
			if (txtRate.Text.Trim() != string.Empty)
			{
				Rate = double.Parse(txtRate.Text);
			}
			if (txtNetBillWeight.Text.Trim() != string.Empty)
			{
				NetWeight = double.Parse(txtNetBillWeight.Text);
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				RateCutTotal = NetWeight / RateUOM * RateCut;
				ItemAmount -= RateCutTotal;
				txtAmount.Text = Math.Round(ItemAmount).ToString();
			}
			else
			{
				txtAmount.Text = "0";
				txtBillAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			if (txtQty.Text != string.Empty && ((Control)(object)comPackUOM).Text != string.Empty)
			{
				double num = double.Parse(txtQty.Text.Trim().ToString());
				double PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				txtGrossWeight.Text = (num * PackUOM).ToString();
			}
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtQty.Text != string.Empty && ((Control)(object)comPackUOM).Text != string.Empty)
			{
				double num = double.Parse(txtQty.Text.Trim().ToString());
				double PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				txtGrossWeight.Text = (num * PackUOM).ToString();
			}
			else
			{
				txtGrossWeight.Text = "0";
			}
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
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

	private void txtEmptybagsUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtwtcut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLss_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratecut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.DocumentTypeId = 99;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			PI.NoOfRecords = NoOfRecords;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			DataTable dthistory = InvSaleInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				grdHistory.DataSource = dthistory;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
			}
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
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "View";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "View";
			grdHistory.RootTable.Columns["View"].Width = 50;
			grdHistory.RootTable.Columns.Add("Detail");
			grdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdHistory.RootTable.Columns["Detail"].Width = 50;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			CommonServices.VoucherReport_118(VoucherHeadId, 99);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty && ((Control)(object)combCommType).Text == "Flat")
			{
				double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				txtcommamount.Text = CommRate3.ToString();
			}
			if (txtcommrate.Text != string.Empty && (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage"))
			{
				double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
					txtcommamount.Text = Math.Round(CommAmt2).ToString();
				}
			}
			if (txtcommrate.Text != string.Empty && ((Control)(object)combCommType).Text == "Comm Weight")
			{
				double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
				if (Weight != null)
				{
					double num = Conversion.ToDouble((object)Weight.ToString());
					double UOM = Conversion.ToDouble((object)((Control)(object)combcommUOM).Text.Trim());
					double CommAmt = num / UOM * CommRate;
					txtcommamount.Text = Math.Round(CommAmt).ToString();
				}
			}
			CommissionProportion();
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
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(comsupplier.Value))
				{
					txtSupplierGLId.Text = dtSupplier.Rows[i]["GlAccountId"].ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_Leave(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GetAll(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
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
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				comItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		try
		{
			if (comborderno.Value != null && Conversion.ToInt(comborderno.Value) != 0)
			{
				ItemNameFill();
			}
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
				comborderno.Value = item.Cells["SaleOrderId"].Value.ToString();
				comItem.Value = item.Cells["ItemId"].Value.ToString();
				PackUOM();
				((Control)(object)comCropYear).Text = item.Cells["CropYear"].Text.ToString();
				comjobLot.Value = item.Cells["JobLotId"].Value.ToString();
				comPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtQty.Text = item.Cells["ItemQty"].Value.ToString();
				comPackUOM.Value = item.Cells["PackUOMId"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtEmptybagsUnit.Text = item.Cells["EmptyBags"].Value.ToString();
				txtEmptyBagsTotal.Text = item.Cells["EmptyBagsTotal"].Value.ToString();
				txtwtcut.Text = item.Cells["WeightCut"].Value.ToString();
				txtWeightCutTotal.Text = item.Cells["WeightCutTotal"].Value.ToString();
				txtAddLss.Text = item.Cells["AddLss"].Value.ToString();
				txtNetBillWeight.Text = item.Cells["NetBillWeight"].Value.ToString();
				txtStockWeight.Text = item.Cells["StockWeight"].Value.ToString();
				txtRate.Text = item.Cells["Rate"].Value.ToString();
				comRateUOM.Value = item.Cells["RateUOMId"].Value.ToString();
				txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				comWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
				txtgpdate.Text = item.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = item.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = item.Cells["VehicleNo"].Value.ToString();
				txtlabanalysisno.Text = item.Cells["LabSampleNo"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
			if (FormValidationDetila())
			{
				dtGrid.Rows[updateDetailIndex]["SaleOrderId"] = comborderno.Value;
				dtGrid.Rows[updateDetailIndex]["SaleOrderNo"] = ((Control)(object)comborderno).Text;
				dtGrid.Rows[updateDetailIndex]["ItemId"] = comItem.Value;
				dtGrid.Rows[updateDetailIndex]["Item"] = ((Control)(object)comItem).Text;
				dtGrid.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)comCropYear).Text;
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = comjobLot.Value;
				dtGrid.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)comjobLot).Text;
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = comPackingType.Value;
				dtGrid.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)comPackingType).Text;
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = txtQty.Text;
				dtGrid.Rows[updateDetailIndex]["PackUOMId"] = comPackUOM.Value;
				dtGrid.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)comPackUOM).Text;
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeight.Text;
				dtGrid.Rows[updateDetailIndex]["EmptyBags"] = txtEmptybagsUnit.Text;
				dtGrid.Rows[updateDetailIndex]["EmptyBagsTotal"] = txtEmptyBagsTotal.Text;
				dtGrid.Rows[updateDetailIndex]["WeightCut"] = txtwtcut.Text;
				dtGrid.Rows[updateDetailIndex]["WeightCutTotal"] = txtWeightCutTotal.Text;
				dtGrid.Rows[updateDetailIndex]["AddLss"] = txtAddLss.Text;
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = txtNetBillWeight.Text;
				dtGrid.Rows[updateDetailIndex]["StockWeight"] = txtStockWeight.Text;
				dtGrid.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
				dtGrid.Rows[updateDetailIndex]["RateUOMId"] = comRateUOM.Value;
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)comRateUOM).Text;
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = txtAmount.Text;
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = comWarehouse.Value;
				dtGrid.Rows[updateDetailIndex]["Warehouse"] = ((Control)(object)comWarehouse).Text;
				dtGrid.Rows[updateDetailIndex]["LabSampleNo"] = txtlabanalysisno.Text;
				dtGrid.Rows[updateDetailIndex]["GpDate"] = txtgpdate.Value;
				dtGrid.Rows[updateDetailIndex]["GpNo"] = txtgatepassno.Text;
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = txtvehicleno.Text;
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				TotalCommissionAmount();
				CommissionProportion();
				ExpProportion();
				FreightProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			CommonServices.SaleInvoiceDirectItemSlip_294(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
			AccountsFill();
			GridComboBind();
			bindWareHouse();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			item();
			PackingType();
			Branchlot();
			cropyear();
			DocumentNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommUOM_Leave_1(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_Leave_1(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
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

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
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

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt(grdHistory.CurrentRow.Cells[1].Value);
				ReadById(Id);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoiceDirectItemSlip_294(Conversion.ToInt(grdHistory.CurrentRow.Cells[1].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.VoucherReport_118(Conversion.ToInt(grdHistory.CurrentRow.Cells[2].Value), 99);
			}
			if (e.Column.Key == "Detail")
			{
				try
				{
					GetDetailGrdByHeadId();
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("EBTotalWt", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].PackTypeDesc, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].CropYear, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].RateUOM, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].EBTotalWt, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount, obj.InvSaleInvoiceDetaillist[i].GrossWeight);
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
		catch (Exception)
		{
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["ItemName"].Width = 200;
			grdDetail.RootTable.Columns["WareHouseName"].Width = 150;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = "0,0";
			grdDetail.RootTable.Columns["RateCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "0,0";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void combcommAgent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
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
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells[1].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combdeliverytrm).Text == "Ponch")
			{
				CmbTransporterAc.Value = OutwardFreightAccountId;
				return;
			}
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtFreightAmount.Text = "0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				FreightProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comWarehouse_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (GetItemagainstwarehouseId)
			{
				item();
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comCropYear_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comPackingType_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
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

	private void txtEmptybagsUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtStockWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtratecut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcommrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Expected O, but got Unknown
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Expected O, but got Unknown
		//IL_1331: Unknown result type (might be due to invalid IL or missing references)
		//IL_133b: Expected O, but got Unknown
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Expected O, but got Unknown
		//IL_14a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Expected O, but got Unknown
		//IL_14b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c1: Expected O, but got Unknown
		//IL_9c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c31: Expected O, but got Unknown
		//IL_9c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c5f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance161 = new Appearance();
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
		Appearance appearance174 = new Appearance();
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
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
		Appearance appearance78 = new Appearance();
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
		Appearance appearance91 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.comWarehouse = new UltraCombo();
		this.comRateUOM = new UltraCombo();
		this.comPackUOM = new UltraCombo();
		this.comjobLot = new UltraCombo();
		this.comCropYear = new UltraCombo();
		this.comPackingType = new UltraCombo();
		this.comItem = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.txtWeightCutTotal = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.label47 = new System.Windows.Forms.Label();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.comstockparty = new UltraCombo();
		this.compartyreference = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtEmptybagsUnit = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtlabanalysisno = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbTransporterAc = new UltraCombo();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.combpttrm = new UltraCombo();
		this.combCommType = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.combcommUOM = new UltraCombo();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.comsupplier = new UltraCombo();
		this.combcommAgent = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.comborderno = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1054, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1046, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1040, 717);
		this.panel1.TabIndex = 7;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(990, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1040, 293);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 424);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1040, 293);
		this.panel8.TabIndex = 32;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 299);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1040, 125);
		this.panel5.TabIndex = 31;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(564, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(476, 125);
		this.panel7.TabIndex = 1;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(476, 125);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(564, 125);
		this.panel6.TabIndex = 0;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(564, 125);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtdeliverydays);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add(this.txtgatepassno);
		this.panel4.Controls.Add(this.txtWeightCutTotal);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label52);
		this.panel4.Controls.Add(this.txtvehicleno);
		this.panel4.Controls.Add(this.validate);
		this.panel4.Controls.Add(this.label47);
		this.panel4.Controls.Add(this.deliverystartdate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comstockparty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.compartyreference);
		this.panel4.Controls.Add(this.txtGrossWeight);
		this.panel4.Controls.Add(this.txtwtcut);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label46);
		this.panel4.Controls.Add(this.txtgpdate);
		this.panel4.Controls.Add(this.txtEmptybagsUnit);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.txtEmptyBagsTotal);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.txtAddLss);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.txtStockWeight);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.txtlabanalysisno);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 156);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1040, 143);
		this.panel4.TabIndex = 30;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Location = new System.Drawing.Point(7, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1015, 130);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(353, 16);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(0, 15);
		this.lblBalance.TabIndex = 80;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(527, 93);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 25);
		this.btnCancelUpdateDetial.TabIndex = 13;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(462, 93);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(62, 25);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnAdd.BackColor = System.Drawing.Color.White;
		this.btnAdd.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(429, 93);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(29, 25);
		this.btnAdd.TabIndex = 11;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(13, 36);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(204, 92);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(735, 36);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(527, 36);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.comCropYear.AllowNull = (DefaultableBoolean)1;
		this.comCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.comCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCropYear).Location = new System.Drawing.Point(426, 36);
		((System.Windows.Forms.Control)(object)this.comCropYear).Name = "comCropYear";
		((System.Windows.Forms.Control)(object)this.comCropYear).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.comCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Leave += new System.EventHandler(comCropYear_Leave);
		this.comPackingType.AllowNull = (DefaultableBoolean)1;
		this.comPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackingType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackingType).Location = new System.Drawing.Point(632, 36);
		((System.Windows.Forms.Control)(object)this.comPackingType).Name = "comPackingType";
		((System.Windows.Forms.Control)(object)this.comPackingType).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.comPackingType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.comPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(178, 36);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(118, 73);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(31, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(425, 16);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(59, 15);
		this.label41.TabIndex = 53;
		this.label41.Text = "Crop Year";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(116, 94);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(85, 23);
		this.txtRate.TabIndex = 8;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(807, 38);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(81, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(804, 16);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(13, 94);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(100, 23);
		this.txtNetBillWeight.TabIndex = 7;
		this.txtNetBillWeight.TabStop = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(12, 73);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(89, 15);
		this.label36.TabIndex = 43;
		this.label36.Text = "Net Bill Weight";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(175, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(31, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(527, 16);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(44, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(9, 16);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(71, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(734, 16);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(64, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "Pack UOM";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(207, 73);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(62, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(292, 94);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(130, 23);
		this.txtAmount.TabIndex = 10;
		this.txtAmount.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(289, 73);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(49, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(629, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 19;
		this.label9.Text = "Packing Type";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(1165, 163);
		this.txtdeliverydays.Multiline = true;
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(44, 26);
		this.txtdeliverydays.TabIndex = 1;
		this.txtdeliverydays.Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1071, 140);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(66, 17);
		this.label45.TabIndex = 45;
		this.label45.Text = "Start Date";
		this.label45.Visible = false;
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(1575, 103);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(77, 25);
		this.txtgatepassno.TabIndex = 20;
		this.txtgatepassno.Visible = false;
		this.txtWeightCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtWeightCutTotal.Location = new System.Drawing.Point(1600, 49);
		this.txtWeightCutTotal.Name = "txtWeightCutTotal";
		this.txtWeightCutTotal.Size = new System.Drawing.Size(79, 25);
		this.txtWeightCutTotal.TabIndex = 11;
		this.txtWeightCutTotal.TabStop = false;
		this.txtWeightCutTotal.Visible = false;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1219, 144);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(81, 17);
		this.label44.TabIndex = 44;
		this.label44.Text = "Validite Date";
		this.label44.Visible = false;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(1655, 83);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(70, 17);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.label48.Visible = false;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(1603, 26);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(59, 17);
		this.label52.TabIndex = 79;
		this.label52.Text = "Cut Total";
		this.label52.Visible = false;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(1655, 103);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(100, 25);
		this.txtvehicleno.TabIndex = 21;
		this.txtvehicleno.Visible = false;
		this.validate.CustomFormat = "dd-MM-yy";
		this.validate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1314, 140);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(43, 25);
		this.validate.TabIndex = 19;
		this.validate.Visible = false;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(1578, 83);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(36, 17);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.label47.Visible = false;
		this.deliverystartdate.CustomFormat = "dd-MM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(1166, 136);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(43, 25);
		this.deliverystartdate.TabIndex = 21;
		this.deliverystartdate.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1071, 168);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(86, 17);
		this.label27.TabIndex = 0;
		this.label27.Text = "Delivery Days";
		this.label27.Visible = false;
		this.comstockparty.AllowNull = (DefaultableBoolean)1;
		this.comstockparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.comstockparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comstockparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comstockparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comstockparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comstockparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comstockparty).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comstockparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comstockparty).Location = new System.Drawing.Point(1377, 172);
		((System.Windows.Forms.Control)(object)this.comstockparty).Name = "comstockparty";
		((System.Windows.Forms.Control)(object)this.comstockparty).Size = new System.Drawing.Size(62, 27);
		((System.Windows.Forms.Control)(object)this.comstockparty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comstockparty).Visible = false;
		this.compartyreference.AllowNull = (DefaultableBoolean)1;
		this.compartyreference.AutoCompleteMode = (AutoCompleteMode)4;
		this.compartyreference.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.compartyreference.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.compartyreference).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.compartyreference).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.compartyreference).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.compartyreference).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.compartyreference.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.compartyreference).Location = new System.Drawing.Point(1377, 144);
		((System.Windows.Forms.Control)(object)this.compartyreference).Name = "compartyreference";
		((System.Windows.Forms.Control)(object)this.compartyreference).Size = new System.Drawing.Size(62, 27);
		((System.Windows.Forms.Control)(object)this.compartyreference).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.compartyreference).Visible = false;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(1314, 49);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(85, 25);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.Visible = false;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtwtcut.Location = new System.Drawing.Point(1548, 49);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(49, 25);
		this.txtwtcut.TabIndex = 10;
		this.txtwtcut.Visible = false;
		this.txtwtcut.TextChanged += new System.EventHandler(txtwtcut_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1403, 25);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(67, 17);
		this.label15.TabIndex = 23;
		this.label15.Text = "PMW/Unit";
		this.label15.Visible = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1548, 25);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(46, 17);
		this.label46.TabIndex = 67;
		this.label46.Text = "Wt.Cut";
		this.label46.Visible = false;
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(1484, 103);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(90, 25);
		this.txtgpdate.TabIndex = 19;
		this.txtgpdate.Visible = false;
		this.txtEmptybagsUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptybagsUnit.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtEmptybagsUnit.Location = new System.Drawing.Point(1403, 49);
		this.txtEmptybagsUnit.Name = "txtEmptybagsUnit";
		this.txtEmptybagsUnit.Size = new System.Drawing.Size(64, 25);
		this.txtEmptybagsUnit.TabIndex = 8;
		this.txtEmptybagsUnit.Visible = false;
		this.txtEmptybagsUnit.TextChanged += new System.EventHandler(txtEmptybagsUnit_TextChanged);
		this.txtEmptybagsUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptybagsUnit_KeyPress);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1473, 26);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(71, 17);
		this.label16.TabIndex = 25;
		this.label16.Text = "PMW Total";
		this.label16.Visible = false;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(1470, 49);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(75, 25);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.Visible = false;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1311, 26);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(88, 17);
		this.label39.TabIndex = 49;
		this.label39.Text = "Groos Weight";
		this.label39.Visible = false;
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtAddLss.Location = new System.Drawing.Point(1338, 106);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(50, 25);
		this.txtAddLss.TabIndex = 12;
		this.txtAddLss.Visible = false;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1335, 86);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(43, 17);
		this.label30.TabIndex = 31;
		this.label30.Text = "AD/LS";
		this.label30.Visible = false;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtStockWeight.Location = new System.Drawing.Point(1236, 104);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.Size = new System.Drawing.Size(96, 25);
		this.txtStockWeight.TabIndex = 14;
		this.txtStockWeight.Visible = false;
		this.txtStockWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockWeight_KeyPress);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(1483, 83);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(55, 17);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.label35.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(1233, 84);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(83, 17);
		this.label34.TabIndex = 35;
		this.label34.Text = "Stock Weight";
		this.label34.Visible = false;
		this.txtlabanalysisno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlabanalysisno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtlabanalysisno.Location = new System.Drawing.Point(1393, 104);
		this.txtlabanalysisno.Name = "txtlabanalysisno";
		this.txtlabanalysisno.Size = new System.Drawing.Size(88, 25);
		this.txtlabanalysisno.TabIndex = 18;
		this.txtlabanalysisno.Visible = false;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1390, 80);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(91, 17);
		this.label37.TabIndex = 41;
		this.label37.Text = "Lab Analysis #";
		this.label37.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtFreightAmount);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterAc);
		this.panel3.Controls.Add(this.ChkResetOnSave);
		this.panel3.Controls.Add(this.ChkPrintslip);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.txtBillAmount);
		this.panel3.Controls.Add(this.txtremarks);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtduedays);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.duedate);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtSupplierReference);
		this.panel3.Controls.Add(this.txtCommissionRemarks);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.txtbillno);
		this.panel3.Controls.Add(this.txtcommrate);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.txtcommamount);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comborderno);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1040, 131);
		this.panel3.TabIndex = 29;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1674, 74);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(44, 15);
		this.label2.TabIndex = 109;
		this.label2.Text = "Freight";
		this.label2.Visible = false;
		this.txtFreightAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmount.Location = new System.Drawing.Point(1762, 67);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.Size = new System.Drawing.Size(68, 23);
		this.txtFreightAmount.TabIndex = 13;
		this.txtFreightAmount.Visible = false;
		this.txtFreightAmount.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.txtFreightAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmount_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1448, 160);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 107;
		this.label1.Text = "Transporter Ac";
		this.label1.Visible = false;
		this.CmbTransporterAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransporterAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Location = new System.Drawing.Point(1537, 152);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Name = "CmbTransporterAc";
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Size = new System.Drawing.Size(177, 24);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Visible = false;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.Location = new System.Drawing.Point(869, 48);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(153, 19);
		this.ChkResetOnSave.TabIndex = 15;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.Location = new System.Drawing.Point(869, 71);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(77, 19);
		this.ChkPrintslip.TabIndex = 16;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(116, 37);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 3;
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(1537, 95);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(40, 24);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.combCommType).Visible = false;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(1695, 119);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(205, 24);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Visible = false;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(1629, 53);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(32, 24);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Visible = false;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(869, 94);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(100, 19);
		this.ChkBok.TabIndex = 17;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(533, 43);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(71, 15);
		this.label51.TabIndex = 103;
		this.label51.Text = "Bill Amount";
		this.txtBillAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(647, 39);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(217, 23);
		this.txtBillAmount.TabIndex = 5;
		this.txtBillAmount.TabStop = false;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(116, 90);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(748, 23);
		this.txtremarks.TabIndex = 8;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(12, 94);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 43);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Terms";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(424, 65);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(104, 23);
		this.txtduedays.TabIndex = 7;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(326, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(116, 65);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(205, 23);
		this.duedate.TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1592, 125);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.label29.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(12, 69);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(56, 15);
		this.label43.TabIndex = 99;
		this.label43.Text = "Due Date";
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(647, 9);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(1537, 36);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(40, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Visible = false;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(533, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Customer Name";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1485, 38);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(79, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1581, 89);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1485, 95);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.txtSupplierReference.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(1787, 30);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(74, 23);
		this.txtSupplierReference.TabIndex = 6;
		this.txtSupplierReference.Visible = false;
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(1537, 125);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(40, 23);
		this.txtCommissionRemarks.TabIndex = 13;
		this.txtCommissionRemarks.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(326, 43);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(90, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1485, 127);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(92, 15);
		this.label25.TabIndex = 95;
		this.label25.Text = "Comm Remarks";
		this.label25.Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1683, 34);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Cust Reference";
		this.label12.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1485, 67);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(70, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Comm Rate";
		this.label21.Visible = false;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(424, 39);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(104, 23);
		this.txtbillno.TabIndex = 4;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(1537, 66);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(40, 23);
		this.txtcommrate.TabIndex = 9;
		this.txtcommrate.Visible = false;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1583, 59);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "UOM";
		this.label19.Visible = false;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1629, 82);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(32, 23);
		this.txtcommamount.TabIndex = 12;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(326, 15);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(116, 11);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(205, 23);
		this.DocDate.TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(12, 15);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(56, 15);
		this.label17.TabIndex = 91;
		this.label17.Text = "Doc Date";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(424, 11);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(104, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1625, 14);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1645, 14);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.comborderno.AllowNull = (DefaultableBoolean)1;
		this.comborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.comborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.comborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comborderno).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comborderno).Location = new System.Drawing.Point(1575, 14);
		((System.Windows.Forms.Control)(object)this.comborderno).Name = "comborderno";
		((System.Windows.Forms.Control)(object)this.comborderno).Size = new System.Drawing.Size(44, 26);
		((System.Windows.Forms.Control)(object)this.comborderno).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comborderno).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Visible = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Leave += new System.EventHandler(comborderno_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1534, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Po #";
		this.label3.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.btnPrint, this.btnAttachment, this.btnSlip, this.btnDelete, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1040, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(242, 22);
		this.toolStripLabel1.Text = "SALE DIRECT INVOICE  - Flour      ";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(51, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(65, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(51, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(93, 22);
		this.btnPrint.Text = "118-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(90, 22);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(75, 22);
		this.btnSlip.Text = "294-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(60, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1046, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 436);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1040, 284);
		this.panel10.TabIndex = 48;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1040, 284);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1040, 406);
		this.panel9.TabIndex = 47;
		this.grdHistory.AllowDelete = (InheritableBoolean)1;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1040, 406);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1040, 27);
		this.panel2.TabIndex = 46;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(991, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripLabel2, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1040, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel2.Name = "toolStripLabel2";
		this.toolStripLabel2.Size = new System.Drawing.Size(258, 22);
		this.toolStripLabel2.Text = "SALE DIRECT INVOICE HISTORY       ";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(67, 22);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1054, 749);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvfrmInvSaleInvoiceDirectFlour";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
