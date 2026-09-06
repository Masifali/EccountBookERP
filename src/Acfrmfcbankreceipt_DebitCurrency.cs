// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using AccountsReconciliationHeader = global::Architecture.Model.Accounts.AccountsReconciliationHeader;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Reporting = global::Architecture.Common.Reporting;
using MultiCurrencyBLL = global::Architecture.BLL.MultiCurrency;
using VoucherHead = Architecture.BLL.Accounts.VoucherHead;
using ProjectsBLL = global::Architecture.BLL.Projects;
using DMSAttachmentsBLL = global::Architecture.BLL.DMSAttachments;
using ImInvoiceBLL = global::Architecture.BLL.Import.ImInvoice;
using ExImFCBankPaymenstsBLL = global::Architecture.BLL.Import.ExImFCBankPaymensts;
using FcYBankChargesTypes = Architecture.WinApp.Common.FcYBankChargesTypes;
using SpecialSreenRightsUserWise = Architecture.WinApp.Common.SpecialSreenRightsUserWise;
using ExImFCBankReceiptsBLL = global::Architecture.BLL.Export.ExImFCBankReceipts;
using ExImFCBankReceipts = global::Architecture.Model.Export.ExImFCBankReceipts;
using FcyBankReceiptBreakUpBLL = global::Architecture.BLL.Export.FcyBankReceiptBreakUp;
using FcyBankReceiptBreakUp = global::Architecture.Model.Export.FcyBankReceiptBreakUp;
using ExImInvoice = global::Architecture.WinApp.Common.ExImInvoiceBLL;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition
{

public class Acfrmfcbankreceipt_DebitCurrency : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private int Id;

	private double PreviousBalance;

	private int VoucherHeadId;

	private int BrancheId;

	private int ProjectId;

	private int GdBreakUpId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtLCOrder = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tablebreakup = new DataTable();

	private DataTable tablePartybreakup = new DataTable();

	private DataTable dtPaymentTermsDetail = new DataTable();

	private LoadExportInvoice LoadExportInvoice;

	private DataTable dtaccounts = new DataTable();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int updateDetailIndex;

	private int updateDetailIndexBreakUp;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	internal GridEX grdFCBank;

	private Button btnAddinGrid;

	private Label label36;

	private Label label37;

	private TextBox txtpkramount;

	private Label label34;

	private Label label35;

	private Label label32;

	private Label label33;

	private TextBox txtfcamount;

	private Panel panel3;

	private GroupBox groupBox1;

	internal UltraCombo comSuplier;

	private DateTimePicker txtdate;

	private Label label31;

	private Label label30;

	private Label label29;

	private Label label28;

	private Label label26;

	private Label label25;

	private Label label24;

	private Label label23;

	private Label label22;

	private Label label20;

	private Label label19;

	private Label label6;

	private Label label5;

	private Label label18;

	private Label label4;

	private TextBox txtexchangeRate;

	private Label label17;

	private TextBox txtNetAmount;

	private Label label14;

	private TextBox txtFCGrosamount;

	private Label label13;

	private TextBox txtNetAmountFC;

	private Label label12;

	private TextBox txtbankDrAmount;

	private Label label11;

	private TextBox txtbankfbpNo;

	private Label label10;

	internal UltraCombo cmbcurrency;

	private Label label9;

	internal UltraCombo cmbbankAcc;

	private Label label8;

	internal UltraCombo cmbInvoiceNo;

	private Label label7;

	internal UltraCombo cmbLCOrder;

	private Label label3;

	private Label label1;

	private TextBox txtdocNumber;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private ToolStrip btnAttachments;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private TabPage tabPage2;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnUpdate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStrip toolStrip2;

	internal UltraCombo cmbranches;

	private Label label39;

	internal UltraCombo cmbproject;

	private Label label40;

	private Label label42;

	private Label label41;

	internal UltraCombo cmbPaymentTerm;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnRefresh;

	private Label label45;

	private TextBox txtRemarks;

	private Label label46;

	private TextBox txtRemarksDetail;

	private Label label47;

	private ToolStripButton btnSlip;

	private Button button1;

	internal UltraCombo CmbChargesType;

	private Label label48;

	private Label label49;

	private Label label50;

	private TextBox txtInvoiceAmount;

	private Label label51;

	private TextBox txtLocalAmount;

	private TextBox txtForeinchargesPkr;

	private Label label52;

	private TextBox txtExportvoucherRate;

	internal UltraCombo CmbChargesAccountType;

	internal UltraCombo CmbGainLossAc;

	private Label label55;

	private Label label54;

	private TextBox txtBalanceAmount;

	private Label label53;

	private TextBox txtRecAmount;

	internal UltraCombo cmbaccounttype;

	private Label label56;

	private Label label57;

	private GroupBox groupBox2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnLoadInvoice;

	private GroupBox groupBox4;

	private GroupBox groupBox3;

	private TextBox txtfcyAmountThirdParty;

	private TextBox txtHcyExchRthirdParty;

	private Label label21;

	private TextBox txtFcyExchRthirdParty;

	private Label label2;

	private Label label16;

	internal UltraCombo cmbFcycodeThirdParty;

	private CheckBox chkpreview;

	private Label label44;

	private TextBox txtChargePercent;

	internal UltraCombo cmbReferenceNo;

	private Label label43;

	private Panel panel4;

	private Panel panel5;

	private Panel panelInvoiceWiseBreakup;

	internal GridEX grdInvoiceWiseBreakUp;

	private Panel panel11;

	private GroupBox groupBox5;

	private Label label58;

	private TextBox txtInvoiceAmountBreakup;

	private Label label59;

	private Label label62;

	private Label label63;

	private TextBox txtRealizedAmtBreakup;

	private Label label64;

	private TextBox txtFDBCNoBreakup;

	internal UltraCombo cmbGdNoBreakup;

	private Button btnAddBreakup;

	private Button BtnUpdatebreakup;

	private Button BtnCancelBreakUp;

	private TextBox txtCommisionamtbreakUp;

	private Label label66;

	internal UltraCombo cmbInvoicenobreakup;

	private Label label60;

	internal UltraCombo cmbStatusBreakup;

	private Label label61;

	internal UltraCombo CmbChargesFDBCNo;

	private Label label68;

	private TextBox txtFBC;

	internal UltraCombo CmbFBCDebitAccount;

	private Label label69;

	private RadioButton RDBankSingleAndPartiesMulti;

	private RadioButton RDMultiBankDevice;

	internal GridEX grdPartyPaymentsBreackUp;

	private Panel panel10;

	private Panel panel12;

	private Label label65;

	private Panel panel13;

	internal UltraCombo CmbConsignee;

	private Label lblConsignee;

	private TextBox txtFINo;

	private Label labFiNo;

	private RadioButton rdMultiBankAndMultiParties;

	private Panel panel14;

	private Panel RemainingBalanceOfBankDeviceBreakUp;

	internal GridEX RemainingBalanceGrid;

	private Button btnRemaingBreakupBalance;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private Label label70;

	private Panel panel15;

	private Label label38;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtReceiverFcyAmount;

	private Label label72;

	private Label label71;

	private TextBox txtReceiverExchangeRate;

	internal GridEX grdPaymentTerms;

	private GroupBox groupBox6;

	private Label label67;

	internal UltraCombo CmbReceiverAcHistory;

	private Label label73;

	internal UltraCombo CmbInvoiceNoHistory;

	private Label label74;

	internal UltraCombo CmbPaymentTermHistory;

	private Button btnShowHistory;

	private Label label75;

	private Label label76;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private TextBox txtDebitFcyAmount;

	private Label label27;

	private TextBox txtDebitExchangeRate;

	private Label label78;

	internal UltraCombo CmbDebitCurrency;

	public Acfrmfcbankreceipt_DebitCurrency(UserAccount user)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Acfrmfcbankreceipt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1700, 750);
			panelInvoiceWiseBreakup.Visible = false;
			RemainingBalanceOfBankDeviceBreakUp.Visible = false;
			btnRemaingBreakupBalance.Visible = false;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbcurrency.ReadOnly = true;
			txtdate.Focus();
			chkpreview.Checked = true;
			lblConsignee.Visible = false;
			((Control)(object)CmbConsignee).Visible = false;
			labFiNo.Visible = false;
			txtFINo.Visible = false;
			txtRemarks.Width = 555;
			PartyPaymentPanelVisible();
			GenerateDocumentNo();
			GetAllAccounts();
			Accounttype();
			GainLossAccountFill();
			CurrencyCOde();
			ChargerType();
			PaymentTermFill();
			CombBranchFill();
			CombProjectFill();
			ChargesTypeFill();
			ReferenceNoFill();
			FBCDebitAccountBind();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("FDBCNo", typeof(string));
			table.Columns.Add("TypeId");
			table.Columns.Add("Type");
			table.Columns.Add("ChargesTypeId");
			table.Columns.Add("ChargesType");
			table.Columns.Add("Prcnt", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("FC-Amount", typeof(double));
			table.Columns.Add("Rs-Amount", typeof(double));
			tablebreakup.Columns.Add("Id", typeof(int));
			tablebreakup.Columns.Add("InvoiceId", typeof(int));
			tablebreakup.Columns.Add("InvoiceNo", typeof(string));
			tablebreakup.Columns.Add("GDId", typeof(int));
			tablebreakup.Columns.Add("GDNO", typeof(string));
			tablebreakup.Columns.Add("GdRefDocTypeId", typeof(int));
			tablebreakup.Columns.Add("InvoiceAmount", typeof(double));
			tablebreakup.Columns.Add("RealizedAmount", typeof(double));
			tablebreakup.Columns.Add("ForiegnAgencyCommAmount", typeof(double));
			tablebreakup.Columns.Add("FDBCNo", typeof(string));
			tablebreakup.Columns.Add("Status", typeof(string));
			grdInvoiceWiseBreakUp.DataSource = tablebreakup;
			grdInvoiceWiseBreakUp.RetrieveStructure();
			gridBreakUpsettings();
			tablePartybreakup.Columns.Add("RealizedAmount", typeof(double));
			tablePartybreakup.Columns.Add("FBC", typeof(double));
			tablePartybreakup.Columns.Add("TotalAmount", typeof(double));
			RowAddinGrdPartyPaymentsBreakUp();
			grdPartyPaymentsBreackUp.RetrieveStructure();
			gridPartyBreakUpsettings();
			dtPaymentTermsDetail.Columns.Add("InvoicePaymentDetailId", typeof(int));
			dtPaymentTermsDetail.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTermsDetail.Columns.Add("PaymentTerm", typeof(string));
			dtPaymentTermsDetail.Columns.Add("Amount", typeof(double));
			dtPaymentTermsDetail.Columns.Add("Received", typeof(double));
			dtPaymentTermsDetail.Columns.Add("Balance", typeof(double));
			dtPaymentTermsDetail.Columns.Add("ThisReceipt", typeof(double));
			grdPaymentTerms.DataSource = dtPaymentTermsDetail;
			grdPaymentTerms.RetrieveStructure();
			gridPaymentTermsSettings();
			((Control)(object)grdPaymentTerms).Visible = false;
			HistoryCombosFill();
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombBranchFill()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				BrancheId = Conversion.ToInt(BranchList.Rows[0]["Id"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0 && dt != null)
			{
				ProjectId = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerm()
	{
		try
		{
			((Control)(object)cmbInvoiceNo).TabStop = true;
			((Control)(object)cmbLCOrder).TabStop = true;
			lblConsignee.Visible = false;
			((Control)(object)CmbConsignee).Visible = false;
			labFiNo.Visible = false;
			txtFINo.Visible = false;
			((Control)(object)grdPaymentTerms).Visible = false;
			txtRemarks.Width = 555;
			if (((Control)(object)cmbPaymentTerm).Text == "Advanced Payment")
			{
				cmbLCOrder.ReadOnly = false;
				cmbInvoiceNo.ReadOnly = true;
				cmbcurrency.ReadOnly = false;
				((Control)(object)cmbInvoiceNo).Text = "";
				txtInvoiceAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtExportvoucherRate.Text = "0";
				lblConsignee.Visible = true;
				((Control)(object)CmbConsignee).Visible = true;
				labFiNo.Visible = true;
				txtFINo.Visible = true;
				txtRemarks.Width = 234;
				RemainingBalanceOfBankDeviceBreakUp.Visible = false;
				btnRemaingBreakupBalance.Visible = false;
				if (cmbInvoiceNo.ReadOnly)
				{
					((Control)(object)cmbInvoiceNo).TabStop = false;
				}
				LcOrder();
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				cmbInvoiceNo.ReadOnly = false;
				cmbLCOrder.ReadOnly = true;
				((Control)(object)cmbLCOrder).Text = "";
				cmbcurrency.ReadOnly = true;
				((Control)(object)grdPaymentTerms).Visible = true;
				if (cmbLCOrder.ReadOnly)
				{
					((Control)(object)cmbLCOrder).TabStop = false;
				}
				RemainingBalanceOfBankDeviceBreakUp.Visible = false;
				btnRemaingBreakupBalance.Visible = false;
				lblConsignee.Visible = true;
				((Control)(object)CmbConsignee).Visible = true;
				labFiNo.Visible = true;
				txtFINo.Visible = true;
				txtRemarks.Width = 234;
				CustomersFillByInvoicePayment();
				InvoiceNo();
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Other Payment")
			{
				cmbLCOrder.ReadOnly = true;
				((Control)(object)cmbLCOrder).Text = "";
				cmbInvoiceNo.ReadOnly = true;
				cmbcurrency.ReadOnly = false;
				((Control)(object)cmbInvoiceNo).Text = "";
				txtInvoiceAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtExportvoucherRate.Text = "0";
				((Control)(object)cmbInvoiceNo).TabStop = false;
				((Control)(object)cmbLCOrder).TabStop = false;
				((UltraGridBase)cmbLCOrder).DataSource = null;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4)
			{
				cmbLCOrder.ReadOnly = true;
				cmbInvoiceNo.ReadOnly = true;
				cmbcurrency.ReadOnly = false;
				((Control)(object)cmbInvoiceNo).Text = "";
				txtInvoiceAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtExportvoucherRate.Text = "0";
				if (cmbInvoiceNo.ReadOnly)
				{
					((Control)(object)cmbInvoiceNo).TabStop = false;
				}
				if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
				{
					InvoiceNoForBreakUp();
					panelInvoiceWiseBreakup.Visible = true;
					btnRemaingBreakupBalance.Visible = true;
				}
				else
				{
					panelInvoiceWiseBreakup.Visible = false;
					grdInvoiceWiseBreakUp.ClearStructure();
					btnRemaingBreakupBalance.Visible = false;
				}
			}
			else
			{
				panelInvoiceWiseBreakup.Visible = false;
				grdInvoiceWiseBreakUp.ClearStructure();
			}
			if (((Control)(object)cmbPaymentTerm).Text != "Invoice Payment")
			{
				ExportCustomersFillFomGlobal();
			}
			else
			{
				ExportConsigneeFillFomGlobal();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportCustomersFillFomGlobal()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName", typeof(string));
			dt.Columns.Add("GlAccountId", typeof(int));
			dt.Columns.Add("GlAccountCurrency", typeof(string));
			dt.Columns.Add("GlAccountCurrencyId", typeof(int));
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7))
			{
				dt.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToInt((object)item.GlAccountId), Conversion.ToString((object)item.GlAccountCurrencyCode), Conversion.ToInt((object)item.GlAccountCurrencyId));
			}
			InfragisticsHelper.BindAndRetainSelection(comSuplier, dt, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(comSuplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbConsignee, dt, "Id", "CompanyName", "Consignee", AllColumns: true, Conversion.ToInt(CmbConsignee.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)comSuplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comSuplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbConsignee).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbConsignee).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ExportConsigneeFillFomGlobal()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName", typeof(string));
			dt.Columns.Add("GlAccountId", typeof(int));
			dt.Columns.Add("GlAccountCurrency", typeof(string));
			dt.Columns.Add("GlAccountCurrencyId", typeof(int));
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7))
			{
				dt.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToInt((object)item.GlAccountId), Conversion.ToString((object)item.GlAccountCurrencyCode), Conversion.ToInt((object)item.GlAccountCurrencyId));
			}
			InfragisticsHelper.BindAndRetainSelection(CmbConsignee, dt, "Id", "CompanyName", "Consignee", AllColumns: true, Conversion.ToInt(CmbConsignee.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbConsignee).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbConsignee).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTermFill()
	{
		try
		{
			DataTable dt = ExImFCBankReceiptsBLL.getFcyReceiptsPaymentType();
			DDL.BindDDL(dt, cmbPaymentTerm, "Id", "PaymentType", "Payment Terms", ZeroIndex: false);
			((UltraGridBase)cmbPaymentTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargesTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ChargesType");
			dt.Rows.Add(1, "Local");
			dt.Rows.Add(2, "Foreign");
			DDL.BindDDLNew(dt, CmbChargesType, "Id", "ChargesType", "ChargesType", false);
			((UltraGridBase)CmbChargesType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferenceNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImFCBankReceiptsBLL.GetReferenceNo(new ExImFCBankReceipts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtRef = new DataTable();
				dtRef.Columns.Add("Id");
				dtRef.Columns.Add("ReferenceNo");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtRef.Rows.Add(dt.Rows[i]["ReferenceNo"], dt.Rows[i]["ReferenceNo"]);
				}
				DDL.BindDDL(dtRef, cmbReferenceNo, "Id", "ReferenceNo", "Reference No", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CustomersFillByInvoicePayment()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName");
			dt.Columns.Add("GlAccountId", typeof(int));
			dt.Columns.Add("GlAccountCurrency");
			dt.Columns.Add("GlAccountCurrencyId", typeof(int));
			DataTable dtInvoice = CommonServices.GetInvoiceNoandPartiesForFcBankReceipts();
			if (dtInvoice.Rows.Count > 0)
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					if (Conversion.ToString(dtInvoice.Rows[i]["FilterType"]) == "PartyName")
					{
						dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["RefName"], 0, dtInvoice.Rows[i]["CurrenyCode"], dtInvoice.Rows[i]["CurrencyId"]);
					}
				}
				InfragisticsHelper.BindAndRetainSelection(comSuplier, dt, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(comSuplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				if (dt.Rows.Count > 0)
				{
					((UltraGridBase)comSuplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)comSuplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
			}
			else
			{
				((Control)(object)comSuplier).Text = string.Empty;
				((UltraGridBase)comSuplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNo()
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				DataTable dtInvoice = CommonServices.GetInvoiceNoandPartiesForFcBankReceipts(Conversion.ToInt(comSuplier.Value));
				if (dtInvoice.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("InvoiceNo");
					for (int i = 0; i < dtInvoice.Rows.Count; i++)
					{
						if (Conversion.ToString(dtInvoice.Rows[i]["FilterType"]) == "InvoiceNo")
						{
							dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["RefName"]);
						}
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
						return;
					}
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((UltraGridBase)cmbInvoiceNo).DataSource = null;
				}
				else
				{
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((UltraGridBase)cmbInvoiceNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoForBreakUp()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				DataTable dtInvoice = ExImInvoice.GetCommercialInvoicesaginstPreInvoices(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.OrganizationId
				});
				if (dtInvoice.Rows.Count > 0)
				{
					DDL.BindDDL(dtInvoice, cmbInvoicenobreakup, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[1].Width = 200;
				}
				else
				{
					((Control)(object)cmbInvoicenobreakup).Text = string.Empty;
					((UltraGridBase)cmbInvoicenobreakup).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbInvoicenobreakup).Text = string.Empty;
				((UltraGridBase)cmbInvoicenobreakup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Accounttype()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Accounttype");
			dt.Rows.Add(1, "Bank");
			dt.Rows.Add(2, "Parties");
			dt.Rows.Add(3, "Cash");
			dt.Rows.Add(4, "Expense");
			DDL.BindDDL(dt, cmbaccounttype, "Id", "Accounttype", "Accounttype", ZeroIndex: false);
			((UltraGridBase)cmbaccounttype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbaccounttype_ValueChanged(object sender, EventArgs e)
	{
	}

	private void BankAccountDr(int accounttypeId)
	{
		try
		{
			if (dtaccounts.Rows.Count > 0)
			{
				DataTable dtdrac = new DataTable();
				dtdrac.Columns.Add("Id", typeof(int));
				dtdrac.Columns.Add("AccountTitle");
				dtdrac.Columns.Add("Currency");
				dtdrac.Columns.Add("CurrencyId", typeof(int));
				for (int i = 0; i < dtaccounts.Rows.Count; i++)
				{
					if (Conversion.ToInt((object)accounttypeId) == 22)
					{
						if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 3 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 6 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 8 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 22)
						{
							dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"], dtaccounts.Rows[i]["CurrencyCode"], dtaccounts.Rows[i]["CurrencyId"]);
						}
					}
					else if (Conversion.ToInt((object)accounttypeId) == 11)
					{
						if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 11 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 13 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 14 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 20 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 21)
						{
							dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"], dtaccounts.Rows[i]["CurrencyCode"], dtaccounts.Rows[i]["CurrencyId"]);
						}
					}
					else if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == Conversion.ToInt((object)accounttypeId))
					{
						dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"], dtaccounts.Rows[i]["CurrencyCode"], dtaccounts.Rows[i]["CurrencyId"]);
					}
				}
				if (dtdrac.Rows.Count > 0)
				{
					InfragisticsHelper.BindAndRetainSelection(cmbbankAcc, dtdrac, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(cmbbankAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					((UltraGridBase)comSuplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
				else
				{
					((Control)(object)cmbbankAcc).Text = string.Empty;
					((UltraGridBase)cmbbankAcc).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbbankAcc).Text = string.Empty;
				((UltraGridBase)cmbbankAcc).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllAccounts()
	{
		try
		{
			dtaccounts = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GainLossAccountFill()
	{
		try
		{
			DataTable dtAccount = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,4,11,12,15");
			if (dtAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccount, CmbGainLossAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FBCDebitAccountBind()
	{
		try
		{
			DataTable dtAccount = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,13,14,20,21");
			if (dtAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccount, CmbFBCDebitAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyCOde()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCurrency = MultiCurrencyBLL.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCurrency, cmbcurrency, "Id", "CurrencyCode", "Fcy Code", false);
				DDL.BindDDLNew(dtCurrency, cmbFcycodeThirdParty, "Id", "CurrencyCode", "Fcy Code", false);
				DDL.BindDDLNew(dtCurrency, CmbDebitCurrency, "Id", "CurrencyCode", "Fcy Code", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargerType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtchargestype = ExImProceedsChargesType.Getall(new ExImProceedsChargesType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtchargestype.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtchargestype, CmbChargesAccountType, "Id", "ProceedsChargesTypeCode", "Charges Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetChargesTypeByCustomerId(int ID)
	{
	}

	private void GenerateDocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = ExImFCBankReceiptsBLL.GenerateCode(new ExImFCBankReceipts
			{
				DocumentTypeId = 203,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocNumber.Text = Code.ToString();
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 203);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPaymentTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text != "Other Payment")
			{
				((Control)(object)comSuplier).Text = string.Empty;
				((UltraGridBase)comSuplier).DataSource = null;
			}
			PaymentTerm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			double ForeignAmount = 0.0;
			double LocalAmount = 0.0;
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1 && (((Control)(object)CmbChargesFDBCNo).Text.Trim() == string.Empty || ((UltraGridBase)CmbChargesFDBCNo).ActiveRow == null))
			{
				MessageBox.Show("Please Select FDBC #");
				CmbChargesFDBCNo.Focus();
				return;
			}
			if (((Control)(object)CmbChargesAccountType).Text.Trim() == string.Empty || Conversion.ToInt(CmbChargesAccountType.Value) == 0)
			{
				MessageBox.Show("Please Select AccountTitle");
				CmbChargesAccountType.Focus();
				return;
			}
			if (txtpkramount.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter PKR Amount");
				txtpkramount.Focus();
				return;
			}
			if (((Control)(object)CmbChargesType).Text.Trim() == "" || Conversion.ToInt(CmbChargesType.Value) == 0)
			{
				MessageBox.Show("Please select Chargestype");
				CmbChargesType.Focus();
				return;
			}
			double Fcamount = Conversion.ToDouble((object)txtfcamount.Text);
			double RsAmount = Conversion.ToDouble((object)txtpkramount.Text);
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				Fcamount += Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				RsAmount += Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
			}
			if (Fcamount > Conversion.ToDouble((object)txtNetAmountFC.Text))
			{
				throw new Exception("Fcy Bank Charges Amount not greater than FcyNetAmount");
			}
			if (RsAmount > Conversion.ToDouble((object)txtNetAmount.Text))
			{
				throw new Exception("Bank Charges Amount not greater than Amount(PKR) Debit Amount");
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows.Add(CmbChargesFDBCNo.Value, CmbChargesType.Value, ((Control)(object)CmbChargesType).Text.Trim(), CmbChargesAccountType.Value, ((Control)(object)CmbChargesAccountType).Text, Conversion.ToDouble((object)txtChargePercent.Text), txtRemarksDetail.Text.Trim(), Conversion.ToDouble((object)txtfcamount.Text.Trim()), Conversion.ToDouble((object)txtpkramount.Text.Trim()));
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			rows = grdFCBank.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString((object)item.Cells["Type"].Text) == "Foreign")
				{
					ForeignAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
				else
				{
					LocalAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
			}
			txtForeinchargesPkr.Text = ForeignAmount.ToString("#,##0.####");
			txtLocalAmount.Text = LocalAmount.ToString("#,##0.####");
			CmbChargesAccountType.Value = "";
			txtChargePercent.Text = "";
			txtfcamount.Text = "";
			txtpkramount.Text = "";
			txtRemarksDetail.Text = "";
			BankDebitAmountCalculate();
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				CmbChargesFDBCNo.Focus();
			}
			else
			{
				CmbChargesType.Focus();
			}
			CalculateBankInvoiceBreakupRemaining();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateBankInvoiceBreakupRemaining()
	{
		try
		{
			if (panelInvoiceWiseBreakup.Visible && grdInvoiceWiseBreakUp.GetRows().Count() > 0)
			{
				RemainingBalanceOfBankDeviceBreakUp.Visible = true;
				List<string> distinct = (from row in tablebreakup.AsEnumerable()
					select Conversion.ToString(row["FDBCNo"])).Distinct().ToList();
				if (distinct.Count <= 0)
				{
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("FDBCNo", typeof(string));
				dt.Columns.Add("Realized", typeof(double));
				dt.Columns.Add("ExRate", typeof(double));
				dt.Columns.Add("LcyAmount", typeof(double));
				dt.Columns.Add("Expense", typeof(double));
				dt.Columns.Add("NetLcyAmount", typeof(double));
				int i;
				for (i = 0; i < distinct.Count; i++)
				{
					double realizedAmount = 0.0;
					double TotalExpense = 0.0;
					DataRow dr = tablebreakup.AsEnumerable().FirstOrDefault((DataRow r) => distinct[i].Contains(Conversion.ToString(r["FDBCNo"])));
					if (dr != null)
					{
						realizedAmount = Conversion.ToDouble(dr["RealizedAmount"]);
					}
					double Lcy = Math.Round(realizedAmount * Conversion.ToDouble((object)txtexchangeRate.Text), 4);
					TotalExpense = (from row in table.AsEnumerable()
						where row.Field<string>("FDBCNo") == distinct[i].ToString()
						select row).Sum((DataRow row) => row.Field<double>("Rs-Amount"));
					dt.Rows.Add(distinct[i], realizedAmount, Conversion.ToDouble((object)txtexchangeRate.Text), Lcy, TotalExpense, Lcy - TotalExpense);
				}
				RemainingBalanceGrid.DataSource = dt;
				RemainingBalanceGrid.RetrieveStructure();
				RemainingBalanceGrid.RootTable.Columns["Realized"].AggregateFunction = (AggregateFunction)2;
				RemainingBalanceGrid.RootTable.Columns["Realized"].TextAlignment = (TextAlignment)3;
				RemainingBalanceGrid.RootTable.Columns["Realized"].FormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["Realized"].TotalFormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["ExRate"].TextAlignment = (TextAlignment)3;
				RemainingBalanceGrid.RootTable.Columns["ExRate"].FormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
				RemainingBalanceGrid.RootTable.Columns["LcyAmount"].FormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["LcyAmount"].TotalFormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
				RemainingBalanceGrid.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
				RemainingBalanceGrid.RootTable.Columns["Expense"].FormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["Expense"].TotalFormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["NetLcyAmount"].AggregateFunction = (AggregateFunction)2;
				RemainingBalanceGrid.RootTable.Columns["NetLcyAmount"].TextAlignment = (TextAlignment)3;
				RemainingBalanceGrid.RootTable.Columns["NetLcyAmount"].FormatString = "#,##0.####";
				RemainingBalanceGrid.RootTable.Columns["NetLcyAmount"].TotalFormatString = "#,##0.####";
				CommonServices.GridAutoAdjustmentNew(RemainingBalanceGrid);
			}
			else
			{
				RemainingBalanceOfBankDeviceBreakUp.Visible = false;
				RemainingBalanceGrid.ClearStructure();
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
			double ForeignAmount = 0.0;
			double LocalAmount = 0.0;
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1 && (((Control)(object)CmbChargesFDBCNo).Text.Trim() == string.Empty || ((UltraGridBase)CmbChargesFDBCNo).ActiveRow == null))
			{
				MessageBox.Show("Please Select FDBC #");
				CmbChargesFDBCNo.Focus();
				return;
			}
			if (((Control)(object)CmbChargesAccountType).Text.Trim() == "")
			{
				MessageBox.Show("Please Select AccountTitle");
				CmbChargesAccountType.Focus();
				return;
			}
			if (txtpkramount.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter PKR Amount");
				txtpkramount.Focus();
				return;
			}
			if (((Control)(object)CmbChargesType).Text.Trim() == "" || Conversion.ToInt(CmbChargesType.Value) == 0)
			{
				MessageBox.Show("Please select Chargestype");
				CmbChargesType.Focus();
				return;
			}
			double Fcamount = Conversion.ToDouble((object)txtfcamount.Text);
			double RsAmount = Conversion.ToDouble((object)txtpkramount.Text);
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				Fcamount += Conversion.ToDouble((object)r2.Cells["FC-Amount"].Text);
				RsAmount += Conversion.ToDouble((object)r2.Cells["Rs-Amount"].Text);
			}
			if (Fcamount > Conversion.ToDouble((object)txtNetAmountFC.Text))
			{
				throw new Exception("Fcy Bank Charges Amount not gratter than FcyNetAmount");
			}
			if (RsAmount > Conversion.ToDouble((object)txtNetAmount.Text))
			{
				throw new Exception("Bank Charges Amount not gratter than Amount(PKR) Debit Amount");
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows[updateDetailIndex]["FDBCNo"] = ((Control)(object)CmbChargesFDBCNo).Text;
			table.Rows[updateDetailIndex]["TypeId"] = CmbChargesType.Value;
			table.Rows[updateDetailIndex]["Type"] = ((Control)(object)CmbChargesType).Text;
			table.Rows[updateDetailIndex]["ChargesTypeId"] = CmbChargesAccountType.Value;
			table.Rows[updateDetailIndex]["ChargesType"] = ((Control)(object)CmbChargesAccountType).Text;
			table.Rows[updateDetailIndex]["Prcnt"] = Conversion.ToDouble((object)txtChargePercent.Text);
			table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
			table.Rows[updateDetailIndex]["FC-Amount"] = Conversion.ToDouble((object)txtfcamount.Text);
			table.Rows[updateDetailIndex]["Rs-Amount"] = Conversion.ToDouble((object)txtpkramount.Text);
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			resetdetail();
			BankDebitAmountCalculate();
			btnAddinGrid.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToString((object)r.Cells["Type"].Text) == "Foreign")
				{
					ForeignAmount += Conversion.ToDouble(r.Cells["Rs-Amount"].Value);
				}
				else
				{
					LocalAmount += Conversion.ToDouble(r.Cells["Rs-Amount"].Value);
				}
			}
			txtForeinchargesPkr.Text = ForeignAmount.ToString();
			txtLocalAmount.Text = LocalAmount.ToString();
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				CmbChargesFDBCNo.Focus();
			}
			else
			{
				CmbChargesType.Focus();
			}
			CalculateBankInvoiceBreakupRemaining();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		resetdetail();
		btnAddinGrid.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdFCBank_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdFCBank.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbChargesFDBCNo.Value = item.Cells["FDBCNo"].Value.ToString();
				CmbChargesType.Value = item.Cells["TypeId"].Value.ToString();
				((Control)(object)CmbChargesType).Text = item.Cells["Type"].Text.ToString();
				CmbChargesAccountType.Value = item.Cells["ChargesTypeId"].Value.ToString();
				((Control)(object)CmbChargesAccountType).Text = item.Cells["ChargesType"].Text.ToString();
				txtChargePercent.Text = Conversion.ToDouble((object)item.Cells["Prcnt"].Text).ToString("#,##0.####");
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				txtfcamount.Text = Conversion.ToDouble(item.Cells["FC-Amount"].Value).ToString("#,##0.####");
				txtpkramount.Text = Conversion.ToDouble(item.Cells["Rs-Amount"].Value).ToString("#,##0.####");
				CmbChargesAccountType.Focus();
				btnAddinGrid.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				grdFCBank.DataSource = table;
				grdFCBank.RetrieveStructure();
				gridsettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridsettings()
	{
		try
		{
			grdFCBank.RootTable.Columns["TypeId"].Visible = false;
			grdFCBank.RootTable.Columns["ChargesTypeId"].Visible = false;
			grdFCBank.RootTable.Groups.Add("FDBCNo");
			grdFCBank.RootTable.GroupTotals = (GroupTotals)2;
			grdFCBank.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdFCBank.RootTable.Columns["ChargesType"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["FC-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["Rs-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["ChargesType"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["FC-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["Rs-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["FDBCNo"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["Rs-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["Rs-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns["Rs-Amount"].FormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["Rs-Amount"].TotalFormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["FC-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["FC-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns["FC-Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFCBank.RootTable.Columns["FC-Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFCBank.RootTable.Columns.Add("Delete");
			grdFCBank.RootTable.Columns["Delete"].Key = "Delete";
			grdFCBank.RootTable.Columns["Delete"].Caption = "X";
			grdFCBank.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFCBank.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFCBank.RootTable.Columns["Delete"].ButtonText = "X";
			grdFCBank.RootTable.Columns["Delete"].Width = 20;
			grdFCBank.RootTable.Columns["Delete"].Position = 0;
			grdFCBank.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(grdFCBank);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtbankfbpNo.Text.Trim() == string.Empty || txtbankfbpNo.Text.Trim() == "0")
		{
			MessageBox.Show("FBP Bank Field Required");
			txtbankfbpNo.Focus();
			return false;
		}
		if (txtFCGrosamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFCGrosamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fc Gross Field Required");
			txtFCGrosamount.Focus();
			return false;
		}
		if (txtNetAmountFC.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetAmountFC.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fc Net Amount Field Required");
			txtNetAmountFC.Focus();
			return false;
		}
		if (txtexchangeRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtexchangeRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Exchange Rate Field Required");
			txtexchangeRate.Focus();
			return false;
		}
		if (txtNetAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Amount Field Required");
			txtNetAmount.Focus();
			return false;
		}
		if (((UltraGridBase)comSuplier).ActiveRow == null)
		{
			MessageBox.Show("Customer  Field Required");
			comSuplier.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPaymentTerm).ActiveRow == null)
		{
			MessageBox.Show("Payment term Field Required");
			cmbPaymentTerm.Focus();
			return false;
		}
		if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment" && ((UltraGridBase)CmbGainLossAc).ActiveRow == null)
		{
			MessageBox.Show("GainLossAc Field Required");
			CmbGainLossAc.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 2 && ((UltraGridBase)CmbConsignee).ActiveRow == null)
		{
			MessageBox.Show("Consignee Field Required");
			CmbConsignee.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 2 && txtFINo.Text == string.Empty)
		{
			txtFINo.Focus();
			MessageBox.Show("FI No Field Required");
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 1 && ((Control)(object)cmbInvoiceNo).Text == string.Empty)
		{
			MessageBox.Show("Invoice Field Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbaccounttype).ActiveRow == null)
		{
			MessageBox.Show(" Account Type Field Required");
			cmbaccounttype.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFBC.Text) > 0.0 && ((UltraGridBase)CmbFBCDebitAccount).ActiveRow == null)
		{
			MessageBox.Show("FBC Debit Account Field Required");
			CmbFBCDebitAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcurrency).ActiveRow == null)
		{
			MessageBox.Show("Currency Code Field Required");
			cmbcurrency.Focus();
			return false;
		}
		if (((UltraGridBase)cmbbankAcc).ActiveRow == null)
		{
			MessageBox.Show(" Account (DR) Field Required");
			cmbbankAcc.Focus();
			return false;
		}
		if (((UltraGridBase)cmbFcycodeThirdParty).ActiveRow == null || Conversion.ToInt(cmbFcycodeThirdParty.Value) == 0)
		{
			MessageBox.Show("Customer Third Currency Field Required");
			cmbFcycodeThirdParty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFcyExchRthirdParty.Text) == 0.0)
		{
			MessageBox.Show("Customer Exchange Rate Field Required");
			txtFcyExchRthirdParty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDebitCurrency).ActiveRow == null || Conversion.ToInt(CmbDebitCurrency.Value) == 0)
		{
			MessageBox.Show("Debit Fcy Currency Field Required");
			CmbDebitCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDebitExchangeRate.Text) == 0.0)
		{
			MessageBox.Show("Debit Exchange Rate Field Required");
			txtDebitExchangeRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDebitFcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Debit Fcy Amount Field Required");
			txtDebitFcyAmount.Focus();
			return false;
		}
		return true;
	}

	private void RESETMAIN()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			Id = 0;
			PreviousBalance = 0.0;
			table.Rows.Clear();
			GenerateDocumentNo();
			txtdate.Focus();
			((Control)(object)comSuplier).Text = "";
			((Control)(object)cmbLCOrder).Text = "";
			((Control)(object)cmbInvoiceNo).Text = "";
			((Control)(object)cmbbankAcc).Text = "";
			txtbankfbpNo.Text = "";
			((Control)(object)cmbcurrency).Text = "";
			((Control)(object)CmbConsignee).Text = "";
			txtFINo.Text = "";
			txtFBC.Text = "";
			((Control)(object)CmbFBCDebitAccount).Text = "";
			((Control)(object)cmbFcycodeThirdParty).Text = "";
			txtfcyAmountThirdParty.Text = "";
			txtFcyExchRthirdParty.Text = "";
			txtHcyExchRthirdParty.Text = "";
			txtReceiverExchangeRate.Text = "";
			txtReceiverFcyAmount.Text = "";
			((Control)(object)CmbDebitCurrency).Text = "";
			txtDebitExchangeRate.Text = "";
			txtDebitFcyAmount.Text = "";
			txtFCGrosamount.Text = "";
			txtForeinchargesPkr.Text = "";
			txtNetAmountFC.Text = "";
			txtexchangeRate.Text = "";
			txtNetAmount.Text = "";
			((Control)(object)cmbaccounttype).Text = "";
			txtbankDrAmount.Text = "";
			txtLocalAmount.Text = "";
			((UltraGridBase)CmbChargesType).Rows[0].Activate();
			txtRemarks.Clear();
			grdFCBank.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			VoucherHeadId = 0;
			BrancheId = 0;
			ProjectId = 0;
			txtExportvoucherRate.Text = "0";
			((Control)(object)cmbReferenceNo).Text = string.Empty;
			if (Conversion.ToInt(cmbPaymentTerm.Value) != 2)
			{
				lblConsignee.Visible = false;
				((Control)(object)CmbConsignee).Visible = false;
				labFiNo.Visible = false;
				txtFINo.Visible = false;
				txtRemarks.Width = 555;
			}
			tablebreakup.Rows.Clear();
			grdInvoiceWiseBreakUp.ClearStructure();
			((Control)(object)CmbChargesFDBCNo).Text = string.Empty;
			((UltraGridBase)CmbChargesFDBCNo).DataSource = null;
			tablePartybreakup.Rows.Clear();
			grdPartyPaymentsBreackUp.DataSource = tablePartybreakup;
			RowAddinGrdPartyPaymentsBreakUp();
			grdPartyPaymentsBreackUp.RetrieveStructure();
			gridPartyBreakUpsettings();
			PartyPaymentPanelVisible();
			RemainingBalanceGrid.ClearStructure();
			CalculateBankInvoiceBreakupRemaining();
			((Control)(object)grdPaymentTerms).Visible = false;
			dtPaymentTermsDetail.Rows.Clear();
			grdPaymentTerms.DataSource = dtPaymentTermsDetail;
			grdPaymentTerms.RetrieveStructure();
			gridPaymentTermsSettings();
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
			DatatableHelper.GlobalSupplierCustomerListsFillDbCall(UserAccount.OrganizationId, UserAccount.CompanyId);
			GetAllAccounts();
			CurrencyCOde();
			ChargerType();
			ReferenceNoFill();
			if (Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				BankAccountDr(15);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 2)
			{
				BankAccountDr(22);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 3)
			{
				BankAccountDr(2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		txtChargePercent.Text = "";
		txtRemarksDetail.Text = "";
		CmbChargesAccountType.Value = "";
		txtfcamount.Text = "";
		txtpkramount.Text = "";
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		RESETMAIN();
		resetdetail();
		table.Clear();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		grdFCBank.ClearStructure();
		ResetBreakUp();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Expected O, but got Unknown
		//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Expected O, but got Unknown
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Expected O, but got Unknown
		//IL_0d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Expected O, but got Unknown
		//IL_0ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Expected O, but got Unknown
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ExImFCBankReceipts fcbank = new ExImFCBankReceipts();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fcbank.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				double GrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double FBC = Conversion.ToDouble((object)txtFBC.Text);
				double BalanceAmount = Conversion.ToDouble((object)txtBalanceAmount.Text);
				if (GrossAmount > BalanceAmount)
				{
					throw new Exception("FCY Amount cannot be greater than Balance Amount please check");
				}
				double ThisReciptTotal = Conversion.ToDouble(grdPaymentTerms.GetTotal(grdPaymentTerms.RootTable.Columns["ThisReceipt"], (AggregateFunction)2));
				if (Conversion.ToDecimal((object)ThisReciptTotal) != Conversion.ToDecimal((object)GrossAmount) + Conversion.ToDecimal((object)FBC))
				{
					throw new Exception($"Fcy Amount not equal to Payment term grid total. Fcy Amount is {GrossAmount} & Payment Grid Total is {ThisReciptTotal}.");
				}
			}
			else if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				double columnSum = GridEX_Helper.GetColumnSum(grdInvoiceWiseBreakUp, "RealizedAmount");
				double totalFcGrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
				if (columnSum != totalFcGrossAmount)
				{
					throw new Exception("Total Realized Amount Is Not Equal to Fc Amount. Please Check!");
				}
			}
			if (Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) > 0.0)
			{
				if (((UltraGridBase)cmbFcycodeThirdParty).ActiveRow == null || Conversion.ToInt(cmbFcycodeThirdParty.Value) == 0)
				{
					MessageBox.Show("Third Currency field is required");
					return;
				}
				if (Conversion.ToDouble((object)txtFcyExchRthirdParty.Text) == 0.0)
				{
					MessageBox.Show("Third Currency Customer ExchangeRate field is required");
					return;
				}
			}
			fcbank.ProjectId = Conversion.ToInt(cmbproject.Value);
			fcbank.BranchId = Conversion.ToInt(cmbranches.Value);
			fcbank.DocumentTypeId = 203;
			fcbank.DocumentNo = Conversion.ToInt((object)txtdocNumber.Text);
			fcbank.DocumentDate = Conversion.ToDateTime((object)txtdate.Value);
			fcbank.SupplierCustomerId = Conversion.ToInt(comSuplier.Value);
			fcbank.ExImLcOrderId = Conversion.ToInt(cmbLCOrder.Value);
			fcbank.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			fcbank.InvoiceNo = Conversion.ToString((object)((Control)(object)cmbInvoiceNo).Text);
			fcbank.DebitAccountTypeId = Conversion.ToInt(cmbaccounttype.Value);
			fcbank.BankGlDrAc = Conversion.ToInt(cmbbankAcc.Value);
			fcbank.CompanyName = Conversion.ToString((object)((Control)(object)cmbbankAcc).Text);
			fcbank.BankFbpNo = Conversion.ToString((object)txtbankfbpNo.Text);
			fcbank.ExmLcPaymentTerm = ((Control)(object)cmbPaymentTerm).Text;
			fcbank.MultiCurrencyId = Conversion.ToInt(cmbcurrency.Value);
			fcbank.CurrencyCode = Conversion.ToString((object)((Control)(object)cmbcurrency).Text);
			fcbank.FcGrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
			fcbank.FcFBCharges = Conversion.ToDouble((object)txtFBC.Text);
			fcbank.FBCDebitAccountId = Conversion.ToInt(CmbFBCDebitAccount.Value);
			fcbank.FcNetAmount = Conversion.ToDouble((object)txtNetAmountFC.Text);
			fcbank.ThirdCurrencyId = Conversion.ToInt(cmbFcycodeThirdParty.Value);
			fcbank.ThirdCurrencyCode = Conversion.ToString((object)((Control)(object)cmbFcycodeThirdParty).Text);
			fcbank.ThirdCurrencyHcyExchangeRate = Conversion.ToDouble((object)txtHcyExchRthirdParty.Text);
			fcbank.ThirdCurrencyFcyExchangeRate = Conversion.ToDouble((object)txtFcyExchRthirdParty.Text);
			fcbank.ThirdCurrencyAmount = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
			fcbank.ThirdCurrencyReceiverExchangeRate = Conversion.ToDouble((object)txtReceiverExchangeRate.Text);
			fcbank.ThirdCurrencyReceiverFcyAmount = Conversion.ToDouble((object)txtReceiverFcyAmount.Text);
			fcbank.DebitAccountCurrencyId = Conversion.ToInt(CmbDebitCurrency.Value);
			fcbank.DebitAccountCurrencyExchangeRate = Conversion.ToDouble((object)txtDebitExchangeRate.Text);
			fcbank.DebitAccountCurrencyAmount = Conversion.ToDouble((object)txtDebitFcyAmount.Text);
			fcbank.ExchangeRate = Conversion.ToDouble((object)txtexchangeRate.Text);
			fcbank.NetAmountRs = Conversion.ToDouble((object)txtNetAmount.Text);
			fcbank.BankDrAmount = Conversion.ToDouble((object)txtbankDrAmount.Text);
			fcbank.VoucherHeadId = Conversion.ToInt(CmbGainLossAc.Value);
			fcbank.FcBankChargesAmountRs = fcbank.FcFBCharges * fcbank.ExchangeRate;
			fcbank.FcBankChargesTypeAcId = 0;
			fcbank.VoucherExchangeRate = Conversion.ToDouble((object)txtExportvoucherRate.Text);
			fcbank.Remarks = Conversion.ToString((object)txtRemarks.Text);
			if (RDBankSingleAndPartiesMulti.Checked)
			{
				fcbank.TransTypeId = 1;
			}
			else if (RDMultiBankDevice.Checked)
			{
				fcbank.TransTypeId = 2;
			}
			else if (rdMultiBankAndMultiParties.Checked)
			{
				fcbank.TransTypeId = 3;
			}
			fcbank.OrganizationId = UserAccount.OrganizationId;
			fcbank.CompanyId = UserAccount.CompanyId;
			fcbank.BranchId = BrancheId;
			fcbank.ProjectId = ProjectId;
			fcbank.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			fcbank.EntryUser = UserAccount.ID;
			fcbank.ModifyUser = UserAccount.ID;
			fcbank.EntryDate = DateTime.Now;
			fcbank.ModifyDate = DateTime.Now;
			fcbank.PostDate = DateTime.Now;
			fcbank.ReferenceNo = ((Control)(object)cmbReferenceNo).Text;
			fcbank.FcBankReceiptDetail = new List<ExImFcBankReceiptsDeductions>();
			fcbank.FcyBankReceiptBreakUpList = new List<FcyBankReceiptBreakUp>();
			fcbank.FcyBankReceiptBreakUpofPartyInvoiceslist = new List<FcyBankReceiptBreakUpofPartyInvoices>();
			fcbank.FcyPartyPaymentBreackUpForFinancialslist = new List<FcyPartyPaymentBreackUpForFinancials>();
			fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist = new List<ExImShipmentPartyAgainstAdvancesPayment>();
			fcbank.FcyBankReceiptPaymentTermDetails = new List<FcyBankReceiptPaymentTermDetail>();
			GridEXRow[] dataRows = grdFCBank.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				ExImFcBankReceiptsDeductions brd2 = new ExImFcBankReceiptsDeductions();
				brd2.FDBCNo = Conversion.ToString((object)r3.Cells["FDBCNo"].Text);
				if ((fcbank.TransTypeId == 2 || fcbank.TransTypeId == 3) && Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) != 4 && brd2.FDBCNo == string.Empty)
				{
					throw new Exception("FDBCNo Field is Required in Charges Grid ......");
				}
				brd2.Type = Conversion.ToString((object)r3.Cells["Type"].Text);
				brd2.ExImProceedsChargesTypeId = Conversion.ToInt(r3.Cells["ChargesTypeId"].Value);
				brd2.ProceedsRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
				brd2.ChargesPercent = Conversion.ToDouble((object)r3.Cells["Prcnt"].Text);
				brd2.FcAmount = Conversion.ToDouble((object)r3.Cells["FC-Amount"].Text);
				brd2.RsAmount = Conversion.ToDouble((object)r3.Cells["Rs-Amount"].Text);
				fcbank.FcBankReceiptDetail.Add(brd2);
			}
			string FDBCNo = "";
			dataRows = grdInvoiceWiseBreakUp.GetRows();
			foreach (GridEXRow r2 in dataRows)
			{
				FcyBankReceiptBreakUp Break2 = new FcyBankReceiptBreakUp();
				Break2.Id = Conversion.ToInt((object)r2.Cells["Id"].Text);
				if (Break2.Id == 0)
				{
					Break2.ActionTypeId = 1;
				}
				else if (Break2.Id > 0)
				{
					Break2.ActionTypeId = 2;
				}
				Break2.SenderRefNo = "";
				Break2.ReceiverRefNo = Conversion.ToString((object)r2.Cells["FDBCNo"].Text);
				if (fcbank.TransTypeId == 2 || fcbank.TransTypeId == 3)
				{
					if (r2.RowIndex == 0)
					{
						FDBCNo = Break2.ReceiverRefNo;
					}
					if (FDBCNo == Break2.ReceiverRefNo && r2.RowIndex != 0)
					{
						throw new Exception("FDBC # already add in Grid....");
					}
					FDBCNo = Break2.ReceiverRefNo;
				}
				Break2.StepStatus = Conversion.ToString((object)r2.Cells["Status"].Text);
				Break2.RefDocRecordId = Conversion.ToInt(r2.Cells["InvoiceId"].Value);
				Break2.GDId = Conversion.ToInt(r2.Cells["GDId"].Value);
				Break2.GdRefDocTypeId = Conversion.ToInt(r2.Cells["GdRefDocTypeId"].Value);
				Break2.RefDocumentTypeId = 4;
				Break2.RealizedAmount = Conversion.ToDecimal((object)r2.Cells["RealizedAmount"].Text);
				Break2.AgencyComm = Conversion.ToDecimal((object)r2.Cells["ForiegnAgencyCommAmount"].Text);
				Break2.SortNo = 1;
				fcbank.FcyBankReceiptBreakUpList.Add(Break2);
			}
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 1 || Conversion.ToInt(cmbPaymentTerm.Value) == 2)
			{
				FcyBankReceiptBreakUp Break = new FcyBankReceiptBreakUp();
				Break.Id = 0;
				Break.SenderRefNo = "";
				Break.ReceiverRefNo = fcbank.BankFbpNo;
				Break.StepStatus = "Final Part";
				Break.RefDocRecordId = ((Conversion.ToInt(cmbPaymentTerm.Value) == 1) ? Conversion.ToInt(cmbInvoiceNo.Value) : ((Conversion.ToInt(cmbPaymentTerm.Value) == 2) ? Conversion.ToInt(cmbLCOrder.Value) : 0));
				Break.RefDocumentTypeId = ((Conversion.ToInt(cmbPaymentTerm.Value) == 1) ? 1 : ((Conversion.ToInt(cmbPaymentTerm.Value) == 2) ? 2 : 0));
				Break.RealizedAmount = Conversion.ToDecimal((object)fcbank.FcGrossAmount);
				Break.AgencyComm = 0m;
				Break.ActionTypeId = 1;
				Break.SortNo = 1;
				fcbank.FcyBankReceiptBreakUpList.Add(Break);
			}
			if (fcbank.TransTypeId == 1 || fcbank.TransTypeId == 3)
			{
				decimal RealizedAmount = default(decimal);
				dataRows = grdPartyPaymentsBreackUp.GetRows();
				foreach (GridEXRow r in dataRows)
				{
					FcyPartyPaymentBreackUpForFinancials brd = new FcyPartyPaymentBreackUpForFinancials();
					if (Conversion.ToDecimal((object)r.Cells["TotalAmount"].Text) > 0m)
					{
						brd.TotalAmount = Conversion.ToDecimal((object)r.Cells["TotalAmount"].Text);
						brd.FBC = Conversion.ToDecimal((object)r.Cells["FBC"].Text);
						brd.RealizedAmount = Conversion.ToDecimal((object)r.Cells["RealizedAmount"].Text);
						if (brd.RealizedAmount == 0m)
						{
							throw new Exception("RealizedAmount Field is Required in PartyPaymentsBreackUp");
						}
						fcbank.FcyPartyPaymentBreackUpForFinancialslist.Add(brd);
						RealizedAmount += brd.RealizedAmount;
					}
				}
				if (RealizedAmount != Conversion.ToDecimal((object)fcbank.FcGrossAmount))
				{
					throw new Exception("Total Party Realized Amount not equal to FCY Amount please Check");
				}
			}
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1 && fcbank.FcyBankReceiptBreakUpList.Count == 0)
			{
				throw new Exception("InvoiceWise BreakUp Not Found in Grid please Check");
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				int LineId = 1;
				dataRows = grdPaymentTerms.GetRows();
				foreach (GridEXRow item in dataRows)
				{
					FcyBankReceiptPaymentTermDetail pd = new FcyBankReceiptPaymentTermDetail();
					pd.ExImInvoicePaymentTermsDetailId = Conversion.ToInt(item.Cells["InvoicePaymentDetailId"].Value);
					pd.ThisReceipt = Conversion.ToDouble(item.Cells["ThisReceipt"].Value);
					pd.PaymentTermId = Conversion.ToInt(item.Cells["PaymentTermId"].Value);
					pd.ExImInvoiceId = fcbank.ExImInvoiceId;
					pd.LineId = LineId;
					fcbank.FcyBankReceiptPaymentTermDetails.Add(pd);
					LineId++;
				}
			}
			if ((Conversion.ToInt(cmbPaymentTerm.Value) == 1 || Conversion.ToInt(cmbPaymentTerm.Value) == 2) && Conversion.ToInt(CmbConsignee.Value) > 0 && !string.IsNullOrEmpty(txtFINo.Text.Trim()))
			{
				ExImShipmentPartyAgainstAdvancesPayment shpAdvanec = new ExImShipmentPartyAgainstAdvancesPayment();
				shpAdvanec.ContractId = Conversion.ToInt(cmbLCOrder.Value);
				shpAdvanec.ConsigneeId = Conversion.ToInt(CmbConsignee.Value);
				shpAdvanec.FINo = Conversion.ToString((object)txtFINo.Text.Trim());
				fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist.Add(shpAdvanec);
			}
			fcbank.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			fcbank.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImFCBankReceipts>(fcbank, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImFCBankReceiptsBLL.Save(fcbank);
			if (Id == 0)
			{
				MessageBox.Show("Record Save Successfully [" + fcbank.DocumentNo + "]");
			}
			else
			{
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachmentsBLL.RemoveByIdAndNames(Id, base.Name, 203, 0);
				}
				MessageBox.Show("Record Update Successfully [" + fcbank.DocumentNo + "]");
			}
			RESETMAIN();
			Id = success;
			VoucherHeadIdGet();
			if (chkpreview.Checked)
			{
				DataTable dt = new DataTable();
				dt = VoucherReports.VoucherReport(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 203,
					Id = VoucherHeadId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = null;
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
					val.RptPerameter("@CompanyName", UserAccount.CompName);
					val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
				}
			}
			Id = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			Id = ID;
			PreviousBalance = 0.0;
			ExImFCBankReceipts fcbank = new ExImFCBankReceipts();
			fcbank = ExImFCBankReceiptsBLL.GetByID(ID);
			if (fcbank != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				VoucherHeadIdGet();
				txtdocNumber.Text = fcbank.DocumentNo.ToString();
				tabControl1.SelectedIndex = 0;
				txtdate.Value = fcbank.DocumentDate;
				((Control)(object)cmbPaymentTerm).Text = fcbank.ExmLcPaymentTerm;
				txtFCGrosamount.Text = fcbank.FcGrossAmount.ToString("#,#.###");
				PreviousBalance = Conversion.ToDouble((object)txtFCGrosamount.Text.Trim());
				if (fcbank.ExmLcPaymentTerm == "Advanced Payment" || fcbank.ExImLcOrderId > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("LcOrderNo");
					dt.Rows.Add(fcbank.ExImLcOrderId, fcbank.LcOrderNo);
					DDL.BindDDL(dt, cmbLCOrder, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
					((UltraGridBase)cmbLCOrder).Rows[0].Activate();
					cmbLCOrder.ReadOnly = false;
					cmbInvoiceNo.ReadOnly = true;
					cmbPaymentTerm_Leave(null, null);
					if (fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist.Count > 0)
					{
						lblConsignee.Visible = true;
						((Control)(object)CmbConsignee).Visible = true;
						labFiNo.Visible = true;
						txtFINo.Visible = true;
						txtRemarks.Width = 234;
						CmbConsignee.Value = Conversion.ToInt((object)fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist[0].ConsigneeId);
						txtFINo.Text = Conversion.ToString((object)fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist[0].FINo);
					}
				}
				if (Conversion.ToInt((object)fcbank.ExImInvoiceId) > 0)
				{
					CustomersFillByInvoicePayment();
					DataTable dtInvoice = new DataTable();
					dtInvoice.Columns.Add("Id");
					dtInvoice.Columns.Add("InvoiceNo");
					dtInvoice.Rows.Add(fcbank.ExImInvoiceId, fcbank.InvoiceNo);
					DDL.BindDDL(dtInvoice, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoiceNo).Rows[0].Activate();
					cmbInvoiceNo.ReadOnly = false;
					cmbLCOrder.ReadOnly = true;
					((Control)(object)grdPaymentTerms).Visible = true;
					cmbInvoiceNo_Leave(null, null);
				}
				if (fcbank.ExmLcPaymentTerm != "Invoice Payment")
				{
					ExportCustomersFillFomGlobal();
				}
				if (Conversion.ToInt((object)fcbank.ExImInvoiceId) == 0 && Conversion.ToInt((object)fcbank.ExImLcOrderId) == 0)
				{
					CustomersFillByInvoicePayment();
				}
				comSuplier.Value = fcbank.SupplierCustomerId;
				cmbFcycodeThirdParty.Value = fcbank.ThirdCurrencyId;
				txtbankfbpNo.Text = fcbank.BankFbpNo;
				cmbaccounttype.Value = fcbank.DebitAccountTypeId;
				if (Conversion.ToInt(cmbaccounttype.Value) == 1)
				{
					BankAccountDr(15);
				}
				if (Conversion.ToInt(cmbaccounttype.Value) == 2)
				{
					BankAccountDr(22);
				}
				if (Conversion.ToInt(cmbaccounttype.Value) == 3)
				{
					BankAccountDr(2);
				}
				cmbbankAcc.Value = fcbank.BankGlDrAc;
				cmbbankAcc_Leave(null, null);
				cmbcurrency.Value = fcbank.MultiCurrencyId;
				txtNetAmountFC.Text = fcbank.FcNetAmount.ToString("#,#.###");
				txtexchangeRate.Text = Conversion.ToDouble((object)fcbank.ExchangeRate).ToString();
				CmbDebitCurrency.Value = fcbank.DebitAccountCurrencyId;
				txtDebitExchangeRate.Text = Conversion.ToDouble((object)fcbank.DebitAccountCurrencyExchangeRate).ToString("#,##0.####");
				txtDebitFcyAmount.Text = Conversion.ToDouble((object)fcbank.DebitAccountCurrencyAmount).ToString("#,##0.####");
				txtHcyExchRthirdParty.Text = fcbank.ThirdCurrencyHcyExchangeRate.ToString();
				txtFcyExchRthirdParty.Text = fcbank.ThirdCurrencyFcyExchangeRate.ToString();
				txtfcyAmountThirdParty.Text = fcbank.ThirdCurrencyAmount.ToString();
				txtReceiverExchangeRate.Text = fcbank.ThirdCurrencyReceiverExchangeRate.ToString();
				txtReceiverFcyAmount.Text = fcbank.ThirdCurrencyReceiverFcyAmount.ToString();
				txtNetAmount.Text = fcbank.NetAmountRs.ToString("#,#.##");
				txtbankDrAmount.Text = fcbank.BankDrAmount.ToString("#,#.##");
				CmbGainLossAc.Value = fcbank.VoucherHeadId;
				txtRemarks.Text = Conversion.ToString((object)fcbank.Remarks);
				((Control)(object)cmbReferenceNo).Text = fcbank.ReferenceNo;
				txtFBC.Text = Conversion.ToString((object)fcbank.FcFBCharges);
				CmbFBCDebitAccount.Value = Conversion.ToInt((object)fcbank.FBCDebitAccountId);
				if (Conversion.ToInt((object)fcbank.TransTypeId) == 1)
				{
					RDBankSingleAndPartiesMulti.Checked = true;
				}
				else if (Conversion.ToInt((object)fcbank.TransTypeId) == 3)
				{
					rdMultiBankAndMultiParties.Checked = true;
				}
				else
				{
					RDMultiBankDevice.Checked = true;
				}
				table.Clear();
				double ForeignAmount = 0.0;
				double LocalAmount = 0.0;
				for (int k = 0; k < fcbank.FcBankReceiptDetail.Count; k++)
				{
					string FDBCNO = "";
					FDBCNO = ((fcbank.FcBankReceiptDetail[k].FDBCNo != null && !(fcbank.FcBankReceiptDetail[k].FDBCNo == string.Empty)) ? fcbank.FcBankReceiptDetail[k].FDBCNo : fcbank.BankFbpNo);
					table.Rows.Add(FDBCNO, 1, fcbank.FcBankReceiptDetail[k].Type, fcbank.FcBankReceiptDetail[k].ExImProceedsChargesTypeId, fcbank.FcBankReceiptDetail[k].ProceedsChargesTypeCode, fcbank.FcBankReceiptDetail[k].ChargesPercent, fcbank.FcBankReceiptDetail[k].ProceedsRemarks, fcbank.FcBankReceiptDetail[k].FcAmount, fcbank.FcBankReceiptDetail[k].RsAmount);
					if (fcbank.FcBankReceiptDetail[k].Type == "Foreign")
					{
						ForeignAmount += fcbank.FcBankReceiptDetail[k].RsAmount;
					}
					else
					{
						LocalAmount += fcbank.FcBankReceiptDetail[k].RsAmount;
					}
				}
				if (Conversion.ToInt(cmbPaymentTerm.Value) == 4)
				{
					comSuplier_Leave(null, null);
					panelInvoiceWiseBreakup.Visible = true;
					for (int j = 0; j < fcbank.FcyBankReceiptBreakUpList.Count; j++)
					{
						decimal TotalAmount = default(decimal);
						TotalAmount = fcbank.FcyBankReceiptBreakUpList[j].RealizedAmount + fcbank.FcyBankReceiptBreakUpList[j].AgencyComm;
						tablebreakup.Rows.Add(fcbank.FcyBankReceiptBreakUpList[j].Id, fcbank.FcyBankReceiptBreakUpList[j].RefDocRecordId, fcbank.FcyBankReceiptBreakUpList[j].InvoiceNo, fcbank.FcyBankReceiptBreakUpList[j].GDId, fcbank.FcyBankReceiptBreakUpList[j].GDNo, fcbank.FcyBankReceiptBreakUpList[j].GdRefDocTypeId, TotalAmount, fcbank.FcyBankReceiptBreakUpList[j].RealizedAmount, fcbank.FcyBankReceiptBreakUpList[j].AgencyComm, fcbank.FcyBankReceiptBreakUpList[j].ReceiverRefNo, fcbank.FcyBankReceiptBreakUpList[j].StepStatus);
					}
					GdBindinExpensesInvoice();
				}
				if (tablebreakup.Rows.Count > 0)
				{
					grdInvoiceWiseBreakUp.DataSource = tablebreakup;
					grdInvoiceWiseBreakUp.RetrieveStructure();
					gridBreakUpsettings();
				}
				else
				{
					panelInvoiceWiseBreakup.Visible = false;
				}
				txtForeinchargesPkr.Text = ForeignAmount.ToString("0,0");
				txtLocalAmount.Text = LocalAmount.ToString("0,0");
				grdFCBank.DataSource = table;
				grdFCBank.RetrieveStructure();
				gridsettings();
				tablePartybreakup.Rows.Clear();
				if (fcbank.FcyPartyPaymentBreackUpForFinancialslist.Count > 0)
				{
					for (int i = 0; i < fcbank.FcyPartyPaymentBreackUpForFinancialslist.Count; i++)
					{
						tablePartybreakup.Rows.Add(fcbank.FcyPartyPaymentBreackUpForFinancialslist[i].RealizedAmount, fcbank.FcyPartyPaymentBreackUpForFinancialslist[i].FBC, fcbank.FcyPartyPaymentBreackUpForFinancialslist[i].TotalAmount);
					}
					grdPartyPaymentsBreackUp.DataSource = tablePartybreakup;
					grdPartyPaymentsBreackUp.RetrieveStructure();
					gridPartyBreakUpsettings();
				}
				else
				{
					RowAddinGrdPartyPaymentsBreakUp();
					gridPartyBreakUpsettings();
				}
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				CalculateBankInvoiceBreakupRemaining();
			}
			else
			{
				RESETMAIN();
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
		else
		{
			txtdate.Focus();
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
			DataGridHistory.DataSource = null;
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
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtTerm = new DataTable();
			dtTerm.Columns.Add("Id", typeof(int));
			dtTerm.Columns.Add("Name", typeof(string));
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("Name", typeof(string));
			DataTable dtAc = new DataTable();
			dtAc.Columns.Add("Id", typeof(int));
			dtAc.Columns.Add("Name", typeof(string));
			DataTable dt = ExImFCBankReceiptsBLL.GetDataForDropDownFromFcyBankReceipts(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PaymentTerm")
					{
						dtTerm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ReceiverAccount")
					{
						dtAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
					{
						dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			}
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDL(dtTerm, CmbPaymentTermHistory, "Id", "Name", "Payment Term", ZeroIndex: false);
				((UltraGridBase)CmbPaymentTermHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoiceNoHistory, "Id", "Name", "Invoice No", ZeroIndex: false);
			}
			if (dtAc.Rows.Count > 0)
			{
				DDL.BindDDL(dtAc, CmbReceiverAcHistory, "Id", "Name", "Receiver/Bank Ac", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			bindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters br = new ReportsParameters();
			br.OrganizationId = UserAccount.OrganizationId;
			br.CompanyId = UserAccount.CompanyId;
			br.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			br.DocumentTypeId = 203;
			br.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!br.CanViewAllRecord)
			{
				br.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				br.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				br.ToDate = ToDateHistory.Value;
			}
			br.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			br.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			br.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			br.PaymentTerm = Conversion.ToString((object)((Control)(object)CmbPaymentTermHistory).Text);
			br.InvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			br.ReceiverAcId = Conversion.ToInt(CmbReceiverAcHistory.Value);
			DataTable Historydt = ExImFCBankReceiptsBLL.HistoryForm(br);
			if (Historydt.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentNo", typeof(int));
				dt.Columns.Add("DocumentDate", typeof(DateTime));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("BankGLDrAc", typeof(string));
				dt.Columns.Add("BankFbpNo", typeof(string));
				dt.Columns.Add("BankDrAmount", typeof(double));
				dt.Columns.Add("CurrencyCode", typeof(string));
				dt.Columns.Add("FcGrossAmount", typeof(double));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("FBCAccount", typeof(string));
				dt.Columns.Add("FcFBCharges", typeof(double));
				dt.Columns.Add("FcNetAmount", typeof(double));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("NetAmountRs", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("UserName", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < Historydt.Rows.Count; i++)
				{
					dt.Rows.Add(Historydt.Rows[i]["Id"], Historydt.Rows[i]["VoucherHeadId"], Historydt.Rows[i]["DocumentTypeId"], Historydt.Rows[i]["DocumentNo"], Historydt.Rows[i]["DocumentDate"], Historydt.Rows[i]["CustomerName"], Historydt.Rows[i]["InvoiceNo"], Historydt.Rows[i]["PaymentTerm"], Historydt.Rows[i]["AccountType"], Historydt.Rows[i]["BankGLDrAc"], Historydt.Rows[i]["BankFbpNo"], Historydt.Rows[i]["BankDrAmount"], Historydt.Rows[i]["CurrencyCode"], Historydt.Rows[i]["FcGrossAmount"], Historydt.Rows[i]["ReferenceNo"], Historydt.Rows[i]["FBCAccount"], Historydt.Rows[i]["FcFBCharges"], Historydt.Rows[i]["FcNetAmount"], Historydt.Rows[i]["ExchangeRate"], Historydt.Rows[i]["NetAmountRs"], Historydt.Rows[i]["EntryDate"], Historydt.Rows[i]["UserName"], Historydt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.Columns["DocumentNo"].Caption = "DocNo";
			DataGridHistory.RootTable.Columns["DocumentDate"].Caption = "DocDate";
			DataGridHistory.RootTable.Columns["CurrencyCode"].Caption = "Fcy Code";
			DataGridHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["InvoiceNo"].Caption = "InvoiceNo / ContractNo";
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 65;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("GLBreakup");
			DataGridHistory.RootTable.Columns["GLBreakup"].Caption = "G&LBreakup";
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonText = "G&LBreakup";
			DataGridHistory.RootTable.Columns["GLBreakup"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["Edit"].Position = 2;
			DataGridHistory.RootTable.Columns["GLBreakup"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BankDrAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BankDrAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcFBCharges"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcFBCharges"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcFBCharges"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcFBCharges"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcNetAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcNetAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetAmountRs"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetAmountRs"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetAmountRs"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetAmountRs"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DocumentNo"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.TermConstant;
			DataGridHistory.RootTable.Columns["AccountType"].Width = Constants.ExportConstants.CarrierTypeConstant;
			DataGridHistory.RootTable.Columns["BankGLDrAc"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["BankFbpNo"].Width = 120;
			DataGridHistory.RootTable.Columns["BankDrAmount"].Width = Constants.ExportConstants.LcyAmountConstant;
			DataGridHistory.RootTable.Columns["CurrencyCode"].Width = Constants.ExportConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ReferenceNo"].Width = 120;
			DataGridHistory.RootTable.Columns["FBCAccount"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["FcFBCharges"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FcNetAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["NetAmountRs"].Width = Constants.ExportConstants.LcyAmountConstant;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["UserName"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachment";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachment";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 110;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 203);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Voucher")
			{
				DataTable dt = new DataTable();
				dt = VoucherReports.VoucherReport(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 203,
					Id = Conversion.ToInt(r.Cells["VoucherHeadId"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = null;
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
				val.RptPerameter("@CompanyName", UserAccount.CompName);
				val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
			}
			else if (e.Column.Key == "Print")
			{
				DataTable dt2 = new DataTable();
				dt2 = ExportReports.ExImRptFCBankReceipts_516(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(r.Cells["Id"].Value)
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val2 = null;
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
				val2.RptPerameter("@CompanyName", UserAccount.CompName);
				val2.ShowReportWithDataTable(dt2, "516-ExImRptFCBankReceipts.rpt");
			}
			else if (e.Column.Key == "Edit")
			{
				RESETMAIN();
				Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			else if (e.Column.Key == "GLBreakup")
			{
				GainAndLossBreakUp gainAndLossBreakUp = new GainAndLossBreakUp(UserAccount);
				gainAndLossBreakUp.FcyReceiptId = Conversion.ToInt(r.Cells["Id"].Value);
				gainAndLossBreakUp.Show();
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 203, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
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
			Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
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

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExImRptFCBankReceipts_516(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		GenerateReportVoucher102();
	}

	private void GenerateReportVoucher102()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 203,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = null;
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Acfrmfcbankreceipt_KeyDown(object sender, KeyEventArgs e)
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					cmbInvoicenobreakup.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
					{
						CmbChargesFDBCNo.Focus();
					}
					else
					{
						CmbChargesType.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdate.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			new FcYBankChargesTypes(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceAmountGet()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		try
		{
			double InvoiceAmt = 0.0;
			double RecAmount = 0.0;
			double BalanceAmt = 0.0;
			txtInvoiceAmount.Text = "0";
			txtRecAmount.Text = "0";
			txtBalanceAmount.Text = "0";
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				DataTable dt = ExImInvoice.GetFcyBankInvoicesBalance(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(cmbInvoicenobreakup.Value)
				});
				if (dt.Rows.Count > 0)
				{
					InvoiceAmt = Conversion.ToDouble(dt.Rows[0]["InvoiceAmount"]);
					RecAmount = Conversion.ToDouble(dt.Rows[0]["FcyReceivedAmount"]);
					BalanceAmt = InvoiceAmt - RecAmount;
				}
			}
			else if (Conversion.ToInt(cmbInvoiceNo.Value) > 0)
			{
				DataTable dt2 = ExImInvoice.GetFcYAmount(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(cmbInvoiceNo.Value)
				});
				if (dt2.Rows.Count > 0)
				{
					InvoiceAmt = Conversion.ToDouble(dt2.Rows[0]["InvoiceAmount"]);
					RecAmount = Conversion.ToDouble(dt2.Rows[0]["FcyReceivedAmount"]);
					RecAmount -= PreviousBalance;
					BalanceAmt = InvoiceAmt - RecAmount;
				}
			}
			txtInvoiceAmount.Text = InvoiceAmt.ToString("#,#.###");
			txtRecAmount.Text = RecAmount.ToString("#,#.###");
			txtBalanceAmount.Text = BalanceAmt.ToString("#,#.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtbankcharges_Leave(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void ThirdExchRateByFcGross()
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtfcyAmountThirdParty.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double FCGrosamount = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double FcyExchRthirdParty = num / FCGrosamount;
				txtFcyExchRthirdParty.Text = Conversion.ToString((object)Math.Round(FcyExchRthirdParty, 3));
			}
			else
			{
				txtFcyExchRthirdParty.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FcGrossAmtByThirdParty()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFcyExchRthirdParty.Text) != 0.0 && Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double FcyExchRthirdParty = Conversion.ToDouble((object)txtFcyExchRthirdParty.Text);
				double ThirdFcAmt = num / FcyExchRthirdParty;
				txtFCGrosamount.Text = Conversion.ToString((object)Math.Round(ThirdFcAmt, 3));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtbankcharges_TextChanged(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void txtexchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b;
				txtNetAmount.Text = c.ToString("#,##0.####");
				txtbankDrAmount.Text = c.ToString("#,##0.####");
			}
			else
			{
				txtNetAmount.Text = "0";
				txtbankDrAmount.Text = "0";
			}
			NetAmountFC();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				txtNetAmount.Text = (num * b).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankDebitAmountCalculate()
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtNetAmount.Text);
				double c = double.Parse(grdFCBank.GetTotal(grdFCBank.RootTable.Columns["Rs-Amount"], (AggregateFunction)2).ToString());
				txtbankDrAmount.Text = (num - c).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetAmountFC()
	{
		try
		{
			if (!(txtexchangeRate.Text != string.Empty) || !(txtFCGrosamount.Text != string.Empty))
			{
				return;
			}
			double CommAmount = 0.0;
			GridEXRow[] rows;
			if (grdInvoiceWiseBreakUp.GetRows().Length != 0)
			{
				rows = grdInvoiceWiseBreakUp.GetRows();
				foreach (GridEXRow item in rows)
				{
					CommAmount += Conversion.ToDouble(item.Cells["ForiegnAgencyCommAmount"].Value);
				}
			}
			double a = Conversion.ToDouble((object)txtexchangeRate.Text);
			double b = Conversion.ToDouble((object)txtFCGrosamount.Text);
			double c = a * b;
			txtbankDrAmount.Text = c.ToString("#,##0.####");
			txtNetAmountFC.Text = b.ToString("#,##0.####");
			double RsAmountDetail = Conversion.ToDouble((object)txtfcamount.Text) * a;
			txtpkramount.Text = RsAmountDetail.ToString("#,##0.####");
			if (grdFCBank.GetRows().Count() <= 0)
			{
				return;
			}
			double Fcamount = 0.0;
			double RsAmount = 0.0;
			rows = grdFCBank.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (r.Cells["Type"].Text == "Foreign")
				{
					Fcamount += Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				}
				if (Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text.ToString()) > 0.0)
				{
					double prcnt = Conversion.ToDouble(r.Cells["Prcnt"].Value);
					if (prcnt > 0.0)
					{
						double amount = (b + CommAmount) * a * prcnt / 100.0;
						r.Cells["Rs-Amount"].Value = amount;
					}
					RsAmount += Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				}
				r.EndEdit();
			}
			double BankAmount = c - RsAmount;
			double FcyNetAmount = b - Fcamount;
			txtNetAmountFC.Text = FcyNetAmount.ToString("#,##0.####");
			txtbankDrAmount.Text = BankAmount.ToString("#,##0,####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comSuplier_Leave(object sender, EventArgs e)
	{
		try
		{
			PaymentTerm();
			if (((UltraGridBase)comSuplier).ActiveRow != null && Conversion.ToInt(comSuplier.Value) > 0)
			{
				cmbFcycodeThirdParty.Value = Conversion.ToInt(((UltraDropDownBase)comSuplier).SelectedRow.Cells["GlAccountCurrencyId"].Value);
			}
			else
			{
				((Control)(object)cmbFcycodeThirdParty).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGainLossAc_Leave(object sender, EventArgs e)
	{
		try
		{
			CmbChargesType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExchangeRateandCurrencyFromVoucher()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImInvoice obj = new ExImInvoice();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4)
			{
				obj.Id = Conversion.ToInt(cmbInvoicenobreakup.Value);
			}
			else
			{
				obj.Id = Conversion.ToInt(cmbInvoiceNo.Value);
			}
			DataTable dt = ExImInvoice.GetExchangeRateandCurrencyFromVoucher(obj);
			if (dt.Rows.Count > 0)
			{
				txtExportvoucherRate.Text = Conversion.ToDouble(dt.Rows[0]["ExchangeCurrencyRate"]).ToString("#,#.##");
				cmbcurrency.Value = Conversion.ToInt(dt.Rows[0]["MultiCurrencyId"]);
			}
			else
			{
				txtExportvoucherRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getPaymentTermDetailByInvoiceId(int InvoiceId)
	{
		try
		{
			dtPaymentTermsDetail.Rows.Clear();
			DataTable dt = ExImInvoice.getPaymentTermDetailByInvoiceId(InvoiceId, Id);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow item in dt.Rows)
				{
					dtPaymentTermsDetail.Rows.Add(item["Id"], item["PaymentTermId"], item["PaymentTerm"], item["FcyAmount"], item["Received"], item["Balance"], item["ThisReceipt"]);
				}
				grdPaymentTerms.DataSource = dtPaymentTermsDetail;
				grdPaymentTerms.RetrieveStructure();
				gridPaymentTermsSettings();
			}
			else
			{
				grdPaymentTerms.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				InvoiceAmountGet();
				GetExchangeRateandCurrencyFromVoucher();
				getPaymentTermDetailByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value));
			}
			else
			{
				dtPaymentTermsDetail.Rows.Clear();
				grdPaymentTerms.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_ValueChanged(object sender, EventArgs e)
	{
		InvoiceAmountGet();
		GetExchangeRateandCurrencyFromVoucher();
	}

	private void txtpkramount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcamount_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		NetAmountFC();
	}

	private void txtFCGrosamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b;
				txtNetAmount.Text = c.ToString("#,##0.####");
				txtbankDrAmount.Text = c.ToString("#,##0.####");
			}
			NetAmountFC();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesContractAmountGet()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetFcYAmountByContractId(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbLCOrder.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InvoiceAmt = 0.0;
				double RecAmount = 0.0;
				double BalanceAmt = 0.0;
				InvoiceAmt = Conversion.ToDouble(dt.Rows[0]["InvoiceAmount"]);
				RecAmount = Conversion.ToDouble(dt.Rows[0]["FcyReceivedAmount"]);
				BalanceAmt = InvoiceAmt - RecAmount;
				txtInvoiceAmount.Text = InvoiceAmt.ToString("0,0");
				txtRecAmount.Text = RecAmount.ToString("0,0");
				txtBalanceAmount.Text = BalanceAmt.ToString("0,0");
			}
			else
			{
				txtInvoiceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				PreviousBalance = 0.0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLCOrder_Leave(object sender, EventArgs e)
	{
		try
		{
			SalesContractAmountGet();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLCOrder_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			SalesContractAmountGet();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmountThirdParty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			FcGrossAmtByThirdParty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyExchRthirdParty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			FcGrossAmtByThirdParty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFCGrosamount_Leave(object sender, EventArgs e)
	{
		try
		{
			ThirdExchRateByFcGross();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatePercntAmount()
	{
		try
		{
			if (((UltraGridBase)CmbChargesType).ActiveRow != null)
			{
				if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToDouble((object)txtexchangeRate.Text) > 0.0 && ((UltraGridBase)CmbChargesFDBCNo).ActiveRow != null)
				{
					double ChargePercent = Conversion.ToDouble((object)txtChargePercent.Text);
					double NetpkrAmount = 0.0;
					GridEXRow[] rows = grdInvoiceWiseBreakUp.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToString(CmbChargesFDBCNo.Value).ToUpper() == Conversion.ToString(item.Cells["FDBCNo"].Value).ToUpper())
						{
							NetpkrAmount += (Conversion.ToDouble(item.Cells["RealizedAmount"].Value) + Conversion.ToDouble(item.Cells["ForiegnAgencyCommAmount"].Value)) * Conversion.ToDouble((object)txtexchangeRate.Text);
						}
					}
					if (!(((Control)(object)CmbChargesType).Text == "Local"))
					{
						return;
					}
					if (Conversion.ToString(base.ActiveControl.Tag) == "Percent")
					{
						if (ChargePercent > 0.0 && NetpkrAmount > 0.0)
						{
							double PkrAmount = NetpkrAmount * ChargePercent / 100.0;
							txtpkramount.Text = PkrAmount.ToString("#,##0.####");
						}
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "PkrAmount")
					{
						txtChargePercent.Text = string.Empty;
					}
					return;
				}
				double ChargePercent2 = Conversion.ToDouble((object)txtChargePercent.Text);
				double NetpkrAmount2 = Conversion.ToDouble((object)txtNetAmount.Text);
				Conversion.ToDouble((object)txtpkramount.Text);
				if (!(((Control)(object)CmbChargesType).Text == "Local") || !(NetpkrAmount2 > 0.0))
				{
					return;
				}
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "Percent")
				{
					if (ChargePercent2 > 0.0 && NetpkrAmount2 > 0.0)
					{
						double PkrAmount2 = NetpkrAmount2 * ChargePercent2 / 100.0;
						txtpkramount.Text = PkrAmount2.ToString("#,##0.####");
					}
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PkrAmount")
				{
					txtChargePercent.Text = string.Empty;
				}
			}
			else
			{
				txtfcamount.Text = string.Empty;
				txtpkramount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbChargesType_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtChargePercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFCGrosamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtexchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtpkramount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfcamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfcyAmountThirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFcyExchRthirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtHcyExchRthirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtChargePercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFBC_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdFCBank_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFCBank.CurrentRow;
				table.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
			}
			grdFCBank.UpdateData();
			BankDebitAmountCalculate();
			double LocalAmount = 0.0;
			double ForeignAmount = 0.0;
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString((object)item.Cells["Type"].Text) == "Foreign")
				{
					ForeignAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
				else
				{
					LocalAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
			}
			txtForeinchargesPkr.Text = ForeignAmount.ToString("#,##0.####");
			txtLocalAmount.Text = LocalAmount.ToString("#,##0.####");
			CalculateBankInvoiceBreakupRemaining();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LcOrder()
	{
		try
		{
			DataTable dtLcOrder = CommonServices.GetLcOrderNoBySupplierCustomerId(Conversion.ToInt(comSuplier.Value));
			if (dtLcOrder.Rows.Count > 0)
			{
				DDL.BindDDL(dtLcOrder, cmbLCOrder, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadInvoice_Click(object sender, EventArgs e)
	{
		try
		{
			LoadExportInvoice = new LoadExportInvoice(UserAccount);
			LoadExportInvoice.ShowDialog();
			LoadExportInvoice.BringToFront();
			BindInvoiceDataOnForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInvoiceDataOnForm()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (LoadExportInvoice.dtSupply.Rows.Count > 0)
			{
				DataTable dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(LoadExportInvoice.dtSupply.Rows[0]["InvoiceId"])
				});
				if (dt.Rows.Count > 0)
				{
					cmbPaymentTerm_Leave(null, null);
					comSuplier.Value = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
					InvoiceNo();
					cmbInvoiceNo.Value = Conversion.ToInt(LoadExportInvoice.dtSupply.Rows[0]["InvoiceId"]);
					cmbInvoiceNo_Leave(null, null);
				}
				else
				{
					((Control)(object)cmbLCOrder).Text = string.Empty;
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((Control)(object)comSuplier).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddBreakup_Click(object sender, EventArgs e)
	{
		try
		{
			GdBreakUpId = 0;
			if (FormValidationbreakUp())
			{
				tablebreakup.Rows.Add(0, Conversion.ToInt(cmbInvoicenobreakup.Value), Conversion.ToString((object)((Control)(object)cmbInvoicenobreakup).Text.Trim()), Conversion.ToInt(cmbGdNoBreakup.Value), Conversion.ToString((object)((Control)(object)cmbGdNoBreakup).Text.Trim()), Conversion.ToInt(((UltraDropDownBase)cmbGdNoBreakup).SelectedRow.Cells["DocumentTypeId"].Value), Conversion.ToDouble((object)txtInvoiceAmountBreakup.Text), Conversion.ToDouble((object)txtRealizedAmtBreakup.Text.Trim()), Conversion.ToDouble((object)txtCommisionamtbreakUp.Text.Trim()), Conversion.ToString((object)txtFDBCNoBreakup.Text.Trim()), Conversion.ToString((object)((Control)(object)cmbStatusBreakup).Text.Trim()));
				grdInvoiceWiseBreakUp.DataSource = tablebreakup;
				grdInvoiceWiseBreakUp.RetrieveStructure();
				gridBreakUpsettings();
				ResetBreakUp();
				AddTotalRealizedAmountInHeaderFcAmount();
				cmbInvoicenobreakup.Focus();
				GdBindinExpensesInvoice();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GdBindinExpensesInvoice()
	{
		try
		{
			((Control)(object)CmbChargesFDBCNo).Text = string.Empty;
			((UltraGridBase)CmbChargesFDBCNo).DataSource = null;
			if (tablebreakup != null && tablebreakup.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("FDBCNoId", typeof(string));
				dt.Columns.Add("FDBCNo", typeof(string));
				for (int i = 0; i < tablebreakup.Rows.Count; i++)
				{
					dt.Rows.Add(tablebreakup.Rows[i]["FDBCNo"], tablebreakup.Rows[i]["FDBCNo"]);
				}
				if (dt != null && dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbChargesFDBCNo, "FDBCNoId", "FDBCNo", "FDBC #", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddTotalRealizedAmountInHeaderFcAmount()
	{
		try
		{
			if (grdInvoiceWiseBreakUp.GetRows().Count() > 0)
			{
				double TotalRealizedAmount = Conversion.ToDouble(grdInvoiceWiseBreakUp.GetTotal(grdInvoiceWiseBreakUp.RootTable.Columns["RealizedAmount"], (AggregateFunction)2));
				txtFCGrosamount.Text = TotalRealizedAmount.ToString("#,##0.###");
			}
			else
			{
				txtFCGrosamount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdatebreakup_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationbreakUp())
			{
				tablebreakup.Rows[updateDetailIndexBreakUp]["InvoiceId"] = Conversion.ToInt(cmbInvoicenobreakup.Value);
				tablebreakup.Rows[updateDetailIndexBreakUp]["InvoiceNo"] = Conversion.ToString((object)((Control)(object)cmbInvoicenobreakup).Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["GDId"] = Conversion.ToInt(cmbGdNoBreakup.Value);
				tablebreakup.Rows[updateDetailIndexBreakUp]["GDNO"] = Conversion.ToString((object)((Control)(object)cmbGdNoBreakup).Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["GdRefDocTypeId"] = Conversion.ToInt(((UltraDropDownBase)cmbGdNoBreakup).SelectedRow.Cells["DocumentTypeId"].Value);
				tablebreakup.Rows[updateDetailIndexBreakUp]["InvoiceAmount"] = Conversion.ToDouble((object)txtInvoiceAmountBreakup.Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["RealizedAmount"] = Conversion.ToDouble((object)txtRealizedAmtBreakup.Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["ForiegnAgencyCommAmount"] = Conversion.ToDouble((object)txtCommisionamtbreakUp.Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["FDBCNo"] = Conversion.ToString((object)txtFDBCNoBreakup.Text);
				tablebreakup.Rows[updateDetailIndexBreakUp]["Status"] = Conversion.ToString((object)((Control)(object)cmbStatusBreakup).Text);
				grdInvoiceWiseBreakUp.DataSource = tablebreakup;
				grdInvoiceWiseBreakUp.RetrieveStructure();
				gridBreakUpsettings();
				CommonServices.GridAutoAdjustmentNew(grdInvoiceWiseBreakUp);
				ResetBreakUp();
				AddTotalRealizedAmountInHeaderFcAmount();
				cmbaccounttype_Leave(null, null);
				cmbInvoicenobreakup.Focus();
				GdBindinExpensesInvoice();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnCancelBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			ResetBreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridBreakUpsettings()
	{
		try
		{
			grdInvoiceWiseBreakUp.RootTable.Columns["Id"].Visible = false;
			grdInvoiceWiseBreakUp.RootTable.Columns["GDId"].Visible = false;
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceId"].Visible = false;
			grdInvoiceWiseBreakUp.RootTable.Columns["GdRefDocTypeId"].Visible = false;
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceAmount"].Caption = "TotalAmount";
			grdInvoiceWiseBreakUp.RootTable.Columns["ForiegnAgencyCommAmount"].Caption = "Commission";
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns["InvoiceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdInvoiceWiseBreakUp.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdInvoiceWiseBreakUp.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns["ForiegnAgencyCommAmount"].AggregateFunction = (AggregateFunction)2;
			grdInvoiceWiseBreakUp.RootTable.Columns["ForiegnAgencyCommAmount"].TextAlignment = (TextAlignment)3;
			grdInvoiceWiseBreakUp.RootTable.Columns["ForiegnAgencyCommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns["ForiegnAgencyCommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvoiceWiseBreakUp.RootTable.Columns.Add("Delete");
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].Caption = "X";
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].Width = 20;
			grdInvoiceWiseBreakUp.RootTable.Columns["Delete"].Position = 0;
			grdInvoiceWiseBreakUp.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(grdInvoiceWiseBreakUp);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPartyBreakUpsettings()
	{
		try
		{
			grdPartyPaymentsBreackUp.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grdPartyPaymentsBreackUp.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdPartyPaymentsBreackUp.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdPartyPaymentsBreackUp.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdPartyPaymentsBreackUp.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdPartyPaymentsBreackUp.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns["FBC"].AggregateFunction = (AggregateFunction)2;
			grdPartyPaymentsBreackUp.RootTable.Columns["FBC"].TextAlignment = (TextAlignment)3;
			grdPartyPaymentsBreackUp.RootTable.Columns["FBC"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns["FBC"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyPaymentsBreackUp.RootTable.Columns.Add("Delete");
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].Key = "Delete";
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].Caption = "X";
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].ButtonText = "X";
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].Width = 20;
			grdPartyPaymentsBreackUp.RootTable.Columns["Delete"].Position = 0;
			grdPartyPaymentsBreackUp.RootTable.Columns.Add("Add");
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].Key = "Add";
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].Caption = "+";
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].ButtonText = "+";
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].Width = 20;
			grdPartyPaymentsBreackUp.RootTable.Columns["Add"].Position = 1;
			grdPartyPaymentsBreackUp.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(grdPartyPaymentsBreackUp);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPaymentTermsSettings()
	{
		try
		{
			grdPaymentTerms.RootTable.HeaderLines = 2;
			grdPaymentTerms.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["Received"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["Balance"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["PaymentTermId"].Visible = false;
			grdPaymentTerms.RootTable.Columns["InvoicePaymentDetailId"].Visible = false;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].Width = 70;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].HeaderAlignment = (TextAlignment)2;
			grdPaymentTerms.RootTable.Columns["Amount"].Width = 70;
			grdPaymentTerms.RootTable.Columns["Received"].Width = 70;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].Width = 70;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].HeaderAlignment = (TextAlignment)2;
			grdPaymentTerms.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Amount"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Received"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Received"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Received"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Received"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Balance"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Balance"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].TotalFormatString = "#,#.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationbreakUp()
	{
		try
		{
			if (((Control)(object)cmbInvoicenobreakup).Text.Trim() == string.Empty || Conversion.ToInt(cmbInvoicenobreakup.Value) == 0)
			{
				MessageBox.Show("Please Select InvoiceNo");
				cmbInvoicenobreakup.Focus();
				return false;
			}
			if (((UltraGridBase)cmbGdNoBreakup).ActiveRow == null)
			{
				MessageBox.Show("Please Select GD No");
				cmbGdNoBreakup.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtInvoiceAmountBreakup.Text) == 0.0)
			{
				MessageBox.Show("Invoice Amount required");
				txtInvoiceAmountBreakup.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtRealizedAmtBreakup.Text) == 0.0)
			{
				MessageBox.Show("Realized Amount required");
				txtRealizedAmtBreakup.Focus();
				return false;
			}
			decimal CommAmount = default(decimal);
			decimal num = Conversion.ToDecimal((object)txtInvoiceAmountBreakup.Text.Trim());
			CommAmount = Conversion.ToDecimal((object)txtCommisionamtbreakUp.Text.Trim());
			decimal TotalRealizedAmount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)txtRealizedAmtBreakup.Text.Trim())) + Conversion.ToDecimal((object)CommAmount);
			if (num != TotalRealizedAmount)
			{
				MessageBox.Show("RealizedAmount and CommissionAmount cannot be equal to TotalAmount.");
				txtInvoiceAmountBreakup.Focus();
				return false;
			}
			if (txtFDBCNoBreakup.Text.Trim() == "")
			{
				MessageBox.Show("FDBC No required");
				txtFDBCNoBreakup.Focus();
				return false;
			}
			if (((Control)(object)cmbStatusBreakup).Text.Trim() == "" || ((UltraGridBase)cmbStatusBreakup).ActiveRow == null)
			{
				MessageBox.Show("Status required");
				cmbStatusBreakup.Focus();
				return false;
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private void ResetBreakUp()
	{
		try
		{
			((Control)(object)cmbInvoicenobreakup).Text = string.Empty;
			((Control)(object)cmbGdNoBreakup).Text = string.Empty;
			txtInvoiceAmountBreakup.Text = string.Empty;
			txtRealizedAmtBreakup.Text = string.Empty;
			txtCommisionamtbreakUp.Text = string.Empty;
			txtFDBCNoBreakup.Text = string.Empty;
			((Control)(object)cmbStatusBreakup).Text = string.Empty;
			btnAddBreakup.Visible = true;
			BtnUpdatebreakup.Visible = false;
			BtnCancelBreakUp.Visible = false;
			GdBreakUpId = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdInvoiceWiseBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdInvoiceWiseBreakUp.CurrentRow;
			if (item != null)
			{
				updateDetailIndexBreakUp = item.RowIndex;
				DataTable dtInvoice = new DataTable();
				dtInvoice.Columns.Add("Id");
				dtInvoice.Columns.Add("InvoiceNo");
				dtInvoice.Columns.Add("GDNO");
				dtInvoice.Columns.Add("Status");
				dtInvoice.Columns.Add("DocumentTypeId");
				dtInvoice.Rows.Add(Conversion.ToInt(item.Cells["InvoiceId"].Value), Conversion.ToString((object)item.Cells["InvoiceNo"].Text), Conversion.ToString((object)item.Cells["GDNO"].Text), Conversion.ToString((object)item.Cells["Status"].Text), 0);
				if (dtInvoice.Rows.Count > 0)
				{
					DDL.BindDDL(dtInvoice, cmbInvoicenobreakup, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[1].Width = 200;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)cmbInvoicenobreakup).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
				cmbInvoicenobreakup.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				txtRealizedAmtBreakup.Text = Conversion.ToString(item.Cells["RealizedAmount"].Value);
				txtCommisionamtbreakUp.Text = Conversion.ToString(item.Cells["ForiegnAgencyCommAmount"].Value);
				double TotalAmount = Conversion.ToDouble(item.Cells["RealizedAmount"].Value) + Conversion.ToDouble(item.Cells["ForiegnAgencyCommAmount"].Value);
				txtInvoiceAmountBreakup.Text = TotalAmount.ToString();
				txtFDBCNoBreakup.Text = Conversion.ToString((object)item.Cells["FDBCNo"].Text);
				((Control)(object)cmbStatusBreakup).Text = Conversion.ToString((object)item.Cells["Status"].Text);
				GdBreakUpId = Conversion.ToInt(item.Cells["Id"].Value);
				cmbInvoicenobreakup.Focus();
				btnAddBreakup.Visible = false;
				BtnUpdatebreakup.Visible = true;
				BtnCancelBreakUp.Visible = true;
				grdInvoiceWiseBreakUp.DataSource = tablebreakup;
				grdInvoiceWiseBreakUp.RetrieveStructure();
				gridBreakUpsettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdInvoiceWiseBreakUp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInvoiceWiseBreakUp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					MessageBox.Show("Saved Record Can not be Deleted...", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				r.Delete();
				AddTotalRealizedAmountInHeaderFcAmount();
				CalculateBankInvoiceBreakupRemaining();
			}
			grdInvoiceWiseBreakUp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void StatusInvoiceWisebreakupBind(string LastSavedStatus)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			if (LastSavedStatus == "Final Part")
			{
				dt.Rows.Add(1, "Final Part");
			}
			else
			{
				dt.Rows.Add(1, "Final Part");
				if (string.IsNullOrEmpty(LastSavedStatus))
				{
					dt.Rows.Add(2, "1 Part");
				}
				else
				{
					int lastSavedNumber = int.Parse(LastSavedStatus.Split()[0]);
					string newStatus = lastSavedNumber + " Part";
					if (btnsave.Visible && btnsave.Enabled)
					{
						newStatus = lastSavedNumber + 1 + " Part";
					}
					dt.Rows.Add(2, newStatus);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbStatusBreakup, "Id", "Status", "Status", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbInvoicenobreakup_Leave(object sender, EventArgs e)
	{
		try
		{
			InvoiceAmountGet();
			GetExchangeRateandCurrencyFromVoucher();
			if (((UltraGridBase)cmbInvoicenobreakup).ActiveRow != null && Conversion.ToInt(cmbInvoicenobreakup.Value) > 0)
			{
				int RefDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)cmbInvoicenobreakup).SelectedRow.Cells["DocumentTypeId"].Value);
				DataTable dtGd = ExImInvoice.GetGDsAgainstAdvancePaymentUtilizeInInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbInvoicenobreakup.Value), GdBreakUpId, RefDocumentTypeId);
				if (dtGd.Rows.Count > 0)
				{
					DDL.BindDDL(dtGd, cmbGdNoBreakup, "Id", "GDNO", "GD No", ZeroIndex: false);
				}
				else
				{
					((Control)(object)cmbGdNoBreakup).Text = string.Empty;
					((UltraGridBase)cmbGdNoBreakup).DataSource = null;
				}
				StatusInvoiceWisebreakupBind(Conversion.ToString(((UltraDropDownBase)cmbInvoicenobreakup).SelectedRow.Cells[3].Value));
			}
			else
			{
				((Control)(object)cmbGdNoBreakup).Text = string.Empty;
				((UltraGridBase)cmbGdNoBreakup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRealizedAmtBreakup_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCommisionamtbreakUp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			RealizedAmountAutoCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Acfrmfcbankreceipt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbaccounttype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 4 && Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				InvoiceNoForBreakUp();
				panelInvoiceWiseBreakup.Visible = true;
			}
			else
			{
				panelInvoiceWiseBreakup.Visible = false;
				grdInvoiceWiseBreakUp.ClearStructure();
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				BankAccountDr(15);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 2)
			{
				BankAccountDr(22);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 3)
			{
				BankAccountDr(2);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 4)
			{
				BankAccountDr(11);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RealizedAmountAutoCalculate()
	{
		try
		{
			double TotalAmount = 0.0;
			double CommAmount = 0.0;
			double RealizedAmount = 0.0;
			TotalAmount = Conversion.ToDouble((object)txtInvoiceAmountBreakup.Text.Trim());
			CommAmount = Conversion.ToDouble((object)txtCommisionamtbreakUp.Text.Trim());
			if (TotalAmount > 0.0 && CommAmount > 0.0)
			{
				RealizedAmount = TotalAmount - CommAmount;
				txtRealizedAmtBreakup.Text = RealizedAmount.ToString();
			}
			else if (TotalAmount > 0.0 && CommAmount == 0.0)
			{
				txtRealizedAmtBreakup.Text = TotalAmount.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInvoiceAmountBreakup_TextChanged(object sender, EventArgs e)
	{
		RealizedAmountAutoCalculate();
	}

	private void grdPartyPaymentsBreackUp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPartyPaymentsBreackUp.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (tablePartybreakup.Rows.Count == 0)
				{
					RowAddinGrdPartyPaymentsBreakUp();
				}
			}
			if (e.Column.Key == "Add")
			{
				RowAddinGrdPartyPaymentsBreakUp();
			}
			grdPartyPaymentsBreackUp.UpdateData();
			FBCCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RowAddinGrdPartyPaymentsBreakUp()
	{
		try
		{
			tablePartybreakup.Rows.Add(0, 0, 0);
			grdPartyPaymentsBreackUp.DataSource = tablePartybreakup;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPartyPaymentsBreackUp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdPartyPaymentsBreackUp.CurrentRow;
			double FBC = Conversion.ToDouble(r.Cells["FBC"].Value);
			double RealizedAmount = Conversion.ToDouble(r.Cells["RealizedAmount"].Value);
			if (RealizedAmount > 0.0)
			{
				r.Cells["TotalAmount"].Value = RealizedAmount + FBC;
			}
			grdPartyPaymentsBreackUp.UpdateData();
			FBCCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FBCCalculations()
	{
		try
		{
			double FBC = 0.0;
			GridEXRow[] rows = grdPartyPaymentsBreackUp.GetRows();
			foreach (GridEXRow item in rows)
			{
				FBC += Conversion.ToDouble(item.Cells["FBC"].Value);
			}
			txtFBC.Text = FBC.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyPaymentPanelVisible()
	{
		try
		{
			if (RDBankSingleAndPartiesMulti.Checked || rdMultiBankAndMultiParties.Checked)
			{
				panel10.Visible = true;
			}
			else
			{
				panel10.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RDSingleDevice_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			PartyPaymentPanelVisible();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RDMultiBankDevice_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			PartyPaymentPanelVisible();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRemaingBreakupBalance_Click(object sender, EventArgs e)
	{
		try
		{
			CalculateBankInvoiceBreakupRemaining();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ThirdPartyReceiverAmountCalculations()
	{
		try
		{
			txtReceiverFcyAmount.Text = "0";
			if (Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) != 0.0 && Conversion.ToDouble((object)txtReceiverExchangeRate.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double ReciverExchRthirdParty = Conversion.ToDouble((object)txtReceiverExchangeRate.Text);
				double ThirdFcAmt = num / ReciverExchRthirdParty;
				txtReceiverFcyAmount.Text = Conversion.ToString((object)Math.Round(ThirdFcAmt, 3));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtReceiverExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ThirdPartyReceiverAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerms_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTerms.CurrentRow;
			if (!(e.Column.Key == "ThisReceipt"))
			{
				return;
			}
			if (double.TryParse(item.Cells["Balance"].Value?.ToString(), out var balance) && double.TryParse(item.Cells["ThisReceipt"].Value?.ToString(), out var thisReceipt))
			{
				if (thisReceipt > balance)
				{
					item.Cells["ThisReceipt"].Value = 0;
					MessageBox.Show($"This Receipt cannot be greater than the balance amount. Balance amount is {balance} and This Receipt is {thisReceipt}.");
				}
			}
			else
			{
				MessageBox.Show("Invalid input in This Receipt field.");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbChargesAccountType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text.Trim() == "Invoice Payment" && string.IsNullOrEmpty(txtRemarksDetail.Text.Trim()))
			{
				txtRemarksDetail.Text = Conversion.ToString((object)((Control)(object)CmbChargesAccountType).Text.Trim()) + " AGAINST INVOICE NO " + ((Control)(object)cmbInvoiceNo).Text.Trim();
				txtRemarksDetail.SelectAll();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbbankAcc_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbbankAcc).ActiveRow != null && Conversion.ToInt(cmbbankAcc.Value) > 0)
			{
				CmbDebitCurrency.Value = Conversion.ToInt(((UltraDropDownBase)cmbbankAcc).SelectedRow.Cells["CurrencyId"].Value);
			}
			else
			{
				((Control)(object)CmbDebitCurrency).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDebitExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDebitExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DebitFcyAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitFcyAmountCalculate()
	{
		try
		{
			txtDebitFcyAmount.Text = "0";
			if (Conversion.ToDouble((object)txtFCGrosamount.Text) != 0.0 && Conversion.ToDouble((object)txtDebitExchangeRate.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double DebitExchangeRate = Conversion.ToDouble((object)txtDebitExchangeRate.Text);
				double DebitFcyAmount = num * DebitExchangeRate;
				txtDebitFcyAmount.Text = Conversion.ToString((object)Math.Round(DebitFcyAmount, 4));
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
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee7: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1118: Expected O, but got Unknown
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Expected O, but got Unknown
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Expected O, but got Unknown
		//IL_1a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a23: Expected O, but got Unknown
		//IL_1e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0c: Expected O, but got Unknown
		//IL_384e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3858: Expected O, but got Unknown
		//IL_58a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ad: Expected O, but got Unknown
		//IL_58ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c4: Expected O, but got Unknown
		//IL_d11a: Unknown result type (might be due to invalid IL or missing references)
		//IL_d124: Expected O, but got Unknown
		//IL_d131: Unknown result type (might be due to invalid IL or missing references)
		//IL_d13b: Expected O, but got Unknown
		GridEXLayout RemainingBalanceGrid_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFCBank_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTerms_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
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
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		GridEXLayout grdInvoiceWiseBreakUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
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
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		GridEXLayout grdPartyPaymentsBreackUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance211 = new Appearance();
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
		Appearance appearance225 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
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
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
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
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
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
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt_DebitCurrency));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
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
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance111 = new Appearance();
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
		Appearance appearance125 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.RemainingBalanceOfBankDeviceBreakUp = new System.Windows.Forms.Panel();
		this.RemainingBalanceGrid = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdFCBank = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.grdPaymentTerms = new GridEX();
		this.btnRemaingBreakupBalance = new System.Windows.Forms.Button();
		this.label44 = new System.Windows.Forms.Label();
		this.txtChargePercent = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtfcamount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtpkramount = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbChargesAccountType = new UltraCombo();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.CmbChargesFDBCNo = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.CmbChargesType = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.panelInvoiceWiseBreakup = new System.Windows.Forms.Panel();
		this.grdInvoiceWiseBreakUp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtCommisionamtbreakUp = new System.Windows.Forms.TextBox();
		this.label66 = new System.Windows.Forms.Label();
		this.cmbStatusBreakup = new UltraCombo();
		this.label58 = new System.Windows.Forms.Label();
		this.txtInvoiceAmountBreakup = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtRealizedAmtBreakup = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.txtFDBCNoBreakup = new System.Windows.Forms.TextBox();
		this.cmbGdNoBreakup = new UltraCombo();
		this.btnAddBreakup = new System.Windows.Forms.Button();
		this.BtnUpdatebreakup = new System.Windows.Forms.Button();
		this.BtnCancelBreakUp = new System.Windows.Forms.Button();
		this.cmbInvoicenobreakup = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdPartyPaymentsBreackUp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.rdMultiBankAndMultiParties = new System.Windows.Forms.RadioButton();
		this.RDMultiBankDevice = new System.Windows.Forms.RadioButton();
		this.RDBankSingleAndPartiesMulti = new System.Windows.Forms.RadioButton();
		this.label68 = new System.Windows.Forms.Label();
		this.txtFBC = new System.Windows.Forms.TextBox();
		this.cmbReferenceNo = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtDebitExchangeRate = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.CmbDebitCurrency = new UltraCombo();
		this.txtDebitFcyAmount = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtfcyAmountThirdParty = new System.Windows.Forms.TextBox();
		this.txtReceiverFcyAmount = new System.Windows.Forms.TextBox();
		this.txtHcyExchRthirdParty = new System.Windows.Forms.TextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbFcycodeThirdParty = new UltraCombo();
		this.txtReceiverExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFcyExchRthirdParty = new System.Windows.Forms.TextBox();
		this.txtbankDrAmount = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtNetAmountFC = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtInvoiceAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtExportvoucherRate = new System.Windows.Forms.TextBox();
		this.txtRecAmount = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtBalanceAmount = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbaccounttype = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbConsignee = new UltraCombo();
		this.lblConsignee = new System.Windows.Forms.Label();
		this.CmbFBCDebitAccount = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.CmbGainLossAc = new UltraCombo();
		this.txtForeinchargesPkr = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.txtLocalAmount = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.comSuplier = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtFINo = new System.Windows.Forms.TextBox();
		this.labFiNo = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbPaymentTerm = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.cmbranches = new UltraCombo();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.cmbproject = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtexchangeRate = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtFCGrosamount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtbankfbpNo = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbbankAcc = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbLCOrder = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocNumber = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.chkpreview = new System.Windows.Forms.CheckBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnAttachments = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadInvoice = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label70 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label67 = new System.Windows.Forms.Label();
		this.CmbReceiverAcHistory = new UltraCombo();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label75 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label38 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.RemainingBalanceOfBankDeviceBreakUp.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.RemainingBalanceGrid).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesFDBCNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).BeginInit();
		this.panelInvoiceWiseBreakup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvoiceWiseBreakUp).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatusBreakup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGdNoBreakup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoicenobreakup).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPartyPaymentsBreackUp).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceNo).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFcycodeThirdParty).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFBCDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.btnAttachments.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1600, 752);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1600, 752);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1592, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panelInvoiceWiseBreakup);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1586, 717);
		this.panel2.TabIndex = 1;
		this.panel4.Controls.Add(this.RemainingBalanceOfBankDeviceBreakUp);
		this.panel4.Controls.Add(this.panel14);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(542, 386);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1044, 331);
		this.panel4.TabIndex = 5;
		this.RemainingBalanceOfBankDeviceBreakUp.Controls.Add((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid);
		this.RemainingBalanceOfBankDeviceBreakUp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.RemainingBalanceOfBankDeviceBreakUp.Location = new System.Drawing.Point(612, 118);
		this.RemainingBalanceOfBankDeviceBreakUp.Name = "RemainingBalanceOfBankDeviceBreakUp";
		this.RemainingBalanceOfBankDeviceBreakUp.Size = new System.Drawing.Size(432, 213);
		this.RemainingBalanceOfBankDeviceBreakUp.TabIndex = 2;
		this.RemainingBalanceGrid.AllowEdit = (InheritableBoolean)2;
		this.RemainingBalanceGrid.DefaultFilterRowComparison = (FilterConditionOperator)7;
		RemainingBalanceGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.RemainingBalanceGrid.DesignTimeLayout = RemainingBalanceGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.RemainingBalanceGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).Name = "RemainingBalanceGrid";
		((ScrollableBase)this.RemainingBalanceGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).Size = new System.Drawing.Size(432, 213);
		((System.Windows.Forms.Control)(object)this.RemainingBalanceGrid).TabIndex = 1;
		this.RemainingBalanceGrid.TotalRow = (InheritableBoolean)1;
		this.RemainingBalanceGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.RemainingBalanceGrid).VisualStyle = (VisualStyle)4;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdFCBank);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(0, 118);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(612, 213);
		this.panel14.TabIndex = 1;
		grdFCBank_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFCBank.DesignTimeLayout = grdFCBank_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdFCBank).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFCBank).Name = "grdFCBank";
		((ScrollableBase)this.grdFCBank).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Size = new System.Drawing.Size(612, 213);
		((System.Windows.Forms.Control)(object)this.grdFCBank).TabIndex = 0;
		this.grdFCBank.TotalRow = (InheritableBoolean)1;
		this.grdFCBank.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFCBank).VisualStyle = (VisualStyle)4;
		this.grdFCBank.ColumnButtonClick += new ColumnActionEventHandler(grdFCBank_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFCBank).DoubleClick += new System.EventHandler(grdFCBank_DoubleClick);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1044, 118);
		this.panel5.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerms);
		this.groupBox2.Controls.Add(this.btnRemaingBreakupBalance);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtChargePercent);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtfcamount);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtpkramount);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesAccountType);
		this.groupBox2.Controls.Add(this.btnAddinGrid);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.button1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesType);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1044, 118);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Export Charges";
		grdPaymentTerms_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerms.DesignTimeLayout = grdPaymentTerms_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerms.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Location = new System.Drawing.Point(618, 6);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Name = "grdPaymentTerms";
		((ScrollableBase)this.grdPaymentTerms).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Size = new System.Drawing.Size(401, 112);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).TabIndex = 12434;
		this.grdPaymentTerms.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerms.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerms).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerms.CellUpdated += new ColumnActionEventHandler(grdPaymentTerms_CellUpdated);
		this.btnRemaingBreakupBalance.BackColor = System.Drawing.Color.Teal;
		this.btnRemaingBreakupBalance.FlatAppearance.BorderSize = 0;
		this.btnRemaingBreakupBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRemaingBreakupBalance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRemaingBreakupBalance.ForeColor = System.Drawing.Color.White;
		this.btnRemaingBreakupBalance.Location = new System.Drawing.Point(528, 54);
		this.btnRemaingBreakupBalance.Name = "btnRemaingBreakupBalance";
		this.btnRemaingBreakupBalance.Size = new System.Drawing.Size(84, 41);
		this.btnRemaingBreakupBalance.TabIndex = 130;
		this.btnRemaingBreakupBalance.TabStop = false;
		this.btnRemaingBreakupBalance.Text = "Generate Remaining";
		this.btnRemaingBreakupBalance.UseVisualStyleBackColor = false;
		this.btnRemaingBreakupBalance.Click += new System.EventHandler(btnRemaingBreakupBalance_Click);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(367, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 129;
		this.label44.Text = "Prcnt";
		this.txtChargePercent.BackColor = System.Drawing.Color.White;
		this.txtChargePercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChargePercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChargePercent.Location = new System.Drawing.Point(363, 41);
		this.txtChargePercent.Name = "txtChargePercent";
		this.txtChargePercent.Size = new System.Drawing.Size(49, 23);
		this.txtChargePercent.TabIndex = 3;
		this.txtChargePercent.Tag = "Percent";
		this.txtChargePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtChargePercent.TextChanged += new System.EventHandler(txtChargePercent_TextChanged);
		this.txtChargePercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtChargePercent_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(94, 21);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(77, 15);
		this.label48.TabIndex = 126;
		this.label48.Text = "Charges Type";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(481, 21);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(12, 15);
		this.label32.TabIndex = 31;
		this.label32.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(280, 21);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(12, 15);
		this.label34.TabIndex = 34;
		this.label34.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(191, 21);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(78, 15);
		this.label35.TabIndex = 33;
		this.label35.Text = "Account Title";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(3, 21);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(46, 15);
		this.label61.TabIndex = 126;
		this.label61.Text = "FDBS #";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(414, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(67, 15);
		this.label33.TabIndex = 30;
		this.label33.Text = "FC Amount";
		this.txtfcamount.BackColor = System.Drawing.Color.White;
		this.txtfcamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcamount.Location = new System.Drawing.Point(415, 41);
		this.txtfcamount.Name = "txtfcamount";
		this.txtfcamount.Size = new System.Drawing.Size(106, 23);
		this.txtfcamount.TabIndex = 4;
		this.txtfcamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcamount.TextChanged += new System.EventHandler(txtfcamount_TextChanged);
		this.txtfcamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcamount_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 68);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(66, 15);
		this.label37.TabIndex = 36;
		this.label37.Text = "Rs Amount";
		this.txtpkramount.BackColor = System.Drawing.Color.White;
		this.txtpkramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpkramount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpkramount.Location = new System.Drawing.Point(3, 85);
		this.txtpkramount.Name = "txtpkramount";
		this.txtpkramount.Size = new System.Drawing.Size(116, 23);
		this.txtpkramount.TabIndex = 5;
		this.txtpkramount.Tag = "PkrAmount";
		this.txtpkramount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtpkramount.TextChanged += new System.EventHandler(txtpkramount_TextChanged);
		this.txtpkramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtpkramount_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(76, 68);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(12, 15);
		this.label36.TabIndex = 37;
		this.label36.Text = "*";
		this.CmbChargesAccountType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesAccountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesAccountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesAccountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesAccountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Location = new System.Drawing.Point(184, 39);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Name = "CmbChargesAccountType";
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Leave += new System.EventHandler(CmbChargesAccountType_Leave);
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(363, 84);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 25);
		this.btnAddinGrid.TabIndex = 8;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(390, 85);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 23);
		this.btnUpdateDetail.TabIndex = 9;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(457, 85);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 23);
		this.btnCancelUpdateDetial.TabIndex = 10;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtRemarksDetail.BackColor = System.Drawing.Color.White;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(124, 85);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(238, 23);
		this.txtRemarksDetail.TabIndex = 6;
		this.CmbChargesFDBCNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesFDBCNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesFDBCNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesFDBCNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesFDBCNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesFDBCNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).Location = new System.Drawing.Point(3, 39);
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).Name = "CmbChargesFDBCNo";
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbChargesFDBCNo).Leave += new System.EventHandler(cmbInvoicenobreakup_Leave);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(121, 68);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 123;
		this.label47.Text = "Remarks";
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(335, 17);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(27, 23);
		this.button1.TabIndex = 2;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.CmbChargesType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Location = new System.Drawing.Point(103, 39);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Name = "CmbChargesType";
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Leave += new System.EventHandler(CmbChargesType_Leave);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(175, 21);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(12, 15);
		this.label49.TabIndex = 127;
		this.label49.Text = "*";
		this.panelInvoiceWiseBreakup.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp);
		this.panelInvoiceWiseBreakup.Controls.Add(this.panel11);
		this.panelInvoiceWiseBreakup.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelInvoiceWiseBreakup.Location = new System.Drawing.Point(0, 386);
		this.panelInvoiceWiseBreakup.Name = "panelInvoiceWiseBreakup";
		this.panelInvoiceWiseBreakup.Size = new System.Drawing.Size(542, 331);
		this.panelInvoiceWiseBreakup.TabIndex = 4;
		this.grdInvoiceWiseBreakUp.AllowEdit = (InheritableBoolean)2;
		grdInvoiceWiseBreakUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvoiceWiseBreakUp.DesignTimeLayout = grdInvoiceWiseBreakUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvoiceWiseBreakUp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).Location = new System.Drawing.Point(0, 118);
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).Name = "grdInvoiceWiseBreakUp";
		((ScrollableBase)this.grdInvoiceWiseBreakUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).Size = new System.Drawing.Size(542, 213);
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).TabIndex = 0;
		this.grdInvoiceWiseBreakUp.TotalRow = (InheritableBoolean)1;
		this.grdInvoiceWiseBreakUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvoiceWiseBreakUp).VisualStyle = (VisualStyle)4;
		this.grdInvoiceWiseBreakUp.ColumnButtonClick += new ColumnActionEventHandler(grdInvoiceWiseBreakUp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvoiceWiseBreakUp).DoubleClick += new System.EventHandler(grdInvoiceWiseBreakUp_DoubleClick);
		this.panel11.Controls.Add(this.groupBox5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(542, 118);
		this.panel11.TabIndex = 0;
		this.groupBox5.Controls.Add(this.label60);
		this.groupBox5.Controls.Add(this.txtCommisionamtbreakUp);
		this.groupBox5.Controls.Add(this.label66);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatusBreakup);
		this.groupBox5.Controls.Add(this.label58);
		this.groupBox5.Controls.Add(this.txtInvoiceAmountBreakup);
		this.groupBox5.Controls.Add(this.label59);
		this.groupBox5.Controls.Add(this.label62);
		this.groupBox5.Controls.Add(this.label63);
		this.groupBox5.Controls.Add(this.txtRealizedAmtBreakup);
		this.groupBox5.Controls.Add(this.label64);
		this.groupBox5.Controls.Add(this.txtFDBCNoBreakup);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup);
		this.groupBox5.Controls.Add(this.btnAddBreakup);
		this.groupBox5.Controls.Add(this.BtnUpdatebreakup);
		this.groupBox5.Controls.Add(this.BtnCancelBreakUp);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup);
		this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(0, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(542, 118);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Invoice Wise Breakup";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(255, 67);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(40, 15);
		this.label60.TabIndex = 131;
		this.label60.Text = "Status";
		this.txtCommisionamtbreakUp.BackColor = System.Drawing.Color.White;
		this.txtCommisionamtbreakUp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommisionamtbreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommisionamtbreakUp.Location = new System.Drawing.Point(322, 41);
		this.txtCommisionamtbreakUp.Name = "txtCommisionamtbreakUp";
		this.txtCommisionamtbreakUp.Size = new System.Drawing.Size(105, 23);
		this.txtCommisionamtbreakUp.TabIndex = 3;
		this.txtCommisionamtbreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommisionamtbreakUp.TextChanged += new System.EventHandler(txtCommisionamtbreakUp_TextChanged);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(322, 22);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(73, 15);
		this.label66.TabIndex = 123;
		this.label66.Text = "Commission";
		this.cmbStatusBreakup.AllowNull = (DefaultableBoolean)1;
		this.cmbStatusBreakup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatusBreakup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatusBreakup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatusBreakup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatusBreakup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatusBreakup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatusBreakup).Location = new System.Drawing.Point(256, 83);
		((System.Windows.Forms.Control)(object)this.cmbStatusBreakup).Name = "cmbStatusBreakup";
		((System.Windows.Forms.Control)(object)this.cmbStatusBreakup).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatusBreakup).TabIndex = 6;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(223, 22);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(80, 15);
		this.label58.TabIndex = 129;
		this.label58.Text = "Total Amount";
		this.txtInvoiceAmountBreakup.BackColor = System.Drawing.Color.White;
		this.txtInvoiceAmountBreakup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceAmountBreakup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmountBreakup.Location = new System.Drawing.Point(223, 41);
		this.txtInvoiceAmountBreakup.Name = "txtInvoiceAmountBreakup";
		this.txtInvoiceAmountBreakup.Size = new System.Drawing.Size(98, 23);
		this.txtInvoiceAmountBreakup.TabIndex = 2;
		this.txtInvoiceAmountBreakup.Tag = "Percent";
		this.txtInvoiceAmountBreakup.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInvoiceAmountBreakup.TextChanged += new System.EventHandler(txtInvoiceAmountBreakup_TextChanged);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(4, 22);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(65, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Invoice No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(111, 22);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(34, 15);
		this.label62.TabIndex = 33;
		this.label62.Text = "GD #";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(426, 22);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(98, 15);
		this.label63.TabIndex = 30;
		this.label63.Text = "Realized Amount";
		this.txtRealizedAmtBreakup.BackColor = System.Drawing.Color.White;
		this.txtRealizedAmtBreakup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRealizedAmtBreakup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRealizedAmtBreakup.Location = new System.Drawing.Point(429, 41);
		this.txtRealizedAmtBreakup.Name = "txtRealizedAmtBreakup";
		this.txtRealizedAmtBreakup.Size = new System.Drawing.Size(108, 23);
		this.txtRealizedAmtBreakup.TabIndex = 4;
		this.txtRealizedAmtBreakup.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRealizedAmtBreakup.TextChanged += new System.EventHandler(txtRealizedAmtBreakup_TextChanged);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(4, 68);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(46, 15);
		this.label64.TabIndex = 36;
		this.label64.Text = "FDBC #";
		this.txtFDBCNoBreakup.BackColor = System.Drawing.Color.White;
		this.txtFDBCNoBreakup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFDBCNoBreakup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFDBCNoBreakup.Location = new System.Drawing.Point(4, 85);
		this.txtFDBCNoBreakup.Name = "txtFDBCNoBreakup";
		this.txtFDBCNoBreakup.Size = new System.Drawing.Size(249, 23);
		this.txtFDBCNoBreakup.TabIndex = 5;
		this.txtFDBCNoBreakup.Tag = "PkrAmount";
		this.cmbGdNoBreakup.AllowNull = (DefaultableBoolean)1;
		this.cmbGdNoBreakup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGdNoBreakup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGdNoBreakup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGdNoBreakup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGdNoBreakup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup).Location = new System.Drawing.Point(107, 39);
		((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup).Name = "cmbGdNoBreakup";
		((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.cmbGdNoBreakup).TabIndex = 1;
		this.btnAddBreakup.BackColor = System.Drawing.Color.Teal;
		this.btnAddBreakup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddBreakup.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddBreakup.ForeColor = System.Drawing.Color.White;
		this.btnAddBreakup.Location = new System.Drawing.Point(378, 84);
		this.btnAddBreakup.Name = "btnAddBreakup";
		this.btnAddBreakup.Size = new System.Drawing.Size(27, 25);
		this.btnAddBreakup.TabIndex = 7;
		this.btnAddBreakup.Text = "+";
		this.btnAddBreakup.UseVisualStyleBackColor = false;
		this.btnAddBreakup.Click += new System.EventHandler(btnAddBreakup_Click);
		this.BtnUpdatebreakup.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatebreakup.FlatAppearance.BorderSize = 0;
		this.BtnUpdatebreakup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatebreakup.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdatebreakup.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatebreakup.Location = new System.Drawing.Point(405, 85);
		this.BtnUpdatebreakup.Name = "BtnUpdatebreakup";
		this.BtnUpdatebreakup.Size = new System.Drawing.Size(65, 23);
		this.BtnUpdatebreakup.TabIndex = 8;
		this.BtnUpdatebreakup.Text = "Update";
		this.BtnUpdatebreakup.UseVisualStyleBackColor = false;
		this.BtnUpdatebreakup.Visible = false;
		this.BtnUpdatebreakup.Click += new System.EventHandler(BtnUpdatebreakup_Click);
		this.BtnCancelBreakUp.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelBreakUp.FlatAppearance.BorderSize = 0;
		this.BtnCancelBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelBreakUp.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelBreakUp.ForeColor = System.Drawing.Color.White;
		this.BtnCancelBreakUp.Location = new System.Drawing.Point(472, 85);
		this.BtnCancelBreakUp.Name = "BtnCancelBreakUp";
		this.BtnCancelBreakUp.Size = new System.Drawing.Size(65, 23);
		this.BtnCancelBreakUp.TabIndex = 9;
		this.BtnCancelBreakUp.Text = "Cancel";
		this.BtnCancelBreakUp.UseVisualStyleBackColor = false;
		this.BtnCancelBreakUp.Visible = false;
		this.BtnCancelBreakUp.Click += new System.EventHandler(BtnCancelBreakUp_Click);
		this.cmbInvoicenobreakup.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoicenobreakup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoicenobreakup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoicenobreakup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoicenobreakup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoicenobreakup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).Location = new System.Drawing.Point(4, 39);
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).Name = "cmbInvoicenobreakup";
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbInvoicenobreakup).Leave += new System.EventHandler(cmbInvoicenobreakup_Leave);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 56);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1586, 330);
		this.panel3.TabIndex = 0;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.panel10);
		this.groupBox1.Controls.Add(this.rdMultiBankAndMultiParties);
		this.groupBox1.Controls.Add(this.RDMultiBankDevice);
		this.groupBox1.Controls.Add(this.RDBankSingleAndPartiesMulti);
		this.groupBox1.Controls.Add(this.label68);
		this.groupBox1.Controls.Add(this.txtFBC);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceNo);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.groupBox3);
		this.groupBox1.Controls.Add(this.txtbankDrAmount);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtNetAmountFC);
		this.groupBox1.Controls.Add(this.groupBox4);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbaccounttype);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbConsignee);
		this.groupBox1.Controls.Add(this.lblConsignee);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount);
		this.groupBox1.Controls.Add(this.label69);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGainLossAc);
		this.groupBox1.Controls.Add(this.txtForeinchargesPkr);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.txtLocalAmount);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.txtNetAmount);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comSuplier);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtFINo);
		this.groupBox1.Controls.Add(this.labFiNo);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbranches);
		this.groupBox1.Controls.Add(this.txtdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtexchangeRate);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtFCGrosamount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtbankfbpNo);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbankAcc);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLCOrder);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtdocNumber);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1352, 326);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Fcy Receipt";
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Location = new System.Drawing.Point(997, 92);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(345, 219);
		this.panel10.TabIndex = 12436;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 32);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(345, 187);
		this.panel13.TabIndex = 12434;
		grdPartyPaymentsBreackUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPartyPaymentsBreackUp.DesignTimeLayout = grdPartyPaymentsBreackUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPartyPaymentsBreackUp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).Name = "grdPartyPaymentsBreackUp";
		((ScrollableBase)this.grdPartyPaymentsBreackUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).Size = new System.Drawing.Size(345, 187);
		((System.Windows.Forms.Control)(object)this.grdPartyPaymentsBreackUp).TabIndex = 12433;
		this.grdPartyPaymentsBreackUp.TotalRow = (InheritableBoolean)1;
		this.grdPartyPaymentsBreackUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPartyPaymentsBreackUp).VisualStyle = (VisualStyle)4;
		this.grdPartyPaymentsBreackUp.CellUpdated += new ColumnActionEventHandler(grdPartyPaymentsBreackUp_CellUpdated);
		this.grdPartyPaymentsBreackUp.ColumnButtonClick += new ColumnActionEventHandler(grdPartyPaymentsBreackUp_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label65);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(345, 32);
		this.panel12.TabIndex = 0;
		this.label65.AutoSize = true;
		this.label65.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label65.Location = new System.Drawing.Point(6, 6);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(213, 19);
		this.label65.TabIndex = 1;
		this.label65.Text = "Party Payments BreakUp";
		this.rdMultiBankAndMultiParties.AutoSize = true;
		this.rdMultiBankAndMultiParties.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rdMultiBankAndMultiParties.Location = new System.Drawing.Point(990, 67);
		this.rdMultiBankAndMultiParties.Name = "rdMultiBankAndMultiParties";
		this.rdMultiBankAndMultiParties.Size = new System.Drawing.Size(345, 21);
		this.rdMultiBankAndMultiParties.TabIndex = 12432;
		this.rdMultiBankAndMultiParties.Text = "BANK MULTI TIME DEBIT BUT PARTY ONE TIME CREDIT";
		this.rdMultiBankAndMultiParties.UseVisualStyleBackColor = true;
		this.RDMultiBankDevice.AutoSize = true;
		this.RDMultiBankDevice.Checked = true;
		this.RDMultiBankDevice.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RDMultiBankDevice.Location = new System.Drawing.Point(990, 41);
		this.RDMultiBankDevice.Name = "RDMultiBankDevice";
		this.RDMultiBankDevice.Size = new System.Drawing.Size(345, 21);
		this.RDMultiBankDevice.TabIndex = 12432;
		this.RDMultiBankDevice.TabStop = true;
		this.RDMultiBankDevice.Text = "BANK MULTI TIME DEBIT BUT PARTY ONE TIME CREDIT";
		this.RDMultiBankDevice.UseVisualStyleBackColor = true;
		this.RDMultiBankDevice.CheckedChanged += new System.EventHandler(RDMultiBankDevice_CheckedChanged);
		this.RDBankSingleAndPartiesMulti.AutoSize = true;
		this.RDBankSingleAndPartiesMulti.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RDBankSingleAndPartiesMulti.Location = new System.Drawing.Point(990, 14);
		this.RDBankSingleAndPartiesMulti.Name = "RDBankSingleAndPartiesMulti";
		this.RDBankSingleAndPartiesMulti.Size = new System.Drawing.Size(355, 21);
		this.RDBankSingleAndPartiesMulti.TabIndex = 12432;
		this.RDBankSingleAndPartiesMulti.Text = "BANK ONE TIME DEBIT BUT PARTIES MULTI TIME CREDIT";
		this.RDBankSingleAndPartiesMulti.UseVisualStyleBackColor = true;
		this.RDBankSingleAndPartiesMulti.CheckedChanged += new System.EventHandler(RDSingleDevice_CheckedChanged);
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(5, 241);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(92, 15);
		this.label68.TabIndex = 12431;
		this.label68.Text = "Foreign Charges";
		this.txtFBC.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFBC.Location = new System.Drawing.Point(125, 236);
		this.txtFBC.Name = "txtFBC";
		this.txtFBC.Size = new System.Drawing.Size(234, 25);
		this.txtFBC.TabIndex = 12430;
		this.txtFBC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFBC_KeyPress);
		this.cmbReferenceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance287;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance293).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Location = new System.Drawing.Point(125, 182);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Name = "cmbReferenceNo";
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).TabIndex = 7;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(5, 188);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(72, 15);
		this.label43.TabIndex = 12428;
		this.label43.Text = "Reference # ";
		this.groupBox3.Controls.Add(this.txtDebitExchangeRate);
		this.groupBox3.Controls.Add(this.label78);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitCurrency);
		this.groupBox3.Controls.Add(this.txtDebitFcyAmount);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.txtfcyAmountThirdParty);
		this.groupBox3.Controls.Add(this.txtReceiverFcyAmount);
		this.groupBox3.Controls.Add(this.txtHcyExchRthirdParty);
		this.groupBox3.Controls.Add(this.label72);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label71);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty);
		this.groupBox3.Controls.Add(this.txtReceiverExchangeRate);
		this.groupBox3.Controls.Add(this.txtFcyExchRthirdParty);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(689, 117);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(293, 157);
		this.groupBox3.TabIndex = 19;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Third Currency Info";
		this.txtDebitExchangeRate.BackColor = System.Drawing.Color.White;
		this.txtDebitExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDebitExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitExchangeRate.Location = new System.Drawing.Point(163, 101);
		this.txtDebitExchangeRate.Name = "txtDebitExchangeRate";
		this.txtDebitExchangeRate.Size = new System.Drawing.Size(126, 23);
		this.txtDebitExchangeRate.TabIndex = 7;
		this.txtDebitExchangeRate.TextChanged += new System.EventHandler(txtDebitExchangeRate_TextChanged);
		this.txtDebitExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDebitExchangeRate_KeyPress);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(1, 97);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(72, 30);
		this.label78.TabIndex = 12439;
		this.label78.Text = "Debit Fcy\r\nCode && Rate";
		this.CmbDebitCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance261;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance269).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).Location = new System.Drawing.Point(76, 99);
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).Name = "CmbDebitCurrency";
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitCurrency).TabIndex = 6;
		this.txtDebitFcyAmount.BackColor = System.Drawing.Color.White;
		this.txtDebitFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDebitFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitFcyAmount.Location = new System.Drawing.Point(76, 128);
		this.txtDebitFcyAmount.Name = "txtDebitFcyAmount";
		this.txtDebitFcyAmount.ReadOnly = true;
		this.txtDebitFcyAmount.Size = new System.Drawing.Size(213, 23);
		this.txtDebitFcyAmount.TabIndex = 8;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(2, 126);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(54, 26);
		this.label27.TabIndex = 12436;
		this.label27.Text = "Debit Fcy\r\nAmount";
		this.txtfcyAmountThirdParty.BackColor = System.Drawing.Color.White;
		this.txtfcyAmountThirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmountThirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcyAmountThirdParty.Location = new System.Drawing.Point(163, 22);
		this.txtfcyAmountThirdParty.Name = "txtfcyAmountThirdParty";
		this.txtfcyAmountThirdParty.Size = new System.Drawing.Size(126, 23);
		this.txtfcyAmountThirdParty.TabIndex = 1;
		this.txtfcyAmountThirdParty.TextChanged += new System.EventHandler(txtfcyAmountThirdParty_TextChanged);
		this.txtfcyAmountThirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcyAmountThirdParty_KeyPress);
		this.txtReceiverFcyAmount.BackColor = System.Drawing.Color.White;
		this.txtReceiverFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceiverFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceiverFcyAmount.Location = new System.Drawing.Point(201, 73);
		this.txtReceiverFcyAmount.Name = "txtReceiverFcyAmount";
		this.txtReceiverFcyAmount.ReadOnly = true;
		this.txtReceiverFcyAmount.Size = new System.Drawing.Size(88, 23);
		this.txtReceiverFcyAmount.TabIndex = 5;
		this.txtReceiverFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHcyExchRthirdParty_KeyPress);
		this.txtHcyExchRthirdParty.BackColor = System.Drawing.Color.White;
		this.txtHcyExchRthirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHcyExchRthirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHcyExchRthirdParty.Location = new System.Drawing.Point(201, 47);
		this.txtHcyExchRthirdParty.Name = "txtHcyExchRthirdParty";
		this.txtHcyExchRthirdParty.Size = new System.Drawing.Size(88, 23);
		this.txtHcyExchRthirdParty.TabIndex = 3;
		this.txtHcyExchRthirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHcyExchRthirdParty_KeyPress);
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(125, 78);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(67, 13);
		this.label72.TabIndex = 12432;
		this.label72.Text = "Fcy Amount";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(125, 52);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(65, 13);
		this.label21.TabIndex = 12432;
		this.label21.Text = "Hcy Ex.Rate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(70, 30);
		this.label16.TabIndex = 12430;
		this.label16.Text = "Fcy Code && \r\nAmount";
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(1, 78);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(65, 13);
		this.label71.TabIndex = 12428;
		this.label71.Text = "Rec Ex.Rate";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 13);
		this.label2.TabIndex = 12428;
		this.label2.Text = "Cust Ex.Rate";
		this.cmbFcycodeThirdParty.AllowNull = (DefaultableBoolean)1;
		this.cmbFcycodeThirdParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFcycodeThirdParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFcycodeThirdParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFcycodeThirdParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Location = new System.Drawing.Point(76, 20);
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Name = "cmbFcycodeThirdParty";
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).TabIndex = 0;
		this.txtReceiverExchangeRate.BackColor = System.Drawing.Color.White;
		this.txtReceiverExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceiverExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceiverExchangeRate.Location = new System.Drawing.Point(76, 73);
		this.txtReceiverExchangeRate.Name = "txtReceiverExchangeRate";
		this.txtReceiverExchangeRate.Size = new System.Drawing.Size(46, 23);
		this.txtReceiverExchangeRate.TabIndex = 4;
		this.txtReceiverExchangeRate.TextChanged += new System.EventHandler(txtReceiverExchangeRate_TextChanged);
		this.txtReceiverExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyExchRthirdParty_KeyPress);
		this.txtFcyExchRthirdParty.BackColor = System.Drawing.Color.White;
		this.txtFcyExchRthirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyExchRthirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyExchRthirdParty.Location = new System.Drawing.Point(76, 47);
		this.txtFcyExchRthirdParty.Name = "txtFcyExchRthirdParty";
		this.txtFcyExchRthirdParty.Size = new System.Drawing.Size(46, 23);
		this.txtFcyExchRthirdParty.TabIndex = 2;
		this.txtFcyExchRthirdParty.TextChanged += new System.EventHandler(txtFcyExchRthirdParty_TextChanged);
		this.txtFcyExchRthirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyExchRthirdParty_KeyPress);
		this.txtbankDrAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtbankDrAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankDrAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankDrAmount.Location = new System.Drawing.Point(487, 156);
		this.txtbankDrAmount.Name = "txtbankDrAmount";
		this.txtbankDrAmount.ReadOnly = true;
		this.txtbankDrAmount.Size = new System.Drawing.Size(193, 23);
		this.txtbankDrAmount.TabIndex = 14;
		this.txtbankDrAmount.TabStop = false;
		this.txtbankDrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(364, 133);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(80, 15);
		this.label17.TabIndex = 39;
		this.label17.Text = "Amount(PKR)";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(364, 160);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(96, 15);
		this.label12.TabIndex = 42;
		this.label12.Text = "Bank Dr Amount";
		this.txtNetAmountFC.BackColor = System.Drawing.SystemColors.Menu;
		this.txtNetAmountFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmountFC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountFC.Location = new System.Drawing.Point(487, 183);
		this.txtNetAmountFC.Name = "txtNetAmountFC";
		this.txtNetAmountFC.ReadOnly = true;
		this.txtNetAmountFC.Size = new System.Drawing.Size(193, 23);
		this.txtNetAmountFC.TabIndex = 15;
		this.txtNetAmountFC.TabStop = false;
		this.txtNetAmountFC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox4.Controls.Add(this.label52);
		this.groupBox4.Controls.Add(this.txtInvoiceAmount);
		this.groupBox4.Controls.Add(this.label50);
		this.groupBox4.Controls.Add(this.txtExportvoucherRate);
		this.groupBox4.Controls.Add(this.txtRecAmount);
		this.groupBox4.Controls.Add(this.label53);
		this.groupBox4.Controls.Add(this.txtBalanceAmount);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(689, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(293, 116);
		this.groupBox4.TabIndex = 18;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Invoice Info";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(6, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(126, 15);
		this.label52.TabIndex = 134;
		this.label52.Text = "Invoice Exchange Rate";
		this.txtInvoiceAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtInvoiceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmount.Location = new System.Drawing.Point(132, 39);
		this.txtInvoiceAmount.Name = "txtInvoiceAmount";
		this.txtInvoiceAmount.ReadOnly = true;
		this.txtInvoiceAmount.Size = new System.Drawing.Size(157, 23);
		this.txtInvoiceAmount.TabIndex = 1800;
		this.txtInvoiceAmount.TabStop = false;
		this.txtInvoiceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(6, 43);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 129;
		this.label50.Text = "Invoice Amount";
		this.txtExportvoucherRate.BackColor = System.Drawing.SystemColors.Menu;
		this.txtExportvoucherRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExportvoucherRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportvoucherRate.Location = new System.Drawing.Point(132, 14);
		this.txtExportvoucherRate.Name = "txtExportvoucherRate";
		this.txtExportvoucherRate.ReadOnly = true;
		this.txtExportvoucherRate.Size = new System.Drawing.Size(157, 23);
		this.txtExportvoucherRate.TabIndex = 1600;
		this.txtExportvoucherRate.TabStop = false;
		this.txtExportvoucherRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRecAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtRecAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRecAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRecAmount.Location = new System.Drawing.Point(132, 64);
		this.txtRecAmount.Name = "txtRecAmount";
		this.txtRecAmount.ReadOnly = true;
		this.txtRecAmount.Size = new System.Drawing.Size(157, 23);
		this.txtRecAmount.TabIndex = 1900;
		this.txtRecAmount.TabStop = false;
		this.txtRecAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(6, 68);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(101, 15);
		this.label53.TabIndex = 136;
		this.label53.Text = "Received Amount";
		this.txtBalanceAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalanceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceAmount.Location = new System.Drawing.Point(132, 89);
		this.txtBalanceAmount.Name = "txtBalanceAmount";
		this.txtBalanceAmount.ReadOnly = true;
		this.txtBalanceAmount.Size = new System.Drawing.Size(157, 23);
		this.txtBalanceAmount.TabIndex = 2000;
		this.txtBalanceAmount.TabStop = false;
		this.txtBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 93);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(116, 15);
		this.label54.TabIndex = 138;
		this.label54.Text = "Invoice Balance Amt";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(364, 187);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(96, 15);
		this.label13.TabIndex = 33;
		this.label13.Text = "Net Amount FCY";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(470, 160);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(12, 15);
		this.label28.TabIndex = 14;
		this.label28.Text = "*";
		this.cmbaccounttype.AllowNull = (DefaultableBoolean)1;
		this.cmbaccounttype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbaccounttype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbaccounttype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbaccounttype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Location = new System.Drawing.Point(125, 209);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Name = "cmbaccounttype";
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).TabIndex = 8;
		this.cmbaccounttype.ValueChanged += new System.EventHandler(cmbaccounttype_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Leave += new System.EventHandler(cmbaccounttype_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(470, 187);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(12, 15);
		this.label31.TabIndex = 13;
		this.label31.Text = "*";
		this.CmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance274;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance280).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Location = new System.Drawing.Point(487, 263);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Name = "CmbConsignee";
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.lblConsignee.AutoSize = true;
		this.lblConsignee.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblConsignee.Location = new System.Drawing.Point(364, 269);
		this.lblConsignee.Name = "lblConsignee";
		this.lblConsignee.Size = new System.Drawing.Size(62, 15);
		this.lblConsignee.TabIndex = 140;
		this.lblConsignee.Text = "Consignee";
		this.CmbFBCDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbFBCDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFBCDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFBCDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance279).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFBCDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Location = new System.Drawing.Point(487, 235);
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Name = "CmbFBCDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(364, 241);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(76, 15);
		this.label69.TabIndex = 140;
		this.label69.Text = "FBC Debit Ac";
		this.CmbGainLossAc.AllowNull = (DefaultableBoolean)1;
		this.CmbGainLossAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGainLossAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGainLossAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGainLossAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Location = new System.Drawing.Point(487, 208);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Name = "CmbGainLossAc";
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.txtForeinchargesPkr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtForeinchargesPkr.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForeinchargesPkr.Location = new System.Drawing.Point(805, 275);
		this.txtForeinchargesPkr.Name = "txtForeinchargesPkr";
		this.txtForeinchargesPkr.ReadOnly = true;
		this.txtForeinchargesPkr.Size = new System.Drawing.Size(173, 23);
		this.txtForeinchargesPkr.TabIndex = 1300;
		this.txtForeinchargesPkr.Text = "0";
		this.txtForeinchargesPkr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(687, 279);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(116, 15);
		this.label45.TabIndex = 120;
		this.label45.Text = "Foreign Charges PKR";
		this.txtLocalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLocalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmount.Location = new System.Drawing.Point(487, 291);
		this.txtLocalAmount.Name = "txtLocalAmount";
		this.txtLocalAmount.ReadOnly = true;
		this.txtLocalAmount.Size = new System.Drawing.Size(193, 23);
		this.txtLocalAmount.TabIndex = 1700;
		this.txtLocalAmount.TabStop = false;
		this.txtLocalAmount.Text = "0";
		this.txtLocalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(364, 295);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(104, 15);
		this.label51.TabIndex = 131;
		this.label51.Text = "Local Charges PKR";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(364, 214);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(76, 15);
		this.label55.TabIndex = 140;
		this.label55.Text = "Gain/Loss Ac";
		this.txtNetAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmount.Location = new System.Drawing.Point(487, 129);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.ReadOnly = true;
		this.txtNetAmount.Size = new System.Drawing.Size(193, 23);
		this.txtNetAmount.TabIndex = 13;
		this.txtNetAmount.TabStop = false;
		this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(470, 133);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(12, 15);
		this.label24.TabIndex = 10;
		this.label24.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(111, 215);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(12, 15);
		this.label56.TabIndex = 143;
		this.label56.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(5, 215);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(77, 15);
		this.label57.TabIndex = 142;
		this.label57.Text = "AccountType";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(125, 99);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		this.cmbInvoiceNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		this.cmbInvoiceNo.ValueChanged += new System.EventHandler(cmbInvoiceNo_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 105);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(59, 15);
		this.label8.TabIndex = 15;
		this.label8.Text = "Invoice # ";
		this.comSuplier.AllowNull = (DefaultableBoolean)1;
		this.comSuplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSuplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSuplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSuplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comSuplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSuplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSuplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSuplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comSuplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSuplier).Location = new System.Drawing.Point(125, 72);
		((System.Windows.Forms.Control)(object)this.comSuplier).Name = "comSuplier";
		((System.Windows.Forms.Control)(object)this.comSuplier).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.comSuplier).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comSuplier).Leave += new System.EventHandler(comSuplier_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(111, 105);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(12, 15);
		this.label22.TabIndex = 11;
		this.label22.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(111, 78);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(12, 15);
		this.label25.TabIndex = 7;
		this.label25.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 78);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(93, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Customer Name";
		this.txtFINo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFINo.Location = new System.Drawing.Point(805, 299);
		this.txtFINo.Name = "txtFINo";
		this.txtFINo.Size = new System.Drawing.Size(173, 23);
		this.txtFINo.TabIndex = 20;
		this.labFiNo.AutoSize = true;
		this.labFiNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labFiNo.Location = new System.Drawing.Point(687, 304);
		this.labFiNo.Name = "labFiNo";
		this.labFiNo.Size = new System.Drawing.Size(114, 15);
		this.labFiNo.TabIndex = 121;
		this.labFiNo.Text = "Financial Instr (F.I) #";
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(125, 263);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(234, 51);
		this.txtRemarks.TabIndex = 18;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(5, 267);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(52, 15);
		this.label46.TabIndex = 121;
		this.label46.Text = "Remarks";
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(125, 45);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Leave += new System.EventHandler(cmbPaymentTerm_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(1563, 67);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(12, 15);
		this.label42.TabIndex = 115;
		this.label42.Text = "*";
		this.label42.Visible = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(1554, 22);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(12, 15);
		this.label41.TabIndex = 8;
		this.label41.Text = "*";
		this.label41.Visible = false;
		this.cmbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbranches).Location = new System.Drawing.Point(1512, 46);
		((System.Windows.Forms.Control)(object)this.cmbranches).Name = "cmbranches";
		((System.Windows.Forms.Control)(object)this.cmbranches).Size = new System.Drawing.Size(89, 27);
		((System.Windows.Forms.Control)(object)this.cmbranches).TabIndex = 10000;
		((System.Windows.Forms.Control)(object)this.cmbranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbranches).Visible = false;
		this.txtdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(221, 20);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(137, 23);
		this.txtdate.TabIndex = 1;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance240).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(1512, 91);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(85, 27);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 12425;
		((System.Windows.Forms.Control)(object)this.cmbproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(1509, 71);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(48, 17);
		this.label40.TabIndex = 114;
		this.label40.Text = "Project";
		this.label40.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(470, 51);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(12, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1509, 21);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(47, 17);
		this.label39.TabIndex = 113;
		this.label39.Text = "Branch";
		this.label39.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(111, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(12, 15);
		this.label29.TabIndex = 10;
		this.label29.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(470, 24);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(12, 15);
		this.label26.TabIndex = 5;
		this.label26.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(470, 77);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(12, 16);
		this.label23.TabIndex = 8;
		this.label23.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(470, 105);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(12, 15);
		this.label20.TabIndex = 9;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(111, 133);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(12, 15);
		this.label19.TabIndex = 11;
		this.label19.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(111, 160);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(12, 15);
		this.label6.TabIndex = 7;
		this.label6.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(111, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(12, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(364, 105);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(84, 15);
		this.label18.TabIndex = 36;
		this.label18.Text = "Exchange Rate";
		this.txtexchangeRate.BackColor = System.Drawing.Color.White;
		this.txtexchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexchangeRate.Location = new System.Drawing.Point(487, 101);
		this.txtexchangeRate.Name = "txtexchangeRate";
		this.txtexchangeRate.Size = new System.Drawing.Size(193, 23);
		this.txtexchangeRate.TabIndex = 12;
		this.txtexchangeRate.Text = "0";
		this.txtexchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangeRate.TextChanged += new System.EventHandler(txtexchangeRate_TextChanged);
		this.txtexchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexchangeRate_KeyPress);
		this.txtexchangeRate.Leave += new System.EventHandler(txtexchangeRate_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(364, 78);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(74, 15);
		this.label14.TabIndex = 27;
		this.label14.Text = "FCY Amount";
		this.txtFCGrosamount.BackColor = System.Drawing.Color.White;
		this.txtFCGrosamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFCGrosamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCGrosamount.Location = new System.Drawing.Point(487, 74);
		this.txtFCGrosamount.Name = "txtFCGrosamount";
		this.txtFCGrosamount.Size = new System.Drawing.Size(193, 23);
		this.txtFCGrosamount.TabIndex = 11;
		this.txtFCGrosamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCGrosamount.TextChanged += new System.EventHandler(txtFCGrosamount_TextChanged);
		this.txtFCGrosamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFCGrosamount_KeyPress);
		this.txtFCGrosamount.Leave += new System.EventHandler(txtFCGrosamount_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(364, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 21;
		this.label11.Text = "Bank FBP # ";
		this.txtbankfbpNo.BackColor = System.Drawing.Color.White;
		this.txtbankfbpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankfbpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankfbpNo.Location = new System.Drawing.Point(487, 47);
		this.txtbankfbpNo.Name = "txtbankfbpNo";
		this.txtbankfbpNo.Size = new System.Drawing.Size(193, 23);
		this.txtbankfbpNo.TabIndex = 10;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 160);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(55, 15);
		this.label10.TabIndex = 24;
		this.label10.Text = "Fcy Code";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance248;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance255).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(125, 154);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(364, 24);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 15);
		this.label9.TabIndex = 18;
		this.label9.Text = "Account (Dr)";
		this.cmbbankAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbbankAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbankAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbankAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbankAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Location = new System.Drawing.Point(487, 18);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Name = "cmbbankAcc";
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Leave += new System.EventHandler(cmbbankAcc_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 133);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(101, 15);
		this.label7.TabIndex = 12;
		this.label7.Text = "Sales Contract No";
		this.cmbLCOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbLCOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLCOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLCOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance300;
		((SpecialBoxBase)((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLCOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Location = new System.Drawing.Point(125, 127);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Name = "cmbLCOrder";
		this.cmbLCOrder.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).TabIndex = 5;
		this.cmbLCOrder.ValueChanged += new System.EventHandler(cmbLCOrder_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Leave += new System.EventHandler(cmbLCOrder_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 51);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 9;
		this.label3.Text = "Payment Term";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(110, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Voucher Date && No";
		this.txtdocNumber.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocNumber.Location = new System.Drawing.Point(126, 20);
		this.txtdocNumber.Name = "txtdocNumber";
		this.txtdocNumber.ReadOnly = true;
		this.txtdocNumber.Size = new System.Drawing.Size(93, 23);
		this.txtdocNumber.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.chkpreview);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1586, 28);
		this.panel6.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(11, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(107, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "FCY Receipt";
		this.chkpreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkpreview.AutoSize = true;
		this.chkpreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkpreview.ForeColor = System.Drawing.Color.White;
		this.chkpreview.Location = new System.Drawing.Point(1501, 3);
		this.chkpreview.Name = "chkpreview";
		this.chkpreview.Size = new System.Drawing.Size(84, 20);
		this.chkpreview.TabIndex = 20;
		this.chkpreview.Text = "Preview";
		this.chkpreview.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.btnAttachments);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1586, 28);
		this.panel7.TabIndex = 1;
		this.btnAttachments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachments.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnPrint, this.btnAttachment, this.btnSlip, this.toolStripSeparator1, this.BtnLoadInvoice });
		this.btnAttachments.Location = new System.Drawing.Point(0, 0);
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(1586, 27);
		this.btnAttachments.TabIndex = 0;
		this.btnAttachments.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(108, 24);
		this.btnPrint.Text = "&Voucher102";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(116, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(92, 24);
		this.btnSlip.Text = "516-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadInvoice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadInvoice.Image = (System.Drawing.Image)resources.GetObject("BtnLoadInvoice.Image");
		this.BtnLoadInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadInvoice.Name = "BtnLoadInvoice";
		this.BtnLoadInvoice.Size = new System.Drawing.Size(95, 24);
		this.BtnLoadInvoice.Text = "Load Invoice";
		this.BtnLoadInvoice.Click += new System.EventHandler(BtnLoadInvoice_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1592, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 151);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1586, 569);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 49;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.label70);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 127);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1586, 24);
		this.panel9.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1546, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 46;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(2081, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(56, 26);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label70.AutoSize = true;
		this.label70.BackColor = System.Drawing.Color.Teal;
		this.label70.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label70.Location = new System.Drawing.Point(2, 2);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(142, 19);
		this.label70.TabIndex = 0;
		this.label70.Text = "Filtered Records";
		this.panel15.Controls.Add(this.groupBox6);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 55);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1586, 72);
		this.panel15.TabIndex = 0;
		this.groupBox6.Controls.Add(this.label67);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory);
		this.groupBox6.Controls.Add(this.label73);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox6.Controls.Add(this.label74);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox6.Controls.Add(this.btnShowHistory);
		this.groupBox6.Controls.Add(this.label75);
		this.groupBox6.Controls.Add(this.label76);
		this.groupBox6.Controls.Add(this.label89);
		this.groupBox6.Controls.Add(this.label90);
		this.groupBox6.Controls.Add(this.label91);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox6.Controls.Add(this.ToDocNoHistory);
		this.groupBox6.Controls.Add(this.FromDocNoHistory);
		this.groupBox6.Controls.Add(this.ToDateHistory);
		this.groupBox6.Controls.Add(this.FromDateHistory);
		this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(0, 0);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1030, 72);
		this.groupBox6.TabIndex = 1;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Filters";
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(673, 41);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 31);
		this.label67.TabIndex = 93;
		this.label67.Text = "Receiver / Bank Ac";
		this.CmbReceiverAcHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance263).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance264).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance264).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance264).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Location = new System.Drawing.Point(739, 41);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Name = "CmbReceiverAcHistory";
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).TabIndex = 7;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(397, 47);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(65, 15);
		this.label73.TabIndex = 91;
		this.label73.Text = "Invoice No";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(482, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(397, 20);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(84, 15);
		this.label74.TabIndex = 89;
		this.label74.Text = "Payment Term";
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(482, 14);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 4;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(962, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 8;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(673, 20);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(58, 15);
		this.label75.TabIndex = 87;
		this.label75.Text = "Customer";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(214, 47);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(86, 15);
		this.label76.TabIndex = 86;
		this.label76.Text = "To Voucher No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(214, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(101, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Voucher No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(5, 47);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(5, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(739, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 6;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(315, 43);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(315, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Controls.Add(this.label38);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1586, 52);
		this.panel8.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1586, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.Teal;
		this.label38.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label38.Location = new System.Drawing.Point(5, 30);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(165, 19);
		this.label38.TabIndex = 0;
		this.label38.Text = "Fcy Receipt History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1600, 752);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "Acfrmfcbankreceipt_DebitCurrency";
		this.Text = "Acfrmfcbankreceipt";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Acfrmfcbankreceipt_FormClosing);
		base.Load += new System.EventHandler(Acfrmfcbankreceipt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Acfrmfcbankreceipt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.RemainingBalanceOfBankDeviceBreakUp.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.RemainingBalanceGrid).EndInit();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesFDBCNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).EndInit();
		this.panelInvoiceWiseBreakup.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvoiceWiseBreakUp).EndInit();
		this.panel11.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatusBreakup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGdNoBreakup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoicenobreakup).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPartyPaymentsBreackUp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceNo).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFcycodeThirdParty).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFBCDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.btnAttachments.ResumeLayout(false);
		this.btnAttachments.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
