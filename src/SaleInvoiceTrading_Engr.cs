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
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.SaleTrading
{

public class SaleInvoiceTrading_Engr : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtGPGRNS = new DataTable();

	private int GRNID;

	private int VoucherHeadId;

	public int RecId;

	private int SaleOrderId;

	private int BaseCurrency;

	private double BaseRate;

	private bool HasMultiCurrencyFeature;

	private bool Approved;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private bool CommissionDebitToExpenses;

	private bool FreightDebitToExpenses;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool IsCommgrdLock;

	private int ItemId;

	private int POID;

	private int DeliveryTypeId;

	public static int SupplierId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtComm = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCommType = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public string GRNS = string.Empty;

	private DataTable dtTerms = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private bool TaxPercntEditableOrNot;

	private UserAccount UserAccount = new UserAccount();

	private LoadGdnTrading_Engr LoadGDN;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

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

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private TextBox txtrefno;

	private Label label12;

	private TextBox txtbillno;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel6;

	private Timer timer1;

	internal GridEX grd;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	private TextBox txtBillAmount;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Panel panel13;

	private ToolStripButton toolStripButton1;

	internal GridEX grdHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private CheckBox ChkBok;

	private ToolStripButton btnDelete;

	internal UltraCombo cmbcommtype;

	internal UltraCombo cmbcommuom;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSlipFormat2;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdDetail;

	private CheckBox ChkPrintSlip;

	internal UltraCombo CmbCommissionDebitAc;

	private Label lblDebitAccount;

	private RichTextBox txtcommremarks;

	private Panel panel7;

	private Panel panel9;

	private Panel panel4;

	private Panel panel10;

	private Label label2;

	private Panel panel22;

	private Panel panel23;

	private Panel panel15;

	private Panel panel18;

	internal GridEX grdComm;

	private Panel panel19;

	private Label label6;

	private Panel panel16;

	private Panel panel17;

	private Label label1;

	private Panel panel8;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar7;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel11;

	private Panel panel5;

	private Label label9;

	private Panel panel12;

	private Label label10;

	private Panel panel26;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label14;

	private Panel panel28;

	private Panel panel21;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label5;

	private Label label15;

	internal UltraCombo CmbTaxAccount;

	private Label label43;

	private TextBox txtExchangeRate;

	private TextBox txtFcyAmount;

	private Label label44;

	internal UltraCombo cmbCurrency;

	private Label label45;

	private Label label16;

	private TextBox txtRemarks;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnshortcutkeys;

	private TextBox txtDueDays;

	private DateTimePicker DueDate;

	private Label label3;

	internal UltraCombo CmbPaymentTerm;

	private Label label24;

	private Label label22;

	internal UltraCombo cmbDeliveryTerm;

	private Panel panel20;

	private Label label27;

	private TextBox txtFreightAmount;

	private Label label23;

	internal UltraCombo CmbFreightAcCr;

	private Panel panel29;

	internal GridEX grdPaymentTerm;

	private Label label28;

	internal UltraCombo cmbFreightAcDr;

	private Panel panel30;

	private Label label54;

	private Button btnshow;

	private Panel panel31;

	private GroupBox groupBox4;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label18;

	private Label label77;

	private TextBox txtFromNoHistory;

	private Panel panel14;

	private Label label29;

	private Panel panel24;

	private Panel panel3;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Label label13;

	private Label label58;

	private TextBox txtFreightRemarks;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label31;

	internal UltraCombo CmbDiscountAccount;

	private Label label30;

	private TextBox txtDiscountAmountHeader;

	public SaleInvoiceTrading_Engr(UserAccount user)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			((Control)(object)CmbCommissionDebitAc).Visible = false;
			lblDebitAccount.Visible = false;
			txtcommremarks.Height = 78;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("DebitAmountChargetoExpenseAcFreightGrid,DebitAmountChargetoExpenseAcOfCommission,CommissionEditableOnInvoice,TaxPercentEditable");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DebitAmountChargetoExpenseAcFreightGrid")
					{
						FreightDebitToExpenses = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DebitAmountChargetoExpenseAcOfCommission")
					{
						CommissionDebitToExpenses = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "CommissionEditableOnInvoice")
					{
						IsCommgrdLock = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "TaxPercentEditable")
					{
						TaxPercntEditableOrNot = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (CommissionDebitToExpenses)
			{
				((Control)(object)CmbCommissionDebitAc).Visible = true;
				lblDebitAccount.Visible = true;
				txtcommremarks.Height = 51;
			}
			dtCommType.Columns.Add("Id");
			dtCommType.Columns.Add("CommissionType");
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			DocumentNo();
			PaymentTerms();
			DeliveryTerm();
			CurrencyFill();
			MultiCurrencyFeature();
			ConfigurationDefault();
			TransPorterDebitFill();
			SupplierNameFilll();
			OtherItemsBind();
			CommissionDebitAccountFill();
			CommissionTypeFill();
			CommissionUOMFill();
			TaxAccountFill();
			GetallTaxType();
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			toolStripButton1.Enabled = formright.DoHavePrintRights;
			btnSlipFormat2.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
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
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar5.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar5.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar5.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar5.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar5.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar5.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar6.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar6.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar6.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar6.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar6.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar6.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar7.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar7.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar7.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar7.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar7.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar7.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtGPGRNS.Columns.Add("Id");
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("GdnId", typeof(int));
			dtGrid.Columns.Add("GdnDetailId", typeof(int));
			dtGrid.Columns.Add("SaleOrderId", typeof(int));
			dtGrid.Columns.Add("SaleOrderDetailId", typeof(int));
			dtGrid.Columns.Add("SaleOrder", typeof(int));
			dtGrid.Columns.Add("OrderDate", typeof(DateTime));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("PackUomId", typeof(int));
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("PackEquivalent", typeof(double));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUomId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("AddLessRate", typeof(double));
			dtGrid.Columns.Add("NetRate", typeof(double));
			dtGrid.Columns.Add("DiscPrcnt", typeof(double));
			dtGrid.Columns.Add("DiscAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("TaxNameId", typeof(int));
			dtGrid.Columns.Add("TaxPrcnt", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("PaymentTermId", typeof(int));
			dtGrid.Columns.Add("DueDays", typeof(int));
			dtGrid.Columns.Add("DueDate", typeof(DateTime));
			dtGrid.Columns.Add("BillTypeId", typeof(int));
			dtGrid.Columns.Add("BillType", typeof(string));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("GlAccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
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
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("%ofTotal", typeof(double));
			dtPaymentTerm.Columns.Add("Amount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("Remarks");
			AddRowInPaymentGrid();
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			dtComm.Columns.Add("Id");
			dtComm.Columns.Add("OrderId");
			dtComm.Columns.Add("OrderNo");
			dtComm.Columns.Add("InvoiceId");
			dtComm.Columns.Add("CommissionAgentId");
			dtComm.Columns.Add("CommType");
			dtComm.Columns.Add("CommRate", typeof(double));
			dtComm.Columns.Add("CommUom");
			dtComm.Columns.Add("CommissionAmount", typeof(double));
			dtComm.Columns.Add("DebitAccountId");
			dtComm.Columns.Add("Remarks");
			AddRowInCommissionGrid();
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			cmbsuppliername.Focus();
			DiscountAccountFill();
			PreviousDiscountAc();
			txtFromdateHistory.Value = DateTime.Now;
			txtToDateHistory.Value = DateTime.Now;
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreviousDiscountAc()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int Id = InvSaleInvoice.GetLastDiscountAcId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Id > 0)
			{
				CmbDiscountAccount.Value = Id;
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 1609,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
				return;
			}
			throw new Exception("Max Number Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "1609"
			};
			DataTable dtCustomer = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"CustomerName"
				}
			};
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbCustomerHistory.Value = Id;
				}
				else
				{
					((Control)(object)CmbCustomerHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransPorterDebitFill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbFreightAcDr.Value);
			int ID2 = Conversion.ToInt(CmbFreightAcCr.Value);
			dtAccountlst.Rows.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = SupplierCustomer.GetVendorsAndCustomersForTransporter(UserAccount.OrganizationId, UserAccount.CompanyId);
				if (dt.Rows.Count > 0)
				{
					dtAccountlst.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtAccountlst.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
					}
				}
				if (dtAccountlst.Rows.Count > 0)
				{
					DDL.BindDDL(dtAccountlst, cmbFreightAcDr, "SupplierCustomerId", "AccountTitle", "Freight/Ac Dr", ZeroIndex: false);
					DDL.BindDDL(dtAccountlst, CmbFreightAcCr, "SupplierCustomerId", "AccountTitle", "Freight/Ac Cr", ZeroIndex: false);
					if (ID > 0)
					{
						if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == ID))
						{
							cmbFreightAcDr.Value = ID;
						}
						else
						{
							((Control)(object)cmbFreightAcDr).Text = string.Empty;
						}
					}
					if (ID2 > 0)
					{
						if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == ID2))
						{
							CmbFreightAcCr.Value = ID2;
						}
						else
						{
							((Control)(object)CmbFreightAcCr).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)cmbFreightAcDr).Text = string.Empty;
					((UltraGridBase)cmbFreightAcDr).DataSource = null;
					((Control)(object)CmbFreightAcCr).Text = string.Empty;
					((UltraGridBase)CmbFreightAcCr).DataSource = null;
				}
			}
			else
			{
				if (FreightDebitToExpenses)
				{
					DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
					if (dt2.Rows.Count > 0)
					{
						for (int j = 0; j < dt2.Rows.Count; j++)
						{
							dtAccountlst.Rows.Add(dt2.Rows[j]["Id"], 0, dt2.Rows[j]["AccountTitle"]);
						}
					}
				}
				else
				{
					DataTable dt3 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
					if (dt3.Rows.Count > 0)
					{
						for (int k = 0; k < dt3.Rows.Count; k++)
						{
							dtAccountlst.Rows.Add(dt3.Rows[k]["Id"], 0, dt3.Rows[k]["AccountTitle"]);
						}
					}
				}
				if (dtAccountlst.Rows.Count > 0)
				{
					DDL.BindDDL(dtAccountlst, cmbFreightAcDr, "Id", "AccountTitle", "Freight/Ac Dr", ZeroIndex: false);
					DDL.BindDDL(dtAccountlst, CmbFreightAcCr, "Id", "AccountTitle", "Freight/Ac Cr", ZeroIndex: false);
					if (ID > 0)
					{
						if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
						{
							cmbFreightAcDr.Value = ID;
						}
						else
						{
							((Control)(object)cmbFreightAcDr).Text = string.Empty;
						}
					}
					if (ID2 > 0)
					{
						if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
						{
							CmbFreightAcCr.Value = ID2;
						}
						else
						{
							((Control)(object)CmbFreightAcCr).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)cmbFreightAcDr).Text = string.Empty;
					((UltraGridBase)cmbFreightAcDr).DataSource = null;
					((Control)(object)CmbFreightAcCr).Text = string.Empty;
					((UltraGridBase)CmbFreightAcCr).DataSource = null;
				}
			}
			((UltraGridBase)cmbFreightAcDr).DisplayLayout.Bands[0].Columns[1].Hidden = true;
			((UltraGridBase)cmbFreightAcDr).DisplayLayout.Bands[0].Columns[2].Width = 300;
			((UltraGridBase)CmbFreightAcCr).DisplayLayout.Bands[0].Columns[1].Hidden = true;
			((UltraGridBase)CmbFreightAcCr).DisplayLayout.Bands[0].Columns[2].Width = 300;
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
			int ID = Conversion.ToInt(CmbPaymentTerm.Value);
			dtTerms.Rows.Clear();
			dtTerms = CommonServices.GetDueTermServiceBind();
			if (dtTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerms, CmbPaymentTerm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)CmbPaymentTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dtTerms.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPaymentTerm.Value = ID;
					}
					else
					{
						((Control)(object)CmbPaymentTerm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPaymentTerm).Text = string.Empty;
				((UltraGridBase)CmbPaymentTerm).DataSource = null;
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
			DDL.BindDDL(dt, cmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)cmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DDL.BindDDL(dt, cmbcommuom, "Id", "Uom", "UOM", ZeroIndex: false);
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
			dtCommType.Rows.Add(1, "Flat");
			dtCommType.Rows.Add(2, "Percent");
			DDL.BindDDL(dtCommType, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFilll()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbsuppliername.Value);
			int ID2 = Conversion.ToInt(cmbcommagent.Value);
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtSupplier = SupplierCustomer.Getall(supplier);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbsuppliername, "Id", "CompanyName", "Customer Name", false);
				DDL.BindDDLNew(dtSupplier, cmbcommagent, "Id", "CompanyName", "Commission Agent Name", false);
				if (ID > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == ID))
					{
						cmbsuppliername.Value = ID;
					}
					else
					{
						((Control)(object)cmbsuppliername).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == ID2))
					{
						cmbcommagent.Value = ID2;
					}
					else
					{
						((Control)(object)cmbcommagent).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbsuppliername).Text = string.Empty;
				((UltraGridBase)cmbsuppliername).DataSource = null;
				((Control)(object)cmbcommagent).Text = string.Empty;
				((UltraGridBase)cmbcommagent).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionDebitAccountFill()
	{
		try
		{
			if (!CommissionDebitToExpenses)
			{
				return;
			}
			int ID = Conversion.ToInt(CmbCommissionDebitAc.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCommissionDebitAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCommissionDebitAc.Value = ID;
					}
					else
					{
						((Control)(object)CmbCommissionDebitAc).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCommissionDebitAc).Text = string.Empty;
				((UltraGridBase)CmbCommissionDebitAc).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
	}

	private void TaxAccountFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbTaxAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,16,17,18,19");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxAccount, "Id", "AccountTitle", "Tax Account", ZeroIndex: false);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbTaxAccount.Value = ID;
					}
					else
					{
						((Control)(object)CmbTaxAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbTaxAccount).Text = string.Empty;
				((UltraGridBase)CmbTaxAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DiscountAccountFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbDiscountAccount.Value);
			DataTable dt = CommonServices.GetAccountTitleByAccountClassIds("4");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbDiscountAccount, "Id", "AccountTitle", "Discount Account", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbDiscountAccount.Value = ID;
					}
					else
					{
						((Control)(object)CmbDiscountAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbDiscountAccount).Text = string.Empty;
				((UltraGridBase)CmbDiscountAccount).DataSource = null;
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 1609);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetallTaxType()
	{
		try
		{
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
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

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurrency.Value = id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label43.Visible = true;
				label44.Visible = true;
				label45.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label43.Visible = false;
				label44.Visible = false;
				label45.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1609",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
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
					double amount = Conversion.ToDouble(obj.Cells["ItemAmount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
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
			BillAmount();
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

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == null || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo  field is Required");
			txtdocno.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		if (((UltraGridBase)cmbsuppliername).ActiveRow == null)
		{
			MessageBox.Show("Customer Name field is Required");
			cmbsuppliername.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbDeliveryTerm.Value) <= 0)
		{
			MessageBox.Show("Delivery Term field is Required");
			cmbDeliveryTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) <= 0)
		{
			MessageBox.Show("Payment Term field is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) <= 0)
		{
			MessageBox.Show("Due Days field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0) && (Conversion.ToInt(CmbFreightAcCr.Value) > 0 || Conversion.ToInt(cmbFreightAcDr.Value) > 0))
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtFreightAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0 && Conversion.ToInt(CmbFreightAcCr.Value) <= 0)
		{
			MessageBox.Show("Freight/Ac Cr Field is Required");
			CmbFreightAcCr.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0 && txtFreightRemarks.Text.Trim() == "")
		{
			MessageBox.Show("Freight Remarks Field is Required");
			txtFreightRemarks.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			DeliveryTypeId = 0;
			((Control)(object)cmbDeliveryTerm).Enabled = false;
			((Control)(object)cmbDeliveryTerm).Enabled = false;
			ItemId = 0;
			POID = 0;
			RecId = 0;
			SaleOrderId = 0;
			Approved = false;
			((Control)(object)cmbsuppliername).Enabled = true;
			SupplierGLCreditAmount = 0.0;
			SupplierGLDebitAmount = 0.0;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			((Control)(object)CmbFreightAcCr).Text = string.Empty;
			((Control)(object)cmbFreightAcDr).Text = string.Empty;
			txtFreightAmount.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			dtGrid.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtComm.Rows.Clear();
			txtbillno.Text = string.Empty;
			txtrefno.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			grd.DataSource = dtGrid;
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtPaymentTerm.Rows.Clear();
			AddRowInPaymentGrid();
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			AddRowInCommissionGrid();
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			SupplierNameFilll();
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			cmbsuppliername.Focus();
			dtGPGRNS.Rows.Clear();
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleInvoiceTrading_Engr_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
			AccountsFill();
			GridComboBind();
			SupplierNameFilll();
			TaxAccountFill();
			TransPorterDebitFill();
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("DebitAmountChargetoExpenseAcFreightGrid,DebitAmountChargetoExpenseAcOfCommission");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DebitAmountChargetoExpenseAcFreightGrid")
					{
						FreightDebitToExpenses = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DebitAmountChargetoExpenseAcOfCommission")
					{
						CommissionDebitToExpenses = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "CommissionEditableOnInvoice")
					{
						IsCommgrdLock = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (CommissionDebitToExpenses)
			{
				grdComm.RootTable.Columns["DebitAccountId"].Visible = true;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
			}
			else
			{
				grdComm.RootTable.Columns["DebitAccountId"].Visible = false;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)0;
			}
			ConfigurationDefault();
			DiscountAccountFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPaymentGrid()
	{
		try
		{
			dtPaymentTerm.Rows.Add(0, 0, 0, 0, DateTime.Now, "");
			grdPaymentTerm.DataSource = dtPaymentTerm;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm()
	{
		try
		{
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].HasValueList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].LimitToList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerms.DefaultView, "Id", "TermsDescription");
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Width = 120;
			grdPaymentTerm.RootTable.Columns["DueDate"].EditType = (EditType)2;
			grdPaymentTerm.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdPaymentTerm.RootTable.Columns["%ofTotal"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].FormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TotalFormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["DueDays"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["DueDays"].FormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["DueDays"].TotalFormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentTerm.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentTerm);
			grdPaymentTerm.RootTable.Columns["Remarks"].Width = 300;
			grdPaymentTerm.RootTable.Columns.Add("Delete");
			grdPaymentTerm.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTerm.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].Width = 20;
			grdPaymentTerm.RootTable.Columns.Add("Add");
			grdPaymentTerm.RootTable.Columns["Add"].Key = "Add";
			grdPaymentTerm.RootTable.Columns["Add"].Caption = "+";
			grdPaymentTerm.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Add"].ButtonText = "+";
			grdPaymentTerm.RootTable.Columns["Add"].Width = 20;
			grdPaymentTerm.RootTable.Columns["Add"].Position = 0;
			grdPaymentTerm.RootTable.Columns["Delete"].Position = 1;
			grdPaymentTerm.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "%ofTotal" && grd.GetRows().Count() > 0)
				{
					double num = Conversion.ToDouble((object)txtBillAmount.Text);
					double prcnt2 = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
					if (prcnt2 > 100.0)
					{
						MessageBox.Show("%of Total Can't Greater than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["%ofTotal"].Value = 100;
						prcnt2 = 100.0;
					}
					double amount2 = num * prcnt2 / 100.0;
					item.Cells["Amount"].Value = Math.Round(amount2, 4);
				}
				if (e.Column.Key == "Amount")
				{
					if (grd.GetRows().Count() <= 0)
					{
						throw new Exception("No Detail Record Found");
					}
					double billAmount2 = Conversion.ToDouble((object)txtBillAmount.Text);
					double amount3 = Conversion.ToDouble(item.Cells["Amount"].Value);
					if (billAmount2 < amount3)
					{
						MessageBox.Show("Amount Cant be Greater than BillAmount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["Amount"].Value = billAmount2;
						amount3 = billAmount2;
					}
					double prcnt3 = amount3 * 100.0 / billAmount2;
					item.Cells["%ofTotal"].Value = Math.Round(prcnt3, 8);
				}
				if (e.Column.Key == "DueDays")
				{
					int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = DocDate.Value.AddDays(days2);
				}
				if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < DocDate.Value.Date)
					{
						item.Cells["DueDate"].Value = DocDate.Value;
						throw new Exception("Due Date Can't less Than DocDate");
					}
					int days = (date.Date - DocDate.Value.Date).Days;
					item.Cells["DueDays"].Value = days;
				}
				if (e.Column.Key == "PaymentTermId")
				{
					if (Conversion.ToInt(item.Cells["PaymentTermId"].Value) == 3 && SaleOrderId > 0)
					{
						DataTable dt = SaleOrder.GetSaleOrderRemningAmountByOrderId(UserAccount.OrganizationId, UserAccount.CompanyId, SaleOrderId, RecId);
						if (dt != null)
						{
							item.Cells["Amount"].Value = Conversion.ToDouble(dt.Rows[0]["BalanceAmount"]);
							double billAmount = Conversion.ToDouble((object)txtBillAmount.Text);
							double amount = Conversion.ToDouble(item.Cells["Amount"].Value);
							if (billAmount < amount)
							{
								MessageBox.Show("Amount Cant be Greater than BillAmount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								item.Cells["Amount"].Value = billAmount;
								amount = billAmount;
							}
							double prcnt = Math.Round(amount * 100.0 / billAmount, 8);
							item.Cells["%ofTotal"].Value = prcnt;
						}
					}
					else
					{
						item.Cells["Amount"].Value = 0.ToString();
					}
				}
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "%ofTotal" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdPaymentTerm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPaymentTerm.CurrentRow;
				dtPaymentTerm.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddRowInPaymentGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPaymentGrid();
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentTerm.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPaymentTerm.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
				}
				if (grdPaymentTerm.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentGrid();
				}
				grdPaymentTerm.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentDetailRecalculate()
	{
		try
		{
			grdPaymentTerm.GetRows().Count();
			_ = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdComm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "CommRate" || e.Column.Key == "CommissionAmount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdComm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdComm.CurrentRow;
				if (Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
				{
					if (grdComm.GetRows().Length == 1)
					{
						MessageBox.Show("You Can not Delete Row Because The Above Entry Is From Sale Order");
						return;
					}
					dtComm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
				else
				{
					dtComm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInCommissionGrid();
			}
			grdComm.UpdateData();
			PropotionateCommissiongrid();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateCommissiongrid()
	{
		try
		{
			if (!CommissionDebitToExpenses)
			{
				double TotalItemAmount = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow row2 in rows)
				{
					TotalItemAmount += Conversion.ToDouble(row2.Cells["ItemAmount"].Value);
					row2.BeginEdit();
					row2.Cells["Commission"].Value = 0;
					row2.EndEdit();
				}
				rows = grdComm.GetRows();
				foreach (GridEXRow row in rows)
				{
					GridEXRow[] rows2 = grd.GetRows();
					foreach (GridEXRow item in rows2)
					{
						int saleOrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
						if (Conversion.ToInt(row.Cells["OrderId"].Value) > 0)
						{
							if (Conversion.ToInt(row.Cells["OrderId"].Value) == saleOrderId)
							{
								double PreCommission = Conversion.ToDouble(item.Cells["Commission"].Value);
								double itemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
								Conversion.ToDouble(item.Cells["ItemQty"].Value);
								item.BeginEdit();
								if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
								{
									double percent = Conversion.ToDouble(row.Cells["CommRate"].Value);
									double commission = itemAmount * percent / 100.0;
									item.Cells["Commission"].Value = commission + PreCommission;
								}
								else if (row.Cells["CommType"].Text.ToString() == "Flat")
								{
									double commissionAmount = Conversion.ToDouble(row.Cells["CommRate"].Value);
									commissionAmount = itemAmount / TotalItemAmount * 100.0 * commissionAmount / 100.0;
									item.Cells["Commission"].Value = commissionAmount + PreCommission;
								}
								item.EndEdit();
							}
						}
						else
						{
							double PreCommission2 = Conversion.ToDouble(item.Cells["Commission"].Value);
							double itemAmount2 = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
							Conversion.ToDouble(item.Cells["ItemQty"].Value);
							item.BeginEdit();
							if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
							{
								double percent2 = Conversion.ToDouble(row.Cells["CommRate"].Value);
								double commission2 = itemAmount2 * percent2 / 100.0;
								item.Cells["Commission"].Value = commission2 + PreCommission2;
							}
							else if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Flat")
							{
								double commissionAmount2 = Conversion.ToDouble(row.Cells["CommRate"].Value);
								commissionAmount2 = itemAmount2 / TotalItemAmount * 100.0 * commissionAmount2 / 100.0;
								item.Cells["Commission"].Value = commissionAmount2 + PreCommission2;
							}
							item.EndEdit();
						}
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateCommissionGrid()
	{
		try
		{
			if (grdComm.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdComm.GetRows();
			foreach (GridEXRow item in rows)
			{
				int orderId = Conversion.ToInt(item.Cells["OrderId"].Value);
				double totalAmount = 0.0;
				GridEXRow[] rows2 = grd.GetRows();
				foreach (GridEXRow row in rows2)
				{
					if (orderId > 0)
					{
						if (Conversion.ToInt(row.Cells["SaleOrderId"].Value) == orderId)
						{
							totalAmount += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
						}
					}
					else
					{
						totalAmount += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
					}
				}
				item.BeginEdit();
				if (Conversion.ToString((object)item.Cells["CommType"].Text) == "Percent")
				{
					double commissionRate = Conversion.ToDouble(item.Cells["CommRate"].Value);
					double commissionAmount = totalAmount * commissionRate / 100.0;
					if (CommissionDebitToExpenses)
					{
						item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["CommissionAmount"].Value = commissionAmount;
					}
				}
				else if (item.Cells["CommType"].Text.ToString() == "Flat")
				{
					double commissionAmount2 = Conversion.ToDouble(item.Cells["CommRate"].Value);
					if (CommissionDebitToExpenses)
					{
						item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["CommissionAmount"].Value = commissionAmount2;
					}
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdComm.UpdateData();
			if (e.Column.Key == "CommRate")
			{
				if (Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Percent")
				{
					double TotalPercentage = 0.0;
					GridEXRow[] rows = grdComm.GetRows();
					foreach (GridEXRow item in rows)
					{
						TotalPercentage += Conversion.ToDouble(item.Cells["CommRate"].Value);
					}
					if (TotalPercentage > 100.0)
					{
						double Percent = Conversion.ToDouble(grdComm.CurrentRow.Cells["CommRate"].Value);
						double i = TotalPercentage - 100.0;
						grdComm.CurrentRow.Cells["CommRate"].Value = Percent - i;
						MessageBox.Show("TotalPercentage Can not be Greater than 100...");
						return;
					}
				}
				CalculateCommissionGrid();
			}
			if (e.Column.Key == "CommType")
			{
				if (Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Flat" || Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Percent")
				{
					grdComm.RootTable.Columns["CommUom"].Visible = false;
				}
				else
				{
					grdComm.RootTable.Columns["CommUom"].Visible = true;
				}
				CalculateCommissionGrid();
			}
			if (e.Column.Key == "OrderNo")
			{
				GetOrderIdAgainstOrderNo();
				CalculateCommissionGrid();
				GetOrderIdAgainstOrderNo();
			}
			grdComm.UpdateData();
			PropotionateCommissiongrid();
			BillAmount();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInCommissionGrid()
	{
		try
		{
			dtComm.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0);
			grdComm.DataSource = dtComm;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommissionSettings()
	{
		try
		{
			grdComm.RootTable.Columns["Id"].EditType = (EditType)0;
			grdComm.RootTable.Columns["InvoiceId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommissionAgentId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommType"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommRate"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommUom"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommissionAmount"].EditType = (EditType)0;
			grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdComm.RootTable.Columns["Id"].Visible = false;
			grdComm.RootTable.Columns["InvoiceId"].Visible = false;
			grdComm.RootTable.Columns["OrderId"].Visible = false;
			grdComm.RootTable.Columns["DebitAccountId"].Visible = false;
			grdComm.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdComm.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdComm.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommRate"].AggregateFunction = (AggregateFunction)3;
			grdComm.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdComm.RootTable.Columns["CommRate"].FormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommRate"].TotalFormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommissionAgentId"].ColumnType = (ColumnType)0;
			grdComm.RootTable.Columns["CommissionAgentId"].HasValueList = true;
			grdComm.RootTable.Columns["CommissionAgentId"].LimitToList = true;
			grdComm.RootTable.Columns["CommissionAgentId"].Caption = "Commission Agent";
			grdComm.RootTable.Columns["CommissionAgentId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "AccountTitle");
			grdComm.RootTable.Columns["CommissionAgentId"].Width = 120;
			DataTable dtCommUom = new DataTable();
			dtCommUom.Columns.Add("Id");
			dtCommUom.Columns.Add("Uom");
			dtCommUom.Rows.Add(1, "1");
			dtCommUom.Rows.Add(2, "5");
			dtCommUom.Rows.Add(3, "10");
			dtCommUom.Rows.Add(4, "25");
			dtCommUom.Rows.Add(5, "40");
			dtCommUom.Rows.Add(6, "50");
			dtCommUom.Rows.Add(7, "60");
			dtCommUom.Rows.Add(8, "65");
			dtCommUom.Rows.Add(9, "80");
			dtCommUom.Rows.Add(10, "100");
			grdComm.RootTable.Columns["CommUom"].ColumnType = (ColumnType)0;
			grdComm.RootTable.Columns["CommUom"].HasValueList = true;
			grdComm.RootTable.Columns["CommUom"].LimitToList = true;
			grdComm.RootTable.Columns["CommUom"].ValueList.PopulateValueList((IEnumerable)dtCommUom.DefaultView, "Id", "Uom");
			grdComm.RootTable.Columns["CommUom"].Width = 120;
			grdComm.RootTable.Columns["CommType"].ColumnType = (ColumnType)0;
			grdComm.RootTable.Columns["CommType"].HasValueList = true;
			grdComm.RootTable.Columns["CommType"].LimitToList = true;
			grdComm.RootTable.Columns["CommType"].ValueList.PopulateValueList((IEnumerable)dtCommType.DefaultView, "Id", "CommissionType");
			grdComm.RootTable.Columns["CommType"].Width = 120;
			CommissionDebitAccountBind();
			CommGridLock();
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionDebitAccountBind()
	{
		try
		{
			if (CommissionDebitToExpenses)
			{
				DataTable dtCommDrAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21");
				grdComm.RootTable.Columns["DebitAccountId"].ColumnType = (ColumnType)0;
				grdComm.RootTable.Columns["DebitAccountId"].HasValueList = true;
				grdComm.RootTable.Columns["DebitAccountId"].LimitToList = true;
				grdComm.RootTable.Columns["DebitAccountId"].Caption = "Dr Account";
				grdComm.RootTable.Columns["DebitAccountId"].ValueList.PopulateValueList((IEnumerable)dtCommDrAc.DefaultView, "Id", "AccountTitle");
				grdComm.RootTable.Columns["DebitAccountId"].Width = 130;
				grdComm.RootTable.Columns["DebitAccountId"].Visible = true;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommGridLock()
	{
		try
		{
			if (IsCommgrdLock)
			{
				grdComm.RootTable.Columns.Add("Delete");
				grdComm.RootTable.Columns["Delete"].Key = "Delete";
				grdComm.RootTable.Columns["Delete"].Caption = "X";
				grdComm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdComm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdComm.RootTable.Columns["Delete"].ButtonText = "X";
				grdComm.RootTable.Columns["Delete"].Visible = true;
				grdComm.RootTable.Columns.Add("Add");
				grdComm.RootTable.Columns["Add"].Key = "Add";
				grdComm.RootTable.Columns["Add"].Caption = "+";
				grdComm.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdComm.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grdComm.RootTable.Columns["Add"].ButtonText = "+";
				grdComm.RootTable.Columns["Add"].Visible = true;
				grdComm.RootTable.Columns["Add"].Width = 20;
				grdComm.RootTable.Columns["Delete"].Width = 20;
				grdComm.RootTable.Columns["Add"].Position = 0;
				grdComm.RootTable.Columns["Delete"].Position = 1;
				grdComm.FrozenColumns = 2;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommissionAgentId"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommType"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommRate"].EditType = (EditType)1;
				grdComm.RootTable.Columns["CommUom"].EditType = (EditType)4;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetOrderIdAgainstOrderNo()
	{
		try
		{
			int DocNo = Conversion.ToInt(grdComm.CurrentRow.Cells["OrderNo"].Value);
			int OrderId = 0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["SaleOrder"].Value) == DocNo)
				{
					OrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
					break;
				}
			}
			if (OrderId > 0)
			{
				grd.UpdateData();
				grdComm.CurrentRow.Cells["OrderId"].Value = OrderId;
				grd.UpdateData();
			}
			else
			{
				grd.UpdateData();
				grdComm.CurrentRow.Cells["OrderId"].Value = 0;
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommGridOrderNo()
	{
		try
		{
			if (Conversion.ToInt(grdComm.CurrentRow.Cells["OrderId"].Value) != 0)
			{
				return;
			}
			var list = (from GridEXRow row in grd.GetRows()
				where Conversion.ToInt(row.Cells["SaleOrderId"].Value) > 0
				select new
				{
					OrderId = Conversion.ToInt(row.Cells["SaleOrder"].Value)
				}).Distinct().ToList();
			DataTable dtOrderNo = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"OrderNo",
						typeof(int)
					}
				}
			};
			foreach (var order in list)
			{
				dtOrderNo.Rows.Add(order.OrderId, order.OrderId);
			}
			if (dtOrderNo.Rows.Count > 0)
			{
				GridEXColumn obj = grdComm.RootTable.Columns["OrderNo"];
				obj.ColumnType = (ColumnType)0;
				obj.EditType = (EditType)4;
				obj.HasValueList = true;
				obj.LimitToList = true;
				obj.ValueList.PopulateValueList((IEnumerable)dtOrderNo.DefaultView, "OrderNo", "OrderNo");
				obj.Width = 70;
			}
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
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage")
			{
				double TotalPercentage = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["Percentage"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdGLedger.CurrentRow.Cells["Percentage"].Value);
					double i = TotalPercentage - 100.0;
					grdGLedger.CurrentRow.Cells["Percentage"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
				}
				if (item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
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
						item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero));
						item.Cells["Credit"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
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
				if (!SubsidiaryAccountAllownOnVouchers && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Customer Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid();
			}
			grdGLedger.UpdateData();
			BillAmount();
			LedgerProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdGLedger.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						item.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				else if (grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				BillAmount();
			}
			else if (e.KeyCode == Keys.F1 && grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					item.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					item.Cells["AccountId"].Value = obj2.ReturnId;
				}
				SupCustIdUpdateforGLGrid();
			}
			((Control)(object)grdGLedger).Focus();
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
			dtGrdGL.Rows.Add(0, 0, "", 0, 0, 0, 0, 0);
			grdGLedger.DataSource = dtGrdGL;
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
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
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
			grdGLedger.RootTable.Columns["Add"].Position = 0;
			grdGLedger.RootTable.Columns["Delete"].Position = 1;
			grdGLedger.FrozenColumns = 2;
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar7_Load(null, null);
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
				string selectedAcId = Conversion.ToString((object)r.Cells["AccountId"].Text);
				if (selectedAcId != "0" && selectedAcId != string.Empty)
				{
					DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
					if (matchingRows.Length != 0)
					{
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
						r.EndEdit();
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Selected Account Can not be Same As Supplier Account");
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = 0;
						r.Cells["AccountId"].Value = 0;
						r.EndEdit();
						return;
					}
				}
				else if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
				{
					MessageBox.Show("Selected Account Can not be Same As Supplier Account");
					r.BeginEdit();
					r.Cells["AccountId"].Value = 0;
					r.Cells["GlAccountId"].Value = 0;
					r.EndEdit();
					return;
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
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
					MessageBox.Show("Debit Side is aleady added");
				}
				if (Item != null && (int)Item.RowType == 0 && Conversion.ToDouble((object)Item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					Item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
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
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
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
			grdInvExp.RootTable.Columns["ItemId"].Width = 200;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
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
			grdInvExp.RootTable.Columns["Add"].Position = 0;
			grdInvExp.RootTable.Columns["Delete"].Position = 1;
			grdInvExp.FrozenColumns = 2;
			ctrlGrdBar6_Load(null, null);
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

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "AddLessRate" || e.Column.Key == "GpNo" || e.Column.Key == "TaxPrcnt" || e.Column.Key == "DiscPrcnt" || e.Column.Key == "DiscAmount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "ItemRate" || e.Column.Key == "RateUOM" || e.Column.Key == "AddLessRate" || e.Column.Key == "DiscPrcnt")
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.BeginEdit();
					double num = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					double Rate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double AddLessRate = Conversion.ToDouble(item.Cells["AddLessRate"].Value);
					double NetRate = ((AddLessRate != 0.0) ? (Rate + AddLessRate) : Rate);
					item.Cells["NetRate"].Value = NetRate;
					if (DeliveryTypeId != 1)
					{
						item.Cells["RateEquivalent"].Value = Conversion.ToDouble(item.Cells["RateUOM"].Value);
					}
					double EquivalentPoRate = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Text);
					double Amount = num / EquivalentPoRate * NetRate;
					double DiscountPrcnt = Conversion.ToDouble(item.Cells["DiscPrcnt"].Value);
					double DiscAmount = Amount * DiscountPrcnt / 100.0;
					item.Cells["DiscAmount"].Value = DiscAmount;
					Amount = Math.Round(Amount - DiscAmount, 4);
					item.Cells["ItemAmount"].Value = Amount;
					item.EndEdit();
				}
			}
			else if (e.Column.Key == "DiscAmount")
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.BeginEdit();
					double num2 = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					double Rate2 = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double AddLessRate2 = Conversion.ToDouble(item.Cells["AddLessRate"].Value);
					double NetRate2 = ((AddLessRate2 != 0.0) ? (Rate2 + AddLessRate2) : Rate2);
					item.Cells["NetRate"].Value = NetRate2;
					double EquivalentPoRate2 = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Text);
					double Amount2 = num2 / EquivalentPoRate2 * NetRate2;
					double DiscAmount2 = Conversion.ToDouble(item.Cells["DiscAmount"].Value);
					double DiscountPrcnt2 = DiscAmount2 * 100.0 / Amount2;
					item.Cells["DiscPrcnt"].Value = DiscountPrcnt2;
					Amount2 = Math.Round(Amount2 - DiscAmount2, 4);
					item.Cells["ItemAmount"].Value = Amount2;
					item.EndEdit();
				}
			}
			else if (e.Column.Key == "DueDays")
			{
				if (Conversion.ToInt(item.Cells["DueDays"].Value) > 0)
				{
					DateTime DueDate = DocDate.Value.AddDays(Conversion.ToInt(item.Cells["DueDays"].Value));
					item.Cells["DueDate"].Value = DueDate;
				}
				else
				{
					item.Cells["DueDate"].Value = DocDate.Value;
				}
			}
			else if (e.Column.Key == "TaxNameId")
			{
				int selectedTaxId = Conversion.ToInt(item.Cells["TaxNameId"].Value);
				if (selectedTaxId == 0)
				{
					item.Cells["TaxPrcnt"].Value = 0;
				}
				else
				{
					DataRow[] matchingRows = dtTaxType.Select("TaxNameId = " + selectedTaxId);
					if (matchingRows.Length != 0)
					{
						item.BeginEdit();
						item.Cells["TaxPrcnt"].Value = Conversion.ToDouble(matchingRows[0]["TaxPercent"]);
						item.EndEdit();
					}
				}
			}
			else if (e.Column.Key == "TaxPrcnt" && Conversion.ToDouble(item.Cells["TaxPrcnt"].Value) == 0.0)
			{
				int selectedTaxId2 = Conversion.ToInt(item.Cells["TaxNameId"].Value);
				DataRow[] matchingRows2 = dtTaxType.Select("TaxNameId = " + selectedTaxId2);
				if (matchingRows2.Length != 0)
				{
					item.BeginEdit();
					item.Cells["TaxPrcnt"].Value = Conversion.ToDouble(matchingRows2[0]["TaxPercent"]);
					item.EndEdit();
				}
			}
			int TaxType = Conversion.ToInt(item.Cells["TaxNameId"].Value);
			double TaxPercent = Conversion.ToDouble(item.Cells["TaxPrcnt"].Value);
			double ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
			double TaxAmount = 0.0;
			if (TaxPercent > 100.0)
			{
				item.BeginEdit();
				MessageBox.Show("Tax % Can't be greater than 100...");
				TaxPercent = 100.0;
				item.Cells["TaxPrcnt"].Value = TaxPercent;
				item.EndEdit();
			}
			if (ItemAmount > 0.0 && TaxPercent > 0.0 && TaxType > 0)
			{
				TaxAmount = ItemAmount * TaxPercent / 100.0;
				item.BeginEdit();
				item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, 4);
				item.EndEdit();
			}
			else
			{
				item.BeginEdit();
				item.Cells["TaxPrcnt"].Value = 0;
				item.Cells["TaxAmount"].Value = 0;
				item.EndEdit();
			}
			grd.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			BillAmount();
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
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrder"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderDate"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["NetRate"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Freights"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["Journal"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grd.RootTable.Columns["DueDays"].EditType = (EditType)1;
			grd.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grd.RootTable.Columns["BillTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["BillType"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscAmount"].EditType = (EditType)1;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			if (DeliveryTypeId != 1)
			{
				grd.RootTable.Columns["RateUOM"].EditType = (EditType)1;
				grd.RootTable.Columns["ItemRate"].EditType = (EditType)1;
				grd.RootTable.Columns["SaleOrder"].Visible = false;
				grd.RootTable.Columns["OrderDate"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["RateUOM"].EditType = (EditType)0;
				grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
				grd.RootTable.Columns["SaleOrder"].Visible = true;
				grd.RootTable.Columns["OrderDate"].Visible = true;
			}
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["PackUom"].Caption = "Uom";
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["GdnId"].Visible = false;
			grd.RootTable.Columns["GdnDetailId"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["Journal"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["Commission"].Visible = false;
			grd.RootTable.Columns["BillTypeId"].Visible = false;
			grd.RootTable.Columns["PaymentTermId"].Visible = false;
			grd.RootTable.Columns["DueDays"].Visible = false;
			grd.RootTable.Columns["DueDate"].Visible = false;
			grd.RootTable.Columns["SaleOrder"].Position = 0;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxPrcnt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxPrcnt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["DiscPrcnt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscPrcnt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Freights"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Commission"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			grd.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PaymentTermId"].HasValueList = true;
			grd.RootTable.Columns["PaymentTermId"].EditType = (EditType)5;
			grd.RootTable.Columns["PaymentTermId"].LimitToList = true;
			grd.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grd.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerm.DefaultView, "Id", "TermsDescription");
			grd.RootTable.Columns["TaxNameId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["TaxNameId"].HasValueList = true;
			grd.RootTable.Columns["TaxNameId"].LimitToList = true;
			grd.RootTable.Columns["TaxNameId"].EditType = (EditType)(TaxPercntEditableOrNot ? 4 : 0);
			grd.RootTable.Columns["TaxNameId"].Caption = "Tax Name";
			grd.RootTable.Columns["TaxNameId"].ValueList.PopulateValueList((IEnumerable)dtTaxType.DefaultView, "TaxNameId", "TaxName");
			grd.RootTable.Columns["TaxNameId"].Width = 60;
			if (!TaxPercntEditableOrNot)
			{
				grd.RootTable.Columns["TaxPrcnt"].EditType = (EditType)0;
			}
			else
			{
				grd.RootTable.Columns["TaxPrcnt"].EditType = (EditType)1;
			}
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	public void ReadById(int ID)
	{
		try
		{
			SaleOrderId = 0;
			RecId = Conversion.ToInt((object)ID);
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(RecId);
			txtdocno.Text = invSI.DocNo.ToString();
			DocDate.Value = invSI.DocDate;
			tabControl1.SelectedIndex = 0;
			cmbsuppliername.Value = invSI.SupplierCustomerId;
			if (CommissionDebitToExpenses)
			{
				CmbCommissionDebitAc.Value = invSI.CommissionDebitAcGLId;
			}
			((Control)(object)cmbsuppliername).Enabled = false;
			txtrefno.Text = invSI.SupplierReferenceNo.ToString();
			txtbillno.Text = invSI.ManualBillNo;
			if (invSI.TaxAccountId > 0)
			{
				CmbTaxAccount.Value = invSI.TaxAccountId;
			}
			if (invSI.TransporterId > 0)
			{
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					CmbFreightAcCr.Value = invSI.TransporterId;
				}
				else
				{
					CmbFreightAcCr.Value = invSI.TransporterCreditPartyId;
				}
			}
			if (invSI.TransporterDebitGLId > 0)
			{
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					cmbFreightAcDr.Value = invSI.TransporterDebitGLId;
				}
				else
				{
					cmbFreightAcDr.Value = invSI.TransporterDebitPartyId;
				}
			}
			txtFreightAmount.Text = invSI.FreightAmount.ToString("#,##0.####");
			cmbcommagent.Value = invSI.CommissionAgentId;
			txtcommrate.Text = invSI.CommRate.ToString();
			txtcommamount.Text = invSI.CommAmount.ToString();
			((Control)(object)cmbcommtype).Text = Conversion.ToString((object)invSI.CommissionType);
			((Control)(object)cmbcommuom).Text = invSI.UomScheduleIdCmRate;
			txtcommremarks.Text = Conversion.ToString((object)invSI.CommissionRemarks);
			txtRemarks.Text = Conversion.ToString((object)invSI.RemarksHeader);
			((Control)(object)cmbDeliveryTerm).Text = invSI.DeliveryTerm;
			CmbPaymentTerm.Value = invSI.PaymentTermId;
			txtDueDays.Text = Conversion.ToString((object)invSI.DueDays);
			DueDate.Value = Conversion.ToDateTime((object)invSI.DueDate);
			if (DeliveryTypeId != 1)
			{
				((Control)(object)cmbDeliveryTerm).Enabled = true;
				((Control)(object)CmbPaymentTerm).Enabled = true;
			}
			else
			{
				((Control)(object)cmbDeliveryTerm).Enabled = false;
				((Control)(object)CmbPaymentTerm).Enabled = false;
			}
			txtBillAmount.Text = invSI.BillAmount.ToString();
			cmbCurrency.Value = invSI.CurrencyId;
			txtDiscountAmountHeader.Text = invSI.DiscountAmount.ToString("#,##0.####");
			if (invSI.DiscountAccountId > 0)
			{
				CmbDiscountAccount.Value = invSI.DiscountAccountId;
			}
			AttachmentsValues = invSI.AttachmentsValues;
			CustomAttachmentsValues = invSI.CustomAttachmentsValues;
			txtFcyAmount.Text = invSI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			Approved = invSI.IsApproved;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int m = 0; m < invSI.InvSaleInvoiceDetaillist.Count; m++)
			{
				if (m == 0)
				{
					SaleOrderId = invSI.InvSaleInvoiceDetaillist[m].SaleOrderId;
					if (SaleOrderId > 0)
					{
						DeliveryTypeId = 1;
					}
					else
					{
						DeliveryTypeId = 2;
					}
				}
				dtGrid.Rows.Add(invSI.InvSaleInvoiceDetaillist[m].Id, invSI.InvSaleInvoiceDetaillist[m].InvGdnId, invSI.InvSaleInvoiceDetaillist[m].InvGdnDetailId, invSI.InvSaleInvoiceDetaillist[m].SaleOrderId, invSI.InvSaleInvoiceDetaillist[m].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[m].SaleOrder, invSI.InvSaleInvoiceDetaillist[m].OrderDate, invSI.InvSaleInvoiceDetaillist[m].WarehouseId, invSI.InvSaleInvoiceDetaillist[m].ItemCode, invSI.InvSaleInvoiceDetaillist[m].ItemId, invSI.InvSaleInvoiceDetaillist[m].ItemName, invSI.InvSaleInvoiceDetaillist[m].ItemUOMId, invSI.InvSaleInvoiceDetaillist[m].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[m].PackEquivalent, invSI.InvSaleInvoiceDetaillist[m].JobLotId, invSI.InvSaleInvoiceDetaillist[m].ItemQty, invSI.InvSaleInvoiceDetaillist[m].ItemRate - invSI.InvSaleInvoiceDetaillist[m].RateCut, invSI.InvSaleInvoiceDetaillist[m].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[m].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[m].RateUOM, invSI.InvSaleInvoiceDetaillist[m].RateCut, invSI.InvSaleInvoiceDetaillist[m].ItemRate, invSI.InvSaleInvoiceDetaillist[m].ItemDiscount, invSI.InvSaleInvoiceDetaillist[m].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[m].ItemAmount, invSI.InvSaleInvoiceDetaillist[m].FcyAmount, invSI.InvSaleInvoiceDetaillist[m].TaxNameId, invSI.InvSaleInvoiceDetaillist[m].TaxPercent, invSI.InvSaleInvoiceDetaillist[m].TaxAmount, invSI.InvSaleInvoiceDetaillist[m].FreightAmount, invSI.InvSaleInvoiceDetaillist[m].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[m].JournalAmount, invSI.InvSaleInvoiceDetaillist[m].CommissionAmount, invSI.InvSaleInvoiceDetaillist[m].BillAmount, invSI.InvSaleInvoiceDetaillist[m].VehicleNo, invSI.InvSaleInvoiceDetaillist[m].GpNo, invSI.InvSaleInvoiceDetaillist[m].PaymentTermId, invSI.InvSaleInvoiceDetaillist[m].DueDays, invSI.InvSaleInvoiceDetaillist[m].DueDate, (Conversion.ToInt((object)invSI.InvSaleInvoiceDetaillist[m].BillCalculateTypeId) != 2) ? 1 : 2, (invSI.InvSaleInvoiceDetaillist[m].BillCalculateTypeId == 2) ? "On Qty" : "On Weight");
				ItemId = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
				POID = Conversion.ToInt(grd.CurrentRow.Cells["SaleOrderId"].Value);
			}
			dtInvExp.Rows.Clear();
			for (int l = 0; l < invSI.InvSaleInvoiceExpenseList.Count; l++)
			{
				dtInvExp.Rows.Add(invSI.InvSaleInvoiceExpenseList[l].InvRevExpItemId, invSI.InvSaleInvoiceExpenseList[l].Qty, invSI.InvSaleInvoiceExpenseList[l].Rate, invSI.InvSaleInvoiceExpenseList[l].Amount, invSI.InvSaleInvoiceExpenseList[l].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int k = 0; k < invSI.InvSaleInvoiceJournalList.Count; k++)
			{
				dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invSI.InvSaleInvoiceJournalList[k].TransporterSupCustId : invSI.InvSaleInvoiceJournalList[k].ChartofAccountId, invSI.InvSaleInvoiceJournalList[k].ChartofAccountId, invSI.InvSaleInvoiceJournalList[k].JvRemarks, invSI.InvSaleInvoiceJournalList[k].JvPrcnt, invSI.InvSaleInvoiceJournalList[k].JvQty, invSI.InvSaleInvoiceJournalList[k].JvRate, invSI.InvSaleInvoiceJournalList[k].JvDebit, invSI.InvSaleInvoiceJournalList[k].JvCredit);
			}
			dtComm.Rows.Clear();
			for (int j = 0; j < invSI.InvSaleInvoiceCommission.Count; j++)
			{
				dtComm.Rows.Add(invSI.InvSaleInvoiceCommission[j].Id, invSI.InvSaleInvoiceCommission[j].SaleOrderId, invSI.InvSaleInvoiceCommission[j].OrderNo, invSI.InvSaleInvoiceCommission[j].InvSaleInvoiceId, invSI.InvSaleInvoiceCommission[j].CommissionAgentId, invSI.InvSaleInvoiceCommission[j].CommType, invSI.InvSaleInvoiceCommission[j].Rate, invSI.InvSaleInvoiceCommission[j].RateUom, invSI.InvSaleInvoiceCommission[j].CommAmount, invSI.InvSaleInvoiceCommission[j].DebitAccountId, invSI.InvSaleInvoiceCommission[j].Remarks);
			}
			dtPaymentTerm.Rows.Clear();
			for (int i = 0; i < invSI.SaleInvoicePaymentTermsDetailslist.Count; i++)
			{
				dtPaymentTerm.Rows.Add(invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentTermId, invSI.SaleInvoicePaymentTermsDetailslist[i].PrcntOfTotal, invSI.SaleInvoicePaymentTermsDetailslist[i].Amount, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDays, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDate, invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentRemarks);
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
			if (dtPaymentTerm.Rows.Count == 0)
			{
				AddRowInPaymentGrid();
			}
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			if (dtComm.Rows.Count == 0)
			{
				AddRowInCommissionGrid();
			}
			grdComm.DataSource = dtComm;
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdSettingsPaymentTerm();
			grdInvExpSettings();
			gridGLSettings();
			ExpProportion();
			LedgerProportion();
			FreightProportion();
			CommissionProportion();
			CalculateCommissionGrid();
			PropotionateCommissiongrid();
			TotalCommissionAmount();
			BillAmount();
			BindCommGridOrderNo();
			tabControl1.SelectedIndex = 0;
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.ActionId = 2;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			if (invSI.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)invSI.ExchangeRate);
			}
			else if (invSI.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			DocDate.Focus();
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
			if (RecId == 0)
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
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1202: Expected O, but got Unknown
		//IL_1317: Unknown result type (might be due to invalid IL or missing references)
		//IL_131e: Expected O, but got Unknown
		//IL_16d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16de: Expected O, but got Unknown
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1534: Expected O, but got Unknown
		//IL_189c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Expected O, but got Unknown
		//IL_1af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af7: Expected O, but got Unknown
		//IL_1bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb9: Expected O, but got Unknown
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
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				saleInvoice.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (((UltraGridBase)CmbDiscountAccount).ActiveRow != null && Conversion.ToInt(CmbDiscountAccount.Value) > 0 && Conversion.ToDouble((object)txtDiscountAmountHeader.Text) == 0.0)
			{
				throw new Exception("Discount Amount Required When Discount Account Is Present...");
			}
			if (Conversion.ToDouble((object)txtDiscountAmountHeader.Text) != 0.0 && (((UltraGridBase)CmbDiscountAccount).ActiveRow == null || Conversion.ToInt(CmbDiscountAccount.Value) == 0))
			{
				throw new Exception("Discount Ac Required When Discount Amount Is Present...");
			}
			if (Conversion.ToDouble((object)txtDiscountAmountHeader.Text) >= Conversion.ToDouble((object)txtBillAmount.Text))
			{
				throw new Exception("Discount Amount Can not be Greater than or Equal to Bill Amount");
			}
			TotalCommissionAmount();
			BillAmount();
			FreightProportion();
			ExpProportion();
			CommissionProportion();
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
			foreach (GridEXRow r8 in rows)
			{
				if (Conversion.ToDouble(r8.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r8.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			int Comm = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
			string CommType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
			bool IsCommsame = true;
			double CommissionAmt = 0.0;
			rows = grdComm.GetRows();
			foreach (GridEXRow item in rows)
			{
				CommissionAmt += Conversion.ToDouble(item.Cells["CommissionAmount"].Value);
				if (Conversion.ToDouble(item.Cells["CommissionAmount"].Value) > 0.0)
				{
					if (Conversion.ToInt(item.Cells["CommissionAgentId"].Value) == 0)
					{
						MessageBox.Show("Please Select Commission Agent In Commission Grid");
						return;
					}
					if (Conversion.ToInt(item.Cells["CommissionAgentId"].Value) != Comm || Conversion.ToString(item.Cells["CommType"].Value) != CommType)
					{
						IsCommsame = false;
					}
					if (CommissionDebitToExpenses && Conversion.ToInt(item.Cells["DebitAccountId"].Value) == 0)
					{
						MessageBox.Show("Debit Account Feild Required In Commission Grid ...");
						return;
					}
				}
			}
			if (IsCommsame && CommissionAmt > 0.0)
			{
				saleInvoice.CommissionAgentId = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
				saleInvoice.CommissionDebitAcGLId = Conversion.ToInt(grdComm.CurrentRow.Cells["DebitAccountId"].Value);
				saleInvoice.CommissionType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
				saleInvoice.CommRate = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommRate"], (AggregateFunction)3).ToString());
				saleInvoice.CommAmount = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommissionAmount"], (AggregateFunction)2).ToString());
				saleInvoice.CommissionRemarks = Conversion.ToString(grdComm.CurrentRow.Cells["Remarks"].Value);
			}
			double TaxAmount = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToDouble(r7.Cells["TaxAmount"].Value) > 0.0)
				{
					TaxAmount += Conversion.ToDouble(r7.Cells["TaxAmount"].Value);
					if (((UltraGridBase)CmbTaxAccount).ActiveRow == null)
					{
						MessageBox.Show("Tax Account is Required");
						CmbTaxAccount.Focus();
						return;
					}
				}
			}
			saleInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			saleInvoice.DocDate = DocDate.Value;
			saleInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.DocumentTypeId = 1609;
			saleInvoice.ManualBillNo = txtbillno.Text.Trim();
			saleInvoice.RemarksHeader = txtRemarks.Text.Trim();
			saleInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			saleInvoice.SupplierInvoiceDate = DateTime.Now;
			saleInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.SupplierReferenceNo = Conversion.ToString((object)txtrefno.Text.Trim());
			saleInvoice.ScreenName = Conversion.ToString((object)base.Name);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				saleInvoice.TransporterId = Conversion.ToInt(CmbFreightAcCr.Value);
				saleInvoice.TransporterCreditPartyId = 0;
				saleInvoice.TransporterDebitGLId = Conversion.ToInt(cmbFreightAcDr.Value);
				saleInvoice.TransporterDebitPartyId = 0;
			}
			else
			{
				if (Conversion.ToInt(CmbFreightAcCr.Value) > 0)
				{
					saleInvoice.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbFreightAcCr).SelectedRow.Cells[0].Value);
					saleInvoice.TransporterCreditPartyId = Conversion.ToInt(((UltraDropDownBase)CmbFreightAcCr).SelectedRow.Cells[1].Value);
				}
				if (Conversion.ToInt(cmbFreightAcDr.Value) > 0)
				{
					saleInvoice.TransporterDebitGLId = Conversion.ToInt(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[0].Value);
					saleInvoice.TransporterDebitPartyId = Conversion.ToInt(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[1].Value);
				}
			}
			saleInvoice.FreightAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			saleInvoice.FreightRemark = txtFreightRemarks.Text.Trim();
			saleInvoice.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			saleInvoice.IsTaxable = ((saleInvoice.TaxAccountId > 0 && TaxAmount > 0.0) ? true : false);
			saleInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			saleInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			saleInvoice.OrganizationId = UserAccount.OrganizationId;
			saleInvoice.CompanyId = UserAccount.CompanyId;
			saleInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			saleInvoice.EntryUser = UserAccount.ID;
			saleInvoice.EntryDate = DateTime.Now;
			saleInvoice.ModifyDate = DateTime.Now;
			saleInvoice.ModifyUser = UserAccount.ID;
			saleInvoice.DiscountAmount = Conversion.ToDecimal((object)txtDiscountAmountHeader.Text);
			saleInvoice.DiscountAccountId = Conversion.ToInt(CmbDiscountAccount.Value);
			saleInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			saleInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			saleInvoice.DeliveryTerm = Conversion.ToString((object)((Control)(object)cmbDeliveryTerm).Text);
			saleInvoice.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
			saleInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			saleInvoice.DueDate = Conversion.ToDateTime((object)DueDate.Value);
			saleInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			saleInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			rows = grd.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				InvSaleInvoiceDetail Pd = new InvSaleInvoiceDetail();
				Pd.LineId = r6.RowIndex + 1;
				Pd.Id = Conversion.ToInt(r6.Cells["Id"].Value);
				Pd.SaleOrderId = Conversion.ToInt(r6.Cells["SaleOrderId"].Value);
				Pd.SaleOrderDetailId = Conversion.ToInt(r6.Cells["SaleOrderDetailId"].Value);
				Pd.SaleOrder = Conversion.ToInt(r6.Cells["SaleOrder"].Value);
				Pd.OrderDate = Conversion.ToDateTime(r6.Cells["OrderDate"].Value);
				Pd.UOMCodeItem = Conversion.ToString(r6.Cells["PackUom"].Value);
				if (DeliveryTypeId == 1 && Pd.SaleOrderId == 0)
				{
					MessageBox.Show("SaleOrderId Field Required");
					return;
				}
				Pd.InvGdnDetailId = Conversion.ToInt(r6.Cells["GdnDetailId"].Value);
				if (Pd.InvGdnDetailId == 0)
				{
					MessageBox.Show("InvGdnDetailId not found");
					return;
				}
				Pd.InvGdnId = Conversion.ToInt(r6.Cells["GdnId"].Value);
				if (Pd.InvGdnId == 0)
				{
					MessageBox.Show("Gdn Id not found");
					return;
				}
				Pd.ItemId = Conversion.ToInt(r6.Cells["ItemId"].Value);
				if (Pd.ItemId == 0)
				{
					MessageBox.Show("Item Required");
					return;
				}
				Pd.ItemQty = Conversion.ToDouble(r6.Cells["ItemQty"].Value);
				if (Pd.ItemQty == 0.0)
				{
					MessageBox.Show("ItemQty mustbe greater than zero");
					return;
				}
				Pd.ItemUOMId = Conversion.ToInt(r6.Cells["PackUomId"].Value);
				if (Pd.ItemUOMId == 0)
				{
					MessageBox.Show("ItemUOMId Field Required");
					return;
				}
				Pd.JobLotId = Conversion.ToInt(r6.Cells["JobLotId"].Value);
				if (Pd.JobLotId == 0)
				{
					MessageBox.Show("JobLot Required");
					return;
				}
				Pd.GrossWeight = Pd.ItemQty;
				Pd.AdLsWeight = 0.0;
				Pd.NetBillWeight = Pd.ItemQty;
				Pd.NetStockWeight = Pd.ItemQty;
				Pd.ItemRate = Conversion.ToDouble(r6.Cells["NetRate"].Value);
				if (Pd.ItemRate == 0.0)
				{
					MessageBox.Show("ItemRate Field Required");
					return;
				}
				Pd.RateUOM = Conversion.ToInt(r6.Cells["RateEquivalent"].Value);
				if (DeliveryTypeId != 1)
				{
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r6.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + Pd.RateUOM + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("RateUom of the Item " + Conversion.ToString((object)r6.Cells["ItemId"].Text) + " in row No " + (r6.RowIndex + 1) + " is not define please check ");
							return;
						}
						Pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
					}
				}
				else
				{
					Pd.UomScheduleIdRate = Conversion.ToInt(r6.Cells["RateUomId"].Value);
				}
				if (Pd.UomScheduleIdRate == 0)
				{
					MessageBox.Show("Rate UOM Required");
					return;
				}
				Pd.RateCut = Conversion.ToDouble(r6.Cells["AddLessRate"].Value);
				Pd.ItemAmount = Math.Round(Conversion.ToDouble(r6.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Pd.ItemAmount == 0.0)
				{
					MessageBox.Show("ItemAmount Field Required");
					return;
				}
				Pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				Pd.FcyAmount = Conversion.ToDecimal(r6.Cells["FcyAmount"].Value);
				Pd.WarehouseId = Conversion.ToInt(r6.Cells["WarehouseId"].Value);
				if (Pd.WarehouseId == 0)
				{
					MessageBox.Show("Warehouse Required");
					return;
				}
				Pd.ItemDiscount = Conversion.ToDouble(r6.Cells["DiscPrcnt"].Value);
				Pd.ItemDiscountAmount = Conversion.ToDouble(r6.Cells["DiscAmount"].Value);
				Pd.TaxNameId = Conversion.ToInt(r6.Cells["TaxNameId"].Value);
				Pd.TaxPercent = Conversion.ToDouble(r6.Cells["TaxPrcnt"].Value);
				Pd.TaxAmount = Conversion.ToDouble(r6.Cells["TaxAmount"].Value);
				Pd.IsTaxable = ((Pd.TaxNameId > 0 && Pd.TaxPercent > 0.0 && Pd.TaxAmount > 0.0) ? "true" : "false");
				Pd.BillAmount = Math.Round(Conversion.ToDouble(r6.Cells["BillAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (!FreightDebitToExpenses)
				{
					Pd.FreightAmount = Conversion.ToDouble(r6.Cells["Freights"].Value);
				}
				Pd.JournalAmount = Conversion.ToDouble(r6.Cells["Journal"].Value);
				Pd.ExpenseAmount = Conversion.ToDouble(r6.Cells["Expense"].Value);
				if (!CommissionDebitToExpenses)
				{
					Pd.CommissionAmount = Conversion.ToDouble(r6.Cells["Commission"].Value);
				}
				Pd.GpDate = DateTime.Now;
				Pd.GpNo = Conversion.ToInt(r6.Cells["GpNo"].Value);
				Pd.VehicleNo = Conversion.ToString(r6.Cells["VehicleNo"].Value);
				Pd.PaymentTermId = Conversion.ToInt(r6.Cells["PaymentTermId"].Value);
				Pd.BillCalculateTypeId = Conversion.ToInt(r6.Cells["BillTypeId"].Value);
				Pd.DueDays = Conversion.ToInt(r6.Cells["DueDays"].Value);
				Pd.DueDate = Conversion.ToDateTime(r6.Cells["DueDate"].Value);
				Pd.VehicleNo = Conversion.ToString(r6.Cells["VehicleNo"].Value);
				saleInvoice.InvSaleInvoiceDetaillist.Add(Pd);
			}
			saleInvoice.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			saleInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r4.Cells["Amount"].Value) != 0.0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r4.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					saleInvoice.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			saleInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ2 = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					}
					else
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ2.JvRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					PJ2.JvPrcnt = Conversion.ToDouble(r3.Cells["Percentage"].Value);
					PJ2.JvQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					PJ2.JvRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PJ2.JvDebit = Conversion.ToDouble(r3.Cells["Debit"].Value);
					PJ2.JvCredit = Conversion.ToDouble(r3.Cells["Credit"].Value);
					saleInvoice.InvSaleInvoiceJournalList.Add(PJ2);
				}
			}
			decimal PaymentDetailAmount = default(decimal);
			decimal PrcntOfTotal = default(decimal);
			saleInvoice.SaleInvoicePaymentTermsDetailslist = new List<SaleInvoicePaymentTermsDetail>();
			double sumOfPaymentDetailAmount = Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", string.Empty));
			if (grdPaymentTerm.GetRows().Count() > 0 && sumOfPaymentDetailAmount > 0.0)
			{
				rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDecimal(r2.Cells["Amount"].Value) > 0m)
					{
						SaleInvoicePaymentTermsDetail obj = new SaleInvoicePaymentTermsDetail();
						obj.PaymentTermId = Conversion.ToInt(r2.Cells["PaymentTermId"].Value);
						obj.PrcntOfTotal = Conversion.ToDecimal(r2.Cells["%ofTotal"].Value);
						PrcntOfTotal += obj.PrcntOfTotal;
						obj.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
						PaymentDetailAmount += obj.Amount;
						obj.DueDays = Conversion.ToInt(r2.Cells["DueDays"].Value);
						obj.DueDate = Conversion.ToDateTime(r2.Cells["DueDate"].Value);
						obj.PaymentRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						if (obj.PaymentTermId <= 0)
						{
							MessageBox.Show("Payment Term Required in row#" + (r2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							((Control)(object)grdPaymentTerm).Focus();
							return;
						}
						if (obj.PaymentTermId == 2 && obj.DueDays <= 0)
						{
							MessageBox.Show("Due Days Required In case Of Credit row in row#" + (r2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							((Control)(object)grdPaymentTerm).Focus();
							return;
						}
						saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(obj);
					}
				}
			}
			else
			{
				SaleInvoicePaymentTermsDetail obj2 = new SaleInvoicePaymentTermsDetail();
				obj2.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
				obj2.PrcntOfTotal = 100m;
				PrcntOfTotal = 100m;
				obj2.Amount = Conversion.ToDecimal((object)(100.0 * saleInvoice.BillAmount / 100.0));
				PaymentDetailAmount = obj2.Amount;
				obj2.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				obj2.DueDate = Conversion.ToDateTime((object)DueDate.Value);
				obj2.PaymentRemarks = "";
				saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(obj2);
			}
			if (PrcntOfTotal != 100m)
			{
				MessageBox.Show("Payment Detail Total% not equal to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			if (PaymentDetailAmount != Conversion.ToDecimal((object)saleInvoice.BillAmount))
			{
				MessageBox.Show("Payment Detail Amount " + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Invoice Amount " + saleInvoice.BillAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			saleInvoice.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			rows = grdComm.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["CommissionAgentId"].Value.ToString()) != 0 && Conversion.ToDouble(r.Cells["CommissionAmount"].Value) > 0.0)
				{
					InvSaleInvoiceCommission PJ = new InvSaleInvoiceCommission();
					PJ.Id = Conversion.ToInt(r.Cells["Id"].Value);
					PJ.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					PJ.InvSaleInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					PJ.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
					PJ.CommissionAgentId = Conversion.ToInt(r.Cells["CommissionAgentId"].Value);
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Percent")
					{
						PJ.CommType = 2.0;
					}
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Flat")
					{
						PJ.CommType = 1.0;
					}
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Comm Weight")
					{
						PJ.CommType = 3.0;
					}
					if (Conversion.ToDouble(r.Cells["CommType"].Value) > 0.0)
					{
						PJ.CommType = Conversion.ToDouble(r.Cells["CommType"].Value);
					}
					PJ.Rate = Conversion.ToDouble(r.Cells["CommRate"].Value);
					PJ.RateUom = Conversion.ToDouble(r.Cells["CommUom"].Value);
					PJ.CommAmount = Conversion.ToDouble(r.Cells["CommissionAmount"].Value);
					PJ.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					saleInvoice.InvSaleInvoiceCommission.Add(PJ);
				}
			}
			saleInvoice.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				saleInvoice.AttachmentsList.Add(attach);
			}
			saleInvoice.AttachmentsValues = AttachmentsValues;
			saleInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				saleInvoice.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				for (int j = 0; j < AT.RemovedAttachmentListInUpdateCase.Count; j++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[j].Path);
					delete.ScreenName = base.Name;
					saleInvoice.DeleteAttachmentsList.Add(delete);
				}
			}
			int sucess = InvSaleInvoice.Save(saleInvoice);
			if (RecId == 0)
			{
				MessageBox.Show("Record saved Successfully" + saleInvoice.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully" + saleInvoice.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 1609, 0);
				}
			}
			Reset();
			if (ChkBok.Checked)
			{
				RecId = sucess;
				VoucherHeadIdGet();
				RecId = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1609);
			}
			if (ChkPrintSlip.Checked)
			{
				CommonServices.SaleInvoicelSlip_1609(sucess);
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record cannot be  Delete beacause Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 1609,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
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
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 1609;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			PI.NoOfRecords = NoOfRecords;
			if (drdocdate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.FromDate = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ApprovedDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ApprovedDateTo = txtToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dthistory = InvSaleInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("ManualBillNo");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("PaymentTerm");
				dt.Columns.Add("TransporterCreditAc", typeof(string));
				dt.Columns.Add("TransporterDebitAc", typeof(string));
				dt.Columns.Add("FreightAmount", typeof(double));
				dt.Columns.Add("TaxAccount", typeof(string));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments");
				dt.Columns.Add("Remarks");
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["CustomerName"], dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["DeliveryTerm"], dthistory.Rows[i]["PaymentTerm"], dthistory.Rows[i]["TransporterCreditAc"], dthistory.Rows[i]["TransporterDebitAc"], dthistory.Rows[i]["FreightAmount"], Conversion.ToString(dthistory.Rows[i]["TaxAccount"]), Conversion.ToDouble(dthistory.Rows[i]["BillAmount"]), dthistory.Rows[i]["UserName"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUserName"], dthistory.Rows[i]["ApprovedDate"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocNo"].Caption = "InvoiceNo";
			grdHistory.RootTable.Columns["DocDate"].Caption = "InvoiceDate";
			grdHistory.RootTable.Columns["TransporterDebitAc"].Caption = "FreightAccountDr";
			grdHistory.RootTable.Columns["TransporterCreditAc"].Caption = "FreightAccountCr";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "1609-Slip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "1609-Slip";
			grdHistory.RootTable.Columns["View"].Width = 85;
			grdHistory.RootTable.Columns["View"].Position = 1;
			grdHistory.RootTable.Columns.Add("Print1");
			grdHistory.RootTable.Columns["Print1"].Caption = "1609A-Slip";
			grdHistory.RootTable.Columns["Print1"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print1"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print1"].ButtonText = "1609A_Slip";
			grdHistory.RootTable.Columns["Print1"].Width = 85;
			grdHistory.RootTable.Columns["Print1"].Position = 2;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 70;
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
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
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoicelSlip_1609(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 1609);
			}
			if (e.Column.Key == "Print1")
			{
				CommonServices.SaleInvoicelSlip_1609A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("SaleOrder");
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("AddLessRate", typeof(double));
			dtdetail.Columns.Add("NetRate", typeof(double));
			dtdetail.Columns.Add("DiscPrcnt", typeof(double));
			dtdetail.Columns.Add("DiscAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("FcyAmount", typeof(double));
			dtdetail.Columns.Add("TaxName", typeof(string));
			dtdetail.Columns.Add("TaxPrcnt", typeof(double));
			dtdetail.Columns.Add("TaxAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtdetail.Columns.Add("BillAmount", typeof(double));
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("PaymentTerm");
			dtdetail.Columns.Add("DueDays");
			dtdetail.Columns.Add("DueDate", typeof(DateTime));
			dtdetail.Columns.Add("BillType");
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].ItemRate - obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].UOMCodeRate, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].ItemDiscount, obj.InvSaleInvoiceDetaillist[i].ItemDiscountAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].TaxName, obj.InvSaleInvoiceDetaillist[i].TaxPercent, obj.InvSaleInvoiceDetaillist[i].TaxAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount, obj.InvSaleInvoiceDetaillist[i].BillAmount, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].PaymentTerm, obj.InvSaleInvoiceDetaillist[i].DueDays, Conversion.ToDateTime((object)obj.InvSaleInvoiceDetaillist[i].DueDate).ToShortDateString(), (obj.InvSaleInvoiceDetaillist[i].BillCalculateTypeId == 1) ? "On Weight" : "On Qty");
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetail.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grdDetail.RootTable.Columns["ItemUOM"].Caption = "Uom";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["DiscPrcnt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DiscPrcnt"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TaxPrcnt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxPrcnt"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar2_Load(null, null);
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
			txtFromdateHistory.Focus();
		}
	}

	private void btnSaleHistory_Click(object sender, EventArgs e)
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
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1609);
				}
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
			RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		GetDetailGrdByHeadId();
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromdateHistory.Value = DateTime.Now;
			txtToDateHistory.Value = DateTime.Now;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			txtFromdateHistory.Focus();
			grdHistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryComboBind();
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "grdHistory")
			{
				txtFromdateHistory.Focus();
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
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Flat")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					txtcommamount.Text = CommRate3.ToString();
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Percent" || ((Control)(object)cmbcommtype).Text == "Percentage")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
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
				if (((Control)(object)cmbcommtype).Text == "Comm Weight")
				{
					double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)cmbcommuom).Text.Trim());
						double CommAmt = num / UOM * CommRate;
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

	private void CalculateItemDiscount()
	{
		try
		{
			grd.UpdateData();
			if (grd.CurrentRow.Cells.Count > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					double ItemPercentage = 0.0;
					double ItemDiscount = 0.0;
					double ItemAmount = 0.0;
					ItemPercentage = Conversion.ToDouble((object)item.Cells["ItemDiscount"].Text);
					ItemAmount = Conversion.ToDouble((object)item.Cells["ItemAmount"].Text);
					if (ItemPercentage > 0.0)
					{
						ItemDiscount = ItemAmount * ItemPercentage / 100.0;
						item.Cells["DiscAmount"].Text = ItemDiscount.ToString();
					}
				}
			}
			grd.UpdateData();
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
			if (grd.GetRows().Count() > 0)
			{
				double BillAmount = 0.0;
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double TaxAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString());
				double JournalDebit = 0.0;
				double JournalCredit = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0)
					{
						JournalDebit += Conversion.ToDouble(r.Cells["Debit"].Value);
						JournalCredit += Conversion.ToDouble(r.Cells["Credit"].Value);
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(CmbFreightAcCr.Value) > 0 && txtSupplierGLId.Text.ToString() == Conversion.ToString(CmbFreightAcCr.Value))
					{
						BillAmount -= Conversion.ToDouble((object)txtFreightAmount.Text);
					}
				}
				else if (Conversion.ToInt(CmbFreightAcCr.Value) > 0 && Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(CmbFreightAcCr.Value))
				{
					BillAmount -= Conversion.ToDouble((object)txtFreightAmount.Text);
				}
				if (Conversion.ToInt(cmbFreightAcDr.Value) > 0)
				{
					if (!SubsidiaryAccountAllownOnVouchers)
					{
						if (txtSupplierGLId.Text.ToString() == Conversion.ToString(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[0].Value))
						{
							BillAmount += Conversion.ToDouble((object)txtFreightAmount.Text);
						}
					}
					else if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(cmbFreightAcDr.Value))
					{
						BillAmount += Conversion.ToDouble((object)txtFreightAmount.Text);
					}
				}
				BillAmount = BillAmount + ItemAmount + ExpAmount + TaxAmount;
				SupplierGLCreditAmount = JournalCredit;
				SupplierGLDebitAmount = JournalDebit;
				double GlDifference = Math.Round(SupplierGLCreditAmount - SupplierGLDebitAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double DiffGl = Math.Abs(GlDifference);
				BillAmount = ((!(GlDifference < 0.0)) ? (BillAmount + GlDifference) : (BillAmount - DiffGl));
				double PartyCommAmount = 0.0;
				rows = grdComm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(item.Cells["CommissionAgentId"].Value))
					{
						PartyCommAmount += Conversion.ToDouble((object)item.Cells["CommissionAmount"].Text);
					}
				}
				double DiscAmountHeader = Conversion.ToDouble((object)txtDiscountAmountHeader.Text);
				BillAmount = BillAmount - PartyCommAmount - DiscAmountHeader;
				txtBillAmount.Text = BillAmount.ToString("#,##0.####");
				txtFcyAmount.Text = (BillAmount / Conversion.ToDouble((object)txtExchangeRate.Text)).ToString("#,##0.####");
				PaymentDetailRecalculate();
			}
			else
			{
				txtFcyAmount.Text = "0";
			}
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
			double TotalQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					obj.Cells["Expense"].Value = TotalAmount / TotalQty * Qty;
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
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)txtFreightAmount.Text);
			if (CreditForFreight > 0.0 && Conversion.ToInt(CmbFreightAcCr.Value) > 0 && Conversion.ToInt(cmbFreightAcDr.Value) <= 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					double freight = Math.Round(CreditForFreight / NetWeight * Weight, 4);
					obj.Cells["Freights"].Value = freight;
				}
			}
			else if (!(CreditForFreight > 0.0) || Conversion.ToInt(cmbFreightAcDr.Value) > 0)
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
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Credit"], (AggregateFunction)2).ToString()) + TotalAmount;
			double DebitForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Debit"], (AggregateFunction)2).ToString());
			if (DebitForFreight > CreditForFreight)
			{
				double TotalDiffernce = DebitForFreight - CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
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
				double ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
				double Expense = Conversion.ToDouble(r.Cells["Expense"].Value);
				double Commission = Conversion.ToDouble(r.Cells["Commission"].Value);
				double TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
				double Freight = Conversion.ToDouble(r.Cells["Freights"].Value);
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = ItemAmount;
					}
					else
					{
						r.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense + TaxAmount - Freight - Commission, 4);
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense + TaxAmount - Freight - Commission, 4);
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
	}

	private void CmbFreightAcCr_Leave(object sender, EventArgs e)
	{
		try
		{
			int FrDrId = Conversion.ToInt(cmbFreightAcDr.Value);
			int FrCrId = Conversion.ToInt(CmbFreightAcCr.Value);
			if (FrCrId > 0 && FrDrId > 0 && FrDrId == FrCrId)
			{
				((Control)(object)CmbFreightAcCr).Text = string.Empty;
				MessageBox.Show("Freight Credit And Freight Debit Account Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbFreightAcDr_Leave(object sender, EventArgs e)
	{
		try
		{
			int FrCrId = Conversion.ToInt(CmbFreightAcCr.Value);
			int FrDrId = Conversion.ToInt(cmbFreightAcDr.Value);
			if (FrDrId > 0 && FrDrId > 0 && FrDrId == FrCrId)
			{
				((Control)(object)cmbFreightAcDr).Text = string.Empty;
				MessageBox.Show("Freight Credit And Freight Debit Account Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			FreightProportion();
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
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtDiscountAmountHeader.Text) >= Conversion.ToDouble((object)txtBillAmount.Text))
			{
				txtDiscountAmountHeader.Text = 0.ToString("#,##.###");
				MessageBox.Show("Discount Amount Can not be Greater than or Equal to Bill Amount");
			}
			txtExchangeRate_TextChanged(null, null);
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
			LoadGDN = new LoadGdnTrading_Engr(UserAccount);
			LoadGDN.DocumentTypeId = 1612;
			LoadGDN.ShowDialog();
			LoadGDN.BringToFront();
			LoadInGridDetail();
			BindCommGridOrderNo();
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
			SaleOrderId = 0;
			DataTable invGdn = InvSaleInvoice.GetGdnLoad_Eng(IDs);
			if (invGdn.Rows.Count <= 0)
			{
				return;
			}
			if (((UltraGridBase)cmbsuppliername).ActiveRow != null && Conversion.ToInt(invGdn.Rows[0]["SupplierCustomerId"]) != Conversion.ToInt(cmbsuppliername.Value))
			{
				throw new Exception("You Can't Load GDN Of Different Customer At Same Time");
			}
			cmbsuppliername.Value = invGdn.Rows[0]["SupplierCustomerId"];
			((Control)(object)cmbsuppliername).Enabled = false;
			((Control)(object)cmbDeliveryTerm).Text = invGdn.Rows[0]["DeliveryTerm"].ToString();
			CmbPaymentTerm.Value = invGdn.Rows[0]["PaymentTermsId"];
			txtDueDays.Text = Conversion.ToString(invGdn.Rows[0]["OrderDueDays"]);
			if (DeliveryTypeId != 1)
			{
				((Control)(object)cmbDeliveryTerm).Enabled = true;
				((Control)(object)CmbPaymentTerm).Enabled = true;
			}
			else
			{
				((Control)(object)cmbDeliveryTerm).Enabled = false;
				((Control)(object)CmbPaymentTerm).Enabled = false;
			}
			if (Conversion.ToString(invGdn.Rows[0]["AccountTitle"]) != null && Conversion.ToString(invGdn.Rows[0]["AccountTitle"]) != string.Empty)
			{
				((Control)(object)CmbFreightAcCr).Text = Conversion.ToString(invGdn.Rows[0]["AccountTitle"]);
			}
			txtFreightAmount.Text = Conversion.ToDouble(invGdn.Rows[0]["CarriageAmount"]).ToString("#,##0.###");
			txtRemarks.Text = invGdn.Rows[0]["RemarksHeader"].ToString();
			dtComm.Clear();
			for (int j = 0; j < invGdn.Rows.Count; j++)
			{
				int saleOrderId = Conversion.ToInt((object)invGdn.Rows[j]["SaleOrderId"].ToString());
				bool orderExists = false;
				foreach (DataRow row in dtComm.Rows)
				{
					int orderId = Conversion.ToInt((object)row["OrderId"].ToString());
					if (saleOrderId == orderId)
					{
						orderExists = true;
						break;
					}
				}
				if (!orderExists && Conversion.ToInt(invGdn.Rows[j]["BrokerAgentSupCustId"]) != 0)
				{
					dtComm.Rows.Add(0, invGdn.Rows[j]["SaleOrderId"], invGdn.Rows[j]["SaleOrder"], 0, invGdn.Rows[j]["BrokerAgentSupCustId"], invGdn.Rows[j]["CommissionType"].ToString(), invGdn.Rows[j]["CommRate"].ToString(), invGdn.Rows[j]["UomScheduleIdCmRate"].ToString(), invGdn.Rows[j]["CommAmount"].ToString(), invGdn.Rows[j]["CommissionRemarks"].ToString());
				}
			}
			bool flag = false;
			for (int i = 0; i < invGdn.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int k = 0; k < rows.Length; k++)
					{
						if (Conversion.ToInt(rows[k].Cells["GdnDetailId"].Value) == Conversion.ToInt(invGdn.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				double itemAmount = 0.0;
				if (!flag)
				{
					dtGrid.Rows.Add(0, invGdn.Rows[i]["invGdnId"], invGdn.Rows[i]["Id"], invGdn.Rows[i]["SaleOrderId"], invGdn.Rows[i]["SaleOrderDetailId"], invGdn.Rows[i]["SaleOrder"], invGdn.Rows[i]["OrderDate"], invGdn.Rows[i]["WarehouseId"], invGdn.Rows[i]["ItemCode"], invGdn.Rows[i]["ItemId"], invGdn.Rows[i]["ItemName"], invGdn.Rows[i]["ItemUomId"], invGdn.Rows[i]["PackUom"], invGdn.Rows[i]["PackEquivalent"], invGdn.Rows[i]["JobLotId"], invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["ItemRate"], invGdn.Rows[i]["OrderItemRateUOMId"], (DeliveryTypeId != 1) ? ((object)1) : invGdn.Rows[i]["RateUom"], (DeliveryTypeId != 1) ? ((object)1) : invGdn.Rows[i]["RateEquivalent"], 0, invGdn.Rows[i]["ItemRate"], 0, 0, itemAmount = Math.Round(Conversion.ToDouble(invGdn.Rows[i]["ItemAmount"]), 4), 0, invGdn.Rows[i]["TaxNameId"], invGdn.Rows[i]["TaxPercent"], itemAmount * Conversion.ToDouble(invGdn.Rows[i]["TaxPercent"]) / 100.0, 0, 0, 0, 0, 0, invGdn.Rows[i]["VehicleNo"], invGdn.Rows[i]["GpNo"], invGdn.Rows[i]["PaymentTermsId"], invGdn.Rows[i]["OrderDueDays"], Conversion.ToDateTime(invGdn.Rows[i]["OrderDueDate"]).ToShortDateString(), invGdn.Rows[i]["BillCalculateTypeId"], (Conversion.ToInt(invGdn.Rows[i]["BillCalculateTypeId"]) == 2) ? "On Qty" : "On Weight");
				}
				flag = false;
			}
			SaleOrderId = Conversion.ToInt(invGdn.Rows[0]["SaleOrderId"]);
			ItemId = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
			POID = Conversion.ToInt(grd.CurrentRow.Cells["SaleOrderId"].Value);
			if (DeliveryTypeId == 1)
			{
				LoadPaymentDetailBySaleOrderIds(Conversion.ToString(invGdn.Rows[0]["SaleOrderId"]));
				if (dtPaymentTerm.Rows.Count == 0 || dtPaymentTerm == null)
				{
					AddRowInPaymentGrid();
				}
				grdPaymentTerm.RetrieveStructure();
				grdSettingsPaymentTerm();
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadExpensesFromGdn(string ID)
	{
		try
		{
			DataTable item = InvSaleInvoice.GetExpensesFromGdn(ID);
			if (item.Rows.Count > 0)
			{
				if (Conversion.ToDouble(dtInvExp.Compute("Sum(Amount)", string.Empty)) == 0.0)
				{
					dtInvExp.Rows.Clear();
				}
				for (int i = 0; i < item.Rows.Count; i++)
				{
					dtInvExp.Rows.Add(item.Rows[i]["ItemId"], item.Rows[i]["Qty"], 0, 0, Conversion.ToString(item.Rows[i]["Remarks"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPaymentDetailBySaleOrderIds(string Ids)
	{
		try
		{
			DataTable item = InvSaleInvoice.SaleOrderPaymentTermDetailBySoIds(Ids);
			if (item.Rows.Count > 0)
			{
				if (Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", string.Empty)) == 0.0)
				{
					dtPaymentTerm.Rows.Clear();
				}
				for (int i = 0; i < item.Rows.Count; i++)
				{
					dtPaymentTerm.Rows.Add(item.Rows[i]["PaymentTermId"], item.Rows[i]["PrcntOfTotal"], item.Rows[i]["Amount"], item.Rows[i]["DueDays"], item.Rows[i]["DueDate"], Conversion.ToString(item.Rows[i]["PaymentRemarks"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			if (LoadGDN.dtGDNS.Rows.Count <= 0)
			{
				return;
			}
			string GdnIds = "";
			for (int i = 0; i < LoadGDN.dtGDNS.Rows.Count; i++)
			{
				GdnIds = GdnIds + "," + int.Parse(LoadGDN.dtGDNS.Rows[i][0].ToString());
			}
			if (DeliveryTypeId > 0 && DeliveryTypeId != Conversion.ToInt(LoadGDN.dtGDNS.Rows[0][1]))
			{
				MessageBox.Show("Already Loaded Rows Have Different Delivery Type. So you can't Load New Rows", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			DeliveryTypeId = Conversion.ToInt(LoadGDN.dtGDNS.Rows[0][1]);
			LoadDataDetailGridAgainstGP(GdnIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			if (dtComm.Rows.Count == 0 || dtComm == null)
			{
				AddRowInCommissionGrid();
			}
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			LoadExpensesFromGdn(GdnIds);
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			TotalCommissionAmount();
			CalculateCommissionGrid();
			PropotionateCommissiongrid();
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPaymentTerm).Name);
			if (fs != null)
			{
				grdPaymentTerm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdComm).Name);
			if (fs != null)
			{
				grdComm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInvExp).Name);
			if (fs != null)
			{
				grdInvExp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGLedger).Name);
			if (fs != null)
			{
				grdGLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1609);
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

	private void btnSlipFormat2_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.SaleInvoicelSlip_1609A(RecId);
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.SaleInvoicelSlip_1609(RecId);
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
			if (dtSupplier.Rows.Count > 0 && ((UltraGridBase)cmbsuppliername).ActiveRow != null)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(cmbsuppliername.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
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

	private void txtDiscountAmountHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					cmbsuppliername.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					txtFromdateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (btnDelete.Visible && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbsuppliername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdInvExp).Focus();
					}
					else if (((Control)(object)grdInvExp).Focused)
					{
						((Control)(object)grdGLedger).Focus();
					}
					else if (((Control)(object)grdGLedger).Focused)
					{
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (((Control)(object)grdPaymentTerm).Focused)
					{
						((Control)(object)grdComm).Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbsuppliername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					toolStripButton1_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					toolStripButton1_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnSlipFormat2_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				txtFromdateHistory.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNewHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefreshHistory_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save in form tab and for show data in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 1608");
			dt.Rows.Add("Alt+2", "For Print Slip 1608A");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null && (!e.Control || e.KeyCode != Keys.Space) && (!e.Control || e.KeyCode != Keys.D) && (!e.Control || e.KeyCode != Keys.Delete) && e.Control)
			{
				_ = e.KeyCode;
				_ = 13;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (grdHistory.CurrentColumn.Key == "View")
				{
					CommonServices.SaleInvoicelSlip_1609(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 1609);
				}
				else if (grdHistory.CurrentColumn.Key == "Print1")
				{
					CommonServices.SaleInvoicelSlip_1609A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 1609);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.SaleInvoicelSlip_1609(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInvExp.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
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
				if (grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			((Control)(object)grdInvExp).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdComm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdComm.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdComm.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
					{
						if (grdComm.GetRows().Length == 1)
						{
							MessageBox.Show("You Can not Delete Row Because The Above Entry Is From Sale Order");
							return;
						}
						dtComm.Rows[item.RowIndex].Delete();
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
						if (dtComm.Rows.Count == 0)
						{
							AddRowInCommissionGrid();
						}
					}
					else
					{
						dtComm.Rows[item.RowIndex].Delete();
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
						if (dtComm.Rows.Count == 0)
						{
							AddRowInCommissionGrid();
						}
					}
				}
				if (grdComm.CurrentColumn.Key == "Add")
				{
					AddRowInCommissionGrid();
				}
				grdComm.UpdateData();
				PropotionateCommissiongrid();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
				{
					if (grdComm.GetRows().Length == 1)
					{
						MessageBox.Show("You Can not Delete Row Because The Above Entry Is From Sale Order");
						return;
					}
					dtComm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
				else
				{
					dtComm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
				grdComm.UpdateData();
				PropotionateCommissiongrid();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInCommissionGrid();
				grdComm.UpdateData();
				PropotionateCommissiongrid();
				BillAmount();
			}
			((Control)(object)grdComm).Focus();
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
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1)
			{
				txtDueDays.Enabled = false;
				txtDueDays.Text = "0";
				DueDate.Value = DocDate.Value;
			}
			else
			{
				txtDueDays.Enabled = true;
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
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Expected O, but got Unknown
		//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Expected O, but got Unknown
		//IL_11a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ac: Expected O, but got Unknown
		//IL_11b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c3: Expected O, but got Unknown
		//IL_1600: Unknown result type (might be due to invalid IL or missing references)
		//IL_160a: Expected O, but got Unknown
		//IL_1617: Unknown result type (might be due to invalid IL or missing references)
		//IL_1621: Expected O, but got Unknown
		//IL_162e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1638: Expected O, but got Unknown
		//IL_196a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1974: Expected O, but got Unknown
		//IL_1981: Unknown result type (might be due to invalid IL or missing references)
		//IL_198b: Expected O, but got Unknown
		//IL_1998: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a2: Expected O, but got Unknown
		//IL_1dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc9: Expected O, but got Unknown
		//IL_1dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de0: Expected O, but got Unknown
		//IL_1ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Expected O, but got Unknown
		//IL_2190: Unknown result type (might be due to invalid IL or missing references)
		//IL_219a: Expected O, but got Unknown
		//IL_21a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b1: Expected O, but got Unknown
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c8: Expected O, but got Unknown
		//IL_8245: Unknown result type (might be due to invalid IL or missing references)
		//IL_824f: Expected O, but got Unknown
		//IL_8273: Unknown result type (might be due to invalid IL or missing references)
		//IL_827d: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdComm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
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
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdComm = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel29 = new System.Windows.Forms.Panel();
		this.grdPaymentTerm = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.txtFreightRemarks = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbDiscountAccount = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbFreightAcDr = new UltraCombo();
		this.txtDiscountAmountHeader = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbFreightAcCr = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbDeliveryTerm = new UltraCombo();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbCommissionDebitAc = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.lblDebitAccount = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtcommremarks = new System.Windows.Forms.RichTextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.cmbcommtype = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbcommuom = new UltraCombo();
		this.txtrefno = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbcommagent = new UltraCombo();
		this.cmbsuppliername = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnSlipFormat2 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel24 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel26 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label29 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel30 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label54 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdComm).BeginInit();
		this.panel19.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel23.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel17.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel21.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreightAcDr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAcCr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel24.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel26.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel31.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel30.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel13.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1150, 708);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1150, 708);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1142, 679);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel7.AutoScroll = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel4);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1136, 673);
		this.panel7.TabIndex = 4;
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 494);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1136, 179);
		this.panel9.TabIndex = 3;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 21);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1136, 158);
		this.panel11.TabIndex = 2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1136, 158);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1136, 21);
		this.panel5.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1097, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(74, 14);
		this.label9.TabIndex = 43;
		this.label9.Text = "Detail  Grid";
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 220);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1136, 274);
		this.panel4.TabIndex = 2;
		this.panel10.Controls.Add(this.panel18);
		this.panel10.Controls.Add(this.panel22);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(603, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(533, 274);
		this.panel10.TabIndex = 1;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdComm);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 140);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(533, 134);
		this.panel18.TabIndex = 1;
		this.panel18.Visible = false;
		((ScrollableBase)this.grdComm).BackColor = System.Drawing.Color.White;
		grdComm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdComm.DesignTimeLayout = grdComm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdComm).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdComm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdComm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdComm).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdComm).Name = "grdComm";
		((ScrollableBase)this.grdComm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdComm).Size = new System.Drawing.Size(533, 113);
		((System.Windows.Forms.Control)(object)this.grdComm).TabIndex = 0;
		this.grdComm.TotalRow = (InheritableBoolean)1;
		this.grdComm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdComm).VisualStyle = (VisualStyle)4;
		this.grdComm.CellUpdated += new ColumnActionEventHandler(grdComm_CellUpdated);
		this.grdComm.UpdatingCell += new UpdatingCellEventHandler(grdComm_UpdatingCell);
		this.grdComm.ColumnButtonClick += new ColumnActionEventHandler(grdComm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdComm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdComm_KeyDown);
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.ctrlGrdBar5);
		this.panel19.Controls.Add(this.label6);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(533, 21);
		this.panel19.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(493, -2);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdComm;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar5.TabIndex = 44;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(107, 14);
		this.label6.TabIndex = 43;
		this.label6.Text = "Commission Grid";
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel22.Controls.Add(this.panel23);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(533, 140);
		this.panel22.TabIndex = 0;
		this.grdGLedger.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(533, 118);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar7);
		this.panel23.Controls.Add(this.label2);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(533, 22);
		this.panel23.TabIndex = 0;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(492, -2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdGLedger;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar7.TabIndex = 42;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(102, 14);
		this.label2.TabIndex = 41;
		this.label2.Text = "Party Add/Less";
		this.panel15.Controls.Add(this.panel28);
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(603, 274);
		this.panel15.TabIndex = 0;
		this.panel28.Controls.Add(this.panel29);
		this.panel28.Controls.Add(this.panel17);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(0, 140);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(603, 134);
		this.panel28.TabIndex = 1;
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 22);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(603, 112);
		this.panel29.TabIndex = 1;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(603, 112);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 0;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.CellUpdated += new ColumnActionEventHandler(grdPaymentTerm_CellUpdated);
		this.grdPaymentTerm.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTerm_UpdatingCell);
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar4);
		this.panel17.Controls.Add(this.label1);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(603, 22);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(563, -2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(128, 14);
		this.label1.TabIndex = 40;
		this.label1.Text = "Payment Detail Grid";
		this.panel16.Controls.Add(this.panel20);
		this.panel16.Controls.Add(this.panel21);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(603, 140);
		this.panel16.TabIndex = 0;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(0, 22);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(603, 118);
		this.panel20.TabIndex = 2;
		this.grdInvExp.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(603, 118);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.ctrlGrdBar6);
		this.panel21.Controls.Add(this.label5);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(603, 22);
		this.panel21.TabIndex = 1;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(673, -2);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.grdInvExp;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar6.TabIndex = 43;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(135, 14);
		this.label5.TabIndex = 0;
		this.label5.Text = "Other Item Expenses";
		this.panel8.Controls.Add(this.label58);
		this.panel8.Controls.Add(this.txtFreightRemarks);
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.label31);
		this.panel8.Controls.Add(this.label28);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountAccount);
		this.panel8.Controls.Add(this.label30);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFreightAcDr);
		this.panel8.Controls.Add(this.txtDiscountAmountHeader);
		this.panel8.Controls.Add(this.label27);
		this.panel8.Controls.Add(this.txtFreightAmount);
		this.panel8.Controls.Add(this.label23);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightAcCr);
		this.panel8.Controls.Add(this.label22);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm);
		this.panel8.Controls.Add(this.txtDueDays);
		this.panel8.Controls.Add(this.DueDate);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.panel8.Controls.Add(this.label24);
		this.panel8.Controls.Add(this.label43);
		this.panel8.Controls.Add(this.txtExchangeRate);
		this.panel8.Controls.Add(this.txtFcyAmount);
		this.panel8.Controls.Add(this.label44);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.panel8.Controls.Add(this.label45);
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.lblDebitAccount);
		this.panel8.Controls.Add(this.txtdocno);
		this.panel8.Controls.Add(this.txtcommremarks);
		this.panel8.Controls.Add(this.DocDate);
		this.panel8.Controls.Add(this.txtRemarks);
		this.panel8.Controls.Add(this.txtbillno);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.panel8.Controls.Add(this.label12);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommuom);
		this.panel8.Controls.Add(this.txtrefno);
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add(this.txtCommAgentId);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Controls.Add(this.txtSupplierGLId);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.panel8.Controls.Add(this.label20);
		this.panel8.Controls.Add(this.label26);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.txtBillAmount);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Controls.Add(this.txtcommamount);
		this.panel8.Controls.Add(this.txtcommrate);
		this.panel8.Controls.Add(this.label19);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 28);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1136, 192);
		this.panel8.TabIndex = 0;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(590, 114);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(92, 15);
		this.label58.TabIndex = 102;
		this.label58.Text = "Freight Remarks";
		this.txtFreightRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarks.Location = new System.Drawing.Point(698, 109);
		this.txtFreightRemarks.Multiline = true;
		this.txtFreightRemarks.Name = "txtFreightRemarks";
		this.txtFreightRemarks.ShortcutsEnabled = false;
		this.txtFreightRemarks.Size = new System.Drawing.Size(207, 25);
		this.txtFreightRemarks.TabIndex = 15;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(358, 115);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 15);
		this.label13.TabIndex = 95;
		this.label13.Text = "Due Date";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(590, 141);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(88, 15);
		this.label31.TabIndex = 94;
		this.label31.Text = "Discount Dr Ac";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(590, 62);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(108, 15);
		this.label28.TabIndex = 94;
		this.label28.Text = "Freight Account Dr";
		this.CmbDiscountAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Location = new System.Drawing.Point(698, 135);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Name = "CmbDiscountAccount";
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Leave += new System.EventHandler(cmbFreightAcDr_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(590, 168);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(102, 15);
		this.label30.TabIndex = 92;
		this.label30.Text = "Discount Amount";
		this.cmbFreightAcDr.AllowNull = (DefaultableBoolean)1;
		this.cmbFreightAcDr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFreightAcDr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFreightAcDr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFreightAcDr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Location = new System.Drawing.Point(698, 56);
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Name = "cmbFreightAcDr";
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Leave += new System.EventHandler(cmbFreightAcDr_Leave);
		this.txtDiscountAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmountHeader.Location = new System.Drawing.Point(698, 164);
		this.txtDiscountAmountHeader.Name = "txtDiscountAmountHeader";
		this.txtDiscountAmountHeader.ShortcutsEnabled = false;
		this.txtDiscountAmountHeader.Size = new System.Drawing.Size(207, 23);
		this.txtDiscountAmountHeader.TabIndex = 17;
		this.txtDiscountAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmountHeader.TextChanged += new System.EventHandler(txtDiscountAmountHeader_TextChanged);
		this.txtDiscountAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountAmountHeader_KeyPress);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(590, 89);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 92;
		this.label27.Text = "Freight Amount";
		this.txtFreightAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmount.Location = new System.Drawing.Point(698, 85);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.ShortcutsEnabled = false;
		this.txtFreightAmount.Size = new System.Drawing.Size(207, 23);
		this.txtFreightAmount.TabIndex = 14;
		this.txtFreightAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmount.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.txtFreightAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmount_KeyPress);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(590, 35);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(106, 15);
		this.label23.TabIndex = 90;
		this.label23.Text = "Freight Account Cr";
		this.CmbFreightAcCr.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightAcCr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightAcCr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightAcCr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightAcCr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Location = new System.Drawing.Point(698, 29);
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Name = "CmbFreightAcCr";
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Leave += new System.EventHandler(CmbFreightAcCr_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(358, 35);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(80, 15);
		this.label22.TabIndex = 88;
		this.label22.Text = "Delivery Term";
		this.cmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Location = new System.Drawing.Point(448, 29);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Name = "cmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).TabIndex = 7;
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(448, 85);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(138, 23);
		this.txtDueDays.TabIndex = 9;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Enabled = false;
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(448, 111);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(138, 23);
		this.DueDate.TabIndex = 10;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(358, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 82;
		this.label3.Text = "Payment Term";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance133;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance139).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(448, 56);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TextChanged += new System.EventHandler(CmbPaymentTerm_TextChanged);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(358, 89);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 87;
		this.label24.Text = "Due Days";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(911, 87);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(84, 19);
		this.label43.TabIndex = 81;
		this.label43.Text = "Fcy Amount";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(997, 58);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(131, 23);
		this.txtExchangeRate.TabIndex = 19;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtFcyAmount.Location = new System.Drawing.Point(997, 84);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(131, 25);
		this.txtFcyAmount.TabIndex = 20;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(911, 62);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 80;
		this.label44.Text = "Exchange Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(997, 29);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(911, 35);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(55, 15);
		this.label45.TabIndex = 77;
		this.label45.Text = "Fcy Code";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(358, 138);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(89, 20);
		this.label16.TabIndex = 75;
		this.label16.Text = "Bill Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(8, 89);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(73, 15);
		this.label15.TabIndex = 43;
		this.label15.Text = "Tax Account";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(114, 83);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 3;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label10);
		this.panel12.Controls.Add(this.ChkPrintSlip);
		this.panel12.Controls.Add(this.ChkBok);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.ForeColor = System.Drawing.Color.White;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1136, 27);
		this.panel12.TabIndex = 41;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(8, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(108, 19);
		this.label10.TabIndex = 42;
		this.label10.Text = "Sale Invoice";
		this.ChkPrintSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Checked = true;
		this.ChkPrintSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.Location = new System.Drawing.Point(959, 6);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(76, 19);
		this.ChkPrintSlip.TabIndex = 15;
		this.ChkPrintSlip.TabStop = false;
		this.ChkPrintSlip.Text = "Print Slip";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(1037, 6);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(102, 19);
		this.ChkBok.TabIndex = 16;
		this.ChkBok.TabStop = false;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(8, 141);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(85, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Manual Bill No";
		this.CmbCommissionDebitAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionDebitAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionDebitAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionDebitAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionDebitAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Location = new System.Drawing.Point(1435, 121);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Name = "CmbCommissionDebitAc";
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Size = new System.Drawing.Size(96, 24);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).TabIndex = 39;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(8, 35);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(105, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Invoice No && Date";
		this.lblDebitAccount.AutoSize = true;
		this.lblDebitAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAccount.Location = new System.Drawing.Point(1375, 126);
		this.lblDebitAccount.Name = "lblDebitAccount";
		this.lblDebitAccount.Size = new System.Drawing.Size(53, 15);
		this.lblDebitAccount.TabIndex = 38;
		this.lblDebitAccount.Text = "Debit Ac";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(114, 31);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(91, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtcommremarks.Location = new System.Drawing.Point(1435, 93);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(96, 24);
		this.txtcommremarks.TabIndex = 15;
		this.txtcommremarks.Text = "";
		this.txtcommremarks.Visible = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(207, 31);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(146, 23);
		this.DocDate.TabIndex = 1;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(114, 164);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(472, 23);
		this.txtRemarks.TabIndex = 6;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(114, 137);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(239, 23);
		this.txtbillno.TabIndex = 5;
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(1281, 66);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(93, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 115);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(97, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Customer Ref No";
		this.cmbcommuom.AllowNull = (DefaultableBoolean)1;
		this.cmbcommuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Location = new System.Drawing.Point(1281, 93);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Name = "cmbcommuom";
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Size = new System.Drawing.Size(93, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TextChanged += new System.EventHandler(cmbcommuom_Leave);
		this.txtrefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefno.Location = new System.Drawing.Point(114, 111);
		this.txtrefno.Name = "txtrefno";
		this.txtrefno.Size = new System.Drawing.Size(239, 23);
		this.txtrefno.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 168);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Remarks";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1281, 122);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(31, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 62);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Customer Name";
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1318, 122);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(31, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(1281, 39);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(93, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Leave += new System.EventHandler(cmbcommagent_Leave);
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(114, 56);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 2;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1195, 44);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1375, 71);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1195, 71);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1375, 98);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.txtBillAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(448, 136);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(138, 25);
		this.txtBillAmount.TabIndex = 11;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1375, 44);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate";
		this.label21.Visible = false;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1435, 68);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(96, 21);
		this.txtcommamount.TabIndex = 14;
		this.txtcommamount.Visible = false;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(1435, 41);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(96, 21);
		this.txtcommrate.TabIndex = 12;
		this.txtcommrate.Visible = false;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1195, 98);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 15);
		this.label19.TabIndex = 14;
		this.label19.Text = "Comm UOM";
		this.label19.Visible = false;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1136, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator3, this.btnSlipFormat2, this.btnAttachment, this.toolStripSeparator4, this.toolStripButton1,
			this.btnPrint, this.toolStripButton3, this.toolStripSeparator1, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1136, 28);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(60, 25);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 25);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 25);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 25);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(74, 25);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 25);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(94, 25);
		this.toolStripButton1.Text = "1609-Slip";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnSlipFormat2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlipFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipFormat2.Name = "btnSlipFormat2";
		this.btnSlipFormat2.Size = new System.Drawing.Size(103, 25);
		this.btnSlipFormat2.Text = "1609A-Slip";
		this.btnSlipFormat2.Click += new System.EventHandler(btnSlipFormat2_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(116, 25);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(93, 25);
		this.toolStripButton3.Text = "Load &Gdn";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(127, 25);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.tabHistory.Controls.Add(this.panel24);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel31);
		this.tabHistory.Controls.Add(this.panel30);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1142, 679);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel24.Controls.Add(this.panel3);
		this.panel24.Controls.Add(this.panel26);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(3, 479);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1136, 197);
		this.panel24.TabIndex = 51;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 24);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1136, 173);
		this.panel3.TabIndex = 48;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1136, 173);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel26.BackColor = System.Drawing.Color.Teal;
		this.panel26.Controls.Add(this.ctrlGrdBar2);
		this.panel26.Controls.Add(this.label14);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.ForeColor = System.Drawing.Color.White;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1136, 24);
		this.panel26.TabIndex = 47;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1096, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(5, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(317, 19);
		this.label14.TabIndex = 42;
		this.label14.Text = "Invoice Detail Of Above Selected Row";
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 138);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1136, 341);
		this.panel14.TabIndex = 50;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.FrozenColumns = 4;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1136, 341);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel31.Controls.Add(this.groupBox4);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 57);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1136, 81);
		this.panel31.TabIndex = 49;
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label18);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(6, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(996, 76);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(884, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(884, 22);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(803, 22);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(803, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(731, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(160, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(48, 15);
		this.label29.TabIndex = 179;
		this.label29.Text = "To Date";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(528, 18);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(93, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Customer Name";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(528, 35);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(162, 37);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(147, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(7, 19);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "From Date";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(422, 37);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(101, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(6, 37);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(152, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(424, 19);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(45, 15);
		this.label18.TabIndex = 173;
		this.label18.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(313, 19);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(313, 37);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(105, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.panel30.BackColor = System.Drawing.Color.Teal;
		this.panel30.Controls.Add(this.ctrlGrdBar3);
		this.panel30.Controls.Add(this.label54);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(3, 30);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1136, 27);
		this.panel30.TabIndex = 48;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1097, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(176, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Sale Invoice History ";
		this.panel6.Controls.Add(this.panel13);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1136, 27);
		this.panel6.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.toolStrip2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1136, 27);
		this.panel13.TabIndex = 27;
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewHistory, this.BtnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1136, 25);
		this.toolStrip2.TabIndex = 22;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(56, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(77, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(74, 22);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnSaleHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1150, 708);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "SaleInvoiceTrading_Engr";
		this.Text = "Sale Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleInvoiceTrading_Engr_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdComm).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreightAcDr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAcCr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel31.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
