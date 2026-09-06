// using Architecture.Reports;
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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.PurchaseTrading;
using Architecture.WinApp.Special_Rights.Configurations;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Purchase
{

public class InvfrmPurchasedirectInvoice : Form
{
	private WaitForm waitForm = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtAccountlstForFreight = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtPmItems = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtJobLoat = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCrAccountForEmptyBag = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private bool FormInitialized;

	private int updateDetailIndex = -1;

	private int VoucherHeadId;

	public int RecId;

	private int ScreenId;

	private bool Approved;

	private int DocumentTypeId = 57;

	private string ScreenName = "InvfrmPurchasedirectInvoice";

	private int PurchaseOrderId;

	public static string OrderType = "PurchaseOrder";

	public static string GridIds = "";

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private bool BranchImplemented;

	private bool BranchFeature;

	private bool WagesStatus;

	private int EmptyBagsWeightCutStatus;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WagesActiveOrInActive;

	private bool FreightDebitToExpenses;

	private bool ItemSearchWithNameOrCode;

	private LoadPurchaseOrder LoadPO;

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

	private Label label2;

	private Label label8;

	private TextBox txtSupplierReference;

	private Label label6;

	private Label label12;

	private TextBox txtbillno;

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

	private DateTimePicker txtgpdate;

	internal UltraCombo comstockparty;

	internal UltraCombo compartyreference;

	internal UltraCombo comsupplier;

	internal UltraCombo comPackUOM;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbCropYear;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo comRateUOM;

	internal UltraCombo comCompany;

	private Label label1;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

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

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label27;

	private DateTimePicker deliverystartdate;

	private Label label29;

	private Label label43;

	private DateTimePicker validate;

	private Label label44;

	private Label label45;

	private TextBox txtdeliverydays;

	private Label label24;

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

	internal UltraCombo CmbOrderNo;

	private Panel panel8;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grd;

	private TextBox txtBillAmount;

	private Label label51;

	private TextBox txtWeightCutTotal;

	private Label label52;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CheckBox ChkBok;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combCommType;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combcommUOM;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnRefresh;

	internal GridEX grdFreight;

	internal GridEX grdDetail;

	private CheckBox ChkPrintSlip;

	private ToolStripButton btnPurchaseOrderLoader;

	private TabControl tabControl2;

	private TabPage tabFreight;

	private TabPage tabOtherExpense;

	private TabPage tabSupplierAddLess;

	private TabPage tabEmptyBags;

	internal GridEX grdEmptyBags;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryAc;

	private Label label23;

	private Label label53;

	private Label label54;

	private Label label55;

	private TextBox txtBrokeryRate;

	private Label label56;

	private TextBox txtBrokeryAmount;

	private ToolStripButton btn225PartySlip;

	private NotifyIcon notifyIcon1;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel12;

	private Label label5;

	private Panel panel6;

	private Label label57;

	private Panel panel11;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label58;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnSpecialRights;

	private Panel panel18;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel9;

	private Panel panel7;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel10;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label64;

	private Label label13;

	private TabPage tabDetail;

	private Panel panel5;

	private Label label65;

	private Panel panel13;

	private Label label66;

	private Panel panel14;

	private Label label67;

	private Panel panel15;

	private Label label68;

	private TextBox txtremarks;

	private CheckBox chkPartySlip;

	private CheckBox chkCustomAccounts;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	public InvfrmPurchasedirectInvoice(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
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
			dtAccountlstForFreight.Columns.Add("Id");
			dtAccountlstForFreight.Columns.Add("SupplierCustomerId");
			dtAccountlstForFreight.Columns.Add("AccountTitle");
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
				dtWh.Columns.Add("BranchId", typeof(int));
				dtWh.Columns.Add("BranchName", typeof(string));
			}
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("InventoryParentCategoriesId");
			dtItem.Columns.Add("ItemCategoryId");
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("ItemTypeId");
			dtItem.Columns.Add("ItemType");
			dtItem.Columns.Add("ProductionStageId");
			dtItem.Columns.Add("ProductionStage");
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("CropYear");
			if (dtPackingType.Columns.Count == 0)
			{
				dtPackingType.Columns.Add("Id");
				dtPackingType.Columns.Add("PackTypeDesc");
			}
			if (dtJobLoat.Columns.Count == 0)
			{
				dtJobLoat.Columns.Add("Id");
				dtJobLoat.Columns.Add("JobLotDescription");
				dtJobLoat.Columns.Add("BranchId", typeof(int));
				dtJobLoat.Columns.Add("BranchName", typeof(string));
			}
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderNo", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("EmptyBags", typeof(double));
			dtGrid.Columns.Add("EmptyBagsTotal", typeof(double));
			dtGrid.Columns.Add("WeightCut", typeof(double));
			dtGrid.Columns.Add("WeightCutTotal", typeof(double));
			dtGrid.Columns.Add("AddLss", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutTotal", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("LabSampleNo");
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("PurchaseGLAC", typeof(double));
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			dtFreight.Columns.Add("OrderId", typeof(int));
			dtFreight.Columns.Add("FreightId");
			dtFreight.Columns.Add("FreightAcId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Percentage", typeof(double));
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("FreightAmount", typeof(double));
			dtFreight.Columns.Add("Remarks");
			dtFreight.Columns.Add("GlAccountId", typeof(int));
			dtInvExp.Columns.Add("Id");
			dtInvExp.Columns.Add("PurchaseOrderId");
			dtInvExp.Columns.Add("PurchaseOrderSupplierExpId");
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtEmptyBags.Columns.Add("PurchaseOrderId");
			dtEmptyBags.Columns.Add("Type");
			dtEmptyBags.Columns.Add("ItemId");
			dtEmptyBags.Columns.Add("ItemConditionId");
			dtEmptyBags.Columns.Add("ReceivedQty", typeof(double));
			dtEmptyBags.Columns.Add("PurchaseQty", typeof(double));
			dtEmptyBags.Columns.Add("Rate", typeof(double));
			dtEmptyBags.Columns.Add("Amount", typeof(double));
			dtEmptyBags.Columns.Add("Remarks");
			dtEmptyBags.Columns.Add("CreditAccountId");
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
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
			ScreenName = Conversion.ToString((object)base.Name);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			ScreenId = CommonServices.GetScreenIdByName(base.Name);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			string documentNoData = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId));
			string branchSrNoData = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCodeBranch(DocumentTypeId, UserAccount.BranchesId));
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
			HistoryBranchdtFillDBCall();
			DataTable historyComboData = HistoryComboDbCall(ValidateBranch: false);
			DataTable dueTermServiceBind = CommonServices.GetDueTermServiceBind();
			DataTable specialRightsByScreenIdDbCall = DatatableHelper.GetSpecialRightsByScreenIdDbCall(ScreenId);
			OtherItemdtDbCall();
			return new
			{
				documentNoData = documentNoData,
				BranchSrNoData = branchSrNoData,
				HistoryComboData = historyComboData,
				PaymentTermData = dueTermServiceBind,
				ScreenRightData = specialRightsByScreenIdDbCall
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnSave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Checked = formright.DoHavePrintRights;
				btnSlip.Enabled = formright.DoHavePrintRights;
				btn225PartySlip.Enabled = formright.DoHavePrintRights;
				ChkPrintSlip.Checked = formright.DoHavePrintRights;
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
				ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				GetConfigurationsFromGlobal();
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
					UpdateBranchSrNoUI(result.BranchSrNoData);
				}
				SpecialRightsImplement(result.ScreenRightData);
				SupplierDtFillFromGlobal();
				BindSupplierName();
				PaymentTermBind(result.PaymentTermData);
				DeliveryTerm();
				CommissionTypeFill();
				CommissionUOMFill();
				AccountsDtFillFromGlobal();
				WarehousesDtFillFromGlobalAndBind();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				CropDtFillFromGlobalAndBind();
				JobLotDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				BindGrids();
				HistoryComboBranchBind();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				btnSave.Visible = true;
				btnUpdate.Visible = false;
				DocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void SpecialRightsImplement(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 1)
					{
						ChkPrintSlip.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
					}
					if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 2)
					{
						ChkBok.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
					}
					if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 7)
					{
						chkPartySlip.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
					}
				}
			}
			else
			{
				ChkPrintSlip.Checked = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSpecialRights_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialSreenRightsUserWise obj = new SpecialSreenRightsUserWise(UserAccount);
			obj.ScreenId = ScreenId;
			((Control)(object)obj.cmbScreenName).Enabled = false;
			obj.UserId = Conversion.ToInt((object)UserAccount.ID);
			if (UserAccount.RoleName != "Admin")
			{
				((Control)(object)obj.cmbUserName).Enabled = false;
			}
			obj.Show();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchdtFillDBCall()
	{
		try
		{
			dtBranch.Clear();
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
				return;
			}
			DataTable dt = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 57);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBranchBind()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			((UltraGridBase)cmbBranchName).DataSource = null;
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

	private DataTable HistoryComboDbCall(bool ValidateBranch = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "57";
			obj.Activity = "Supplier";
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
			return InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
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
			InfragisticsHelper.BindAndRetainSelection(cmbSupplierNameHistory, dt, "Id", "ReferenceName", "Supplier Name", AllColumns: true, Conversion.ToInt(cmbSupplierNameHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnPurchaseInvoiceDirect"));
			WagesActiveOrInActive = GlobalVariables_Helper.GetWagesRefDocumentsStatusById(57);
			FreightDebitToExpenses = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DebitAmountChargetoExpenseAcFreightGridPurchase"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseInvoiceDirectBranchWise"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
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
			int wareHouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Warehouse"));
			if (wareHouseId != 0)
			{
				CmbWarehouse.Value = wareHouseId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0)
			{
				CmbCropYear.Value = cropYearId;
			}
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0)
			{
				CmbJobLot.Value = jobId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				CmbPackingType.Value = packingTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string No)
	{
		txtdocno.Text = No;
	}

	private void UpdateBranchSrNoUI(string No)
	{
		txtBranchSrNo.Text = No;
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> allSuppliers = (from r in clsGlobalVariables.globalSupplierCustomer.AsEnumerable()
				where r.CustomerGroupId != 7
				select r).ToList();
			dtSupplier.Clear();
			IEnumerable<getGlobalSupplierCustomer> enumerable2;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				IEnumerable<getGlobalSupplierCustomer> enumerable = allSuppliers;
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = allSuppliers.Where((getGlobalSupplierCustomer r) => r.PartyTypeId == 1);
			}
			IEnumerable<getGlobalSupplierCustomer> filteredSuppliers = enumerable2;
			if (filteredSuppliers == null || !filteredSuppliers.Any())
			{
				return;
			}
			foreach (getGlobalSupplierCustomer supplier in filteredSuppliers)
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)supplier.Id), Conversion.ToString((object)supplier.CompanyName), Conversion.ToString((object)supplier.PartyCode), Conversion.ToInt((object)supplier.GlAccountId), Conversion.ToInt((object)supplier.CityId), Conversion.ToString((object)supplier.CityName), Conversion.ToString((object)supplier.MobilePersonal));
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
			InfragisticsHelper.BindAndRetainSelection(comsupplier, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, Conversion.ToInt(comsupplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(combcommAgent, dtSupplier, "Id", "CompanyName", "Commission Agent", AllColumns: true, Conversion.ToInt(combcommAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryAc, dtSupplier, "Id", "CompanyName", "BrokerAc", AllColumns: true, Conversion.ToInt(CmbBrokeryAc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTermBind(DataTable dtTerm)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(combpttrm, dtTerm, "Id", "TermsDescription", "Payment Term", AllColumns: true, Conversion.ToInt(combpttrm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			dt.Rows.Add(3, "Load & PartyWeight");
			dt.Rows.Add(4, "Load & FactoryWeight");
			dt.Rows.Add(5, "Ponch & PartyWeight");
			dt.Rows.Add(6, "Ponch & FactoryWeight");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
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
			DDL.BindDDL(dt, CmbBrokeryType, "Id", "CommissionType", "BrokeryType", ZeroIndex: false);
			((UltraGridBase)combCommType).Rows[1].Activate();
			((UltraGridBase)CmbBrokeryType).Rows[1].Activate();
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
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, combcommUOM, "Id", "type", "UOM", ZeroIndex: false);
				DDL.BindDDL(dt, CmbBrokeryRateUom, "Id", "type", "UOM", ZeroIndex: false);
				((UltraGridBase)combcommUOM).Rows[0].Activate();
				((UltraGridBase)CmbBrokeryRateUom).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsDtFillFromGlobal()
	{
		try
		{
			dtAccountlst.Clear();
			dtAccountlstForFreight.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
				if (lst == null || lst.Count <= 0)
				{
					return;
				}
				{
					foreach (getGlobalVendorsAndCustomersForTransporter item in lst)
					{
						object[] row = new object[3] { item.GlAccountId, item.Id, item.CompanyName };
						dtAccountlst.Rows.Add(row);
						dtAccountlstForFreight.Rows.Add(row);
					}
					return;
				}
			}
			HashSet<int> excludedIdsMain = new HashSet<int> { 2, 11, 12, 13, 14, 15, 20, 21, 22 };
			HashSet<int> excludedIdsFreight = (FreightDebitToExpenses ? new HashSet<int> { 2, 15, 22 } : excludedIdsMain);
			FillAccountList(clsGlobalVariables.AllAccountsWithCustomGroupId, dtAccountlst, excludedIdsMain);
			if (FreightDebitToExpenses)
			{
				FillAccountList(clsGlobalVariables.AllAccountsWithCustomGroupId, dtAccountlstForFreight, excludedIdsFreight);
				return;
			}
			foreach (DataRow row2 in dtAccountlst.Rows)
			{
				dtAccountlstForFreight.ImportRow(row2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillAccountList(IEnumerable<getGlobalAllAccountsWithCustomGroup> accounts, DataTable table, HashSet<int> excludedIds)
	{
		HashSet<int> existingIds = new HashSet<int>();
		foreach (getGlobalAllAccountsWithCustomGroup acc in accounts)
		{
			int id = Conversion.ToInt((object)acc.ChartOfAccountId);
			if (!excludedIds.Contains(acc.AccountTypeId) && !existingIds.Contains(id))
			{
				table.Rows.Add(acc.ChartOfAccountId, 0, acc.AccountTitle);
				existingIds.Add(id);
			}
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 57);
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
				HistoryComboBind(HistoryComboDbCall());
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

	private void WarehousesDtFillFromGlobalAndBind()
	{
		try
		{
			dtWh.Clear();
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches;
			if (BranchFeature && BranchImplemented)
			{
				lst = lst.Where((GlobalWarehousesWithBranch r) => r.BranchId == UserAccount.BranchesId).ToList();
			}
			foreach (GlobalWarehousesWithBranch row in lst)
			{
				dtWh.Rows.Add(row.WarehouseId, row.WarehouseName, row.BranchId, row.BranchName);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouse, dtWh, "Id", "WareHouseName", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWarehouse.Value), insertDefaultRow: true);
			if (dtWh.Rows.Count > 0)
			{
				((UltraGridBase)CmbWarehouse).Rows[1].Activate();
				((UltraGridBase)CmbWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtItem.Clear();
			dtPmItems.Clear();
			IEnumerable<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17);
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllItems row in allItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.InventoryParentCategoriesId, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType, row.ItemProductionStageId, row.productionStageName);
			}
			dtPmItems = CommonServices.GetPackingMaterialItemsAllocateToFlow();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[8].Hidden = true;
			}
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CropDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotDtFillFromGlobalAndBind()
	{
		try
		{
			dtJobLoat.Clear();
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			if (BranchFeature && BranchImplemented)
			{
				lst = lst.Where((GetJobLotGlIdsandName r) => r.BranchId == UserAccount.BranchesId).ToList();
			}
			foreach (GetJobLotGlIdsandName row in lst)
			{
				dtJobLoat.Rows.Add(row.Id, row.JobLotDescription, row.BranchId, row.BranchName);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dtJobLoat, "Id", "JobLotDescription", "JobLot", AllColumns: true, Conversion.ToInt(CmbJobLot.Value), insertDefaultRow: true);
			if (dtJobLoat.Rows.Count > 0)
			{
				((UltraGridBase)CmbJobLot).Rows[1].Activate();
				((UltraGridBase)CmbJobLot).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbJobLot).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			int[] Ids = new int[4] { 1, 2, 5, 10 };
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType.Where((InvPackingType r) => Ids.Contains(r.Id)).ToList();
			DatatableHelper.PopulateDataTable(dtPackingType, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPackingType, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)comPackUOM).Text;
			string Rateuom = ((Control)(object)comPackUOM).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(comPackUOM, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			InfragisticsHelper.BindAndRetainSelection(comRateUOM, dtUOM, "Id", "UOMCode", "Rate Uom", AllColumns: true, Rateuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OtherItemdtDbCall()
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

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
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
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if ((((UltraGridBase)combcommAgent).ActiveRow != null || Conversion.ToDouble((object)txtcommamount.Text.Trim()) > 0.0) && txtCommissionRemarks.Text == string.Empty)
		{
			MessageBox.Show("Commission Remarks Field is Required");
			txtCommissionRemarks.Focus();
			return false;
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
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (((UltraGridBase)comPackUOM).ActiveRow == null || Conversion.ToInt(comPackUOM.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			comPackUOM.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetBillWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetBillWeight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetBillWeight.Focus();
			return false;
		}
		if (txtStockWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtStockWeight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtStockWeight.Focus();
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
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWarehouse.Focus();
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			if (dtGrid != null && dtGrid.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
			if (dtInvExp != null && dtInvExp.Columns.Count > 0)
			{
				DataTable dataTable = dtInvExp;
				if (dataTable != null && dataTable.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
			if (dtFreight != null && dtFreight.Columns.Count > 0)
			{
				DataTable dataTable2 = dtFreight;
				if (dataTable2 != null && dataTable2.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.RetrieveStructure();
				grdFreightSettings();
			}
			if (dtGrdGL != null && dtGrdGL.Columns.Count > 0)
			{
				DataTable dataTable3 = dtGrdGL;
				if (dataTable3 != null && dataTable3.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.RetrieveStructure();
				gridGLSettings();
			}
			if (dtEmptyBags != null && dtEmptyBags.Columns.Count > 0)
			{
				DataTable dataTable4 = dtEmptyBags;
				if (dataTable4 != null && dataTable4.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
				grdEmptyBags.RetrieveStructure();
				grdEmptyBagsSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmptyBagsInformationFromOrder()
	{
		try
		{
			string Ids = GridEX_Helper.GetAllValuesOfColumn<int>(grd, "PurchaseOrderId", null, null, distinct: true);
			if (string.IsNullOrEmpty(Ids))
			{
				return;
			}
			DataTable dtPOEmptyBagsDetail = PurchaseOrder.GetPurchaseOrderEmptyBagsDetailByOrderId(0, Ids);
			if (dtPOEmptyBagsDetail.Rows.Count == 0)
			{
				return;
			}
			dtEmptyBags.BeginLoadData();
			HashSet<int> processedOrders = new HashSet<int>();
			Dictionary<int, double> orderQtyMap = new Dictionary<int, double>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int orderId2 = Conversion.ToInt(obj.Cells["PurchaseOrderId"].Value);
				double qty = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
				if (orderQtyMap.ContainsKey(orderId2))
				{
					orderQtyMap[orderId2] += qty;
				}
				else
				{
					orderQtyMap.Add(orderId2, qty);
				}
			}
			foreach (DataRow row in dtPOEmptyBagsDetail.Rows)
			{
				int type = Conversion.ToInt(row["Type"]);
				Conversion.ToInt(row["PackingTypeId"]);
				switch (type)
				{
				case 1:
					txtEmptybagsUnit.Text = Conversion.ToString(row["WeightCut"]);
					break;
				case 2:
					txtEmptybagsUnit.Text = Conversion.ToString(row["WeightCut"]);
					break;
				case 3:
					txtEmptybagsUnit.Text = Conversion.ToString(row["WeightCut"]);
					break;
				}
				double purQty = 0.0;
				int orderId = Conversion.ToInt(row["PurchaseOrderId"]);
				if (!processedOrders.Contains(orderId))
				{
					if (orderQtyMap.ContainsKey(orderId))
					{
						purQty = orderQtyMap[orderId];
					}
					processedOrders.Add(orderId);
				}
				AddOrUpdateEmptyBagRowFromOrder(row, purQty);
			}
			dtEmptyBags.EndLoadData();
			if (dtEmptyBags.Rows.Count > 0)
			{
				dtEmptyBags.Rows[0]["ItemConditionId"] = 1;
			}
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
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

	private void AddOrUpdateEmptyBagRowFromOrder(DataRow orderRow, double purQty)
	{
		int orderId = Conversion.ToInt(orderRow["PurchaseOrderId"]);
		int type = Conversion.ToInt(orderRow["Type"]);
		int ItemId = Conversion.ToInt(orderRow["ItemId"]);
		DataRow existingRow = dtEmptyBags.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["PurchaseOrderId"]) == orderId && Conversion.ToInt(r["Type"]) == type && (Conversion.ToInt(r["ItemId"]) == 0 || Conversion.ToInt(r["ItemId"]) == ItemId));
		bool canSave = btnSave.Visible && btnSave.Enabled;
		if (existingRow != null)
		{
			if (Conversion.ToDouble(existingRow["PurchaseQty"]) == 0.0)
			{
				if (!canSave)
				{
					purQty = 0.0;
				}
				existingRow["PurchaseQty"] = purQty;
			}
			if (Conversion.ToInt(existingRow["ItemId"]) == 0)
			{
				existingRow["ItemId"] = ItemId;
			}
			if (Conversion.ToInt(existingRow["ItemConditionId"]) == 0)
			{
				existingRow["ItemConditionId"] = 1;
			}
			return;
		}
		if (!canSave)
		{
			purQty = 0.0;
		}
		DataRow newRow = dtEmptyBags.NewRow();
		newRow["PurchaseOrderId"] = orderId;
		newRow["Type"] = type;
		newRow["ItemId"] = ItemId;
		newRow["ItemConditionId"] = 1;
		newRow["ReceivedQty"] = 0.0;
		newRow["PurchaseQty"] = purQty;
		newRow["Rate"] = orderRow["Rate"];
		newRow["Amount"] = 0;
		newRow["Remarks"] = "";
		newRow["CreditAccountId"] = 0;
		dtEmptyBags.Rows.Add(newRow);
	}

	private void comPackingType_Leave(object sender, EventArgs e)
	{
		try
		{
			GetEmptyBagsInformationFromOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetEmptyBagsInformationFromOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		DataRow newRow = dtEmptyBags.NewRow();
		dtEmptyBags.Rows.Add(newRow);
		grdEmptyBags.DataSource = dtEmptyBags;
	}

	private void EBComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["ItemId"], dtPmItems, "ItemId", "ItemName", "Item Name", 200);
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["Type"], dtEmpType, "Id", "Type", "Type", 100);
			DataTable dtCrAccountForEmptyBag = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[1] { 10 });
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["CreditAccountId"], dtCrAccountForEmptyBag, "Id", "AccountTitle", "Credit Account", 150);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["ItemConditionId"], dt, "Id", "Description", "Item Condition", Constants.InventoryConstants.StringConstant);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			grdEmptyBags.ColumnAutoResize = false;
			List<string> hiddenColumns = new List<string> { "Id", "PurchaseOrderId" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"ReceivedQty",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"PurchaseQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBags, 2, 3, hiddenColumns, editCol);
			EBComboBind();
			grdEmptyBags.RootTable.Columns["Remarks"].Width = 400;
			GridEX_Helper.AddButton(grdEmptyBags, "Delete", "X", 25, 0);
			GridEX_Helper.AddButton(grdEmptyBags, "Add", "+", 25, 1);
			grdEmptyBags.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_Click(object sender, EventArgs e)
	{
		try
		{
			EmptyGridsColumnsEdit();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			EmptyGridsColumnsEdit();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyGridsColumnsEdit()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int num = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
				if (num > 0)
				{
					grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)0;
					grdEmptyBags.RootTable.Columns["Rate"].EditType = (EditType)0;
				}
				else
				{
					grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)4;
					grdEmptyBags.RootTable.Columns["Rate"].EditType = (EditType)1;
				}
				int TypeId = Conversion.ToInt(r.Cells["Type"].Value);
				if (num > 0 && TypeId != 1)
				{
					grdEmptyBags.RootTable.Columns["ReceivedQty"].EditType = (EditType)0;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdEmptyBags_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "ReceivedQty" || e.Column.Key == "PurchaseQty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdEmptyBags.CurrentRow;
				dtEmptyBags.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtEmptyBags.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Column.Key == "Add" && grdEmptyBags.GetRows().Length == 1)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmptyBags.UpdateData();
			GridEXRow item = grdEmptyBags.CurrentRow;
			if ((e.Column.Key == "Rate" || e.Column.Key == "PurchaseQty") && item != null && (int)item.RowType == 0 && item.Cells["PurchaseQty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble(item.Cells["PurchaseQty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount;
			}
			grdEmptyBags.UpdateData();
			BillAmount();
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
			if (FormValidationDetail())
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
					UOMId = Convert.ToDouble(comPackUOM.Value.ToString());
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
				if (txtratecut.Text == string.Empty)
				{
					txtratecut.Text = "0";
				}
				if (txtratecuttotal.Text == string.Empty)
				{
					txtratecuttotal.Text = "0";
				}
				if (txtAmount.Text == string.Empty)
				{
					txtAmount.Text = "0";
				}
				if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
				{
					throw new Exception("Please Check ItemAmount");
				}
				int OrderDetailId = 0;
				if (((UltraGridBase)CmbOrderNo).ActiveRow != null && Conversion.ToInt(CmbOrderNo.Value) > 0)
				{
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PurchaseOrderDetailId"].Value);
				}
				int BranchId = 0;
				string BranchName = "";
				int JobBranchId = 0;
				string JobBranchName = "";
				if (((UltraGridBase)CmbWarehouse).ActiveRow != null && Conversion.ToInt(CmbWarehouse.Value) > 0)
				{
					BranchId = Conversion.ToInt(((UltraDropDownBase)CmbWarehouse).SelectedRow.Cells["BranchId"].Value);
					BranchName = Conversion.ToString(((UltraDropDownBase)CmbWarehouse).SelectedRow.Cells["BranchName"].Value);
				}
				if (((UltraGridBase)CmbJobLot).ActiveRow != null && Conversion.ToInt(CmbJobLot.Value) > 0)
				{
					JobBranchId = Conversion.ToInt(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchId"].Value);
					JobBranchName = Conversion.ToString(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchName"].Value);
				}
				if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
				{
					MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				dtGrid.Rows.Add(0, CmbOrderNo.Value, OrderDetailId, Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text), CmbItemName.Value, ((Control)(object)CmbItemName).Text, ((Control)(object)CmbCropYear).Text, CmbJobLot.Value, ((Control)(object)CmbJobLot).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, UOMId, UOM, Qty, txtGrossWeight.Text, EbU, EBT, txtwtcut.Text, txtWeightCutTotal.Text, txtAddLss.Text, txtNetBillWeight.Text, txtStockWeight.Text, txtRate.Text, comRateUOM.Value, ((Control)(object)comRateUOM).Text, txtratecut.Text, txtratecuttotal.Text, txtAmount.Text, CmbWarehouse.Value, ((Control)(object)CmbWarehouse).Text, txtlabanalysisno.Text, txtgpdate.Value.ToShortDateString(), txtgatepassno.Text, txtvehicleno.Text, 0, 0, 0, 0, 0, 0, BranchId, BranchName);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				TotalCommissionAmount();
				TotalBrokeryAmount();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				BillAmount();
				ResetDetail();
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
				if (Conversion.ToInt(item.Cells["PurchaseOrderId"].Value) > 0)
				{
					DataTable dtpo = new DataTable();
					dtpo.Columns.Add("PurchaseOrderId");
					dtpo.Columns.Add("PurchaseOrderDetailId");
					dtpo.Columns.Add("PurchaseOrderNo");
					dtpo.Rows.Add(item.Cells["PurchaseOrderId"].Value, item.Cells["PurchaseOrderDetailId"].Value, item.Cells["PurchaseOrderNo"].Value);
					DDL.BindDDL(dtpo, CmbOrderNo, "PurchaseOrderId", "PurchaseOrderNo", "PurchaseOrderNo", ZeroIndex: false);
					((UltraGridBase)CmbOrderNo).Rows[0].Activate();
					txtRate.Enabled = false;
					((Control)(object)CmbItemName).Enabled = false;
				}
				else
				{
					txtRate.Enabled = true;
					((Control)(object)CmbItemName).Enabled = true;
				}
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
				((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtQty.Text = Conversion.ToDouble(item.Cells["ItemQty"].Value).ToString("#,##0.###");
				comPackUOM.Value = item.Cells["PackUOMId"].Value.ToString();
				((Control)(object)comPackUOM).Text = item.Cells["PackUOM"].Value.ToString();
				txtGrossWeight.Text = Conversion.ToDouble(item.Cells["GrossWeight"].Value).ToString("#,##0.###");
				txtEmptybagsUnit.Text = Conversion.ToDouble(item.Cells["EmptyBags"].Value).ToString("#,##0.###");
				txtEmptyBagsTotal.Text = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value).ToString("#,##0.###");
				txtwtcut.Text = Conversion.ToDouble(item.Cells["WeightCut"].Value).ToString("#,##0.###");
				txtWeightCutTotal.Text = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value).ToString("#,##0.###");
				txtAddLss.Text = Conversion.ToDouble(item.Cells["AddLss"].Value).ToString("#,##0.###");
				txtNetBillWeight.Text = Conversion.ToDouble(item.Cells["NetBillWeight"].Value).ToString("#,##0.###");
				txtStockWeight.Text = Conversion.ToDouble(item.Cells["StockWeight"].Value).ToString("#,##0.###");
				txtRate.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				comRateUOM.Value = item.Cells["RateUOMId"].Value.ToString();
				((Control)(object)comRateUOM).Text = item.Cells["RateUOM"].Value.ToString();
				txtratecut.Text = Conversion.ToDouble(item.Cells["RateCut"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtratecuttotal.Text = Conversion.ToDouble(item.Cells["RateCutTotal"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtAmount.Text = Conversion.ToDouble(item.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				CmbWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
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
			if (FormValidationDetail())
			{
				int OrderDetailId = 0;
				if (((UltraGridBase)CmbOrderNo).ActiveRow != null && Conversion.ToInt(CmbOrderNo.Value) > 0)
				{
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PurchaseOrderDetailId"].Value);
				}
				int BranchId = 0;
				string BranchName = "";
				int JobBranchId = 0;
				string JobBranchName = "";
				if (((UltraGridBase)CmbWarehouse).ActiveRow != null && Conversion.ToInt(CmbWarehouse.Value) > 0)
				{
					BranchId = Conversion.ToInt(((UltraDropDownBase)CmbWarehouse).SelectedRow.Cells["BranchId"].Value);
					BranchName = Conversion.ToString(((UltraDropDownBase)CmbWarehouse).SelectedRow.Cells["BranchName"].Value);
				}
				if (((UltraGridBase)CmbJobLot).ActiveRow != null && Conversion.ToInt(CmbJobLot.Value) > 0)
				{
					JobBranchId = Conversion.ToInt(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchId"].Value);
					JobBranchName = Conversion.ToString(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchName"].Value);
				}
				if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
				{
					MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				dtGrid.Rows[updateDetailIndex]["PurchaseOrderId"] = Conversion.ToInt(CmbOrderNo.Value);
				dtGrid.Rows[updateDetailIndex]["PurchaseOrderDetailId"] = OrderDetailId;
				dtGrid.Rows[updateDetailIndex]["PurchaseOrderNo"] = Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				dtGrid.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemName).Text;
				dtGrid.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtGrid.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				dtGrid.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text;
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtQty.Text);
				dtGrid.Rows[updateDetailIndex]["PackUOMId"] = comPackUOM.Value;
				dtGrid.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)comPackUOM).Text;
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtGrid.Rows[updateDetailIndex]["EmptyBags"] = Conversion.ToDouble((object)txtEmptybagsUnit.Text);
				dtGrid.Rows[updateDetailIndex]["EmptyBagsTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCut"] = Conversion.ToDouble((object)txtwtcut.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCutTotal"] = Conversion.ToDouble((object)txtWeightCutTotal.Text);
				dtGrid.Rows[updateDetailIndex]["AddLss"] = Conversion.ToDouble((object)txtAddLss.Text);
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = Conversion.ToDouble((object)txtNetBillWeight.Text);
				dtGrid.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtStockWeight.Text);
				dtGrid.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["RateUOMId"] = comRateUOM.Value;
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)comRateUOM).Text;
				dtGrid.Rows[updateDetailIndex]["RateCut"] = Conversion.ToDouble((object)txtratecut.Text);
				dtGrid.Rows[updateDetailIndex]["RateCutTotal"] = Conversion.ToDouble((object)txtratecuttotal.Text);
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = CmbWarehouse.Value;
				dtGrid.Rows[updateDetailIndex]["Warehouse"] = ((Control)(object)CmbWarehouse).Text;
				dtGrid.Rows[updateDetailIndex]["LabSampleNo"] = txtlabanalysisno.Text;
				dtGrid.Rows[updateDetailIndex]["GpDate"] = txtgpdate.Value;
				dtGrid.Rows[updateDetailIndex]["GpNo"] = txtgatepassno.Text;
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = txtvehicleno.Text;
				dtGrid.Rows[updateDetailIndex]["BranchId"] = BranchId;
				dtGrid.Rows[updateDetailIndex]["BranchName"] = BranchName;
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				((Control)(object)CmbOrderNo).Text = string.Empty;
				((UltraGridBase)CmbOrderNo).DataSource = null;
				TotalCommissionAmount();
				TotalBrokeryAmount();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				BillAmount();
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
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailrow(r);
				}
				if (e.Column.Key == "Add")
				{
					DataRow dr = dtGrid.Rows[r.RowIndex];
					dtGrid.Rows.Add(dr.ItemArray);
					grd.DataSource = dtGrid;
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
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id > 0)
				{
					if (MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
					InvPurchaseInvoice.StockInReferenceValidationReferredOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId, RecId, Id);
				}
				r.Delete();
			}
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

	private void grdSettings()
	{
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["Freights"].Caption = "Charge To Product";
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["PurchaseOrderNo"].Caption = "Order No";
			grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["PurchaseOrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EmptyBags"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["EmptyBagsTotal"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["AddLss"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateCutTotal"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["LabSampleNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Journal"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBags"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBags"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBagsTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBagsTotal"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBagsTotal"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AddLss"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLss"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLss"].FormatString = "#,##0.###";
			grd.RootTable.Columns["AddLss"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grd, "Add", "+", 20, 1);
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
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
				grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
				grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
				grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
				grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
				grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
				grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			}
			if (dtAccountlstForFreight.Rows.Count > 0)
			{
				grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
				grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
				grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
				grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
				grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
				if (SubsidiaryAccountAllownOnVouchers)
				{
					grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "SupplierCustomerId", "AccountTitle");
				}
				else
				{
					grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "Id", "AccountTitle");
				}
				grdFreight.RootTable.Columns["Transporter"].Width = 250;
			}
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

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["Id"].Visible = false;
			grdInvExp.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grdInvExp.RootTable.Columns["PurchaseOrderSupplierExpId"].Visible = false;
			grdInvExp.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["PurchaseOrderId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["PurchaseOrderSupplierExpId"].EditType = (EditType)0;
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
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble(item.Cells["Qty"].Value);
					double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
					double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Amount"].Value = Amount;
				}
			}
			else if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0 && item.Cells["Amount"].Value.ToString() != string.Empty)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				double Amount2 = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount2;
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

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0, 0);
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
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Supplier Add/Less";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SuppliercustomerId", "AccountTitle");
			}
			else
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns["Remarks"].Width = 300;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			SupCustIdUpdateforGLGrid();
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
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Credit" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					item.Delete();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Round(Math.Abs(TotlaPercentage), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit")
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit")
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Supplier Account Not select");
						item.Cells["GlAccountId"].Value = 0;
						return;
					}
				}
				else if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
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

	private void grdFreight_Enter(object sender, EventArgs e)
	{
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0);
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			if (!FreightDebitToExpenses)
			{
				grdFreight.RootTable.Columns["Debit"].Visible = false;
			}
			else
			{
				grdFreight.RootTable.Columns["Debit"].Visible = true;
			}
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["OrderId"].Visible = false;
			grdFreight.RootTable.Columns["FreightId"].Visible = false;
			grdFreight.RootTable.Columns["FreightAcId"].Visible = false;
			grdFreight.RootTable.Columns["FreightAmount"].Visible = false;
			grdFreight.RootTable.Columns["FreightAmount"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["FreightId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["FreightAcId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["FreightAmount"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].Caption = "AccountTitle";
			grdFreight.RootTable.Columns["Percentage"].Caption = "%";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Charge To Product";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "SuppliercustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns["Transporter"].Width = 150;
			grdFreight.RootTable.Columns["Remarks"].Width = 120;
			grdFreight.RootTable.Columns["Freight"].Width = 80;
			grdFreight.RootTable.Columns["Percentage"].Width = 50;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdFreight.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdFreight.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			SupCustIdUpdateforFrieghtGrid();
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
						DataRow[] matchingRows = dtAccountlstForFreight.Select("AccountTitle='" + selectedAcId + "'");
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

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtFreight.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
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
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Freight"].Value = amount;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				double num2 = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num2 / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Freight"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Transporter")
			{
				SupCustIdUpdateforFrieghtGrid();
			}
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			if (FreightDebitToExpenses)
			{
				if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
					{
						item.Cells["Freight"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
					{
						item.Cells["Debit"].Value = 0;
						MessageBox.Show("Credit Side is aleady added");
					}
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

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Freight" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Expected O, but got Unknown
		//IL_0db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbd: Expected O, but got Unknown
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Expected O, but got Unknown
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d9: Expected O, but got Unknown
		//IL_13d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13de: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				purchaseInvoice.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double TotalFreightCredit = 0.0;
			double TotalFreightDebit = 0.0;
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				TotalFreightCredit += Conversion.ToDouble(r9.Cells["Freight"].Value);
				TotalFreightDebit += Conversion.ToDouble(r9.Cells["Debit"].Value);
				if (!(Conversion.ToDouble(r9.Cells["Freight"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r9.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
				if (Conversion.ToDouble(r9.Cells["Freight"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r9.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			if (FreightDebitToExpenses && (TotalFreightCredit > 0.0 || TotalFreightDebit > 0.0) && TotalFreightCredit != TotalFreightDebit)
			{
				MessageBox.Show("Freight Debit Amount not equal to Credit Amount Please Check");
				return;
			}
			if (!FreightDebitToExpenses && TotalFreightDebit > 0.0)
			{
				MessageBox.Show("Freight Debit Row Remove first");
				return;
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				if ((Conversion.ToDouble(r8.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r8.Cells["Debit"].Value) > 0) && Conversion.ToInt(r8.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToDouble(r7.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r7.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against Expense First");
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0)
				{
					if (Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("EmptyBags Item field required");
					}
					if (Conversion.ToInt(r6.Cells["Type"].Value) == 0)
					{
						throw new Exception("EmptyBagsType field required");
					}
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				throw new Exception("Please Select Commission Agent Account First");
			}
			TotalCommissionAmount();
			BillAmount();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			purchaseInvoice.BrokerAgentId = Conversion.ToInt(CmbBrokeryAc.Value);
			purchaseInvoice.BrokeryType = ((Control)(object)CmbBrokeryType).Text.Trim();
			purchaseInvoice.BrokeryRate = Conversion.ToDouble((object)txtBrokeryRate.Text);
			purchaseInvoice.BrokeryUom = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text);
			purchaseInvoice.BrokeryAmount = Conversion.ToDouble((object)txtBrokeryAmount.Text);
			purchaseInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			purchaseInvoice.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			purchaseInvoice.CommissionType = ((Control)(object)combCommType).Text.Trim();
			purchaseInvoice.DeliveryTerm = ((Control)(object)combdeliverytrm).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			purchaseInvoice.UomScheduleIdCmRate = Conversion.ToString((object)((Control)(object)combcommUOM).Text);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocumentTypeId = 57;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.RemarksHeader = txtremarks.Text.Trim();
			purchaseInvoice.OtherRemarks = txtremarks.Text.Trim();
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			purchaseInvoice.CustomAccounts = Conversion.ToBool((object)chkCustomAccounts.Checked);
			purchaseInvoice.SupplierInvoiceDate = DateTime.Now;
			purchaseInvoice.DueDate = duedate.Value;
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtduedays.Text);
			purchaseInvoice.SupplierReferenceNo = Conversion.ToString((object)txtSupplierReference.Text.Trim());
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.ScreenName = Conversion.ToString((object)base.Name);
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			double GrossWt = 0.0;
			if (grd.RowCount > 0)
			{
				double InvoiceQty = 0.0;
				double EmptyBagsRecQty = 0.0;
				double EmptyBagsPurQty = 0.0;
				rows = grd.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r5.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r5.Cells["PurchaseOrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(r5.Cells["PurchaseOrderDetailId"].Value);
					Pd.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					Pd.CropYear = Conversion.ToString((object)r5.Cells["CropYear"].Text);
					if (Pd.CropYear == string.Empty)
					{
						MessageBox.Show("CropYear field required");
						return;
					}
					Pd.JobLotId = Conversion.ToInt(r5.Cells["JobLotId"].Value);
					if (Pd.JobLotId == 0)
					{
						MessageBox.Show("JobLot field required");
						return;
					}
					Pd.PackingTypeId = Conversion.ToInt(r5.Cells["PackingTypeId"].Value);
					if (Pd.PackingTypeId == 0)
					{
						MessageBox.Show("PackingType field required");
						return;
					}
					Pd.ItemUOMId = Conversion.ToInt(r5.Cells["PackUOMId"].Value);
					if (Pd.ItemUOMId == 0)
					{
						MessageBox.Show("PackUOM field required");
						return;
					}
					Pd.ItemQty = Conversion.ToDouble(r5.Cells["ItemQty"].Value);
					InvoiceQty += Pd.ItemQty;
					if (Pd.ItemQty == 0.0)
					{
						MessageBox.Show("ItemQty field required");
						return;
					}
					Pd.GrossWeight = Conversion.ToDouble(r5.Cells["GrossWeight"].Value);
					if (Pd.GrossWeight == 0.0)
					{
						MessageBox.Show("GrossWeight field required");
						return;
					}
					GrossWt += Pd.GrossWeight;
					Pd.EBWeight = Conversion.ToDouble(r5.Cells["EmptyBags"].Value);
					Pd.EBTotalWt = Conversion.ToDouble(r5.Cells["EmptyBagsTotal"].Value);
					Pd.WeightCut = Conversion.ToSingle(r5.Cells["WeightCut"].Value);
					Pd.WeightCutTotal = Conversion.ToSingle(r5.Cells["WeightCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDouble(r5.Cells["AddLss"].Value);
					Pd.NetBillWeight = Conversion.ToDouble(r5.Cells["NetBillWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDouble(r5.Cells["StockWeight"].Value);
					if (Pd.NetBillWeight == 0.0)
					{
						MessageBox.Show("NetBillWeight field required");
						return;
					}
					if (Pd.NetStockWeight == 0.0)
					{
						MessageBox.Show("NetStockWeight field required");
						return;
					}
					Pd.ItemRate = Conversion.ToDouble(r5.Cells["Rate"].Value);
					if (Pd.ItemRate == 0.0)
					{
						MessageBox.Show("ItemRate field required");
						return;
					}
					Pd.UomScheduleIdRate = Conversion.ToInt(r5.Cells["RateUOMId"].Value);
					if (Pd.UomScheduleIdRate == 0)
					{
						MessageBox.Show("RateUom field required");
						return;
					}
					Pd.RateCut = Conversion.ToDouble(r5.Cells["RateCut"].Value);
					Pd.RateCutAmount = Conversion.ToDouble(r5.Cells["RateCutTotal"].Value);
					Pd.ItemAmount = Conversion.ToDouble(r5.Cells["ItemAmount"].Value);
					if (Pd.ItemAmount == 0.0)
					{
						MessageBox.Show("ItemAmount field required");
						return;
					}
					Pd.WarehouseId = Conversion.ToInt(r5.Cells["WarehouseId"].Value);
					Pd.WareHouseName = Conversion.ToString(r5.Cells["Warehouse"].Value);
					if (Pd.WarehouseId == 0)
					{
						MessageBox.Show("Warehouse field required");
						return;
					}
					Pd.LabAnalisysNo = Conversion.ToString(r5.Cells["LabSampleNo"].Value);
					Pd.GpDate = Conversion.ToDateTime(r5.Cells["GpDate"].Value);
					Pd.GpNo = Conversion.ToInt(r5.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r5.Cells["VehicleNo"].Value);
					Pd.BillAmount = Conversion.ToDouble(r5.Cells["BillAmount"].Value);
					Pd.JournalAmount = Conversion.ToDouble(r5.Cells["Journal"].Value);
					Pd.ExpenseAmount = Conversion.ToDouble(r5.Cells["Expense"].Value);
					Pd.FreightAmount = Conversion.ToDouble(r5.Cells["Freights"].Value);
					Pd.CommissionAmount = Conversion.ToDouble(r5.Cells["Commission"].Value);
					Pd.BranchId = Conversion.ToInt(r5.Cells["BranchId"].Value);
					Pd.BranchName = Conversion.ToString(r5.Cells["BranchName"].Value);
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToInt((object)r4.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.PurchaseOrderId = Conversion.ToInt(r4.Cells["OrderId"].Value);
						PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
						PF.InvGrnId = 0;
						PF.FreightId = Conversion.ToInt(r4.Cells["FreightId"].Value);
						PF.SupplierCustomerId = Conversion.ToInt(r4.Cells["Transporter"].Value);
						PF.Percentage = Conversion.ToDouble(r4.Cells["Percentage"].Value);
						PF.FrQty = Conversion.ToDouble(r4.Cells["Qty"].Value);
						PF.FrRate = Conversion.ToDouble(r4.Cells["Rate"].Value);
						PF.FreightAmount = Conversion.ToDouble(r4.Cells["Freight"].Value);
						PF.Debit = Conversion.ToDouble(r4.Cells["Debit"].Value);
						PF.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
						PE.Id = Conversion.ToInt(r3.Cells["Id"].Value);
						PE.PurchaseOrderId = Conversion.ToInt(r3.Cells["PurchaseOrderId"].Value);
						PE.PurchaseOrderSupplierExpId = Conversion.ToInt(r3.Cells["PurchaseOrderSupplierExpId"].Value);
						PE.InvRevExpItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToInt(r3.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						PE.CustomRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						if (PE.Qty > 0.0 && PE.Rate > 0.0)
						{
							PE.Remarks = "ItemName : " + Conversion.ToString((object)r3.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PE.Qty} " + $"Rate: {PE.Rate} " + PE.Remarks;
						}
						purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
					}
				}
				purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						if (SubsidiaryAccountAllownOnVouchers)
						{
							PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
							PJ.SupplierCustomerId = Conversion.ToInt(r2.Cells["AccountId"].Value);
						}
						else
						{
							PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
							PJ.SupplierCustomerId = Conversion.ToInt((object)0);
						}
						PJ.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						if (PJ.JvRemarks == string.Empty)
						{
							PJ.JvRemarks = Conversion.ToString((object)r2.Cells["AccountId"].Text);
						}
						PJ.JvPrcnt = Conversion.ToDouble(r2.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r2.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r2.Cells["Credit"].Value);
						PJ.InvGrnId = 0;
						PJ.FreightId = 0;
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) <= 0)
					{
						continue;
					}
					InvPurchaseInvoiceEmptyBags PEB = new InvPurchaseInvoiceEmptyBags();
					PEB.TypeId = Conversion.ToInt(r.Cells["Type"].Value);
					if (PEB.TypeId > 0)
					{
						if (PEB.TypeId == 1 || PEB.TypeId == 4 || PEB.TypeId == 5)
						{
							PEB.CreditAccountId = 0;
						}
						else
						{
							PEB.CreditAccountId = Conversion.ToInt(r.Cells["CreditAccountId"].Value);
							if (PEB.CreditAccountId <= 0)
							{
								MessageBox.Show("Credit Account Required in Empty Bag Grid in Row#" + (r.RowIndex + 1), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								tabControl2.SelectedIndex = 4;
								((Control)(object)grdEmptyBags).Focus();
								return;
							}
						}
					}
					PEB.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					PEB.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					PEB.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					PEB.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
					if (PEB.ItemConditionId == 0)
					{
						MessageBox.Show("Item Condition Required in Empty Bag Grid in Row#" + (r.RowIndex + 1), "Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					PEB.ReceivedQty = Conversion.ToDouble(r.Cells["ReceivedQty"].Value);
					PEB.PurchaseQty = Conversion.ToDouble(r.Cells["PurchaseQty"].Value);
					EmptyBagsRecQty += PEB.ReceivedQty;
					EmptyBagsPurQty += PEB.PurchaseQty;
					PEB.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PEB.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					if ((PEB.TypeId == 1 || PEB.TypeId == 2 || PEB.TypeId == 3) && PEB.Amount == 0.0)
					{
						throw new Exception("Packing Material Amount Required");
					}
					PEB.CustomRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					PEB.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					if (PEB.PurchaseQty > 0.0 && PEB.Rate > 0.0)
					{
						PEB.Remarks = "ItemName : " + Conversion.ToString((object)r.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PEB.PurchaseQty} " + $"Rate: {PEB.Rate} " + PEB.Remarks;
					}
					purchaseInvoice.InvPurchaseInvoiceEmptyBagslist.Add(PEB);
				}
				double EmpBagsQty = EmptyBagsRecQty + EmptyBagsPurQty;
				if (EmpBagsQty > 0.0 && EmpBagsQty > InvoiceQty)
				{
					MessageBox.Show("Empty Bags Quantity Not greater than ItemQty");
					return;
				}
				purchaseInvoice.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				purchaseInvoice.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				purchaseInvoice.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : purchaseInvoice.AttachmentsValues);
				purchaseInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 57, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Saved Successfully");
				}
				if (WagesStatus && WagesActiveOrInActive)
				{
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 57;
					frmwagesBillHeader.RefDocId = sucess;
					frmwagesBillHeader.GrossWeightTotal = GrossWt;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				Reset();
				if (ChkBok.Checked)
				{
					RecId = sucess;
					VoucherHeadIdGet();
					RecId = 0;
					CommonServices.VoucherReport_118(VoucherHeadId, 57);
				}
				if (ChkPrintSlip.Checked)
				{
					CommonServices.PurchaseInvoicetDirectSlip_225(sucess);
				}
				if (chkPartySlip.Checked)
				{
					CommonServices.PurchaseInvoicetDirectSlip_225A(sucess);
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
			RecId = 0;
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
			RecId = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(RecId);
			comCompany.Value = invPI.CompanyId;
			txtBranchSrNo.Text = Conversion.ToString((object)invPI.BranchSrNo);
			txtdocno.Text = Conversion.ToString((object)invPI.DocNo);
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			comsupplier.Value = invPI.SupplierCustomerId;
			compartyreference.Value = invPI.ReferencePartyId;
			comstockparty.Value = invPI.StockPartyId;
			txtSupplierReference.Text = Conversion.ToString((object)invPI.SupplierReferenceNo);
			txtbillno.Text = invPI.ManualBillNo;
			if (invPI.CommissionAgentId > 0)
			{
				combcommAgent.Value = invPI.CommissionAgentId;
			}
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = invPI.CommRate.ToString();
			((Control)(object)combcommUOM).Text = invPI.UomScheduleIdCmRate;
			txtcommamount.Text = invPI.CommAmount.ToString();
			txtCommissionRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			if (invPI.BrokerAgentId > 0)
			{
				CmbBrokeryAc.Value = invPI.BrokerAgentId;
			}
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)invPI.BrokeryType);
			txtBrokeryRate.Text = invPI.BrokeryRate.ToString();
			txtBrokeryAmount.Text = Conversion.ToString((object)invPI.BrokeryAmount);
			((Control)(object)CmbBrokeryRateUom).Text = invPI.BrokeryUom.ToString();
			txtremarks.Text = Conversion.ToString((object)invPI.OtherRemarks);
			txtBillAmount.Text = invPI.BillAmount.ToString();
			combpttrm.Value = Conversion.ToInt((object)invPI.PaymentTermsId);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)invPI.DeliveryTerm);
			duedate.Value = invPI.DueDate;
			txtduedays.Text = Conversion.ToString((object)invPI.DueDays);
			AttachmentsValues = invPI.AttachmentsValues;
			CustomAttachmentsValues = invPI.CustomAttachmentsValues;
			chkCustomAccounts.Checked = Conversion.ToBool((object)invPI.CustomAccounts);
			Approved = invPI.IsApproved;
			if (((Control)(object)combdeliverytrm).Text == "Ponch")
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble(r.Cells["Freight"].Value) > 0.0)
					{
						r.BeginEdit();
						r.Cells["Freight"].Value = 0;
						r.Cells["Transporter"].Value = string.Empty;
						r.EndEdit();
					}
				}
				grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)0;
				grdFreight.RootTable.Columns["Freight"].EditType = (EditType)0;
			}
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int l = 0; l < invPI.invPurchaseInvoiceDetailList.Count; l++)
			{
				dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[l].Id, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderId, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderDetailId, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[l].ItemId, invPI.invPurchaseInvoiceDetailList[l].ItemName, invPI.invPurchaseInvoiceDetailList[l].CropYear, invPI.invPurchaseInvoiceDetailList[l].JobLotId, invPI.invPurchaseInvoiceDetailList[l].JobLotDescription, invPI.invPurchaseInvoiceDetailList[l].PackingTypeId, invPI.invPurchaseInvoiceDetailList[l].PackTypeDesc, invPI.invPurchaseInvoiceDetailList[l].ItemUOMId, invPI.invPurchaseInvoiceDetailList[l].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[l].ItemQty, invPI.invPurchaseInvoiceDetailList[l].GrossWeight, invPI.invPurchaseInvoiceDetailList[l].EBWeight, Math.Round(invPI.invPurchaseInvoiceDetailList[l].EBTotalWt), invPI.invPurchaseInvoiceDetailList[l].WeightCut, Math.Round(invPI.invPurchaseInvoiceDetailList[l].WeightCutTotal), invPI.invPurchaseInvoiceDetailList[l].AdLsWeight, invPI.invPurchaseInvoiceDetailList[l].NetBillWeight, invPI.invPurchaseInvoiceDetailList[l].NetStockWeight, invPI.invPurchaseInvoiceDetailList[l].ItemRate, invPI.invPurchaseInvoiceDetailList[l].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[l].RateUom, invPI.invPurchaseInvoiceDetailList[l].RateCut, invPI.invPurchaseInvoiceDetailList[l].RateCutAmount, invPI.invPurchaseInvoiceDetailList[l].ItemAmount, invPI.invPurchaseInvoiceDetailList[l].WarehouseId, invPI.invPurchaseInvoiceDetailList[l].WareHouseName, invPI.invPurchaseInvoiceDetailList[l].LabAnalisysNo, invPI.invPurchaseInvoiceDetailList[l].GpDate, invPI.invPurchaseInvoiceDetailList[l].GpNo, invPI.invPurchaseInvoiceDetailList[l].VehicleNo, invPI.invPurchaseInvoiceDetailList[l].BillAmount, invPI.invPurchaseInvoiceDetailList[l].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[l].FreightAmount, invPI.invPurchaseInvoiceDetailList[l].JournalAmount, invPI.invPurchaseInvoiceDetailList[l].CommissionAmount, invPI.invPurchaseInvoiceDetailList[l].PurchaseGLAC, invPI.invPurchaseInvoiceDetailList[l].BranchId, invPI.invPurchaseInvoiceDetailList[l].BranchName);
				if (invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderId > 0)
				{
					PurchaseOrderId = invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderId;
					((Control)(object)comsupplier).Enabled = false;
					((Control)(object)combdeliverytrm).Enabled = false;
					((Control)(object)combpttrm).Enabled = false;
				}
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invPI.InvPurchaseInvoiceExpList.Count; k++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[k].Id, invPI.InvPurchaseInvoiceExpList[k].PurchaseOrderId, invPI.InvPurchaseInvoiceExpList[k].PurchaseOrderSupplierExpId, invPI.InvPurchaseInvoiceExpList[k].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[k].Qty, invPI.InvPurchaseInvoiceExpList[k].Rate, invPI.InvPurchaseInvoiceExpList[k].Amount, invPI.InvPurchaseInvoiceExpList[k].CustomRemarks);
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoicejournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invPI.InvPurchaseInvoicejournalList[j].SupplierCustomerId : invPI.InvPurchaseInvoicejournalList[j].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[j].JvRemarks, invPI.InvPurchaseInvoicejournalList[j].JvPrcnt, invPI.InvPurchaseInvoicejournalList[j].JvQty, invPI.InvPurchaseInvoicejournalList[j].JvRate, invPI.InvPurchaseInvoicejournalList[j].JvDebit, invPI.InvPurchaseInvoicejournalList[j].JvCredit, invPI.InvPurchaseInvoicejournalList[j].ChartofAccountId);
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < invPI.InvPurchaseInvoiceFreightList.Count; i++)
			{
				dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[i].PurchaseOrderId, invPI.InvPurchaseInvoiceFreightList[i].FreightId, invPI.InvPurchaseInvoiceFreightList[i].SupplierCustomerId, invPI.InvPurchaseInvoiceFreightList[i].SupplierCustomerId, invPI.InvPurchaseInvoiceFreightList[i].Percentage, invPI.InvPurchaseInvoiceFreightList[i].FrQty, invPI.InvPurchaseInvoiceFreightList[i].FrRate, Math.Round(invPI.InvPurchaseInvoiceFreightList[i].FreightAmount), invPI.InvPurchaseInvoiceFreightList[i].Debit, invPI.InvPurchaseInvoiceFreightList[i].FreightAmount, invPI.InvPurchaseInvoiceFreightList[i].Remarks, invPI.InvPurchaseInvoiceFreightList[i].TansporterId);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			dtEmptyBags.Rows.Clear();
			foreach (InvPurchaseInvoiceEmptyBags bag in invPI.InvPurchaseInvoiceEmptyBagslist)
			{
				DataRow newRow = dtEmptyBags.NewRow();
				newRow["PurchaseOrderId"] = bag.PurchaseOrderId;
				newRow["Type"] = bag.TypeId;
				newRow["ItemId"] = bag.ItemId;
				newRow["ItemConditionId"] = bag.ItemConditionId;
				newRow["ReceivedQty"] = bag.ReceivedQty;
				newRow["PurchaseQty"] = bag.PurchaseQty;
				newRow["Rate"] = bag.Rate;
				newRow["Amount"] = bag.Amount;
				newRow["Remarks"] = bag.CustomRemarks;
				newRow["CreditAccountId"] = bag.CreditAccountId;
				dtEmptyBags.Rows.Add(newRow);
			}
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
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
			BillAmount();
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						EntryUser = UserAccount.ID,
						DocumentTypeId = 57,
						Id = RecId
					});
					MessageBox.Show("Delete Voucher Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
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
			RecId = 0;
			VoucherHeadId = 0;
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
			Approved = false;
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
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
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
			((Control)(object)CmbWarehouse).Text = string.Empty;
			txtlabanalysisno.Text = string.Empty;
			txtgatepassno.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			dtFreight.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			dtEmptyBags.Rows.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)CmbItemName).Enabled = true;
			((Control)(object)comsupplier).Enabled = true;
			((Control)(object)combpttrm).Enabled = true;
			((Control)(object)combdeliverytrm).Enabled = true;
			txtRate.Enabled = true;
			chkCustomAccounts.Checked = false;
			btnDelete.Visible = false;
			BindGrids();
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId)));
			UpdateBranchSrNoUI(Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCodeBranch(DocumentTypeId, UserAccount.BranchesId)));
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			tabControl2.SelectedIndex = 0;
			DocDate.Focus();
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
			((Control)(object)CmbItemName).Text = string.Empty;
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
			CmbItemName.Focus();
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
			List<string> allServices = new List<string>
			{
				"AccountsWithCustomGroupId", "LookUpsEngr", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities",
				"CropYear", "JobLot", "InvPackingType", "AllBrands", "ItemConditions"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
			CommissionUOMFill();
			GetConfigurationsFromGlobal();
			SpecialRightsImplement(DatatableHelper.GetSpecialRightsByScreenIdDbCall(ScreenId));
			SupplierDtFillFromGlobal();
			BindSupplierName();
			PaymentTermBind(CommonServices.GetDueTermServiceBind());
			AccountsDtFillFromGlobal();
			WarehousesDtFillFromGlobalAndBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			OtherItemdtDbCall();
			EBComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		try
		{
			Show();
			notifyIcon1.Visible = false;
			base.WindowState = FormWindowState.Maximized;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchasedirectInvoice_Resize(object sender, EventArgs e)
	{
	}

	private void InvfrmPurchasedirectInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double TotalQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
					obj.Cells["Expense"].Value = Math.Round(TotalAmount / TotalQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
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
			if (!FreightDebitToExpenses)
			{
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
				double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
				CreditForFreight = Math.Round(CreditForFreight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (CreditForFreight > 0.0 && NetWeight > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
						obj.Cells["Freights"].Value = CreditForFreight / NetWeight * Weight;
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
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["Freights"].Value = 0;
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
			foreach (GridEXRow obj in rows)
			{
				double ItemAmount = Math.Round(Conversion.ToDouble(obj.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Expense = Math.Round(Conversion.ToDouble(obj.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Freights = Math.Round(Conversion.ToDouble(obj.Cells["Freights"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Commission = Math.Round(Conversion.ToDouble(obj.Cells["Commission"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.BeginEdit();
				obj.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense + Freights + Commission, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.EndEdit();
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
			double NetWeight = GridEX_Helper.GetColumnSum(grd, "NetBillWeight");
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)combCommType).Text == "Percent")
					{
						double ItemAmount = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString());
						r.Cells["Commission"].Value = Math.Round(ItemAmount * CommPercent / 100.0, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						r.EndEdit();
					}
					else
					{
						double Weight = Conversion.ToDouble((object)r.Cells["NetBillWeight"].Value.ToString());
						r.Cells["Commission"].Value = Math.Round(TotalAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
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

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			double ExpAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			double EmptyBagsAmount = GridEX_Helper.GetColumnSum(grdEmptyBags, "Amount");
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (int.Parse(r2.Cells["AccountId"].Value.ToString()) > 0)
				{
					if (txtSupplierGLId.Text.ToString() == r2.Cells["AccountId"].Value.ToString())
					{
						r2.BeginEdit();
						r2.Cells["AccountId"].Value = 0.ToString();
						throw new Exception("You cannot select supplier Account");
					}
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
			ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			ExpAmount = Math.Round(ExpAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			EmptyBagsAmount = Math.Round(EmptyBagsAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			JournalDebit = Math.Round(JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			JournalCredit = Math.Round(JournalCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			TransporterCredit = Math.Round(TransporterCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			BillAmount = ItemAmount + ExpAmount + EmptyBagsAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount += TransporterCredit;
			if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
			{
				BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			if (((UltraGridBase)CmbBrokeryAc).ActiveRow != null)
			{
				if (Conversion.ToDouble((object)txtBrokeryAmount.Text) > 0.0)
				{
					BillAmount -= Conversion.ToDouble((object)txtBrokeryAmount.Text.Trim());
				}
				else
				{
					txtBrokeryAmount.Text = "0";
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
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
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)combCommType).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					txtcommamount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtcommamount.Text = "0";
				}
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
						txtcommamount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)combCommType).Text == "Comm Weight")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)combcommUOM).Text.Trim());
						double CommAmt = num / UOM * CommRate2;
						txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		CommissionProportion();
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(comsupplier.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(comsupplier.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void combcommUOM_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void Total()
	{
		try
		{
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = Conversion.ToDouble((object)txtwtcut.Text);
			double WeightCutTotal = Conversion.ToDouble((object)txtWeightCutTotal.Text);
			double AddLess = 0.0;
			if (((UltraGridBase)comPackUOM).ActiveRow != null)
			{
				Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (Conversion.ToDouble((object)txtQty.Text) != 0.0)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text);
			}
			if (Conversion.ToDouble((object)txtGrossWeight.Text) != 0.0)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
			}
			if (Conversion.ToDouble((object)txtEmptybagsUnit.Text) != 0.0)
			{
				EBUnit = Conversion.ToDouble((object)txtEmptybagsUnit.Text);
			}
			if (Conversion.ToDouble((object)txtEmptyBagsTotal.Text) != 0.0)
			{
				EBTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmUnit")
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.###");
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmTotal")
			{
				EBUnit = EBTotal / Qty;
				txtEmptybagsUnit.Text = EBUnit.ToString("#,##0.####");
			}
			else
			{
				EBUnit = EBTotal / Qty;
				txtEmptybagsUnit.Text = EBUnit.ToString("#,##0.####");
			}
			if (Conversion.ToDouble((object)txtNetBillWeight.Text) != 0.0)
			{
				Conversion.ToDouble((object)txtNetBillWeight.Text);
			}
			if (Conversion.ToDouble((object)txtStockWeight.Text) != 0.0)
			{
				Conversion.ToDouble((object)txtStockWeight.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "WtCut")
			{
				WeightCutTotal = Qty * WeightCut;
				txtWeightCutTotal.Text = WeightCutTotal.ToString("#,##0.###");
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "WtCutTotal")
			{
				WeightCut = WeightCutTotal / Qty;
				txtwtcut.Text = WeightCut.ToString("#,##0.####");
			}
			else
			{
				WeightCut = WeightCutTotal / Qty;
				txtwtcut.Text = WeightCut.ToString("#,##0.####");
			}
			AddLess = ((Conversion.ToDouble((object)txtAddLss.Text) == 0.0) ? 0.0 : Conversion.ToDouble((object)txtAddLss.Text));
			double Weight = 0.0;
			Weight = ((!GridEX_Helper.IsAnyCellValueHaveSameValue(grdEmptyBags, "Type", 2)) ? (Math.Round(GrossWeight, 2) - EBTotal - Math.Round(WeightCutTotal, 2) + AddLess) : (Math.Round(GrossWeight, 2) - Math.Round(WeightCutTotal, 2) + AddLess));
			txtNetBillWeight.Text = Math.Round(Weight, 2).ToString("#,##0.###");
			if (GrossWeight > 0.0)
			{
				double StockWeiht = Math.Round(GrossWeight, 2) - EBTotal - Math.Round(WeightCutTotal, 2) + AddLess;
				txtStockWeight.Text = StockWeiht.ToString("#,##0.###");
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
			if (btnSave.Visible && btnSave.Enabled)
			{
				double PackUOM = 0.0;
				double Qty = 0.0;
				double GrossWeight = 0.0;
				if (((UltraGridBase)comPackUOM).ActiveRow != null)
				{
					PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				}
				if (Conversion.ToDouble((object)txtQty.Text) != 0.0)
				{
					Qty = Conversion.ToDouble((object)txtQty.Text);
				}
				if (Conversion.ToDouble((object)txtGrossWeight.Text) != 0.0)
				{
					GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
				}
				txtGrossWeight.Text = (PackUOM * Qty).ToString("#,##0.##");
			}
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
			if (((UltraGridBase)comRateUOM).ActiveRow != null)
			{
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
			}
			if (Conversion.ToDouble((object)txtRate.Text) != 0.0)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (Conversion.ToDouble((object)txtratecut.Text) != 0.0)
			{
				RateCut = Conversion.ToDouble((object)txtratecut.Text);
			}
			if (Conversion.ToDouble((object)txtNetBillWeight.Text) != 0.0)
			{
				NetWeight = Conversion.ToDouble((object)txtNetBillWeight.Text);
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = Math.Round(NetWeight / RateUOM * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				RateCutTotal = Math.Round(NetWeight / RateUOM * RateCut, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtratecuttotal.Text = RateCutTotal.ToString(clsGlobalVariables.stringFormatsingle);
				ItemAmount -= RateCutTotal;
				txtAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
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

	private void TotalBrokeryAmount()
	{
		try
		{
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					txtBrokeryAmount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Percent")
				{
					double CommRate3 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
						txtBrokeryAmount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Comm Weight")
				{
					double CommRate2 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
						double CommAmt = num / UOM * CommRate2;
						txtBrokeryAmount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			BillAmount();
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
			TotalWeight();
			Total();
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			TotalWeight();
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			TotalBrokeryAmount();
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
			AmountCaluculation();
			TotalCommissionAmount();
			TotalBrokeryAmount();
			BillAmount();
			CommonServices.CommasApplyWhileTyping(sender, e);
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
			TotalBrokeryAmount();
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
			TotalBrokeryAmount();
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
			TotalBrokeryAmount();
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			AmountCaluculation();
			TotalCommissionAmount();
			TotalBrokeryAmount();
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
			TotalBrokeryAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void combcommAgent_TextChanged(object sender, EventArgs e)
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

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtStockWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryAc_Leave(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void CmbBrokeryType_Leave(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void txtBrokeryRate_Leave(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void CmbBrokeryRateUom_Leave(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		txtvehicleno.Text = txtvehicleno.Text.ToUpper();
	}

	private void txtWeightCutTotal_TextChanged(object sender, EventArgs e)
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

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 57;
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
					dt.Columns.Add("OrderNo", typeof(int));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("PurchaseAgainst", typeof(string));
					dt.Columns.Add("DueDays", typeof(int));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("ManualBillNo", typeof(string));
					dt.Columns.Add("SupplierName", typeof(string));
					dt.Columns.Add("ReferenceParty", typeof(string));
					dt.Columns.Add("CommissionAgent", typeof(string));
					dt.Columns.Add("CommType", typeof(string));
					dt.Columns.Add("CommRate", typeof(double));
					dt.Columns.Add("CommAmount", typeof(double));
					dt.Columns.Add("CommRemarks", typeof(string));
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("ApprovedStatus", typeof(string));
					dt.Columns.Add("EntryUser", typeof(string));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("ModifyUser", typeof(string));
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUser", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("NoOfAttachments", typeof(int));
					dt.Columns.Add("Remarks", typeof(string));
					for (int i = 0; i < dthistory.Rows.Count; i++)
					{
						dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocumentTypeId"], dthistory.Rows[i]["OrderNo"], dthistory.Rows[i]["DocNo"], dthistory.Rows[i]["BranchSrNo"], dthistory.Rows[i]["BranchName"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["PurchaseAgainst"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["ReferencePartyName"], dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], dthistory.Rows[i]["CommAmount"], dthistory.Rows[i]["CommissionRemarks"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["ApprovedStatus"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUser"], dthistory.Rows[i]["PostDate"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
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
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["PurchaseAgainst"].Width = 100;
			grdHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.ReferenceParty;
			grdHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			grdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["CommissionAgent"].Caption = "CommAgent";
			grdHistory.RootTable.Columns["ReferenceParty"].Caption = "SupReference";
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("Slip");
			grdHistory.RootTable.Columns["Slip"].Caption = "Slip";
			grdHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdHistory.RootTable.Columns["Slip"].Position = 1;
			grdHistory.RootTable.Columns["Slip"].Width = 40;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Position = 2;
			grdHistory.RootTable.Columns["PartySlip"].Width = 70;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.FrozenColumns = 4;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 57);
			}
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
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Id;
				ReadById(RecId);
			}
			else if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 57);
			}
			else if (e.Column.Key == "Slip")
			{
				CommonServices.PurchaseInvoicetDirectSlip_225(Id);
			}
			else if (e.Column.Key == "PartySlip")
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = Id;
				DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "225A-InvRepPurchaseBillDirectWithoutPo.rpt", lstSubReports);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 57, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("OrderNo");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("EBTotalWt", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLessWt", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("GpDate", typeof(DateTime));
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("ItemNetAmount", typeof(double));
			dtdetail.Columns.Add("BranchName", typeof(string));
			InvPurchaseInvoice obj = new InvPurchaseInvoice();
			obj = InvPurchaseInvoice.GetByID(Id);
			if (obj.invPurchaseInvoiceDetailList != null && obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.invPurchaseInvoiceDetailList.Count; i++)
				{
					dtdetail.Rows.Add(obj.invPurchaseInvoiceDetailList[i].PurchaseOrder, obj.invPurchaseInvoiceDetailList[i].ItemName, obj.invPurchaseInvoiceDetailList[i].CropYear, obj.invPurchaseInvoiceDetailList[i].JobLotDescription, obj.invPurchaseInvoiceDetailList[i].PackTypeDesc, obj.invPurchaseInvoiceDetailList[i].UOMCodeItem, obj.invPurchaseInvoiceDetailList[i].ItemQty, obj.invPurchaseInvoiceDetailList[i].GrossWeight, obj.invPurchaseInvoiceDetailList[i].EBTotalWt, obj.invPurchaseInvoiceDetailList[i].WeightCutTotal, obj.invPurchaseInvoiceDetailList[i].AdLsWeight, obj.invPurchaseInvoiceDetailList[i].NetBillWeight, obj.invPurchaseInvoiceDetailList[i].NetStockWeight, obj.invPurchaseInvoiceDetailList[i].ItemRate, obj.invPurchaseInvoiceDetailList[i].RateUom, obj.invPurchaseInvoiceDetailList[i].RateCut, obj.invPurchaseInvoiceDetailList[i].RateCutAmount, obj.invPurchaseInvoiceDetailList[i].ItemAmount, obj.invPurchaseInvoiceDetailList[i].WareHouseName, Conversion.ToDateTime((object)obj.invPurchaseInvoiceDetailList[i].GpDate).ToShortDateString(), obj.invPurchaseInvoiceDetailList[i].GpNo, obj.invPurchaseInvoiceDetailList[i].VehicleNo, obj.invPurchaseInvoiceDetailList[i].ExpenseAmount, obj.invPurchaseInvoiceDetailList[i].FreightAmount, obj.invPurchaseInvoiceDetailList[i].CommissionAmount, obj.invPurchaseInvoiceDetailList[i].BillAmount, obj.invPurchaseInvoiceDetailList[i].BranchName);
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			grdDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["EBTotalWt"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["AddLessWt"].Width = Constants.InventoryConstants.AddLessConstant;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["NetStockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["ExpenseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdDetail.RootTable.Columns["AddLessWt"].Caption = "Ad / Ls";
			grdDetail.RootTable.Columns["ItemUOM"].Caption = "Pack UOM";
			grdDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["NetStockWeight"].Caption = "Stock Weight";
			grdDetail.RootTable.Columns["ItemRate"].Caption = "Rate";
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLessWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessWt"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLessWt"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar3_Load(null, null);
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
			if (item != null && (int)item.RowType == 0)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				GetDetailGrdByHeadId(id);
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
			HistoryBranchdtFillDBCall();
			HistoryComboBranchBind();
			HistoryComboBind(HistoryComboDbCall());
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

	private void btnPurchaseOrderLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					GridIds = GridIds + "," + Conversion.ToString(r.Cells["PurchaseOrderId"].Value);
				}
				GridEXRow item = grd.CurrentRow;
				if (item != null && Conversion.ToInt(item.Cells["PurchaseOrderId"].Value) == 0)
				{
					MessageBox.Show("You cannot Load PurchaseOrder because you add record without Order");
					return;
				}
			}
			LoadPO = new LoadPurchaseOrder(UserAccount);
			LoadPO.DocumentTypeId = 41;
			LoadPO.ShowDialog();
			LoadPO.BringToFront();
			LoadInGridDetail();
			GridIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			string POIds = "";
			PurchaseOrderId = 0;
			for (int i = 0; i < LoadPO.dtSupply.Rows.Count; i++)
			{
				PurchaseOrderId = int.Parse(LoadPO.dtSupply.Rows[0][0].ToString());
				POIds = POIds + "," + int.Parse(LoadPO.dtSupply.Rows[i][0].ToString());
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			obj.Ids = POIds;
			LoadPurchaseOrderDataForInvoice(obj);
			LoadPurchaseOrderExpensesChargToProductData(POIds);
			LoadExpData(POIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			GetEmptyBagsInformationFromOrder();
			POIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadPurchaseOrderDataForInvoice(ReportsParameters obj)
	{
		try
		{
			DataTable invGrn = InvPurchaseInvoice.PurchaseOrderLoadForPurchaseInvoice(obj);
			if (invGrn.Rows.Count <= 0)
			{
				return;
			}
			((Control)(object)combCommType).Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			comsupplier.Value = invGrn.Rows[0]["SupplierCustomerId"];
			combcommAgent.Value = invGrn.Rows[0]["BrokerAgentSupCustId"];
			((Control)(object)combCommType).Text = invGrn.Rows[0]["CommissionType"].ToString();
			txtcommrate.Text = invGrn.Rows[0]["CommRate"].ToString();
			((Control)(object)combcommUOM).Text = invGrn.Rows[0]["UomScheduleIdCmRate"].ToString();
			txtcommamount.Text = invGrn.Rows[0]["CommAmount"].ToString();
			txtCommissionRemarks.Text = invGrn.Rows[0]["CommissionRemarks"].ToString();
			CmbBrokeryAc.Value = invGrn.Rows[0]["BrokerAgentId"];
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString(invGrn.Rows[0]["BrokeryType"]);
			txtBrokeryRate.Text = Conversion.ToString(invGrn.Rows[0]["BrokeryRate"]);
			((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString(invGrn.Rows[0]["BrokeryUom"]);
			txtBrokeryAmount.Text = Conversion.ToString(invGrn.Rows[0]["BrokeryAmount"]);
			txtremarks.Text = invGrn.Rows[0]["RemarksHeader"].ToString();
			((Control)(object)combdeliverytrm).Text = invGrn.Rows[0]["DeliveryTerm"].ToString();
			combpttrm.Value = invGrn.Rows[0]["PaymentTermsId"];
			duedate.Value = Conversion.ToDateTime(invGrn.Rows[0]["OrderDueDate"]);
			txtduedays.Text = Conversion.ToString(invGrn.Rows[0]["OrderDueDays"]);
			((Control)(object)comsupplier).Enabled = false;
			((Control)(object)combdeliverytrm).Enabled = false;
			((Control)(object)combpttrm).Enabled = false;
			((Control)(object)CmbItemName).Enabled = false;
			DueDaysCalculate();
			if (((Control)(object)combdeliverytrm).Text == "Ponch" || ((Control)(object)combdeliverytrm).Text == "Ponch & PartyWeight" || ((Control)(object)combdeliverytrm).Text == "Ponch & FactoryWeight")
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble(r.Cells["Freight"].Value) > 0.0)
					{
						r.BeginEdit();
						r.Cells["Freight"].Value = 0;
						r.Cells["Transporter"].Value = string.Empty;
						r.EndEdit();
					}
				}
				grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)0;
				grdFreight.RootTable.Columns["Freight"].EditType = (EditType)0;
			}
			bool flag = false;
			for (int i = 0; i < invGrn.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["PurchaseOrderId"].Value) != Conversion.ToInt(invGrn.Rows[i]["Id"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Detail");
							return;
						}
						if (Conversion.ToInt(item.Cells["PurchaseOrderDetailId"].Value) == Conversion.ToInt(invGrn.Rows[i]["PODetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, invGrn.Rows[i]["Id"], invGrn.Rows[i]["PODetailId"], invGrn.Rows[i]["OrderNo"], invGrn.Rows[i]["ItemId"], invGrn.Rows[i]["ItemName"], invGrn.Rows[i]["Crop"], invGrn.Rows[i]["JobLotId"], invGrn.Rows[i]["JobLotDescription"], 0, 0, invGrn.Rows[i]["OrderItemUOMId"], invGrn.Rows[i]["PackUom"], invGrn.Rows[i]["ItemQty"], invGrn.Rows[i]["GrossWeight"], invGrn.Rows[i]["EmptyBags"], invGrn.Rows[i]["EmptyBagsTotal"], 0, 0, 0, Conversion.ToDouble(invGrn.Rows[i]["GrossWeight"]) - Conversion.ToDouble(invGrn.Rows[i]["EmptyBagsTotal"]), Conversion.ToDouble(invGrn.Rows[i]["GrossWeight"]) - Conversion.ToDouble(invGrn.Rows[i]["EmptyBagsTotal"]), invGrn.Rows[i]["ItemRate"], invGrn.Rows[i]["OrderItemRateUOMId"], invGrn.Rows[i]["RateUomCode"], 0, 0, (Conversion.ToDouble(invGrn.Rows[i]["GrossWeight"]) - Conversion.ToDouble(invGrn.Rows[i]["EmptyBagsTotal"])) / Conversion.ToDouble(invGrn.Rows[i]["RateUOM"]) * Conversion.ToDouble(invGrn.Rows[i]["ItemRate"]), 0, "", invGrn.Rows[i]["LabSampleNo"], DBNull.Value, 0, "", (Conversion.ToDouble(invGrn.Rows[i]["GrossWeight"]) - Conversion.ToDouble(invGrn.Rows[i]["EmptyBagsTotal"])) / Conversion.ToDouble(invGrn.Rows[i]["RateUOM"]) * Conversion.ToDouble(invGrn.Rows[i]["ItemRate"]), 0, 0, 0, 0, invGrn.Rows[i]["PurchaseGLAC"], invGrn.Rows[i]["BranchId"], invGrn.Rows[i]["BranchName"]);
				}
				flag = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPurchaseOrderExpensesChargToProductData(string POIds)
	{
		try
		{
			if (!(POIds != string.Empty))
			{
				return;
			}
			string DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			DataTable item = InvPurchaseInvoice.GetPurchaseOrderExpensesChargToProdut(0, POIds);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < item.Rows.Count; i++)
			{
				double Amount = Conversion.ToDouble(item.Rows[i]["Amount"]);
				if (SubsidiaryAccountAllownOnVouchers)
				{
					switch (DeliveryTerm)
					{
					case "Load":
					case "Load & PartyWeight":
					case "Load & FactoryWeight":
						dtFreight.Rows.Add(item.Rows[i]["PurchaseOrderId"], 0, item.Rows[i]["SupplierCustomerId"], item.Rows[i]["SupplierCustomerId"], item.Rows[i]["Percentage"], item.Rows[i]["Qty"], item.Rows[i]["Rate"], Math.Round(Amount), 0, 0, item.Rows[i]["Remarks"], item.Rows[i]["AccountId"]);
						break;
					}
				}
				else
				{
					switch (DeliveryTerm)
					{
					case "Load":
					case "Load & PartyWeight":
					case "Load & FactoryWeight":
						dtFreight.Rows.Add(item.Rows[i]["PurchaseOrderId"], 0, item.Rows[i]["AccountId"], item.Rows[i]["AccountId"], item.Rows[i]["Percentage"], item.Rows[i]["Qty"], item.Rows[i]["Rate"], Math.Round(Amount), 0, 0, item.Rows[i]["Remarks"], 0);
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string IDs)
	{
		try
		{
			DataTable item = InvPurchaseInvoice.PurchaseOrderSupplierExpenseByOrderIds(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				bool flag = false;
				if (grdInvExp.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdInvExp.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) > 0 && Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) != Conversion.ToInt(item.Rows[i]["PurchaseOrderId"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Expense Grid");
							return;
						}
						if (Conversion.ToInt(r.Cells["PurchaseOrderSupplierExpId"].Value) == Conversion.ToInt(item.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtInvExp.Rows.Add(0, item.Rows[i]["PurchaseOrderId"], item.Rows[i]["Id"], item.Rows[i]["InvRevExpItemId"], Conversion.ToDouble(item.Rows[i]["Qty"]), Conversion.ToDouble(item.Rows[i]["Rate"]), Conversion.ToDouble(item.Rows[i]["Amount"]), Conversion.ToString(item.Rows[i]["Remarks"]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId, 57);
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
			CommonServices.PurchaseInvoicetDirectSlip_225(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn225PartySlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoicetDirectSlip_225A(RecId);
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void txtBrokeryRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWeightCutTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEmptyBagsTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void combpttrm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				duedate.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDaysCalculate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
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

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void duedate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (duedate.Value.Date < DocDate.Value.Date)
			{
				duedate.Value = DocDate.Value;
			}
			int days = (int)(duedate.Value.Date - DocDate.Value.Date).TotalDays;
			txtduedays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						saveToolStripButton_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				btnSlip_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbItemName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				tabControl2.SelectedIndex = 0;
				((Control)(object)grdFreight).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdInvExp).Focus();
					return;
				}
				if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
					((Control)(object)grdGLedger).Focus();
					return;
				}
				if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 3;
					((Control)(object)grdEmptyBags).Focus();
					return;
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					GridEXRow item4 = grdFreight.CurrentRow;
					dtFreight.Rows[item4.RowIndex].Delete();
					if (item4 != null && (int)item4.RowType == 0)
					{
						item4.Delete();
						FreightProportion();
					}
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (tabControl2.SelectedIndex == 1)
				{
					GridEXRow item3 = grdInvExp.CurrentRow;
					dtInvExp.Rows[item3.RowIndex].Delete();
					if (item3 != null && (int)item3.RowType == 0)
					{
						item3.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (tabControl2.SelectedIndex == 2)
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
				if (tabControl2.SelectedIndex == 3)
				{
					GridEXRow item = grdEmptyBags.CurrentRow;
					dtEmptyBags.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtEmptyBags.Rows.Count == 0)
					{
						AddRowInvEmptyBagsGrid();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					AddRowInFreightGrid();
				}
				if (tabControl2.SelectedIndex == 1)
				{
					AddRowInvExpGrid();
				}
				if (tabControl2.SelectedIndex == 2)
				{
					AddRowInGLGrid();
				}
				if (tabControl2.SelectedIndex == 3)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					CmbOrderNo.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter" && e.KeyCode == Keys.F1)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["Transporter"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlstForFreight, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["Transporter"].Value = obj2.ReturnId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGLedger.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId" && e.KeyCode == Keys.F1)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["AccountId"].Value = obj2.ReturnId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Expected O, but got Unknown
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Expected O, but got Unknown
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Expected O, but got Unknown
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df5: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_1787: Unknown result type (might be due to invalid IL or missing references)
		//IL_1791: Expected O, but got Unknown
		//IL_1a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a50: Expected O, but got Unknown
		//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a67: Expected O, but got Unknown
		//IL_1a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7e: Expected O, but got Unknown
		//IL_1d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d51: Expected O, but got Unknown
		//IL_1d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d68: Expected O, but got Unknown
		//IL_1d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7f: Expected O, but got Unknown
		//IL_200c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2016: Expected O, but got Unknown
		//IL_2023: Unknown result type (might be due to invalid IL or missing references)
		//IL_202d: Expected O, but got Unknown
		//IL_203a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Expected O, but got Unknown
		//IL_22db: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e5: Expected O, but got Unknown
		//IL_22f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fc: Expected O, but got Unknown
		//IL_2309: Unknown result type (might be due to invalid IL or missing references)
		//IL_2313: Expected O, but got Unknown
		//IL_c6f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_c6fa: Expected O, but got Unknown
		//IL_c71e: Unknown result type (might be due to invalid IL or missing references)
		//IL_c728: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
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
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
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
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance209 = new Appearance();
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
		Appearance appearance222 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.InvfrmPurchasedirectInvoice));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabFreight = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.tabOtherExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label66 = new System.Windows.Forms.Label();
		this.tabSupplierAddLess = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label67 = new System.Windows.Forms.Label();
		this.tabEmptyBags = new System.Windows.Forms.TabPage();
		this.grdEmptyBags = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label68 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtWeightCutTotal = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.txtratecuttotal = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbOrderNo = new UltraCombo();
		this.txtratecut = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.comRateUOM = new UltraCombo();
		this.comPackUOM = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtlabanalysisno = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEmptybagsUnit = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.compartyreference = new UltraCombo();
		this.comstockparty = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.label5 = new System.Windows.Forms.Label();
		this.chkPartySlip = new System.Windows.Forms.CheckBox();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.CmbBrokeryType = new UltraCombo();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.CmbBrokeryAc = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.combCommType = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.combcommUOM = new UltraCombo();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
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
		this.comCompany = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btn225PartySlip = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPurchaseOrderLoader = new System.Windows.Forms.ToolStripButton();
		this.BtnSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label58 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label57 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.tabFreight.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel5.SuspendLayout();
		this.tabOtherExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel13.SuspendLayout();
		this.tabSupplierAddLess.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel14.SuspendLayout();
		this.tabEmptyBags.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel6.SuspendLayout();
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
		this.tabControl1.Size = new System.Drawing.Size(1318, 704);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1310, 675);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1304, 669);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabFreight);
		this.tabControl2.Controls.Add(this.tabOtherExpense);
		this.tabControl2.Controls.Add(this.tabSupplierAddLess);
		this.tabControl2.Controls.Add(this.tabEmptyBags);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 326);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1304, 343);
		this.tabControl2.TabIndex = 0;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel8);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Size = new System.Drawing.Size(1296, 314);
		this.tabDetail.TabIndex = 4;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1296, 294);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1296, 20);
		this.panel8.TabIndex = 32;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1250, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 18);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabFreight.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFreight.Controls.Add(this.panel5);
		this.tabFreight.Location = new System.Drawing.Point(4, 25);
		this.tabFreight.Name = "tabFreight";
		this.tabFreight.Padding = new System.Windows.Forms.Padding(3);
		this.tabFreight.Size = new System.Drawing.Size(1296, 314);
		this.tabFreight.TabIndex = 0;
		this.tabFreight.Text = "Charge to Product";
		this.tabFreight.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 21);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1290, 290);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).Enter += new System.EventHandler(grdFreight_Enter);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label65);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.ForeColor = System.Drawing.Color.White;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1290, 18);
		this.panel5.TabIndex = 70;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(5, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(121, 14);
		this.label65.TabIndex = 42;
		this.label65.Text = "Charge to Product";
		this.tabOtherExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabOtherExpense.Controls.Add(this.panel13);
		this.tabOtherExpense.Location = new System.Drawing.Point(4, 25);
		this.tabOtherExpense.Name = "tabOtherExpense";
		this.tabOtherExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherExpense.Size = new System.Drawing.Size(1296, 314);
		this.tabOtherExpense.TabIndex = 1;
		this.tabOtherExpense.Text = "Other Expenses Credit to Supplier & Charge to Product";
		this.tabOtherExpense.UseVisualStyleBackColor = true;
		this.grdInvExp.AutomaticSort = false;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(3, 21);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1290, 290);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label66);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.ForeColor = System.Drawing.Color.White;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1290, 18);
		this.panel13.TabIndex = 71;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(5, 2);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(337, 14);
		this.label66.TabIndex = 42;
		this.label66.Text = "Other Expenses Credit to Supplier & Charge to Product";
		this.tabSupplierAddLess.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabSupplierAddLess.Controls.Add(this.panel14);
		this.tabSupplierAddLess.Location = new System.Drawing.Point(4, 25);
		this.tabSupplierAddLess.Name = "tabSupplierAddLess";
		this.tabSupplierAddLess.Size = new System.Drawing.Size(1296, 314);
		this.tabSupplierAddLess.TabIndex = 2;
		this.tabSupplierAddLess.Text = "Supplier AddLess";
		this.tabSupplierAddLess.UseVisualStyleBackColor = true;
		this.grdGLedger.AutomaticSort = false;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 18);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1296, 296);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label67);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.ForeColor = System.Drawing.Color.White;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1296, 18);
		this.panel14.TabIndex = 72;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(5, 2);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(112, 14);
		this.label67.TabIndex = 42;
		this.label67.Text = "Supplier AddLess";
		this.tabEmptyBags.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.tabEmptyBags.Controls.Add(this.panel15);
		this.tabEmptyBags.Location = new System.Drawing.Point(4, 25);
		this.tabEmptyBags.Name = "tabEmptyBags";
		this.tabEmptyBags.Size = new System.Drawing.Size(1296, 314);
		this.tabEmptyBags.TabIndex = 3;
		this.tabEmptyBags.Text = "EmptyBags";
		this.tabEmptyBags.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 18);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(1296, 296);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 2;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.UpdatingCell += new UpdatingCellEventHandler(grdEmptyBags_UpdatingCell);
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.grdEmptyBags.SelectionChanged += new System.EventHandler(grdEmptyBags_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Click += new System.EventHandler(grdEmptyBags_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label68);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.ForeColor = System.Drawing.Color.White;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1296, 18);
		this.panel15.TabIndex = 73;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(5, 2);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(79, 14);
		this.label68.TabIndex = 42;
		this.label68.Text = "Empty Bags";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtdeliverydays);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.compartyreference);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comstockparty);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.validate);
		this.panel4.Controls.Add(this.deliverystartdate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 194);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1304, 132);
		this.panel4.TabIndex = 30;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.rdSearchByCode);
		this.groupBox1.Controls.Add(this.rdSearchByName);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtWeightCutTotal);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtratecuttotal);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox1.Controls.Add(this.txtratecut);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtwtcut);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.txtlabanalysisno);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtStockWeight);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtAddLss);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtEmptybagsUnit);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1291, 123);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(187, 13);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 56;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(128, 13);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 55;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1224, 88);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(59, 25);
		this.btnCancelUpdateDetial.TabIndex = 49;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1160, 88);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(62, 25);
		this.btnUpdateDetail.TabIndex = 48;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtWeightCutTotal.BackColor = System.Drawing.Color.White;
		this.txtWeightCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCutTotal.Location = new System.Drawing.Point(1129, 38);
		this.txtWeightCutTotal.Name = "txtWeightCutTotal";
		this.txtWeightCutTotal.Size = new System.Drawing.Size(79, 23);
		this.txtWeightCutTotal.TabIndex = 33;
		this.txtWeightCutTotal.Tag = "WtCutTotal";
		this.txtWeightCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightCutTotal.TextChanged += new System.EventHandler(txtWeightCutTotal_TextChanged);
		this.txtWeightCutTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutTotal_KeyPress);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(1132, 15);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(54, 15);
		this.label52.TabIndex = 22;
		this.label52.Text = "Cut Total";
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgatepassno.Location = new System.Drawing.Point(998, 89);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(50, 23);
		this.txtgatepassno.TabIndex = 45;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1131, 88);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(27, 25);
		this.btnAdd.TabIndex = 47;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(426, 70);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(80, 15);
		this.label50.TabIndex = 34;
		this.label50.Text = "Rate Cut Total";
		this.txtratecuttotal.BackColor = System.Drawing.Color.White;
		this.txtratecuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecuttotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratecuttotal.Location = new System.Drawing.Point(428, 89);
		this.txtratecuttotal.Name = "txtratecuttotal";
		this.txtratecuttotal.ReadOnly = true;
		this.txtratecuttotal.Size = new System.Drawing.Size(86, 23);
		this.txtratecuttotal.TabIndex = 40;
		this.txtratecuttotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(368, 70);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(51, 15);
		this.label49.TabIndex = 32;
		this.label49.Text = "Rate Cut";
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(11, 37);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(83, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(CmbOrderNo_Leave);
		this.txtratecut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratecut.Location = new System.Drawing.Point(371, 89);
		this.txtratecut.Name = "txtratecut";
		this.txtratecut.Size = new System.Drawing.Size(54, 23);
		this.txtratecut.TabIndex = 39;
		this.txtratecut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtratecut.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtratecut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(1050, 70);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 46;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(1050, 89);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(79, 23);
		this.txtvehicleno.TabIndex = 46;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(1004, 70);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 44;
		this.label47.Text = "GP #";
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtcut.Location = new System.Drawing.Point(1077, 38);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(49, 23);
		this.txtwtcut.TabIndex = 32;
		this.txtwtcut.Tag = "WtCut";
		this.txtwtcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwtcut.TextChanged += new System.EventHandler(txtwtcut_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1077, 15);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(44, 15);
		this.label46.TabIndex = 20;
		this.label46.Text = "Wt.Cut";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(636, 88);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 42;
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(297, 88);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(69, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 38;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(669, 37);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 27;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(422, 37);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 26;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance225;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance230).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance231).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance233).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(298, 37);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 25;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance238;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance244).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(543, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 26;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(97, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 24;
		this.CmbItemName.ValueChanged += new System.EventHandler(comItem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(comItem_Leave);
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(891, 89);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(104, 23);
		this.txtgpdate.TabIndex = 44;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(211, 70);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(30, 15);
		this.label42.TabIndex = 28;
		this.label42.Text = "Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(300, 15);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(57, 15);
		this.label41.TabIndex = 4;
		this.label41.Text = "Crop Year";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(209, 89);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(85, 23);
		this.txtRate.TabIndex = 37;
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
		this.txtGrossWeight.Location = new System.Drawing.Point(825, 38);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(101, 23);
		this.txtGrossWeight.TabIndex = 29;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(822, 15);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 14;
		this.label39.Text = "Gross Weight";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(741, 38);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(81, 23);
		this.txtQty.TabIndex = 28;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(738, 15);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 12;
		this.label38.Text = "Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(890, 70);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 42;
		this.label35.Text = "GP Date";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(9, 89);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(95, 23);
		this.txtNetBillWeight.TabIndex = 35;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(8, 70);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 24;
		this.label36.Text = "Net Bill Weight";
		this.txtlabanalysisno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlabanalysisno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlabanalysisno.Location = new System.Drawing.Point(795, 89);
		this.txtlabanalysisno.Name = "txtlabanalysisno";
		this.txtlabanalysisno.Size = new System.Drawing.Size(95, 23);
		this.txtlabanalysisno.TabIndex = 43;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(792, 70);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(82, 15);
		this.label37.TabIndex = 40;
		this.label37.Text = "Lab Analysis #";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(99, 15);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 2;
		this.label32.Text = "Item Name";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(424, 15);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 6;
		this.label33.Text = "Job Lot";
		this.txtStockWeight.BackColor = System.Drawing.Color.White;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(107, 89);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(96, 23);
		this.txtStockWeight.TabIndex = 36;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockWeight.TextChanged += new System.EventHandler(txtStockWeight_TextChanged);
		this.txtStockWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockWeight_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(102, 70);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 15);
		this.label34.TabIndex = 26;
		this.label34.Text = "Stock Weight";
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLss.Location = new System.Drawing.Point(1210, 38);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(58, 23);
		this.txtAddLss.TabIndex = 34;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.txtAddLss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLss_KeyPress);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(633, 70);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 38;
		this.label31.Text = "WareHouse";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1207, 15);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(42, 15);
		this.label30.TabIndex = 31;
		this.label30.Text = "AD/LS";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(12, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(31, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Po #";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(668, 15);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 10;
		this.label28.Text = "Pack UOM";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(298, 70);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 30;
		this.label22.Text = "Rate UOM";
		this.txtEmptyBagsTotal.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(998, 38);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(75, 23);
		this.txtEmptyBagsTotal.TabIndex = 31;
		this.txtEmptyBagsTotal.Tag = "PmTotal";
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.txtEmptyBagsTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptyBagsTotal_KeyPress);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1002, 15);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(66, 15);
		this.label16.TabIndex = 18;
		this.label16.Text = "PMW Total";
		this.txtEmptybagsUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptybagsUnit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptybagsUnit.Location = new System.Drawing.Point(930, 38);
		this.txtEmptybagsUnit.Name = "txtEmptybagsUnit";
		this.txtEmptybagsUnit.Size = new System.Drawing.Size(64, 23);
		this.txtEmptybagsUnit.TabIndex = 30;
		this.txtEmptybagsUnit.Tag = "PmUnit";
		this.txtEmptybagsUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptybagsUnit.TextChanged += new System.EventHandler(txtEmptybagsUnit_TextChanged);
		this.txtEmptybagsUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptybagsUnit_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(927, 15);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 16;
		this.label15.Text = "PMW/Unit";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(516, 89);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(117, 23);
		this.txtAmount.TabIndex = 41;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(518, 70);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 36;
		this.label14.Text = "Amount";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(543, 15);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 8;
		this.label9.Text = "Packing Type";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(1152, 63);
		this.txtdeliverydays.Multiline = true;
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(78, 26);
		this.txtdeliverydays.TabIndex = 22;
		this.txtdeliverydays.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1068, 92);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 17);
		this.label8.TabIndex = 0;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1068, 109);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(99, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "Reference Party";
		this.label2.Visible = false;
		this.compartyreference.AllowNull = (DefaultableBoolean)1;
		this.compartyreference.AutoCompleteMode = (AutoCompleteMode)4;
		this.compartyreference.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.compartyreference.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.compartyreference).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.compartyreference).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.compartyreference).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.compartyreference).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.compartyreference.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.compartyreference).Location = new System.Drawing.Point(1223, 97);
		((System.Windows.Forms.Control)(object)this.compartyreference).Name = "compartyreference";
		((System.Windows.Forms.Control)(object)this.compartyreference).Size = new System.Drawing.Size(64, 27);
		((System.Windows.Forms.Control)(object)this.compartyreference).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.compartyreference).Visible = false;
		this.comstockparty.AllowNull = (DefaultableBoolean)1;
		this.comstockparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.comstockparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comstockparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comstockparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comstockparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comstockparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comstockparty).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comstockparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comstockparty).Location = new System.Drawing.Point(1152, 95);
		((System.Windows.Forms.Control)(object)this.comstockparty).Name = "comstockparty";
		((System.Windows.Forms.Control)(object)this.comstockparty).Size = new System.Drawing.Size(65, 27);
		((System.Windows.Forms.Control)(object)this.comstockparty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comstockparty).Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1060, 37);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(66, 17);
		this.label45.TabIndex = 45;
		this.label45.Text = "Start Date";
		this.label45.Visible = false;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1060, 9);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(81, 17);
		this.label44.TabIndex = 44;
		this.label44.Text = "Validite Date";
		this.label44.Visible = false;
		this.validate.CustomFormat = "dd-MM-yy";
		this.validate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1153, 8);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(77, 25);
		this.validate.TabIndex = 19;
		this.validate.Visible = false;
		this.deliverystartdate.CustomFormat = "dd-MM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(1153, 36);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(77, 25);
		this.deliverystartdate.TabIndex = 21;
		this.deliverystartdate.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1060, 65);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(86, 17);
		this.label27.TabIndex = 49;
		this.label27.Text = "Delivery Days";
		this.label27.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtremarks);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.txtBranchSrNo);
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.label53);
		this.panel3.Controls.Add(this.label54);
		this.panel3.Controls.Add(this.label55);
		this.panel3.Controls.Add(this.txtBrokeryRate);
		this.panel3.Controls.Add(this.label56);
		this.panel3.Controls.Add(this.txtBrokeryAmount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.txtBillAmount);
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
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1304, 167);
		this.panel3.TabIndex = 0;
		this.txtremarks.Location = new System.Drawing.Point(96, 139);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(1170, 23);
		this.txtremarks.TabIndex = 6;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 34);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(88, 15);
		this.label13.TabIndex = 2;
		this.label13.Text = "Doc No && Date";
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(170, 30);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(61, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.chkCustomAccounts);
		this.panel12.Controls.Add(this.label5);
		this.panel12.Controls.Add(this.chkPartySlip);
		this.panel12.Controls.Add(this.ChkPrintSlip);
		this.panel12.Controls.Add(this.ChkBok);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.ForeColor = System.Drawing.Color.White;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1304, 25);
		this.panel12.TabIndex = 69;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(726, 3);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(153, 20);
		this.chkCustomAccounts.TabIndex = 512;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(267, 19);
		this.label5.TabIndex = 42;
		this.label5.Text = "PURCHASE DIRECT INVOICE     ";
		this.chkPartySlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPartySlip.AutoSize = true;
		this.chkPartySlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPartySlip.Location = new System.Drawing.Point(1019, 3);
		this.chkPartySlip.Margin = new System.Windows.Forms.Padding(2);
		this.chkPartySlip.Name = "chkPartySlip";
		this.chkPartySlip.Size = new System.Drawing.Size(139, 20);
		this.chkPartySlip.TabIndex = 0;
		this.chkPartySlip.Text = "Party Slip-225A";
		this.chkPartySlip.UseVisualStyleBackColor = true;
		this.ChkPrintSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.Location = new System.Drawing.Point(888, 3);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(125, 20);
		this.ChkPrintSlip.TabIndex = 0;
		this.ChkPrintSlip.Text = "Item Slip-225";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(1174, 3);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(124, 20);
		this.ChkBok.TabIndex = 1;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance251;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance257).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(1037, 56);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TextChanged += new System.EventHandler(CmbBrokeryType_Leave);
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance217).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(1037, 84);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_Leave);
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance264;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(1037, 28);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Leave += new System.EventHandler(CmbBrokeryAc_Leave);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(955, 34);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(82, 15);
		this.label23.TabIndex = 34;
		this.label23.Text = "Brokery Agent";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(1125, 90);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(51, 15);
		this.label53.TabIndex = 41;
		this.label53.Text = "Amount";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(955, 62);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(75, 15);
		this.label54.TabIndex = 36;
		this.label54.Text = "Brokery Type";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(1125, 62);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(73, 15);
		this.label55.TabIndex = 38;
		this.label55.Text = "Brokery Rate";
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryRate.Location = new System.Drawing.Point(1198, 58);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(68, 23);
		this.txtBrokeryRate.TabIndex = 19;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_Leave);
		this.txtBrokeryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryRate_KeyPress);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(955, 90);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(61, 15);
		this.label56.TabIndex = 39;
		this.label56.Text = "Rate UOM";
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(1198, 86);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(68, 23);
		this.txtBrokeryAmount.TabIndex = 21;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(461, 28);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 7;
		this.combpttrm.ValueChanged += new System.EventHandler(combpttrm_ValueChanged);
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(707, 56);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.combCommType).TextChanged += new System.EventHandler(combCommType_TextChanged);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(461, 111);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(707, 84);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 14;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1351, 44);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.TabStop = false;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1351, 73);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.TabStop = false;
		this.txtSupplierGLId.Visible = false;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(955, 117);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(70, 15);
		this.label51.TabIndex = 42;
		this.label51.Text = "Bill Amount";
		this.txtBillAmount.BackColor = System.Drawing.Color.White;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(1037, 111);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(229, 27);
		this.txtBillAmount.TabIndex = 22;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(5, 143);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 20;
		this.label24.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(369, 34);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "Payment Terms";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(461, 58);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(137, 23);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(369, 62);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 14;
		this.label10.Text = "Due Days";
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(461, 86);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(136, 23);
		this.duedate.TabIndex = 9;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(369, 117);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 18;
		this.label29.Text = "Delivery Term";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(369, 90);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(57, 15);
		this.label43.TabIndex = 16;
		this.label43.Text = "Due Date";
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(96, 56);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 3;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(707, 28);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(242, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.combcommAgent).TextChanged += new System.EventHandler(combcommAgent_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 62);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 6;
		this.label7.Text = "Supplier Name";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(598, 34);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 22;
		this.label20.Text = "Comm Agent";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(788, 90);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 30;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(598, 62);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 24;
		this.label11.Text = "Comm Type";
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(96, 113);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(264, 23);
		this.txtSupplierReference.TabIndex = 5;
		this.txtCommissionRemarks.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(707, 113);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(242, 23);
		this.txtCommissionRemarks.TabIndex = 16;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 90);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 8;
		this.label6.Text = "Manual Bill No";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(598, 117);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 32;
		this.label25.Text = "Comm Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 117);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(83, 15);
		this.label12.TabIndex = 10;
		this.label12.Text = "Sup Reference";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(788, 62);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 26;
		this.label21.Text = "Comm Rate";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(96, 86);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(264, 23);
		this.txtbillno.TabIndex = 4;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(859, 58);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(88, 23);
		this.txtcommrate.TabIndex = 13;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(598, 90);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(61, 15);
		this.label19.TabIndex = 28;
		this.label19.Text = "Rate UOM";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(859, 86);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(90, 23);
		this.txtcommamount.TabIndex = 15;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(169, 274);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(101, 279);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(232, 30);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(128, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(96, 30);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(73, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator5, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator3, this.btnSlip,
			this.btn225PartySlip, this.btnPrint, this.toolStripSeparator2, this.btnPurchaseOrderLoader, this.BtnSpecialRights, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1304, 27);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(85, 24);
		this.btnSlip.Text = "225-Slip";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btn225PartySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn225PartySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn225PartySlip.Name = "btn225PartySlip";
		this.btn225PartySlip.Size = new System.Drawing.Size(120, 24);
		this.btn225PartySlip.Text = "225-PartySlip";
		this.btn225PartySlip.Click += new System.EventHandler(btn225PartySlip_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "118-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPurchaseOrderLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPurchaseOrderLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseOrderLoader.Name = "btnPurchaseOrderLoader";
		this.btnPurchaseOrderLoader.Size = new System.Drawing.Size(98, 24);
		this.btnPurchaseOrderLoader.Text = "LoadOrder";
		this.btnPurchaseOrderLoader.Click += new System.EventHandler(btnPurchaseOrderLoader_Click);
		this.BtnSpecialRights.Image = Architecture.WinApp.Properties.Resources.RightsIcon;
		this.BtnSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSpecialRights.Name = "BtnSpecialRights";
		this.BtnSpecialRights.Size = new System.Drawing.Size(123, 24);
		this.BtnSpecialRights.Text = "Special Rights";
		this.BtnSpecialRights.Click += new System.EventHandler(BtnSpecialRights_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1310, 675);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 422);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1304, 250);
		this.panel9.TabIndex = 52;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 24);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1304, 226);
		this.panel10.TabIndex = 48;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1304, 226);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label58);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1304, 24);
		this.panel11.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1265, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(3, 2);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(355, 19);
		this.label58.TabIndex = 0;
		this.label58.Text = "Detail Information Of Above Selected Row";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 130);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1304, 292);
		this.panel7.TabIndex = 51;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1304, 292);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel18.Controls.Add(this.groupBox3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 55);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1304, 75);
		this.panel18.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox3.Controls.Add(this.label64);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1181, 69);
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
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 37);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 146;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(6, 21);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(79, 15);
		this.label64.TabIndex = 145;
		this.label64.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1052, 41);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1052, 21);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(971, 21);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(971, 41);
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
		this.btnshow.Location = new System.Drawing.Point(896, 37);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(69, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(653, 18);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(86, 15);
		this.label59.TabIndex = 142;
		this.label59.Text = "Supplier Name";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(552, 18);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(450, 18);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(332, 18);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(211, 18);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance243).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance254;
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
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(653, 37);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(552, 39);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(450, 39);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(335, 39);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(113, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(214, 39);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(116, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label57);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 32);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1304, 23);
		this.panel6.TabIndex = 46;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1263, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(3, 2);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(274, 19);
		this.label57.TabIndex = 0;
		this.label57.Text = "PURCHASE INVOICE HISTORY    ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1304, 29);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1304, 27);
		this.toolStrip2.TabIndex = 28;
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
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
		this.notifyIcon1.Text = "notifyIcon1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1318, 704);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "InvfrmPurchasedirectInvoice";
		this.Text = "Eccount Book ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmPurchasedirectInvoice_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.tabFreight.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabOtherExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabSupplierAddLess.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabEmptyBags.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
