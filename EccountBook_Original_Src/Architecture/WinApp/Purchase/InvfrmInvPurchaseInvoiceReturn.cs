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
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Purchase.Return;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class InvfrmInvPurchaseInvoiceReturn : Form
{
	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private bool ItemSearchWithNameOrCode;

	private bool HasMultiCurrencyFeature;

	private bool BranchImplemented;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private double SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int Id;

	private bool Approved;

	private bool WagesStatus;

	private bool WagesActiveOrInActive;

	private bool BranchFeature;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private frmLoadGdnForPurchaseReturn LoadSo;

	private UserAccount UserAccount = new UserAccount();

	private frmLoadPurchaseInvoiceForReturn LoadInvoice;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox txtEmptyBagsTotal;

	private Label label16;

	private TextBox txtEmptybagsUnit;

	private Label label15;

	private TextBox txtAmount;

	private Label label14;

	private Label label9;

	private Label label3;

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

	private Label label32;

	private Label label33;

	private TextBox txtStockWeight;

	private Label label34;

	private TextBox txtAddLss;

	private Label label31;

	private Label label30;

	private Label label28;

	private Label label22;

	private DateTimePicker txtgpdate;

	internal UltraCombo comPackUOM;

	internal UltraCombo comjobLot;

	internal UltraCombo comCropYear;

	internal UltraCombo comPackingType;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	private Label label44;

	private TextBox txtwtcut;

	private Label label46;

	private Label label47;

	private Label label48;

	private TextBox txtvehicleno;

	private Label label50;

	private TextBox txtratecuttotal;

	private Label label49;

	private TextBox txtratecut;

	private Button btnAdd;

	private TextBox txtgatepassno;

	internal UltraCombo comborderno;

	private Panel panel5;

	private Panel panel7;

	private Panel panel8;

	private TextBox txtWeightCutTotal;

	private Label label52;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private CheckBox ChkBok;

	private Label label51;

	private TextBox txtBillAmount;

	private Label label24;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	internal UltraCombo comsupplier;

	internal UltraCombo combcommAgent;

	private Label label7;

	private Label label20;

	private Label label26;

	private Label label11;

	private TextBox txtSupplierReference;

	private Label label6;

	private Label label25;

	private Label label12;

	private Label label21;

	private TextBox txtbillno;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	internal UltraCombo combpttrm;

	internal UltraCombo combCommType;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combcommUOM;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private CtrlGrdBar ctrlGrdBar2;

	private CheckBox ChkPrintslip;

	private Label lblBalance;

	private TabControl tabControl2;

	private TabPage tabFreight;

	private TabPage tabOtherExpense;

	private TabPage tabSupplierAddLess;

	private GroupBox groupBox2;

	private RichTextBox txtCommissionRemarks;

	private RichTextBox txtremarks;

	private Panel panel6;

	private Label label2;

	private TextBox txtRemarksDetail;

	internal GridEX grd;

	private Panel panel14;

	private Label label27;

	internal GridEX grdFreight;

	private Panel panel11;

	private Label label5;

	internal GridEX grdInvExp;

	private Panel panel12;

	private Label label8;

	internal GridEX grdGLedger;

	private Panel panel13;

	private Label label23;

	internal GridEX grdDetail;

	private Label label56;

	private RadioButton rdSearchByName;

	private RadioButton rdSearchByCode;

	internal UltraCombo cmbCurrency;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private TextBox txtOrderWeight;

	private TextBox txtOrderQty;

	private Label label58;

	private Label label57;

	private Label label43;

	private Label label54;

	private Label label45;

	private Label label53;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnSlip;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel16;

	private Label label55;

	private Panel panel15;

	private Label label1;

	private Label label59;

	private Panel panel17;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private Label label64;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel18;

	private Panel panel10;

	private Label label37;

	private Panel panel19;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label65;

	private ToolStripButton BtnLoader;

	private ToolStripButton BtnLoadSaleInvoice;

	private CheckBox chkCustomAccounts;

	public InvfrmInvPurchaseInvoiceReturn(UserAccount user)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(1094, 804);
			formright = CommonServices.SetRightsValueInRightsObject("InvfrmPurchaseReturn");
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ChkBok.Enabled = formright.DoHavePrintRights;
			ChkBok.Checked = formright.DoHavePrintRights;
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
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnPurchaseInvoiceReturn"));
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceReturnBranchWise"));
			BranchFeature = CommonServices.GetERPFeatureById(11);
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 59);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			dtAccountlst.Columns.Add("Id", typeof(int));
			dtAccountlst.Columns.Add("AccountTitle", typeof(string));
			AccountsFill();
			OtherItemsBind();
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("GdnId", typeof(int));
			dtGrid.Columns.Add("GdnDetailId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse", typeof(string));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("Item", typeof(string));
			dtGrid.Columns.Add("CropYear", typeof(string));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot", typeof(string));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PackingType", typeof(string));
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("UOM", typeof(string));
			dtGrid.Columns.Add("UomEquivalent", typeof(double));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("EB/Unit", typeof(double));
			dtGrid.Columns.Add("EBTotal", typeof(double));
			dtGrid.Columns.Add("WeightCut", typeof(double));
			dtGrid.Columns.Add("WeightCutTotal", typeof(double));
			dtGrid.Columns.Add("Ad/Ls", typeof(double));
			dtGrid.Columns.Add("NetWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM", typeof(string));
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutTotal", typeof(double));
			dtGrid.Columns.Add("Amount", typeof(double));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo", typeof(string));
			dtGrid.Columns.Add("RemarksDetail", typeof(string));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("RefDocumentType", typeof(string));
			dtGrid.Columns.Add("RefDocId", typeof(int));
			dtGrid.Columns.Add("RefDocNo", typeof(int));
			dtGrid.Columns.Add("RefDocSubId", typeof(int));
			dtGrid.Columns.Add("RefInvoiceQty", typeof(double));
			dtGrid.Columns.Add("RefInvoiceWeight", typeof(double));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("GdnId", typeof(int));
			dtInvExp.Columns.Add("GdnExpId", typeof(int));
			dtInvExp.Columns.Add("GdnNo", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks", typeof(string));
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("Remarks", typeof(string));
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Percentage");
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			HistoryComboBranchFill();
			HistoryComboFill(ValidateBranch: false);
			bindWareHouse();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			item();
			Project();
			PackingType();
			Branchlot();
			braches();
			cropyear();
			DocumentNo();
			DocumentNoBranch();
			CurrencyFill();
			DocDate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			multiCurrencyFeature();
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
			{
				MessageBox.Show("Fcy Amount Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				return false;
			}
		}
		return true;
	}

	private bool FormValidationDetila()
	{
		if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)comCropYear).ActiveRow == null || Conversion.ToInt(comCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			comCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)comPackingType).ActiveRow == null || Conversion.ToInt(comPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			comPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)comPackUOM).ActiveRow == null || Conversion.ToInt(comPackUOM.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			comPackUOM.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetBillWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Weight Field is Required");
			txtNetBillWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtStockWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtStockWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			comRateUOM.Focus();
			return false;
		}
		return true;
	}

	private void bindWareHouse()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(comWarehouse.Value);
			DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = ((BranchFeature && BranchImplemented) ? UserAccount.BranchesId : 0)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comWarehouse, "Id", "WareHouseName", "WareHouse ", ZeroIndex: true);
				((UltraGridBase)comWarehouse).Rows[1].Activate();
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comWarehouse.Value = Id;
				}
				else
				{
					((Control)(object)comWarehouse).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)comWarehouse).DataSource = null;
				((Control)(object)comWarehouse).Text = string.Empty;
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
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			int SuppId = Conversion.ToInt(comsupplier.Value);
			int CommAgentId = Conversion.ToInt(combcommAgent.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, comsupplier, "Id", "CompanyName", "Suplier Name ", true);
				DDL.BindDDLNew(dtSupplier, combcommAgent, "Id", "CompanyName", "Comm Agent", true);
				((UltraGridBase)combcommAgent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)comsupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (SuppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SuppId))
				{
					comsupplier.Value = SuppId;
				}
				else
				{
					((Control)(object)comsupplier).Text = string.Empty;
				}
			}
			if (CommAgentId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CommAgentId))
				{
					combcommAgent.Value = CommAgentId;
				}
				else
				{
					((Control)(object)combcommAgent).Text = string.Empty;
				}
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
			int ItemId = Conversion.ToInt(comItem.Value);
			dtitem.Rows.Clear();
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item", ZeroIndex: true);
			}
			((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (ItemId > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItemId))
				{
					comItem.Value = ItemId;
				}
				else
				{
					((Control)(object)comItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
	}

	private void braches()
	{
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			int PayId = Conversion.ToInt(combpttrm.Value);
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)combpttrm).Rows[1].Activate();
				((UltraGridBase)combpttrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (PayId > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PayId))
				{
					combpttrm.Value = PayId;
				}
				else
				{
					combpttrm.Value = string.Empty;
				}
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
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			int DeliveryId = Conversion.ToInt(combdeliverytrm.Value);
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: true);
			((UltraGridBase)combdeliverytrm).Rows[1].Activate();
			((UltraGridBase)combdeliverytrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (DeliveryId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DeliveryId))
				{
					combdeliverytrm.Value = DeliveryId;
				}
				else
				{
					combdeliverytrm.Value = string.Empty;
				}
			}
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
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CommissionType", typeof(string));
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			int commId = Conversion.ToInt(combCommType.Value);
			DDL.BindDDL(dt, combCommType, "Id", "CommissionType", "Type", ZeroIndex: true);
			((UltraGridBase)combCommType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)combCommType).Rows[1].Activate();
			if (commId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == commId))
				{
					combCommType.Value = commId;
				}
				else
				{
					combCommType.Value = string.Empty;
				}
			}
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
			int cuomId = Conversion.ToInt(combcommUOM.Value);
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, combcommUOM, "Id", "type", "UOM", true);
			((UltraGridBase)combcommUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)combcommUOM).Rows[1].Activate();
			if (cuomId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == cuomId))
				{
					combcommUOM.Value = cuomId;
				}
				else
				{
					combcommUOM.Value = string.Empty;
				}
			}
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
			int PackId = Conversion.ToInt(comPackingType.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, comPackingType, "Id", "PackTypeDesc", "Packing Type ", true);
				((UltraGridBase)comPackingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (PackId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PackId))
				{
					comPackingType.Value = PackId;
				}
				else
				{
					((Control)(object)comPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void AccountsFill()
	{
		try
		{
			int[] excludedIds = new int[6] { 2, 10, 11, 12, 15, 22 };
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup acc) => !excludedIds.Contains(acc.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc2 in enumerable)
			{
				int id = Conversion.ToInt((object)acc2.ChartOfAccountId);
				if (existingIds.Add(id))
				{
					dtAccountlst.Rows.Add(acc2.ChartOfAccountId, acc2.AccountTitle);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)comPackUOM).Text;
			string Rateuom = ((Control)(object)comRateUOM).Text;
			DataTable dt = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			if (dt != null)
			{
				InfragisticsHelper.BindAndRetainSelection(comPackUOM, dt, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
				InfragisticsHelper.BindAndRetainSelection(comRateUOM, dt, "Id", "UOMCode", "Rate Uom", AllColumns: true, Rateuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cropyear()
	{
		try
		{
			DataTable lst = CommonServices.CropYearGetAllService();
			int cropId = Conversion.ToInt(comCropYear.Value);
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, comCropYear, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)comCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (cropId > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == cropId))
				{
					comCropYear.Value = cropId;
				}
				else
				{
					((Control)(object)comCropYear).Text = string.Empty;
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
			int Code = CommonServices.PurchaseInvoiceGenerateCode(59);
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

	private void DocumentNoBranch()
	{
		try
		{
			int Code = CommonServices.PurchaseInvoiceGenerateCodeBranch(59, UserAccount.BranchesId);
			if (Code > 0)
			{
				txtBranchSrNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(comborderno.Value)
			});
			int itmId = Conversion.ToInt(comItem.Value);
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, comItem, "Id", "ItemName", "Item", true);
				((UltraGridBase)comItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (itmId > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == itmId))
				{
					comItem.Value = itmId;
				}
				else
				{
					((Control)(object)comItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			VoucherHead voucherHead = new VoucherHead();
			voucherHead.OrganizationId = UserAccount.OrganizationId;
			voucherHead.CompanyId = UserAccount.CompanyId;
			voucherHead.DocumentTypeId = 59;
			voucherHead.DocumentTypeSrNo = Id;
			VoucherHeadId = VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId(voucherHead);
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

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryComboFill();
				return;
			}
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			((UltraGridBase)cmbSupplierNameHistory).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			UomFromGlobalBind(Conversion.ToInt(comItem.Value));
			AvailableStockGetByItem();
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

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(comItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(comItem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				comItem.Value = ItemId;
				comItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void multiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label56.Visible = true;
				label57.Visible = true;
				label58.Visible = true;
				label43.Visible = true;
				txtremarks.Location = new Point(100, 134);
				txtremarks.Size = new Size(437, 50);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label56.Visible = false;
				label57.Visible = false;
				label58.Visible = false;
				label43.Visible = false;
				txtremarks.Location = new Point(100, 134);
				txtremarks.Size = new Size(652, 50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal amount = Conversion.ToDecimal(obj.Cells["BillAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtOrderWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtOrderQty.Text = "0";
				txtOrderWeight.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				comjobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int BaseCurrency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				cmbCurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = Conversion.ToInt((object)BaseRatelist[0].ConfigKey.ToString());
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			if (clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ItemSearchByCode").ToList().Count > 0)
			{
				ItemSearchWithNameOrCode = Conversion.ToBool((object)BaseRatelist[0].ConfigKey.ToString());
			}
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "rdSearchByName")
			{
				comWarehouse.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "rdSearchByName")
			{
				comWarehouse.Focus();
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
			if (grd.GetRows().Count() > 0)
			{
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GdnId"))
				{
					throw new Exception("Grid has records from Gdn. Manual Entry is not allowed!");
				}
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocId"))
				{
					throw new Exception("Grid has records from Purchase Invoice. Manual Entry is not allowed!");
				}
			}
			if (FormValidationDetila())
			{
				if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
				{
					throw new Exception("Please Check Item Amount");
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
				dtGrid.Rows.Add(0, 0, 0, 0, Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToInt(comItem.Value), ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value, ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value, Conversion.ToString((object)((Control)(object)comCropYear).Text), Conversion.ToInt(comjobLot.Value), Conversion.ToString((object)((Control)(object)comjobLot).Text), Conversion.ToInt(comPackingType.Value), Conversion.ToString((object)((Control)(object)comPackingType).Text), Conversion.ToInt(comPackUOM.Value), Conversion.ToString((object)((Control)(object)comPackUOM).Text), Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtEmptybagsUnit.Text.Trim()), Conversion.ToDouble((object)txtEmptyBagsTotal.Text.Trim()), Conversion.ToDouble((object)txtwtcut.Text), Conversion.ToDouble((object)txtWeightCutTotal.Text), Conversion.ToDouble((object)txtAddLss.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtStockWeight.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToInt(comRateUOM.Value), Conversion.ToString((object)((Control)(object)comRateUOM).Text), Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtratecut.Text), Conversion.ToDouble((object)txtratecuttotal.Text), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToDateTime((object)txtgpdate.Value), Conversion.ToInt((object)txtgatepassno.Text), Conversion.ToString((object)txtvehicleno.Text), Conversion.ToString((object)txtRemarksDetail.Text), Conversion.ToDecimal((object)txtFcyAmount.Text), 0, 0, 0, 0, BranchId, BranchName);
				txtExchangeRate_TextChanged(null, null);
				TotalCommissionAmount();
				FrightCalculations();
				CalculateTotalInformation();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				BillAmount();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
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
			if (FormValidationDetila())
			{
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
				dtGrid.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)comWarehouse).Text);
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
				dtGrid.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value;
				dtGrid.Rows[updateDetailIndex]["Item"] = ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value;
				dtGrid.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)comCropYear).Text);
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(comjobLot.Value);
				dtGrid.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)comjobLot).Text);
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(comPackingType.Value);
				dtGrid.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)comPackingType).Text);
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtQty.Text);
				dtGrid.Rows[updateDetailIndex]["PackUOMId"] = Conversion.ToInt(comPackUOM.Value);
				dtGrid.Rows[updateDetailIndex]["UOM"] = Conversion.ToString((object)((Control)(object)comPackUOM).Text);
				dtGrid.Rows[updateDetailIndex]["UomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtGrid.Rows[updateDetailIndex]["EB/Unit"] = Conversion.ToDouble((object)txtEmptybagsUnit.Text);
				dtGrid.Rows[updateDetailIndex]["EBTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCut"] = Conversion.ToDouble((object)txtwtcut.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCutTotal"] = Conversion.ToDouble((object)txtWeightCutTotal.Text);
				dtGrid.Rows[updateDetailIndex]["Ad/Ls"] = Conversion.ToDouble((object)txtAddLss.Text);
				dtGrid.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetBillWeight.Text);
				dtGrid.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtStockWeight.Text);
				dtGrid.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["RateUOMId"] = Conversion.ToInt(comRateUOM.Value);
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToString((object)((Control)(object)comRateUOM).Text);
				dtGrid.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["RateCut"] = Conversion.ToDouble((object)txtratecut.Text);
				dtGrid.Rows[updateDetailIndex]["RateCutTotal"] = Conversion.ToDouble((object)txtratecuttotal.Text);
				dtGrid.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value);
				dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
				dtGrid.Rows[updateDetailIndex]["RemarksDetail"] = Conversion.ToString((object)txtRemarksDetail.Text);
				dtGrid.Rows[updateDetailIndex]["BranchId"] = BranchId;
				dtGrid.Rows[updateDetailIndex]["BranchName"] = BranchName;
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				TotalCommissionAmount();
				FrightCalculations();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				BillAmount();
				txtExchangeRate_TextChanged(null, null);
				CalculateTotalInformation();
				ResetDetail();
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

	private void grdSettings()
	{
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["GdnId"].Visible = false;
			grd.RootTable.Columns["GdnDetailId"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["UomEquivalent"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocumentType"].Visible = false;
			grd.RootTable.Columns["RefDocId"].Visible = false;
			grd.RootTable.Columns["RefDocNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubId"].Visible = false;
			grd.RootTable.Columns["RefInvoiceQty"].Visible = false;
			grd.RootTable.Columns["RefInvoiceWeight"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["BranchId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["UomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			int GdnId = 0;
			int RefDocId = 0;
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
				RefDocId = Conversion.ToInt(r.Cells["RefDocId"].Value);
			}
			grd.RootTable.Columns["GdnNo"].Visible = GdnId > 0;
			grd.RootTable.Columns["RefDocNo"].Visible = RefDocId > 0;
			grd.RootTable.Columns["RefDocumentType"].Visible = RefDocId > 0;
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EBTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Commission"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Freights"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			if (GdnId == 0)
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Caption = "Delete";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "Delete";
				grd.RootTable.Columns["Delete"].Width = 50;
				grd.RootTable.Columns["Delete"].Position = 0;
			}
			if (formright.DoHaveUpdateRights)
			{
				grd.RootTable.Columns.Add("Edit");
				grd.RootTable.Columns["Edit"].Caption = "Edit";
				grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Edit"].ButtonText = "Edit";
				grd.RootTable.Columns["Edit"].Width = 50;
				grd.RootTable.Columns["Edit"].Position = 1;
			}
			grd.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				comWarehouse.Value = Conversion.ToInt(item.Cells["WareHouseId"].Value).ToString();
				comItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value).ToString();
				UomFromGlobalBind(Conversion.ToInt(comItem.Value));
				((Control)(object)comCropYear).Text = item.Cells["CropYear"].Text.ToString();
				comjobLot.Value = Conversion.ToInt(item.Cells["JobLotId"].Value).ToString();
				comPackingType.Value = Conversion.ToInt(item.Cells["PackingTypeId"].Value).ToString();
				comPackUOM.Value = Conversion.ToInt(item.Cells["PackUOMId"].Value).ToString();
				txtQty.Text = Conversion.ToDouble(item.Cells["ItemQty"].Value).ToString("#,#.##");
				txtGrossWeight.Text = Conversion.ToDouble(item.Cells["GrossWeight"].Value).ToString("#,#.##");
				txtEmptybagsUnit.Text = Conversion.ToDouble(item.Cells["EB/Unit"].Value).ToString("#,#.##");
				txtEmptyBagsTotal.Text = Conversion.ToDouble(item.Cells["EBTotal"].Value).ToString("#,#.##");
				txtwtcut.Text = Conversion.ToDouble(item.Cells["WeightCut"].Value).ToString("#,#.##");
				txtWeightCutTotal.Text = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value).ToString("#,#.##");
				txtAddLss.Text = Conversion.ToDouble(item.Cells["Ad/Ls"].Value).ToString("#,#.##");
				txtNetBillWeight.Text = Conversion.ToDouble(item.Cells["NetWeight"].Value).ToString("#,#.##");
				txtStockWeight.Text = Conversion.ToDouble(item.Cells["StockWeight"].Value).ToString("#,#.##");
				txtRate.Text = Conversion.ToDouble(item.Cells["ItemRate"].Value).ToString("#,#.##");
				comRateUOM.Value = Conversion.ToInt(item.Cells["RateUOMId"].Value).ToString();
				txtratecut.Text = Conversion.ToDouble(item.Cells["RateCut"].Value).ToString("#,#.##");
				txtratecuttotal.Text = Conversion.ToDouble(item.Cells["RateCutTotal"].Value).ToString("#,#.##");
				txtAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,#.##");
				txtgpdate.Text = item.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = item.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = item.Cells["VehicleNo"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["RemarksDetail"].Value.ToString();
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

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			TotalCommissionAmount();
			FreightProportion();
			BillAmount();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailrow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
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
			if (r != null && (int)r.RowType == 0 && (Conversion.ToInt(r.Cells["Id"].Value) <= 0 || MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No))
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void AddRowInvExpGrid()
	{
		try
		{
			dtInvExp.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, "");
			grdInvExp.DataSource = dtInvExp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.RootTable.Columns["Id"].Visible = false;
			grdInvExp.RootTable.Columns["GdnId"].Visible = false;
			grdInvExp.RootTable.Columns["GdnExpId"].Visible = false;
			grdInvExp.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["GdnId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["GdnExpId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["GdnNo"].EditType = (EditType)0;
			int GdnId = 0;
			GridEXRow r = grdInvExp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
			}
			grdInvExp.RootTable.Columns["GdnNo"].Visible = GdnId > 0;
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Other Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns["Remarks"].Width = 400;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns["Delete"].Position = 0;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Add"].Position = 1;
			grdInvExp.FrozenColumns = 2;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdInvExp);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
				if (item != null && (int)item.RowType == 0 && grdInvExp.CurrentColumn != null)
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
				item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		try
		{
			dtFreight.Rows.Add(0, 0, 0, 0, 0, "");
			grdFreight.DataSource = dtFreight;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["Freight"].Caption = "Debit Amount";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Less To Product";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 300;
			grdFreight.RootTable.Columns["Freight"].Width = 80;
			grdFreight.RootTable.Columns["Remarks"].Width = 500;
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
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdFreight);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFreight.CurrentRow;
				dtFreight.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0 && grdFreight.CurrentColumn != null)
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
				if (item != null && (int)item.RowType == 0 && grdFreight.CurrentColumn != null)
				{
					dtFreight.Rows.Add(item.Cells["Transporter"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
					grdFreight.DataSource = dtFreight;
				}
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

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Freight"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Freight"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					item.Cells["Freight"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
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

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Freight" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns["Remarks"].Width = 300;
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
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdGLedger);
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Round(Math.Abs(TotlaPercentage), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
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

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Debit" || e.Column.Key == "Credit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GridcomboBind()
	{
		try
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
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Expected O, but got Unknown
		//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f66: Expected O, but got Unknown
		//IL_109c: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a3: Expected O, but got Unknown
		//IL_1260: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Expected O, but got Unknown
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
			foreach (GridEXRow r4 in rows)
			{
				if (((Control)(object)combdeliverytrm).Text == "Ponch" || ((Control)(object)combdeliverytrm).Text == "Ponch & PartyWeight" || ((Control)(object)combdeliverytrm).Text == "Ponch & FactoryWeight")
				{
					r4.BeginEdit();
					r4.Cells["Freight"].Value = 0;
					r4.Cells["Transporter"].Value = string.Empty;
					r4.EndEdit();
					continue;
				}
				if (Conversion.ToDouble(r4.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r4.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
				if (Conversion.ToDouble(r4.Cells["Freight"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if ((Conversion.ToDouble(r5.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r5.Cells["Debit"].Value) > 0) && Conversion.ToInt(r5.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against Expense First");
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				return;
			}
			purchaseInvoice.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			purchaseInvoice.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			purchaseInvoice.CommissionType = ((Control)(object)combCommType).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			purchaseInvoice.UomScheduleIdCmRate = Conversion.ToString((object)((Control)(object)combcommUOM).Text);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocumentTypeId = 59;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.ReferencePartyId = 0;
			purchaseInvoice.RemarksHeader = txtremarks.Text.Trim();
			purchaseInvoice.StockPartyId = 0;
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			purchaseInvoice.SupplierInvoiceDate = duedate.Value;
			purchaseInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtduedays.Text);
			purchaseInvoice.SupplierReferenceNo = Conversion.ToString((object)txtSupplierReference.Text);
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.DueDate = duedate.Value;
			purchaseInvoice.PostDate = DateTime.Now;
			purchaseInvoice.SupplierInvoiceDate = DateTime.Now;
			purchaseInvoice.InvoiceWeight = Conversion.ToDecimal((object)txtOrderWeight.Text);
			purchaseInvoice.InvoiceQty = Conversion.ToDecimal((object)txtOrderQty.Text);
			purchaseInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			purchaseInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			purchaseInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			purchaseInvoice.CustomAccounts = Conversion.ToBool((object)chkCustomAccounts.Checked);
			purchaseInvoice.ScreenName = Conversion.ToString((object)"InvfrmPurchaseReturn");
			double GrossWtTotal = 0.0;
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r7 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.PurchaseOrderId = 0;
					Pd.Id = Conversion.ToInt(r7.Cells["Id"].Value);
					Pd.GdnId = Conversion.ToInt(r7.Cells["GdnId"].Value);
					Pd.GdnDetailId = Conversion.ToInt(r7.Cells["GdnDetailId"].Value);
					Pd.GdnDocumentTypeId = 703;
					Pd.GdnNo = Conversion.ToInt(r7.Cells["GdnNo"].Value);
					Pd.WarehouseId = Conversion.ToInt(r7.Cells["WarehouseId"].Value);
					Pd.ItemId = Conversion.ToInt(r7.Cells["ItemId"].Value);
					Pd.CropYear = Conversion.ToString(r7.Cells["CropYear"].Value);
					Pd.JobLotId = Conversion.ToInt(r7.Cells["JobLotId"].Value);
					Pd.PackingTypeId = Conversion.ToInt(r7.Cells["PackingTypeId"].Value);
					Pd.ItemUOMId = Conversion.ToInt(r7.Cells["PackUOMId"].Value);
					Pd.ItemQty = Conversion.ToDouble(r7.Cells["ItemQty"].Value);
					Pd.GrossWeight = Conversion.ToDouble(r7.Cells["GrossWeight"].Value);
					GrossWtTotal += Pd.GrossWeight;
					Pd.EBWeight = Conversion.ToDouble(r7.Cells["EB/Unit"].Value);
					Pd.EBTotalWt = Conversion.ToDouble(r7.Cells["EBTotal"].Value);
					Pd.WeightCut = Conversion.ToSingle(r7.Cells["WeightCut"].Value);
					Pd.WeightCutTotal = Conversion.ToSingle(r7.Cells["WeightCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDouble(r7.Cells["Ad/Ls"].Value);
					Pd.NetBillWeight = Conversion.ToDouble(r7.Cells["NetWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDouble(r7.Cells["StockWeight"].Value);
					Pd.ItemRate = Conversion.ToDouble(r7.Cells["ItemRate"].Value);
					Pd.UomScheduleIdRate = Conversion.ToInt(r7.Cells["RateUOMId"].Value);
					Pd.EquivalentPoRate = Conversion.ToDouble(r7.Cells["RateEquivalent"].Value);
					Pd.RateCut = Conversion.ToDouble(r7.Cells["RateCut"].Value);
					Pd.RateCutAmount = Conversion.ToDouble(r7.Cells["RateCutTotal"].Value);
					Pd.ItemAmount = Conversion.ToDouble(r7.Cells["Amount"].Value);
					Pd.GpDate = Conversion.ToDateTime(r7.Cells["GpDate"].Value);
					Pd.GpNo = Conversion.ToInt(r7.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r7.Cells["VehicleNo"].Value);
					Pd.RemarksDetail = Conversion.ToString(r7.Cells["RemarksDetail"].Value);
					Pd.CommissionAmount = Conversion.ToDouble(r7.Cells["Commission"].Value);
					Pd.ExpenseAmount = Conversion.ToDouble(r7.Cells["Expense"].Value);
					Pd.FreightAmount = Conversion.ToDouble(r7.Cells["Freights"].Value);
					Pd.BillAmount = Conversion.ToDouble(r7.Cells["BillAmount"].Value);
					Pd.FcyAmount = Conversion.ToDecimal(r7.Cells["FcyAmount"].Value);
					Pd.BranchId = Conversion.ToInt(r7.Cells["BranchId"].Value);
					Pd.CurrencyId = purchaseInvoice.CurrencyId;
					Pd.ExchangeRate = purchaseInvoice.ExchangeRate;
					Pd.RefDocumentTypeId = Conversion.ToInt(r7.Cells["RefDocumentTypeId"].Value);
					Pd.RefDocId = Conversion.ToInt(r7.Cells["RefDocId"].Value);
					Pd.RefDocSubId = Conversion.ToInt(r7.Cells["RefDocSubId"].Value);
					if (Conversion.ToInt((object)Pd.WarehouseId) == 0)
					{
						MessageBox.Show("Warehouse Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt((object)Pd.ItemId) == 0)
					{
						MessageBox.Show("Item Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (string.IsNullOrWhiteSpace(Conversion.ToString((object)Pd.CropYear)))
					{
						MessageBox.Show("Crop Year Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt((object)Pd.JobLotId) == 0)
					{
						MessageBox.Show("Job/Lot Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt((object)Pd.PackingTypeId) == 0)
					{
						MessageBox.Show("Packing Type Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt((object)Pd.ItemUOMId) == 0)
					{
						MessageBox.Show("UOM Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)Pd.ItemQty) == 0.0)
					{
						MessageBox.Show("Item Qty Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)Pd.GrossWeight) == 0.0)
					{
						MessageBox.Show("Gross Weight Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)Pd.NetBillWeight) == 0.0)
					{
						MessageBox.Show("Net Weight Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)Pd.NetStockWeight) == 0.0)
					{
						MessageBox.Show("Stock Weight Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)Pd.ItemRate) == 0.0)
					{
						MessageBox.Show("Item Rate Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt((object)Pd.UomScheduleIdRate) == 0)
					{
						MessageBox.Show("Rate UOM Field is Required in Detail Grid And row No: " + (r7.RowIndex + 1));
						return;
					}
					if (Pd.RefDocumentTypeId == 0 && Pd.RefDocId > 0)
					{
						throw new Exception("$Ref DocumentType Id not Found Please Check");
					}
					if (Pd.RefDocumentTypeId > 0 && Pd.RefDocId == 0)
					{
						throw new Exception("$RefDocId not Found Please Check");
					}
					if (Pd.RefDocumentTypeId > 0 && Pd.NetBillWeight > Conversion.ToDouble(r7.Cells["RefInvoiceWeight"].Value))
					{
						throw new Exception("NetBillWeight can not greater than Balance Weight");
					}
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt((object)r3.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.TansporterId = Conversion.ToInt(r3.Cells["Transporter"].Value);
						PF.Percentage = Conversion.ToDouble(r3.Cells["Percentage"].Value);
						PF.FrQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
						PF.FrRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
						PF.FreightAmount = Conversion.ToDouble(r3.Cells["Freight"].Value);
						PF.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
						PE.GdnId = Conversion.ToInt(r2.Cells["GdnId"].Value);
						PE.GdnExpId = Conversion.ToInt(r2.Cells["GdnExpId"].Value);
						PE.InvRevExpItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString((object)r2.Cells["Remarks"].Text.Trim());
						if (PE.Remarks == "0" || PE.Remarks == string.Empty || PE.Remarks == null)
						{
							PE.Remarks = "Other Item:  " + Conversion.ToString((object)r2.Cells["ItemId"].Text.Trim());
						}
						purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
					}
				}
				purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r.Cells["Credit"].Value);
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.AttachmentsList = new List<DMSAttachments>();
				purchaseInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				purchaseInvoice.AttachmentsValues = AttachmentsValues;
				purchaseInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (Id == 0)
				{
					MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
					FormHelper.DeleteAllAttachmentsFromDb(AT, Id, 59, base.Name);
				}
				if (purchaseInvoice.invPurchaseInvoiceDetailList[0].GdnId == 0 && WagesStatus && WagesActiveOrInActive)
				{
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 59;
					frmwagesBillHeader.RefDocId = sucess;
					frmwagesBillHeader.GrossWeightTotal = GrossWtTotal;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				int SupCustId = Conversion.ToInt(comsupplier.Value);
				DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
				Reset();
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.VoucherReport_118(VoucherHeadId, 59);
				}
				if (ChkPrintslip.Checked)
				{
					GenerateReportSlip220A(sucess, SupCustId, Date);
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

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Id);
			txtBranchSrNo.Text = Conversion.ToString((object)invPI.BranchSrNo);
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			comsupplier.Value = invPI.SupplierCustomerId;
			txtSupplierReference.Text = Conversion.ToString((object)invPI.SupplierReferenceNo);
			txtbillno.Text = Conversion.ToString((object)invPI.ManualBillNo);
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
			cmbCurrency.Value = Conversion.ToInt((object)invPI.CurrencyId);
			txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
			txtFcyAmount.Text = Conversion.ToString((object)invPI.FcyAmount);
			txtOrderQty.Text = Conversion.ToString((object)invPI.InvoiceQty);
			txtOrderWeight.Text = Conversion.ToString((object)invPI.InvoiceWeight);
			AttachmentsValues = Conversion.ToString((object)invPI.AttachmentsValues);
			CustomAttachmentsValues = Conversion.ToString((object)invPI.CustomAttachmentsValues);
			chkCustomAccounts.Checked = Conversion.ToBool((object)invPI.CustomAccounts);
			if (invPI.CurrencyId == 0 || invPI.ExchangeRate == 0m)
			{
				DefaultConfigurations();
			}
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int l = 0; l < invPI.invPurchaseInvoiceDetailList.Count; l++)
			{
				dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[l].Id, invPI.invPurchaseInvoiceDetailList[l].GdnId, invPI.invPurchaseInvoiceDetailList[l].GdnDetailId, invPI.invPurchaseInvoiceDetailList[l].GdnNo, invPI.invPurchaseInvoiceDetailList[l].WarehouseId, invPI.invPurchaseInvoiceDetailList[l].WareHouseName, invPI.invPurchaseInvoiceDetailList[l].ItemId, invPI.invPurchaseInvoiceDetailList[l].ItemCode, invPI.invPurchaseInvoiceDetailList[l].ItemName, invPI.invPurchaseInvoiceDetailList[l].CropYear, invPI.invPurchaseInvoiceDetailList[l].JobLotId, invPI.invPurchaseInvoiceDetailList[l].JobLotDescription, invPI.invPurchaseInvoiceDetailList[l].PackingTypeId, invPI.invPurchaseInvoiceDetailList[l].PackTypeDesc, invPI.invPurchaseInvoiceDetailList[l].ItemUOMId, invPI.invPurchaseInvoiceDetailList[l].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[l].ItemUOMEquivalent, invPI.invPurchaseInvoiceDetailList[l].ItemQty, invPI.invPurchaseInvoiceDetailList[l].GrossWeight, invPI.invPurchaseInvoiceDetailList[l].EBWeight, Math.Round(invPI.invPurchaseInvoiceDetailList[l].EBTotalWt), invPI.invPurchaseInvoiceDetailList[l].WeightCut, Math.Round(invPI.invPurchaseInvoiceDetailList[l].WeightCutTotal), invPI.invPurchaseInvoiceDetailList[l].AdLsWeight, invPI.invPurchaseInvoiceDetailList[l].NetBillWeight, invPI.invPurchaseInvoiceDetailList[l].NetStockWeight, invPI.invPurchaseInvoiceDetailList[l].ItemRate, invPI.invPurchaseInvoiceDetailList[l].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[l].RateUom, invPI.invPurchaseInvoiceDetailList[l].EquivalentPoRate, invPI.invPurchaseInvoiceDetailList[l].RateCut, invPI.invPurchaseInvoiceDetailList[l].RateCutAmount, Math.Round(invPI.invPurchaseInvoiceDetailList[l].ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invPI.invPurchaseInvoiceDetailList[l].GpDate, invPI.invPurchaseInvoiceDetailList[l].GpNo, invPI.invPurchaseInvoiceDetailList[l].VehicleNo, invPI.invPurchaseInvoiceDetailList[l].RemarksDetail, invPI.invPurchaseInvoiceDetailList[l].FcyAmount, Math.Round(invPI.invPurchaseInvoiceDetailList[l].BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invPI.invPurchaseInvoiceDetailList[l].CommissionAmount, invPI.invPurchaseInvoiceDetailList[l].FreightAmount, invPI.invPurchaseInvoiceDetailList[l].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[l].BranchId, invPI.invPurchaseInvoiceDetailList[l].BranchName, Conversion.ToInt((object)invPI.invPurchaseInvoiceDetailList[l].RefDocumentTypeId), Conversion.ToString((object)invPI.invPurchaseInvoiceDetailList[l].RefDocumentType), Conversion.ToInt((object)invPI.invPurchaseInvoiceDetailList[l].RefDocId), Conversion.ToInt((object)invPI.invPurchaseInvoiceDetailList[l].RefDocNo), Conversion.ToInt((object)invPI.invPurchaseInvoiceDetailList[l].RefDocSubId), Conversion.ToDouble((object)invPI.invPurchaseInvoiceDetailList[l].RefInvoiceQty), Conversion.ToDouble((object)invPI.invPurchaseInvoiceDetailList[l].RefInvoiceWeight));
				if (((Control)(object)comsupplier).Enabled && invPI.invPurchaseInvoiceDetailList[l].GdnId > 0)
				{
					EnableDisableDetailFields(1, Value: false);
				}
				else if (((Control)(object)comsupplier).Enabled && invPI.invPurchaseInvoiceDetailList[l].RefDocumentTypeId > 0 && ((Control)(object)comsupplier).Enabled && invPI.invPurchaseInvoiceDetailList[l].RefDocId > 0)
				{
					EnableDisableDetailFields(2, Value: false);
				}
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invPI.InvPurchaseInvoiceExpList.Count; k++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[k].Id, invPI.InvPurchaseInvoiceExpList[k].GdnId, invPI.InvPurchaseInvoiceExpList[k].GdnExpId, invPI.InvPurchaseInvoiceExpList[k].GdnNo, invPI.InvPurchaseInvoiceExpList[k].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[k].Qty, invPI.InvPurchaseInvoiceExpList[k].Rate, invPI.InvPurchaseInvoiceExpList[k].Amount, invPI.InvPurchaseInvoiceExpList[k].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoicejournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[j].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[j].JvRemarks, invPI.InvPurchaseInvoicejournalList[j].JvPrcnt, invPI.InvPurchaseInvoicejournalList[j].JvQty, invPI.InvPurchaseInvoicejournalList[j].JvRate, invPI.InvPurchaseInvoicejournalList[j].JvDebit, invPI.InvPurchaseInvoicejournalList[j].JvCredit);
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < invPI.InvPurchaseInvoiceFreightList.Count; i++)
			{
				dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[i].TansporterId, invPI.InvPurchaseInvoiceFreightList[i].Percentage, invPI.InvPurchaseInvoiceFreightList[i].FrQty, invPI.InvPurchaseInvoiceFreightList[i].FrRate, invPI.InvPurchaseInvoiceFreightList[i].FreightAmount, invPI.InvPurchaseInvoiceFreightList[i].Remarks);
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
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			gridGLSettings();
			grdFreightSettings();
			ExpProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			txtExchangeRate_TextChanged(null, null);
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

	private void btnSave_Click(object sender, EventArgs e)
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

	private void Reset()
	{
		try
		{
			Id = 0;
			VoucherHeadId = 0;
			Approved = false;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			((Control)(object)comsupplier).Text = string.Empty;
			txtSupplierReference.Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)combcommAgent).Text = string.Empty;
			((Control)(object)combCommType).Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			txtremarks.Text = string.Empty;
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
			txtratecut.Text = string.Empty;
			txtratecuttotal.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			DocDate.Focus();
			dtFreight.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			txtFcyAmount.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtOrderQty.Text = string.Empty;
			txtOrderWeight.Text = string.Empty;
			AddRowInGLGrid();
			AddRowInvExpGrid();
			AddRowInFreightGrid();
			EnableDisableDetailFields(0);
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			chkCustomAccounts.Checked = false;
			DocumentNo();
			DocumentNoBranch();
			DefaultConfigurations();
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
			txtRemarksDetail.Text = string.Empty;
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
			txtratecut.Text = string.Empty;
			txtratecuttotal.Text = string.Empty;
			txtAmount.Text = string.Empty;
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
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		comItem.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
			AccountsFill();
			CommissionUOMFill();
			GridcomboBind();
			bindWareHouse();
			suppliercustomer();
			item();
			Project();
			PackingType();
			Branchlot();
			braches();
			cropyear();
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmInvPurchaseInvoiceReturn_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click_1(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
			AccountsFill();
			GridcomboBind();
			bindWareHouse();
			suppliercustomer();
			item();
			Project();
			PackingType();
			Branchlot();
			braches();
			cropyear();
			DefaultConfigurations();
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "drdocdate")
			{
				FromDateHistory.Focus();
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
			FromDateHistory.Focus();
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 59;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
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
			DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("BranchSrNo", typeof(int));
				dt.Columns.Add("BranchName");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("ReferenceParty", typeof(string));
				dt.Columns.Add("CommAgent", typeof(string));
				dt.Columns.Add("CommType", typeof(string));
				dt.Columns.Add("CommRate", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CommRemarks", typeof(string));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("ApprovedStatus", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("Remarks", typeof(string));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dthistory.Rows[i]["Id"]), Conversion.ToInt(dthistory.Rows[i]["VoucherHeadId"]), Conversion.ToInt(dthistory.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dthistory.Rows[i]["SupplierCustomerId"]), Conversion.ToInt(dthistory.Rows[i]["DocNo"]), Conversion.ToInt(dthistory.Rows[i]["BranchSrNo"]), Conversion.ToString(dthistory.Rows[i]["BranchName"]), Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dthistory.Rows[i]["SupplierName"]), Conversion.ToString(dthistory.Rows[i]["ManualBillNo"]), Conversion.ToInt(dthistory.Rows[i]["DueDays"]), Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), Conversion.ToString(dthistory.Rows[i]["ReferencePartyName"]), Conversion.ToString(dthistory.Rows[i]["CommissionAgent"]), Conversion.ToString(dthistory.Rows[i]["CommissionType"]), Conversion.ToDouble(dthistory.Rows[i]["CommRate"]), Conversion.ToDouble(dthistory.Rows[i]["CommAmount"]), Conversion.ToString(dthistory.Rows[i]["CommissionRemarks"]), Conversion.ToDouble(dthistory.Rows[i]["BillAmount"]), Conversion.ToString(dthistory.Rows[i]["ApprovedStatus"]), Conversion.ToDateTime(dthistory.Rows[i]["EntryDate"]).ToShortDateString(), Conversion.ToString(dthistory.Rows[i]["EntryUser"]), Conversion.ToDateTime(dthistory.Rows[i]["ModifyDate"]).ToShortDateString(), Conversion.ToString(dthistory.Rows[i]["ModifyUser"]), Conversion.ToInt(dthistory.Rows[i]["NoOfAttachments"]), Conversion.ToString(dthistory.Rows[i]["RemarksHeader"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
				grdDetail.ClearStructure();
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
			grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3, null, null, null, ColumnSetting: false);
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.ReferenceParty;
			grdHistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			grdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants - 10;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant + 15;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Slip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Slip";
			grdHistory.RootTable.Columns["View"].Width = 40;
			grdHistory.RootTable.Columns["View"].Position = 0;
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 40;
				grdHistory.RootTable.Columns["Edit"].Position = 1;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Voucher");
				grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].Width = 60;
				grdHistory.RootTable.Columns["Voucher"].Position = 2;
			}
			grdHistory.FrozenColumns = 3;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
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
			if (item != null && (int)item.RowType == 0 && grdHistory.CurrentColumn != null)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 59);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					Id = id;
					ReadById(Id);
					VoucherHeadIdGet();
					CommissionProportion();
					BillProportion();
				}
				else if (e.Column.Key == "View")
				{
					int SupCustId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					GenerateReportSlip220A(id, SupCustId, Date);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 59);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, id, 59, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("GdnNo", typeof(int));
				dtdetail.Columns.Add("Warehouse", typeof(string));
				dtdetail.Columns.Add("ItemCode", typeof(string));
				dtdetail.Columns.Add("Item", typeof(string));
				dtdetail.Columns.Add("CropYear", typeof(string));
				dtdetail.Columns.Add("JobLot", typeof(string));
				dtdetail.Columns.Add("PackingType");
				dtdetail.Columns.Add("UOM", typeof(string));
				dtdetail.Columns.Add("ItemQty", typeof(double));
				dtdetail.Columns.Add("GrossWeight", typeof(double));
				dtdetail.Columns.Add("EB/Unit", typeof(double));
				dtdetail.Columns.Add("EBTotal", typeof(double));
				dtdetail.Columns.Add("WeightCut", typeof(double));
				dtdetail.Columns.Add("WeightCutTotal", typeof(double));
				dtdetail.Columns.Add("Ad/Ls", typeof(double));
				dtdetail.Columns.Add("NetWeight", typeof(double));
				dtdetail.Columns.Add("StockWeight", typeof(double));
				dtdetail.Columns.Add("ItemRate", typeof(double));
				dtdetail.Columns.Add("RateUOM", typeof(string));
				dtdetail.Columns.Add("RateCut", typeof(double));
				dtdetail.Columns.Add("RateCutAmount", typeof(double));
				dtdetail.Columns.Add("Amount", typeof(double));
				dtdetail.Columns.Add("GpDate", typeof(DateTime));
				dtdetail.Columns.Add("GpNo", typeof(int));
				dtdetail.Columns.Add("VehicleNo", typeof(string));
				dtdetail.Columns.Add("BillAmount", typeof(double));
				dtdetail.Columns.Add("Commission", typeof(double));
				dtdetail.Columns.Add("Freight", typeof(double));
				dtdetail.Columns.Add("Expense", typeof(double));
				dtdetail.Columns.Add("RemarksDetail", typeof(string));
				dtdetail.Columns.Add("ExchangeRate", typeof(decimal));
				dtdetail.Columns.Add("FcyAmount", typeof(decimal));
				dtdetail.Columns.Add("BranchName", typeof(string));
				dtdetail.Columns.Add("RefDocumentType", typeof(string));
				dtdetail.Columns.Add("RefDocNo", typeof(int));
				InvPurchaseInvoice invPI = new InvPurchaseInvoice();
				invPI = InvPurchaseInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
				if (invPI.invPurchaseInvoiceDetailList.Count > 0)
				{
					dtdetail.Rows.Clear();
					for (int i = 0; i < invPI.invPurchaseInvoiceDetailList.Count; i++)
					{
						dtdetail.Rows.Add(invPI.invPurchaseInvoiceDetailList[i].GdnNo, invPI.invPurchaseInvoiceDetailList[i].WareHouseName, invPI.invPurchaseInvoiceDetailList[i].ItemCode, invPI.invPurchaseInvoiceDetailList[i].ItemName, invPI.invPurchaseInvoiceDetailList[i].CropYear, invPI.invPurchaseInvoiceDetailList[i].JobLotDescription, invPI.invPurchaseInvoiceDetailList[i].PackTypeDesc, invPI.invPurchaseInvoiceDetailList[i].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[i].ItemQty, invPI.invPurchaseInvoiceDetailList[i].GrossWeight, invPI.invPurchaseInvoiceDetailList[i].EBWeight, Math.Round(invPI.invPurchaseInvoiceDetailList[i].EBTotalWt), invPI.invPurchaseInvoiceDetailList[i].WeightCut, Math.Round(invPI.invPurchaseInvoiceDetailList[i].WeightCutTotal), invPI.invPurchaseInvoiceDetailList[i].AdLsWeight, invPI.invPurchaseInvoiceDetailList[i].NetBillWeight, invPI.invPurchaseInvoiceDetailList[i].NetStockWeight, invPI.invPurchaseInvoiceDetailList[i].ItemRate, invPI.invPurchaseInvoiceDetailList[i].EquivalentPoRate, invPI.invPurchaseInvoiceDetailList[i].RateCut, invPI.invPurchaseInvoiceDetailList[i].RateCutAmount, Math.Round(invPI.invPurchaseInvoiceDetailList[i].ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invPI.invPurchaseInvoiceDetailList[i].GpDate, invPI.invPurchaseInvoiceDetailList[i].GpNo, invPI.invPurchaseInvoiceDetailList[i].VehicleNo, Math.Round(invPI.invPurchaseInvoiceDetailList[i].BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invPI.invPurchaseInvoiceDetailList[i].CommissionAmount, invPI.invPurchaseInvoiceDetailList[i].FreightAmount, invPI.invPurchaseInvoiceDetailList[i].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[i].RemarksDetail, invPI.invPurchaseInvoiceDetailList[i].ExchangeRate, invPI.invPurchaseInvoiceDetailList[i].FcyAmount, invPI.invPurchaseInvoiceDetailList[i].BranchName, invPI.invPurchaseInvoiceDetailList[i].RefDocumentType, invPI.invPurchaseInvoiceDetailList[i].RefDocNo);
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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
			int GdnId = 0;
			int RefDocId = 0;
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GdnId = Conversion.ToInt(r.Cells["GdnNo"].Value);
				RefDocId = Conversion.ToInt(r.Cells["RefDocNo"].Value);
			}
			grdDetail.RootTable.Columns["GdnNo"].Visible = GdnId > 0;
			grdDetail.RootTable.Columns["RefDocNo"].Visible = RefDocId > 0;
			grdDetail.RootTable.Columns["RefDocumentType"].Visible = RefDocId > 0;
			grdDetail.RootTable.Columns["FcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["ExchangeRate"].Visible = false;
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCut"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["EBTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
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

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (formright.DoHaveUpdateRights)
				{
					Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
					ReadById(Id);
					ExpProportion();
					CommissionProportion();
					BillProportion();
				}
				else
				{
					MessageBox.Show("You don't have right to update");
				}
			}
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
			drdocdate.Checked = true;
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
			HistoryComboBranchFill();
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBranchFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 59);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
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
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)cmbBranchName).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)cmbBranchName).Text = BranchName;
				}
				else
				{
					((Control)(object)cmbBranchName).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
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
			obj.DocumentTypeIds = "59";
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
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
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

	private void GenerateReportSlip220A(int InvoiceId, int SupCustId, DateTime Date)
	{
		try
		{
			CommonServices.PurchaseInvoicettSlip_220A(InvoiceId, SupCustId, 0, Date, Date);
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

	private void btnAttachment_Click_1(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
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

	public void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			ItemAmount = Math.Round(ItemAmount);
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
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
					SupplierGLDebitAmount = JournalDebit;
					SupplierGLCreditAmount = JournalCredit;
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
			BillAmount = ItemAmount + ExpAmount;
			double GlDifference = SupplierGLCreditAmount - SupplierGLDebitAmount;
			double DiffGl = Math.Abs(GlDifference);
			BillAmount = ((!(GlDifference < 0.0)) ? (BillAmount + GlDifference) : (BillAmount - DiffGl));
			BillAmount += TransporterCredit;
			if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
			{
				BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
				SupplierCommissionAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			else
			{
				SupplierCommissionAmount = 0.0;
			}
			txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
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
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetWeight"].Value.ToString());
					obj.Cells["Expense"].Value = TotalAmount / NetWeight * Weight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
					obj2.EndEdit();
				}
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
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				double TotalDiffernce = CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetWeight"].Value.ToString());
					obj.Cells["Freights"].Value = TotalDiffernce / NetWeight * Weight;
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
				r.Cells["BillAmount"].Value = Math.Round(Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Freights"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString()), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
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
			double CommPercent = Conversion.ToDouble((object)txtcommrate.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)combCommType).Text == "Percent")
					{
						double ItemAmount = Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString());
						r.Cells["Commission"].Value = ItemAmount * CommPercent / 100.0;
						r.EndEdit();
					}
					else
					{
						double Weight = Conversion.ToDouble((object)r.Cells["NetWeight"].Value.ToString());
						r.Cells["Commission"].Value = TotalAmount / NetWeight * Weight;
						r.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
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
			if (txtQty.Text != string.Empty && ((UltraGridBase)comPackUOM).ActiveRow != null && Conversion.ToInt(comPackUOM.Value) > 0)
			{
				double num = Conversion.ToDouble((object)txtQty.Text.Trim().ToString());
				double PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				txtGrossWeight.Text = (num * PackUOM).ToString("#,##0.##");
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
			if (txtQty.Text != string.Empty && ((UltraGridBase)comPackUOM).ActiveRow != null && Conversion.ToInt(comPackUOM.Value) > 0)
			{
				double num = Conversion.ToDouble((object)txtQty.Text.Trim());
				double packUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				txtGrossWeight.Text = (num * packUOM).ToString("#,##0.##");
			}
			else
			{
				txtGrossWeight.Text = 0.ToString("#,##0.##");
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
			if (((UltraGridBase)comPackUOM).ActiveRow != null && Conversion.ToInt(comPackUOM.Value) > 0)
			{
				Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim().ToString());
			}
			if (txtEmptybagsUnit.Text != string.Empty && txtEmptybagsUnit.Text != null)
			{
				EBUnit = Conversion.ToDouble((object)txtEmptybagsUnit.Text.Trim().ToString());
			}
			if (EBUnit > 0.0)
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.##");
			}
			else
			{
				EBTotal = 0.0;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.##");
			}
			if (txtNetBillWeight.Text != string.Empty && txtNetBillWeight.Text != null)
			{
				Conversion.ToDouble((object)txtNetBillWeight.Text.Trim().ToString());
			}
			if (txtStockWeight.Text != string.Empty && txtStockWeight.Text != null)
			{
				Conversion.ToDouble((object)txtStockWeight.Text.Trim().ToString());
			}
			if (txtwtcut.Text != string.Empty && txtwtcut.Text != null)
			{
				WeightCut = Conversion.ToDouble((object)txtwtcut.Text.Trim().ToString());
				WeightCutTotal = Qty * WeightCut;
				txtWeightCutTotal.Text = WeightCutTotal.ToString("#,##0.##");
			}
			else
			{
				txtWeightCutTotal.Text = 0.ToString("#,##0.##");
			}
			if (txtAddLss.Text != string.Empty && txtAddLss.Text != null)
			{
				AddLess = txtAddLss.Text.Trim().ToString();
			}
			if (AddLess == "-")
			{
				double Weight = GrossWeight - EBTotal - WeightCutTotal;
				txtNetBillWeight.Text = Math.Round(Weight, 2).ToString("#,##0.##");
				txtStockWeight.Text = Math.Round(Weight, 2).ToString("#,##0.##");
			}
			else
			{
				double Weight2 = GrossWeight - EBTotal - WeightCutTotal + double.Parse(AddLess);
				txtNetBillWeight.Text = Math.Round(Weight2, 2).ToString("#,##0.##");
				txtStockWeight.Text = Math.Round(Weight2, 2).ToString("#,##0.##");
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
			if (((UltraGridBase)comRateUOM).ActiveRow != null && Conversion.ToInt(comRateUOM.Value) > 0)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.ToString());
			}
			if (txtGrossWeight.Text != string.Empty)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.ToString());
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
			RateUOM = ((((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) <= 0) ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value));
			if (txtRate.Text.Trim() != string.Empty)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (txtratecut.Text.Trim() != string.Empty)
			{
				RateCut = Conversion.ToDouble((object)txtratecut.Text);
			}
			if (txtNetBillWeight.Text.Trim() != string.Empty)
			{
				NetWeight = Conversion.ToDouble((object)txtNetBillWeight.Text);
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				RateCutTotal = NetWeight / RateUOM * RateCut;
				txtratecuttotal.Text = Math.Round(RateCutTotal).ToString();
				ItemAmount -= RateCutTotal;
				txtAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
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

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty && ((Control)(object)combCommType).Text == "Flat")
			{
				double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				txtcommamount.Text = CommRate3.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			if (txtcommrate.Text != string.Empty && (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage"))
			{
				double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
					txtcommamount.Text = CommAmt2.ToString(clsGlobalVariables.stringFormatsingle);
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
					txtcommamount.Text = CommAmt.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrightCalculations()
	{
		try
		{
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double ItemQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (ItemAmount > 0.0)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToDouble(item.Cells["Freight"].Value) > 0.0)
					{
						if (Conversion.ToDouble(item.Cells["Percentage"].Value) > 0.0)
						{
							item.BeginEdit();
							double Amount = ItemAmount * Conversion.ToDouble(item.Cells["Percentage"].Value) / 100.0;
							item.Cells["Freight"].Value = Amount;
						}
						else
						{
							item.BeginEdit();
							double Amount2 = Conversion.ToDouble((object)ItemQty) * Conversion.ToDouble(item.Cells["Rate"].Value);
							item.Cells["Freight"].Value = Amount2;
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

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
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

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				lblBalance.Text = CurrStock.ToString("#,##0.##");
			}
			else
			{
				lblBalance.Text = 0.ToString("#,##0.##");
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
			item();
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
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtgatepassno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLss_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHavePrintRights)
			{
				int SupCustId = Conversion.ToInt(comsupplier.Value);
				DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
				GenerateReportSlip220A(Id, SupCustId, Date);
			}
			else
			{
				MessageBox.Show("You don't have Print right!!!");
			}
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
			if (formright.DoHaveUpdateRights)
			{
				CommonServices.VoucherReport_118(VoucherHeadId, 59);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnSave_Click(null, null);
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
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 0)
			{
				comWarehouse.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grd).Focused)
				{
					((Control)(object)grdFreight).Focus();
					tabControl2.SelectedIndex = 0;
				}
				else if (((Control)(object)grdFreight).Focused)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdInvExp).Focus();
				}
				else if (((Control)(object)grdInvExp).Focused)
				{
					tabControl2.SelectedIndex = 2;
					((Control)(object)grdGLedger).Focus();
				}
				else
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 1)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 1)
			{
				((Control)(object)grdHistory).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grdHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdHistory.CurrentColumn != null && grdHistory.CurrentColumn != null)
				{
					int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
					if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
					{
						GridEXRow item = grdHistory.CurrentRow;
						if (item != null && (int)item.RowType == 0)
						{
							CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 59);
						}
					}
					else if (grdHistory.CurrentColumn.Key == "View")
					{
						int SupCustId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						DateTime Date = Conversion.ToDateTime(r.Cells["DocDate"].Value);
						GenerateReportSlip220A(Id, SupCustId, Date);
					}
					else if (grdHistory.CurrentColumn.Key == "Voucher")
					{
						CommonServices.VoucherReport_118(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 59);
					}
					else if (grdHistory.CurrentColumn.Key == "Edit")
					{
						Reset();
						Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
						ReadById(Id);
						VoucherHeadIdGet();
						CommissionProportion();
						BillProportion();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				this.Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(this.Id);
				ExpProportion();
				CommissionProportion();
				BillProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdFreight.CurrentColumn != null)
			{
				Conversion.ToInt(r.Cells["Transporter"].Value);
				if (grdFreight.CurrentColumn.Key == "Delete")
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grd.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
				{
					Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString());
					if (grd.CurrentColumn.Key == "Edit")
					{
						grd_DoubleClick(null, null);
					}
					if (grd.CurrentColumn.Key == "Delete")
					{
						DeleteDetailrow(r);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On WareHouse in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Focus on Fright Grid");
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

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow row = grd.GetRow(0);
				object obj;
				if (row == null)
				{
					obj = null;
				}
				else
				{
					GridEXCell obj2 = row.Cells["GdnId"];
					obj = ((obj2 != null) ? obj2.Value : null);
				}
				int gdnId = Conversion.ToInt(obj);
				object obj3;
				if (row == null)
				{
					obj3 = null;
				}
				else
				{
					GridEXCell obj4 = row.Cells["RefDocId"];
					obj3 = ((obj4 != null) ? obj4.Value : null);
				}
				int RefDocId = Conversion.ToInt(obj3);
				if (gdnId == 0 && RefDocId == 0)
				{
					throw new Exception("You can't load Gdn because a direct entry already exists.");
				}
				if (gdnId == 0 && RefDocId != 0)
				{
					throw new Exception("You can't load Gdn because a entry against Invoice already exists.");
				}
			}
			LoadSo = new frmLoadGdnForPurchaseReturn(UserAccount);
			LoadSo.ShowDialog();
			LoadSo.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadInGridDetail()
	{
		try
		{
			DataTable dt = LoadSo?.dtLoader;
			if (dt == null || dt.Rows.Count == 0)
			{
				return;
			}
			int supplierId = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
			if (grd.GetRows().Count() > 0)
			{
				int currentSupplier = Conversion.ToInt(comsupplier.Value);
				if (supplierId != currentSupplier)
				{
					throw new Exception("Already loaded rows have a different supplier. You can't load rows of a different supplier!");
				}
			}
			EnableDisableDetailFields(1, Value: false);
			comsupplier.Value = supplierId;
			txtremarks.Text = Conversion.ToString(dt.Rows[0]["RemarksHeader"]);
			HashSet<int> existingDetailIds = new HashSet<int>(from GridEXRow row in grd.GetRows()
				select Conversion.ToInt(row.Cells["GdnDetailId"].Value));
			foreach (DataRow dataRow in dt.Rows)
			{
				int detailId = Conversion.ToInt(dataRow["DetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					dtGrid.Rows.Add(0, dataRow["Id"], dataRow["DetailId"], dataRow["DocNo"], dataRow["WarehouseId"], dataRow["WareHouseName"], dataRow["ItemId"], dataRow["ItemCode"], dataRow["ItemName"], dataRow["CropYear"], dataRow["JobLotId"], dataRow["JobLot"], dataRow["PackingTypeId"], dataRow["PackingType"], dataRow["ItemUomId"], dataRow["ItemUom"], dataRow["ItemEquivalent"], dataRow["ItemQty"], dataRow["GrossWeight"], dataRow["EbUnit"], dataRow["EbTotal"], dataRow["WtCut"], dataRow["WtCutTotal"], dataRow["AdLsWeight"], dataRow["NetBillWeight"], dataRow["StockWeight"], 0, dataRow["ItemUomId"], dataRow["ItemUom"], dataRow["ItemEquivalent"], 0, 0, 0, dataRow["GPDate"], dataRow["GpNo"], dataRow["VehicleNo"], dataRow["DetailRemarks"], 0, 0, 0, 0, 0, dataRow["BranchesId"], dataRow["BranchName"]);
				}
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			string GdnIds = string.Join(",", from row in dt.AsEnumerable()
				select row.Field<int>("Id"));
			LoadExpData(GdnIds);
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			ExpProportion();
			CommissionProportion();
			BillProportion();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string ID)
	{
		DataTable dt = InvGdn.GdnExpensesByGdnIds(ID);
		if (dt.Rows.Count <= 0)
		{
			return;
		}
		HashSet<int> existingDetailIds = new HashSet<int>(from GridEXRow row in grdInvExp.GetRows()
			select Conversion.ToInt(row.Cells["GdnExpId"].Value));
		foreach (DataRow dataRow in dt.Rows)
		{
			int detailId = Conversion.ToInt(dataRow["Id"]);
			if (!existingDetailIds.Contains(detailId))
			{
				dtInvExp.Rows.Add(0, dataRow["InvGdnId"], dataRow["Id"], dataRow["GdnNo"], dataRow["ItemId"], dataRow["Qty"], 0, 0, dataRow["Remarks"]);
			}
		}
	}

	private void EnableDisableDetailFields(int Case, bool Value = true)
	{
		try
		{
			((Control)(object)comsupplier).Enabled = true;
			((Control)(object)comWarehouse).Enabled = true;
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comCropYear).Enabled = true;
			((Control)(object)comjobLot).Enabled = true;
			((Control)(object)comPackingType).Enabled = true;
			((Control)(object)comPackUOM).Enabled = true;
			txtQty.Enabled = true;
			txtGrossWeight.Enabled = true;
			txtEmptybagsUnit.Enabled = true;
			txtEmptyBagsTotal.Enabled = true;
			txtwtcut.Enabled = true;
			txtWeightCutTotal.Enabled = true;
			txtAddLss.Enabled = true;
			txtNetBillWeight.Enabled = true;
			txtStockWeight.Enabled = true;
			txtratecut.Enabled = true;
			txtgpdate.Enabled = true;
			txtgatepassno.Enabled = true;
			txtvehicleno.Enabled = true;
			((Control)(object)comRateUOM).Enabled = true;
			txtRate.Enabled = true;
			switch (Case)
			{
			case 1:
				((Control)(object)comsupplier).Enabled = Value;
				((Control)(object)comWarehouse).Enabled = Value;
				((Control)(object)comItem).Enabled = Value;
				((Control)(object)comCropYear).Enabled = Value;
				((Control)(object)comjobLot).Enabled = Value;
				((Control)(object)comPackingType).Enabled = Value;
				((Control)(object)comPackUOM).Enabled = Value;
				txtQty.Enabled = Value;
				txtGrossWeight.Enabled = Value;
				txtEmptybagsUnit.Enabled = Value;
				txtEmptyBagsTotal.Enabled = Value;
				txtwtcut.Enabled = Value;
				txtWeightCutTotal.Enabled = Value;
				txtAddLss.Enabled = Value;
				txtNetBillWeight.Enabled = Value;
				txtStockWeight.Enabled = Value;
				break;
			case 2:
				((Control)(object)comsupplier).Enabled = Value;
				((Control)(object)comItem).Enabled = Value;
				((Control)(object)comRateUOM).Enabled = Value;
				txtRate.Enabled = Value;
				break;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnLoadSaleInvoice_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow row = grd.GetRow(0);
				object obj;
				if (row == null)
				{
					obj = null;
				}
				else
				{
					GridEXCell obj2 = row.Cells["GdnId"];
					obj = ((obj2 != null) ? obj2.Value : null);
				}
				int gdnId = Conversion.ToInt(obj);
				object obj3;
				if (row == null)
				{
					obj3 = null;
				}
				else
				{
					GridEXCell obj4 = row.Cells["RefDocId"];
					obj3 = ((obj4 != null) ? obj4.Value : null);
				}
				int RefDocId = Conversion.ToInt(obj3);
				if (RefDocId == 0 && gdnId == 0)
				{
					throw new Exception("You can't load Invoice because a direct entry already exists.");
				}
				if (RefDocId == 0 && gdnId != 0)
				{
					throw new Exception("You can't load Gdn because an entry against Gdn already exists.");
				}
			}
			LoadInvoice = new frmLoadPurchaseInvoiceForReturn(UserAccount);
			LoadInvoice.ShowDialog();
			LoadInvoice.BringToFront();
			AddInGridDetailFromInvoice();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddInGridDetailFromInvoice()
	{
		try
		{
			DataTable dtData = LoadInvoice.dtLoader;
			if (dtData.Rows.Count <= 0)
			{
				return;
			}
			string InvoiceIds = "";
			foreach (DataRow r in dtData.Rows)
			{
				InvoiceIds = InvoiceIds + "," + Conversion.ToInt(r["Id"]);
			}
			Conversion.ToInt(dtData.Rows[0]["DocumentTypeId"]);
			int CommissionAgentId = Conversion.ToInt(dtData.Rows[0]["CommissionAgentId"]);
			string CommissionType = Conversion.ToString(dtData.Rows[0]["CommissionType"]);
			string CommUom = Conversion.ToString(dtData.Rows[0]["CommUom"]);
			double CommRate = Conversion.ToDouble(dtData.Rows[0]["CommRate"]);
			double CommAmount = Conversion.ToDouble(dtData.Rows[0]["CommAmount"]);
			combcommAgent.Value = CommissionAgentId;
			txtcommrate.Text = CommRate.ToString("#,##.##");
			((Control)(object)combCommType).Text = CommissionType;
			((Control)(object)combcommUOM).Text = CommUom;
			txtcommamount.Text = CommAmount.ToString("#,##.##");
			LoadDataDetailGridFromSaleInvoice(dtData);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			TotalCommissionAmount();
			FreightProportion();
			ExpProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			InvoiceIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridFromSaleInvoice(DataTable invGrn)
	{
		try
		{
			if (invGrn.Rows.Count <= 0)
			{
				return;
			}
			if (((UltraGridBase)comsupplier).ActiveRow != null)
			{
				if (Conversion.ToInt(invGrn.Rows[0]["SupplierCustomerId"]) != Conversion.ToInt(comsupplier.Value))
				{
					throw new Exception("You Can't Load Grn Of Different Party At Same Time");
				}
				if (!GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "RefDocumentTypeId", invGrn.Rows[0]["DocumentTypeId"]))
				{
					throw new Exception("You Can't Load Grn Of Different DocumentType At Same Time");
				}
			}
			comsupplier.Value = invGrn.Rows[0]["SupplierCustomerId"];
			((Control)(object)comsupplier).Enabled = false;
			comsupplier_ValueChanged(null, null);
			((Control)(object)combdeliverytrm).Text = invGrn.Rows[0]["DeliveryTerm"].ToString();
			EnableDisableDetailFields(2, Value: false);
			HashSet<int> existingDetailIds = new HashSet<int>(from GridEXRow row in grd.GetRows()
				select Conversion.ToInt(row.Cells["RefDocSubId"].Value));
			foreach (DataRow dataRow in invGrn.Rows)
			{
				int detailId = Conversion.ToInt(dataRow["DetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					dtGrid.Rows.Add(0, 0, 0, 0, dataRow["WarehouseId"], dataRow["WareHouseName"], dataRow["ItemId"], dataRow["ItemCode"], dataRow["ItemName"], dataRow["CropYear"], dataRow["JobLotId"], dataRow["JobLotCode"], dataRow["PackingTypeId"], dataRow["PackTypeDesc"], dataRow["ItemUOMId"], dataRow["PackUomCode"], dataRow["PackUomEquivalent"], dataRow["BalQty"], dataRow["BalWeight"], dataRow["EBWeight"], dataRow["EBTotalWt"], dataRow["WtCut"], dataRow["WtCutTotal"], dataRow["AdLsWeight"], dataRow["BalWeight"], dataRow["StockWeight"], dataRow["ItemRate"], dataRow["RateUomId"], dataRow["RateUomCode"], dataRow["RateUomEquivalent"], dataRow["RateCut"], dataRow["RateCutAmount"], dataRow["BalAmount"], dataRow["GpDate"], dataRow["GpNo"], dataRow["VehicleNo"], "", 0, 0, 0, 0, 0, dataRow["BranchIdDetail"], dataRow["BranchNameDetail"], dataRow["DocumentTypeId"], dataRow["DocumentType"], dataRow["Id"], dataRow["DocNo"], dataRow["DetailId"], dataRow["BalQty"], dataRow["BalWeight"]);
				}
			}
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
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Expected O, but got Unknown
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Expected O, but got Unknown
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Expected O, but got Unknown
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Expected O, but got Unknown
		//IL_0b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Expected O, but got Unknown
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb6: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Expected O, but got Unknown
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Expected O, but got Unknown
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Expected O, but got Unknown
		//IL_0cbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc9: Expected O, but got Unknown
		//IL_0cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Expected O, but got Unknown
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Expected O, but got Unknown
		//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Expected O, but got Unknown
		//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Expected O, but got Unknown
		//IL_0d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d84: Expected O, but got Unknown
		//IL_0dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e08: Expected O, but got Unknown
		//IL_1b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b16: Expected O, but got Unknown
		//IL_201a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2024: Expected O, but got Unknown
		//IL_2031: Unknown result type (might be due to invalid IL or missing references)
		//IL_203b: Expected O, but got Unknown
		//IL_2048: Unknown result type (might be due to invalid IL or missing references)
		//IL_2052: Expected O, but got Unknown
		//IL_22f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2303: Expected O, but got Unknown
		//IL_2310: Unknown result type (might be due to invalid IL or missing references)
		//IL_231a: Expected O, but got Unknown
		//IL_2327: Unknown result type (might be due to invalid IL or missing references)
		//IL_2331: Expected O, but got Unknown
		//IL_25b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25bc: Expected O, but got Unknown
		//IL_25c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d3: Expected O, but got Unknown
		//IL_25e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ea: Expected O, but got Unknown
		//IL_b5bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_b5c7: Expected O, but got Unknown
		//IL_b5eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_b5f5: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance42 = new Appearance();
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
		Appearance appearance55 = new Appearance();
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
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
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
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance130 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadSaleInvoice = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label27 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabFreight = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.tabOtherExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.tabSupplierAddLess = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label2 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtWeightCutTotal = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.txtratecuttotal = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtratecut = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.comWarehouse = new UltraCombo();
		this.comRateUOM = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.comPackUOM = new UltraCombo();
		this.comjobLot = new UltraCombo();
		this.comCropYear = new UltraCombo();
		this.comPackingType = new UltraCombo();
		this.comItem = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEmptybagsUnit = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.label55 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.RichTextBox();
		this.txtremarks = new System.Windows.Forms.RichTextBox();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.comsupplier = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.combCommType = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.combpttrm = new UltraCombo();
		this.combcommUOM = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.combcommAgent = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.comborderno = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label65 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label56 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabFreight.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel11.SuspendLayout();
		this.tabOtherExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel12.SuspendLayout();
		this.tabSupplierAddLess.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel13.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).BeginInit();
		this.tabPage2.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel9.SuspendLayout();
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
		this.tabControl1.Size = new System.Drawing.Size(1264, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1256, 720);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnPrint, this.btnAttachment, this.btnSlip, this.btnDelete, this.BtnLoader, this.BtnLoadSaleInvoice,
			this.btnShortCutKeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1250, 25);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "118-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click_1);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(97, 22);
		this.btnSlip.Text = "220A-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.BtnLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(78, 22);
		this.BtnLoader.Text = "Load Gdn";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.BtnLoadSaleInvoice.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadSaleInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadSaleInvoice.Name = "BtnLoadSaleInvoice";
		this.BtnLoadSaleInvoice.Size = new System.Drawing.Size(145, 22);
		this.BtnLoadSaleInvoice.Text = "Load &Purchase Invoice";
		this.BtnLoadSaleInvoice.Click += new System.EventHandler(BtnLoadSaleInvoice_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1250, 714);
		this.panel1.TabIndex = 7;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 611);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1250, 103);
		this.panel8.TabIndex = 32;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1250, 81);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar1);
		this.panel14.Controls.Add(this.label27);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1250, 22);
		this.panel14.TabIndex = 108;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1204, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(46, 17);
		this.label27.TabIndex = 66;
		this.label27.Text = "Detail";
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 408);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1250, 203);
		this.panel5.TabIndex = 31;
		this.panel7.Controls.Add(this.tabControl2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1250, 203);
		this.panel7.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabFreight);
		this.tabControl2.Controls.Add(this.tabOtherExpense);
		this.tabControl2.Controls.Add(this.tabSupplierAddLess);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1250, 203);
		this.tabControl2.TabIndex = 1;
		this.tabFreight.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFreight.Controls.Add(this.panel11);
		this.tabFreight.Location = new System.Drawing.Point(4, 25);
		this.tabFreight.Name = "tabFreight";
		this.tabFreight.Padding = new System.Windows.Forms.Padding(3);
		this.tabFreight.Size = new System.Drawing.Size(1242, 174);
		this.tabFreight.TabIndex = 0;
		this.tabFreight.Text = "Less to Product";
		this.tabFreight.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		this.grdFreight.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1236, 146);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1236, 22);
		this.panel11.TabIndex = 105;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(120, 17);
		this.label5.TabIndex = 66;
		this.label5.Text = " Less to Product";
		this.tabOtherExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabOtherExpense.Controls.Add(this.panel12);
		this.tabOtherExpense.Location = new System.Drawing.Point(4, 25);
		this.tabOtherExpense.Name = "tabOtherExpense";
		this.tabOtherExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherExpense.Size = new System.Drawing.Size(1242, 174);
		this.tabOtherExpense.TabIndex = 1;
		this.tabOtherExpense.Text = "Other Expenses";
		this.tabOtherExpense.UseVisualStyleBackColor = true;
		this.grdInvExp.AutomaticSort = false;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1236, 146);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 107;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label8);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1236, 22);
		this.panel12.TabIndex = 106;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(352, 17);
		this.label8.TabIndex = 66;
		this.label8.Text = "Other Expenses Debit to Supplier & Less to Product";
		this.tabSupplierAddLess.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabSupplierAddLess.Controls.Add(this.panel13);
		this.tabSupplierAddLess.Location = new System.Drawing.Point(4, 25);
		this.tabSupplierAddLess.Name = "tabSupplierAddLess";
		this.tabSupplierAddLess.Size = new System.Drawing.Size(1242, 174);
		this.tabSupplierAddLess.TabIndex = 2;
		this.tabSupplierAddLess.Text = "Supplier AddLess";
		this.tabSupplierAddLess.UseVisualStyleBackColor = true;
		this.grdGLedger.AutomaticSort = false;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1242, 152);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label23);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1242, 22);
		this.panel13.TabIndex = 107;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(315, 17);
		this.label23.TabIndex = 66;
		this.label23.Text = "Journal Debit / Credit Entry Against Supplier";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 241);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1250, 167);
		this.panel4.TabIndex = 0;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.rdSearchByCode);
		this.groupBox1.Controls.Add(this.rdSearchByName);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtWeightCutTotal);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtratecuttotal);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.txtratecut);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtwtcut);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtStockWeight);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtAddLss);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtEmptybagsUnit);
		this.groupBox1.Controls.Add(this.label59);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1058, 158);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = " Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(292, 18);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 108;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(233, 18);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 107;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(306, 113);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Remarks";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(306, 130);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(578, 23);
		this.txtRemarksDetail.TabIndex = 23;
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(359, 20);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(81, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "Balance Stock";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(988, 127);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(67, 28);
		this.btnCancelUpdateDetial.TabIndex = 26;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(918, 127);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 28);
		this.btnUpdateDetail.TabIndex = 25;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtWeightCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutTotal.Enabled = false;
		this.txtWeightCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCutTotal.Location = new System.Drawing.Point(213, 87);
		this.txtWeightCutTotal.Name = "txtWeightCutTotal";
		this.txtWeightCutTotal.ShortcutsEnabled = false;
		this.txtWeightCutTotal.Size = new System.Drawing.Size(75, 23);
		this.txtWeightCutTotal.TabIndex = 11;
		this.txtWeightCutTotal.TabStop = false;
		this.txtWeightCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(213, 68);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(73, 15);
		this.label52.TabIndex = 79;
		this.label52.Text = "Wt.Cut Total";
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(125, 130);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.ShortcutsEnabled = false;
		this.txtgatepassno.Size = new System.Drawing.Size(77, 23);
		this.txtgatepassno.TabIndex = 21;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(884, 127);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(37, 28);
		this.btnAdd.TabIndex = 24;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(829, 68);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(80, 15);
		this.label50.TabIndex = 74;
		this.label50.Text = "Rate Cut Total";
		this.txtratecuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecuttotal.Enabled = false;
		this.txtratecuttotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtratecuttotal.Location = new System.Drawing.Point(832, 87);
		this.txtratecuttotal.Name = "txtratecuttotal";
		this.txtratecuttotal.ShortcutsEnabled = false;
		this.txtratecuttotal.Size = new System.Drawing.Size(91, 23);
		this.txtratecuttotal.TabIndex = 18;
		this.txtratecuttotal.TabStop = false;
		this.txtratecuttotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(751, 68);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(51, 15);
		this.label49.TabIndex = 72;
		this.label49.Text = "Rate Cut";
		this.txtratecut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtratecut.Location = new System.Drawing.Point(754, 87);
		this.txtratecut.Name = "txtratecut";
		this.txtratecut.ShortcutsEnabled = false;
		this.txtratecut.Size = new System.Drawing.Size(75, 23);
		this.txtratecut.TabIndex = 17;
		this.txtratecut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtratecut.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtratecut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(204, 113);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtcut.Location = new System.Drawing.Point(146, 87);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.ShortcutsEnabled = false;
		this.txtwtcut.Size = new System.Drawing.Size(64, 23);
		this.txtwtcut.TabIndex = 10;
		this.txtwtcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwtcut.TextChanged += new System.EventHandler(txtwtcut_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(146, 68);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(44, 15);
		this.label46.TabIndex = 67;
		this.label46.Text = "Wt.Cut";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(204, 130);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(100, 23);
		this.txtvehicleno.TabIndex = 22;
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(3, 39);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(671, 85);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(128, 113);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(814, 39);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(556, 39);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.comCropYear.AllowNull = (DefaultableBoolean)1;
		this.comCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.comCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCropYear).Location = new System.Drawing.Point(453, 39);
		((System.Windows.Forms.Control)(object)this.comCropYear).Name = "comCropYear";
		((System.Windows.Forms.Control)(object)this.comCropYear).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.comCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Leave += new System.EventHandler(comCropYear_Leave);
		this.comPackingType.AllowNull = (DefaultableBoolean)1;
		this.comPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackingType).Location = new System.Drawing.Point(701, 39);
		((System.Windows.Forms.Control)(object)this.comPackingType).Name = "comPackingType";
		((System.Windows.Forms.Control)(object)this.comPackingType).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.comPackingType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.comPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance127;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(193, 39);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(257, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(585, 68);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Item Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(452, 20);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(57, 15);
		this.label41.TabIndex = 53;
		this.label41.Text = "Crop Year";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(583, 87);
		this.txtRate.Name = "txtRate";
		this.txtRate.ShortcutsEnabled = false;
		this.txtRate.Size = new System.Drawing.Size(85, 23);
		this.txtRate.TabIndex = 15;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(969, 41);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ShortcutsEnabled = false;
		this.txtGrossWeight.Size = new System.Drawing.Size(85, 23);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(966, 20);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(80, 15);
		this.label39.TabIndex = 49;
		this.label39.Text = "Groos Weight";
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(6, 130);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(117, 23);
		this.txtgpdate.TabIndex = 20;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(885, 41);
		this.txtQty.Name = "txtQty";
		this.txtQty.ShortcutsEnabled = false;
		this.txtQty.Size = new System.Drawing.Size(81, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(885, 20);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Item Qty";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetBillWeight.Location = new System.Drawing.Point(356, 87);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.ShortcutsEnabled = false;
		this.txtNetBillWeight.Size = new System.Drawing.Size(112, 23);
		this.txtNetBillWeight.TabIndex = 13;
		this.txtNetBillWeight.TabStop = false;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(355, 68);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(68, 15);
		this.label36.TabIndex = 43;
		this.label36.Text = "Net Weight";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(190, 20);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(556, 20);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStockWeight.Location = new System.Drawing.Point(471, 87);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ShortcutsEnabled = false;
		this.txtStockWeight.Size = new System.Drawing.Size(110, 23);
		this.txtStockWeight.TabIndex = 14;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockWeight_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(472, 68);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(73, 15);
		this.label34.TabIndex = 35;
		this.label34.Text = "Stock Wight";
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLss.Location = new System.Drawing.Point(290, 87);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.ShortcutsEnabled = false;
		this.txtAddLss.Size = new System.Drawing.Size(64, 23);
		this.txtAddLss.TabIndex = 12;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.txtAddLss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLss_KeyPress);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(3, 20);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(5, 113);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(287, 68);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(38, 15);
		this.label30.TabIndex = 31;
		this.label30.Text = "Ad/Ls";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(813, 20);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(35, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "UOM";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(671, 68);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Enabled = false;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(69, 87);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.ShortcutsEnabled = false;
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(75, 23);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(306, -152);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(52, 15);
		this.label16.TabIndex = 25;
		this.label16.Text = "E.B Total";
		this.txtEmptybagsUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptybagsUnit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptybagsUnit.Location = new System.Drawing.Point(3, 87);
		this.txtEmptybagsUnit.Name = "txtEmptybagsUnit";
		this.txtEmptybagsUnit.ShortcutsEnabled = false;
		this.txtEmptybagsUnit.Size = new System.Drawing.Size(64, 23);
		this.txtEmptybagsUnit.TabIndex = 8;
		this.txtEmptybagsUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptybagsUnit.TextChanged += new System.EventHandler(txtEmptybagsUnit_TextChanged);
		this.txtEmptybagsUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptybagsUnit_KeyPress);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(68, 69);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(54, 15);
		this.label59.TabIndex = 23;
		this.label59.Text = "E.B/Total";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(3, 68);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(50, 15);
		this.label15.TabIndex = 23;
		this.label15.Text = "E.B/Unit";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(925, 87);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.ShortcutsEnabled = false;
		this.txtAmount.Size = new System.Drawing.Size(130, 23);
		this.txtAmount.TabIndex = 19;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(925, 68);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(698, 20);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 19;
		this.label9.Text = "Packing Type";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(918, 18);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(81, 17);
		this.label44.TabIndex = 44;
		this.label44.Text = "Validite Date";
		this.label44.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel16);
		this.panel3.Controls.Add(this.panel15);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comborderno);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1250, 241);
		this.panel3.TabIndex = 0;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.chkCustomAccounts);
		this.panel16.Controls.Add(this.label55);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 22);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1250, 27);
		this.panel16.TabIndex = 107;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(1127, 5);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(120, 19);
		this.chkCustomAccounts.TabIndex = 512;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(4, 6);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(174, 17);
		this.label55.TabIndex = 67;
		this.label55.Text = "Purchase Return invoice";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1250, 22);
		this.panel15.TabIndex = 106;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 17);
		this.label1.TabIndex = 66;
		this.label1.Text = " Less to Product";
		this.groupBox2.Controls.Add(this.txtBranchSrNo);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.txtOrderWeight);
		this.groupBox2.Controls.Add(this.txtOrderQty);
		this.groupBox2.Controls.Add(this.label58);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.txtCommissionRemarks);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.ChkPrintslip);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.ChkBok);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtSupplierReference);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add(this.txtBillAmount);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.txtcommrate);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.txtcommamount);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 48);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1060, 188);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(180, 19);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.ShortcutsEnabled = false;
		this.txtBranchSrNo.Size = new System.Drawing.Size(58, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(338, 51);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(57, 15);
		this.label53.TabIndex = 110;
		this.label53.Text = "Due Date";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(650, 105);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(102, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 14;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(650, 160);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(102, 23);
		this.txtFcyAmount.TabIndex = 16;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmount_KeyPress);
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(650, 133);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(102, 23);
		this.txtExchangeRate.TabIndex = 15;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderWeight.Location = new System.Drawing.Point(650, 47);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.ShortcutsEnabled = false;
		this.txtOrderWeight.Size = new System.Drawing.Size(102, 23);
		this.txtOrderWeight.TabIndex = 12;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderWeight_KeyPress);
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderQty.Location = new System.Drawing.Point(650, 19);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.ShortcutsEnabled = false;
		this.txtOrderQty.Size = new System.Drawing.Size(102, 23);
		this.txtOrderQty.TabIndex = 11;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderQty_KeyPress);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(563, 164);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(72, 15);
		this.label58.TabIndex = 104;
		this.label58.Text = "Fcy Amount";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(563, 137);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(84, 15);
		this.label57.TabIndex = 105;
		this.label57.Text = "Exchange Rate";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(563, 110);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(55, 15);
		this.label43.TabIndex = 106;
		this.label43.Text = "Fcy Code";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(563, 51);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(88, 15);
		this.label54.TabIndex = 108;
		this.label54.Text = "Invoice Weight";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(563, 23);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(68, 15);
		this.label45.TabIndex = 109;
		this.label45.Text = "Invoice Qty";
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(835, 105);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(218, 51);
		this.txtCommissionRemarks.TabIndex = 22;
		this.txtCommissionRemarks.Text = "";
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(100, 134);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(463, 50);
		this.txtremarks.TabIndex = 6;
		this.txtremarks.Text = "";
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.Location = new System.Drawing.Point(837, 162);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(74, 19);
		this.ChkPrintslip.TabIndex = 23;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(100, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.ShortcutsEnabled = false;
		this.txtdocno.Size = new System.Drawing.Size(78, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(930, 162);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(98, 19);
		this.ChkBok.TabIndex = 24;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(240, 19);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(92, 23);
		this.DocDate.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(100, 45);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 3;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(756, 112);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(75, 46);
		this.label25.TabIndex = 95;
		this.label25.Text = "Comm Remarks";
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(963, 45);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(7, 138);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Remarks";
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(429, 74);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 9;
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(835, 74);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Supplier Name";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(100, 106);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(232, 23);
		this.txtbillno.TabIndex = 5;
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
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
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(429, 104);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 10;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 110);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(835, 17);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(756, 23);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Comm Agent";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(563, 80);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(70, 15);
		this.label51.TabIndex = 103;
		this.label51.Text = "Bill Amount";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(912, 80);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(338, 80);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Terms";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(912, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(32, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Type";
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSupplierReference.Location = new System.Drawing.Point(100, 76);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(232, 23);
		this.txtSupplierReference.TabIndex = 4;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(429, 19);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.ShortcutsEnabled = false;
		this.txtduedays.Size = new System.Drawing.Size(134, 23);
		this.txtduedays.TabIndex = 7;
		this.txtduedays.Text = "0";
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtBillAmount.Location = new System.Drawing.Point(650, 75);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.ShortcutsEnabled = false;
		this.txtBillAmount.Size = new System.Drawing.Size(102, 25);
		this.txtBillAmount.TabIndex = 13;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(756, 51);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Comm Rate";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(338, 23);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(61, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days ";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(835, 47);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.ShortcutsEnabled = false;
		this.txtcommrate.Size = new System.Drawing.Size(76, 23);
		this.txtcommrate.TabIndex = 18;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(7, 80);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(90, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Supp Reference";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(756, 80);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "Comm UOM";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(429, 47);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(134, 23);
		this.duedate.TabIndex = 8;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(963, 76);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.ShortcutsEnabled = false;
		this.txtcommamount.Size = new System.Drawing.Size(90, 23);
		this.txtcommamount.TabIndex = 21;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(338, 110);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 91;
		this.label17.Text = "Doc No && Date";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1064, 25);
		this.panel6.TabIndex = 104;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1148, 80);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1148, 48);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1065, 54);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1065, 86);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1351, 44);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1351, 73);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.comborderno.AllowNull = (DefaultableBoolean)1;
		this.comborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.comborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
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
		((System.Windows.Forms.Control)(object)this.comborderno).Location = new System.Drawing.Point(1351, 126);
		((System.Windows.Forms.Control)(object)this.comborderno).Name = "comborderno";
		((System.Windows.Forms.Control)(object)this.comborderno).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.comborderno).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comborderno).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Visible = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Leave += new System.EventHandler(comborderno_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1352, 105);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Po #";
		this.label3.Visible = false;
		this.tabPage2.Controls.Add(this.panel19);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1256, 720);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 566);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1250, 151);
		this.panel19.TabIndex = 54;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1250, 151);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 50;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label37);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 547);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1250, 19);
		this.panel10.TabIndex = 53;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(4, 1);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(200, 16);
		this.label37.TabIndex = 0;
		this.label37.Text = "Detail Of Above Selected Row";
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 135);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1250, 412);
		this.panel18.TabIndex = 52;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1250, 412);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel17.Controls.Add(this.groupBox3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 56);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1250, 79);
		this.panel17.TabIndex = 51;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox3.Controls.Add(this.label65);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.label64);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 6);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1239, 69);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(1, 37);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(1, 21);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(79, 15);
		this.label65.TabIndex = 147;
		this.label65.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1087, 41);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1087, 23);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1006, 23);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1006, 41);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(929, 37);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(72, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(686, 17);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(86, 15);
		this.label60.TabIndex = 142;
		this.label60.Text = "Supplier Name";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(585, 17);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(64, 15);
		this.label61.TabIndex = 141;
		this.label61.Text = "To Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(483, 17);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(79, 15);
		this.label62.TabIndex = 140;
		this.label62.Text = "From Doc No";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(350, 17);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(48, 15);
		this.label63.TabIndex = 139;
		this.label63.Text = "To Date";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(212, 18);
		this.label64.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(63, 15);
		this.label64.TabIndex = 13;
		this.label64.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(686, 37);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(585, 39);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(483, 39);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(350, 39);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(212, 39);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label56);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1250, 26);
		this.panel9.TabIndex = 47;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(3, 2);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(363, 19);
		this.label56.TabIndex = 68;
		this.label56.Text = "PURCHASE RETURN INVOICE HISTORY       ";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1206, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 24);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1250, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnLoadAll, this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1250, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(73, 22);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1264, 749);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvfrmInvPurchaseInvoiceReturn";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvPurchaseInvoiceReturn_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabFreight.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabOtherExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabSupplierAddLess.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
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
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).EndInit();
		this.tabPage2.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
