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
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.Steel.Purchase
{

public class PurchaseInvoiceDirect_St : Form
{
	private Rightsobjects formRights = new Rightsobjects();

	private int[] AryColWidth;

	private List<InvPurchaseInvoiceDetail> lstRemoveRecord = new List<InvPurchaseInvoiceDetail>();

	private Attachment AT = new Attachment();

	private bool Approved;

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool IsDetailHasPurchaseOrderId;

	private int VoucherHeadId;

	private int RecId;

	public static int SupplierId;

	private int TaxNameId;

	private int updateDetailIndex;

	private double SupplierCommissionAmount;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtSchedule = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	public string GRNS = string.Empty;

	public bool GRNSelected;

	private UserAccount UserAccount = new UserAccount();

	private LoadPurchaseOrder_St LoadPO;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnLoadPo;

	private Panel panel5;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtCommRate;

	private Label label19;

	private TextBox txtCommAmount;

	private Label label7;

	private Label label2;

	private Label label8;

	private Label label6;

	private TextBox txtManualBillNo;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtDocNo;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Timer timer1;

	internal GridEX grd;

	private Panel panel11;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grdSchedule;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbstockprty;

	internal UltraCombo CmbTaxAccount;

	internal UltraCombo CmbSupplier;

	internal UltraCombo CmbCommAgent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private CheckBox ChkBok;

	private Panel panel15;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnFrmRefresh;

	internal UltraCombo CmbBranch;

	internal UltraCombo CmbProject;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbPaymentTerm;

	private Label label10;

	internal UltraCombo CmbDeliveryTerm;

	private Label label30;

	private RichTextBox txtCommRemarks;

	internal UltraCombo cmbCommType;

	internal UltraCombo cmbCommUom;

	private DateTimePicker GpDate;

	private TextBox txtVehicleNo;

	private Label label12;

	internal UltraCombo CmbRateUom;

	private Label label43;

	private Label label48;

	internal UltraCombo CmbWareHouse;

	private TextBox txtTaxAmount;

	private TextBox txtTotalAmount;

	private TextBox txtTaxPercent;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbPackUom;

	private Label label49;

	private Label label34;

	internal UltraCombo CmbItem;

	private Button btnplus;

	private TextBox txtGpNo;

	private Label label47;

	private TextBox txtItemRate;

	private TextBox txtItemAmount;

	private TextBox txtItemQty;

	private Label label40;

	private Label label44;

	private Label label14;

	private Label label41;

	private Label label38;

	private Label label36;

	private Label label37;

	private Label label15;

	private RadioButton btnWithPO;

	private RadioButton btnWithoutPO;

	private Label label16;

	private Panel panel12;

	private Panel panel8;

	private Panel panel3;

	private Panel panel4;

	private RichTextBox txtRemarks;

	private Panel panel17;

	private Label label1;

	private Panel panel18;

	private Label label23;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker DueDate;

	private TextBox txtExchangeRate;

	private Label label57;

	internal UltraCombo cmbCurrency;

	private Label label56;

	private TextBox txtTotalQty;

	private Label label60;

	private TextBox txtTotalWeight;

	private TextBox txtFcyAmount;

	private Label label58;

	private Label label5;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryAc;

	private Label label24;

	private Label label29;

	private Label label31;

	private Label label32;

	private TextBox txtBrokeryRate;

	private Label label33;

	private TextBox txtBrokeryAmount;

	private RichTextBox txtBrokeryRemarks;

	private Label label35;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabFreight;

	private TabPage tabOtherItemExpense;

	private TabPage tabPartyAddLess;

	internal UltraCombo CmbPackingType;

	private Label label50;

	private TextBox txtGrossWeight;

	private Label label39;

	private Label label42;

	private Label label45;

	private TextBox txtWeighttCutTotal;

	private TextBox txtWeighttCut;

	private Label label46;

	private Label label51;

	private TextBox txtNetWeight;

	private Label label52;

	private TextBox txtStockWeight;

	private TextBox txtAddLessWeight;

	private TextBox txtRateCutAmount;

	private Label label55;

	private TextBox txtRateCut;

	private Label label54;

	internal UltraCombo CmbCity;

	private Label label53;

	private Label label59;

	private Label label61;

	private Label label27;

	internal GridEX grdFreight;

	private Label label28;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnSlipDetail1510;

	private ToolStripButton btnSlip1510A;

	private CheckBox chkVoucherPreview;

	private TextBox txtRemarksDetail;

	private Label label62;

	private Panel panel7;

	private GroupBox groupBox4;

	private Button btnShow;

	internal UltraCombo CmbDeliveryTermHistory;

	private Label label67;

	internal UltraCombo CmbPaymentTermHistory;

	private Label label63;

	internal UltraCombo CmbCustomerHistory;

	private Label label64;

	private TextBox ToDocNoHistory;

	private Label label65;

	private TextBox FromDocNoHistory;

	private Label label66;

	private DateTimePicker ToDateHistory;

	private Label label68;

	private DateTimePicker FromDateHistory;

	private Label label69;

	private Panel panel10;

	private Panel panel9;

	private Panel panel13;

	private ToolStripButton btnResetHistory;

	private ToolStripButton BtnRefreshHistory;

	public PurchaseInvoiceDirect_St(UserAccount user)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	public void grdInvExpSettings()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			List<InventoryItemsOther> lst = new List<InventoryItemsOther>();
			DataTable dt = InventoryItemsOther.GetAll(new InventoryItemsOther
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					InventoryItemsOther items = new InventoryItemsOther();
					items.Id = Conversion.ToInt(dt.Rows[i]["Id"]);
					items.OtherItemName = dt.Rows[i]["OtherItemName"].ToString();
					lst.Add(items);
				}
			}
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)lst, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
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
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
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
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["TransporterId"].Caption = "AccountTitle";
			grdFreight.RootTable.Columns["Percentage"].Caption = "%";
			grdFreight.RootTable.Columns["TransporterId"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["TransporterId"].HasValueList = true;
			grdFreight.RootTable.Columns["TransporterId"].LimitToList = true;
			grdFreight.RootTable.Columns["TransporterId"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["TransporterId"].Caption = "Charge To Product";
			grdFreight.RootTable.Columns["TransporterId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["TransporterId"].Width = 150;
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
			grdFreight.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void gridGLSettings()
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["ItemUomId"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WtCut"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			if (IsDetailHasPurchaseOrderId)
			{
				grd.RootTable.Columns["OrderNo"].Visible = true;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
				grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Position = 0;
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.RootTable.Columns["OrderNo"].Visible = false;
				grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, "");
		grdFreight.DataSource = dtFreight;
	}

	public void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	public void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	public void AddRowInScheduleGrid()
	{
		try
		{
			dtSchedule.Rows.Add("", "", 0, 0);
			grdSchedule.DataSource = dtSchedule;
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
			if (e.Column.Key == "AccountId" && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
			{
				MessageBox.Show("Please Select an other Account");
				item.Cells["AccountId"].Value = 0;
			}
			else
			{
				grdGLedger.UpdateData();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "ItemQty" && item != null && (int)item.RowType == 0)
			{
				string obj = item.Cells["ItemQty"].Value.ToString();
				string RateUOM = item.Cells["RateUOM"].Value.ToString();
				string ItemRate = item.Cells["ItemRate"].Text.ToString();
				double ItemAmount = Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)RateUOM) * Conversion.ToDouble((object)ItemRate);
				item.Cells["ItemAmount"].Value = ItemAmount;
			}
			grd.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSchedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdSchedule.UpdateData();
			GridEXRow item = grdSchedule.CurrentRow;
			if (e.Column.Key == "DueDays")
			{
				item.Cells["DueDate"].Value = DocDate.Value.AddDays(Conversion.ToInt((object)item.Cells["DueDays"].Text.ToString()));
			}
			if (e.Column.Key == "Due%")
			{
				if (Conversion.ToInt(item.Cells["Due%"].Value) > 100)
				{
					MessageBox.Show("Percent should be less or equal then 100");
					return;
				}
				double percent = double.Parse(grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString()) * (double.Parse(item.Cells["Due%"].Value.ToString()) / 100.0);
				item.Cells["Amount"].Value = Math.Round(percent, 2);
			}
			grdSchedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterDebit = 0.0;
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
				if (Conversion.ToInt(r.Cells["TransporterId"].Value) > 0 && txtSupplierGLId.Text.ToString() == r.Cells["TransporterId"].Value.ToString())
				{
					TransporterDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
					TransporterCredit += Conversion.ToDouble((object)r.Cells["Freight"].Value.ToString());
				}
			}
			BillAmount = ItemAmount + ExpAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount = BillAmount + TransporterCredit - TransporterDebit;
			if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCommAgent.Value))
			{
				if (txtCommRate.Text != string.Empty)
				{
					BillAmount += Conversion.ToDouble((object)txtCommAmount.Text.Trim());
					SupplierCommissionAmount = Conversion.ToDouble((object)txtCommAmount.Text.Trim());
				}
				else
				{
					txtCommAmount.Text = "0";
				}
			}
			else
			{
				SupplierCommissionAmount = 0.0;
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
			txtBillAmount.Text = BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalQtyWeightAndFcyAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2));
				decimal TotalFcyAmount = ((Conversion.ToDecimal((object)txtBillAmount.Text) > 0m && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? (Conversion.ToDecimal((object)txtBillAmount.Text) / Conversion.ToDecimal((object)txtExchangeRate.Text)) : 0m);
				txtTotalQty.Text = TotalQty.ToString("#,##0.##");
				txtTotalWeight.Text = TotalWeight.ToString("#,##0.##");
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtTotalQty.Text = "0";
				txtTotalWeight.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbBranch).ActiveRow == null)
		{
			MessageBox.Show("Branch Name is Required");
			CmbBranch.Focus();
			return false;
		}
		if (((UltraGridBase)CmbProject).ActiveRow == null)
		{
			MessageBox.Show("Project Name is Required");
			CmbProject.Focus();
			return false;
		}
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == "0" || txtDocNo.Text.Trim() == null)
		{
			MessageBox.Show("Doc No is Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Name is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (txtRemarks.Text == null || txtRemarks.Text == string.Empty || txtRemarks.Text == "0")
		{
			MessageBox.Show("Invoice Remarks Field is Required");
			txtRemarks.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCurrency).ActiveRow == null)
		{
			MessageBox.Show("Currency Field is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (txtExchangeRate.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m || txtExchangeRate.Text.Trim() == null)
		{
			MessageBox.Show("Exchange Rate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0" || txtFcyAmount.Text.Trim() == null)
		{
			MessageBox.Show("Fcy Amount Field is Required");
			txtFcyAmount.Focus();
			return false;
		}
		if (txtBillAmount.Text.Trim() == string.Empty || txtBillAmount.Text.Trim() == "0" || txtBillAmount.Text.Trim() == null)
		{
			MessageBox.Show("Bill Amount Field is Required");
			txtBillAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("JobLot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtItemQty.Text.Trim() == string.Empty || txtItemQty.Text.Trim() == "0" || txtItemQty.Text.Trim() == null)
		{
			MessageBox.Show("Qty is Required");
			txtItemQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || txtGrossWeight.Text.Trim() == "0" || txtGrossWeight.Text.Trim() == null)
		{
			MessageBox.Show("Gross Weight is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == string.Empty || txtNetWeight.Text.Trim() == "0" || txtNetWeight.Text.Trim() == null)
		{
			MessageBox.Show("Net Weight is Required");
			txtNetWeight.Focus();
			return false;
		}
		if (txtStockWeight.Text.Trim() == string.Empty || txtStockWeight.Text.Trim() == "0" || txtStockWeight.Text.Trim() == null)
		{
			MessageBox.Show("Stock Weight is Required");
			txtStockWeight.Focus();
			return false;
		}
		if (txtItemRate.Text.Trim() == string.Empty || txtItemRate.Text.Trim() == "0" || txtItemRate.Text.Trim() == null)
		{
			MessageBox.Show("Item Rate is Required");
			txtItemRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null)
		{
			MessageBox.Show("RateUom is Required");
			CmbRateUom.Focus();
			return false;
		}
		if (txtItemAmount.Text.Trim() == string.Empty || txtItemAmount.Text.Trim() == "0" || txtItemAmount.Text.Trim() == null)
		{
			MessageBox.Show("Item Amount is Required");
			txtItemAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null)
		{
			MessageBox.Show("WareHouse Name is Required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null)
		{
			MessageBox.Show("City Name is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private void BranchFill()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, CmbBranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)CmbBranch).Rows[0].Activate();
				((UltraGridBase)CmbBranch).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbProject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)CmbProject).Rows[0].Activate();
			((UltraGridBase)CmbProject).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtSupplier, CmbCommAgent, "Id", "CompanyName", "Commission Agent Name", ZeroIndex: false);
				((UltraGridBase)CmbCommAgent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtSupplier, CmbBrokeryAc, "Id", "CompanyName", "Broker Name", ZeroIndex: false);
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, CmbPaymentTerm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)CmbPaymentTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbPaymentTerm).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", false);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CommissionAndBrokeryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "By Weight");
			DDL.BindDDL(dt, cmbCommType, "Id", "CommType", "Comm Type", ZeroIndex: false);
			((UltraGridBase)cmbCommType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dt, CmbBrokeryType, "Id", "CommType", "Comm Type", ZeroIndex: false);
			((UltraGridBase)CmbBrokeryType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "5");
			dt.Rows.Add(2, "25");
			dt.Rows.Add(3, "40");
			dt.Rows.Add(4, "50");
			dt.Rows.Add(5, "60");
			dt.Rows.Add(6, "80");
			dt.Rows.Add(7, "100");
			DDL.BindDDL(dt, cmbCommUom, "Id", "UOM", "Comm UOM", ZeroIndex: false);
			((UltraGridBase)cmbCommUom).Rows[2].Activate();
			((UltraGridBase)cmbCommUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dt, CmbBrokeryRateUom, "Id", "UOM", "Brokery UOM", ZeroIndex: false);
			((UltraGridBase)CmbBrokeryRateUom).Rows[2].Activate();
			((UltraGridBase)CmbBrokeryRateUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountsFill()
	{
		try
		{
			dtAccountlst.Rows.Clear();
			dtAccountlst = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,15,20,21");
			if (dtAccountlst.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccountlst, CmbTaxAccount, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvPurchaseInvoice.GenerateInvPurchaseInvoiceCode(new InvPurchaseInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1503
			});
			if (Code > 0)
			{
				txtDocNo.Text = Code.ToString();
				return;
			}
			throw new Exception("Max Number Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable dtitem = CommonServices.AllItemsBindForSteel();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			string Rateuom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUom).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void joblotfill()
	{
		try
		{
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLot", false);
				((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			DataTable dtpkt = CommonServices.InvPackingTypeGetAllService();
			if (dtpkt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpkt, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", false);
				((UltraGridBase)CmbPackingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCity, "Id", "CityName", "CityName", false);
				((UltraGridBase)CmbCity).DisplayLayout.Bands[0].Columns[1].Width = 150;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable lst = CommonServices.getActiveWareHouse();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbWareHouse, "Id", "WareHouseName", "WareHouse Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TaxScheduleGet()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemTaxSchedule.GetTaxScheduleForItemId(new ItemTaxSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(CmbItem.Value),
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtTaxPercent.Text = dt.Rows[0]["TaxPercent"].ToString();
				TaxNameId = Conversion.ToInt(dt.Rows[0]["TaxNameId"]);
			}
			else
			{
				txtTaxPercent.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		bindRateUomAndItemPackUom();
		TotalCommissionAmount();
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
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

	private void cmbcommuom_Leave(object sender, EventArgs e)
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

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1503);
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
			GenerateReportVoucher103();
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 1503);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void defaultConfiquration()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstwarehouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstwarehouse.Count > 0)
			{
				int warehouse = Conversion.ToInt((object)lstwarehouse[0].ConfigKey.ToString());
				CmbWareHouse.Value = warehouse;
			}
			List<ConfigrationsAllocation> lstCity = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (lstCity.Count > 0)
			{
				int CityId = Conversion.ToInt((object)lstCity[0].ConfigKey.ToString());
				CmbCity.Value = CityId;
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
				txtExchangeRate.Text = BaseCurrencyRate.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 850);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			btnPrint.Enabled = formRights.DoHavePrintRights;
			btnDelete.Visible = formRights.DoHaveCanDelete;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formRights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formRights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formRights.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
			AccountsFill();
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("OrderId");
			dtGrid.Columns.Add("OrderDetailId");
			dtGrid.Columns.Add("OrderNo");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("ItemUomId");
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("PackEquivalent");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("WtCut", typeof(double));
			dtGrid.Columns.Add("WtCutTotal", typeof(double));
			dtGrid.Columns.Add("AdLsWeight", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUomId");
			dtGrid.Columns.Add("RateUom");
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("Freight", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("ExchangeRate", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("WarehouseId");
			dtGrid.Columns.Add("WarehouseName");
			dtGrid.Columns.Add("CityId");
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("GpDate");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("Remarks");
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtFreight.Columns.Add("TransporterId");
			dtFreight.Columns.Add("Percentage");
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
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
			BranchFill();
			ProjectFill();
			DocumentNo();
			SupplierNameFilll();
			PaymentTerms();
			DeliveryTerm();
			CurrencyFill();
			CommissionAndBrokeryTypeFill();
			CommissionUOMFill();
			ItemDetailFill();
			joblotfill();
			PackingTypeFill();
			WareHouseFill();
			CityFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBok.Checked = true;
			defaultConfiquration();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllData = InvPurchaseInvoice.AllComboAgainstPurchaseInvoiceForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllData.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsup = new DataTable();
			dtsup.Columns.Add("Id");
			dtsup.Columns.Add("Name");
			DataTable dtPayTerm = new DataTable();
			dtPayTerm.Columns.Add("Id");
			dtPayTerm.Columns.Add("Name");
			DataTable dtDelTerm = new DataTable();
			dtDelTerm.Columns.Add("Id");
			dtDelTerm.Columns.Add("Name");
			for (int i = 0; i < dtAllData.Rows.Count; i++)
			{
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "Supplier")
				{
					dtsup.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				else if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "PaymentTerm")
				{
					dtPayTerm.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				else if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "DeliveryTerm")
				{
					dtDelTerm.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
			}
			DDL.BindDDL(dtsup, CmbCustomerHistory, "Id", "Name", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtPayTerm, CmbPaymentTermHistory, "Id", "Name", "Payment Term", ZeroIndex: false);
			DDL.BindDDL(dtDelTerm, CmbDeliveryTermHistory, "Id", "Name", "Delivery Term", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadDataDetailGridAgainstOrder()
	{
		try
		{
			DataTable Po = LoadPO.dtOrder;
			if (Po.Rows.Count <= 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0 && Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) != Conversion.ToInt(Po.Rows[0]["OrderId"]))
			{
				throw new Exception("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
			}
			((Control)(object)CmbSupplier).Enabled = false;
			CmbSupplier.Value = Conversion.ToInt(Po.Rows[0]["SupplierCustomerId"]);
			CmbPaymentTerm.Value = Conversion.ToInt(Po.Rows[0]["PaymentTermsId"]);
			txtDueDays.Text = Conversion.ToString(Po.Rows[0]["OrderDueDays"]);
			((Control)(object)CmbDeliveryTerm).Text = Po.Rows[0]["DeliveryTerm"].ToString();
			cmbCurrency.Value = Conversion.ToInt(Po.Rows[0]["CurrencyId"]);
			CmbCommAgent.Value = Po.Rows[0]["CommAgentId"];
			((Control)(object)cmbCommType).Text = string.Empty;
			((Control)(object)cmbCommType).Text = Po.Rows[0]["CommissionType"].ToString();
			txtCommRate.Text = Conversion.ToDecimal(Po.Rows[0]["CommRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)cmbCommUom).Text = string.Empty;
			((Control)(object)cmbCommUom).Text = Po.Rows[0]["CommRateUom"].ToString();
			txtCommAmount.Text = Conversion.ToDecimal(Po.Rows[0]["CommAmount"]).ToString(clsGlobalVariables.stringFormatsingle);
			txtCommRemarks.Text = Po.Rows[0]["CommissionRemarks"].ToString();
			CmbBrokeryAc.Value = Po.Rows[0]["BrokerAgentId"];
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = Po.Rows[0]["BrokeryType"].ToString();
			txtBrokeryRate.Text = Conversion.ToDecimal(Po.Rows[0]["BrokeryRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = Po.Rows[0]["brokeryRateUom"].ToString();
			txtBrokeryAmount.Text = Conversion.ToDecimal(Po.Rows[0]["BrokeryAmount"]).ToString(clsGlobalVariables.stringFormatsingle);
			txtBrokeryRemarks.Text = Po.Rows[0]["BrokeryRemarks"].ToString();
			for (int i = 0; i < Po.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["OrderDetailId"].Value) == Conversion.ToInt(Po.Rows[i]["OrderDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, Po.Rows[i]["OrderId"], Po.Rows[i]["OrderDetailId"], Po.Rows[i]["DocNo"], Po.Rows[i]["OrderItemId"], Po.Rows[i]["ItemName"], Po.Rows[i]["JobLotId"], Po.Rows[i]["JobLotDescription"], Po.Rows[i]["PackingTypeId"], Po.Rows[i]["PackTypeDesc"], Po.Rows[i]["OrderItemUOMId"], Po.Rows[i]["PackUom"], Po.Rows[i]["PUomEquivalent"], Po.Rows[i]["BalQty"], Po.Rows[i]["BalWeight"], 0, 0, 0, Po.Rows[i]["BalWeight"], Po.Rows[i]["BalWeight"], Po.Rows[i]["OrderItemRate"], Po.Rows[i]["OrderItemRateUOMId"], Po.Rows[i]["RateUom"], Po.Rows[i]["RUomEquivalent"], 0, 0, Po.Rows[i]["ItemAmount"], 0, 0, 0, Po.Rows[i]["ItemAmount"], 0, 0, 0, "", Po.Rows[i]["CityId"], Po.Rows[i]["CityName"], 0, DateTime.Now.ToShortDateString(), 0);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			txtExchangeRate.Text = Conversion.ToDecimal(Po.Rows[0]["ExchangeRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
			IsDetailHasPurchaseOrderId = true;
			TotalCommissionAmount();
			TotalBrokeryAmount();
			ExpProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
				obj.Cells["Expense"].Value = (TotalAmount / NetWeight * Weight).ToString();
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble(r.Cells["ItemAmount"].Value) + Conversion.ToDouble(r.Cells["Freight"].Value) + Conversion.ToDouble(r.Cells["Expense"].Value) + Conversion.ToDouble(r.Cells["Commission"].Value);
				r.EndEdit();
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void CommissionProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtCommAmount.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
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

	private void GetAll(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1503;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
			obj.NoOfRecords = NoOfRecords;
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
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.PaymenetTermId = Conversion.ToInt(CmbPaymentTermHistory.Value);
			obj.DeliveryTerm = ((Control)(object)CmbDeliveryTermHistory).Text;
			DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("PaymentTerm");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("CommAgent");
				dt.Columns.Add("CommType");
				dt.Columns.Add("CommRate", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CommRemarks");
				dt.Columns.Add("BrokerName");
				dt.Columns.Add("BrokeryType");
				dt.Columns.Add("BrokeryRate", typeof(double));
				dt.Columns.Add("BrokeryAmount", typeof(double));
				dt.Columns.Add("BrokeryRemarks");
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("TotalWeight", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("CurrencyName");
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["TermsDescription"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], dthistory.Rows[i]["CommAmount"], dthistory.Rows[i]["CommissionRemarks"], dthistory.Rows[i]["BrokerName"], dthistory.Rows[i]["BrokeryType"], dthistory.Rows[i]["BrokeryRate"], dthistory.Rows[i]["BrokeryAmount"], dthistory.Rows[i]["BrokeryRemarks"], dthistory.Rows[i]["TotalQty"], dthistory.Rows[i]["TotalWeight"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["CurrencyName"], dthistory.Rows[i]["ExchangeRate"], dthistory.Rows[i]["FcyAmount"], Conversion.ToBool(dthistory.Rows[i]["IsApproved"]) ? "Approved" : "Not Approved", dthistory.Rows[i]["UserName"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dt;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalWeight"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Item Slip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Item Slip";
			grdHistory.RootTable.Columns["View"].Width = 65;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Width = 65;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Voucher"].Position = 0;
			grdHistory.RootTable.Columns["PartySlip"].Position = 1;
			grdHistory.RootTable.Columns["View"].Position = 2;
			grdHistory.RootTable.Columns["Edit"].Position = 3;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			Approved = false;
			TaxNameId = 0;
			SupplierCommissionAmount = 0.0;
			dtGrid.Rows.Clear();
			dtFreight.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtSchedule.Rows.Clear();
			DocDate.Focus();
			((Control)(object)CmbCommAgent).Text = string.Empty;
			((Control)(object)cmbCommType).Text = string.Empty;
			txtCommRate.Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			((Control)(object)cmbCommUom).Text = string.Empty;
			txtCommRemarks.Text = string.Empty;
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			txtBrokeryRemarks.Text = string.Empty;
			txtManualBillNo.Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			txtDueDays.Clear();
			DueDate.Value = DateTime.Now;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			((Control)(object)cmbCurrency).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtTotalQty.Text = string.Empty;
			txtTotalWeight.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			DocDate.Value = DateTime.Now;
			grd.DataSource = dtGrid;
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			CmbSupplier.Focus();
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbItem).Enabled = true;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtItemQty.Clear();
			txtGrossWeight.Clear();
			txtWeighttCut.Clear();
			txtWeighttCutTotal.Clear();
			txtAddLessWeight.Clear();
			txtNetWeight.Clear();
			txtStockWeight.Clear();
			txtItemRate.Clear();
			txtItemRate.Enabled = true;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtRateCut.Clear();
			txtRateCutAmount.Clear();
			txtItemAmount.Clear();
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			txtGpNo.Clear();
			txtVehicleNo.Clear();
			GpDate.Value = DateTime.Now;
			IsDetailHasPurchaseOrderId = false;
			((Control)(object)CmbSupplier).Enabled = true;
			defaultConfiquration();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetDetail()
	{
		try
		{
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbItem).Enabled = true;
			((Control)(object)CmbPackingType).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtItemQty.Clear();
			txtGrossWeight.Clear();
			txtWeighttCut.Clear();
			txtWeighttCutTotal.Clear();
			txtAddLessWeight.Clear();
			txtNetWeight.Clear();
			txtStockWeight.Clear();
			txtItemRate.Clear();
			txtItemRate.Enabled = true;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtRateCut.Clear();
			txtRateCutAmount.Clear();
			txtItemAmount.Clear();
			txtGpNo.Clear();
			txtVehicleNo.Clear();
			GpDate.Value = DateTime.Now;
			CmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && IsDetailHasPurchaseOrderId)
			{
				ResetDetail();
				throw new Exception("You Can't Add A Direct Entry Because Record Against Purchase Order Already Present");
			}
			if (FormValidationDetail())
			{
				dtGrid.Rows.Add(0, 0, 0, 0, CmbItem.Value, ((Control)(object)CmbItem).Text, CmbJobLot.Value, ((Control)(object)CmbJobLot).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, CmbPackUom.Value, ((Control)(object)CmbPackUom).Text, ((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value, Conversion.ToDecimal((object)txtItemQty.Text), Conversion.ToDecimal((object)txtGrossWeight.Text), Conversion.ToDecimal((object)txtWeighttCut.Text), Conversion.ToDecimal((object)txtWeighttCutTotal.Text), Conversion.ToDecimal((object)txtAddLessWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToDecimal((object)txtStockWeight.Text), Conversion.ToDecimal((object)txtItemRate.Text), CmbRateUom.Value, ((Control)(object)CmbRateUom).Text, ((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value, Conversion.ToDecimal((object)txtRateCut.Text), Conversion.ToDecimal((object)txtRateCutAmount.Text), Conversion.ToDecimal((object)txtItemAmount.Text), 0, 0, 0, 0, (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Conversion.ToDecimal((object)txtExchangeRate.Text) : 0m, (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Math.Round(Conversion.ToDecimal((object)txtItemAmount.Text) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount) : 0m, CmbWareHouse.Value, ((Control)(object)CmbWareHouse).Text, CmbCity.Value, ((Control)(object)CmbCity).Text, txtGpNo.Text, GpDate.Value.ToShortDateString(), txtVehicleNo.Text.Trim(), txtRemarksDetail.Text.Trim());
				IsDetailHasPurchaseOrderId = false;
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				CalculateTotalQtyWeightAndFcyAmount();
				TotalBrokeryAmount();
				TotalCommissionAmount();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Expected O, but got Unknown
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_109e: Expected O, but got Unknown
		//IL_11cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e6: Expected O, but got Unknown
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
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (!(Conversion.ToDouble(r4.Cells["Freight"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r4.Cells["TransporterId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First in Transporter Grid");
				}
				if (Conversion.ToDouble(r4.Cells["Freight"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception("Transport Grid Remarks required Please Check");
					}
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if ((Conversion.ToDouble(r5.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r5.Cells["Debit"].Value) > 0) && Conversion.ToInt(r5.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			if (Conversion.ToDouble((object)txtCommAmount.Text) > 0.0 && Conversion.ToInt(CmbCommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				return;
			}
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(CmbCommAgent.Value);
			purchaseInvoice.CommissionType = ((Control)(object)cmbCommType).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDecimal((object)txtCommRate.Text);
			purchaseInvoice.CommRateUom = ((Control)(object)CmbRateUom).Text;
			purchaseInvoice.CommAmount = Conversion.ToDecimal((object)txtCommAmount.Text);
			if (txtCommRemarks.Text.Trim() == string.Empty && purchaseInvoice.CommAmount > 0m)
			{
				purchaseInvoice.CommissionRemarks = "Rate:  " + purchaseInvoice.CommRate + " Type: " + purchaseInvoice.CommissionType + " Rate Uom: " + purchaseInvoice.CommRateUom + " Amount: " + purchaseInvoice.CommAmount;
			}
			else
			{
				purchaseInvoice.CommissionRemarks = txtCommRemarks.Text.Trim();
			}
			if (Conversion.ToDecimal((object)txtBrokeryAmount.Text) > 0m && Conversion.ToInt(CmbBrokeryAc.Value) == 0)
			{
				throw new Exception("Please Select Broker Account First");
			}
			purchaseInvoice.BrokerAgentId = Conversion.ToInt(CmbBrokeryAc.Value);
			purchaseInvoice.BrokeryType = ((Control)(object)CmbBrokeryType).Text.Trim();
			purchaseInvoice.BrokeryRate = Conversion.ToDecimal((object)txtBrokeryRate.Text);
			purchaseInvoice.BrokeryRateUom = Conversion.ToDecimal((object)((Control)(object)CmbBrokeryRateUom).Text);
			purchaseInvoice.BrokeryAmount = Conversion.ToDecimal((object)txtBrokeryAmount.Text);
			if (txtBrokeryRemarks.Text == string.Empty && purchaseInvoice.BrokeryAmount > 0m)
			{
				purchaseInvoice.BrokeryRemarks = "BrokeryRate: " + purchaseInvoice.BrokeryRate + " BrokeryType: " + purchaseInvoice.BrokeryType + " RateUom: " + purchaseInvoice.BrokeryRateUom + " Amount: " + purchaseInvoice.BrokeryAmount;
			}
			else
			{
				purchaseInvoice.BrokeryRemarks = txtBrokeryRemarks.Text.Trim();
			}
			BillAmount();
			CommissionProportion();
			FreightProportion();
			ExpProportion();
			TotalCommissionAmount();
			TotalBrokeryAmount();
			purchaseInvoice.BranchesId = Conversion.ToInt(CmbBranch.Value);
			purchaseInvoice.ProjectsId = Conversion.ToInt(CmbProject.Value);
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocumentTypeId = 1503;
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			purchaseInvoice.ManualBillNo = txtManualBillNo.Text.Trim();
			purchaseInvoice.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			purchaseInvoice.DueDate = DueDate.Value;
			purchaseInvoice.DeliveryTerm = ((Control)(object)CmbDeliveryTerm).Text;
			purchaseInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			purchaseInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			purchaseInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			purchaseInvoice.BillAmount = Conversion.ToDecimal((object)txtBillAmount.Text);
			purchaseInvoice.TotalQty = Conversion.ToDecimal((object)txtTotalQty.Text);
			purchaseInvoice.TotalWeight = Conversion.ToDecimal((object)txtTotalWeight.Text);
			purchaseInvoice.RemarksHeader = txtRemarks.Text.Trim();
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.EntryUserId = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ModifyUserId = UserAccount.ID;
			purchaseInvoice.ApprovedDate = DateTime.Now;
			purchaseInvoice.ApprovedUserId = UserAccount.ID;
			purchaseInvoice.DeliverystartDate = DateTime.Now;
			purchaseInvoice.ExpiryDate = DateTime.Now;
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(lstRemoveRecord[i]);
				}
			}
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r7 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r7.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r7.Cells["OrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(r7.Cells["OrderDetailId"].Value);
					if (Conversion.ToInt(r7.Cells["ItemId"].Value) != 0)
					{
						Pd.ItemId = Conversion.ToInt(r7.Cells["ItemId"].Value);
						Pd.JobLotId = Conversion.ToInt(r7.Cells["JobLotId"].Value);
						Pd.JobLotDescription = Conversion.ToString((object)r7.Cells["JobLot"].Text);
						if (Pd.JobLotId == 0)
						{
							MessageBox.Show("JobLot Required In Row#" + (r7.RowIndex + 1));
							return;
						}
						Pd.PackingTypeId = Conversion.ToInt(r7.Cells["PackingTypeId"].Value);
						if (Pd.PackingTypeId == 0)
						{
							MessageBox.Show("Packing Type Required In Row#" + (r7.RowIndex + 1));
							return;
						}
						if (Conversion.ToInt(r7.Cells["ItemUomId"].Value) != 0)
						{
							Pd.ItemUOMId = Conversion.ToInt(r7.Cells["ItemUomId"].Value);
							Pd.UOMCode = Conversion.ToString((object)r7.Cells["PackUOM"].Text);
							if (Conversion.ToDecimal(r7.Cells["ItemQty"].Value) != 0m)
							{
								Pd.ItemQty = Conversion.ToDecimal(r7.Cells["ItemQty"].Value);
								if (Conversion.ToDecimal(r7.Cells["GrossWeight"].Value) != 0m)
								{
									Pd.GrossWeight = Conversion.ToDecimal(r7.Cells["GrossWeight"].Value);
									Pd.WCut = Conversion.ToDecimal(r7.Cells["WtCut"].Value);
									Pd.WcutTotal = Conversion.ToDecimal(r7.Cells["WtCutTotal"].Value);
									Pd.AdLsWeight = Conversion.ToDecimal(r7.Cells["AdLsWeight"].Value);
									if (Conversion.ToDecimal(r7.Cells["NetBillWeight"].Value) != 0m)
									{
										Pd.NetBillWeight = Conversion.ToDecimal(r7.Cells["NetBillWeight"].Value);
										if (Conversion.ToDecimal(r7.Cells["StockWeight"].Value) != 0m)
										{
											Pd.NetStockWeight = Conversion.ToDecimal(r7.Cells["StockWeight"].Value);
											if (Conversion.ToDecimal(r7.Cells["ItemRate"].Value) != 0m)
											{
												Pd.ItemRate = Conversion.ToDecimal(r7.Cells["ItemRate"].Value);
												if (Conversion.ToInt(r7.Cells["RateUomId"].Value) != 0)
												{
													Pd.UomScheduleIdRate = Conversion.ToInt(r7.Cells["RateUomId"].Value);
													Pd.RateCut = Conversion.ToDecimal(r7.Cells["RateCut"].Value);
													Pd.RateCutAmount = Conversion.ToDecimal(r7.Cells["RateCutAmount"].Value);
													if (Conversion.ToDecimal(r7.Cells["ItemAmount"].Value) != 0m)
													{
														Pd.ItemAmount = Conversion.ToDecimal(r7.Cells["ItemAmount"].Value);
														if (Conversion.ToInt(r7.Cells["WarehouseId"].Value) != 0)
														{
															Pd.WarehouseId = Conversion.ToInt(r7.Cells["WarehouseId"].Value);
															if (Conversion.ToInt(r7.Cells["CityId"].Value) != 0)
															{
																Pd.CityId = Conversion.ToInt(r7.Cells["CityId"].Value);
																Pd.ItemCgsRate = 0m;
																Pd.GpNo = Conversion.ToInt(r7.Cells["GpNo"].Value);
																Pd.GpDate = Conversion.ToDateTime(r7.Cells["GpDate"].Value);
																Pd.VehicleNo = Conversion.ToString(r7.Cells["VehicleNo"].Value);
																Pd.RemarksDetail = Conversion.ToString(r7.Cells["Remarks"].Value);
																if (Conversion.ToSingle(r7.Cells["BillAmount"].Value) != 0f)
																{
																	Pd.ItemNetAmount = Conversion.ToDecimal(r7.Cells["BillAmount"].Value);
																	Pd.FreightAmount = Conversion.ToDecimal(r7.Cells["Freight"].Value);
																	Pd.ExpenseAmount = Conversion.ToDecimal(r7.Cells["Expense"].Value);
																	Pd.CommissionAmount = Conversion.ToDecimal(r7.Cells["Commission"].Value);
																	Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
																	Pd.ExchangeRate = Conversion.ToDecimal(r7.Cells["ExchangeRate"].Value);
																	Pd.FcyAmount = Conversion.ToDecimal(r7.Cells["FcyAmount"].Value);
																	if (Pd.Id > 0)
																	{
																		Pd.ActionTypeId = 2;
																	}
																	else
																	{
																		Pd.ActionTypeId = 1;
																	}
																	purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
																	continue;
																}
																MessageBox.Show("Bill Amount Required In Row#" + (r7.RowIndex + 1));
																return;
															}
															MessageBox.Show("City Name Required In Row#" + (r7.RowIndex + 1));
															return;
														}
														MessageBox.Show("WareHouse Required In Row#" + (r7.RowIndex + 1));
														return;
													}
													MessageBox.Show("Item Amount Required In Row#" + (r7.RowIndex + 1));
													return;
												}
												MessageBox.Show("Rate Uom Required In Row#" + (r7.RowIndex + 1));
												return;
											}
											MessageBox.Show("Item Rate Required In Row#" + (r7.RowIndex + 1));
											return;
										}
										MessageBox.Show("Stock Weight Required In Row#" + (r7.RowIndex + 1));
										return;
									}
									MessageBox.Show("Net Weight Required In Row#" + (r7.RowIndex + 1));
									return;
								}
								MessageBox.Show("Gross Weight Required In Row#" + (r7.RowIndex + 1));
								return;
							}
							MessageBox.Show("ItemQty Required In Row#" + (r7.RowIndex + 1));
							return;
						}
						MessageBox.Show("PackUom Required In Row#" + (r7.RowIndex + 1));
						return;
					}
					MessageBox.Show("Item Name Required In Row#" + (r7.RowIndex + 1));
					return;
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt((object)r3.Cells["TransporterId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.TansporterId = Conversion.ToInt(r3.Cells["TransporterId"].Value);
						PF.Percentage = Conversion.ToDecimal(r3.Cells["Percentage"].Value);
						PF.FrQty = Conversion.ToDecimal(r3.Cells["Qty"].Value);
						PF.FrRate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
						PF.CreditAmount = Conversion.ToDecimal(r3.Cells["Freight"].Value);
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
						PE.InvOtherItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
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
						PJ.JvPrcnt = Conversion.ToDecimal(r.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDecimal(r.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDecimal(r.Cells["Credit"].Value);
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				purchaseInvoice.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 1503, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				Reset();
				if (ChkBok.Checked)
				{
					CommonServices.PurchaseInvoiceSteelSlip_1510(sucess);
				}
				if (chkVoucherPreview.Checked)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(sucess, 1503), 1503);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(grdHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("you Cannot Update Approved Record");
			}
			ReadById(Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			lstRemoveRecord.Clear();
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(RecId);
			if (invPI != null)
			{
				tabControl1.SelectedIndex = 0;
				CmbBranch.Value = invPI.BranchesId;
				CmbProject.Value = invPI.ProjectsId;
				txtDocNo.Text = invPI.DocNo.ToString();
				DocDate.Value = invPI.DocDate;
				CmbSupplier.Value = invPI.SupplierCustomerId;
				txtManualBillNo.Text = invPI.ManualBillNo;
				CmbPaymentTerm.Value = invPI.PaymentTermsId;
				DueDate.Value = invPI.DueDate;
				txtDueDays.Text = invPI.DueDays.ToString();
				((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)invPI.DeliveryTerm);
				cmbCurrency.Value = invPI.CurrencyId;
				txtExchangeRate.Text = invPI.ExchangeRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtTotalQty.Text = invPI.TotalQty.ToString("#,##0.##");
				txtTotalWeight.Text = invPI.TotalWeight.ToString("#,##0.##");
				CmbCommAgent.Value = invPI.CommissionAgentId;
				((Control)(object)cmbCommType).Text = Conversion.ToString((object)invPI.CommissionType);
				txtCommRate.Text = invPI.CommRate.ToString(clsGlobalVariables.DecimalRateFormate);
				((Control)(object)cmbCommUom).Text = Conversion.ToString((object)invPI.CommRateUom);
				txtCommAmount.Text = invPI.CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtCommRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
				CmbBrokeryAc.Value = invPI.BrokerAgentId;
				((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)invPI.BrokeryType);
				txtBrokeryRate.Text = invPI.BrokeryRate.ToString(clsGlobalVariables.DecimalRateFormate);
				((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString((object)invPI.BrokeryRateUom);
				txtBrokeryAmount.Text = invPI.BrokeryAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtBrokeryRemarks.Text = Conversion.ToString((object)invPI.BrokeryRemarks);
				txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtRemarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
				Approved = invPI.IsApproved;
				if (invPI.invPurchaseInvoiceDetailList[0].PurchaseOrderId > 0)
				{
					((Control)(object)CmbSupplier).Enabled = false;
					IsDetailHasPurchaseOrderId = true;
				}
				dtGrid.Rows.Clear();
				for (int l = 0; l < invPI.invPurchaseInvoiceDetailList.Count; l++)
				{
					dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[l].Id, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderId, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrderDetailId, invPI.invPurchaseInvoiceDetailList[l].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[l].ItemId, invPI.invPurchaseInvoiceDetailList[l].ItemName, invPI.invPurchaseInvoiceDetailList[l].JobLotId, invPI.invPurchaseInvoiceDetailList[l].JobLotDescription, invPI.invPurchaseInvoiceDetailList[l].PackingTypeId, invPI.invPurchaseInvoiceDetailList[l].PackingType, invPI.invPurchaseInvoiceDetailList[l].ItemUOMId, invPI.invPurchaseInvoiceDetailList[l].UOMCode, invPI.invPurchaseInvoiceDetailList[l].PackUom, invPI.invPurchaseInvoiceDetailList[l].ItemQty, invPI.invPurchaseInvoiceDetailList[l].GrossWeight, invPI.invPurchaseInvoiceDetailList[l].WCut, invPI.invPurchaseInvoiceDetailList[l].WcutTotal, invPI.invPurchaseInvoiceDetailList[l].AdLsWeight, invPI.invPurchaseInvoiceDetailList[l].NetBillWeight, invPI.invPurchaseInvoiceDetailList[l].NetStockWeight, invPI.invPurchaseInvoiceDetailList[l].ItemRate, invPI.invPurchaseInvoiceDetailList[l].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[l].RateUom, invPI.invPurchaseInvoiceDetailList[l].EquivalentPoRate, invPI.invPurchaseInvoiceDetailList[l].RateCut, invPI.invPurchaseInvoiceDetailList[l].RateCutAmount, invPI.invPurchaseInvoiceDetailList[l].ItemAmount, invPI.invPurchaseInvoiceDetailList[l].FreightAmount, invPI.invPurchaseInvoiceDetailList[l].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[l].CommissionAmount, invPI.invPurchaseInvoiceDetailList[l].ItemNetAmount, invPI.invPurchaseInvoiceDetailList[l].ExchangeRate, invPI.invPurchaseInvoiceDetailList[l].FcyAmount, invPI.invPurchaseInvoiceDetailList[l].WarehouseId, invPI.invPurchaseInvoiceDetailList[l].WareHouseName, invPI.invPurchaseInvoiceDetailList[l].CityId, invPI.invPurchaseInvoiceDetailList[l].CityName, invPI.invPurchaseInvoiceDetailList[l].GpNo, invPI.invPurchaseInvoiceDetailList[l].GpDate.ToShortDateString(), invPI.invPurchaseInvoiceDetailList[l].VehicleNo, invPI.invPurchaseInvoiceDetailList[l].RemarksDetail);
				}
				dtFreight.Rows.Clear();
				for (int k = 0; k < invPI.InvPurchaseInvoiceFreightList.Count; k++)
				{
					dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[k].TansporterId, invPI.InvPurchaseInvoiceFreightList[k].Percentage, invPI.InvPurchaseInvoiceFreightList[k].FrQty, invPI.InvPurchaseInvoiceFreightList[k].FrRate, Math.Round(invPI.InvPurchaseInvoiceFreightList[k].CreditAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount), invPI.InvPurchaseInvoiceFreightList[k].Remarks);
				}
				dtInvExp.Rows.Clear();
				for (int j = 0; j < invPI.InvPurchaseInvoiceExpList.Count; j++)
				{
					dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[j].InvOtherItemId, invPI.InvPurchaseInvoiceExpList[j].Qty, invPI.InvPurchaseInvoiceExpList[j].Rate, invPI.InvPurchaseInvoiceExpList[j].Amount, invPI.InvPurchaseInvoiceExpList[j].Remarks);
				}
				dtGrdGL.Rows.Clear();
				for (int i = 0; i < invPI.InvPurchaseInvoicejournalList.Count; i++)
				{
					dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[i].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[i].JvRemarks, invPI.InvPurchaseInvoicejournalList[i].JvPrcnt, invPI.InvPurchaseInvoicejournalList[i].JvQty, invPI.InvPurchaseInvoicejournalList[i].JvRate, invPI.InvPurchaseInvoicejournalList[i].JvDebit, invPI.InvPurchaseInvoicejournalList[i].JvCredit);
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grdFreightSettings();
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.DataSource = dtInvExp;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				gridGLSettings();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				VoucherHeadIdGet();
				ExpProportion();
				CommissionProportion();
				BillProportion();
				CalculateTotalQtyWeightAndFcyAmount();
				tabControl1.SelectedIndex = 0;
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
			}
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
			if (RecId == 0)
			{
				throw new Exception("Record Not Update beacause Record Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.F10 && tabControl1.SelectedIndex == 0)
			{
				btnAttachment_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnResetHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnFrmRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					BtnRefreshHistory_Click(null, null);
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
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 0)
			{
				toolStripButton3_Click_1(null, null);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Total()
	{
		try
		{
			decimal Qty = default(decimal);
			decimal GrossWeight = default(decimal);
			decimal WeightCut = default(decimal);
			decimal WeightCutTotal = default(decimal);
			decimal AddLess = default(decimal);
			if (((Control)(object)CmbPackUom).Text != string.Empty)
			{
				Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			if (txtItemQty.Text != string.Empty && txtItemQty.Text != null)
			{
				Qty = Conversion.ToDecimal((object)txtItemQty.Text.Trim());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim());
			}
			if (txtNetWeight.Text != string.Empty && txtNetWeight.Text != null)
			{
				Conversion.ToDecimal((object)txtNetWeight.Text.Trim());
			}
			if (txtStockWeight.Text != string.Empty && txtStockWeight.Text != null)
			{
				Conversion.ToDecimal((object)txtStockWeight.Text.Trim());
			}
			if (txtWeighttCut.Text != string.Empty && txtWeighttCut.Text != null)
			{
				WeightCut = Conversion.ToDecimal((object)txtWeighttCut.Text);
				WeightCutTotal = Qty * WeightCut;
				txtWeighttCutTotal.Text = WeightCutTotal.ToString("#,##0.##");
			}
			else
			{
				txtWeighttCutTotal.Text = "0";
			}
			AddLess = ((!(txtAddLessWeight.Text != string.Empty) || txtAddLessWeight.Text == null) ? default(decimal) : Conversion.ToDecimal((object)txtAddLessWeight.Text.Trim()));
			decimal Weight = GrossWeight - WeightCutTotal + AddLess;
			txtNetWeight.Text = Weight.ToString("#,##0.##");
			if (GrossWeight > 0m)
			{
				decimal StockWeiht = GrossWeight - WeightCutTotal + AddLess;
				txtStockWeight.Text = StockWeiht.ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalWeight()
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				decimal PackUOM = default(decimal);
				decimal Qty = default(decimal);
				decimal GrossWeight = default(decimal);
				if (((Control)(object)CmbPackUom).Text != string.Empty)
				{
					PackUOM = Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				}
				if (txtItemQty.Text != string.Empty)
				{
					Qty = Conversion.ToDecimal((object)txtItemQty.Text.Trim());
				}
				if (txtGrossWeight.Text != string.Empty)
				{
					GrossWeight = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim());
				}
				GrossWeight = PackUOM * Qty;
				txtGrossWeight.Text = GrossWeight.ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AmountCaluculation()
	{
		try
		{
			decimal RateUOM = default(decimal);
			decimal Rate = default(decimal);
			decimal RateCut = default(decimal);
			decimal NetWeight = default(decimal);
			decimal ItemAmount = default(decimal);
			decimal RateCutTotal = default(decimal);
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToInt(CmbRateUom.Value) > 0)
			{
				RateUOM = Conversion.ToDecimal(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
			}
			if (txtItemRate.Text.Trim() != string.Empty)
			{
				Rate = Conversion.ToDecimal((object)txtItemRate.Text);
			}
			if (txtRateCut.Text.Trim() != string.Empty)
			{
				RateCut = Conversion.ToDecimal((object)txtRateCut.Text);
			}
			if (txtNetWeight.Text.Trim() != string.Empty)
			{
				NetWeight = Conversion.ToDecimal((object)txtNetWeight.Text);
			}
			if (NetWeight > 0m && RateUOM > 0m && Rate > 0m)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				RateCutTotal = NetWeight / RateUOM * RateCut;
				txtRateCutAmount.Text = RateCutTotal.ToString(clsGlobalVariables.stringFormatsingle);
				ItemAmount -= RateCutTotal;
				txtItemAmount.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtItemAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					double BrokeryRate3 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					txtBrokeryAmount.Text = BrokeryRate3.ToString(clsGlobalVariables.stringFormatsingle);
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
					double BrokeryRate2 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double BrokeryAmount2 = Conversion.ToDouble((object)total.ToString()) * BrokeryRate2 / 100.0;
						txtBrokeryAmount.Text = BrokeryAmount2.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "By Weight")
				{
					double BrokeryRate = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
						double BrokeryAmount = num / UOM * BrokeryRate;
						txtBrokeryAmount.Text = BrokeryAmount.ToString(clsGlobalVariables.stringFormatsingle);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtCommRate.Text != string.Empty && ((Control)(object)cmbCommType).Text == "Flat")
			{
				double CommRate3 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				txtCommAmount.Text = CommRate3.ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (txtCommRate.Text != string.Empty && ((Control)(object)cmbCommType).Text == "Percent")
			{
				double CommRate2 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
					txtCommAmount.Text = CommAmt2.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			if (txtCommRate.Text != string.Empty && ((Control)(object)cmbCommType).Text == "By Weight")
			{
				double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
				if (Weight != null)
				{
					double num = Conversion.ToDouble((object)Weight.ToString());
					double UOM = Conversion.ToDouble((object)((Control)(object)cmbCommUom).Text.Trim());
					double CommAmt = num / UOM * CommRate;
					txtCommAmount.Text = CommAmt.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		CommissionProportion();
	}

	public void CalculateTaxAmount()
	{
	}

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				LoadPO = new LoadPurchaseOrder_St(UserAccount);
				LoadPO.ShowDialog();
				LoadPO.BringToFront();
				LoadDataDetailGridAgainstOrder();
				return;
			}
			if (Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) > 0)
			{
				LoadPO = new LoadPurchaseOrder_St(UserAccount);
				LoadPO.ShowDialog();
				LoadPO.BringToFront();
				LoadDataDetailGridAgainstOrder();
				return;
			}
			throw new Exception("You Can't Load Purchase Order Because Direct Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BranchFill();
			ProjectFill();
			SupplierNameFilll();
			PaymentTerms();
			CurrencyFill();
			ItemDetailFill();
			joblotfill();
			PackingTypeFill();
			WareHouseFill();
			CityFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void closeHistory_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete beacause Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.DeleteById(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1503);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnWithPO_CheckedChanged(object sender, EventArgs e)
	{
		if (btnWithPO.Checked)
		{
			panel8.Visible = false;
			return;
		}
		panel8.Visible = true;
		Reset();
	}

	private void btnWithoutPO_CheckedChanged(object sender, EventArgs e)
	{
		if (btnWithoutPO.Checked)
		{
			panel8.Visible = true;
			Reset();
		}
		else
		{
			panel8.Visible = false;
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			TotalBrokeryAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combrateuom_Leave(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			tabControl2.SelectedIndex = 0;
			GridEXRow item = grd.CurrentRow;
			if (Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
			{
				((Control)(object)CmbItem).Enabled = false;
				txtItemRate.Enabled = false;
			}
			else
			{
				((Control)(object)CmbItem).Enabled = true;
				txtItemRate.Enabled = true;
			}
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["ItemUomId"].Value.ToString();
				txtItemQty.Text = item.Cells["ItemQty"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtWeighttCut.Text = item.Cells["WtCut"].Value.ToString();
				txtWeighttCutTotal.Text = item.Cells["WtCutTotal"].Value.ToString();
				txtAddLessWeight.Text = item.Cells["AdLsWeight"].Value.ToString();
				txtNetWeight.Text = item.Cells["NetBillWeight"].Value.ToString();
				txtStockWeight.Text = item.Cells["StockWeight"].Value.ToString();
				txtItemRate.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				txtRateCut.Text = item.Cells["RateCut"].Value.ToString();
				txtRateCutAmount.Text = item.Cells["RateCutAmount"].Value.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WarehouseId"].Value.ToString();
				CmbCity.Value = item.Cells["CityId"].Value.ToString();
				txtGpNo.Text = item.Cells["GpNo"].Text.ToString();
				GpDate.Value = Conversion.ToDateTime(item.Cells["GpDate"].Value);
				txtVehicleNo.Text = item.Cells["VehicleNo"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtGrid.Rows[updateDetailIndex]["ItemId"] = CmbItem.Value;
				dtGrid.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItem).Text;
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtGrid.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				dtGrid.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text;
				dtGrid.Rows[updateDetailIndex]["ItemUomId"] = CmbPackUom.Value;
				dtGrid.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				dtGrid.Rows[updateDetailIndex]["PackEquivalent"] = ((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value;
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = txtItemQty.Text;
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeight.Text;
				dtGrid.Rows[updateDetailIndex]["WtCut"] = txtWeighttCut.Text;
				dtGrid.Rows[updateDetailIndex]["WtCutTotal"] = txtWeighttCutTotal.Text;
				dtGrid.Rows[updateDetailIndex]["AdLsWeight"] = txtAddLessWeight.Text;
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = txtNetWeight.Text;
				dtGrid.Rows[updateDetailIndex]["StockWeight"] = txtStockWeight.Text;
				dtGrid.Rows[updateDetailIndex]["ItemRate"] = txtItemRate.Text;
				dtGrid.Rows[updateDetailIndex]["RateUomId"] = CmbRateUom.Value;
				dtGrid.Rows[updateDetailIndex]["RateUom"] = ((Control)(object)CmbRateUom).Text;
				dtGrid.Rows[updateDetailIndex]["RateEquivalent"] = ((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value;
				dtGrid.Rows[updateDetailIndex]["RateCut"] = txtRateCut.Text;
				dtGrid.Rows[updateDetailIndex]["RateCutAmount"] = txtRateCutAmount.Text;
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = txtItemAmount.Text;
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = CmbWareHouse.Value;
				dtGrid.Rows[updateDetailIndex]["WarehouseName"] = ((Control)(object)CmbWareHouse).Text;
				dtGrid.Rows[updateDetailIndex]["CityId"] = CmbCity.Value;
				dtGrid.Rows[updateDetailIndex]["CityName"] = ((Control)(object)CmbCity).Text;
				dtGrid.Rows[updateDetailIndex]["GpNo"] = txtGpNo.Text;
				dtGrid.Rows[updateDetailIndex]["GpDate"] = GpDate.Value.ToShortDateString();
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = txtVehicleNo.Text;
				dtGrid.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				txtExchangeRate_TextChanged(null, null);
				grd.DataSource = dtGrid;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		updateDetailIndex = -1;
		ResetDetail();
	}

	private void btnclose_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 1503), 1503);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.PurchaseInvoiceSteelSlip_1510(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "PartySlip")
			{
				CommonServices.PurchaseInvoiceSteelSlip_1510A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					obj.Cells["ExchangeRate"].Value = txtExchangeRate.Text;
					decimal amount = Conversion.ToDecimal(obj.Cells["ItemAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
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
					obj2.Cells["ExchangeRate"].Value = 0;
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalQtyWeightAndFcyAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeighttCut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtItemRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRateCut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double amount = num * Rate;
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
					item.Cells["Freight"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FreightProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["NetBillWeight"].Value);
					obj.Cells["Freight"].Value = Math.Round(CreditForFreight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount) / NetWeight * Weight;
					obj.BeginEdit();
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPackUOM_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeighttCut_TextChanged(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddLessWeight_TextChanged(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbRateUom_ValueChanged(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRateCut_TextChanged(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DuedaysCalculates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DuedaysCalculates()
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbBrokeryType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtBrokeryRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbBrokeryRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCommRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("PurchaseOrder");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtdetail.Columns.Add("ItemNetAmount", typeof(double));
			dtdetail.Columns.Add("ExchangeRate", typeof(double));
			dtdetail.Columns.Add("FcyAmount", typeof(double));
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("CityName");
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			InvPurchaseInvoice obj = new InvPurchaseInvoice();
			obj = InvPurchaseInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.invPurchaseInvoiceDetailList.Count; i++)
				{
					dtdetail.Rows.Add(obj.invPurchaseInvoiceDetailList[i].PurchaseOrder, obj.invPurchaseInvoiceDetailList[i].ItemName, obj.invPurchaseInvoiceDetailList[i].JobLotDescription, obj.invPurchaseInvoiceDetailList[i].PackingType, obj.invPurchaseInvoiceDetailList[i].UOMCode, obj.invPurchaseInvoiceDetailList[i].ItemQty, obj.invPurchaseInvoiceDetailList[i].GrossWeight, obj.invPurchaseInvoiceDetailList[i].WcutTotal, obj.invPurchaseInvoiceDetailList[i].NetBillWeight, obj.invPurchaseInvoiceDetailList[i].NetStockWeight, obj.invPurchaseInvoiceDetailList[i].ItemRate, obj.invPurchaseInvoiceDetailList[i].RateUom, obj.invPurchaseInvoiceDetailList[i].RateCut, obj.invPurchaseInvoiceDetailList[i].RateCutAmount, obj.invPurchaseInvoiceDetailList[i].ItemAmount, obj.invPurchaseInvoiceDetailList[i].CommissionAmount, obj.invPurchaseInvoiceDetailList[i].ExpenseAmount, obj.invPurchaseInvoiceDetailList[i].FreightAmount, obj.invPurchaseInvoiceDetailList[i].ItemNetAmount, obj.invPurchaseInvoiceDetailList[i].ExchangeRate, obj.invPurchaseInvoiceDetailList[i].FcyAmount, obj.invPurchaseInvoiceDetailList[i].WareHouseName, obj.invPurchaseInvoiceDetailList[i].CityName, obj.invPurchaseInvoiceDetailList[i].GpNo, obj.invPurchaseInvoiceDetailList[i].VehicleNo);
				}
				GridDetail.DataSource = dtdetail;
				GridDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			GridDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			GridDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			GridDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			GridDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			GridDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			GridDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				GridEXRow r = grd.CurrentRow;
				DataRow dr = dtGrid.Rows[r.RowIndex];
				dtGrid.Rows.Add(dr.ItemArray);
				grd.DataSource = dtGrid;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
				}
			}
			else if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (updateDetailIndex == Conversion.ToInt((object)item.RowIndex))
				{
					throw new Exception("You can't Delete this Record Because This Record is In Update Mode");
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				if (btnSave.Visible && btnSave.Enabled)
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
				else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(item.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(item.Cells["OrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(item.Cells["OrderDetailId"].Value);
					Pd.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					Pd.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
					Pd.PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
					Pd.ItemUOMId = Conversion.ToInt(item.Cells["ItemUomId"].Value);
					Pd.ItemQty = Conversion.ToDecimal(item.Cells["ItemQty"].Value);
					Pd.GrossWeight = Conversion.ToDecimal(item.Cells["GrossWeight"].Value);
					Pd.WCut = Conversion.ToDecimal(item.Cells["WtCut"].Value);
					Pd.WcutTotal = Conversion.ToDecimal(item.Cells["WtCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDecimal(item.Cells["AdLsWeight"].Value);
					Pd.NetBillWeight = Conversion.ToDecimal(item.Cells["NetBillWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDecimal(item.Cells["StockWeight"].Value);
					Pd.ItemRate = Conversion.ToDecimal(item.Cells["ItemRate"].Value);
					Pd.UomScheduleIdRate = Conversion.ToInt(item.Cells["RateUomId"].Value);
					Pd.RateCut = Conversion.ToDecimal(item.Cells["RateCut"].Value);
					Pd.RateCutAmount = Conversion.ToDecimal(item.Cells["RateCutAmount"].Value);
					Pd.ItemAmount = Conversion.ToDecimal(item.Cells["ItemAmount"].Value);
					Pd.WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
					Pd.CityId = Conversion.ToInt(item.Cells["CityId"].Value);
					Pd.ItemCgsRate = 0m;
					Pd.GpNo = Conversion.ToInt(item.Cells["GpNo"].Value);
					Pd.GpDate = Conversion.ToDateTime(item.Cells["GpDate"].Value);
					Pd.VehicleNo = Conversion.ToString(item.Cells["VehicleNo"].Value);
					Pd.RemarksDetail = Conversion.ToString(item.Cells["Remarks"].Value);
					Pd.ItemNetAmount = Conversion.ToDecimal(item.Cells["BillAmount"].Value);
					Pd.FreightAmount = Conversion.ToDecimal(item.Cells["Freight"].Value);
					Pd.ExpenseAmount = Conversion.ToDecimal(item.Cells["Expense"].Value);
					Pd.CommissionAmount = Conversion.ToDecimal(item.Cells["Commission"].Value);
					Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					Pd.ExchangeRate = Conversion.ToDecimal(item.Cells["ExchangeRate"].Value);
					Pd.FcyAmount = Conversion.ToDecimal(item.Cells["FcyAmount"].Value);
					Pd.ActionTypeId = 3;
					lstRemoveRecord.Add(Pd);
					dtGrid.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					grd.UpdateData();
				}
				else if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			if (grd.GetRows().Count() == 0 && IsDetailHasPurchaseOrderId)
			{
				Reset();
			}
			CommissionProportion();
			FreightProportion();
			ExpProportion();
			BillAmount();
			CalculateTotalQtyWeightAndFcyAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSlipDetail1510_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.PurchaseInvoiceSteelSlip_1510(RecId);
				return;
			}
			throw new Exception("No Record Seleted");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSlip1510A_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.PurchaseInvoiceSteelSlip_1510A(RecId);
				return;
			}
			throw new Exception("No Record Seleted");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void txtCommRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtCommRate.Text = Conversion.ToDecimal((object)txtCommRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtBrokeryRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtBrokeryRate.Text = Conversion.ToDecimal((object)txtBrokeryRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtGrossWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtGrossWeight.Text = Conversion.ToDecimal((object)txtGrossWeight.Text).ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeighttCut_Leave(object sender, EventArgs e)
	{
		try
		{
			txtWeighttCut.Text = Conversion.ToDecimal((object)txtWeighttCut.Text).ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddLessWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
			if (e.KeyChar == '.' && txtAddLessWeight.Text.Contains("."))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddLessWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtAddLessWeight.Text = Conversion.ToDecimal((object)txtAddLessWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtItemRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtItemRate.Text = Conversion.ToDecimal((object)txtItemRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRateCut_Leave(object sender, EventArgs e)
	{
		try
		{
			txtRateCut.Text = Conversion.ToDecimal((object)txtRateCut.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbCommAgent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CalculateTotalQtyWeightAndFcyAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PurchaseInvoiceDirect_St_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbPaymentTermHistory).Text = string.Empty;
			((Control)(object)CmbDeliveryTermHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			GridDetail.DataSource = null;
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
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Expected O, but got Unknown
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Expected O, but got Unknown
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Expected O, but got Unknown
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Expected O, but got Unknown
		//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Expected O, but got Unknown
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd0: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1a: Expected O, but got Unknown
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Expected O, but got Unknown
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Expected O, but got Unknown
		//IL_0d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da9: Expected O, but got Unknown
		//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Expected O, but got Unknown
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Expected O, but got Unknown
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e64: Expected O, but got Unknown
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Expected O, but got Unknown
		//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Expected O, but got Unknown
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb1: Expected O, but got Unknown
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebc: Expected O, but got Unknown
		//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Expected O, but got Unknown
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Expected O, but got Unknown
		//IL_104f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Expected O, but got Unknown
		//IL_1065: Unknown result type (might be due to invalid IL or missing references)
		//IL_106f: Expected O, but got Unknown
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1085: Expected O, but got Unknown
		//IL_180a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1814: Expected O, but got Unknown
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_182b: Expected O, but got Unknown
		//IL_540d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5417: Expected O, but got Unknown
		//IL_5424: Unknown result type (might be due to invalid IL or missing references)
		//IL_542e: Expected O, but got Unknown
		//IL_563a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5644: Expected O, but got Unknown
		//IL_5651: Unknown result type (might be due to invalid IL or missing references)
		//IL_565b: Expected O, but got Unknown
		//IL_5867: Unknown result type (might be due to invalid IL or missing references)
		//IL_5871: Expected O, but got Unknown
		//IL_587e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5888: Expected O, but got Unknown
		//IL_818c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8196: Expected O, but got Unknown
		//IL_c642: Unknown result type (might be due to invalid IL or missing references)
		//IL_c64c: Expected O, but got Unknown
		//IL_c670: Unknown result type (might be due to invalid IL or missing references)
		//IL_c67a: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance78 = new Appearance();
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
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
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
		Appearance appearance152 = new Appearance();
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
		GridEXLayout grdSchedule_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
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
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
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
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbCity = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.txtRateCutAmount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtRateCut = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.GpDate = new System.Windows.Forms.DateTimePicker();
		this.txtAddLessWeight = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtWeighttCutTotal = new System.Windows.Forms.TextBox();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.txtWeighttCut = new System.Windows.Forms.TextBox();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbPackingType = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.CmbRateUom = new UltraCombo();
		this.txtGpNo = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.tabFreight = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.label27 = new System.Windows.Forms.Label();
		this.tabOtherItemExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPartyAddLess = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.label61 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkVoucherPreview = new System.Windows.Forms.CheckBox();
		this.txtBrokeryRemarks = new System.Windows.Forms.RichTextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbBrokeryType = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.CmbBrokeryAc = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.txtTotalQty = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtTotalWeight = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdSchedule = new GridEX();
		this.label49 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbstockprty = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.CmbBranch = new UltraCombo();
		this.CmbPaymentTerm = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbProject = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnWithPO = new System.Windows.Forms.RadioButton();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.CmbTaxAccount = new UltraCombo();
		this.cmbCommUom = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.btnWithoutPO = new System.Windows.Forms.RadioButton();
		this.txtManualBillNo = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.cmbCommType = new UltraCombo();
		this.CmbCommAgent = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtCommRemarks = new System.Windows.Forms.RichTextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoadPo = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlipDetail1510 = new System.Windows.Forms.ToolStripButton();
		this.btnSlip1510A = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.label28 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbDeliveryTermHistory = new UltraCombo();
		this.label67 = new System.Windows.Forms.Label();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.label63 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label66 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label68 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label69 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		this.tabFreight.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.tabOtherItemExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.tabPartyAddLess.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.panel18.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1210, 730);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1210, 730);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1202, 704);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1196, 670);
		this.panel5.TabIndex = 3;
		this.panel12.Controls.Add(this.panel17);
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 497);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1196, 173);
		this.panel12.TabIndex = 7;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 23);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1196, 150);
		this.panel17.TabIndex = 21;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1196, 150);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(1196, 23);
		this.label1.TabIndex = 5;
		this.label1.Text = "Detail Grid";
		this.panel8.Controls.Add(this.tabControl2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 310);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1196, 187);
		this.panel8.TabIndex = 5;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabFreight);
		this.tabControl2.Controls.Add(this.tabOtherItemExpense);
		this.tabControl2.Controls.Add(this.tabPartyAddLess);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1196, 187);
		this.tabControl2.TabIndex = 0;
		this.tabDetail.Controls.Add(this.txtRemarksDetail);
		this.tabDetail.Controls.Add(this.label62);
		this.tabDetail.Controls.Add(this.btnUpdateDetail);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.tabDetail.Controls.Add(this.btnCancelUpdateDetial);
		this.tabDetail.Controls.Add(this.btnplus);
		this.tabDetail.Controls.Add(this.label53);
		this.tabDetail.Controls.Add(this.txtRateCutAmount);
		this.tabDetail.Controls.Add(this.label16);
		this.tabDetail.Controls.Add(this.label55);
		this.tabDetail.Controls.Add(this.label14);
		this.tabDetail.Controls.Add(this.txtRateCut);
		this.tabDetail.Controls.Add(this.label54);
		this.tabDetail.Controls.Add(this.label46);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.tabDetail.Controls.Add(this.label51);
		this.tabDetail.Controls.Add(this.txtNetWeight);
		this.tabDetail.Controls.Add(this.label52);
		this.tabDetail.Controls.Add(this.txtStockWeight);
		this.tabDetail.Controls.Add(this.GpDate);
		this.tabDetail.Controls.Add(this.txtAddLessWeight);
		this.tabDetail.Controls.Add(this.label42);
		this.tabDetail.Controls.Add(this.label45);
		this.tabDetail.Controls.Add(this.label47);
		this.tabDetail.Controls.Add(this.txtWeighttCutTotal);
		this.tabDetail.Controls.Add(this.txtItemRate);
		this.tabDetail.Controls.Add(this.txtWeighttCut);
		this.tabDetail.Controls.Add(this.txtItemAmount);
		this.tabDetail.Controls.Add(this.txtGrossWeight);
		this.tabDetail.Controls.Add(this.label39);
		this.tabDetail.Controls.Add(this.txtVehicleNo);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.tabDetail.Controls.Add(this.label50);
		this.tabDetail.Controls.Add(this.label44);
		this.tabDetail.Controls.Add(this.label12);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.tabDetail.Controls.Add(this.txtGpNo);
		this.tabDetail.Controls.Add(this.label37);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.tabDetail.Controls.Add(this.label34);
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.tabDetail.Controls.Add(this.label36);
		this.tabDetail.Controls.Add(this.txtItemQty);
		this.tabDetail.Controls.Add(this.label38);
		this.tabDetail.Controls.Add(this.label40);
		this.tabDetail.Controls.Add(this.label41);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1188, 158);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Invoice Detail ";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(96, 115);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(906, 23);
		this.txtRemarksDetail.TabIndex = 21;
		this.label62.BackColor = System.Drawing.Color.LightGray;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(8, 115);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(82, 23);
		this.label62.TabIndex = 64;
		this.label62.Text = "Remarks";
		this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1037, 115);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 23);
		this.btnUpdateDetail.TabIndex = 23;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(626, 83);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 17;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1104, 115);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(61, 23);
		this.btnCancelUpdateDetial.TabIndex = 24;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1003, 115);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(33, 23);
		this.btnplus.TabIndex = 22;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(626, 66);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(62, 15);
		this.label53.TabIndex = 62;
		this.label53.Text = "City Name";
		this.txtRateCutAmount.Enabled = false;
		this.txtRateCutAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateCutAmount.Location = new System.Drawing.Point(247, 85);
		this.txtRateCutAmount.Name = "txtRateCutAmount";
		this.txtRateCutAmount.Size = new System.Drawing.Size(99, 23);
		this.txtRateCutAmount.TabIndex = 14;
		this.txtRateCutAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(947, 66);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(47, 15);
		this.label16.TabIndex = 38;
		this.label16.Text = "GpDate";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(248, 66);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(98, 15);
		this.label55.TabIndex = 60;
		this.label55.Text = "Rate Cut Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(462, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(104, 15);
		this.label14.TabIndex = 0;
		this.label14.Text = "WareHouse Name";
		this.txtRateCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateCut.Location = new System.Drawing.Point(167, 85);
		this.txtRateCut.Name = "txtRateCut";
		this.txtRateCut.Size = new System.Drawing.Size(78, 23);
		this.txtRateCut.TabIndex = 13;
		this.txtRateCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateCut.TextChanged += new System.EventHandler(txtRateCut_TextChanged);
		this.txtRateCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateCut_KeyPress);
		this.txtRateCut.Leave += new System.EventHandler(txtRateCut_Leave);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(164, 66);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(51, 15);
		this.label54.TabIndex = 58;
		this.label54.Text = "Rate Cut";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(923, 15);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(75, 15);
		this.label46.TabIndex = 52;
		this.label46.Text = "AdLs Weight";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(462, 83);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 16;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(1000, 15);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(87, 15);
		this.label51.TabIndex = 53;
		this.label51.Text = "Net Bill Weight";
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(1001, 35);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(87, 23);
		this.txtNetWeight.TabIndex = 9;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(1091, 15);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 54;
		this.label52.Text = "Stock Weight";
		this.txtStockWeight.BackColor = System.Drawing.Color.White;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Enabled = false;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(1090, 35);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(90, 23);
		this.txtStockWeight.TabIndex = 10;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GpDate.CustomFormat = "dd-MMM-yy";
		this.GpDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GpDate.Location = new System.Drawing.Point(950, 85);
		this.GpDate.Name = "GpDate";
		this.GpDate.Size = new System.Drawing.Size(99, 23);
		this.GpDate.TabIndex = 19;
		this.GpDate.TabStop = false;
		this.txtAddLessWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessWeight.Location = new System.Drawing.Point(924, 35);
		this.txtAddLessWeight.Name = "txtAddLessWeight";
		this.txtAddLessWeight.Size = new System.Drawing.Size(75, 23);
		this.txtAddLessWeight.TabIndex = 8;
		this.txtAddLessWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessWeight.TextChanged += new System.EventHandler(txtAddLessWeight_TextChanged);
		this.txtAddLessWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLessWeight_KeyPress);
		this.txtAddLessWeight.Leave += new System.EventHandler(txtAddLessWeight_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(843, 15);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(73, 15);
		this.label42.TabIndex = 48;
		this.label42.Text = "Wt Cut Total";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(770, 15);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(44, 15);
		this.label45.TabIndex = 47;
		this.label45.Text = "Wt Cut";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(878, 66);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(38, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "GpNo";
		this.txtWeighttCutTotal.BackColor = System.Drawing.Color.White;
		this.txtWeighttCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighttCutTotal.Enabled = false;
		this.txtWeighttCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeighttCutTotal.Location = new System.Drawing.Point(840, 35);
		this.txtWeighttCutTotal.Name = "txtWeighttCutTotal";
		this.txtWeighttCutTotal.ReadOnly = true;
		this.txtWeighttCutTotal.Size = new System.Drawing.Size(83, 23);
		this.txtWeighttCutTotal.TabIndex = 7;
		this.txtWeighttCutTotal.TabStop = false;
		this.txtWeighttCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(8, 85);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.Size = new System.Drawing.Size(87, 23);
		this.txtItemRate.TabIndex = 11;
		this.txtItemRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemRate_KeyPress);
		this.txtItemRate.Leave += new System.EventHandler(txtItemRate_Leave);
		this.txtWeighttCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighttCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeighttCut.Location = new System.Drawing.Point(772, 35);
		this.txtWeighttCut.Name = "txtWeighttCut";
		this.txtWeighttCut.Size = new System.Drawing.Size(66, 23);
		this.txtWeighttCut.TabIndex = 6;
		this.txtWeighttCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeighttCut.TextChanged += new System.EventHandler(txtWeighttCut_TextChanged);
		this.txtWeighttCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeighttCut_KeyPress);
		this.txtWeighttCut.Leave += new System.EventHandler(txtWeighttCut_Leave);
		this.txtItemAmount.BackColor = System.Drawing.Color.White;
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Enabled = false;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(347, 85);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(113, 23);
		this.txtItemAmount.TabIndex = 15;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(691, 35);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(79, 23);
		this.txtGrossWeight.TabIndex = 5;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.txtGrossWeight.Leave += new System.EventHandler(txtGrossWeight_Leave);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(690, 15);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 31;
		this.label39.Text = "Gross Weight";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(1051, 85);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(127, 23);
		this.txtVehicleNo.TabIndex = 20;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(391, 33);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 2;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(394, 15);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(77, 15);
		this.label50.TabIndex = 29;
		this.label50.Text = "Packing Type";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(344, 66);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1053, 66);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "Vehicle No";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(8, 33);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(96, 84);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(69, 24);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 12;
		this.CmbRateUom.ValueChanged += new System.EventHandler(CmbRateUom_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(combrateuom_Leave);
		this.txtGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.Location = new System.Drawing.Point(874, 85);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(75, 23);
		this.txtGpNo.TabIndex = 18;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(11, 15);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(247, 33);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 1;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(246, 15);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(48, 15);
		this.label34.TabIndex = 23;
		this.label34.Text = "Job/Lot";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(528, 33);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		this.CmbPackUom.ValueChanged += new System.EventHandler(CmbPackUOM_ValueChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(531, 15);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(610, 35);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.Size = new System.Drawing.Size(79, 23);
		this.txtItemQty.TabIndex = 4;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(612, 15);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(8, 66);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(93, 66);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.tabFreight.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFreight.Controls.Add(this.label27);
		this.tabFreight.Location = new System.Drawing.Point(4, 25);
		this.tabFreight.Name = "tabFreight";
		this.tabFreight.Padding = new System.Windows.Forms.Padding(3);
		this.tabFreight.Size = new System.Drawing.Size(1188, 158);
		this.tabFreight.TabIndex = 1;
		this.tabFreight.Text = "Transporter";
		this.tabFreight.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1182, 129);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 6;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		this.label27.BackColor = System.Drawing.Color.Teal;
		this.label27.Dock = System.Windows.Forms.DockStyle.Top;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(3, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(1182, 23);
		this.label27.TabIndex = 5;
		this.tabOtherItemExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabOtherItemExpense.Controls.Add(this.label59);
		this.tabOtherItemExpense.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItemExpense.Name = "tabOtherItemExpense";
		this.tabOtherItemExpense.Size = new System.Drawing.Size(1188, 158);
		this.tabOtherItemExpense.TabIndex = 2;
		this.tabOtherItemExpense.Text = "Other Items Expense";
		this.tabOtherItemExpense.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1188, 135);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.label59.BackColor = System.Drawing.Color.Teal;
		this.label59.Dock = System.Windows.Forms.DockStyle.Top;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(0, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(1188, 23);
		this.label59.TabIndex = 4;
		this.tabPartyAddLess.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabPartyAddLess.Controls.Add(this.label61);
		this.tabPartyAddLess.Location = new System.Drawing.Point(4, 25);
		this.tabPartyAddLess.Name = "tabPartyAddLess";
		this.tabPartyAddLess.Size = new System.Drawing.Size(1188, 158);
		this.tabPartyAddLess.TabIndex = 3;
		this.tabPartyAddLess.Text = "Party AddLess";
		this.tabPartyAddLess.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1188, 135);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.label61.BackColor = System.Drawing.Color.Teal;
		this.label61.Dock = System.Windows.Forms.DockStyle.Top;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.White;
		this.label61.Location = new System.Drawing.Point(0, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(1188, 23);
		this.label61.TabIndex = 5;
		this.panel3.Controls.Add(this.chkVoucherPreview);
		this.panel3.Controls.Add(this.txtBrokeryRemarks);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add(this.label31);
		this.panel3.Controls.Add(this.label32);
		this.panel3.Controls.Add(this.txtBrokeryRate);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.txtBrokeryAmount);
		this.panel3.Controls.Add(this.txtTotalQty);
		this.panel3.Controls.Add(this.label60);
		this.panel3.Controls.Add(this.txtTotalAmount);
		this.panel3.Controls.Add(this.txtTotalWeight);
		this.panel3.Controls.Add(this.label48);
		this.panel3.Controls.Add(this.txtFcyAmount);
		this.panel3.Controls.Add(this.label58);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtExchangeRate);
		this.panel3.Controls.Add(this.label57);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.panel3.Controls.Add(this.label56);
		this.panel3.Controls.Add(this.txtDueDays);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.DueDate);
		this.panel3.Controls.Add(this.panel18);
		this.panel3.Controls.Add(this.txtTaxAmount);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add(this.label49);
		this.panel3.Controls.Add(this.txtRemarks);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtTaxPercent);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtCommAmount);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.txtBillAmount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProject);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label30);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.btnWithPO);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Controls.Add(this.txtDocNo);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.txtCommRate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommUom);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.btnWithoutPO);
		this.panel3.Controls.Add(this.txtManualBillNo);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgent);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtCommRemarks);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1196, 310);
		this.panel3.TabIndex = 4;
		this.chkVoucherPreview.AutoSize = true;
		this.chkVoucherPreview.Location = new System.Drawing.Point(888, 285);
		this.chkVoucherPreview.Margin = new System.Windows.Forms.Padding(2);
		this.chkVoucherPreview.Name = "chkVoucherPreview";
		this.chkVoucherPreview.Size = new System.Drawing.Size(135, 20);
		this.chkVoucherPreview.TabIndex = 617;
		this.chkVoucherPreview.Text = "Voucher Preview";
		this.chkVoucherPreview.UseVisualStyleBackColor = true;
		this.txtBrokeryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryRemarks.Location = new System.Drawing.Point(636, 254);
		this.txtBrokeryRemarks.Name = "txtBrokeryRemarks";
		this.txtBrokeryRemarks.Size = new System.Drawing.Size(247, 48);
		this.txtBrokeryRemarks.TabIndex = 28;
		this.txtBrokeryRemarks.Text = "";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(551, 258);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(52, 15);
		this.label35.TabIndex = 616;
		this.label35.Text = "Remarks";
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(636, 198);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 24;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TextChanged += new System.EventHandler(CmbBrokeryType_TextChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(1056, 349);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(37, 15);
		this.label43.TabIndex = 31;
		this.label43.Text = "Tax %";
		this.label43.Visible = false;
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(636, 226);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 26;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_TextChanged);
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(636, 170);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 23;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(551, 176);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(82, 15);
		this.label24.TabIndex = 607;
		this.label24.Text = "Brokery Agent";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(728, 232);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(51, 15);
		this.label29.TabIndex = 608;
		this.label29.Text = "Amount";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(551, 204);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(75, 15);
		this.label31.TabIndex = 605;
		this.label31.Text = "Brokery Type";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(728, 204);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(73, 15);
		this.label32.TabIndex = 609;
		this.label32.Text = "Brokery Rate";
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryRate.Location = new System.Drawing.Point(801, 200);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(82, 23);
		this.txtBrokeryRate.TabIndex = 25;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_TextChanged);
		this.txtBrokeryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryRate_KeyPress);
		this.txtBrokeryRate.Leave += new System.EventHandler(txtBrokeryRate_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(551, 232);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(61, 15);
		this.label33.TabIndex = 606;
		this.label33.Text = "Rate UOM";
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Enabled = false;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(801, 228);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(82, 23);
		this.txtBrokeryAmount.TabIndex = 27;
		this.txtBrokeryAmount.TabStop = false;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalQty.Enabled = false;
		this.txtTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalQty.Location = new System.Drawing.Point(419, 200);
		this.txtTotalQty.Name = "txtTotalQty";
		this.txtTotalQty.Size = new System.Drawing.Size(122, 23);
		this.txtTotalQty.TabIndex = 15;
		this.txtTotalQty.TabStop = false;
		this.txtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(338, 176);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(75, 15);
		this.label60.TabIndex = 604;
		this.label60.Text = "Total Weight";
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(1192, 376);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(87, 22);
		this.txtTotalAmount.TabIndex = 30;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.Visible = false;
		this.txtTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeight.Enabled = false;
		this.txtTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalWeight.Location = new System.Drawing.Point(419, 172);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.Size = new System.Drawing.Size(122, 23);
		this.txtTotalWeight.TabIndex = 14;
		this.txtTotalWeight.TabStop = false;
		this.txtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(1114, 349);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(71, 15);
		this.label48.TabIndex = 33;
		this.label48.Text = "Tax Amount";
		this.label48.Visible = false;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(419, 118);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(122, 23);
		this.txtFcyAmount.TabIndex = 12;
		this.txtFcyAmount.TabStop = false;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(338, 122);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(72, 15);
		this.label58.TabIndex = 603;
		this.label58.Text = "Fcy Amount";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(338, 204);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(55, 15);
		this.label5.TabIndex = 602;
		this.label5.Text = "Total Qty";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(419, 89);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(122, 23);
		this.txtExchangeRate.TabIndex = 11;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(338, 93);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(58, 15);
		this.label57.TabIndex = 595;
		this.label57.Text = "Exch Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(419, 60);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 10;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(338, 66);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(55, 15);
		this.label56.TabIndex = 596;
		this.label56.Text = "Fcy Code";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(113, 200);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(98, 23);
		this.txtDueDays.TabIndex = 7;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(9, 204);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(98, 15);
		this.label22.TabIndex = 40;
		this.label22.Text = "Due Days && Date";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Enabled = false;
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(216, 200);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(116, 23);
		this.DueDate.TabIndex = 8;
		this.panel18.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel18.Controls.Add(this.label23);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Margin = new System.Windows.Forms.Padding(2);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1196, 29);
		this.panel18.TabIndex = 37;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(174, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "PURCHASE INVOICE";
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(1113, 375);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(73, 22);
		this.txtTaxAmount.TabIndex = 10;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.Visible = false;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdSchedule);
		this.panel11.Location = new System.Drawing.Point(913, 347);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(85, 84);
		this.panel11.TabIndex = 2;
		this.panel11.Visible = false;
		((ScrollableBase)this.grdSchedule).BackColor = System.Drawing.Color.White;
		this.grdSchedule.ColumnAutoResize = true;
		grdSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSchedule.DesignTimeLayout = grdSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSchedule.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Location = new System.Drawing.Point(8, 9);
		((System.Windows.Forms.Control)(object)this.grdSchedule).Name = "grdSchedule";
		((ScrollableBase)this.grdSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Size = new System.Drawing.Size(69, 57);
		((System.Windows.Forms.Control)(object)this.grdSchedule).TabIndex = 0;
		this.grdSchedule.TotalRow = (InheritableBoolean)1;
		this.grdSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSchedule).VisualStyle = (VisualStyle)4;
		this.grdSchedule.CellUpdated += new ColumnActionEventHandler(grdSchedule_CellUpdated);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(1194, 349);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(79, 15);
		this.label49.TabIndex = 35;
		this.label49.Text = "Total Amount";
		this.label49.Visible = false;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(113, 228);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(428, 74);
		this.txtRemarks.TabIndex = 16;
		this.txtRemarks.Text = "";
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1004, 347);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(44, 26);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(1054, 375);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(56, 22);
		this.txtTaxPercent.TabIndex = 9;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1004, 376);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(44, 26);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1004, 436);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(52, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1001, 411);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(9, 176);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(84, 15);
		this.label10.TabIndex = 31;
		this.label10.Text = "Payment Term";
		this.txtCommAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAmount.Enabled = false;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAmount.Location = new System.Drawing.Point(801, 89);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.Size = new System.Drawing.Size(82, 23);
		this.txtCommAmount.TabIndex = 21;
		this.txtCommAmount.TabStop = false;
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1056, 490);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(90, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "PurchaseOrder";
		this.label15.Visible = false;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Enabled = false;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(419, 144);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.Size = new System.Drawing.Size(122, 23);
		this.txtBillAmount.TabIndex = 13;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(113, 32);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 0;
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(113, 170);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(551, 93);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(72, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm Uom";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(9, 93);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 38);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.CmbProject.AllowNull = (DefaultableBoolean)1;
		this.CmbProject.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbProject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbProject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbProject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProject).Location = new System.Drawing.Point(113, 60);
		((System.Windows.Forms.Control)(object)this.CmbProject).Name = "CmbProject";
		((System.Windows.Forms.Control)(object)this.CmbProject).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbProject).TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(731, 93);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(338, 38);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 33;
		this.label30.Text = "Delivery Term";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1053, 528);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Tax Account";
		this.label2.Visible = false;
		this.btnWithPO.AutoSize = true;
		this.btnWithPO.Location = new System.Drawing.Point(1152, 488);
		this.btnWithPO.Name = "btnWithPO";
		this.btnWithPO.Size = new System.Drawing.Size(48, 20);
		this.btnWithPO.TabIndex = 17;
		this.btnWithPO.TabStop = true;
		this.btnWithPO.Text = "Yes";
		this.btnWithPO.UseVisualStyleBackColor = true;
		this.btnWithPO.Visible = false;
		this.btnWithPO.CheckedChanged += new System.EventHandler(btnWithPO_CheckedChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(9, 148);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual Bill No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(9, 230);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(98, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Invoice Remarks";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(551, 66);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(419, 32);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 9;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Location = new System.Drawing.Point(888, 263);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(112, 20);
		this.ChkBok.TabIndex = 29;
		this.ChkBok.Text = "Print Preview";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(113, 89);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(98, 23);
		this.txtDocNo.TabIndex = 2;
		this.txtDocNo.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(338, 148);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Bill Amount";
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRate.Location = new System.Drawing.Point(801, 62);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(82, 23);
		this.txtCommRate.TabIndex = 19;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRate_KeyPress);
		this.txtCommRate.Leave += new System.EventHandler(txtCommRate_Leave);
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance232).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(1132, 523);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(129, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Visible = false;
		this.cmbCommUom.AllowNull = (DefaultableBoolean)1;
		this.cmbCommUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance238;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance243).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance244).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommUom).Location = new System.Drawing.Point(636, 87);
		((System.Windows.Forms.Control)(object)this.cmbCommUom).Name = "cmbCommUom";
		((System.Windows.Forms.Control)(object)this.cmbCommUom).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommUom).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.cmbCommUom).Leave += new System.EventHandler(cmbcommuom_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(9, 122);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Supplier Name";
		this.btnWithoutPO.AutoSize = true;
		this.btnWithoutPO.Location = new System.Drawing.Point(1210, 488);
		this.btnWithoutPO.Name = "btnWithoutPO";
		this.btnWithoutPO.Size = new System.Drawing.Size(42, 20);
		this.btnWithoutPO.TabIndex = 18;
		this.btnWithoutPO.TabStop = true;
		this.btnWithoutPO.Text = "No";
		this.btnWithoutPO.UseVisualStyleBackColor = true;
		this.btnWithoutPO.Visible = false;
		this.btnWithoutPO.CheckedChanged += new System.EventHandler(btnWithoutPO_CheckedChanged);
		this.txtManualBillNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualBillNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualBillNo.Location = new System.Drawing.Point(113, 144);
		this.txtManualBillNo.Name = "txtManualBillNo";
		this.txtManualBillNo.Size = new System.Drawing.Size(219, 23);
		this.txtManualBillNo.TabIndex = 5;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(731, 66);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance250;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance255).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance256).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(113, 116);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.cmbCommType.AllowNull = (DefaultableBoolean)1;
		this.cmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance262;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance267).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance268).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance269).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommType).Location = new System.Drawing.Point(636, 60);
		((System.Windows.Forms.Control)(object)this.cmbCommType).Name = "cmbCommType";
		((System.Windows.Forms.Control)(object)this.cmbCommType).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommType).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.cmbCommType).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.CmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance274;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance280).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Location = new System.Drawing.Point(636, 32);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Name = "CmbCommAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Leave += new System.EventHandler(CmbCommAgent_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(216, 89);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(116, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.txtCommRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRemarks.Location = new System.Drawing.Point(636, 117);
		this.txtCommRemarks.Name = "txtCommRemarks";
		this.txtCommRemarks.Size = new System.Drawing.Size(247, 50);
		this.txtCommRemarks.TabIndex = 22;
		this.txtCommRemarks.Text = "";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(551, 38);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(551, 121);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(9, 66);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1691, 185);
		this.panel4.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1196, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.btnFrmRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnSlipDetail1510, this.btnSlip1510A,
			this.btnLoadPo
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1196, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnFrmRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnLoadPo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadPo.Name = "btnLoadPo";
		this.btnLoadPo.Size = new System.Drawing.Size(56, 24);
		this.btnLoadPo.Text = "Load &PO";
		this.btnLoadPo.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(97, 24);
		this.btnPrint.Text = "103-&Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSlipDetail1510.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlipDetail1510.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipDetail1510.Name = "btnSlipDetail1510";
		this.btnSlipDetail1510.Size = new System.Drawing.Size(109, 24);
		this.btnSlipDetail1510.Text = "1510-&SlipDetail";
		this.btnSlipDetail1510.Click += new System.EventHandler(btnSlipDetail1510_Click);
		this.btnSlip1510A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip1510A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip1510A.Name = "btnSlip1510A";
		this.btnSlip1510A.Size = new System.Drawing.Size(117, 24);
		this.btnSlip1510A.Text = "1510A-Party Slip";
		this.btnSlip1510A.Click += new System.EventHandler(btnSlip1510A_Click);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1202, 704);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add(this.panel10);
		this.panel15.Controls.Add(this.panel9);
		this.panel15.Controls.Add(this.panel7);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 59);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1196, 642);
		this.panel15.TabIndex = 30;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.DynamicFiltering = true;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1196, 198);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 46;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Dock = System.Windows.Forms.DockStyle.Top;
		this.label28.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(0, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(1196, 24);
		this.label28.TabIndex = 45;
		this.label28.Text = "Detail Information";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1196, 346);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 44;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 31);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1196, 28);
		this.panel14.TabIndex = 29;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(254, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "PURCHASE INVOICE HISTORY";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1196, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.BtnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1196, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(51, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1155, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1156, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar3.TabIndex = 47;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1154, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel7.Controls.Add(this.groupBox4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1196, 74);
		this.panel7.TabIndex = 48;
		this.groupBox4.Controls.Add(this.btnShow);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory);
		this.groupBox4.Controls.Add(this.label67);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox4.Controls.Add(this.label63);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label64);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.label65);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.label66);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.label68);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Controls.Add(this.label69);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(7, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(975, 67);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(913, 35);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 24);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbDeliveryTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Location = new System.Drawing.Point(791, 34);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Name = "CmbDeliveryTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).TabIndex = 6;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(791, 15);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(80, 15);
		this.label67.TabIndex = 16;
		this.label67.Text = "Delivery Term";
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(669, 34);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 5;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(669, 15);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(84, 15);
		this.label63.TabIndex = 14;
		this.label63.Text = "Payment Term";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance287).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance288).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(459, 34);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(461, 15);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(51, 15);
		this.label64.TabIndex = 12;
		this.label64.Text = "Supplier";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(366, 36);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(368, 15);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(61, 15);
		this.label65.TabIndex = 10;
		this.label65.Text = "To DocNo";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(274, 36);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(276, 15);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(76, 15);
		this.label66.TabIndex = 8;
		this.label66.Text = "From DocNo";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(140, 36);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(132, 23);
		this.ToDateHistory.TabIndex = 1;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(140, 15);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(48, 15);
		this.label68.TabIndex = 7;
		this.label68.Text = "To Date";
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 36);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(132, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(6, 15);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(63, 15);
		this.label69.TabIndex = 5;
		this.label69.Text = "From Date";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 74);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1196, 346);
		this.panel9.TabIndex = 49;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Controls.Add(this.label28);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 420);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1196, 222);
		this.panel10.TabIndex = 50;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 24);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1196, 198);
		this.panel13.TabIndex = 48;
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1210, 730);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceDirect_St";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseInvoiceDirect_St_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.tabDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		this.tabFreight.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.tabOtherItemExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.tabPartyAddLess.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
