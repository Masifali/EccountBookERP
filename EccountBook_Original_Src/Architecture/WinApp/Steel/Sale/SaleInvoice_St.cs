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

namespace Architecture.WinApp.Steel.Sale;

public class SaleInvoice_St : Form
{
	private Attachment AT = new Attachment();

	private int[] AryColWidth;

	private string OrderType = "";

	public DataTable dtGPGRNS = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int GRNID;

	private int GPId;

	private int VoucherHeadId;

	private int Id;

	private bool Approved;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private double SupplierTransporterDebitAmount;

	private double SupplierCommissionAmount;

	private int ItemId;

	private int SOID;

	private int DefaultDaysToLessFromHistoryFromDate;

	public static int SupplierId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public string GRNS = string.Empty;

	private UserAccount UserAccount = new UserAccount();

	private frmLoadGDN_St LoadGDN;

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

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Panel panel4;

	private Panel panel5;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label2;

	private Label label8;

	private TextBox txtRefNo;

	private Label label6;

	private Label label12;

	private TextBox txtBillNo;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtDocNo;

	private CtrlGrdBar ctrlGrdBar1;

	private Timer timer1;

	private Panel panel12;

	internal GridEX grd;

	private Panel panel7;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grdFreight;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbstockprty;

	internal UltraCombo cmbrefprty;

	internal UltraCombo CmbSupplier;

	internal UltraCombo CmbCommAgent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Panel panel13;

	private ToolStripButton toolStripButton1;

	internal GridEX grdHistory;

	private ToolStrip toolStrip2;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker DueDate;

	private Label label16;

	private CheckBox ChkBok;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbCommType;

	internal UltraCombo CmbCommUom;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSlipFormat2;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdDetail;

	private CheckBox ChkPrintSlip;

	private Panel panel17;

	private Label label23;

	private Panel panel18;

	private Label label5;

	internal UltraCombo CmbBranch;

	internal UltraCombo CmbProject;

	private Label label13;

	private Label label18;

	internal UltraCombo CmbPaymentTerm;

	private Label label31;

	private TextBox txtExchangeRate;

	private Label label57;

	internal UltraCombo cmbCurrency;

	private Label label56;

	private TextBox txtDeliveryTerm;

	private Label label24;

	private TextBox txtTotalQty;

	private Label label60;

	private TextBox txtTotalWeight;

	private TextBox txtFcyAmount;

	private Label label58;

	private Label label27;

	private RichTextBox txtCommRemarks;

	private RichTextBox txtRemarks;

	private Panel panel14;

	private Label label45;

	private CtrlGrdBar ctrlGrdBar4;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private Label label28;

	private TabPage tabPage2;

	private Label label29;

	private TabPage tabPage3;

	private Label label30;

	private Label label1;

	private Label label9;

	private GroupBox groupBox2;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShow;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel3;

	private Label label49;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	internal UltraCombo CmbPaymentTermHistory;

	internal UltraCombo CmbDeliveryTermHistory;

	private Label label61;

	private Label label67;

	public SaleInvoice_St(UserAccount user)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 120;
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
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.stringFormatsingle;
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
			grdFreight.RootTable.Columns["InvGdnId"].Visible = false;
			grdFreight.RootTable.Columns["Debit"].Visible = false;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 120;
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
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.stringFormatsingle;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				if (Conversion.ToInt(grd.CurrentRow.Cells["SaleOrder"].Value) == 0)
				{
					grd.RootTable.Columns["SaleOrder"].Visible = false;
				}
				if (Conversion.ToDouble(grd.CurrentRow.Cells["WtCut"].Value) == 0.0)
				{
					grd.RootTable.Columns["WtCut"].Visible = false;
				}
				if (Conversion.ToDouble(grd.CurrentRow.Cells["WtCutTotal"].Value) == 0.0)
				{
					grd.RootTable.Columns["WtCutTotal"].Visible = false;
				}
			}
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["InvGdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["InvGdnDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrder"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCut"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLessWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["CityId"].EditType = (EditType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCutAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Freights"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["Journal"].EditType = (EditType)0;
			grd.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCut"].EditType = (EditType)1;
			if (SOID == 0)
			{
				grd.RootTable.Columns["ItemRate"].EditType = (EditType)1;
				grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
				grd.RootTable.Columns["RateUom"].Visible = false;
				grd.RootTable.Columns["EquivalentSoRate"].Visible = true;
				grd.RootTable.Columns["EquivalentSoRate"].EditType = (EditType)1;
				grd.RootTable.Columns["EquivalentSoRate"].Caption = "Rate Uom";
			}
			else
			{
				grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
				grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
				grd.RootTable.Columns["EquivalentSoRate"].EditType = (EditType)1;
				grd.RootTable.Columns["EquivalentSoRate"].Visible = false;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvGdnId"].Visible = false;
			grd.RootTable.Columns["InvGdnDetailId"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["Journal"].Visible = false;
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
			grd.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.##";
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
			grd.RootTable.Columns["EquivalentSoRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EquivalentSoRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		try
		{
			dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
			grdGLedger.DataSource = dtGrdGL;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		try
		{
			dtInvExp.Rows.Add(0, 0, 0, 0, "");
			grdInvExp.DataSource = dtInvExp;
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
			dtFreight.Rows.Add(GRNID, 0, 0, 0);
			grdFreight.DataSource = dtFreight;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
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
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
			}
			grdFreight.UpdateData();
			FreightProportion();
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
			FreightProportion();
			BillAmount();
			grdFreight.UpdateData();
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
			ExpProportion();
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
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate;
					item.Cells["Amount"].Value = Amount;
				}
			}
			else if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
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
			if (e.Column.Key == "AccountId" && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
			{
				MessageBox.Show("Customer Account Not select");
				item.Cells["AccountId"].Value = 0;
			}
			else
			{
				grdGLedger.UpdateData();
				BillAmount();
				LedgerProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "RateCut" || e.Column.Key == "ItemRate" || e.Column.Key == "EquivalentSoRate") && item != null && (int)item.RowType == 0)
			{
				string obj = item.Cells["NetBillWeight"].Value.ToString();
				string RateCut = item.Cells["RateCut"].Value.ToString();
				string Rate = item.Cells["ItemRate"].Value.ToString();
				double EquivalentSoRate = Conversion.ToDouble((object)item.Cells["EquivalentSoRate"].Text.ToString());
				double RateCutAmount = Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)EquivalentSoRate) * Conversion.ToDouble((object)RateCut);
				item.Cells["RateCutAmount"].Value = RateCutAmount;
				double ItemAmount = Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)EquivalentSoRate) * Conversion.ToDouble((object)Rate);
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount - RateCutAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
				{
					item.Cells["FcyAmount"].Value = Math.Round(Conversion.ToDecimal(item.Cells["ItemAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
				}
			}
			grd.UpdateData();
			BillAmount();
			CalculateTotalInformation();
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
				Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoiceSteelSlip_1514(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 1509);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("SaleOrder", typeof(int));
			dt.Columns.Add("ItemName");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("PackUom");
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("WeightCut", typeof(double));
			dt.Columns.Add("WeightCutTotal", typeof(double));
			dt.Columns.Add("AddLessWeight", typeof(double));
			dt.Columns.Add("NetBillWeight", typeof(double));
			dt.Columns.Add("NetStockWeight", typeof(double));
			dt.Columns.Add("ItemRate", typeof(double));
			dt.Columns.Add("RateUom");
			dt.Columns.Add("RateCut", typeof(double));
			dt.Columns.Add("RateCutAmount", typeof(double));
			dt.Columns.Add("ItemAmount", typeof(double));
			dt.Columns.Add("WareHouse");
			dt.Columns.Add("CityName");
			dt.Columns.Add("GpDate", typeof(DateTime));
			dt.Columns.Add("GpNo");
			dt.Columns.Add("VehicleNo");
			dt.Columns.Add("ExchangeRate", typeof(double));
			dt.Columns.Add("FcyAmount", typeof(double));
			dt.Columns.Add("BillAmount", typeof(double));
			dt.Columns.Add("CommAmount", typeof(double));
			dt.Columns.Add("ExpenseAmount", typeof(double));
			dt.Columns.Add("FreightAmount", typeof(double));
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dt.Rows.Add(obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].PackingType, obj.InvSaleInvoiceDetaillist[i].UOMCode, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].WeightCut, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].AdLsWeight, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].RateUom, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].CityName, obj.InvSaleInvoiceDetaillist[i].GpDate.ToShortDateString(), obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].ExchangeRate, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].ItemNetAmount, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount);
				}
				grdDetail.DataSource = dt;
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
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
			ctrlGrdBar4_Load(null, null);
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
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0)
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
			double TransDifference = TransporterCredit;
			if (TransDifference > 0.0)
			{
				BillAmount -= Math.Abs(TransDifference);
			}
			double GlDifference = SupplierGLCreditAmount - SupplierGLDebitAmount;
			double DiffGl = Math.Abs(GlDifference);
			BillAmount = ((!(GlDifference < 0.0)) ? (BillAmount + GlDifference) : (BillAmount - DiffGl));
			if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCommAgent.Value))
			{
				if (txtcommrate.Text != string.Empty)
				{
					BillAmount -= Conversion.ToDouble((object)txtcommamount.Text.Trim());
					SupplierCommissionAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
				}
				else
				{
					txtcommamount.Text = "0";
				}
			}
			else
			{
				SupplierCommissionAmount = 0.0;
			}
			txtBillAmount.Text = BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbBranch).ActiveRow == null)
		{
			MessageBox.Show("Branch Name  field is Required");
			CmbBranch.Focus();
			return false;
		}
		if (((UltraGridBase)CmbProject).ActiveRow == null)
		{
			MessageBox.Show("Project Name  field is Required");
			CmbProject.Focus();
			return false;
		}
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == null || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo  field is Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplier).ActiveRow == null)
		{
			MessageBox.Show("Party Name field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && (txtDueDays.Text.Trim() == string.Empty || txtDueDays.Text.Trim() == "0" || txtDueDays.Text.Trim() == null))
		{
			MessageBox.Show("Due Days Field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (txtRemarks.Text == null || txtRemarks.Text == string.Empty || txtRemarks.Text == "0")
		{
			MessageBox.Show("Invoice Remarks Field is Required");
			txtRemarks.Focus();
			return false;
		}
		if (txtBillAmount.Text.Trim() == string.Empty || txtBillAmount.Text.Trim() == "0" || txtBillAmount.Text.Trim() == null)
		{
			MessageBox.Show("Bill Amount Field is Required");
			txtBillAmount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCurrency).ActiveRow == null)
		{
			MessageBox.Show("Currency is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (txtExchangeRate.Text == string.Empty || txtExchangeRate.Text == "0")
		{
			MessageBox.Show("Exchange Rate is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0" || txtFcyAmount.Text.Trim() == null)
		{
			MessageBox.Show("Fcy Amount Should Greater Than 0");
			return false;
		}
		return true;
	}

	private void defaultConfiquration()
	{
		try
		{
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

	private void BranchFill()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, CmbBranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)CmbBranch).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbBranch).Rows[0].Activate();
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtSupplier = SupplierCustomer.Getall(supplier);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, CmbSupplier, "Id", "CompanyName", "Supplier Name", false);
				DDL.BindDDLNew(dtSupplier, cmbstockprty, "Id", "CompanyName", "Supplier Name", false);
				DDL.BindDDLNew(dtSupplier, cmbrefprty, "Id", "CompanyName", "Refrence Party", false);
				DDL.BindDDLNew(dtSupplier, CmbCommAgent, "Id", "CompanyName", "Commission Agent", false);
				((UltraGridBase)CmbCommAgent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Uom");
			dt.Rows.Add(1, "1");
			dt.Rows.Add(2, "5");
			dt.Rows.Add(3, "10");
			dt.Rows.Add(4, "25");
			dt.Rows.Add(5, "40");
			dt.Rows.Add(6, "50");
			dt.Rows.Add(7, "60");
			dt.Rows.Add(8, "65");
			dt.Rows.Add(9, "80");
			dt.Rows.Add(10, "100");
			((UltraGridBase)CmbCommUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dt, CmbCommUom, "Id", "Uom", "UOM", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add(3, "Weight");
			DDL.BindDDL(dt, CmbCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)CmbCommType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dtAccountlst = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
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

	private void DocumentNo()
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
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1509,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)CmbCommType).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					txtcommamount.Text = CommRate.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else if (((Control)(object)CmbCommType).Text == "Percent" || ((Control)(object)CmbCommType).Text == "Percentage")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt = Math.Round(Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
						txtcommamount.Text = CommAmt.ToString();
					}
				}
				else if (((Control)(object)CmbCommType).Text == "Weight")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)CmbCommUom).Text.Trim());
						double CommAmt2 = num / UOM * CommRate3;
						txtcommamount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount).ToString();
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
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 95);
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 1509);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1300, 750);
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			AccountsFill();
			OtherItemsBind();
			dtGPGRNS.Columns.Add("Id");
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("InvGdnId");
			dtGrid.Columns.Add("InvGdnDetailId");
			dtGrid.Columns.Add("SaleOrderId");
			dtGrid.Columns.Add("SaleOrderDetailId");
			dtGrid.Columns.Add("SaleOrder");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("PackUomId");
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("WtCut", typeof(double));
			dtGrid.Columns.Add("WtCutTotal", typeof(double));
			dtGrid.Columns.Add("AddLessWeight", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUomId");
			dtGrid.Columns.Add("RateUom");
			dtGrid.Columns.Add("EquivalentSoRate", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("WarehouseId");
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("CityId");
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("ExchangeRate", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage");
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Columns.Add("InvGdnId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			BranchFill();
			ProjectFill();
			DocumentNo();
			SupplierNameFilll();
			PaymentTerms();
			CommissionTypeFill();
			CommissionUOMFill();
			CurrencyFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			CmbSupplier.Focus();
			defaultConfiquration();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryComboFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridAgainstGP(string IDs)
	{
		try
		{
			DataTable invGdn = InvSaleInvoice.GdnLoadForSaleInvoice(IDs);
			if (invGdn.Rows.Count > 0)
			{
				((Control)(object)CmbSupplier).Enabled = false;
				CmbSupplier.Value = invGdn.Rows[0]["SupplierCustomerId"];
				CmbCommAgent.Value = invGdn.Rows[0]["CommissionAgentId"];
				((Control)(object)CmbCommType).Text = string.Empty;
				((Control)(object)CmbCommType).Text = invGdn.Rows[0]["CommissionType"].ToString();
				txtcommrate.Text = invGdn.Rows[0]["CommRate"].ToString();
				((Control)(object)CmbCommUom).Text = string.Empty;
				((Control)(object)CmbCommUom).Text = invGdn.Rows[0]["CommRateUom"].ToString();
				txtcommamount.Text = invGdn.Rows[0]["CommAmount"].ToString();
				txtCommRemarks.Text = invGdn.Rows[0]["CommRemarks"].ToString();
				txtDueDays.Text = invGdn.Rows[0]["OrderDueDays"].ToString();
				DueDate.Value = Conversion.ToDateTime(invGdn.Rows[0]["OrderDueDate"]);
				OrderType = Conversion.ToString(invGdn.Rows[0]["OrderType"]);
				if (OrderType == "GateSale")
				{
					txtDeliveryTerm.Text = "Load";
				}
				else
				{
					txtDeliveryTerm.Text = Conversion.ToString(invGdn.Rows[0]["DeliveryTerm"]);
				}
				txtRemarks.Text = invGdn.Rows[0]["RemarksHeader"].ToString();
				for (int i = 0; i < invGdn.Rows.Count; i++)
				{
					dtGrid.Rows.Add(0, invGdn.Rows[i]["InvGdnId"], invGdn.Rows[i]["GdnDetailId"], invGdn.Rows[i]["SaleOrderId"], invGdn.Rows[i]["SaleOrderDetailId"], Conversion.ToInt(invGdn.Rows[i]["OrderNo"]), invGdn.Rows[i]["ItemId"], invGdn.Rows[i]["ItemName"], invGdn.Rows[i]["JobLotId"], invGdn.Rows[i]["JobLotDescription"], invGdn.Rows[i]["PackingTypeId"], invGdn.Rows[i]["PackTypeDesc"], invGdn.Rows[i]["ItemUomId"], invGdn.Rows[i]["UOMCodeItem"], invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["GrossWeight"], invGdn.Rows[i]["WtCut"], invGdn.Rows[i]["WtCutTotal"], invGdn.Rows[i]["AdLsWeight"], invGdn.Rows[i]["NetBillWeight"], invGdn.Rows[i]["StockWeight"], invGdn.Rows[i]["OrderItemRate"], invGdn.Rows[i]["OrderItemRateUOMId"], invGdn.Rows[i]["RateUom"], invGdn.Rows[i]["EquivalentSoRate"], 0, 0, invGdn.Rows[i]["ItemAmount"], invGdn.Rows[i]["WarehouseId"], invGdn.Rows[i]["WareHouseName"], invGdn.Rows[i]["CityId"], invGdn.Rows[i]["CityName"], invGdn.Rows[i]["GpNo"], invGdn.Rows[i]["VehicleNo"], 0, (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Conversion.ToDecimal((object)txtExchangeRate.Text) : 0m, (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Math.Round(Conversion.ToDecimal(invGdn.Rows[i]["ItemAmount"]) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount) : 0m, 0, 0, 0, 0);
					ItemId = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
					SOID = Conversion.ToInt(grd.CurrentRow.Cells["SaleOrderId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadFreightData(string ID)
	{
		DataTable item = InvSaleInvoice.GetTransporterAndFreightFromGdn(ID);
		if (item.Rows.Count > 0)
		{
			for (int i = 0; i < item.Rows.Count; i++)
			{
				dtFreight.Rows.Add(item.Rows[i]["MainId"], item.Rows[i]["Transporter"], item.Rows[i]["CarriageAmount"], 0);
			}
		}
	}

	private void LoadExpData(string ID)
	{
		DataTable item = InvSaleInvoice.GetItemQtyAndItemRateFromSO(ID);
		if (item.Rows.Count > 0)
		{
			for (int i = 0; i < item.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(0, item.Rows[i]["ItemQty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["BagPrice"]), 0);
			}
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			dtGrid.Rows.Clear();
			string GdnIds = "";
			for (int i = 0; i < LoadGDN.dtGDNS.Rows.Count; i++)
			{
				GdnIds = GdnIds + "," + int.Parse(LoadGDN.dtGDNS.Rows[i][0].ToString());
			}
			LoadDataDetailGridAgainstGP(GdnIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtFreight.Clear();
			LoadFreightData(GdnIds);
			if (dtFreight.Rows.Count == 0 || dtFreight == null)
			{
				AddRowInFreightGrid();
			}
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtInvExp.Clear();
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.RetrieveStructure();
			TotalCommissionAmount();
			grdInvExpSettings();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			LedgerProportion();
			BillProportion();
			BillAmount();
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Expense"].Value = (TotalAmount / NetWeight * Weight).ToString();
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
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Freights"].Value = Math.Round(CreditForFreight) / NetWeight * Weight;
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

	private void LedgerProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Credit"], (AggregateFunction)2).ToString()) + TotalAmount;
			double DebitForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Debit"], (AggregateFunction)2).ToString());
			if (DebitForFreight > CreditForFreight)
			{
				double TotalDiffernce = DebitForFreight - CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Journal"].Value = TotalDiffernce / NetWeight * Weight;
				}
			}
			else
			{
				if (DebitForFreight > CreditForFreight)
				{
					double TotalDiffernce3 = DebitForFreight - CreditForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Journal"].Value = TotalDiffernce3;
					}
				}
				if (DebitForFreight == CreditForFreight)
				{
					double TotalDiffernce2 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj3 in rows)
					{
						obj3.BeginEdit();
						obj3.Cells["Journal"].Value = TotalDiffernce2;
					}
				}
				if (DebitForFreight < CreditForFreight)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj4 in rows)
					{
						obj4.BeginEdit();
						obj4.Cells["Journal"].Value = 0;
					}
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
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString());
					}
					else
					{
						r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) - Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) - Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
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
			double CommPercent = Conversion.ToDouble((object)txtcommrate.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)CmbCommType).Text == "Percent")
					{
						double ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
						r.Cells["Commission"].Value = ItemAmount * CommPercent / 100.0;
						r.EndEdit();
					}
					else
					{
						double Weight = Conversion.ToDouble(r.Cells["NetBillWeight"].Value);
						r.Cells["Commission"].Value = (TotalAmount / NetWeight * Weight).ToString();
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			Historyfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
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
			DataTable dtAllData = InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel(new ReportsParameters
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
				if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "Customer")
				{
					dtsup.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "PaymentTerm")
				{
					dtPayTerm.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "DeliveryTerm")
				{
					dtDelTerm.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
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

	private void Historyfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters SI = new ReportsParameters();
			SI.OrganizationId = UserAccount.OrganizationId;
			SI.CompanyId = UserAccount.CompanyId;
			SI.BranchesId = UserAccount.BranchesId;
			SI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			SI.DocumentTypeId = 1509;
			SI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!SI.CanViewAllRecord)
			{
				SI.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			SI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			SI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			SI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			SI.PaymenetTermId = Conversion.ToInt(CmbPaymentTermHistory.Value);
			SI.DeliveryTerm = ((Control)(object)CmbDeliveryTermHistory).Text;
			DataTable dthistory = InvSaleInvoice.FormHistory(SI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("CommAgent");
				dt.Columns.Add("CommType");
				dt.Columns.Add("CommRate", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CommRemarks");
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("TotalWeight", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("UserName");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["SupplierName"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommType"], dthistory.Rows[i]["CommRate"], Conversion.ToDouble(dthistory.Rows[i]["CommAmount"]), dthistory.Rows[i]["CommRemarks"], dthistory.Rows[i]["TotalQty"], dthistory.Rows[i]["TotalWeight"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["ExchangeRate"], dthistory.Rows[i]["FcyAmount"], dthistory.Rows[i]["UserName"], Conversion.ToDateTime(dthistory.Rows[i]["EntryDate"]).ToShortDateString(), dthistory.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dthistory.Rows[i]["ModifyDate"]).ToShortDateString(), Conversion.ToBool(dthistory.Rows[i]["IsApproved"]) ? "Approved" : "Not Approved", dthistory.Rows[i]["ApprovedUserName"], Conversion.ToDateTime(dthistory.Rows[i]["ApprovedDate"]).ToShortDateString(), dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"]);
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["CommType"].Visible = false;
			grdHistory.RootTable.Columns["CommRemarks"].Visible = false;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Slip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Slip";
			grdHistory.RootTable.Columns["View"].Width = 50;
			grdHistory.RootTable.Columns["View"].Position = 1;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 70;
			grdHistory.RootTable.Columns["Voucher"].Position = 2;
			grdHistory.FrozenColumns = 3;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar3_Load(null, null);
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
			OrderType = "";
			ItemId = 0;
			SOID = 0;
			Id = 0;
			Approved = false;
			((Control)(object)CmbSupplier).Enabled = true;
			SupplierGLCreditAmount = 0.0;
			SupplierGLDebitAmount = 0.0;
			SupplierTransporterDebitAmount = 0.0;
			SupplierCommissionAmount = 0.0;
			((Control)(object)CmbSupplier).Enabled = true;
			((Control)(object)CmbCommAgent).Text = string.Empty;
			((Control)(object)CmbCommType).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)CmbCommUom).Text = string.Empty;
			txtCommRemarks.Text = string.Empty;
			txtBillNo.Text = string.Empty;
			txtRefNo.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			((Control)(object)cmbCurrency).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtTotalQty.Text = string.Empty;
			txtTotalWeight.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			dtGrdGL.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Rows.Clear();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Rows.Clear();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			BranchFill();
			ProjectFill();
			SupplierNameFilll();
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			CmbSupplier.Focus();
			dtGPGRNS.Rows.Clear();
			defaultConfiquration();
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
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(Id);
			CmbBranch.Value = invSI.BranchesId;
			CmbProject.Value = invSI.ProjectsId;
			txtDocNo.Text = invSI.DocNo.ToString();
			DocDate.Value = invSI.DocDate;
			tabControl1.SelectedIndex = 0;
			CmbSupplier.Value = invSI.SupplierCustomerId;
			((Control)(object)CmbSupplier).Enabled = false;
			txtRefNo.Text = invSI.SupplierReferenceNo.ToString();
			txtBillNo.Text = invSI.ManualBillNo;
			CmbPaymentTerm.Value = invSI.PaymentTermsId;
			txtDueDays.Text = Conversion.ToString((object)invSI.DueDays);
			DueDate.Value = Conversion.ToDateTime((object)invSI.DueDate);
			txtDeliveryTerm.Text = invSI.DeliveryTerm;
			cmbCurrency.Value = invSI.CurrencyId;
			txtExchangeRate.Text = invSI.ExchangeRate.ToString(clsGlobalVariables.DecimalRateFormate);
			txtTotalQty.Text = invSI.TotalQty.ToString("#,##0.##");
			txtTotalWeight.Text = invSI.TotalWeight.ToString("#,##0.##");
			txtFcyAmount.Text = invSI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtBillAmount.Text = invSI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			CmbCommAgent.Value = invSI.CommAgentId;
			((Control)(object)CmbCommType).Text = Conversion.ToString((object)invSI.CommType);
			txtcommrate.Text = invSI.CommRate.ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)CmbCommUom).Text = invSI.CommUOM.ToString();
			txtcommamount.Text = invSI.CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtCommRemarks.Text = Conversion.ToString((object)invSI.CommRemarks);
			txtRemarks.Text = Conversion.ToString((object)invSI.RemarksHeader);
			Approved = invSI.IsApproved;
			OrderType = ((invSI.InvoiceTypeId == 1) ? "SaleOrder" : ((invSI.InvoiceTypeId == 2) ? "DeliveryOrder" : ((invSI.InvoiceTypeId == 3) ? "GateSale" : "")));
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int l = 0; l < invSI.InvSaleInvoiceDetaillist.Count; l++)
			{
				dtGrid.Rows.Add(invSI.InvSaleInvoiceDetaillist[l].Id, invSI.InvSaleInvoiceDetaillist[l].InvGdnId, invSI.InvSaleInvoiceDetaillist[l].InvGdnDetailId, invSI.InvSaleInvoiceDetaillist[l].SaleOrderId, invSI.InvSaleInvoiceDetaillist[l].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[l].SaleOrder, invSI.InvSaleInvoiceDetaillist[l].ItemId, invSI.InvSaleInvoiceDetaillist[l].ItemName, invSI.InvSaleInvoiceDetaillist[l].JobLotId, invSI.InvSaleInvoiceDetaillist[l].JobLotDescription, invSI.InvSaleInvoiceDetaillist[l].PackingTypeId, invSI.InvSaleInvoiceDetaillist[l].PackingType, invSI.InvSaleInvoiceDetaillist[l].PackUOMId, invSI.InvSaleInvoiceDetaillist[l].UOMCode, invSI.InvSaleInvoiceDetaillist[l].ItemQty, invSI.InvSaleInvoiceDetaillist[l].GrossWeight, invSI.InvSaleInvoiceDetaillist[l].WeightCut, invSI.InvSaleInvoiceDetaillist[l].WeightCutTotal, invSI.InvSaleInvoiceDetaillist[l].AdLsWeight, invSI.InvSaleInvoiceDetaillist[l].NetBillWeight, invSI.InvSaleInvoiceDetaillist[l].NetStockWeight, invSI.InvSaleInvoiceDetaillist[l].ItemRate, invSI.InvSaleInvoiceDetaillist[l].RateUomScheduleId, invSI.InvSaleInvoiceDetaillist[l].RateUom, invSI.InvSaleInvoiceDetaillist[l].EquivalentSoRate, invSI.InvSaleInvoiceDetaillist[l].RateCut, invSI.InvSaleInvoiceDetaillist[l].RateCutAmount, invSI.InvSaleInvoiceDetaillist[l].ItemAmount, invSI.InvSaleInvoiceDetaillist[l].WarehouseId, invSI.InvSaleInvoiceDetaillist[l].WareHouseName, invSI.InvSaleInvoiceDetaillist[l].CityId, invSI.InvSaleInvoiceDetaillist[l].CityName, invSI.InvSaleInvoiceDetaillist[l].GpNo, invSI.InvSaleInvoiceDetaillist[l].VehicleNo, invSI.InvSaleInvoiceDetaillist[l].ItemNetAmount, invSI.InvSaleInvoiceDetaillist[l].ExchangeRate, invSI.InvSaleInvoiceDetaillist[l].FcyAmount, invSI.InvSaleInvoiceDetaillist[l].FreightAmount, invSI.InvSaleInvoiceDetaillist[l].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[l].CommissionAmount, invSI.InvSaleInvoiceDetaillist[l].JournalAmount);
				ItemId = Conversion.ToInt(grd.CurrentRow.Cells[3].Value);
				SOID = Conversion.ToInt(grd.CurrentRow.Cells["SaleOrderId"].Value);
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invSI.InvSaleInvoiceExpenseList.Count; k++)
			{
				dtInvExp.Rows.Add(invSI.InvSaleInvoiceExpenseList[k].InvOtherItemId, invSI.InvSaleInvoiceExpenseList[k].Qty, invSI.InvSaleInvoiceExpenseList[k].Rate, invSI.InvSaleInvoiceExpenseList[k].Amount, invSI.InvSaleInvoiceExpenseList[k].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invSI.InvSaleInvoiceJournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(invSI.InvSaleInvoiceJournalList[j].ChartofAccountId, invSI.InvSaleInvoiceJournalList[j].JvRemarks, invSI.InvSaleInvoiceJournalList[j].JvPrcnt, invSI.InvSaleInvoiceJournalList[j].JvQty, invSI.InvSaleInvoiceJournalList[j].JvRate, invSI.InvSaleInvoiceJournalList[j].JvDebit, invSI.InvSaleInvoiceJournalList[j].JvCredit);
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < invSI.InvSaleInvoiceFreightList.Count; i++)
			{
				dtFreight.Rows.Add(invSI.InvSaleInvoiceFreightList[i].InvGdnId, invSI.InvSaleInvoiceFreightList[i].TansporterId, invSI.InvSaleInvoiceFreightList[i].CreditAmount, invSI.InvSaleInvoiceFreightList[i].DebitAmount);
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
			grdFreightSettings();
			grdInvExpSettings();
			gridGLSettings();
			TotalCommissionAmount();
			BillAmount();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ExpProportion();
			LedgerProportion();
			FreightProportion();
			CommissionProportion();
			BillProportion();
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
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e32: Expected O, but got Unknown
		//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f23: Expected O, but got Unknown
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Expected O, but got Unknown
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
			InvSaleInvoice saleInvoice = new InvSaleInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				saleInvoice.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			TotalCommissionAmount();
			BillAmount();
			FreightProportion();
			ExpProportion();
			CommissionProportion();
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToDouble(r4.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r4.Cells["Transporter"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against Freight First");
					return;
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
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(CmbCommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				return;
			}
			saleInvoice.BranchesId = Conversion.ToInt(CmbBranch.Value);
			saleInvoice.ProjectsId = Conversion.ToInt(CmbProject.Value);
			saleInvoice.DocDate = DocDate.Value;
			saleInvoice.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			saleInvoice.DocumentTypeId = 1509;
			saleInvoice.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			saleInvoice.SupplierReferenceNo = Conversion.ToInt((object)txtRefNo.Text.Trim());
			saleInvoice.ManualBillNo = txtBillNo.Text.Trim();
			saleInvoice.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
			saleInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text.Trim());
			saleInvoice.DueDate = DueDate.Value;
			saleInvoice.DeliveryTerm = txtDeliveryTerm.Text.Trim();
			saleInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			saleInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
			saleInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text.Trim());
			saleInvoice.TotalQty = Conversion.ToDecimal((object)txtTotalQty.Text.Trim());
			saleInvoice.TotalWeight = Conversion.ToDecimal((object)txtTotalWeight.Text.Trim());
			saleInvoice.CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			saleInvoice.CommAgentId = Conversion.ToInt(CmbCommAgent.Value);
			saleInvoice.CommRemarks = txtCommRemarks.Text.Trim();
			saleInvoice.CommUOM = Conversion.ToInt((object)((Control)(object)CmbCommUom).Text.Trim());
			saleInvoice.CommType = ((Control)(object)CmbCommType).Text.Trim().ToString();
			saleInvoice.CommRate = Conversion.ToDecimal((object)txtcommrate.Text);
			saleInvoice.RemarksHeader = txtRemarks.Text.Trim();
			saleInvoice.BillAmount = Conversion.ToDecimal((object)txtBillAmount.Text.Trim());
			saleInvoice.OrganizationId = UserAccount.OrganizationId;
			saleInvoice.CompanyId = UserAccount.CompanyId;
			saleInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			saleInvoice.EntryDate = DateTime.Now;
			saleInvoice.EntryUserId = UserAccount.ID;
			saleInvoice.ModifyDate = DateTime.Now;
			saleInvoice.ModifyUserId = UserAccount.ID;
			saleInvoice.ApprovedDate = DateTime.Now;
			saleInvoice.DeliverystartDate = DateTime.Now;
			saleInvoice.ExpiryDate = DateTime.Now;
			saleInvoice.InvoiceTypeId = ((OrderType == "SaleOrder") ? 1 : ((OrderType == "DeliveryOrder") ? 2 : ((OrderType == "GateSale") ? 3 : 0)));
			saleInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			rows = grd.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				InvSaleInvoiceDetail Pd = new InvSaleInvoiceDetail();
				Pd.LineId = r7.RowIndex + 1;
				Pd.Id = Conversion.ToInt(r7.Cells["Id"].Value);
				Pd.InvGdnId = Conversion.ToInt(r7.Cells["InvGdnId"].Value);
				Pd.InvGdnDetailId = Conversion.ToInt(r7.Cells["InvGdnDetailId"].Value);
				if (Pd.InvGdnId == 0)
				{
					MessageBox.Show("GdnId not found");
					return;
				}
				if (Pd.InvGdnDetailId == 0)
				{
					MessageBox.Show("GdnDetail Id not found");
					return;
				}
				if (SOID > 0)
				{
					Pd.SaleOrderId = Conversion.ToInt(r7.Cells["SaleOrderId"].Value);
					if (Pd.SaleOrderId == 0)
					{
						MessageBox.Show("SaleOrderId Field Required");
						return;
					}
					Pd.SaleOrderDetailId = Conversion.ToInt(r7.Cells["SaleOrderDetailId"].Value);
					if (Pd.SaleOrderId == 0)
					{
						MessageBox.Show("SaleOrderDetailId Field Required");
						return;
					}
				}
				Pd.ItemId = Conversion.ToInt(r7.Cells["ItemId"].Value);
				if (Pd.ItemId == 0)
				{
					MessageBox.Show("Item Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.JobLotId = Conversion.ToInt(r7.Cells["JobLotId"].Value);
				if (Pd.JobLotId == 0)
				{
					MessageBox.Show("JobLot Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.PackingTypeId = Conversion.ToInt(r7.Cells["PackingTypeId"].Value);
				if (Pd.PackingTypeId == 0)
				{
					MessageBox.Show("PackingType Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.PackUOMId = Conversion.ToInt(r7.Cells["PackUomId"].Value);
				Pd.UOMCode = Conversion.ToString(r7.Cells["PackUom"].Value);
				if (Pd.PackUOMId == 0)
				{
					MessageBox.Show("PackUomId Field Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.ItemQty = Conversion.ToDecimal(r7.Cells["ItemQty"].Value);
				if (Pd.ItemQty == 0m)
				{
					MessageBox.Show("ItemQty must be greater than zero");
					return;
				}
				Pd.GrossWeight = Conversion.ToDecimal(r7.Cells["GrossWeight"].Value);
				Pd.WeightCut = Conversion.ToDecimal(r7.Cells["WtCut"].Value);
				Pd.WeightCutTotal = Conversion.ToDecimal(r7.Cells["WtCutTotal"].Value);
				Pd.NetBillWeight = Conversion.ToDecimal(r7.Cells["NetBillWeight"].Value);
				if (Pd.NetBillWeight == 0m)
				{
					MessageBox.Show("NetBillWeight Field Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.NetStockWeight = Conversion.ToDecimal(r7.Cells["StockWeight"].Value);
				if (Pd.NetStockWeight == 0m)
				{
					MessageBox.Show("NetStockWeight Field Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.ItemRate = Conversion.ToDecimal(r7.Cells["ItemRate"].Value);
				if (Pd.ItemRate == 0m)
				{
					MessageBox.Show("ItemRate Field Required");
					return;
				}
				Pd.RateUomScheduleId = Conversion.ToInt(r7.Cells["RateUomId"].Value);
				if (Pd.SaleOrderId > 0)
				{
					if (Pd.RateUomScheduleId == 0)
					{
						MessageBox.Show("Rate UOM Required In Row# " + (r7.RowIndex + 1));
						return;
					}
				}
				else if (Pd.SaleOrderId == 0 && Pd.RateUomScheduleId == 0)
				{
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r7.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r7.Cells["EquivalentSoRate"].Value) + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("This " + r7.Cells["ItemName"].Value?.ToString() + " Item RateUom not define please check");
							return;
						}
						Pd.RateUomScheduleId = Conversion.ToInt(dr[0].ItemArray[0]);
					}
				}
				Pd.RateUom = Conversion.ToString(r7.Cells["RateUom"].Value);
				Pd.RateCut = Conversion.ToDecimal(r7.Cells["RateCut"].Value);
				Pd.RateCutAmount = Conversion.ToDecimal(r7.Cells["RateCutAmount"].Value);
				Pd.ItemAmount = Conversion.ToDecimal(r7.Cells["ItemAmount"].Value);
				if (Pd.ItemAmount == 0m)
				{
					MessageBox.Show("ItemAmount Field Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.WarehouseId = Conversion.ToInt(r7.Cells["WarehouseId"].Value);
				if (Pd.WarehouseId == 0)
				{
					MessageBox.Show("Warehouse Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.CityId = Conversion.ToInt(r7.Cells["CityId"].Value);
				if (Pd.CityId == 0)
				{
					MessageBox.Show("City Required In Row# " + (r7.RowIndex + 1));
					return;
				}
				Pd.ItemNetAmount = Conversion.ToDecimal(r7.Cells["BillAmount"].Value);
				Pd.FreightAmount = Conversion.ToDecimal(r7.Cells["Freights"].Value);
				Pd.JournalAmount = Conversion.ToDecimal(r7.Cells["Journal"].Value);
				Pd.ExpenseAmount = Conversion.ToDecimal(r7.Cells["Expense"].Value);
				Pd.CommissionAmount = Conversion.ToDecimal(r7.Cells["Commission"].Value);
				Pd.GpDate = DateTime.Now;
				Pd.GpNo = Conversion.ToInt(r7.Cells["GpNo"].Value);
				Pd.VehicleNo = Conversion.ToString(r7.Cells["VehicleNo"].Value);
				Pd.ExchangeRate = Conversion.ToDecimal(r7.Cells["ExchangeRate"].Value);
				Pd.FcyAmount = Conversion.ToDecimal(r7.Cells["FcyAmount"].Value);
				Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				Pd.ActionTypeId = ((Pd.Id <= 0) ? 1 : 2);
				saleInvoice.InvSaleInvoiceDetaillist.Add(Pd);
			}
			saleInvoice.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			rows = grdFreight.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvSaleInvoiceFreight PF = new InvSaleInvoiceFreight();
					PF.InvGdnId = Conversion.ToInt(r3.Cells["InvGdnId"].Value);
					PF.TansporterId = Conversion.ToInt(r3.Cells["Transporter"].Value);
					PF.CreditAmount = Conversion.ToDecimal(r3.Cells["Freight"].Value);
					PF.DebitAmount = Conversion.ToDecimal(r3.Cells["Debit"].Value);
					saleInvoice.InvSaleInvoiceFreightList.Add(PF);
				}
			}
			saleInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvOtherItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					saleInvoice.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			saleInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
					PJ.ChartofAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDecimal(r.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDecimal(r.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDecimal(r.Cells["Credit"].Value);
					saleInvoice.InvSaleInvoiceJournalList.Add(PJ);
				}
			}
			saleInvoice.AttachmentsList = new List<DMSAttachments>();
			saleInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(saleInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int sucess = InvSaleInvoice.Save(saleInvoice);
			if (Id == 0)
			{
				MessageBox.Show("Record saved Successfully" + saleInvoice.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully" + saleInvoice.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(Id, base.Name, 1509, 0);
				}
			}
			Reset();
			if (ChkBok.Checked)
			{
				Id = sucess;
				VoucherHeadIdGet();
				Id = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1509);
			}
			if (ChkPrintSlip.Checked)
			{
				CommonServices.SaleInvoiceSteelSlip_1514(sucess);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				toolStripButton3_Click_1(null, null);
			}
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			LoadGDN = new frmLoadGDN_St(UserAccount);
			LoadGDN.ShowDialog();
			LoadGDN.BringToFront();
			LoadInGridDetail();
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			try
			{
				grdInvExp.UpdateData();
				GridEXRow item = grdInvExp.CurrentRow;
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate2 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate2;
					item.Cells["Amount"].Value = Amount;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			try
			{
				grdInvExp.UpdateData();
				BillAmount();
				ExpProportion();
				grdFreight.UpdateData();
				GridEXRow items = grdFreight.CurrentRow;
				if (items != null && (int)items.RowType == 0 && Conversion.ToDouble((object)items.Cells["Debit"].Value.ToString()) > 0.0)
				{
					items.Cells["Freight"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
				if (items != null && (int)items.RowType == 0 && Conversion.ToDouble((object)items.Cells["Freight"].Value.ToString()) > 0.0)
				{
					items.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
				grdFreight.UpdateData();
				BillAmount();
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
			}
			try
			{
				grdGLedger.UpdateData();
				GridEXRow Item = grdGLedger.CurrentRow;
				if (Item != null && (int)Item.RowType == 0 && Item.Cells["Qty"].Value.ToString() != string.Empty && Item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)Item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)Item.Cells["Rate"].Value.ToString());
					Item.Cells["Credit"].Value = Qty * Rate;
					Item.Cells["Debit"].Value = 0;
					Item.Cells["Percentage"].Value = 0;
				}
				if (Item != null && (int)Item.RowType == 0 && Item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)Item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num2 / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						Item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
						Item.Cells["Debit"].Value = 0;
					}
					else
					{
						Item.Cells["Debit"].Value = Math.Round(TotlaPercentage);
						Item.Cells["Credit"].Value = 0;
					}
					Item.Cells["Qty"].Value = 0;
					Item.Cells["Rate"].Value = 0;
				}
				if (Item != null && (int)Item.RowType == 0 && Conversion.ToDouble((object)Item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					Item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is already added");
				}
				if (Item != null && (int)Item.RowType == 0 && Conversion.ToDouble((object)Item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					Item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is already added");
				}
				grdGLedger.UpdateData();
				BillAmount();
				LedgerProportion();
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
			}
		}
		catch (Exception ex4)
		{
			MessageBox.Show(ex4.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id > 0)
			{
				CommonServices.SaleInvoiceSteelSlip_1514(Id);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnSaleHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Historyfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record cannot be Delete because Record has approved");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvSaleInvoice.DeleteById(Id, UserAccount.ID);
				MessageBox.Show("Invoice Deleted Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DateTime.Now;
			}
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
			AccountsFill();
			OtherItemsBind();
			BranchFill();
			ProjectFill();
			GridComboBind();
			SupplierNameFilll();
			PaymentTerms();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipFormat2_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id > 0)
			{
				CommonServices.SaleInvoiceSteelSlip_1514A(Id);
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					Conversion.ToInt(item.Cells["Id"].Value);
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1509);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommagent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsuppliername_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(CmbSupplier.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
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
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
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
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
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
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtTotalQty.Text = TotalQty.ToString("#,##0.##");
				txtTotalWeight.Text = TotalWeight.ToString("#,##0.###");
				txtFcyAmount.Text = Math.Round(TotalFcyAmount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount).ToString(clsGlobalVariables.stringFormatsingleForFcy);
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
			throw new Exception(ex.Message);
		}
	}

	private void SaleInvoice_St_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Expected O, but got Unknown
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Expected O, but got Unknown
		//IL_13ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f4: Expected O, but got Unknown
		//IL_1401: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Expected O, but got Unknown
		//IL_163f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1649: Expected O, but got Unknown
		//IL_1656: Unknown result type (might be due to invalid IL or missing references)
		//IL_1660: Expected O, but got Unknown
		//IL_1883: Unknown result type (might be due to invalid IL or missing references)
		//IL_188d: Expected O, but got Unknown
		//IL_189a: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a4: Expected O, but got Unknown
		//IL_6b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b78: Expected O, but got Unknown
		//IL_6b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ba6: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
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
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.label28 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.label29 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.label30 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.txtCommRemarks = new System.Windows.Forms.RichTextBox();
		this.txtTotalQty = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtTotalWeight = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.CmbCommType = new UltraCombo();
		this.CmbCommUom = new UltraCombo();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.CmbProject = new UltraCombo();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.CmbCommAgent = new UltraCombo();
		this.cmbstockprty = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbrefprty = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.txtRefNo = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtBillNo = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnSlipFormat2 = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.label45 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.label1 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.CmbDeliveryTermHistory = new UltraCombo();
		this.label67 = new System.Windows.Forms.Label();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel17.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1113, 691);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1113, 691);
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
		this.tabForm.Size = new System.Drawing.Size(1105, 665);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 275);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1099, 387);
		this.panel5.TabIndex = 3;
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.panel17);
		this.panel12.Controls.Add(this.label23);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 176);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1099, 211);
		this.panel12.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1060, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 25);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1099, 188);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 23);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1099, 188);
		this.panel17.TabIndex = 5;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Dock = System.Windows.Forms.DockStyle.Top;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(0, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(1099, 23);
		this.label23.TabIndex = 0;
		this.label23.Text = "Detail Grid";
		this.panel7.Controls.Add(this.tabControl2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1099, 176);
		this.panel7.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1099, 176);
		this.tabControl2.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabPage1.Controls.Add(this.label28);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1091, 147);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Freight";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdFreight.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1085, 118);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Dock = System.Windows.Forms.DockStyle.Top;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 3);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(1085, 23);
		this.label28.TabIndex = 3;
		this.label28.Text = "Freight Grid";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabPage2.Controls.Add(this.label29);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1296, 147);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Other Items Expense";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdInvExp.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1290, 118);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.label29.BackColor = System.Drawing.Color.Teal;
		this.label29.Dock = System.Windows.Forms.DockStyle.Top;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(3, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(1290, 23);
		this.label29.TabIndex = 4;
		this.label29.Text = "Other Items Expense Grid";
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabPage3.Controls.Add(this.label30);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1296, 147);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Party Add/Less";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdGLedger.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1296, 124);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.label30.BackColor = System.Drawing.Color.Teal;
		this.label30.Dock = System.Windows.Forms.DockStyle.Top;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(0, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(1296, 23);
		this.label30.TabIndex = 5;
		this.label30.Text = "Party Add/Less Grid";
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.txtCommRemarks);
		this.panel4.Controls.Add(this.txtTotalQty);
		this.panel4.Controls.Add(this.label60);
		this.panel4.Controls.Add(this.txtTotalWeight);
		this.panel4.Controls.Add(this.txtFcyAmount);
		this.panel4.Controls.Add(this.label58);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.txtExchangeRate);
		this.panel4.Controls.Add(this.label57);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.panel4.Controls.Add(this.label56);
		this.panel4.Controls.Add(this.txtDeliveryTerm);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.panel18);
		this.panel4.Controls.Add(this.ChkPrintSlip);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommUom);
		this.panel4.Controls.Add(this.ChkBok);
		this.panel4.Controls.Add(this.txtDueDays);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.DueDate);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProject);
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgent);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrefprty);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtBillAmount);
		this.panel4.Controls.Add(this.txtRefNo);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtBillNo);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.txtDocNo);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1099, 244);
		this.panel4.TabIndex = 0;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(735, 183);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(255, 52);
		this.txtRemarks.TabIndex = 23;
		this.txtRemarks.Text = "";
		this.txtCommRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRemarks.Location = new System.Drawing.Point(735, 125);
		this.txtCommRemarks.Name = "txtCommRemarks";
		this.txtCommRemarks.Size = new System.Drawing.Size(255, 52);
		this.txtCommRemarks.TabIndex = 22;
		this.txtCommRemarks.Text = "";
		this.txtTotalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalQty.Enabled = false;
		this.txtTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalQty.Location = new System.Drawing.Point(395, 126);
		this.txtTotalQty.Name = "txtTotalQty";
		this.txtTotalQty.Size = new System.Drawing.Size(230, 23);
		this.txtTotalQty.TabIndex = 13;
		this.txtTotalQty.TabStop = false;
		this.txtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(313, 158);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(52, 15);
		this.label60.TabIndex = 604;
		this.label60.Text = "Total Wt";
		this.txtTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeight.Enabled = false;
		this.txtTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalWeight.Location = new System.Drawing.Point(395, 154);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.Size = new System.Drawing.Size(230, 23);
		this.txtTotalWeight.TabIndex = 14;
		this.txtTotalWeight.TabStop = false;
		this.txtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(395, 183);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(230, 23);
		this.txtFcyAmount.TabIndex = 15;
		this.txtFcyAmount.TabStop = false;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(313, 187);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(72, 15);
		this.label58.TabIndex = 603;
		this.label58.Text = "Fcy Amount";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(313, 130);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 602;
		this.label27.Text = "Total Qty";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(526, 98);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(99, 23);
		this.txtExchangeRate.TabIndex = 12;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(471, 102);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(58, 15);
		this.label57.TabIndex = 597;
		this.label57.Text = "Exch Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(395, 96);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 11;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(313, 102);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(55, 15);
		this.label56.TabIndex = 598;
		this.label56.Text = "Fcy Code";
		this.txtDeliveryTerm.BackColor = System.Drawing.Color.White;
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(395, 69);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.ReadOnly = true;
		this.txtDeliveryTerm.Size = new System.Drawing.Size(230, 23);
		this.txtDeliveryTerm.TabIndex = 10;
		this.txtDeliveryTerm.TabStop = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(313, 73);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 596;
		this.label24.Text = "Delivery Term";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(94, 209);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 7;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(5, 215);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(84, 15);
		this.label31.TabIndex = 583;
		this.label31.Text = "Payment Term";
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1099, 36);
		this.panel18.TabIndex = 38;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(10, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(108, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "Sale Invoice";
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.Location = new System.Drawing.Point(995, 213);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(74, 19);
		this.ChkPrintSlip.TabIndex = 25;
		this.ChkPrintSlip.Text = "Print Slip";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.CmbCommType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Location = new System.Drawing.Point(735, 69);
		((System.Windows.Forms.Control)(object)this.CmbCommType).Name = "CmbCommType";
		((System.Windows.Forms.Control)(object)this.CmbCommType).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommType).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.CmbCommType).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCommType).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.CmbCommUom.AllowNull = (DefaultableBoolean)1;
		this.CmbCommUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Location = new System.Drawing.Point(735, 96);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Name = "CmbCommUom";
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).TextChanged += new System.EventHandler(cmbcommuom_Leave);
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(995, 186);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(98, 19);
		this.ChkBok.TabIndex = 24;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(395, 42);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(76, 23);
		this.txtDueDays.TabIndex = 8;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(313, 46);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 36;
		this.label22.Text = "Due Days";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(526, 42);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(99, 23);
		this.DueDate.TabIndex = 9;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(470, 46);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(57, 15);
		this.label16.TabIndex = 35;
		this.label16.Text = "Due Date";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(94, 40);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 0;
		this.CmbProject.AllowNull = (DefaultableBoolean)1;
		this.CmbProject.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbProject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbProject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbProject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProject).Location = new System.Drawing.Point(94, 69);
		((System.Windows.Forms.Control)(object)this.CmbProject).Name = "CmbProject";
		((System.Windows.Forms.Control)(object)this.CmbProject).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbProject).TabIndex = 1;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1396, 3);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(31, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1396, 28);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(31, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.CmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Location = new System.Drawing.Point(735, 40);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Name = "CmbCommAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Leave += new System.EventHandler(cmbcommagent_Leave);
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(771, 256);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(632, 45);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.cmbrefprty.AllowNull = (DefaultableBoolean)1;
		this.cmbrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Location = new System.Drawing.Point(770, 286);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Name = "cmbrefprty";
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(817, 102);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(632, 75);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(94, 125);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.CmbSupplier.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(632, 131);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(817, 75);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 131);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Party Name";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(892, 71);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(98, 23);
		this.txtcommrate.TabIndex = 19;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(632, 187);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(94, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Invoice Remarks";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(665, 291);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Reference Party";
		this.label2.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(632, 102);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 15);
		this.label19.TabIndex = 14;
		this.label19.Text = "Comm UOM";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(892, 98);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(98, 23);
		this.txtcommamount.TabIndex = 21;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(666, 260);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.txtBillAmount.BackColor = System.Drawing.Color.White;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(395, 211);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(230, 23);
		this.txtBillAmount.TabIndex = 16;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefNo.Location = new System.Drawing.Point(94, 154);
		this.txtRefNo.Name = "txtRefNo";
		this.txtRefNo.Size = new System.Drawing.Size(212, 23);
		this.txtRefNo.TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 45);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 187);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(42, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Bill No";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(313, 215);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 18;
		this.label3.Text = "Bill Amount";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 158);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(69, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Reference #";
		this.txtBillNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillNo.Location = new System.Drawing.Point(94, 183);
		this.txtBillNo.Name = "txtBillNo";
		this.txtBillNo.Size = new System.Drawing.Size(212, 23);
		this.txtBillNo.TabIndex = 6;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 75);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(196, 98);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(110, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(94, 98);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(100, 23);
		this.txtDocNo.TabIndex = 2;
		this.txtDocNo.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 102);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1099, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.btnSave, this.btnUpdate, this.btnDelete, this.btnRefresh, this.btnAttachment, this.toolStripButton3, this.btnPrint, this.toolStripButton1, this.btnSlipFormat2 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(680, 27);
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
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(62, 24);
		this.toolStripButton3.Text = "Load &Gdn";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(97, 24);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(79, 24);
		this.toolStripButton1.Text = "1514-Slip";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnSlipFormat2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlipFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipFormat2.Name = "btnSlipFormat2";
		this.btnSlipFormat2.Size = new System.Drawing.Size(87, 24);
		this.btnSlipFormat2.Text = "1514A-Slip";
		this.btnSlipFormat2.Click += new System.EventHandler(btnSlipFormat2_Click);
		this.tabHistory.Controls.Add(this.rdapproveddate);
		this.tabHistory.Controls.Add(this.rdmodifydate);
		this.tabHistory.Controls.Add(this.drdocdate);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.rdentrydate);
		this.tabHistory.Controls.Add(this.groupBox2);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabHistory.Controls.Add(this.label45);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1105, 665);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 392);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1099, 270);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 7;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1052, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar4.TabIndex = 46;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label1);
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 126);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1099, 26);
		this.panel14.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1048, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.FrozenColumns = 4;
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 152);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1099, 221);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 6;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.label45.BackColor = System.Drawing.Color.White;
		this.label45.Dock = System.Windows.Forms.DockStyle.Top;
		this.label45.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(3, 52);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(1099, 74);
		this.label45.TabIndex = 0;
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label9);
		this.panel13.Controls.Add(this.toolStrip2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1099, 49);
		this.panel13.TabIndex = 27;
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1099, 25);
		this.toolStrip2.TabIndex = 22;
		this.toolStrip2.Text = "toolStrip2";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(142, 19);
		this.label1.TabIndex = 24;
		this.label1.Text = "Filtered Records";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(2, 28);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(240, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "SALE INVOICE HISTORY       ";
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label67);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 53);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(888, 72);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(978, 88);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 5;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(981, 67);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 4;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(900, 67);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 2;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(900, 89);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 3;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(825, 43);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(624, 20);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(34, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Party";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(238, 48);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(238, 20);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(5, 48);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(5, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(664, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 6;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(323, 44);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(323, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(74, 44);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(159, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(74, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(159, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label49);
		this.panel3.Controls.Add(this.ctrlGrdBar4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(3, 373);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1099, 19);
		this.panel3.TabIndex = 51;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(64, 22);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.CmbDeliveryTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Location = new System.Drawing.Point(498, 42);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Name = "CmbDeliveryTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermHistory).TabIndex = 5;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(408, 48);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(80, 15);
		this.label67.TabIndex = 55;
		this.label67.Text = "Delivery Term";
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(498, 14);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 4;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(408, 20);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(84, 15);
		this.label61.TabIndex = 54;
		this.label61.Text = "Payment Term";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1113, 691);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "SaleInvoice_St";
		this.Text = "Sale Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleInvoice_St_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		base.ResumeLayout(false);
	}
}
